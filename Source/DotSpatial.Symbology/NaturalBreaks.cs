using System;
using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Jenks natural breaks optimization used for symbolizing based on minimizing variance within categories.
    /// </summary>
    /// <remarks>
    /// This code replaces the JenksBreaks code which was based on MapWinGIS and was buggy.
    /// This version is based on (http://en.wikipedia.org/wiki/Jenks_natural_breaks_optimization)
    /// Implementations: [1](http://danieljlewis.org/files/2010/06/Jenks.pdf) (python),
    /// [2](https://github.com/vvoovv/djeo-jenks/blob/master/main.js) (buggy),
    /// [3](https://github.com/simogeo/geostats/blob/master/lib/geostats.js#L407) (works)
    /// Adapted by Dan Ames from "literate" version created by Tom MacWright
    /// presented here: https://macwright.org/2013/02/18/literate-jenks.html and here https://gist.github.com/tmcw/4977508
    /// </remarks>
    internal class NaturalBreaks
    {
        #region Fields

        private int[,] _lowerClassLimits = null;
        private double[,] _varianceCombinations = null;
        private double[] _values = null;
        private int _numClasses;
        private int _numValues;
        private List<double> _resultClasses = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalBreaks"/> class.
        /// </summary>
        /// <param name="values">data values used for calculation.</param>
        /// <param name="numClasses">Number of breaks that should be calculated.</param>
        public NaturalBreaks(List<double> values, int numClasses)
        {
            _numClasses = numClasses;
            _numValues = values.Count;

            _values = values.ToArray();

            // the number of classes must be greater than one and less than the number of data elements.
            if (_numClasses > _values.Length) return;
            if (_numClasses < 2) return;

            // sort data in numerical order, since this is expected by the matrices function
            Array.Sort(_values);

            // get our basic matrices
            GetMatrices();

            // extract n_classes out of the computed matrices
            GetBreaks();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compute the matrices required for Jenks breaks. These matrices
        /// can be used for any classing of data with `classes <= n_classes`
        /// </summary>
        private void GetMatrices()
        {
            // in the original implementation, these matrices are referred to as `LC` and `OP`
            //
            // * lower_class_limits (LC): optimal lower class limits
            // * variance_combinations (OP): optimal variance combinations for all classes - declared at class level now
            // loop counters
            int i, j;

            // the variance, as computed at each step in the calculation
            double variance = 0;

            // Initialize and fill each matrix with zeroes
            _lowerClassLimits = new int[_values.Length + 1, _numClasses + 1];
            _varianceCombinations = new double[_values.Length + 1, _numClasses + 1];

            for (i = 1; i < _numClasses + 1; i++)
            {
                _lowerClassLimits[1, i] = 1;
                _varianceCombinations[1, i] = 0;

                // in the original implementation, 9999999 is used but
                // since Javascript has `Infinity`, we use that.
                for (j = 2; j < _values.Length + 1; j++)
                {
                    _varianceCombinations[j, i] = double.PositiveInfinity;
                }
            }

            for (var l = 2; l < _values.Length + 1; l++)
            {
                // `SZ` originally. this is the sum of the values seen thus far when calculating variance.
                double sum = 0;

                // `ZSQ` originally. the sum of squares of values seen thus far
                double sum_squares = 0;

                // `WT` originally. This is the number of
                int w = 0;

                // `IV` originally
                int i4 = 0;

                // in several instances, you could say `Math.pow(x, 2)` instead of `x * x`, but this is slower in some browsers introduces an unnecessary concept.
                for (var m = 1; m < l + 1; m++)
                {
                    // `III` originally
                    var lower_class_limit = l - m + 1;
                    var val = _values[lower_class_limit - 1];

                    // here we're estimating variance for each potential classing
                    // of the data, for each potential number of classes. `w`
                    // is the number of data points considered so far.
                    w++;

                    // increase the current sum and sum-of-squares
                    sum += val;
                    sum_squares += val * val;

                    // the variance at this point in the sequence is the difference
                    // between the sum of squares and the total x 2, over the number
                    // of samples.
                    variance = sum_squares - (sum * sum) / w;

                    i4 = lower_class_limit - 1;

                    if (i4 != 0)
                    {
                        for (j = 2; j < _numClasses + 1; j++)
                        {
                            // if adding this element to an existing class
                            // will increase its variance beyond the limit, break
                            // the class at this point, setting the lower_class_limit
                            // at this point.
                            if (_varianceCombinations[l, j] >=
                                (variance + _varianceCombinations[i4, j - 1]))
                            {
                                _lowerClassLimits[l, j] = lower_class_limit;
                                _varianceCombinations[l, j] = variance +
                                    _varianceCombinations[i4, j - 1];
                            }
                        }
                    }
                }

                _lowerClassLimits[l, 1] = 1;
                _varianceCombinations[l, 1] = variance;
            }
        }

        /// <summary>
        /// the second part of the jenks recipe: take the calculated matrices and derive an array of n breaks.
        /// </summary>
        private void GetBreaks()
        {
            int k = _values.Length - 1;
            double[] kclass = new double[_numClasses + 1];
            int countNum = _numClasses;

            // the calculation of classes will never include the upper and
            // lower bounds, so we need to explicitly set them
            kclass[_numClasses] = _values[_values.Length - 1];
            kclass[0] = _values[0];

            // the lower_class_limits matrix is used as indexes into itself
            // here: the `k` variable is reused in each iteration.
            while (countNum > 1)
            {
                kclass[countNum - 1] = _values[_lowerClassLimits[k, countNum] - 2];
                k = _lowerClassLimits[k, countNum] - 1;
                countNum--;
            }

            _resultClasses = kclass.ToList();
        }

        /// <summary>
        /// Convert the _resultClasses which is a list of doubles into a list of integer IDs of the corresponding range values.
        /// </summary>
        /// <returns> _</returns>
        public List<double> GetResults()
        {
            return _resultClasses;
        }
        #endregion
    }
}

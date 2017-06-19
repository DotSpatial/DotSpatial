// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Calculates Jenks Natural Breaks
    /// </summary>
    /// <remarks>
    /// Ported from http://svn.mapwindow.org/svnroot/MapWinGIS4Dev/JenksBreaks.h
    /// </remarks>
    internal class JenksBreaksCalcuation
    {
        #region Fields

        private readonly List<JenksBreak> _classes;
        private readonly int _numClasses;
        private readonly int _numValues;

        private readonly List<JenksData> _values;
        private int _leftBound; // the id of classes between which optimization takes place
        private int _previousMaxId; // to prevent stalling in the local optimum
        private int _previousTargetId;
        private int _rightBound; // initially it's all classes, then it's reducing

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JenksBreaksCalcuation"/> class.
        /// </summary>
        /// <param name="values">Values used for calculation.</param>
        /// <param name="numClasses">Number of breaks that should be calculated.</param>
        public JenksBreaksCalcuation(IList<double> values, int numClasses)
        {
            _numClasses = numClasses;
            _numValues = values.Count;

            double classCount = _numValues / (double)numClasses;

            // fill values
            _values = new List<JenksData>(_numValues);
            for (int i = 0; i < _numValues; i++)
            {
                JenksData data = new JenksData(values[i], values[i] * values[i])
                {
                    ClassId = (int)Math.Floor(i * 1.0 / classCount)
                };
                _values.Add(data);
            }

            _classes = new List<JenksBreak>(_numClasses);
            for (int i = 0; i < _numClasses; i++)
            {
                _classes.Add(new JenksBreak());
            }

            // calculate initial deviations for classes
            int lastId = -1;
            for (int i = 0; i < _numValues; i++)
            {
                int classId = _values[i].ClassId;
                if (classId >= 0 && classId < _numClasses)
                {
                    _classes[classId].Value += _values[i].Value;
                    _classes[classId].Square += _values[i].Square;

                    // saving bound between classes
                    if (classId != lastId)
                    {
                        _classes[classId].StartId = i;
                        lastId = classId;

                        if (classId > 0) _classes[classId - 1].EndId = i - 1;
                    }
                }
            }

            _classes[_numClasses - 1].EndId = _numValues - 1;

            for (int i = 0; i < _numClasses; i++) _classes[i].RefreshStandardDeviations();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returning of results (indices of values to start each class)
        /// </summary>
        /// <returns>A list of the indices that start a class.</returns>
        public List<int> GetResults()
        {
            var results = new List<int>(_numClasses);
            for (int i = 0; i < _numClasses; i++)
            {
                results.Add(_classes[i].StartId);
            }

            return results;
        }

        /// <summary>
        /// Optimization routine
        /// </summary>
        public void Optimize()
        {
            // initialization
            double minValue = GetSumStandardDeviations(); // current best minimum
            _leftBound = 0; // we'll consider all classes in the beginning
            _rightBound = _classes.Count - 1;
            _previousMaxId = -1;
            _previousTargetId = -1;
            int numAttemmpts = 0;

            bool proceed = true;
            while (proceed)
            {
                for (int i = 0; i < _numValues; i++)
                {
                    FindShift();

                    // when there are only 2 classes left we should stop on the local max value
                    if (_rightBound - _leftBound == 0)
                    {
                        double val = GetSumStandardDeviations(); // the final minimum
                        numAttemmpts++;

                        if (numAttemmpts > 5)
                        {
                            return;
                        }
                    }
                }

                double value = GetSumStandardDeviations();
                proceed = value < minValue; // if the deviations became smaller we'll execute one more loop

                if (value < minValue) minValue = value;
            }
        }

        // MakeShift()
        // Passing the value from one class to another to another. Criteria - standard deviation.
        private void FindShift()
        {
            // first we'll find classes with the smallest and largest SD
            int maxId = 0, minId = 0;
            double minValue = double.MaxValue, maxValue = 0.0; // use constant
            for (int i = _leftBound; i <= _rightBound; i++)
            {
                if (_classes[i].SDev > maxValue)
                {
                    maxValue = _classes[i].SDev;
                    maxId = i;
                }

                if (_classes[i].SDev < minValue)
                {
                    minValue = _classes[i].SDev;
                    minId = i;
                }
            }

            // then pass one observation from the max class in the direction of min class
            int valueId;
            int targetId;
            if (maxId > minId)
            {
                // <-  we should find first value of max class
                valueId = _classes[maxId].StartId;
                targetId = maxId - 1;
                _classes[maxId].StartId++;
                _classes[targetId].EndId++;
            }
            else if (maxId < minId)
            {
                // ->  we should find last value of max class
                valueId = _classes[maxId].EndId;
                targetId = maxId + 1;
                _classes[maxId].EndId--;
                _classes[targetId].StartId--;
            }
            else
            {
                // only one class left or the deviations withinb classes are equal
                return;
            }

            // Prevents stumbling in local optimum - algorithm will be repeating the same move
            // To prevent this we'll exclude part of classes with less standard deviation
            if (_previousMaxId == targetId && _previousTargetId == maxId)
            {
                // Now we choose which of the two states provides less deviation
                double value1 = GetSumStandardDeviations();

                // change to second state
                MakeShift(maxId, targetId, valueId);
                double value2 = GetSumStandardDeviations();

                // if first state is better revert to it
                if (value1 < value2)
                {
                    MakeShift(targetId, maxId, valueId);
                }

                // now we can exclude part of the classes where no improvements can be expected
                int min = Math.Min(targetId, maxId);
                int max = Math.Max(targetId, maxId);

                double avg = GetSumStandardDeviations() / (_rightBound - _leftBound + 1);

                // analyze left side of distribution
                double sumLeft = 0, sumRight = 0;
                for (int j = _leftBound; j <= min; j++)
                {
                    sumLeft += Math.Pow(_classes[j].SDev - avg, 2.0);
                }

                sumLeft /= min - _leftBound + 1;

                // analyze right side of distribution
                for (int j = _rightBound; j >= max; j--)
                {
                    sumRight += Math.Pow(_classes[j].SDev - avg, 2.0);
                }

                sumRight /= _rightBound - max + 1;

                // exluding left part
                if (sumLeft >= sumRight)
                {
                    _leftBound = max;
                }

                // exluding right part
                else if (sumLeft < sumRight)
                {
                    _rightBound = min;
                }
            }
            else
            {
                MakeShift(maxId, targetId, valueId);
            }
        }

        /// <summary>
        /// Calculates the sum of standard deviations of individual variants from the class mean through all class.
        /// It's the objective function - should be minimized
        /// </summary>
        /// <returns>The sum of standard deviations.</returns>
        private double GetSumStandardDeviations()
        {
            double sum = 0.0;
            for (int i = 0; i < _numClasses; i++)
            {
                sum += _classes[i].SDev;
            }

            return sum;
        }

        // perform actual shift
        private void MakeShift(int maxId, int targetId, int valueId)
        {
            // saving the last shift
            _previousMaxId = maxId;
            _previousTargetId = targetId;

            var data = _values[valueId];

            // removing from max class
            _classes[maxId].Value -= data.Value;
            _classes[maxId].Square -= data.Square;
            _classes[maxId].RefreshStandardDeviations();

            // passing to target class
            _classes[targetId].Value += data.Value;
            _classes[targetId].Square += data.Square;
            _classes[targetId].RefreshStandardDeviations();

            // mark that the value was passed
            _values[valueId].ClassId = targetId;
        }

        #endregion

        #region Classes

        private class JenksBreak
        {
            #region Constructors

            public JenksBreak()
            {
                Value = 0.0;
                Square = 0.0;
                SDev = 0.0;
                StartId = -1;
                EndId = -1;
            }

            #endregion

            #region Properties

            public int EndId { get; set; }

            public double SDev { get; set; }

            public double Square { get; set; }

            public int StartId { get; set; }

            public double Value { get; set; }

            #endregion

            #region Methods

            // the formula for every class is: sum((a[k])^2) - sum(a[k])^2/n
            public void RefreshStandardDeviations()
            {
                SDev = Square - (Math.Pow(Value, 2.0) / (EndId - StartId + 1));
            }

            #endregion
        }

        private class JenksData
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="JenksData"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="square">The square.</param>
            public JenksData(double value, double square)
            {
                Value = value;
                Square = square;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the number of the break to which a value belongs.
            /// </summary>
            public int ClassId { get; set; }

            public double Square { get; }

            public double Value { get; }

            #endregion
        }

        #endregion
    }
}
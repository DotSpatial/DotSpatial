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

        private readonly List<JenksData> _values;
        private readonly List<JenksBreak> _classes;
        private readonly int _numClasses;
        private readonly int _numValues;
        private int _previousMaxId;		// to prevent stalling in the local optimum
        private int _previousTargetId;
        private int _leftBound;			// the id of classes between which optimization takes place
        private int _rightBound;		// initially it's all classes, then it's reducing

        #endregion

        #region Nested classes

        private class JenksData
        {
            public readonly double value;
            public readonly double square;
            public int classId;	// number of break to which a value belongs

            public JenksData(double value, double square)
            {
                this.value = value;
                this.square = square;
            }
        };

        private class JenksBreak
        {
            public double value;
            public double square;
            public int count;
            public double SDev;
            public int startId;
            public int endId;
		
            public JenksBreak()
            {
                value = 0.0;
                square = 0.0;
                count = 0;
                SDev = 0.0;
                startId = -1;
                endId = -1;
            }
		
            // the formula for every class is: sum((a[k])^2) - sum(a[k])^2/n
            public void RefreshStandardDeviations()
            {
                SDev = square - Math.Pow(value, 2.0)/(endId - startId + 1);
            }
        }

        #endregion

        #region Ctor

        public JenksBreaksCalcuation(IList<double> values, int numClasses)
        {
            _numClasses = numClasses;
            _numValues = values.Count;
			
            double classCount = _numValues/(double)numClasses;
            			
            // fill values
            _values = new List<JenksData>(_numValues);
            for (int i = 0; i < _numValues; i++)
            {
                JenksData data = new JenksData(values[i], values[i]*values[i]) {classId = (int) Math.Floor(i*1.0/classCount)};
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
                int classId = _values[i].classId;
                if (classId >= 0  && classId < _numClasses)
                {
                    _classes[classId].value += _values[i].value;
                    _classes[classId].square += _values[i].square;
                    _classes[classId].count += 1;

                    // saving bound between classes
                    if (classId != lastId)
                    {
                        _classes[classId].startId = i;
                        lastId = classId;
						
                        if (classId > 0)
                            _classes[classId - 1].endId = i - 1;
                    }
                }
            }
            _classes[_numClasses - 1].endId = _numValues - 1;
			
            for (int i = 0; i < _numClasses; i++)
                _classes[i].RefreshStandardDeviations();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Optimization routine
        /// </summary>
        public void Optimize()
        {
            // initialization
            double minValue = get_SumStandardDeviations();	// current best minimum
            _leftBound = 0;							// we'll consider all classes in the beginning
            _rightBound = _classes.Count - 1;
            _previousMaxId = -1;
            _previousTargetId = - 1;
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
                        double val = get_SumStandardDeviations();	// the final minimum
                        numAttemmpts++; 
					
                        if ( numAttemmpts > 5)
                        {
                            return;
                        }
                    }
                }
                double value = get_SumStandardDeviations();
                proceed = (value < minValue)?true:false;	// if the deviations became smaller we'll execute one more loop
			
                if (value < minValue)
                    minValue = value;
            }
        }
        
        /// <summary>
        /// Returning of results (indices of values to start each class)
        /// </summary>
        public List<int> GetResults()
        {
            var results = new List<int>(_numClasses);
            for (int i = 0; i < _numClasses; i++ )
            {
                results.Add(_classes[i].startId);
            }
            return results;
        }

       #endregion

        #region Private methods

        // ******************************************************************
        // Calculates the sum of standard deviations of individual variants 
        // from the class mean through all class
        // It's the objective function - should be minimized
        // 
        // ******************************************************************
        private double get_SumStandardDeviations()
        {
            double sum = 0.0;
            for (int i = 0; i < _numClasses; i++) 
            {
                sum += _classes[i].SDev;
            }
            return sum;
        }
	
        // ******************************************************************
        //	  MakeShift()
        // ******************************************************************
        // Passing the value from one class to another to another. Criteria - standard deviation.
        private void FindShift()
        {
            // first we'll find classes with the smallest and largest SD
            int maxId = 0, minId = 0; 
            double minValue = double.MaxValue, maxValue = 0.0;	// use constant
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
            int valueId = -1;
            int targetId = -1;
            if (maxId > minId)
            {
                //<-  we should find first value of max class
                valueId = _classes[maxId].startId; 
                targetId = maxId - 1;
                _classes[maxId].startId++;
                _classes[targetId].endId++;
            }
            else if (maxId < minId)
            {
                //->  we should find last value of max class
                valueId = _classes[maxId].endId; 
                targetId = maxId + 1;
                _classes[maxId].endId--;
                _classes[targetId].startId--;
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
                double value1 = get_SumStandardDeviations();
			
                // change to second state
                MakeShift(maxId, targetId, valueId);
                double value2 = get_SumStandardDeviations();
			
                // if first state is better revert to it
                if (value1 < value2)
                {
                    MakeShift(targetId, maxId, valueId);
                }
			
                // now we can exclude part of the classes where no improvements can be expected
                int min = Math.Min(targetId, maxId);
                int max = Math.Max(targetId, maxId);
			
                double avg = get_SumStandardDeviations()/(_rightBound - _leftBound + 1);

                // analyze left side of distribution
                double sumLeft = 0, sumRight = 0;
                for (int j = _leftBound; j <= min; j++)
                {
                    sumLeft += Math.Pow(_classes[j].SDev - avg, 2.0);
                }
                sumLeft /= (min - _leftBound + 1);

                // analyze right side of distribution
                for (int j = _rightBound; j >= max; j--)
                {
                    sumRight += Math.Pow(_classes[j].SDev - avg, 2.0);
                }
                sumRight /= (_rightBound - max + 1);

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

        // perform actual shift
        private void MakeShift(int maxId, int targetId, int valueId)
        {
            // saving the last shift
            _previousMaxId = maxId;
            _previousTargetId = targetId;

            var data = _values[valueId];

            // removing from max class
            _classes[maxId].value -= data.value;
            _classes[maxId].square -= data.square;
            _classes[maxId].count -= 1;
            _classes[maxId].RefreshStandardDeviations();
		
            // passing to target class
            _classes[targetId].value += data.value;
            _classes[targetId].square += data.square;
            _classes[targetId].count += 1;
            _classes[targetId].RefreshStandardDeviations();
		
            // mark that the value was passed
            _values[valueId].classId = targetId;
        }
        
        #endregion
    };
}
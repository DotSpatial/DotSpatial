// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Collections.Generic;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Position averaging filter
    /// </summary>
    public sealed class PositionAverageFilter : PrecisionFilter
    {
        /// <summary>
        ///
        /// </summary>
        private List<Position3D> _samples;
        /// <summary>
        ///
        /// </summary>
        private List<DateTime> _sampleTimes;

        /// <summary>
        ///
        /// </summary>
        private Position3D _filteredPositon;

        /// <summary>
        ///
        /// </summary>
        private int _sampleCount;

        /// <summary>
        /// Creates an uninitialized filter with a sample count of 4
        /// </summary>
        public PositionAverageFilter()
            : this(4)
        { }

        /// <summary>
        /// Creates an uninitialized filter with a sample count.
        /// </summary>
        /// <param name="sampleCount">The sample count.</param>
        public PositionAverageFilter(int sampleCount)
        {
            _samples = new List<Position3D>(sampleCount);
            _sampleCount = sampleCount;
        }

        /// <summary>
        /// Creates an initialized filter.
        /// </summary>
        /// <param name="positions">The positions.</param>
        /// <remarks>The sample count equals the number of positions passed in the positions argument.</remarks>
        public PositionAverageFilter(params Position[] positions)
            : this(new List<Position>(positions), positions.Length)
        { }

        /// <summary>
        /// Creates an initialzed filter
        /// </summary>
        /// <param name="positions">The positions.</param>
        /// <param name="sampleCount">The sample count.</param>
        /// <remarks>If the number of positions supllied in the positions parameter and the sample
        /// count parameter are not equal, the filter is uninitialized.</remarks>
        public PositionAverageFilter(IList<Position> positions, int sampleCount)
        {
            Initialize(positions);
            _sampleCount = sampleCount;
        }

        // Average position calculation
        /// <summary>
        /// Filters this instance.
        /// </summary>
        private void Filter()
        {
            double x = 0, y = 0, z = 0;

            int count = _samples.Count;
            for (int i = 0; i < count; i++)
            {
                x += _samples[i].Longitude.DecimalDegrees;
                y += _samples[i].Latitude.DecimalDegrees;
                z += _samples[i].Altitude.ToMeters().Value;
            }

            _filteredPositon =
                new Position3D(
                    new Longitude(x / count),
                    new Latitude(y / count),
                    Distance.FromMeters(z / count));
        }

        /// <summary>
        /// Gets the number of samples required for a valid fintered value.
        /// </summary>
        public int SampleCount
        {
            get { return _sampleCount; }
        }

        /// <summary>
        /// Gets a list of the current observed locations
        /// </summary>
        public Position3D[] ObservedLocations
        {
            get { return _samples.ToArray(); }
        }

        /// <summary>
        /// Not implemented in the PrositionAverage filter. Use ObservedLocations to get a list
        /// of observed locations.
        /// </summary>
        public override Position3D ObservedLocation
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the filtered location
        /// </summary>
        public override Position3D FilteredLocation
        {
            get { return _filteredPositon; }
        }

        /// <summary>
        /// Gets a value indicating the state of the filter.
        /// </summary>
        /// <remarks>The filter is considered initialized and thus reporting valid filtered
        /// locations, when the SampleCount property equals the number of values returned
        /// by the ObservedLocations property.</remarks>
        public override bool IsInitialized
        {
            get { return _samples.Count == _sampleCount; }
        }

        /// <summary>
        /// Gets the time elapsed from the oldest to the most resent position sample.
        /// </summary>
        public override TimeSpan Delay
        {
            get
            {
                TimeSpan result = TimeSpan.Zero;

                int count = _sampleTimes.Count;
                if (count > 0)
                    result = _sampleTimes[count - 1].Subtract(_sampleTimes[0]);

                return result;
            }
        }

        /// <summary>
        /// Initializes the filter
        /// </summary>
        /// <param name="positions">The initial sample positions.</param>
        /// <remarks>This method updates the SampleCount property to the number of
        /// initialization positions providedand updates the FilteredLocation
        /// property.</remarks>
        public void Initialize(IList<Position> positions)
        {
            List<Position3D> samples = new List<Position3D>(positions.Count + 1);

            for (int i = 0; i < positions.Count; i++)
            {
                samples.Add(new Position3D(positions[i]));
            }

            Initialize(samples);
        }

        /// <summary>
        /// Initializes the filter
        /// </summary>
        /// <param name="positions">The initial sample positions.</param>
        /// <remarks>This method updates the SampleCount property to the number of
        /// initialization positions provided and updates the FilteredLocation
        /// property.</remarks>
        public void Initialize(IList<Position3D> positions)
        {
            _samples = new List<Position3D>(_sampleCount + 1);
            _sampleTimes = new List<DateTime>(_sampleCount + 1);

            _samples.AddRange(positions);
            _sampleTimes.Add(DateTime.Now);

            _sampleCount = positions.Count;

            Filter();
        }

        /// <summary>
        /// Adds an initialization position.
        /// </summary>
        /// <param name="gpsPosition">The initialization position to add.</param>
        /// <remarks>This method does not update the SampleCount or the FilteredLocation
        /// properties.</remarks>
        public override void Initialize(Position gpsPosition)
        {
            Initialize(new Position3D(gpsPosition));
        }

        /// <summary>
        /// Adds an initialization position.
        /// </summary>
        /// <param name="gpsPosition">The initialization position to add.</param>
        /// <remarks>This method does not update the SampleCount or the FilteredLocation
        /// properties.</remarks>
        public override void Initialize(Position3D gpsPosition)
        {
            _samples = new List<Position3D>(_sampleCount + 1);
            _sampleTimes = new List<DateTime>(_sampleCount + 1);

            _samples.Add(gpsPosition);
            _sampleTimes.Add(DateTime.Now);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition">The new observation to add to the filter.</param>
        /// <returns>The filtered position.</returns>
        /// <remarks>This method updates the FilteredLocation property without consideration for SampleCount.</remarks>
        public override Position Filter(Position gpsPosition)
        {
            Position3D pos3D = Filter(new Position3D(gpsPosition));
            return new Position(pos3D.Latitude, pos3D.Longitude);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition">The new observation to add to the filter.</param>
        /// <param name="deviceError">A DeviceError, which does not currently affect position averaging.</param>
        /// <param name="horizontalDOP">A horizontal dilution of position, which does not currently affect position averaging.</param>
        /// <param name="verticalDOP">A vertical dilution of positoin which does not currently affect position averaging.</param>
        /// <param name="bearing">A directional bearing, which does not currently affect position averaging.</param>
        /// <param name="speed">A speed, which does not currently affect position averaging.</param>
        /// <returns></returns>
        /// <remarks>This method updates the FilteredLocation property without consideration for SampleCount.</remarks>
        public override Position Filter(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            Position3D pos3D = Filter(new Position3D(gpsPosition), deviceError, horizontalDOP, verticalDOP, bearing, speed);
            return new Position(pos3D.Latitude, pos3D.Longitude);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition">The new observation to add to the filter.</param>
        /// <returns>The filtered position.</returns>
        /// <remarks>This method updates the FilteredLocation property without consideration for SampleCount.</remarks>
        public override Position3D Filter(Position3D gpsPosition)
        {
            return Filter(gpsPosition);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition">The new observation to add to the filter.</param>
        /// <param name="deviceError">A DeviceError, which does not currently affect position averaging.</param>
        /// <param name="horizontalDOP">A horizontal dilution of position, which does not currently affect position averaging.</param>
        /// <param name="verticalDOP">A vertical dilution of positoin which does not currently affect position averaging.</param>
        /// <param name="bearing">A directional bearing, which does not currently affect position averaging.</param>
        /// <param name="speed">A speed, which does not currently affect position averaging.</param>
        /// <returns></returns>
        /// <remarks>This method updates the FilteredLocation property without consideration for SampleCount.</remarks>
        public override Position3D Filter(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            _samples.Add(gpsPosition);
            _sampleTimes.Add(DateTime.Now);

            int count = _samples.Count;
            int timeCount = _sampleTimes.Count;
            int maxCount = 0;

            // Only average the number of samples specified in the constructor
            while (count > _sampleCount)
            {
                _samples.RemoveAt(0);
                count--;
                maxCount++;
            }

            // Only 2 times are needed, oldest and most recent.
            // Try to remove as many as were removed from the sample collection.
            while (timeCount > 2 && maxCount > 0)
            {
                _sampleTimes.RemoveAt(0);
                timeCount--;
            }

            Filter();

            return _filteredPositon;
        }
    }
}
using System;
using System.Collections.Generic;

namespace DotSpatial.Positioning.Gps.Filters
{
    /// <summary>
    /// Position averaging filter
    /// </summary>
    public sealed class PositionAverageFilter : PrecisionFilter
    {
        private List<Position3D> _samples;
        private List<DateTime> _sampleTimes;

        private Position3D _filteredPositon;

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
        /// <param name="sampleCount"> The number of samples used in an initialized filter. </param>
        public PositionAverageFilter(int sampleCount)
        {
            this._samples = new List<Position3D>(sampleCount);
            this._sampleCount = sampleCount;
        }
        
        /// <summary>
        /// Creates an initialized filter.
        /// </summary>
        /// <param name="positions"> The initialization positions. </param>
        /// <remarks>
        /// The sample count equals the number of positions passed in the positions argument.
        /// </remarks>
        public PositionAverageFilter(params Position[] positions)
            : this(new List<Position>(positions), positions.Length)
        { }

        /// <summary>
        /// Creates an initialzed filter
        /// </summary>
        /// <param name="positions"> The initialization positions. </param>
        /// <param name="sampleCount"> The initialized sample count. </param>
        /// <remarks>
        /// If the number of positions supllied in the positions parameter and the sample
        /// count parameter are not equal, the filter is uninitialized. 
        /// </remarks>
        public PositionAverageFilter(IList<Position> positions, int sampleCount)
        {
            Initialize(positions);
            _sampleCount = sampleCount;
        }

        // Average position calculation
        private void Filter()
        {
            double x = 0, y = 0, z = 0;

            int count = this._samples.Count;
            for (int i = 0; i < count; i++)
            {
                x += _samples[i].Longitude.DecimalDegrees;
                y += _samples[i].Latitude.DecimalDegrees;
                z += _samples[i].Altitude.ToMeters().Value;
            }

            this._filteredPositon = 
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
            get { return ((List<Position3D>)_samples).ToArray(); }
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
        /// <remarks>
        /// The filter is considered initialized and thus reporting valid filtered 
        /// locations, when the SampleCount property equals the number of values returned
        /// by the ObservedLocations property.
        /// </remarks>
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
        /// <param name="positions"> The initial sample positions. </param>
        /// <remarks>
        /// This method updates the SampleCount property to the number of 
        /// initialization positions providedand updates the FilteredLocation
        /// property.
        /// </remarks>
        public void Initialize(IList<Position> positions)
        {
            List<Position3D> samples = new List<Position3D>(positions.Count + 1);
            int count = positions.Count;
            for (int i = 0; i < 0; i++)
            {
                samples.Add(new Position3D(positions[i]));
            }

            Initialize(samples);
        }

        /// <summary>
        /// Initializes the filter
        /// </summary>
        /// <param name="positions"> The initial sample positions. </param>
        /// <remarks>
        /// This method updates the SampleCount property to the number of 
        /// initialization positions provided and updates the FilteredLocation
        /// property.
        /// </remarks>
        public void Initialize(IList<Position3D> positions)
        {
            this._samples = new List<Position3D>(_sampleCount + 1);
            this._sampleTimes = new List<DateTime>(_sampleCount + 1);

            this._samples.AddRange(positions);
            this._sampleTimes.Add(DateTime.Now);

            this._sampleCount = positions.Count;

            Filter();
        }

        /// <summary>
        /// Adds an initialization position.
        /// </summary>
        /// <param name="gpsPosition"> The initialization position to add. </param>
        /// <remarks>
        /// This method does not update the SampleCount or the FilteredLocation
        /// properties.
        /// </remarks>
        public override void Initialize(Position gpsPosition)
        {
            this.Initialize(new Position3D(gpsPosition));
        }

        /// <summary>
        /// Adds an initialization position.
        /// </summary>
        /// <param name="gpsPosition"> The initialization position to add. </param>
        /// <remarks>
        /// This method does not update the SampleCount or the FilteredLocation
        /// properties.
        /// </remarks>
        public override void Initialize(Position3D gpsPosition)
        {
            this._samples = new List<Position3D>(_sampleCount + 1);
            this._sampleTimes = new List<DateTime>(_sampleCount + 1);

            this._samples.Add(gpsPosition);
            this._sampleTimes.Add(DateTime.Now);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition"> The new observation to add to the filter. </param>
        /// <returns> The filtered position. </returns>
        /// <remarks>
        /// This method updates the FilteredLocation property without consideration for SampleCount.
        /// </remarks>
        public override Position Filter(Position gpsPosition)
        {
            Position3D pos3d = Filter(new Position3D(gpsPosition));
            return new Position(pos3d.Latitude, pos3d.Longitude);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition"> The new observation to add to the filter. </param>
        /// <param name="deviceError"> Does not currently affect position averaging. </param>
        /// <param name="horizontalDOP"> Does not currently affect position averaging. </param>
        /// <param name="verticalDOP"> Does not currently affect position averaging. </param>
        /// <param name="bearing"> Does not currently affect position averaging. </param>
        /// <param name="speed"> Does not currently affect position averaging. </param>
        /// <remarks>
        /// This method updates the FilteredLocation property without consideration for SampleCount.
        /// </remarks>
        public override Position Filter(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            Position3D pos3d = Filter(new Position3D(gpsPosition), deviceError, horizontalDOP, verticalDOP, bearing, speed);
            return new Position(pos3d.Latitude, pos3d.Longitude);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition"> The new observation to add to the filter. </param>
        /// <returns> The filtered position. </returns>
        /// <remarks>
        /// This method updates the FilteredLocation property without consideration for SampleCount.
        /// </remarks>
        public override Position3D Filter(Position3D gpsPosition)
        {
            return Filter(gpsPosition);
        }

        /// <summary>
        /// Adds a new observation and applies the filter.
        /// </summary>
        /// <param name="gpsPosition"> The new observation to add to the filter. </param>
        /// <param name="deviceError"> Does not currently affect position averaging. </param>
        /// <param name="horizontalDOP"> Does not currently affect position averaging. </param>
        /// <param name="verticalDOP"> Does not currently affect position averaging. </param>
        /// <param name="bearing"> Does not currently affect position averaging. </param>
        /// <param name="speed"> Does not currently affect position averaging. </param>
        /// <remarks>
        /// This method updates the FilteredLocation property without consideration for SampleCount.
        /// </remarks>
        public override Position3D Filter(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            this._samples.Add(gpsPosition);
            this._sampleTimes.Add(DateTime.Now);

            int count = this._samples.Count;
            int timeCount = this._sampleTimes.Count;
            int maxCount = 0;

            // Only average the number of samples specified in the constructor
            while (count > _sampleCount)
            {
                this._samples.RemoveAt(0);
                count--;
                maxCount++;
            }

            // Only 2 times are needed, oldest and most recent.
            // Try to remove as many as were removed from the sample collection.
            while (timeCount > 2 && maxCount > 0)
            {
                this._sampleTimes.RemoveAt(0);
                timeCount--;
            }

            Filter();

            return _filteredPositon;
        }
    }
}

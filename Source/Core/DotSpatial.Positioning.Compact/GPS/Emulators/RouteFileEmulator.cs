using System;
using System.IO;

namespace DotSpatial.Positioning.Gps.Emulators
{
    /// <summary>
    /// This emulator follows a custom route defined in a text file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This emulator is similar to the <see cref="TextFileEmulator"/>, except that it does not require real
    /// GPS data. Instead, it expects a text file that contains a <see cref="Position"/> value on each line
    /// (e.g. HH°MM'SS.SSSS,HH°MM'SS.SSSS).
    /// </para>
    /// <para>
    /// The <see cref="RouteFileEmulator"/> will move one point further along its route at each 
    /// <see cref="Emulator.Interval"/> period, so the <see cref="Emulator.Speed"/> will automatically adjust
    /// as needed to move the required distance each time.
    /// </para>
    /// </remarks>
    public class RouteFileEmulator : NmeaEmulator
    {
        private readonly TimeSpan _Interval;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteFileEmulator"/> class that reads
        /// its route from the specified file.
        /// </summary>
        /// <param name="filePath">The absolute or relative path of the file containing the route.</param>
        public RouteFileEmulator(string filePath)
            : this(filePath, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteFileEmulator"/> class that reads
        /// its route from the specified file in the specified direction.
        /// </summary>
        /// <param name="filePath">The absolute or relative path of the file containing the route.</param>
        /// <param name="reverse">
        /// If <see langword="true"/>, the route file will be read in reverse order. That is, the last
        /// <see cref="Position"/> in the file will be the starting point of the route.
        /// </param>
        public RouteFileEmulator(string filePath, bool reverse)
            : this(filePath, reverse, TimeSpan.FromSeconds(1))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteFileEmulator"/> class that reads
        /// its route from the specified file in the specified direction at the specified interval.
        /// </summary>
        /// <param name="filePath">The absolute or relative path of the file containing the route.</param>
        /// <param name="reverse">
        /// If <see langword="true"/>, the route file will be read in reverse order. That is, the last
        /// <see cref="Position"/> in the file will be the starting point of the route.
        /// </param>
        /// <param name="interval">The amount of time that should elapse between each point in the route.</param>
        public RouteFileEmulator(string filePath, bool reverse, TimeSpan interval)
        {
            ReadRoute(filePath, reverse);
            _Interval = interval;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the amount of time the emulator waits before processing new data.
        /// </summary>
        public override TimeSpan Interval
        {
            get { return _Interval; }
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Moves to the next point in the route, and calls the base <see cref="NmeaEmulator"/> 
        /// implementation to generate the appropriate NMEA sentences.
        /// </summary>
        protected override void OnEmulation()
        {
            // Determine the distance to the next point on the route
            Distance distanceToNextPoint = CurrentPosition.DistanceTo(CurrentDestination);

            if (distanceToNextPoint.Value > 0)
            {
                // Pad the distance by 1% to account for rounding inaccuracies that can occur
                // when the base Emulator class calculates the distance based on the speed
                distanceToNextPoint = new Distance(distanceToNextPoint.Value * 1.01, distanceToNextPoint.Units);

                // Adjust the Speed to cover the required distance in a single Interval period
                Speed = distanceToNextPoint.ToSpeed(Interval);
            }

            base.OnEmulation();
        }

        /// <summary>
        /// Reads the route file and populates the <see cref="Emulator.Route"/> property.
        /// </summary>
        /// <param name="filePath">The absolute or relative path of the file containing the route.</param>
        /// <param name="reverse">
        /// If <see langword="true"/>, the route file will be read in reverse order. That is, the last
        /// <see cref="Position"/> in the file will be the starting point of the route.
        /// </param>
        private void ReadRoute(string filePath, bool reverse)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Split the file into an array of lines
                    string[] lines = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray());

                    // Determine the order to read the lines
                    int start = reverse ? lines.Length - 1 : 0;
                    int end = reverse ? 0 : lines.Length - 1;

                    for (int i = start; i != end;)
                    {
                        string line = lines[i];

                        // Ignore blank lines
                        if (!String.IsNullOrEmpty(line))
                        {
                            // Convert the line to a Position struct
                            Position position = new Position(line);

                            if (i == start)
                            {
                                // Set the CurrentPosition and CurrentDestination properties
                                // Note: This must be done *before* populating the Route property, or an exception will occur
                                CurrentPosition = position;
                                CurrentDestination = position;
                            }

                            // Add this point to the route
                            Route.Add(position);
                        }

                        // Move to the next (or previous) line
                        i += reverse ? -1 : 1;
                    }
                }
            }
        }

        #endregion
    }
}
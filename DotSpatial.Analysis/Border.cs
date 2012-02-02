namespace DotSpatial.Analysis
{
    /// <summary>
    /// Used to represent a line segment.
    /// </summary>
    public struct Border
    {
        /// <summary>
        /// Gets or sets the x1.
        /// </summary>
        /// <value>
        /// The x1.
        /// </value>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the x2.
        /// </summary>
        /// <value>
        /// The x2.
        /// </value>
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the M.
        /// </summary>
        /// <value>
        /// The M.
        /// </value>
        public double M { get; set; }

        /// <summary>
        /// Gets or sets the Q.
        /// </summary>
        /// <value>
        /// The Q.
        /// </value>
        public double Q { get; set; }
    }
}
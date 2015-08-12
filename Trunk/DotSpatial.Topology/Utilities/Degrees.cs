namespace DotSpatial.Topology.Utilities
{
	/// <summary>
	/// Converts degrees to radians.
	/// </summary>
	public class Degrees
	{
	    #region Methods

	    /// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">The angle in degrees.</param>
		/// <returns>The angle in radians.</returns>
		public static double ToRadians(double degrees)
		{
			return degrees * 0.0174532925199432958;
		}

	    #endregion
	}
}

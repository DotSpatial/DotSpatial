namespace DotSpatial.Projections
{
    internal static class ProjectionInfoExtensions
    {
        /// <summary>
        /// Sets names for given projection info
        /// </summary>
        /// <param name="pi">Projection Info</param>
        /// <param name="projectionName">Projection name</param>
        /// <param name="geograficInfoName">Geografic info name</param>
        /// <param name="datumName">Geografic info datu name</param>
        public static ProjectionInfo SetNames(this ProjectionInfo pi, string projectionName, string geograficInfoName, string datumName)
        {
            pi.Name = projectionName;
            pi.GeographicInfo.Name = geograficInfoName;
            pi.GeographicInfo.Datum.Name = datumName;
            return pi;
        }
    }
}
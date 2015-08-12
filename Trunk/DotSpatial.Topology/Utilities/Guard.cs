using System;

namespace DotSpatial.Topology.Utilities
{
    public static class Guard
    {
        #region Methods

        public static void IsNotNull(object candidate, string propertyName)
        {
            if (candidate == null)
                throw new ArgumentNullException(propertyName);
        }

        #endregion
    }
}

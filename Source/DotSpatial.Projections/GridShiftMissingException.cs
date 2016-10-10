using System;
using System.Runtime.Serialization;

namespace DotSpatial.Projections
{
    [Serializable]
    public class GridShiftMissingException : Exception
    {
        public GridShiftMissingException()
            : base("Grid shift is missing. For details go to https://github.com/DotSpatial/DotSpatial/wiki/Grid-Shifts")
        {
        }

        protected GridShiftMissingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
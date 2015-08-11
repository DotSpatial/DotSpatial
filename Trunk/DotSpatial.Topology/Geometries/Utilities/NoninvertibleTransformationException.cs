using System;

namespace DotSpatial.Topology.Geometries.Utilities
{
    ///<summary>
    /// Indicates that an <see cref="AffineTransformation"/> is non-invertible.
    ///</summary>
    ///<author>Martin Davis</author>
    public class NoninvertibleTransformationException : Exception
    {
        #region Constructors

        public NoninvertibleTransformationException()
        {
        }

        public NoninvertibleTransformationException(string transformationIsNonInvertible)
            :base(transformationIsNonInvertible)
        {
                
        }

        #endregion
    }
}
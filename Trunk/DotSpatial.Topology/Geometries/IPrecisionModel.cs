using System;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Interface for classes specifying the precision model of the <c>Coordinate</c>s in a <c>IGeometry</c>.
    /// In other words, specifies the grid of allowable points for all <c>IGeometry</c>s.
    /// </summary>
    public interface IPrecisionModel : IComparable, IComparable<IPrecisionModel>, IEquatable<IPrecisionModel>
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating if this precision model has floating precision
        /// </summary>
        bool IsFloating { get; }

        /// <summary>
        /// Gets a value indicating the maximum precision digits
        /// </summary>
        int MaximumSignificantDigits { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="PrecisionModelType">precision model</see> type
        /// </summary>
        PrecisionModelType PrecisionModelType { get; }

        /// <summary>
        /// Gets a value indicating the scale factor of a fixed precision model
        /// </summary>
        /// <remarks>
        /// The number of decimal places of precision is
        /// equal to the base-10 logarithm of the scale factor.
        /// Non-integral and negative scale factors are supported.
        /// Negative scale factors indicate that the places
        /// of precision is to the left of the decimal point.
        /// </remarks>
        double Scale { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Function to compute a precised value of <paramref name="val"/>
        /// </summary>
        /// <param name="val">The value to precise</param>
        /// <returns>The precised value</returns>
        double MakePrecise(double val);

        /// <summary>
        /// Method to precise <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord">The coordinate to precise</param>
        void MakePrecise(Coordinate coord);

        #endregion
    }
}

using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Precision
{
    ///<summary>
    /// Reduces the precision of the <see cref="Coordinate"/>s in a
    /// <see cref="ICoordinateSequence"/> to match the supplied <see cref="IPrecisionModel"/>.
    ///</summary>
    /// <remarks>
    /// Uses <see cref="IPrecisionModel.MakePrecise(double)"/>.
    /// The input is modified in-place, so
    /// it should be cloned beforehand if the
    /// original should not be modified.
    /// </remarks>
    /// <author>mbdavis</author>
    public class CoordinatePrecisionReducerFilter : ICoordinateSequenceFilter
    {
        #region Fields

        private readonly IPrecisionModel _precModel;

        #endregion

        #region Constructors

        ///<summary>
        /// Creates a new precision reducer filter.
        ///</summary>
        /// <param name="precModel">The PrecisionModel to use</param>
        public CoordinatePrecisionReducerFilter(IPrecisionModel precModel)
        {
            _precModel = precModel;
        }

        #endregion

        #region Properties

        ///<summary>
        /// Always runs over all geometry components.
        ///</summary>
        public bool Done { get { return false; } }

        ///<summary>
        /// Always reports that the geometry has changed
        ///</summary>
        public bool GeometryChanged { get { return true; } }

        #endregion

        #region Methods

        ///<summary>
        /// Rounds the Coordinates in the sequence to match the PrecisionModel
        ///</summary>
        public void Filter(ICoordinateSequence seq, int i)
        {
            seq.SetOrdinate(i, Ordinate.X, _precModel.MakePrecise(seq.GetOrdinate(i, Ordinate.X)));
            seq.SetOrdinate(i, Ordinate.Y, _precModel.MakePrecise(seq.GetOrdinate(i, Ordinate.Y)));
        }

        #endregion
    }
}
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology
{
    ///// <summary>
    ///// Delegate function to get a coordinate system from a given initialization string
    ///// </summary>
    ///// <param name="init">The initialization string</param>
    ///// <typeparam name="TCoordinateSystem">The type of the coordinate sytem.</typeparam>
    //[Obsolete("Not used", true)]
    //public delegate TCoordinateSystem GetCoordinateSystemDelegate<TCoordinateSystem>(string init);
    
    /// <summary>
    /// An interface for classes that offer access to geometry creating facillities.
    /// </summary>
    public interface IGeometryServices
    {
        #region Properties

        /// <summary>
        /// Gets or sets the coordiate sequence factory to use
        /// </summary>
        ICoordinateSequenceFactory DefaultCoordinateSequenceFactory { get; }

        /// <summary>
        /// Gets or sets the default precision model
        /// </summary>
        IPrecisionModel DefaultPrecisionModel { get; }

        /// <summary>
        /// Gets the default spatial reference id
        /// </summary>
        int DefaultSrid { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new geometry factory, using <see cref="DefaultPrecisionModel"/>, <see cref="DefaultSrid"/> and <see cref="DefaultCoordinateSequenceFactory"/>.
        /// </summary>
        /// <returns>The geometry factory</returns>
        IGeometryFactory CreateGeometryFactory();

        /// <summary>
        /// Creates a geometry fractory using <see cref="DefaultPrecisionModel"/> and <see cref="DefaultCoordinateSequenceFactory"/>.
        /// </summary>
        /// <param name="srid"></param>
        /// <returns>The geometry factory</returns>
        IGeometryFactory CreateGeometryFactory(int srid);

        /// <summary>
        /// Creates a geometry factory using the given <paramref name="coordinateSequenceFactory"/> along with <see cref="DefaultPrecisionModel"/> and <see cref="DefaultSrid"/>.
        /// </summary>
        /// <param name="coordinateSequenceFactory">The coordinate sequence factory to use.</param>
        /// <returns>The geometry factory.</returns>
        IGeometryFactory CreateGeometryFactory(ICoordinateSequenceFactory coordinateSequenceFactory);

        /// <summary>
        /// Creates a geometry factory using the given <paramref name="precisionModel"/> along with <see cref="DefaultCoordinateSequenceFactory"/> and <see cref="DefaultSrid"/>.
        /// </summary>
        /// <param name="precisionModel">The coordinate sequence factory to use.</param>
        /// <returns>The geometry factory.</returns>
        IGeometryFactory CreateGeometryFactory(IPrecisionModel precisionModel);

        /// <summary>
        /// Creates a geometry factory using the given <paramref name="precisionModel"/> along with <see cref="DefaultCoordinateSequenceFactory"/> and <see cref="DefaultSrid"/>.
        /// </summary>
        /// <param name="precisionModel">The coordinate sequence factory to use.</param>
        /// <param name="srid">The spatial reference id.</param>
        /// <returns>The geometry factory.</returns>
        IGeometryFactory CreateGeometryFactory(IPrecisionModel precisionModel, int srid);

        /// <summary>
        /// Creates a geometry factory using the given <paramref name="precisionModel"/>,
        /// <paramref name="srid"/> and <paramref name="coordinateSequenceFactory"/>.
        /// </summary>
        /// <param name="precisionModel">The coordinate sequence factory to use.</param>
        /// <param name="srid">The spatial reference id.</param>
        /// <param name="coordinateSequenceFactory">The coordinate sequence factory.</param>
        /// <returns>The geometry factory.</returns>
        IGeometryFactory CreateGeometryFactory(IPrecisionModel precisionModel, int srid,ICoordinateSequenceFactory coordinateSequenceFactory);

        /// <summary>
        /// Creates a precision model based on given precision model type
        /// </summary>
        /// <returns>The precision model type</returns>
        IPrecisionModel CreatePrecisionModel(PrecisionModelType modelType);

        /// <summary>
        /// Creates a precision model based on given precision model.
        /// </summary>
        /// <returns>The precision model</returns>
        IPrecisionModel CreatePrecisionModel(IPrecisionModel modelType);

        /// <summary>
        /// Creates a precision model based on the given scale factor.
        /// </summary>
        /// <param name="scale">The scale factor</param>
        /// <returns>The precision model.</returns>
        IPrecisionModel CreatePrecisionModel(double scale);

        /// <summary>
        /// Reads the configuration from the configuration
        /// </summary>
        void ReadConfiguration();

        /// <summary>
        /// Writes the current configuration to the configuration
        /// </summary>
        void WriteConfiguration();

        #endregion
    }
}
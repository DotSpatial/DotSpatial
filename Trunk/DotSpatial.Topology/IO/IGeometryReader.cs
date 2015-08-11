using System.IO;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.IO
{
    /// <summary>
    /// Interface for input/parsing of <see cref="IGeometry"/> instances.
    /// </summary>
    /// <typeparam name="TSource">The type of the source to read from.</typeparam>
    public interface IGeometryReader<TSource> : IGeometryIOSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets whether invalid linear rings should be fixed
        /// </summary>
        bool RepairRings { get; set; }

        #endregion

        #region Methods

        ///// <summary>
        ///// Gets or sets the geometry factory used to create the parsed geometries
        ///// </summary>
        //[Obsolete]
        //IGeometryFactory Factory { get; set; }

        /*
        /// <summary>
        /// Gets the coordinate sequence factory
        /// </summary>
        ICoordinateSequenceFactory CoordinateSequenceFactory { get; }

        /// <summary>
        /// Gets the precision model
        /// </summary>
        IPrecisionModel PrecisionModel { get; }
         */

        /// <summary>
        /// Reads a geometry representation from a <typeparamref name="TSource"/> to a <c>Geometry</c>.
        /// </summary>
        /// <param name="source">
        /// The source to read the geometry from
        /// <para>For WKT <typeparamref name="TSource"/> is <c>string</c>, </para>
        /// <para>for WKB <typeparamref name="TSource"/> is <c>byte[]</c>, </para>
        /// </param>
        /// <returns>
        /// A <c>Geometry</c>
        /// </returns>
        IGeometry Read(TSource source);

        /// <summary>
        /// Reads a geometry representation from a <see cref="Stream"/> to a <c>Geometry</c>.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// A <c>Geometry</c>
        IGeometry Read(Stream stream);

        #endregion
    }

    /// <summary>
    /// Interface for textual input of <see cref="IGeometry"/> instances.
    /// </summary>
    public interface ITextGeometryReader : IGeometryReader<string>
    {}

    /// <summary>
    /// Interface for binary input of <see cref="IGeometry"/> instances.
    /// </summary>
    public interface IBinaryGeometryReader : IGeometryReader<byte[]>
    {}
}
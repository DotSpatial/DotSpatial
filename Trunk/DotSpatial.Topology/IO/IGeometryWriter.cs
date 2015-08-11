using System.IO;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.IO
{
    /// <summary>
    /// Base interface for geometry reader or writer interfaces.
    /// </summary>
    public interface IGeometryIOSettings
    {
        #region Properties

        /// <summary>
        /// Gets and <see cref="Ordinates"/> flag that indicate which ordinates can be handled.
        /// </summary>
        /// <remarks>
        /// This flag must always return at least <see cref="Ordinates.XY"/>.
        /// </remarks>
        Ordinates AllowedOrdinates { get; }

        /// <summary>
        /// Gets and sets <see cref="Ordinates"/> flag that indicate which ordinates shall be handled.
        /// </summary>
        /// <remarks>
        /// No matter which <see cref="Ordinates"/> flag you supply, <see cref="Ordinates.XY"/> are always processed,
        /// the rest is binary and 'ed with <see cref="AllowedOrdinates"/>.
        /// </remarks>
        Ordinates HandleOrdinates { get; set; }

        /// <summary>
        /// Gets or sets whether the SpatialReference ID must be handled.
        /// </summary>
        bool HandleSRID { get; set; }

        #endregion
    }
    
    /// <summary>
    /// Interface for binary output of <see cref="IGeometry"/> instances.
    /// </summary>
    /// <typeparam name="TSink">The type of the output to produce.</typeparam>
    public interface IGeometryWriter<TSink> : IGeometryIOSettings
    {
        #region Methods

        /// <summary>
        /// Writes a binary representation of a given geometry.
        /// </summary>
        /// <param name="geometry">The geometry</param>
        /// <returns>The binary representation of <paramref name="geometry"/></returns>
        TSink Write(IGeometry geometry);

        /// <summary>
        /// Writes a binary representation of a given geometry.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="stream"></param>
        void Write(IGeometry geometry, Stream stream);

        #endregion
    }

    /// <summary>
    /// Interface for binary output of <see cref="IGeometry"/> instances.
    /// </summary>
    public interface IBinaryGeometryWriter : IGeometryWriter<byte[]>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the desired <see cref="ByteOrder"/>
        /// </summary>
        ByteOrder ByteOrder { get; set; }

        #endregion
    }
    
    /// <summary>
    /// Interface for textual output of <see cref="IGeometry"/> instances.
    /// </summary>
    public interface ITextGeometryWriter : IGeometryWriter<string>
    {}

}
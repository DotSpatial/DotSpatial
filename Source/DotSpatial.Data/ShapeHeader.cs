// ********************************************************************************************************
// Product Name: DotSpatial.Data.Vectors.Shapefiles.dll Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from DotSpatial.Data.Vectors.Shapefiles.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// A simple structure that contains the elements of a shapefile that must exist.
    /// </summary>
    public struct ShapeHeader
    {
        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the offset in 16-bit words.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets the offset in bytes.
        /// </summary>
        public int ByteOffset => Offset * 2;

        /// <summary>
        /// Gets the length in bytes.
        /// </summary>
        public int ByteLength => ContentLength * 2;
    }
}
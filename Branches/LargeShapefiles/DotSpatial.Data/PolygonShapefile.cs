// *******************************************************************************************************
// Product: DotSpatial.Data.PolygonShapefile.cs
// Description:  A shapefile class that handles the special case where the data is made up of polygons.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  07/2014           | Created.
// *******************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the data is made up of polygons
    /// </summary>
    public class PolygonShapefile : LineShapefile
    {
        #region Ctor

        /// <summary>
        /// Creates a new instance of a PolygonShapefile for in-ram handling only.
        /// </summary>
        public PolygonShapefile()
            : this(null)
            
        {
        }

        /// <summary>
        /// Creates a new instance of a PolygonShapefile that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public PolygonShapefile(string fileName)
            : base(fileName, true)
        {

        }

        #endregion
    }
}
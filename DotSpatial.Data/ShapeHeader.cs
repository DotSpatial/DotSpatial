// ********************************************************************************************************
// Product Name: DotSpatial.Data.Vectors.Shapefiles.dll Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// A simple structure that contains the elements of a shapefile that must exist
    /// </summary>
    public struct ShapeHeader
    {
        /// <summary>
        /// The content length
        /// </summary>
        public int ContentLength;

        /// <summary>
        /// The offset in 16-bit words
        /// </summary>
        public int Offset;

        /// <summary>
        /// The offset in bytes
        /// </summary>
        public int ByteOffset
        {
            get { return Offset * 2; }
        }

        /// <summary>
        /// The length in bytes
        /// </summary>
        public int ByteLength
        {
            get { return ContentLength * 2; }
        }
    }
}
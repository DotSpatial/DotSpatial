// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 10:04:07 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.IO;
using System.Xml.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// PyramidHeader
    /// </summary>
    [XmlRoot("PyramidHeader")]
    public class PyramidHeader
    {
        #region Private Variables

        private PyramidImageHeader[] _imageHeaders;

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the header size for this
        /// </summary>
        /// <returns></returns>
        public long HeaderSize()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer s = new XmlSerializer(typeof(PyramidHeader));
            s.Serialize(ms, this);
            return ms.Length;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the array that encompasses all of the basic header content
        /// necessary for working with this image.
        /// </summary>
        [XmlElement("ImageHeaders")]
        public PyramidImageHeader[] ImageHeaders
        {
            get { return _imageHeaders; }
            set { _imageHeaders = value; }
        }

        #endregion
    }
}
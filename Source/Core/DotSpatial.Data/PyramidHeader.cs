// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using System.Xml.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// PyramidHeader.
    /// </summary>
    [XmlRoot("PyramidHeader")]
    public class PyramidHeader
    {
        #region Properties

        /// <summary>
        /// Gets or sets the array that encompasses all of the basic header content
        /// necessary for working with this image.
        /// </summary>
        [XmlElement("ImageHeaders")]
        public PyramidImageHeader[] ImageHeaders { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the header size for this.
        /// </summary>
        /// <returns>The header size.</returns>
        public long HeaderSize()
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer s = new XmlSerializer(typeof(PyramidHeader));
            s.Serialize(ms, this);
            return ms.Length;
        }

        #endregion
    }
}
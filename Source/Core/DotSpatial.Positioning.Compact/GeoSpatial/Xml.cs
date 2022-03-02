using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DotSpatial.Positioning
{
    internal static class Xml
    {
        /// <summary>
        /// Returns the XML namespace for GML documents.
        /// </summary>
        public const string GmlXmlNamespace = "http://www.opengis.net/gml";

        /// <summary>
        /// Returns the prefix applied to all GML XML elements.
        /// </summary>
        public const string GmlXmlPrefix = "gml";

        /// <summary>
        /// Returns the XML namespace for DotSpatial.Positioning documents.
        /// </summary>
        public const string DotSpatialPositioningXmlNamespace = "http://dotspatial.codeplex.com/gml";

        /// <summary>
        /// Returns the prefix applied to all DotSpatial.Positioning XML elements.
        /// </summary>
        public const string DotSpatialPositioningXmlPrefix = "DotSpatial.Positioning";

#if Framework30

        /// <summary>
        /// Used to test the <see cref="IXmlSerializable.WriteXml"/> implementations of DotSpatial.Positioning types.
        /// </summary>
        internal static string Serialize<T>(T obj)
            where T: IXmlSerializable
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Used to test the <see cref="IXmlSerializable.ReadXml"/> implementations of DotSpatial.Positioning types.
        /// </summary>
        internal static T Deserialize<T>(string xml)
            where T : IXmlSerializable, new()
        {
            using (StringReader reader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

#endif
    }
}

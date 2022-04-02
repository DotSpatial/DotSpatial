// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

using System.IO;
using System.Xml.Serialization;

namespace DotSpatial.Positioning
{
    /// <summary>
    ///
    /// </summary>
    internal static class Xml
    {
        /// <summary>
        /// Returns the XML namespace for GML documents.
        /// </summary>
        public const string GML_XML_NAMESPACE = "http://www.opengis.net/gml";

        /// <summary>
        /// Returns the prefix applied to all GML XML elements.
        /// </summary>
        public const string GML_XML_PREFIX = "gml";

        /// <summary>
        /// Returns the XML namespace for DotSpatial.Positioning documents.
        /// </summary>
        public const string DOT_SPATIAL_POSITIONING_XML_NAMESPACE = "http://dotspatial.codeplex.com/gml";

        /// <summary>
        /// Returns the prefix applied to all DotSpatial.Positioning XML elements.
        /// </summary>
        public const string DOT_SPATIAL_POSITIONING_XML_PREFIX = "DotSpatial.Positioning";

#if Framework30

        /// <summary>
        /// Used to test the <see cref="IXmlSerializable.WriteXml"/> implementations of DotSpatial.Positioning types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        internal static string Serialize<T>(T obj)
    where T : IXmlSerializable
        {
            using StringWriter writer = new();
            XmlSerializer serializer = new(typeof(T));
            serializer.Serialize(writer, obj);

            return writer.ToString();
        }

        /// <summary>
        /// Used to test the <see cref="IXmlSerializable.ReadXml"/> implementations of DotSpatial.Positioning types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        internal static T Deserialize<T>(string xml)
    where T : IXmlSerializable, new()
        {
            using StringReader reader = new(xml);
            XmlSerializer serializer = new(typeof(T));
            return (T)serializer.Deserialize(reader);
        }

#endif
    }
}
// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
    }
}
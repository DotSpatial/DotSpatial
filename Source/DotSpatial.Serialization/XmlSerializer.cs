// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Darrel Brown. Created 9/10/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************
// This code looks like an early version of http://www.codeproject.com/KB/XML/deepserializer.aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// Serializes data to XML. A valid serialization map must be available for all classes being serialized.
    /// The serialization map is generally created at runtime from fields and properties marked with the
    /// <see cref="SerializeAttribute"/>. For classes that cannot be directly marked up with attributes a
    /// new map class can be created explicitly as needed.
    /// </summary>
    public class XmlSerializer
    {
        #region Fields

        private readonly object _lockObject = new object();

        private int _nextTypeId;
        private Dictionary<Type, int> _typeCache;
        private ObjectXmlReferences _visitedObjects;

        #endregion

        #region Methods

        /// <summary>
        /// Converts an object into XML.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A string containing the serialization of the given object.</returns>
        public string Serialize(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            lock (_lockObject)
            {
                // Don't allow reentrance
                _visitedObjects = new ObjectXmlReferences();
                _typeCache = new Dictionary<Type, int>();

                var document = new XDocument(WriteValue(XmlConstants.Root, null, value));
                document.Root?.AddFirst(WriteTypeCache());
                string result = ConvertDocumentToString(document);

                CleanUp();

                return result;
            }
        }

        private void CleanUp()
        {
            _visitedObjects.Clear();
            _visitedObjects = null;
        }

        private string ConvertDocumentToString(XDocument document)
        {
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                document.Save(sw);
                sw.Flush();

                return sw.ToString();
            }
        }

        private XElement CreateElement(string elementName, object value, SerializeAttribute attribute, params object[] content)
        {
            Type type = value.GetType();

            var result = new XElement(elementName, content);
            result.Add(new XAttribute(XmlConstants.TypeId, GetTypeId(type).ToString()));

            if (attribute != null && attribute.ConstructorArgumentIndex >= 0) result.Add(new XAttribute(XmlConstants.Arg, attribute.ConstructorArgumentIndex));

            if (!type.IsValueType && !type.Equals(typeof(string))) // Don't cache value types or strings
                _visitedObjects.Add(value, result);

            return result;
        }

        private XAttribute[] GetFormattedValue(Type formatterType, object value)
        {
            var ctor = formatterType.GetConstructor(Type.EmptyTypes);
            var formatter = (SerializationFormatter)ctor.Invoke(null);

            // var formatter = (SerializationFormatter)formatterType.Assembly.CreateInstance(formatterType.AssemblyQualifiedName);
            return new[] { new XAttribute(XmlConstants.Value, XmlHelper.EscapeInvalidCharacters(formatter.ToString(value))), new XAttribute(XmlConstants.Formatter, GetTypeId(formatterType).ToString()), };
        }

        private int GetTypeId(Type type)
        {
            int typeId;
            if (!_typeCache.TryGetValue(type, out typeId))
            {
                typeId = _nextTypeId;
                _typeCache[type] = typeId;
                _nextTypeId++;
            }

            return typeId;
        }

        private string GetTypeString(Type type)
        {
            // Remove the "strong name" portions of any types that aren't signed
            // TODO: Extend this to omit the strong name signiture for assemblies that "opt out" through an assembly-level attribute
            return Regex.Replace(type.AssemblyQualifiedName, ", Version=\\S+ Culture=\\S+ PublicKeyToken=null", string.Empty);
        }

        private void WriteDictionaryElements(XElement parent, IDictionary values)
        {
            foreach (DictionaryEntry entry in values)
            {
                // TODO: Handle null values
                var element = new XElement(XmlConstants.DictionaryEntry, WriteValue(XmlConstants.DictionaryKey, null, entry.Key), WriteValue(XmlConstants.DictionaryValue, null, entry.Value));
                parent.Add(element);
            }
        }

        private void WriteListElements(XElement parent, IEnumerable values)
        {
            foreach (object item in values)
            {
                if (item == null) continue;

                parent.Add(WriteValue(XmlConstants.Item, null, item));
            }
        }

        private void WriteMembers(XElement parent, IEnumerable<SerializationMapEntry> members, object value)
        {
            foreach (SerializationMapEntry entry in members)
            {
                object childValue;
                var info = entry.Member as PropertyInfo;
                if (info != null)
                {
                    childValue = info.GetValue(value, null);
                }
                else
                {
                    var member = entry.Member as FieldInfo;
                    if (member != null)
                    {
                        childValue = member.GetValue(value);
                    }
                    else
                    {
                        throw new InvalidOperationException("Only fields and properties are supported.");
                    }
                }

                if (childValue == null) continue;

                string memberName = !string.IsNullOrEmpty(entry.Attribute?.Name) ? entry.Attribute.Name : entry.Member.Name;
                XElement e = WriteValue(XmlConstants.Member, entry.Attribute, childValue, new XAttribute(XmlConstants.Name, memberName));
                parent.Add(e);
            }
        }

        private XElement WriteTypeCache()
        {
            return new XElement(XmlConstants.TypeCache, _typeCache.Select(kvp => new XElement(XmlConstants.Item, new XAttribute(XmlConstants.Key, kvp.Value), new XAttribute(XmlConstants.Value, GetTypeString(kvp.Key)))));
        }

        private XElement WriteValue(string elementName, SerializeAttribute serializeAttribute, object value, params object[] content)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            XElement element;

            Type type = value.GetType();
            int refId = _visitedObjects.FindRefId(value);
            if (refId >= 0)
            {
                element = new XElement(elementName, content, new XAttribute(XmlConstants.Ref, refId.ToString(CultureInfo.InvariantCulture)));
                if (serializeAttribute != null && serializeAttribute.ConstructorArgumentIndex >= 0) element.Add(new XAttribute(XmlConstants.Arg, serializeAttribute.ConstructorArgumentIndex));
                return element;
            }

            element = CreateElement(elementName, value, serializeAttribute, content);
            if (serializeAttribute != null && serializeAttribute.Formatter != null)
            {
                element.Add(GetFormattedValue(serializeAttribute.Formatter, value));
            }
            else if (type.IsPrimitive)
            {
                element.Add(new XAttribute(XmlConstants.Value, Convert.ToString(value, CultureInfo.InvariantCulture)));
            }
            else if (type.IsEnum)
            {
                element.Add(new XAttribute(XmlConstants.Value, value.ToString()));
            }
            else if (value is string)
            {
                element.Add(new XAttribute(XmlConstants.Value, XmlHelper.EscapeInvalidCharacters((string)value)));
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                WriteDictionaryElements(element, (IDictionary)value);
            }
            else if (typeof(IList).IsAssignableFrom(type) || typeof(ICollection).IsAssignableFrom(type))
            {
                WriteListElements(element, (IEnumerable)value);
            }
            else if (type == typeof(DateTime))
            {
                element.Add(new XAttribute(XmlConstants.Value, Convert.ToString(value, CultureInfo.InvariantCulture)));
            }
            else if (type == typeof(Color))
            {
                element.Add(new XAttribute(XmlConstants.Value, ColorTranslator.ToHtml((Color)value)));
            }
            else if (type == typeof(PointF))
            {
                PointF p = (PointF)value;
                element.Add(new XAttribute(XmlConstants.Value, XmlHelper.EscapeInvalidCharacters(p.X + "|" + p.Y)));
            }
            else
            {
                var map = SerializationMap.FromType(value.GetType());
                WriteMembers(element, map.Members, value);
            }

            return element;
        }

        #endregion

        #region Classes

        private class ObjectXmlReferences
        {
            #region Fields

            private readonly List<KeyValuePair<object, XElement>> _objectReferences = new List<KeyValuePair<object, XElement>>();
            private int _nextId;

            #endregion

            #region Methods

            public void Add(object o, XElement element)
            {
                CheckForDuplicates(o);
                _objectReferences.Add(new KeyValuePair<object, XElement>(o, element));
            }

            public void Clear()
            {
                _objectReferences.Clear();
                _nextId = 0;
            }

            public int FindRefId(object o)
            {
                int result = -1;

                XElement element = _objectReferences.Where(kvp => ReferenceEquals(kvp.Key, o)).Select(kvp => kvp.Value).FirstOrDefault();
                if (element != null)
                {
                    XAttribute idAttribute = element.Attribute(XmlConstants.Id);
                    if (idAttribute == null)
                    {
                        element.Add(new XAttribute(XmlConstants.Id, _nextId.ToString(CultureInfo.InvariantCulture)));
                        result = _nextId;
                        _nextId++;
                    }
                    else
                    {
                        result = int.Parse(idAttribute.Value, CultureInfo.InvariantCulture);
                    }
                }

                return result;
            }

            [Conditional("DEBUG")]
            private void CheckForDuplicates(object o)
            {
                if (_objectReferences.Any(kvp => ReferenceEquals(kvp.Key, o))) throw new InvalidOperationException("Duplicate entry detected");
            }

            #endregion
        }

        #endregion
    }
}
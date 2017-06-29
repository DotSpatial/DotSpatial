// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// Serializes data to XML.  A valid serialization map must be available for all classes being serialized.
    /// The serialization map is generally created at runtime from fields and properties marked with the
    /// <see cref="SerializeAttribute"/>.  For classes that cannot be directly marked up with attributes a
    /// new map class can be created explicitly as needed.
    /// </summary>
    public class XmlSerializer
    {
        private readonly object _lockObject = new object();

        private int _nextTypeID;
        private Dictionary<Type, int> _typeCache;
        private ObjectXmlReferences _visitedObjects;

        /// <summary>
        /// Converts an object into XML.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A string containing the serialization of the given object.</returns>
        public string Serialize(object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            lock (_lockObject)
            {
                // Don't allow reentrance
                _visitedObjects = new ObjectXmlReferences();
                _typeCache = new Dictionary<Type, int>();

                var document = new XDocument(WriteValue(XmlConstants.ROOT, null, value));
                if (document.Root != null)
                    document.Root.AddFirst(WriteTypeCache());
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

        private XElement WriteValue(string elementName, SerializeAttribute serializeAttribute, object value, params object[] content)
        {
            if (value == null) throw new ArgumentNullException("value");

            XElement element;

            Type type = value.GetType();
            int refID = _visitedObjects.FindRefID(value);
            if (refID >= 0)
            {
                element = new XElement(elementName, content, new XAttribute(XmlConstants.REF, refID.ToString(CultureInfo.InvariantCulture)));
                if (serializeAttribute != null && serializeAttribute.ConstructorArgumentIndex >= 0)
                    element.Add(new XAttribute(XmlConstants.ARG, serializeAttribute.ConstructorArgumentIndex));
                return element;
            }

            element = CreateElement(elementName, value, serializeAttribute, content);
            if (serializeAttribute != null && serializeAttribute.Formatter != null)
            {
                element.Add(GetFormattedValue(serializeAttribute.Formatter, value));
            }
            else if (type.IsPrimitive)
            {
                element.Add(new XAttribute(XmlConstants.VALUE, Convert.ToString(value, CultureInfo.InvariantCulture)));
            }
            else if (type.IsEnum)
            {
                element.Add(new XAttribute(XmlConstants.VALUE, value.ToString()));
            }
            else if (value is string)
            {
                element.Add(new XAttribute(XmlConstants.VALUE, XmlHelper.EscapeInvalidCharacters((string)value)));
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
                element.Add(new XAttribute(XmlConstants.VALUE, Convert.ToString(value, CultureInfo.InvariantCulture)));
            }
            else if (type == typeof(Color))
            {
                element.Add(new XAttribute(XmlConstants.VALUE, ColorTranslator.ToHtml((Color)value)));
            }
            else if (type == typeof(PointF))
            {
                PointF p = (PointF)value;
                element.Add(new XAttribute(XmlConstants.VALUE, XmlHelper.EscapeInvalidCharacters(p.X + "|" + p.Y)));
            }
            else
            {
                var map = SerializationMap.FromType(value.GetType());
                WriteMembers(element, map.Members, value);
            }

            return element;
        }

        private XElement CreateElement(string elementName, object value, SerializeAttribute attribute, params object[] content)
        {
            Type type = value.GetType();

            var result = new XElement(elementName, content);
            result.Add(new XAttribute(XmlConstants.TYPE_ID, GetTypeID(type).ToString()));

            if (attribute != null && attribute.ConstructorArgumentIndex >= 0)
                result.Add(new XAttribute(XmlConstants.ARG, attribute.ConstructorArgumentIndex));

            if (!type.IsValueType && !type.Equals(typeof(string))) // Don't cache value types or strings
                _visitedObjects.Add(value, result);

            return result;
        }

        private int GetTypeID(Type type)
        {
            int typeID;
            if (!_typeCache.TryGetValue(type, out typeID))
            {
                typeID = _nextTypeID;
                _typeCache[type] = typeID;
                _nextTypeID++;
            }

            return typeID;
        }

        private XAttribute[] GetFormattedValue(Type formatterType, object value)
        {
            var ctor = formatterType.GetConstructor(Type.EmptyTypes);
            var formatter = (SerializationFormatter)ctor.Invoke(null);
            //var formatter = (SerializationFormatter)formatterType.Assembly.CreateInstance(formatterType.AssemblyQualifiedName);
            return new[]
			       	{
			       		new XAttribute(XmlConstants.VALUE, XmlHelper.EscapeInvalidCharacters(formatter.ToString(value))),
			       		new XAttribute(XmlConstants.FORMATTER, GetTypeID(formatterType).ToString()),
			       	};
        }

        private void WriteMembers(XElement parent, IEnumerable<SerializationMapEntry> members, object value)
        {
            foreach (SerializationMapEntry entry in members)
            {
                object childValue;
                if (entry.Member is PropertyInfo)
                    childValue = ((PropertyInfo)entry.Member).GetValue(value, null);
                else if (entry.Member is FieldInfo)
                    childValue = ((FieldInfo)entry.Member).GetValue(value);
                else
                    throw new InvalidOperationException("Only fields and properties are supported.");

                if (childValue == null)
                    continue;

                string memberName;
                if (entry.Attribute != null && !string.IsNullOrEmpty(entry.Attribute.Name))
                    memberName = entry.Attribute.Name;
                else
                    memberName = entry.Member.Name;

                XElement e = WriteValue(XmlConstants.MEMBER, entry.Attribute, childValue, new XAttribute(XmlConstants.NAME, memberName));

                parent.Add(e);
            }
        }

        private void WriteDictionaryElements(XElement parent, IDictionary values)
        {
            foreach (DictionaryEntry entry in values)
            {
                // TODO: Handle null values
                var element = new XElement(XmlConstants.DICTIONARY_ENTRY,
                                           WriteValue(XmlConstants.DICTIONARY_KEY, null, entry.Key),
                                           WriteValue(XmlConstants.DICTIONARY_VALUE, null, entry.Value));
                parent.Add(element);
            }
        }

        private void WriteListElements(XElement parent, IEnumerable values)
        {
            foreach (object item in values)
            {
                if (item == null)
                    continue;

                parent.Add(WriteValue(XmlConstants.ITEM, null, item));
            }
        }

        private XElement WriteTypeCache()
        {
            return new XElement(XmlConstants.TYPE_CACHE,
                                _typeCache.Select(kvp => new XElement(XmlConstants.ITEM,
                                                                      new XAttribute(XmlConstants.KEY, kvp.Value),
                                                                      new XAttribute(XmlConstants.VALUE, GetTypeString(kvp.Key)))));
        }

        private string GetTypeString(Type type)
        {
            // Remove the "strong name" portions of any types that aren't signed
            // TODO: Extend this to omit the strong name signiture for assemblies that "opt out" through an assembly-level attribute
            return Regex.Replace(type.AssemblyQualifiedName, ", Version=\\S+ Culture=\\S+ PublicKeyToken=null", string.Empty);
        }

        #region Nested type: ObjectXmlReferences

        private class ObjectXmlReferences
        {
            private readonly List<KeyValuePair<object, XElement>> _objectReferences = new List<KeyValuePair<object, XElement>>();
            private int _nextID;

            public int FindRefID(object o)
            {
                int result = -1;

                XElement element = _objectReferences.Where(kvp => ReferenceEquals(kvp.Key, o)).Select(kvp => kvp.Value).FirstOrDefault();
                if (element != null)
                {
                    XAttribute idAttribute = element.Attribute(XmlConstants.ID);
                    if (idAttribute == null)
                    {
                        element.Add(new XAttribute(XmlConstants.ID, _nextID.ToString(CultureInfo.InvariantCulture)));
                        result = _nextID;
                        _nextID++;
                    }
                    else
                        result = int.Parse(idAttribute.Value, CultureInfo.InvariantCulture);
                }

                return result;
            }

            public void Add(object o, XElement element)
            {
                CheckForDuplicates(o);
                _objectReferences.Add(new KeyValuePair<object, XElement>(o, element));
            }

            [Conditional("DEBUG")]
            private void CheckForDuplicates(object o)
            {
                if (_objectReferences.Any(kvp => ReferenceEquals(kvp.Key, o)))
                    throw new InvalidOperationException("Duplicate entry detected");
            }

            public void Clear()
            {
                _objectReferences.Clear();
                _nextID = 0;
            }
        }

        #endregion
    }
}
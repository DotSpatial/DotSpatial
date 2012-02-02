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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// Converts serialized XML into an object.
    /// </summary>
    public class XmlDeserializer
    {
        private Dictionary<string, object> _references;
        private Dictionary<string, Type> _typeCache;

        /// <summary>
        /// Converts the given XML string into an object.
        /// </summary>
        /// <typeparam name="T">The type to cast the returned object as.</typeparam>
        /// <param name="xml">The serialized XML text.</param>
        /// <returns>The object represented by the serialized XML.</returns>
        public T Deserialize<T>(string xml)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(xml));

            _references = new Dictionary<string, object>();

            try
            {
                using (StringReader stringReader = new StringReader(xml))
                {
                    XDocument document = XDocument.Load(stringReader);
                    XElement rootNode = document.Root;
                    if (rootNode == null)
                        throw new XmlException("Could not find a root XML element.");

                    _typeCache = ReadTypeCache(rootNode);
                    return (T)ReadObject(rootNode, null);
                }
            }
            finally
            {
                _references.Clear();
                if (_typeCache != null)
                {
                    _typeCache.Clear();
                    _typeCache = null;
                }
            }
        }

        /// <summary>
        /// Deserializes the given XML, writing the values into the given object.
        /// </summary>
        /// <typeparam name="T">The type of the given object.</typeparam>
        /// <param name="existingObject">An existing object to fill with data.</param>
        /// <param name="xml">The serialized XML text.</param>
        public void Deserialize<T>(T existingObject, string xml)
        {
            // Value types will never evaluate to null, and that's just fine.
            if (!typeof(T).IsValueType)
            {
                if (existingObject == null) throw new ArgumentNullException("existingObject");
            }
            _references = new Dictionary<string, object>();

            try
            {
                using (StringReader stringReader = new StringReader(xml))
                {
                    XDocument document = XDocument.Load(stringReader);
                    XElement rootNode = document.Root;
                    if (rootNode == null)
                        throw new XmlException("Could not find a root XML element.");

                    _typeCache = ReadTypeCache(rootNode);
                    FillObject(rootNode, existingObject, true);
                }
            }
            finally
            {
                _references.Clear();
                if (_typeCache != null)
                {
                    _typeCache.Clear();
                    _typeCache = null;
                }
            }
        }

        private static Dictionary<string, Type> ReadTypeCache(XElement rootNode)
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();
            TypeNameManager typeNameManager = new TypeNameManager();
            foreach (var typeNode in rootNode.Elements(XmlConstants.TYPE_CACHE).Elements(XmlConstants.ITEM))
            {
                var keyAttrib = typeNode.Attribute(XmlConstants.KEY);
                var valueAttrib = typeNode.Attribute(XmlConstants.VALUE);
                if (keyAttrib == null || valueAttrib == null)
                    continue;

                // string[] parts = valueAttrib.Value.Split(',');

                Type t;

                try
                {
                    // This method is too strict to handle auto-incrementing versioned dll references, but is
                    // needed to correctly grab core .Net Framework stuff and works faster for same-version dll files.
                    t = Type.GetType(valueAttrib.Value);
                }
                catch (FileLoadException)
                {
                    // Attempting to load the fully qualified name failed.  Search under more general terms
                    // and see if we can find a valid assembly with the specified type.
                    string updatedName = typeNameManager.UpdateTypename(valueAttrib.Value);
                    t = Type.GetType(updatedName);
                }
                result.Add(keyAttrib.Value, t);
            }

            return result;
        }

        /// <summary>
        /// Creates and populates an object from the given XML element.
        /// </summary>
        /// <param name="element">The element containing the object description</param>
        /// <param name="parent">The parent.</param>
        /// <returns>
        /// A populated object specified by the given XML element.
        /// </returns>
        private object ReadObject(XElement element, object parent)
        {
            // See if this element is an object reference
            var refElement = element.Attribute(XmlConstants.REF);
            if (refElement != null)
                return _references[refElement.Value];

            object result = GetObjectFromFormatter(element);
            if (result != null)
                return result;

            Type type = GetType(element);
            string value = GetValue(element);
            if (type == null)
            {
                // must be a plugin
                throw new ArgumentNullException("Couldn't find the assembly that contains the type that was serialized into that element");
            }
            else if (type.IsPrimitive)
            {
                result = value != "NaN" ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture) : Convert.ChangeType(Double.NaN, type);
            }
            else if (type.IsEnum)
            {
                result = Enum.Parse(type, value);
            }
            else if (type.Equals(typeof(string)))
            {
                result = XmlHelper.UnEscapeInvalidCharacters(value);
            }
            else if (type == typeof(DateTime))
            {
                result = Convert.ToDateTime(value);
            }
            else if (type.Equals(typeof(Color)))
            {
                result = ColorTranslator.FromHtml(value);
            }
            else if (type.Equals(typeof(PointF)))
            {
                string[] vals = XmlHelper.UnEscapeInvalidCharacters(value).Split('|');
                result = new PointF(float.Parse(vals[0]), float.Parse(vals[1]));
            }
            else
            {
                if (parent == null)
                    result = ConstructObject(type, element);
                else
                    result = parent;

                FillObject(element, result, false);
            }

            return result;
        }

        private void FillObject(XElement element, object parent, bool includeConstructorElements)
        {
            Type type = parent.GetType();
            var map = SerializationMap.FromType(type);

            var idAttribute = element.Attribute(XmlConstants.ID);
            if (idAttribute != null)
                _references[idAttribute.Value] = parent;

            // Populate the rest of the members
            var nonConstructorArgElements = from m in element.Elements(XmlConstants.MEMBER)
                                            where m.Attribute(XmlConstants.ARG) == null || includeConstructorElements
                                            select m;

            foreach (var member in nonConstructorArgElements)
            {
                string name = GetName(member);
                var mapEntry = map.Members.FirstOrDefault(m => m.Attribute.Name == name);
                if (mapEntry == null)
                    continue; // XML is probably out of date with the code if this happens

                if (mapEntry.Member is PropertyInfo)
                {
                    PropertyInfo propertyInfo = (PropertyInfo)mapEntry.Member;
                    if (propertyInfo.CanWrite)
                        propertyInfo.SetValue(parent, ReadObject(member, null), null);
                    else
                        FillObject(member, propertyInfo.GetValue(parent, null), false);
                }
                else
                {
                    ((FieldInfo)mapEntry.Member).SetValue(parent, ReadObject(member, null));
                }
            }
            if (type.IsArray)
            {
                PopulateArray(element, (Array)parent);
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                PopulateDictionary(element, (IDictionary)parent);
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                PopulateList(element, (IList)parent);
            }
        }

        private void PopulateArray(XElement element, Array array)
        {
            int index = 0;
            foreach (var item in element.Elements(XmlConstants.ITEM))
            {
                object newItem = ReadObject(item, array.GetValue(index));
                array.SetValue(newItem, index++);
            }
        }

        private void PopulateDictionary(XElement element, IDictionary dictionary)
        {
            if (dictionary.Count > 0)
                dictionary.Clear();

            foreach (var item in element.Elements(XmlConstants.DICTIONARY_ENTRY))
            {
                var keyElement = item.Element(XmlConstants.KEY);
                var valueElement = item.Element(XmlConstants.VALUE);
                if (keyElement == null || valueElement == null)
                    continue;

                object key = ReadObject(keyElement, null);
                object value = ReadObject(valueElement, null);

                if (key != null && value != null)
                    dictionary.Add(key, value);
            }
        }

        private void PopulateList(XElement element, IList list)
        {
            if (list.Count > 0)
                list.Clear();

            foreach (var item in element.Elements(XmlConstants.ITEM))
            {
                object newItem = ReadObject(item, null);
                if (newItem != null)
                    list.Add(newItem);
            }
        }

        private object GetObjectFromFormatter(XElement element)
        {
            var formatterAttrib = element.Attribute(XmlConstants.FORMATTER);
            if (formatterAttrib == null)
                return null;

            var valueAttrib = element.Attribute(XmlConstants.VALUE);
            if (valueAttrib == null)
                throw new XmlException("Missing value attribute for formattable element " + GetFullPath(element));

            var formatterType = _typeCache[formatterAttrib.Value];
            SerializationFormatter formatter = (SerializationFormatter)formatterType.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
            return formatter.FromString(XmlHelper.UnEscapeInvalidCharacters(valueAttrib.Value));
        }

        private Type GetType(XElement element)
        {
            var typeAttrib = element.Attribute(XmlConstants.TYPE_ID);
            if (typeAttrib == null)
                throw new XmlException("Missing type attribute for node " + GetFullPath(element));
            return _typeCache[typeAttrib.Value];
        }

        private static string GetValue(XElement element)
        {
            var valueAttrib = element.Attribute(XmlConstants.VALUE);
            if (valueAttrib == null)
                return null;
            return valueAttrib.Value;
        }

        private static string GetName(XElement element)
        {
            var nameAttrib = element.Attribute(XmlConstants.NAME);
            if (nameAttrib == null)
                throw new XmlException("Missing name attribute for node " + GetFullPath(element));
            return nameAttrib.Value;
        }

        private object ConstructObject(Type type, XElement element)
        {
            List<object> constructorArgs;
            if (type.IsArray)
            {
                int arrayLength = element.Elements(XmlConstants.ITEM).Count();
                constructorArgs = new List<object> { arrayLength };
            }
            else
            {
                constructorArgs = GetConstructorArgs(element);
            }
            Type[] types = constructorArgs.Select(arg => arg.GetType()).ToArray();
            var ctor = type.GetConstructor(types);
            return ctor.Invoke(constructorArgs.ToArray());
        }

        private List<object> GetConstructorArgs(XElement element)
        {
            List<object> constructorArgs = new List<object>();
            int constructorArgSanityCheck = 0;

            var constructorArgElements = from m in element.Elements(XmlConstants.MEMBER)
                                         let arg = m.Attribute(XmlConstants.ARG)
                                         where arg != null
                                         orderby arg.Value ascending
                                         select m;

            foreach (var argElement in constructorArgElements)
            {
                var argAttrib = argElement.Attribute(XmlConstants.ARG);
                if (argAttrib == null)
                    continue;

                int arg = int.Parse(argAttrib.Value, CultureInfo.InvariantCulture);
                if (arg != constructorArgSanityCheck)
                    throw new XmlException("Missing constructor argument " + constructorArgSanityCheck);
                constructorArgSanityCheck++;

                constructorArgs.Add(ReadObject(argElement, null));
            }
            return constructorArgs;
        }

        private static string GetFullPath(XElement element)
        {
            StringBuilder sb = new StringBuilder();
            Stack<string> pathStack = new Stack<string>();

            do
            {
                pathStack.Push(element.Name.LocalName);
                element = element.Parent;
            } while (element != null);

            sb.Append("/");

            do
            {
                sb.Append(pathStack.Pop());
                if (pathStack.Count > 0)
                    sb.Append("/");
            } while (pathStack.Count > 0);

            return sb.ToString();
        }
    }
}
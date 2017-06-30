// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
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
        #region Fields

        private Dictionary<string, object> _references;
        private Dictionary<string, Type> _typeCache;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the given XML string into an object.
        /// </summary>
        /// <typeparam name="T">The type to cast the returned object as.</typeparam>
        /// <param name="xml">The serialized XML text.</param>
        /// <returns>The object represented by the serialized XML.</returns>
        public T Deserialize<T>(string xml)
        {
            if (xml == null) throw new ArgumentNullException(nameof(xml));

            return DoDeserialize(default(T), xml, false);
        }

        /// <summary>
        /// Deserializes the given XML, writing the values into the given object.
        /// </summary>
        /// <typeparam name="T">The type of the given object.</typeparam>
        /// <param name="existingObject">An existing object to fill with data.</param>
        /// <param name="xml">The serialized XML text.</param>
        public void Deserialize<T>(T existingObject, string xml)
        {
            if (xml == null) throw new ArgumentNullException(nameof(xml));

            // Value types will never evaluate to null, and that's just fine.
            if (!typeof(T).IsValueType)
            {
                if (existingObject == null) throw new ArgumentNullException(nameof(existingObject));
            }

            DoDeserialize(existingObject, xml, true);
        }

        private static string GetFullPath(XElement element)
        {
            StringBuilder sb = new StringBuilder();
            Stack<string> pathStack = new Stack<string>();

            do
            {
                pathStack.Push(element.Name.LocalName);
                element = element.Parent;
            }
            while (element != null);

            sb.Append("/");

            do
            {
                sb.Append(pathStack.Pop());
                if (pathStack.Count > 0) sb.Append("/");
            }
            while (pathStack.Count > 0);

            return sb.ToString();
        }

        private static string GetName(XElement element)
        {
            var nameAttrib = element.Attribute(XmlConstants.Name);
            if (nameAttrib == null) throw new XmlException("Missing name attribute for node " + GetFullPath(element));

            return nameAttrib.Value;
        }

        private static string GetValue(XElement element)
        {
            var valueAttrib = element.Attribute(XmlConstants.Value);

            return valueAttrib?.Value;
        }

        private static Dictionary<string, Type> ReadTypeCache(XContainer rootNode)
        {
            var result = new Dictionary<string, Type>();
            var typeNameManager = new TypeNameManager();
            foreach (var typeNode in rootNode.Elements(XmlConstants.TypeCache).Elements(XmlConstants.Item))
            {
                var keyAttrib = typeNode.Attribute(XmlConstants.Key);
                var valueAttrib = typeNode.Attribute(XmlConstants.Value);
                if (keyAttrib == null || valueAttrib == null) continue;

                Type t;
                try
                {
                    // This method is too strict to handle auto-incrementing versioned dll references, but is
                    // needed to correctly grab core .Net Framework stuff and works faster for same-version dll files.
                    t = Type.GetType(valueAttrib.Value) ?? Type.GetType(valueAttrib.Value, name => AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(z => z.FullName == name.FullName), null);

                    if (t == null)
                    {
                        // check whether the needed object was moved to another assembly
                        MovedTypes mt = new MovedTypes();
                        foreach (TypeMoveDefintion tc in mt.Types)
                        {
                            if (tc.IsApplicable(valueAttrib.Value))
                            {
                                t = Type.GetType(tc.MoveType(valueAttrib.Value));
                                if (t != null) break;
                            }
                        }
                    }
                }
                catch (FileLoadException)
                {
                    // Attempting to load the fully qualified name failed. Search under more general terms
                    // and see if we can find a valid assembly with the specified type.
                    string updatedName = typeNameManager.UpdateTypename(valueAttrib.Value);
                    t = Type.GetType(updatedName);
                }

                result.Add(keyAttrib.Value, t);
            }

            return result;
        }

        private object ConstructObject(Type type, XElement element)
        {
            List<object> constructorArgs;
            if (type.IsArray)
            {
                int arrayLength = element.Elements(XmlConstants.Item).Count();
                constructorArgs = new List<object>
                                  {
                                      arrayLength
                                  };
            }
            else
            {
                constructorArgs = GetConstructorArgs(element);
            }

            Type[] types = constructorArgs.Select(arg => arg.GetType()).ToArray();
            //var ctor = type.GetConstructor(types);
            //return ctor.Invoke(constructorArgs.ToArray());
			//CGX
            if (types != null)
            {
                var ctor = type.GetConstructor(types);
                if (ctor != null)
                    return ctor.Invoke(constructorArgs.ToArray());
            }
            return null;
            //Fin CGX
        }

        private T DoDeserialize<T>(T existingObject, string xml, bool fill)
        {
            _references = new Dictionary<string, object>();
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    XDocument document = XDocument.Load(stringReader);
                    XElement rootNode = document.Root;
                    if (rootNode == null) throw new XmlException("Could not find a root XML element.");

                    _typeCache = ReadTypeCache(rootNode);
                    if (fill)
                    {
                        FillObject(rootNode, existingObject, true);
                        return existingObject;
                    }

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

        private void FillObject(XElement element, object parent, bool includeConstructorElements)
        {
            Type type = parent.GetType();
            var map = SerializationMap.FromType(type);

            var idAttribute = element.Attribute(XmlConstants.Id);
            if (idAttribute != null) _references[idAttribute.Value] = parent;

            // Populate the rest of the members
            var nonConstructorArgElements = from m in element.Elements(XmlConstants.Member) where m.Attribute(XmlConstants.Arg) == null || includeConstructorElements select m;

            foreach (var member in nonConstructorArgElements)
            {
                string name = GetName(member);
                var mapEntry = map.Members.FirstOrDefault(m => m.Attribute.Name == name);
                if (mapEntry == null) continue; // XML is probably out of date with the code if this happens

                var propertyInfo = mapEntry.Member as PropertyInfo;
                if (propertyInfo != null)
                {
                    if (propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(parent, ReadObject(member, null), null);
                    }
                    else
                    {
                        FillObject(member, propertyInfo.GetValue(parent, null), false);
                    }
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

        private List<object> GetConstructorArgs(XElement element)
        {
            List<object> constructorArgs = new List<object>();
            int constructorArgSanityCheck = 0;

            var constructorArgElements = from m in element.Elements(XmlConstants.Member) let arg = m.Attribute(XmlConstants.Arg) where arg != null orderby arg.Value ascending select m;

            foreach (var argElement in constructorArgElements)
            {
                var argAttrib = argElement.Attribute(XmlConstants.Arg);
                if (argAttrib == null) continue;

                int arg = int.Parse(argAttrib.Value, CultureInfo.InvariantCulture);
                if (arg != constructorArgSanityCheck) throw new XmlException("Missing constructor argument " + constructorArgSanityCheck);

                constructorArgSanityCheck++;

                constructorArgs.Add(ReadObject(argElement, null));
            }

            return constructorArgs;
        }

        private object GetObjectFromFormatter(XElement element)
        {
            var formatterAttrib = element.Attribute(XmlConstants.Formatter);
            if (formatterAttrib == null) return null;

            var valueAttrib = element.Attribute(XmlConstants.Value);
            if (valueAttrib == null) throw new XmlException("Missing value attribute for formattable element " + GetFullPath(element));

            var formatterType = _typeCache[formatterAttrib.Value];
            SerializationFormatter formatter = (SerializationFormatter)formatterType.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
            return formatter.FromString(XmlHelper.UnEscapeInvalidCharacters(valueAttrib.Value));
        }

        private Type GetType(XElement element)
        {
            var typeAttrib = element.Attribute(XmlConstants.TypeId);
            if (typeAttrib == null) throw new XmlException("Missing type attribute for node " + GetFullPath(element));

            return _typeCache[typeAttrib.Value];
        }

        private void PopulateArray(XElement element, Array array)
        {
            int index = 0;
            foreach (var item in element.Elements(XmlConstants.Item))
            {
                object newItem = ReadObject(item, array.GetValue(index));
                array.SetValue(newItem, index++);
            }
        }

        private void PopulateDictionary(XElement element, IDictionary dictionary)
        {
            if (dictionary.Count > 0) dictionary.Clear();

            foreach (var item in element.Elements(XmlConstants.DictionaryEntry))
            {
                var keyElement = item.Element(XmlConstants.Key);
                var valueElement = item.Element(XmlConstants.Value);
                if (keyElement == null || valueElement == null) continue;

                object key = ReadObject(keyElement, null);
                object value = ReadObject(valueElement, null);

                if (key != null && value != null) dictionary.Add(key, value);
            }
        }

        private void PopulateList(XElement element, IList list)
        {
            if (list.Count > 0) list.Clear();

            foreach (var item in element.Elements(XmlConstants.Item))
            {
                object newItem = ReadObject(item, null);
                if (newItem != null) list.Add(newItem);
            }
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
            var refElement = element.Attribute(XmlConstants.Ref);
            if (refElement != null) return _references[refElement.Value];

            object result = GetObjectFromFormatter(element);
            if (result != null) return result;

            Type type = GetType(element);
            string value = GetValue(element);
            if (type == null)
            {
                // must be a plugin
                throw new FileNotFoundException("Couldn't find the assembly that contains the type that was serialized into that element.");
            }

            if (type.IsPrimitive)
            {
                result = value != "NaN" ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture) : Convert.ChangeType(double.NaN, type);
            }
            else if (type.IsEnum)
            {
                result = Enum.Parse(type, value);
            }
            else if (type == typeof(string))
            {
                result = XmlHelper.UnEscapeInvalidCharacters(value);
            }
            else if (type == typeof(DateTime))
            {
                result = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(Color))
            {
                result = ColorTranslator.FromHtml(value);
            }
            else if (type == typeof(PointF))
            {
                string[] vals = XmlHelper.UnEscapeInvalidCharacters(value).Split('|');
                result = new PointF(float.Parse(vals[0]), float.Parse(vals[1]));
            }
            else
            {
                if (parent == null)
                {
                    try
                    {
                        result = ConstructObject(type, element);
                    }
                    catch
                    {
                        // If a project file (such as a layer) is missing, this exception is thrown.
                        // We still want to be able to open the project; setting result to null seems to make this work.
                        result = null;
                    }
                }
                else
                {
                    result = parent;
                }

                if (result != null) FillObject(element, result, false);
            }

            return result;
        }

        #endregion
    }
}
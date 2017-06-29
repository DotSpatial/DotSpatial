using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// http://mutable.net/blog/archive/2010/07/07/yet-another-take-on-a-dynamicobject-wrapper-for-xml.aspx
    /// </summary>
    public class DynamicXMLNode : DynamicObject
    {
        private XElement node;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicXMLNode"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public DynamicXMLNode(XElement node)
        {
            this.node = node;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicXMLNode"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DynamicXMLNode(String name)
        {
            node = new XElement(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicXMLNode"/> class.
        /// </summary>
        public DynamicXMLNode()
        {
        }

        /// <summary>
        /// Loads the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(string uri)
        {
            return new DynamicXMLNode(XElement.Load(uri));
        }

        /// <summary>
        /// Loads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(Stream stream)
        {
            return new DynamicXMLNode(XElement.Load(stream));
        }

        /// <summary>
        /// Loads the specified text reader.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(TextReader textReader)
        {
            return new DynamicXMLNode(XElement.Load(textReader));
        }

        /// <summary>
        /// Loads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(XmlReader reader)
        {
            return new DynamicXMLNode(XElement.Load(reader));
        }

        /// <summary>
        /// Loads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(Stream stream, LoadOptions options)
        {
            return new DynamicXMLNode(XElement.Load(stream, options));
        }

        /// <summary>
        /// Loads the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(string uri, LoadOptions options)
        {
            return new DynamicXMLNode(XElement.Load(uri, options));
        }

        /// <summary>
        /// Loads the specified text reader.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(TextReader textReader, LoadOptions options)
        {
            return new DynamicXMLNode(XElement.Load(textReader, options));
        }

        /// <summary>
        /// Loads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static DynamicXMLNode Load(XmlReader reader, LoadOptions options)
        {
            return new DynamicXMLNode(XElement.Load(reader, options));
        }

        /// <summary>
        /// Parses the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static DynamicXMLNode Parse(string text)
        {
            return new DynamicXMLNode(XElement.Parse(text));
        }

        /// <summary>
        /// Parses the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static DynamicXMLNode Parse(string text, LoadOptions options)
        {
            return new DynamicXMLNode(XElement.Parse(text, options));
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            XElement setNode = node.Element(binder.Name);
            if (setNode != null)
            {
                setNode.SetValue(value);
            }
            else
            {
                if (value.GetType() == typeof(DynamicXMLNode))
                {
                    node.Add(new XElement(binder.Name));
                }
                else
                {
                    node.Add(new XElement(binder.Name, value));
                }
            }
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            XElement getNode = node.Element(binder.Name);
            if (getNode != null)
            {
                result = new DynamicXMLNode(getNode);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. </param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Type xmlType = typeof(XElement);
            try
            {
                // unwrap parameters: if the parameters are DynamicXMLNode then pass in the inner XElement node
                List<object> newargs = null;
                var argtypes = args.Select(x => x.GetType()); // because GetTypeArray is not supported in Silverlight
                if (argtypes.Contains(typeof(DynamicXMLNode)) || argtypes.Contains(typeof(DynamicXMLNode[])))
                {
                    newargs = new List<object>();
                    foreach (var arg in args)
                    {
                        if (arg.GetType() == typeof(DynamicXMLNode))
                        {
                            newargs.Add(((DynamicXMLNode)arg).node);
                        }
                        else if (arg.GetType() == typeof(DynamicXMLNode[]))
                        {
                            // unwrap array of DynamicXMLNode
                            newargs.Add(((DynamicXMLNode[])arg).Select(x => (x).node));
                        }
                        else
                        {
                            newargs.Add(arg);
                        }
                    }
                }

                result = xmlType.InvokeMember(
                          binder.Name,
                          BindingFlags.InvokeMethod |
                          BindingFlags.Public |
                          BindingFlags.Instance,
                          null, node, newargs == null ? args : newargs.ToArray());

                // wrap return value: if the results are an IEnumerable<XElement>, then return a wrapped collection of DynamicXMLNode
                if (result != null && typeof(IEnumerable<XElement>).IsAssignableFrom(result.GetType()))
                {
                    result = ((IEnumerable<XElement>)result).Select(x => new DynamicXMLNode(x));
                }
                // Note: we could wrap single XElement too but nothing returns that

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Provides the implementation for operations that set a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that access objects by a specified index.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation.</param>
        /// <param name="value">The value to set to the object that has the specified index. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="value"/> is equal to 10.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.
        /// </returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes[0].GetType() == typeof(String))
            {
                node.SetAttributeValue((string)indexes[0], value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. </param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes[0].GetType() == typeof(String))
            {
                XAttribute attr = node.Attribute((string)indexes[0]);
                if (attr != null)
                {
                    result = attr.Value;
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Type returns the <see cref="T:System.String"/> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(XElement))
            {
                result = node;
            }
            else if (binder.Type == typeof(String))
            {
                result = node.Value;
            }
            else
            {
                result = false;
                return false;
            }

            return true;
        }
    }
}
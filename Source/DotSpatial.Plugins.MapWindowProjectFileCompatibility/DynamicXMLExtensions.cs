using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Helper extensions that are used to parse an xml file and display its schema.
    /// </summary>
    public static class DynamicXMLExtensions
    {
        #region Methods

        /// <summary>
        /// Gets the X path.
        /// </summary>
        /// <param name="xobj">The xobj.</param>
        /// <returns>The x Path.</returns>
        public static string GetXPath(this XObject xobj)
        {
            if (xobj.Parent == null)
            {
                XDocument doc = xobj as XDocument;
                if (doc != null)
                    return ".";
                XElement el = xobj as XElement;
                if (el != null)
                    return "/" + NameWithPredicate(el);

                // the XPath data model does not include white space text nodes
                // that are children of a document, so this method returns null.
                XText xt = xobj as XText;
                if (xt != null)
                    return null;
                XComment com = xobj as XComment;
                if (com?.Document != null)
                    return "/" + (com.Document.Nodes().OfType<XComment>().Count() != 1 ? "comment()[" + (com.NodesBeforeSelf().OfType<XComment>().Count() + 1) + "]" : "comment()");
                XProcessingInstruction pi = xobj as XProcessingInstruction;
                if (pi?.Document != null)
                    return "/" + (pi.Document.Nodes().OfType<XProcessingInstruction>().Count() != 1 ? "processing-instruction()[" + (pi.NodesBeforeSelf().OfType<XProcessingInstruction>().Count() + 1) + "]" : "processing-instruction()");
                return null;
            }
            else
            {
                XElement el = xobj as XElement;
                if (el != null)
                {
                    return "/" + el.Ancestors().InDocumentOrder().Select(NameWithPredicate).StrCat("/") + NameWithPredicate(el);
                }

                XAttribute at = xobj as XAttribute;
                if (at?.Parent != null)
                    return "/" + at.Parent.AncestorsAndSelf().InDocumentOrder().Select(NameWithPredicate).StrCat("/") + "@" + GetQName(at);
                XComment com = xobj as XComment;
                if (com?.Parent != null)
                    return "/" + com.Parent.AncestorsAndSelf().InDocumentOrder().Select(NameWithPredicate).StrCat("/") + (com.Parent.Nodes().OfType<XComment>().Count() != 1 ? "comment()[" + (com.NodesBeforeSelf().OfType<XComment>().Count() + 1) + "]" : "comment()");
                XCData cd = xobj as XCData;
                if (cd?.Parent != null)
                    return "/" + cd.Parent.AncestorsAndSelf().InDocumentOrder().Select(NameWithPredicate).StrCat("/") + (cd.Parent.Nodes().OfType<XText>().Count() != 1 ? "text()[" + (cd.NodesBeforeSelf().OfType<XText>().Count() + 1) + "]" : "text()");
                XText tx = xobj as XText;
                if (tx?.Parent != null)
                    return "/" + tx.Parent.AncestorsAndSelf().InDocumentOrder().Select(NameWithPredicate).StrCat("/") + (tx.Parent.Nodes().OfType<XText>().Count() != 1 ? "text()[" + (tx.NodesBeforeSelf().OfType<XText>().Count() + 1) + "]" : "text()");
                XProcessingInstruction pi = xobj as XProcessingInstruction;
                if (pi?.Parent != null)
                    return "/" + pi.Parent.AncestorsAndSelf().InDocumentOrder().Select(NameWithPredicate).StrCat("/") + (pi.Parent.Nodes().OfType<XProcessingInstruction>().Count() != 1 ? "processing-instruction()[" + (pi.NodesBeforeSelf().OfType<XProcessingInstruction>().Count() + 1) + "]" : "processing-instruction()");
                return null;
            }
        }

        /// <summary>
        /// STRs the cat.
        /// </summary>
        /// <typeparam name="T">Type of the elements in source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The produced string.</returns>
        public static string StrCat<T>(this IEnumerable<T> source, string separator)
        {
            return source.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i.ToString()).Append(separator), s => s.ToString());
        }

        private static string GetQName(XElement xe)
        {
            string prefix = xe.GetPrefixOfNamespace(xe.Name.Namespace);
            if (xe.Name.Namespace == XNamespace.None || prefix == null)
                return xe.Name.LocalName;

            return prefix + ":" + xe.Name.LocalName;
        }

        private static string GetQName(XAttribute xa)
        {
            if (xa.Parent == null) return string.Empty;
            string prefix = xa.Parent.GetPrefixOfNamespace(xa.Name.Namespace);
            if (xa.Name.Namespace == XNamespace.None || prefix == null)
                return xa.Name.ToString();

            return prefix + ":" + xa.Name.LocalName;
        }

        private static string NameWithPredicate(XElement el)
        {
            if (el.Parent != null && el.Parent.Elements(el.Name).Count() != 1)
                return GetQName(el) + "[" + (el.ElementsBeforeSelf(el.Name).Count() + 1) + "]";

            return GetQName(el);
        }

        private static void PrintOutAttributeNames(string fileName)
        {
            XDocument doc = XDocument.Load(fileName);

            foreach (XNode obj in doc.DescendantNodes())
            {
                Console.WriteLine(obj.GetXPath());
                XElement el = obj as XElement;
                if (el != null)
                {
                    foreach (XAttribute at in el.Attributes())
                        Console.WriteLine(at.GetXPath());
                }
            }
        }

        #endregion
    }
}
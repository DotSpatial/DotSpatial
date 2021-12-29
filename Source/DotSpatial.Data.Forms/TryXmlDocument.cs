// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.IO;
using System.Xml;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// This class can be used to read things from Xml documents.
    /// </summary>
    public class TryXmlDocument
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TryXmlDocument"/> class.
        /// </summary>
        public TryXmlDocument()
        {
            Document = new XmlDocument();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current node that should be referenced for reading attributes.
        /// </summary>
        public XmlElement CurrentElement { get; set; }

        /// <summary>
        /// Gets or sets the XmlDocument that this class uses for data access.
        /// </summary>
        public XmlDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the fileName for this document.
        /// </summary>
        public string Filename { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This changes CurrentNode to the child node with the specified name.
        /// If there are no child nodes, or the child node is not found, this returns false.
        /// If the navigation is successful, this returns true.
        /// </summary>
        /// <param name="name">Name of the child.</param>
        /// <returns>If there are no child nodes, or the child node is not found, this returns false.
        /// If the navigation is successful, this returns true.</returns>
        public bool NavigateToChild(string name)
        {
            if (CurrentElement.HasChildNodes == false) return false;
            foreach (XmlElement node in CurrentElement.ChildNodes)
            {
                if (node.Name == name)
                {
                    CurrentElement = node;
                    return true;
                }
            }

            // node was not found
            return false;
        }

        /// <summary>
        /// Attempts to navigate to the parent.
        /// </summary>
        /// <returns>True, if the parent was found.</returns>
        public bool NavigateToParent()
        {
            if (CurrentElement.ParentNode != null)
            {
                CurrentElement = CurrentElement.ParentNode as XmlElement;
                if (CurrentElement != null) return true;
            }

            // this node may be a root node with no parent
            return false;
        }

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file that gets opened.</param>
        /// <exception cref="FileNotFoundException">Thrown if the file doesn't exist.</exception>
        public void Open(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            Document.Load(fileName);
            CurrentElement = Document.DocumentElement;
        }

        /// <summary>
        /// Attempts to read the boolean value from the specified attribute, translating it from a text equivalent.
        /// </summary>
        /// <param name="attribute">The string name of the attribute to read from the CurrentElement.</param>
        /// <returns>A boolean value based on parsing the text.</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified.</exception>
        public bool ReadBool(string attribute)
        {
            if (CurrentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }

            bool result = false;
            try
            {
                if (CurrentElement.HasAttribute(attribute))
                {
                    result = bool.Parse(CurrentElement.GetAttribute(attribute));
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Attempts to read the color from a text representation of an argb integer value.
        /// </summary>
        /// <param name="attribute">The name of the attribute to read from the CurrentElement.</param>
        /// <returns>A Color structure.</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified.</exception>
        public Color ReadColor(string attribute)
        {
            if (CurrentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }

            Color result = Color.Empty;
            try
            {
                if (CurrentElement.HasAttribute(attribute))
                {
                    string txtCol = CurrentElement.GetAttribute(attribute);
                    result = Color.FromArgb(int.Parse(txtCol));
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Attempts to read the double value from the specified attribute, translating it from
        /// a text equivalent via parsing.
        /// </summary>
        /// <param name="attribute">The string name of the attribute to read from the CurrentElement.</param>
        /// <returns>A double value parsed from the inner text of the specified attribute on the CurrentElement.</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified.</exception>
        public double ReadDouble(string attribute)
        {
            if (CurrentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }

            double result = 0.0;
            try
            {
                if (CurrentElement.HasAttribute(attribute))
                {
                    result = double.Parse(CurrentElement.GetAttribute(attribute));
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Attempts to read the integer value from the specified attribute, translating it from
        /// a text equivalent via parsing.
        /// </summary>
        /// <param name="attribute">The string name of the attribute to read from the CurrentElement.</param>
        /// <returns>An integer parsed from the inner text of the specified attribute on the CurrentElement.</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified.</exception>
        public int ReadInteger(string attribute)
        {
            if (CurrentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }

            int result = 0;
            try
            {
                if (CurrentElement.HasAttribute(attribute))
                {
                    result = int.Parse(CurrentElement.GetAttribute(attribute));
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Attempts to read the string for the specified value. This will first test to see if the
        /// attribute exists and encloses the test in a try block. If it fails or the node does not
        /// exist, the default value is returned.
        /// </summary>
        /// <param name="attribute">The string name for the attribute to read from the CurrentElement.</param>
        /// <returns>A string specifying the value.</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified.</exception>
        public string ReadText(string attribute)
        {
            if (CurrentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }

            string result = string.Empty;
            try
            {
                if (CurrentElement.HasAttribute(attribute))
                {
                    result = CurrentElement.GetAttribute(attribute);
                }
            }
            catch
            {
            }

            return result;
        }

        #endregion
    }
}
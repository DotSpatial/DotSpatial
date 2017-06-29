// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/16/2009 4:22:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.IO;
using System.Xml;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// TryXmlDocument
    /// </summary>
    public class TryXmlDocument
    {
        #region Private Variables

        private XmlElement _currentElement;
        private XmlDocument _doc;
        private string _fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TryXmlDocument
        /// </summary>
        public TryXmlDocument()
        {
            _doc = new XmlDocument();
        }

        #endregion

        #region Methods

        /// <summary>
        /// This changes CurrentNode to the child node with the specified name.
        /// If there are no child nodes, or the child node is not found, this returns false.
        /// If the navigation is successful, this returns true.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool NavigateToChild(string name)
        {
            if (_currentElement.HasChildNodes == false) return false;
            foreach (XmlElement node in _currentElement.ChildNodes)
            {
                if (node.Name == name)
                {
                    _currentElement = node;
                    return true;
                }
            }
            // node was not found
            return false;
        }

        /// <summary>
        /// Attempts to navigate to the parent.
        /// </summary>
        /// <returns></returns>
        public bool NavigateToParent()
        {
            if (_currentElement.ParentNode != null)
            {
                _currentElement = _currentElement.ParentNode as XmlElement;
                if (_currentElement != null) return true;
            }
            // this node may be a root node with no parent
            return false;
        }

        /// <summary>
        /// Opens the specified
        /// </summary>
        public void Open(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                throw new FileNotFoundException(fileName);
            }

            _doc.Load(fileName);
            _currentElement = _doc.DocumentElement;
        }

        /// <summary>
        /// Attempts to read the string for the specified value.  This will first test to see if the
        /// attribute exists and encloses the test in a try block.  If it fails or the node does not
        /// exist, the default value is returned.
        /// </summary>
        /// <param name="attribute">The string name for the attribute to read from the CurrentElement.</param>
        /// <returns>A string specifying the value</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified</exception>
        public string ReadText(string attribute)
        {
            if (_currentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }
            string result = string.Empty;
            try
            {
                if (_currentElement.HasAttribute(attribute))
                {
                    result = _currentElement.GetAttribute(attribute);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Attempts to read the color from a text representation of an argb integer value.
        /// </summary>
        /// <param name="attribute">The name of the attribute to read from the CurrentElement</param>
        /// <returns>A Color structure</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified</exception>
        public Color ReadColor(string attribute)
        {
            if (_currentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }
            Color result = Color.Empty;
            try
            {
                if (_currentElement.HasAttribute(attribute))
                {
                    string txtCol = _currentElement.GetAttribute(attribute);
                    result = Color.FromArgb(int.Parse(txtCol));
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Attempts to read the boolean value from the specified attribute, translating it from
        /// a text equivalent.
        /// </summary>
        /// <param name="attribute">The string name of the attribute to read from the CurrentElement</param>
        /// <returns>A boolean value based on parsing the text.</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified</exception>
        public bool ReadBool(string attribute)
        {
            if (_currentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }
            bool result = false;
            try
            {
                if (_currentElement.HasAttribute(attribute))
                {
                    result = bool.Parse(_currentElement.GetAttribute(attribute));
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Attempts to read the integer value from the specified attribute, translating it from
        /// a text equivalent via parsing.
        /// </summary>
        /// <param name="attribute">The string name of the attribute to read from the CurrentElement</param>
        /// <returns>An integer parsed from the inner text of the specified attribute on the CurrentElement</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified</exception>
        public int ReadInteger(string attribute)
        {
            if (_currentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }
            int result = 0;
            try
            {
                if (_currentElement.HasAttribute(attribute))
                {
                    result = int.Parse(_currentElement.GetAttribute(attribute));
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Attempts to read the double value from the specified attribute, translating it from
        /// a text equivalent via parsing.
        /// </summary>
        /// <param name="attribute">The string name of the attribute to read from the CurrentElement</param>
        /// <returns>A double value parsed from the inner text of the specified attribute on the CurrentElement</returns>
        /// <exception cref="TryXmlDocumentException">CurrentElement Not Specified</exception>
        public double ReadDouble(string attribute)
        {
            if (_currentElement == null)
            {
                throw new TryXmlDocumentException(DataFormsMessageStrings.CurrentElementNotSpecified);
            }
            double result = 0.0;
            try
            {
                if (_currentElement.HasAttribute(attribute))
                {
                    result = double.Parse(_currentElement.GetAttribute(attribute));
                }
            }
            catch { }
            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the fileName for this document.
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Gets or sets the XmlDocument that this class uses for data access.
        /// </summary>
        public XmlDocument Document
        {
            get { return _doc; }
            set { _doc = value; }
        }

        /// <summary>
        /// Gets or sets the current node that should be referenced for reading attributes.
        /// </summary>
        public XmlElement CurrentElement
        {
            get { return _currentElement; }
            set { _currentElement = value; }
        }

        #endregion
    }
}
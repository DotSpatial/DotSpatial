/*
 * Erstellt mit SharpDevelop.
 * Benutzer: grunwald
 * Datum: 27.08.2007
 * Zeit: 14:25
 *
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using ICSharpCode.SharpDevelop.Dom.VBNet;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace CSharpEditor
{
    /// <summary>
    /// Represents an item in the code completion window.
    /// </summary>
    internal class CodeCompletionData : DefaultCompletionData, ICompletionData
    {
        #region Fields

        private static readonly CSharpAmbience CsharpAmbience = new CSharpAmbience();
        private static readonly VBNetAmbience VbAmbience = new VBNetAmbience();
        private readonly IClass _c;
        private readonly IMember _member;
        private string _description;
        private int _overloads;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCompletionData"/> class.
        /// </summary>
        /// <param name="member">Member used for initialization.</param>
        public CodeCompletionData(IMember member)
            : base(member.Name, null, GetMemberImageIndex(member))
        {
            _member = member;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCompletionData"/> class.
        /// </summary>
        /// <param name="c">Class used for initialization.</param>
        public CodeCompletionData(IClass c)
            : base(c.Name, null, GetClassImageIndex(c))
        {
            _c = c;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a description.
        /// </summary>
        string ICompletionData.Description
        {
            get
            {
                if (_description == null)
                {
                    IEntity entity = (IEntity)_member ?? _c;
                    _description = GetText(entity);
                    if (_overloads > 1)
                    {
                        _description += " (+" + _overloads + " overloads)";
                    }

                    _description += Environment.NewLine + XmlDocumentationToText(entity.Documentation);
                }

                return _description;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the given XML documentation to text.
        /// </summary>
        /// <param name="xmlDoc">XML documentation that gets converted.</param>
        /// <returns>The result of the conversion.</returns>
        public static string XmlDocumentationToText(string xmlDoc)
        {
            Debug.WriteLine(xmlDoc);
            StringBuilder b = new StringBuilder();
            try
            {
                using (XmlTextReader reader = new XmlTextReader(new StringReader("<root>" + xmlDoc + "</root>")))
                {
                    reader.XmlResolver = null;
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Text:
                                b.Append(reader.Value);
                                break;
                            case XmlNodeType.Element:
                                switch (reader.Name)
                                {
                                    case "filterpriority":
                                        reader.Skip();
                                        break;
                                    case "returns":
                                        b.AppendLine();
                                        b.Append("Returns: ");
                                        break;
                                    case "param":
                                        b.AppendLine();
                                        b.Append(reader.GetAttribute("name") + ": ");
                                        break;
                                    case "remarks":
                                        b.AppendLine();
                                        b.Append("Remarks: ");
                                        break;
                                    case "see":
                                        if (reader.IsEmptyElement)
                                        {
                                            b.Append(reader.GetAttribute("cref"));
                                        }
                                        else
                                        {
                                            reader.MoveToContent();
                                            b.Append(reader.HasValue ? reader.Value : reader.GetAttribute("cref"));
                                        }

                                        break;
                                }

                                break;
                        }
                    }
                }

                return b.ToString();
            }
            catch (XmlException)
            {
                return xmlDoc;
            }
        }

        /// <summary>
        /// Adds an overload.
        /// </summary>
        public void AddOverload()
        {
            _overloads++;
        }

        private static int GetClassImageIndex(IClass c)
        {
            switch (c.ClassType)
            {
                case ClassType.Enum: return 4;
                default: return 0;
            }
        }

        private static int GetMemberImageIndex(IMember member)
        {
            // Missing: different icons for private/public member
            if (member is IMethod) return 1;
            if (member is IProperty) return 2;
            if (member is IField) return 3;
            if (member is IEvent) return 6;

            return 3;
        }

        /// <summary>
        /// Converts a member to text.
        /// Returns the declaration of the member as C# or VB code, e.g.
        /// "public void MemberName(string parameter)"
        /// </summary>
        /// <param name="entity">Entity that gets converted.</param>
        /// <returns>The result of the conversion</returns>
        private static string GetText(IEntity entity)
        {
            IAmbience ambience = MainForm.IsVisualBasic ? (IAmbience)VbAmbience : CsharpAmbience;

            var method = entity as IMethod;
            if (method != null) return ambience.Convert(method);

            var property = entity as IProperty;
            if (property != null) return ambience.Convert(property);

            var entityAsEvent = entity as IEvent;
            if (entityAsEvent != null) return ambience.Convert(entityAsEvent);

            var field = entity as IField;
            if (field != null) return ambience.Convert(field);

            var entityAsClass = entity as IClass;
            if (entityAsClass != null) return ambience.Convert(entityAsClass);

            // unknown entity:
            return entity.ToString();
        }

        #endregion
    }
}
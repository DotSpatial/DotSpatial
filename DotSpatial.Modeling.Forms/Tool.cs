// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tool.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Extensions;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A class from which Tools may be derived. Implments mundane parts of ITool for Assembly based tools.
    /// </summary>
    public abstract class Tool : AssemblyInformation, ITool
    {
        #region Constants and Fields

        private string author;
        private string description;
        private string helpUrl;
        private string name;

        #endregion

        #region Public Properties

        /// <summary>
        /// Author of the plugin.
        /// </summary>
        public override string Author
        {
            get
            {
                if (author == null)
                    author = base.Author;

                return author;
            }
            set
            {
                author = value;
            }
        }

        /// <summary>
        /// Returns the category of tool that the ITool should be added to
        /// </summary>
        public virtual string Category { get; set; }

        /// <summary>
        /// Short description of the plugin.
        /// </summary>
        public override string Description
        {
            get
            {
                if (description == null)
                    description = base.Description;

                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Image displayed in the help area when no input field is selected
        /// </summary>
        public virtual Bitmap HelpImage { get; set; }

        /// <summary>
        /// Returns the address of the tools help web page in HTTP://... format. Returns null if no URL has been specified.
        /// </summary>
        public virtual string HelpUrl
        {
            get
            {
                return helpUrl;
            }
            set
            {
                helpUrl = value;
            }
        }

        /// <summary>
        /// 32x32 Bitmap - The Large icon that will appears in the Tool Dialog Next to the tools name
        /// </summary>
        public virtual Bitmap Icon { get; set; }

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public abstract Parameter[] InputParameters { get; }

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public override string Name
        {
            get
            {
                if (name == null)
                    name = base.Name;

                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public abstract Parameter[] OutputParameters { get; }

        /// <summary>
        /// Returns a brief description displayed when the user hovers over the tool in the toolbox
        /// </summary>
        public virtual string ToolTip { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        /// <param name="cancelProgressHandler">A cancel progress handler that used to indicate how much of the tool is done</param>
        /// <returns></returns>
        public abstract bool Execute(ICancelProgressHandler cancelProgressHandler);

        /// <summary>
        /// The parameters array should be populated with default values here
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate other parameters default values.
        /// </summary>
        /// <param name="sender"></param>
        public virtual void ParameterChanged(Parameter sender) { }

        #endregion
    }
}
// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Extensions;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A class from which Tools may be derived. Implements mundane parts of ITool for Assembly based tools.
    /// </summary>
    public abstract class Tool : AssemblyInformation, ITool
    {
        #region Fields

        private string _author;
        private string _description;
        private string _name;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the author of the plugin.
        /// </summary>
        public override string Author
        {
            get
            {
                return _author ?? (_author = base.Author);
            }

            set
            {
                _author = value;
            }
        }

        /// <summary>
        /// Gets or sets the category of tool that the ITool should be added to.
        /// </summary>
        public virtual string Category { get; set; }

        /// <summary>
        /// Gets or sets a short description of the plugin.
        /// </summary>
        public override string Description
        {
            get
            {
                return _description ?? (_description = base.Description);
            }

            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Gets or sets an image displayed in the help area when no input field is selected.
        /// </summary>
        public virtual Bitmap HelpImage { get; set; }

        /// <summary>
        /// Gets or sets the address of the tools help web page in HTTP://... format. Returns null if no URL has been specified.
        /// </summary>
        public virtual string HelpUrl { get; set; }

        /// <summary>
        /// Gets or sets a 32x32 Bitmap - The Large icon that will appears in the Tool Dialog Next to the tools name.
        /// </summary>
        public virtual Bitmap Icon { get; set; }

        /// <summary>
        /// Gets the input paramater array,.
        /// </summary>
        public abstract Parameter[] InputParameters { get; }

        /// <summary>
        /// Gets or sets the name of the plugin.
        /// </summary>
        public override string Name
        {
            get
            {
                return _name ?? (_name = base.Name);
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets the output paramater array.
        /// </summary>
        public abstract Parameter[] OutputParameters { get; }

        /// <summary>
        /// Gets or sets a brief description displayed when the user hovers over the tool in the toolbox.
        /// </summary>
        public virtual string ToolTip { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Once the parameters have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">A cancel progress handler that used to indicate how much of the tool is done.</param>
        /// <returns>True, if the tool was executed successfully.</returns>
        public abstract bool Execute(ICancelProgressHandler cancelProgressHandler);

        /// <summary>
        /// The parameters array should be populated with default values here.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output parameters value, this can be used to populate other parameters default values.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        public virtual void ParameterChanged(Parameter sender)
        {
        }

        #endregion
    }
}
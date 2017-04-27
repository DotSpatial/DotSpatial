// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DialogSpacerElement
// Description:  DialogSpacerElement Element for use in the tool dialog
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// An element for DialogSpacing
    /// </summary>
    public partial class DialogSpacerElement : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogSpacerElement"/> class.
        /// </summary>
        public DialogSpacerElement()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current status the input.
        /// </summary>
        public override ToolStatus Status
        {
            get
            {
                return ToolStatus.Ok;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the text of the spacer.
        /// </summary>
        public override string Text
        {
            get
            {
                return _label1.Text;
            }

            set
            {
                _label1.Text = value;
            }
        }

        #endregion

        #region Methods

        private void Label1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion
    }
}
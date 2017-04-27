// ********************************************************************************************************
// Product Name: DotSpatial.Tools.StringElement
// Description:  String Element for use in the tool dialog
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// String element.
    /// </summary>
    internal partial class StringElement : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public StringElement(StringParam param)
        {
            // Needed by the designer
            InitializeComponent();
            GroupBox.Text = param.Name;

            // We save the parameters passed in
            Param = param;
            SetupDefaultLighting();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new StringParam Param
        {
            get
            {
                return (StringParam)base.Param;
            }

            set
            {
                base.Param = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Refresh()
        {
            SetupDefaultLighting();
        }

        private void SetupDefaultLighting()
        {
            // We load the default parameters
            if (Param.DefaultSpecified)
            {
                textBox1.Text = Param.Value;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
            }
            else
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
            }
        }

        /// <summary>
        /// When the text box is clicked this event fires.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            Param.Value = textBox1.Text;
        }

        #endregion
    }
}
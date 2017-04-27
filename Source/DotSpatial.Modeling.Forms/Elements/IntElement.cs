// ********************************************************************************************************
// Product Name: DotSpatial.Tools.IntegerElement
// Description:  Integer Element for use in the tool dialog
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                 |    Date              |   Comments
// ---------------------|----------------------|----------------------------------------------------
// Ted Dunsford         |  8/28/2009           |  Cleaned up some formatting content using re-sharper.
// ********************************************************************************************************

using System;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// an element for integers
    /// </summary>
    public partial class IntElement : DialogElement
    {
        #region Fields

        private bool _enableUpdate = true;
        private string _oldText = string.Empty;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public IntElement(IntParam param)
        {
            // Needed by the designer
            InitializeComponent();
            GroupBox.Text = param.Name;

            // We save the parameters passed in
            Param = param;

            HandleStatusLight();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new IntParam Param
        {
            get
            {
                return (IntParam)base.Param;
            }

            set
            {
                base.Param = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the status lights
        /// </summary>
        public override void Refresh()
        {
            HandleStatusLight();
        }

        /// <summary>
        /// Checks if text contains a value integer
        /// </summary>
        /// <param name="theValue">The text to text</param>
        /// <returns>Returns true if it is a valid integer</returns>
        private static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void HandleStatusLight()
        {
            _enableUpdate = false;

            // We load the default parameters
            if (Param.DefaultSpecified)
            {
                int value = Param.Value;
                if ((value >= Param.Min) && (value <= Param.Max))
                {
                    Status = ToolStatus.Ok;
                    LightTipText = ModelingMessageStrings.ParameterValid;
                    txtValue.Text = value.ToString();
                }
                else
                {
                    Status = ToolStatus.Empty;
                    LightTipText = ModelingMessageStrings.InvalidInteger.Replace("%min", Param.Min.ToString()).Replace("%max", Param.Max.ToString());
                }
            }
            else
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
            }

            _enableUpdate = true;
        }

        /// <summary>
        /// When the text box is clicked this event fires
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (IsInteger(txtValue.Text))
                {
                    _oldText = txtValue.Text;
                    Param.Value = Convert.ToInt32(txtValue.Text);
                }
                else
                {
                    txtValue.Text = _oldText;
                }
            }
        }

        #endregion
    }
}
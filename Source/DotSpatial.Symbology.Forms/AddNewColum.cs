// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/11/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Dialog for adding new column information to a data table.
    /// </summary>
    public partial class AddNewColum : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewColum"/> class.
        /// </summary>
        public AddNewColum()
        {
            InitializeComponent();
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Name of the field.
        /// </summary>
        public new string Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the field.
        /// </summary>
        public new int Size { get; set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        public Type Type { get; set; }

        #endregion

        #region Methods

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            Name = txtName.Text;
            string type = Convert.ToString(cmbType.SelectedItem);
            switch (type)
            {
                case "Double":
                    Type = typeof(double);
                    break;
                case "String":
                    Type = typeof(string);
                    break;
                case "int":
                    Type = typeof(int);
                    break;
                default:
                    Type = typeof(double);
                    break;
            }

            Size = Convert.ToInt32(nudSize.Value);
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}
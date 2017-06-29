// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
        /// <summary>
        /// This form will display when the user want to add new fiels in the Table
        /// </summary>
        ///
        private string _name;

        private int _size;
        private Type _type;

        #region properties

        /// <summary>
        /// set or get the Name of the field.
        /// </summary>
        public new string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// set or get the type of the field.
        /// </summary>
        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// set or get the size of the field.
        /// </summary>
        public new int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        #endregion

        /// <summary>
        /// A public constructor for creating a new column
        /// </summary>
        public AddNewColum()
        {
            InitializeComponent();
            dialogButtons1.OkClicked += btnOk_Click;
            dialogButtons1.CancelClicked += btnCancel_Click;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _name = txtName.Text;
            string type = Convert.ToString(cmbType.SelectedItem);
            switch (type)
            {
                case ("Double"):
                    _type = typeof(double);
                    break;
                case ("String"):
                    _type = typeof(string);
                    break;
                case ("int"):
                    _type = typeof(int);
                    break;
                default:
                    _type = typeof(double);
                    break;
            }
            _size = Convert.ToInt32(nudSize.Value);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
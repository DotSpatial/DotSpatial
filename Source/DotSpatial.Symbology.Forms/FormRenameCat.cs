using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class FormRenameCat : Form
    {
        ILabelCategory _lc  = null;
        IList<ILabelCategory> _ListCats = null;
     
        public ILabelCategory LabelCategory
        {
            get
            {
                return _lc;
            }
        }
        public FormRenameCat(ILabelCategory lc, IList<ILabelCategory> ListCats)
        {
            InitializeComponent();
            _lc = lc;
            _ListCats = ListCats;
            textBoxName.Text = _lc.Name;
        }

        private bool IsExist()
        {
            bool bRet = false;

            foreach (ILabelCategory cat in _ListCats)
            {
                if (cat.Name == textBoxName.Text && _lc.Name != textBoxName.Text)
                    return true;
            }
            return bRet; 

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            _lc.Name = textBoxName.Text;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text) || IsExist() )
            {
                
                buttonOk.Enabled = false;
                
            }
            else
                buttonOk.Enabled = true;
        }
    }
}

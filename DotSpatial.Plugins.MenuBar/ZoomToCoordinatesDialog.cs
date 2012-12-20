using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Plugins.MenuBar
{
    public partial class ZoomToCoordinatesDialog : Form
    {

        public double D1 { get { return Double.Parse(d1.Text.ToString()); } }
        public double M1 { get { return Double.Parse(m1.Text.ToString()); } }
        public double S1 { get { return Double.Parse(s1.Text.ToString()); } }
        public String Dir1 { get { return dir1.Text.ToString(); } }

        public double D2 { get { return Double.Parse(d2.Text.ToString()); } }
        public double M2 { get { return Double.Parse(m2.Text.ToString()); } }
        public double S2 { get { return Double.Parse(s2.Text.ToString()); } }
        public String Dir2 { get { return dir2.Text.ToString(); } }

        public ZoomToCoordinatesDialog()
        {
            InitializeComponent();
        }

        private void ZoomToCoordinatesDialog_Load(object sender, EventArgs e)
        {

        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void enableAccept()
        {
            if ((d1.TextLength > 0)
                && (m1.TextLength > 0)
                && (s1.TextLength > 0)
                && (dir1.TextLength > 0)
                && (d2.TextLength > 0)
                && (m2.TextLength > 0)
                && (s2.TextLength > 0)
                && (dir2.TextLength > 0))
            {
                AcceptButton.Enabled = true;
            }
            else
            {
                AcceptButton.Enabled = false;
            }
        }

        private void d1_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void m1_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void s1_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void dir1_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void d2_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void m2_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void s2_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void dir2_TextChanged(object sender, EventArgs e)
        {
            enableAccept();
        }

        private void d1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void m1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void s1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dir1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                dir1.Text = "";
            }
        }

        private void d2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void m2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void s2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dir2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                dir2.Text = "";
            }
        }
    }
}

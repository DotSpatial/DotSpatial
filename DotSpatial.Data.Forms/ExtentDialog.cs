// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A Dialog that allows users to enter an X, Y, Z or M extent.
    /// </summary>
    public partial class ExtentDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the ExtentDialog class.
        /// </summary>
        public ExtentDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the extent specified by this form.
        /// </summary>
        public Extent Extent
        {
            get
            {
                Extent result;
                if (chkZ.Checked)
                {
                    ExtentMZ mz = new ExtentMZ();
                    if (dbxMinimumZ.IsValid)
                    {
                        mz.MinZ = dbxMinimumZ.Value;
                    }
                    if (dbxMaximumZ.IsValid)
                    {
                        mz.MaxZ = dbxMaximumZ.Value;
                    }
                    if (dbxMinimumM.IsValid)
                    {
                        mz.MinM = dbxMinimumM.Value;
                    }
                    if (dbxMaximumM.IsValid)
                    {
                        mz.MaxM = dbxMaximumM.Value;
                    }
                    result = mz;
                }
                else if (chkM.Checked)
                {
                    ExtentM m = new ExtentM();
                    if (dbxMinimumM.IsValid)
                    {
                        m.MinM = dbxMinimumM.Value;
                    }
                    if (dbxMaximumM.IsValid)
                    {
                        m.MaxM = dbxMaximumM.Value;
                    }
                    result = m;
                }
                else
                {
                    result = new Extent();
                }
                if (dbxMinimumX.IsValid)
                {
                    result.MinX = dbxMinimumX.Value;
                }
                if (dbxMaximumX.IsValid)
                {
                    result.MaxX = dbxMaximumX.Value;
                }
                if (dbxMinimumY.IsValid)
                {
                    result.MinY = dbxMinimumY.Value;
                }
                if (dbxMaximumY.IsValid)
                {
                    result.MaxY = dbxMaximumY.Value;
                }
                return result;
            }
            set
            {
                if (value == null)
                {
                    Clear();
                    return;
                }
                ExtentMZ mz = value as ExtentMZ;
                ExtentM m = value as ExtentM;
                if (mz != null)
                {
                    chkZ.Checked = true;
                    dbxMinimumZ.Enabled = true;
                    dbxMaximumZ.Enabled = true;
                    chkM.Checked = true;
                    dbxMinimumM.Enabled = true;
                    dbxMaximumM.Enabled = true;
                    dbxMinimumZ.Value = mz.MinZ;
                    dbxMaximumZ.Value = mz.MaxZ;
                    dbxMinimumM.Value = mz.MinM;
                    dbxMaximumM.Value = mz.MaxM;
                }
                else if (m != null)
                {
                    chkZ.Checked = false;
                    dbxMinimumZ.Enabled = false;
                    dbxMaximumZ.Enabled = false;
                    chkM.Checked = true;
                    dbxMinimumM.Enabled = true;
                    dbxMaximumM.Enabled = true;
                    dbxMinimumM.Value = m.MinM;
                    dbxMaximumM.Value = m.MaxM;
                }

                // X and Y content is always available.
                dbxMinimumX.Value = value.MinX;
                dbxMaximumX.Value = value.MaxX;
                dbxMinimumY.Value = value.MinY;
                dbxMaximumY.Value = value.MaxY;
            }
        }

        /// <summary>
        /// Resets all the values to 0.
        /// </summary>
        public void Clear()
        {
            dbxMinimumZ.Value = 0;
            dbxMaximumZ.Value = 0;
            dbxMinimumM.Value = 0;
            dbxMaximumM.Value = 0;
            dbxMaximumX.Value = 0;
            dbxMinimumX.Value = 0;
            dbxMaximumY.Value = 0;
            dbxMinimumY.Value = 0;
        }

        /// <summary>
        /// Prevents closing as "OK" if there are invalid double values.
        /// </summary>
        /// <param name="e">CancelEventArgs that allow canceling close.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                return;
            }
            List<string> names = new List<string>();
            if (chkZ.Checked)
            {
                if (!dbxMinimumZ.IsValid)
                {
                    names.Add("Minimum Z");
                }
                if (!dbxMaximumZ.IsValid)
                {
                    names.Add("Maximum Z");
                }
            }
            if (chkM.Checked)
            {
                if (!dbxMinimumM.IsValid)
                {
                    names.Add("Minimum M");
                }
                if (!dbxMaximumM.IsValid)
                {
                    names.Add("Maximum M");
                }
            }
            if (!dbxMinimumX.IsValid)
            {
                names.Add("Minimum X");
            }
            if (!dbxMaximumX.IsValid)
            {
                names.Add("Maximum X");
            }
            if (!dbxMinimumY.IsValid)
            {
                names.Add("Minimum Y");
            }
            if (!dbxMaximumY.IsValid)
            {
                names.Add("Maximum Y");
            }
            if (names.Count > 0)
            {
                MessageBox.Show(DataFormsMessageStrings.ExtentDialog_Fail_Pre + Join(names) +
                                DataFormsMessageStrings.ExtentDialog_Fail_Post,
                                DataFormsMessageStrings.ExtentDialog_Fail_Invalid_Value);

                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private static string Join(List<string> names)
        {
            string result = names[0];
            if (names.Count == 1)
            {
                return result;
            }
            if (names.Count > 2)
            {
                for (int i = 1; i < names.Count - 1; i++)
                {
                    result += ", " + names[i];
                }
            }
            result += ", and " + names[names.Count - 1];
            return result;
        }

        private void chkM_CheckedChanged(object sender, EventArgs e)
        {
            if (chkM.Checked)
            {
                dbxMinimumM.Enabled = true;
                dbxMaximumM.Enabled = true;
            }
            else
            {
                dbxMinimumM.Enabled = false;
                dbxMaximumM.Enabled = false;
            }
        }

        private void chkZ_CheckedChanged(object sender, EventArgs e)
        {
            if (chkZ.Checked)
            {
                dbxMinimumZ.Enabled = true;
                dbxMaximumZ.Enabled = true;
            }
            else
            {
                dbxMinimumZ.Enabled = false;
                dbxMaximumZ.Enabled = false;
            }
        }
    }
}
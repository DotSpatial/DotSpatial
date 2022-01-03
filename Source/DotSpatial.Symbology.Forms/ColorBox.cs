// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ColorBox.
    /// </summary>
    [DefaultEvent("SelectedItemChanged")]
    [DefaultProperty("Value")]
    public partial class ColorBox : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBox"/> class.
        /// </summary>
        public ColorBox()
        {
            InitializeComponent();
            cddColor.SelectedIndexChanged += CddColorSelectedIndexChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the selected color has been changed in the drop-down
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the font for the label portion of the component.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or set the font for the label portion of the component.")]
        public new Font Font
        {
            get
            {
                return lblColor.Font;
            }

            set
            {
                lblColor.Font = value;
                Reset();
            }
        }

        /// <summary>
        /// Gets or sets the text for the label portion.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the text for the label portion")]
        public string LabelText
        {
            get
            {
                return lblColor.Text;
            }

            set
            {
                lblColor.Text = value;
                Reset();
            }
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the selected color")]
        public Color Value
        {
            get
            {
                return cddColor.Value;
            }

            set
            {
                cddColor.Value = value;
            }
        }

        #endregion

        #region Methods

        private void CddColorSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CmdShowDialogClick(object sender, EventArgs e)
        {
            ColorDialog cdlg = new ColorDialog();
            if (cdlg.ShowDialog(ParentForm) != DialogResult.OK) return;
            foreach (object item in cddColor.Items)
            {
                if (item is KnownColor)
                {
                    KnownColor kn = (KnownColor)item;
                    if (Color.FromKnownColor(kn) == cdlg.Color)
                    {
                        cddColor.SelectedItem = kn;
                        return;
                    }
                }
            }

            if (cddColor.Items.Contains(cdlg.Color))
            {
                cddColor.SelectedItem = cdlg.Color;
            }
            else
            {
                cddColor.Items.Add(cdlg.Color);
                cddColor.SelectedIndex = cddColor.Items.Count - 1;
            }
        }

        /// <summary>
        /// Changes the starting location of the color drop down based on the current text.
        /// </summary>
        private void Reset()
        {
            cddColor.Left = lblColor.Width + 5;
            cddColor.Width = cmdShowDialog.Left - cddColor.Left - 10;
        }

        #endregion
    }
}
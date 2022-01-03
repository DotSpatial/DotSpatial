// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A UserControl for specifying the font family.
    /// </summary>
    [DefaultEvent("SelectedItemChanged")]
    [DefaultProperty("SelectedFamily")]
    public partial class FontFamilyControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFamilyControl"/> class, pre-loading a font drop down.
        /// </summary>
        public FontFamilyControl()
        {
            InitializeComponent();
            foreach (var family in FontFamily.Families)
            {
                ffdNames.Items.Add(family.Name);
            }

            ffdNames.SelectedItem = ffdNames.Items.Contains("Arial") ? "Arial" : ffdNames.Items[0]; // Arial does not exist on Linux
            ffdNames.SelectedValueChanged += FfdNamesSelectedValueChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently selected font family name.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedFamily
        {
            get
            {
                return ffdNames.SelectedItem.ToString();
            }

            set
            {
                ffdNames.SelectedItem = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the selected family name as a FontFamily object.
        /// </summary>
        /// <returns>A FontFamily object.</returns>
        public FontFamily GetSelectedFamily()
        {
            return new FontFamily(ffdNames.SelectedItem.ToString());
        }

        /// <summary>
        /// Throws a new event when the selected item changed.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        private void FfdNamesSelectedValueChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        #endregion
    }
}
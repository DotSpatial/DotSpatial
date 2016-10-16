// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created ?
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A UserControl for specifying the font family.
    /// </summary>
    [DefaultEvent("SelectedItemChanged"), DefaultProperty("SelectedFamily")]
    public partial class FontFamilyControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the Font Family control, pre-loading a font drop down.
        /// </summary>
        public FontFamilyControl()
        {
            InitializeComponent();
            foreach (var family in FontFamily.Families)
            {
                ffdNames.Items.Add(family.Name);
            }

            ffdNames.SelectedItem = ffdNames.Items.Contains("Arial") ? "Arial" : ffdNames.Items[0];  // Arial does not exist on Linux
            ffdNames.SelectedValueChanged += FfdNamesSelectedValueChanged;
        }

        /// <summary>
        /// Gets or sets the currently selected font family name.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedFamily
        {
            get { return ffdNames.SelectedItem.ToString(); }
            set { ffdNames.SelectedItem = value; }
        }

        /// <summary>
        /// Event
        /// </summary>
        public event EventHandler SelectedItemChanged;

        /// <summary>
        /// Gets the selected family name as a FontFamily object
        /// </summary>
        /// <returns>A FontFamily object</returns>
        public FontFamily GetSelectedFamily()
        {
            return new FontFamily(ffdNames.SelectedItem.ToString());
        }

        private void FfdNamesSelectedValueChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        /// <summary>
        /// Throws a new event when the selected item changed.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, EventArgs.Empty);
        }
    }
}
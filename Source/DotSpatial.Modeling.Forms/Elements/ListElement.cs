// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// List Element for use in the tool dialog.
    /// </summary>
    internal partial class ListElement : DialogElement
    {
        #region Fields
        private bool _disableComboRefresh;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents.</param>
        public ListElement(ListParam param)
        {
            // Needed by the designer
            InitializeComponent();
            GroupBox.Text = param.Name;

            // We save the parameters passed in
            Param = param;

            DoRefresh();

            // Update the state of the status light
            ComboBox1SelectedValueChanged(null, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new ListParam Param
        {
            get
            {
                return (ListParam)base.Param;
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
            DoRefresh();
        }

        /// <summary>
        /// When the control is clicked this event fires.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ComboBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void ComboBox1SelectedValueChanged(object sender, EventArgs e)
        {
            if (_disableComboRefresh) return;

            if (comboBox1.SelectedItem == null)
            {
                Status = ToolStatus.Empty;
                LightTipText = "The seletion is invalid, select and item from the drop down list.";
            }
            else
            {
                Status = ToolStatus.Ok;
                LightTipText = "The selection is valid";
                Param.Value = comboBox1.SelectedIndex;
            }
        }

        private void DoRefresh()
        {
            _disableComboRefresh = true;

            comboBox1.Items.Clear();

            // We load the items in the list
            for (int i = 0; i < Param.ValueList.Count; i++)
            {
                comboBox1.Items.Insert(i, Param.ValueList[i]);
            }

            // We set the default value
            if (base.Param.DefaultSpecified && (Param.Value >= 0))
            {
                comboBox1.SelectedIndex = Param.Value;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }

            _disableComboRefresh = false;
        }
        #endregion
    }
}
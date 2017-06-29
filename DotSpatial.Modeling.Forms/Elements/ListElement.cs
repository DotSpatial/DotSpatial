// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ListElement
// Description:  List Element for use in the tool dialog
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
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    internal class ListElement : DialogElement
    {
        #region Class Variables

        private bool _disableComboRefresh;
        private ComboBox comboBox1;

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public ListElement(ListParam param)
        {
            //Needed by the designer
            InitializeComponent();
            GroupBox.Text = param.Name;

            //We save the parameters passed in
            Param = param;

            DoRefresh();

            //Update the state of the status light
            ComboBox1SelectedValueChanged(null, null);
        }

        private void DoRefresh()
        {
            _disableComboRefresh = true;

            comboBox1.Items.Clear();

            //We load the items in the list
            for (int i = 0; i < Param.ValueList.Count; i++)
            {
                comboBox1.Items.Insert(i, Param.ValueList[i]);
            }

            //We set the default value
            if ((base.Param.DefaultSpecified) && (Param.Value >= 0))
            {
                comboBox1.SelectedIndex = Param.Value;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.FeaturesetValid;
            }

            _disableComboRefresh = false;
        }

        /// <summary>
        ///
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        #endregion

        #region Events

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
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
                return;
            }
        }

        /// <summary>
        /// When the control is clicked this event fires
        /// </summary>
        private void ComboBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new ListParam Param
        {
            get { return (ListParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #region Generate by the designer

        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(comboBox1);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(comboBox1, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblStatus
            //
            StatusLabel.Location = new Point(12, 20);
            //
            // comboBox1
            //
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(44, 17);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(440, 21);
            comboBox1.TabIndex = 5;
            comboBox1.SelectedValueChanged += ComboBox1SelectedValueChanged;
            comboBox1.Click += ComboBox1Click;
            //
            // ListElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "ListElement";
            Size = new Size(492, 46);
            GroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/4/2009 1:56:04 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LineJoinControl
    /// </summary>
    [DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    public class LineJoinControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when one of the radio buttons is clicked, changing the current value.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Private Variables

        private LineJoinType _joinType;
        private GroupBox grpLineJoins;
        private RadioButton radBevel;
        private RadioButton radMiter;
        private RadioButton radRound;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineJoinControl
        /// </summary>
        public LineJoinControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LineJoinControl));
            this.grpLineJoins = new GroupBox();
            this.radBevel = new RadioButton();
            this.radRound = new RadioButton();
            this.radMiter = new RadioButton();
            this.grpLineJoins.SuspendLayout();
            this.SuspendLayout();
            //
            // grpLineJoins
            //
            this.grpLineJoins.AccessibleDescription = null;
            this.grpLineJoins.AccessibleName = null;
            resources.ApplyResources(this.grpLineJoins, "grpLineJoins");
            this.grpLineJoins.BackgroundImage = null;
            this.grpLineJoins.Controls.Add(this.radBevel);
            this.grpLineJoins.Controls.Add(this.radRound);
            this.grpLineJoins.Controls.Add(this.radMiter);
            this.grpLineJoins.Font = null;
            this.grpLineJoins.Name = "grpLineJoins";
            this.grpLineJoins.TabStop = false;
            this.grpLineJoins.Enter += this.grpLineJoins_Enter;
            //
            // radBevel
            //
            this.radBevel.AccessibleDescription = null;
            this.radBevel.AccessibleName = null;
            resources.ApplyResources(this.radBevel, "radBevel");
            this.radBevel.BackgroundImage = null;
            this.radBevel.Font = null;
            this.radBevel.Name = "radBevel";
            this.radBevel.UseVisualStyleBackColor = true;
            this.radBevel.CheckedChanged += this.radBevel_CheckedChanged;
            //
            // radRound
            //
            this.radRound.AccessibleDescription = null;
            this.radRound.AccessibleName = null;
            resources.ApplyResources(this.radRound, "radRound");
            this.radRound.BackgroundImage = null;
            this.radRound.Checked = true;
            this.radRound.Font = null;
            this.radRound.Name = "radRound";
            this.radRound.TabStop = true;
            this.radRound.UseVisualStyleBackColor = true;
            this.radRound.CheckedChanged += this.radRound_CheckedChanged;
            //
            // radMiter
            //
            this.radMiter.AccessibleDescription = null;
            this.radMiter.AccessibleName = null;
            resources.ApplyResources(this.radMiter, "radMiter");
            this.radMiter.BackgroundImage = null;
            this.radMiter.Font = null;
            this.radMiter.Name = "radMiter";
            this.radMiter.UseVisualStyleBackColor = true;
            this.radMiter.CheckedChanged += this.radMiter_CheckedChanged;
            //
            // LineJoinControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.grpLineJoins);
            this.Font = null;
            this.Name = "LineJoinControl";
            this.grpLineJoins.ResumeLayout(false);
            this.grpLineJoins.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string text
        /// </summary>
        public override string Text
        {
            get
            {
                if (grpLineJoins != null) return grpLineJoins.Text;
                return null;
            }
            set
            {
                if (grpLineJoins != null) grpLineJoins.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the current line join type shown by the control.
        /// </summary>
        public LineJoinType Value
        {
            get { return _joinType; }
            set
            {
                _joinType = value;
                switch (value)
                {
                    case LineJoinType.Bevel: radBevel.Checked = true; break;
                    case LineJoinType.Mitre: radMiter.Checked = true; break;
                    case LineJoinType.Round: radRound.Checked = true; break;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the on value changed event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }

        #endregion

        private void radMiter_CheckedChanged(object sender, EventArgs e)
        {
            if (radMiter.Checked && _joinType != LineJoinType.Mitre)
            {
                _joinType = LineJoinType.Mitre;
                OnValueChanged();
            }
        }

        private void radRound_CheckedChanged(object sender, EventArgs e)
        {
            if (radRound.Checked && _joinType != LineJoinType.Round)
            {
                _joinType = LineJoinType.Round;
                OnValueChanged();
            }
        }

        private void radBevel_CheckedChanged(object sender, EventArgs e)
        {
            if (radBevel.Checked && _joinType != LineJoinType.Bevel)
            {
                _joinType = LineJoinType.Bevel;
                OnValueChanged();
            }
        }

        private void grpLineJoins_Enter(object sender, EventArgs e)
        {
        }
    }
}
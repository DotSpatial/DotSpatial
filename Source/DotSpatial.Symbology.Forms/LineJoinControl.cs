// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LineJoinControl
    /// </summary>
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    public partial class LineJoinControl : UserControl
    {
        #region Fields

        private LineJoinType _joinType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineJoinControl"/> class.
        /// </summary>
        public LineJoinControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when one of the radio buttons is clicked, changing the current value.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string text
        /// </summary>
        public override string Text
        {
            get
            {
                return grpLineJoins?.Text;
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
            get
            {
                return _joinType;
            }

            set
            {
                _joinType = value;
                switch (value)
                {
                    case LineJoinType.Bevel:
                        radBevel.Checked = true;
                        break;
                    case LineJoinType.Mitre:
                        radMiter.Checked = true;
                        break;
                    case LineJoinType.Round:
                        radRound.Checked = true;
                        break;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the on value changed event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void GrpLineJoinsEnter(object sender, EventArgs e)
        {
        }

        private void RadBevelCheckedChanged(object sender, EventArgs e)
        {
            if (radBevel.Checked && _joinType != LineJoinType.Bevel)
            {
                _joinType = LineJoinType.Bevel;
                OnValueChanged();
            }
        }

        private void RadMiterCheckedChanged(object sender, EventArgs e)
        {
            if (radMiter.Checked && _joinType != LineJoinType.Mitre)
            {
                _joinType = LineJoinType.Mitre;
                OnValueChanged();
            }
        }

        private void RadRoundCheckedChanged(object sender, EventArgs e)
        {
            if (radRound.Checked && _joinType != LineJoinType.Round)
            {
                _joinType = LineJoinType.Round;
                OnValueChanged();
            }
        }

        #endregion
    }
}
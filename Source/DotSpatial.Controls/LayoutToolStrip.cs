// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a base class for the layout tool strips.
    /// </summary>
    public abstract class LayoutToolStrip : ToolStrip
    {
        #region Fields

        private LayoutControl _layoutControl;

        #endregion

        #region Events

        /// <summary>
        /// This fires after a button was clicked to indicate to the layout control that it should inform the other toolstrips that they have to deactivate all their buttons.
        /// </summary>
        public event EventHandler ButtonChecked;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layout control associated with this toolstrip.
        /// </summary>
        [Browsable(false)]
        public virtual LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (_layoutControl != null)
                {
                    _layoutControl.ButtonChecked -= LayoutControlButtonChecked;
                }

                _layoutControl = value;
                _layoutControl.ButtonChecked += LayoutControlButtonChecked;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the given button and unchecks all others if the given button is not already checked.
        /// </summary>
        /// <param name="button">The button that should be checked.</param>
        protected void SetChecked(ToolStripButton button)
        {
            if (!button.Checked)
            {
                button.Checked = true;
                UncheckAll(button);
            }
        }

        /// <summary>
        /// Unchecks all buttons except ignored.
        /// </summary>
        /// <param name="ignored">The button that should not be unchecked.</param>
        protected void UncheckAll(ToolStripButton ignored = null)
        {
            foreach (var button in Items.OfType<ToolStripButton>())
            {
                if (button != ignored)
                {
                    button.Checked = false;
                }
            }

            if (ignored != null)
            {
                OnButtonChecked();
            }
        }

        /// <summary>
        /// Unchecks all the buttons when the layout control indicates that a button of another layout toolstrip was activated.
        /// </summary>
        /// <param name="sender">The toolstrip that contains the button that was activated.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlButtonChecked(object sender, EventArgs e)
        {
            if (sender != this)
            {
                UncheckAll();
            }
        }

        /// <summary>
        /// Call this to indicated that another button was checked.
        /// </summary>
        private void OnButtonChecked()
        {
            ButtonChecked?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
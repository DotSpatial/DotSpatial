// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 10:46:15 AM
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
    /// A collection of controls that are specifically designed to work with the outline on a polygon.
    /// </summary>
    [DefaultEvent("OutlineChanged")]
    public partial class OutlineControl : UserControl
    {
        #region Fields

        private readonly IPattern _original;
        private bool _ignoreChanges;
        private IPattern _pattern;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlineControl"/> class.
        /// </summary>
        public OutlineControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlineControl"/> class that uses the specified pattern to define its configuration.
        /// </summary>
        /// <param name="pattern">The pattern that will be modified during changes.</param>
        public OutlineControl(IPattern pattern)
        {
            _pattern = pattern;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlineControl"/> class that uses the specified pattern to define its configuration.
        /// </summary>
        /// <param name="original">When apply changes are clicked, the map will be updated when these changes are copied over.</param>
        /// <param name="display">The pattern that will be modified during changes.</param>
        public OutlineControl(IPattern original, IPattern display)
        {
            _original = original;
            _pattern = display;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs specifically when changes are applied from the line symbolizer editor
        /// </summary>
        public event EventHandler ChangesApplied;

        /// <summary>
        /// Occurs when any of the symbolic aspects of this control are changed.
        /// </summary>
        public event EventHandler OutlineChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pattern for this control
        /// </summary>
        public IPattern Pattern
        {
            get
            {
                return _pattern;
            }

            set
            {
                _pattern = value;
                UpdateOutlineControls();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the OutlineChanged event.
        /// </summary>
        protected virtual void OnChangesApplied()
        {
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the OutlineChanged event.
        /// </summary>
        protected virtual void OnOutlineChanged()
        {
            OutlineChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnEditOutlineClick(object sender, EventArgs e)
        {
            DetailedLineSymbolDialog dlg;
            if (_original != null)
            {
                dlg = new DetailedLineSymbolDialog(_pattern.Outline);
            }
            else if (_pattern != null)
            {
                dlg = new DetailedLineSymbolDialog(_pattern.Outline);
            }
            else
            {
                return;
            }

            dlg.ChangesApplied += DlgChangesApplied;
            dlg.ShowDialog();
        }

        private void CbOutlineColorColorChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;

            _pattern?.Outline.SetFillColor(cbOutlineColor.Color);

            sldOutlineOpacity.Value = cbOutlineColor.Color.GetOpacity();
            sldOutlineOpacity.MaximumColor = cbOutlineColor.Color.ToOpaque();
            OnOutlineChanged();
        }

        private void ChkUseOutlineCheckedChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;

            if (_pattern != null)
            {
                _pattern.UseOutline = chkUseOutline.Checked;
            }

            OutlineChanged?.Invoke(this, EventArgs.Empty);
        }

        private void DbxOutlineWidthTextChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;

            if (_pattern != null)
            {
                if (_pattern.Outline == null)
                {
                    _pattern.Outline = new LineSymbolizer(cbOutlineColor.Color, dbxOutlineWidth.Value);
                }
                else
                {
                    _pattern.Outline.SetWidth(dbxOutlineWidth.Value);
                }
            }

            OnOutlineChanged();
        }

        private void DlgChangesApplied(object sender, EventArgs e)
        {
            UpdateOutlineControls();
            OnChangesApplied();
            OnOutlineChanged();
        }

        private void SldOutlineOpacityValueChanged(object sender, EventArgs e)
        {
            if (_ignoreChanges) return;

            _pattern?.Outline.SetFillColor(_pattern.Outline.GetFillColor().ToTransparent((float)sldOutlineOpacity.Value));

            cbOutlineColor.Color = sldOutlineOpacity.MaximumColor.ToTransparent((float)sldOutlineOpacity.Value);
            OnOutlineChanged();
        }

        private void UpdateOutlineControls()
        {
            _ignoreChanges = true;
            if (_pattern != null)
            {
                chkUseOutline.Checked = _pattern.UseOutline;
                if (_pattern.Outline != null)
                {
                    cbOutlineColor.Color = _pattern.Outline.GetFillColor();
                    sldOutlineOpacity.MaximumColor = cbOutlineColor.Color.ToOpaque();
                    sldOutlineOpacity.Value = cbOutlineColor.Color.GetOpacity();
                    dbxOutlineWidth.Value = _pattern.Outline.GetWidth();
                }
            }

            _ignoreChanges = false;
        }

        #endregion
    }
}
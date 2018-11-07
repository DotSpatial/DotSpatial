// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// FeatureSizeRangeControl
    /// </summary>
    [DefaultEvent("SizeRangeChanged")]
    public partial class FeatureSizeRangeControl : UserControl
    {
        #region Fields

        private readonly DetailedLineSymbolDialog _lineDialog;
        private readonly DetailedPointSymbolDialog _pointDialog;
        private bool _ignore;
        private IFeatureScheme _scheme;
        private FeatureSizeRange _sizeRange;
        private CultureInfo _featSizeCulture;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSizeRangeControl"/> class.
        /// </summary>
        public FeatureSizeRangeControl()
        {
            InitializeComponent();
            _featSizeCulture = new CultureInfo(string.Empty);

            _pointDialog = new DetailedPointSymbolDialog();
            _pointDialog.ChangesApplied += PointDialogChangesApplied;
            _lineDialog = new DetailedLineSymbolDialog();
            _lineDialog.ChangesApplied += LineDialogChangesApplied;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when either the sizes or the template has changed.
        /// </summary>
        public event EventHandler<SizeRangeEventArgs> SizeRangeChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the point scheme to work with.
        /// </summary>
        public IFeatureScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                _scheme = value;
                IPointScheme ps = _scheme as IPointScheme;
                if (ps?.Categories.Count > 0)
                {
                    _sizeRange.Symbolizer = ps.Categories[0].Symbolizer;
                }

                ILineScheme ls = _scheme as ILineScheme;
                if (ls?.Categories.Count > 0)
                {
                    _sizeRange.Symbolizer = ls.Categories[0].Symbolizer;
                }

                UpdateControls();
            }
        }

        /// <summary>
        /// Gets or sets the point Size Range, which controls the symbolizer,
        /// as well as allowing the creation of a dynamically sized version
        /// of the symbolizer.
        /// </summary>
        public FeatureSizeRange SizeRange
        {
            get
            {
                return _sizeRange;
            }

            set
            {
                _sizeRange = value;
                UpdateControls();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the culture to use for resources.
        /// </summary>
        public virtual CultureInfo FeatSizeCulture
        {
            get
            {
                return _featSizeCulture;
            }

            set
            {
                if (_featSizeCulture == value) return;
                _featSizeCulture = value;
                if (_featSizeCulture == null) _featSizeCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _featSizeCulture;
                Thread.CurrentThread.CurrentUICulture = _featSizeCulture;
                Refresh();

                UpdateFeatSizeResources();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this point size range control.
        /// </summary>
        /// <param name="args">The SizeRangeEventArgs</param>
        public void Initialize(SizeRangeEventArgs args)
        {
            if (_sizeRange == null) return;

            _sizeRange.Start = args.StartSize;
            _sizeRange.End = args.EndSize;
            _sizeRange.Symbolizer = args.Template;
            _sizeRange.UseSizeRange = args.UseSizeRange;
            IPointSymbolizer ps = args.Template as IPointSymbolizer;
            if (ps != null)
            {
                psvStart.Visible = true;
                psvEnd.Visible = true;
                lsvStart.Visible = false;
                lsvEnd.Visible = false;
            }

            ILineSymbolizer ls = args.Template as ILineSymbolizer;
            if (ls != null)
            {
                lsvStart.Visible = true;
                lsvEnd.Visible = true;
                psvStart.Visible = false;
                psvEnd.Visible = false;
            }
        }

        /// <summary>
        /// Handles the inter-connectivity of the various controls and updates them all to match the latest value.
        /// </summary>
        public void UpdateControls()
        {
            if (_ignore) return;

            _ignore = true;

            if (_sizeRange == null)
            {
                _ignore = false;
                return;
            }

            chkSizeRange.Checked = _sizeRange.UseSizeRange;
            trkStart.Value = (int)_sizeRange.Start;
            trkEnd.Value = (int)_sizeRange.End;
            IPointSymbolizer ps = _sizeRange.Symbolizer as IPointSymbolizer;
            if (ps != null)
            {
                Color color = ps.GetFillColor();
                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.StartColor;
                }

                if (chkSizeRange.Checked)
                {
                    psvStart.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.Start, color) as IPointSymbolizer;
                }
                else
                {
                    IPointSymbolizer sm = ps.Copy();
                    sm.SetFillColor(color);
                    psvStart.Symbolizer = sm;
                }

                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.EndColor;
                }

                if (chkSizeRange.Checked)
                {
                    psvEnd.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.End, color) as IPointSymbolizer;
                }
                else
                {
                    IPointSymbolizer sm = ps.Copy();
                    sm.SetFillColor(color);
                    psvEnd.Symbolizer = sm;
                }
            }

            ILineSymbolizer ls = _sizeRange.Symbolizer as ILineSymbolizer;
            if (ls != null)
            {
                Color color = ls.GetFillColor();
                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.StartColor;
                }

                if (chkSizeRange.Checked)
                {
                    lsvStart.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.Start, color) as ILineSymbolizer;
                }
                else
                {
                    ILineSymbolizer sm = ls.Copy();
                    sm.SetFillColor(color);
                    lsvStart.Symbolizer = sm;
                }

                if (_scheme != null && _scheme.EditorSettings.UseColorRange)
                {
                    color = _scheme.EditorSettings.EndColor;
                }

                if (chkSizeRange.Checked)
                {
                    lsvEnd.Symbolizer = _sizeRange.GetSymbolizer(_sizeRange.End, color) as ILineSymbolizer;
                }
                else
                {
                    ILineSymbolizer sm = ls.Copy();
                    sm.SetFillColor(color);
                    lsvEnd.Symbolizer = sm;
                }
            }

            nudStart.Value = (decimal)_sizeRange.Start;
            nudEnd.Value = (decimal)_sizeRange.End;

            _ignore = false;

            OnSizeRangeChanged();
        }

        /// <summary>
        /// Fires the SizeRangeChanged event args
        /// </summary>
        protected virtual void OnSizeRangeChanged()
        {
            SizeRangeChanged?.Invoke(this, new SizeRangeEventArgs(_sizeRange));
        }

        private void LineDialogChangesApplied(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.Symbolizer = _lineDialog.Symbolizer;
            UpdateControls();
        }

        private void PointDialogChangesApplied(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.Symbolizer = _pointDialog.Symbolizer;
            UpdateControls();
        }

        private void BtnEditClick(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            IPointSymbolizer ps = _sizeRange.Symbolizer as IPointSymbolizer;
            if (ps != null)
            {
                _pointDialog.Symbolizer = ps;
                _pointDialog.ShowDialog(this);
            }

            ILineSymbolizer ls = _sizeRange.Symbolizer as ILineSymbolizer;
            if (ls != null)
            {
                _lineDialog.Symbolizer = ls;
                _lineDialog.ShowDialog(this);
            }
        }

        private void ChkSizeRangeCheckedChanged(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.UseSizeRange = chkSizeRange.Checked;
            UpdateControls();
        }

        private void NudEndValueChanged(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.End = (int)nudEnd.Value;
            UpdateControls();
        }

        private void NudStartValueChanged(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.Start = (int)nudStart.Value;
            UpdateControls();
        }

        private void TrkEndScroll(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.End = trkEnd.Value;
            UpdateControls();
        }

        private void TrkStartScroll(object sender, EventArgs e)
        {
            if (_sizeRange == null) return;

            _sizeRange.Start = trkStart.Value;
            UpdateControls();
        }

        private void UpdateFeatSizeResources()
        {
            resources.ApplyResources(chkSizeRange, "chkSizeRange");
            resources.ApplyResources(btnEdit, "btnEdit");
            resources.ApplyResources(label1, "label1");
            resources.ApplyResources(label2, "label2");
        }

        #endregion
    }
}
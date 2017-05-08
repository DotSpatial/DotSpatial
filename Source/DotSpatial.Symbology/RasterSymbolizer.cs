// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created February 2008
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// RasterSymbolizer
    /// </summary>
    [Serializable]
    public class RasterSymbolizer : LegendItem, IRasterSymbolizer
    {
        #region Fields

        private bool _colorSchemeHasChanged;
        private bool _colorSchemeHasUpdated;
        private bool _drapeVectorLayers;
        private float _elevationFactor;
        private float _extrusion;
        private float[][] _hillshade;
        private IFeatureSymbolizerOld _imageOutline;

        private bool _isElevation;
        private bool _isSmoothed;
        private bool _isVisible = true;
        private bool _meshHasChanged;
        private Color _noDataColor;
        private IRasterLayer _parentLayer;
        private IRaster _raster;
        private IColorScheme _scheme;
        private IShadedRelief _shadedRelief;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSymbolizer"/> class.
        /// </summary>
        /// <param name="layer">The parent item</param>
        public RasterSymbolizer(IRasterLayer layer)
        {
            SetParentItem(layer);
            _parentLayer = layer;
            _raster = layer.DataSet;

            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSymbolizer"/> class.
        /// </summary>
        public RasterSymbolizer()
        {
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs if any of the properties that would contribute to bitmap construction have changed
        /// </summary>
        public event EventHandler ColorSchemeChanged;

        /// <summary>
        /// This event occurs after a new bitmap has been created to act as a texture.
        /// </summary>
        public event EventHandler ColorSchemeUpdated;

        /// <summary>
        /// Occurs when the symbology has been changed
        /// </summary>
        public event EventHandler SymbologyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the color has changed.
        /// </summary>
        public bool ColorSchemeHasChanged
        {
            get
            {
                return _colorSchemeHasChanged;
            }

            set
            {
                _colorSchemeHasChanged = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the texture needs to be reloaded from a file.
        /// </summary>
        public bool ColorSchemeHasUpdated
        {
            get
            {
                return _colorSchemeHasUpdated;
            }

            set
            {
                _colorSchemeHasUpdated = value;
                if (_colorSchemeHasUpdated)
                {
                    ColorSchemeHasChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the vector layers are drawn onto the texture, whenever a texture is created.
        /// </summary>
        public bool DrapeVectorLayers
        {
            get
            {
                return _drapeVectorLayers;
            }

            set
            {
                _drapeVectorLayers = value;
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the editor settings class to help setup up the symbology controls appropriately.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Serialize("EditorSettings")]
        public RasterEditorSettings EditorSettings
        {
            get
            {
                return _scheme.EditorSettings;
            }

            set
            {
                _scheme.EditorSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the elevation factor. This is kept separate from extrusion to reduce confusion.
        /// This is a conversion factor that will convert the units of elevation into the same units that
        /// the latitude and longitude are stored in. To convert feet to decimal degrees is around a factor
        /// of .00000274. This is used only in the 3D-context and does not affect ShadedRelief.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the factor required to change elevation into the same horizontal and vertical units.")]
        [Serialize("Elevation Factor")]
        public virtual float ElevationFactor
        {
            get
            {
                return _elevationFactor;
            }

            set
            {
                _elevationFactor = value;
                _meshHasChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets a float value expression that modifies the "height" of the apparent shaded relief. A value
        /// of 1 should show the mountains at their true elevations, presuming the ElevationFactor is
        /// correct. A value of 0 would be totally flat, while 2 would be twice the value. This controls
        /// the 3D effects and has nothing to do with the creation of shaded releif on the texture.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the exageration of the natural elevation values.")]
        [Serialize("Extrusion")]
        public virtual float Extrusion
        {
            get
            {
                return _extrusion;
            }

            set
            {
                _extrusion = value;
                _meshHasChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the calculated hillshade map.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float[][] HillShade
        {
            get
            {
                if (_shadedRelief.IsUsed == false) return null;

                if (ShadedRelief.HasChanged || _hillshade == null)
                {
                    _hillshade = Raster.CreateHillShade(ShadedRelief);
                }

                return _hillshade;
            }

            set
            {
                _hillshade = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbol characteristics for the border of this raster.
        /// </summary>
        [Category("Symbology")]
        [TypeConverter(typeof(FeatureSymbolizerOld))]
        [Description("Gets or sets the characteristics that control the features of the border around this raster.")]
        [Serialize("ImageOutline")]
        public virtual IFeatureSymbolizerOld ImageOutline
        {
            get
            {
                return _imageOutline;
            }

            set
            {
                _imageOutline = value;
                OnSymbologyChange();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to treat the values as if they are elevation
        /// in the 3-D context. If this is true, then it will automatically use this grid for
        /// calculating elevation values. This does not affect ShadedRelief texture creation.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets whether this raster is an elevation raster and should use itself as the source for elevations.")]
        [Serialize("IsElevation")]
        public virtual bool IsElevation
        {
            get
            {
                return _isElevation;
            }

            set
            {
                _isElevation = value;
                _meshHasChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this raster should be anti-alliased.
        /// </summary>
        [Serialize("IsSmoothed")]
        public virtual bool IsSmoothed
        {
            get
            {
                return _isSmoothed;
            }

            set
            {
                _isSmoothed = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this raster should render itself.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets whether this raster should render itself")]
        [Serialize("IsVisible")]
        public virtual bool IsVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the elevation values have changed.
        /// </summary>
        public bool MeshHasChanged
        {
            get
            {
                return _meshHasChanged;
            }

            set
            {
                _meshHasChanged = value;
            }
        }

        /// <summary>
        /// Gets or sets the color to use if the value of the cell corresponds to a No-Data value.
        /// </summary>
        [Category("Symbology")]
        [Description("Gets or sets the color to use if the value of the cell corresponds to a No-Data value.")]
        [Serialize("NoDataColor")]
        public Color NoDataColor
        {
            get
            {
                return _noDataColor;
            }

            set
            {
                _noDataColor = value;
                OnColorSchemeChanged();
            }
        }

        /// <summary>
        /// Gets or sets a float value from 0 to 1, where 1 is fully opaque while 0 is fully transparent.
        /// </summary>
        public float Opacity
        {
            get
            {
                return _scheme?.Opacity ?? 1;
            }

            set
            {
                if (_scheme == null) return;

                _scheme.Opacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent layer. This is not always used, but can be useful for symbolic editing
        /// that may require a bitmap to be drawn with draped vector layers.
        /// </summary>
        [ShallowCopy]
        public IRasterLayer ParentLayer
        {
            get
            {
                return _parentLayer;
            }

            set
            {
                _parentLayer = value;
            }
        }

        /// <summary>
        /// Gets or sets the raster that should provide elevation values, but only if "IsElevation" is false.
        /// </summary>
        [ReadOnly(true)]
        [DisplayName(@"Elevation Raster")]
        [Description("The raster object to use as an Elevation Grid")]
        [ShallowCopy]
        public virtual IRaster Raster
        {
            get
            {
                return _raster;
            }

            set
            {
                _raster = value;
            }
        }

        /// <summary>
        /// Gets or sets the raster coloring scheme.
        /// </summary>
        [Serialize("Scheme")]
        public IColorScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                _scheme?.SetParentItem(null);
                _scheme = value;
                _scheme?.SetParentItem(_parentLayer);
            }
        }

        /// <summary>
        /// Gets or sets the characteristics of the shaded relief. This is specifically used
        /// to control HillShade characteristics of the BitMap texture creation.
        /// </summary>
        [Category("Symbology")]
        [Description("Gets or sets the set of characteristics that control the HillShade characteristics for the visual texture.")]
        [Serialize("ShadedRelief")]
        public virtual IShadedRelief ShadedRelief
        {
            get
            {
                return _shadedRelief;
            }

            set
            {
                _shadedRelief = value;

                // still not sure what kind of update to use here, if any
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a bmp from the in-memory portion of the raster. This will be stored as a
        /// fileName with the same name as the current raster, but ends in bmp.
        /// </summary>
        /// <returns>The created bmp.</returns>
        public Bitmap CreateBitmap()
        {
            string fileName = Path.ChangeExtension(_raster.Filename, ".bmp");
            Bitmap bmp = new Bitmap(_raster.NumRows, _raster.NumColumns, PixelFormat.Format32bppArgb);
            bmp.Save(fileName); // this is needed so that lockbits doesn't cause exceptions
            _raster.DrawToBitmap(this, bmp);
            bmp.Save(fileName);
            return bmp;
        }

        /// <summary>
        /// Causes the raster to calculate a hillshade based on this symbolizer
        /// </summary>
        public void CreateHillShade()
        {
            _hillshade = Raster.CreateHillShade(_shadedRelief);
        }

        /// <summary>
        /// Causes the raster to calculate a hillshade using the specified progress handler
        /// </summary>
        /// <param name="progressHandler">The progress handler to use</param>
        public void CreateHillShade(IProgressHandler progressHandler)
        {
            _hillshade = Raster.CreateHillShade(_shadedRelief, progressHandler);
        }

        /// <summary>
        /// Gets the color information for a specific value. This does not include any hillshade information.
        /// </summary>
        /// <param name="value">Specifies the value to obtain a color for.</param>
        /// <returns>A Color</returns>
        public virtual Color GetColor(double value)
        {
            if (value == Raster.NoDataValue) return NoDataColor;

            foreach (var cb in _scheme.Categories)
            {
                if (cb.Contains(value))
                {
                    return cb.CalculateColor(value);
                }
            }

            return Color.Transparent;
        }

        /// <summary>
        /// Creates a bitmap based on the specified RasterSymbolizer
        /// </summary>
        /// <param name="bitmap"> the bitmap to paint to</param>
        /// <param name="progressHandler">The progress handler</param>
        public void PaintShadingToBitmap(Bitmap bitmap, IProgressHandler progressHandler)
        {
            BitmapData bmpData;

            if (_hillshade == null)
            {
                return;
            }

            // Create a new Bitmap and use LockBits combined with Marshal.Copy to get an array of bytes to work with.
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName == "format")
                {
                    throw new BitmapFormatException();
                }

                throw;
            }

            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);

            float[][] hillshade = _hillshade;

            ProgressMeter pm = new ProgressMeter(progressHandler, SymbologyMessageStrings.DesktopRasterExt_PaintingHillshade, bitmap.Height);
            if (bitmap.Width * bitmap.Height < 100000) pm.StepPercent = 50;
            if (bitmap.Width * bitmap.Height < 500000) pm.StepPercent = 10;
            if (bitmap.Width * bitmap.Height < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < bitmap.Height; row++)
            {
                for (int col = 0; col < bitmap.Width; col++)
                {
                    int offset = (row * bmpData.Stride) + (col * 4);
                    byte b = rgbData[offset];
                    byte g = rgbData[offset + 1];
                    byte r = rgbData[offset + 2];

                    // rgbData[offset + 3] = a; don't worry about alpha
                    int red = Convert.ToInt32(r * hillshade[row][col]);
                    int green = Convert.ToInt32(g * hillshade[row][col]);
                    int blue = Convert.ToInt32(b * hillshade[row][col]);
                    if (red > 255) red = 255;
                    if (green > 255) green = 255;
                    if (blue > 255) blue = 255;
                    if (red < 0) red = 0;
                    if (green < 0) green = 0;
                    if (blue < 0) blue = 0;
                    b = (byte)blue;
                    r = (byte)red;
                    g = (byte)green;

                    rgbData[offset] = b;
                    rgbData[offset + 1] = g;
                    rgbData[offset + 2] = r;
                }

                pm.CurrentValue = row;
            }

            pm.Reset();

            // Copy the values back into the bitmap
            Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// Sends a symbology updated event, which should cause the layer to be refreshed.
        /// </summary>
        public void Refresh()
        {
            OnColorSchemeChanged();
            _meshHasChanged = true;
        }

        /// <summary>
        /// Indicates that the bitmap has been updated and that the colorscheme is currently
        /// synchronized with the characteristics of this symbolizer. This also fires the
        /// ColorSchemeChanged event.
        /// </summary>
        public void Validate()
        {
            _colorSchemeHasUpdated = false;
            OnColorSchemeChanged();
        }

        /// <summary>
        /// Fires the on color scheme changed event
        /// </summary>
        protected virtual void OnColorSchemeChanged()
        {
            _colorSchemeHasChanged = true;
            ColorSchemeChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the SymbologyUpdated event, which should happen after symbology choices are finalized,
        /// a new texture has been created and we are ready for an update.
        /// </summary>
        protected virtual void OnColorSchemeUpdated()
        {
            ColorSchemeHasChanged = false;
            _colorSchemeHasUpdated = true;
            ColorSchemeUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the SymbologyChanged event
        /// </summary>
        protected virtual void OnSymbologyChange()
        {
            SymbologyChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Adds a category with the given values to the Scheme.
        /// </summary>
        /// <param name="startValue">First value, that belongs to this category.</param>
        /// <param name="endValue">Last value, that belongs to this category.</param>
        /// <param name="color">Color of this category.</param>
        private void AddCategory(double startValue, double endValue, Color color)
        {
            ICategory newCat = new ColorCategory(startValue, endValue, color, color);
            newCat.Range.MaxIsInclusive = true;
            newCat.Range.MinIsInclusive = true;
            newCat.LegendText = startValue.ToString(CultureInfo.CurrentCulture);
            Scheme.AddCategory(newCat);
        }

        private void Configure()
        {
            _shadedRelief = new ShadedRelief();
            _noDataColor = Color.Transparent;
            Scheme = new ColorScheme();
            if (_raster != null)
            {
                var colors = _raster.CategoryColors();
                if (colors != null && colors.Length > 0)
                {
                    // Use colors that are built into the raster, e.g. GeoTIFF with palette
                    _isElevation = false;

                    // use all colors instead of unique colors because unique colors are not always set/correct
                    int lastColor = colors[0].ToArgb(); // changed by jany_ 2015-06-02
                    int firstNr = 0;

                    // group succeeding values with the same color to the same category
                    for (int i = 1; i < colors.Length; i++)
                    {
                        int hash = colors[i].ToArgb();
                        if (hash != lastColor)
                        {
                            // the current color differs from the one before so we add a category for the color before
                            AddCategory(firstNr, i - 1, colors[firstNr]);
                            firstNr = i;
                            lastColor = hash;
                        }

                        if (i == colors.Length - 1) // this is the last color, so we add the last category
                            AddCategory(firstNr, i, colors[firstNr]);
                    }
                }
                else
                {
                    // Assume grid is elevation
                    _elevationFactor = 1 / 3640000f;
                    _isElevation = true;
                    _extrusion = 1;
                    _scheme.ApplyScheme(ColorSchemeType.FallLeaves, _raster);
                }
            }
        }

        #endregion
    }
}
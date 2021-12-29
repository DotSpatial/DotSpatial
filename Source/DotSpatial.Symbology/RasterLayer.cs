// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A raster layer describes using a single raster, and the primary application will be using this as a texture.
    /// </summary>
    public class RasterLayer : Layer, IRasterLayer
    {
        #region Fields

        private IGetBitmap _bitmapGetter;

        [Serialize("Symbolizer", ConstructorArgumentIndex = 1)]
        private IRasterSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterLayer"/> class.
        /// Opens the specified fileName using the layer manager.
        /// </summary>
        /// <param name="fileName">The string fileName to use in order to open the file.</param>
        /// <param name="symbolizer">The symbolizer to use for this layer.</param>
        public RasterLayer(string fileName, IRasterSymbolizer symbolizer)
        {
            DataSet = DataManager.DefaultDataManager.OpenRaster(fileName);
            Symbolizer = symbolizer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterLayer"/> class.
        /// Opens the specified fileName and automatically creates a raster that can be used by this raster layer.
        /// </summary>
        /// <param name="fileName">The string fileName to use in order to open the file.</param>
        /// <param name="inProgressHandler">The progress handler to show progress messages.</param>
        public RasterLayer(string fileName, IProgressHandler inProgressHandler)
            : base(inProgressHandler)
        {
            DataSet = DataManager.DefaultDataManager.OpenRaster(fileName, true, inProgressHandler);
            Symbolizer = new RasterSymbolizer(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterLayer"/> class using the progress handler defined on the DefaultLayerManager.
        /// </summary>
        /// <param name="raster">The raster to create this layer for.</param>
        public RasterLayer(IRaster raster)
        {
            DataSet = raster;
            Symbolizer = new RasterSymbolizer(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterLayer"/> class.
        /// </summary>
        /// <param name="raster">The raster to create this layer for.</param>
        /// <param name="inProgressHandler">The Progress handler for any status updates.</param>
        public RasterLayer(IRaster raster, IProgressHandler inProgressHandler)
            : base(inProgressHandler)
        {
            DataSet = raster;
            Symbolizer = new RasterSymbolizer(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum number of cells which can be stored in memory.
        /// By default it is 8000 * 8000.
        /// </summary>
        public static int MaxCellsInMemory { get; set; } = 8000 * 8000;

        /// <summary>
        /// Gets or sets the bitmamp. This is what the raster layer uses to retrieve a bitmap representing the specified
        /// extent. This could later be redesigned to generate the bitmap on the fly, but I think
        /// that that would be slow, so caching is probably better.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IGetBitmap BitmapGetter
        {
            get
            {
                return _bitmapGetter;
            }

            set
            {
                if (value == _bitmapGetter) return;

                _bitmapGetter?.Dispose(); // Dispose previous bitmapGetter to avoid memory leaks
                _bitmapGetter = value;
            }
        }

        /// <summary>
        /// Gets or sets the boundaries of the raster.
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(Forms.PropertyGridEditor), typeof(UITypeEditor))]
        /// [TypeConverter(typeof(Forms.GeneralTypeConverter))].
        /// </remarks>
        [Category("Bounds")]
        [Description("Shows more detail about the geographic position of the raster.")]
        public virtual IRasterBounds Bounds
        {
            get
            {
                return DataSet?.Bounds;
            }

            set
            {
                if (DataSet != null) DataSet.Bounds = value;
                if (BitmapGetter != null) BitmapGetter.Bounds = value;
            }
        }

        /// <summary>
        /// Gets the geographic height of the cells for this raster (North-South).
        /// </summary>
        [Category("Raster Properties")]
        [Description("The geographic width of each cell in this raster.")]
        public virtual double CellHeight => DataSet?.CellHeight ?? 0;

        /// <summary>
        /// Gets the geographic width of the cells for this raster (East-West).
        /// </summary>
        [Category("Raster Properties")]
        [Description("The geographic width of each cell in this raster.")]
        public virtual double CellWidth => DataSet?.CellWidth ?? 0;

        /// <summary>
        /// Gets or sets a value indicating whether this should appear as checked in the legend.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Checked
        {
            get
            {
                return _symbolizer == null || _symbolizer.IsVisible;
            }

            set
            {
                if (_symbolizer == null) return;

                if (value != _symbolizer.IsVisible)
                {
                    IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlying dataset.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(Forms.GeneralTypeConverter))]
        /// [Editor(typeof(Forms.PropertyGridEditor), typeof(UITypeEditor))].
        /// </remarks>
        [Category("Data")]
        [DisplayName(@"Raster Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This gives access to more comprehensive information about the underlying data.")]
        [ShallowCopy]
        [Browsable(false)]
        public new IRaster DataSet
        {
            get
            {
                return base.DataSet as IRaster;
            }

            set
            {
                base.DataSet = value;
            }
        }

        /// <summary>
        /// Gets the data type of the values in this raster.
        /// </summary>
        [Category("Raster Properties")]
        [Description("The numeric data type of the values in this raster.")]
        public Type DataType => DataSet?.DataType;

        /// <summary>
        /// Gets the eastern boundary of this raster.
        /// </summary>
        [Category("Bounds")]
        [Description("The East boundary of this raster.")]
        public virtual double East => DataSet?.Bounds?.Right() ?? 0;

        /// <summary>
        /// Gets or sets the conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.
        /// </summary>
        [DisplayName(@"Elevation Factor")]
        [Category("Symbology")]
        [Description("This is a conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.")]
        public virtual float ElevationFactor
        {
            get
            {
                return _symbolizer?.ElevationFactor ?? 0f;
            }

            set
            {
                if (_symbolizer == null) return;

                _symbolizer.ElevationFactor = value;
            }
        }

        /// <summary>
        /// Gets the datasets extent.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Extent Extent => DataSet?.Extent;

        /// <summary>
        /// Gets or sets the exaggeration beyond normal elevation values. A value of 1 is normal elevation,
        /// a value of 0 would be flat, while a value of 2 would be twice the normal elevation.
        /// This applies to the three-dimensional rendering and is not related to the shaded relief pattern
        /// created by the texture.
        /// </summary>
        [DisplayName(@"Extrusion")]
        [Category("Symbology")]
        [Description("the exaggeration beyond normal elevation values. A value of 1 is normal elevation, a value of 0 would be flat, while a value of 2 would be twice the normal elevation. This applies to the three-dimensional rendering and is not related to the shaded relief pattern created by the texture.")]
        public virtual float Extrusion
        {
            get
            {
                if (_symbolizer != null) return _symbolizer.Extrusion;

                return 0f;
            }

            set
            {
                if (_symbolizer == null) return;

                _symbolizer.Extrusion = value;
            }
        }

        /// <summary>
        /// Gets the file name where this raster is saved.
        /// </summary>
        [Category("Raster Properties")]
        [Description("The file name of this raster.")]
        public string Filename => DataSet != null ? DataSet.Filename : "No Raster Specified";

        /// <summary>
        /// Gets the relative file path to where this raster is saved.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Serialize("FilePath", ConstructorArgumentIndex = 0)]
        public string FilePath => DataSet?.FilePath;

        /// <summary>
        /// Gets or sets a value indicating whether the layer is visible. If this is false, then the drawing function will not render anything.
        /// Warning! This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        [Category("Symbology")]
        [DisplayName(@"Visible")]
        [Description("Controls whether or not this layer will be drawn.")]
        public override bool IsVisible
        {
            get
            {
                return _symbolizer != null && _symbolizer.IsVisible;
            }

            set
            {
                if (_symbolizer == null) return;

                _symbolizer.IsVisible = value;
                OnVisibleChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the complete list of legend items contained within this legend item.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnumerable<ILegendItem> LegendItems => _symbolizer?.Scheme.Categories;

        /// <summary>
        /// Gets or sets the text that will appear in the legend.
        /// </summary>
        [Category("Appearance")]
        [DisplayName(@"Caption")]
        [Description(" The text that will appear in the legend")]
        public override string LegendText
        {
            get
            {
                if (base.LegendText == null && DataSet != null) base.LegendText = DataSet.Name;
                return base.LegendText;
            }

            set
            {
                base.LegendText = value;
            }
        }

        /// <summary>
        /// Gets the maximum value of this raster. If this is an elevation raster, this is also the top.
        /// </summary>
        [Category("Raster Properties")]
        [Description("The maximum value of this raster. If this is an elevation raster, this is also the top.")]
        public virtual double Maximum => DataSet?.Maximum ?? 0;

        /// <summary>
        /// Gets the minimum value of this raster. If this is an elevation raster, this is also the bottom.
        /// </summary>
        [Category("Raster Properties")]
        [Description("The minimum value of this raster. If this is an elevation raster, this is also the bottom.")]
        public virtual double Minimum => DataSet?.Minimum ?? 0;

        /// <summary>
        /// Gets the value that is used when no actual data exists for the specified location.
        /// </summary>
        [Category("Raster Properties")]
        [Description("The value that is used when no actual data exists for the specified location.")]
        public virtual double NoDataValue => DataSet?.NoDataValue ?? 0;

        /// <summary>
        /// Gets the northern boundary of this raster.
        /// </summary>
        [Category("Bounds")]
        [Description("The North boundary of this raster.")]
        public virtual double North => DataSet?.Bounds?.Top() ?? 0;

        /// <summary>
        /// Gets the number of bands in this raster.
        /// </summary>
        [DisplayName(@"Number of Bands")]
        [Category("Raster Properties")]
        [Description("Gets the number of bands in this raster.")]
        public virtual int NumBands => DataSet?.NumBands ?? 0;

        /// <summary>
        /// Gets the number of columns in this raster.
        /// </summary>
        [DisplayName(@"Number of Columns")]
        [Category("Raster Properties")]
        [Description("Gets the number of columns in this raster.")]
        public virtual int NumColumns => DataSet?.NumColumns ?? 0;

        /// <summary>
        /// Gets the number of rows in this raster.
        /// </summary>
        [DisplayName(@"Number of Rows")]
        [Category("Raster Properties")]
        [Description("Gets the number of rows in this raster.")]
        public virtual int NumRows => DataSet?.NumRows ?? 0;

        /// <summary>
        /// Gets or sets custom actions for RasterLayer.
        /// </summary>
        [Browsable(false)]
        public IRasterLayerActions RasterLayerActions { get; set; }

        /// <summary>
        /// Gets the southern boundary of this raster.
        /// </summary>
        [Category("Bounds")]
        [Description("The South boundary of this raster.")]
        public virtual double South => DataSet?.Bounds?.Bottom() ?? 0;

        /// <summary>
        /// Gets or sets the collection of symbolzier properties to use for this raster.
        /// [Editor(typeof(Forms.RasterColorSchemeEditor), typeof(UITypeEditor))]
        /// [TypeConverter(typeof(Forms.GeneralTypeConverter))].
        /// </summary>
        [Category("Symbology")]
        [DisplayName(@"Color Scheme")]
        [Browsable(false)]
        [ShallowCopy]
        public IRasterSymbolizer Symbolizer
        {
            get
            {
                return _symbolizer;
            }

            set
            {
                if (_symbolizer == value) return;

                if (_symbolizer != null) _symbolizer.ColorSchemeUpdated -= SymbolizerSymbologyUpdated;
                _symbolizer = value;
                if (_symbolizer == null) return;

                _symbolizer.ParentLayer = this;
                _symbolizer.Scheme.SetParentItem(this);
                _symbolizer.ColorSchemeUpdated += SymbolizerSymbologyUpdated;
            }
        }

        /// <summary>
        /// Gets the western boundary of this raster.
        /// </summary>
        [Category("Bounds")]
        [Description("The West boundary of this raster.")]
        public virtual double West => DataSet?.Bounds?.Left() ?? 0;

        #endregion

        #region Methods

        /// <summary>
        /// Render the full raster block by block, and then save the values to the pyramid raster.
        /// This will probably be nasty and time consuming, but what can you do.
        /// </summary>
        /// <param name="pyrFile">File name of the file the pyramid image should be saved to.</param>
        /// <param name="progressHandler">The progress handler.</param>
        /// <returns>The created pyramid image.</returns>
        public IImageData CreatePyramidImage(string pyrFile, IProgressHandler progressHandler)
        {
            PyramidImage py = new PyramidImage(pyrFile, DataSet.Bounds);
            int width = DataSet.Bounds.NumColumns;
            int blockHeight = 32000000 / width;
            if (blockHeight > DataSet.Bounds.NumRows) blockHeight = DataSet.Bounds.NumRows;
            int numBlocks = (int)Math.Ceiling(DataSet.Bounds.NumRows / (double)blockHeight);
            int count = DataSet.NumRows;
            if (_symbolizer.ShadedRelief.IsUsed)
            {
                count = count * 2;
            }

            ProgressMeter pm = new ProgressMeter(progressHandler, "Creating Pyramids", count);
            PerformanceCounter pcRemaining = new PerformanceCounter("Memory", "Available Bytes");
            Process proc = Process.GetCurrentProcess();

            for (int j = 0; j < numBlocks; j++)
            {
                int h = blockHeight;
                if (j == numBlocks - 1)
                {
                    h = DataSet.Bounds.NumRows - (j * blockHeight);
                }
#if DEBUG
                var mem = proc.PrivateMemorySize64 / 1000000;
                var freeRam = Convert.ToInt64(pcRemaining.NextValue()) / 1000000;
                Debug.WriteLine("Memory before: " + mem + ", " + freeRam + " remaining.");
#endif
                pm.BaseMessage = "Reading from Raster";
                pm.SendProgress();
                using (IRaster r = DataSet.ReadBlock(0, j * blockHeight, width, h))
                {
                    byte[] vals = new byte[h * 4 * width];
                    r.DrawToBitmap(Symbolizer, vals, width * 4, pm);
                    pm.BaseMessage = "Writing to Pyramids";
                    pm.SendProgress();
                    py.WriteWindow(vals, j * blockHeight, 0, h, width, 0);
                    Symbolizer.HillShade = null;
                }
#if DEBUG
                mem = proc.PrivateMemorySize64 / 1000000;
                freeRam = Convert.ToInt64(pcRemaining.NextValue()) / 1000000;
                Debug.WriteLine("Memory after: " + mem + "Mb | " + freeRam + " remaining Mb.");
#endif
            }

            pm.Reset();
            py.ProgressHandler = ProgressHandler;
            py.CreatePyramids();
            py.WriteHeader(pyrFile);
            return py;
        }

        /// <summary>
        ///  Creates a bmp texture and saves it to the specified fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to write to.</param>
        /// <param name="bandType">The color band type.</param>
        public void ExportBitmap(string fileName, ImageBandType bandType)
        {
            ExportBitmap(fileName, DataSet.ProgressHandler, bandType);
        }

        /// <summary>
        /// Creates a new filename and saves the content from the current BitmapGetter to the
        /// file format. This relies on the DataManager and will only be successful for
        /// formats supported by the write format possibility. This will not update this raster.
        /// </summary>
        /// <param name="fileName">The string fileName to write to.</param>
        /// <param name="progressHandler">The progress handler for creating a new bitmap.</param>
        /// <param name="bandType">The band type ot use.</param>
        public void ExportBitmap(string fileName, IProgressHandler progressHandler, ImageBandType bandType)
        {
            int rows = DataSet.NumRowsInFile;
            int cols = DataSet.NumColumnsInFile;

            IImageData result = DataManager.DefaultDataManager.CreateImage(fileName, rows, cols, false, progressHandler, bandType);
            int numBlocks = 1;
            const int MaxRc = 8000 * 8000;
            if (rows * cols > MaxRc)
            {
                numBlocks = Convert.ToInt32(Math.Ceiling(MaxRc / (double)cols));
            }

            int blockRows = MaxRc / cols;
            ProjectionHelper ph = new ProjectionHelper(DataSet.Extent, new Rectangle(0, 0, cols, rows));
            for (int iblock = 0; iblock < numBlocks; iblock++)
            {
                int rowCount = blockRows;
                if (iblock == numBlocks - 1) rowCount = rows - (blockRows * iblock);
                Rectangle r = new Rectangle(0, iblock * blockRows, cols, rowCount);
                Bitmap block = BitmapGetter.GetBitmap(ph.PixelToProj(r), r);
                result.WriteBlock(block, 0, iblock * blockRows);
            }
        }

        /// <summary>
        /// Reprojects the dataset for this layer.
        /// </summary>
        /// <param name="targetProjection">The target projection to use.</param>
        public override void Reproject(ProjectionInfo targetProjection)
        {
            if (DataSet != null)
            {
                DataSet.Reproject(targetProjection);
                if (BitmapGetter != null)
                {
                    double[] aff = new double[6];
                    Array.Copy(DataSet.Bounds.AffineCoefficients, aff, 6);
                    BitmapGetter.Bounds.AffineCoefficients = aff;
                }
            }
        }

        /// <summary>
        /// This only updates the bitmap representation of the raster. It does not write to a file unless
        /// the file is too large to fit in memory, in which case it will update the pyramid image.
        /// </summary>
        public void WriteBitmap()
        {
            WriteBitmap(ProgressHandler);
        }

        /// <summary>
        /// This only updates the bitmap representation of this raster. This can be overridden, but currently
        /// uses the default implementation.
        /// </summary>
        /// <param name="progressHandler">An implementation of IProgressHandler to receive status messages.</param>
        public virtual void WriteBitmap(IProgressHandler progressHandler)
        {
            DefaultWriteBitmap(progressHandler);
        }

        /// <summary>
        /// This does not have to be used to work, but provides a default implementation for writing bitmap,
        /// and will be used by the MapRasterLayer class during file creation.
        /// </summary>
        /// <param name="progressHandler">The progress handler.</param>
        protected void DefaultWriteBitmap(IProgressHandler progressHandler)
        {
            if ((long)DataSet.NumRowsInFile * DataSet.NumColumnsInFile > MaxCellsInMemory)
            {
                // For huge images, assume that GDAL or something was needed anyway,
                // and we would rather avoid having to re-create the pyramids if there is any chance
                // that the old values will work ok.
                string pyrFile = Path.ChangeExtension(DataSet.Filename, ".mwi");

                BitmapGetter = CreatePyramidImage(pyrFile, progressHandler);
                OnItemChanged(this);
                return;
            }

            Bitmap bmp = new Bitmap(DataSet.NumColumns, DataSet.NumRows, PixelFormat.Format32bppArgb);

            if (_symbolizer.DrapeVectorLayers == false)
            {
                // Generate the colorscheme, modified by hillshading if that hillshading is used all in one pass
                DataSet.DrawToBitmap(Symbolizer, bmp, progressHandler);
            }
            else
            {
                // work backwards. when we get to this layer do the colorscheme.
                // First, use this raster and its colorscheme to drop the background
                DataSet.PaintColorSchemeToBitmap(Symbolizer, bmp, progressHandler);

                // Set up a graphics object with a transformation pre-set so drawing a geographic coordinate
                // will draw to the correct location on the bitmap
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Extent extents = DataSet.Extent;
                Rectangle target = new Rectangle(0, 0, bmp.Width, bmp.Height);
                ImageProjection ip = new ImageProjection(extents, target);

                // Cycle through each layer, and as long as it is not this layer, draw the bmp
                foreach (ILegendItem layer in GetParentItem().LegendItems)
                {
                    // Temporarily I am only interested in doing this for vector datasets
                    IFeatureLayer fl = layer as IFeatureLayer;
                    fl?.DrawSnapShot(g, ip);
                }

                if (Symbolizer.ShadedRelief.IsUsed)
                {
                    // After we have drawn the underlying texture, apply a hillshade if it is requested
                    Symbolizer.PaintShadingToBitmap(bmp, progressHandler);
                }
            }

            InRamImage image = new InRamImage(bmp)
            {
                Bounds = DataSet.Bounds.Copy()
            };
            BitmapGetter = image;
            Symbolizer.Validate();
            OnInvalidate(this, EventArgs.Empty);
            OnItemChanged();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BitmapGetter = null;
                RasterLayerActions = null;
                Symbolizer = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Handles the situation for exporting the layer as a new source.
        /// </summary>
        protected override void OnExportData()
        {
            var rla = RasterLayerActions;
            rla?.ExportData(DataSet);
        }

        /// <summary>
        /// Occurs when this member should raise the shared event to show the property dialog for this raster layer.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnShowProperties(HandledEventArgs e)
        {
            var rla = RasterLayerActions;
            rla?.ShowProperties(this);
            e.Handled = true;
        }

        private void SymbolizerSymbologyUpdated(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion

        #region Classes

        private class ProjectionHelper : IProj
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ProjectionHelper"/> class.
            /// </summary>
            /// <param name="geographicExtents">The geographic extents to project to and from.</param>
            /// <param name="viewRectangle">The view rectangle in pixels to transform with.</param>
            public ProjectionHelper(Extent geographicExtents, Rectangle viewRectangle)
            {
                GeographicExtents = geographicExtents;
                ImageRectangle = viewRectangle;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the geographic extent to use.
            /// </summary>
            public Extent GeographicExtents { get; }

            /// <summary>
            /// Gets the rectangular pixel region to use.
            /// </summary>
            public Rectangle ImageRectangle { get; }

            #endregion
        }

        #endregion
    }
}
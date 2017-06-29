// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2008 3:42:21 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        #region Private Variables

        [Serialize("Symbolizer", ConstructorArgumentIndex = 1)]
        private IRasterSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Opens the specified fileName using the layer manager.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="symbolizer"></param>
        public RasterLayer(string fileName, IRasterSymbolizer symbolizer)
        {
            IRaster r = DataManager.DefaultDataManager.OpenRaster(fileName);
            DataSet = r;
            _symbolizer = symbolizer;
            _symbolizer.ParentLayer = this;
            _symbolizer.Scheme.SetParentItem(this);

            symbolizer.ColorSchemeUpdated += _symbolizer_SymbologyUpdated;
        }

        /// <summary>
        /// Opens the specified fileName and automatically creates a raster that can be used by this raster layer.
        /// </summary>
        /// <param name="fileName">The string fileName to use in order to open the file</param>
        /// <param name="inProgressHandler">The progress handler to show progress messages</param>
        public RasterLayer(string fileName, IProgressHandler inProgressHandler)
        {
            base.ProgressHandler = inProgressHandler;
            IRaster r = DataManager.DefaultDataManager.OpenRaster(fileName, true, inProgressHandler);
            Configure(r);
        }

        /// <summary>
        /// Creates a new raster layer using the progress handler defined on the DefaultLayerManager
        /// </summary>
        /// <param name="raster">The raster to create this layer for</param>
        public RasterLayer(IRaster raster)
        {
            Configure(raster);
        }

        /// <summary>
        /// Creates a new instance of RasterLayer
        /// </summary>
        /// <param name="raster">The Raster</param>
        /// <param name="inProgressHandler">The Progress handler for any status updates</param>
        public RasterLayer(IRaster raster, IProgressHandler inProgressHandler)
        {
            base.ProgressHandler = inProgressHandler;
            Configure(raster);
        }

        private void Configure(IRaster raster)
        {
            DataSet = raster;
            _symbolizer = new RasterSymbolizer(this);
            _symbolizer.ColorSchemeUpdated += _symbolizer_SymbologyUpdated;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This only updates the bitmap representation of the raster.  It does not write to a file unless
        /// the file is too large to fit in memory, in which case it will update the pyramid image.
        /// </summary>
        public void WriteBitmap()
        {
            WriteBitmap(ProgressHandler);
        }

        /// <summary>
        /// This only updates the bitmap representation of this raster.  This can be overridden, but currently
        /// uses the default implementation.
        /// </summary>
        /// <param name="progressHandler">An implementation of IProgressHandler to receive status messages</param>
        public virtual void WriteBitmap(IProgressHandler progressHandler)
        {
            DefaultWriteBitmap(progressHandler);
        }

        /// <summary>
        ///  Creates a bmp texture and saves it to the specified fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to write to</param>
        /// <param name="bandType">The color band type.</param>
        public void ExportBitmap(string fileName, ImageBandType bandType)
        {
            ExportBitmap(fileName, DataSet.ProgressHandler, bandType);
        }

        /// <summary>
        /// Creates a new filename and saves the content from the current BitmapGetter to the
        /// file format.  This relies on the DataManager and will only be successful for
        /// formats supported by the write format possibility.  This will not update this raster
        /// </summary>
        /// <param name="fileName">The string fileName to write to</param>
        /// <param name="progressHandler">The progress handler for creating a new bitmap.</param>
        /// <param name="bandType">The band type ot use.</param>
        public void ExportBitmap(string fileName, IProgressHandler progressHandler, ImageBandType bandType)
        {
            int rows = DataSet.NumRowsInFile;
            int cols = DataSet.NumColumnsInFile;

            IImageData result = DataManager.DefaultDataManager.CreateImage(fileName, rows, cols, false, progressHandler, bandType);
            int numBlocks = 1;
            if (rows * cols > 8000 * 8000)
            {
                numBlocks = Convert.ToInt32(Math.Ceiling(8000 * 8000 / (double)cols));
            }
            int blockRows = (8000 * 8000) / cols;
            ProjectionHelper ph = new ProjectionHelper(DataSet.Extent, new Rectangle(0, 0, cols, rows));
            for (int iblock = 0; iblock < numBlocks; iblock++)
            {
                int rowCount = blockRows;
                if (iblock == numBlocks - 1) rowCount = rows - blockRows * iblock;
                Rectangle r = new Rectangle(0, iblock * blockRows, cols, rowCount);
                Bitmap block = BitmapGetter.GetBitmap(ph.PixelToProj(r), r);
                result.WriteBlock(block, 0, iblock * blockRows);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // Grid is

            if (disposing)
            {
                _symbolizer = null;
            }
        }

        /// <summary>
        /// Render the full raster block by block, and then save the values to the pyramid raster.
        /// This will probably be nasty and time consuming, but what can you do.
        /// </summary>
        /// <param name="pyrFile"></param>
        /// <param name="progressHandler"></param>
        /// <returns></returns>
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
            long mem;
            long freeRAM;

            for (int j = 0; j < numBlocks; j++)
            {
                int h = blockHeight;
                if (j == numBlocks - 1)
                {
                    h = DataSet.Bounds.NumRows - j * blockHeight;
                }

                mem = proc.PrivateMemorySize64 / 1000000;
                freeRAM = Convert.ToInt64(pcRemaining.NextValue()) / 1000000;
                Debug.WriteLine("Memory before: " + mem + ", " + freeRAM + " remaining.");
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
                mem = proc.PrivateMemorySize64 / 1000000;
                freeRAM = Convert.ToInt64(pcRemaining.NextValue()) / 1000000;
                Debug.WriteLine("Memory after: " + mem + "Mb | " + freeRAM + " remaining Mb.");
            }
            pm.Reset();
            py.ProgressHandler = ProgressHandler;
            py.CreatePyramids();
            py.WriteHeader(pyrFile);
            return py;
        }

        /// <summary>
        /// This does not have to be used to work, but provides a default implementation for writing bitmap,
        /// and will be used by the MapRasterLayer class during file creation.
        /// </summary>
        /// <param name="progressHandler"></param>
        protected void DefaultWriteBitmap(IProgressHandler progressHandler)
        {
            if (DataSet.NumRowsInFile * DataSet.NumColumnsInFile > 8000 * 8000)
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
                // work backwards.  when we get to this layer do the colorscheme.
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
                    if (fl == null) continue;
                    fl.DrawSnapShot(g, ip);
                }
                if (Symbolizer.ShadedRelief.IsUsed)
                {
                    // After we have drawn the underlying texture, apply a hillshade if it is requested
                    Symbolizer.PaintShadingToBitmap(bmp, progressHandler);
                }
            }
            InRamImage image = new InRamImage(bmp);
            image.Bounds = DataSet.Bounds.Copy();
            BitmapGetter = image;
            Symbolizer.Validate();
            OnInvalidate(this, new EventArgs());
            OnItemChanged();
        }

        /// <summary>
        /// Handles the situation for exporting the layer as a new source.
        /// </summary>
        protected override void OnExportData()
        {
            var rla = RasterLayerActions;
            if (rla != null)
            {
                rla.ExportData(DataSet);
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets custom actions for RasterLayer
        /// </summary>
        [Browsable(false)]
        public IRasterLayerActions RasterLayerActions { get; set; }

        /// <summary>
        /// Gets or sets the boundaries of the raster.
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(Forms.PropertyGridEditor), typeof(UITypeEditor))]
        /// [TypeConverter(typeof(Forms.GeneralTypeConverter))]
        /// </remarks>
        [Category("Bounds")]
        [Description("Shows more detail about the geographic position of the raster.")]
        public virtual IRasterBounds Bounds
        {
            get
            {
                if (DataSet != null) return DataSet.Bounds;
                return null;
            }
            set
            {
                if (DataSet != null)
                {
                    DataSet.Bounds = value;
                }
                if (BitmapGetter != null)
                {
                    BitmapGetter.Bounds = value;
                }
            }
        }

        /// <summary>
        /// This is what the raster layer uses to retrieve a bitmap representing the specified
        /// extent.  This could later be redesigned to generate the bitmap on the fly, but I think
        /// that that would be slow, so caching is probably better.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IGetBitmap BitmapGetter { get; set; }

        /// <summary>
        /// Gets the geographic height of the cells for this raster (North-South)
        /// </summary>
        [Category("Raster Properties"), Description("The geographic width of each cell in this raster.")]
        public virtual double CellHeight
        {
            get
            {
                if (DataSet != null) return DataSet.CellHeight;
                return 0;
            }
        }

        /// <summary>
        /// Gets the geographic width of the cells for this raster (East-West)
        /// </summary>
        [Category("Raster Properties"), Description("The geographic width of each cell in this raster.")]
        public virtual double CellWidth
        {
            get
            {
                if (DataSet != null) return DataSet.CellWidth;
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets whether this should appear as checked in the legend.  This is also how the
        /// layer will
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Checked
        {
            get
            {
                if (_symbolizer == null) return true;
                return _symbolizer.IsVisible;
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
        /// Gets the data type of the values in this raster.
        /// </summary>
        [Category("Raster Properties"), Description("The numeric data type of the values in this raster.")]
        public Type DataType
        {
            get
            {
                if (DataSet != null) return DataSet.DataType;
                return null;
            }
        }

        /// <summary>
        /// Gets the eastern boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The East boundary of this raster.")]
        public virtual double East
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Right();
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// This is a conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.
        /// </summary>
        [DisplayName(@"Elevation Factor"), Category("Symbology"), Description("This is a conversion factor that is required in order to convert the elevation units into the same units as the geospatial projection for the latitude and logitude values of the grid.")]
        public virtual float ElevationFactor
        {
            get
            {
                if (_symbolizer != null) return _symbolizer.ElevationFactor;
                return 0f;
            }
            set
            {
                if (_symbolizer == null) return;
                _symbolizer.ElevationFactor = value;
            }
        }

        /// <summary>
        /// Obtains an envelope
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Extent Extent
        {
            get
            {
                if (DataSet != null)
                {
                    return DataSet.Extent;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the exaggeration beyond normal elevation values.  A value of 1 is normal elevation,
        ///  a value of 0 would be flat,  while a value of 2 would be twice the normal elevation.
        /// This applies to the three-dimensional rendering and is not related to the shaded relief pattern
        /// created by the texture.
        /// </summary>
        [DisplayName(@"Extrusion")]
        [Category("Symbology")]
        [Description("the exaggeration beyond normal elevation values.  A value of 1 is normal elevation, a value of 0 would be flat, while a value of 2 would be twice the normal elevation.  This applies to the three-dimensional rendering and is not related to the shaded relief pattern created by the texture.")]
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
        /// Gets the fileName where this raster is saved.
        /// </summary>
        [Category("Raster Properties")]
        [Description("The fileName of this raster.")]
        [Serialize("Filename", ConstructorArgumentIndex = 0)]
        public virtual string Filename
        {
            get
            {
                if (DataSet != null) return DataSet.Filename;
                return "No Raster Specified";
            }
        }

        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
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
                if (_symbolizer != null) return _symbolizer.IsVisible;
                return false;
            }
            set
            {
                _symbolizer.IsVisible = value;
                OnVisibleChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets the complete list of legend items contained within this legend item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                if (_symbolizer == null) return null;
                return _symbolizer.Scheme.Categories.Cast<ILegendItem>();
            }
        }

        /// <summary>
        /// The text that will appear in the legend
        /// </summary>
        [Category("Appearance"), DisplayName(@"Caption"), Description(" The text that will appear in the legend")]
        public override string LegendText
        {
            get
            {
                if (base.LegendText == null && DataSet != null)
                {
                    base.LegendText = DataSet.Name;
                }
                return base.LegendText;
            }
            set { base.LegendText = value; }
        }

        /// <summary>
        /// Gets the maximum value of this raster.  If this is an elevation raster, this is also the top.
        /// </summary>
        [Category("Raster Properties"),
         Description("The maximum value of this raster.  If this is an elevation raster, this is also the top.")]
        public virtual double Maximum
        {
            get
            {
                if (DataSet != null) return DataSet.Maximum;
                return 0;
            }
        }

        /// <summary>
        /// Gets the minimum value of this raster.  If this is an elevation raster, this is also the bottom.
        /// </summary>
        [Category("Raster Properties"),
         Description("The minimum value of this raster.  If this is an elevation raster, this is also the bottom.")]
        public virtual double Minimum
        {
            get
            {
                if (DataSet != null) return DataSet.Minimum;
                return 0;
            }
        }

        /// <summary>
        /// Gets the value that is used when no actual data exists for the specified location.
        /// </summary>
        [Category("Raster Properties"),
         Description("The value that is used when no actual data exists for the specified location.")]
        public virtual double NoDataValue
        {
            get
            {
                if (DataSet != null) return DataSet.NoDataValue;
                return 0;
            }
        }

        /// <summary>
        /// Gets the northern boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The North boundary of this raster.")]
        public virtual double North
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Top();
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of bands in this raster.
        /// </summary>
        [DisplayName(@"Number of Bands"), Category("Raster Properties"),
         Description("Gets the number of bands in this raster.")]
        public virtual int NumBands
        {
            get
            {
                if (DataSet != null) return DataSet.NumBands;
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of columns in this raster.
        /// </summary>
        [DisplayName(@"Number of Columns"), Category("Raster Properties"),
         Description("Gets the number of columns in this raster.")]
        public virtual int NumColumns
        {
            get
            {
                if (DataSet != null) return DataSet.NumColumns;
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of rows in this raster.
        /// </summary>
        [DisplayName(@"Number of Rows"), Category("Raster Properties"),
         Description("Gets the number of rows in this raster.")]
        public virtual int NumRows
        {
            get
            {
                if (DataSet != null) return DataSet.NumRows;
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the underlying dataset
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(Forms.GeneralTypeConverter))]
        /// [Editor(typeof(Forms.PropertyGridEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Category("Data")]
        [DisplayName(@"Raster Properties")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This gives access to more comprehensive information about the underlying data.")]
        [ShallowCopy]
        public new IRaster DataSet
        {
            get { return base.DataSet as IRaster; }
            set { base.DataSet = value; }
        }

        /// <summary>
        /// Gets or sets the collection of symbolzier properties to use for this raster.
        /// [Editor(typeof(Forms.RasterColorSchemeEditor), typeof(UITypeEditor))]
        /// [TypeConverter(typeof(Forms.GeneralTypeConverter))]
        /// </summary>
        [Category("Symbology")]
        [DisplayName(@"Color Scheme")]
        [ShallowCopy]
        public IRasterSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set
            {
                if (_symbolizer == value) return;
                _symbolizer = value;
                _symbolizer.ParentLayer = this;
                _symbolizer.Scheme.SetParentItem(this);
                _symbolizer.ColorSchemeUpdated += _symbolizer_SymbologyUpdated;
            }
        }

        /// <summary>
        /// Gets the southern boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The South boundary of this raster.")]
        public virtual double South
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Bottom();
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the western boundary of this raster.
        /// </summary>
        [Category("Bounds"), Description("The West boundary of this raster.")]
        public virtual double West
        {
            get
            {
                if (DataSet != null)
                {
                    if (DataSet.Bounds != null)
                    {
                        return DataSet.Bounds.Left();
                    }
                }
                return 0;
            }
        }

        #endregion

        /// <summary>
        /// Occurs when this member should raise the shared event to show the property dialog for this raster layer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShowProperties(HandledEventArgs e)
        {
            var rla = RasterLayerActions;
            if (rla != null)
            {
                rla.ShowProperties(this);
            }
            e.Handled = true;
        }

        #region Event Handlers

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

        private void _symbolizer_SymbologyUpdated(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion
    }
}
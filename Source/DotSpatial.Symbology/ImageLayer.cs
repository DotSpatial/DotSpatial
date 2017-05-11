// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 2:46:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for images.
    /// </summary>
    public class ImageLayer : Layer, IImageLayer
    {
        #region Fields

        private IImageSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer"/> class that needs to be configured later.
        /// </summary>
        public ImageLayer()
        {
            Symbolizer = new ImageSymbolizer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer"/> class by opening the specified fileName.
        /// </summary>
        /// <param name="fileName">The fileName to open</param>
        public ImageLayer(string fileName)
        {
            Symbolizer = new ImageSymbolizer();
            DataSet = DataManager.DefaultDataManager.OpenImage(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer"/> class by opening the specified fileName, relaying progress to the
        /// specified handler, and automatically adds the new layer to the specified container.
        /// </summary>
        /// <param name="fileName">The fileName to open</param>
        /// <param name="progressHandler">A ProgressHandler that can receive progress updates</param>
        /// <param name="container">The layer list that should contain this image layer</param>
        public ImageLayer(string fileName, IProgressHandler progressHandler, ICollection<ILayer> container)
            : base(container)
        {
            Symbolizer = new ImageSymbolizer();
            DataSet = DataManager.DefaultDataManager.OpenImage(fileName, progressHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer"/> class by opening the specified fileName and
        /// relaying progress to the specified handler.
        /// </summary>
        /// <param name="fileName">The fileName to open</param>
        /// <param name="progressHandler">The progressHandler</param>
        public ImageLayer(string fileName, IProgressHandler progressHandler)
        {
            Symbolizer = new ImageSymbolizer();
            DataSet = DataManager.DefaultDataManager.OpenImage(fileName, progressHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer"/> class.
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        public ImageLayer(IImageData baseImage)
        {
            Symbolizer = new ImageSymbolizer();
            DataSet = baseImage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer"/> class.
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="container">The Layers collection that keeps track of the image layer</param>
        public ImageLayer(IImageData baseImage, ICollection<ILayer> container)
            : base(container)
        {
            Symbolizer = new ImageSymbolizer();
            DataSet = baseImage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the underlying data for this object.
        /// </summary>
        [Serialize("ImageData")]
        public new IImageData DataSet
        {
            get
            {
                return base.DataSet as IImageData;
            }

            set
            {
                var current = DataSet;
                if (current == value) return;

                base.DataSet = value;
                OnDataSetChanged(value);
            }
        }

        /// <summary>
        /// Gets the geographic bounding envelope for the image.
        /// </summary>
        public override Extent Extent => DataSet?.Extent;

        /// <summary>
        /// Gets or sets the image being drawn by this layer
        /// </summary>
        public IImageData Image
        {
            get
            {
                return DataSet;
            }

            set
            {
                DataSet = value;
            }
        }

        /// <summary>
        /// Gets or sets custom actions for ImageLayer
        /// </summary>
        [Browsable(false)]
        public IImageLayerActions ImageLayerActions { get; set; }

        /// <summary>
        /// Gets or sets a class that has some basic parameters that control how the image layer
        /// is drawn.
        /// </summary>
        [Browsable(false)]
        [ShallowCopy]
        [Serialize("Symbolizer")]
        public IImageSymbolizer Symbolizer
        {
            get
            {
                return _symbolizer;
            }

            set
            {
                _symbolizer = value;
                OnItemChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose memory objects.
        /// </summary>
        /// <param name="disposeManagedResources">True if managed memory objects should be set to null.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                _symbolizer = null;
                ImageLayerActions = null;
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// This updates the things that depend on the DataSet so that they fit to the changed DataSet.
        /// </summary>
        /// <param name="value">DataSet that was changed.</param>
        protected virtual void OnDataSetChanged(IImageData value)
        {
            IsVisible = value != null;

            // Change legendText only if image data refers to real file
            if (value != null && File.Exists(value.Filename))
            {
                LegendText = Path.GetFileName(value.Filename);
            }
        }

        /// <summary>
        /// Handles export data from this layer.
        /// </summary>
        protected override void OnExportData()
        {
            ImageLayerActions?.ExportData(Image);
        }

        /// <summary>
        /// Handles when this layer should show its properties by firing the event on the shared event sender
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnShowProperties(HandledEventArgs e)
        {
            ImageLayerActions?.ShowProperties(this);
        }

        #endregion
    }
}
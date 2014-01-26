// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
    /// GeoImageLayer
    /// </summary>
    public class ImageLayer : Layer, IImageLayer
    {
        #region Private Variables

        private IImageSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a blank image layer that needs to be configured later.
        /// </summary>
        public ImageLayer()
        {
        }

        /// <summary>
        /// Creates a new instance of the ImageLayer by opening the specified fileName
        /// </summary>
        /// <param name="fileName"></param>
        public ImageLayer(string fileName)
        {
            DataSet = DataManager.DefaultDataManager.OpenImage(fileName);
        }

        /// <summary>
        /// Creates a new instance of the ImageLayer by opening the specified fileName, relaying progress to the
        /// specified handler, and automatically adds the new layer to the specified container.
        /// </summary>
        /// <param name="fileName">The fileName to open</param>
        /// <param name="progressHandler">A ProgressHandler that can receive progress updates</param>
        /// <param name="container">The layer list that should contain this image layer</param>
        public ImageLayer(string fileName, IProgressHandler progressHandler, ICollection<ILayer> container)
            : base(container)
        {
            DataSet = DataManager.DefaultDataManager.OpenImage(fileName, progressHandler);
        }

        /// <summary>
        /// Creates a new instance of the image layer by opening the specified fileName and
        /// relaying progress to the specified handler.
        /// </summary>
        /// <param name="fileName">The fileName to open</param>
        /// <param name="progressHandler">The progressHandler</param>
        public ImageLayer(string fileName, IProgressHandler progressHandler)
        {
            DataSet = DataManager.DefaultDataManager.OpenImage(fileName, progressHandler);
        }

        /// <summary>
        /// Creates a new instance of GeoImageLayer
        /// </summary>
        public ImageLayer(IImageData baseImage)
        {
            DataSet = baseImage;
        }

        /// <summary>
        /// Creates a new instance of a GeoImageLayer
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="container">The Layers collection that keeps track of the image layer</param>
        public ImageLayer(IImageData baseImage, ICollection<ILayer> container)
            : base(container)
        {
            DataSet = baseImage;
        }
    

        #endregion

        #region IImageLayer Members

        /// <summary>
        /// Gets or sets the underlying data for this object
        /// </summary>
        [Serialize("ImageData")]
        public new IImageData DataSet
        {
            get { return base.DataSet as IImageData; }
            set
            {
                var current = DataSet;
                if (current == value) return;
                base.DataSet = value;
                OnDataSetChanged(value);
            }
        }

        protected virtual void OnDataSetChanged(IImageData value)
        {
            IsVisible = value != null;
            LegendText = value == null ? null : Path.GetFileName(value.Filename);
        }

        /// <summary>
        /// Gets the geographic bounding envelope for the image
        /// </summary>
        public override Extent Extent
        {
            get
            {
                if (DataSet == null) return null;
                return DataSet.Extent;
            }
        }

        /// <summary>
        /// Gets or sets a class that has some basic parameters that control how the image layer
        /// is drawn.
        /// </summary>
        public IImageSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set { _symbolizer = value; }
        }

        /// <summary>
        /// Gets or sets the image being drawn by this layer
        /// </summary>
        public IImageData Image
        {
            get { return DataSet; }
            set { DataSet = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets custom actions for ImageLayer
        /// </summary>
        [Browsable(false)]
        public IImageLayerActions ImageLayerActions { get; set; }

        /// <summary>
        /// Handles when this layer should show its properties by firing the event on the shared event sender
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShowProperties(HandledEventArgs e)
        {
            var ila = ImageLayerActions;
            if (ila != null)
            {
                ila.ShowProperties(this);
            }
        }
        /// <summary>
        /// Handles export data from this layer.
        /// </summary>
        protected override void OnExportData()
        {
            var ila = ImageLayerActions;
            if (ila != null)
            {
                ila.ExportData(Image);
            }
        }

        /// <summary>
        /// Dispose memory objects.
        /// </summary>
        /// <param name="disposeManagedResources">True if managed memory objects should be set to null.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
            {
                _symbolizer = null;
            }
        }
    }
}
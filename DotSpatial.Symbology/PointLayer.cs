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
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Modified to do 3D in January 2008 by Ted Dunsford
// ********************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles point drawing
    /// </summary>
    public class PointLayer : FeatureLayer, IPointLayer
    {
        #region Constructors

        /// <summary>
        /// This creates a new layer with an empty dataset configured to the point feature type.
        /// </summary>
        public PointLayer()
            : this(new FeatureSet(FeatureType.Point), null)
        {
        }

        /// <summary>
        /// Creates a new instance of a PointLayer without sending any status messages
        /// </summary>
        /// <param name="inFeatureSet">The IFeatureLayer of data values to turn into a graphical PointLayer</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point</exception>
        public PointLayer(IFeatureSet inFeatureSet)
            : this(inFeatureSet, null)
        {
            // this simply handles the default case where no status messages are requested
        }

        /// <summary>
        /// Creates a new instance of a PointLayer for storing and drawing points
        /// </summary>
        /// <param name="inFeatureSet">Any implentation of an IFeatureLayer</param>
        /// <param name="progressHandler">A valid implementation of the IProgressHandler interface.</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point</exception>
        public PointLayer(IFeatureSet inFeatureSet, IProgressHandler progressHandler)
            : base(inFeatureSet, null, progressHandler)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Creates a new instance of a PointLayer for storing and drawing points.
        /// </summary>
        /// <param name="inFeatureSet">Any implementation of an IFeatureLayer.</param>
        /// <param name="container">An IContainer to contain this layer.</param>
        /// <param name="progressHandler">A valid implementation of the IProgressHandler interface.</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is
        ///  not point or multi-point.</exception>
        public PointLayer(IFeatureSet inFeatureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(inFeatureSet, container, progressHandler)
        {
            Configure(inFeatureSet);
        }

        private void Configure(IFeatureSet inFeatureSet)
        {
            FeatureType ft = inFeatureSet.FeatureType;
            if (ft != FeatureType.Point && ft != FeatureType.MultiPoint && ft != FeatureType.Unspecified)
            {
                throw new PointFeatureTypeException();
            }
            if (inFeatureSet.NumRows() == 0)
            {
                MyExtent = new Extent(-180, -90, 180, 90);
            }
            if (inFeatureSet.NumRows() == 1)
            {
                MyExtent = inFeatureSet.Extent.Copy();
                MyExtent.ExpandBy(10, 10);
            }
            Symbology = new PointScheme();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pointSymbolizer characteristics to use for the selected features.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the symbolic characteristics to use for the selected features."),
            //TypeConverter(typeof(GeneralTypeConverter)),
            //Editor(typeof(PointSymbolizerEditor), typeof(UITypeEditor)),
         ShallowCopy]
        public new IPointSymbolizer SelectionSymbolizer
        {
            get { return base.SelectionSymbolizer as IPointSymbolizer; }
            set
            {
                base.SelectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbolic characteristics for this layer.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the symbolic characteristics for this layer."),
            //TypeConverter(typeof(GeneralTypeConverter)),
            // Editor(typeof(PointSymbolizerEditor), typeof(UITypeEditor)),
         ShallowCopy]
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }
            set
            {
                base.Symbolizer = value;
            }
        }

        /// <summary>
        /// Gets the currently applied scheme.  Because setting the scheme requires a processor intensive
        /// method, we use the ApplyScheme method for assigning a new scheme.  This allows access
        /// to editing the members of an existing scheme directly, however.
        /// </summary>
        [Category("Appearance"), Description("Gets the currently applied scheme."),
            //TypeConverter(typeof(GeneralTypeConverter)),
            //Editor(typeof(PointSchemePropertyGridEditor), typeof(UITypeEditor)),
         Serialize("Symbology")]
        public new IPointScheme Symbology
        {
            get { return base.Symbology as IPointScheme; }
            set { base.Symbology = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Attempts to create a new PointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <param name="progressHandler">Any valid implementation of IProgressHandler for receiving progress messages</param>
        /// <returns>A PointLayer created from the specified fileName.</returns>
        public static new IPointLayer OpenFile(string fileName, IProgressHandler progressHandler)
        {
            ILayer fl = LayerManager.DefaultLayerManager.OpenLayer(fileName, progressHandler);
            return fl as PointLayer;
        }

        /// <summary>
        /// Attempts to create a new PointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <returns>A PointLayer created from the specified fileName.</returns>
        public static new IPointLayer OpenFile(string fileName)
        {
            IFeatureLayer fl = LayerManager.DefaultLayerManager.OpenVectorLayer(fileName);
            return fl as PointLayer;
        }

        #endregion
    }
}
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
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for LineStrings
    /// </summary>
    public class PolygonLayer : FeatureLayer, IPolygonLayer
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        ///<param name="inFeatureSet">A featureset that contains polygons</param>
        ///<exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public PolygonLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="inFeatureSet">A featureset that contains polygons</param>
        /// <param name="progressHandler">An IProgressHandler to receive progress messages</param>
        /// <exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public PolygonLayer(IFeatureSet inFeatureSet, IProgressHandler progressHandler)
            : base(inFeatureSet, null, progressHandler)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Initializes a new instance of the PolygonLayer class.
        /// </summary>
        /// <param name="inFeatureSet">A featureset that contains polygons.</param>
        /// <param name="container">A Container to store the newly created layer in.</param>
        /// <param name="progressHandler">An IProgressHandler to receive progress messages.</param>
        /// <exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public PolygonLayer(IFeatureSet inFeatureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(inFeatureSet, container, progressHandler)
        {
            Configure(inFeatureSet);
        }

        private void Configure(IFeatureSet inFeatureSet)
        {
            if (inFeatureSet.FeatureType != FeatureType.Polygon)
            {
                throw new PolygonFeatureTypeException();
            }
            PolygonScheme ps = new PolygonScheme();
            ps.SetParentItem(this);
            Symbology = ps;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws some section of the extent to the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        /// <param name="p">The projection interface that specifies how to transform geographic coordinates to an image.</param>
        public override void DrawSnapShot(Graphics g, IProj p)
        {
            //bool To_DO_DRaw_Polygon_Snapshot = true;
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolic characteristics for the members of this symbol class that have been selected.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Category("Appearance")]
        [Description("Gets or sets the symbolic characteristics for the regular polygons in this layer or symbol class")]
        [ShallowCopy]
        public new IPolygonSymbolizer SelectionSymbolizer
        {
            get
            {
                return base.SelectionSymbolizer as IPolygonSymbolizer;
            }
            set
            {
                base.SelectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the default Polygon Symbolizer to use with all the lines on this layer.
        /// Setting this will not clear the existing individually specified Symbolizers,
        /// only the default symbolizer.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Category("Appearance")]
        [Description("Gets or sets the symbolic characteristics for the regular polygons in this layer or symbol class")]
        [ShallowCopy]
        public new IPolygonSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPolygonSymbolizer;
            }
            set
            {
                base.Symbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the polygon scheme that symbolically breaks down the drawing into symbol categories.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(PolygonSchemePropertyGridEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Category("Appearance")]
        [Description("Gets or sets the entire scheme to use for symbolizing this polygon layer.")]
        [Serialize("Symbology")]
        public new IPolygonScheme Symbology
        {
            get { return base.Symbology as IPolygonScheme; }
            set
            {
                base.Symbology = value;
            }
        }

        #endregion
    }
}
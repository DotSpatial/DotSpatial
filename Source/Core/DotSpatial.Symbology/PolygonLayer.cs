// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for polygons.
    /// </summary>
    public class PolygonLayer : FeatureLayer, IPolygonLayer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">A featureset that contains polygons.</param>
        /// <exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public PolygonLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">A featureset that contains polygons.</param>
        /// <param name="progressHandler">An IProgressHandler to receive progress messages.</param>
        /// <exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public PolygonLayer(IFeatureSet inFeatureSet, IProgressHandler progressHandler)
            : base(inFeatureSet, null, progressHandler)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonLayer"/> class.
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolic characteristics for the members of this symbol class that have been selected.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))].
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
        /// [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))].
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
        /// [Editor(typeof(PolygonSchemePropertyGridEditor), typeof(UITypeEditor))].
        /// </remarks>
        [Category("Appearance")]
        [Description("Gets or sets the entire scheme to use for symbolizing this polygon layer.")]
        [Serialize("Symbology")]
        public new IPolygonScheme Symbology
        {
            get
            {
                return base.Symbology as IPolygonScheme;
            }

            set
            {
                base.Symbology = value;
            }
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
            // bool To_DO_DRaw_Polygon_Snapshot = true;
            throw new NotImplementedException();
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
    }
}
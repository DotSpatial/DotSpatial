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
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for LineStrings
    /// </summary>
    public class LineLayer : FeatureLayer, ILineLayer
    {
        #region Constructors

        /// <summary>
        /// This creates a new layer with an empty dataset configured to the point feature type.
        /// </summary>
        public LineLayer()
            : this(new FeatureSet(FeatureType.Line))
        {
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="inFeatureSet">A featureset that contains lines</param>
        /// <param name="progressHandler">An IProgressHandler for receiving status messages</param>
        /// <exception cref="LineFeatureTypeException">Thrown if a non-line featureSet is supplied</exception>
        public LineLayer(IFeatureSet inFeatureSet, IProgressHandler progressHandler = null)
            : base(inFeatureSet, null, progressHandler)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Constructor that also shows progress.
        /// </summary>
        /// <param name="inFeatureSet">A featureset that contains lines.</param>
        /// <param name="container">An IContainer that the line layer should be created in.</param>
        /// <param name="progressHandler">An IProgressHandler for receiving status messages.</param>
        /// <exception cref="LineFeatureTypeException">Thrown if a non-line featureSet is supplied.</exception>
        public LineLayer(IFeatureSet inFeatureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(inFeatureSet, container, progressHandler)
        {
            Configure(inFeatureSet);
        }

        private void Configure(IFeatureSet inFeatureSet)
        {
            if (inFeatureSet == null) throw new ArgumentNullException("inFeatureSet");
            if (inFeatureSet.FeatureType != FeatureType.Line) throw new LineFeatureTypeException();

            Symbology = new LineScheme();
            Symbology.SetParentItem(this);
        }

        #endregion
      
        #region Properties

        /// <summary>
        /// Gets or sets the line symbolizer to use for the selection
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Category("Appearance")]
        [Description("Gets or sets the set of characteristics that describe the selected features on this line layer.")]
        [ShallowCopy]
        public new ILineSymbolizer SelectionSymbolizer
        {
            get { return base.SelectionSymbolizer as ILineSymbolizer; }
            set { base.SelectionSymbolizer = value; }
        }

        /// <summary>
        /// Gets or sets the FeatureSymbolizerOld determining the shared properties.  This is actually still the PointSymbolizerOld
        /// and should not be used directly on Polygons or Lines.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        /// <exception cref="System.InvalidOperationException">Unable to assign a non-point symbolizer to a PointLayer</exception>
        [Category("Appearance")]
        [Description("Gets or sets the set of characteristics that describe the un-selected features on this line layer.")]
        [ShallowCopy]
        public new ILineSymbolizer Symbolizer
        {
            get { return base.Symbolizer as ILineSymbolizer; }
            set { base.Symbolizer = value; }
        }

        /// <summary>
        /// Gets or sets the line scheme subdividing the layer into categories.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(LineSchemePropertyGridEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Category("Appearance")]
        [Description("Gets or sets the line scheme subdividing the layer into categories.")]
        [Serialize("Symbology")]
        public new ILineScheme Symbology
        {
            get { return base.Symbology as ILineScheme; }
            set { base.Symbology = value; }
        }

        #endregion
    }
}
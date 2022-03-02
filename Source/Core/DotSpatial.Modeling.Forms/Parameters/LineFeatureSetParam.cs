// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Line Feature Parameter past back from a ITool to the toolbox manager.
    /// </summary>
    public class LineFeatureSetParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineFeatureSetParam"/> class.
        /// Creates a new Line Feature Set parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public LineFeatureSetParam(string name)
        {
            Name = name;
            Value = new FeatureSet();
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial LineFeatureSet Param";
            DefaultSpecified = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input).
        /// </summary>
        public new IFeatureSet Value
        {
            get
            {
                return (IFeatureSet)base.Value;
            }

            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a default instance of the data type so that tools have something to write too.
        /// </summary>
        /// <param name="path">Path of the generated shapefile.</param>
        public override void GenerateDefaultOutput(string path)
        {
            FeatureSet addedFeatureSet = new LineShapefile
                                             {
                                                 Filename = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + ModelName + ".shp"
                                             };
            Value = addedFeatureSet;
        }

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new LineElement(this, dataSets);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new LineElementOut(this, dataSets);
        }

        #endregion
    }
}
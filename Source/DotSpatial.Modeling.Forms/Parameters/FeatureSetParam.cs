// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// FeatureSet parameter allows ITools to specify that they require a FeatureSet as input.
    /// </summary>
    public class FeatureSetParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSetParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public FeatureSetParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial FeatureSet Param";
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
        /// Generates a default instance of the data type so that tools have something to write to.
        /// </summary>
        /// <param name="path">Path of the generated featureset.</param>
        public override void GenerateDefaultOutput(string path)
        {
            FeatureSet addedFeatureSet = new FeatureSet
                                             {
                                                 Filename = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + ModelName + ".shp"
                                             };
            Value = addedFeatureSet;
        }

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new FeatureSetElement(this, dataSets);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new FeatureSetElementOut(this, dataSets);
        }

        #endregion
    }
}
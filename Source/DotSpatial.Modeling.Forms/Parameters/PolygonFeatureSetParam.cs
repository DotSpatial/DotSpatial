﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Polygon Feature Set Parameters past back from a ITool to the toolbox manager
    /// </summary>
    public class PolygonFeatureSetParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonFeatureSetParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public PolygonFeatureSetParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial PolygonFeatureSet Param";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input)
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
            FeatureSet addedFeatureSet = new PolygonShapefile
                                             {
                                                 Filename = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + ModelName + ".shp"
                                             };
            Value = addedFeatureSet;
        }

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new PolygonElement(this, dataSets);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new PolygonElementOut(this, dataSets);
        }

        #endregion
    }
}
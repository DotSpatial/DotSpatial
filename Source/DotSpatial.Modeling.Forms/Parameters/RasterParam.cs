// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Raster parameter allows ITools to specify that they require a Raster data set as input
    /// </summary>
    public class RasterParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterParam"/> class.
        /// Creates a new Raster parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public RasterParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.Always;
            ParamType = "DotSpatial Raster Param";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input)
        /// </summary>
        public new IRaster Value
        {
            get
            {
                return (IRaster)base.Value;
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
        /// Creates a new blank raster.
        /// </summary>
        /// <param name="path">Filename of the raster.</param>
        public override void GenerateDefaultOutput(string path)
        {
            Value = new Raster
                        {
                            Filename = path
                        };
        }

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new RasterElement(this, dataSets);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new RasterElementOut(this, dataSets);
        }

        #endregion
    }
}
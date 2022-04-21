// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;
using DotSpatial.Projections;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// ProjectionParam.
    /// </summary>
    public class ProjectionParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionParam"/> class with the specified name and a default projection of WGS1984.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public ProjectionParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Projection Param";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionParam"/> class with the specified name
        /// and the specified projection as the default projection that will appear if no changes are made.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="defaultProjection">The default projection.</param>
        public ProjectionParam(string name, ProjectionInfo defaultProjection)
        {
            Name = name;
            Value = defaultProjection;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial String Param";
            DefaultSpecified = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input).
        /// </summary>
        public new ProjectionInfo Value
        {
            get
            {
                return (ProjectionInfo)base.Value;
            }

            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new ProjectionElement(this);
        }

        /// <inheritdoc/>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new ProjectionElement(this);
        }

        #endregion
    }
}
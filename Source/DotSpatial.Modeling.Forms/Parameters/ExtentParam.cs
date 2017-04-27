// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2009 1:48:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// ExtentParam
    /// </summary>
    public class ExtentParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentParam"/> class with the specified name.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        public ExtentParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Extent Param";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentParam"/> class with the specified name
        /// and the specified default extent.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="defaultExtent">Default extent.</param>
        public ExtentParam(string name, Extent defaultExtent)
        {
            Name = name;
            Value = defaultExtent;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Extent Param";
            DefaultSpecified = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the extent should be modified to show the MapExtent
        /// before being shown in the tool dialog.
        /// </summary>
        public bool DefaultToMapExtent { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input)
        /// </summary>
        public new Extent Value
        {
            get
            {
                return (Extent)base.Value;
            }

            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new ExtentElement(this);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new ExtentElement(this);
        }

        #endregion
    }
}
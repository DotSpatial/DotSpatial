// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// ProjectionParam
    /// </summary>
    public class ExtentParam : Parameter
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectionParam with the specified name
        /// and a default projection of WGS1984
        /// </summary>
        public ExtentParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Extent Param";
        }

        /// <summary>
        /// Creates a new instance of an Extent Param with the specified name
        /// and the specified projection as the default projection that will
        /// appear if no changes are made.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultExtent"></param>
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
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new Extent Value
        {
            get { return (Extent)base.Value; }
            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualize INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new ExtentElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualize OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new ExtentElement(this));
        }

        #endregion
    }
}
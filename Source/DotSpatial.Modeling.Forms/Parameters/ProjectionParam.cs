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
using DotSpatial.Modeling.Forms.Elements;
using DotSpatial.Projections;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// ProjectionParam
    /// </summary>
    public class ProjectionParam : Parameter
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectionParam with the specified name
        /// and a default projection of WGS1984
        /// </summary>
        public ProjectionParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Projection Param";
        }

        /// <summary>
        /// Creates a new instance of a Projection Param with the specified name
        /// and the specified projection as the default projection that will
        /// appear if no changes are made.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultProjection"></param>
        public ProjectionParam(string name, ProjectionInfo defaultProjection)
        {
            Name = name;
            Value = defaultProjection;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial String Param";
            DefaultSpecified = true;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new ProjectionInfo Value
        {
            get { return (ProjectionInfo)base.Value; }
            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new ProjectionElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new ProjectionElement(this));
        }

        #endregion
    }
}
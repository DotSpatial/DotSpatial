// ********************************************************************************************************
// Product Name: DotSpatial.Tools.StringParam
// Description:  String Parameters returned by an ITool allows the tool to specify a default value
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
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
    /// Index Parameters returned by an ITool allows the tool to select the index for given Featureset indexs.
    /// </summary>
    public class IndexParam : Parameter
    {
        #region Fields

        private IndexElement _indexEle;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public IndexParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial String Param";
            DefaultSpecified = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the featureset used to populate the query generator.
        /// </summary>
        public FeatureSet Fs { get; set; } = new FeatureSet();

        /// <summary>
        /// Gets the value of the parameter (This will give the result featureset that user want handle.)
        /// </summary>
        public new string Value => _indexEle.Expression;

        #endregion

        #region Methods

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            _indexEle = new IndexElement(this);
            return _indexEle;
        }

        #endregion
    }
}
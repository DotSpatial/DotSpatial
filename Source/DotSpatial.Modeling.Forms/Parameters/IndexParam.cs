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
        #region variables

        private FeatureSet _fs = new FeatureSet();
        private IndexElement _indexEle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Index parameter
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

        #region properties

        /// <summary>
        /// The featureset used to populate the query generator
        /// </summary>
        public FeatureSet Fs
        {
            get { return _fs; }
            set { _fs = value; }
        }

        /// <summary>
        /// Specifies the value of the parameter (This will give the result featureset that user want handle.)
        /// </summary>
        public new string Value
        {
            get
            {
                if (_indexEle.Expression == null)
                    return null;
                return _indexEle.Expression;
            }
        }

        #endregion

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            _indexEle = new IndexElement(this);
            return (_indexEle);
        }
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DataSetArray
// Description:  DataSetArray used to create an array of DataSets with their associated name
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Used to create arrays of data sets with an associated name to be passed into Tools to populate their dialog boxes
    /// </summary>
    public class DataSetArray
    {
        private IDataSet _dataSet;
        private string _name;

        /// <summary>
        /// Creates an instance of a simple object that holds a name and a dataset
        /// </summary>
        /// <param name="name"> The name of the DataSet in this object</param>
        /// <param name="dataSet">The IDataSet in this object</param>
        public DataSetArray(string name, IDataSet dataSet)
        {
            _name = name;
            _dataSet = dataSet;
        }

        /// <summary>
        /// The name of the DataSet in this object
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The IDataSet in this object
        /// </summary>
        public IDataSet DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }

        /// <summary>
        /// Returns the Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _name;
        }
    }
}
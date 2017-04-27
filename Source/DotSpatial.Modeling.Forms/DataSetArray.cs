// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DataSetArray
// Description:  DataSetArray used to create an array of DataSets with their associated name
//
// ********************************************************************************************************
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
    /// Used to create arrays of data sets with an associated name to be passed into Tools to populate their dialog boxes.
    /// </summary>
    public class DataSetArray
    {
        #region Fields

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetArray"/> class that holds a name and a dataset.
        /// </summary>
        /// <param name="name"> The name of the DataSet in this object</param>
        /// <param name="dataSet">The IDataSet in this object</param>
        public DataSetArray(string name, IDataSet dataSet)
        {
            Name = name;
            DataSet = dataSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IDataSet in this object.
        /// </summary>
        public IDataSet DataSet { get; set; }

        /// <summary>
        /// Gets or sets the name of the DataSet in this object.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the Name
        /// </summary>
        /// <returns>The name.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
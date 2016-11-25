// *******************************************************************************************************
// Product: DotSpatial.Tools.Area.cs
// Description:  A tool to calculate the area of a given feature set.

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name                   |   Date                 |         Comments
//------------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for Area
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using NetTopologySuite.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Area
    /// </summary>
    public class Area : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the area calculation tool
        /// </summary>
        public Area()
        {
            this.Category = TextStrings.Statistics;
            this.ToolTip = TextStrings.AreaDescription;
            this.Description = TextStrings.AreaDescription;
            this.Name = TextStrings.CalculateAreas;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters
        {
            get
            {
                return _inputParam;
            }
        }

        /// <summary>
        /// Gets or Sets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters
        {
            get
            {
                return _outputParam;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if succesful
        /// </summary>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            if (input != null)
            {
                input.FillAttributes();
            }

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, output, cancelProgressHandler);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="cancelProgressHandler"></param>
        /// <returns></returns>
        public bool Execute(IFeatureSet input, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            // We add all the fields
            foreach (DataColumn inputColumn in input.DataTable.Columns)
            {
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));
            }

            // We add the area field
            bool addField = true;
            string fieldCount = string.Empty;
            int i = 0;
            while (addField)
            {
                if (output.DataTable.Columns.Contains(TextStrings.Area + fieldCount) == false)
                {
                    output.DataTable.Columns.Add(new DataColumn(TextStrings.Area + fieldCount, typeof(double)));
                    addField = false;
                }
                else
                {
                    fieldCount = i.ToString();
                    i++;
                }
            }

            // we add all the old features to output
            for (int j = 0; j < input.Features.Count; j++)
            {
                Feature newFeature = new Feature(input.Features[j].Geometry, output);
                foreach (DataColumn colSource in input.DataTable.Columns)
                {
                    newFeature.DataRow[colSource.ColumnName] = input.Features[j].DataRow[colSource.ColumnName];
                }

                newFeature.DataRow[TextStrings.Area + fieldCount] = output.Features[j].Geometry.Area;

                // Status updates is done here
                cancelProgressHandler.Progress(
                    string.Empty,
                    Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(input.Features.Count)) * 100),
                    input.Features[j].DataRow[0].ToString());
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }
            }

            output.AttributesPopulated = true;
            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[1];
            _inputParam[0] = new PolygonFeatureSetParam(TextStrings.PolygonFeatureSet);

            _outputParam = new Parameter[2];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.PolygonFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}
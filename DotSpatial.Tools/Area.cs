// ********************************************************************************************************
// Product Name: MapWindow.Tools.mwArea
// Description:  This is the first tool ever written for the MapWindow Toolbox. It calculates the are of a
//               polygon
//
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the MapWindow 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Jan, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -----------------------|------------------------|------------------------------------------------------
// Ted Dunsford           |  8/24/2009             |  Cleaned up some formatting issues using re-sharper
// KP                     |  9/2009                |  Used IDW as model for Area
// Ping Yang              |  12/2009               |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;

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
                Feature newFeature = new Feature(input.Features[j].BasicGeometry, output);
                foreach (DataColumn colSource in input.DataTable.Columns)
                {
                    newFeature.DataRow[colSource.ColumnName] = input.Features[j].DataRow[colSource.ColumnName];
                }

                newFeature.DataRow[TextStrings.Area + fieldCount] =
                    MultiPolygon.FromBasicGeometry(output.Features[j].BasicGeometry).Area;

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

            _outputParam = new Parameter[1];
            _outputParam[0] = new PolygonFeatureSetParam(TextStrings.PolygonFeatureSet);
        }

        #endregion
    }
}
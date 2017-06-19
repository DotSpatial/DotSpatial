// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Area
    /// </summary>
    public class Area : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Area"/> class.
        /// </summary>
        public Area()
        {
            Category = TextStrings.Statistics;
            ToolTip = TextStrings.AreaDescription;
            Description = TextStrings.AreaDescription;
            Name = TextStrings.CalculateAreas;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array
        /// </summary>
        public override Parameter[] OutputParameters => _outputParam;

        #endregion

        #region Methods

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IFeatureSet input = _inputParam[0].Value as IFeatureSet;
            input?.FillAttributes();

            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, output, cancelProgressHandler);
        }

        /// <summary>
        /// Once the Parameter have been configured the Execute command can be called, it returns true if successful.
        /// </summary>
        /// <param name="input">The input IFeatureSet.</param>
        /// <param name="output">The output IFeatureSet.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
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
                cancelProgressHandler.Progress(string.Empty, Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(input.Features.Count)) * 100), input.Features[j].DataRow[0].ToString());
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
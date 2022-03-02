// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using NetTopologySuite.Geometries;
using NetTopologySuite.Simplify;

namespace DotSpatial.Tools
{
    /// <summary>
    /// This tool reduces the number of points on polylines using the Douglas-Peucker line
    /// simplification algorithm.
    /// </summary>
    public class DpSimplification : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DpSimplification"/> class.
        /// </summary>
        public DpSimplification()
        {
            Name = TextStrings.SimplifyLines;
            Category = TextStrings.Generalization;
            Description = TextStrings.DouglasPeuckerlinesimplification;
            ToolTip = TextStrings.DPlinesimplification;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input paramater array.
        /// </summary>
        public override Parameter[] InputParameters => _inputParam;

        /// <summary>
        /// Gets the output paramater array.
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

            double tolerance = (double)_inputParam[1].Value;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, tolerance, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the DP line simplefy tool programmatically.
        /// Ping Yang Added it for external Testing.
        /// </summary>
        /// <param name="input">The input polygon feature set.</param>
        /// <param name="tolerance">The tolerance to use when simplefiying.</param>
        /// <param name="output">The output polygon feature set.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input, double tolerance, IFeatureSet output)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            // We copy all the fields
            foreach (DataColumn inputColumn in input.DataTable.Columns)
            {
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));
            }

            foreach (IFeature t in input.Features)
            {
                Geometry geom = t.Geometry as Geometry;
                if (geom != null)
                {
                    for (int part = 0; part < geom.NumGeometries; part++)
                    {
                        Geometry geomPart = (Geometry)geom.GetGeometryN(part);

                        // do the simplification
                        Coordinate[] oldCoords = geomPart.Coordinates;
                        Coordinate[] newCoords = DouglasPeuckerLineSimplifier.Simplify(oldCoords, tolerance);

                        // convert the coordinates back to a geometry
                        Geometry newGeom = new LineString(newCoords);
                        Feature newFeature = new Feature(newGeom, output);
                        foreach (DataColumn colSource in input.DataTable.Columns)
                        {
                            newFeature.DataRow[colSource.ColumnName] = t.DataRow[colSource.ColumnName];
                        }
                    }
                }
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// Executes the DP line simplefy tool programmatically.
        /// </summary>
        /// <param name="input">The input polygon feature set.</param>
        /// <param name="tolerance">The tolerance to use when simplefiying.</param>
        /// <param name="output">The output polygon feature set.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>True, if executed successfully.</returns>
        public bool Execute(IFeatureSet input, double tolerance, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            // We copy all the fields
            foreach (DataColumn inputColumn in input.DataTable.Columns)
            {
                output.DataTable.Columns.Add(new DataColumn(inputColumn.ColumnName, inputColumn.DataType));
            }

            int numTotalOldPoints = 0;
            int numTotalNewPoints = 0;

            for (int j = 0; j < input.Features.Count; j++)
            {
                int numOldPoints = 0;
                int numNewPoints = 0;

                Geometry geom = input.Features[j].Geometry as Geometry;
                if (geom != null)
                {
                    numOldPoints = geom.NumPoints;
                }

                numTotalOldPoints += numOldPoints;
                if (geom != null)
                {
                    for (int part = 0; part < geom.NumGeometries; part++)
                    {
                        Geometry geomPart = (Geometry)geom.GetGeometryN(part);

                        // do the simplification
                        Coordinate[] oldCoords = geomPart.Coordinates;
                        Coordinate[] newCoords = DouglasPeuckerLineSimplifier.Simplify(oldCoords, tolerance);

                        // convert the coordinates back to a geometry
                        Geometry newGeom = new LineString(newCoords);
                        numNewPoints += newGeom.NumPoints;
                        numTotalNewPoints += numNewPoints;
                        Feature newFeature = new Feature(newGeom, output);
                        foreach (DataColumn colSource in input.DataTable.Columns)
                        {
                            newFeature.DataRow[colSource.ColumnName] = input.Features[j].DataRow[colSource.ColumnName];
                        }
                    }
                }

                // Status updates is done here, shows number of old / new points
                cancelProgressHandler.Progress(Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(input.Features.Count)) * 100), numOldPoints + "-->" + numNewPoints);
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }
            }

            cancelProgressHandler.Progress(100, TextStrings.Originalnumberofpoints + numTotalOldPoints + " " + TextStrings.Newnumberofpoints + numTotalNewPoints);

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new LineFeatureSetParam(TextStrings.LineFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.Tolerance)
            {
                Value = 10.0
            };

            _outputParam = new Parameter[2];
            _outputParam[0] = new LineFeatureSetParam(TextStrings.LineFeatureSet);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        #endregion
    }
}
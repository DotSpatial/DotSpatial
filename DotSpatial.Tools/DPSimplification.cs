// *******************************************************************************************************
// Product: DotSpatial.Tools.DPSimplification.cs
// Description:  This tool reduces the number of points on polylines using the Douglas-Peucker line
//               simplification algorithm
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Brian Marchionni   |  04/30/2009        |  Cleaned it up
// Ted Dunsford       |  8/24/2009         |  Cleaned up some unnecessary references using re-sharper
// KP                 |  9/2009            |  Used IDW as model for DPSimplification
// Ping Yang          |  12/2009           |  Cleaning code and fixing bugs.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Topology;
using DotSpatial.Topology.Simplify;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Line-simplification using Douglas-Peucker algorithm
    /// </summary>
    public class DpSimplification : Tool
    {
        #region Constants and Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the DpSimplification class.
        /// </summary>
        public DpSimplification()
        {
            this.Name = TextStrings.SimplifyLines;
            this.Category = TextStrings.Generalization;
            this.Description = TextStrings.DouglasPeuckerlinesimplification;
            this.ToolTip = TextStrings.DPlinesimplification;
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

            double tolerance = (double)_inputParam[1].Value;
            IFeatureSet output = _outputParam[0].Value as IFeatureSet;

            return Execute(input, tolerance, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the DP line simplefy tool programaticaly
        /// Ping Yang Added it for external Testing
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="tolerance">The tolerance to use when simplefiying</param>
        /// <param name="output">The output polygon feature set</param>
        /// <returns></returns>
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
                Geometry geom = t.BasicGeometry as Geometry;
                if (geom != null)
                {
                    for (int part = 0; part < geom.NumGeometries; part++)
                    {
                        Geometry geomPart = (Geometry)geom.GetGeometryN(part);

                        // do the simplification
                        IList<Coordinate> oldCoords = geomPart.Coordinates;
                        IList<Coordinate> newCoords = DouglasPeuckerLineSimplifier.Simplify(
                            oldCoords, tolerance);

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
        /// Executes the DP line simplefy tool programaticaly
        /// </summary>
        /// <param name="input">The input polygon feature set</param>
        /// <param name="tolerance">The tolerance to use when simplefiying</param>
        /// <param name="output">The output polygon feature set</param>
        /// <param name="cancelProgressHandler">The progress handler</param>
        /// <returns></returns>
        public bool Execute(
            IFeatureSet input, double tolerance, IFeatureSet output, ICancelProgressHandler cancelProgressHandler)
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

                Geometry geom = input.Features[j].BasicGeometry as Geometry;
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
                        IList<Coordinate> oldCoords = geomPart.Coordinates;
                        IList<Coordinate> newCoords = DouglasPeuckerLineSimplifier.Simplify(
                            oldCoords, tolerance);

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
                cancelProgressHandler.Progress(
                    string.Empty,
                    Convert.ToInt32((Convert.ToDouble(j) / Convert.ToDouble(input.Features.Count)) * 100),
                    numOldPoints + "-->" + numNewPoints);
                if (cancelProgressHandler.Cancel)
                {
                    return false;
                }
            }

            cancelProgressHandler.Progress(
                string.Empty,
                100,
                TextStrings.Originalnumberofpoints + numTotalOldPoints + " " + TextStrings.Newnumberofpoints
                + numTotalNewPoints);

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new LineFeatureSetParam(TextStrings.LineFeatureSet);
            _inputParam[1] = new DoubleParam(TextStrings.Tolerance) { Value = 10.0 };

            _outputParam = new Parameter[1];
            _outputParam[0] = new LineFeatureSetParam(TextStrings.LineFeatureSet);
        }

        #endregion
    }
}
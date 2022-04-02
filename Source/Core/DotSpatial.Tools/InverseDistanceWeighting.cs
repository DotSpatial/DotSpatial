// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.KdTree;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Fixed Count or Distance.
    /// </summary>
    public enum NeighborhoodType
    {
        /// <summary>
        /// Returns as many neighbors as fall within the specified range
        /// </summary>
        FixedDistance,

        /// <summary>
        /// Returns the nearest x values, regardless of how close.
        /// </summary>
        FixedCount
    }

    /// <summary>
    /// Creates a raster from a point feature set using Inverse Distance Weighting.
    /// </summary>
    public class InverseDistanceWeighting : Tool
    {
        #region Fields
        private Parameter[] _inputParam;
        private List<string> _neighborhoodType;
        private Parameter[] _outputParam;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InverseDistanceWeighting"/> class.
        /// </summary>
        public InverseDistanceWeighting()
        {
            Name = "IDW";
            Category = TextStrings.Interpolation;
            Description = TextStrings.IDWDescription;
            ToolTip = TextStrings.InverseDistanceWeighting;
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
        /// <param name="cancelProgressHandler">A progress handler for receiving progress messages.</param>
        /// <returns>A boolean, true if the IDW process worked correctly.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            if (!(_inputParam[0].Value is IFeatureSet input) || !(_inputParam[1] is ListParam lp)) return false;

            input.FillAttributes();

            string zField = lp.ValueList[lp.Value];
            double cellSize = (double)_inputParam[2].Value;
            double power = (double)_inputParam[3].Value;
            NeighborhoodType neighborType = _inputParam[4].Value as string == TextStrings.FixedDistance ? NeighborhoodType.FixedDistance : NeighborhoodType.FixedCount;
            int pointCount = (int)_inputParam[5].Value;
            double distance = (double)_inputParam[6].Value;
            IRaster output = _outputParam[0].Value as IRaster;
            return Execute(input, zField, cellSize, power, neighborType, pointCount, distance, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Area tool with programatic input
        /// Ping delete static for external testing.
        /// </summary>
        /// <param name="input">The input raster.</param>
        /// <param name="zField">The field name containing the values to interpolate.</param>
        /// <param name="cellSize">The double geographic size of the raster cells to create.</param>
        /// <param name="power">The double power representing the inverse.</param>
        /// <param name="neighborType">Fixed distance of fixed number of neighbors.</param>
        /// <param name="pointCount">The number of neighbors to include if the neighborhood type
        /// is Fixed.</param>
        /// <param name="distance">Points further from the raster cell than this distance are not included
        /// in the calculation if the neighborhood type is Fixed Distance.</param>
        /// <param name="output">The output raster where values are stored. The fileName is used, but the number
        /// of rows and columns will be computed from the cellSize and input featureset.</param>
        /// <param name="cancelProgressHandler">A progress handler for receiving progress messages.</param>
        /// <returns>A boolean, true if the IDW process worked correctly.</returns>
        public bool Execute(IFeatureSet input, string zField, double cellSize, double power, NeighborhoodType neighborType, int pointCount, double distance, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input == null || output == null)
            {
                return false;
            }

            // If the cellSize is 0 we calculate a cell size based on the input extents
            if (cellSize == 0)
            {
                cellSize = input.Extent.Width / 255;
            }

            // Defines the dimensions and position of the raster
            int numColumns = Convert.ToInt32(Math.Round(input.Extent.Width / cellSize));
            int numRows = Convert.ToInt32(Math.Round(input.Extent.Height / cellSize));

            output = Raster.CreateRaster(output.Filename, string.Empty, numColumns, numRows, 1, typeof(double), new[] { string.Empty });

            output.CellHeight = cellSize;
            output.CellWidth = cellSize;
            output.Xllcenter = input.Extent.MinX + (cellSize / 2);
            output.Yllcenter = input.Extent.MinY + (cellSize / 2);

            // Used to calculate progress
            int lastUpdate = 0;

            // Populates the KD tree
            var kd = new KdTreeEx<IFeature>();
            List<int> randomList = new();
            for (int i = 0; i < input.Features.Count; i++)
            {
                randomList.Add(i);
            }

            Random rnd = new();
            List<int> completed = new();
            while (randomList.Count > 0)
            {
                int index = rnd.Next(0, randomList.Count - 1);
                Coordinate coord = input.Features[randomList[index]].Geometry.Coordinates[0];
                while (kd.Search(coord) != null)
                {
                    coord.X *= 1.000000000000001D;
                }

                kd.Insert(coord, input.Features[randomList[index]]);
                completed.Add(randomList[index]);
                randomList.RemoveAt(index);
            }

            if (neighborType == NeighborhoodType.FixedCount)
            {
                // we add all the old features to output
                for (int x = 0; x < numColumns; x++)
                {
                    for (int y = 0; y < numRows; y++)
                    {
                        // Gets the pointCount number of cells closest to the current cell
                        Coordinate cellCenter = output.CellToProj(y, x);
                        var coord = output.CellToProj(y, x);
                        var result = kd.NearestNeighbor(coord);
                        var featurePt = result?.Data;
                        if (featurePt != null)
                        {
                            // Sets up the IDW numerator and denominator
                            double top = 0;
                            double bottom = 0;

                            double distanceToCell = cellCenter.Distance(featurePt.Geometry.Coordinates[0]);
                            if (distanceToCell <= distance || distance == 0)
                            {
                                // If we can't convert the value to a double throw it out
                                try
                                {
                                    Convert.ToDouble(featurePt.DataRow[zField]);
                                }
                                catch
                                {
                                    continue;
                                }

                                if (power == 2)
                                {
                                    top += (1 / (distanceToCell * distanceToCell)) * Convert.ToDouble(featurePt.DataRow[zField]);
                                    bottom += 1 / (distanceToCell * distanceToCell);
                                }
                                else
                                {
                                    top += (1 / Math.Pow(distanceToCell, power)) * Convert.ToDouble(featurePt.DataRow[zField]);
                                    bottom += 1 / Math.Pow(distanceToCell, power);
                                }
                            }

                            output.Value[y, x] = top / bottom;
                        }
                    }

                    // Checks if we need to update the status bar
                    if (Convert.ToInt32(Convert.ToDouble(x * numRows) / Convert.ToDouble(numColumns * numRows) * 100) > lastUpdate)
                    {
                        lastUpdate = Convert.ToInt32(Convert.ToDouble(x * numRows) / Convert.ToDouble(numColumns * numRows) * 100);
                        cancelProgressHandler.Progress(lastUpdate, "Cell: " + (x * numRows) + " of " + (numColumns * numRows));
                        if (cancelProgressHandler.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[7];
            _inputParam[0] = new PointFeatureSetParam(TextStrings.PointFeatureSet);
            _inputParam[1] = new ListParam(TextStrings.Zvalue)
            {
                HelpText = TextStrings.layercontainsvalues
            };
            _inputParam[2] = new DoubleParam(TextStrings.CellSize, 0, 0, double.MaxValue)
            {
                HelpText = TextStrings.Thecellsizeingeographicunits
            };
            _inputParam[3] = new DoubleParam(TextStrings.Power, 2, 1, double.MaxValue)
            {
                HelpText = TextStrings.Theinfluenceofdistance
            };
            _neighborhoodType = new List<string>
                                    {
                                        TextStrings.FixedDistance,
                                        TextStrings.FixedCount
                                    };
            _inputParam[4] = new ListParam(TextStrings.NeighborhoodType, _neighborhoodType, 0)
            {
                HelpText = TextStrings.Selectthetypeofneighborhood
            };
            _inputParam[5] = new IntParam(TextStrings.MinMaxnumberofpoints, 12, 0, int.MaxValue)
            {
                HelpText = TextStrings.FixedDistanceHelpText
            };
            _inputParam[6] = new DoubleParam(TextStrings.MinMaxdistance, 0, 0, double.MaxValue)
            {
                HelpText = TextStrings.FixedDistanceHelpText
            };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.Raster);
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Fires when one of the paramters value has beend changed, usually when a user changes a input or output Parameter value, this can be used to populate other Parameter default values.
        /// </summary>
        /// <param name="sender">The sender that fired the event.</param>
        public override void ParameterChanged(Parameter sender)
        {
            if (sender == _inputParam[0])
            {
                if (_inputParam[0].Value is FeatureSet fs && _inputParam[1] is ListParam lp)
                {
                    lp.ValueList.Clear();
                    for (int i = 0; i < fs.DataTable.Columns.Count; i++)
                    {
                        lp.ValueList.Add(fs.DataTable.Columns[i].ColumnName);
                    }

                    lp.Value = -1;
                }
            }
        }

        #endregion
    }
}
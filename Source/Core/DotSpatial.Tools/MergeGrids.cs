// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel.DataAnnotations;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using NetTopologySuite.Geometries;

namespace DotSpatial.Tools
{
    /// <summary>
    /// Tool that merges two raster layers.
    /// </summary>
    public class MergeGrids : Tool
    {
        #region Fields

        private Parameter[] _inputParam;

        private Parameter[] _outputParam;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeGrids"/> class.
        /// </summary>
        public MergeGrids()
        {
            Name = TextStrings.MergeRasterLayers;
            Category = TextStrings.Analysis;
            Description = TextStrings.MergeGridsDescription;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the help text to be displayed when no input field is selected.
        /// </summary>
        public string HelpText => TextStrings.MergeGridsDescription;

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
        /// <returns>Boolean, true if the merge is successful.</returns>
        public override bool Execute(ICancelProgressHandler cancelProgressHandler)
        {
            IRaster input1 = _inputParam[0].Value as IRaster;
            IRaster input2 = _inputParam[1].Value as IRaster;

            IRaster output = _outputParam[0].Value as IRaster;

            return Execute(input1, input2, output, cancelProgressHandler);
        }

        /// <summary>
        /// Executes the Erase Opaeration tool programmatically
        /// Ping deleted static for external testing 01/2010.
        /// </summary>
        /// <param name="input1">The first input raster.</param>
        /// <param name="input2">The second input raster.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the merge is successful.</returns>
        public bool Execute(IRaster input1, IRaster input2, IRaster output, ICancelProgressHandler cancelProgressHandler)
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }

            Extent envelope = UnionEnvelope(input1, input2);

            // Figures out which raster has smaller cells
            IRaster smallestCellRaster = input1.CellWidth < input2.CellWidth ? input1 : input2;

            // Given the envelope of the two rasters we calculate the number of columns / rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / smallestCellRaster.CellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / smallestCellRaster.CellHeight));

            // Determine what dataType the output will be. This ESRI source https://desktop.arcgis.com/en/arcmap/10.3/manage-data/raster-and-images/what-is-raster-data.htm
            // says that for a raster "Cell values can be either positive or negative, integer, or floating point. Integer values are best used to represent categorical (discrete)
            // data and floating-point values to represent continuous surfaces". this defintion will make us conclude that output type should depend on the type of the two input
            // rasters. i.e If either inputs is of type float then the type of the output should be float. If both are of type int then the output should be int. Similarly, if the
            // type of one of the rasters is double and the other is of type float then type of output should be double, and so on.
            Type outType = typeof(int); //default is integer
            if (input1.DataType == input2.DataType)
            {
                // if both types are the same then make the output type equal the type of the first raster
                outType = input1.DataType;
            }
            else if (true)
            {
                // otherwise determine which type is the minimum covariant type for best fit
                outType = GetClosestType(input1.DataType, input2.DataType);
            }

            // create output raster of type that can hold the data type of both input rasters
            //output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, typeof(int), new[] { string.Empty });
            output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, outType, new[] { string.Empty });
            RasterBounds bound = new(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            output.NoDataValue = input1.NoDataValue;

            int previous = 0;
            int max = output.Bounds.NumRows + 1;
            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    Coordinate cellCenter = output.CellToProj(i, j);
                    var v1 = input1.ProjToCell(cellCenter);
                    double val1;
                    if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                    {
                        val1 = input1.Value[v1.Row, v1.Column];
                    }
                    else
                    {
                        val1 = input1.NoDataValue;
                    }

                    var v2 = input2.ProjToCell(cellCenter);
                    double val2;
                    if (v2.Row <= input2.EndRow && v2.Column <= input2.EndColumn && v2.Row > -1 && v2.Column > -1)
                    {
                        val2 = input2.Value[v2.Row, v2.Column];
                    }
                    else
                    {
                        val2 = input2.NoDataValue;
                    }

                    if (val1 == input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        output.Value[i, j] = output.NoDataValue;
                    }
                    else if (val1 != input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        output.Value[i, j] = val1;
                    }
                    else if (val1 == input1.NoDataValue && val2 != input2.NoDataValue)
                    {
                        output.Value[i, j] = val2;
                    }
                    else
                    {
                        output.Value[i, j] = val1;
                    }

                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }

                int current = Convert.ToInt32(Math.Round(i * 100D / max));

                // only update when increment in persentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(current, current + TextStrings.progresscompleted);
                }

                previous = current;
            }

            // output = Temp;
            output.Save();
            return true;
        }

        /// <summary>
        /// The Parameter array should be populated with default values here.
        /// </summary>
        public override void Initialize()
        {
            _inputParam = new Parameter[2];
            _inputParam[0] = new RasterParam(TextStrings.input1Raster)
                                 {
                                     HelpText = TextStrings.InputFirstRaster
                                 };
            _inputParam[1] = new RasterParam(TextStrings.input2Raster)
                                 {
                                     HelpText = TextStrings.InputSecondRasterforMerging
                                 };

            _outputParam = new Parameter[2];
            _outputParam[0] = new RasterParam(TextStrings.OutputRaster)
                                  {
                                      HelpText = TextStrings.ResultRasterDirectory
                                  };
            _outputParam[1] = new BooleanParam(TextStrings.OutputParameter_AddToMap, TextStrings.OutputParameter_AddToMap_CheckboxText, true);
        }

        /// <summary>
        /// Execute the union region for output envelope.
        /// </summary>
        /// <param name="input1">the first input raster to union the envelope for.</param>
        /// <param name="input2">input second input raster to union the envelope for.</param>
        /// <returns>The combined envelope.</returns>
        private static Extent UnionEnvelope(IRaster input1, IRaster input2)
        {
            Extent e1 = input1.Bounds.Extent;
            Extent e2 = input2.Bounds.Extent;
            e1.ExpandToInclude(e2);
            return e1;
        }

        /// <summary>
        /// Determine the minimum covariant type for best fit between two types.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Type GetClosestType(Type a, Type b)
        {
            var t = a;

            while (a != null)
            {
                if (a.IsAssignableFrom(b))
                    return a;

                a = a.BaseType;
            }

            return null;
        }

        #endregion
    }
}
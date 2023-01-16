// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Executes the Merge Grid Operation tool programmatically.
        /// Ping deleted static for external testing 01/2010.
        /// The term 'Merge' here is equivalent to taking the non-empty value from each cell location. If both rasters have values
        /// for a location, then the first raster wins. You can think of this routine as filling in holes in raster1 from raster2.
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

            // Determine coverage for both input rasters.
            Extent envelope = UnionEnvelope(input1, input2);

            // Figures out which raster has smaller cells.
            IRaster smallestCellRaster = input1.CellWidth < input2.CellWidth ? input1 : input2;

            // Given the envelope of the two rasters we calculate the number of columns / rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / smallestCellRaster.CellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / smallestCellRaster.CellHeight));

            // Create output raster.
            output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, typeof(int), new[] { string.Empty });
            RasterBounds bound = new(noOfRow, noOfCol, envelope);
            output.Bounds = bound;

            // The NoDataValue value will be from the first raster.
            output.NoDataValue = input1.NoDataValue;

            // Cycle through the output raster cells, get the pixel value for the first raster, get the pixel value for the second raster,
            // and determine which value we should output.
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
        /// Executes the Merge Grid Operation tool programmatically.
        /// Ping deleted static for external testing 01/2010.
        /// The term 'Merge' here is equivalent to taking the non-empty value from each cell location. If both rasters have values
        /// for a location, then the first raster wins. You can think of this routine as filling in holes in raster1 from raster2.
        /// </summary>
        /// <param name="input1">The first input raster.</param>
        /// <param name="input2">The second input raster.</param>
        /// <param name="output">The output raster.</param>
        /// <param name="cancelProgressHandler">The progress handler.</param>
        /// <returns>Boolean, true if the merge is successful.</returns>
#pragma warning disable CA1822 // Member als statisch markieren
        public bool ExecuteNew(IRaster input1, IRaster input2, IRaster output, ICancelProgressHandler cancelProgressHandler)
#pragma warning restore CA1822 // Member als statisch markieren
        {
            // Validates the input and output data
            if (input1 == null || input2 == null || output == null)
            {
                return false;
            }

            // Determine coverage for both input rasters.
            Extent envelope = UnionEnvelope(input1, input2);

            // Figures out which raster has smaller cells.
            IRaster smallestCellRaster = input1.CellWidth < input2.CellWidth ? input1 : input2;

            // Given the envelope of the two rasters we calculate the number of columns / rows
            int noOfCol = Convert.ToInt32(Math.Abs(envelope.Width / smallestCellRaster.CellWidth));
            int noOfRow = Convert.ToInt32(Math.Abs(envelope.Height / smallestCellRaster.CellHeight));
            Debug.Print("noOfCol={0} noOfRow={1}", noOfCol, noOfRow);

            // Determine what dataType the output will be. This ESRI source https://desktop.arcgis.com/en/arcmap/10.3/manage-data/raster-and-images/what-is-raster-data.htm
            // says that for a raster "Cell values can be either positive or negative, integer, or floating point. Integer values are best used to represent categorical (discrete)
            // data and floating-point values to represent continuous surfaces". This defintion will make us conclude that output type should depend on the type of the two input
            // rasters. i.e If either inputs is of type float then the type of the output should be float. If both are of type int then the output should be int. Similarly, if the
            // type of one of the rasters is double and the other is of type float then type of output should be double, and so on. The routine GetOutputType will attempt to
            // determine which type is the best fit for both data types. ESRI has a list of various raster types here: https://pro.arcgis.com/en/pro-app/latest/help/data/imagery/supported-raster-dataset-file-formats.htm
            // todo: it would also make sense to allow the user to input an output type. an idea is to add a parameter to the Execute for OutDataType and if its null then that
            // would be considered an auto-determine, otherwise use what was passed in.
            Type outType = GetOutputType(input1.DataType, input2.DataType, output.FileType);

            // If a good output type cant be found then qwe default to an int, which is what it was before this PR. However, there is a good argument to make the default a double.
            // Hard to determine the default as there are so many combinations to consider.
            if (outType == null)
            {
                outType = typeof(int);
            }

            // Determine if the output type is a floating point type
            bool isOutTypFloatPnt = IsFloatingPoint(outType);
            Debug.Print("outType={0} isOutTypFloatPnt={1}", outType, isOutTypFloatPnt);

            // Create output raster of type that can hold the data type of both input rasters and also abide by the raster type possibilities.
            output = Raster.CreateRaster(output.Filename, string.Empty, noOfCol, noOfRow, 1, outType, new[] { string.Empty });
            output.Bounds = new RasterBounds(noOfRow, noOfCol, envelope);
            output.NoDataValue = input1.NoDataValue;

            int previous = 0;
            int max = output.Bounds.NumRows + 1;
            Debug.Print("output.Bounds.NumRows={0} output.Bounds.NumColumns={1}", output.Bounds.NumRows, output.Bounds.NumColumns);

            for (int i = 0; i < output.Bounds.NumRows; i++)
            {
                for (int j = 0; j < output.Bounds.NumColumns; j++)
                {
                    // Get the cell value from the first raster. Currently the cell value is defined as a double. At some point it should probably be the same type as the raster type.
                    Coordinate cellCenter = output.CellToProj(i, j);
                    RcIndex v1 = input1.ProjToCell(cellCenter);
                    double val1;

                    if (v1.Row <= input1.EndRow && v1.Column <= input1.EndColumn && v1.Row > -1 && v1.Column > -1)
                    {
                        val1 = input1.Value[v1.Row, v1.Column];
                    }
                    else
                    {
                        val1 = input1.NoDataValue;
                    }

                    // Get the cell value from the second raster. Currently the cell value is defined as a double. At some point it should probably be the same type as the raster type.
                    RcIndex v2 = input2.ProjToCell(cellCenter);
                    double val2;

                    if (v2.Row <= input2.EndRow && v2.Column <= input2.EndColumn && v2.Row > -1 && v2.Column > -1)
                    {
                        val2 = input2.Value[v2.Row, v2.Column];
                    }
                    else
                    {
                        val2 = input2.NoDataValue;
                    }

                    // Determine the resultant cell value. Currently the resultant value is ALWAYS a double. Maybe at some point we possibly need a value type equal to the raster output type.
                    double valRes;

                    if (val1 == input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        valRes = output.NoDataValue;
                    }
                    else if (val1 != input1.NoDataValue && val2 == input2.NoDataValue)
                    {
                        valRes = val1;
                    }
                    else if (val1 == input1.NoDataValue && val2 != input2.NoDataValue)
                    {
                        valRes = val2;
                    }
                    else
                    {
                        valRes = val1;
                    }

                    // Assign the resulant value to the output cell. It is the final value assignment. We must check if a non-finite value
                    // is being assigned to a type other than float or double. In other words, we can't assign a NaN to an Integer type.
                    if (double.IsFinite(valRes))
                    {
                        // value is finite so any type can hold it
                        output.Value[i, j] = valRes;
                        Debug.Print("@i={0} j={1} valRes={2}", i, j, valRes);
                    }
                    else
                    {
                        // Value is not finite so determine if the output type can hold it. If it can then assign the value to the output
                        // cell. If not then throw an exception.
                        if (isOutTypFloatPnt)
                        {
                            output.Value[i, j] = valRes;
                            Debug.Print("$i={0} j={1} valRes={2}", i, j, valRes);
                        }
                        else
                        {
                            Debug.Print("!i={0} j={1} valRes={2}", i, j, valRes);
                            string msg = string.Format("The raster output type of {0} can not hold a non-finite value of {1} at row={2}, col={3}.", outType, valRes, i, j);
                            throw new NotFiniteNumberException(msg);
                        }
                    }

                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                }

                int current = Convert.ToInt32(Math.Round(i * 100D / max));

                // only update when increment in percentage
                if (current > previous)
                {
                    cancelProgressHandler.Progress(current, current + TextStrings.progresscompleted);
                }

                previous = current;
            }

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
        /// Determine the best fit for the two input data types which also conforms to data types supported by the output raster
        /// file format. Note that merging two rasters may not be able to be saved in certain output raster file formats.
        /// </summary>
        /// <param name="typ1">Data <see cref="Type"/> of input 1</param>
        /// <param name="typ2">Data <see cref="Type"/> of input 2</param>
        /// <param name="outFileTyp">The <see cref="RasterFileType"/> of the output raster.</param>
        /// <returns>A data <see cref="Type"/>.</returns>
        public static Type GetOutputType(Type typ1, Type typ2, RasterFileType outFileTyp)
        {
            if (typ1 == null || typ2 == null)
            {
                // one or more null input types
                return null;
            }
            else
            {
                // The input types can be the same or they can be different but we still have to determine if the output raster file format can support the output data type.
                List<Type> suppTypes = GetSupportedTypes(outFileTyp);
                if (suppTypes == null || suppTypes.Count <= 0)
                {
                    // No supported types came back so the output raster file format is probabably a RasterFileType.Custom.
                    // In that case we try to intelligently determine what output data type best fits the two input data types.
                    Type outTyp = GetClosestType(typ1, typ2);
                    return outTyp;
                }
                else
                {
                    // We have supported types so the task now is to determine which of the supported types can hold the two input data types.
                    Type outTyp = GetClosestType(typ1, typ2, suppTypes);
                    return outTyp;
                }
            }
        }

        /// <summary>
        /// Gets the supported data types for a raster file format.
        /// </summary>
        /// <param name="rstFileType">A raster file type.</param>
        /// <returns>A list of <see cref="Type"/>.</returns>
        private static List<Type> GetSupportedTypes(RasterFileType rstFileType)
        {
            List<Type> suppTypes = new();

            // get the supported data types for a raster file format
            switch (rstFileType)
            {
                case RasterFileType.Ascii:
                    // 16-bit signed integer or 32-bit floating point
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(float));
                    break;
                case RasterFileType.Bil:
                    // 1-, 4-, and 8-bit unsigned integer
                    suppTypes.Add(typeof(byte));
                    break;
                case RasterFileType.Binary:
                    // From the MapWindow code on GitHub it states that BGD cannot be read by GDAL as it's a native format for 
                    // Utah State University. Sometimes it is referred to as a USU Binary Grid. The source code also seems to
                    // imply that the allowed data types are float, double, short and int as data types.
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(int));
                    suppTypes.Add(typeof(float));
                    suppTypes.Add(typeof(double));
                    break;
                case RasterFileType.Dted:
                    // 16-bit signed integer
                    suppTypes.Add(typeof(short));
                    break;
                case RasterFileType.Ecw:
                    // 8-bit unsigned integer
                    suppTypes.Add(typeof(byte));
                    break;
                case RasterFileType.Esri:
                    // The ESRI site only says it is integer or floating point. Since its an older format I'm gonna guess its the same as the ASCII types.
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(float));
                    break;
                case RasterFileType.Flt:
                    // 32-bit floating point
                    suppTypes.Add(typeof(float));
                    break;
                case RasterFileType.GeoTiff:
                    // 8-, 16-, and 32-bit unsigned/signed integer, 32-bit floating point, 64-bit complex
                    suppTypes.Add(typeof(byte));
                    suppTypes.Add(typeof(ushort));
                    suppTypes.Add(typeof(uint));
                    suppTypes.Add(typeof(sbyte));
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(int));
                    suppTypes.Add(typeof(float));
                    suppTypes.Add(typeof(long)); // not sure that this is 64-bit complex
                    break;
                case RasterFileType.MrSid:
                    // MrSID is  8-, and 16-bit unsigned integer
                    // MrSID LIDAR is 64-bit double precision
                    suppTypes.Add(typeof(byte));
                    suppTypes.Add(typeof(ushort));
                    suppTypes.Add(typeof(double));
                    break;
                case RasterFileType.Paux:
                    // 8-bit, 16-bit unsigned integer, 16-bit signed integer, and 32-bit floating point
                    suppTypes.Add(typeof(byte));
                    suppTypes.Add(typeof(ushort));
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(float));
                    break;
                case RasterFileType.PciDsk:
                    // 8-, and 16-bit unsigned integer, 16-bit signed integer, 32-bit floating point
                    suppTypes.Add(typeof(byte));
                    suppTypes.Add(typeof(ushort));
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(float));
                    break;
                case RasterFileType.Sdts:
                    // 16-bit signed integer or 32-bit floating point
                    suppTypes.Add(typeof(short));
                    suppTypes.Add(typeof(float));
                    break;
                case RasterFileType.Custom:
                    break;
                default:
                    break;
            }

            return suppTypes;
        }

        /// <summary>
        /// Determine the closest data type for the best fit between two types.
        /// </summary>
        /// <param name="typ1">Data type of input 1</param>
        /// <param name="typ2">Data type of input 2</param>
        /// <returns>
        /// </returns>
        public static Type GetClosestType(Type typ1, Type typ2)
        {
            if (CanConvertRange(typ1, typ2))
            {
                return typ2;
            }

            if (CanConvertRange(typ2, typ1))
            {
                return typ1;
            }

            return null;
        }

        /// <summary>
        /// Determine the closest data type for the best fit between two types that is in the list of supported data types.
        /// </summary>
        /// <param name="typ1">Data type of input 1</param>
        /// <param name="typ2">Data type of input 2</param>
        /// <param name="suppTypes">A list of Types that are supported.</param>
        /// <returns>
        /// </returns>
        private static Type GetClosestType(Type typ1, Type typ2, List<Type> suppTypes)
        {
            // We have a list of supported types so we know the result has to be one of these types. So we cycle through all the
            // supported types and determine if both input types can be stored. If they can then we have a result.
            foreach (Type suppTyp in suppTypes)
            {
                if (CanConvertRange(typ1, suppTyp) && CanConvertRange(typ2, suppTyp))
                {
                    return suppTyp;
                }
            }

            return null;
        }

        /// <summary>
        /// A crude method to determine if a full range from one type can be cast to another type.
        /// </summary>
        /// <param name="typFrom"></param>
        /// <param name="typTo"></param>
        /// <returns></returns>
        private static bool CanConvertRange(Type typFrom, Type typTo)
        {
            try
            {
                // basic check to make sure that both types are not null
                if (typFrom == null || typTo == null)
                {
                    return false;
                }

                // basic check to make sure that both types are value types
                if (!typFrom.IsValueType || !typTo.IsValueType)
                {
                    return false;
                }

                // value types must have a min and a max value so get them from typFrom.
                object valFromMin = typFrom.GetField("MinValue").GetValue(null);
                object valFromMax = typFrom.GetField("MaxValue").GetValue(null);

                // convert the value types to typTo. if they do not conform, then an exception will be thrown.
                object valToMin = Convert.ChangeType(valFromMin, typTo);
                object valToMax = Convert.ChangeType(valFromMax, typTo);

                return true;
            }
            catch (Exception ex)
            {
                //Debug.Print("***CanConvertRange error: typFrom={0} typTo={1}", typFrom, typTo);
                //Debug.Print(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Determines if a type is a floating point type.
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://learn.microsoft.com/en-us/dotnet/api/system.valuetype?view=net-7.0
        /// https://stackoverflow.com/questions/983030/type-checking-typeof-gettype-or-is
        /// </remarks>
        public static bool IsFloatingPoint(Type typ)
        {
            return typ == typeof(float) | typ == typeof(double) | typ == typeof(decimal);
        }

        #endregion
    }
}
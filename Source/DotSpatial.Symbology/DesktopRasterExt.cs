// *******************************************************************************************************
// Product: DotSpatial.Symbology.DesktopRasterExt.cs
// Description:  Methods for draw rasters.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
// *******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DotSpatial.Data;
using DotSpatial.NTSExtension;

namespace DotSpatial.Symbology
{
    public static class DesktopRasterExt
    {
        #region CreateHillShade

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.
        /// This should be a little faster since we are accessing the Data field directly instead of working
        /// through a value parameter.
        /// </summary>
        /// <param name="raster">The raster to create the hillshade from.</param>
        /// <param name="shadedRelief">An implementation of IShadedRelief describing how the hillshade should be created.</param>
        /// <param name="progressHandler">An implementation of IProgressHandler for progress messages</param>
        public static float[][] CreateHillShade(this IRaster raster, IShadedRelief shadedRelief, IProgressHandler progressHandler = null)
        {
            if (progressHandler == null) progressHandler = raster.ProgressHandler;
            var pm = new ProgressMeter(progressHandler, SymbologyMessageStrings.DesktopRasterExt_CreatingShadedRelief, raster.NumRows);

            Func<int, int, double> getValue;
            if (raster.DataType == typeof(int))
            {
                var r = raster.ToRaster<int>();
                getValue = (row, col) => r.Data[row][col];
            }
            else if (raster.DataType == typeof(float))
            {
                var r = raster.ToRaster<float>();
                getValue = (row, col) => r.Data[row][col];
            }
            else if (raster.DataType == typeof(short))
            {
                var r = raster.ToRaster<short>();
                getValue = (row, col) => r.Data[row][col];
            }
            else if (raster.DataType == typeof(byte))
            {
                var r = raster.ToRaster<byte>();
                getValue = (row, col) => r.Data[row][col];
            }
            else if (raster.DataType == typeof(double))
            {
                var r = raster.ToRaster<double>();
                getValue = (row, col) => r.Data[row][col];
            }
            else
            {
                getValue = (row, col) => raster.Value[row, col];
            }


            return CreateHillShadeT(raster, getValue, shadedRelief, pm);
        }

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.
        /// This should be a little faster since we are accessing the Data field directly instead of working
        /// through a value parameter.
        /// </summary>
        /// <param name="raster">The raster to create the hillshade from.</param>
        /// <param name="shadedRelief">An implementation of IShadedRelief describing how the hillshade should be created.</param>
        /// <param name="progressMeter">An implementation of IProgressHandler for progress messages</param>
        public static float[][] CreateHillShadeT<T>(this Raster<T> raster, IShadedRelief shadedRelief, ProgressMeter progressMeter) where T : IEquatable<T>, IComparable<T>
        {
            return CreateHillShadeT(raster, (row, col) => raster.Data[row][col], shadedRelief, progressMeter);
        }

        private static float[][] CreateHillShadeT<T>(this IRaster raster,
            Func<int, int, T> getValue,
            IShadedRelief shadedRelief, ProgressMeter progressMeter) where T : IEquatable<T>, IComparable<T>
        {
            if (!raster.IsInRam) return null;
            int numCols = raster.NumColumns;
            int numRows = raster.NumRows;
            var noData = Convert.ToSingle(raster.NoDataValue);
            float extrusion = shadedRelief.Extrusion;
            float elevationFactor = shadedRelief.ElevationFactor;
            float lightIntensity = shadedRelief.LightIntensity;
            float ambientIntensity = shadedRelief.AmbientIntensity;
            FloatVector3 lightDirection = shadedRelief.GetLightDirection();

            float[] aff = new float[6]; // affine coefficients converted to float format
            for (int i = 0; i < 6; i++)
            {
                aff[i] = Convert.ToSingle(raster.Bounds.AffineCoefficients[i]);
            }
            float[][] hillshade = new float[numRows][];
            if (progressMeter != null) progressMeter.BaseMessage = "Creating Shaded Relief";
            for (int row = 0; row < numRows; row++)
            {
                hillshade[row] = new float[numCols];

                for (int col = 0; col < numCols; col++)
                {
                    // 3D position vectors of three points to create a triangle.
                    FloatVector3 v1 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v2 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v3 = new FloatVector3(0f, 0f, 0f);

                    float val = Convert.ToSingle(getValue(row, col));
                    // Cannot compute polygon ... make the best guess)
                    if (col >= numCols - 1 || row <= 0)
                    {
                        if (col >= numCols - 1 && row <= 0)
                        {
                            v1.Z = val;
                            v2.Z = val;
                            v3.Z = val;
                        }
                        else if (col >= numCols - 1)
                        {
                            v1.Z = Convert.ToSingle(getValue(row, col - 1));        // 3 - 2
                            v2.Z = Convert.ToSingle(getValue(row - 1, col));        // | /
                            v3.Z = Convert.ToSingle(getValue(row - 1, col - 1));    // 1   *
                        }
                        else if (row <= 0)
                        {
                            v1.Z = Convert.ToSingle(getValue(row + 1, col));         //  3* 2
                            v2.Z = Convert.ToSingle(getValue(row, col + 1));         //  | /
                            v3.Z = val;                         //  1
                        }
                    }
                    else
                    {
                        v1.Z = val;                              //  3 - 2
                        v2.Z = Convert.ToSingle(getValue(row - 1, col + 1));          //  | /
                        v3.Z = Convert.ToSingle(getValue(row - 1, col));              //  1*
                    }

                    // Test for no-data values and don't calculate hillshade in that case
                    if (v1.Z == noData || v2.Z == noData || v3.Z == noData)
                    {
                        hillshade[row][col] = -1; // should never be negative otherwise.
                        continue;
                    }
                    // Apply the Conversion Factor to put elevation into the same range as lat/lon
                    v1.Z = v1.Z * elevationFactor * extrusion;
                    v2.Z = v2.Z * elevationFactor * extrusion;
                    v3.Z = v3.Z * elevationFactor * extrusion;

                    // Complete the vectors using the latitude/longitude coordinates
                    v1.X = aff[0] + aff[1] * col + aff[2] * row;
                    v1.Y = aff[3] + aff[4] * col + aff[5] * row;

                    v2.X = aff[0] + aff[1] * (col + 1) + aff[2] * (row + 1);
                    v2.Y = aff[3] + aff[4] * (col + 1) + aff[5] * (row + 1);

                    v3.X = aff[0] + aff[1] * col + aff[2] * (row + 1);
                    v3.Y = aff[3] + aff[4] * col + aff[5] * (row + 1);

                    // We need two direction vectors in order to obtain a cross product
                    FloatVector3 dir2 = FloatVector3.Subtract(v2, v1); // points from 1 to 2
                    FloatVector3 dir3 = FloatVector3.Subtract(v3, v1); // points from 1 to 3

                    FloatVector3 cross = FloatVector3.CrossProduct(dir3, dir2); // right hand rule - cross direction should point into page... reflecting more if light direction is in the same direction

                    // Normalizing this vector ensures that this vector is a pure direction and won't affect the intensity
                    cross.Normalize();

                    // Hillshade now has an "intensity" modifier that should be applied to the R, G and B values of the color found at each pixel.
                    hillshade[row][col] = FloatVector3.Dot(cross, lightDirection) * lightIntensity + ambientIntensity;
                }
                if (progressMeter != null) progressMeter.Next();
            }
            // Setting this indicates that a hillshade has been created more recently than characteristics have been changed.
            shadedRelief.HasChanged = false;
            return hillshade;
        }

        #endregion

        #region DrawToBitmap


        /// <summary>
        /// Creates a bitmap from this raster using the specified rasterSymbolizer
        /// </summary>
        /// <param name="raster">The raster to draw to a bitmap</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use for assigning colors</param>
        /// <param name="bitmap">This must be an Format32bbpArgb bitmap that has already been saved to a file so that it exists.</param>
        /// <param name="progressHandler">The progress handler to use.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null</exception>
        public static void DrawToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler = null)
        {
            if (raster == null) throw new ArgumentNullException("raster");
            if (rasterSymbolizer == null) throw new ArgumentNullException("rasterSymbolizer");
            if (bitmap == null) throw new ArgumentNullException("bitmap");
            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            BitmapData bmpData;
            var rect = new Rectangle(0, 0, raster.NumColumns, raster.NumRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }
            catch (Exception)
            {
                // if they have not saved the bitmap yet, it can cause an exception
                var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }

            var numRows = raster.NumRows;
            var numColumns = raster.NumColumns;

            // Prepare progress meter
            if (progressHandler == null) progressHandler = raster.ProgressHandler;
            var pm = new ProgressMeter(progressHandler, "Drawing to Bitmap", numRows);
            if (numRows * numColumns < 100000) pm.StepPercent = 50;
            if (numRows * numColumns < 500000) pm.StepPercent = 10;
            if (numRows * numColumns < 1000000) pm.StepPercent = 5;

            DrawToBitmap(raster, rasterSymbolizer, bmpData.Scan0, bmpData.Stride, pm);
            bitmap.UnlockBits(bmpData);

            rasterSymbolizer.ColorSchemeHasUpdated = true;
        }

        /// <summary>
        /// Creates a bitmap from this raster using the specified rasterSymbolizer
        /// </summary>
        /// <param name="raster">The raster to draw to a bitmap</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use for assigning colors</param>
        /// <param name="rgbData">Byte values representing the ARGB image bytes</param>
        /// <param name="stride">The stride</param>
        /// <param name="pm">The progress meter to use.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null</exception>
        public static void DrawToBitmapT<T>(Raster<T> raster, IRasterSymbolizer rasterSymbolizer, byte[] rgbData, int stride, ProgressMeter pm)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            DrawToBitmapT(raster, GetNoData(raster), (row, col) => raster.Data[row][col],
                i => rgbData[i], (i, b) => rgbData[i] = b, rasterSymbolizer, stride, pm);

            if (rasterSymbolizer.IsSmoothed)
            {
                var mySmoother = new Smoother(stride, raster.NumColumns, raster.NumRows, rgbData, pm.ProgressHandler);
                mySmoother.Smooth();
            }
        }

        private static void DrawToBitmapT<T>(Raster<T> raster, IRasterSymbolizer rasterSymbolizer, IntPtr rgbData, int stride, ProgressMeter pm)
           where T : struct, IEquatable<T>, IComparable<T>
        {
            DrawToBitmapT(raster, GetNoData(raster), (row, col) => raster.Data[row][col],
               i => Marshal.ReadByte(rgbData, i), (i, b) => Marshal.WriteByte(rgbData, i, b), rasterSymbolizer, stride, pm);

            if (rasterSymbolizer.IsSmoothed)
            {
                var mySmoother = new Smoother(stride, raster.NumColumns, raster.NumRows, rgbData, pm.ProgressHandler);
                mySmoother.Smooth();
            }
        }

        public static void DrawToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, byte[] rgbData, int stride, ProgressMeter pm)
        {
            if (raster.DataType == typeof(int))
            {
                DrawToBitmapT(raster.ToRaster<int>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(float))
            {
                DrawToBitmapT(raster.ToRaster<float>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(short))
            {
                DrawToBitmapT(raster.ToRaster<short>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(byte))
            {
                DrawToBitmapT(raster.ToRaster<byte>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(double))
            {
                DrawToBitmapT(raster.ToRaster<double>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else
            {
                DrawToBitmapT(raster, raster.NoDataValue, (row, col) => raster.Value[row, col],
                     i => rgbData[i], (i, b) => rgbData[i] = b, rasterSymbolizer, stride, pm);

                if (rasterSymbolizer.IsSmoothed)
                {
                    var mySmoother = new Smoother(stride, raster.NumColumns, raster.NumRows, rgbData, pm.ProgressHandler);
                    mySmoother.Smooth();
                }
            }
        }

        private static void DrawToBitmap(IRaster raster, IRasterSymbolizer rasterSymbolizer, IntPtr rgbData, int stride, ProgressMeter pm)
        {
            if (raster.DataType == typeof(int))
            {
                DrawToBitmapT(raster.ToRaster<int>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(float))
            {
                DrawToBitmapT(raster.ToRaster<float>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(short))
            {
                DrawToBitmapT(raster.ToRaster<short>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(byte))
            {
                DrawToBitmapT(raster.ToRaster<byte>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else if (raster.DataType == typeof(double))
            {
                DrawToBitmapT(raster.ToRaster<double>(), rasterSymbolizer, rgbData, stride, pm);
            }
            else
            {
                DrawToBitmapT(raster, raster.NoDataValue, (row, col) => raster.Value[row, col],
                    i => Marshal.ReadByte(rgbData, i), (i, b) => Marshal.WriteByte(rgbData, i, b), rasterSymbolizer, stride, pm);

                if (rasterSymbolizer.IsSmoothed)
                {
                    var mySmoother = new Smoother(stride, raster.NumColumns, raster.NumRows, rgbData, pm.ProgressHandler);
                    mySmoother.Smooth();
                }
            }
        }

        private static void DrawToBitmapT<T>(IRaster raster, T noData, Func<int, int, T> getValue, Func<int, byte> getByte,
            Action<int, byte> setByte, IRasterSymbolizer rasterSymbolizer, int stride, ProgressMeter pm)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if (raster == null) throw new ArgumentNullException("raster");
            if (rasterSymbolizer == null) throw new ArgumentNullException("rasterSymbolizer");
            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            float[][] hillshade = null;
            if (rasterSymbolizer.ShadedRelief.IsUsed)
            {
                pm.BaseMessage = "Calculating Shaded Relief";
                hillshade = rasterSymbolizer.HillShade ?? raster.CreateHillShadeT(getValue, rasterSymbolizer.ShadedRelief, pm);
            }

            pm.BaseMessage = "Calculating Colors";
            var sets = GetColorSets<T>(rasterSymbolizer.Scheme.Categories);
            var noDataColor = Argb.FromColor(rasterSymbolizer.NoDataColor);
            for (int row = 0; row < raster.NumRows; row++)
            {
                for (int col = 0; col < raster.NumColumns; col++)
                {
                    var value = getValue(row, col);
                    Argb argb;
                    if (value.Equals(noData))
                    {
                        argb = noDataColor;
                    }
                    else
                    {
                        // Usually values are not random, so check neighboring previous cells for same color
                        int? srcOffset = null;
                        if (col > 0)
                        {
                            if (value.Equals(getValue(row, col - 1)))
                            {
                                srcOffset = Offset(row, col - 1, stride);
                            }
                        }
                        if (srcOffset == null && row > 0)
                        {
                            if (value.Equals(getValue(row - 1, col)))
                            {
                                srcOffset = Offset(row - 1, col, stride);
                            }
                        }
                        if (srcOffset != null)
                        {
                            argb = new Argb(getByte((int)srcOffset + 3),
                                getByte((int)srcOffset + 2),
                                getByte((int)srcOffset + 1),
                                getByte((int)srcOffset));
                        }
                        else
                        {
                            argb = GetColor(sets, value);
                        }
                    }

                    if (hillshade != null)
                    {
                        if (hillshade[row][col] == -1 || float.IsNaN(hillshade[row][col]))
                        {
                            argb = new Argb(argb.A, noDataColor.R, noDataColor.G, noDataColor.B);
                        }
                        else
                        {
                            var red = (int)(argb.R * hillshade[row][col]);
                            var green = (int)(argb.G * hillshade[row][col]);
                            var blue = (int)(argb.B * hillshade[row][col]);
                            argb = new Argb(argb.A, red, green, blue);
                        }
                    }

                    var offset = Offset(row, col, stride);
                    setByte(offset, argb.B);
                    setByte(offset + 1, argb.G);
                    setByte(offset + 2, argb.R);
                    setByte(offset + 3, argb.A);
                }
                pm.Next();
            }
        }

        #endregion

        #region PaintColorSchemeToBitmap

        /// <summary>
        /// Creates a bitmap using only the colorscheme, even if a hillshade was specified.
        /// </summary>
        /// <param name="raster">The Raster containing values that need to be drawn to the bitmap as a color scheme.</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use.</param>
        /// <param name="bitmap">The bitmap to edit.  Ensure that this has been created and saved at least once.</param>
        /// <param name="progressHandler">An IProgressHandler implementation to receive progress updates.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null.</exception>
        public static void PaintColorSchemeToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
        {
            if (raster.DataType == typeof(int))
            {
                PaintColorSchemeToBitmapT(raster.ToRaster<int>(), rasterSymbolizer, bitmap, progressHandler);
            }
            else if (raster.DataType == typeof(float))
            {
                PaintColorSchemeToBitmapT(raster.ToRaster<float>(), rasterSymbolizer, bitmap, progressHandler);
            }
            else if (raster.DataType == typeof(short))
            {
                PaintColorSchemeToBitmapT(raster.ToRaster<short>(), rasterSymbolizer, bitmap, progressHandler);
            }
            else if (raster.DataType == typeof(byte))
            {
                PaintColorSchemeToBitmapT(raster.ToRaster<byte>(), rasterSymbolizer, bitmap, progressHandler);
            }
            else if (raster.DataType == typeof(double))
            {
                PaintColorSchemeToBitmapT(raster.ToRaster<double>(), rasterSymbolizer, bitmap, progressHandler);
            }
            else
            {
                PaintColorSchemeToBitmapT(raster, raster.NoDataValue, (row, col) => raster.Value[row, col], rasterSymbolizer, bitmap, progressHandler);
            }
        }

        /// <summary>
        /// Creates a bitmap using only the colorscheme, even if a hillshade was specified.
        /// </summary>
        /// <param name="raster">The Raster containing values that need to be drawn to the bitmap as a color scheme.</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use.</param>
        /// <param name="bitmap">The bitmap to edit.  Ensure that this has been created and saved at least once.</param>
        /// <param name="progressHandler">An IProgressHandler implementation to receive progress updates.</param>
        /// <exception cref="ArgumentNullException"><see cref="rasterSymbolizer"/> cannot be null, <see cref="raster"/> cannot be null, <see cref="bitmap"/> cannot be null</exception>
        public static void PaintColorSchemeToBitmapT<T>(this Raster<T> raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            PaintColorSchemeToBitmapT(raster, GetNoData(raster), (row, col) => raster.Data[row][col],
                rasterSymbolizer, bitmap, progressHandler);
        }

        private static void PaintColorSchemeToBitmapT<T>(this IRaster raster,
            T noData, Func<int, int, T> getValue,
            IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
           where T : struct, IEquatable<T>, IComparable<T>
        {
            if (raster == null) throw new ArgumentNullException("raster");
            if (rasterSymbolizer == null) throw new ArgumentNullException("rasterSymbolizer");
            if (bitmap == null) throw new ArgumentNullException("bitmap");
            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            BitmapData bmpData;
            var numRows = raster.NumRows;
            var numColumns = raster.NumColumns;
            var rect = new Rectangle(0, 0, numColumns, numRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }
            catch
            {
                var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.MemoryBmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }

            // Prepare progress meter
            var pm = new ProgressMeter(progressHandler, SymbologyMessageStrings.DesktopRasterExt_PaintingColorScheme, numRows);
            if (numRows * numColumns < 100000) pm.StepPercent = 50;
            if (numRows * numColumns < 500000) pm.StepPercent = 10;
            if (numRows * numColumns < 1000000) pm.StepPercent = 5;

            var sets = GetColorSets<T>(rasterSymbolizer.Scheme.Categories);
            var noDataColor = Argb.FromColor(rasterSymbolizer.NoDataColor);
            var alpha = Argb.ByteRange(Convert.ToInt32(rasterSymbolizer.Opacity * 255));
            var ptr = bmpData.Scan0;
            for (var row = 0; row < numRows; row++)
            {
                for (var col = 0; col < numColumns; col++)
                {
                    var val = getValue(row, col);
                    Argb argb;
                    if (val.Equals(noData))
                    {
                        argb = noDataColor;
                    }
                    else
                    {
                        // Usually values are not random, so check neighboring previous cells for same color
                        int? srcOffset = null;
                        if (col > 0)
                        {
                            if (val.Equals(getValue(row, col - 1)))
                            {
                                srcOffset = Offset(row, col - 1, bmpData.Stride);
                            }
                        }
                        if (srcOffset == null && row > 0)
                        {
                            if (val.Equals(getValue(row - 1, col)))
                            {
                                srcOffset = Offset(row - 1, col, bmpData.Stride);
                            }
                        }
                        if (srcOffset != null)
                        {
                            argb = new Argb(
                                Marshal.ReadByte(ptr, (int)srcOffset + 3),
                                Marshal.ReadByte(ptr, (int)srcOffset + 2),
                                Marshal.ReadByte(ptr, (int)srcOffset + 1),
                                Marshal.ReadByte(ptr, (int)srcOffset));
                        }
                        else
                        {
                            var color = GetColor(sets, val);
                            argb = new Argb(alpha, color.R, color.G, color.B);
                        }
                    }

                    var offset = Offset(row, col, bmpData.Stride);
                    Marshal.WriteByte(ptr, offset, argb.B);
                    Marshal.WriteByte(ptr, offset + 1, argb.G);
                    Marshal.WriteByte(ptr, offset + 2, argb.R);
                    Marshal.WriteByte(ptr, offset + 3, argb.A);
                }
                pm.CurrentValue = row;
            }
            pm.Reset();
            if (rasterSymbolizer.IsSmoothed)
            {
                var mySmoother = new Smoother(bmpData.Stride, bmpData.Width, bmpData.Height, bmpData.Scan0, progressHandler);
                mySmoother.Smooth();
            }
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
        }

        #endregion

        private static List<ColorSet<T>> GetColorSets<T>(IEnumerable<IColorCategory> categories) where T : struct, IComparable<T>
        {
            var result = new List<ColorSet<T>>();
            foreach (var c in categories)
            {
                var cs = new ColorSet<T>();
                Color high = c.HighColor;
                Color low = c.LowColor;
                cs.Color = Argb.FromColor(low);
                if (high != low)
                {
                    cs.GradientModel = c.GradientModel;
                    cs.Gradient = true;
                    cs.MinA = low.A;
                    cs.MinR = low.R;
                    cs.MinG = low.G;
                    cs.MinB = low.B;
                    cs.RangeA = high.A - cs.MinA;
                    cs.RangeR = high.R - cs.MinR;
                    cs.RangeG = high.G - cs.MinG;
                    cs.RangeB = high.B - cs.MinB;
                }

                cs.Max = Global.MaximumValue<T>();
                var testMax = Convert.ToDouble(cs.Max);
                cs.Min = Global.MinimumValue<T>();
                var testMin = Convert.ToDouble(cs.Min);

                if (c.Range.Maximum != null && c.Range.Maximum < testMax)
                {
                    if (c.Range.Maximum < testMin)
                        cs.Max = cs.Min;
                    else
                        cs.Max = (T)Convert.ChangeType(c.Range.Maximum.Value, typeof(T));
                }

                if (c.Range.Minimum != null && c.Range.Minimum > testMin)
                {
                    if (c.Range.Minimum > testMax)
                        cs.Min = Global.MaximumValue<T>();
                    else
                        cs.Min = (T)Convert.ChangeType(c.Range.Minimum.Value, typeof(T));
                }
                cs.MinInclusive = c.Range.MinIsInclusive;
                cs.MaxInclusive = c.Range.MaxIsInclusive;
                result.Add(cs);
            }
            // The normal order uses "overwrite" behavior, so that each color is drawn
            // if it qualifies until all the ranges are tested, overwriting previous.
            // This can be mimicked by going through the sets in reverse and choosing
            // the first that qualifies.  For lots of color ranges, opting out of
            // a large portion of the range testing should be faster.
            result.Reverse();
            return result;
        }

        private static Argb GetColor<T>(IEnumerable<ColorSet<T>> sets, T value) where T : struct, IComparable<T>
        {
            foreach (var set in sets)
            {
                if (set.Contains(value))
                {
                    if (!set.Gradient) return set.Color;
                    if (set.Min == null || set.Max == null) return set.Color;

                    double lowVal = Convert.ToDouble(set.Min.Value);
                    double range = Math.Abs(Convert.ToDouble(set.Max.Value) - lowVal);
                    double p = 0; // the portion of the range, where 0 is LowValue & 1 is HighValue
                    double ht;
                    double dVal = Convert.ToDouble(value);
                    switch (set.GradientModel)
                    {
                        case GradientModel.Linear:
                            p = (dVal - lowVal) / range;
                            break;
                        case GradientModel.Exponential:
                            ht = dVal;
                            if (ht < 1) ht = 1.0;
                            if (range > 1)
                                p = (Math.Pow(ht - lowVal, 2) / Math.Pow(range, 2));
                            else
                                return set.Color;
                            break;
                        case GradientModel.Logarithmic:
                            ht = dVal;
                            if (ht < 1) ht = 1.0;
                            if (range > 1.0 && ht - lowVal > 1.0)
                                p = Math.Log(ht - lowVal) / Math.Log(range);
                            else
                                return set.Color;
                            break;
                    }

                    return new Argb(
                        set.MinA + (int)(set.RangeA * p),
                        set.MinR + (int)(set.RangeR * p),
                        set.MinG + (int)(set.RangeG * p),
                        set.MinB + (int)(set.RangeB * p));
                }
            }
            return Argb.FromColor(Color.Transparent);
        }

        private static T GetNoData<T>(Raster<T> raster) where T : IEquatable<T>, IComparable<T>
        {
            // Get nodata value.
            var noData = default(T);
            try
            {
                noData = (T)Convert.ChangeType(raster.NoDataValue, typeof(T));
            }
            catch (OverflowException)
            {
                // For whatever reason, GDAL occasionally is reporting noDataValues
                // That will not fit in the specified band type. Is this due to a
                // malformed GeoTiff file?
                // http://dotspatial.codeplex.com/workitem/343
                Trace.WriteLine("OverflowException while getting NoDataValue");
            }
            return noData;
        }

        private static int Offset(int row, int col, int stride)
        {
            return row * stride + col * 4;
        }

        /// <summary>
        /// Obtains an set of unique values.  If there are more than maxCount values, the process stops and overMaxCount is set to true.
        /// </summary>
        /// <param name="raster">the raster to obtain the unique values from.</param>
        /// <param name="maxCount">An integer specifying the maximum number of values to add to the list of unique values</param>
        /// <param name="overMaxCount">A boolean that will be true if the process was halted prematurely.</param>
        /// <returns>A set of doubles representing the independant values.</returns>
        public static ISet<double> GetUniqueValues(this IRaster raster, int maxCount, out bool overMaxCount)
        {
            overMaxCount = false;
            var result = new HashSet<double>();

            var totalPossibleCount = int.MaxValue;

            // Optimization for integer types
            if (raster.DataType == typeof(byte) ||
                raster.DataType == typeof(int) ||
                raster.DataType == typeof(sbyte) ||
                raster.DataType == typeof(uint) ||
                raster.DataType == typeof(short) ||
                raster.DataType == typeof(ushort))
            {
                totalPossibleCount = (int)(raster.Maximum - raster.Minimum + 1);
            }

            // NumRows and NumColumns - virtual properties, so copy them local variables for faster access
            var numRows = raster.NumRows;
            var numCols = raster.NumColumns;
            var valueGrid = raster.Value;

            for (var row = 0; row < numRows; row++)
                for (var col = 0; col < numCols; col++)
                {
                    double val = valueGrid[row, col];
                    if (result.Add(val))
                    {
                        if (result.Count > maxCount)
                        {
                            overMaxCount = true;
                            goto fin;
                        }
                        if (result.Count == totalPossibleCount)
                            goto fin;
                    }
                }
        fin:
            return result;
        }

        /// <summary>
        /// This will sample randomly from the raster, preventing duplicates.
        /// If the sampleSize is larger than this raster, this returns all of the
        /// values from the raster.  If a "Sample" has been prefetched and stored
        /// in the Sample array, then this will return that.
        /// </summary>
        /// <param name="raster"></param>
        /// <param name="sampleSize"></param>
        /// <returns></returns>
        public static List<double> GetRandomValues(this IRaster raster, int sampleSize)
        {
            if (raster.Sample != null) return raster.Sample.ToList();
            int numRows = raster.NumRows;
            int numCols = raster.NumColumns;
            List<double> result = new List<double>();
            double noData = raster.NoDataValue;
            if (numRows * numCols < sampleSize)
            {
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        double val = raster.Value[row, col];
                        if (val != noData) result.Add(raster.Value[row, col]);
                    }
                }
                return result;
            }
            Random rnd = new Random(DateTime.Now.Millisecond);
            if (numRows * (long)numCols < (long)sampleSize * 5 && numRows * (long)numCols < int.MaxValue)
            {
                // When the raster is only just barely larger than the sample size,
                // we want to prevent lots of repeat guesses that fail (hit the same previously sampled values).
                // We create a copy of all the values and sample from this reservoir while removing sampled values.
                List<double> resi = new List<double>();
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {
                        double val = raster.Value[row, col];
                        if (val != noData) resi.Add(val);
                    }
                }
                //int count = numRows * numCols; //this could failed if there's lot of noDataValues
                long longcount = raster.NumValueCells;
                int count = numRows * numCols;
                if (count < int.MaxValue)
                    count = (int)longcount;

                for (int i = 0; i < sampleSize; i++)
                {
                    if (resi.Count == 0) break;

                    int indx = rnd.Next(count);
                    result.Add(resi[indx]);
                    resi.RemoveAt(indx);
                    count--;
                }
                raster.Sample = result;
                return result;
            }

            // Use a HashSet here, because it has O(1) lookup for preventing duplicates
            HashSet<long> exclusiveResults = new HashSet<long>();
            int remaining = sampleSize;
            while (remaining > 0)
            {
                int row = rnd.Next(numRows);
                int col = rnd.Next(numCols);
                long index = row * numCols + col;
                if (exclusiveResults.Contains(index)) continue;
                exclusiveResults.Add(index);
                remaining--;
            }
            // Sorting is O(n ln(n)), but sorting once is better than using a SortedSet for previous lookups.
            List<long> sorted = exclusiveResults.ToList();
            sorted.Sort();

            // Sorted values are much faster to read than reading values in at random, since the file actually
            // is reading in a whole line at a time.  If we can get more than one value from a line, then that
            // is better than getting one value, discarding the cache and then comming back later for the value
            // next to it.
            result = raster.GetValues(sorted);

            raster.Sample = result;
            return result;
        }

        #region HelperClass: ColorSet

        private class ColorSet<T>
            where T : struct, IComparable<T>
        {
            public Argb Color; //  for non bivalue case.
            public bool Gradient;
            public GradientModel GradientModel;
            public T? Max;
            public bool MaxInclusive;

            public T? Min;
            public int MinA;
            public int MinB;
            public int MinG;
            public bool MinInclusive;
            public int MinR;

            public int RangeA;
            public int RangeB;
            public int RangeG;
            public int RangeR;

            public bool Contains(T value)
            {
                // Checking for nulls
                if (Max == null && Min == null) return true;
                if (Min == null)
                    return MaxInclusive ? value.CompareTo(Max.Value) <= 0 : value.CompareTo(Max.Value) < 0;
                if (Max == null)
                    return MinInclusive ? value.CompareTo(Min.Value) >= 0 : value.CompareTo(Min.Value) > 0;

                // Normal checking
                double cMax = value.CompareTo(Max.Value);
                if (cMax > 0 || (!MaxInclusive && cMax == 0)) return false; //value bigger than max or max excluded

                double cMin = value.CompareTo(Min.Value);
                if (cMin < 0 || (cMin == 0 && !MinInclusive)) return false; //value smaller than min or min excluded

                return true;
            }
        }

        #endregion
    }
}
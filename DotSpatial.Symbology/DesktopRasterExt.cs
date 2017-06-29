// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/12/2009 5:30:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A new model, now that we support 3.5 framework and extension methods that are essentially
    /// derived characteristics away from the IRaster interface, essentially reducing it
    /// to the simplest interface possible for future implementers, while extending the most
    /// easy-to-find functionality to the users.
    /// </summary>
    public static class DesktopRasterExt
    {
        #region Methods

        #region Symbology

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.
        /// This uses the progress handler defined on this raster.
        /// </summary>
        /// <param name="raster">The raster to create hillshade information for</param>
        /// <param name="shadedRelief">An implementation of IShadedRelief describing how the hillshade should be created.</param>
        public static float[][] CreateHillShade(this IRaster raster, IShadedRelief shadedRelief)
        {
            return CreateHillShade(raster, shadedRelief, raster.ProgressHandler);
        }

        /// <summary>
        /// Create Hillshade of values ranging from 0 to 1, or -1 for no-data regions.
        /// This should be a little faster since we are accessing the Data field directly instead of working
        /// through a value parameter.
        /// </summary>
        /// <param name="raster">The raster to create the hillshade from.</param>
        /// <param name="shadedRelief">An implementation of IShadedRelief describing how the hillshade should be created.</param>
        /// <param name="progressHandler">An implementation of IProgressHandler for progress messages</param>
        public static float[][] CreateHillShade(this IRaster raster, IShadedRelief shadedRelief, IProgressHandler progressHandler)
        {
            if (!raster.IsInRam) return null;
            int numCols = raster.NumColumns;
            int numRows = raster.NumRows;
            double noData = raster.NoDataValue;
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

            ProgressMeter pm = new ProgressMeter(progressHandler, SymbologyMessageStrings.DesktopRasterExt_CreatingShadedRelief, numRows);
            for (int row = 0; row < numRows; row++)
            {
                hillshade[row] = new float[numCols];

                for (int col = 0; col < numCols; col++)
                {
                    // 3D position vectors of three points to create a triangle.
                    FloatVector3 v1 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v2 = new FloatVector3(0f, 0f, 0f);
                    FloatVector3 v3 = new FloatVector3(0f, 0f, 0f);

                    double val = raster.Value[row, col];
                    // Cannot compute polygon ... make the best guess
                    if (col >= numCols - 1 || row <= 0)
                    {
                        if (col >= numCols - 1 && row <= 0)
                        {
                            v1.Z = (float)val;
                            v2.Z = (float)val;
                            v3.Z = (float)val;
                        }
                        else if (col >= numCols - 1)
                        {
                            v1.Z = (float)raster.Value[row, col - 1];        // 3 - 2
                            v2.Z = (float)raster.Value[row - 1, col];        // | /
                            v3.Z = (float)raster.Value[row - 1, col - 1];    // 1   *
                        }
                        else if (row <= 0)
                        {
                            v1.Z = (float)raster.Value[row + 1, col];         //  3* 2
                            v2.Z = (float)raster.Value[row, col + 1];         //  | /
                            v3.Z = (float)val;                         //  1
                        }
                    }
                    else
                    {
                        v1.Z = (float)val;                              //  3 - 2
                        v2.Z = (float)raster.Value[row - 1, col + 1];          //  | /
                        v3.Z = (float)raster.Value[row - 1, col];              //  1*
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
                pm.CurrentValue = row;
            }
            pm.Reset();
            // Setting this indicates that a hillshade has been created more recently than characteristics have been changed.
            shadedRelief.HasChanged = false;
            return hillshade;
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
            if (!raster.IsInRam) return null;
            int numCols = raster.NumColumns;
            int numRows = raster.NumRows;
            double noData = raster.NoDataValue;
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
            var values = raster.Data;

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

                    float val = Convert.ToSingle(values[row][col]);
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
                            v1.Z = Convert.ToSingle(values[row][col - 1]);        // 3 - 2
                            v2.Z = Convert.ToSingle(values[row - 1][col]);        // | /
                            v3.Z = Convert.ToSingle(values[row - 1][col - 1]);    // 1   *
                        }
                        else if (row <= 0)
                        {
                            v1.Z = Convert.ToSingle(values[row + 1][col]);         //  3* 2
                            v2.Z = Convert.ToSingle(values[row][col + 1]);         //  | /
                            v3.Z = val;                         //  1
                        }
                    }
                    else
                    {
                        v1.Z = val;                              //  3 - 2
                        v2.Z = Convert.ToSingle(values[row - 1][col + 1]);          //  | /
                        v3.Z = Convert.ToSingle(values[row - 1][col]);              //  1*
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

        /// <summary>
        /// Creates a bitmap from this raster using the specified rasterSymbolizer
        /// </summary>
        /// <param name="raster">The raster to draw to the bitmap based on the layout specified in the rasterSymbolizer</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use for assigning colors</param>
        /// <param name="bitmap">This must be an Format32bbpArgb bitmap that has already been saved to a file so that it exists.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null</exception>
        public static void DrawToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap)
        {
            DrawToBitmap(raster, rasterSymbolizer, bitmap, raster.ProgressHandler);
        }

        /// <summary>
        /// Creates a bitmap from this raster using the specified rasterSymbolizer
        /// </summary>
        /// <param name="raster">The raster to draw to a bitmap</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use for assigning colors</param>
        /// <param name="bitmap">This must be an Format32bbpArgb bitmap that has already been saved to a file so that it exists.</param>
        /// <param name="progressHandler">The progress handler to use.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null</exception>
        public static void DrawToBitmap(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
        {
            BitmapData bmpData;

            if (rasterSymbolizer == null)
            {
                throw new ArgumentNullException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            Rectangle rect = new Rectangle(0, 0, raster.NumColumns, raster.NumRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            catch (Exception)
            {
                // if they have not saved the bitmap yet, it can cause an exception
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                // any further exceptions should simply throw exceptions to allow easy debugging
            }

            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);
            ProgressMeter pm = new ProgressMeter(progressHandler, "Drawing to Bitmap", raster.NumRows);
            DrawToBitmap(raster, rasterSymbolizer, rgbData, bmpData.Stride, pm);
            // Copy the values back into the bitmap
            Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
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
                // use the slow version if we can't parse the data type.
                DrawToBitmapOld(raster, rasterSymbolizer, rgbData, stride, pm);
            }
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
        public static void DrawToBitmapOld(this IRaster raster, IRasterSymbolizer rasterSymbolizer, byte[] rgbData, int stride, ProgressMeter pm)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (rasterSymbolizer == null)
            {
                throw new ArgumentNullException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            bool useHillShade = false;
            float[][] hillshade = rasterSymbolizer.HillShade;
            if (rasterSymbolizer.ShadedRelief.IsUsed)
            {
                hillshade = rasterSymbolizer.HillShade ?? CreateHillShade(raster, rasterSymbolizer.ShadedRelief);

                useHillShade = true;
            }
            Color pixelColor;
            try
            {
                for (int row = 0; row < raster.NumRows; row++)
                {
                    for (int col = 0; col < raster.NumColumns; col++)
                    {
                        // use the colorbreaks to calculate the colors
                        double value = raster.Value[row, col];
                        if (value == raster.NoDataValue)
                        {
                            int off = row * stride + col * 4;
                            rgbData[off] = rasterSymbolizer.NoDataColor.B;
                            rgbData[off + 1] = rasterSymbolizer.NoDataColor.G;
                            rgbData[off + 2] = rasterSymbolizer.NoDataColor.R;
                            rgbData[off + 3] = rasterSymbolizer.NoDataColor.A;
                            continue;
                        }
                        pixelColor = rasterSymbolizer.GetColor(value);

                        // control transparency here
                        float alpha = rasterSymbolizer.Opacity * 255f;
                        if (alpha > 255f) alpha = 255f;
                        if (alpha < 0f) alpha = 0f;
                        byte a = Convert.ToByte(alpha);
                        byte g;
                        byte r;
                        byte b;
                        if (useHillShade && hillshade != null)
                        {
                            if (hillshade[row][col] == -1 || float.IsNaN(hillshade[row][col]))
                            {
                                pixelColor = rasterSymbolizer.NoDataColor;
                                r = pixelColor.R;
                                g = pixelColor.G;
                                b = pixelColor.B;
                            }
                            else
                            {
                                float red = pixelColor.R * hillshade[row][col];
                                float green = pixelColor.G * hillshade[row][col];
                                float blue = pixelColor.B * hillshade[row][col];
                                if (red > 255f) red = 255f;
                                if (green > 255f) green = 255f;
                                if (blue > 255f) blue = 255f;
                                if (red < 0f) red = 0f;
                                if (green < 0f) green = 0f;
                                if (blue < 0f) blue = 0f;
                                b = Convert.ToByte(blue);
                                r = Convert.ToByte(red);
                                g = Convert.ToByte(green);
                            }
                        }
                        else
                        {
                            r = pixelColor.R;
                            g = pixelColor.G;
                            b = pixelColor.B;
                        }

                        int offset = row * stride + col * 4;
                        rgbData[offset] = b;
                        rgbData[offset + 1] = g;
                        rgbData[offset + 2] = r;
                        rgbData[offset + 3] = a;
                    }
                    pm.Next();
                }
            }
            catch
            {
                Debug.WriteLine(" Unable to write data to raster.");
            }
            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother mySmoother = new Smoother(stride, raster.NumColumns, raster.NumRows, rgbData, pm.ProgressHandler);
                mySmoother.Smooth();
            }
            sw.Stop();
            Debug.WriteLine("SymbologyRasterExt_DrawToBitmap: " + sw.ElapsedMilliseconds + "milliseconds");
        }

        private static List<ColorSet<T>> GetColorSets<T>(IEnumerable<IColorCategory> categories) where T : struct, IComparable<T>
        {
            var result = new List<ColorSet<T>>();
            foreach (IColorCategory c in categories)
            {
                var cs = new ColorSet<T>();
                Color high = c.HighColor;
                Color low = c.LowColor;
                cs.Color = low;
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
                    {
                        cs.Max = cs.Min;
                        cs.Maximum = Convert.ToDouble(cs.Max);
                    }
                    else
                    {
                        cs.Max = (T) Convert.ChangeType(c.Range.Maximum.Value, typeof (T));
                        cs.Maximum = Convert.ToDouble(c.Range.Maximum.Value);
                    }
                }

                if (c.Range.Minimum != null && c.Range.Minimum > testMin)
                {
                    if (c.Range.Minimum > testMax)
                    {
                        cs.Min = Global.MaximumValue<T>();
                        cs.Minimum = Convert.ToDouble(cs.Min);
                    }
                    else
                    {
                        cs.Min = (T)Convert.ChangeType(c.Range.Minimum.Value, typeof(T));
                        cs.Minimum = Convert.ToDouble(c.Range.Minimum.Value);
                    }
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

        private static Color GetColor<T>(IEnumerable<ColorSet<T>> sets, T value) where T : struct, IComparable<T>, IEquatable<T>
        {
            foreach (var set in sets)
            {
                if (set.Contains(value))
                {
                    if (!set.Gradient) return set.Color;
                    if (set.Minimum == null || set.Maximum == null) return set.Color;

                    double lowVal = set.Minimum.Value;
                    double range = Math.Abs(set.Maximum.Value - lowVal);
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
                            {
                                p = (Math.Pow(ht - lowVal, 2) / Math.Pow(range, 2));
                            }
                            else
                            {
                                return set.Color;
                            }
                            break;
                        case GradientModel.Logarithmic:
                            ht = dVal;
                            if (ht < 1) ht = 1.0;
                            if (range > 1.0 && ht - lowVal > 1.0)
                            {
                                p = Math.Log(ht - lowVal) / Math.Log(range);
                            }
                            else
                            {
                                return set.Color;
                            }
                            break;
                    }

                    int alpha = ByteRange(set.MinA + (int)(set.RangeA * p));
                    int red = ByteRange(set.MinR + (int)(set.RangeR * p));
                    int green = ByteRange(set.MinG + (int)(set.RangeG * p));
                    int blue = ByteRange(set.MinB + (int)(set.RangeB * p));
                    return Color.FromArgb(alpha, red, green, blue);
                }
            }
            return Color.Transparent;
        }

        /// <summary>
        /// Returns an integer that ranges from 0 to 255.  If value is larger than 255, the value will be equal to 255.
        /// If the value is smaller than 255, it will be equal to 255.
        /// </summary>
        /// <param name="value">A Double value to convert.</param>
        /// <returns>An integer ranging from 0 to 255</returns>
        private static int ByteRange(int value)
        {
            if (value > 255) return 255;
            if (value < 0) return 0;
            return value;
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
        private static void DrawToBitmapT<T>(Raster<T> raster, IRasterSymbolizer rasterSymbolizer, byte[] rgbData, int stride, ProgressMeter pm) 
            where T : struct, IEquatable<T>, IComparable<T>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (rasterSymbolizer == null)
            {
                throw new ArgumentNullException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            bool useHillShade = false;
            float[][] hillshade = rasterSymbolizer.HillShade;
            if (rasterSymbolizer.ShadedRelief.IsUsed)
            {
                pm.BaseMessage = "Calculating Shaded Relief";
                hillshade = rasterSymbolizer.HillShade ?? CreateHillShadeT(raster, rasterSymbolizer.ShadedRelief, pm);
                useHillShade = true;
            }
            pm.BaseMessage = "Calculating Colors";
            Color pixelColor;
            T[][] values = raster.Data;
            List<ColorSet<T>> sets = GetColorSets<T>(rasterSymbolizer.Scheme.Categories);
            T noData = GetNoData(raster);
            try
            {
                for (int row = 0; row < raster.NumRows; row++)
                {
                    for (int col = 0; col < raster.NumColumns; col++)
                    {
                        T value = values[row][col];
                        if (value.Equals(noData))
                        {
                            int off = row * stride + col * 4;
                            rgbData[off] = rasterSymbolizer.NoDataColor.B;
                            rgbData[off + 1] = rasterSymbolizer.NoDataColor.G;
                            rgbData[off + 2] = rasterSymbolizer.NoDataColor.R;
                            rgbData[off + 3] = rasterSymbolizer.NoDataColor.A;
                            continue;
                        }
                        pixelColor = GetColor(sets, value);

                        byte a = pixelColor.A;

                        byte g;
                        byte r;
                        byte b;
                        if (useHillShade && hillshade != null)
                        {
                            if (hillshade[row][col] == -1 || float.IsNaN(hillshade[row][col]))
                            {
                                pixelColor = rasterSymbolizer.NoDataColor;
                                r = pixelColor.R;
                                g = pixelColor.G;
                                b = pixelColor.B;
                            }
                            else
                            {
                                float red = pixelColor.R * hillshade[row][col];
                                float green = pixelColor.G * hillshade[row][col];
                                float blue = pixelColor.B * hillshade[row][col];
                                if (red > 255f) red = 255f;
                                if (green > 255f) green = 255f;
                                if (blue > 255f) blue = 255f;
                                if (red < 0f) red = 0f;
                                if (green < 0f) green = 0f;
                                if (blue < 0f) blue = 0f;
                                b = Convert.ToByte(blue);
                                r = Convert.ToByte(red);
                                g = Convert.ToByte(green);
                            }
                        }
                        else
                        {
                            r = pixelColor.R;
                            g = pixelColor.G;
                            b = pixelColor.B;
                        }

                        int offset = row * stride + col * 4;
                        rgbData[offset] = b;
                        rgbData[offset + 1] = g;
                        rgbData[offset + 2] = r;
                        rgbData[offset + 3] = a;
                    }
                    pm.Next();
                }
            }
            catch
            {
                Debug.WriteLine(" Unable to write data to raster.");
            }
            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother mySmoother = new Smoother(stride, raster.NumColumns, raster.NumRows, rgbData, pm.ProgressHandler);
                mySmoother.Smooth();
            }
            sw.Stop();
            Debug.WriteLine("SymbologyRasterExt_DrawToBitmapT: " + sw.ElapsedMilliseconds + "milliseconds");
        }

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
                // use the slow version if we can't parse the data type.
                PaintColorSchemeToBitmapOld(raster, rasterSymbolizer, bitmap, progressHandler);
            }
        }

        /// <summary>
        /// Creates a bitmap using only the colorscheme, even if a hillshade was specified.
        /// </summary>
        /// <param name="raster">The Raster containing values that need to be drawn to the bitmap as a color scheme.</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use.</param>
        /// <param name="bitmap">The bitmap to edit.  Ensure that this has been created and saved at least once.</param>
        /// <param name="progressHandler">An IProgressHandler implementation to receive progress updates.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null.</exception>
        public static void PaintColorSchemeToBitmapOld(this IRaster raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler)
        {
            BitmapData bmpData;
            int numRows = raster.NumRows;
            int numColumns = raster.NumColumns;
            if (rasterSymbolizer == null)
            {
                throw new ArgumentNullException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            // Create a new Bitmap and use LockBits combined with Marshal.Copy to get an array of bytes to work with.

            Rectangle rect = new Rectangle(0, 0, numColumns, numRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            catch
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.MemoryBmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);

            Color pixelColor;
            ProgressMeter pm = new ProgressMeter(progressHandler, SymbologyMessageStrings.DesktopRasterExt_PaintingColorScheme, raster.NumRows);
            if (numRows * numColumns < 100000) pm.StepPercent = 50;
            if (numRows * numColumns < 500000) pm.StepPercent = 10;
            if (numRows * numColumns < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    if (raster.Value[row, col] == raster.NoDataValue)
                    {
                        int off = row * bmpData.Stride + col * 4;
                        rgbData[off] = rasterSymbolizer.NoDataColor.B;
                        rgbData[off + 1] = rasterSymbolizer.NoDataColor.G;
                        rgbData[off + 2] = rasterSymbolizer.NoDataColor.R;
                        rgbData[off + 3] = rasterSymbolizer.NoDataColor.A;
                        continue;
                    }
                    // use the colorbreaks to calculate the colors
                    pixelColor = rasterSymbolizer.GetColor(raster.Value[row, col]);

                    // control transparency here
                    int alpha = Convert.ToInt32(rasterSymbolizer.Opacity * 255);
                    if (alpha > 255) alpha = 255;
                    if (alpha < 0) alpha = 0;
                    byte a = (byte)alpha;

                    byte r = pixelColor.R;
                    byte g = pixelColor.G;
                    byte b = pixelColor.B;

                    int offset = row * bmpData.Stride + col * 4;
                    rgbData[offset] = b;
                    rgbData[offset + 1] = g;
                    rgbData[offset + 2] = r;
                    rgbData[offset + 3] = a;
                }
                pm.CurrentValue = row;
            }
            pm.Reset();
            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother mySmoother = new Smoother(bmpData.Stride, bmpData.Width, bmpData.Height, rgbData, progressHandler);
                mySmoother.Smooth();
            }

            // Copy the values back into the bitmap
            Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
        }

        private static T GetNoData<T>(Raster<T> raster) where T : IEquatable<T>, IComparable<T>
        {
            T noData;
            // Get nodata value.
            noData = default(T);
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
                Debug.WriteLine("OverflowException while getting NoDataValue");
            }
            return noData;
        }

        /// <summary>
        /// Creates a bitmap using only the colorscheme, even if a hillshade was specified.
        /// </summary>
        /// <param name="raster">The Raster containing values that need to be drawn to the bitmap as a color scheme.</param>
        /// <param name="rasterSymbolizer">The raster symbolizer to use.</param>
        /// <param name="bitmap">The bitmap to edit.  Ensure that this has been created and saved at least once.</param>
        /// <param name="progressHandler">An IProgressHandler implementation to receive progress updates.</param>
        /// <exception cref="ArgumentNullException">rasterSymbolizer cannot be null.</exception>
        public static void PaintColorSchemeToBitmapT<T>(this Raster<T> raster, IRasterSymbolizer rasterSymbolizer, Bitmap bitmap, IProgressHandler progressHandler) 
            where T : struct, IEquatable<T>, IComparable<T>
        {
            BitmapData bmpData;
            int numRows = raster.NumRows;
            int numColumns = raster.NumColumns;
            if (rasterSymbolizer == null)
            {
                throw new ArgumentNullException("rasterSymbolizer");
            }

            if (rasterSymbolizer.Scheme.Categories == null || rasterSymbolizer.Scheme.Categories.Count == 0) return;

            // Create a new Bitmap and use LockBits combined with Marshal.Copy to get an array of bytes to work with.

            Rectangle rect = new Rectangle(0, 0, numColumns, numRows);
            try
            {
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            catch
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.MemoryBmp);
                ms.Position = 0;
                bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            }
            int numBytes = bmpData.Stride * bmpData.Height;
            byte[] rgbData = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbData, 0, numBytes);
            T noData = GetNoData(raster);

            T[][] values = raster.Data;
            List<ColorSet<T>> sets = GetColorSets<T>(rasterSymbolizer.Scheme.Categories);
            Color pixelColor;
            ProgressMeter pm = new ProgressMeter(progressHandler, SymbologyMessageStrings.DesktopRasterExt_PaintingColorScheme, raster.NumRows);
            if (numRows * numColumns < 100000) pm.StepPercent = 50;
            if (numRows * numColumns < 500000) pm.StepPercent = 10;
            if (numRows * numColumns < 1000000) pm.StepPercent = 5;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    T val = values[row][col];
                    if (val.Equals(noData))
                    {
                        int off = row * bmpData.Stride + col * 4;
                        rgbData[off] = rasterSymbolizer.NoDataColor.B;
                        rgbData[off + 1] = rasterSymbolizer.NoDataColor.G;
                        rgbData[off + 2] = rasterSymbolizer.NoDataColor.R;
                        rgbData[off + 3] = rasterSymbolizer.NoDataColor.A;
                        continue;
                    }
                    pixelColor = GetColor(sets, val);

                    // control transparency here
                    int alpha = Convert.ToInt32(rasterSymbolizer.Opacity * 255);
                    if (alpha > 255) alpha = 255;
                    if (alpha < 0) alpha = 0;
                    byte a = (byte)alpha;

                    byte r = pixelColor.R;
                    byte g = pixelColor.G;
                    byte b = pixelColor.B;

                    int offset = row * bmpData.Stride + col * 4;
                    rgbData[offset] = b;
                    rgbData[offset + 1] = g;
                    rgbData[offset + 2] = r;
                    rgbData[offset + 3] = a;
                }
                pm.CurrentValue = row;
            }
            pm.Reset();
            if (rasterSymbolizer.IsSmoothed)
            {
                Smoother mySmoother = new Smoother(bmpData.Stride, bmpData.Width, bmpData.Height, rgbData, progressHandler);
                mySmoother.Smooth();
            }

            // Copy the values back into the bitmap
            Marshal.Copy(rgbData, 0, bmpData.Scan0, numBytes);
            bitmap.UnlockBits(bmpData);
            rasterSymbolizer.ColorSchemeHasUpdated = true;
            return;
        }

        private class ColorSet<T> 
            where T : struct, IComparable<T>
        {
            public Color Color; //  for non bivalue case.
            public bool Gradient;
            public GradientModel GradientModel;
            public T? Max;
            public bool MaxInclusive;
            public double? Maximum;
            public T? Min;
            public int MinA;
            public int MinB;
            public int MinG;
            public bool MinInclusive;
            public int MinR;
            public double? Minimum;
            public int RangeA;
            public int RangeB;
            public int RangeG;
            public int RangeR;

            public bool Contains(T value)
            {
                //Try to convert value to double for a more precise comparison with Maximum and Minimum.
                //If fail, compare cMax and cMin with value of same type.
                try
                {
                    double doublevalue = Convert.ToDouble(value);

                    // Checking for nulls
                    if (Maximum == null && Minimum == null) return true;
                    if (Minimum == null)
                    {
                        return MaxInclusive ? (doublevalue <= Maximum) : (doublevalue < Maximum);
                    }
                    if (Maximum == null)
                    {
                        return MinInclusive ? (doublevalue >= Minimum) : (doublevalue > Minimum);
                    }

                    // Normal checking
                    var cMax = doublevalue - Maximum.Value;
                    if (cMax > 0) return false;
                    if (!MaxInclusive && cMax == 0) return false;
                    var cMin = doublevalue - Minimum.Value;
                    if (cMin < 0) return false;
                    if (cMin == 0 && !MinInclusive) return false;
                    return true;
                }
                catch
                {
                    // Checking for nulls
                    if (Max == null && Min == null) return true;
                    if (Min == null)
                    {
                        return MaxInclusive ? value.CompareTo(Max.Value) <= 0 : value.CompareTo(Max.Value) < 0;
                    }
                    if (Max == null)
                    {
                        return MinInclusive ? value.CompareTo(Min.Value) >= 0 : value.CompareTo(Min.Value) > 0;
                    }

                    // Normal checking
                    double cMax = value.CompareTo(Max.Value);
                    if (cMax > 1) return false;
                    if (!MaxInclusive && cMax == 0) return false;
                    double cMin = value.CompareTo(Min.Value);
                    if (cMin < 1) return false;
                    if (cMin == 0 && !MinInclusive) return false;
                    return true;
                }
            }
        }

        #endregion

        #region Unique Values

        /// <summary>
        /// Obtains a list of unique values from the grid.
        /// </summary>
        /// <param name="raster">The IRaster to obtain unique values for</param>
        /// <returns>A list of double values, where no value is repeated.</returns>
        public static List<double> GetUniqueValues(this IRaster raster)
        {
            List<double> list = new List<double>();
            for (int row = 0; row < raster.NumRows; row++)
            {
                for (int col = 0; col < raster.NumColumns; col++)
                {
                    double val = raster.Value[row, col];
                    if (list.Contains(val) == false)
                    {
                        list.Add(val);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Obtains a list of unique values.  If there are more than maxCount values, the process stops and overMaxCount is set to true.
        /// </summary>
        /// <param name="raster">the raster to obtain the unique values from.</param>
        /// <param name="maxCount">An integer specifying the maximum number of values to add to the list of unique values</param>
        /// <param name="overMaxCount">A boolean that will be true if the process was halted prematurely.</param>
        /// <returns>A list of doubles representing the independant values.</returns>
        public static List<double> GetUniqueValues(this IRaster raster, int maxCount, out bool overMaxCount)
        {
            overMaxCount = false;
            List<double> list = new List<double>();
            for (int row = 0; row < raster.NumRows; row++)
            {
                for (int col = 0; col < raster.NumColumns; col++)
                {
                    double val = raster.Value[row, col];
                    if (list.Contains(val) == false)
                    {
                        list.Add(val);
                        if (list.Count > maxCount)
                        {
                            overMaxCount = true;
                            return list;
                        }
                    }
                }
            }

            return list;
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

            Stopwatch sw = new Stopwatch();
            sw.Start();

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

            sw.Stop();
            Debug.WriteLine("GetValues time: " + sw.ElapsedMilliseconds + " for " + sampleSize + " values.");
            raster.Sample = result;
            return result;
        }

        #endregion

        #endregion
    }
}
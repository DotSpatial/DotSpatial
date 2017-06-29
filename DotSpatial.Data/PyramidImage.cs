// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 10:01:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// The Pyramid Image is designed with the expectation that the image will be too big to access all at once.
    /// It is stored with multiple resolutions in a "mwi or DotSpatial Image" format.  It is raw bytes in argb order.
    /// The header content is stored in xml format.
    /// </summary>
    public class PyramidImage : ImageData
    {
        #region Private Variables

        private PyramidHeader _header;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PyramidImage
        /// </summary>
        public PyramidImage()
        {
        }

        /// <summary>
        /// Creates a new PyramidImage by reading in the header content for the specified fileName.
        /// </summary>
        /// <param name="fileName"></param>
        public PyramidImage(string fileName)
        {
            Filename = Path.ChangeExtension(fileName, ".mwi");
            if (File.Exists(fileName)) ReadHeader(fileName);
        }

        /// <summary>
        /// Creates a new Pyramid image, and uses the raster bounds to specify the number or rows and columns.
        /// No data is written at this time.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bounds"></param>
        public PyramidImage(string fileName, IRasterBounds bounds)
        {
            Filename = fileName;
            Bounds = bounds;
            CreateHeaders(bounds.NumRows, bounds.NumColumns, bounds.AffineCoefficients);
            Width = _header.ImageHeaders[0].NumColumns;
            Height = _header.ImageHeaders[0].NumRows;
        }

        #endregion

        #region Methods

        /// <summary>
        /// For big images this won't work, but this gets the 0 scale original image as a bitmap.
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetBitmap()
        {
            PyramidImageHeader ph = Header.ImageHeaders[0];
            Rectangle bnds = new Rectangle(0, 0, ph.NumColumns, ph.NumRows);
            byte[] data = ReadWindow(0, 0, ph.NumRows, ph.NumColumns, 0);
            Bitmap bmp = new Bitmap(ph.NumColumns, ph.NumRows);
            BitmapData bData = bmp.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(data, 0, bData.Scan0, data.Length);
            bmp.UnlockBits(bData);
            return bmp;
        }

        /// <summary>
        /// For big images the scale that is just one step larger than the specified window will be used.
        /// </summary>
        /// <param name="envelope"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public override Bitmap GetBitmap(Extent envelope, Rectangle window)
        {
            if (window.Width == 0 || window.Height == 0) return null;
            if (_header == null) return null;
            if (_header.ImageHeaders == null) return null;
            if (_header.ImageHeaders[0] == null) return null;

            Rectangle expWindow = window.ExpandBy(1);
            IEnvelope expEnvelope = envelope.ToEnvelope().Reproportion(window, expWindow);

            IEnvelope env = expEnvelope.Intersection(Bounds.Extent.ToEnvelope());
            if (env == null || env.IsNull || env.Height == 0 || env.Width == 0) return null;

            PyramidImageHeader he = _header.ImageHeaders[0];
            int scale;

            double cwa = expWindow.Width / expEnvelope.Width;
            double cha = expWindow.Height / expEnvelope.Height;

            for (scale = 0; scale < _header.ImageHeaders.Length; scale++)
            {
                PyramidImageHeader ph = _header.ImageHeaders[scale];

                if (cwa > ph.NumColumns / Bounds.Width || cha > ph.NumRows / Bounds.Height)
                {
                    if (scale > 0) scale -= 1;
                    break;
                }
                he = ph;
            }

            RasterBounds overviewBounds = new RasterBounds(he.NumRows, he.NumColumns, he.Affine);
            Rectangle r = overviewBounds.CellsContainingExtent(envelope);
            if (r.Width == 0 || r.Height == 0) return null;
            byte[] vals = ReadWindow(r.Y, r.X, r.Height, r.Width, scale);
            Bitmap bmp = new Bitmap(r.Width, r.Height);
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, r.Width, r.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            bmp.UnlockBits(bData);

            // Use the cell coordinates to determine the affine coefficients for the cells retrieved.
            double[] affine = new double[6];
            Array.Copy(he.Affine, affine, 6);
            affine[0] = affine[0] + r.X * affine[1] + r.Y * affine[2];
            affine[3] = affine[3] + r.X * affine[4] + r.Y * affine[5];

            if (window.Width == 0 || window.Height == 0)
            {
                return null;
            }

            Bitmap result = new Bitmap(window.Width, window.Height);
            Graphics g = Graphics.FromImage(result);

            //

            // Gets the scaling factor for converting from geographic to pixel coordinates
            double dx = (window.Width / envelope.Width);
            double dy = (window.Height / envelope.Height);

            double[] a = affine;

            // gets the affine scaling factors.
            float m11 = Convert.ToSingle(a[1] * dx);
            float m22 = Convert.ToSingle(a[5] * -dy);
            float m21 = Convert.ToSingle(a[2] * dx);
            float m12 = Convert.ToSingle(a[4] * -dy);
            float l = (float)(a[0] - .5 * (a[1] + a[2])); // Left of top left pixel
            float t = (float)(a[3] - .5 * (a[4] + a[5])); // top of top left pixel
            float xShift = (float)((l - envelope.MinX) * dx);
            float yShift = (float)((envelope.MaxY - t) * dy);
            g.Transform = new Matrix(m11, m12, m21, m22, xShift, yShift);
            g.PixelOffsetMode = PixelOffsetMode.Half;
            if (m11 > 1 || m22 > 1)
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
            }

            g.DrawImage(bmp, new PointF(0, 0));
            bmp.Dispose();
            g.Dispose();
            return result;
        }

        /// <inheritdoc/>
        public override void Open()
        {
            ReadHeader(Filename);
        }

        /// <summary>
        /// Creates the headers using the existing RasterBounds for this image
        /// </summary>
        public void CreateHeaders()
        {
            CreateHeaders(Bounds.NumRows, Bounds.NumColumns, Bounds.AffineCoefficients);
        }

        /// <summary>
        /// This takes an original image and calculates the header content for all the lower resolution tiles.
        /// This does not actually write the bytes for those images.
        /// </summary>
        /// <param name="originalImage">The raster bounds for the original image.</param>
        public void CreateHeaders(RasterBounds originalImage)
        {
            Bounds = originalImage;
            CreateHeaders(originalImage.NumRows, originalImage.NumColumns, originalImage.AffineCoefficients);
        }

        /// <summary>
        /// This takes an original image and calculates the header content for all the lower resolution tiles.
        /// This does not actually write the bytes for those images.
        /// </summary>
        /// <param name="numRows">The number of rows in the original image</param>
        /// <param name="numColumns">The number of columns in the original image</param>
        /// <param name="affineCoefficients">
        /// the array of doubles in ABCDEF order
        /// X' = A + Bx + Cy
        /// Y' = D + Ex + Fy
        /// </param>
        public void CreateHeaders(int numRows, int numColumns, double[] affineCoefficients)
        {
            _header = new PyramidHeader();
            List<PyramidImageHeader> headers = new List<PyramidImageHeader>();
            int scale = 0;
            long offset = 0;
            int nr = numRows;
            int nc = numColumns;
            while (nr > 2 && nc > 2)
            {
                PyramidImageHeader ph = new PyramidImageHeader();
                ph.SetAffine(affineCoefficients, scale);
                ph.SetNumRows(numRows, scale);
                ph.SetNumColumns(numColumns, scale);
                ph.Offset = offset;
                offset += (ph.NumRows * ph.NumColumns * 4);
                nr = nr / 2;
                nc = nc / 2;
                scale++;
                headers.Add(ph);
            }
            _header.ImageHeaders = headers.ToArray();
        }

        /// <summary>
        /// This assumes that the base image has been written to the file.  This will now attempt to calculate
        /// the down-sampled images.
        /// </summary>
        public void CreatePyramids2()
        {
            double count = _header.ImageHeaders[0].NumRows;
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Generating Pyramids", count);
            int prog = 0;
            for (int scale = 0; scale < _header.ImageHeaders.Length - 1; scale++)
            {
                PyramidImageHeader ph = _header.ImageHeaders[scale];
                int rows = ph.NumRows;
                int cols = ph.NumColumns;
                // Horizontal Blur Pass
                byte[] r1 = ReadWindow(0, 0, 1, cols, scale);
                byte[] r2 = ReadWindow(1, 0, 1, cols, scale);

                byte[] vals = Blur(null, r1, r2);
                vals = DownSample(vals);
                WriteWindow(vals, 0, 0, 1, cols / 2, scale + 1);
                prog++;
                pm.CurrentValue = prog;

                byte[] r3 = ReadWindow(2, 0, 1, cols, scale);
                vals = Blur(r1, r2, r3);
                vals = DownSample(vals);
                WriteWindow(vals, 1, 0, 1, cols / 2, scale + 1);
                prog++;
                pm.CurrentValue = prog;
                for (int row = 3; row < rows - 1; row++)
                {
                    r1 = r2;
                    r2 = r3;
                    r3 = ReadWindow(row, 0, 1, cols, scale);
                    prog++;
                    pm.CurrentValue = prog;
                    if (row % 2 == 1) continue;
                    vals = Blur(r1, r2, r3);
                    vals = DownSample(vals);
                    WriteWindow(vals, row / 2 - 1, 0, 1, cols / 2, scale + 1);
                }
                if ((rows - 1) % 2 == 0)
                {
                    vals = Blur(r2, r3, r2);
                    vals = DownSample(vals);
                    WriteWindow(vals, rows / 2 - 1, 0, 1, cols / 2, scale + 1);
                }

                prog++;
                pm.CurrentValue = prog;
            }
            pm.Reset();
        }

        /// <summary>
        /// This assumes that the base image has been written to the file.  This will now attempt to calculate
        /// the down-sampled images.
        /// </summary>
        public void CreatePyramids()
        {
            int w = _header.ImageHeaders[0].NumColumns;
            int h = _header.ImageHeaders[0].NumRows;
            int blockHeight = 32000000 / w;
            if (blockHeight > h) blockHeight = h;
            int numBlocks = (int)Math.Ceiling(h / (double)blockHeight);
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Generating Pyramids",
                                                 _header.ImageHeaders.Length * numBlocks);
            for (int block = 0; block < numBlocks; block++)
            {
                // Normally block height except for the lowest block which is usually smaller
                int bh = blockHeight;
                if (block == numBlocks - 1) bh = h - block * blockHeight;

                // Read a block of bytes into a bitmap
                byte[] vals = ReadWindow(block * blockHeight, 0, bh, w, 0);

                Bitmap bmp = new Bitmap(w, bh);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, w, bh), ImageLockMode.WriteOnly,
                                             PixelFormat.Format32bppArgb);
                Marshal.Copy(vals, 0, bd.Scan0, vals.Length);
                bmp.UnlockBits(bd);

                // cycle through the scales, and write the resulting smaller bitmap in an appropriate spot
                int sw = w; // scale width
                int sh = bh; // scale height
                int sbh = blockHeight;
                for (int scale = 1; scale < _header.ImageHeaders.Length - 1; scale++)
                {
                    sw = sw / 2;
                    sh = sh / 2;
                    sbh = sbh / 2;
                    if (sh == 0 || sw == 0)
                    {
                        break;
                    }
                    Bitmap subSet = new Bitmap(sw, sh);
                    Graphics g = Graphics.FromImage(subSet);
                    g.DrawImage(bmp, 0, 0, sw, sh);
                    bmp.Dispose(); // since we keep getting smaller, don't bother keeping the big image in memory any more.
                    bmp = subSet;  // keep the most recent image alive for making even smaller subsets.
                    g.Dispose();
                    BitmapData bdata = bmp.LockBits(new Rectangle(0, 0, sw, sh), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    byte[] res = new byte[sw * sh * 4];
                    Marshal.Copy(bdata.Scan0, res, 0, res.Length);
                    bmp.UnlockBits(bdata);
                    WriteWindow(res, sbh * block, 0, sh, sw, scale);
                    pm.CurrentValue = block * _header.ImageHeaders.Length + scale;
                }
                vals = null;
                bmp.Dispose();
            }
            pm.Reset();
        }

        /// <summary>
        /// downsamples this row by selecting every other byte
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static byte[] DownSample(byte[] row)
        {
            int len = row.Length / 2;
            byte[] result = new byte[len];
            for (int col = 0; col < len / 4; col++)
            {
                result[col * 4] = row[col * 8]; // A
                result[col * 4 + 1] = row[col * 8 + 1]; // R
                result[col * 4 + 2] = row[col * 8 + 2]; // G
                result[col * 4 + 3] = row[col * 8 + 3]; // B
            }
            return result;
        }

        /// <summary>
        /// This is normally weighted for calculations for the middle row.  If the top or bottom row
        /// is null, it will use mirror symetry by grabbing values from the other row instead.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="r3"></param>
        private static byte[] Blur(byte[] r1, byte[] r2, byte[] r3)
        {
            byte[] result = new byte[r2.Length];
            byte[] top = r1;
            byte[] bottom = r3;
            double[] temp = new double[r2.Length];
            if (top == null)
            {
                top = new byte[r3.Length];
                Array.Copy(r3, top, r3.Length);
            }
            if (bottom == null)
            {
                bottom = new byte[r1.Length];
                Array.Copy(r1, bottom, r1.Length);
            }

            // Convolve vertically first, storing output in temp
            for (int i = 0; i < r2.Length; i++)
            {
                temp[i] = bottom[i] * .25 + r2[i] * .5 + top[i] * .25;
            }
            // Convolve horiziontally second, only on the temp row
            for (int i = 0; i < r2.Length; i++)
            {
                result[i] = Blur(temp, i);
            }
            return result;
        }

        private static byte Blur(double[] values, int index)
        {
            double a = index < 4 ? values[index + 4] : values[index - 4];
            double b = values[index];
            double c = index < values.Length - 4 ? values[index + 4] : values[index - 4];
            return (byte)(a * .25 + b * .5 + c * .25);
        }

        /// <summary>
        /// This writes a window of byte values (ARGB order) to the file.  This assumes that the headers already exist.
        /// If the headers have not been created or the bounds extend beyond the header numRows and numColumns for the
        /// specified scale, this will throw an exception.
        /// </summary>
        /// <param name="startRow">The integer start row</param>
        /// <param name="startColumn">The integer start column</param>
        /// <param name="numRows">The integer number of rows in the window</param>
        /// <param name="numColumns">The integer number of columns in the window</param>
        /// <param name="scale">The integer scale.  0 is the original image.</param>
        /// <returns>The bytes created by this process</returns>
        /// <exception cref="PyramidUndefinedHeaderException">Occurs when attempting to write data before the headers are defined</exception>
        /// <exception cref="PyramidOutOfBoundsException">Occurs if the range specified is outside the bounds for the specified image scale</exception>
        public byte[] ReadWindow(int startRow, int startColumn, int numRows, int numColumns, int scale)
        {
            byte[] bytes = new byte[numRows * numColumns * 4];
            if (_header == null || _header.ImageHeaders.Length <= scale || _header.ImageHeaders[scale] == null)
            {
                throw new PyramidUndefinedHeaderException();
            }
            PyramidImageHeader ph = _header.ImageHeaders[scale];
            if (startRow < 0 || startColumn < 0 || numRows + startRow > ph.NumRows || numColumns + startColumn > ph.NumColumns)
            {
                throw new PyramidOutOfBoundsException();
            }

            if (startColumn == 0 && numColumns == ph.NumColumns)
            {
                // write all in one pass.
                FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read);

                fs.Seek(ph.Offset, SeekOrigin.Begin);
                fs.Seek((startRow * ph.NumColumns) * 4, SeekOrigin.Current);
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
            }
            else
            {
                // write all in one pass.
                FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read);
                fs.Seek(ph.Offset, SeekOrigin.Begin);
                fs.Seek((startRow * ph.NumColumns) * 4, SeekOrigin.Current);
                int before = startColumn * 4;
                int after = (ph.NumColumns - (startColumn + numColumns)) * 4;
                for (int row = startRow; row < startRow + numRows; row++)
                {
                    fs.Seek(before, SeekOrigin.Current);
                    fs.Read(bytes, (row - startRow) * numColumns * 4, numColumns * 4);
                    fs.Seek(after, SeekOrigin.Current);
                }
                fs.Close();
            }
            return bytes;
        }

        /// <summary>
        /// This writes a window of byte values (ARGB order) to the file.  This assumes that the headers already exist.
        /// If the headers have not been created or the bounds extend beyond the header numRows and numColumns for the
        /// specified scale, this will throw an exception.
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <param name="startRow">The integer start row</param>
        /// <param name="startColumn">The integer start column</param>
        /// <param name="numRows">The integer number of rows in the window</param>
        /// <param name="numColumns">The integer number of columns in the window</param>
        /// <param name="scale">The integer scale. 0 is the original image.</param>
        /// <exception cref="PyramidUndefinedHeaderException">Occurs when attempting to write data before the headers are defined</exception>
        /// <exception cref="PyramidOutOfBoundsException">Occurs if the range specified is outside the bounds for the specified image scale</exception>
        public void WriteWindow(byte[] bytes, int startRow, int startColumn, int numRows, int numColumns, int scale)
        {
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Saving Pyramid Values", numRows);
            WriteWindow(bytes, startRow, startColumn, numRows, numColumns, scale, pm);
            pm.Reset();
        }

        /// <summary>
        /// This writes a window of byte values (ARGB order) to the file.  This assumes that the headers already exist.
        /// If the headers have not been created or the bounds extend beyond the header numRows and numColumns for the
        /// specified scale, this will throw an exception.
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <param name="startRow">The integer start row</param>
        /// <param name="startColumn">The integer start column</param>
        /// <param name="numRows">The integer number of rows in the window</param>
        /// <param name="numColumns">The integer number of columns in the window</param>
        /// <param name="scale">The integer scale. 0 is the original image.</param>
        /// <param name="pm">The progress meter to advance by row.  Calls Next() for each row.</param>
        /// <exception cref="PyramidUndefinedHeaderException">Occurs when attempting to write data before the headers are defined</exception>
        /// <exception cref="PyramidOutOfBoundsException">Occurs if the range specified is outside the bounds for the specified image scale</exception>
        public void WriteWindow(byte[] bytes, int startRow, int startColumn, int numRows, int numColumns, int scale, ProgressMeter pm)
        {
            if (_header == null || _header.ImageHeaders.Length <= scale || _header.ImageHeaders[scale] == null)
            {
                throw new PyramidUndefinedHeaderException();
            }
            PyramidImageHeader ph = _header.ImageHeaders[scale];
            if (startRow < 0 || startColumn < 0 || numRows + startRow > ph.NumRows || numColumns + startColumn > ph.NumColumns)
            {
                throw new PyramidOutOfBoundsException();
            }

            if (startColumn == 0 && numColumns == ph.NumColumns)
            {
                // write all in one pass.
                FileStream fs = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Write);

                fs.Seek(ph.Offset, SeekOrigin.Begin);
                fs.Seek((startRow * ph.NumColumns) * 4, SeekOrigin.Current);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            else
            {
                // write one row at a time
                FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Write);
                fs.Seek(ph.Offset, SeekOrigin.Begin);
                fs.Seek((startRow * ph.NumColumns) * 4, SeekOrigin.Current);
                int before = startColumn * 4;
                int after = (ph.NumColumns - (startColumn + numColumns)) * 4;
                for (int row = startRow; row < startRow + numRows; row++)
                {
                    fs.Seek(before, SeekOrigin.Current);
                    fs.Write(bytes, (row - startRow) * numColumns * 4, numColumns * 4);
                    fs.Seek(after, SeekOrigin.Current);
                    pm.Next();
                }
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// Reads the header only from the specified mwi file.  The header is in xml format.
        /// This is a test.  We may have to jurry rig the thing to ensure it ignores the actual
        /// image content.
        /// </summary>
        /// <param name="fileName">Whether this is the mwi or mwh file, this reads the mwh file for the fileName.</param>
        public void ReadHeader(string fileName)
        {
            string header = Path.ChangeExtension(fileName, "mwh");
            XmlSerializer s = new XmlSerializer(typeof(PyramidHeader));
            TextReader r = new StreamReader(header);
            _header = (PyramidHeader)s.Deserialize(r);
            PyramidImageHeader ph = _header.ImageHeaders[0];
            Bounds = new RasterBounds(ph.NumRows, ph.NumColumns, ph.Affine);
            Width = _header.ImageHeaders[0].NumColumns;
            Height = _header.ImageHeaders[0].NumRows;
            r.Close();
        }

        /// <summary>
        /// Writes the header to the specified fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to write the header to.</param>
        public void WriteHeader(string fileName)
        {
            string header = Path.ChangeExtension(fileName, "mwh");
            if (File.Exists(header))
            {
                File.Delete(header);
            }
            XmlSerializer s = new XmlSerializer(typeof(PyramidHeader));
            TextWriter w = new StreamWriter(header, false);
            s.Serialize(w, _header);
            w.Close();
        }

        /// <summary>
        /// Gets a block of data directly, converted into a bitmap.  This is a general
        /// implementation, so assumes you are reading and writing to the 0 scale.
        /// This is always in ARGB format.
        /// </summary>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        /// <param name="xSize">The integer number of pixel columns in the block. </param>
        /// <param name="ySize">The integer number of pixel rows in the block.</param>
        /// <returns>A Bitmap that is xSize, ySize.</returns>
        public override Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize)
        {
            byte[] vals = ReadWindow(yOffset, xOffset, ySize, xSize, 0);
            Bitmap bmp = new Bitmap(xSize, ySize);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, xSize, ySize), ImageLockMode.WriteOnly,
                                         PixelFormat.Format32bppArgb);

            for (int row = 0; row < ySize; row++)
            {
                Marshal.Copy(vals, row * xSize * 4, bd.Scan0, xSize * 4);
            }
            bmp.UnlockBits(bd);
            return bmp;
        }

        /// <summary>
        /// Saves a bitmap of data as a continuous block into the specified location.  This
        /// is a general implementation, so assumes you are reading and writing to the 0 scale.
        /// This should be in ARGB pixel format or it will fail.
        /// </summary>
        /// <param name="value">The bitmap value to save.</param>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        public override void WriteBlock(Bitmap value, int xOffset, int yOffset)
        {
            byte[] vals = new byte[value.Width * value.Height * 4];

            BitmapData bd = value.LockBits(new Rectangle(0, 0, value.Width, value.Height), ImageLockMode.WriteOnly,
                                           PixelFormat.Format32bppArgb);

            for (int row = 0; row < value.Height; row++)
            {
                Marshal.Copy(bd.Scan0, vals, row * bd.Width * 4, bd.Width * 4);
            }
            value.UnlockBits(bd);
            WriteWindow(vals, yOffset, xOffset, value.Height, value.Width, 0);
        }

        /// <summary>
        /// Updates overviews.  In the case of a Pyramid image, this recalculates the values.
        /// </summary>
        public override void UpdateOverviews()
        {
            CreatePyramids();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pyramid image header
        /// </summary>
        public PyramidHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        #endregion
    }
}
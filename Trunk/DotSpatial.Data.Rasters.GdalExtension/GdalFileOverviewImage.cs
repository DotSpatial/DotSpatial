// ********************************************************************************************************
// Product Name: DotSpatial.Gdal
// Description:  This is a data extension for the System.Spatial framework.
// ********************************************************************************************************
// The contents of this file are subject to the Gnu Lesser General Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a plugin for MapWindow version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/18/2010 11:30:50 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |     Name          |    Date     |              Comments
// |-------------------|-------------|-------------------------------------------------------------------
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// GdalFileOverviewImage represents an image too large to fit in ram.
    /// </summary>
    public class GdalFileOverviewImage : ImageData
    {
        #region Private Variables

        //  private GdalImageSource _source;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GdalFileOverviewImage
        /// </summary>
        public GdalFileOverviewImage(string fileName)
        {
            //_source = new GdalImageSource(fileName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method checks the various overviews available and attempts to create an image that will work
        /// best.
        /// </summary>
        /// <param name="envelope"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public override Bitmap GetBitmap(Extent envelope, Rectangle window)
        {
            //if (window.Width == 0 || window.Height == 0) return null;

            //Rectangle expWindow = window.ExpandBy(1);
            //IEnvelope expEnvelope = envelope.Reproportion(window, expWindow);

            //IEnvelope env = expEnvelope.Intersection(Bounds.Envelope);
            //if (env == null || env.IsNull || env.Height == 0 || env.Width == 0) return null;

            //Size[] he = _header.ImageHeaders[0];
            //int scale;

            //double cwa = expWindow.Width / expEnvelope.Width;
            //double cha = expWindow.Height / expEnvelope.Height;

            //for (scale = 0; scale < _header.ImageHeaders.Length; scale++)
            //{
            //    PyramidImageHeader ph = _header.ImageHeaders[scale];

            //    if (cwa > ph.NumColumns / Bounds.Width || cha > ph.NumRows / Bounds.Height)
            //    {
            //        if (scale > 0) scale -= 1;
            //        break;
            //    }
            //    he = ph;
            //}

            //// Get the cell coordinates of the part of the source bitmap to read
            //int x = (int)((he.NumColumns / Bounds.Width) * (env.X - he.Affine[0]));
            //int y = (int)((he.NumRows / Bounds.Height) * (he.Affine[3] - env.Y));
            //int w = (int)((he.NumColumns / Bounds.Width) * env.Width);
            //int h = (int)((he.NumRows / Bounds.Height) * env.Height);
            //if (w == 0 || h == 0) return null;
            //byte[] vals = ReadWindow(y, x, h, w, scale);
            //Bitmap bmp = new Bitmap(w, h);
            //BitmapData bData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            //Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            //bmp.UnlockBits(bData);

            //// Use the cell coordinates to determine the affine coefficients for the cells retrieved.
            //double[] affine = new double[6];
            //Array.Copy(he.Affine, affine, 6);
            //affine[0] = affine[0] + x * affine[1] + y * affine[2];
            //affine[3] = affine[3] + x * affine[4] + y * affine[5];

            //Bitmap result = new Bitmap(window.Width, window.Height);
            //Graphics g = Graphics.FromImage(result);

            //double imageToWorldW = affine[1];
            //double imageToWorldH = affine[5];

            //float cw = (float)(imageToWorldW * (window.Width / envelope.Width)); // cell width
            //float ch = -(float)(imageToWorldH * (window.Height / envelope.Height)); // cell height
            ////float sx = cw * (float)(_worldFile.Affine[2] / _worldFile.Affine[1]);
            ////float sy = ch * (float)(_worldFile.Affine[4] / _worldFile.Affine[5]);
            //const float sx = 0;
            //const float sy = 0;
            //float l = (float)(affine[0]);
            //float t = (float)(affine[3]);
            //float dx = (float)((l - envelope.Minimum.X) * (window.Width / envelope.Width));
            //float dy = (float)((envelope.Maximum.Y - t) * (window.Height / envelope.Height));
            //g.Transform = new Matrix(cw, sx, sy, ch, dx, dy);
            //g.PixelOffsetMode = PixelOffsetMode.Half;
            //if (cw > 1 || ch > 1) g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //g.DrawImage(bmp, new PointF(0, 0));
            //bmp.Dispose();
            //g.Dispose();
            //return result;
            return null;
        }

        #endregion

        #region Properties

        #endregion
    }
}
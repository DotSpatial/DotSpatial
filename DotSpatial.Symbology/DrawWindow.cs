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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/4/2008 4:36:35 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A Draw Window is a special type of envelope that supports some basic transformations
    /// </summary>
    public class DrawWindow : Envelope
    {
        #region Private Variables

        private IEnvelope _geographicView;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DrawWindow, making the assumption that the map is in Geographic coordinates of decimal degrees
        /// </summary>
        public DrawWindow()
        {
            _geographicView = new Envelope(-180, 180, -90, 90);
        }

        /// <summary>
        /// Creates a new draw window with the specified coordinates
        /// </summary>
        /// <param name="x1">The first x-value.</param>
        /// <param name="x2">The second x-value.</param>
        /// <param name="y1">The first y-value.</param>
        /// <param name="y2">The second y-value.</param>
        /// <param name="z1">The first z-value.</param>
        /// <param name="z2">The second z-value.</param>
        public DrawWindow(double x1, double x2, double y1, double y2, double z1, double z2) :
            base(x1, x2, y1, y2, z1, z2)
        {
            _geographicView = new Envelope(x1, x2, y1, y2);
        }

        /// <summary>
        /// Constructs a new DrawWindow based on the specified IEnvelope.  The envelope becomes
        /// the GeographicView for this DrawWindow.
        /// </summary>
        /// <param name="env"></param>
        public DrawWindow(IEnvelope env)
            : base(env)
        {
            _geographicView = env.Copy();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Replaces the inherited Envelope copy in order to create a copy of the DrawWindow instead
        /// </summary>
        /// <returns></returns>
        public new DrawWindow Copy()
        {
            IEnvelope env = base.Copy();
            DrawWindow dw = new DrawWindow(env);
            dw.GeographicView = _geographicView;
            return dw;
        }

        /// <summary>
        /// Replaces the inherited clone in order to make a copy of the DrawWindow
        /// </summary>
        /// <returns></returns>
        public new object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// Converts two dimensions of the specified coordinate into a two dimensional PointF
        /// </summary>
        /// <param name="inX"></param>
        /// <param name="inY"></param>
        /// <returns>A PointF</returns>
        public PointF ProjToDrawWindow(double inX, double inY)
        {
            double x = inX - Minimum.X;
            double y = inY - Minimum.Y;
            x = x / base.Width;
            y = y / base.Height;
            return new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
        }

        /// <summary>
        /// Converts two dimensions of the specified coordinate into a two dimensional PointF
        /// </summary>
        /// <param name="inCoord">Any valid ICoordinate</param>
        /// <returns>A PointF</returns>
        public PointF ProjToDrawWindow(Coordinate inCoord)
        {
            double x = inCoord.X - Minimum.X;
            double y = inCoord.Y - Minimum.Y;
            x = x / base.Width;
            y = y / base.Height;
            return new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
        }

        /// <summary>
        /// Converts two dimensions of the specified coordinate into a two dimensional PointF.
        /// </summary>
        /// <param name="inCoords"></param>
        /// <returns></returns>
        public PointF[] ProjToDrawWindow(List<Coordinate> inCoords)
        {
            if (inCoords == null || inCoords.Count == 0) return null;
            double minX = Minimum.X;
            double minY = Minimum.Y;
            double w = base.Width;
            double h = base.Height;
            PointF[] result = new PointF[inCoords.Count];
            for (int i = 0; i < inCoords.Count; i++)
            {
                double x = inCoords[i].X - minX;
                double y = inCoords[i].Y - minY;
                x = x / w;
                y = y / h;
                result[i] = new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
            }
            return result;
        }

        /// <summary>
        /// Converts a PointF from the draw window back into the double precision data coordinate.
        /// </summary>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        public Coordinate DrawWindowToProj(PointF inPoint)
        {
            double x = Convert.ToDouble(inPoint);
            double y = Convert.ToDouble(inPoint);
            x = base.Width * x + Minimum.X;
            y = base.Height * y + Minimum.Y;
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Converts an Array of PointF values from the draw window back into the double precision data coordinates.
        /// There will be uncertainty based on how zoomed in you are.
        /// </summary>
        /// <param name="inPoints"></param>
        /// <returns></returns>
        public List<Coordinate> DrawWindowToProj(PointF[] inPoints)
        {
            List<Coordinate> result = new List<Coordinate>();

            double minX = Minimum.X;
            double minY = Minimum.Y;
            double w = base.Width;
            double h = base.Height;
            foreach (PointF point in inPoints)
            {
                double x = Convert.ToDouble(point);
                double y = Convert.ToDouble(point);
                x = w * x + minX;
                y = h * y + minY;
                result.Add(new Coordinate(x, y));
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public RectangleF ProjToDrawWindow(IEnvelope env)
        {
            PointF ul = ProjToDrawWindow(env.Minimum.X, env.Maximum.Y);
            PointF lr = ProjToDrawWindow(env.Maximum.X, env.Minimum.Y);
            return new RectangleF(ul.X, ul.Y, lr.X - ul.X, ul.Y - lr.Y);
        }

        /// <summary>
        /// Calculates a geographic envelope from the specified window in the DrawWindow coordinates.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public IEnvelope DrawWindowToProj(RectangleF window)
        {
            PointF lr = new PointF(window.Right, window.Bottom);
            return new Envelope(DrawWindowToProj(window.Location), DrawWindowToProj(lr));
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is the the current extent of the map in geographic coordinates.
        /// </summary>
        public virtual IEnvelope GeographicView
        {
            get { return _geographicView; }
            set { _geographicView = value; }
        }

        /// <summary>
        /// Retrieves the MinX value of the GeographicView, in DrawWindow coordinates
        /// </summary>
        public RectangleF View
        {
            get
            {
                return ProjToDrawWindow(_geographicView);
            }
        }

        /// <summary>
        /// This calculates the DrawWindowView from the GeographicView
        /// </summary>
        public virtual RectangleF GetDrawWindowView()
        {
            return ProjToDrawWindow(_geographicView);
        }

        #endregion
    }
}
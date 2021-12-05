// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using DotSpatial.NTSExtension;
using NetTopologySuite.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A Draw Window is a special type of envelope that supports some basic transformations.
    /// </summary>
    public class DrawWindow : Envelope
    {
        #region Fields

        private Envelope _geographicView;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawWindow"/> class,
        /// making the assumption that the map is in Geographic coordinates of decimal degrees.
        /// </summary>
        public DrawWindow()
        {
            _geographicView = new Envelope(-180, 180, -90, 90);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawWindow"/> class based on the specified Envelope.
        /// The envelope becomes the GeographicView for this DrawWindow.
        /// </summary>
        /// <param name="env">Envelope the DrawWindow is based on.</param>
        public DrawWindow(Envelope env)
            : base(env)
        {
            _geographicView = env.Copy();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current extent of the map in geographic coordinates.
        /// </summary>
        public virtual Envelope GeographicView
        {
            get
            {
                return _geographicView;
            }

            set
            {
                _geographicView = value;
            }
        }

        /// <summary>
        /// Gets the MinX value of the GeographicView, in DrawWindow coordinates.
        /// </summary>
        public RectangleF View => ProjToDrawWindow(_geographicView);

        #endregion

        #region Methods

        /// <summary>
        /// Replaces the inherited Envelope copy in order to create a copy of the DrawWindow instead.
        /// </summary>
        /// <returns>A copy of the DrawWindow.</returns>
        public new DrawWindow Copy()
        {
            Envelope env = base.Copy();
            return new DrawWindow(env)
            {
                GeographicView = _geographicView
            };
        }

        /// <summary>
        /// Converts a PointF from the draw window back into the double precision data coordinate.
        /// </summary>
        /// <param name="inPoint">Point to convert.</param>
        /// <returns>The resulting double precision data coordinate.</returns>
        public Coordinate DrawWindowToProj(PointF inPoint)
        {
            double x = Convert.ToDouble(inPoint);
            double y = Convert.ToDouble(inPoint);
            x = (Width * x) + MinX;
            y = (Height * y) + MinY;
            return new Coordinate(x, y);
        }

        /// <summary>
        /// Converts an Array of PointF values from the draw window back into the double precision data coordinates.
        /// There will be uncertainty based on how zoomed in you are.
        /// </summary>
        /// <param name="inPoints">Points to convert.</param>
        /// <returns>The resulting list of double precision data coordinates.</returns>
        public List<Coordinate> DrawWindowToProj(PointF[] inPoints)
        {
            List<Coordinate> result = new List<Coordinate>();

            double minX = MinX;
            double minY = MinY;
            double w = Width;
            double h = Height;
            foreach (PointF point in inPoints)
            {
                double x = Convert.ToDouble(point);
                double y = Convert.ToDouble(point);
                x = (w * x) + minX;
                y = (h * y) + minY;
                result.Add(new Coordinate(x, y));
            }

            return result;
        }

        /// <summary>
        /// Calculates a geographic envelope from the specified window in the DrawWindow coordinates.
        /// </summary>
        /// <param name="window">The rectangle to convert.</param>
        /// <returns>The resulting double precision envelope.</returns>
        public Envelope DrawWindowToProj(RectangleF window)
        {
            PointF lr = new PointF(window.Right, window.Bottom);
            return new Envelope(DrawWindowToProj(window.Location), DrawWindowToProj(lr));
        }

        /// <summary>
        /// This calculates the DrawWindowView from the GeographicView.
        /// </summary>
        /// <returns>The draw window view.</returns>
        public virtual RectangleF GetDrawWindowView()
        {
            return ProjToDrawWindow(_geographicView);
        }

        /// <summary>
        /// Converts two dimensions of the specified coordinate into a two dimensional PointF.
        /// </summary>
        /// <param name="inX">The x value.</param>
        /// <param name="inY">The y value.</param>
        /// <returns>A PointF.</returns>
        public PointF ProjToDrawWindow(double inX, double inY)
        {
            double x = inX - MinX;
            double y = inY - MinY;
            x /= Width;
            y /= Height;
            return new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
        }

        /// <summary>
        /// Converts two dimensions of the specified coordinate into a two dimensional PointF.
        /// </summary>
        /// <param name="inCoord">Any valid ICoordinate.</param>
        /// <returns>A PointF.</returns>
        public PointF ProjToDrawWindow(Coordinate inCoord)
        {
            double x = inCoord.X - MinX;
            double y = inCoord.Y - MinY;
            x /= Width;
            y /= Height;
            return new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
        }

        /// <summary>
        /// Converts two dimensions of the specified coordinate into a two dimensional PointF.
        /// </summary>
        /// <param name="inCoords">The coordinates to convert.</param>
        /// <returns>The resulting list of converted points.</returns>
        public PointF[] ProjToDrawWindow(List<Coordinate> inCoords)
        {
            if (inCoords == null || inCoords.Count == 0) return null;

            double minX = MinX;
            double minY = MinY;
            double w = Width;
            double h = Height;
            PointF[] result = new PointF[inCoords.Count];
            for (int i = 0; i < inCoords.Count; i++)
            {
                double x = inCoords[i].X - minX;
                double y = inCoords[i].Y - minY;
                x /= w;
                y /= h;
                result[i] = new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
            }

            return result;
        }

        /// <summary>
        /// Calculates a window in the DrawWindow coordinates from the specified geographic envelope.
        /// </summary>
        /// <param name="env">Envelope that gets converted.</param>
        /// <returns>The resulting window in DrawWindow coordinates.</returns>
        public RectangleF ProjToDrawWindow(Envelope env)
        {
            PointF ul = ProjToDrawWindow(env.MinX, env.MaxY);
            PointF lr = ProjToDrawWindow(env.MaxX, env.MinY);
            return new RectangleF(ul.X, ul.Y, lr.X - ul.X, ul.Y - lr.Y);
        }

        #endregion
    }
}
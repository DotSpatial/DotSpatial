﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Controls
{
    /// <summary>
    /// North Arrow control for the Layout.
    /// </summary>
    public class LayoutNorthArrow : LayoutElement
    {
        #region Fields

        private Color _color;
        private NorthArrowStyle _northArrowStyle;
        private float _rotation;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNorthArrow"/> class.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public LayoutNorthArrow()
        {
            _color = Color.Black;
            _northArrowStyle = NorthArrowStyle.Default;
            Name = "North Arrow";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the style of the north arrow to draw
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public NorthArrowStyle NorthArrowStyle
        {
            get
            {
                return _northArrowStyle;
            }

            set
            {
                _northArrowStyle = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the rotations of the north arrow
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public float Rotation
        {
            get
            {
                return _rotation;
            }

            set
            {
                _rotation = value % 360F;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">boolean, true if printing to the actual paper/document, false if drawing as a control</param>
        public override void Draw(Graphics g, bool printing)
        {
            GraphicsPath gp = new GraphicsPath();
            Point[] pts;
            Matrix m = new Matrix(Size.Width / 100F, 0F, 0F, Size.Height / 100F, Location.X, Location.Y);
            m.RotateAt(_rotation, new PointF(50F, 50F), MatrixOrder.Prepend);

            Pen mypen = new Pen(_color, 2F);
            SolidBrush fillBrush = new SolidBrush(_color);
            mypen.LineJoin = LineJoin.Round;
            mypen.StartCap = LineCap.Round;
            mypen.EndCap = LineCap.Round;

            // All north arrows are defined as a graphics path in 100x100 size which is then scaled to fit the rectangle of the element
            switch (_northArrowStyle)
            {
                case NorthArrowStyle.BlackArrow:

                    gp.AddLine(50, 5, 5, 50);
                    gp.AddLine(5, 50, 30, 50);
                    gp.AddLine(30, 50, 30, 95);
                    gp.AddLine(30, 95, 70, 95);
                    gp.AddLine(70, 95, 70, 50);
                    gp.AddLine(70, 50, 95, 50);
                    gp.CloseFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    g.FillPath(fillBrush, gp);

                    // N
                    gp = new GraphicsPath();
                    gp.AddLine(40, 80, 40, 45);
                    gp.StartFigure();
                    gp.AddLine(40, 45, 60, 80);
                    gp.StartFigure();
                    gp.AddLine(60, 80, 60, 45);
                    gp.StartFigure();
                    mypen.Color = Color.White;
                    mypen.Width = Size.Width / 20;
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    break;

                case NorthArrowStyle.Default:

                    // draw the outline
                    DrawOutline(gp);
                    gp.CloseFigure();

                    // Draw the N
                    gp.AddLine(45, 57, 45, 43);
                    gp.StartFigure();
                    gp.AddLine(45, 43, 55, 57);
                    gp.StartFigure();
                    gp.AddLine(55, 57, 55, 43);
                    gp.StartFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);

                    // Draw the top arrow
                    gp = new GraphicsPath();
                    gp.AddLine(50, 5, 60, 40);
                    gp.AddBezier(60, 40, 55, 35, 45, 35, 40, 40);
                    gp.CloseFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    g.FillPath(fillBrush, gp);

                    break;

                case NorthArrowStyle.CenterStar:

                    // Outline
                    DrawOutline(gp);
                    gp.AddBezier(40F, 40F, 45F, 35F, 55F, 35F, 60f, 40F);
                    gp.AddLine(60, 40, 50, 5);
                    gp.CloseFigure();

                    // N
                    gp.AddLine(47, 33, 47, 20);
                    gp.StartFigure();
                    gp.AddLine(47, 20, 53, 33);
                    gp.StartFigure();
                    gp.AddLine(53, 33, 53, 20);
                    gp.StartFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);

                    // Draw the center circle
                    gp = new GraphicsPath();
                    gp.AddLine(30, 30, 40, 50);
                    gp.AddLine(40, 50, 30, 70);
                    gp.AddLine(30, 70, 50, 60);
                    gp.AddLine(50, 60, 70, 70);
                    gp.AddLine(70, 70, 60, 50);
                    gp.AddLine(60, 50, 70, 30);
                    gp.AddLine(70, 30, 50, 40);
                    gp.CloseFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    g.FillPath(fillBrush, gp);

                    break;

                case NorthArrowStyle.TriangleN:
                    pts = new Point[3];
                    pts[0] = new Point(50, 5);
                    pts[1] = new Point(5, 95);
                    pts[2] = new Point(95, 95);
                    gp.AddLines(pts);
                    gp.CloseFigure();
                    gp.AddLine(40, 80, 40, 45);
                    gp.StartFigure();
                    gp.AddLine(40, 45, 60, 80);
                    gp.StartFigure();
                    gp.AddLine(60, 80, 60, 45);
                    gp.StartFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    break;

                case NorthArrowStyle.TriangleHat:
                    pts = new Point[3];
                    pts[0] = new Point(50, 5);
                    pts[1] = new Point(5, 60);
                    pts[2] = new Point(95, 60);
                    gp.AddLines(pts);
                    gp.CloseFigure();
                    gp.AddLine(5, 95, 5, 65);
                    gp.StartFigure();
                    gp.AddLine(5, 65, 95, 95);
                    gp.StartFigure();
                    gp.AddLine(95, 95, 95, 65);
                    gp.StartFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    break;

                case NorthArrowStyle.ArrowN:
                    pts = new Point[3];
                    pts[0] = new Point(5, 25);
                    pts[1] = new Point(50, 5);
                    pts[2] = new Point(95, 25);
                    gp.AddLines(pts);
                    gp.CloseFigure();
                    gp.AddLine(50, 25, 50, 95);
                    gp.StartFigure();
                    gp.AddLine(5, 80, 5, 45);
                    gp.StartFigure();
                    gp.AddLine(5, 45, 95, 80);
                    gp.StartFigure();
                    gp.AddLine(95, 80, 95, 45);
                    gp.StartFigure();
                    gp.Transform(m);
                    g.DrawPath(mypen, gp);
                    break;
            }

            gp.Dispose();
        }

        private static void DrawOutline(GraphicsPath gp)
        {
            gp.AddLine(40, 40, 5, 50);
            gp.AddLine(5, 50, 40, 60);
            gp.AddLine(40, 60, 50, 95);
            gp.AddLine(50, 95, 60, 60);
            gp.AddLine(60, 60, 95, 50);
            gp.AddLine(95, 50, 60, 40);
            gp.AddBezier(60F, 40F, 65F, 45F, 65F, 55F, 60F, 60F);
            gp.AddBezier(60F, 60F, 55F, 65F, 45F, 65F, 40F, 60F);
            gp.AddBezier(40F, 60F, 35F, 55F, 35F, 45F, 40F, 40F);
        }

        #endregion
    }
}
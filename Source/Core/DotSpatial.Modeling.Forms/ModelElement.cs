// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;
using Point = System.Drawing.Point;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Defines the base class for all model components.
    /// </summary>
    public class ModelElement : ICloneable
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelElement"/> class.
        /// </summary>
        /// <param name="modelElements">A list of all the elements in the model.</param>
        public ModelElement(List<ModelElement> modelElements)
        {
            Location = new Point(0, 0);
            Height = 100;
            Width = 170;
            Color = Color.Wheat;
            Shape = ModelShape.Triangle;
            Font = SystemFonts.MessageBoxFont;
            Name = string.Empty;
            Highlight = 1;
            ModelElements = modelElements;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base color of the shapes gradient.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the font used to draw the text on the element.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the shape of the element.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the highlight. Returns 1 if the object is not highlighted less than 1 if it is highlighted.
        /// </summary>
        public double Highlight { get; set; }

        /// <summary>
        /// Gets or sets the location of the element in the parent form.
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the text that is drawn on the element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a rectangle representing the element, top left corner being the location of the parent form of the element.
        /// </summary>
        public Rectangle Rectangle => new Rectangle(Location.X, Location.Y, Width, Height);

        /// <summary>
        /// Gets or sets the shape of the model component.
        /// </summary>
        public ModelShape Shape { get; set; }

        /// <summary>
        /// Gets or sets the width of the element.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets a list of all elements in the model.
        /// </summary>
        internal List<ModelElement> ModelElements { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This returns a duplicate of this object.
        /// </summary>
        /// <returns>The copy.</returns>
        public object Clone()
        {
            return Copy();
        }

        /// <summary>
        /// Returns a shallow copy of the Parameter class.
        /// </summary>
        /// <returns>A new Parameters class that is a shallow copy of the original parameters class.</returns>
        public ModelElement Copy()
        {
            return MemberwiseClone() as ModelElement;
        }

        /// <summary>
        /// When a double click is caught by the parent class call this method.
        /// </summary>
        /// <returns>True.</returns>
        public virtual bool DoubleClick()
        {
            return true;
        }

        /// <summary>
        /// Returns true if the element intersects the rectangle from the parent class.
        /// </summary>
        /// <param name="rect">The rectangle to test must be in the virtual modeling coordinant plane.</param>
        /// <returns>True, if the element intersects the rectangle from the parent class.</returns>
        public virtual bool ElementInRectangle(Rectangle rect)
        {
            Geometry rectanglePoly;
            if (rect.Height == 0 && rect.Width == 0)
            {
                rectanglePoly = new NetTopologySuite.Geometries.Point(rect.X, rect.Y);
            }
            else if (rect.Width == 0)
            {
                Coordinate[] rectanglePoints = new Coordinate[2];
                rectanglePoints[0] = new Coordinate(rect.X, rect.Y);
                rectanglePoints[1] = new Coordinate(rect.X, rect.Y + rect.Height);
                rectanglePoly = new LineString(rectanglePoints);
            }
            else if (rect.Height == 0)
            {
                Coordinate[] rectanglePoints = new Coordinate[2];
                rectanglePoints[0] = new Coordinate(rect.X, rect.Y);
                rectanglePoints[1] = new Coordinate(rect.X + rect.Width, rect.Y);
                rectanglePoly = new LineString(rectanglePoints);
            }
            else
            {
                Coordinate[] rectanglePoints = new Coordinate[5];
                rectanglePoints[0] = new Coordinate(rect.X, rect.Y);
                rectanglePoints[1] = new Coordinate(rect.X, rect.Y + rect.Height);
                rectanglePoints[2] = new Coordinate(rect.X + rect.Width, rect.Y + rect.Height);
                rectanglePoints[3] = new Coordinate(rect.X + rect.Width, rect.Y);
                rectanglePoints[4] = new Coordinate(rect.X, rect.Y);
                rectanglePoly = new Polygon(new LinearRing(rectanglePoints));
            }

            switch (Shape)
            {
                case ModelShape.Rectangle:
                    return rect.IntersectsWith(Rectangle);

                case ModelShape.Ellipse:
                    int b = Height / 2;
                    int a = Width / 2;
                    Coordinate[] ellipsePoints = new Coordinate[(4 * a) + 1];
                    for (int x = -a; x <= a; x++)
                    {
                        if (x == 0)
                        {
                            ellipsePoints[x + a] = new Coordinate(Location.X + x + a, Location.Y);
                            ellipsePoints[(3 * a) - x] = new Coordinate(Location.X + x + a, Location.Y + Height);
                        }
                        else
                        {
                            ellipsePoints[x + a] = new Coordinate(Location.X + x + a, Location.Y + b - Math.Sqrt(Math.Abs(((b * b * x * x) / (a * a)) - (b * b))));
                            ellipsePoints[(3 * a) - x] = new Coordinate(Location.X + x + a, Location.Y + b + Math.Sqrt(Math.Abs(((b * b * x * x) / (a * a)) - (b * b))));
                        }
                    }

                    Polygon ellipsePoly = new Polygon(new LinearRing(ellipsePoints));
                    return ellipsePoly.Intersects(rectanglePoly);

                case ModelShape.Triangle:
                    Coordinate[] trianglePoints = new Coordinate[4];
                    trianglePoints[0] = new Coordinate(Location.X, Location.Y);
                    trianglePoints[1] = new Coordinate(Location.X, Location.Y + Height);
                    trianglePoints[2] = new Coordinate(Location.X + Width - 5, Location.Y + ((Height - 5) / 2));
                    trianglePoints[3] = new Coordinate(Location.X, Location.Y);
                    Polygon trianglePoly = new Polygon(new LinearRing(trianglePoints));
                    return trianglePoly.Intersects(rectanglePoly);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a list of all model elements that are direct children of this element.
        /// </summary>
        /// <returns>A list of all model element that are direct children of this element.</returns>
        public List<ModelElement> GetChildren()
        {
            return GetChildren(this);
        }

        /// <summary>
        /// Returns a list of all model elements that are direct parents of this element.
        /// </summary>
        /// <returns>A list of all model elements that are direct parents of this element.</returns>
        public List<ModelElement> GetParents()
        {
            return GetParents(this);
        }

        /// <summary>
        /// Darkens the component slightly.
        /// </summary>
        /// <param name="highlighted">Darkens if true returns to normal if false.</param>
        public virtual void Highlighted(bool highlighted)
        {
            Highlight = highlighted ? 0.85 : 1.0;
        }

        /// <summary>
        /// Returns true if this model element is downstram of the potentialUpstream element.
        /// </summary>
        /// <param name="potentialUpstream">Element to check against.</param>
        /// <returns>True if this model element is downstram of the potentialUpstream element.</returns>
        public bool IsDownstreamOf(ModelElement potentialUpstream)
        {
            return IsDownstreamOf(potentialUpstream, this);
        }

        /// <summary>
        /// Returns true if this model element is downstream of the potentialUpstream element.
        /// </summary>
        /// <param name="potentialDownstream">Element to check against.</param>
        /// <returns>True if this model element is downstream of the potentialUpstream element.</returns>
        public bool IsUpstreamOf(ModelElement potentialDownstream)
        {
            return IsUpstreamOf(potentialDownstream, this);
        }

        /// <summary>
        /// Repaints the form with cool background and stuff.
        /// </summary>
        /// <param name="graph">The graphics object to paint to, the element will be drawn to 0, 0.</param>
        public virtual void Paint(Graphics graph)
        {
            // Sets up the colors to use
            Pen outlinePen = new Pen(SymbologyGlobal.ColorFromHsl(Color.GetHue(), Color.GetSaturation(), Color.GetBrightness() * 0.6 * Highlight), 1.75F);
            Color gradientTop = SymbologyGlobal.ColorFromHsl(Color.GetHue(), Color.GetSaturation(), Color.GetBrightness() * 0.7 * Highlight);
            Color gradientBottom = SymbologyGlobal.ColorFromHsl(Color.GetHue(), Color.GetSaturation(), Color.GetBrightness() * 1.0 * Highlight);

            // The path used for drop shadows
            GraphicsPath shadowPath = new GraphicsPath();
            ColorBlend colorBlend = new ColorBlend(3)
            {
                Colors = new[] { Color.Transparent, Color.FromArgb(180, Color.DarkGray), Color.FromArgb(180, Color.DimGray) },
                Positions = new[] { 0f, 0.125f, 1f }
            };

            // Draws Rectangular Shapes
            if (Shape == ModelShape.Rectangle)
            {
                // Draws the shadow
                shadowPath.AddPath(GetRoundedRect(new Rectangle(5, 5, Width, Height), 10), true);
                PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath)
                {
                    WrapMode = WrapMode.Clamp,
                    InterpolationColors = colorBlend
                };
                graph.FillPath(shadowBrush, shadowPath);

                // Draws the basic shape
                Rectangle fillRectange = new Rectangle(0, 0, Width - 5, Height - 5);
                GraphicsPath fillArea = GetRoundedRect(fillRectange, 5);
                LinearGradientBrush myBrush = new LinearGradientBrush(fillRectange, gradientBottom, gradientTop, LinearGradientMode.Vertical);
                graph.FillPath(myBrush, fillArea);
                graph.DrawPath(outlinePen, fillArea);

                // Draws the status light
                DrawStatusLight(graph);

                // Draws the text
                SizeF textSize = graph.MeasureString(Name, Font, Width);
                RectangleF textRect;
                if ((textSize.Width < Width) || (textSize.Height < Height))
                    textRect = new RectangleF((Width - textSize.Width) / 2, (Height - textSize.Height) / 2, textSize.Width, textSize.Height);
                else
                    textRect = new RectangleF(0, (Height - textSize.Height) / 2, Width, textSize.Height);
                graph.DrawString(Name, Font, new SolidBrush(Color.FromArgb(50, Color.Black)), textRect);
                textRect.X = textRect.X - 1;
                textRect.Y = textRect.Y - 1;
                graph.DrawString(Name, Font, Brushes.Black, textRect);

                // Garbage collection
                fillArea.Dispose();
                myBrush.Dispose();
            }

            // Draws Ellipse Shapes
            if (Shape == ModelShape.Ellipse)
            {
                // Draws the shadow
                shadowPath.AddEllipse(0, 5, Width + 5, Height);
                PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath)
                {
                    WrapMode = WrapMode.Clamp,
                    InterpolationColors = colorBlend
                };
                graph.FillPath(shadowBrush, shadowPath);

                // Draws the Ellipse
                Rectangle fillArea = new Rectangle(0, 0, Width, Height);
                LinearGradientBrush myBrush = new LinearGradientBrush(fillArea, gradientBottom, gradientTop, LinearGradientMode.Vertical);
                graph.FillEllipse(myBrush, 1, 1, Width - 5, Height - 5);
                graph.DrawEllipse(outlinePen, 1, 1, Width - 5, Height - 5);

                // Draws the text
                SizeF textSize = graph.MeasureString(Name, Font, Width);
                RectangleF textRect;
                if ((textSize.Width < Width) || (textSize.Height < Height))
                    textRect = new RectangleF((Width - textSize.Width) / 2, (Height - textSize.Height) / 2, textSize.Width, textSize.Height);
                else
                    textRect = new RectangleF(0, (Height - textSize.Height) / 2, Width, textSize.Height);
                graph.DrawString(Name, Font, new SolidBrush(Color.FromArgb(50, Color.Black)), textRect);
                textRect.X = textRect.X - 1;
                textRect.Y = textRect.Y - 1;
                graph.DrawString(Name, Font, Brushes.Black, textRect);

                // Garbage collection
                myBrush.Dispose();
            }

            // Draws Triangular Shapes
            if (Shape == ModelShape.Triangle)
            {
                // Draws the shadow
                Point[] ptShadow = new Point[4];
                ptShadow[0] = new Point(5, 5);
                ptShadow[1] = new Point(Width + 5, ((Height - 5) / 2) + 5);
                ptShadow[2] = new Point(5, Height + 2);
                ptShadow[3] = new Point(5, 5);
                shadowPath.AddLines(ptShadow);
                PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath)
                {
                    WrapMode = WrapMode.Clamp,
                    InterpolationColors = colorBlend
                };
                graph.FillPath(shadowBrush, shadowPath);

                // Draws the shape
                Point[] pt = new Point[4];
                pt[0] = new Point(0, 0);
                pt[1] = new Point(Width - 5, (Height - 5) / 2);
                pt[2] = new Point(0, Height - 5);
                pt[3] = new Point(0, 0);
                GraphicsPath myPath = new GraphicsPath();
                myPath.AddLines(pt);
                Rectangle fillArea = new Rectangle(1, 1, Width - 5, Height - 5);
                LinearGradientBrush myBrush = new LinearGradientBrush(fillArea, gradientBottom, gradientTop, LinearGradientMode.Vertical);
                graph.FillPath(myBrush, myPath);
                graph.DrawPath(outlinePen, myPath);

                // Draws the text
                SizeF textSize = graph.MeasureString(Name, Font, Width);
                RectangleF textRect;
                if ((textSize.Width < Width) || (textSize.Height < Height))
                    textRect = new RectangleF((Width - textSize.Width) / 2, (Height - textSize.Height) / 2, textSize.Width, textSize.Height);
                else
                    textRect = new RectangleF(0, (Height - textSize.Height) / 2, Width, textSize.Height);
                graph.DrawString(Name, Font, Brushes.Black, textRect);

                // Garbage collection
                myBrush.Dispose();
            }

            // Garbage collection
            shadowPath.Dispose();
            outlinePen.Dispose();
        }

        /// <summary>
        /// Calculates if a point is within the shape that defines the element.
        /// </summary>
        /// <param name="point">A point to test in the virtual modeling plane.</param>
        /// <returns>True, if the point is within the shape that defines the element.</returns>
        public virtual bool PointInElement(Point point)
        {
            Point pt = new Point(point.X - Location.X, point.Y - Location.Y);

            switch (Shape)
            {
                case ModelShape.Rectangle:
                    if (pt.X > 0 && pt.X < Width && pt.Y > 0 && pt.Y < Height)
                        return true;
                    break;

                case ModelShape.Ellipse:
                    double a = Width / 2.0;
                    double b = Height / 2.0;
                    double x = pt.X - a;
                    double y = pt.Y - b;
                    if (((x * x) / (a * a)) + ((y * y) / (b * b)) <= 1)
                        return true;
                    break;

                case ModelShape.Triangle:
                    if ((pt.X >= 0) && (pt.X < Width))
                    {
                        double y1 = (((Height / 2.0) / Width) * pt.X) + 0;
                        double y2 = (-((Height / 2.0) / Width) * pt.X) + Height;
                        if ((pt.Y < y2) && (pt.Y > y1))
                            return true;
                    }

                    break;

                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// This does nothing in the base class but child classes may override it.
        /// </summary>
        /// <param name="graph">The graphics object used for drawing.</param>
        protected virtual void DrawStatusLight(Graphics graph)
        {
        }

        /// <summary>
        /// Returns true if the point is in the extents rectangle of the element.
        /// </summary>
        /// <param name="pt">A point to test, assuming 0, 0 is the top left corner of the shapes drawing rectangle.</param>
        /// <returns>True if the point is in the extents rectangle of the element.</returns>
        protected virtual bool PointInExtents(Point pt)
        {
            return pt.X > 0 && pt.X < Width && pt.Y > 0 && pt.Y < Height;
        }

        /// <summary>
        /// Returns true if a point is in a rectangle.
        /// </summary>
        /// <param name="pt">Point to check.</param>
        /// <param name="rect">Rectangle to check.</param>
        /// <returns>True if a point is in a rectangle.</returns>
        protected virtual bool PointInRectangle(Point pt, Rectangle rect)
        {
            return (pt.X >= rect.X && pt.X <= (rect.X + rect.Width)) && (pt.Y >= rect.Y && pt.Y <= (rect.Y + rect.Height));
        }

        /// <summary>
        /// Creates a rounded corner rectangle from a regular rectangle.
        /// </summary>
        /// <param name="baseRect">Rectangle used for creation.</param>
        /// <param name="radius">Radius for rounding the corners.</param>
        /// <returns>The round cornered rectangle.</returns>
        private static GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            if ((radius <= 0.0F) || radius >= (Math.Min(baseRect.Width, baseRect.Height) / 2.0))
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            // top left arc
            path.AddArc(arc, 180, 90);

            // top right arc
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private List<ModelElement> GetChildren(ModelElement parent)
        {
            List<ModelElement> listChildren = new List<ModelElement>();
            foreach (ModelElement mEl in ModelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr?.StartElement != null && mAr.StartElement == parent)
                    listChildren.Add(mAr.StopElement);
            }

            return listChildren;
        }

        private List<ModelElement> GetParents(ModelElement child)
        {
            List<ModelElement> listParents = new List<ModelElement>();
            foreach (ModelElement mEl in ModelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr?.StopElement != null && mAr.StopElement == child)
                    listParents.Add(mAr.StartElement);
            }

            return listParents;
        }

        private bool IsDownstreamOf(ModelElement potentialUpstream, ModelElement child)
        {
            foreach (ModelElement mEl in ModelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr != null)
                {
                    if (mAr.StopElement == null) continue;
                    if (mAr.StopElement == child)
                    {
                        if (mAr.StartElement == null) continue;
                        if (mAr.StartElement == potentialUpstream) return true;
                        foreach (ModelElement parents in GetParents(mAr.StartElement))
                        {
                            if (IsDownstreamOf(potentialUpstream, parents)) return true;
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        private bool IsUpstreamOf(ModelElement potentialDownstream, ModelElement parent)
        {
            foreach (ModelElement mEl in ModelElements)
            {
                ArrowElement mAr = mEl as ArrowElement;
                if (mAr?.StartElement != null)
                {
                    if (mAr.StartElement == parent)
                    {
                        if (mAr.StopElement == null) continue;
                        if (mAr.StopElement == potentialDownstream) return true;
                        foreach (ModelElement children in GetChildren(mAr.StartElement))
                        {
                            if (IsUpstreamOf(potentialDownstream, children)) return true;
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
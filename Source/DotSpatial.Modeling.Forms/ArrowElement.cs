// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ArrowElement
// Description:  An abstract class that handles drawing arrows between elements in the modeler window
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Point = System.Drawing.Point;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// An arrow
    /// </summary>
    public class ArrowElement : ModelElement
    {
        #region Fields

        private GraphicsPath _arrowPath = new GraphicsPath();

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrowElement"/> class.
        /// </summary>
        /// <param name="sourceElement">The element the arrow starts at</param>
        /// <param name="destElement">the element the arrow ends at</param>
        /// <param name="modelElements">A list of all the elements in the model</param>
        public ArrowElement(ModelElement sourceElement, ModelElement destElement, List<ModelElement> modelElements)
            : base(modelElements)
        {
            StartElement = sourceElement;
            StopElement = destElement;
            UpdateDimensions();
            Shape = ModelShape.Arrow;
            Location = StartElement.Location;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the source element.
        /// </summary>
        public ModelElement StartElement { get; set; }

        /// <summary>
        /// Gets or sets the point in the arrows coordinants we draw from.
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the destination Element.
        /// </summary>
        public ModelElement StopElement { get; set; }

        /// <summary>
        /// Gets or sets the point in the arrows coordinants we stop drawing from.
        /// </summary>
        public Point StopPoint { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the element intersect the rectangle from the parent class.
        /// </summary>
        /// <param name="rect">The rectangle to test must be in the virtual modeling coordinant plane</param>
        /// <returns>True if the element intersect the rectangle from the parent class.</returns>
        public override bool ElementInRectangle(Rectangle rect)
        {
            IGeometry rectanglePoly;
            if ((rect.Height == 0) && (rect.Width == 0))
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

            if (Shape == ModelShape.Arrow)
            {
                Coordinate[] arrowPoints = new Coordinate[_arrowPath.PointCount];
                for (int i = 0; i < _arrowPath.PointCount; i++)
                {
                    arrowPoints[i] = new Coordinate(_arrowPath.PathPoints[i].X + Location.X, _arrowPath.PathPoints[i].Y + Location.Y);
                }

                LineString arrowLine = new LineString(arrowPoints);
                return arrowLine.Intersects(rectanglePoly);
            }

            return false;
        }

        /// <summary>
        /// Repaints the form with cool background and stuff
        /// </summary>
        /// <param name="graph">The graphics object to paint to, the element will be drawn to 0, 0</param>
        public override void Paint(Graphics graph)
        {
            // Draws Rectangular Shapes
            if (Shape == ModelShape.Arrow)
            {
                _arrowPath = new GraphicsPath();

                // Draws the basic shape
                Pen arrowPen = Highlight < 1 ? new Pen(Color.Cyan, 3F) : new Pen(Color.Black, 3F);

                // Draws the curved arrow
                Point[] lineArray = new Point[4];
                lineArray[0] = new Point(StartPoint.X, StartPoint.Y);
                lineArray[1] = new Point(StartPoint.X - ((StartPoint.X - StopPoint.X) / 3), StartPoint.Y);
                lineArray[2] = new Point(StopPoint.X - ((StopPoint.X - StartPoint.X) / 3), StopPoint.Y);
                lineArray[3] = new Point(StopPoint.X, StopPoint.Y);
                graph.DrawBeziers(arrowPen, lineArray);
                _arrowPath.AddBeziers(lineArray);
                _arrowPath.Flatten();

                // Draws the arrow head
                Point[] arrowArray = new Point[3];
                arrowArray[0] = StopPoint;
                arrowArray[1] = new Point(StopPoint.X - (5 * Math.Sign(StopPoint.X - StartPoint.X)), StopPoint.Y - 2);
                arrowArray[2] = new Point(StopPoint.X - (5 * Math.Sign(StopPoint.X - StartPoint.X)), StopPoint.Y + 2);
                graph.DrawPolygon(arrowPen, arrowArray);

                // Garbage collection
                arrowPen.Dispose();
            }
        }

        /// <summary>
        /// Calculates if a point is within the shape that defines the element.
        /// </summary>
        /// <param name="point">A point to test in the virtual modeling plane</param>
        /// <returns>True, if the point is within the shape that defines the element.</returns>
        public override bool PointInElement(Point point)
        {
            Rectangle temp = new Rectangle(point, new Size(1, 1));
            temp.Inflate(2, 2);
            return ElementInRectangle(temp);
        }

        /// <summary>
        /// Updates the dimensions.
        /// </summary>
        public void UpdateDimensions()
        {
            // Updates the location and size of the element based on the elements its attached to
            Location = StartElement.Location;
            Width = StopElement.Location.X - StartElement.Location.X;
            Height = StopElement.Location.Y - StartElement.Location.Y;
            StartPoint = new Point(StartElement.Width - 4, StartElement.Height / 2);
            StopPoint = new Point(Width, Height + (StopElement.Height / 2));
        }

        #endregion
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ArrowElement
// Description:  An abstract class that handles drawing arrows between elements in the modeler window
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// An arrow
    /// </summary>
    public class ArrowElement : ModelElement
    {
        GraphicsPath _arrowPath = new GraphicsPath();
        ModelElement _startElement;

        Point _startPoint;
        ModelElement _stopElement;
        Point _stopPoint;

        /// <summary>
        /// Creates an instance of an ArrowElement
        /// </summary>
        /// <param name="sourceElement">The element the arrow starts at</param>
        /// <param name="destElement">the element the arrow ends at</param>
        /// <param name="modelElements">A list of all the elements in the model</param>
        public ArrowElement(ModelElement sourceElement, ModelElement destElement, List<ModelElement> modelElements)
            : base(modelElements)
        {
            _startElement = sourceElement;
            _stopElement = destElement;
            UpdateDimentions();
            Shape = ModelShape.Arrow;
            Location = _startElement.Location;
        }

        /// <summary>
        /// Updates the dimentions
        /// </summary>
        public void UpdateDimentions()
        {
            //Updates the location and size of the element based on the elements its attached to
            Location = _startElement.Location;
            Width = _stopElement.Location.X - _startElement.Location.X;
            Height = _stopElement.Location.Y - _startElement.Location.Y;
            _startPoint = new Point(_startElement.Width - 4, (_startElement.Height / 2));
            _stopPoint = new Point(Width, Height + (StopElement.Height / 2));
        }

        /// <summary>
        /// Repaints the form with cool background and stuff
        /// </summary>
        /// <param name="graph">The graphics object to paint to, the element will be drawn to 0, 0</param>
        public override void Paint(Graphics graph)
        {
            //Draws Rectangular Shapes
            if (Shape == ModelShape.Arrow)
            {
                _arrowPath = new GraphicsPath();

                //Draws the basic shape
                Pen arrowPen;
                if (Highlight < 1)
                    arrowPen = new Pen(Color.Cyan, 3F);
                else
                    arrowPen = new Pen(Color.Black, 3F);

                //Draws the curved arrow
                Point[] lineArray = new Point[4];
                lineArray[0] = new Point(_startPoint.X, _startPoint.Y);
                lineArray[1] = new Point(_startPoint.X - ((_startPoint.X - _stopPoint.X) / 3), _startPoint.Y);
                lineArray[2] = new Point(_stopPoint.X - ((_stopPoint.X - _startPoint.X) / 3), _stopPoint.Y);
                lineArray[3] = new Point(_stopPoint.X, _stopPoint.Y);
                graph.DrawBeziers(arrowPen, lineArray);
                _arrowPath.AddBeziers(lineArray);
                _arrowPath.Flatten();

                //Draws the arrow head
                Point[] arrowArray = new Point[3];
                arrowArray[0] = _stopPoint;
                arrowArray[1] = new Point(_stopPoint.X - (5 * Math.Sign(_stopPoint.X - _startPoint.X)), _stopPoint.Y - 2);
                arrowArray[2] = new Point(_stopPoint.X - (5 * Math.Sign(_stopPoint.X - _startPoint.X)), _stopPoint.Y + 2);
                graph.DrawPolygon(arrowPen, arrowArray);

                //Garbage collection
                arrowPen.Dispose();
            }
        }

        /// <summary>
        /// Calculates if a point is within the shape that defines the element
        /// </summary>
        /// <param name="point">A point to test in the virtual modeling plane</param>
        /// <returns></returns>
        public override bool PointInElement(Point point)
        {
            Rectangle temp = new Rectangle(point, new Size(1, 1));
            temp.Inflate(2, 2);
            return ElementInRectangle(temp);
        }

        /// <summary>
        /// Returns true if the element intersect the rectangle from the parent class
        /// </summary>
        /// <param name="rect">The rectangle to test must be in the virtual modeling coordinant plane</param>
        /// <returns></returns>
        public override bool ElementInRectangle(Rectangle rect)
        {
            IGeometry rectanglePoly;
            if ((rect.Height == 0) && (rect.Width == 0))
            {
                rectanglePoly = new Topology.Point(rect.X, rect.Y);
            }
            else if (rect.Width == 0)
            {
                Topology.Point[] rectanglePoints = new Topology.Point[2];
                rectanglePoints[0] = new Topology.Point(rect.X, rect.Y);
                rectanglePoints[1] = new Topology.Point(rect.X, rect.Y + rect.Height);
                rectanglePoly = new LineString(rectanglePoints);
            }
            else if (rect.Height == 0)
            {
                Topology.Point[] rectanglePoints = new Topology.Point[2];
                rectanglePoints[0] = new Topology.Point(rect.X, rect.Y);
                rectanglePoints[1] = new Topology.Point(rect.X + rect.Width, rect.Y);
                rectanglePoly = new LineString(rectanglePoints);
            }
            else
            {
                Topology.Point[] rectanglePoints = new Topology.Point[5];
                rectanglePoints[0] = new Topology.Point(rect.X, rect.Y);
                rectanglePoints[1] = new Topology.Point(rect.X, rect.Y + rect.Height);
                rectanglePoints[2] = new Topology.Point(rect.X + rect.Width, rect.Y + rect.Height);
                rectanglePoints[3] = new Topology.Point(rect.X + rect.Width, rect.Y);
                rectanglePoints[4] = new Topology.Point(rect.X, rect.Y);
                rectanglePoly = new Polygon(new LinearRing(rectanglePoints));
            }

            if (Shape == ModelShape.Arrow)
            {
                Topology.Point[] arrowPoints = new Topology.Point[_arrowPath.PointCount];
                for (int i = 0; i < _arrowPath.PointCount; i++)
                {
                    arrowPoints[i] = new Topology.Point(_arrowPath.PathPoints[i].X + Location.X, _arrowPath.PathPoints[i].Y + Location.Y);
                }
                LineString arrowLine = new LineString(arrowPoints);
                return (arrowLine.Intersects(rectanglePoly));
            }
            return false;
        }

        #region -------------------- Properties

        /// <summary>
        /// Gets or sets the destination Element
        /// </summary>
        public ModelElement StopElement
        {
            get { return _stopElement; }
            set { _stopElement = value; }
        }

        /// <summary>
        /// Gets or sets the source element
        /// </summary>
        public ModelElement StartElement
        {
            get { return _startElement; }
            set { _startElement = value; }
        }

        /// <summary>
        /// the point in the arrows coordinants we draw from
        /// </summary>
        public Point StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        /// <summary>
        /// the point in the arrows coordinants we stop drawing from
        /// </summary>
        public Point StopPoint
        {
            get { return _stopPoint; }
            set { _stopPoint = value; }
        }

        #endregion
    }
}
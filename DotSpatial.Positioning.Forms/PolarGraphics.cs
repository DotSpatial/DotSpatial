// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Forms.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#if !PocketPC || DesignTime
#endif

using System.Reflection;

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime
#if Framework20

    /// <summary>
    /// Encapsulates a GDI+ drawing surface using polar coordinates instead of pixel coordinates.
    /// </summary>
    [Obfuscation(Feature = "renaming", Exclude = false, ApplyToMembers = true)]
    [Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    [Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
#endif
    public sealed class PolarGraphics
    {
        private readonly Graphics _g;
        private Angle _pRotation;
        private Azimuth _pOrigin;
        private readonly PolarCoordinateOrientation _pOrientation;
        private readonly float _pCenterR;
        private readonly float _pMaximumR;
        private readonly double _halfWidth;
        private readonly double _halfHeight;
        private readonly double _spanSize;
        private readonly double _horizontalScale;
        private readonly double _verticalScale;
#if !PocketPC
        private static readonly StringFormat _pStringFormat = new StringFormat();
#endif

#if !PocketPC

        internal static void Dispose()
        {
            if (_pStringFormat != null)
            {
                try
                {
                    _pStringFormat.Dispose();
                }
                catch
                {
                }
            }
        }

#endif

#if PocketPC
		internal PolarGraphics(Graphics g, Rectangle visibleClipBounds,
            Angle rotation, Azimuth origin, PolarCoordinateOrientation orientation,
            float centerR, float maximumR)
#else

        internal PolarGraphics(Graphics g, Angle rotation, Azimuth origin,
            PolarCoordinateOrientation orientation, float centerR, float maximumR)
#endif
        {
            _g = g;
            _pRotation = rotation;
            _pOrigin = origin;
            _pOrientation = orientation;
            _pCenterR = centerR;
            _pMaximumR = maximumR;
#if PocketPC
            HalfWidth = (visibleClipBounds.Width * 0.5);
            HalfHeight = (visibleClipBounds.Height * 0.5);

//			// Create smoother graphics
//			gx = new DotSpatial.Positioning.Drawing.GraphicsX(visibleClipBounds.Width, visibleClipBounds.Height);
//			gx.Clear(Color.White);
//			gx.ResetTransform();
#else
            _halfWidth = g.VisibleClipBounds.Width * 0.5;
            _halfHeight = g.VisibleClipBounds.Height * 0.5;
#endif
            _spanSize = Math.Abs(_pMaximumR - _pCenterR);
            _horizontalScale = _halfWidth / _spanSize;
            _verticalScale = _halfHeight / _spanSize;

#if !PocketPC
            // Set up the string format
            _pStringFormat.LineAlignment = StringAlignment.Center;
            _pStringFormat.Alignment = StringAlignment.Center;
#endif
        }

        /// <summary>Converts a polar coordinate to a pixel coordinate.</summary>
        public Point ToPoint(PolarCoordinate coordinate)
        {
            PointD point = ToPointD(coordinate);
            return new Point((int)point.X, (int)point.Y);
        }

        /// <summary>Converts a polar coordinate to a highly-precise pixel coordinate.</summary>
        public PointD ToPointD(PolarCoordinate coordinate)
        {
            return coordinate.ToOrientation(_pOrigin, _pOrientation).Rotate(-_pRotation.DecimalDegrees).ToPointD()
                .Multiply(_horizontalScale, _verticalScale).Add(_halfWidth, _halfHeight);
        }

        /// <summary>
        /// To Polar Coordinate
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PolarCoordinate ToPolarCoordinate(Point point)
        {
            PointD pointD = new PointD(point.X, point.Y);
            return ToPolarCoordinate(pointD);
        }

#if !PocketPC

        /// <summary>
        /// To Polar Coordinate
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PolarCoordinate ToPolarCoordinate(PointF point)
        {
            PointD pointD = new PointD(point.X, point.Y);
            return ToPolarCoordinate(pointD);
        }

#endif

        /// <summary>
        /// To Polar coordinate
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PolarCoordinate ToPolarCoordinate(PointD point)
        {
            //return point.Subtract(HalfWidth, HalfHeight).Divide(HorizontalScale, VerticalScale)
            //    .ToPolarCoordinate().Rotate(pRotation.DecimalDegrees).ToOrientation(pOrigin, pOrientation);
            PointD value = point.Subtract(_halfWidth, _halfHeight).Divide(_horizontalScale, _verticalScale);
            PolarCoordinate value2 = PointDToPolarCoordinate(value);
            return value2.Rotate(_pRotation.DecimalDegrees).ToOrientation(_pOrigin, _pOrientation);
        }

        /// <summary>Converts the current instance into a polar coordinate.</summary>
        private static PolarCoordinate PointDToPolarCoordinate(PointD value)
        {
            //			double Value;
            //			double TempY = -pY;
            //			if (pX == 0)
            //				Value = 0;
            //			else
            //				Value = TempY / pX;
            // Calculate the coordinate using the default: 0° = East
            // and counter-clockwise movement
            Radian result = new Radian(Math.Atan2(-value.Y, value.X));
            PolarCoordinate result2 = new PolarCoordinate((float)Math.Sqrt(value.X * value.X + value.Y * value.Y), result.ToAngle(), Azimuth.East, PolarCoordinateOrientation.Counterclockwise);
            // And re-orient it at North and Clockwise
            return result2.ToOrientation(Azimuth.North, PolarCoordinateOrientation.Clockwise);
        }

#if !PocketPC

        /// <summary>Converts a polar coordinate to a precise pixel coordinate.</summary>
        public PointF ToPointF(PolarCoordinate coordinate)
        {
            return ToPointF(ToPointD(coordinate)); //fixes problems with DrawRotatedString
        }

        /// <summary>
        /// To PointF
        /// </summary>
        /// <param name="pointD"></param>
        /// <returns></returns>
        public PointF ToPointF(PointD pointD)
        {
            return new PointF((float)pointD.X, (float)pointD.Y);
        }

#endif

        /// <summary>
        /// Returns the compass direction which matches zero degrees.
        /// </summary>
        public Azimuth Origin
        {
            get
            {
                return _pOrigin;
            }
        }

        /// <summary>
        /// Returns whether positive values are applied in a clockwise or counter-clockwise direction.
        /// </summary>
        public PolarCoordinateOrientation Orientation
        {
            get
            {
                return _pOrientation;
            }
        }

        /// <summary>Returns the value of <em>R</em> associated with the center of the control.</summary>
        public float CenterR
        {
            get
            {
                return _pCenterR;
            }
        }

        /// <summary>Returns the value of <em>R</em> associated with the edge of the control.</summary>
        public float MaximumR
        {
            get
            {
                return _pMaximumR;
            }
        }

        /// <summary>Erases the control's background to the specified color.</summary>
        public void Clear(Color backgroundColor)
        {
            _g.Clear(backgroundColor);
        }

        /// <summary>Draws a single straight line.</summary>
        public void DrawLine(Pen pen, PolarCoordinate pt1, PolarCoordinate pt2)
        {
#if PocketPC
            // Convert to pixel coordinates
            PointD start = ToPointD(pt1);
            PointD end = ToPointD(pt2);
			//gx.DrawLine(new PenX(pen.Color, pen.Width), (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
            g.DrawLine(pen, (int)start.X, (int)start.Y, (int)end.X, (int)end.Y);
#else
            // Convert to pixel coordinates
            _g.DrawLine(pen, ToPointF(pt1), ToPointF(pt2));
#endif
        }

#if !PocketPC

        /// <summary>
        /// DrawString
        /// </summary>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <param name="brush"></param>
        /// <param name="point"></param>
        /// <param name="format"></param>
        public void DrawString(string s, Font font, Brush brush, PolarCoordinate point, StringFormat format)
        {
            _g.DrawString(s, font, brush, ToPointF(point), format);
        }

#endif

        /// <summary>
        /// Draw String
        /// </summary>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <param name="brush"></param>
        /// <param name="point"></param>
        public void DrawString(string s, Font font, SolidBrush brush, PolarCoordinate point)
        {
            PointD location = ToPointD(point);
#if PocketPC
            g.DrawString(s, font, brush, new RectangleF((float)Location.X, (float)Location.Y, 240.0f, 320.0f));
#else
            _g.DrawString(s, font, brush, ToPointF(location));
#endif
        }

#if !PocketPC

        /// <summary>
        /// Draw Centered String
        /// </summary>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <param name="brush"></param>
        /// <param name="point"></param>
        /// <param name="format"></param>
        public void DrawCenteredString(string s, Font font, Brush brush, PolarCoordinate point, StringFormat format)
        {
            PointD startPoint = ToPointD(point);
            SizeF stringSize = _g.MeasureString(s, font);
            PointD newStart = new PointD(startPoint.X - stringSize.Width * 0.5, startPoint.Y - stringSize.Height * 0.5);
            _g.DrawString(s, font, brush, ToPointF(newStart), format);
        }

#endif

        /// <summary>
        /// Draw Centered String
        /// </summary>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <param name="brush"></param>
        /// <param name="point"></param>
        public void DrawCenteredString(string s, Font font, SolidBrush brush, PolarCoordinate point)
        {
            PointD startPoint = ToPointD(point);
            SizeF stringSize = _g.MeasureString(s, font);
            PointD newStart = new PointD(startPoint.X - stringSize.Width * 0.5, startPoint.Y - stringSize.Height * 0.5);
#if PocketPC
            g.DrawString(s, font, brush, (float)NewStart.X, (float)NewStart.Y); //, StringSize.Width, StringSize.Height));
#else
            _g.DrawString(s, font, brush, ToPointF(newStart));
#endif
        }

        /// <summary>Draws a square or rectangular shape.</summary>
        public void DrawRectangle(Pen pen, PolarCoordinate upperLeft, PolarCoordinate lowerRight)
        {
            PointD ul = ToPointD(upperLeft);
            PointD lr = ToPointD(lowerRight);
#if PocketPC
            g.DrawRectangle(pen, (int)UL.X, (int)UL.Y, (int)Math.Abs(LR.X - UL.X), (int)Math.Abs(LR.Y - UL.Y));
#else
            _g.DrawRectangle(pen, (float)ul.X, (float)ul.Y, (float)Math.Abs(lr.X - ul.X), (float)Math.Abs(lr.Y - ul.Y));
#endif
        }

        /// <summary>Fills the interior of a circular shape.</summary>
        public void FillEllipse(Brush brush, PolarCoordinate center, float radius)
        {
            // Translate the coordinate to the center
            PointD centerPoint = ToPointD(center);
            PointD controlCenterPoint = ToPointD(PolarCoordinate.Empty);

            // Calculate the bounding box for the ellipse
            double minX = controlCenterPoint.X;
            double minY = controlCenterPoint.Y;
            double maxX = controlCenterPoint.X;
            double maxY = controlCenterPoint.Y;
            for (int angle = 0; angle < 360; angle += 10)
            {
                PointD x = ToPointD(new PolarCoordinate(radius, new Angle(angle), Azimuth.North, PolarCoordinateOrientation.Clockwise));
                minX = Math.Min(minX, x.X);
                minY = Math.Min(minY, x.Y);
                maxX = Math.Max(maxX, x.X);
                maxY = Math.Max(maxY, x.Y);
            }
            // Now translate the values by the center point
            double width = Math.Abs(maxX - minX);
            double height = Math.Abs(maxY - minY);

#if PocketPC
//			gx.FillEllipse(new SolidBrushX(brush.Color),
//				(float)(CenterPoint.X - (width * 0.5)), (float)(CenterPoint.Y - (height * 0.5)), (float)width, (float)height);

            g.FillEllipse(brush, (int)(CenterPoint.X - (width * 0.5)), (int)(CenterPoint.Y - (height * 0.5)), (int)width, (int)height);
#else
            _g.FillEllipse(brush, (float)(centerPoint.X - (width * 0.5)), (float)(centerPoint.Y - (height * 0.5)), (float)width, (float)height);
#endif
        }

        /// <summary>Draws a circular shape.</summary>
        public void DrawEllipse(Pen pen, PolarCoordinate center, float radius)
        {
            // Translate the coordinate to the center
            PointD centerPoint = ToPointD(center);
            PointD controlCenterPoint = ToPointD(PolarCoordinate.Empty);

            // Calculate the bounding box for the ellipse
            double minX = controlCenterPoint.X;
            double minY = controlCenterPoint.Y;
            double maxX = controlCenterPoint.X;
            double maxY = controlCenterPoint.Y;
            for (int angle = 0; angle < 360; angle += 10)
            {
                PointD x = ToPointD(new PolarCoordinate(radius, new Angle(angle), Azimuth.North, PolarCoordinateOrientation.Clockwise));
                minX = Math.Min(minX, x.X);
                minY = Math.Min(minY, x.Y);
                maxX = Math.Max(maxX, x.X);
                maxY = Math.Max(maxY, x.Y);
            }
            // Now translate the values by the center point
            double width = Math.Abs(maxX - minX);
            double height = Math.Abs(maxY - minY);

#if PocketPC
//			gx.DrawEllipse(new PenX(pen.Color, pen.Width),
//				(float)(CenterPoint.X - (width * 0.5)), (float)(CenterPoint.Y - (height * 0.5)), (float)width, (float)height);

            g.DrawEllipse(pen, (int)(CenterPoint.X - (width * 0.5)), (int)(CenterPoint.Y - (height * 0.5)), (int)width, (int)height);
#else
            _g.DrawEllipse(pen, (float)(centerPoint.X - (width * 0.5)), (float)(centerPoint.Y - (height * 0.5)), (float)width, (float)height);
#endif
        }

        /// <summary>Fills the interior of a closed shape.</summary>
        public void FillPolygon(Brush brush, PolarCoordinate[] points)
        {
#if PocketPC
			//gx.FillPolygon(new SolidBrushX(brush.Color), ToPointArray(points));
            g.FillPolygon(brush, ToPointArray(points));
#else
            _g.FillPolygon(brush, ToPointFArray(points));
#endif
        }

#if !PocketPC

        /// <summary>
        /// Converts an array of polar coordinates into a <strong>GraphicsPath</strong>
        /// object.
        /// </summary>
        public GraphicsPath ToGraphicsPath(PolarCoordinate[] points)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLines(ToPointFArray(points));
            return path;
        }

#endif

        /// <summary>Fills and outlines a polygon using the specified style.</summary>
        public void DrawAndFillPolygon(Pen pen, Brush brush, PolarCoordinate[] points)
        {
#if PocketPC
			Point[] ConvertedPoints = ToPointArray(points);
#else
            PointF[] convertedPoints = ToPointFArray(points);
#endif
            if (brush != null)
                _g.FillPolygon(brush, convertedPoints);
            if (pen != null)
                _g.DrawPolygon(pen, convertedPoints);
        }

        /// <summary>Draws a closed shape.</summary>
        public void DrawPolygon(Pen pen, PolarCoordinate[] points)
        {
#if PocketPC
			//gx.DrawPolygon(new PenX(pen.Color, pen.Width), ToPointArray(points));
            g.DrawPolygon(pen, ToPointArray(points));
#else
            _g.DrawPolygon(pen, ToPointFArray(points));
            //            GraphicsPath Path = new GraphicsPath();
            //            Path.AddLines(ToPointFArray(points));
            //            g.DrawPath(pen, Path);
            //            Path.Dispose();
#endif
        }

#if !PocketPC

        /// <summary>Draws text rotated by the specified amount.</summary>
        public void DrawRotatedString(string s, Font font, Brush brush, PolarCoordinate point)
        {
            PointF h = ToPointF(point);

            _g.RotateTransform(Convert.ToSingle(point.Theta.DecimalDegrees + _pOrigin.DecimalDegrees - _pRotation.DecimalDegrees), MatrixOrder.Append);
            _g.TranslateTransform(h.X, h.Y, MatrixOrder.Append);
            _g.DrawString(s, font, brush, PointF.Empty, _pStringFormat);
            _g.ResetTransform();
        }

#endif

        /// <summary>Converts an array of polar coordinates to an array of pixel coordinates.</summary>
        public Point[] ToPointArray(PolarCoordinate[] points)
        {
            // Convert to an array of PointF objects
            Point[] result = new Point[points.Length];
            for (int index = 0; index < points.Length; index++)
                result[index] = ToPoint(points[index]);
            return result;
        }

#if !PocketPC

        /// <summary>
        /// Converts an array of polar coordinates to an array of precise pixel
        /// coordinates.
        /// </summary>
        public PointF[] ToPointFArray(PolarCoordinate[] points)
        {
            // Convert to an array of PointF objects
            //int Count = points.Length;
            PointF[] result = new PointF[points.Length];
            for (int index = 0; index < points.Length; index++)
                result[index] = ToPointF(points[index]);
            return result;
        }

#endif

        /// <summary>
        /// Converts an array of polar coordinates to an array of highly-precise pixel
        /// coordinates.
        /// </summary>
        public PointD[] ToPointDArray(PolarCoordinate[] points)
        {
            // Convert to an array of PointF objects
            //int Count = points.Length;
            PointD[] result = new PointD[points.Length];
            for (int index = 0; index < points.Length; index++)
                result[index] = ToPointD(points[index]);
            return result;
        }

#if !PocketPC

        /// <summary>Draws a rounded line.</summary>
        public void DrawArc(Pen pen, PolarCoordinate pt1, PolarCoordinate pt2)
        {
            // Calculate pixel coordinates
            PointF start = ToPointF(pt1);
            PointF end = ToPointF(pt2);
            SizeF size = new SizeF(end.X - start.X, end.Y - start.Y);
            // Now draw the arc
            _g.DrawArc(pen, new RectangleF(start, size), (float)pt1.Theta, (float)pt2.Theta);
        }

        /// <summary>Draws a rounded line that travels through several points.</summary>
        public void DrawBezier(Pen pen, PolarCoordinate pt1, PolarCoordinate pt2, PolarCoordinate pt3, PolarCoordinate pt4)
        {
            _g.DrawBezier(pen, ToPointF(pt1), ToPointF(pt2), ToPointF(pt3), ToPointF(pt4));
        }

        /// <summary>Draws multiple rounded lines that travels through several points.</summary>
        public void DrawBeziers(Pen pen, PolarCoordinate[] points)
        {
            _g.DrawBeziers(pen, ToPointFArray(points));
        }

        /// <summary>
        /// Draw Closed Curve
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        public void DrawClosedCurve(Pen pen, PolarCoordinate[] points)
        {
            _g.DrawClosedCurve(pen, ToPointFArray(points));
        }

        /// <summary>
        /// Draw Closed Curve
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        /// <param name="tension"></param>
        /// <param name="fillMode"></param>
        public void DrawClosedCurve(Pen pen, PolarCoordinate[] points, float tension, FillMode fillMode)
        {
            _g.DrawClosedCurve(pen, ToPointFArray(points), tension, fillMode);
        }

        /// <summary>
        /// Draw Curve
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        public void DrawCurve(Pen pen, PolarCoordinate[] points)
        {
            _g.DrawCurve(pen, ToPointFArray(points));
        }

        /// <summary>
        /// Draw Curve
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="points"></param>
        /// <param name="offset"></param>
        /// <param name="numberOfSegments"></param>
        /// <param name="tension"></param>
        public void DrawCurve(Pen pen, PolarCoordinate[] points, int offset, int numberOfSegments, float tension)
        {
            _g.DrawCurve(pen, ToPointFArray(points), offset, numberOfSegments, tension);
        }

#endif

        /// <summary>Returns the GDI+ drawing surface used for painting.</summary>
        public Graphics Graphics
        {
            get
            {
                return _g;
            }
        }

        /// <summary>Returns the amount of rotation applied to the entire control.</summary>
        public Angle Rotation
        {
            get
            {
                return _pRotation;
            }
        }
    }
}
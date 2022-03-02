using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using DotSpatial.Positioning;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif
using System.Reflection;

namespace DotSpatial.Positioning.Drawing
{
    /// <summary>
	/// Encapsulates a GDI+ drawing surface using polar coordinates instead of pixel coordinates.
	/// </summary>
#if !PocketPC || DesignTime
#if Framework20
    [Obfuscation(Feature = "renaming", Exclude = false, ApplyToMembers = true)]
    [Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    [Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
#endif
	public sealed class PolarGraphics 
	{
        private Graphics g;
		private Angle pRotation;
		private Azimuth pOrigin;
		private PolarCoordinateOrientation pOrientation;
		private float pCenterR;
		private float pMaximumR;
		private double HalfWidth;
		private double HalfHeight;
		private double SpanSize;
		private double HorizontalScale;
		private double VerticalScale;
#if !PocketPC
		private static StringFormat pStringFormat = new StringFormat();
#endif

//#if PocketPC
//        [StructLayout(LayoutKind.Sequential)]
//#else
//        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
//#endif
//        private class LOGFONT
//        {
//            public int Height = 0;
//            public int Width = 0;
//            public int Escapement = 0;
//            public int Orientation = 0;
//            public int Weight = 0;
//            public byte Italic = 0;
//            public byte Underline = 0;
//            public byte StrikeOut = 0;
//            public byte CharSet = 0;
//            public byte OutPrecision = 0;
//            public byte ClipPrecision = 0;
//            public byte Quality = 0;
//            public byte PitchAndFamily = 0;
//            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
//            public string FaceName = null;
//        }
//
////**************
////
////            LOGFONT myLogFont = new LOGFONT();
////
////            Font myFont = new Font("Arial", 16);
////            myFont.ToLogFont(myLogFont);

#if !PocketPC
        internal static void Dispose()
        {
            if (pStringFormat != null)
            {
                try
                {
                    pStringFormat.Dispose();
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
			this.g = g;
			pRotation = rotation;
			pOrigin = origin;
			pOrientation = orientation;
			pCenterR = centerR;
			pMaximumR = maximumR;
#if PocketPC
            HalfWidth = (visibleClipBounds.Width * 0.5);
            HalfHeight = (visibleClipBounds.Height * 0.5);


//			// Create smoother graphics
//			gx = new DotSpatial.Positioning.Drawing.GraphicsX(visibleClipBounds.Width, visibleClipBounds.Height);
//			gx.Clear(Color.White);
//			gx.ResetTransform();
#else
			HalfWidth = g.VisibleClipBounds.Width * 0.5;
            HalfHeight = g.VisibleClipBounds.Height * 0.5;
#endif
            SpanSize = Math.Abs(pMaximumR - pCenterR);
			HorizontalScale = HalfWidth / SpanSize;
			VerticalScale = HalfHeight / SpanSize;

#if !PocketPC
			// Set up the string format
			pStringFormat.LineAlignment = StringAlignment.Center;
			pStringFormat.Alignment = StringAlignment.Center;
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
			return coordinate.ToOrientation(pOrigin, pOrientation).Rotate(-pRotation.DecimalDegrees).ToPointD()
				.Multiply(HorizontalScale, VerticalScale).Add(HalfWidth, HalfHeight); 
		}

		public PolarCoordinate ToPolarCoordinate(Point point)
		{
            PointD pointD = new PointD((double)point.X, (double)point.Y);
            return ToPolarCoordinate(pointD);
		}

#if !PocketPC
		public PolarCoordinate ToPolarCoordinate(PointF point)
		{
            PointD pointD = new PointD((double)point.X, (double)point.Y);
            return ToPolarCoordinate(pointD);
        }
#endif

		public PolarCoordinate ToPolarCoordinate(PointD point)
		{
            //return point.Subtract(HalfWidth, HalfHeight).Divide(HorizontalScale, VerticalScale)
            //    .ToPolarCoordinate().Rotate(pRotation.DecimalDegrees).ToOrientation(pOrigin, pOrientation);
            PointD value = point.Subtract(HalfWidth, HalfHeight).Divide(HorizontalScale, VerticalScale);
            PolarCoordinate value2 = PointDToPolarCoordinate(value);
            return value2.Rotate(pRotation.DecimalDegrees).ToOrientation(pOrigin, pOrientation);
        }

        /// <summary>Converts the current instance into a polar coordinate.</summary>
        private PolarCoordinate PointDToPolarCoordinate(PointD value)
        {
            //			double Value;
            //			double TempY = -pY;
            //			if(pX == 0)
            //				Value = 0;
            //			else
            //				Value = TempY / pX;
            // Calculate the coordinate using the default: 0° = East
            // and counter-clockwise movement
            Radian Result = new Radian(Math.Atan2(-value.Y, value.X));
            PolarCoordinate Result2 = new PolarCoordinate((float)Math.Sqrt(value.X * value.X + value.Y * value.Y), Result.ToAngle(), Azimuth.East, PolarCoordinateOrientation.Counterclockwise);
            // And re-orient it at North and Clockwise
            return Result2.ToOrientation(Azimuth.North, PolarCoordinateOrientation.Clockwise);
        }

#if !PocketPC
		/// <summary>Converts a polar coordinate to a precise pixel coordinate.</summary>
		public PointF ToPointF(PolarCoordinate coordinate)
		{
            return ToPointF(ToPointD(coordinate)); //fixes problems with DrawRotatedString
		}
        
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
				return pOrigin;
			}
		}

		/// <summary>
		/// Returns whether positive values are applied in a clockwise or counter-clockwise direction.
		/// </summary>
		public PolarCoordinateOrientation Orientation
		{
			get
			{
				return pOrientation;
			}
		}

		/// <summary>Returns the value of <em>R</em> associated with the center of the control.</summary>
		public float CenterR
		{
			get
			{
				return pCenterR;
			}
		}

		/// <summary>Returns the value of <em>R</em> associated with the edge of the control.</summary>
        public float MaximumR
		{
			get
			{
				return pMaximumR;
			}
		}

		/// <summary>Erases the control's background to the specified color.</summary>
		public void Clear(Color backgroundColor)
		{
			g.Clear(backgroundColor);
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
                g.DrawLine(pen, ToPointF(pt1), ToPointF(pt2));
#endif
		}

#if !PocketPC
		public void DrawString(string s, Font font, Brush brush, PolarCoordinate point, StringFormat format)
		{
			g.DrawString(s, font, brush, ToPointF(point), format);
		}
#endif
        
        public void DrawString(string s, Font font, SolidBrush brush, PolarCoordinate point)
		{
            PointD Location = ToPointD(point);
#if PocketPC
            g.DrawString(s, font, brush, new RectangleF((float)Location.X, (float)Location.Y, 240.0f, 320.0f));
#else
			g.DrawString(s, font, brush, ToPointF(Location));
#endif
		}

#if !PocketPC
        public void DrawCenteredString(string s, Font font, Brush brush, PolarCoordinate point, StringFormat format)
        {
            PointD StartPoint = ToPointD(point);
            SizeF StringSize = g.MeasureString(s, font);
            PointD NewStart = new PointD(StartPoint.X - StringSize.Width * 0.5, StartPoint.Y - StringSize.Height * 0.5);
			g.DrawString(s, font, brush, ToPointF(NewStart), format);
        }
#endif
        public void DrawCenteredString(string s, Font font, SolidBrush brush, PolarCoordinate point)
		{
            PointD StartPoint = ToPointD(point);
            SizeF StringSize = g.MeasureString(s, font);
            PointD NewStart = new PointD(StartPoint.X - StringSize.Width * 0.5, StartPoint.Y - StringSize.Height * 0.5);
#if PocketPC
            g.DrawString(s, font, brush, (float)NewStart.X, (float)NewStart.Y); //, StringSize.Width, StringSize.Height));
#else
			g.DrawString(s, font, brush, ToPointF(NewStart));
#endif
		}

		/// <summary>Draws a square or rectangular shape.</summary>
		public void DrawRectangle(Pen pen, PolarCoordinate upperLeft, PolarCoordinate lowerRight)
		{
            PointD UL = ToPointD(upperLeft);
            PointD LR = ToPointD(lowerRight);
#if PocketPC
            g.DrawRectangle(pen, (int)UL.X, (int)UL.Y, (int)Math.Abs(LR.X - UL.X), (int)Math.Abs(LR.Y - UL.Y));
#else
			g.DrawRectangle(pen, (float)UL.X, (float)UL.Y, (float)Math.Abs(LR.X - UL.X), (float)Math.Abs(LR.Y - UL.Y));
#endif
		}

		/// <summary>Fills the interior of a circular shape.</summary>
		public void FillEllipse(Brush brush, PolarCoordinate center, float radius)
		{
            // Translate the coordinate to the center
            PointD CenterPoint = ToPointD(center);
            PointD ControlCenterPoint = ToPointD(PolarCoordinate.Empty);

            // Calculate the bounding box for the ellipse
            double minX = ControlCenterPoint.X;
            double minY = ControlCenterPoint.Y;
            double maxX = ControlCenterPoint.X;
            double maxY = ControlCenterPoint.Y;
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
            g.FillEllipse(brush, (float)(CenterPoint.X - (width * 0.5)), (float)(CenterPoint.Y - (height * 0.5)), (float)width, (float)height);
#endif
            }

		/// <summary>Draws a circular shape.</summary>
		public void DrawEllipse(Pen pen, PolarCoordinate center, float radius)
		{
			// Translate the coordinate to the center
			PointD CenterPoint = ToPointD(center);
			PointD ControlCenterPoint = ToPointD(PolarCoordinate.Empty);

			// Calculate the bounding box for the ellipse
			double minX = ControlCenterPoint.X;
			double minY = ControlCenterPoint.Y;
			double maxX = ControlCenterPoint.X;
			double maxY = ControlCenterPoint.Y;
			for(int angle = 0; angle < 360; angle += 10)
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
			g.DrawEllipse(pen, (float)(CenterPoint.X - (width * 0.5)), (float)(CenterPoint.Y - (height * 0.5)), (float)width, (float)height);
#endif
			
		}

		/// <summary>Fills the interior of a closed shape.</summary>
		public void FillPolygon(Brush brush, PolarCoordinate[] points)
		{
#if PocketPC
			//gx.FillPolygon(new SolidBrushX(brush.Color), ToPointArray(points));
            g.FillPolygon(brush, ToPointArray(points));
#else
			g.FillPolygon(brush, ToPointFArray(points));
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
			PointF[] ConvertedPoints = ToPointFArray(points);
#endif
			if(brush != null)
				g.FillPolygon(brush, ConvertedPoints);
			if(pen != null)
				g.DrawPolygon(pen, ConvertedPoints);
		}

		/// <summary>Draws a closed shape.</summary>
		public void DrawPolygon(Pen pen, PolarCoordinate[] points)
		{
#if PocketPC
			//gx.DrawPolygon(new PenX(pen.Color, pen.Width), ToPointArray(points));
            g.DrawPolygon(pen, ToPointArray(points));
#else
			g.DrawPolygon(pen, ToPointFArray(points));
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

			g.RotateTransform(Convert.ToSingle(point.Theta.DecimalDegrees + pOrigin.DecimalDegrees - pRotation.DecimalDegrees), MatrixOrder.Append);
			g.TranslateTransform(h.X, h.Y, MatrixOrder.Append);		
			g.DrawString(s, font, brush, PointF.Empty, pStringFormat);
			g.ResetTransform();
		}
#endif

		/// <summary>Converts an array of polar coordinates to an array of pixel coordinates.</summary>
        public Point[] ToPointArray(PolarCoordinate[] points)
		{
			// Convert to an array of PointF objects
			Point[] Result = new Point[points.Length];
			for(int index = 0; index < points.Length; index++)
				Result[index] = ToPoint(points[index]);
			return Result;
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
			PointF[] Result = new PointF[points.Length];
			for(int index = 0; index < points.Length; index++)
				Result[index] = ToPointF(points[index]);
			return Result;
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
			PointD[] Result = new PointD[points.Length];
			for(int index = 0; index < points.Length; index++)
				Result[index] = ToPointD(points[index]);
			return Result;
		}

#if !PocketPC
		/// <summary>Draws a rounded line.</summary>
		public void DrawArc(Pen pen, PolarCoordinate pt1, PolarCoordinate pt2)
		{
			// Calculate pixel coordinates
			PointF Start = ToPointF(pt1);
			PointF End = ToPointF(pt2);
			SizeF size = new SizeF(End.X - Start.X, End.Y - Start.Y);
			// Now draw the arc
			g.DrawArc(pen, new RectangleF(Start, size), (float)pt1.Theta, (float)pt2.Theta);
		}

		/// <summary>Draws a rounded line that travels through several points.</summary>
		public void DrawBezier(Pen pen, PolarCoordinate pt1, PolarCoordinate pt2, PolarCoordinate pt3, PolarCoordinate pt4)
		{
			g.DrawBezier(pen, ToPointF(pt1), ToPointF(pt2), ToPointF(pt3), ToPointF(pt4));
		}

		/// <summary>Draws multiple rounded lines that travels through several points.</summary>
        public void DrawBeziers(Pen pen, PolarCoordinate[] points)
        {
			g.DrawBeziers(pen, ToPointFArray(points));
		}

		public void DrawClosedCurve(Pen pen, PolarCoordinate[] points)
		{
			g.DrawClosedCurve(pen, ToPointFArray(points));
		}

		public void DrawClosedCurve(Pen pen, PolarCoordinate[] points, float tension, FillMode fillMode)
		{
			g.DrawClosedCurve(pen, ToPointFArray(points), tension, fillMode);
		}

		public void DrawCurve(Pen pen, PolarCoordinate[] points)
		{
			g.DrawCurve(pen, ToPointFArray(points));
		}

		public void DrawCurve(Pen pen, PolarCoordinate[] points, int offset, int numberOfSegments, float tension)
		{
			g.DrawCurve(pen, ToPointFArray(points), offset, numberOfSegments, tension);
		}
#endif

		/// <summary>Returns the GDI+ drawing surface used for painting.</summary>
        public Graphics Graphics
		{
			get
			{
				return g;
			}
		}

		/// <summary>Returns the amount of rotation applied to the entire control.</summary>
		public Angle Rotation
		{
			get
			{
				return pRotation;
			}
		}

    }
}

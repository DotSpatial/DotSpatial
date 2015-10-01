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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/11/2009 2:34:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Globalization;
using DotSpatial.Serialization;
using GeoAPI.Geometries;
namespace DotSpatial.Data
{
    /// <summary>
    /// Extent works like an envelope but is faster acting, has a minimum memory profile,
    /// only works in 2D and has no events.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Extent : ICloneable, IExtent//, IRectangle
    {
        /// <summary>
        /// Creates a new instance of Extent. This introduces no error checking and assumes
        /// that the user knows what they are doing when working with this.
        /// </summary>
        public Extent()
        {
            MinX = double.NaN; //changed by jany_ (2015-07-17) default extent is empty because if there is no content there is no extent
            MaxX = double.NaN;
            MinY = double.NaN;
            MaxY = double.NaN;
        }

        /// <summary>
        /// Creates a new extent from the specified ordinates
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMax"></param>
        public Extent(double xMin, double yMin, double xMax, double yMax)
        {
            MinX = xMin;
            MinY = yMin;
            MaxX = xMax;
            MaxY = yMax;
        }

        /// <summary>
        /// Given a long array of doubles, this builds an extent from a small part of that
        /// xmin, ymin, xmax, ymax
        /// </summary>
        /// <param name="values"></param>
        /// <param name="offset"></param>
        public Extent(double[] values, int offset)
        {
            if (values.Length < 4 + offset)
                throw new IndexOutOfRangeException(
                    "The length of the array of double values should be greater than or equal to 4 plus the value of the offset.");

            MinX = values[0 + offset];
            MinY = values[1 + offset];
            MaxX = values[2 + offset];
            MaxY = values[3 + offset];
        }

        /// <summary>
        /// XMin, YMin, XMax, YMax order
        /// </summary>
        /// <param name="values"></param>
        public Extent(double[] values)
        {
            if (values.Length < 4)
                throw new IndexOutOfRangeException("The length of the array of double values should be greater than or equal to 4.");

            MinX = values[0];
            MinY = values[1];
            MaxX = values[2];
            MaxY = values[3];
        }

        /// <summary>
        /// Creates a new extent from the specified envelope
        /// </summary>
        /// <param name="env"></param>
        public Extent(Envelope env)
        {
            if (Equals(env, null))
                throw new ArgumentNullException("env");

            MinX = env.MinX;
            MinY = env.MinY;
            MaxX = env.MaxX;
            MaxY = env.MaxY;
        }

        /// <summary>
        /// Gets the Center of this extent.
        /// </summary>
        public Coordinate Center
        {
            get
            {
                double x = MinX + (MaxX - MinX) / 2;
                double y = MinY + (MaxY - MinY) / 2;
                return new Coordinate(x, y);
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Produces a clone, rather than using this same object.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return new Extent(MinX, MinY, MaxX, MaxY);
        }

        #endregion

        #region IExtent Members

        /// <summary>
        /// Gets or sets whether the M values are used.  M values are considered optional,
        /// and not mandatory.  Unused could mean either bound is NaN for some reason, or
        /// else that the bounds are invalid by the Min being less than the Max.
        /// </summary>
        public virtual bool HasM
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets whether the M values are used.  M values are considered optional,
        /// and not mandatory.  Unused could mean either bound is NaN for some reason, or
        /// else that the bounds are invalid by the Min being less than the Max.
        /// </summary>
        public virtual bool HasZ
        {
            get { return false; }
        }

        /// <summary>
        /// Max X
        /// </summary>
        [Serialize("MaxX")]
        public double MaxX { get; set; }

        /// <summary>
        /// Max Y
        /// </summary>
        [Serialize("MaxY")]
        public double MaxY { get; set; }

        /// <summary>
        /// Min X
        /// </summary>
        [Serialize("MinX")]
        public double MinX { get; set; }

        /// <summary>
        /// Min Y
        /// </summary>
        [Serialize("MinY")]
        public double MinY { get; set; }

        #endregion

        #region IRectangle Members

        /// <summary>
        /// Gets MaxY - MinY.  Setting this will update MinY, keeping MaxY the same.  (Pinned at top left corner).
        /// </summary>
        public double Height
        {
            get { return MaxY - MinY; }
            set { MinY = MaxY - value; }
        }

        /// <summary>
        /// Gets MaxX - MinX.  Setting this will update MaxX, keeping MinX the same. (Pinned at top left corner).
        /// </summary>
        public double Width
        {
            get { return MaxX - MinX; }
            set { MaxX = MinX + value; }
        }

        /// <summary>
        /// Gets MinX.  Setting this will shift both MinX and MaxX, keeping the width the same.
        /// </summary>
        public double X
        {
            get { return MinX; }
            set
            {
                double w = Width;
                MinX = value;
                Width = w;
            }
        }

        /// <summary>
        /// Gets MaxY.  Setting this will shift both MinY and MaxY, keeping the height the same.
        /// </summary>
        public double Y
        {
            get { return MaxY; }
            set
            {
                double h = Height;
                MaxY = value;
                Height = h;
            }
        }

        #endregion

        /// <summary>
        /// Equality test
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Extent left, IExtent right)
        {
            if (((object)left) == null) return ((right) == null);
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality test
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Extent left, IExtent right)
        {
            if (((object)left) == null) return ((right) != null);
            return !left.Equals(right);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public virtual bool Contains(IExtent ext)
        {
            if (Equals(ext, null))
                throw new ArgumentNullException("ext");

            if (MinX > ext.MinX)
            {
                return false;
            }
            if (MaxX < ext.MaxX)
            {
                return false;
            }
            if (MinY > ext.MinY)
            {
                return false;
            }
            return !(MaxY < ext.MaxY);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="c">The coordinate to test.</param>
        /// <returns>Boolean</returns>
        public virtual bool Contains(Coordinate c)
        {
            if (Equals(c, null))
                throw new ArgumentNullException("c");

            if (MinX > c.X)
            {
                return false;
            }
            if (MaxX < c.X)
            {
                return false;
            }
            if (MinY > c.Y)
            {
                return false;
            }
            return !(MaxY < c.Y);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public virtual bool Contains(Envelope env)
        {
            if (Equals(env, null))
                throw new ArgumentNullException("env");

            if (MinX > env.MinX)
            {
                return false;
            }
            if (MaxX < env.MaxX)
            {
                return false;
            }
            if (MinY > env.MinY)
            {
                return false;
            }
            return !(MaxY < env.MaxY);
        }

        /// <summary>
        /// Copies the MinX, MaxX, MinY, MaxY values from extent.
        /// </summary>
        /// <param name="extent">Any IExtent implementation.</param>
        public virtual void CopyFrom(IExtent extent)
        {
            if (Equals(extent, null))
                throw new ArgumentNullException("extent");

            MinX = extent.MinX;
            MaxX = extent.MaxX;
            MinY = extent.MinY;
            MaxY = extent.MaxY;
        }

        /// <summary>
        /// Allows equality testing for extents that is derived on the extent itself.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // Check the identity case for reference equality
            if (base.Equals(obj))
            {
                return true;
            }
            IExtent other = obj as IExtent;
            if (other == null)
            {
                return false;
            }
            if (MinX != other.MinX)
            {
                return false;
            }
            if (MaxX != other.MaxX)
            {
                return false;
            }
            if (MinY != other.MinY)
            {
                return false;
            }
            if (MaxY != other.MaxY)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Expand will adjust both the minimum and maximum by the specified sizeX and sizeY
        /// </summary>
        /// <param name="padX">The amount to expand left and right.</param>
        /// <param name="padY">The amount to expand up and down.</param>
        public void ExpandBy(double padX, double padY)
        {
            MinX -= padX;
            MaxX += padX;
            MinY -= padY;
            MaxY += padY;
        }

        /// <summary>
        /// This expand the extent by the specified padding on all bounds.  So the width will
        /// change by twice the padding for instance.  To Expand only x and y, use
        /// the overload with those values explicitly specified.
        /// </summary>
        /// <param name="padding">The double padding to expand the extent.</param>
        public virtual void ExpandBy(double padding)
        {
            MinX -= padding;
            MaxX += padding;
            MinY -= padding;
            MaxY += padding;
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified extent
        /// </summary>
        /// <param name="ext">The extent to expand to include</param>
        public virtual void ExpandToInclude(IExtent ext)
        {
            if (ext == null) //Simplify, avoiding nested if
                return;

            if (double.IsNaN(MinX) || ext.MinX < MinX)
            {
                MinX = ext.MinX;
            }
            if (double.IsNaN(MinY) || ext.MinY < MinY)
            {
                MinY = ext.MinY;
            }
            if (double.IsNaN(MaxX) || ext.MaxX > MaxX)
            {
                MaxX = ext.MaxX;
            }
            if (double.IsNaN(MaxY) || ext.MaxY > MaxY)
            {
                MaxY = ext.MaxY;
            }
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ExpandToInclude(double x, double y)
        {
            if (double.IsNaN(MinX) || x < MinX)
            {
                MinX = x;
            }
            if (double.IsNaN(MinY) || y < MinY)
            {
                MinY = y;
            }
            if (double.IsNaN(MaxX) || x > MaxX)
            {
                MaxX = x;
            }
            if (double.IsNaN(MaxY) || y > MaxY)
            {
                MaxY = y;
            }
        }

        /// <summary>
        /// Spreads the values for the basic X, Y extents across the whole range of int.
        /// Repetition will occur, but it should be rare.
        /// </summary>
        /// <returns>Integer</returns>
        public override int GetHashCode()
        {
            // 215^4 ~ Int.MaxValue so the value will cover the range based mostly on first 2 sig figs.
            int xmin = Convert.ToInt32(MinX * 430 / MinX - 215);
            int xmax = Convert.ToInt32(MaxX * 430 / MaxX - 215);
            int ymin = Convert.ToInt32(MinY * 430 / MinY - 215);
            int ymax = Convert.ToInt32(MaxY * 430 / MaxY - 215);
            return (xmin * xmax * ymin * ymax);
        }

        /// <summary>
        /// Calculates the intersection of this extent and the other extent.  A result
        /// with a min greater than the max in either direction is considered invalid
        /// and represents no intersection.
        /// </summary>
        /// <param name="other">The other extent to intersect with.</param>
        public virtual Extent Intersection(Extent other)
        {
            if (Equals(other, null))
                throw new ArgumentNullException("other");

            Extent result = new Extent
            {
                MinX = (MinX > other.MinX) ? MinX : other.MinX,
                MaxX = (MaxX < other.MaxX) ? MaxX : other.MaxX,
                MinY = (MinY > other.MinY) ? MinY : other.MinY,
                MaxY = (MaxY < other.MaxY) ? MaxY : other.MaxY
            };
            return result;
        }

        /// <summary>
        /// Returns true if the coordinate exists anywhere within this envelope
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool Intersects(Coordinate c)
        {
            if (Equals(c, null))
                throw new ArgumentNullException("c");

            if (double.IsNaN(c.X) || double.IsNaN(c.Y))
            {
                return false;
            }
            return c.X >= MinX && c.X <= MaxX && c.Y >= MinY && c.Y <= MaxY;
        }

        /// <summary>
        /// Tests for intersection with the specified coordinate
        /// </summary>
        /// <param name="x">The double ordinate to test intersection with in the X direction</param>
        /// <param name="y">The double ordinate to test intersection with in the Y direction</param>
        /// <returns>True if a point with the specified x and y coordinates is within or on the border
        /// of this extent object.  NAN values will always return false.</returns>
        public bool Intersects(double x, double y)
        {
            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return false;
            }
            return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
        }

        /// <summary>
        /// Tests to see if the point is inside or on the boundary of this extent.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public bool Intersects(Vertex vert)
        {
            if (vert.X < MinX)
            {
                return false;
            }
            if (vert.X > MaxX)
            {
                return false;
            }
            if (vert.Y < MinY)
            {
                return false;
            }
            return !(vert.Y > MaxY);
        }

        /// <summary>
        /// Tests for an intersection with the specified extent
        /// </summary>
        /// <param name="ext">The other extent</param>
        /// <returns>Boolean, true if they overlap anywhere, or even touch</returns>
        public virtual bool Intersects(IExtent ext)
        {
            if (Equals(ext, null))
                throw new ArgumentNullException("ext");

            if (ext.MaxX < MinX)
            {
                return false;
            }
            if (ext.MaxY < MinY)
            {
                return false;
            }
            if (ext.MinX > MaxX)
            {
                return false;
            }
            return !(ext.MinY > MaxY);
        }

        /// <summary>
        /// Tests with the specified envelope for a collision
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public virtual bool Intersects(Envelope env)
        {
            if (Equals(env, null))
                throw new ArgumentNullException("env");

            if (env.MaxX < MinX)
            {
                return false;
            }
            if (env.MaxY < MinY)
            {
                return false;
            }
            if (env.MinX > MaxX)
            {
                return false;
            }
            return !(env.MinY > MaxY);
        }

        /// <summary>
        /// If this is undefined, it will have a min that is larger than the max, or else
        /// any value is NaN. This only applies to the X and Z terms. Check HasM or HasZ higher dimensions.
        /// </summary>
        /// <returns>Boolean, true if the envelope has not had values set for it yet.</returns>
        public bool IsEmpty()
        {
            if (double.IsNaN(MinX) || double.IsNaN(MaxX))
            {
                return true;
            }
            if (double.IsNaN(MinY) || double.IsNaN(MaxY))
            {
                return true;
            }
            return (MinX > MaxX || MinY > MaxY); // Simplified
        }

        /// <summary>
        /// This allows parsing the X and Y values from a string version of the extent as:
        /// 'X[-180|180], Y[-90|90]'  Where minimum always precedes maximum.  The correct
        /// M or MZ version of extent will be returned if the string has those values.
        /// </summary>
        /// <param name="text">The string text to parse.</param>
        public static Extent Parse(string text)
        {
            Extent result;
            string fail;
            if (TryParse(text, out result, out fail)) return result;
            var ep = new ExtentParseException(String.Format("Attempting to read an extent string failed while reading the {0} term.", fail))
            {
                Expression = text
            };
            throw ep;
        }

        /// <summary>
        /// This reads the string and attempts to derive values from the text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <param name="nameFailed"></param>
        /// <returns></returns>
        public static bool TryParse(string text, out Extent result, out string nameFailed)
        {
            double xmin, xmax, ymin, ymax, mmin, mmax;
            result = null;
            if (text.Contains("Z"))
            {
                double zmin, zmax;
                nameFailed = "Z";
                ExtentMZ mz = new ExtentMZ();
                if (TryExtract(text, "Z", out zmin, out zmax) == false) return false;
                mz.MinZ = zmin;
                mz.MaxZ = zmax;
                nameFailed = "M";
                if (TryExtract(text, "M", out mmin, out mmax) == false) return false;
                mz.MinM = mmin;
                mz.MaxM = mmax;
                result = mz;
            }
            else if (text.Contains("M"))
            {
                nameFailed = "M";
                ExtentM me = new ExtentM();
                if (TryExtract(text, "M", out mmin, out mmax) == false) return false;
                me.MinM = mmin;
                me.MaxM = mmax;
                result = me;
            }
            else
            {
                result = new Extent();
            }
            nameFailed = "X";
            if (TryExtract(text, "X", out xmin, out xmax) == false) return false;
            result.MinX = xmin;
            result.MaxX = xmax;
            nameFailed = "Y";
            if (TryExtract(text, "Y", out ymin, out ymax) == false) return false;
            result.MinY = ymin;
            result.MaxY = ymax;
            return true;
        }

        /// <summary>
        /// This centers the X and Y aspect of the extent on the specified center location.
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetCenter(double centerX, double centerY, double width, double height)
        {
            MinX = centerX - width / 2;
            MaxX = centerX + width / 2;
            MinY = centerY - height / 2;
            MaxY = centerY + height / 2;
        }

        /// <summary>
        /// This centers the X and Y aspect of the extent on the specified center location.
        /// </summary>
        /// <param name="center">The center coordinate to to set.</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetCenter(Coordinate center, double width, double height)
        {
            SetCenter(center.X, center.Y, width, height);
        }

        /// <summary>
        /// This centers the extent on the specified coordinate, keeping the width and height the same.
        /// </summary>
        public void SetCenter(Coordinate center)
        {
            //prevents NullReferenceException when accessing center.X and center.Y
            if (Equals(center, null))
                throw new ArgumentNullException("center");

            SetCenter(center.X, center.Y, Width, Height);
        }

        /// <summary>
        /// Sets the values for xMin, xMax, yMin and yMax.
        /// </summary>
        /// <param name="minX">The double Minimum in the X direction.</param>
        /// <param name="minY">The double Minimum in the Y direction.</param>
        /// <param name="maxX">The double Maximum in the X direction.</param>
        /// <param name="maxY">The double Maximum in the Y direction.</param>
        public void SetValues(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        /// <summary>
        /// Creates a geometric envelope interface from this
        /// </summary>
        /// <returns></returns>
        public Envelope ToEnvelope()
        {
            return new Envelope(MinX, MaxX, MinY, MaxY);
        }

        /// <summary>
        /// Creates a string that shows the extent.
        /// </summary>
        /// <returns>The string form of the extent.</returns>
        public override string ToString()
        {
            return "X[" + MinX + "|" + MaxX + "], Y[" + MinY + "|" + MaxY + "]";
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public virtual bool Within(IExtent ext)
        {
            if (Equals(ext, null))
                throw new ArgumentNullException("ext");

            if (MinX < ext.MinX)
            {
                return false;
            }
            if (MaxX > ext.MaxX)
            {
                return false;
            }
            if (MinY < ext.MinY)
            {
                return false;
            }
            return !(MaxY > ext.MaxY);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public virtual bool Within(Envelope env)
        {
            if (Equals(env, null))
                throw new ArgumentNullException("env");

            if (MinX < env.MinX)
            {
                return false;
            }
            if (MaxX > env.MaxX)
            {
                return false;
            }
            if (MinY < env.MinY)
            {
                return false;
            }
            return !(MaxY > env.MaxY);
        }

        /// <summary>
        /// Attempts to extract the min and max from one element of text.  The element should be
        /// formatted like X[1.5, 2.7] using an invariant culture.
        /// </summary>
        /// <param name="entireText"></param>
        /// <param name="name">The name of the dimension, like X.</param>
        /// <param name="min">The minimum that gets assigned</param>
        /// <param name="max">The maximum that gets assigned</param>
        /// <returns>Boolean, true if the parse was successful.</returns>
        private static bool TryExtract(string entireText, string name, out double min, out double max)
        {
            int i = entireText.IndexOf(name, StringComparison.Ordinal);
            i += name.Length + 1;
            int j = entireText.IndexOf(']', i);
            string vals = entireText.Substring(i, j - i);
            return TryParseExtremes(vals, out min, out max);
        }

        /// <summary>
        /// This method converts the X and Y text into two doubles.
        /// </summary>
        /// <returns></returns>
        private static bool TryParseExtremes(string numeric, out double min, out double max)
        {
            string[] res = numeric.Split('|');
            max = double.NaN;
            if (double.TryParse(res[0].Trim(), NumberStyles.Any,
                CultureInfo.InvariantCulture, out min) == false) return false;
            if (double.TryParse(res[1].Trim(), NumberStyles.Any,
                CultureInfo.InvariantCulture, out max) == false) return false;
            return true;
        }
    }
}
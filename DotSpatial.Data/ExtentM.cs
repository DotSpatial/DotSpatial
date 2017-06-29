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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2010 9:30:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.ComponentModel;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// The ExtentsM class extends the regular X and Y extent to the X, Y and M case.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExtentM : Extent, IExtentM
    {
        /// <summary>
        /// Initializes a new instance of the ExtentM class.
        /// </summary>
        public ExtentM()
        {
            MinM = double.MaxValue;
            MaxM = double.MinValue;
        }

        /// <summary>
        /// Initializes a new instance of the ExtentM class.
        /// </summary>
        /// <param name="minX">The double Minimum in the X direction.</param>
        /// <param name="minY">The double Minimum in the Y direction.</param>
        /// <param name="minM">The double Minimum in the Measure category.</param>
        /// <param name="maxX">The double Maximum in the X direction.</param>
        /// <param name="maxY">The double Maximum in the Y direction.</param>
        ///  <param name="maxM">The double Maximum in the Measure category.</param>
        public ExtentM(double minX, double minY, double minM, double maxX, double maxY, double maxM)
        {
            SetValues(minX, minY, minM, maxX, maxY, maxM);
        }

        /// <summary>
        /// Initializes a new instance of the ExtentM class that is specially designed to work
        /// with shapefile formats that have a Measure value.  Obviously other formats can use
        /// this as well.
        /// </summary>
        /// <param name="xyExtent">An extent that contains only the x and y boundaries.</param>
        /// <param name="minM">The minimum M.</param>
        /// <param name="maxM">The maximum M.</param>
        public ExtentM(IExtent xyExtent, double minM, double maxM)
        {
            base.CopyFrom(xyExtent);
            MinM = minM;
            MaxM = maxM;
        }

        /// <summary>
        /// Initializes a new instance of the ExtentM class.  This overload works from an envelope.
        /// </summary>
        /// <param name="env">The envelope with extent values to read.</param>
        public ExtentM(IEnvelope env)
        {
            SetValues(env.Minimum.X, env.Minimum.Y, env.Minimum.M, env.Maximum.X, env.Maximum.Y, env.Maximum.M);
        }

        /// <summary>
        /// Gets or sets whether the M values are used.  M values are considered optional,
        /// and not mandatory.  Unused could mean either bound is NaN for some reason, or
        /// else that the bounds are invalid by the Min being less than the Max.
        /// </summary>
        public override bool HasM
        {
            get
            {
                if (double.IsNaN(MinM) || double.IsNaN(MaxM))
                {
                    return false;
                }
                return MinM <= MaxM;
            }
        }

        #region IExtentM Members

        /// <inheritdoc/>
        [Serialize("MaxM")]
        public double MaxM { get; set; }

        /// <inheritdoc/>
        [Serialize("MinM")]
        public double MinM { get; set; }

        #endregion

        /// <summary>
        /// Produces a clone, rather than using this same object.
        /// </summary>
        /// <returns>Returns a copy of this object.</returns>
        public override object Clone()
        {
            ExtentM copy = new ExtentM(MinX, MinY, MinM, MaxX, MaxY, MaxM);
            return copy;
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="c">The coordinate to test.</param>
        /// <returns>Boolean.</returns>
        public override bool Contains(Coordinate c)
        {
            if (HasM && !double.IsNaN(c.M))
            {
                if (MaxM < c.M)
                {
                    return false;
                }
                if (MinM > c.M)
                {
                    return false;
                }
            }
            return base.Contains(c);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public override bool Contains(IExtent ext)
        {
            IExtentM mExt = ext as IExtentM;
            if (mExt != null && ext.HasM && HasM)
            {
                if (mExt.MaxM > MaxM)
                {
                    return false;
                }
                if (mExt.MinM < MinM)
                {
                    return false;
                }
            }
            return base.Within(ext);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope.  If either element
        /// does not support M values, then only the default X and Y contains test is used.
        /// </summary>
        /// <param name="env">The envelope to test.</param>
        /// <returns>Boolean.</returns>
        public override bool Contains(IEnvelope env)
        {
            if (!double.IsNaN(env.Minimum.M) && !double.IsNaN(env.Maximum.M) && HasM)
            {
                if (env.Maximum.M < MinM)
                {
                    return false;
                }
                if (env.Minimum.M > MaxM)
                {
                    return false;
                }
            }
            return base.Contains(env);
        }

        /// <summary>
        /// Copies from the implementation of IExtent.  This checks to see if IExtentM is implemented
        /// and if not, this only sets the X and Y bounds.
        /// </summary>
        /// <param name="extent"></param>
        public override void CopyFrom(IExtent extent)
        {
            base.CopyFrom(extent);
            IExtentM mvals = extent as IExtentM;
            if (mvals == null || (double.IsNaN(mvals.MinM) || double.IsNaN(mvals.MaxM)))
            {
                MinM = double.MaxValue;
                MaxM = double.MinValue;
            }
        }

        /// <summary>
        /// This expand the extent by the specified padding on all bounds.  So the width will
        /// change by twice the padding for instance.  To Expand only x and y, use
        /// the overload with those values explicitly specified.
        /// </summary>
        /// <param name="padding">The double padding to expand the extent.</param>
        public override void ExpandBy(double padding)
        {
            if (HasM)
            {
                MinM -= padding;
                MaxM += padding;
            }
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified extent.  If the specified case
        /// doesn't support IExtentM or HasM is false for that extent, then this test will default
        /// to the XY case.
        /// </summary>
        /// <param name="ext">The extent to expand to include.</param>
        public override void ExpandToInclude(IExtent ext)
        {
            IExtentM mExt = ext as IExtentM;
            if (mExt != null && ext.HasM && HasM)
            {
                if (mExt.MinM < MinM)
                {
                    MinM = mExt.MinM;
                }
                if (mExt.MaxM > MaxM)
                {
                    MaxM = mExt.MaxM;
                }
            }
            base.ExpandToInclude(ext);
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified point
        /// </summary>
        /// <param name="x">The x ordinate to expand to.</param>
        /// <param name="y">The y ordinate to expand to.</param>
        /// <param name="m">The m ordinate to expand to.</param>
        public void ExpandToInclude(double x, double y, double m)
        {
            if (HasM && !double.IsNaN(m))
            {
                if (m < MinM)
                {
                    MinM = m;
                }
                if (m > MaxM)
                {
                    MaxM = m;
                }
            }
            ExpandToInclude(x, y);
        }

        /// <summary>
        /// Calculates the intersection of this extent and the other extent.  A result
        /// with a min greater than the max in either direction is considered invalid
        /// and represents no intersection.
        /// </summary>
        /// <param name="other">The other extent to intersect with.</param>
        public override Extent Intersection(Extent other)
        {
            IExtentM mOther = other as IExtentM;
            Extent result;
            if (HasM && mOther != null && other.HasM)
            {
                ExtentM mResult = new ExtentM
                                  {
                                      MinM = (MinM > mOther.MinM) ? MinM : mOther.MinM,
                                      MaxM = (MaxM < mOther.MaxM) ? MaxM : mOther.MaxM
                                  };

                result = mResult;
            }
            else
            {
                result = new Extent();
            }

            result.MinX = (MinX > other.MinX) ? MinX : other.MinX;
            result.MaxX = (MaxX < other.MaxX) ? MaxX : other.MaxX;
            result.MinY = (MinY > other.MinY) ? MinY : other.MinY;
            result.MaxY = (MaxY < other.MaxY) ? MaxY : other.MaxY;
            return result;
        }

        /// <summary>
        /// Returns true if the coordinate exists anywhere within this envelope.  If this
        /// envelope represents a valid M extent by having a max greater than min and
        /// neither value being NaN, then this will also test the coordinate for the
        /// M range.
        /// </summary>
        /// <param name="c">The Coordinate to test.</param>
        /// <returns>Boolean</returns>
        public override bool Intersects(Coordinate c)
        {
            if ((HasM && !double.IsNaN(c.M)) && (c.M < MinM || c.M > MaxM))
            {
                return false;
            }
            return base.Intersects(c);
        }

        /// <summary>
        /// Tests for intersection with the specified coordinate.  If the m is double.NaN
        /// then it degenerates to only testing X and Y, even if this envelope has an M range.
        /// </summary>
        /// <param name="x">The double ordinate to test intersection with in the X direction</param>
        /// <param name="y">The double ordinate to test intersection with in the Y direction</param>
        /// <param name="m">The optional double measure parameter to test.</param>
        /// <returns>Boolean</returns>
        public bool Intersects(double x, double y, double m)
        {
            // Both parties must opt into an M comparison.
            if ((HasM && !double.IsNaN(m)) && (m < MinM || m > MaxM))
            {
                return false;
            }
            return Intersects(x, y);
        }

        /// <summary>
        /// Tests for an intersection with the specified extent.  Both this extent and the
        /// other must implement IExtentM and HasM must be true for both, or else just
        ///  the X and Y are compared.
        /// </summary>
        /// <param name="ext">The other extent.  If the extent doesn't implement IExtentM, then
        /// this comparison simply defaults to the X Y intersect case.</param>
        /// <returns>Boolean, true if they overlap anywhere, or even touch.</returns>
        public override bool Intersects(IExtent ext)
        {
            IExtentM mExt = ext as IExtentM;
            if (mExt != null && ext.HasM && HasM)
            {
                if (mExt.MaxM < MinM)
                {
                    return false;
                }
                if (mExt.MinM > MaxM)
                {
                    return false;
                }
            }
            return base.Intersects(ext);
        }

        /// <summary>
        /// Tests with the specified envelope for a collision.  If any part of the M bounds
        /// are invalid, this will default to the XY Intersect comparison.
        /// </summary>
        /// <param name="env">The envelope to test.</param>
        /// <returns>Boolean.</returns>
        public override bool Intersects(IEnvelope env)
        {
            if (!double.IsNaN(env.Minimum.M) && !double.IsNaN(env.Maximum.M) && HasM)
            {
                if (env.Maximum.M < MinM)
                {
                    return false;
                }
                if (env.Minimum.M > MaxM)
                {
                    return false;
                }
            }
            return base.Intersects(env);
        }

        /// <summary>
        /// Since M values are optional, they can be set to an invalid state, which will behave the
        /// same as if the M bounds did not exist.
        /// </summary>
        public void RemoveM()
        {
            MinM = double.MaxValue;
            MaxM = double.MinValue;
        }

        /// <summary>
        /// Sets the values for xMin, xMax, yMin and yMax.
        /// </summary>
        /// <param name="minX">The double Minimum in the X direction.</param>
        /// <param name="minY">The double Minimum in the Y direction.</param>
        /// <param name="minM">The double Minimum in the Measure category.</param>
        /// <param name="maxX">The double Maximum in the X direction.</param>
        /// <param name="maxY">The double Maximum in the Y direction.</param>
        ///  <param name="maxM">The double Maximum in the Measure category.</param>
        public void SetValues(double minX, double minY, double minM, double maxX, double maxY, double maxM)
        {
            SetValues(minX, minY, maxX, maxY);
            MinM = minM;
            MaxM = maxM;
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope.  If either party doesn't have
        /// M constraints, they will not be used for this test.
        /// </summary>
        /// <param name="ext">implementation of IExtent to compare to.</param>
        /// <returns></returns>
        public override bool Within(IExtent ext)
        {
            IExtentM mExt = ext as IExtentM;
            if (mExt != null && ext.HasM && HasM)
            {
                if (mExt.MaxM < MaxM)
                {
                    return false;
                }
                if (mExt.MinM > MinM)
                {
                    return false;
                }
            }
            return base.Within(ext);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope.  If either envelope doesn't
        /// support M then only the X and Y case will be tested.
        /// </summary>
        /// <param name="env">The envelope to compare.</param>
        /// <returns>Boolean.</returns>
        public override bool Within(IEnvelope env)
        {
            if (!double.IsNaN(env.Minimum.M) && !double.IsNaN(env.Maximum.M) && HasM)
            {
                if (env.Maximum.M > MinM)
                {
                    return false;
                }
                if (env.Minimum.M < MaxM)
                {
                    return false;
                }
            }
            return base.Within(env);
        }

        /// <summary>
        /// Allows equality testing for extents that is derived on the extent itself.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            IExtent other = obj as IExtent;
            if (other == null) return false;
            IExtentM mother = other as IExtentM;
            // If either party claims it has no M values, then w can ignore that part of the equality check.
            if (!HasM || !other.HasM || mother == null) return base.Equals(obj);
            if (MinM != mother.MinM) return false;
            if (MaxM != mother.MaxM) return false;
            return base.Equals(obj);
        }

        /// <summary>
        /// Creates a string that shows the extent.
        /// </summary>
        /// <returns>The string form of the extent.</returns>
        public override string ToString()
        {
            return "X[" + MinX + "|" + MaxX + "], Y[" + MinY + "|" + MaxY + "]" +
                "M[" + MinM + "|" + MaxM + "]";
        }

        /// <summary>
        /// Spreads the values for the basic X, Y extents across the whole range of int.
        /// Repetition will occur, but it should be rare.
        /// </summary>
        /// <returns>Integer</returns>
        public override int GetHashCode()
        {
            if (!HasM) return base.GetHashCode();
            // 14^6 ~ Int32.MaxValue so spread across the range based on first sig fig of values.
            int xmin = Convert.ToInt32(MinX * 28 / MinX - 14);
            int xmax = Convert.ToInt32(MaxX * 28 / MaxX - 14);
            int ymin = Convert.ToInt32(MinY * 28 / MinY - 14);
            int ymax = Convert.ToInt32(MaxY * 28 / MaxY - 14);
            int mmin = Convert.ToInt32(MinM * 28 / MinM - 14);
            int mmax = Convert.ToInt32(MaxM * 28 / MinM - 14);
            return (xmin * xmax * ymin * ymax * mmin * mmax);
        }
    }
}
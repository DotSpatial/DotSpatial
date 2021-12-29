// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using DotSpatial.Serialization;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// For shapefiles, the extents may or may not have M and Z values depending on the file type.
    /// These are valid IExtent implementations, but add both the M and Z extent options.
    /// For the file format, the Z values are mandatory, and so whatever the values are will be
    /// stored. This may cause issues when viewing in ArcGIS if you have entered some bad Z
    /// envelope extents.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExtentMz : ExtentM, IExtentZ
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentMz"/> class.
        /// </summary>
        public ExtentMz()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentMz"/> class.
        /// </summary>
        /// <param name="minX">The double Minimum in the X direction.</param>
        /// <param name="minY">The double Minimum in the Y direction.</param>
        /// <param name="minM">The double Minimum in the Measure category.</param>
        /// <param name="minZ">The double Minimum in the Z direction.</param>
        /// <param name="maxX">The double Maximum in the X direction.</param>
        /// <param name="maxY">The double Maximum in the Y direction.</param>
        /// <param name="maxM">The double Maximum in the Measure category.</param>
        /// <param name="maxZ">The double Maximum in the Z direction.</param>
        public ExtentMz(double minX, double minY, double minM, double minZ, double maxX, double maxY, double maxM, double maxZ)
        {
            SetValues(minX, minY, minM, minZ, maxX, maxY, maxM, maxZ);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentMz"/> class.
        /// </summary>
        /// <param name="env">The Envelope to read the minimum and maximum values from.</param>
        /// <param name="minM">The double Minimum in the Measure category.</param>
        /// <param name="minZ">The double Minimum in the Z direction.</param>
        /// <param name="maxM">The double Maximum in the Measure category.</param>
        /// <param name="maxZ">The double Maximum in the Z direction.</param>
        public ExtentMz(Envelope env, double minM, double minZ, double maxM, double maxZ)
        {
            SetValues(env.MinX, env.MinY, minM, minZ, env.MaxX, env.MaxY, maxM, maxZ);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the Z values are used. Z values are considered optional,
        /// and not mandatory. Unused could mean either bound is NaN for some reason, or
        /// else that the bounds are invalid by the Min being less than the Max.
        /// </summary>
        public override bool HasZ
        {
            get
            {
                if (double.IsNaN(MinZ) || double.IsNaN(MaxZ))
                {
                    return false;
                }

                return MinZ <= MaxZ;
            }
        }

        /// <inheritdoc/>
        [Serialize("MaxZ")]
        public double MaxZ { get; set; }

        /// <inheritdoc/>
        [Serialize("MinZ")]
        public double MinZ { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Produces a clone, rather than using this same object.
        /// </summary>
        /// <returns>Returns a copy of this object.</returns>
        public override object Clone()
        {
            ExtentMz copy = new ExtentMz(MinX, MinY, MinM, MinZ, MaxX, MaxY, MaxM, MinZ);
            return copy;
        }

        /// <summary>
        /// Tests if this contains the given coordinate.
        /// </summary>
        /// <param name="c">The coordinate to test.</param>
        /// <returns>True, if this contains the coordinate.</returns>
        public override bool Contains(Coordinate c)
        {
            if (HasZ && !double.IsNaN(c.Z))
            {
                if (MaxZ < c.Z || MinZ > c.Z) return false;
            }

            return base.Contains(c);
        }

        /// <summary>
        /// Tests if this contains the specified extent. If either element
        /// does not support M values, then only the default XY contains test is used.
        /// </summary>
        /// <param name="ext">The extent to test.</param>
        /// <returns>True, if this contains the specified extent.</returns>
        public override bool Contains(IExtent ext)
        {
            if (ext is IExtentZ mExt && ext.HasZ && HasZ)
            {
                if (mExt.MaxZ < MinZ || mExt.MinZ > MaxZ) return false;
            }

            return base.Contains(ext);
        }

        /// <summary>
        /// Tests if this contains the specified envelope. If either element
        /// does not support M values, then only the default XY contains test is used.
        /// </summary>
        /// <param name="env">The envelope to test.</param>
        /// <returns>True if this contains the given envelope.</returns>
        public override bool Contains(Envelope env)
        {
            return base.Contains(env);
        }

        /// <summary>
        /// Copies from the implementation of IExtent. This checks to see if IExtentM is implemented
        /// and if not, this only sets the X and Y bounds.
        /// </summary>
        /// <param name="extent">Extent to copy from.</param>
        public override void CopyFrom(IExtent extent)
        {
            base.CopyFrom(extent);
            if (!(extent is IExtentZ mvals) || double.IsNaN(mvals.MinZ) || double.IsNaN(mvals.MaxZ))
            {
                MinZ = double.MaxValue;
                MaxZ = double.MinValue;
            }
        }

        /// <summary>
        /// Allows equality testing for extents that is derived on the extent itself.
        /// </summary>
        /// <param name="obj">The other IExtent.</param>
        /// <returns>True, if the extents are equal.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is IExtent other)) return false;

            // If either party claims it has no Z values, then ignore that part of the equality check.
            if (!HasZ || !other.HasM || !(other is IExtentZ zother)) return base.Equals(obj);
            if (MinZ != zother.MinZ) return false;
            if (MaxZ != zother.MaxZ) return false;

            return base.Equals(obj);
        }

        /// <summary>
        /// This expand the extent by the specified padding on all bounds. So the width will
        /// change by twice the padding for instance. To Expand only x and y, use
        /// the overload with those values explicitly specified.
        /// </summary>
        /// <param name="padding">The double padding to expand the extent.</param>
        public override void ExpandBy(double padding)
        {
            if (HasZ)
            {
                MinZ -= padding;
                MaxZ += padding;
            }
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified extent. If the specified case
        /// doesn't support IExtentM or HasM is false for that extent, then this test will default
        /// to the M, X and Y case.
        /// </summary>
        /// <param name="ext">The extent to expand to include.</param>
        public override void ExpandToInclude(IExtent ext)
        {
            if (ext is IExtentZ mExt && ext.HasZ && HasZ)
            {
                if (mExt.MinZ < MinZ) MinZ = mExt.MinZ;
                if (mExt.MaxZ > MaxZ) MaxZ = mExt.MaxZ;
            }

            base.ExpandToInclude(ext);
        }

        /// <summary>
        /// Expands this extent to include the domain of the specified point.
        /// </summary>
        /// <param name="x">The x ordinate to expand to.</param>
        /// <param name="y">The y ordinate to expand to.</param>
        /// <param name="m">The m ordinate to expand to.</param>
        /// <param name="z">The z ordinate to expand to.</param>
        public void ExpandToInclude(double x, double y, double m, double z)
        {
            if (HasZ && !double.IsNaN(z))
            {
                if (z < MinZ) MinZ = m;
                if (z > MaxZ) MaxZ = m;
            }

            ExpandToInclude(x, y, m);
        }

        /// <summary>
        /// Spreads the values for the basic X, Y extents across the whole range of int.
        /// Repetition will occur for extents that are close, but should be rare.
        /// </summary>
        /// <returns>Integer.</returns>
        public override int GetHashCode()
        {
            if (!HasZ) return base.GetHashCode();

            // 3.6^8 ~ Int32.MaxValue so spread across the range based on first sig fig of values.
            int xmin = Convert.ToInt32((MinX * 7.6 / MinX) - 3.8);
            int xmax = Convert.ToInt32((MaxX * 7.6 / MaxX) - 3.8);
            int ymin = Convert.ToInt32((MinY * 7.6 / MinY) - 3.8);
            int ymax = Convert.ToInt32((MaxY * 7.6 / MaxY) - 3.8);
            int mmin = Convert.ToInt32((MinM * 7.6 / MinM) - 3.8);
            int mmax = Convert.ToInt32((MaxM * 7.6 / MinM) - 3.8);
            int zmin = Convert.ToInt32((MinZ * 7.6 / MinZ) - 3.8);
            int zmax = Convert.ToInt32((MaxZ * 7.6 / MaxZ) - 3.8);
            return xmin * xmax * ymin * ymax * mmin * mmax * zmin * zmax;
        }

        /// <summary>
        /// Calculates the intersection of this extent and the other extent. A result
        /// with a min greater than the max in either direction is considered invalid
        /// and represents no intersection.
        /// </summary>
        /// <param name="other">The other extent to intersect with.</param>
        /// <returns>The resulting intersection.</returns>
        public override Extent Intersection(Extent other)
        {
            Extent result = null;

            if (HasZ && other is IExtentZ zOther && other.HasZ)
            {
                ExtentMz zResult = new ExtentMz
                {
                    MinZ = MinZ > zOther.MinZ ? MinZ : zOther.MinZ,
                    MaxZ = MaxZ < zOther.MaxZ ? MaxZ : zOther.MaxZ
                };

                result = zResult;
            }

            if (HasM && other is IExtentM mOther && other.HasM)
            {
                ExtentM mResult = (ExtentM)result ?? new ExtentM();
                mResult.MinM = MinM > mOther.MinM ? MinM : mOther.MinM;
                mResult.MaxM = MaxM < mOther.MaxM ? MaxM : mOther.MaxM;
                result = mResult;
            }
            else
            {
                result = new Extent();
            }

            result.MinX = MinX > other.MinX ? MinX : other.MinX;
            result.MaxX = MaxX < other.MaxX ? MaxX : other.MaxX;
            result.MinY = MinY > other.MinY ? MinY : other.MinY;
            result.MaxY = MaxY < other.MaxY ? MaxY : other.MaxY;
            return result;
        }

        /// <summary>
        /// Returns true if the coordinate exists anywhere within this envelope. If this
        /// envelope represents a valid Z extent by having a max greater than min and
        /// neither value being NaN, then this will also test the coordinate for the
        /// Z range.
        /// </summary>
        /// <param name="c">The Coordinate to test.</param>
        /// <returns>Boolean.</returns>
        public override bool Intersects(Coordinate c)
        {
            if ((HasZ && !double.IsNaN(c.Z)) && (c.Z < MinZ || c.Z > MaxZ))
            {
                return false;
            }

            return base.Intersects(c);
        }

        /// <summary>
        /// Tests for intersection with the specified coordinate. If the m is double.NaN
        /// then it degenerates to only testing M, X and Y, even if this envelope has an M range.
        /// </summary>
        /// <param name="x">The double ordinate to test intersection with in the X direction.</param>
        /// <param name="y">The double ordinate to test intersection with in the Y direction.</param>
        /// <param name="m">The optional double measure parameter to test.</param>
        /// <param name="z">The double ordinate to test intersection with in the Z direction.</param>
        /// <returns>Boolean.</returns>
        public bool Intersects(double x, double y, double m, double z)
        {
            // Both parties must opt into an M comparison.
            if ((HasZ && !double.IsNaN(z)) && (z < MinZ || z > MaxZ)) return false;

            return Intersects(x, y, m);
        }

        /// <summary>
        /// Tests for an intersection with the specified extent. Both this extent and the
        /// other must implement IExtentM and HasM must be true for both, or else just
        ///  the X and Y are compared.
        /// </summary>
        /// <param name="ext">The other extent. If the extent doesn't implement IExtentM, then
        /// this comparison simply defaults to the M, X and Y intersect case.</param>
        /// <returns>Boolean, true if they overlap anywhere, or even touch.</returns>
        public override bool Intersects(IExtent ext)
        {
            if (ext is IExtentZ mExt && ext.HasZ && HasZ && (mExt.MaxZ < MinZ || mExt.MinZ > MaxZ)) return false;

            return base.Intersects(ext);
        }

        /// <summary>
        /// Tests with the specified envelope for a collision. This will default to the X and Y Intersect comparison.
        /// </summary>
        /// <param name="env">The envelope to test.</param>
        /// <returns>Boolean.</returns>
        public override bool Intersects(Envelope env)
        {
            return base.Intersects(env);
        }

        /// <summary>
        /// Since M values are optional, they can be set to an invalid state, which will behave the
        /// same as if the M bounds did not exist.
        /// </summary>
        public void RemoveZ()
        {
            MinM = double.MaxValue;
            MaxZ = double.MinValue;
        }

        /// <summary>
        /// Sets the values for xZin, xMax, yMin and yMax.
        /// </summary>
        /// <param name="minX">The double Minimum in the X direction.</param>
        /// <param name="minY">The double Minimum in the Y direction.</param>
        /// <param name="minM">The double Minimum in the Measure category.</param>
        /// <param name="minZ">The double Minimum in the Z direction.</param>
        /// <param name="maxX">The double Maximum in the X direction.</param>
        /// <param name="maxY">The double Maximum in the Y direction.</param>
        /// <param name="maxM">The double Maximum in the Measure category.</param>
        /// <param name="maxZ">The double Maximum in the Z direction.</param>
        public void SetValues(double minX, double minY, double minM, double minZ, double maxX, double maxY, double maxM, double maxZ)
        {
            SetValues(minX, minY, minM, maxX, maxY, maxM);
            MinZ = minZ;
            MaxZ = maxZ;
        }

        /// <summary>
        /// Creates a string that shows the extent.
        /// </summary>
        /// <returns>The string form of the extent.</returns>
        public override string ToString()
        {
            return "X[" + MinX + "|" + MaxX + "], Y[" + MinY + "|" + MaxY + "]" + "M[" + MinM + "|" + MaxM + "], Z[" + MinZ + "|" + MaxZ + "]";
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope. If either party doesn't have
        /// M constraints, they will not be used for this test.
        /// </summary>
        /// <param name="ext">implementation of IExtent to compare to.</param>
        /// <returns>Boolean.</returns>
        public override bool Within(IExtent ext)
        {
            if (ext is IExtentZ mExt && ext.HasZ && HasZ)
            {
                if (mExt.MaxZ < MaxZ || mExt.MinZ > MinZ) return false;
            }

            return base.Within(ext);
        }

        /// <summary>
        /// Tests if this envelope is contained by the specified envelope. Only the XY case will be tested.
        /// </summary>
        /// <param name="env">The envelope to compare.</param>
        /// <returns>Boolean.</returns>
        public override bool Within(Envelope env)
        {
            return base.Within(env);
        }

        #endregion
    }
}
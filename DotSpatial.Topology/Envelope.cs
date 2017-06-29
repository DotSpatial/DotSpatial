// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the source code
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.ComponentModel;
using DotSpatial.Serialization;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Defines a rectangular region of the 2D coordinate plane.
    /// It is often used to represent the bounding box of a <c>Geometry</c>,
    /// e.g. the minimum and maximum x and y values of the <c>Coordinate</c>s.
    /// Notice that Envelopes support infinite or half-infinite regions, by using the values of
    /// <c>Double.PositiveInfinity</c> and <c>Double.NegativeInfinity</c>.
    /// When Envelope objects are created or initialized,
    /// the supplies extent values are automatically sorted into the correct order.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Envelope : IEnvelope
    {
        #region Variables

        [Serialize("Max")]
        private Coordinate _max; // contains maximum value in each dimenions

        [Serialize("Min")]
        private Coordinate _min; // contains X, Y, Z for minimum value in each dimension

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a null <c>Envelope</c>.
        /// </summary>
        public Envelope()
        {
            DoInit();
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by maximum and minimum values.
        /// </summary>
        /// <param name="x1">The first x-value.</param>
        /// <param name="x2">The second x-value.</param>
        /// <param name="y1">The first y-value.</param>
        /// <param name="y2">The second y-value.</param>
        public Envelope(double x1, double x2, double y1, double y2)
        {
            Coordinate min = new Coordinate(Math.Min(x1, x2), Math.Min(y1, y2));
            Coordinate max = new Coordinate(Math.Max(x1, x2), Math.Max(y1, y2));
            DoInit(min, max);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by maximum and minimum values.
        /// </summary>
        /// <param name="x1">The first x-value.</param>
        /// <param name="x2">The second x-value.</param>
        /// <param name="y1">The first y-value.</param>
        /// <param name="y2">The second y-value.</param>
        /// <param name="z1">The first z-value.</param>
        /// <param name="z2">The second z-value.</param>
        public Envelope(double x1, double x2, double y1, double y2, double z1, double z2)
        {
            Coordinate min = new Coordinate(Math.Min(x1, x2), Math.Min(y1, y2), Math.Min(z1, z2));
            Coordinate max = new Coordinate(Math.Max(x1, x2), Math.Max(y1, y2), Math.Max(z1, z2));
            DoInit(min, max);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by an Vector.IEnvelope
        /// </summary>
        /// <param name="inEnvelope">The IEnvelope to create an envelope from</param>
        public Envelope(IEnvelope inEnvelope)
        {
            Coordinate min = inEnvelope.Minimum.Copy();
            Coordinate max = inEnvelope.Maximum.Copy();
            DoInit(min, max);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by two Coordinates.
        /// </summary>
        /// <param name="p1">The first Coordinate.</param>
        /// <param name="p2">The second Coordinate.</param>
        public Envelope(Coordinate p1, Coordinate p2)
        {
            if (p1 == null || p2 == null)
            {
                return;
            }
            int numDimensions = Math.Min(p1.NumOrdinates, p2.NumOrdinates);
            _min = new Coordinate();
            _max = new Coordinate();
            for (int i = 0; i < numDimensions; i++)
            {
                _min[i] = Math.Min(p1[i], p2[i]);
                _max[i] = Math.Max(p1[i], p2[i]);
            }
            _min.M = Math.Min(p1.M, p2.M);
            _max.M = Math.Min(p1.M, p2.M);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by a single Coordinate.
        /// </summary>
        /// <param name="p">The Coordinate.</param>
        public Envelope(Coordinate p)
        {
            DoInit(p.Copy(), p.Copy());
        }

        /// <summary>
        /// This is a special constructor.  This will not fire the OnEnvelopeChanged
        /// event, and expects values to be ordered as:
        /// XMin, XMax, YMin, YMax, ZMin, ZMax, higher dimenions...
        /// This constructor.
        /// </summary>
        /// <param name="extents">The ordinal extents.</param>
        /// <param name="mMin">The minimum m value.</param>
        /// <param name="mMax">The max M value.</param>
        public Envelope(double[] extents, double mMin, double mMax)
        {
            _min = new Coordinate();
            _max = new Coordinate();
            for (int ordinate = 0; ordinate < extents.Length / 2; ordinate++)
            {
                _min[ordinate] = extents[ordinate * 2];
                _max[ordinate] = extents[ordinate * 2 + 1];
            }
            _min.M = mMin;
            _max.M = mMax;
        }

        /// <summary>
        /// This is a special constructor.  This will not fire the OnEnvelopeChanged
        /// event, and expects values to be ordered as:
        /// XMin, XMax, YMin, YMax, ZMin, ZMax, higher dimenions...
        /// This constructor sets M bounds to be 0.
        /// </summary>
        /// <param name="extents">The ordinal extents.</param>
        public Envelope(double[] extents)
            : this(extents, 0, 0)
        {
        }

        /// <summary>
        /// Initialize an Envelope of any dimension for a region defined by two Coordinates.
        /// The number of ordinates (dimension) of the coordinates must match.  M values
        /// will also be considered.
        /// </summary>
        /// <param name="p1">The first Coordinate.</param>
        /// <param name="p2">The second Coordinate.</param>
        public virtual void Init(Coordinate p1, Coordinate p2)
        {
            // ensure the number of ordinates match.
            if (p1 == null && p2 == null)
            {
                Init();
                return;
            }
            if (p1 == null)
            {
                Init(p2, p2);
                return;
            }
            if (p2 == null)
            {
                Init(p1, p1);
                return;
            }
            int numDimensions = Math.Min(p1.NumOrdinates, p2.NumOrdinates);
            _min = new Coordinate();
            _max = new Coordinate();
            for (int i = 0; i < numDimensions; i++)
            {
                _min[i] = Math.Min(p1[i], p2[i]);
                _max[i] = Math.Max(p1[i], p2[i]);
            }
            _min.M = Math.Min(p1.M, p2.M);
            _max.M = Math.Min(p1.M, p2.M);
            OnEnvelopeChanged();
        }

        /// <summary>
        /// This initializes the values without passing any coordinates and creates the default, 2D null envelope.
        /// </summary>
        private void DoInit()
        {
            _min = new Coordinate(0, 0);
            _max = new Coordinate(-1, -1);
        }

        /// <summary>
        /// This is an internal method forcing assignments to the internal coordinates
        /// </summary>
        /// <param name="min">The coordinate to use for the minimum in X, Y, Z etc.</param>
        /// <param name="max">The coordinate to use for the maximum in X, Y, Z etc.</param>
        private void DoInit(Coordinate min, Coordinate max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Initialize to a null <c>Envelope</c>.
        /// </summary>
        public virtual void Init()
        {
            SetToNull();
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> for a region defined by maximum and minimum values.
        /// </summary>
        /// <param name="x1">The first x-value.</param>
        /// <param name="x2">The second x-value.</param>
        /// <param name="y1">The first y-value.</param>
        /// <param name="y2">The second y-value.</param>
        public virtual void Init(double x1, double x2, double y1, double y2)
        {
            _min = new Coordinate(); // 2 ordinates, x & y
            _max = new Coordinate();
            _min.X = Math.Min(x1, x2);
            _max.X = Math.Max(x1, x2);
            _min.Y = Math.Min(y1, y2);
            _max.Y = Math.Max(y1, y2);
            OnEnvelopeChanged();
        }

        /// <summary>
        /// This will set all 3 dimensions.  Be warned, the Z dimensions are just place holders
        /// for any topology opperations and do not have any true functionality.  Whichever
        /// is smaller becomes the minimum and whichever is larger becomes the maximum.
        /// </summary>
        /// <param name="x1">An X coordinate </param>
        /// <param name="x2">Another X coordinate</param>
        /// <param name="y1">A Y coordinate</param>
        /// <param name="y2">Another Y coordinate</param>
        /// <param name="z1">A Z coordinate</param>
        /// <param name="z2">Another Z coordinate</param>
        public virtual void Init(double x1, double x2, double y1, double y2, double z1, double z2)
        {
            _min = new Coordinate(); // 3 ordinates, x & y & z
            _max = new Coordinate();
            _min.X = Math.Min(x1, x2);
            _max.X = Math.Max(x1, x2);
            _min.Y = Math.Min(y1, y2);
            _max.Y = Math.Max(y1, y2);
            _max.Z = Math.Max(z1, z2);
            _min.Z = Math.Min(z1, z2);
            OnEnvelopeChanged();
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> for a region defined by a single Coordinate.
        /// </summary>
        /// <param name="p">The Coordinate.</param>
        public virtual void Init(Coordinate p)
        {
            if (p == null)
            {
                Init();
                return;
            }
            _min = p.Copy();
            _max = p.Copy();
            OnEnvelopeChanged();
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> from an existing Envelope.
        /// </summary>
        /// <param name="env">The Envelope to initialize from.</param>
        public virtual void Init(IEnvelope env)
        {
            if (env.Maximum == null || env.Minimum == null)
            {
                Init();
            }
            if (env.Maximum != null) _max = env.Maximum.Copy();
            if (env.Minimum != null) _min = env.Minimum.Copy();
            OnEnvelopeChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// True only if the M coordinates are not NaN and the max is greater than the min.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool HasM()
        {
            if (double.IsNaN(Minimum.M) || double.IsNaN(Maximum.M)) return false;
            if (Minimum.M > Maximum.M) return false;
            return true;
        }

        /// <summary>
        /// True only of the Z ordinates are not NaN and the max is greater than the min.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool HasZ()
        {
            if (double.IsNaN(Minimum.Z) || double.IsNaN(Maximum.Z)) return false;
            if (Minimum.Z > Maximum.Z) return false;
            return true;
        }

        /// <summary>
        /// Creates a copy of the current envelope.
        /// </summary>
        /// <returns>An object satisfying ICloneable</returns>
        public virtual object Clone()
        {
            return Copy();
        }

        IEnvelope IEnvelope.Copy()
        {
            return Copy();
        }

        /// <summary>
        /// Makes this <c>Envelope</c> a "null" envelope.
        /// </summary>
        /// <remarks>Internaly, envelopes that vioate that sacred criteria of min being less than max
        /// will be counted as a null case</remarks>
        public virtual void SetToNull()
        {
            _min = new Coordinate();
            _max = new Coordinate();
            for (int i = 0; i < NumOrdinates; i++)
            {
                _min[i] = 0;
                _max[i] = -1;
            }
            OnEnvelopeChanged();
        }

        /// <summary>
        /// Creates a copy of the current envelope.
        /// </summary>
        /// <returns>An IEnvelope Interface that is a duplicate of this envelope</returns>
        public virtual Envelope Copy()
        {
            if (IsNull) return new Envelope();
            return new Envelope(_min, _max);
        }

        /// <summary>
        /// Test the point q to see whether it intersects the Envelope
        /// defined by p1-p2.
        /// </summary>
        /// <param name="p1">One extremal point of the envelope.</param>
        /// <param name="p2">Another extremal point of the envelope.</param>
        /// <param name="q">Point to test for intersection.</param>
        /// <returns><c>true</c> if q intersects the envelope p1-p2.</returns>
        public static bool Intersects(Coordinate p1, Coordinate p2, Coordinate q)
        {
            Envelope env = new Envelope(p1, p2);
            return env.Intersects(q);
        }

        /// <summary>
        /// Test the envelope defined by p1-p2 for intersection
        /// with the envelope defined by q1-q2
        /// </summary>
        /// <param name="p1">One extremal point of the envelope Point.</param>
        /// <param name="p2">Another extremal point of the envelope Point.</param>
        /// <param name="q1">One extremal point of the envelope Q.</param>
        /// <param name="q2">Another extremal point of the envelope Q.</param>
        /// <returns><c>true</c> if Q intersects Point</returns>
        public static bool Intersects(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
        {
            Envelope a = new Envelope(p1, p2);
            Envelope b = new Envelope(q1, q2);
            return a.Intersects(b);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies that this is in fact an envelope
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual GeometryType GeometryType
        {
            get
            {
                return GeometryType.Envelope;
            }
        }

        /// <summary>
        /// Gets the Horizontal boundaries in geographic coordinates
        /// </summary>
        [Category("Range"), Description("Gets the Horizontal boundaries in geographic coordinates")]
        public string BoundsX
        {
            get { return _min.X + " - " + _max.X; }
        }

        /// <summary>
        /// Gets the vertical boundaries in geographic coordinates
        /// </summary>
        [Category("Range"), Description("Gets the Horizontal boundaries in geographic coordinates")]
        public string BoundsY
        {
            get { return _min.Y + " - " + _max.Y; }
        }

        /// <summary>
        /// Gets the minimum coordinate
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Coordinate Minimum
        {
            get { return _min; }
        }

        /// <summary>
        /// Gets the maximum coordinate
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Coordinate Maximum
        {
            get { return _max; }
        }

        /// <summary>
        /// Gets the number of ordinates for this envelope.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NumOrdinates
        {
            get { return _min.NumOrdinates; }
        }

        /// <summary>
        /// Returns <c>true</c> if this <c>Envelope</c> is a "null" envelope.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this <c>Envelope</c> is uninitialized
        /// or is the envelope of the empty point.
        /// </returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsNull
        {
            get
            {
                if (_max == null || _min == null) return true;
                return _max.X < _min.X;
            }
        }

        /// <summary>
        /// Gets or sets the difference between the maximum and minimum y values.
        /// Setting this will only change the _yMin value (leaving the top alone)
        /// </summary>
        /// <returns>max y - min y, or 0 if this is a null <c>Envelope</c>.</returns>
        /// <remarks>To resemble the precedent set by the dot net framework controls, this is left as a property</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual double Height
        {
            get
            {
                if (IsNull)
                    return 0;
                return Maximum.Y - Minimum.Y;
            }
            set
            {
                double h = Math.Abs(value);
                _max.Y = _min.Y + h;
                OnEnvelopeChanged();
            }
        }

        /// <summary>
        /// Gets or Sets the difference between the maximum and minimum x values.
        /// Setting this will change only the Maximum X value, and leave the minimum X alone
        /// </summary>
        /// <returns>max x - min x, or 0 if this is a null <c>Envelope</c>.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual double Width
        {
            get
            {
                if (IsNull)
                    return 0;
                return _max.X - _min.X;
            }
            set
            {
                _max.X = value + _min.X;
                OnEnvelopeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Left extent of the envelope, keeping the width the same.
        /// In the precedent set by controls, envelopes support an X value and a width.
        /// The X value represents the Minimum.X coordinate for this envelope.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual double X
        {
            get
            {
                return _min.X;
            }
            set
            {
                double width = Width;
                _min.X = value;
                _max.X = value + width;
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate for the top of the envelope.  (YMax)
        /// In the precedent set by controls, envelopes support a Y value and a height.
        /// the Y value represents the Top of the envelope, (Maximum.Y) and the Height
        /// represents the span between Maximum and Minimum Y.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual double Y
        {
            get
            {
                return _max.Y;
            }
            set
            {
                double height = Height;
                _max.Y = value;
                _min.Y = value - height;
            }
        }

        #endregion

        /// <summary>
        /// Generates a string form of this envelope.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This should not be an extension method because it overrides ToString.</remarks>
        public override string ToString()
        {
            string result = "Env[";
            for (int i = 0; i < NumOrdinates; i++)
            {
                if (i > 0) result += ", ";
                result += Minimum[i] + " : " + Maximum[i];
            }
            return result;
        }

        #region Events

        /// <summary>
        /// Occurs when there is a basic change in the envelope.
        /// </summary>
        public event EventHandler EnvelopeChanged;

        /// <summary>
        /// Fires the EnvelopeChanged event
        /// </summary>
        protected void OnEnvelopeChanged()
        {
            if (EnvelopeChanged == null) return;
            EnvelopeChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
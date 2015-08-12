using System;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Distance3D
{
    /// <summary>
    /// A <see cref="ICoordinateSequence"/> wrapper which 
    /// projects 3D coordinates into one of the
    /// three Cartesian axis planes,
    /// using the standard orthonormal projection
    /// (i.e. simply selecting the appropriate ordinates into the XY ordinates).
    /// The projected data is represented as 2D coordinates.
    /// </summary>
    /// <author>Martin Davis</author>
    public class AxisPlaneCoordinateSequence : ICoordinateSequence
    {
        #region Fields

        private readonly Ordinate[] _indexMap;
        private readonly ICoordinateSequence _seq;

        #endregion

        #region Constructors

        private AxisPlaneCoordinateSequence(ICoordinateSequence seq, Ordinate[] indexMap)
        {
            _seq = seq;
            _indexMap = indexMap;
        }

        #endregion

        #region Properties

        public int Count
        {
            get { return _seq.Count; }
        }

        public int Dimension
        {
            get { return 2; }
        }

        public Ordinates Ordinates { get { return _seq.Ordinates; } }

        #endregion

        #region Methods

        public Object Clone()
        {
            throw new NotSupportedException();
        }

        public Envelope ExpandEnvelope(Envelope env)
        {
            throw new NotSupportedException();
        }

        public Coordinate GetCoordinate(int i)
        {
            return GetCoordinateCopy(i);
        }

        public void GetCoordinate(int index, Coordinate coord)
        {
            coord.X = GetOrdinate(index, Ordinate.X);
            coord.Y = GetOrdinate(index, Ordinate.Y);
            coord.Z = GetOrdinate(index, Ordinate.Z);
        }

        public Coordinate GetCoordinateCopy(int i)
        {
            return new Coordinate(GetX(i), GetY(i), GetZ(i));
        }

        public double GetOrdinate(int index, Ordinate ordinateIndex)
        {
            // Z ord is always 0
            if (ordinateIndex > Ordinate.Y) return 0;
            return _seq.GetOrdinate(index, _indexMap[(int) ordinateIndex]);
        }

        public double GetX(int index)
        {
            return GetOrdinate(index, Ordinate.X);
        }

        public double GetY(int index)
        {
            return GetOrdinate(index, Ordinate.Y);
        }

        public double GetZ(int index)
        {
            return GetOrdinate(index, Ordinate.Z);
        }

        public ICoordinateSequence Reversed()
        {
            throw new NotSupportedException();
        }

        public void SetOrdinate(int index, Ordinate ordinateIndex, double value)
        {
            throw new NotSupportedException();
        }

        public Coordinate[] ToCoordinateArray()
        {
            throw new NotSupportedException();
        }

        #endregion

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Creates a wrapper projecting to the XY plane.
        /// </summary>
        /// <param name="seq">The sequence to be projected</param>
        /// <returns>A sequence which projects coordinates</returns>
        public static ICoordinateSequence ProjectToXY(ICoordinateSequence seq)
        {
            /**
		 * This is just a no-op, but return a wrapper
		 * to allow better testing
		 */
            return new AxisPlaneCoordinateSequence(seq, XYIndex);
        }

        /// <summary>
        /// Creates a wrapper projecting to the XZ plane.
        /// </summary>
        /// <param name="seq">The sequence to be projected</param>
        /// <returns>A sequence which projects coordinates</returns>
        public static ICoordinateSequence ProjectToXZ(ICoordinateSequence seq)
        {
            return new AxisPlaneCoordinateSequence(seq, XZIndex);
        }

        /// <summary>
        /// Creates a wrapper projecting to the YZ plane.
        /// </summary>
        /// <param name="seq">The sequence to be projected</param>
        /// <returns>A sequence which projects coordinates</returns>
        public static ICoordinateSequence ProjectToYZ(ICoordinateSequence seq)
        {
            return new AxisPlaneCoordinateSequence(seq, YZIndex);
        }

        private static readonly Ordinate[] XYIndex = new[] {Ordinate.X, Ordinate.Y};
        private static readonly Ordinate[] XZIndex = new[] {Ordinate.X, Ordinate.Z};
        private static readonly Ordinate[] YZIndex = new[] {Ordinate.Y, Ordinate.Z};
        // ReSharper restore InconsistentNaming
    }
}
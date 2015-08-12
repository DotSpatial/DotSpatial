using System;
using System.Collections.Generic;

namespace DotSpatial.Topology.Geometries.Implementation
{
    /// <summary>
    /// A coordinate sequence factory class that creates DotSpatial's Shape/ShapeRange like coordinate sequences.
    /// </summary>
#if !PCL   
    [Serializable]
#endif
    public class DotSpatialAffineCoordinateSequenceFactory : ICoordinateSequenceFactory
    {
        #region Fields

        private static readonly object InstanceLock = new object();
        private static DotSpatialAffineCoordinateSequenceFactory _instance;

        #endregion

        #region Constructors

        private DotSpatialAffineCoordinateSequenceFactory() { }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the singleton instance of DotSpatialAffineCoordinateSequenceFactory.
        /// </summary>
        /// <returns></returns>
        public static DotSpatialAffineCoordinateSequenceFactory Instance
        {
            get
            {
                lock(InstanceLock)
                {
                    return (_instance ?? (_instance = new DotSpatialAffineCoordinateSequenceFactory()));
                }
            }
        }

        /// <summary>
        /// Gets the Ordinate flags that sequences created by this factory can cope with.
        /// </summary>
        public Ordinates Ordinates
        {
            get { return Ordinates.XYZM; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Returns a CoordinateArraySequence based on the given IEnumerable.
        /// </summary>
        /// <param name="coordinates">the coordinates, which may not be null nor contain null elements.</param>
        /// <returns></returns>
        public ICoordinateSequence Create(IEnumerable<Coordinate> coordinates)
        {
            return new DotSpatialAffineCoordinateSequence(coordinates);
        }

        /// <summary>
        /// Creates a <see cref="ICoordinateSequence" />  which is a copy
        /// of the given <see cref="ICoordinateSequence" />.
        /// This method must handle null arguments by creating an empty sequence.
        /// </summary>
        /// <param name="coordSeq"></param>
        /// <returns>A coordinate sequence</returns>
        public ICoordinateSequence Create(ICoordinateSequence coordSeq)
        {
            return new DotSpatialAffineCoordinateSequence(coordSeq);
        }

        /// <summary>
        /// Creates a <see cref="ICoordinateSequence" /> of the specified size and dimension.
        /// For this to be useful, the <see cref="ICoordinateSequence" /> implementation must be mutable.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="dimension">the dimension of the coordinates in the sequence 
        /// (if user-specifiable, otherwise ignored)</param>
        /// <returns>A coordinate sequence</returns>
        public ICoordinateSequence Create(int size, int dimension)
        {

            return new DotSpatialAffineCoordinateSequence(size, OrdinatesUtility.DimensionToOrdinates(dimension));
        }

        /// <summary>
        /// Creates a <see cref="ICoordinateSequence" /> of the specified size and ordinates.
        /// For this to be useful, the <see cref="ICoordinateSequence" /> implementation must be mutable.
        /// </summary>
        /// <param name="size">The number of coordinates.</param>
        /// <param name="ordinates">
        /// The ordinates each coordinate has. <see cref="Geometries.Ordinates.XY"/> is fix, 
        /// <see cref="Geometries.Ordinates.Z"/> and <see cref="Geometries.Ordinates.M"/> can be set.
        /// </param>
        /// <returns>A coordinate sequence.</returns>
        public ICoordinateSequence Create(int size, Ordinates ordinates)
        {
            return new DotSpatialAffineCoordinateSequence(size, Ordinates & ordinates);
        }

        public ICoordinateSequence Create(Coordinate coord)
        {
            return new DotSpatialAffineCoordinateSequence(new List<Coordinate>{coord});
        }

        public ICoordinateSequence Create()
        {
            return new DotSpatialAffineCoordinateSequence(new List<Coordinate> { new Coordinate() });
        }

        #endregion
    }
}

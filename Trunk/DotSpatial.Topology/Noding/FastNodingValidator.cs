using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.IO;

namespace DotSpatial.Topology.Noding
{
    ///<summary>
    /// Validates that a collection of <see cref="ISegmentString"/>s is correctly noded.
    /// Indexing is used to improve performance.
    ///</summary>
    /// <remarks>
    /// <para>
    /// In the most common use case, validation stops after a single
    /// non-noded intersection is detected,
    /// but the class can be requested to detect all intersections
    /// by using the <see cref="FindAllIntersections"/> property.
    /// <para/>
    /// The validator does not check for a-b-a topology collapse situations.
    /// <para/> 
    /// The validator does not check for endpoint-interior vertex intersections.
    /// This should not be a problem, since the JTS noders should be
    /// able to compute intersections between vertices correctly.
    /// </para>
    /// <para>
    /// The client may either test the <see cref="IsValid"/> condition,
    /// or request that a suitable <see cref="TopologyException"/> be thrown.
    /// </para>
    /// </remarks>
    public class FastNodingValidator
    {
        #region Fields

        private readonly LineIntersector _li = new RobustLineIntersector();
        private readonly List<ISegmentString> _segStrings = new List<ISegmentString>();
        private bool _isValid = true;
        private InteriorIntersectionFinder _segInt;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new noding validator for a given set of linework.
        /// </summary>
        /// <param name="segStrings">A collection of <see cref="ISegmentString"/>s</param>
        public FastNodingValidator(IEnumerable<ISegmentString> segStrings)
        {
            _segStrings.AddRange(segStrings);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether all intersections should be found.
        /// </summary>
        public bool FindAllIntersections { get; set; }

        /// <summary>
        /// Gets a list of all intersections found.
        /// <remarks>
        /// Intersections are represented as <see cref="Coordinate"/>s.
        /// List is empty if none were found.
        /// </remarks>
        /// </summary>
        public IList<Coordinate> Intersections
        {
            get { return _segInt.Intersections; }
        }

        ///<summary>
        /// Checks for an intersection and reports if one is found.
        ///</summary>
        public bool IsValid
        {
            get
            {
                Execute();
                return _isValid;
            }
        }

        #endregion

        #region Methods

        private void CheckInteriorIntersections()
        {
            // MD - It may even be reliable to simply check whether end segments (of SegmentStrings) have
            //an interior intersection, since noding should have split any true interior intersections already.
            _isValid = true;
            _segInt = new InteriorIntersectionFinder(_li) { FindAllIntersections = FindAllIntersections };
            MCIndexNoder noder = new MCIndexNoder(_segInt);
            noder.ComputeNodes(_segStrings);
            if (_segInt.HasIntersection) _isValid = false;
        }

        ///<summary>
        /// Checks for an intersection and throws
        /// a TopologyException if one is found.
        ///</summary>
        ///<exception cref="TopologyException">if an intersection is found</exception>
        public void CheckValid()
        {
            if (!IsValid) throw new TopologyException(GetErrorMessage(), _segInt.InteriorIntersection);
        }

        public static IList<Coordinate> ComputeIntersections(IEnumerable<ISegmentString> segStrings)
        {
            FastNodingValidator nv = new FastNodingValidator(segStrings) { FindAllIntersections = true };
            bool temp = nv.IsValid;
            return nv.Intersections;
        }

        private void Execute()
        {
            if (_segInt != null) return;
            CheckInteriorIntersections();
        }

        ///<summary>
        /// Returns an error message indicating the segments containing the intersection.
        ///</summary>
        ///<returns>an error message documenting the intersection location</returns>
        public string GetErrorMessage()
        {
            if (IsValid) return TopologyText.FastNodingValidator_NoIntersectionFound;

            IList<Coordinate> intSegs = _segInt.IntersectionSegments;
            return string.Format(TopologyText.FastNodingValidator_FoundNonNodedIntersection,
                WKTWriter.ToLineString(intSegs[0], intSegs[1]), WKTWriter.ToLineString(intSegs[2], intSegs[3]));
        }

        #endregion
    }
}
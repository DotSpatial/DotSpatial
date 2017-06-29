// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
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
using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Wraps a <see cref="INoder" /> and transforms its input into the integer domain.
    /// This is intended for use with Snap-Rounding noders,
    /// which typically are only intended to work in the integer domain.
    /// Offsets can be provided to increase the number of digits of available precision.
    /// </summary>
    public class ScaledNoder : INoder
    {
        private readonly bool _isScaled;
        private readonly INoder _noder;
        private readonly double _offsetX;
        private readonly double _offsetY;
        private readonly double _scaleFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaledNoder"/> class.
        /// </summary>
        /// <param name="noder"></param>
        /// <param name="scaleFactor"></param>
        public ScaledNoder(INoder noder, double scaleFactor)
            : this(noder, scaleFactor, 0, 0) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="noder"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public ScaledNoder(INoder noder, double scaleFactor, double offsetX, double offsetY)
        {
            _offsetX = offsetX;
            _offsetY = offsetY;
            _noder = noder;
            _scaleFactor = scaleFactor;
            // no need to scale if input precision is already integral
            _isScaled = !IsIntegerPrecision;
        }

        /// <summary>
        ///
        /// </summary>
        private bool IsIntegerPrecision
        {
            get
            {
                return _scaleFactor == 1.0;
            }
        }

        #region INoder Members

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList GetNodedSubstrings()
        {
            IList splitSS = _noder.GetNodedSubstrings();
            if (_isScaled)
                Rescale(splitSS);
            return splitSS;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputSegStrings"></param>
        public void ComputeNodes(IList inputSegStrings)
        {
            IList intSegStrings = inputSegStrings;
            if (_isScaled)
                intSegStrings = Scale(inputSegStrings);
            _noder.ComputeNodes(intSegStrings);
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStrings"></param>
        /// <returns></returns>
        private IList Scale(ICollection segStrings)
        {
            return CollectionUtil.Transform(segStrings, delegate(object obj)
            {
                SegmentString ss = (SegmentString)obj;
                return new SegmentString(Scale(ss.Coordinates), ss.Data);
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        private Coordinate[] Scale(IList<Coordinate> pts)
        {
            Coordinate[] roundPts = new Coordinate[pts.Count];
            for (int i = 0; i < pts.Count; i++)
                roundPts[i] = new Coordinate(Math.Round((pts[i].X - _offsetX) * _scaleFactor),
                                             Math.Round((pts[i].Y - _offsetY) * _scaleFactor));
            return roundPts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStrings"></param>
        private void Rescale(ICollection segStrings)
        {
            CollectionUtil.Apply(segStrings, delegate(object obj)
            {
                SegmentString ss = (SegmentString)obj;
                Rescale(ss.Coordinates);
                return null;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        private void Rescale(IList<Coordinate> pts)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                pts[i].X = pts[i].X / _scaleFactor + _offsetX;
                pts[i].Y = pts[i].Y / _scaleFactor + _offsetY;
            }
        }
    }
}
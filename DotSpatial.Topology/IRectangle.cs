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
// *********************************************************************************************************

namespace DotSpatial.Topology
{
    /// <summary>
    /// An IRectangle is not as specific to being a geometry, and can apply to anything as long as it is willing
    /// to support a double valued height, width, X and Y value.
    /// </summary>
    public interface IRectangle
    {
        #region Properties

        /// <summary>
        /// Gets or sets the difference between the maximum and minimum y values.
        /// Setting this will change only the minimum Y value, leaving the Top alone
        /// </summary>
        /// <returns>max y - min y, or 0 if this is a null <c>Envelope</c>.</returns>
        double Height { get; set; }

        /// <summary>
        /// Gets or Sets the difference between the maximum and minimum x values.
        /// Setting this will change only the Maximum X value, and leave the minimum X alone
        /// </summary>
        /// <returns>max x - min x, or 0 if this is a null <c>Envelope</c>.</returns>
        double Width { get; set; }

        /// <summary>
        /// In the precedent set by controls, envelopes support an X value and a width.
        /// The X value represents the Minimum.X coordinate for this envelope.
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// In the precedent set by controls, envelopes support a Y value and a height.
        /// the Y value represents the Top of the envelope, (Maximum.Y) and the Height
        /// represents the span between Maximum and Minimum Y.
        /// </summary>
        double Y { get; set; }

        #endregion
    }
}
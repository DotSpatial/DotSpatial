// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer or other object capable of containing
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface ILabelContainer
    {
        /// <summary>
        /// The integer indexed list of all the labels in this container.
        /// </summary>
        Dictionary<int, ILabel> Labels
        {
            get;
            set;
        }

        /// <summary>
        /// If this is set to true, then the only labels being sent to GetLabelExtents
        /// should be the labels that actually need to be drawn.
        /// </summary>
        bool LabelCollisionTesting
        {
            get;
            set;
        }

        /// <summary>
        /// A priority rating for all the labels in this layer.  The higher this number,
        /// the more likely the labels from this layer are to win a collision contest
        /// between layers.
        /// </summary>
        int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Draws every label in the layer without worrying about any collision testing
        /// or specific tests outside of this label container.  If LabelCollisionTesting
        /// is set to true, it should still do collision testing for the labels INSIDE
        /// this label container.
        /// </summary>
        void DrawLabels();

        /// <summary>
        /// The list of integers corresponds to the integer values that were returned as
        /// keys to the GetLabelExtents dictionary.  The Layer will then draw each of the
        /// specific lables to draw.
        /// </summary>
        /// <param name="labelsToDraw"></param>
        void DrawLabels(List<int> labelsToDraw);

        /// <summary>
        /// Gets the list of non-rotated extents for labels visible in the current display.
        /// The integer keys represent the index of the labeled item.
        /// The envelope values represent the actual rectangles that the labels occupy for
        /// collision testing.  If LabelCollisionTesting is true, then collisions should
        /// be resolved for the entire layer first based on whatever internal priority
        /// methods exist.  Only the non-colliding visible labels will be returned.
        /// </summary>
        Dictionary<int, IEnvelope> GetLabelExtents();

        /// <summary>
        /// Gets the list of possibly rotated label boundaries visible in the current display.
        /// The integer keys represent the index of the labeled item.
        /// The envelope values represent the actual rectangles that the labels occupy for
        /// collision testing.  If LabelCollisionTesting is true, then collisions should
        /// be resolved for the entire layer first based on whatever internal priority
        /// methods exist.  Only the non-colliding visible labels will be returned.
        /// </summary>
        Dictionary<int, IPolygon> GetRotatedLabelBounds();
    }
}
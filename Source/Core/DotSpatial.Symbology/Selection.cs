// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Selection is used to remember all the selected features of the corresponding IFeatureSet.
    /// </summary>
    public class Selection : FeatureSelection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Selection"/> class.
        /// </summary>
        /// <param name="fs">The feature set.</param>
        /// <param name="inFilter">The drawing filter.</param>
        public Selection(IFeatureSet fs, IDrawingFilter inFilter)
            : base(fs, inFilter, FilterType.Selection)
        {
            Selected = true;
            UseSelection = true;
            UseCategory = false;
            UseVisibility = false;
            UseChunks = false;
            SelectionMode = SelectionMode.IntersectsExtent;
        }

        #endregion
    }
}
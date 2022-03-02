// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// ShapefileFeatureSourceSearchAndModifyAttributeParameters.
    /// </summary>
    internal class ShapefileFeatureSourceSearchAndModifyAttributeParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileFeatureSourceSearchAndModifyAttributeParameters"/> class.
        /// </summary>
        /// <param name="featureEditCallback">The featureEditCallback.</param>
        public ShapefileFeatureSourceSearchAndModifyAttributeParameters(FeatureSourceRowEditEvent featureEditCallback)
        {
            FeatureEditCallback = featureEditCallback;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CurrentSearchAndModifyAttributeShapes.
        /// </summary>
        public Dictionary<int, Shape> CurrentSearchAndModifyAttributeShapes { get; set; }

        /// <summary>
        /// Gets or sets the FeatureEditCallback.
        /// </summary>
        public FeatureSourceRowEditEvent FeatureEditCallback { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Edits the feature of the row in the event.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <returns>True, if FeatureEditCallback returns true.</returns>
        public bool OnRowEditEvent(RowEditEventArgs e)
        {
            var shape = CurrentSearchAndModifyAttributeShapes[e.RowNumber];
            return FeatureEditCallback(new FeatureSourceRowEditEventArgs(e, shape));
        }

        #endregion
    }
}
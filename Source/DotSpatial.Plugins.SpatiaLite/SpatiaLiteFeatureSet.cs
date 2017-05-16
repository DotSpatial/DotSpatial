// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// A feature set for SpatiaLite data.
    /// </summary>
    public class SpatiaLiteFeatureSet : FeatureSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpatiaLiteFeatureSet"/> class.
        /// </summary>
        /// <param name="fType">The feature type of the contained features.</param>
        public SpatiaLiteFeatureSet(FeatureType fType)
            : base(fType)
        {
        }

        /// <summary>
        /// Gets the feature at the given index.
        /// </summary>
        /// <param name="index">Index of the feature that should be returned.</param>
        /// <returns>The feature belonging to the given index.</returns>
        public override IFeature GetFeature(int index)
        {
            var res = base.GetFeature(index);
            if (res.DataRow == null)
            {
                res.DataRow = DataTable.Rows[index];
            }

            return res;
        }
    }
}
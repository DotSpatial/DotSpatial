// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// FieldJoinType.
    /// </summary>
    public enum FieldJoinType
    {
        /// <summary>
        /// Output datasets have all fields from both input and output sets. Fields with duplicate field names will be appended with a number.
        /// Features from this dataset may appear more than once if more than one valid intersection occurs with the features from the
        /// other featureset.
        /// </summary>
        All,

        /// <summary>
        /// The fields will be created from the fields in the other featureset.
        /// </summary>
        ForeignOnly,

        /// <summary>
        /// All the fields from this FeatureSet are used, and all of the features from the other featureset are considered
        /// to be a single geometry so that features from this set will appear no more than once in the output set.
        /// </summary>
        LocalOnly,

        /// <summary>
        /// No fields will be copied, but features from this featureset will be considered independantly and added as separate
        /// features to the output featureset.
        /// </summary>
        None,
    }
}
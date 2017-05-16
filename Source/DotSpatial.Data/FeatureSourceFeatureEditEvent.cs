// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Callback specified when calling IFeatureSource.SearchAndModifyAttributes
    /// </summary>
    /// <param name="e">The event args.</param>
    /// <returns>Boolean</returns>
    public delegate bool FeatureSourceRowEditEvent(FeatureSourceRowEditEventArgs e);
}
// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in FeatureLayer any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface IFeatureLayerActions
    {
        /// <summary>
        /// Show properties
        /// </summary>
        /// <param name="e">Feature layer</param>
        void ShowProperties(IFeatureLayer e);

        /// <summary>
        /// Show Join Excel
        /// </summary>
        /// <param name="e">Feature set</param>
        void ExcelJoin(IFeatureSet e);

        /// <summary>
        /// Show dynamic visibility dialog
        /// </summary>
        /// <param name="e">Dynamic visibility</param>
        void LabelExtents(IDynamicVisibility e);

        /// <summary>
        /// Show label setup dialog
        /// </summary>
        /// <param name="e">Label layer</param>
        void LabelSetup(ILabelLayer e);

        /// <summary>
        /// CGX Show label setup dialog
        /// </summary>
        /// <param name="e">Label layer</param>
        void LabelSetup2(ILabelLayer e);

        /// <summary>
        /// Show attributes dialog
        /// </summary>
        /// <param name="e">Feature layer</param>
        void ShowAttributes(IFeatureLayer e);

        /// <summary>
        /// Show export dialog
        /// </summary>
        /// <param name="e">Feature layer</param>
        void ExportData(IFeatureLayer e);

        /// <summary>
        /// Shows select by attributes dialog
        /// </summary>
        /// <param name="featureLayer">Feature layer</param>
        void SelectByAttributes(FeatureLayer featureLayer);
    }
}
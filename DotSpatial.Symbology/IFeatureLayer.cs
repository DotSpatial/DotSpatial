// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is DotSpatial.dll for the DotSpatial project
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is should not be instantiated because it cannot in itself perform the necessary functions.
    /// Instead, most of the specified functionality must be implemented in the more specific classes.
    /// This is also why there is no direct constructor for this class.  You can use the static
    /// "FromFile" or "FromFeatureLayer" to create FeatureLayers from a file.
    /// </summary>
    public interface IFeatureLayer : ILayer
    {
        #region Events

        /// <summary>
        /// Occurs before the attribute Table is displayed, also allowing this event to be handled.
        /// </summary>
        event HandledEventHandler ViewAttributes;

        /// <summary>
        /// Occurs after a snapshot is taken, and contains an event argument with the bitmap
        /// to be displayed.
        /// </summary>
        event EventHandler<SnapShotEventArgs> SnapShotTaken;

        /// <summary>
        /// Occurs after a new symbolic scheme has been applied to the layer.
        /// </summary>
        event EventHandler SchemeApplied;

        #endregion

        #region Methods

        /// <summary>
        /// Applies the specified scheme to this layer, applying the filter constraints in the scheme.
        /// It is at this time when features are grouped as a one-time operation into their symbol
        /// categories, so that this doesn't have to happen independently each drawing cycle.
        /// </summary>
        /// <param name="inScheme">
        /// The scheme to be applied to this layer.
        /// </param>
        void ApplyScheme(IFeatureScheme inScheme);

        /// <summary>
        /// Assigns the fast drawn states based on the latest feature setup.
        /// </summary>
        void AssignFastDrawnStates();

        /// <summary>
        /// This method actually draws the image to the snapshot using the graphics object.  This should be
        /// overridden in sub-classes because the drawing methods are very different.
        /// </summary>
        /// <param name="g">
        /// A graphics object to draw to
        /// </param>
        /// <param name="p">
        /// A projection handling interface designed to translate geographic coordinates to screen
        /// coordinates
        /// </param>
        void DrawSnapShot(Graphics g, IProj p);

        /// <summary>
        /// Saves a featureset with only the selected features to the specified fileName.
        /// </summary>
        /// <param name="fileName">
        /// The string fileName to export features to.
        /// </param>
        void ExportSelection(string fileName);

        /// <summary>
        /// Gets the visible characteristic for an individual feature, regardless of whether
        /// this layer is in edit mode.
        /// </summary>
        /// <param name="index">
        /// </param>
        IFeatureCategory GetCategory(int index);

        /// <summary>
        /// Gets the visible characteristic for a given feature, rather than using the index,
        /// regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="feature">
        /// </param>
        IFeatureCategory GetCategory(IFeature feature);

        /// <summary>
        /// Gets the visible characteristic for an individual feature
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// The get visible.
        /// </returns>
        bool GetVisible(int index);

        /// <summary>
        /// Gets the visible characteristic for a given feature, rather than using the index.
        /// </summary>
        /// <param name="feature">
        /// </param>
        /// <returns>
        /// The get visible.
        /// </returns>
        bool GetVisible(IFeature feature);

        /// <summary>
        /// This method will remove the in ram features from the underlying dataset.
        /// This will not affect the data source.  Beware!  Removing features
        /// without populating the DataTable first will almost assuredly mean
        /// indexing failures if you choose to load it later.
        /// </summary>
        /// <param name="indexValues">The list or array of integer index values.</param>
        void RemoveFeaturesAt(IEnumerable<int> indexValues);

        /// <summary>
        /// This forces the removal of all the selected features.
        /// </summary>
        void RemoveSelectedFeatures();

        /// <summary>
        /// Selects the specified list of features.  If the specified feature is already selected,
        /// this method will not alter it.
        /// </summary>
        /// <param name="featureIndices">
        /// A List of integers representing the zero-based feature index values
        /// </param>
        void Select(IEnumerable<int> featureIndices);

        /// <summary>
        /// Selects a single feature specified by the integer index in the Features list.
        /// </summary>
        /// <param name="featureIndex">
        /// The zero-based integer index of the feature.
        /// </param>
        void Select(int featureIndex);

        /// <summary>
        /// Selects the specified feature.
        /// </summary>
        /// <param name="feature">
        /// The feature.
        /// </param>
        void Select(IFeature feature);

        /// <summary>
        /// Cycles through all the features and selects them
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Selects all the features in this layer that are associated
        /// with the specified attribute, clearing the selection first.
        /// </summary>
        /// <param name="filterExpression">
        /// The string expression to
        /// identify based on attributes the features to select.
        /// </param>
        void SelectByAttribute(string filterExpression);

        /// <summary>
        /// Modifies the features with a new selection based on the modifyMode.
        /// </summary>
        /// <param name="filterExpression">
        /// The string filter expression to use
        /// </param>
        /// <param name="modifyMode">
        /// Determines how the newly chosen features should interact with the existing
        /// selection
        /// </param>
        void SelectByAttribute(string filterExpression, ModifySelectionMode modifyMode);

        /// <summary>
        /// Sets the category for the specified shape index regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="index">
        /// The 0 based integer shape index
        /// </param>
        /// <param name="category">
        /// The category for this feature.  The exact kind of category depends on the
        /// feature type.
        /// </param>
        void SetCategory(int index, IFeatureCategory category);

        /// <summary>
        /// Sets the visible characteristic for a given feature, rather than using the index
        /// regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="feature">
        /// The actual reference to the feature object to update
        /// </param>
        /// <param name="category">
        /// The new category to use for the specified feature
        /// </param>
        void SetCategory(IFeature feature, IFeatureCategory category);

        /// <summary>
        /// This forces the creation of a category for the specified symbolizer, if it doesn't exist.
        /// This will add the specified feature to the category.  Be sure that the symbolizer type
        /// matches the feature type.
        /// </summary>
        /// <param name="index">
        /// The integer index of the shape to control.
        /// </param>
        /// <param name="symbolizer">
        /// The symbolizer to assign.
        /// </param>
        void SetShapeSymbolizer(int index, IFeatureSymbolizer symbolizer);

        /// <summary>
        /// Sets the visible characteristic for an individual feature
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <param name="visible">
        /// </param>
        void SetVisible(int index, bool visible);

        /// <summary>
        /// Sets the visible characteristic for a given feature, rather than using the index.
        /// </summary>
        /// <param name="feature">
        /// </param>
        /// <param name="visible">
        /// </param>
        void SetVisible(IFeature feature, bool visible);

        /// <summary>
        /// Displays a form with the attributes for this shapefile.
        /// </summary>
        void ShowAttributes();

        /// <summary>
        /// Creates a bitmap of the requested size that covers the specified geographic extent using
        /// the current symbolizer for this layer.  This does not have any drawing optimizations,
        /// or techniques to speed up performance and should only be used in special cases like
        /// draping of vector content onto a texture.  It also doesn't worry about selections.
        /// </summary>
        /// <param name="geographicExtent">
        /// The extent to use when computing the snapshot.
        /// </param>
        /// <param name="width">
        /// The integer height of the bitmap
        /// </param>
        /// <returns>
        /// A Bitmap object
        /// </returns>
        Bitmap SnapShot(Extent geographicExtent, int width);

        /// <summary>
        /// Unselects the specified features.  If any features already unselected, they are ignored.
        /// </summary>
        /// <param name="featureIndices">
        /// </param>
        void UnSelect(IEnumerable<int> featureIndices);

        /// <summary>
        /// Unselects the specified feature.
        /// </summary>
        /// <param name="featureIndex">
        /// The integer representing the feature to unselect.
        /// </param>
        void UnSelect(int featureIndex);

        /// <summary>
        /// Removes the specified feature from the selection
        /// </summary>
        /// <param name="feature">
        /// The feature to remove
        /// </param>
        void UnSelect(IFeature feature);

        /// <summary>
        /// Unselects all the features that are currently selected
        /// </summary>
        void UnSelectAll();

        /// <summary>
        /// Zooms to the envelope of the selected features.
        /// </summary>
        void ZoomToSelectedFeatures();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base FeatureSet
        /// </summary>
        new IFeatureSet DataSet { get; set; }

        /// <summary>
        /// Controls the drawn states according to a feature index.  This is used if the EditMode is
        /// false.  When EditMode is true, then drawn states are tied to the features instead.
        /// </summary>
        FastDrawnState[] DrawnStates { get; set; }

        /// <summary>
        /// Gets or sets the Boolean flag that controls whether the DrawnStates are needed.  If nothing is selected,
        /// and there is only one category, and there is no filter expression on that category, then this should be false.
        /// </summary>
        bool DrawnStatesNeeded { get; set; }

        /// <summary>
        /// Gets or sets a Boolean.  If edit mode is true, feature index is ignored, and features
        /// are assumed to be entirely loaded into ram.  If edit mode is false, then index
        /// is used instead and features are not assumed to be loaded into ram.
        /// </summary>
        bool EditMode { get; set; }

        /// <summary>
        /// Gets or sets the label layer
        /// </summary>
        [Category("General"), Description("Gets or sets the label layer associated with this feature layer.")]
        ILabelLayer LabelLayer { get; set; }

        /// <summary>
        /// Gets a Selection class that is allows the user to cycle through all the unselected features with
        /// a for each method, even if the features are in many categories.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ISelection Selection { get; }

        /// <summary>
        /// Gets or sets the shared characteristics to use with the selected features.
        /// </summary>
        IFeatureSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets whether labels should be drawn.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether labels should be drawn.")]
        bool ShowLabels { get; set; }

        /// <summary>
        /// Gets or sets and interface for the shared symbol characteristics between point, line and polygon features
        /// </summary>
        IFeatureSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets the current feature scheme, but to change it ApplyScheme should be called, so that
        /// feature categories are updated as well.
        /// </summary>
        IFeatureScheme Symbology { get; set; }

        #endregion
    }
}
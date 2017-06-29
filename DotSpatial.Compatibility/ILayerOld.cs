// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:44:42 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Layer
    /// </summary>
    public interface ILayerOld
    {
        #region Methods

        /// <summary>
        /// Returns the <c>MapWinGIS.Grid</c> object associated with the layer.  If the layer is not a
        /// grid layer, "Nothing" will be returned.
        /// </summary>
        IRaster GetGridObject { get; }

        /// <summary>
        /// Indicates whether to skip over the layer when saving a project.
        /// </summary>
        bool SkipOverDuringSave { get; set; }

        /// <summary>
        /// Adds a label to this layer.
        /// </summary>
        /// <param name="text">The text of the label.</param>
        /// <param name="textColor">The color of the label text.</param>
        /// <param name="xPos">X position in projected map units.</param>
        /// <param name="yPos">Y position in projected map units.</param>
        /// <param name="justification">Text justification.  Can be hjCenter, hjLeft or hjRight.</param>
        void AddLabel(string text, Color textColor, double xPos, double yPos, HJustification justification);

        ///The following function was added for plug-in 3.1 version compatibility
        /// <summary>
        /// Adds an extended label to this layer.
        /// </summary>
        /// <param name="text">The text of the label.</param>
        /// <param name="textColor">The color of the label text.</param>
        /// <param name="xPos">X position in projected map units.</param>
        /// <param name="yPos">Y position in projected map units.</param>
        /// <param name="justification">Text justification.  Can be hjCenter, hjLeft or hjRight.</param>
        /// <param name="rotation">The rotation angle for the label.</param>
        void AddLabelEx(string text, Color textColor, double xPos, double yPos, HJustification justification, double rotation);

        /// <summary>
        /// Clears all labels for this layer.
        /// </summary>
        void ClearLabels();

        /// <summary>
        /// Clears out the list of images to be used by the tkImageList point type.
        /// See also UserPointImageListAdd, UserPointImageListItem, UserPointImageListCount,
        /// and (on the shape interface) ShapePointImageListID.
        /// </summary>
        void ClearUDPointImageList();

        /// <summary>
        /// Sets the font to use for all labels on this layer.
        /// </summary>
        /// <param name="fontName">Name of the font or font family.  Example:  "Arial"</param>
        /// <param name="fontSize">Size of the font.</param>
        void Font(string fontName, int fontSize);

        /// <summary>
        /// Gets the underlying MapWinGIS object for this layer.  The object can be either a
        /// <c>MapWinGIS.Shapefile</c> or a <c>MapWinGIS.Image</c>.  If the layer is a grid layer the
        /// <c>MapWinGIS.Grid</c> object can be retrieved using the <c>GetGridObject</c> method.
        /// </summary>
        IDataSet GetObject();

        /// <summary>
        /// Gets a single row in the user defined line stipple.  There are 32 rows in a fill stipple
        /// (0-31).  Each row is defined the same way as a <c>UserLineStipple</c>.
        /// </summary>
        /// <param name="row">The index of the row to get.  Must be between 0 and 31 inclusive.</param>
        /// <returns>A single stipple row.</returns>
        int GetUserFillStipple(int row);

        /// <summary>
        /// Hides all vertices in the shapefile.  Only applies to line and polygon shapefiles.
        /// </summary>
        void HideVertices();

        /// <summary>
        /// Loads the shapefile rendering properties from a .mwsr file whose base fileName matches the shapefile.
        /// If the file isn't found, false is returned.
        /// Function call is ignored and returns false if the layer is a grid.
        /// </summary>
        bool LoadShapeLayerProps();

        /// <summary>
        /// Loads the shapefile rendering properties from the specified fileName.
        /// Function call is ignored and returns false if the layer is a grid.
        /// </summary>
        bool LoadShapeLayerProps(string loadFromFilename);

        /// <summary>
        /// Moves this layer to a new position in the layer list.  The highest position is the topmost layer
        /// in the display.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        /// <param name="targetGroup">The group to put this layer in.</param>
        void MoveTo(int newPosition, int targetGroup);

        /// <summary>
        /// Saves the shapefile rendering properties to a .mwsr file whose base fileName matches the shapefile.
        /// Function call is ignored and returns false if the layer is a grid.
        /// </summary>
        bool SaveShapeLayerProps();

        /// <summary>
        /// Saves the shapefile rendering properties to the specified fileName.
        /// Function call is ignored if the layer is a grid.
        /// </summary>
        bool SaveShapeLayerProps(string saveToFilename);

        /// <summary>
        /// Sets a single row in the user defined line stipple.  There are 32 rows in a fill stipple
        /// (0-31).  Each row is defined the same way as a <c>UserLineStipple</c>.
        /// </summary>
        /// <param name="row">The index of the row to set.  Must be between 0 and 31 inclusive.</param>
        /// <param name="value">The row value to set in the fill stipple.</param>
        void SetUserFillStipple(int row, int value);

        /// <summary>
        /// Updates the label information file stored for this layer.
        /// </summary>
        void UpdateLabelInfo();

        /// <summary>
        /// Adds an image to the list of point images used by the tkImageList point type.
        /// See also UserPointImageListItem, UserPointImageListCount, ClearUDPointImageList,
        /// and (on the shape interface) ShapePointImageListID.
        /// </summary>
        /// <param name="newValue">The new image to add.</param>
        /// <returns>The index for this image, to be passed to ShapePointImageListID or other functions.</returns>
        long UserPointImageListAdd(Image newValue);

        /// <summary>
        /// Gets the count of images from the list of images to be used by the tkImageList point type.
        /// See also UserPointImageListAdd, UserPointImageListItem, ClearUDPointImageList,
        /// and (on the shape interface) ShapePointImageListID.
        /// </summary>
        /// <returns>The count of items in the image list.</returns>
        long UserPointImageListCount();

        /// <summary>
        /// Gets an image from the list of images to be used by the tkImageList point type.
        /// See also UserPointImageListAdd, UserPointImageListCount, ClearUDPointImageList,
        /// and (on the shape interface) ShapePointImageListID.
        /// </summary>
        /// <param name="imageIndex">The image index to retrieve.</param>
        /// <returns>The index associated with this index; or null/nothing if nonexistant.</returns>
        Image UserPointImageListItem(long imageIndex);

        /// <summary>
        /// Zooms the display to this layer, taking into acount the <c>View.ExtentPad</c>.
        /// </summary>
        void ZoomTo();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of this shapefile.  Applies only to shapefiles.  Setting the color of the
        /// shapefile will clear any selected shapes and will also reset each individual shape to the same color.
        /// The coloring scheme will also be overriden.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the coloring scheme.  The <c>Shapefile</c> and <c>Grid</c> objects each have
        /// their own coloring scheme object.  It is important to cast the <c>ColoringScheme</c> to the
        /// proper type.
        /// </summary>
        object ColoringScheme { get; set; }

        /// <summary>
        /// Gets or sets whether or not to draw the fill for a polygon shapefile.
        /// </summary>
        bool DrawFill { get; set; }

        /// <summary>
        /// Gets or sets the extents where the layer changes from visible to not visible or vice versa.
        ///
        /// If the map is zoomed beyond these extents, the layer is invisible until the map is zoomed to
        /// be within these extents.
        /// </summary>
        Envelope DynamicVisibilityExtents { get; set; }

        /// <summary>
        /// Gets or sets whether the layer's coloring scheme is expanded in the legend.
        /// </summary>
        bool Expanded { get; set; }

        /// <summary>
        /// Returns the extents of this layer.
        /// </summary>
        Envelope Extents { get; }

        /// <summary>
        /// Returns the fileName of this layer.  If the layer is memory-based only it may not have a valid fileName.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets or sets the stipple pattern to use for the entire shapefile.
        ///
        /// The valid values for this property are:
        /// <list type="bullet">
        /// <item>Custom</item>
        /// <item>DashDotDash</item>
        /// <item>Dashed</item>
        /// <item>Dotted</item>
        /// <item>None</item>
        /// </list>
        /// </summary>
        Stipple FillStipple { get; set; }

        /// <summary>
        /// Gets or sets the position of the layer without respect to any group.
        /// </summary>
        int GlobalPosition { get; set; }

        /// <summary>
        /// Gets or sets the handle of the group that this layer belongs to.
        /// </summary>
        int GroupHandle { get; set; }

        /// <summary>
        /// Gets or sets the position of the layer within a group.
        /// </summary>
        int GroupPosition { get; set; }

        /// <summary>
        /// Returns the layer handle of this layer.  The DotSpatial automatically sets the <c>LayerHandle</c> for the layer, and it cannot be reset.
        /// </summary>
        int Handle { get; set; }

        /// <summary>
        /// Indicates whether to skip over the layer when drawing the legend.
        /// </summary>
        bool HideFromLegend { get; set; }

        /// <summary>
        /// Gets or sets the icon to use in the legend for this layer.
        /// </summary>
        Image Icon { get; set; }

        /// <summary>
        /// Gets or sets the color that represents transparent in an <c>Image</c> layer.
        /// </summary>
        Color ImageTransparentColor { get; set; }

        ///The following label properties were added for plug-in 3.1 version compatibility
        /// <summary>
        /// Determines the distance from the label point to the text for the label in pixels.
        /// </summary>
        int LabelsOffset { get; set; }

        /// <summary>
        /// Determines whether labels are scaled for this layer.
        /// </summary>
        bool LabelsScale { get; set; }

        /// <summary>
        /// Determines whether labels are shadowed for this layer.
        /// </summary>
        bool LabelsShadow { get; set; }

        /// <summary>
        /// Determines the color of the labels shadows.
        /// </summary>
        Color LabelsShadowColor { get; set; }

        /// <summary>
        /// Turns labels on or off for this layer.
        /// </summary>
        bool LabelsVisible { get; set; }

        /// <summary>
        /// Returns the type of this layer.  Valid values are:
        /// <list type="bullet">
        /// <item>Grid</item>
        /// <item>Image</item>
        /// <item>Invalid</item>
        /// <item>LineShapefile</item>
        /// <item>PointShapefile</item>
        /// <item>PolygonShapefile</item>
        /// </list>
        /// </summary>
        LayerType LayerType { get; }

        /// <summary>
        /// Gets or sets the line or point size.  If the <c>PointType</c> is <c>ptUserDefined</c> then the
        /// size of the user defined point will be multiplied by the <c>LineOrPointSize</c>.  For all other
        /// points and for lines, the <c>LineOrPointSize</c> is represented in pixels.
        /// </summary>
        float LineOrPointSize { get; set; }

        ///The following was added for plug-in 3.1 version compatibility
        /// <summary>
        /// Sets the width between lines for multiple-line drawing styles (e.g, doublesolid).
        /// </summary>
        int LineSeparationFactor { get; set; }

        /// <summary>
        /// Gets or sets the stipple pattern to use for the entire shapefile.
        ///
        /// The valid values for this property are:
        /// <list type="bullet">
        /// <item>Custom</item>
        /// <item>DashDotDash</item>
        /// <item>Dashed</item>
        /// <item>Dotted</item>
        /// <item>None</item>
        /// </list>
        /// </summary>
        Stipple LineStipple { get; set; }

        /// <summary>
        /// Returns or sets the projection of this layer.
        /// Projections must be / will be in PROJ4 format.
        /// If no projection is present, "" will be returned.
        /// If an invalid projection is provided, it's not guaranteed to be saved!
        /// </summary>
        string Projection { get; set; }

        /// <summary>
        /// Gets or sets the layer name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the outline color for this layer.  Only applies to polygon shapefile layers.
        /// </summary>
        Color OutlineColor { get; set; }

        /// <summary>
        /// Gets or sets the Point Image Scheme object for a given point layer. To be used from DotSpatial only
        /// </summary>
        object PointImageScheme { get; set; }

        /// <summary>
        /// Gets or sets the point type for this shapefile.
        ///
        /// The valid values for this property are:
        /// <list type="bullet">
        /// <item>ptCircle</item>
        /// <item>ptDiamond</item>
        /// <item>ptSquare</item>
        /// <item>ptTriangleDown</item>
        /// <item>ptTriangleLeft</item>
        /// <item>ptTriangleRight</item>
        /// <item>ptTriangleUp</item>
        /// <item>ptUserDefined</item>
        /// </list>
        /// </summary>
        PointType PointType { get; set; }

        /// <summary>
        /// This property gives access to all shapes in the layer.  Only applies to shapefile layers.
        /// </summary>
        List<IFeature> Shapes { get; }

        /// <summary>
        /// Gets or sets the tag for this layer.  The tag is simply a string that can be used by the
        /// programmer to store any information desired.
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        /// Specifies whether or not to use <c>DynamicVisibility</c>.
        /// </summary>
        bool UseDynamicVisibility { get; set; }

        /// <summary>
        /// Determines whether MapWinGIS ocx will hide labels which collide with already drawn labels or not.
        /// </summary>
        bool UseLabelCollision { get; set; }

        /// <summary>
        /// Gets or sets the user defined line stipple.  A line stipple is simply a 32-bit integer
        /// whose bits define a pattern that can be displayed on the screen.  For example, the value
        /// 0011 0011 in binary would represent a dashed line (  --  --).
        /// </summary>
        int UserLineStipple { get; set; }

        /// <summary>
        /// Gets or sets the user defined point image for this layer.  To display the user defined point
        /// the layer's <c>PointType</c> must be set to <c>ptUserDefined</c>.
        /// </summary>
        Image UserPointType { get; set; }

        /// <summary>
        /// Gets or sets whether to use transparency on an <c>Image</c> layer.
        /// </summary>
        bool UseTransparentColor { get; set; }

        /// <summary>
        /// (Doesn't apply to line shapefiles)
        /// Indicates whether the vertices of a line or polygon are visible.
        /// </summary>
        bool VerticesVisible { get; set; }

        /// <summary>
        /// Gets or sets whether the layer is visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Shows all vertices for the entire shapefile.  Applies only to line and polygon shapefiles.
        /// </summary>
        /// <param name="color">The color to draw vertices with.</param>
        /// <param name="vertexSize">The size of each vertex.</param>
        void ShowVertices(Color color, int vertexSize);

        /// <summary>
        /// Returns information about the given layer in a human-readible string.
        /// </summary>
        string ToString();

        #endregion
    }
}
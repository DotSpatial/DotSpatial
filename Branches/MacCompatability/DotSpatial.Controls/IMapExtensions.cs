using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;

namespace DotSpatial.Controls
{
    public static class IMapExtensions
    {
        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.
        /// This will not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.
        /// Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this
        /// category.</param>
        /// <param name="name">The name of the category.</param>
        [Obsolete("Use featureLayer.AddLabels() instead")] // Marked in 1.7
        public static void AddLabels(this IMap map, IFeatureLayer featureLayer, string expression,
            string filterExpression, ILabelSymbolizer symbolizer, string name)
        {
            featureLayer.AddLabels(expression, filterExpression, symbolizer, name);
        }

        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.  This will
        /// not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.
        /// Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this
        ///  category.</param>
        /// <param name="width">A geographic width, so that if the map is zoomed to a geographic width smaller than
        /// this value, labels should appear.</param>
        [Obsolete("Use featureLayer.AddLabels() instead")] // Marked in 1.7
        public static void AddLabels(this IMap map, IFeatureLayer featureLayer, string expression,
            string filterExpression, ILabelSymbolizer symbolizer, double width)
        {
            featureLayer.AddLabels(expression, filterExpression, symbolizer, width);
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        public static IMapImageLayer[] GetImageLayers(this IMap map)
        {
            return map.MapFrame.Layers.OfType<IMapImageLayer>().ToArray();
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        public static IMapRasterLayer[] GetRasterLayers(this IMap map)
        {
            return map.MapFrame.Layers.OfType<IMapRasterLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public static IMapLineLayer[] GetLineLayers(this IMap map)
        {
            return map.MapFrame.Layers.OfType<IMapLineLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public static IMapPolygonLayer[] GetPolygonLayers(this IMap map)
        {
            return map.MapFrame.Layers.OfType<IMapPolygonLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public static IMapPointLayer[] GetPointLayers(this IMap map)
        {
            return map.MapFrame.Layers.OfType<IMapPointLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the feature layers regardless of whether they are lines, points, or polygons
        /// </summary>
        /// <returns>An array of IMapFeatureLayers</returns>
        public static IMapFeatureLayer[] GetFeatureLayers(this IMap map)
        {
            return map.MapFrame.Layers.OfType<IMapFeatureLayer>().ToArray();
        }

        /// <summary>
        /// Gets the MapFunction based on the string name
        /// </summary>
        /// <param name="name">The string name to find</param>
        /// <returns>The MapFunction with the specified name</returns>
        public static IMapFunction GetMapFunction(this IMap map, string name)
        {
            return map.MapFunctions.First(f => f.Name == name);
        }

        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        public static bool ClearSelection(this IMap map, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (map.MapFrame == null) return false;
            return map.MapFrame.ClearSelection(out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public static bool Select(this IMap map, IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (map.MapFrame == null) return false;
            return map.MapFrame.Select(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode determining how to test for intersection.</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members.</param>
        /// <returns>boolean, true if members were changed by the selection process.</returns>
        public static bool InvertSelection(this IMap map, IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (map.MapFrame == null) return false;
            return map.MapFrame.InvertSelection(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public static bool UnSelect(this IMap map, IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (map.MapFrame == null) return false;
            return map.MapFrame.UnSelect(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// This activates the labels for the specified feature layer that will be the specified expression
        /// where field names are in square brackets like "[Name]: [Value]".  This will label all the features,
        /// and remove any previous labeling.
        /// </summary>
        /// <param name="featureLayer">The FeatureLayer to apply the labels to.</param>
        /// <param name="expression">The string label expression to use where field names are in square brackets like
        /// [Name]</param>
        /// <param name="font">The font to use for these labels</param>
        /// <param name="fontColor">The color for the labels</param>
        [Obsolete("Use featureLayer.AddLabels() instead")] // Marked in 1.7
        public static void AddLabels(this IMap map, IFeatureLayer featureLayer, string expression, Font font, Color fontColor)
        {
            featureLayer.AddLabels(expression, font, fontColor);
        }

        /// <summary>
        /// Removes any existing label categories
        /// </summary>
        [Obsolete("Use featureLayer.ClearLabels() instead")] // Marked in 1.7
        public static void ClearLabels(this IMap map, IFeatureLayer featureLayer)
        {
            featureLayer.ClearLabels();
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        public static void Print(this IMap map, Graphics device, Rectangle targetRectangle)
        {
            map.MapFrame.Print(device, targetRectangle);
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        /// <param name="targetEnvelope">the extents to print in the target rectangle</param>
        public static void Print(this IMap map, Graphics device, Rectangle targetRectangle, Extent targetEnvelope)
        {
            map.MapFrame.Print(device, targetRectangle, targetEnvelope);
        }

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public static void ZoomIn(this IMap map)
        {
            map.MapFrame.ZoomIn();
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public static void ZoomOut(this IMap map)
        {
            map.MapFrame.ZoomOut();  
        }

        /// <summary>
        /// Zooms to the next extent
        /// </summary>
        public static void ZoomToNext(this IMap map)
        {
            map.MapFrame.ZoomToNext();
        }

        /// <summary>
        /// Zooms to the previous extent
        /// </summary>
        public static void ZoomToPrevious(this IMap map)
        {
            map.MapFrame.ZoomToPrevious();
        }

        /// <summary>
        /// Captures an image of whatever the contents of the back buffer would be at the size of the screen.
        /// </summary>
        /// <returns></returns>
        public static Bitmap SnapShot(this IMap map)
        {
            Rectangle clip = map.ClientRectangle;
            Bitmap stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(stencil);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, stencil.Width, stencil.Height));

            // Translate the buffer so that drawing occurs in client coordinates, regardless of whether
            // there is a clip rectangle or not.
            Matrix m = new Matrix();
            m.Translate(-clip.X, -clip.Y);
            g.Transform = m;

            map.MapFrame.Draw(new PaintEventArgs(g, clip));
            g.Dispose();
            return stencil;
        }

        /// <summary>
        /// Creates a snapshot that is scaled to fit to a bitmap of the specified width.
        /// </summary>
        /// <param name="width">The width of the desired bitmap</param>
        /// <returns>A bitmap with the specified width</returns>
        public static Bitmap SnapShot(this IMap map, int width)
        {
            int height = (int)(width * (map.MapFrame.ViewExtents.Height / map.MapFrame.ViewExtents.Width));
            Bitmap bmp = new Bitmap(height, width);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            map.MapFrame.Print(g, new Rectangle(0, 0, width, height));
            g.Dispose();
            return bmp;
        }

        /// <summary>
        /// If the specified function is already in the list of functions, this will properly test the yield style of various
        /// map functions that are currently on and then activate the function.  If this function is not in the list, then
        /// it will add it to the list.  If you need to control the position, then insert the function before using this
        /// method to activate.  Be warned that calling "Activate" directly on your function will activate your function
        /// but not disable any other functions.  You can set "Map.FunctionMode = FunctionModes.None" first, and then
        /// specifically activate the function that you want.
        /// </summary>
        /// <param name="function">The MapFunction to activate, or add.</param>
        public static void ActivateMapFunction(this IMap map, IMapFunction function)
        {
            if (!map.MapFunctions.Contains(function))
            {
                map.MapFunctions.Add(function);
            }
            foreach (var f in map.MapFunctions)
            {
                if ((f.YieldStyle & YieldStyles.AlwaysOn) == YieldStyles.AlwaysOn) continue; // ignore "Always On" functions
                int test = (int)(f.YieldStyle & function.YieldStyle);
                if (test > 0) f.Deactivate(); // any overlap of behavior leads to deactivation
            }
            function.Activate();
        }
            
        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        public static void ClearLayers(this IMap map)
        {
            if (map.MapFrame == null) return;
            map.MapFrame.Layers.Clear();
        }

        /// <summary>
        /// returns a functional list of the ILayer members.  This list will be
        /// separate from the actual list stored, but contains a shallow copy
        /// of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns></returns>
        public static List<ILayer> GetLayers(this IMap map)
        {
            return map.MapFrame == null ? Enumerable.Empty<ILayer>().ToList() : map.MapFrame.Layers.Cast<ILayer>().ToList();
        }

        /// <summary>
        /// Gets all layers of the map including layers which are nested
        /// within groups. The group objects themselves are not included in this list,
        /// but all FeatureLayers, RasterLayers, ImageLayers and other layers are included.
        /// </summary>
        /// <returns>The list of the layers</returns>
        public static List<ILayer> GetAllLayers(this IMap map)
        {
            return map.MapFrame != null ? map.MapFrame.GetAllLayers() : null;
        }

        /// <summary>
        /// Gets all map groups in the map including the nested groups
        /// </summary>
        /// <returns>the list of the groups</returns>
        public static List<IMapGroup> GetAllGroups(this IMap map)
        {
            return map.MapFrame != null ? map.MapFrame.GetAllGroups() : null;
        }
    }
}


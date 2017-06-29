using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public static class IFeatureLayerExtensions
    {
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
        public static void AddLabels(this IFeatureLayer featureLayer, string expression, Font font, Color fontColor)
        {
            featureLayer.ShowLabels = true;

            var ll = new MapLabelLayer();
            ll.Symbology.Categories.Clear();
            var lc = new LabelCategory { Expression = expression };
            ll.Symbology.Categories.Add(lc);

            var ls = ll.Symbolizer;
            ls.Orientation = ContentAlignment.MiddleCenter;
            ls.FontColor = fontColor;
            ls.FontFamily = font.FontFamily.ToString();
            ls.FontSize = font.Size;
            ls.FontStyle = font.Style;
            ls.PartsLabelingMethod = PartLabelingMethod.LabelLargestPart;
            featureLayer.LabelLayer = ll;
        }

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
        public static void AddLabels(this IFeatureLayer featureLayer, string expression, string filterExpression,
                              ILabelSymbolizer symbolizer, string name)
        {
            if (featureLayer.LabelLayer == null) featureLayer.LabelLayer = new MapLabelLayer();
            featureLayer.ShowLabels = true;
            ILabelCategory lc = new LabelCategory
            {
                Expression = expression,
                FilterExpression = filterExpression,
                Symbolizer = symbolizer,
                Name = name,
            };
            featureLayer.LabelLayer.Symbology.Categories.Add(lc);
            featureLayer.LabelLayer.CreateLabels();
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
        public static void AddLabels(this IFeatureLayer featureLayer, string expression, string filterExpression,
                              ILabelSymbolizer symbolizer, double width)
        {
            if (featureLayer.LabelLayer == null) featureLayer.LabelLayer = new MapLabelLayer();
            featureLayer.ShowLabels = true;
            ILabelCategory lc = new LabelCategory
            {
                Expression = expression,
                FilterExpression = filterExpression,
                Symbolizer = symbolizer
            };
            featureLayer.LabelLayer.UseDynamicVisibility = true;
            featureLayer.LabelLayer.DynamicVisibilityWidth = width;
            featureLayer.LabelLayer.Symbology.Categories.Add(lc);
            featureLayer.LabelLayer.CreateLabels();
        }

        /// <summary>
        /// Removes any existing label categories
        /// </summary>
        public static void ClearLabels(this IFeatureLayer featureLayer)
        {
            featureLayer.ShowLabels = false;
        }
    }
}

// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2009 5:49:37 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Items with this setup can both be organized as an item,
    /// and feature the elemental control methods and properties
    /// around drawing.  Layers, MapFrames, groups etc can fall in this
    /// category.
    /// </summary>
    public interface IRenderableLegendItem : IRenderable, ILegendItem
    {
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Enums
// Description:  Enumerations used by the layout engine
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Controls
{
    /// <summary>
    /// An enumeration that defines the Mouses current behavior
    /// </summary>
    internal enum MouseMode
    {
        /// <summary>
        /// The cursor is currently in default mode
        /// </summary>
        Default,

        /// <summary>
        /// The cursor is currently being used to create a new selection
        /// </summary>
        CreateSelection,

        /// <summary>
        /// The cursor is currently is move selection mode
        /// </summary>
        MoveSelection,

        /// <summary>
        /// The cursor is in resize mode because its over the edge of a selected item
        /// </summary>
        ResizeSelected,

        /// <summary>
        /// When in this mode the user can click on the map select an area and an element is inserted at that spot
        /// </summary>
        InsertNewElement,

        /// <summary>
        /// In this mode a cross hair is shown letting the user create a new Insert rectangle
        /// </summary>
        StartInsertNewElement,

        /// <summary>
        /// Puts the mouse into a mode that allows map panning
        /// </summary>
        StartPanMap,

        /// <summary>
        /// The mouse is actually panning a map
        /// </summary>
        PanMap
    }

    /// <summary>
    /// Enumerates all the possible resize direction
    /// </summary>
    internal enum Edge
    {
        None,

        TopLeft,

        Top,

        TopRight,

        Right,

        BottomRight,

        Bottom,

        BottomLeft,

        Left,
    }

    /// <summary>
    /// Enumarates the different ways that a a LayoutElement can handle resize events
    /// </summary>
    public enum ResizeStyle
    {
        /// <summary>
        /// The resize style is determined automatically
        /// </summary>
        HandledInternally,
        /// <summary>
        /// The element is adjusted to fit the extents even if it is distorted
        /// </summary>
        StretchToFit,
        /// <summary>
        /// No scaling occurs whatsoever, and the element is drawn at its original size
        /// </summary>
        NoScaling
    }

    /// <summary>
    /// An enumeration of the possible scale bar units
    /// </summary>
    public enum ScaleBarUnit
    {
        /// <summary>
        /// Kilometers
        /// </summary>
        Kilometers,
        /// <summary>
        /// Meters
        /// </summary>
        Meters,
        /// <summary>
        /// Centimeters
        /// </summary>
        Centimeters,
        /// <summary>
        /// Millimeters
        /// </summary>
        Millimeters,
        /// <summary>
        /// Miles
        /// </summary>
        Miles,
        /// <summary>
        /// Yards
        /// </summary>
        Yards,
        /// <summary>
        /// Feet
        /// </summary>
        Feet,
        /// <summary>
        /// Inches
        /// </summary>
        Inches
    }

    /// <summary>
    /// An enumeration of alignments used for aligning selected layout elements
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Left
        /// </summary>
        Left,
        /// <summary>
        /// Right
        /// </summary>
        Right,
        /// <summary>
        /// Top
        /// </summary>
        Top,
        /// <summary>
        /// Bottom
        /// </summary>
        Bottom,
        /// <summary>
        /// Horizontal
        /// </summary>
        Horizontal,
        /// <summary>
        /// Vertical
        /// </summary>
        Vertical
    }

    /// <summary>
    /// An enumeration of resizing options used for aligning selected layout elements
    /// </summary>
    public enum Fit
    {
        /// <summary>
        /// Width
        /// </summary>
        Width,
        /// <summary>
        /// Height
        /// </summary>
        Height
    }
}
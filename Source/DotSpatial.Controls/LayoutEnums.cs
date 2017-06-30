// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls
{
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

        // CGX
        /// <summary>
        /// Nautical Miles
        /// </summary>
        NauticalMiles,
        // CGX END

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
    /// Enumerates all the possible resize direction.
    /// </summary>
    internal enum Edge
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// TopLeft
        /// </summary>
        TopLeft,

        /// <summary>
        /// Top
        /// </summary>
        Top,

        /// <summary>
        /// TopRight
        /// </summary>
        TopRight,

        /// <summary>
        /// Right
        /// </summary>
        Right,

        /// <summary>
        /// BottomRight
        /// </summary>
        BottomRight,

        /// <summary>
        /// Bottom
        /// </summary>
        Bottom,

        /// <summary>
        /// BottomLeft
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Left
        /// </summary>
        Left,
    }
}
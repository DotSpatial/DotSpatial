﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Anything that can draw itself to the map is an IRenderable. This is implemented by RenderBase.
    /// </summary>
    public interface IRenderable
    {
        #region Events

        /// <summary>
        /// Occurs whenever the geographic bounds for this renderable object have changed
        /// </summary>
        event EventHandler<EnvelopeArgs> EnvelopeChanged;

        /// <summary>
        /// Occurs when an outside request is sent to invalidate this object
        /// </summary>
        event EventHandler Invalidated;

        /// <summary>
        /// Occurs immediately after the visible parameter has been adjusted.
        /// </summary>
        event EventHandler VisibleChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets an Envelope in world coordinates that contains this object.
        /// </summary>
        /// <returns>The extent that contains this object.</returns>
        Extent Extent { get; }

        /// <summary>
        /// Gets a value indicating whether or not the unmanaged drawing structures have been created for this item.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the drawing function will render anything.
        /// Warning! If false this will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        bool IsVisible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invalidates the drawing methods
        /// </summary>
        void Invalidate();

        #endregion
    }
}
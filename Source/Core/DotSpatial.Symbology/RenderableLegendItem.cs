// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// RenderableLegendItem.
    /// </summary>
    public class RenderableLegendItem : LegendItem, IRenderableLegendItem
    {
        #region Fields

        private bool _isInitialized;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderableLegendItem"/> class.
        /// </summary>
        public RenderableLegendItem()
        {
            Checked = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the geographic bounds for this renderable object have changed
        /// </summary>
        public event EventHandler<EnvelopeArgs> EnvelopeChanged;

        /// <summary>
        /// Occurs when an outside request is sent to invalidate this object
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Occurs immediately after the visible parameter has been adjusted.
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets an Envelope in world coordinates that contains this object. This is virtual, and
        /// will usually be reconfigured in subclasses to simply show the dataset extent.
        /// </summary>
        [Category("General")]
        [Description("Obtains an Envelope that contains this object")]
        public virtual Extent Extent => MyExtent;

        /// <summary>
        /// Gets or sets a value indicating whether or not the unmanaged drawing structures have been created for this item.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }

            protected set
            {
                _isInitialized = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is visible.
        /// If this is false, then the drawing function will not render anything.
        /// Warning! This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        [Category("General")]
        [Description("Gets or sets whether or not this object will be drawn or painted.")]
        public virtual bool IsVisible
        {
            get
            {
                return Checked;
            }

            set
            {
                if (Checked != value)
                {
                    Checked = value;
                    OnVisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the protected extent. It is a direct, sealed accessor for the extent variable.
        /// This is safe to use in constructors.
        /// </summary>
        protected Extent MyExtent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invalidates the drawing methods.
        /// </summary>
        public virtual void Invalidate()
        {
            _isInitialized = false;
            OnInvalidate(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the EnvelopeChanged event.
        /// </summary>
        /// <param name="sender">The object sender for this event (this).</param>
        /// <param name="e">The EnvelopeArgs specifying the envelope.</param>
        protected virtual void OnEnvelopeChanged(object sender, EnvelopeArgs e)
        {
            EnvelopeChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires the Invalidated event.
        /// </summary>
        /// <param name="sender">The object sender (usually this).</param>
        /// <param name="e">An EventArgs parameter.</param>
        protected virtual void OnInvalidate(object sender, EventArgs e)
        {
            Invalidated?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires the Visible Changed event.
        /// </summary>
        /// <param name="sender">The object sender (usually this).</param>
        /// <param name="e">An EventArgs parameter.</param>
        protected virtual void OnVisibleChanged(object sender, EventArgs e)
        {
            VisibleChanged?.Invoke(sender, e);
        }

        #endregion
    }
}
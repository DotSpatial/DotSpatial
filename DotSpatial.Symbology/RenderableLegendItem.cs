// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2009 5:49:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// RenderableLegendItem
    /// </summary>
    public class RenderableLegendItem : LegendItem, IRenderableLegendItem
    {
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

        #region Private Variables

        private Extent _extent;
        private bool _isInitialized;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RenderBase
        /// </summary>
        public RenderableLegendItem()
        {
            base.Checked = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invalidates the drawing methods
        /// </summary>
        public virtual void Invalidate()
        {
            _isInitialized = false;
            OnInvalidate(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The protected extent is a direct, sealed accessor for the extent variable.
        /// This is safe to use in constructors.
        /// </summary>
        protected Extent MyExtent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        /// <summary>
        /// Obtains an IEnvelope in world coordinates that contains this object.  This is virtual, and
        /// will usually be reconfigured in subclasses to simply show the dataset extent.
        /// </summary>
        /// <returns></returns>
        [Category("General"), Description("Obtains an IEnvelope that contains this object")]
        public virtual Extent Extent
        {
            get { return _extent; }
        }

        /// <summary>
        /// Gets whether or not the unmanaged drawing structures have been created for this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsInitialized
        {
            get { return _isInitialized; }
            protected set
            {
                _isInitialized = value;
            }
        }

        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        [Category("General"), Description("Gets or sets whether or not this object will be drawn or painted.")]
        public virtual bool IsVisible
        {
            get { return base.Checked; }
            set
            {
                if (base.Checked != value)
                {
                    base.Checked = value;
                    OnVisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the EnvelopeChanged event.
        /// </summary>
        /// <param name="sender">The object sender for this event (this)</param>
        /// <param name="e">The EnvelopeArgs specifying the envelope</param>
        protected virtual void OnEnvelopeChanged(object sender, EnvelopeArgs e)
        {
            if (EnvelopeChanged != null)
            {
                EnvelopeChanged(sender, e);
            }
        }

        /// <summary>
        /// Fires the Invalidated event
        /// </summary>
        /// <param name="sender">The object sender (usually this)</param>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnInvalidate(object sender, EventArgs e)
        {
            if (Invalidated != null)
            {
                Invalidated(sender, e);
            }
        }

        /// <summary>
        /// Fires the Visible Changed event
        /// </summary>
        /// <param name="sender">The object sender (usually this)</param>
        /// <param name="e">An EventArgs parameter</param>
        protected virtual void OnVisibleChanged(object sender, EventArgs e)
        {
            if (VisibleChanged != null) VisibleChanged(sender, e);
        }

        #endregion
    }
}
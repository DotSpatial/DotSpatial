// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Legend item supposed to be located inside legend.
    /// </summary>
    [Serializable]
    public class LegendItem : Descriptor, ILegendItem
    {
        #region Fields
        private bool _changeOccured;
        private bool _checked;
        private bool _isDragable;
        private bool _isExpanded;
        private bool _isLegendGroup;
        private bool _isSelected;
        private int _itemChangedSuspend;
        private bool _legendItemVisible;
        private SymbolMode _legendSymbolMode;
        private Size _legendSymbolSize;
        private string _legendText;
        private bool _legendTextReadInly;
        private LegendType _legendType;
        private List<SymbologyMenuItem> _menuItems;
        private ILegendItem _parentLegendItem;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        public LegendItem()
        {
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the symbol content has been updated
        /// </summary>
        public event EventHandler ItemChanged;

        /// <summary>
        /// Occurs whenever the item should be removed from the parent collection
        /// </summary>
        public event EventHandler RemoveItem;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the changes are suspended.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public bool ChangesSuspended => _itemChangedSuspend > 0;

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Checked
        {
            get
            {
                return _checked;
            }

            set
            {
                _checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the MenuItems that should appear in the context menu of the legend for this category
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public virtual List<SymbologyMenuItem> ContextMenuItems
        {
            get
            {
                return _menuItems;
            }

            set
            {
                _menuItems = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this legend item can be dragged to a new position in the legend.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragable
        {
            get
            {
                return _isDragable;
            }

            set
            {
                _isDragable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the legend should draw the child LegendItems for this category.
        /// </summary>
        [Serialize("IsExpanded")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }

            set
            {
                _isExpanded = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this legend item has been selected in the legend
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
            }
        }

        /// <summary>
        /// Gets whatever the child collection is and returns it as an IEnumerable set of legend items
        /// in order to make it easier to cycle through those values. This defaults to null and must
        /// be overridden in specific cases where child legend items exist.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable<ILegendItem> LegendItems => null;

        /// <summary>
        /// Gets or sets a value indicating whether this item and its child items
        /// appear in the legend when the legend is drawn.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool LegendItemVisible
        {
            get
            {
                return _legendItemVisible;
            }

            set
            {
                _legendItemVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbol mode for the legend. By default this should be "Symbol", but this can be overridden
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SymbolMode LegendSymbolMode
        {
            get
            {
                return _legendSymbolMode;
            }

            protected set
            {
                _legendSymbolMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the text for this category to appear in the legend. This might be a category name,
        /// or a range of values.
        /// </summary>
        [Description("Gets or sets the text for this category to appear in the legend.")]
        [Browsable(false)]
        [Serialize("LegendText")]
        public virtual string LegendText
        {
            get
            {
                return _legendText;
            }

            set
            {
                if (value == _legendText) return;
                _legendText = value;
                OnItemChanged(this);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can change the legend text in GUI.
        /// </summary>
        [Description("Indicates whether the user can change the legend text in GUI.")]
        [Browsable(false)]
        [Serialize("LegendTextReadOnly")]
        public virtual bool LegendTextReadOnly
        {
            get
            {
                return _legendTextReadInly;
            }

            set
            {
                if (value == _legendTextReadInly) return;
                _legendTextReadInly = value;
                OnItemChanged(this);
            }
        }

        /// <summary>
        /// Gets or sets a pre-defined behavior in the legend when referring to drag and drop functionality.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public LegendType LegendType
        {
            get
            {
                return _legendType;
            }

            protected set
            {
                _legendType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a legend group.
        /// </summary>
        [Serialize("IsLegendGroup")]
        protected bool IsLegendGroup
        {
            get
            {
                return _isLegendGroup;
            }

            set
            {
                _isLegendGroup = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a boolean indicating whether or not this item can have other items dropped on it.
        /// By default this is false. This can be overridden for more customized behaviors.
        /// </summary>
        /// <param name="item">The item to test for dropping.</param>
        /// <returns>A boolean indicating whether or not this item can have other items dropped on it.</returns>
        public virtual bool CanReceiveItem(ILegendItem item)
        {
            if (LegendType == LegendType.Scheme)
            {
                if (item.LegendType == LegendType.Symbol) return true;
                return false;
            }

            if (LegendType == LegendType.Group)
            {
                if (item.LegendType == LegendType.Symbol) return false;
                if (item.LegendType == LegendType.Scheme) return false;
                return true;
            }

            if (LegendType == LegendType.Layer)
            {
                if (item.LegendType == LegendType.Symbol) return true;
                if (item.LegendType == LegendType.Scheme) return true;
                return false;
            }

            if (LegendType == LegendType.Symbol)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Gets the size of the symbol to be drawn to the legend.
        /// </summary>
        /// <returns>The size of the symbol.</returns>
        public virtual Size GetLegendSymbolSize()
        {
            return _legendSymbolSize;
        }

        /// <summary>
        /// Gets the Parent Legend item for this category. This should probably be the appropriate layer item.
        /// </summary>
        /// <returns>The parent legend item.</returns>
        public ILegendItem GetParentItem()
        {
            return _parentLegendItem;
        }

        /// <summary>
        /// Draws the symbol for this specific category to the legend.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="box">Rectangle</param>
        public virtual void LegendSymbolPainted(Graphics g, Rectangle box)
        {
        }

        /// <summary>
        /// Prints the formal legend content without any resize boxes or other notations.
        /// </summary>
        /// <param name="g">The graphics object to print to</param>
        /// <param name="font">The system.Drawing.Font to use for the lettering</param>
        /// <param name="fontColor">The color of the font</param>
        /// <param name="maxExtent">Assuming 0, 0 is the top left, this is the maximum extent</param>
        public void PrintLegendItem(Graphics g, Font font, Color fontColor, SizeF maxExtent)
        {
            string text = LegendText;
            if (text == null)
            {
                ILegendItem parent = GetParentItem();
                if (parent?.LegendItems.Count() == 1)
                {
                    // go ahead and use the layer name, but only if this is the only category and the legend text is null
                    text = parent.LegendText;
                }
            }

            // if LegendText is null, the measure string helpfully chooses a height that is 0, so use a fake text for height calc
            SizeF emptyString = g.MeasureString("Sample text", font);

            float h = emptyString.Height;
            float x = 0;
            bool drawBox = false;
            if (LegendSymbolMode == SymbolMode.Symbol)
            {
                drawBox = true;
                x = h * 2 + 4;
            }

            Brush b = new SolidBrush(fontColor);
            StringFormat frmt = new StringFormat
            {
                Alignment = StringAlignment.Near,
                Trimming = StringTrimming.EllipsisCharacter
            };
            float w = maxExtent.Width - x;
            g.DrawString(text, font, b, new RectangleF(x, 2, w, h), frmt);
            if (drawBox) LegendSymbolPainted(g, new Rectangle(2, 2, (int)x - 4, (int)h));

            b.Dispose();
        }

        /// <summary>
        /// Allows the ItemChanged event to fire in response to individual changes again.
        /// This will also fire the event once if there were any changes that warent it
        /// that were made while the event was suspended.
        /// </summary>
        public void ResumeChangeEvent()
        {
            _itemChangedSuspend -= 1;
            if (_itemChangedSuspend == 0)
            {
                if (_changeOccured)
                {
#if DEBUG
                    var sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
#endif
                    OnItemChanged();
#if DEBUG
                    sw.Stop();
                    System.Diagnostics.Debug.WriteLine("OnItemChanged time:" + sw.ElapsedMilliseconds);
#endif
                }
            }

            // Prevent forcing extra negatives.
            if (_itemChangedSuspend < 0) _itemChangedSuspend = 0;
        }

        /// <summary>
        /// Sets the parent legend item for this category.
        /// </summary>
        /// <param name="value">The parent item.</param>
        public void SetParentItem(ILegendItem value)
        {
            OnSetParentItem(value);
        }

        /// <summary>
        /// Each suspend call increments an integer, essentially keeping track of the depth of
        /// suspension. When the same number of ResumeChangeEvents methods have been called
        /// as SuspendChangeEvents have been called, the suspension is aborted and the
        /// legend item is allowed to broadcast its changes.
        /// </summary>
        public void SuspendChangeEvent()
        {
            if (_itemChangedSuspend == 0)
            {
                _changeOccured = false;
            }

            _itemChangedSuspend += 1;
        }

        /// <summary>
        /// Handles updating event handlers during a copy process.
        /// </summary>
        /// <param name="copy">The duplicate descriptor</param>
        protected override void OnCopy(Descriptor copy)
        {
            LegendItem myCopy = copy as LegendItem;
            if (myCopy?.ItemChanged != null)
            {
                foreach (Delegate handler in myCopy.ItemChanged.GetInvocationList())
                {
                    myCopy.ItemChanged -= (EventHandler)handler;
                }
            }

            if (myCopy?.RemoveItem != null)
            {
                foreach (Delegate handler in myCopy.RemoveItem.GetInvocationList())
                {
                    myCopy.RemoveItem -= (EventHandler)handler;
                }
            }

            base.OnCopy(copy);
        }

        /// <summary>
        /// Fires the ItemChanged event
        /// </summary>
        protected virtual void OnItemChanged()
        {
            OnItemChanged(this);
        }

        /// <summary>
        /// Fires the ItemChanged event, optionally specifying a different sender.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        protected virtual void OnItemChanged(object sender)
        {
            if (_itemChangedSuspend > 0)
            {
                _changeOccured = true;
                return;
            }

            ItemChanged?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Instructs the parent legend item to remove this item from the list of legend items.
        /// </summary>
        protected virtual void OnRemoveItem()
        {
            RemoveItem?.Invoke(this, EventArgs.Empty);

            // Maybe we don't need RemoveItem event. We could just invoke a method on the parent.
            // One less thing to wire. But we currently need to wire parents.
        }

        /// <summary>
        /// Allows for the set behavior for the parent item to be overridden in child classes.
        /// </summary>
        /// <param name="value">The parent item.</param>
        protected virtual void OnSetParentItem(ILegendItem value)
        {
            _parentLegendItem = value;
        }

        /// <summary>
        /// Configures the default settings of the legend item
        /// </summary>
        private void Configure()
        {
            _legendItemVisible = true;
            _isSelected = false;
            _isExpanded = false;
            _legendSymbolMode = SymbolMode.Symbol;
            _legendSymbolSize = new Size(16, 16);
            _isDragable = false;
            _legendType = LegendType.Symbol;
        }

        #endregion
    }
}
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/16/2009 5:25:17 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LegendItem
    /// </summary>
    [Serializable]
    public class LegendItem : Descriptor, ILegendItem
    {
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

        #region Private Variables

        // Legend Item properties
        //private IChangeEventList<ILegendItem> _legendItems; // not used by mapwindow, but can be used by developers
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
        private LegendType _legendType;
        private List<SymbologyMenuItem> _menuItems;
        private ILegendItem _parentLegendItem;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the legend item
        /// </summary>
        public LegendItem()
        {
            Configure();
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

        #region Methods

        /// <summary>
        /// Returns a boolean indicating whether or not this item can have other items dropped on it.
        /// By default this is false.  This can be overridden for more customized behaviors.
        /// </summary>
        /// <param name="item">The item to test for dropping.</param>
        /// <returns></returns>
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
        /// Draws the symbol for this specific category to the legend
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public virtual void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            // throw new NotImplementedException("This should be implemented in a sub-class");
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
                if (parent != null)
                {
                    if (parent.LegendItems.Count() == 1)
                    {
                        // go ahead and use the layer name, but only if this is the only category and the legend text is null
                        text = parent.LegendText;
                    }
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
            if (drawBox) LegendSymbol_Painted(g, new Rectangle(2, 2, (int)x - 4, (int)h));

            b.Dispose();
        }

        /// <summary>
        /// Gets the nearest value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected static double GetNearestValue(double value, List<double> values)
        {
            if (values == null || values.Count == 0)
                return 0;
            int indx = values.BinarySearch(value);
            if (indx >= 0)
            {
                return values[indx];
            }
            int iHigh = -indx;
            if (iHigh >= 0 && iHigh < values.Count)
            {
                double high = values[iHigh];
                int iLow = -indx - 1;
                if (iLow >= 0 && iLow < values.Count && iLow != iHigh)
                {
                    double low = values[iLow];
                    return value - low < high - value ? low : high;
                }
            }
            int iLow2 = -indx - 1;
            if (iLow2 >= 0 && iLow2 < values.Count)
            {
                return values[iLow2];
            }
            return 0;
        }

        /// <summary>
        /// Bytes the range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected static int ByteRange(double value)
        {
            int rounded = (int)Math.Round(value);
            if (rounded > 255)
                return 255;
            if (rounded < 0)
                return 0;
            return rounded;
        }

        /// <summary>
        /// Handles updating event handlers during a copy process
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            LegendItem myCopy = copy as LegendItem;
            if (myCopy != null && myCopy.ItemChanged != null)
            {
                foreach (Delegate handler in myCopy.ItemChanged.GetInvocationList())
                {
                    myCopy.ItemChanged -= (EventHandler)handler;
                }
            }
            if (myCopy != null && myCopy.RemoveItem != null)
            {
                foreach (Delegate handler in myCopy.RemoveItem.GetInvocationList())
                {
                    myCopy.RemoveItem -= (EventHandler)handler;
                }
            }
            base.OnCopy(copy);
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
                    var sw = new Stopwatch();
                    sw.Start();
#endif
                    OnItemChanged();
#if DEBUG
                    sw.Stop();
                    Debug.WriteLine("OnItemChanged time:" + sw.ElapsedMilliseconds);
#endif
                }
            }
            // Prevent forcing extra negatives.
            if (_itemChangedSuspend < 0) _itemChangedSuspend = 0;
        }

        /// <summary>
        /// Each suspend call increments an integer, essentially keeping track of the depth of
        /// suspension.  When the same number of ResumeChangeEvents methods have been called
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

        #endregion

        #region Properties

        /// <summary>
        /// Boolean, true if changes are suspended
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         XmlIgnore]
        public bool ChangesSuspended
        {
            get { return (_itemChangedSuspend > 0); }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not this legend item can be dragged to a new position in the legend.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragable
        {
            get { return _isDragable; }
            set { _isDragable = value; }
        }

        /// <summary>
        /// Gets or sets a boolean, that if false will prevent this item, or any of its child items
        /// from appearing in the legend when the legend is drawn.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool LegendItemVisible
        {
            get { return _legendItemVisible; }
            set { _legendItemVisible = value; }
        }

        /// <summary>
        /// Because these are in symbol mode, this is not used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        /// Gets the MenuItems that should appear in the context menu of the legend for this category
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         XmlIgnore]
        public virtual List<SymbologyMenuItem> ContextMenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not the legend should draw the child LegendItems for this category.
        /// </summary>
        [Serialize("IsExpanded")] 
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        /// Gets or sets whether this legend item has been selected in the legend
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        /// in order to make it easier to cycle through those values.  This defaults to null and must
        /// be overridden in specific cases where child legend items exist.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable<ILegendItem> LegendItems
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the symbol mode for the legend.  By default this should be "Symbol", but this can be overridden
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SymbolMode LegendSymbolMode
        {
            get { return _legendSymbolMode; }
            protected set { _legendSymbolMode = value; }
        }

        /// <summary>
        /// Gets or sets the size of the symbol to be drawn to the legend
        /// </summary>
        public virtual Size GetLegendSymbolSize()
        {
            return _legendSymbolSize;
        }

        /// <summary>
        /// Gets or sets the text for this category to appear in the legend.  This might be a category name,
        /// or a range of values.
        /// </summary>
        [Description("Gets or sets the text for this category to appear in the legend.")]
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
        /// Gets or sets a pre-defined behavior in the legend when referring to drag and drop functionality.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public LegendType LegendType
        {
            get { return _legendType; }
            protected set { _legendType = value; }
        }

        /// <summary>
        /// Gets the Parent Legend item for this category.  This should probably be the appropriate layer item.
        /// </summary>
        public ILegendItem GetParentItem()
        {
            return _parentLegendItem;
        }

        /// <summary>
        /// Sets the parent legend item for this category.
        /// </summary>
        /// <param name="value"></param>
        public void SetParentItem(ILegendItem value)
        {
            OnSetParentItem(value);
        }

        /// <summary>
        /// Allows for the set behavior for the parent item to be overridden in child classes
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnSetParentItem(ILegendItem value)
        {
            _parentLegendItem = value;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// If this is true, then "can receive
        /// </summary>
        [Serialize("IsLegendGroup")]
        protected bool IsLegendGroup
        {
            get { return _isLegendGroup; }
            set { _isLegendGroup = value; }
        }

        /// <summary>
        /// Fires the ItemChanged event
        /// </summary>
        protected virtual void OnItemChanged()
        {
            OnItemChanged(this);
        }

        /// <summary>
        /// Fires the ItemChanged event, optionally specifying a different
        /// sender
        /// </summary>
        protected virtual void OnItemChanged(object sender)
        {
            if (_itemChangedSuspend > 0)
            {
                _changeOccured = true;
                return;
            }
            if (ItemChanged == null) return;
            ItemChanged(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Instructs the parent legend item to remove this item from the list of legend items.
        /// </summary>
        protected virtual void OnRemoveItem()
        {
            if (RemoveItem != null) RemoveItem(this, EventArgs.Empty);
            // Maybe we don't need RemoveItem event.  We could just invoke a method on the parent.
            // One less thing to wire.  But we currently need to wire parents.
        }

        #endregion
    }
}
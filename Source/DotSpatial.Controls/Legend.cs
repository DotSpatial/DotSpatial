// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Symbology;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The legend is used to show a list of all the layers inside of Map. It describes the different categories of each layer with the help of a symbol and the categories legend text.
    /// </summary>
    [ToolboxItem(true)]
    public class Legend : ScrollingControl, ILegend
    {
        #region Fields

        private readonly ContextMenu _contextMenu;
        private readonly TextBox _editBox;
        private readonly Icon _icoChecked;
        private readonly Icon _icoUnchecked;
        private readonly HashSet<ILegendItem> _selection;
        private LegendBox _dragItem;
        private LegendBox _dragTarget;
        private IColorCategory _editCategory;
        private Pen _highlightBorderPen;
        private bool _ignoreHide;
        private bool _isDragging;
        private bool _isMouseDown;
        private List<LegendBox> _legendBoxes; // for hit-testing
        private Rectangle _previousLine;
        private LegendBox _previousMouseDown;
        private Brush _selectionFontBrush;
        private Color _selectionFontColor;
        private Color _selectionHighlight;
        private TabColorDialog _tabColorDialog;
        private bool _wasDoubleClick;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        public Legend()
        {
            RootNodes = new List<ILegendItem>();
            _icoChecked = Images.Checked;
            _icoUnchecked = Images.Unchecked;
            _contextMenu = new ContextMenu();
            _selection = new HashSet<ILegendItem>();
            _editBox = new TextBox
            {
                Parent = this,
                Visible = false
            };
            _editBox.LostFocus += EditBoxLostFocus;
            Indentation = 30;
            _legendBoxes = new List<LegendBox>();

            BackColor = Color.White;
            SelectionFontColor = Color.Black;
            SelectionHighlight = Color.FromArgb(215, 238, 252);

            // Adding a legend ensures that symbology dialogs will be properly launched.
            // Otherwise, an ordinary map may not even need them.
            SharedEventHandlers = new SymbologyEventManager
            {
                Owner = FindForm()
            };
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a mouse up is initiated within a checkbox.
        /// </summary>
        public event EventHandler<ItemMouseEventArgs> CheckBoxMouseUp;

        /// <summary>
        /// Occurs when a mouse down is initiated within an expand box.
        /// </summary>
        public event EventHandler<ItemMouseEventArgs> ExpandBoxMouseDown;

        /// <summary>
        /// Occurs when a mousedown is initiated within an existing legend item
        /// </summary>
        public event EventHandler<ItemMouseEventArgs> ItemMouseDown;

        /// <summary>
        /// Occurs when the mouse is moving over an item.
        /// </summary>
        public event EventHandler<ItemMouseEventArgs> ItemMouseMove;

        /// <summary>
        /// Occurs when a mouse up occurs insize of a specific item.
        /// </summary>
        public event EventHandler<ItemMouseEventArgs> ItemMouseUp;

        /// <summary>
        /// Occurs when the drag method is used to alter the order of layers or
        /// groups in the legend.
        /// </summary>
        public event EventHandler OrderChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an integer representing how far child nodes are indented when compared to the parent nodes.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the indentation in pixels between a parent item and its children.")]
        public int Indentation { get; set; }

        /// <summary>
        /// Gets a height for each item based on the height of the font.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets the height (which depends on the font) for each item.")]
        public int ItemHeight
        {
            get
            {
                if (Font.Height < 20) return 20;
                return Font.Height + 4;
            }
        }

        /// <summary>
        /// Gets or sets the progress handler for any progress messages like re-drawing images for rasters
        /// </summary>
        [Category("Controls")]
        [Description("Gets or sets the progress handler for any progress messages like re-drawing images for rasters")]
        public IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Gets or sets the list of map frames being displayed by this legend.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ILegendItem> RootNodes { get; set; }

        /// <summary>
        /// Gets or sets the selection font color
        /// </summary>
        [Category("Appearance")]
        [Description("Specifies the color of the font in selected legend items.")]
        public Color SelectionFontColor
        {
            get
            {
                return _selectionFontColor;
            }

            set
            {
                _selectionFontColor = value;
                _selectionFontBrush?.Dispose();
                _selectionFontBrush = new SolidBrush(_selectionFontColor);
                IsInitialized = false;
            }
        }

        /// <summary>
        /// Gets or sets the principal color that a selected text box is highlighted with.
        /// </summary>
        [Category("Appearance")]
        [Description("Specifies the color that a selected text box is highlighted with.")]
        public Color SelectionHighlight
        {
            get
            {
                return _selectionHighlight;
            }

            set
            {
                _selectionHighlight = value;
                _highlightBorderPen?.Dispose();
                float med = _selectionHighlight.GetBrightness();
                float border = med - .25f;
                if (border < 0f) border = 0f;
                _highlightBorderPen = new Pen(SymbologyGlobal.ColorFromHsl(_selectionHighlight.GetHue(), _selectionHighlight.GetSaturation(), border));
            }
        }

        /// <summary>
        /// Gets or sets the SharedEventHandler that is used for working with shared layer events.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SymbologyEventManager SharedEventHandlers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend is used to determine whether a layer is selectable.
        /// If true, a layer is only selectable if it or a superior object (parental group, mapframe) is selected in legend.
        /// If false, the selectable state of the layers gets either determined by a plugin like SetSelectable or developers handle the selectable state by code.
        /// By default legend is used, but if the SetSelectable plugin gets loaded this is used instead of the legend.
        /// </summary>
        public bool UseLegendForSelection { get; set; } = true;

        /// <summary>
        /// Gets the bottom box in the legend.
        /// </summary>
        private LegendBox BottomBox => _legendBoxes[_legendBoxes.Count - 1];

        #endregion

        #region Methods

        /// <summary>
        /// Adds a map frame as a root node, and links an event handler to update
        /// when the mapframe triggers an ItemChanged event.
        /// </summary>
        /// <param name="mapFrame">The map frame that gets added.</param>
        public void AddMapFrame(IFrame mapFrame)
        {
            mapFrame.IsSelected = true;
            if (!RootNodes.Contains(mapFrame))
            {
                OnIncludeMapFrame(mapFrame);
            }

            RootNodes.Add(mapFrame);
            RefreshNodes();
        }

        /// <summary>
        /// Un-selects any selected items in the legend.
        /// </summary>
        public void ClearSelection()
        {
            var list = _selection.ToList();
            IFrame parentMap = null;
            if (list.Count > 0)
            {
                parentMap = list[0].ParentMapFrame();
                parentMap?.SuspendEvents();
            }

            foreach (var lb in list)
            {
                lb.IsSelected = false;
            }

            _selection.Clear();
            parentMap?.ResumeEvents();
            RefreshNodes();
        }

        /// <summary>
        /// Given the current list of Maps or 3DMaps, it rebuilds the treeview nodes.
        /// </summary>
        public void RefreshNodes()
        {
            // do any code that needs to happen if content changes
            _previousMouseDown = null; // to avoid memory leaks, because LegendBox contains reference to Layer
            IsInitialized = false;
            Invalidate();
        }

        /// <summary>
        /// Removes the specified map frame if it is a root node.
        /// </summary>
        /// <param name="mapFrame">Map frame that gets removed.</param>
        /// <param name="preventRefresh">Boolean, if true, removing the map frame will not automatically force a refresh of the legend.</param>
        public void RemoveMapFrame(IFrame mapFrame, bool preventRefresh)
        {
            RootNodes.Remove(mapFrame);
            if (!RootNodes.Contains(mapFrame)) OnExcludeMapFrame(mapFrame);
            if (preventRefresh) return;
            RefreshNodes();
        }

        /// <summary>
        /// Overrides the drawing method to account for drawing lines when an item is being dragged to a new position.
        /// </summary>
        /// <param name="e">A PaintEventArgs</param>
        protected override void OnDraw(PaintEventArgs e)
        {
            base.OnDraw(e);

            using (var pen = new Pen(BackColor))
            {
                e.Graphics.DrawRectangle(pen, e.ClipRectangle);
            }

            if (_isDragging && !_previousLine.IsEmpty)
            {
                e.Graphics.DrawLine(Pens.Black, _previousLine.X, _previousLine.Y + 2, _previousLine.Right, _previousLine.Y + 2);
            }
        }

        /// <summary>
        /// Occurs when we need to no longer listen to the map frame events.
        /// </summary>
        /// <param name="mapFrame">Map frame that gets excluded.</param>
        protected virtual void OnExcludeMapFrame(IFrame mapFrame)
        {
            mapFrame.ItemChanged -= MapFrameItemChanged;
            mapFrame.LayerSelected -= LayersLayerSelected;
            mapFrame.LayerRemoved -= MapFrameOnLayerRemoved;
        }

        /// <summary>
        /// Also hides the edit box so that it doesn't seem displaced from the item
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected override void OnHorizontalScroll(object sender, ScrollEventArgs e)
        {
            HideEditBox();
            base.OnHorizontalScroll(sender, e);
        }

        /// <summary>
        /// Occurs when linking the map frame.
        /// </summary>
        /// <param name="mapFrame">Map frame that gets included.</param>
        protected virtual void OnIncludeMapFrame(IFrame mapFrame)
        {
            mapFrame.ItemChanged += MapFrameItemChanged;
            mapFrame.LayerSelected += LayersLayerSelected;
            mapFrame.LayerRemoved += MapFrameOnLayerRemoved;
        }

        /// <summary>
        /// Extends initialize to draw "non-selected" elements.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnInitialize(PaintEventArgs e)
        {
            // draw standard background image to buffer
            base.OnInitialize(e);
            PointF topLeft = new Point(0, 0);
            if (RootNodes == null || RootNodes.Count == 0) return;
            _legendBoxes = new List<LegendBox>();
            foreach (var item in RootNodes)
            {
                var args = new DrawLegendItemArgs(e.Graphics, item, ClientRectangle, topLeft);
                OnInitializeItem(args);
                topLeft.Y += SizeItem((int)topLeft.X, item, e.Graphics).Height;
            }
        }

        /// <summary>
        /// Draws the legend item from the DrawLegendItemArgs with all its child items.
        /// </summary>
        /// <param name="e">DrawLegendItemArgs that are needed to draw the legend item.</param>
        /// <returns>The position where the next LegendItem can be drawn.</returns>
        protected virtual PointF OnInitializeItem(DrawLegendItemArgs e)
        {
            if (!e.Item.LegendItemVisible) return e.TopLeft;

            UpdateActions(e.Item);

            PointF topLeft = e.TopLeft;
            PointF tempTopLeft = topLeft;
            if (topLeft.Y > ControlRectangle.Bottom)
            {
                return topLeft; // drawing would be below the screen
            }

            if (topLeft.Y > ControlRectangle.Top - ItemHeight)
            {
                // Draw the item itself
                var itemBox = new LegendBox
                {
                    Item = e.Item,
                    Bounds = new Rectangle(0, (int)topLeft.Y, Width, ItemHeight),
                    Indent = (int)topLeft.X / Indentation
                };
                _legendBoxes.Add(itemBox);

                DrawPlusMinus(e.Graphics, ref tempTopLeft, itemBox);

                int ih = ItemHeight;

                if (e.Item.LegendSymbolMode == SymbolMode.Symbol)
                {
                    Size s = e.Item.GetLegendSymbolSize();
                    if (s.Height > ih) tempTopLeft.Y += 3;
                }

                if (e.Item.LegendSymbolMode == SymbolMode.Symbol || e.Item.LegendSymbolMode == SymbolMode.GroupSymbol)
                {
                    DrawSymbol(e.Graphics, ref tempTopLeft, itemBox);
                }

                if (e.Item.LegendSymbolMode == SymbolMode.Checkbox)
                {
                    DrawCheckBoxes(e.Graphics, ref tempTopLeft, itemBox);
                }

                int width = (int)e.Graphics.MeasureString(e.Item.LegendText, Font).Width;
                int dY = 0;
                if (e.Item.LegendSymbolMode == SymbolMode.Symbol)
                {
                    Size s = e.Item.GetLegendSymbolSize();
                    if (s.Height > ih) dY = (s.Height - ih) / 2;
                    tempTopLeft.Y += dY;
                }

                tempTopLeft.Y += (ih - Font.Height) / 2F;

                itemBox.Textbox = new Rectangle((int)tempTopLeft.X, (int)topLeft.Y + dY, width, ItemHeight);
                if (itemBox.Item.IsSelected)
                {
                    _selection.Add(itemBox.Item);
                    Rectangle innerBox = itemBox.Textbox;
                    innerBox.Inflate(-1, -1);
                    using (var b = HighlightBrush(innerBox))
                    {
                        e.Graphics.FillRectangle(b, innerBox);
                    }

                    SymbologyGlobal.DrawRoundedRectangle(e.Graphics, _highlightBorderPen, itemBox.Textbox);
                    e.Graphics.DrawString(e.Item.LegendText, Font, _selectionFontBrush, tempTopLeft);
                }
                else
                {
                    e.Graphics.DrawString(e.Item.LegendText, Font, Brushes.Black, tempTopLeft);
                }
            }

            int h = ItemHeight;
            if (e.Item.LegendSymbolMode == SymbolMode.Symbol)
            {
                Size s = e.Item.GetLegendSymbolSize();
                if (s.Height > h) h = s.Height + 6;
            }

            topLeft.Y += h;

            if (e.Item.IsExpanded)
            {
                topLeft.X += Indentation;
                if (e.Item.LegendItems != null)
                {
                    List<ILegendItem> items = e.Item.LegendItems.ToList();
                    if (e.Item is IGroup) items.Reverse(); // reverse layers because of drawing order, don't bother reversing categories.
                    foreach (var item in items)
                    {
                        var args = new DrawLegendItemArgs(e.Graphics, item, e.ClipRectangle, topLeft);
                        topLeft = OnInitializeItem(args);
                        if (topLeft.Y > ControlRectangle.Bottom) break;
                    }
                }

                topLeft.X -= Indentation;
            }

            return topLeft;
        }

        /// <summary>
        /// The coordinates are in legend coordinates, but a LegendBox is provided to define the
        /// coordinates of the specified object.
        /// </summary>
        /// <param name="e">An ItemMouseEventArgs</param>
        protected virtual void OnItemMouseDown(ItemMouseEventArgs e)
        {
            ItemMouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// Fires the ItemMouseMove Event, which handles the mouse moving over one of the legend items.
        /// </summary>
        /// <param name="e">An ItemMouseEventArgs</param>
        protected virtual void OnItemMouseMove(ItemMouseEventArgs e)
        {
            ItemMouseMove?.Invoke(this, e);
        }

        /// <summary>
        /// Checks for checkbox changes and fires the ItemMouseUp event.
        /// </summary>
        /// <param name="e">The event  args.</param>
        protected virtual void OnItemMouseUp(ItemMouseEventArgs e)
        {
            ItemMouseUp?.Invoke(this, e);
        }

        /// <inheritdoc />
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            _wasDoubleClick = true;
            Point loc = new Point(e.X + ControlRectangle.X, e.Location.Y + ControlRectangle.Top);
            foreach (LegendBox lb in _legendBoxes)
            {
                if (!lb.Bounds.Contains(loc) || lb.CheckBox.Contains(loc)) continue;
                ILineCategory lc = lb.Item as ILineCategory;
                if (lc != null)
                {
                    using (var lsDialog = new DetailedLineSymbolDialog(lc.Symbolizer))
                    {
                        lsDialog.ShowDialog();
                    }
                }

                IPointCategory pc = lb.Item as IPointCategory;
                if (pc != null)
                {
                    using (var dlg = new DetailedPointSymbolDialog(pc.Symbolizer))
                    {
                        dlg.ShowDialog();
                    }
                }

                IPolygonCategory polyCat = lb.Item as IPolygonCategory;
                if (polyCat != null)
                {
                    using (var dlg = new DetailedPolygonSymbolDialog(polyCat.Symbolizer))
                    {
                        dlg.ShowDialog();
                    }
                }

                IFeatureLayer fl = lb.Item as IFeatureLayer;
                if (fl != null)
                {
                    using (var layDialog = new LayerDialog(fl, new FeatureCategoryControl()))
                    {
                        layDialog.ShowDialog();
                    }
                }

                IRasterLayer rl = lb.Item as IRasterLayer;
                if (rl != null)
                {
                    using (var dlg = new LayerDialog(rl, new RasterCategoryControl()))
                    {
                        dlg.ShowDialog();
                    }
                }

                IColorCategory cb = lb.Item as IColorCategory;
                if (cb != null)
                {
                    _tabColorDialog = new TabColorDialog();
                    _tabColorDialog.ChangesApplied += TabColorDialogChangesApplied;
                    _tabColorDialog.StartColor = cb.LowColor;
                    _tabColorDialog.EndColor = cb.HighColor;
                    _editCategory = cb;
                    _tabColorDialog.ShowDialog(this);
                }
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Handles the case where the mouse down occurs.
        /// </summary>
        /// <param name="e">A MouseEventArgs</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            HideEditBox();
            if (_legendBoxes == null || _legendBoxes.Count == 0) return;
            Point loc = new Point(e.X + ControlRectangle.X, e.Location.Y + ControlRectangle.Top);
            foreach (LegendBox box in _legendBoxes)
            {
                if (box.Bounds.Contains(loc))
                {
                    ItemMouseEventArgs args = new ItemMouseEventArgs(e, box);
                    DoItemMouseDown(args);
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Performs the default handling for mouse movememnt, and decides
        /// whether or not to fire an ItemMouseMove event.
        /// </summary>
        /// <param name="e">A MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_legendBoxes == null) return;
            bool cursorHandled = false;
            LegendBox currentBox = null;
            Point loc = new Point(e.X + ControlRectangle.X, e.Location.Y + ControlRectangle.Top);
            foreach (LegendBox box in _legendBoxes)
            {
                if (box.Bounds.Contains(loc))
                {
                    currentBox = box;
                    ItemMouseEventArgs args = new ItemMouseEventArgs(e, box);
                    DoItemMouseMove(args);
                }
            }

            if (_isMouseDown) _isDragging = true;

            if (_isDragging)
            {
                _dragTarget = currentBox;
                if (_dragTarget == null && ClientRectangle.Contains(e.Location))
                {
                    _dragTarget = BottomBox;
                }

                if (!_previousLine.IsEmpty) Invalidate(_previousLine);
                _previousLine = Rectangle.Empty;

                if (_dragTarget != null && _dragItem != null)
                {
                    int left = 0;
                    LegendBox container = BoxFromItem(_dragTarget != _dragItem ? _dragTarget.Item.GetValidContainerFor(_dragItem.Item) : _dragItem.Item);

                    bool indenting = false;

                    if (container != null)
                    {
                        // if the mouse x is smaller than the right corner of the expand box, the user is trying to move the layer out of the current group into the parent group
                        // so we indent the line based on the parent group
                        if (loc.X < container.ExpandBox.Right)
                        {
                            var c = BoxFromItem(container.Item.GetParentItem());
                            if (c != null) container = c;
                            indenting = true;
                        }

                        left = (container.Indent + 1) * Indentation;
                    }

                    LegendBox boxOverLine;
                    if (_dragTarget.Item.CanReceiveItem(_dragItem.Item))
                    {
                        boxOverLine = _dragTarget;
                    }
                    else if (_dragTarget.Item.LegendType == LegendType.Layer && _dragItem.Item.LegendType == LegendType.Layer)
                    {
                        boxOverLine = BoxFromItem(_dragTarget.Item.BottomMember());
                    }
                    else
                    {
                        boxOverLine = BoxFromItem(_dragTarget.Item.GetParentItem().BottomMember());
                    }

                    if (boxOverLine == null || boxOverLine.Item.IsChildOf(_dragItem.Item) || (boxOverLine.Item == _dragItem.Item && !indenting))
                    {
                        // items may not be moved onto themselves, so we show the forbidden cursor
                        _dragTarget = null;
                        Cursor = Cursors.No;
                        cursorHandled = true;
                    }
                    else
                    {
                        // draw the line on the position the layer would be moved to
                        _dragTarget = boxOverLine;
                        _previousLine = new Rectangle(left, boxOverLine.Bounds.Bottom, Width - left, 4);
                        Cursor = Cursors.Hand;
                        cursorHandled = true;
                        Invalidate(_previousLine);
                    }
                }
                else
                {
                    _isMouseDown = false;
                }

                if (!cursorHandled)
                {
                    Cursor = Cursors.No;
                    cursorHandled = true;
                }
            }

            if (!cursorHandled)
            {
                Cursor = Cursors.Arrow;
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Checks the Mouse Up event to see if it occurs inside a legend item.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point loc = new Point(e.X + ControlRectangle.X, e.Location.Y + ControlRectangle.Top);

            // if it was neither dragged nor double clicked, the item will be renamed
            if (!_wasDoubleClick && !_isDragging)
            {
                foreach (LegendBox box in _legendBoxes)
                {
                    if (!box.Bounds.Contains(loc)) continue;
                    ItemMouseEventArgs args = new ItemMouseEventArgs(e, box);
                    DoItemMouseUp(args);
                }
            }

            // if the item was dragged it has to be moved to the new position
            if (_isDragging)
            {
                if (_dragItem != null && _dragTarget != null)
                {
                    ILegendItem potentialParent = null;
                    if (_dragTarget.Item != _dragItem.Item)
                    {
                        potentialParent = _dragTarget.Item.GetValidContainerFor(_dragItem.Item);
                    }

                    if (potentialParent != null)
                    {
                        // update the parent, if the user is trying to move the item up the tree
                        var container = BoxFromItem(potentialParent);
                        if (loc.X < container.ExpandBox.Right)
                        {
                            potentialParent = container.Item.GetParentItem();
                        }

                        // The target must be a group, and the item must be a layer.
                        ILayer lyr = _dragItem.Item as ILayer;
                        IGroup grp = potentialParent as IGroup;
                        if (lyr != null && grp != null)
                        {
                            // when original location is inside group, remove layer from the group.
                            IGroup grp1 = _dragItem.Item.GetParentItem() as IGroup;

                            int newIndex = potentialParent.InsertIndex(_dragTarget.Item) - 1;
                            var oldIndex = grp1?.IndexOf(lyr);

                            // only move the layer if the user doesn't move it to the same position as before
                            if (grp1 != grp || oldIndex != newIndex)
                            {
                                potentialParent.ParentMapFrame().SuspendEvents();
                                lyr.LockDispose();
                                grp1?.Remove(lyr);

                                var index = potentialParent.InsertIndex(_dragTarget.Item);

                                if (index == -1) index = 0;
                                grp.Insert(index, lyr);

                                // when the target is a group, assign the parent item.
                                lyr.SetParentItem(grp);
                                lyr.UnlockDispose();

                                potentialParent.ParentMapFrame().ResumeEvents();
                                OnOrderChanged();
                                potentialParent.ParentMapFrame().Invalidate();
                            }
                        }
                    }
                }

                Cursor = Cursors.Arrow;
                _isDragging = false;
                Invalidate();
            }

            _isMouseDown = false;
            _wasDoubleClick = false;

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the OrderChanged Event.
        /// </summary>
        protected virtual void OnOrderChanged()
        {
            OrderChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Also hides the edit box so that it doesn't seme displaced from the item.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected override void OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            HideEditBox();
            base.OnVerticalScroll(sender, e);
        }

        /// <summary>
        /// Recursive add method to handle nesting of menu items.
        /// </summary>
        /// <param name="parent">The parent</param>
        /// <param name="mi">The menu item.</param>
        private static void AddMenuItem(Menu.MenuItemCollection parent, SymbologyMenuItem mi)
        {
            MenuItem m;
            if (mi.Icon != null)
            {
                m = new IconMenuItem(mi.Name, mi.Icon, mi.ClickHandler);
            }
            else if (mi.Image != null)
            {
                m = new IconMenuItem(mi.Name, mi.Image, mi.ClickHandler);
            }
            else
            {
                m = new IconMenuItem(mi.Name, mi.ClickHandler);
            }

            parent.Add(m);
            foreach (SymbologyMenuItem child in mi.MenuItems)
            {
                AddMenuItem(m.MenuItems, child);
            }
        }

        /// <summary>
        /// Sets SelectionEnabled true for all the children of the given collection.
        /// </summary>
        /// <param name="layers">Collection whose items SelectionEnabled state should be set to true.</param>
        private static void SetSelectionEnabledForChildren(IMapLayerCollection layers)
        {
            foreach (var mapLayer in layers)
            {
                mapLayer.SelectionEnabled = true;
                mapLayer.IsSelected = false; // the selection comes from higher up, so the layers gets deselected, so the user doesn't assume that only the selected items in the group are used for selection

                var gr = mapLayer as IMapGroup;
                if (gr != null)
                {
                    SetSelectionEnabledForChildren(gr.Layers);
                }
                else
                {
                    IFeatureLayer fl = mapLayer as IFeatureLayer;

                    if (fl != null)
                    {
                        // all categories get selection enabled
                        foreach (var cat in fl.Symbology.GetCategories())
                        {
                            cat.SelectionEnabled = true;
                            cat.IsSelected = false; // the selection comes from higher up, so the category gets deselected, so the user doesn't assume that only the selected categories of the layer get used for selection
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the SelectionEnabled property of all the layers in the given collection.
        /// </summary>
        /// <param name="layers">The collection that gets updated.</param>
        private static void UpdateSelectionEnabled(IEnumerable<object> layers)
        {
            foreach (var mapLayer in layers.OfType<ILayer>())
            {
                mapLayer.SelectionEnabled = mapLayer.IsSelected; // enable selection if the current item is selected

                var gr = mapLayer as IMapGroup;
                if (gr != null)
                {
                    // this is a group
                    if (gr.IsSelected)
                    {
                        SetSelectionEnabledForChildren(gr.Layers); // group is the selected item, so everything below is selected
                    }
                    else
                    {
                        UpdateSelectionEnabled(gr.Layers); // group is not the selected item, so find the selected item
                    }
                }
                else
                {
                    IFeatureLayer fl = mapLayer as IFeatureLayer;

                    if (fl != null)
                    {
                        var cats = fl.Symbology.GetCategories().ToList();

                        // this is a layer with categories
                        if (mapLayer.IsSelected)
                        {
                            // all categories get selection enabled
                            foreach (var cat in cats)
                            {
                                cat.SelectionEnabled = true;
                                cat.IsSelected = false; // layer is selected, so the category gets deselected, so the user doesn't assume that only the selected categories of the layer get used for selection
                            }
                        }
                        else
                        {
                            // only selected categories get selection enabled
                            foreach (var cat in cats)
                            {
                                cat.SelectionEnabled = cat.IsSelected;
                            }

                            mapLayer.SelectionEnabled = cats.Any(_ => _.SelectionEnabled);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Given a legend item, it searches the list of LegendBoxes until it finds it.
        /// </summary>
        /// <param name="item">LegendItem to find.</param>
        /// <returns>LegendBox belonging to the item.</returns>
        private LegendBox BoxFromItem(ILegendItem item)
        {
            return _legendBoxes.FirstOrDefault(box => box.Item == item);
        }

        private void DoItemMouseDown(ItemMouseEventArgs e)
        {
            Point loc = new Point(e.X + ControlRectangle.X, e.Location.Y + ControlRectangle.Top);

            // Toggle expansion
            if (e.ItemBox.ExpandBox.Contains(loc))
            {
                e.ItemBox.Item.IsExpanded = !e.ItemBox.Item.IsExpanded;
                ExpandBoxMouseDown?.Invoke(this, e);

                ResetLegend();
                return;
            }

            // if the item was already selected
            if (e.ItemBox.Item.IsSelected)
            {
                // and the left mouse button is clicked while the shift key is held, we deselect the item
                if (ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
                {
                    e.ItemBox.Item.IsSelected = false;
                    return;
                }

                // Start dragging
                if (e.Button == MouseButtons.Left)
                {
                    var box = e.ItemBox;

                    // otherwise we prepare to edit in textbox
                    if (_previousMouseDown == null)
                    {
                        ClearSelection();

                        box = _legendBoxes.Find(_ => _.Item == e.ItemBox.Item);
                        box.Item.IsSelected = true;
                        _previousMouseDown = box;
                    }

                    _isMouseDown = true;
                    ILegendItem li = box.Item;
                    ILayer lyr = li as ILayer;
                    if (lyr == null)
                    {
                        // this is a category, it may be renamed but not moved
                    }
                    else if (RootNodes.Contains(lyr))
                    {
                        // this is a root node, it may not be renamed or moved
                        _isMouseDown = false;
                    }
                    else
                    {
                        // this is a layer, it may be moved and renamed
                        _dragItem = BoxFromItem(lyr);
                    }
                }
            }
            else
            {
                // Check for textbox clicking
                if (e.ItemBox.Textbox.Contains(loc))
                {
                    if (ModifierKeys != Keys.Control)
                    {
                        ClearSelection();
                    }

                    e.ItemBox.Item.IsSelected = true;

                    if (UseLegendForSelection)
                    {
                        UpdateSelectionEnabled(RootNodes.OfType<IMapFrame>().AsEnumerable());
                    }
                }
            }
        }

        private void DoItemMouseMove(ItemMouseEventArgs e)
        {
            OnItemMouseMove(e);
        }

        private void DoItemMouseUp(ItemMouseEventArgs e)
        {
            Point loc = new Point(e.X + ControlRectangle.X, e.Location.Y + ControlRectangle.Top);
            if (e.Button == MouseButtons.Left)
            {
                if (e.ItemBox.Item.LegendSymbolMode == SymbolMode.Checkbox && e.ItemBox.CheckBox.Contains(loc))
                {
                    IRenderableLegendItem rendItem = e.ItemBox.Item as IRenderableLegendItem;
                    if (rendItem != null)
                    {
                        // force a re-draw in the case where we are talking about layers.
                        rendItem.IsVisible = !rendItem.IsVisible;
                    }
                    else
                    {
                        e.ItemBox.Item.Checked = !e.ItemBox.Item.Checked;
                    }

                    CheckBoxMouseUp?.Invoke(this, e);
                    RefreshNodes();
                }

                // left click on a text box makes the textbox editable, if it was clicked before
                if (e.ItemBox.Textbox.Contains(loc))
                {
                    if (e.ItemBox.Item == _previousMouseDown?.Item)
                    {
                        if (!e.ItemBox.Item.LegendTextReadOnly)
                        {
                            // Edit via text box
                            _editBox.Left = e.ItemBox.Textbox.Left;
                            _editBox.Width = e.ItemBox.Textbox.Width + 10;
                            _editBox.Top = e.ItemBox.Bounds.Top;
                            _editBox.Height = e.ItemBox.Bounds.Height;
                            _editBox.SelectedText = e.ItemBox.Item.LegendText;
                            _editBox.Font = Font;
                            _editBox.Visible = true;
                        }
                    }
                    else if (ModifierKeys == Keys.Control)
                    {
                        RefreshNodes();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // right click shows the context menu
                if (e.ItemBox.Item.ContextMenuItems == null) return;

                _contextMenu.MenuItems.Clear();
                foreach (SymbologyMenuItem mi in e.ItemBox.Item.ContextMenuItems)
                {
                    AddMenuItem(_contextMenu.MenuItems, mi);
                }

                _contextMenu.Show(this, e.Location);
                _contextMenu.MenuItems.Clear();
            }
        }

        /// <summary>
        /// If the LegendBox contains a checkbox item draw the checkbox for an item.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="topLeft">TopLeft position where the symbol should be drawn.</param>
        /// <param name="itemBox">LegendBox of the item, the checkbox should be added to.</param>
        private void DrawCheckBoxes(Graphics g, ref PointF topLeft, LegendBox itemBox)
        {
            ILegendItem item = itemBox.Item;
            if (item?.LegendSymbolMode != SymbolMode.Checkbox) return;

            if (item.Checked)
            {
                int top = (int)topLeft.Y + ((ItemHeight - _icoChecked.Height) / 2);
                int left = (int)topLeft.X + 6;
                g.DrawIcon(_icoChecked, left, top);
                Rectangle box = new Rectangle(left, top, _icoChecked.Width, _icoChecked.Height);
                itemBox.CheckBox = box;
            }
            else
            {
                int top = (int)topLeft.Y + ((ItemHeight - _icoUnchecked.Height) / 2);
                int left = (int)topLeft.X + 6;
                g.DrawIcon(_icoUnchecked, left, top);
                Rectangle box = new Rectangle(left, top, _icoChecked.Width, _icoChecked.Height);
                itemBox.CheckBox = box;
            }

            topLeft.X += 22;
        }

        /// <summary>
        /// If the LegendBox doesn't contain a symbol draw the plus or minus visible for controlling expansion.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="topLeft">TopLeft position where the symbol should be drawn.</param>
        /// <param name="itemBox">LegendBox of the item, the +/- should be added to.</param>
        private void DrawPlusMinus(Graphics g, ref PointF topLeft, LegendBox itemBox)
        {
            ILegendItem item = itemBox.Item;
            if (item == null) return;
            if (item.LegendSymbolMode == SymbolMode.Symbol) return; // don't allow symbols to expand
            Point tl = new Point((int)topLeft.X, (int)topLeft.Y);
            tl.Y += (ItemHeight - 8) / 2;
            tl.X += 3;
            Rectangle box = new Rectangle(tl.X, tl.Y, 8, 8);
            itemBox.ExpandBox = box;
            Point center = new Point(tl.X + 4, (int)topLeft.Y + (ItemHeight / 2));
            g.FillRectangle(Brushes.White, box);
            g.DrawRectangle(Pens.Gray, box);
            if (item.IsExpanded)
            {
                g.DrawRectangle(Pens.Gray, box);
            }
            else if (item.LegendItems != null && item.LegendItems.Any())
            {
                g.DrawLine(Pens.Black, center.X, center.Y - 2, center.X, center.Y + 2);
            }

            g.DrawLine(Pens.Black, center.X - 2, center.Y, center.X + 2, center.Y);
            topLeft.X += 13;
        }

        /// <summary>
        /// Draw the symbol for a particular item.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="topLeft">TopLeft position where the symbol should be drawn.</param>
        /// <param name="itemBox">LegendBox of the item, the symbol should be added to.</param>
        private void DrawSymbol(Graphics g, ref PointF topLeft, LegendBox itemBox)
        {
            ILegendItem item = itemBox.Item;

            // Align symbols so that their right side is about 20 pixels left
            // of the top-left X, but allow up to 128x128 sized symbols
            Size s = item.GetLegendSymbolSize();
            int h = s.Height;
            if (h < 1) h = 1;
            if (h > 128) h = 128;
            int w = s.Width;
            if (w < 1) w = 1;
            if (w > 128) w = 128;

            int tH = ItemHeight;
            int x = (int)topLeft.X + tH - w;
            int y = (int)topLeft.Y;
            if (tH > h) y += (tH - h) / 2;
            Rectangle box = new Rectangle(x, y, w, h);
            itemBox.SymbolBox = box;
            item.LegendSymbolPainted(g, box);
            topLeft.X += tH + 6;
        }

        private void EditBoxLostFocus(object sender, EventArgs e)
        {
            HideEditBox();
        }

        private void HideEditBox()
        {
            if (_editBox.Visible && !_ignoreHide)
            {
                _ignoreHide = true;
                _previousMouseDown.Item.LegendText = _editBox.Text;
                _previousMouseDown = null;
                _editBox.Visible = false;
                _editBox.Text = string.Empty;
                _ignoreHide = false;
                RefreshNodes();
            }
        }

        // a good selectionHighlight color: 215, 238, 252
        private Brush HighlightBrush(Rectangle box)
        {
            float med = _selectionHighlight.GetBrightness();
            float bright = med + 0.05f;
            if (bright > 1f) bright = 1f;
            float dark = med - 0.05f;
            if (dark < 0f) dark = 0f;
            Color brtCol = SymbologyGlobal.ColorFromHsl(_selectionHighlight.GetHue(), _selectionHighlight.GetSaturation(), bright);
            Color drkCol = SymbologyGlobal.ColorFromHsl(_selectionHighlight.GetHue(), _selectionHighlight.GetSaturation(), dark);
            return new LinearGradientBrush(box, brtCol, drkCol, LinearGradientMode.Vertical);
        }

        private void LayersLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            if (e.IsSelected)
            {
                _selection.Add(e.Layer);
            }
            else
            {
                _selection.Remove(e.Layer);
            }
        }

        /// <summary>
        ///  This isn't the best way to catch this. Only items in view should trigger a refresh.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void MapFrameItemChanged(object sender, EventArgs e)
        {
            ResetLegend();
        }

        private void MapFrameOnLayerRemoved(object sender, LayerEventArgs e)
        {
            _selection.Remove(e.Layer);
        }

        private void ResetLegend()
        {
            SizePage();
            ResetScroll();
            RefreshNodes();
        }

        private Size SizeItem(int offset, ILegendItem item, Graphics g)
        {
            if (item == null) return new Size(0, 0);
            int width = offset + 30 + (int)g.MeasureString(item.LegendText, Font).Width;
            int height = ItemHeight;
            if (item.LegendSymbolMode == SymbolMode.Symbol)
            {
                Size s = item.GetLegendSymbolSize();
                if (s.Height > ItemHeight) height = s.Height;
            }

            if (item.IsExpanded)
            {
                if (item.LegendItems != null)
                {
                    foreach (ILegendItem child in item.LegendItems)
                    {
                        Size cs = SizeItem(offset + Indentation, child, g);
                        height += cs.Height;
                        if (cs.Width > width) width = cs.Width;
                    }
                }
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Checks all the legend items and calculates a "page" large enough to contain everything currently visible.
        /// </summary>
        private void SizePage()
        {
            int w = Width;
            int totalHeight = 0;
            using (var g = CreateGraphics())
            {
                foreach (var li in RootNodes)
                {
                    var itemSize = SizeItem(0, li, g);
                    totalHeight += itemSize.Height;
                    if (itemSize.Width > w) w = itemSize.Width;
                }
            }

            int h = totalHeight;
            DocumentRectangle = new Rectangle(0, 0, w, h);
        }

        private void TabColorDialogChangesApplied(object sender, EventArgs e)
        {
            _editCategory.LowColor = _tabColorDialog.StartColor;
            _editCategory.HighColor = _tabColorDialog.EndColor;
            ILegendItem test = _editCategory.GetParentItem();
            IRasterLayer rl = test as IRasterLayer;
            rl?.WriteBitmap();
        }

        private void UpdateActions(ILegendItem mapLayer)
        {
            var manager = SharedEventHandlers;

            var layer = mapLayer as Layer;
            if (layer != null)
            {
                layer.LayerActions = manager?.LayerActions;
            }

            var cc = mapLayer as ColorCategory;
            if (cc != null)
            {
                cc.ColorCategoryActions = manager?.ColorCategoryActions;
            }

            var fl = mapLayer as FeatureLayer;
            if (fl != null)
            {
                fl.FeatureLayerActions = manager?.FeatureLayerActions;
            }

            var il = mapLayer as ImageLayer;
            if (il != null)
            {
                il.ImageLayerActions = manager?.ImageLayerActions;
            }

            var rl = mapLayer as RasterLayer;
            if (rl != null)
            {
                rl.RasterLayerActions = manager?.RasterLayerActions;
            }
        }

        #endregion
    }
}
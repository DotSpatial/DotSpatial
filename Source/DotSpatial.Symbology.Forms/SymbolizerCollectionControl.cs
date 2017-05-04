// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/20/2009 4:23:05 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This control displays a list box where each item is a preview of a category symbolizer
    /// </summary>
    public partial class SymbolizerCollectionControl : UserControl
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IList<IFeatureSymbolizer> _symbolizers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolizerCollectionControl"/> class.
        /// </summary>
        public SymbolizerCollectionControl()
        {
            InitializeComponent();
            _symbolizers = new List<IFeatureSymbolizer>();
            lbxItems.DrawItem += LbxItemsDrawItem;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when someone clicks the add button.
        /// </summary>
        public event EventHandler AddClicked;

        /// <summary>
        /// Occurs when the list has been added, removed, or re-ordered in any way.
        /// </summary>
        public event EventHandler ListChanged;

        /// <summary>
        /// Occurs when either the Promote or Demote function has been used,
        /// changing the order.
        /// </summary>
        public event EventHandler OrderChanged;

        /// <summary>
        /// Occurs when the remove button has been clicked
        /// </summary>
        public event EventHandler RemoveClicked;

        /// <summary>
        /// Occurs when someone selects one of the items in the list box
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the item height for this control.
        /// </summary>
        public int ItemHeight
        {
            get
            {
                return lbxItems.ItemHeight;
            }

            set
            {
                lbxItems.ItemHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale mode.  If the scale mode is set to geographic, then the
        /// specified size will be ignored when drawing the symbolic representation.
        /// </summary>
        public ScaleMode ScaleMode { get; set; }

        /// <summary>
        /// Gets or sets the selected item cast as an object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFeatureSymbolizer SelectedSymbol
        {
            get
            {
                return lbxItems?.SelectedItem as IFeatureSymbolizer;
            }

            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the core list of syms that will be drawn here.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<IFeatureSymbolizer> Symbols
        {
            get
            {
                return _symbolizers;
            }

            set
            {
                _symbolizers = value;
                RefreshList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
        /// </summary>
        public void RefreshList()
        {
            lbxItems.SuspendLayout();
            lbxItems.Items.Clear();
            foreach (IFeatureSymbolizer sym in _symbolizers)
            {
                lbxItems.Items.Insert(0, sym);
            }

            lbxItems.ResumeLayout();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposingManagedResources">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            {
                components?.Dispose();
                if (!lbxItems.IsDisposed) lbxItems.Dispose();
                if (!panel1.IsDisposed) panel1.Dispose();
                if (!btnAdd.IsDisposed) btnAdd.Dispose();
                if (!btnDown.IsDisposed) btnDown.Dispose();
                if (!btnUp.IsDisposed) btnUp.Dispose();
                if (!btnRemove.IsDisposed) btnRemove.Dispose();
                lbxItems = null;
                panel1 = null;
                btnAdd = null;
                btnDown = null;
                btnUp = null;
                btnRemove = null;
                _symbolizers = null;
            }

            base.Dispose(disposingManagedResources);
        }

        /// <summary>
        /// Fires the AddClicked event
        /// </summary>
        protected virtual void OnAdd()
        {
            AddClicked?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the ListChanged event
        /// </summary>
        protected virtual void OnListChanged()
        {
            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the OnOrderChanged event
        /// </summary>
        protected virtual void OnOrderChanged()
        {
            OrderChanged?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the RemoveCLicked event
        /// </summary>
        protected virtual void OnRemoveClick()
        {
            RemoveClicked?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the SelectedItemChanged event
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Draws a preview of a line symbolizer inside of the specified rectangle.
        /// </summary>
        /// <param name="sym">The line symbolizer.</param>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="rect">The rectangle.</param>
        private static void DrawLineSymbolizer(LineSymbolizer sym, Graphics g, Rectangle rect)
        {
            if (sym != null)
            {
                g.FillRectangle(Brushes.White, rect);
                GraphicsPath gp = new GraphicsPath();
                gp.AddLine(10, rect.Height / 2, rect.Width - 20, rect.Height / 2);
                foreach (IStroke stroke in sym.Strokes)
                {
                    stroke.DrawPath(g, gp, 1);
                }

                gp.Dispose();
            }
        }

        /// <summary>
        /// Draws a preview of a point symbolizer inside of the specified rectangle
        /// </summary>
        /// <param name="sym">The point symbolizer.</param>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="rect">The rectangle.</param>
        private static void DrawPointSymbolizer(PointSymbolizer sym, Graphics g, Rectangle rect)
        {
            if (sym != null)
            {
                g.Clear(Color.White);
                Matrix shift = g.Transform;
                shift.Translate(rect.Width / 2F, rect.Height / 2F);
                g.Transform = shift;
                double scale = 1;
                if (sym.ScaleMode == ScaleMode.Geographic || sym.GetSize().Height > (rect.Height - 6))
                {
                    scale = (rect.Height - 6) / sym.GetSize().Height;
                }

                sym.Draw(g, scale);
            }
        }

        /// <summary>
        /// Draws a preview of a polygon symbolizer inside of the specified rectangle
        /// </summary>
        /// <param name="sym">The polygon symbolizer.</param>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="rect">The rectangle.</param>
        private static void DrawPolygonSymbolizer(PolygonSymbolizer sym, Graphics g, Rectangle rect)
        {
            if (sym != null)
            {
                g.Clear(Color.White);
                Rectangle rect2 = new Rectangle(5, 5, rect.Width - 10, rect.Height - 10);
                sym.Draw(g, rect2);
            }
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            OnAdd();
            RefreshList();
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            IFeatureSymbolizer sym = lbxItems.SelectedItem as IFeatureSymbolizer;
            if (sym == null) return;
            _symbolizers.DecreaseIndex(sym);
            RefreshList();
            lbxItems.SelectedItem = sym;
            OnOrderChanged();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            IFeatureSymbolizer sym = lbxItems.SelectedItem as IFeatureSymbolizer;
            int index = _symbolizers.IndexOf(sym);
            _symbolizers.Remove(sym);
            RefreshList();
            if (_symbolizers.Count == 0) return;
            if (index >= _symbolizers.Count)
            {
                index -= 1;
            }

            lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            IFeatureSymbolizer sym = lbxItems.SelectedItem as IFeatureSymbolizer;
            if (sym == null) return;
            _symbolizers.IncreaseIndex(sym);
            RefreshList();
            lbxItems.SelectedItem = sym;
            OnOrderChanged();
        }

        private void LbxItemsDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;

            // prepare to draw the rectangle for symbol display and selection
            Rectangle outer = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, outer);
            }
            else
            {
                Brush b = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(b, outer);
                b.Dispose();
            }

            Rectangle inner = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 1, e.Bounds.Width - 10, e.Bounds.Height - 3);
            e.Graphics.FillRectangle(Brushes.White, inner);
            e.Graphics.DrawRectangle(Pens.Black, inner);

            // draw the symbolizer itself
            PointSymbolizer pointSym = lbxItems.Items[e.Index] as PointSymbolizer;
            if (pointSym != null)
            {
                DrawPointSymbolizer(pointSym, e.Graphics, inner);
                return;
            }

            LineSymbolizer lineSym = lbxItems.Items[e.Index] as LineSymbolizer;
            if (lineSym != null)
            {
                DrawLineSymbolizer(lineSym, e.Graphics, inner);
                return;
            }

            PolygonSymbolizer polySym = lbxItems.Items[e.Index] as PolygonSymbolizer;
            if (polySym != null)
            {
                DrawPolygonSymbolizer(polySym, e.Graphics, inner);
            }
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        #endregion
    }
}
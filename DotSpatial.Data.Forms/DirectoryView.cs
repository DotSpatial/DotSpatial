// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 1:58:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// DirectoryView
    /// </summary>
    public class DirectoryView : ScrollingControl
    {
        #region Private Variables

        /// <summary>
        /// Designer variable
        /// </summary>
        //private IContainer components = null; // the members to be contained
        private string _directory;

        private bool _ignoreSelectChanged;
        private List<DirectoryItem> _items;
        private DirectoryItem _selectedItem; // the most recently selected
        private DirectoryItem _selectionStart;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DirectoryView
        /// </summary>
        public DirectoryView()
        {
            _items = new List<DirectoryItem>();
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected item.  In cases of a multiple select, this is the
        /// last member added to the selection.
        /// </summary>
        public DirectoryItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the string path that should be itemized in the view.
        /// </summary>
        public string Directory
        {
            get { return _directory; }
            set
            {
                _directory = value;
                UpdateContent();
            }
        }

        /// <summary>
        /// Gets or sets the collection of DirectoryItems to draw in this control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<DirectoryItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// Draws
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialize(PaintEventArgs e)
        {
            UpdateContent();

            // translate into document coordinates
            Matrix oldMat = e.Graphics.Transform;
            Matrix mat = new Matrix();
            e.Graphics.Transform = mat;
            foreach (DirectoryItem item in _items)
            {
                if (ControlRectangle.IntersectsWith(item.Bounds))
                {
                    item.Draw(e);
                }
            }
            e.Graphics.Transform = oldMat;
            base.OnInitialize(e);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets or sets the Font to be used for all of the items in this view.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the Font to be used for all of the items in this view.")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                UpdateContent();
            }
        }

        /// <summary>
        /// Handles the situation where the mouse has been pressed down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (DirectoryItem di in _items)
            {
                if (di.Bounds.Contains(e.Location))
                {
                    di.IsSelected = true;
                    RefreshItem(di);
                    Invalidate(DocumentToClient(di.Bounds));
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Updates the buffer in order to correctly re-draw this item, if it is on the page, and then invalidates
        /// the area where this will be drawn.
        /// </summary>
        /// <param name="item">The directory item to invalidate</param>
        public void RefreshItem(DirectoryItem item)
        {
            Graphics g = Graphics.FromImage(Page);
            Brush b = new SolidBrush(item.BackColor);
            g.FillRectangle(b, DocumentToClient(item.Bounds));
            b.Dispose();

            // first translate into document coordinates
            Matrix mat = new Matrix();
            mat.Translate(ClientRectangle.X, ClientRectangle.Y);
            g.Transform = mat;

            item.Draw(new PaintEventArgs(g, item.Bounds));

            g.Dispose();
        }

        ///// <summary>
        ///// This handles drawing but extends the
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnDraw(PaintEventArgs e)
        //{
        //    base.OnDraw(e);
        //    foreach (DirectoryItem item in _items)
        //    {
        //        item.Draw(e);
        //    }

        //}

        #endregion

        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Removes the existing Directory Items from the control
        /// </summary>
        public virtual void Clear()
        {
            if (_items != null) _items.Clear();
        }

        /// <summary>
        /// Causes the control to refresh the current content.
        /// </summary>
        public void UpdateContent()
        {
            if (Controls == null) return;
            SuspendLayout();
            int tp = 1;
            Clear();

            Graphics g = CreateGraphics();
            SizeF fontSize = g.MeasureString("TEST", Font);
            int fontHeight = (int)(fontSize.Height + 4);
            if (fontHeight < 20) fontHeight = 20;
            AddFolders(ref tp, fontHeight);

            AddFiles(ref tp, fontHeight);
            if (tp < fontHeight * _items.Count) tp = fontHeight * _items.Count;
            DocumentRectangle = new Rectangle(DocumentRectangle.X, DocumentRectangle.Y, ClientRectangle.Width, tp);
            ResetScroll();
            ResumeLayout(false);
        }

        private void AddFiles(ref int tp, int fontHeight)
        {
            if (_directory == null) return;
            string[] files = System.IO.Directory.GetFiles(_directory);

            foreach (string file in files)
            {
                FileItem fi = new FileItem(file);
                if (fi.ItemType == ItemType.Custom) continue;
                fi.Top = tp;
                fi.Font = Font;
                fi.Height = fontHeight;
                _items.Add(fi);
                tp += fontHeight;
            }
        }

        private void item_SelectChanged(object sender, SelectEventArgs e)
        {
            if (_ignoreSelectChanged) return;

            DirectoryItem di = sender as DirectoryItem;
            if (di == null) return;

            if ((e.Modifiers & Keys.Control) == Keys.Control && (e.Modifiers & Keys.Shift) != Keys.Shift)
            {
                if (_selectedItem != null)
                {
                    _selectedItem.IsOutlined = false;
                }
                _selectionStart = di;
                _selectedItem = di;
                _selectedItem.IsOutlined = true;
                Invalidate();
                return;
            }
            _ignoreSelectChanged = true;

            if (((e.Modifiers & Keys.Shift) == Keys.Shift) && ((e.Modifiers & Keys.Control) != Keys.Control))
            {
                ClearSelection();
                _selectedItem.IsOutlined = false;
                int iOld;
                if (_selectionStart != null)
                {
                    iOld = _items.IndexOf(_selectionStart);
                }
                else
                {
                    _selectionStart = _items[0];
                    iOld = 0;
                }
                int iNew = _items.IndexOf(di);

                int start = Math.Min(iOld, iNew);
                int end = Math.Max(iOld, iNew);

                for (int i = start; i <= end; i++)
                {
                    _items[i].IsSelected = true;
                }
                _selectionStart.IsOutlined = false;
            }

            // With no control keys or both.
            if ((((e.Modifiers & Keys.Shift) == Keys.Shift) && ((e.Modifiers & Keys.Control) == Keys.Control)) ||
                (((e.Modifiers & Keys.Shift) != Keys.Shift) && ((e.Modifiers & Keys.Control) != Keys.Control)))
            {
                if (_selectedItem != null)
                {
                    _selectedItem.IsOutlined = false;
                }
                ClearSelection();
                di.IsSelected = true;
                _selectionStart = di;
                di.IsOutlined = true;
            }
            _selectedItem = di;
            di.IsOutlined = true;
            Invalidate();

            _ignoreSelectChanged = false;
        }

        /// <summary>
        /// Systematically clears any currently selected items.
        /// </summary>
        public void ClearSelection()
        {
            foreach (DirectoryItem di in _items)
            {
                di.IsSelected = false;
            }
        }

        private void AddFolders(ref int tp, int fontHeight)
        {
            if (_directory == null) return;
            string temp = _directory.Trim(Path.DirectorySeparatorChar);
            string[] currentDirectoryParts = temp.Split(Path.DirectorySeparatorChar);

            if (currentDirectoryParts.Length > 1)
            {
                if (_directory != null)
                {
                    DirectoryInfo parent = System.IO.Directory.GetParent(_directory);
                    if (parent != null)
                    {
                        FolderItem up = new FolderItem(parent.FullName);

                        up.Text = "..";
                        up.Font = this.Font;
                        up.Top = tp;
                        up.Height = fontHeight;
                        _items.Add(up);
                    }
                }
                tp += fontHeight;
            }

            if (_directory != null)
            {
                string[] subDirs = System.IO.Directory.GetDirectories(_directory);
                if (_items == null) _items = new List<DirectoryItem>();
                foreach (string dir in subDirs)
                {
                    FolderItem di = new FolderItem(dir);
                    di.Font = this.Font;
                    di.Top = tp;
                    di.Height = fontHeight;
                    //di.Navigate += new EventHandler<NavigateEventArgs>(item_Navigate);
                    //di.SelectChanged += new EventHandler<SelectEventArgs>(item_SelectChanged);
                    _items.Add(di);
                    tp += fontHeight;
                }
            }
        }

        private void item_Navigate(object sender, NavigateEventArgs e)
        {
            _directory = e.Path;
            UpdateContent();
            Invalidate();
        }
    }
}
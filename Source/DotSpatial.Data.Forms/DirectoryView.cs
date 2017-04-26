// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 1:58:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Fields
        private string _directory;
        private DirectoryItem _selectedItem; // the most recently selected
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryView"/> class.
        /// </summary>
        public DirectoryView()
        {
            Items = new List<DirectoryItem>();
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string path that should be itemized in the view.
        /// </summary>
        public string Directory
        {
            get
            {
                return _directory;
            }

            set
            {
                _directory = value;
                UpdateContent();
            }
        }

        /// <summary>
        /// Gets or sets the Font to be used for all of the items in this view.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the Font to be used for all of the items in this view.")]
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
        /// Gets or sets the collection of DirectoryItems to draw in this control
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<DirectoryItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the selected item. In cases of a multiple select, this is the
        /// last member added to the selection.
        /// </summary>
        public DirectoryItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                _selectedItem = value;
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes the existing Directory Items from the control
        /// </summary>
        public virtual void Clear()
        {
            Items?.Clear();
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

        /// <summary>
        /// Causes the control to refresh the current content.
        /// </summary>
        public void UpdateContent()
        {
            SuspendLayout();
            int tp = 1;
            Clear();

            Graphics g = CreateGraphics();
            SizeF fontSize = g.MeasureString("TEST", Font);
            int fontHeight = (int)(fontSize.Height + 4);
            if (fontHeight < 20) fontHeight = 20;
            AddFolders(ref tp, fontHeight);

            AddFiles(ref tp, fontHeight);
            if (tp < fontHeight * Items.Count) tp = fontHeight * Items.Count;
            DocumentRectangle = new Rectangle(DocumentRectangle.X, DocumentRectangle.Y, ClientRectangle.Width, tp);
            ResetScroll();
            ResumeLayout(false);
        }

        /// <summary>
        /// Draws
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnInitialize(PaintEventArgs e)
        {
            UpdateContent();

            // translate into document coordinates
            Matrix oldMat = e.Graphics.Transform;
            Matrix mat = new Matrix();
            e.Graphics.Transform = mat;
            foreach (DirectoryItem item in Items)
            {
                if (ControlRectangle.IntersectsWith(item.Bounds))
                {
                    item.Draw(e);
                }
            }

            e.Graphics.Transform = oldMat;
            base.OnInitialize(e);
        }

        /// <summary>
        /// Handles the situation where the mouse has been pressed down.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (DirectoryItem di in Items)
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
                Items.Add(fi);
                tp += fontHeight;
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
                        FolderItem up = new FolderItem(parent.FullName)
                        {
                            Text = "..",
                            Font = Font,
                            Top = tp,
                            Height = fontHeight
                        };

                        Items.Add(up);
                    }
                }

                tp += fontHeight;
            }

            if (_directory != null)
            {
                string[] subDirs = System.IO.Directory.GetDirectories(_directory);
                if (Items == null) Items = new List<DirectoryItem>();
                foreach (string dir in subDirs)
                {
                    FolderItem di = new FolderItem(dir)
                    {
                        Font = Font,
                        Top = tp,
                        Height = fontHeight
                    };
                    Items.Add(di);
                    tp += fontHeight;
                }
            }
        }

        private void InitializeComponent()
        {
        }

        #endregion
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/25/2008 9:37:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        |  2/18/2010         |  Added zoom out button and custom mouse cursors
// Kyle Ellison       | 12/15/2010         |  Fixed Issue #190 (Deactivated MapFunctions still involved)
// Eric Hullinger     | 12/28/2012         |  Resolved Issue (Unrestricted Zoom Out)
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The Map Control for 2D applications.
    /// </summary>
    partial class MapCore : UserControl, IMessageFilter
    {
        partial void InitPlatformSpecific()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
        }

        /// <summary>
        /// Handles the resizing in the case where the map uses docking, and therefore
        /// needs to be updated whenever the form changes size.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            KeyUp += Map_KeyUp;
            KeyDown += Map_KeyDown;

            SizeChanged += OnSizeChanged;
        }

        public bool PreFilterMessage(ref Message m) 
        {
            if (m.Msg == 0x0100)
            {
                if(ContainsFocus)
                    OnKeyDown(new KeyEventArgs((Keys)m.WParam.ToInt32()));
            }
            else if (m.Msg == 0x0101)
            {
                if (ContainsFocus)
                    OnKeyUp(new KeyEventArgs((Keys)m.WParam.ToInt32()));
            }
            return false;
        }

        private void Map_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoKeyUp(e);
                if (e.Handled) break;
            }
        }

        private void Map_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoKeyDown(e);
                if (e.Handled) break;
            }
        }

        /// <inheritdoc />
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = drgevent.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            base.OnDragEnter(drgevent);
        }

        /// <inheritdoc />
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            string[] s = (string[])drgevent.Data.GetData(DataFormats.FileDrop, false);
            if (s == null)
            {
                base.OnDragDrop(drgevent);
                return;
            }
            int i;
            bool failed = false;
            for (i = 0; i < s.Length; i++)
            {
                try
                {
                    AddLayer(s[i]);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    // failed at least one effort
                    failed = true;
                }
            }
            if (failed)
            {
                MessageBox.Show(MessageStrings.Map_OnDragDrop_Invalid);
            }
        }

        /// <summary>
        /// Cursor hiding from designer
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
            }
        }

        /// <summary>
        /// Gets or sets the current tool mode.  This rapidly enables or disables specific tools to give
        /// a combination of functionality.  Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        public FunctionMode FunctionMode
        {
            get { return _functionMode; }
            set
            {
                _functionMode = value;
                switch (_functionMode)
                {
                case FunctionMode.ZoomIn:
                    try
                    {
                        MemoryStream ms = new MemoryStream(Images.cursorZoomIn);
                        Cursor = new Cursor(ms);
                    }
                    catch
                    {
                        Cursor = Cursors.Arrow;
                    }
                    break;
                case FunctionMode.ZoomOut:
                    try
                    {
                        MemoryStream ms = new MemoryStream(Images.cursorZoomOut);
                        Cursor = new Cursor(ms);
                    }
                    catch
                    {
                        Cursor = Cursors.Arrow;
                    }
                    break;
                case FunctionMode.Info:
                    Cursor = Cursors.Help;
                    break;
                case FunctionMode.Label:
                    Cursor = Cursors.IBeam;
                    break;
                case FunctionMode.None:
                    Cursor = Cursors.Arrow;
                    break;
                case FunctionMode.Pan:
                    try
                    {
                        MemoryStream ms = new MemoryStream(Images.cursorHand);
                        Cursor = new Cursor(ms);
                    }
                    catch
                    {
                        Cursor = Cursors.SizeAll;
                        break;
                    }
                    break;

                case FunctionMode.Select:
                    try
                    {
                        MemoryStream ms = new MemoryStream(Images.cursorSelect);
                        Cursor = new Cursor(ms);
                    }
                    catch
                    {
                        Cursor = Cursors.Hand;
                    }
                    break;
                }

                // Turn off functions that are not "Always on"
                if (_functionMode == FunctionMode.None)
                {
                    foreach (var f in MapFunctions)
                    {
                        if ((f.YieldStyle & YieldStyles.AlwaysOn) != YieldStyles.AlwaysOn) f.Deactivate();
                    }
                }
                else
                {
                    IMapFunction newMode = _functionLookup[_functionMode];
                    ActivateMapFunction(newMode);
                    // Except for function mode "none" allow scrolling
                    IMapFunction scroll = MapFunctions.Find(f => f.GetType() == typeof(MapFunctionZoom));
                    ActivateMapFunction(scroll);
                }

                //function mode changed event
                OnFunctionModeChanged(this, EventArgs.Empty);
            }
        }

        public Dictionary<FunctionMode, IMapFunction> FunctionLookup
        {
            get
            {
                return _functionLookup;
            }
            set
            {
                _functionLookup = value;
            }
        }

        /// <summary>
        /// This causes all of the data layers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate"
        /// </summary>
        public override void Refresh()
        {
            MapFrame.Initialize();
            base.Refresh();
            Invalidate();
        }

        /// <summary>
        /// Occurs when this control tries to paint the background.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //This is done deliberately to prevent flicker.
            //base.OnPaintBackground(e);
        }

        /// <summary>
        /// Perform custom drawing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (MapFrame.IsPanning) return;

            var clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;

            // if the area to paint is too small, there's nothing to paint.
            // Added to fix http://dotspatial.codeplex.com/workitem/320
            if (clip.Width < 1 || clip.Height < 1) return;

            using (var stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(stencil))
            {
                using(var b = new SolidBrush(BackColor))
                    g.FillRectangle(b, new Rectangle(0, 0, stencil.Width, stencil.Height));

                using (var m = new Matrix())
                {
                    m.Translate(-clip.X, -clip.Y);
                    g.Transform = m;

                    Draw(g, e);

                    var args = new MapDrawArgs(g, clip, MapFrame);
                    foreach (var tool in MapFunctions.Where(_ => _.Enabled))
                    {
                        tool.Draw(args);
                    }

                    var pe = new PaintEventArgs(g, e.ClipRectangle);
                    base.OnPaint(pe);
                }

                e.Graphics.DrawImageUnscaled(stencil, clip.X, clip.Y);
            }
        }

        /// <summary>
        /// Fires the DoMouseDoubleClick method on the ActiveTools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseDoubleClick(args);
                if (args.Handled) break;
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Fires the OnMouseDown event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseDown(args);
                if (args.Handled) break;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Fires the OnMouseUp event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseUp(args);
                if (args.Handled) break;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the OnMouseMove event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseMove(args);
                if (args.Handled) break;
            }

            OnMouseMove(args);

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Fires the OnMouseWheel event for the active tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseWheel(args);
                if (args.Handled) break;
            }

            base.OnMouseWheel(e);
        }
    }
}
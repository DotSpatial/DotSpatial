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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 3:21:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can zoom the map out based on mouse clicks.
    /// </summary>
    public class MapFunctionKeyNavigation : MapFunction
    {
        #region Private Variables

        private FunctionMode previousFunction = FunctionMode.None;
        private bool isPanningTemporarily;
        private int KeyPanCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public MapFunctionKeyNavigation(IMap inMap)
            : base(inMap)
        {
            YieldStyle = YieldStyles.AlwaysOn;
            BusySet = false;
        }

        #endregion

        #region Keyboard Input Hanlders

        /// <summary>
        /// Handles the Key Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            // Allow panning if the space is pressed.
            if (e.KeyCode == Keys.Space && isPanningTemporarily)
            {
                Map.FunctionMode = previousFunction;
                isPanningTemporarily = false;
            }

            // Arrow-Key Panning
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                Map.MapFrame.ResetExtents();
                Map.IsBusy = false;
                BusySet = false;
                KeyPanCount = 0;
            }

            // Large Step Panning
            if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
            {
                Map.IsBusy = true;
                var _source = Map.MapFrame.View;

                switch (e.KeyCode)
                {
                    case Keys.PageUp:
                        Map.MapFrame.View = new Rectangle(_source.X, _source.Y - (int)(_source.Height * 0.75), _source.Width, _source.Height);
                        break;
                    case Keys.PageDown:
                        Map.MapFrame.View = new Rectangle(_source.X, _source.Y + (int)(_source.Height * 0.75), _source.Width, _source.Height);
                        break;
                    case Keys.Home:
                        Map.MapFrame.View = new Rectangle(_source.X - (int)(_source.Width * 0.75), _source.Y, _source.Width, _source.Height);
                        break;
                    case Keys.End:
                        Map.MapFrame.View = new Rectangle(_source.X + (int)(_source.Width * 0.75), _source.Y, _source.Width, _source.Height);
                        break;
                }

                Map.MapFrame.ResetExtents();
                Map.IsBusy = false;
            }
        }

        /// <summary>
        /// Handles the Key Down situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Allow panning if the space is pressed.
            if (e.KeyCode == Keys.Space && !isPanningTemporarily)
            {
                previousFunction = Map.FunctionMode;
                Map.FunctionMode = FunctionMode.Pan;
                isPanningTemporarily = true;
            }

            // Arrow-Key Panning
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                if (!BusySet)
                {
                    Map.IsBusy = true;
                    BusySet = true;
                }

                var _source = Map.MapFrame.View;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        Map.MapFrame.View = new Rectangle(_source.X, _source.Y - 20, _source.Width, _source.Height);
                        break;
                    case Keys.Down:
                        Map.MapFrame.View = new Rectangle(_source.X, _source.Y + 20, _source.Width, _source.Height);
                        break;
                    case Keys.Left:
                        Map.MapFrame.View = new Rectangle(_source.X - 20, _source.Y, _source.Width, _source.Height);
                        break;
                    case Keys.Right:
                        Map.MapFrame.View = new Rectangle(_source.X + 20, _source.Y, _source.Width, _source.Height);
                        break;
                }

                KeyPanCount++;

                if (KeyPanCount == 16)
                {
                    Map.MapFrame.ResetExtents();
                    KeyPanCount = 0;
                }
                else
                    Map.Invalidate();

            }

            // Zoom Out
            if (e.KeyCode == (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17) || e.KeyCode == Keys.Subtract)
            {
                if (Map.IsZoomedToMaxExtent)
                {
                }
                else
                {
                    Map.IsBusy = true;
                    Rectangle r = Map.MapFrame.View;

                    r.Inflate(r.Width / 2, r.Height / 2);
                    Map.MapFrame.View = r;
                    Map.MapFrame.ResetExtents();
                    Map.IsBusy = false;
                }
            }

            // Zoom In
            if (e.KeyCode == (Keys.LButton | Keys.RButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17) || e.KeyCode == Keys.Add)
            {
                Map.IsBusy = true;
                Map.IsZoomedToMaxExtent = false;
                Rectangle r = Map.MapFrame.View;

                r.Inflate(-r.Width / 4, -r.Height / 4);

                Map.MapFrame.View = r;
                Map.MapFrame.ResetExtents();
                Map.IsBusy = false;
            }
        }

        #endregion

        public bool BusySet { get; set; }
    }
}
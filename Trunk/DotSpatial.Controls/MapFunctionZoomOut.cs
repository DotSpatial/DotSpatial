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

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can zoom the map out based on mouse clicks.
    /// </summary>
    public class MapFunctionZoomOut : MapFunction
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public MapFunctionZoomOut(IMap inMap)
            : base(inMap)
        {
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton;
        }

        #endregion

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Map.IsZoomedToMaxExtent)
            {}
            else
            {
                e.Map.IsZoomedToMaxExtent = false;
                Map.Invalidate();

                Rectangle r = e.Map.MapFrame.View;
                int w = r.Width;
                int h = r.Height;

                if (e.Button == MouseButtons.Left)
                {
                    r.Inflate(r.Width / 2, r.Height / 2);
                    r.X += w / 2 - e.X;
                    r.Y += h / 2 - e.Y;
                    e.Map.MapFrame.View = r;
                    e.Map.MapFrame.ResetExtents();
                }
                else
                {
                    r.Inflate(-r.Width / 4, -r.Height / 4);
                    // The mouse cursor should anchor the geographic location during zoom.
                    r.X += (e.X / 2) - w / 4;
                    r.Y += (e.Y / 2) - h / 4;
                    e.Map.MapFrame.View = r;
                    e.Map.MapFrame.ResetExtents();
                }

                base.OnMouseUp(e);
            }
        }
    }
}
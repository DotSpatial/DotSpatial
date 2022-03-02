// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can zoom the map out based on left mouse clicks and in based on right mouse clicks.
    /// </summary>
    public class MapFunctionZoomOut : MapFunction
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionZoomOut"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
        public MapFunctionZoomOut(IMap inMap)
            : base(inMap)
        {
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Mouse Up situation.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (!(e.Map.IsZoomedToMaxExtent && e.Button == MouseButtons.Left))
            {
                e.Map.IsZoomedToMaxExtent = false;
                Map.Invalidate();

                Rectangle r = e.Map.MapFrame.View;
                int w = r.Width;
                int h = r.Height;

                if (e.Button == MouseButtons.Left)
                {
                    r.Inflate(r.Width / 2, r.Height / 2);
                    r.X += (w / 2) - e.X;
                    r.Y += (h / 2) - e.Y;
                    e.Map.MapFrame.View = r;
                    e.Map.MapFrame.ResetExtents();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    r.Inflate(-r.Width / 4, -r.Height / 4);

                    // The mouse cursor should anchor the geographic location during zoom.
                    r.X += (e.X / 2) - (w / 4);
                    r.Y += (e.Y / 2) - (h / 4);
                    e.Map.MapFrame.View = r;
                    e.Map.MapFrame.ResetExtents();
                }

                base.OnMouseUp(e);
            }
        }

        #endregion
    }
}
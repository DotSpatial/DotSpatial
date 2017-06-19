// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can pan the map.
    /// </summary>
    public class MapFunctionPan : MapFunction
    {
        #region Fields

        private Point _dragStart;
        private bool _preventDrag;
        private Rectangle _source;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionPan"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
        public MapFunctionPan(IMap inMap)
            : base(inMap)
        {
            YieldStyle = YieldStyles.LeftButton;
            BusySet = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the map function is currently interacting with the map.
        /// </summary>
        public bool BusySet { get; set; }

        /// <summary>
        /// Gets a value indicating whether this tool is currently being used.
        /// </summary>
        public bool IsDragging { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the actions that the tool controls during the OnMouseDown event
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left && _preventDrag == false)
            {
                _dragStart = e.Location;
                _source = e.Map.MapFrame.View;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event, changing the viewing extents to match the movements
        /// of the mouse if the left mouse button is down.
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_dragStart != Point.Empty && _preventDrag == false)
            {
                if (!BusySet)
                {
                    Map.IsBusy = true;
                    BusySet = true;
                }

                IsDragging = true;
                Point diff = new Point
                             {
                                 X = _dragStart.X - e.X,
                                 Y = _dragStart.Y - e.Y
                             };
                e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width, _source.Height);
                Map.Invalidate();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Mouse Up
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left && IsDragging)
            {
                IsDragging = false;

                _preventDrag = true;
                e.Map.MapFrame.ResetExtents();
                _preventDrag = false;
                Map.IsBusy = false;
                BusySet = false;
            }

            _dragStart = Point.Empty;

            base.OnMouseUp(e);
        }

        #endregion
    }
}
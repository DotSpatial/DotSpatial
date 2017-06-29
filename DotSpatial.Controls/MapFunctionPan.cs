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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/11/2008 3:54:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can pan the map.
    /// </summary>
    public class MapFunctionPan : MapFunction
    {
        #region Private Variables

        private Point _dragStart;
        private bool _isDragging;
        private bool _preventDrag;
        private Rectangle _source;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MapFunctionPan class.
        /// </summary>
        public MapFunctionPan(IMap inMap)
            : base(inMap)
        {
            YieldStyle = YieldStyles.LeftButton;
            BusySet = false;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// This indicates that this tool is currently being used.
        /// </summary>
        public bool IsDragging
        {
            get { return _isDragging; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the actions that the tool controls during the OnMouseDown event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left && _preventDrag == false)
            {
                //PreventBackBuffer = true;
                _dragStart = e.Location;
                _source = e.Map.MapFrame.View;
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event, changing the viewing extents to match the movements
        /// of the mouse if the left mouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_dragStart != Point.Empty && _preventDrag == false)
            {
                if (!BusySet)
                {
                    Map.IsBusy = true;
                    BusySet = true;
                }

                _isDragging = true;
                Point diff = new Point { X = _dragStart.X - e.X, Y = _dragStart.Y - e.Y };
                e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width,
                                                    _source.Height);
                Map.Invalidate();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Mouse Up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left && _isDragging)
            {
                _isDragging = false;

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

        public bool BusySet { get; set; }
    }
}
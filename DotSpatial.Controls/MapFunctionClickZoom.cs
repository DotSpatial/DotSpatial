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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using System.Linq;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can zoom into the map using left mouse clicks or rectangle dragging. It zooms out on right mouse clicks.
    /// </summary>
    public class MapFunctionClickZoom : MapFunction
    {
        #region Private Variables

        private readonly Pen _selectionPen;
        private Point _currentPoint;
        private Coordinate _geoStartPoint;
        private bool _isDragging;
        private Point _startPoint;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public MapFunctionClickZoom(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black) { DashStyle = DashStyle.Dash };
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton;
        }

        #endregion

        protected override void OnDraw(MapDrawArgs e)
        {
            if (_isDragging)
            {
                Rectangle r = Opp.RectangleFromPoints(_startPoint, _currentPoint);
                r.Width -= 1;
                r.Height -= 1;
                e.Graphics.DrawRectangle(Pens.White, r);
                e.Graphics.DrawRectangle(_selectionPen, r);
            }
            base.OnDraw(e);
        }

        /// <summary>
        /// Handles the MouseDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
                _currentPoint = _startPoint;
                _geoStartPoint = e.GeographicLocation;
                _isDragging = true;
                Map.IsBusy = true;
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles MouseMove
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_isDragging)
            {
                int x = Math.Min(Math.Min(_startPoint.X, _currentPoint.X), e.X);
                int y = Math.Min(Math.Min(_startPoint.Y, _currentPoint.Y), e.Y);
                int mx = Math.Max(Math.Max(_startPoint.X, _currentPoint.X), e.X);
                int my = Math.Max(Math.Max(_startPoint.Y, _currentPoint.Y), e.Y);
                _currentPoint = e.Location;
                Map.Invalidate(new Rectangle(x, y, mx - x, my - y));
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (!(e.Map.IsZoomedToMaxExtent && e.Button == MouseButtons.Right))
            {
                e.Map.IsZoomedToMaxExtent = false;
                bool handled = false;
                _currentPoint = e.Location;

                Map.Invalidate();
                if (_isDragging)
                {
                    if (_geoStartPoint != null && _startPoint != e.Location)
                    {
                        IEnvelope env = new Envelope(_geoStartPoint.X, e.GeographicLocation.X,
                                                     _geoStartPoint.Y, e.GeographicLocation.Y);
                        if (Math.Abs(e.X - _startPoint.X) > 1 && Math.Abs(e.Y - _startPoint.Y) > 1)
                        {
                            e.Map.ViewExtents = env.ToExtent();
                            handled = true;
                        }
                    }
                }
                _isDragging = false;

                if (handled == false)
                {
                    /*Rectangle r = e.Map.MapFrame.View;
                    int w = r.Width;
                    int h = r.Height;
                    if (e.Button == MouseButtons.Left)
                    {
                        r.Inflate(-r.Width / 4, -r.Height / 4);
                        // The mouse cursor should anchor the geographic location during zoom.
                        r.X += (e.X / 2) - w / 4;
                        r.Y += (e.Y / 2) - h / 4;
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        r.Inflate(r.Width / 2, r.Height / 2);
                        r.X += w / 2 - e.X;
                        r.Y += h / 2 - e.Y;
                    }
                    e.Map.MapFrame.View = r;
                    e.Map.MapFrame.ResetExtents();*/

                    //CGX
                    int iTemp = Convert.ToInt32(e.Map.MapFrame.CurrentScale);
                    if (iTemp > 1)
                    {
                        int[] array = new int[83]{500000000,400000000,300000000,250000000,200000000,150000000,125000000,100000000,80000000,60000000,40000000,30000000,25000000,20000000,15000000,12500000,10000000,
                        8000000,6000000,5000000,4000000,3000000,2500000,2000000,1500000,1250000,1000000,800000,700000,600000,500000,400000,300000,250000,200000,150000,125000,100000,80000,70000,60000,50000,
                        40000,30000,25000,20000,15000,12500,10000,8000,6000,5000,4000,3000,2500,2000,1500,1250,1000,800,600,500,400,300,250,200,150,125,100,80,60,50,40,30,25,20,15,10,5,4,3,2,1};

                        var closest = array.Aggregate((current, next) => Math.Abs((long)current - iTemp) < Math.Abs((long)next - iTemp) ? current : next);// array.OrderBy(v => Math.Abs((long)v - lTemp)).First();
                        int iTemp2 = Convert.ToInt32(closest);
                        while (iTemp2 >= iTemp)
                        {
                            array = array.Where(val => val != iTemp2).ToArray();
                            closest = Convert.ToInt32(array.Aggregate((current, next) => Math.Abs((long)current - iTemp) < Math.Abs((long)next - iTemp) ? current : next));//  array.OrderBy(v => Math.Abs((long)v - lTemp)).First());
                            iTemp2 = Convert.ToInt32(closest);
                        }

                        e.Map.MapFrame.ComputeExtentFromScale(Convert.ToDouble(closest), e.Location);
                    }
                    //Fin CGX
                }
            }

            base.OnMouseUp(e);
            Map.IsBusy = false;
        }
    }
}
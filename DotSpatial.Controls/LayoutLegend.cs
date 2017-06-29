// ********************************************************************************************************
// Product Name: DotSpatial.Layout.Elements.LayoutLegend
// Description:  The Legend element for use in the DotSpatial Layout
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Draws a legend for the layout
    /// </summary>
    public class LayoutLegend : LayoutElement
    {
        private readonly List<IMapLayer> _layers;
        private Color _color;
        private Font _font;
        private LayoutControl _layoutControl;
        private LayoutMap _layoutMap;
        private int _numCol;
        private TextRenderingHint _textHint;

        #region ------------------ Public Properties

        /// <summary>
        /// Gets or sets the layoutmap to use to base the legend on
        /// </summary>
        [Browsable(true), Category("Symbol"), Editor(typeof(LayoutMapEditor), typeof(UITypeEditor))]
        public virtual LayoutMap Map
        {
            get { return _layoutMap; }
            set
            {
                if (value != null && value != _layoutMap)
                {
                    _layoutMap = value;
                    _layers.Clear();
                    if (Map.MapControl != null)
                    {
                        foreach (IMapLayer t in Map.MapControl.Layers)
                        {
                            if (t.Checked)
                            {
                                _layers.Add(t);
                            }
                        }
                    }

                    base.UpdateThumbnail();
                    base.OnInvalidate();
                }
                else if (value == null)
                {
                    _layers.Clear();
                }
            }
        }

        /// <summary>
        /// Gets or sets the layers to include in the legend
        /// </summary>
        [Browsable(true), Category("Symbol"), Editor(typeof(LayoutLayerEditor), typeof(UITypeEditor))]
        public virtual List<int> Layers
        {
            get
            {
                if (Map == null || Map.MapControl == null)
                    return new List<int>();
                List<int> layerInts = new List<int>();
                for (int i = 0; i < Map.MapControl.Layers.Count; i++)
                    if (_layers.Contains(Map.MapControl.Layers[i])) layerInts.Add(i);
                return layerInts;
            }
            set
            {
                if (Map == null || Map.MapControl != null)
                {
                    _layers.Clear();
                    value.Sort();
                    value.Reverse();
                    foreach (int i in value)
                    {
                        if (i < Map.MapControl.Layers.Count)
                            _layers.Add(Map.MapControl.Layers[i]);
                    }
                }
                else
                {
                    _layers.Clear();
                }
                base.UpdateThumbnail();
                base.OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the font used to draw this text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public virtual Font Font
        {
            get { return _font; }
            set { _font = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public virtual Color Color
        {
            get { return _color; }
            set { _color = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the number of columns to use when rendering the legend
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public virtual int NumColumns
        {
            get { return _numCol; }
            set
            {
                _numCol = value < 1 ? 1 : value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true), Category("Symbol")]
        public virtual TextRenderingHint TextHint
        {
            get { return _textHint; }
            set { _textHint = value; base.UpdateThumbnail(); base.OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets a layout control
        /// </summary>
        [Browsable(false)]
        public virtual LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                if (_layoutControl != null)
                {
                    _layoutControl.ElementsChanged -= LayoutControlElementsChanged;
                }
                _layoutControl = value;
                if (_layoutControl != null)
                {
                    _layoutControl.ElementsChanged += LayoutControlElementsChanged;
                }
            }
        }

        #endregion

        #region ------------------- event handlers

        /// <summary>
        /// Updates the scale bar if the map is deleted
        /// </summary>
        private void LayoutControlElementsChanged(object sender, EventArgs e)
        {
            if (_layoutControl.LayoutElements.Contains(_layoutMap) == false)
                Map = null;
        }

        #endregion

        #region ------------------- public methods

        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutLegend()
        {
            Name = "Legend";
            _font = new Font("Arial", 10);
            _color = Color.Black;
            _textHint = TextRenderingHint.AntiAliasGridFit;
            _numCol = 1;
            _layers = new List<IMapLayer>();
            ResizeStyle = ResizeStyle.NoScaling;
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">A boolean value indicating if the Draw code is being called to print</param>
        public override void Draw(Graphics g, bool printing)
        {
            //Make sure we don't get any null reference exceptions
            if (_layoutMap == null || _layoutMap.MapControl == null || _layoutMap.MapControl.Legend == null) return;

            //Makes sure that it is big enough to be drawn to
            if (Size.Width < 1 || Size.Height < 1) return;

            //Saves the old graphis settings
            TextRenderingHint oldTextHint = g.TextRenderingHint;
            g.TextRenderingHint = _textHint;
            Matrix oldTrans = g.Transform;

            //Calculates the size of each item and the max number of columns and rows
            SizeF itemSize = new SizeF(Size.Width / NumColumns, g.MeasureString("SampleText", _font).Height + 4);
            int maxCol = Convert.ToInt32(Size.Width / itemSize.Width);
            int maxRow = Convert.ToInt32(Math.Floor(Size.Height / itemSize.Height));
            int col = 0;
            int row = 0;

            //Loops through all of the legend items and populates the legend
            foreach (IMapLayer mapLayer in _layers)
            {
                if (mapLayer.LegendItems == null)
                    DrawLegendItem(g, mapLayer, itemSize, ref col, ref row, ref maxCol, ref maxRow);
                else
                    DrawLegendList(g, mapLayer.LegendItems, itemSize, ref col, ref row, ref maxCol, ref maxRow);
            }

            //Restored the old graphics settings
            g.Transform = oldTrans;
            g.TextRenderingHint = oldTextHint;
        }

        private void DrawLegendList(Graphics g, IEnumerable<ILegendItem> items, SizeF itemSize, ref int col, ref int row, ref int maxCol, ref int maxRow)
        {
            //If we are passed the max size return;
            if (col > maxCol)
                return;

            if (items == null || !items.Any()) return;
            List<ILegendItem> itemList = items.ToList();

            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                if (itemList[i].LegendItems == null)
                {
                    DrawLegendItem(g, itemList[i], itemSize, ref col, ref row, ref maxCol, ref maxRow);
                }
                else
                    DrawLegendList(g, itemList[i].LegendItems, itemSize, ref col, ref row, ref maxCol, ref maxRow);
            }
        }

        private void DrawLegendItem(Graphics g, ILegendItem item, SizeF itemSize, ref int col, ref int row, ref int maxCol, ref int maxRow)
        {
            if (row >= maxRow)
            {
                row = 0;
                col++;
                if (col > maxCol)
                    return;
            }

            g.TranslateTransform(LocationF.X + (col * itemSize.Width), LocationF.Y + (row * itemSize.Height));
            item.PrintLegendItem(g, _font, _color, itemSize);
            g.TranslateTransform(-(LocationF.X + (col * itemSize.Width)), -(LocationF.Y + (row * itemSize.Height)));
            row++;
        }

        #endregion
    }
}
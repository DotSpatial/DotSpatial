// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
    /// Draws a legend for the layout.
    /// </summary>
    public class LayoutLegend : LayoutElement
    {
        #region Fields

        private readonly List<IMapLayer> _layers;
        private Color _color;
        private Font _font;
        private LayoutControl _layoutControl;
        private LayoutMap _layoutMap;
        private int _numCol;
        private TextRenderingHint _textHint;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutLegend"/> class.
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public virtual Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the font used to draw this text.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public virtual Font Font
        {
            get
            {
                return _font;
            }

            set
            {
                _font = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the layers to include in the legend.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        [Editor(typeof(LayoutLayerEditor), typeof(UITypeEditor))]
        public virtual List<int> Layers
        {
            get
            {
                if (Map?.MapControl == null)
                    return new List<int>();
                List<int> layerInts = new();
                for (int i = 0; i < Map.MapControl.Layers.Count; i++)
                    if (_layers.Contains(Map.MapControl.Layers[i])) layerInts.Add(i);
                return layerInts;
            }

            set
            {
                _layers.Clear();

                if (Map?.MapControl != null)
                {
                    value.Sort();
                    value.Reverse();
                    foreach (int i in value)
                    {
                        if (i < Map.MapControl.Layers.Count)
                            _layers.Add(Map.MapControl.Layers[i]);
                    }
                }

                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a layout control.
        /// </summary>
        [Browsable(false)]
        public virtual LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

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

        /// <summary>
        /// Gets or sets the layoutmap to use to base the legend on.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        [Editor(typeof(LayoutMapEditor), typeof(UITypeEditor))]
        public virtual LayoutMap Map
        {
            get
            {
                return _layoutMap;
            }

            set
            {
                if (value != null && value != _layoutMap)
                {
                    _layoutMap = value;
                    _layers.Clear();
                    if (Map.MapControl != null)
                    {
                        // find all the layers in the map - not including "mapgroup" type layers
                        foreach (var layer in Map.MapControl.GetAllLayers())
                        {
                            if (layer is IMapLayer lyr && lyr.Checked)
                            {
                                _layers.Add(lyr);
                            }
                        }

                        // reverse the list so that they draw in the correct order
                        _layers.Reverse();
                    }

                    UpdateThumbnail();
                    OnInvalidate();
                }
                else if (value == null)
                {
                    _layers.Clear();
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of columns to use when rendering the legend.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public virtual int NumColumns
        {
            get
            {
                return _numCol;
            }

            set
            {
                _numCol = value < 1 ? 1 : value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the hinting used to draw the text.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public virtual TextRenderingHint TextHint
        {
            get
            {
                return _textHint;
            }

            set
            {
                _textHint = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        /// <param name="printing">A boolean value indicating if the Draw code is being called to print.</param>
        public override void Draw(Graphics g, bool printing)
        {
            // Make sure we don't get any null reference exceptions
            if (_layoutMap?.MapControl?.Legend == null) return;

            // Makes sure that it is big enough to be drawn to
            if (Size.Width < 1 || Size.Height < 1) return;

            // Saves the old graphis settings
            TextRenderingHint oldTextHint = g.TextRenderingHint;
            g.TextRenderingHint = _textHint;
            Matrix oldTrans = g.Transform;

            // Calculates the size of each item and the max number of columns and rows
            SizeF itemSize = new(Size.Width / NumColumns, g.MeasureString("SampleText", _font).Height + 4);
            int maxCol = Convert.ToInt32(Size.Width / itemSize.Width);
            int maxRow = Convert.ToInt32(Math.Floor(Size.Height / itemSize.Height));
            int col = 0;
            int row = 0;

            // Loops through all of the legend items and populates the legend
            foreach (IMapLayer mapLayer in _layers)
            {
                IMapLayer mapLayerClone = (IMapLayer)mapLayer.Clone();
                DrawLegendItem(g, mapLayerClone, itemSize, ref col, ref row, ref maxCol, ref maxRow);
                DrawLegendList(g, mapLayerClone.LegendItems, itemSize, ref col, ref row, ref maxCol, ref maxRow);
            }

            // Restored the old graphics settings
            g.Transform = oldTrans;
            g.TextRenderingHint = oldTextHint;
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
            if (item.LegendText is null)
            {
                item.LegendText = string.Empty;
            }

            item.PrintLegendItem(g, _font, _color, itemSize);
            g.TranslateTransform(-(LocationF.X + (col * itemSize.Width)), -(LocationF.Y + (row * itemSize.Height)));
            row++;
        }

        private void DrawLegendList(Graphics g, IEnumerable<ILegendItem> items, SizeF itemSize, ref int col, ref int row, ref int maxCol, ref int maxRow)
        {
            // If items are null or we are passed the max size return;
            if (items == null || col > maxCol)
                return;

            List<ILegendItem> itemList = items.ToList();

            foreach (ILegendItem item in itemList)
            {
                if (item.LegendItems == null)
                {
                    DrawLegendItem(g, item, itemSize, ref col, ref row, ref maxCol, ref maxRow);
                }
                else
                {
                    DrawLegendList(g, item.LegendItems, itemSize, ref col, ref row, ref maxCol, ref maxRow);
                }
            }
        }

        /// <summary>
        /// Updates the scale bar if the map is deleted.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlElementsChanged(object sender, EventArgs e)
        {
            if (!_layoutControl.LayoutElements.Contains(_layoutMap))
                Map = null;
        }

        #endregion
    }
}
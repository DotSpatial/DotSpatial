// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The actual control controling the layout.
    /// </summary>
    [ToolboxItem(true)]
    public partial class LayoutControl : UserControl
    {
        #region Fields
        private LayoutElement _elementToAddWithMouse;
        private string _fileName;
        private PointF _lastMousePoint;

        private RectangleF _mouseBox;
        private MouseMode _mouseMode;
        private PointF _mouseStartPoint;
        private PointF _paperLocation; // The location of the paper within the screen coordinants

        // Printer Variables
        private PrinterSettings _printerSettings;
        private Edge _resizeSelectedEdge;
        private Bitmap _resizeTempBitmap;
        private bool _showMargin;
        private bool _suppressLEinvalidate;

        private float _zoom; // The zoom of the paper

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControl"/> class.
        /// </summary>
        public LayoutControl()
        {
            InitializeComponent();
            _printerSettings = new PrinterSettings();
            _fileName = string.Empty;
            _suppressLEinvalidate = false;

            if (_printerSettings.IsValid)
            {
                // This code is used to speed up drawing because for some reason accessing .PaperSize is slow with its default value
                // I know its ugly but it speeds things up from 80ms to 0ms
                var tempForm = new PageSetupForm(_printerSettings);
                tempForm.OkButtonClick(null, null);

                DrawingQuality = SmoothingMode.HighQuality;
                _zoom = 1;
                ZoomFitToScreen();
            }
            else
            {
                MessageBox.Show(MessageStrings.LayoutControl_NoPrinterFound);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This fires after a element was added or removed.
        /// </summary>
        public event EventHandler ElementsChanged;

        /// <summary>
        /// This fires when the projects file name is changed.
        /// </summary>
        public event EventHandler FilenameChanged;

        /// <summary>
        /// This fires after a layout was loaded from file.
        /// </summary>
        public event EventHandler LayoutLoaded;

        /// <summary>
        /// This fires after the selection has changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// This fires when the zoom of the layout changes.
        /// </summary>
        public event EventHandler ZoomChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the smoothing mode to use to draw the map.
        /// </summary>
        public SmoothingMode DrawingQuality { get; set; }

        /// <summary>
        /// Gets or sets the fileName of the current project.
        /// </summary>
        public string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
                OnFilenameChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the LayoutDocToolStrip.
        /// </summary>
        public LayoutDocToolStrip LayoutDocToolStrip
        {
            get
            {
                return _layoutDocToolStrip;
            }

            set
            {
                if (value == null) return;
                _layoutDocToolStrip = value;
                _layoutDocToolStrip.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets the list of layoutElements currently loaded in the project.
        /// </summary>
        public List<LayoutElement> LayoutElements { get; } = new List<LayoutElement>();

        /// <summary>
        /// Gets or sets the LayoutInsertToolStrip.
        /// </summary>
        public LayoutInsertToolStrip LayoutInsertToolStrip
        {
            get
            {
                return _layoutInsertToolStrip;
            }

            set
            {
                if (value == null) return;
                _layoutInsertToolStrip = value;
                _layoutInsertToolStrip.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets or sets the layout list box.
        /// </summary>
        public LayoutListBox LayoutListBox
        {
            get
            {
                return _layoutListBox;
            }

            set
            {
                if (value == null) return;
                _layoutListBox = value;
                _layoutListBox.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets or sets the LayoutMapToolStrip.
        /// </summary>
        public LayoutMapToolStrip LayoutMapToolStrip
        {
            get
            {
                return _layoutMapToolStrip;
            }

            set
            {
                if (value == null) return;
                _layoutMapToolStrip = value;
                _layoutMapToolStrip.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets or sets the layout menu strip to use.
        /// </summary>
        public LayoutMenuStrip LayoutMenuStrip
        {
            get
            {
                return _layoutMenuStrip;
            }

            set
            {
                if (value == null) return;
                _layoutMenuStrip = value;
                _layoutMenuStrip.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets or sets the layoutproperty grip to use.
        /// </summary>
        public LayoutPropertyGrid LayoutPropertyGrip
        {
            get
            {
                return _layoutPropertyGrip;
            }

            set
            {
                if (value == null) return;
                _layoutPropertyGrip = value;
                _layoutPropertyGrip.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets or sets the layout tool strip to use.
        /// </summary>
        public LayoutZoomToolStrip LayoutZoomToolStrip
        {
            get
            {
                return _layoutZoomToolStrip;
            }

            set
            {
                if (value == null) return;
                _layoutZoomToolStrip = value;
                _layoutZoomToolStrip.LayoutControl = this;
            }
        }

        /// <summary>
        /// Gets or sets the Map control to use
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Map MapControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map is in pan mode.
        /// </summary>
        [Browsable(false)]
        public bool MapPanMode
        {
            get
            {
                if (_mouseMode == MouseMode.PanMap || _mouseMode == MouseMode.StartPanMap)
                    return true;
                return false;
            }

            set
            {
                _mouseMode = value ? MouseMode.StartPanMap : MouseMode.Default;
            }
        }

        /// <summary>
        /// Gets or sets the printer settings for the layout.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrinterSettings PrinterSettings
        {
            get
            {
                return _printerSettings;
            }

            set
            {
                _printerSettings = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether margins should be shown.
        /// </summary>
        public bool ShowMargin
        {
            get
            {
                return _showMargin;
            }

            set
            {
                _showMargin = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the zoom of the paper.
        /// </summary>
        public float Zoom
        {
            get
            {
                return _zoom;
            }

            set
            {
                var paperCenter = ScreenToPaper((Width - _vScrollBar.Width - 4) / 2F, (Height - _hScrollBar.Height - 4) / 2F);
                _zoom = value <= 0.1F ? 0.1F : value;
                CenterPaperOnPoint(paperCenter);
                OnZoomChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the list of layoutElements currently selected in the project.
        /// </summary>
        // CGX internal -> public
        public List<LayoutElement> SelectedLayoutElements { get; } = new List<LayoutElement>();

        /// <summary>
        /// Gets the heigh of the paper in 1/100 of an inch.
        /// </summary>
        private int PaperHeight
        {
            get
            {
                if (_printerSettings.DefaultPageSettings.Landscape)
                {
                    return GetPaperWidth();
                }

                return GetPaperHeight();
            }
        }

        /// <summary>
        /// Gets the width of the paper in 1/100 of an inch.
        /// </summary>
        private int PaperWidth
        {
            get
            {
                if (_printerSettings.DefaultPageSettings.Landscape)
                {
                    return GetPaperHeight();
                }

                return GetPaperWidth();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows the user to click on the layout and drag a rectangle where they want to insert an element.
        /// </summary>
        /// <param name="le">Layout element that gets added.</param>
        public void AddElementWithMouse(LayoutElement le)
        {
            _elementToAddWithMouse = le;
            ClearSelection();
            _mouseMode = MouseMode.StartInsertNewElement;
            Cursor = Cursors.Cross;
        }

        // CGX
        public void SetDefaultMouseMode()
        {

            ClearSelection();
            _mouseMode = MouseMode.Default;
            MapPanMode = false;
        }
        // Fin CGX

        /// <summary>
        /// Adds a layout element to the layout.
        /// </summary>
        /// <param name="le">Layout element that gets added.</param>
        public void AddToLayout(LayoutElement le)
        {
            var leName = le.Name + " 1";
            var i = 2;
            while (LayoutElements.FindAll(o => o.Name == leName).Count > 0)
            {
                leName = le.Name + " " + i;
                i++;
            }

            le.Name = leName;

            LayoutElements.Insert(0, le);
            OnElementsChanged(EventArgs.Empty);
            le.Invalidated += LeInvalidated;

            // CGX
            le.Unit = LayoutElement.eSizeUnit.Millimeters; // CGX END
            Invalidate(new Region(PaperToScreen(le.Rectangle)));
        }

        /// <summary>
        /// Aligns elements with each other or with the margins.
        /// </summary>
        /// <param name="elements">A list of elements to align</param>
        /// <param name="side">The side to align to</param>
        /// <param name="margin">True to align to paper margins, false to align to the most extreme element of the indicated side</param>
        public void AlignElements(List<LayoutElement> elements, Alignment side, bool margin)
        {
            switch (side)
            {
                case Alignment.Left:
                    if (margin)
                    {
                        foreach (var le in elements)
                            le.LocationF = new PointF(_printerSettings.DefaultPageSettings.Margins.Left, le.LocationF.Y);
                    }
                    else
                    {
                        var leftMost = float.MaxValue;
                        foreach (var le in elements)
                            if (le.LocationF.X < leftMost) leftMost = le.LocationF.X;
                        foreach (var le in elements)
                            le.LocationF = new PointF(leftMost, le.LocationF.Y);
                    }

                    break;
                case Alignment.Right:
                    if (margin)
                    {
                        float rightMost = PaperWidth - _printerSettings.DefaultPageSettings.Margins.Right;
                        foreach (var le in elements)
                            le.LocationF = new PointF(rightMost - le.Size.Width, le.LocationF.Y);
                    }
                    else
                    {
                        var rightMost = float.MinValue;
                        foreach (var le in elements)
                            if (le.LocationF.X + le.Size.Width > rightMost) rightMost = le.LocationF.X + le.Size.Width;
                        foreach (var le in elements)
                            le.LocationF = new PointF(rightMost - le.Size.Width, le.LocationF.Y);
                    }

                    break;
                case Alignment.Top:
                    if (margin)
                    {
                        foreach (var le in elements)
                            le.LocationF = new PointF(le.LocationF.X, _printerSettings.DefaultPageSettings.Margins.Top);
                    }
                    else
                    {
                        var topMost = float.MaxValue;
                        foreach (var le in elements)
                            if (le.LocationF.Y < topMost) topMost = le.LocationF.Y;
                        foreach (var le in elements)
                            le.LocationF = new PointF(le.LocationF.X, topMost);
                    }

                    break;
                case Alignment.Bottom:
                    if (margin)
                    {
                        float bottomMost = PaperHeight - _printerSettings.DefaultPageSettings.Margins.Bottom;
                        foreach (var le in elements)
                            le.LocationF = new PointF(le.LocationF.X, bottomMost - le.Size.Height);
                    }
                    else
                    {
                        var bottomMost = float.MinValue;
                        foreach (var le in elements)
                            if (le.LocationF.Y + le.Size.Height > bottomMost) bottomMost = le.LocationF.Y + le.Size.Height;
                        foreach (var le in elements)
                            le.LocationF = new PointF(le.LocationF.X, bottomMost - le.Size.Height);
                    }

                    break;

                case Alignment.Horizontal:
                    if (margin)
                    {
                        var centerHor = PaperWidth / 2F;
                        foreach (var le in elements)
                            le.LocationF = new PointF(centerHor - (le.Size.Width / 2F), le.LocationF.Y);
                    }
                    else
                    {
                        float centerHor = 0;
                        float widest = 0;
                        foreach (var le in elements)
                        {
                            if (le.Size.Width > widest)
                            {
                                widest = le.Size.Width;
                                centerHor = le.LocationF.X + (widest / 2F);
                            }
                        }

                        foreach (var le in elements)
                            le.LocationF = new PointF(centerHor - (le.Size.Width / 2F), le.LocationF.Y);
                    }

                    break;
                case Alignment.Vertical:
                    if (margin)
                    {
                        var centerVer = PaperHeight / 2F;
                        foreach (var le in elements)
                            le.LocationF = new PointF(le.LocationF.X, centerVer - (le.Size.Height / 2F));
                    }
                    else
                    {
                        float centerVer = 0;
                        float tallest = 0;
                        foreach (var le in elements)
                        {
                            if (le.Size.Height > tallest)
                            {
                                tallest = le.Size.Height;
                                centerVer = le.LocationF.Y + (tallest / 2F);
                            }
                        }

                        foreach (var le in elements)
                            le.LocationF = new PointF(le.LocationF.X, centerVer - (le.Size.Height / 2F));
                    }

                    break;
            }
        }

        /// <summary>
        /// Prepapres the layoutcontrol for closing, prompts the user to save if needed.
        /// </summary>
        public void CloseLayout()
        {
            if (LayoutElements.Count > 0)
            {
                if (MessageBox.Show(this, MessageStrings.LayoutSaveFirst, MessageStrings.LayoutControl_DotSpatialPrintLayout, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    SaveLayout(true);
            }
        }

        /// <summary>
        /// Converts a selected layout element into a bitmap and saves it a the specified location removing the old element and replacing it.
        /// </summary>
        /// <param name="le">The layout element that gets converted.</param>
        /// <param name="fileName">Name of the file the layout element should be saved to.</param>
        public virtual void ConvertElementToBitmap(LayoutElement le, string fileName)
        {
            if (le is LayoutBitmap) return;
            const float Coeff = 1.0f;
            var temp = new Bitmap(Convert.ToInt32(le.Size.Width * Coeff), Convert.ToInt32(le.Size.Height * Coeff), PixelFormat.Format32bppArgb);
            temp.SetResolution(96.0f, 96.0f);
            temp.MakeTransparent();
            var g = Graphics.FromImage(temp);
            g.PageUnit = GraphicsUnit.Pixel;
            g.ScaleTransform(Coeff, Coeff);
            g.TranslateTransform(-le.LocationF.X, -le.LocationF.Y);
            le.Draw(g, false);
            g.Dispose();
            temp.Save(fileName);
            temp.Dispose();
            var newLb = new LayoutBitmap
            {
                Rectangle = le.Rectangle,
                Name = le.Name,
                Filename = fileName,
                Background = le.Background
            };
            LayoutElements.Insert(LayoutElements.IndexOf(le), newLb);
            LayoutElements.Remove(le);
            SelectedLayoutElements.Insert(SelectedLayoutElements.IndexOf(le), newLb);
            SelectedLayoutElements.Remove(le);
            OnSelectionChanged(EventArgs.Empty);
            Invalidate();
        }

        /// <summary>
        /// Converts all of the selected layout elements to bitmaps
        /// </summary>
        public virtual void ConvertSelectedToBitmap()
        {
            foreach (var le in SelectedLayoutElements.ToArray())
            {
                if (le is LayoutBitmap) continue;

                using (var sfd = new SaveFileDialog
                {
                    FileName = le.Name,
                    Filter = @"Portable Network Graphics (*.png)|*.png|Joint Photographic Experts Group (*.jpg)|*.jpg|Microsoft Bitmap (*.bmp)|*.bmp|Graphics Interchange Format (*.gif)|*.gif|Tagged Image File (*.tif)|*.tif",
                    FilterIndex = 1,
                    AddExtension = true
                })
                {
                    if (sfd.ShowDialog(this) == DialogResult.Cancel)
                        return;

                    ConvertElementToBitmap(le, sfd.FileName);
                }
            }
        }

        /// <summary>
        /// Creates an instance of the LegendElement and returns it.
        /// </summary>
        /// <returns>The created legend.</returns>
        public virtual LayoutLegend CreateLegendElement()
        {
            return new LayoutLegend
            {
                LayoutControl = this,
                Map = LayoutElements.OfType<LayoutMap>().FirstOrDefault(),
            };
        }

        /// <summary>
        /// Creates an instance of the MapElement and returns it.
        /// </summary>
        /// <returns>The created map.</returns>
        public virtual LayoutMap CreateMapElement()
        {
            return new LayoutMap(MapControl);
        }

        /// <summary>
        /// Creates an instance of the ScaleBarElement and returns it.
        /// </summary>
        /// <returns>The created scale bar.</returns>
        public virtual LayoutScaleBar CreateScaleBarElement()
        {
            return new LayoutScaleBar
            {
                LayoutControl = this,
                Map = LayoutElements.OfType<LayoutMap>().FirstOrDefault()
            };
        }

        /// <summary>
        /// Deletes all of the selected elements from the model.
        /// </summary>
        public void DeleteSelected()
        {
            foreach (var le in SelectedLayoutElements.ToArray())
                RemoveFromLayout(le);
            Invalidate();
            OnSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Export layout to bitmap.
        /// </summary>
        /// <param name="fileName">Bitmap file name</param>
        /// <exception cref="ArgumentNullException">Throws when fileName is null.</exception>
        public void ExportToBitmap(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            // This sets up our dimensions
            //   int widthInch = PaperWidth * 100;
            //   int heightInch = PaperHeight * 100;
            //   int PPI = 300; // Set this to what you'd like 300 is good for paper printing, 72 is usually monitors, 96 is apple cuz they have to be different etc....
            //    Bitmap outputBitmap = new Bitmap(widthInch * PPI, heightInch * PPI);
            //     outputBitmap.SetResolution(PPI, PPI);
            //     outputBitmap.graphicsUnit = graphicsUnits.Display;
            using (var outputBitmap = new Bitmap(PaperWidth, PaperHeight))
            using (var g = Graphics.FromImage(outputBitmap))
            {
                var paperRect = new RectangleF(0F, 0F, PaperWidth, PaperHeight);
                g.FillRectangle(Brushes.White, paperRect.X, paperRect.Y, paperRect.Width, paperRect.Height);
                g.DrawRectangle(Pens.Black, paperRect.X, paperRect.Y, paperRect.Width - 1, paperRect.Height - 1);
                DrawPage(g);
                outputBitmap.Save(fileName);
            }
        }

        /// <summary>
        /// Export layout to DotSpatial Layout File.
        /// </summary>
        /// <param name="fileName">DotSpatial Layout File</param>
        /// <exception cref="ArgumentNullException">Throws when fileName is null.</exception>
        public void ExportToMwl(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            // Creates the model xml document
            var layoutXmlDoc = new XmlDocument();
            var root = layoutXmlDoc.CreateElement("DotSpatialLayout");
            layoutXmlDoc.AppendChild(root);

            // Creates a serializer to handle the backgrounds
            var backSerializer = new XmlSerializer();

            // Saves the printer paper settings
            var paperElement = layoutXmlDoc.CreateElement("Paper");
            paperElement.SetAttribute("Name", _printerSettings.DefaultPageSettings.PaperSize.PaperName);
            paperElement.SetAttribute("Landscape", XmlHelper.ToString(_printerSettings.DefaultPageSettings.Landscape));
            paperElement.SetAttribute("Margins", XmlHelper.ToString(_printerSettings.DefaultPageSettings.Margins));
            root.AppendChild(paperElement);

            // Saves the Tools and their output configuration to the model
            foreach (var le in LayoutElements)
            {
                var element = layoutXmlDoc.CreateElement("Element");
                element.SetAttribute("Name", le.Name);
                if (le is LayoutBitmap)
                {
                    var lb = le as LayoutBitmap;
                    var bitmap = layoutXmlDoc.CreateElement("Bitmap");
                    bitmap.SetAttribute("Filename", lb.Filename);
                    if (lb.Bitmap != null)
                    {
                        using (var mStr = new MemoryStream())
                        {
                            lb.Bitmap.Save(mStr, ImageFormat.Png);
                            var bitmapData = Convert.ToBase64String(mStr.ToArray());
                            bitmap.SetAttribute("BitmapData", bitmapData);
                        }
                    }

                    bitmap.SetAttribute("PreserveAspectRatio", lb.PreserveAspectRatio.ToString());
                    bitmap.SetAttribute("Brightness", XmlHelper.ToString(lb.Brightness));
                    bitmap.SetAttribute("Contrast", XmlHelper.ToString(lb.Contrast));
                    element.AppendChild(bitmap);
                }
                else if (le is LayoutLegend)
                {
                    var ll = le as LayoutLegend;
                    var legend = layoutXmlDoc.CreateElement("Legend");
                    legend.SetAttribute("TextHint", ll.TextHint.ToString());
                    legend.SetAttribute("Color", XmlHelper.ToString(ll.Color));
                    legend.SetAttribute("Font", XmlHelper.ToString(ll.Font));
                    legend.SetAttribute("Map", LayoutElements.IndexOf(ll.Map).ToString());
                    var layerString = string.Empty;
                    foreach (var i in ll.Layers)
                        layerString = layerString + XmlHelper.ToString(i) + "|";
                    legend.SetAttribute("Layers", layerString);
                    legend.SetAttribute("NumColumns", XmlHelper.ToString(ll.NumColumns));
                    element.AppendChild(legend);
                }
                else if (le is LayoutMap)
                {
                    var lm = le as LayoutMap;
                    var map = layoutXmlDoc.CreateElement("Map");
                    map.SetAttribute("EnvelopeXmin", XmlHelper.ToString(lm.Envelope.MinX));
                    map.SetAttribute("EnvelopeYmin", XmlHelper.ToString(lm.Envelope.MinY));
                    map.SetAttribute("EnvelopeXmax", XmlHelper.ToString(lm.Envelope.MaxX));
                    map.SetAttribute("EnvelopeYmax", XmlHelper.ToString(lm.Envelope.MaxY));
                    element.AppendChild(map);
                }
                else if (le is LayoutNorthArrow)
                {
                    var na = le as LayoutNorthArrow;
                    var northArrow = layoutXmlDoc.CreateElement("NorthArrow");
                    northArrow.SetAttribute("Color", XmlHelper.ToString(na.Color));
                    northArrow.SetAttribute("Style", na.NorthArrowStyle.ToString());
                    northArrow.SetAttribute("Rotation", XmlHelper.ToString(na.Rotation));
                    element.AppendChild(northArrow);
                }
                else if (le is LayoutRectangle)
                {
                    // is this missing a few SetAttribute commands?
                    // LayoutRectangle lr = le as LayoutRectangle;
                    var rectangle = layoutXmlDoc.CreateElement("Rectangle");
                    element.AppendChild(rectangle);
                }
                else if (le is LayoutScaleBar)
                {
                    var lsc = le as LayoutScaleBar;
                    var scaleBar = layoutXmlDoc.CreateElement("ScaleBar");
                    scaleBar.SetAttribute("TextHint", lsc.TextHint.ToString());
                    scaleBar.SetAttribute("Color", XmlHelper.ToString(lsc.Color));
                    scaleBar.SetAttribute("Font", XmlHelper.ToString(lsc.Font));
                    scaleBar.SetAttribute("BreakBeforeZero", lsc.BreakBeforeZero.ToString());
                    scaleBar.SetAttribute("NumberOfBreaks", lsc.NumberOfBreaks.ToString());
                    scaleBar.SetAttribute("Unit", lsc.Unit.ToString());
                    scaleBar.SetAttribute("UnitText", lsc.UnitText);
                    scaleBar.SetAttribute("Map", LayoutElements.IndexOf(lsc.Map).ToString());
                    element.AppendChild(scaleBar);
                }
                else if (le is LayoutText)
                {
                    var lt = le as LayoutText;
                    var layoutText = layoutXmlDoc.CreateElement("Text");
                    layoutText.SetAttribute("TextHint", lt.TextHint.ToString());
                    layoutText.SetAttribute("Color", XmlHelper.ToString(lt.Color));
                    layoutText.SetAttribute("Font", XmlHelper.ToString(lt.Font));

                    // CGX
                    // layoutText.SetAttribute("ContentAlignment", lt.ContentAlignment.ToString());
                    // CGX END
                    layoutText.SetAttribute("Text", lt.Text);
                    element.AppendChild(layoutText);
                }

                element.SetAttribute("RectangleX", XmlHelper.ToString(le.Rectangle.X));
                element.SetAttribute("RectangleY", XmlHelper.ToString(le.Rectangle.Y));
                element.SetAttribute("RectangleWidth", XmlHelper.ToString(le.Rectangle.Width));
                element.SetAttribute("RectangleHeight", XmlHelper.ToString(le.Rectangle.Height));
                element.SetAttribute("Background", backSerializer.Serialize(le.Background));
                element.SetAttribute("ResizeStyle", le.ResizeStyle.ToString());
                root.AppendChild(element);
            }

            layoutXmlDoc.Save(fileName);
        }

        /// <summary>
        /// Inverts the selection.
        /// </summary>
        public void InvertSelection()
        {
            var unselected = LayoutElements.FindAll(o => !SelectedLayoutElements.Contains(o));
            SelectedLayoutElements.Clear();
            SelectedLayoutElements.InsertRange(0, unselected);
            OnSelectionChanged(EventArgs.Empty);
            Invalidate();
        }

        /// <summary>
        /// Shows a load dialog box and prompts the user to open a layout file.
        /// </summary>
        /// <param name="promptSave">Prompts the user if they want to save first</param>
        /// <param name="loadPaperSettings">Loads the paper settings (size, margins, orientation) from the layout</param>
        /// <param name="promptPaperMismatch">Warn the user if the paper size stored in the file doesn't exist in current printer and ask them if they want to load it anyways</param>
        public void LoadLayout(bool promptSave, bool loadPaperSettings, bool promptPaperMismatch)
        {
            if (LayoutElements.Count > 0 && promptSave)
            {
                var dr = MessageBox.Show(this, MessageStrings.LayoutSaveFirst, MessageStrings.LayoutControl_DotSpatialPrintLayout, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.Yes)
                    SaveLayout(true);
            }

            using (var ofd = new OpenFileDialog
            {
                Title = MessageStrings.LayoutLoadDialogTitle,
                CheckFileExists = true,
                Filter = @"DotSpatial Layout File (*.mwl)|*.mwl",
                Multiselect = false
            })
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        LoadLayout(ofd.FileName, loadPaperSettings, promptPaperMismatch);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(MessageStrings.LayoutErrorLoad + e.Message, MessageStrings.LayoutControl_DotSpatialPrintLayout, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the selected layoutfile.
        /// </summary>
        /// <param name="fileName">The layout file to load</param>
        /// <param name="loadPaperSettings">Loads the paper settings (size, margins, orientation) from the layout</param>
        /// <param name="promptPaperMismatch">Warn the user if the paper size stored in the file doesn't exist in current printer and ask them if they want to load it anyways</param>
        public void LoadLayout(string fileName, bool loadPaperSettings, bool promptPaperMismatch)
        {
            // Open the model xml document
            var layoutXmlDoc = new XmlDocument();
            layoutXmlDoc.Load(fileName);
            var root = layoutXmlDoc.DocumentElement;

            var backDeserializer = new XmlDeserializer();

            // Temporarily stores all the elements and settings until the whole XML file is parsed
            var loadList = new List<LayoutElement>();
            var paperSizeSupported = false;
            PaperSize savedPaperSize = null;
            var savedLandscape = true;
            Margins savedMargins = null;

            // Makes sure we are really loading a DotSpatial layout file
            if (root != null && root.Name == "DotSpatialLayout")
            {
                // This creates instances of all the elements
                var child = root.LastChild;
                while (child != null)
                {
                    if (child.Name == "Element")
                    {
                        LayoutElement newLe = null;
                        switch (child.ChildNodes[0].Name)
                        {
                            case "Bitmap":
                                newLe = new LayoutBitmap();
                                break;
                            case "Legend":
                                newLe = CreateLegendElement();
                                break;
                            case "Map":
                                newLe = CreateMapElement();
                                break;
                            case "NorthArrow":
                                newLe = new LayoutNorthArrow();
                                break;
                            case "Rectangle":
                                newLe = new LayoutRectangle();
                                break;
                            case "ScaleBar":
                                newLe = CreateScaleBarElement();
                                break;
                            case "Text":
                                newLe = new LayoutText();
                                break;
                        }

                        if (newLe != null)
                        {
                            newLe.Name = child.Attributes?["Name"].Value;
                            newLe.Invalidated += LeInvalidated;
                            newLe.Rectangle = new RectangleF(float.Parse(child.Attributes["RectangleX"].Value, CultureInfo.InvariantCulture), float.Parse(child.Attributes["RectangleY"].Value, CultureInfo.InvariantCulture), float.Parse(child.Attributes["RectangleWidth"].Value, CultureInfo.InvariantCulture), float.Parse(child.Attributes["RectangleHeight"].Value, CultureInfo.InvariantCulture));
                            newLe.ResizeStyle = XmlHelper.EnumFromString<ResizeStyle>(child.Attributes["ResizeStyle"].Value);
                            if (child.Attributes["Background"] != null)
                                newLe.Background = backDeserializer.Deserialize<PolygonSymbolizer>(child.Attributes["Background"].Value);
                            loadList.Insert(0, newLe);
                        }
                    }
                    else if (child.Name == "Paper" && loadPaperSettings)
                    {
                        // Loads printer paper size
                        // gets the name of the paper size
                        var paperName = child.Attributes["Name"].Value;

                        // Find out if it supports the paper size in the layout file
                        var availableSizes = _printerSettings.PaperSizes;
                        foreach (PaperSize testSize in availableSizes)
                        {
                            if (testSize.PaperName == paperName)
                            {
                                savedPaperSize = testSize;
                                paperSizeSupported = true;
                                break;
                            }
                        }

                        // If needed prompt the user due to a paper size mismatch
                        if (paperSizeSupported == false && promptPaperMismatch)
                        {
                            var str = string.Format(MessageStrings.LayoutControl_CurrentPrinterDoesNotSupportPapersize, _printerSettings.PrinterName, paperName);
                            if (MessageBox.Show(this, str, MessageStrings.LayoutControl_PaperSizeMismatch, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                return;
                        }
                        else
                        {
                            savedLandscape = XmlHelper.FromString<bool>(child.Attributes["Landscape"].Value);
                            savedMargins = XmlHelper.FromString<Margins>(child.Attributes["Margins"].Value);
                        }
                    }

                    child = child.PreviousSibling;
                }

                // Since some of the elements may be dependant on elements already being added we add their other properties after we add them all
                child = root.LastChild;
                for (var i = loadList.Count - 1; i >= 0; i--)
                {
                    if (child != null)
                    {
                        var innerChild = child.ChildNodes[0];
                        var lb = loadList[i] as LayoutBitmap;
                        if (lb != null)
                        {
                            lb.Filename = innerChild.Attributes["Filename"].Value;
                            if (innerChild.Attributes["BitmapData"] != null)
                            {
                                var ba = Convert.FromBase64String(innerChild.Attributes["BitmapData"].Value);
                                var bitmap = new Bitmap(new MemoryStream(ba));
                                lb.Bitmap = bitmap;
                            }

                            lb.PreserveAspectRatio = Convert.ToBoolean(innerChild.Attributes["PreserveAspectRatio"].Value);
                            if (innerChild.Attributes["Brightness"] != null)
                            {
                                lb.Brightness = XmlHelper.FromString<int>(innerChild.Attributes["Brightness"].Value);
                            }

                            if (innerChild.Attributes["Contrast"] != null)
                            {
                                lb.Contrast = XmlHelper.FromString<int>(innerChild.Attributes["Contrast"].Value);
                            }
                        }
                        else if (loadList[i] is LayoutLegend)
                        {
                            var ll = loadList[i] as LayoutLegend;
                            if (ll != null)
                            {
                                ll.LayoutControl = this;
                                ll.TextHint = XmlHelper.EnumFromString<TextRenderingHint>(innerChild.Attributes["TextHint"].Value);
                                ll.Color = XmlHelper.FromString<Color>(innerChild.Attributes["Color"].Value);
                                ll.Font = XmlHelper.FromString<Font>(innerChild.Attributes["Font"].Value);
                            }

                            var mapIndex = Convert.ToInt32(innerChild.Attributes["Map"].Value);
                            if (mapIndex >= 0)
                                if (ll != null) ll.Map = loadList[mapIndex] as LayoutMap;
                            var layStr = innerChild.Attributes["Layers"].Value;
                            var layers = new List<int>();
                            while (layStr.EndsWith("|"))
                            {
                                layStr = layStr.TrimEnd("|".ToCharArray());
                                layers.Add(XmlHelper.FromString<int>(layStr.Substring(layStr.LastIndexOf("|", StringComparison.Ordinal) + 1)));
                                layStr = layStr.Substring(0, layStr.LastIndexOf("|", StringComparison.Ordinal) + 1);
                            }

                            if (ll != null)
                            {
                                ll.NumColumns = XmlHelper.FromString<int>(innerChild.Attributes["NumColumns"].Value);
                                ll.Layers = layers;
                            }
                        }
                        else if (loadList[i] is LayoutMap)
                        {
                            var lm = loadList[i] as LayoutMap;
                            if (lm != null) lm.Envelope = new Envelope(XmlHelper.FromString<double>(innerChild.Attributes["EnvelopeXmin"].Value), XmlHelper.FromString<double>(innerChild.Attributes["EnvelopeXmax"].Value), XmlHelper.FromString<double>(innerChild.Attributes["EnvelopeYmin"].Value), XmlHelper.FromString<double>(innerChild.Attributes["EnvelopeYmax"].Value));
                        }
                        else if (loadList[i] is LayoutNorthArrow)
                        {
                            var na = loadList[i] as LayoutNorthArrow;
                            if (na != null)
                            {
                                na.Color = XmlHelper.FromString<Color>(innerChild.Attributes["Color"].Value);
                                na.NorthArrowStyle = XmlHelper.EnumFromString<NorthArrowStyle>(innerChild.Attributes["Style"].Value);
                                if (innerChild.Attributes["Rotation"] != null)
                                    na.Rotation = XmlHelper.FromString<float>(innerChild.Attributes["Rotation"].Value);
                            }
                        }
                        else if (loadList[i] is LayoutRectangle)
                        {
                            var lr = loadList[i] as LayoutRectangle;
                            if (lr != null)
                            {
                                // This code is to load legacy layouts that had properties for the color/outline of rectangles
                                if (innerChild.Attributes["Color"] != null && innerChild.Attributes["BackColor"] != null && innerChild.Attributes["OutlineWidth"] != null)
                                {
                                    var tempOutlineColor = XmlHelper.FromString<Color>(innerChild.Attributes["Color"].Value);
                                    var tempBackColor = XmlHelper.FromString<Color>(innerChild.Attributes["BackColor"].Value);
                                    var tempOutlineWidth = XmlHelper.FromString<int>(innerChild.Attributes["OutlineWidth"].Value);
                                    lr.Background = new PolygonSymbolizer(tempBackColor, tempOutlineColor, tempOutlineWidth);
                                }
                            }
                        }
                        else if (loadList[i] is LayoutScaleBar)
                        {
                            var lsc = loadList[i] as LayoutScaleBar;
                            if (lsc != null)
                            {
                                lsc.LayoutControl = this;
                                lsc.TextHint = XmlHelper.EnumFromString<TextRenderingHint>(innerChild.Attributes["TextHint"].Value);
                                lsc.Color = XmlHelper.FromString<Color>(innerChild.Attributes["Color"].Value);
                                lsc.Font = XmlHelper.FromString<Font>(innerChild.Attributes["Font"].Value);
                                lsc.BreakBeforeZero = Convert.ToBoolean(innerChild.Attributes["BreakBeforeZero"].Value);
                                lsc.NumberOfBreaks = XmlHelper.FromString<int>(innerChild.Attributes["NumberOfBreaks"].Value);
                                lsc.Unit = XmlHelper.EnumFromString<ScaleBarUnit>(innerChild.Attributes["Unit"].Value);
                                lsc.UnitText = innerChild.Attributes["UnitText"].Value;
                            }

                            var mapIndex = Convert.ToInt32(innerChild.Attributes["Map"].Value);
                            if (mapIndex >= 0)
                                if (lsc != null) lsc.Map = loadList[mapIndex] as LayoutMap;
                        }
                        else if (loadList[i] is LayoutText)
                        {
                            var lt = loadList[i] as LayoutText;
                            if (lt != null)
                            {
                                lt.TextHint = XmlHelper.EnumFromString<TextRenderingHint>(innerChild.Attributes["TextHint"].Value);
                                lt.Color = XmlHelper.FromString<Color>(innerChild.Attributes["Color"].Value);
                                lt.Font = XmlHelper.FromString<Font>(innerChild.Attributes["Font"].Value);

                                // CGX
                                // lt.ContentAlignment = XmlHelper.FromString<ContentAlignment>(innerChild.Attributes["ContentAlignment"].Value);
                                // CGX END
                                lt.Text = innerChild.Attributes["Text"].Value;
                            }
                        }

                        child = child.PreviousSibling;
                    }
                }

                LayoutElements.Clear();
                SelectedLayoutElements.Clear();
                LayoutElements.InsertRange(0, loadList);

                // Loads the papersize if supported and needed
                if (paperSizeSupported)
                {
                    _printerSettings.DefaultPageSettings.PaperSize = savedPaperSize;
                    _printerSettings.DefaultPageSettings.Landscape = savedLandscape;
                    _printerSettings.DefaultPageSettings.Margins = savedMargins;
                }

                Filename = fileName;
                Invalidate();
                OnSelectionChanged(EventArgs.Empty);
                OnElementsChanged(EventArgs.Empty);
                OnLayoutLoaded(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Makes all of the input layout elements have the same width or height
        /// </summary>
        /// <param name="elements">A list of elements to resize to the max size of all elements or the margins</param>
        /// <param name="axis">Fit the width or the height</param>
        /// <param name="margin">True if use margin size false to use arges element in input list</param>
        public void MatchElementsSize(List<LayoutElement> elements, Fit axis, bool margin)
        {
            if (axis == Fit.Width)
            {
                if (margin)
                {
                    float newWidth = PaperWidth - _printerSettings.DefaultPageSettings.Margins.Left - _printerSettings.DefaultPageSettings.Margins.Right;
                    foreach (var le in elements)
                        le.Size = new SizeF(newWidth, le.Size.Height);
                }
                else
                {
                    float newWidth = 0;
                    foreach (var le in elements)
                        if (le.Size.Width > newWidth) newWidth = le.Size.Width;
                    foreach (var le in elements)
                        le.Size = new SizeF(newWidth, le.Size.Height);
                }
            }
            else
            {
                if (margin)
                {
                    float newHeight = PaperHeight - _printerSettings.DefaultPageSettings.Margins.Top - _printerSettings.DefaultPageSettings.Margins.Bottom;
                    foreach (var le in elements)
                        le.Size = new SizeF(le.Size.Width, newHeight);
                }
                else
                {
                    float newHeight = 0;
                    foreach (var le in elements)
                        if (le.Size.Height > newHeight) newHeight = le.Size.Height;
                    foreach (var le in elements)
                        le.Size = new SizeF(le.Size.Width, newHeight);
                }
            }
        }

        /// <summary>
        /// Moves the selection down by one.
        /// </summary>
        public void MoveSelectionDown()
        {
            // CGX
            /*if (SelectedLayoutElements.Count < 1) return;
            var index = 0;
            var indexArray = new int[SelectedLayoutElements.Count];
            for (var i = 0; i < SelectedLayoutElements.Count; i++)
            {
                indexArray[i] = LayoutElements.IndexOf(SelectedLayoutElements[i]);
                if (index < indexArray[i])
                    index = indexArray[i];
            }

            if (index == LayoutElements.Count - 1) return;

            foreach (LayoutElement le in SelectedLayoutElements)
                LayoutElements.Remove(le);

            for (var i = 0; i < SelectedLayoutElements.Count; i++)
            {
                LayoutElements.Insert(indexArray[i] + 1, SelectedLayoutElements[i]);
            }

            OnSelectionChanged(EventArgs.Empty);
            Invalidate();*/

            if (SelectedLayoutElements.Count > 0)
            {

                List<LayoutElement> leSelected = SelectedLayoutElements.OrderBy(o => LayoutElements.IndexOf(o)).ToList();
                leSelected.Reverse();
                int index = LayoutElements.IndexOf(leSelected[0]);
                if (index == (LayoutElements.Count - 1)) return;

                foreach (LayoutElement le in leSelected)
                {
                    index = LayoutElements.IndexOf(le);
                    LayoutElements.Remove(le);
                    LayoutElements.Insert(index + 1, le);
                }

                OnSelectionChanged(EventArgs.Empty);
                Invalidate();
            }// FIN CGX

        }

        /// <summary>
        /// Moves the selection up by one.
        /// </summary>
        public void MoveSelectionUp()
        {
            // CGX
            /*if (SelectedLayoutElements.Count < 1) return;
            var index = LayoutElements.Count - 1;
            foreach (var le in SelectedLayoutElements)
            {
                if (index > LayoutElements.IndexOf(le))
                    index = LayoutElements.IndexOf(le);
            }

            if (index == 0) return;

            foreach (var le in SelectedLayoutElements)
            {
                index = LayoutElements.IndexOf(le);
                LayoutElements.Remove(le);
                LayoutElements.Insert(index - 1, le);
            }

            OnSelectionChanged(EventArgs.Empty);
            Invalidate();*/
            
            if (SelectedLayoutElements.Count > 0)
            {

                List<LayoutElement> leSelected = SelectedLayoutElements.OrderBy(o => LayoutElements.IndexOf(o)).ToList();
                int index = LayoutElements.IndexOf(leSelected[0]);
                if (index == 0) return;

                foreach (LayoutElement le in leSelected)
                {
                    index = LayoutElements.IndexOf(le);
                    LayoutElements.Remove(le);
                    LayoutElements.Insert(index - 1, le);
                }

                OnSelectionChanged(EventArgs.Empty);
                Invalidate();
            }// Fin CGX

        }

        /// <summary>
        /// Creates a new blank layout
        /// </summary>
        /// <param name="promptSave">Prompts the user if they want to save first</param>
        public void NewLayout(bool promptSave)
        {
            if (LayoutElements.Count > 0 && promptSave)
            {
                var dr = MessageBox.Show(this, MessageStrings.LayoutSaveFirst, MessageStrings.LayoutControl_DotSpatialPrintLayout, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.Yes)
                    SaveLayout(true);
            }

            ClearLayout();
            Filename = string.Empty;
        }

        /// <summary>
        /// Pans the map the specified amount
        /// </summary>
        /// <param name="lm">the layout map to pan</param>
        /// <param name="x">The distance to pan the map on x-axis in screen coord</param>
        /// <param name="y">The distance to pan the map on y-axis in screen coord</param>
        public void PanMap(LayoutMap lm, float x, float y)
        {
            // CGX
            if (lm != null)
            {
                var mapOnScreen = PaperToScreen(lm.Rectangle);
                lm.PanMap((lm.Envelope.Width / mapOnScreen.Width) * x, (lm.Envelope.Height / mapOnScreen.Height) * -y);
            }
        }

        /// <summary>
        /// Prints to the printer currently in PrinterSettings
        /// </summary>
        public void Print()
        {
            var pd = new PrintDocument
            {
                PrinterSettings = _printerSettings
            };
            pd.PrintPage += PrintPage;
            pd.Print();
        }

        /// <summary>
        /// Refreshes all of the elements in the layout
        /// </summary>
        public void RefreshElements()
        {
            _suppressLEinvalidate = true;
            foreach (var le in LayoutElements)
                le.RefreshElement();
            _suppressLEinvalidate = false;
            Invalidate();
        }

        /// <summary>
        /// Shows a save dialog box and prompts the user to save a layout file.
        /// </summary>
        /// <param name="promptSaveAs">Show prompt dialog or not. Note that dialog will be always shown when Filename is null or empty.</param>
        public void SaveLayout(bool promptSaveAs)
        {
            if (string.IsNullOrEmpty(Filename) || promptSaveAs)
            {
                var sfd = new SaveFileDialog
                {
                    Title = MessageStrings.LayoutSaveDialogTitle,
                    Filter = @"DotSpatial Layout File (*.mwl)|*.mwl|Portable Network Graphics (*.png)|*.png",
                    AddExtension = true,
                    OverwritePrompt = true
                };
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    SaveLayout(sfd.FileName);
                }
            }
            else if (!string.IsNullOrEmpty(Filename))
            {
                SaveLayout(Filename);
            }
        }

        /// <summary>
        /// Saves the layout to the specified fileName.
        /// If file name has .mwl extension then it will be saved as DotSpatial Layout File.
        /// Otherwise it will be trying to save as Bitmap.
        /// </summary>
        /// <param name="fileName">Specified file name</param>
        /// <exception cref="ArgumentNullException">Throws when fileName is null.</exception>
        public void SaveLayout(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            try
            {
                switch (Path.GetExtension(fileName))
                {
                    case ".mwl":
                        ExportToMwl(fileName);
                        Filename = fileName;
                        break;
                    default:
                        ExportToBitmap(fileName);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(MessageStrings.LayoutErrorSave + @" " + e.Message, MessageStrings.LayoutControl_DotSpatialPrintLayout, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Selects All the elements in the layout.
        /// </summary>
        public void SelectAll()
        {
            SelectedLayoutElements.Clear();
            SelectedLayoutElements.InsertRange(0, LayoutElements);
            OnSelectionChanged(EventArgs.Empty);
            Invalidate();
        }

        /// <summary>
        /// This shows the choose printer dialog.
        /// </summary>
        public void ShowChoosePrinterDialog()
        {
            using (var pd = new PrintDialog())
            {
                pd.PrinterSettings = _printerSettings;
                pd.ShowDialog();
                Invalidate();
            }
        }

        /// <summary>
        /// This shows the pageSetup dialog.
        /// </summary>
        public void ShowPageSetupDialog()
        {
            if (_printerSettings.IsValid)
            {
                using (var setupFrom = new PageSetupForm(_printerSettings))
                {
                    setupFrom.ShowDialog(this);
                    Invalidate();
                }
            }
            else
            {
                MessageBox.Show(MessageStrings.LayoutControl_NoPrinterFound);
            }
        }

        /// <summary>
        /// Zooms the page to fit to the screen and centers it.
        /// </summary>
        public void ZoomFitToScreen()
        {
            var xZoom = (Width - 50) / (PaperWidth * 96F / 100F);
            var yZoom = (Height - 50) / (PaperHeight * 96F / 100F);
            _zoom = xZoom < yZoom ? xZoom : yZoom;
            CenterPaperOnPoint(new PointF(PaperWidth / 2F, PaperHeight / 2F));
            OnZoomChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Zooms all map elements to the full extent of their layers.
        /// </summary>
        public void ZoomFullExtentMap()
        {
            foreach (var mapCtrl in LayoutElements.OfType<LayoutMap>())
                mapCtrl.ZoomToFullExtent();
        }

        /// <summary>
        /// Zoom all maps to the extent of their data view.
        /// </summary>
        public void ZoomFullViewExtentMap()
        {
            foreach (var mapCtrl in LayoutElements.OfType<LayoutMap>())
                mapCtrl.ZoomViewExtent();
        }

        /// <summary>
        /// Zooms into the paper
        /// </summary>
        public void ZoomIn()
        {
            Zoom = Zoom + 0.1F;
        }

        /// <summary>
        /// Zooms all map elements in by 10%.
        /// </summary>
        public void ZoomInMap()
        {
            foreach (var mapCtrl in LayoutElements.OfType<LayoutMap>())
                mapCtrl.ZoomInMap();
        }

        /// <summary>
        /// Zooms out of the paper.
        /// </summary>
        public void ZoomOut()
        {
            Zoom = Zoom - 0.1F;
        }

        /// <summary>
        /// Zooms all map element out by 10%.
        /// </summary>
        public void ZoomOutMap()
        {
            foreach (var mapCtrl in LayoutElements.OfType<LayoutMap>())
                mapCtrl.ZoomOutMap();
        }

        /// <summary>
        /// Adds the specified LayoutElement le to the selection.
        /// </summary>
        /// <param name="le">The layout element.</param>
        // CGX internal -> public
        public void AddToSelection(List<LayoutElement> le)
        {
            SelectedLayoutElements.AddRange(le);
            Invalidate();
            OnSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Clears the current selection.
        /// </summary>
        // CGX internal -> public
        public void ClearSelection()
        {
            SelectedLayoutElements.Clear();
            Invalidate();
            OnSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Adds the specified LayoutElement le to the selection.
        /// </summary>
        /// <param name="le">The layout element.</param>
        internal void AddToSelection(LayoutElement le)
        {
            SelectedLayoutElements.Add(le);
            Invalidate(new Region(PaperToScreen(le.Rectangle)));
            OnSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Clears the the layout of all layoutElements.
        /// </summary>
        internal void ClearLayout()
        {
            SelectedLayoutElements.Clear();
            OnSelectionChanged(EventArgs.Empty);
            LayoutElements.Clear();
            OnElementsChanged(EventArgs.Empty);
            Invalidate();
        }

        /// <summary>
        /// Removes the specified layoutElement from the layout.
        /// </summary>
        /// <param name="le">The layout element.</param>
        internal void RemoveFromLayout(LayoutElement le)
        {
            SelectedLayoutElements.Remove(le);
            OnSelectionChanged(EventArgs.Empty);
            LayoutElements.Remove(le);
            OnElementsChanged(EventArgs.Empty);
            Invalidate(new Region(PaperToScreen(le.Rectangle)));
        }

        /// <summary>
        /// Removes the specified layoutElement from the selection
        /// </summary>
        /// <param name="le">The layout element.</param>
        internal void RemoveFromSelection(LayoutElement le)
        {
            SelectedLayoutElements.Remove(le);
            Invalidate(new Region(PaperToScreen(le.Rectangle)));
            OnSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Drawing code
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Deal with invalidate rectangles that have a size of 0
            if ((e.ClipRectangle.Width <= 0) || (e.ClipRectangle.Height <= 0)) return;

            // Store the cursor so we can show an hour glass while drawing
            var oldCursor = Cursor;

            // Updates the invalidation rectangle to be a bit bigger to deal with overlaps
            var invalRect = Rectangle.Inflate(e.ClipRectangle, 5, 5);
            if (invalRect.X < 0) invalRect.X = 0;
            if (invalRect.Y < 0) invalRect.Y = 0;

            // We paint to a temporary buffer to avoid flickering
            var tempBuffer = new Bitmap(invalRect.Width, invalRect.Height, PixelFormat.Format24bppRgb);
            var graph = Graphics.FromImage(tempBuffer);
            graph.TranslateTransform(-invalRect.X, -invalRect.Y);
            graph.SmoothingMode = DrawingQuality;

            // Fills the background with dark grey
            graph.FillRectangle(Brushes.DarkGray, invalRect);

            // This code draws the paper
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif

            var paperRect = PaperToScreen(0F, 0F, PaperWidth, PaperHeight);
            graph.FillRectangle(Brushes.White, paperRect.X, paperRect.Y, paperRect.Width, paperRect.Height);
            graph.DrawRectangle(Pens.Black, paperRect.X, paperRect.Y, paperRect.Width, paperRect.Height);
            if (_showMargin)
            {
                paperRect = PaperToScreen(_printerSettings.DefaultPageSettings.Margins.Left, _printerSettings.DefaultPageSettings.Margins.Top, PaperWidth - _printerSettings.DefaultPageSettings.Margins.Left - _printerSettings.DefaultPageSettings.Margins.Right, PaperHeight - _printerSettings.DefaultPageSettings.Margins.Top - _printerSettings.DefaultPageSettings.Margins.Bottom);
                graph.DrawRectangle(Pens.LightGray, paperRect.X, paperRect.Y, paperRect.Width, paperRect.Height);
            }

#if DEBUG
            sw.Stop();
            Debug.WriteLine("Time to draw paper: " + sw.ElapsedMilliseconds);
#endif

            // Draws the layout elements
            for (var i = LayoutElements.Count - 1; i >= 0; i--)
            {
                var le = LayoutElements[i];

                // This code deals with drawins a map when its panning
                if (_mouseMode == MouseMode.PanMap && SelectedLayoutElements.Contains(le) && le is LayoutMap && SelectedLayoutElements.Count == 1)
                {
                    graph.TranslateTransform(_paperLocation.X + _mouseBox.Width, _paperLocation.Y + _mouseBox.Height);
                    graph.ScaleTransform(96F / 100F * _zoom, 96F / 100F * _zoom);
                    le.DrawBackground(graph, false);
                    le.Draw(graph, false);
                    le.DrawOutline(graph, false);
                    graph.ResetTransform();
                    graph.TranslateTransform(-invalRect.X, -invalRect.Y);
                }
                else
                {
                    // This code draws the selected elements
                    // Draws the background
                    graph.TranslateTransform(_paperLocation.X, _paperLocation.Y);
                    graph.ScaleTransform(96F / 100F * _zoom, 96F / 100F * _zoom);
                    le.DrawBackground(graph, false);
                    graph.ResetTransform();
                    graph.TranslateTransform(-invalRect.X, -invalRect.Y);

                    // If we've got a selection and we're resizing
                    if (SelectedLayoutElements.Contains(LayoutElements[i]) && _resizeTempBitmap != null)
                    {
                        var papRect = PaperToScreen(SelectedLayoutElements[0].Rectangle);
                        var clipRect = new Rectangle(Convert.ToInt32(papRect.X), Convert.ToInt32(papRect.Y), Convert.ToInt32(papRect.Width), Convert.ToInt32(papRect.Height));

                        // If its stretch to fit just scale it
                        if (SelectedLayoutElements[0].ResizeStyle == ResizeStyle.StretchToFit)
                            graph.DrawImage(_resizeTempBitmap, clipRect);

                        // If there is no scaling we just draw it with a clipping rectangle
                        else if (SelectedLayoutElements[0].ResizeStyle == ResizeStyle.NoScaling)
                            graph.DrawImageUnscaled(_resizeTempBitmap, clipRect);
                    }
                    else
                    {
                        graph.TranslateTransform(_paperLocation.X, _paperLocation.Y);
                        graph.ScaleTransform(96F / 100F * _zoom, 96F / 100F * _zoom);
                        le.Draw(graph, false);
                        graph.ResetTransform();
                        graph.TranslateTransform(-invalRect.X, -invalRect.Y);
                    }

                    // Draws the outline last
                    graph.TranslateTransform(_paperLocation.X, _paperLocation.Y);
                    graph.ScaleTransform(96F / 100F * _zoom, 96F / 100F * _zoom);
                    le.DrawOutline(graph, false);
                    graph.ResetTransform();
                    graph.TranslateTransform(-invalRect.X, -invalRect.Y);
                }
            }

            // Draws the selection rectangle around each selected item
            // var selectionPen = new Pen(Color.Black, 1F);
            // CGX
            Pen selectionPen = new Pen(Color.OrangeRed, 2.5F);
            selectionPen.DashPattern = new[] { 2.0F, 1.0F };
            selectionPen.DashCap = DashCap.Round;
            foreach (var layoutEl in SelectedLayoutElements)
            {
                var leRect = PaperToScreen(layoutEl.Rectangle);
                graph.DrawRectangle(selectionPen, Convert.ToInt32(leRect.X), Convert.ToInt32(leRect.Y), Convert.ToInt32(leRect.Width), Convert.ToInt32(leRect.Height));
            }

            // If the users is dragging a select box or an insert box we draw it here
            if (_mouseMode == MouseMode.CreateSelection || _mouseMode == MouseMode.InsertNewElement)
            {
                Color boxColor = _mouseMode == MouseMode.CreateSelection ? SystemColors.Highlight : Color.Orange;
                var outlinePen = new Pen(boxColor);
                var highlightBrush = new SolidBrush(Color.FromArgb(30, boxColor));

                // CGX
                if (_mouseMode == MouseMode.CreateSelection)
                    graph.FillRectangle(highlightBrush, _mouseBox.X, _mouseBox.Y, _mouseBox.Width - 1, _mouseBox.Height - 1);
                graph.DrawRectangle(outlinePen, _mouseBox.X, _mouseBox.Y, _mouseBox.Width - 1, _mouseBox.Height - 1);

                // garbage collection
                highlightBrush.Dispose();
                outlinePen.Dispose();
            }

            // Draws the temporary bitmap to the screen
            e.Graphics.SmoothingMode = DrawingQuality;
            e.Graphics.DrawImage(tempBuffer, invalRect, new RectangleF(0f, 0f, invalRect.Width, invalRect.Height), GraphicsUnit.Pixel);

            // Garbage collection
            tempBuffer.Dispose();
            graph.Dispose();

            // resets the cursor cuz some times it get jammed
            Cursor = oldCursor;
        }

        /// <summary>
        /// Prevents flicker from any default on paint background operations.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Calculates which edge of a rectangle the point intersects with, within a certain limit.
        /// </summary>
        /// <param name="screen">Rectangle to check for intersection.</param>
        /// <param name="pt">Point to check for intersection.</param>
        /// <param name="limit">Limit needed for check.</param>
        /// <returns>The edge the point intersects with.</returns>
        private static Edge IntersectElementEdge(RectangleF screen, PointF pt, float limit)
        {
            var ptRect = new RectangleF(pt.X - limit, pt.Y - limit, 2F * limit, 2F * limit);
            if ((pt.X >= screen.X - limit && pt.X <= screen.X + limit) && (pt.Y >= screen.Y - limit && pt.Y <= screen.Y + limit))
                return Edge.TopLeft;
            if ((pt.X >= screen.X + screen.Width - limit && pt.X <= screen.X + screen.Width + limit) && (pt.Y >= screen.Y - limit && pt.Y <= screen.Y + limit))
                return Edge.TopRight;
            if ((pt.X >= screen.X + screen.Width - limit && pt.X <= screen.X + screen.Width + limit) && (pt.Y >= screen.Y + screen.Height - limit && pt.Y <= screen.Y + screen.Height + limit))
                return Edge.BottomRight;
            if ((pt.X >= screen.X - limit && pt.X <= screen.X + limit) && (pt.Y >= screen.Y + screen.Height - limit && pt.Y <= screen.Y + screen.Height + limit))
                return Edge.BottomLeft;
            if (ptRect.IntersectsWith(new RectangleF(screen.X, screen.Y, screen.Width, 1F)))
                return Edge.Top;
            if (ptRect.IntersectsWith(new RectangleF(screen.X, screen.Y, 1F, screen.Height)))
                return Edge.Left;
            if (ptRect.IntersectsWith(new RectangleF(screen.X, screen.Y + screen.Height, screen.Width, 1F)))
                return Edge.Bottom;
            if (ptRect.IntersectsWith(new RectangleF(screen.X + screen.Width, screen.Y, 1F, screen.Height)))
                return Edge.Right;
            return Edge.None;
        }

        /// <summary>
        /// Centers the layout on a given point.
        /// </summary>
        /// <param name="centerPoint">A Point on the paper to center on</param>
        private void CenterPaperOnPoint(PointF centerPoint)
        {
            var paperCenterOnScreen = PaperToScreen(centerPoint);
            var diffX = paperCenterOnScreen.X - ((Width - _vScrollBar.Width - 4) / 2F);
            var diffY = paperCenterOnScreen.Y - ((Height - _hScrollBar.Height - 4) / 2F);

            _paperLocation.X = _paperLocation.X - diffX;
            _paperLocation.Y = _paperLocation.Y - diffY;

            UpdateScrollBars();
            Invalidate();
        }

        private void CMnuDeleteClick(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void CMnuMargBottomClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Bottom, true);
        }

        private void CMnuMargHeightClick(object sender, EventArgs e)
        {
            MatchElementsSize(SelectedLayoutElements, Fit.Height, true);
            AlignElements(SelectedLayoutElements, Alignment.Vertical, true);
        }

        private void CMnuMargHorClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Horizontal, true);
        }

        private void CMnuMargLeftClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Left, true);
        }

        private void CMnuMargRightClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Right, true);
        }

        private void CMnuMargTopClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Top, true);
        }

        private void CMnuMargVertClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Vertical, true);
        }

        private void CMnuMargWidthClick(object sender, EventArgs e)
        {
            MatchElementsSize(SelectedLayoutElements, Fit.Width, true);
            AlignElements(SelectedLayoutElements, Alignment.Horizontal, true);
        }

        private void CMnuMoveDownClick(object sender, EventArgs e)
        {
            MoveSelectionDown();
        }

        private void CMnuMoveUpClick(object sender, EventArgs e)
        {
            MoveSelectionUp();
        }

        private void CMnuSelBottomClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Bottom, false);
        }

        private void CMnuSelHeightClick(object sender, EventArgs e)
        {
            MatchElementsSize(SelectedLayoutElements, Fit.Height, false);
            AlignElements(SelectedLayoutElements, Alignment.Vertical, false);
        }

        private void CMnuSelHorClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Horizontal, false);
        }

        private void CMnuSelLeftClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Left, false);
        }

        private void CMnuSelRightClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Right, false);
        }

        private void CMnuSelTopClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Top, false);
        }

        private void CMnuSelVertClick(object sender, EventArgs e)
        {
            AlignElements(SelectedLayoutElements, Alignment.Vertical, false);
        }

        private void CMnuSelWidthClick(object sender, EventArgs e)
        {
            MatchElementsSize(SelectedLayoutElements, Fit.Width, false);
            AlignElements(SelectedLayoutElements, Alignment.Horizontal, false);
        }

        /// <summary>
        /// Draws page to layout.
        /// </summary>
        /// <param name="g">Graphics object needed for drawing.</param>
        private void DrawPage(Graphics g)
        {
            for (var i = LayoutElements.Count - 1; i >= 0; i--)
            {
                var le = LayoutElements[i];
                var lt = le as LayoutText;
                if (lt != null)
                {
                    // Text was incorrectly losing final letters.
                    var r = le.Rectangle;
                    var lettersize = g.MeasureString("0", lt.Font);
                    r.Width += lettersize.Width;
                    g.Clip = new Region(r);
                }
                else
                {
                    g.Clip = new Region(le.Rectangle);
                }

                le.DrawBackground(g, true);
                le.Draw(g, true);
                le.DrawOutline(g, true);
            }
        }

        private int GetPaperHeight()
        {
            try
            {
                return _printerSettings.DefaultPageSettings.PaperSize.Height;
            }
            catch (InvalidPrinterException)
            {
                return 1100;
            }
        }

        private int GetPaperWidth()
        {
            try
            {
                return _printerSettings.DefaultPageSettings.PaperSize.Width;
            }
            catch (InvalidPrinterException)
            {
                return 850;
            }
        }

        /// <summary>
        /// This fires when the hscrollbar is moved.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void HScrollBarScroll(object sender, ScrollEventArgs e)
        {
            _paperLocation.X = _paperLocation.X + (e.OldValue - e.NewValue);
            Invalidate();
        }

        /// <summary>
        /// This allows elements to be refreshed, deleted by key press.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteSelected();
                    break;
                case Keys.F5:
                    RefreshElements();
                    break;
            }
        }

        private void LayoutControlMouseDown(object sender, MouseEventArgs e)
        {
            // When the user clicks down we start tracking the mouses location
            _mouseStartPoint = new PointF(e.X, e.Y);
            _lastMousePoint = new PointF(e.X, e.Y);
            var mousePointPaper = ScreenToPaper(_mouseStartPoint);

            // Deals with left buttons clicks
            if (e.Button == MouseButtons.Left)
            {
                switch (_mouseMode)
                {
                    case MouseMode.Default:

                        // Handles resizing stuff
                        if (_resizeSelectedEdge != Edge.None)
                        {
                            _mouseMode = MouseMode.ResizeSelected;
                            SelectedLayoutElements[0].Resizing = true;
                            if (SelectedLayoutElements[0].ResizeStyle != ResizeStyle.HandledInternally)
                            {
                                var selecteScreenRect = PaperToScreen(SelectedLayoutElements[0].Rectangle);
                                _resizeTempBitmap = new Bitmap(Convert.ToInt32(selecteScreenRect.Width), Convert.ToInt32(selecteScreenRect.Height), PixelFormat.Format32bppArgb);
                                var graph = Graphics.FromImage(_resizeTempBitmap);
                                graph.SmoothingMode = DrawingQuality;
                                graph.ScaleTransform(96F / 100F * _zoom, 96F / 100F * _zoom);
                                graph.TranslateTransform(-SelectedLayoutElements[0].Rectangle.X, -SelectedLayoutElements[0].Rectangle.Y);
                                SelectedLayoutElements[0].Draw(graph, false);
                                graph.Dispose();
                            }

                            return;
                        }

                        // Starts moving selected elements
                        if (ModifierKeys != Keys.Control)
                        {
                            foreach (var le in SelectedLayoutElements)
                            {
                                if (!le.IntersectsWith(mousePointPaper)) continue;
                                _mouseMode = MouseMode.MoveSelection;
                                Cursor = Cursors.SizeAll;

                                // CGX
                                // ShowHideLayoutElement("IACProfile", false);
                                return;
                            }
                        }

                        // Starts the selection code.
                        _mouseMode = MouseMode.CreateSelection;
                        _mouseBox = new RectangleF(e.X, e.Y, 0F, 0F);
                        break;

                    // Start drag rectangle insert new element
                    case MouseMode.StartInsertNewElement:
                        _mouseMode = MouseMode.InsertNewElement;
                        _mouseBox = new RectangleF(e.X, e.Y, 0F, 0F);
                        break;

                    // Starts the pan mode for the map
                    case MouseMode.StartPanMap:
                        _mouseMode = MouseMode.PanMap;
                        _mouseBox = new RectangleF(e.X, e.Y, 0F, 0F);
                        break;
                }
            }

            // Deals with right button clicks
            if (e.Button == MouseButtons.Right)
            {
                switch (_mouseMode)
                {
                    // If the user was in insert mode we cancel it
                    case MouseMode.StartInsertNewElement:
                        _mouseMode = MouseMode.Default;
                        _elementToAddWithMouse = null;
                        Cursor = Cursors.Default;
                        break;
                }
            }
        }

        private void LayoutControlMouseMove(object sender, MouseEventArgs e)
        {
            // The amount the mouse moved since the last time
            var deltaX = _lastMousePoint.X - e.X;
            var deltaY = _lastMousePoint.Y - e.Y;
            _lastMousePoint = e.Location;
            var inflate = 5F;

            // Handles various different mouse modes
            switch (_mouseMode)
            {
                // Deals with inserting new elements
                case MouseMode.InsertNewElement:

                // Deals with creating a selections
                case MouseMode.CreateSelection:
                    Invalidate(new Region(_mouseBox));
                    _mouseBox.Width = Math.Abs(_mouseStartPoint.X - e.X);
                    _mouseBox.Height = Math.Abs(_mouseStartPoint.Y - e.Y);
                    _mouseBox.X = Math.Min(_mouseStartPoint.X, e.X);
                    _mouseBox.Y = Math.Min(_mouseStartPoint.Y, e.Y);
                    Invalidate(new Region(_mouseBox));
                    break;

                // Deals with moving the selection
                case MouseMode.MoveSelection:
                    _suppressLEinvalidate = true;
                    foreach (var le in SelectedLayoutElements)
                    {
                        if (le.Background?.OutlineSymbolizer != null)
                            inflate = inflate + ((float)le.Background.GetOutlineWidth() * _zoom);
                        var invalRect = PaperToScreen(le.Rectangle);
                        invalRect.Inflate(inflate, inflate);
                        Invalidate(new Region(invalRect));
                        var elementLocScreen = PaperToScreen(le.LocationF);
                        le.LocationF = ScreenToPaper(elementLocScreen.X - deltaX, elementLocScreen.Y - deltaY);
                        invalRect = PaperToScreen(le.Rectangle);
                        invalRect.Inflate(inflate, inflate);
                        Invalidate(new Region(invalRect));

                        // CGX
                        if (!le.Visible) continue;

                        Update();
                    }

                    _suppressLEinvalidate = false;
                    break;

                // This handle mouse movement when in resize mode
                case MouseMode.ResizeSelected:

                    // Makes sure that we have a outline to use it to inflate the invalidation rectangle
                    if (SelectedLayoutElements[0].Background?.OutlineSymbolizer != null)
                        inflate = inflate + ((float)SelectedLayoutElements[0].Background.GetOutlineWidth() * _zoom);

                    _suppressLEinvalidate = true;
                    var oldScreenRect = PaperToScreen(SelectedLayoutElements[0].Rectangle);
                    oldScreenRect.Inflate(inflate, inflate);
                    Invalidate(new Region(oldScreenRect));
                    oldScreenRect = PaperToScreen(SelectedLayoutElements[0].Rectangle);
                    switch (_resizeSelectedEdge)
                    {
                        case Edge.TopLeft:
                            oldScreenRect.X = oldScreenRect.X - deltaX;
                            oldScreenRect.Y = oldScreenRect.Y - deltaY;
                            oldScreenRect.Width = oldScreenRect.Width + deltaX;
                            oldScreenRect.Height = oldScreenRect.Height + deltaY;
                            break;
                        case Edge.Top:
                            oldScreenRect.Y = oldScreenRect.Y - deltaY;
                            oldScreenRect.Height = oldScreenRect.Height + deltaY;
                            break;
                        case Edge.TopRight:
                            oldScreenRect.Y = oldScreenRect.Y - deltaY;
                            oldScreenRect.Height = oldScreenRect.Height + deltaY;
                            oldScreenRect.Width = oldScreenRect.Width - deltaX;
                            break;
                        case Edge.Right:
                            oldScreenRect.Width = oldScreenRect.Width - deltaX;
                            break;
                        case Edge.BottomRight:
                            oldScreenRect.Width = oldScreenRect.Width - deltaX;
                            oldScreenRect.Height = oldScreenRect.Height - deltaY;
                            break;
                        case Edge.Bottom:
                            oldScreenRect.Height = oldScreenRect.Height - deltaY;
                            break;
                        case Edge.BottomLeft:
                            oldScreenRect.X = oldScreenRect.X - deltaX;
                            oldScreenRect.Width = oldScreenRect.Width + deltaX;
                            oldScreenRect.Height = oldScreenRect.Height - deltaY;
                            break;
                        case Edge.Left:
                            oldScreenRect.X = oldScreenRect.X - deltaX;
                            oldScreenRect.Width = oldScreenRect.Width + deltaX;
                            break;
                    }

                    SelectedLayoutElements[0].Rectangle = ScreenToPaper(oldScreenRect);
                    oldScreenRect.Inflate(inflate, inflate);
                    Invalidate(new Region(oldScreenRect));
                    Update();
                    _suppressLEinvalidate = false;

                    break;

                case MouseMode.StartPanMap:
                    if (SelectedLayoutElements.Count == 1 && SelectedLayoutElements[0] is LayoutMap)
                    {
                        if (SelectedLayoutElements[0].IntersectsWith(ScreenToPaper(e.X * 1F, e.Y * 1F)))
                            Cursor = new Cursor(Images.Pan.Handle);
                        else
                            Cursor = Cursors.Default;
                    }

                    break;

                case MouseMode.PanMap:
                    // CGX
                    if (SelectedLayoutElements.Count == 1 && SelectedLayoutElements[0] is LayoutMap)
                    {
                        _mouseBox.Width = e.X - _mouseStartPoint.X;
                        _mouseBox.Height = e.Y - _mouseStartPoint.Y;
                        Invalidate(new Region(PaperToScreen(SelectedLayoutElements[0].Rectangle)));
                    }
                    // Fin CGX
                    break;

                case MouseMode.Default:

                    // If theres only one element selected and were on its edge change the cursor to the resize cursor
                    if (SelectedLayoutElements.Count == 1)
                    {
                        _resizeSelectedEdge = IntersectElementEdge(PaperToScreen(SelectedLayoutElements[0].Rectangle), new PointF(e.X, e.Y), 3F);
                        switch (_resizeSelectedEdge)
                        {
                            case Edge.TopLeft:
                            case Edge.BottomRight:
                                Cursor = Cursors.SizeNWSE;
                                break;
                            case Edge.Top:
                            case Edge.Bottom:
                                Cursor = Cursors.SizeNS;
                                break;
                            case Edge.TopRight:
                            case Edge.BottomLeft:
                                Cursor = Cursors.SizeNESW;
                                break;
                            case Edge.Left:
                            case Edge.Right:
                                Cursor = Cursors.SizeWE;
                                break;
                            case Edge.None:
                                Cursor = Cursors.Default;
                                break;
                        }
                    }
                    else
                    {
                        _resizeSelectedEdge = Edge.None;
                        Cursor = Cursors.Default;
                    }

                    break;
            }
        }

        // CGX
        void ShowHideLayoutElement(string Name, bool Visible)
        {
            if (_mouseMode == MouseMode.MoveSelection)
            {
                foreach (LayoutElement le in SelectedLayoutElements)
                {
                    if (!string.IsNullOrEmpty(le.Group))
                    {
                        le.Visible = Visible;
                    }
                }
            }
        }
        // CGX END

        private void LayoutControlMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Handles various different mouse modes
                switch (_mouseMode)
                {
                    // If we are dealing with a selection we look here
                    case MouseMode.CreateSelection:
                        var selectBoxTl = ScreenToPaper(_mouseBox.Location);
                        var selectBoxBr = ScreenToPaper(_mouseBox.Location.X + _mouseBox.Width, _mouseBox.Location.Y + _mouseBox.Height);
                        var selectBoxPaper = new RectangleF(selectBoxTl.X, selectBoxTl.Y, selectBoxBr.X - selectBoxTl.X, selectBoxBr.Y - selectBoxTl.Y);

                        if (ModifierKeys == Keys.Control)
                        {
                            foreach (var le in LayoutElements)
                            {
                                if (le.IntersectsWith(selectBoxPaper))
                                {
                                    if (SelectedLayoutElements.Contains(le))
                                        SelectedLayoutElements.Remove(le);
                                    else
                                        SelectedLayoutElements.Add(le);

                                    // If the box is just a point only select the top most
                                    if (_mouseBox.Width <= 1 && _mouseBox.Height <= 1)
                                        break;
                                }
                            }
                        }
                        else
                        {
                            SelectedLayoutElements.Clear();
                            foreach (var le in LayoutElements)
                            {
                                if (le.IntersectsWith(selectBoxPaper))
                                {
                                    SelectedLayoutElements.Add(le);

                                    // If the box is just a point only select the top most
                                    if (_mouseBox.Width <= 1 && _mouseBox.Height <= 1)
                                        break;
                                }
                            }
                        }

                        OnSelectionChanged(EventArgs.Empty);
                        _mouseMode = MouseMode.Default;
                        Invalidate();
                        break;

                    // Stops moving the selection
                    case MouseMode.MoveSelection:
                        // CGX
                        // ShowHideLayoutElement("IACProfile", true);
                        _mouseMode = MouseMode.Default;
                        Cursor = Cursors.Default;
                        break;

                    // Turns of resize
                    case MouseMode.ResizeSelected:
                        _resizeTempBitmap?.Dispose();
                        _resizeTempBitmap = null;
                        _mouseMode = MouseMode.Default;
                        Cursor = Cursors.Default;
                        SelectedLayoutElements[0].Resizing = false;
                        SelectedLayoutElements[0].Size = SelectedLayoutElements[0].Size;
                        Invalidate(new Region(PaperToScreen(SelectedLayoutElements[0].Rectangle)));
                        break;

                    case MouseMode.InsertNewElement:
                        if (_mouseBox.Width == 0)
                            _mouseBox.Width = _elementToAddWithMouse.Rectangle.Width != 0 ? _elementToAddWithMouse.Rectangle.Width : 200;
                        if (_mouseBox.Height == 0)
                            _mouseBox.Height = _elementToAddWithMouse.Rectangle.Height != 0 ? _elementToAddWithMouse.Rectangle.Height : 100;
                        if (_mouseBox.Width < 0)
                        {
                            _mouseBox.X = _mouseBox.X + _mouseBox.Width;
                            _mouseBox.Width = -_mouseBox.Width;
                        }

                        if (_mouseBox.Height < 0)
                        {
                            _mouseBox.Y = _mouseBox.Y + _mouseBox.Height;
                            _mouseBox.Height = -_mouseBox.Height;
                        }

                        _elementToAddWithMouse.Rectangle = ScreenToPaper(_mouseBox);
                        AddToLayout(_elementToAddWithMouse);
                        AddToSelection(_elementToAddWithMouse);
                        _elementToAddWithMouse = null;
                        _mouseMode = MouseMode.Default;
                        _mouseBox.Inflate(5, 5);
                        Invalidate(new Region(_mouseBox));
                        break;

                    case MouseMode.PanMap:
                        if (_mouseBox.Width != 0 || _mouseBox.Height != 0)
                            PanMap(SelectedLayoutElements[0] as LayoutMap, _mouseBox.Width, _mouseBox.Height);
                        _mouseMode = MouseMode.StartPanMap;
                        break;

                    case MouseMode.Default:
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (_mouseMode)
                {
                    case MouseMode.Default:
                        if (SelectedLayoutElements.Count < 1)
                        {
                            for (var i = 0; i < _contextMenuRight.MenuItems.Count; i++)
                                _contextMenuRight.MenuItems[i].Enabled = false;
                        }
                        else if (SelectedLayoutElements.Count == 1)
                        {
                            _cMnuSelAli.Enabled = false;
                            _cMnuSelFit.Enabled = false;
                        }

                        _contextMenuRight.Show(this, e.Location);
                        for (var i = 0; i < _contextMenuRight.MenuItems.Count; i++)
                            _contextMenuRight.MenuItems[i].Enabled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Fires whenever the LayoutControl is resized.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlResize(object sender, EventArgs e)
        {
            var paperTlPixel = PaperToScreen(new PointF(0, 0));
            var paperBrPixel = PaperToScreen(new PointF(PaperWidth, PaperHeight));
            var paperSizeScreen = new SizeF(paperBrPixel.X - paperTlPixel.X, paperBrPixel.Y - paperTlPixel.Y);

            // Sets up the vertical scroll bars
            if (paperSizeScreen.Width <= (Width - _vScrollBar.Width - 4))
            {
                _paperLocation.X = (Width - _vScrollBar.Width - 4 - paperSizeScreen.Width) / 2F;
            }
            else
            {
                _paperLocation.X = 0;
            }

            // Sets up the horizontal scroll bar
            if (paperSizeScreen.Height <= (Height - _hScrollBar.Height - 4))
            {
                _paperLocation.Y = (Height - _hScrollBar.Height - 4 - paperSizeScreen.Height) / 2F;
            }
            else
            {
                _paperLocation.Y = 0;
            }

            UpdateScrollBars();

            // Invalidate the whole thing since we are moving this around
            Invalidate();
        }

        /// <summary>
        /// This gets fired when one of the layoutElements gets invalidated.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LeInvalidated(object sender, EventArgs e)
        {
            if (_suppressLEinvalidate) return;
            Invalidate();
        }

        /// <summary>
        /// Call this to indicate elements were added or removed.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnElementsChanged(EventArgs e)
        {
            // CGX
            // ElementsChanged?.Invoke(this, e);
        }

        // CGX
        /// <summary>
        /// 
        /// </summary>
        private void OnElementsChanged(string sOperation, LayoutElement le)
        {
            if (ElementsChanged != null)
                ElementsChanged(new Tuple<string, LayoutElement>(sOperation, le), EventArgs.Empty);
        }// CGX END

        /// <summary>
        /// Calls this to indicate the fileName has been changed.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnFilenameChanged(EventArgs e)
        {
            FilenameChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Call this to indicate that a layout was loaded from file.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnLayoutLoaded(EventArgs e)
        {
            LayoutLoaded?.Invoke(this, e);
        }

        /// <summary>
        /// Call this to indicate the selection has changed.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnSelectionChanged(EventArgs e)
        {
            if (_layoutMapToolStrip != null)
            {
                if (SelectedLayoutElements.Count == 1 && SelectedLayoutElements[0] is LayoutMap)
                {
                    _layoutMapToolStrip.Enabled = true;
                }
                else
                {
                    _layoutMapToolStrip.Enabled = false;
                    if (_mouseMode == MouseMode.StartPanMap || _mouseMode == MouseMode.PanMap)
                        _mouseMode = MouseMode.Default;
                }
            }

            if (SelectedLayoutElements.Count == 0 && (_mouseMode == MouseMode.ResizeSelected || _mouseMode == MouseMode.MoveSelection))
                _mouseMode = MouseMode.Default;

            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Calls this to indicate the zoom has been changed.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnZoomChanged(EventArgs e)
        {
            ZoomChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Converts between a point in paper coordinants in 1/100th of an inch to screen coordinants.
        /// </summary>
        /// <param name="paper">Point in paper coordinates.</param>
        /// <returns>Rectangle in screen coordinates.</returns>
        private PointF PaperToScreen(PointF paper)
        {
            return PaperToScreen(paper.X, paper.Y);
        }

        /// <summary>
        /// Converts between a point in paper coordinants in 1/100th of an inch to screen coordinants.
        /// </summary>
        /// <param name="paperX">The paper X coordinate.</param>
        /// <param name="paperY">The paper Y coordinate.</param>
        /// <returns>Point in screen coordinates.</returns>
        private PointF PaperToScreen(float paperX, float paperY)
        {
            var screenX = (paperX / 100F * 96F * _zoom) + _paperLocation.X;
            var screenY = (paperY / 100F * 96F * _zoom) + _paperLocation.Y;
            return new PointF(screenX, screenY);
        }

        /// <summary>
        /// Converts between a rectangle in paper coordinants in 1/100th of an inch to screen coordinants.
        /// </summary>
        /// <param name="paper">Rectangle in paper coordinates.</param>
        /// <returns>Rectangle in screen coordinates.</returns>
        private RectangleF PaperToScreen(RectangleF paper)
        {
            return PaperToScreen(paper.X, paper.Y, paper.Width, paper.Height);
        }

        /// <summary>
        /// Converts a rectangle in screen coordinants to paper coordiants in 1/100 of an inch.
        /// </summary>
        /// <param name="paperX">The paper X coordinate.</param>
        /// <param name="paperY">The paper Y coordinate.</param>
        /// <param name="paperW">The paper width.</param>
        /// <param name="paperH">The paper height.</param>
        /// <returns>Rectangle in screen coordinates.</returns>
        private RectangleF PaperToScreen(float paperX, float paperY, float paperW, float paperH)
        {
            var screenTl = PaperToScreen(paperX, paperY);
            var screenBr = PaperToScreen(paperX + paperW, paperY + paperH);
            return new RectangleF(screenTl.X, screenTl.Y, screenBr.X - screenTl.X, screenBr.Y - screenTl.Y);
        }

        /// <summary>
        /// This event handler is fired by the print document when it prints and draws the layout to the print document
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            DrawPage(e.Graphics);
        }

        /// <summary>
        /// Converts a point in screen coordinants to paper coordinants in 1/100 of an inch
        /// </summary>
        /// <param name="screen">Rectangle in screen coordinates to convert.</param>
        /// <returns>Rectangle in paper coordinates.</returns>
        private PointF ScreenToPaper(PointF screen)
        {
            return ScreenToPaper(screen.X, screen.Y);
        }

        /// <summary>
        /// Converts a point in screen coordinants to paper coordinants in 1/100 of an inch.
        /// </summary>
        /// <param name="screenX">X value of the point.</param>
        /// <param name="screenY">Y value of the point.</param>
        /// <returns>The converted point in paper coordinates.</returns>
        private PointF ScreenToPaper(float screenX, float screenY)
        {
            var paperX = (screenX - _paperLocation.X) / _zoom / 96F * 100F;
            var paperY = (screenY - _paperLocation.Y) / _zoom / 96F * 100F;
            return new PointF(paperX, paperY);
        }

        /// <summary>
        /// Converts a rectangle in screen coordinants to paper coordiants in 1/100 of an inch.
        /// </summary>
        /// <param name="screen">Rectangle in screen coordinates to convert.</param>
        /// <returns>Rectangle in paper coordinates.</returns>
        private RectangleF ScreenToPaper(RectangleF screen)
        {
            return ScreenToPaper(screen.X, screen.Y, screen.Width, screen.Height);
        }

        /// <summary>
        /// Converts a rectangle in screen coordinants to paper coordiants in 1/100 of an inch.
        /// </summary>
        /// <param name="screenX">Screen X coordinate.</param>
        /// <param name="screenY">Screen Y coordinate.</param>
        /// <param name="screenW">Screen width.</param>
        /// <param name="screenH">Scrren height.</param>
        /// <returns>Rectangle in paper coordinates.</returns>
        private RectangleF ScreenToPaper(float screenX, float screenY, float screenW, float screenH)
        {
            var paperTl = ScreenToPaper(screenX, screenY);
            var paperBr = ScreenToPaper(screenX + screenW, screenY + screenH);
            return new RectangleF(paperTl.X, paperTl.Y, paperBr.X - paperTl.X, paperBr.Y - paperTl.Y);
        }

        /// <summary>
        /// Updates the scroll bars so the look and act right.
        /// </summary>
        private void UpdateScrollBars()
        {
            if (_zoom == 0)
                _zoom = 100; // Bug 1457. Zoom can be zero on older printer drivers don't divide by 0
            var papVisRect = ScreenToPaper(0F, 0F, Width, Height);
            _hScrollBar.Minimum = -Convert.ToInt32(PaperWidth * 96 / 100.0 * _zoom);
            _hScrollBar.Maximum = Convert.ToInt32(PaperWidth * 96 / 100.0 * _zoom) + (Convert.ToInt32(papVisRect.Width) / 2);
            _hScrollBar.LargeChange = Convert.ToInt32(papVisRect.Width) / 2;
            if (Convert.ToInt32(papVisRect.X / 2) < _hScrollBar.Minimum)
                _hScrollBar.Value = _hScrollBar.Minimum;
            else if (Convert.ToInt32(papVisRect.X / 2) > _hScrollBar.Maximum - _hScrollBar.LargeChange)
                _hScrollBar.Value = _hScrollBar.Maximum - _hScrollBar.LargeChange;
            else
                _hScrollBar.Value = Convert.ToInt32(papVisRect.X / 2);

            _vScrollBar.Minimum = -Convert.ToInt32(PaperHeight * 96 / 100.0 * _zoom);
            _vScrollBar.Maximum = Convert.ToInt32(PaperHeight * 96 / 100.0 * _zoom) + (Convert.ToInt32(papVisRect.Height) / 2);
            _vScrollBar.LargeChange = Convert.ToInt32(papVisRect.Height) / 2;
            if (Convert.ToInt32(papVisRect.Y / 2) < _vScrollBar.Minimum)
                _vScrollBar.Value = _vScrollBar.Minimum;
            else if (Convert.ToInt32(papVisRect.Y / 2) > _vScrollBar.Maximum - _vScrollBar.LargeChange)
                _vScrollBar.Value = _vScrollBar.Maximum - _vScrollBar.LargeChange;
            else
                _vScrollBar.Value = Convert.ToInt32(papVisRect.Y / 2);
        }

        /// <summary>
        /// This fires when the vscrollbar is moved.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void VScrollBarScroll(object sender, ScrollEventArgs e)
        {
            _paperLocation.Y = _paperLocation.Y + (e.OldValue - e.NewValue);
            Invalidate();
        }

        #endregion

        #region Classes

        // CGX
        public class StockLayoutElement
        {
            public string Name = "";
            public SizeF Size = new SizeF();
            public System.Drawing.Point Location = new System.Drawing.Point();
            public LayoutElement element = null;
        }
        public Dictionary<int, List<StockLayoutElement>> DicoUndoRedoElements = new Dictionary<int, List<StockLayoutElement>>();

        public int iCurrentStep = -1;

        public void SaveLayoutElements()
        {
            try
            {
                List<StockLayoutElement> List = new List<StockLayoutElement>();
                foreach (LayoutElement le in LayoutElements)
                {
                    StockLayoutElement sle = new StockLayoutElement();
                    sle.Name = le.Name;
                    sle.Size = le.Size;
                    sle.Location = le.Location;
                    sle.element = le;
                    List.Add(sle);
                }

                iCurrentStep++;

                //Supression de la pile supperieure 
                for (int i = DicoUndoRedoElements.Keys.Count - 1; i >= iCurrentStep; i--)
                {
                    DicoUndoRedoElements.Remove(i);
                }

                DicoUndoRedoElements.Add(iCurrentStep, List);

                this.Refresh();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }
        }

        private StockLayoutElement GetStockLayoutElement(List<StockLayoutElement> ListElement, LayoutElement element)
        {
            foreach (StockLayoutElement stock in ListElement)
            {
                if (stock.Name == element.Name)
                {
                    return stock;
                }
            }
            return null;
        }

        private LayoutElement GetElement(List<LayoutElement> ListElement, LayoutElement element)
        {
            foreach (LayoutElement stock in ListElement)
            {
                if (stock.Name == element.Name)
                {
                    return stock;
                }
            }
            return null;
        }

        public void UpdateLayoutElements()
        {
            if (DicoUndoRedoElements.ContainsKey(iCurrentStep))
            {
                List<StockLayoutElement> List = DicoUndoRedoElements[iCurrentStep].ToList();

                if (List != null)
                {

                    for (int i = 0; i < LayoutElements.Count; i++)
                    {
                        LayoutElement le = LayoutElements[i];

                        StockLayoutElement le2 = GetStockLayoutElement(List, le);

                        //Update
                        if (le2 != null)
                        {
                            if (le.Size != le2.Size || le.Location != le2.Location)
                            {
                                le.Size = le2.Size;
                                le.Location = le2.Location;
                                OnElementsChanged("UPDATE", le);
                            }

                        }
                        //Delete
                        if (le2 == null)
                        {
                            RemoveFromLayout(le);
                            OnElementsChanged("DELETE", le);
                        }
                    }

                    //Ajout
                    for (int i = 0; i < List.Count; i++)
                    {
                        LayoutElement le = GetElement(LayoutElements, List[i].element);

                        if (le == null)
                        {
                            LayoutElements.Insert(0, List[i].element);
                            OnElementsChanged("ADD", List[i].element);
                        }
                    }


                }
            }


            RefreshElements();
        }// FIN CGX

        private static class XmlHelper
        {
            #region Methods

            public static T EnumFromString<T>(string s)
            {
                return (T)Enum.Parse(typeof(T), s);
            }

            public static T FromString<T>(string s)
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(s);
            }

            public static string ToString<T>(T item)
            {
                return TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(item);
            }

            #endregion
        }

        #endregion
    }
}
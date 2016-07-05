using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotSpatial.Controls;
using System.Drawing;
using System.Web.Caching;
using System.IO;
using DotSpatial.Data;
using System.Globalization;
using DotSpatial.Symbology;
using System.Data;
using DotSpatial.MapWebClient;
using DotSpatial.Projections;
using GeoAPI.Geometries;


namespace DotSpatial.WebControls
{
    #region EventTypes
    /// <summary>
    /// Event Args on click
    /// </summary>
    public class MapClickEventArgs : EventArgs
    {

        private int _button = -999;
        /// <summary>
        /// button clicked 1=left, 2= right
        /// </summary>
        public int Button
        {
            set
            {
                _button = value;
            }
            get
            {
                return _button;
            }

        }

        /// <summary>
        /// x coordinate on map
        /// </summary>
        public double X { set; get; }

        public MapClickEventArgs()
        {
            Y = 0;
            X = 0;
        }

        /// <summary>
        /// y coordinate on map
        /// </summary>
        public double Y { set; get; }
    }

    /// <summary>
    /// Event args on Data Event
    /// </summary>
    public class DataGridEventArgs : EventArgs
    {
        public DataGridEventArgs()
        {
            Recordsource = null;
        }

        /// <summary>
        /// Recordsource
        /// </summary>
        public DataTable Recordsource { set; get; }
    }

    #endregion

    /// <summary>
    /// WebMap Main Class
    /// </summary>
    [ToolboxData("<{0}:WebMap runat=server></{0}:WebMap>")]
    public class WebMap : WebControl, ICallbackEventHandler, IPostBackEventHandler
    {
        /// <summary>
        /// Occurs when map is clicked with Pan&click tool active
        /// </summary>
        public delegate void DelegateMapClick(object sender, MapClickEventArgs e);
        /// <summary>
        /// Occurs when map is clicked with Pan&click tool active
        /// </summary>
        public event DelegateMapClick MapClick;

        public delegate void DelegateDataOnGrid(object sender, DataGridEventArgs e);
        public event DelegateDataOnGrid DataOnGrid;

        public delegate void DelegateAddFeature(object sender, FeatureSet fs, Feature f);
        public event DelegateAddFeature AddFeature;

        public delegate void DelegateOnRedraw(object sender);
        public event DelegateOnRedraw OnRedraw;

        [Browsable(false)]
        public WebMapClient Back
        {
            set
            {
                HttpContext.Current.Session["bck_" + SessionName] = value;
            }
            get
            {
                object o = HttpContext.Current.Session["bck_" + SessionName];

                if (o != null)
                {
                    return (WebMapClient)o;
                }

                return null;

            }
        }

        [Browsable(false)]
        public WebMarkers Markers
        {
            set
            {
                HttpContext.Current.Session["mrk_" + SessionName] = value;
            }
            get
            {
                object o = HttpContext.Current.Session["mrk_" + SessionName];

                if (o != null)
                {
                    return (WebMarkers)o;
                }

                return null;

            }
        }

        public void SetActualTool(WebTool tool)
        {
            StringBuilder sb = new StringBuilder("");

            int tl = (int)tool;

            sb.Append(@"<script language='javascript'>");
            sb.Append(@"WriteTool('" + ClientID + "'," + tl.ToString() + ");");
            sb.Append(@"</script>");

            ClientScriptManager cm = this.Page.ClientScript;

            cm.RegisterStartupScript(this.GetType(), "ajax", sb.ToString(), false);

        }

        public void RaisePostBackEvent(string eventArgument)
        {
            GDIMap m = ControlMap;

            string[] arg = eventArgument.Split('|');

            string cmd = arg[0].ToUpper();

            switch (cmd)
            {

                case "CLICK":
                    {

                        if (MapClick != null)
                        {
                            Point pt1 = new Point(Convert.ToInt32(arg[2]), Convert.ToInt32(arg[3]));
                            Coordinate pm1 = m.PixelToProj(pt1);

                            MapClickEventArgs mc = new MapClickEventArgs
                            {
                                Button = Convert.ToInt32(arg[1]),
                                X = pm1.X,
                                Y = pm1.Y
                            };

                            MapClick(this, mc);
                        }

                    }
                    break;

                //                case "ADDFEATURE":  //moved to RaiseCallbackEvent

                //                case "DATAGRID":  //never gets here - wrong place? moved to RaiseCallbackEvent

                case "FORCEREFRESH":
                    {
                        //simply do nothing 
                    }
                    break;

            }
        }

        //[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            base.AddAttributesToRender(writer);
        }

        [Browsable(false)]
        public GDIMap ControlMap
        {
            get
            {
                GDIMap m = (GDIMap)HttpContext.Current.Session[SessionName];

                if (m == null)
                {
                    m = new GDIMap();

                    HttpContext.Current.Session[SessionName] = m;
                }

                return m;
            }
            set
            {
                object o = HttpContext.Current.Session[SessionName];

                if (o == null)
                {
                    HttpContext.Current.Session[SessionName] = value;
                }
                else
                {
                    GDIMap newMap = value;
                    GDIMap actMap = (GDIMap)HttpContext.Current.Session[SessionName];

                    newMap.Size = actMap.Size;

                    newMap.ViewExtents = actMap.ViewExtents;

                    HttpContext.Current.Session[SessionName] = newMap;

                }
            }
        }

        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        public ProjectionInfo Projection
        {
            get
            {
                return ControlMap.Projection;
            }
            set
            {
                ControlMap.Projection = value;
            }
        }


        /// <summary>
        /// Adds the fileName as a new layer to the map, returning the new layer.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to add</param>
        /// <returns>The IMapLayer added to the file.</returns>
        public virtual IMapLayer AddLayer(string fileName)
        {
            return ControlMap.AddLayer(fileName);
        }

        private string SessionName
        {
            get
            {
                string name = "";

                object o = ViewState[ClientID];

                if (o == null)
                {
                    name = Guid.NewGuid().ToString();
                    ViewState[ClientID] = name;
                }
                else
                {
                    name = (string)ViewState[ClientID];
                }

                return name;
            }

        }

        [Browsable(false)]
        public Extent MapViewExtents
        {
            set
            {
                HttpContext.Current.Session["MapViewExtents" + SessionName] = value;

            }
            get
            {
                object o = HttpContext.Current.Session["MapViewExtents" + SessionName];

                if (o == null)
                {
                    return null;
                }

                return (Extent)HttpContext.Current.Session["MapViewExtents" + SessionName];

            }
        }

        private Size ControlSize
        {
            get
            {
                object o = HttpContext.Current.Session["size_" + SessionName];

                if (o == null)
                {
                    return new Size(0, 0);
                }
                return (Size)HttpContext.Current.Session["size_" + SessionName];
            }
            set
            {
                Size sz = value;
                HttpContext.Current.Session["size_" + SessionName] = sz;

                ControlMap.Size = sz;
            }
        }

        public void ZoomAll(ref GDIMap m)
        {

            if (MapViewExtents == null)
            {
                m.ZoomToMaxExtent();
            }
            else
            {
                m.ViewExtents = MapViewExtents;
            }
        }

        public void Move(ref GDIMap m, int direction)
        {

            int w = ControlSize.Width / 2;
            int h = ControlSize.Height / 2;

            int dx = 0, dy = 0;

            switch (direction)
            {
                case 0:
                    {
                        dx = -w;
                    }
                    break;
                case 1:
                    {
                        dy = -h;
                    }
                    break;

                case 2:
                    {
                        dx = w;
                    }
                    break;
                case 3:
                    {
                        dy = h;
                    }
                    break;
            }

            m.MapFrame.Pan(new Point(dx, dy));
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            base.RenderContents(output);

            if (this.DesignMode)
            {
                output.Write("<table style='border-color:Black;background-color:" + BackColor.ToString() + "'><tr><td style='vertical-align:middle;text-align:center'> Map: " + ClientID + "</td></tr></table>");

                //output.Write("<table style='border-color:Black;background-color:" + BackColor.ToString() + "'><tr><td width=300px height=100px style='vertical-align:middle;text-align:center'> Map: " + ClientID + "</td></tr></table>");

                //string w = WebUtil.UnitFormat(Width);
                //string h = WebUtil.UnitFormat(Height);

                //output.Write("<table style='border-color:Black;background-color:" + BackColor.ToString() + "'><tr><td width="+w+" height="+h+" style='vertical-align:middle;text-align:center'> DSMap: " + ClientID  + "</td></tr></table>");
            }
            else
            {
                if (Page.IsPostBack)
                {
                    GDIMap m = ControlMap;
                    string htm = Redraw(ref m);
                    output.Write(htm);
                }
            }

        }


        private string Refresh(ref GDIMap m)
        {
            Rectangle rect = m.ProjToPixel(m.ViewExtents);

            WebMapClient mapClient = (WebMapClient)HttpContext.Current.Session["bck_" + SessionName];

            string htm = "";
            string ms = Milliseconds();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            MapOnCache(ref m);

            htm = htm + "ImageHandler.ashx?ID=" + (string)ViewState[ClientID] + "?" + ms; // + "|";


            if (mapClient != null)
            {

                string h = mapClient.GetHTML(ref m, ControlSize, ClientID);

                htm += "|" + h + "|" + rect.Left.ToString() + "|" + rect.Top.ToString() + "|" + rect.Width.ToString() + "|" + rect.Height.ToString() + "|" + "copi";

            }
            else
            {
                htm += "|Tile|Tl|Tt|Tw|Th|(C)";
            }

            WebMarkers markers = (WebMarkers)HttpContext.Current.Session["mrk_" + SessionName];

            if (markers != null)
            {
                htm += "|";
                htm += markers.ToHtml(ref m);
            }
            else
            {
                htm += "|Markers";
            }

            htm = htm + "|" + m.ViewExtents.MinX.ToString(nfi) + "|";
            htm = htm + m.ViewExtents.MinY.ToString(nfi) + "|";
            htm = htm + m.ViewExtents.MaxX.ToString(nfi) + "|";
            htm = htm + m.ViewExtents.MaxY.ToString(nfi);

            return htm;
        }

        private string Redraw(ref GDIMap m)
        {
            //MessageBox.Show(this.ID + " " + Name);

            //Rectangle Rect = m.ProjToPixel(m.ViewExtents); 

            //Rectangle Rect = new Rectangle(0, 0, (int)this.Width.Value, (int)this.Height.Value);

            bool nullMap = (this.DesignMode | m == null);

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            string ms = Milliseconds();

            if (!nullMap) MapOnCache(ref m);

            string htm = "";

            htm += "<div id=\"Container_" + ClientID + "\" style=\"position:absolute; left:0px; top:0px; width:100%; height:100%;  overflow:hidden \">";

            WebMapClient mapClient = (WebMapClient)HttpContext.Current.Session["bck_" + SessionName];

            if (mapClient != null)
            {

                string x = mapClient.GetHTML(ref m, ControlSize, ClientID);

                htm += x;

            }

            htm += "<div id=\"DivCanvas_" + ClientID + "\" style=\"position:absolute; left:0px; top:0px; width:100%; height:100%; z-index:2; overflow:hidden \">";

            if (!nullMap)
            {
                htm += "<img id=\"Buffer_" + ClientID + "\" style=\"position:absolute; left:0px; top:0px; z-index:1; \" src=\"" + "ImageHandler.ashx?ID=" + (string)ViewState[ClientID] + "?" + ms + "\" />";
                htm += "<img id=\"Canvas_" + ClientID + "\" style=\"position:absolute; left:0px; top:0px; z-index:2; \" src=\"" + "ImageHandler.ashx?ID=" + (string)ViewState[ClientID] + "?" + ms + "\" />";
            }

            htm += "</div>";

            htm += "<div id=\"EditCanvas_" + ClientID + "\" style=\"position:absolute; left:0px; top:0px; width:100%; height:100%; z-index:1000; overflow:hidden \">";
            htm += "</div>";


            htm += "<div id=\"Wait_" + ClientID + "\" style=\"position:absolute; visibility:visible; left:0px; top:0px; width:100%; height:100%;background-color:white;filter:alpha(opacity=50);z-index:2000\">";

            htm += "<table style=\"width:100%; height:100%\"><tr><td style=\"vertical-align:middle; text-align:center\">";
            htm += "<img id=\"ImgWait_" + ClientID + "\" alt=\"\" src=\"" + Page.ClientScript.GetWebResourceUrl(this.GetType(), "DotSpatial.WebControls.Images.Wait.gif") + "\" style=\"z-index:2001\" />";
            htm += "</td></tr></table>";

            htm += "</div>";

            int activeLayerType = -1;

            if (m.Layers.SelectedLayer != null)
            {
                //                    IMapFeatureLayer MFL = (IMapFeatureLayer)m.Layers.SelectedLayer;
                // exception if cast MapRasterLayer in previous line, so use IMapLayer:
                IMapLayer mfl = m.Layers.SelectedLayer;

                activeLayerType = 0;
                if (mfl.GetType() == typeof(MapPointLayer))
                {
                    activeLayerType = 1;
                }
                if (mfl.GetType() == typeof(MapLineLayer))
                {
                    activeLayerType = 2;
                }
                if (mfl.GetType() == typeof(MapPolygonLayer))
                {
                    activeLayerType = 3;
                }

            }

            htm += "<div id=\"LayerType_" + ClientID + "\" style=\"visibility:hidden;\">" + activeLayerType.ToString() + "</div>";



            // QQQ
            //if (theTiler != null)
            //{
            //    htm += "<div id=\"Copyright_" + ClientID + "\" style=\"position:absolute; top:auto; left:3px; bottom:3px; right:auto; width:auto; z-index:200000; font-family: Arial, Helvetica, sans-serif; font-size: x-small; font-weight: normal; color: #000000;  border: 1px solid #C0C0C0; background-color: #FFFFFF \">&nbsp;" + theTiler.Copyright + "&nbsp;</div>";
            //}

            //htm += "<div id=\"StatusBar_" + ClientID + "\" style=\"position:absolute; top:auto; left:auto; bottom:3px; right:3px; width:auto; height:24px; border: 1px solid #000000; background-color: #FFFFFF; z-index:1000\"></div>";

            htm += "<div id=\"MinX_" + ClientID + "\" style=\"visibility:hidden;\">" + m.ViewExtents.MinX.ToString(nfi) + "</div>";
            htm += "<div id=\"MinY_" + ClientID + "\" style=\"visibility:hidden;\">" + m.ViewExtents.MinY.ToString(nfi) + "</div>";
            htm += "<div id=\"MaxX_" + ClientID + "\" style=\"visibility:hidden;\">" + m.ViewExtents.MaxX.ToString(nfi) + "</div>";
            htm += "<div id=\"MaxY_" + ClientID + "\" style=\"visibility:hidden;\">" + m.ViewExtents.MaxY.ToString(nfi) + "</div>";


            string islatlon = "0";

            //if (m.Extent.MaxX <= 180 && m.Extent.MinX >= -180.0 & m.Extent.MinY >= -90 & m.Extent.MaxY <= 90.0)
            if (m.Projection.IsLatLon == true) islatlon = "1";

            htm += "<div id=\"IsLatLon_" + ClientID + "\" style=\"visibility:hidden;\" >" + islatlon + "</div>";

            htm += "<div id =\"Floater_" + ClientID + "\" style=\"border: 1px solid #000000; background: #FFFF00; position:absolute; width: 0px; height:0px; top:0px; left:0px; visibility:hidden; z-index:11\" onmousedown=\"\" onmousemove=\"\" onmouseup=\"\" ondrag(event)=\"return false\" onSelectStart=\"return false\"></div>";

            WebMarkers markers = (WebMarkers)HttpContext.Current.Session["mrk_" + SessionName];

            if (markers != null)
            {
                htm += "<div id =\"Markers_" + ClientID + "\" style=\"position:absolute; left:0px; top:0px; width:100%; height:100%; z-index:300\">";
                htm += markers.ToHtml(ref m);
                htm += "</div>";
            }


            htm += "</div>";


            return htm;
        }

        private void MapOnCache(ref GDIMap m)
        {
            //Bitmap b = new Bitmap();
            //Graphics g = Graphics.FromImage(b);

            //Rectangle r = new Rectangle(0, 0, m.Width, m.Height);

            //string ID = ClientID;

            //Size sz = ControlSize;

            //m.MapFrame.Print(g, r);

            using (Bitmap b = m.Draw())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] buffer = ms.ToArray();
                    HttpContext.Current.Cache.Insert((string)ViewState[ClientID], buffer, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(1));
                }

                //g.Dispose();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);

            //if (!this.DesignMode & !Page.IsPostBack)
            if (!this.DesignMode)
            {
                ClientScriptManager cm = this.Page.ClientScript;

                if (!cm.IsClientScriptBlockRegistered("CallServer"))
                {
                    //                    String callbackReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "", true);
                    string callbackReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "''", true);
                    callbackReference = callbackReference.Replace("'" + ClientID + "'", "ID");
                    string callBackScript = "function CallServer(ID,arg) {" + callbackReference + "; }";
                    //String callBackScript = "function CallServer(arg, context) {alert(arg.toString()+' '+context.toString()); }";
                    cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callBackScript, true);
                }

                if (!cm.IsClientScriptBlockRegistered("CallPostBack"))
                {
                    string postBackReference = cm.GetPostBackEventReference(this, "arg");
                    postBackReference = postBackReference.Replace("'" + ClientID + "'", "ID");
                    postBackReference = postBackReference.Replace("'arg'", "arg");
                    string postBackScript = "function CallPostBack(ID,arg) {" + postBackReference + ";}";
                    cm.RegisterClientScriptBlock(this.GetType(), "CallPostBack", postBackScript, true);
                }

                //string StartUP = "window.onload = function() {Initialize(\"" + ClientID + "\")}";
                string startUp = "Initialize(\"" + ClientID + "\");";
                cm.RegisterStartupScript(this.GetType(), "StartUp" + ClientID, startUp, true);

                string resourceName = "DotSpatial.WebControls.Script.WebDsScript.js";
                cm.RegisterClientScriptResource(GetType(), resourceName);

                resourceName = "DotSpatial.WebControls.Script.WebToolBar.js";
                cm.RegisterClientScriptResource(GetType(), resourceName);

                resourceName = "DotSpatial.WebControls.Script.wz_jsgraphics.js";
                cm.RegisterClientScriptResource(GetType(), resourceName);

                // These .js scripts no longer needed with HTML tooltips (title):
                //                resourceName = "DotSpatial.WebControls.Script.wz_tooltip.js";
                //                cm.RegisterClientScriptResource(GetType(), resourceName);

                //                resourceName = "DotSpatial.WebControls.Script.tip_balloon.js";
                //                cm.RegisterClientScriptResource(GetType(), resourceName);

                resourceName = "DotSpatial.WebControls.Script.CookieManager.js";
                cm.RegisterClientScriptResource(GetType(), resourceName);
            }

        }

        protected override void CreateChildControls()
        {
            if (!Page.IsCallback & !Page.IsPostBack)
            {
                this.Attributes.Add("onmousedown", "OnMouseDown(this,event)");
                this.Attributes.Add("onmousemove", "OnMouseMove(this,event)");
                this.Attributes.Add("onmouseup", "OnMouseUp(this,event)");
            }

            //base.CreateChildControls();
        }

        private string _returnValue = "";
        private string _returnCommand = "";

        public string GetCallbackResult()
        {

            string sr = string.Format("{0}|{1}|{2}", this.ClientID, _returnCommand, _returnValue);

            return sr;

        }

        public void RaiseCallbackEvent(string eventArgument)
        //        public virtual void RaiseCallbackEvent(String eventArgument)  //to override in subclass
        {
            //            returnCommand = "REFRESH";  //unsightly refresh when change legend selection
            _returnCommand = "NOTHING";

            //            string Nm = SessionName;  //not used

            GDIMap m = (GDIMap)HttpContext.Current.Session[(string)ViewState[ClientID]];

            if (m == null) return;

            string[] arg = eventArgument.Split('|');

            string cmd = arg[0].ToUpper();

            switch (cmd)
            {
                case "ZOOMALL":
                    {
                        ZoomAll(ref m);
                        _returnCommand = "REFRESHANDHIDEBUFFER";
                    }
                    break;

                case "SELECT":
                    {
                        if (m.Layers.SelectedLayer != null)
                        {
                            Point pt1 = new Point(Convert.ToInt32(arg[1]), Convert.ToInt32(arg[2]));
                            Point pt2 = new Point(Convert.ToInt32(arg[3]), Convert.ToInt32(arg[4]));

                            Coordinate pm1 = m.PixelToProj(pt1);
                            Coordinate pm2 = m.PixelToProj(pt2);

                            Extent ex = new Extent(Math.Min(pm1.X, pm2.X), Math.Min(pm1.Y, pm2.Y),
                                                   Math.Max(pm1.X, pm2.X), Math.Max(pm1.Y, pm2.Y));

                            Envelope mapEnv;

                            m.Layers.SelectedLayer.ClearSelection(out mapEnv);


                            m.Layers.SelectedLayer.ClearSelection();

                            Envelope affectedarea = null;

                            //                                m.Layers.SelectedLayer.Select(m.ViewExtents.ToEnvelope(), ex.ToEnvelope(), Symbology.SelectionMode.IntersectsExtent, out affectedarea);
                            m.Layers.SelectedLayer.Select(ex.ToEnvelope(), ex.ToEnvelope(), SelectionMode.Intersects, out affectedarea);

                            _returnCommand = "STRUCTURE";

                        }
                        else
                        {
                            _returnValue = "<table><tr><td>Select a layer first.<p></td></tr><table>";
                            _returnCommand = "POPUP";
                            //                                returnValue = "Select a layer first.";
                            //                                returnCommand = "ALERT";
                        }

                    }
                    break;

                case "INFO":
                    {

                        Point pt = new Point(Convert.ToInt32(arg[2]), Convert.ToInt32(arg[3]));
                        Coordinate pm = m.PixelToProj(pt);

                        Extent ex = new Extent(pm.X, pm.Y, pm.X, pm.Y);

                        if (m.Layers.SelectedLayer != null)
                        {
                            FeatureSet fs = m.Layers.SelectedLayer.DataSet as FeatureSet;

                            //                                List<IFeature> flist = fs.Select(ex);  //returns empty list when IndexMode == false
                            List<int> flist = fs.SelectIndices(ex);

                            int n = flist.Count;

                            //                                returnValue = "<table border='1'>";  //looks goofy
                            _returnValue = "<table>";

                            if (n > 0)
                            {
                                for (int i = 0; i < fs.DataTable.Columns.Count; i++)
                                {
                                    _returnValue += "<tr><td>" + fs.DataTable.Columns[i].ColumnName +
                                        //                                                       "</td><td>" + flist[0].DataRow[i].ToString() + "</td></tr>";
                                                   "</td><td>" + fs.GetFeature(flist[0]).DataRow[i].ToString() + "</td></tr>";
                                }

                                _returnValue += "</table>";
                                _returnCommand = "POPUP";
                            }
                        }
                        else
                        {
                            _returnValue = "<table><tr><td>Select a layer first.<p></td></tr><table>";
                            _returnCommand = "POPUP";
                            //                                returnValue = "Select a layer first.";
                            //                                returnCommand = "ALERT";
                        }
                    }
                    break;


                case "RESIZE":
                    {

                        Size newSz = new Size(Convert.ToInt32(arg[2]), Convert.ToInt32(arg[3]));
                        Size actualSz = ControlSize;

                        if (actualSz.Width == 0 || actualSz.Height == 0)
                        {
                            ControlSize = newSz;

                            ZoomAll(ref m);

                            _returnCommand = "STRUCTURE";
                        }
                        else
                        {
                            if (newSz != actualSz)
                            {
                                ControlSize = newSz;

                                _returnCommand = "STRUCTURE";
                            }
                            else
                            {
                                _returnCommand = "NOTHING";
                            }
                        }

                    }
                    break;

                case "ZOOMRECT":
                    {

                        Point pt1 = new Point(Convert.ToInt32(arg[1]), Convert.ToInt32(arg[2]));
                        Point pt2 = new Point(Convert.ToInt32(arg[3]), Convert.ToInt32(arg[4]));

                        Coordinate pm1 = m.PixelToProj(pt1);
                        Coordinate pm2 = m.PixelToProj(pt2);

                        Extent x = new Extent(Math.Min(pm1.X, pm2.X),
                                              Math.Min(pm1.Y, pm2.Y),
                                              Math.Max(pm1.X, pm2.X),
                                              Math.Max(pm1.Y, pm2.Y));

                        m.ViewExtents = x;
                        _returnCommand = "REFRESHANDHIDEBUFFER";
                    }
                    break;

                case "ZOOMIN":
                    {
                        int x = Convert.ToInt32(arg[1]);
                        int y = Convert.ToInt32(arg[2]);

                        Point pntZoomAndCenter = new Point((x - m.Size.Width / 2), (y - m.Size.Height / 2));
                        m.MapFrame.Pan(pntZoomAndCenter);
                        m.ZoomIn();
                        _returnCommand = "REFRESHANDHIDEBUFFER";
                    }
                    break;

                case "ZOOMOUT":
                    {
                        int x = Convert.ToInt32(arg[1]);
                        int y = Convert.ToInt32(arg[2]);

                        Point pnt = new Point((x - m.Size.Width / 2), (y - m.Size.Height / 2));

                        m.MapFrame.Pan(pnt);
                        m.ZoomOut();
                        _returnCommand = "REFRESHANDHIDEBUFFER";
                    }
                    break;

                case "PAN":
                    {
                        int x = Convert.ToInt32(arg[1]);
                        int y = Convert.ToInt32(arg[2]);

                        // not used:                System.Drawing.Point pnt = new System.Drawing.Point((x - m.Size.Width / 2), (y - m.Size.Height / 2));

                        m.MapFrame.Pan(new Point(x, y));
                        _returnCommand = "REFRESH";
                    }
                    break;

                case "WHEELIN":
                    {
                        m.ZoomIn();
                        _returnCommand = "REFRESHANDHIDEBUFFER";
                    }
                    break;

                case "WHEELOUT":
                    {
                        m.ZoomOut();
                        _returnCommand = "REFRESHANDHIDEBUFFER";
                    }
                    break;

                case "DATAGRID":  //moved to here from RaisePostBackEvent
                    {
                        if (m.Layers.SelectedLayer != null)
                        {
                            //string script=null;

                            IMapFeatureLayer mfl = (IMapFeatureLayer)m.Layers.SelectedLayer;

                            int n = mfl.Selection.Count;

                            FeatureSet fs;
                            DataTable rs;
                            if (n > 0)
                            {
                                fs = mfl.Selection.ToFeatureSet();
                                rs = fs.DataTable;
                            }
                            else
                            {
                                fs = mfl.DataSet as FeatureSet;
                                rs = fs.DataTable;
                            }

                            if (DataOnGrid != null)  //Let event handler display grid?
                            {
                                DataGridEventArgs e = new DataGridEventArgs();
                                e.Recordsource = rs;
                                DataOnGrid(this, e);
                            }
                            else  //Display default HTML grid
                            {
                                _returnValue = "<table border='1'><tr>";

                                for (int h = 0; h < rs.Columns.Count; h++)
                                {
                                    _returnValue += "<th>" + rs.Columns[h].ColumnName + "</th>";
                                }
                                _returnValue += "</tr>";

                                string rowHtml;
                                for (int r = 0; r < rs.Rows.Count; r++)
                                {  //note: _much_ faster if build each row separately
                                    rowHtml = "<tr>";
                                    for (int c = 0; c < rs.Columns.Count; c++)
                                    {
                                        rowHtml += "<td>" + fs.GetFeature(r).DataRow[c].ToString() + "</td>";
                                    }
                                    rowHtml += "</tr>";
                                    _returnValue += rowHtml;
                                }
                                _returnValue += "</table>";
                                _returnCommand = "POPUP";
                            }

                        }
                        else
                        {
                            _returnValue = "<table><tr><td>Select a layer first.<p></td></tr><table>";
                            _returnCommand = "POPUP";
                        }
                    }
                    break;

                case "ADDFEATURE":  //moved to here from RaisePostBackEvent
                    {
                        int num = Convert.ToInt32(arg[1]);

                        Point pt = new Point();
                        Coordinate[] pm = new Coordinate[num];

                        for (int i = 0; i < num; i++)
                        {
                            pt.X = Convert.ToInt32(arg[(i + 1) * 2]);
                            pt.Y = Convert.ToInt32(arg[(i + 1) * 2 + 1]);

                            pm[i] = m.PixelToProj(pt);
                        }

                        if (m.Layers.SelectedLayer != null)
                        {
                            FeatureSet fs = m.Layers.SelectedLayer.DataSet as FeatureSet;
                            FeatureType ft = FeatureType.Unspecified;

                            IMapFeatureLayer mfl = (IMapFeatureLayer)m.Layers.SelectedLayer;

                            if (mfl.GetType() == typeof(MapPointLayer))
                            {
                                ft = FeatureType.Point;
                            }
                            if (mfl.GetType() == typeof(MapLineLayer))
                            {
                                ft = FeatureType.Line;
                            }
                            if (mfl.GetType() == typeof(MapPolygonLayer))
                            {
                                ft = FeatureType.Polygon;
                            }

                            if (ft != FeatureType.Unspecified)
                            {
                                Feature f = new Feature(ft, pm);

                                try
                                {
                                    if (AddFeature != null)
                                    {
                                        AddFeature(this, fs, f);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            fs.Features.Add(f);
                                            fs.Save();
                                        }
                                        catch
                                        {
                                            fs.Features.Remove(f);
                                            throw;  //re-throw exception
                                        }
                                        fs.InitializeVertices();
                                    }
                                    //Apparently have to force recreating labels when add feature.
                                    if (mfl.LabelLayer != null)
                                    {
                                        // Recreating label layer works.
                                        //                                            MapLabelLayer NewLabels = new MapLabelLayer();
                                        //                                            NewLabels.Symbology = MFL.LabelLayer.Symbology;
                                        //                                            NewLabels.Symbolizer = MFL.LabelLayer.Symbolizer;
                                        //                                            MFL.LabelLayer = NewLabels;
                                        // Recreating just labels also works.
                                        mfl.LabelLayer.CreateLabels();
                                    }
                                    _returnCommand = "FORCEREFRESH";
                                }
                                catch (Exception e)
                                {
                                    _returnValue = "Unable to save feature.<p>" + e.Message;
                                    _returnCommand = "POPUPANDREFRESH";  //erase new shape too
                                }

                                //                                    fs.IndexMode = true;
                                //Adding a feature sets FeatureSet.IndexMode to false,
                                // causing fs.Select above in INFO case to return a list
                                // with Count == 0. One workaround is to set IndexMode
                                // back to true. This does cause all existing labels
                                // to disapper with refresh, but recreating label layer
                                // above fixed that. (Also tried InitializeVertices,
                                // InvalidateEnvelope, InvalidateVertices, UpdateExtents, etc.)
                                // Oops, setting IndexMode back to true corrupts shapes...
                            }
                        }

                    }
                    break;

            }

            //ControlMap = m;
            HttpContext.Current.Session[(string)ViewState[ClientID]] = m;

            if (_returnCommand == "STRUCTURE")
            {
                _returnValue = Redraw(ref m);

                if (OnRedraw != null) OnRedraw(this);
            }

            if (_returnCommand == "REFRESH" | _returnCommand == "REFRESHANDHIDEBUFFER")
            {
                _returnValue = Refresh(ref m);

                if (OnRedraw != null) OnRedraw(this);
            }

        }


        private string Milliseconds()
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.Subtract(dt1);

            string s = ts.TotalMilliseconds.ToString();

            s = s.Replace(".", "");

            return s;
        }

    }
}

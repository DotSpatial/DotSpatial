using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotSpatial.Symbology;
using System.Drawing;
using System.IO;
using System.Web.Caching;
using DotSpatial.MapWebClient;

namespace DotSpatial.WebControls
{


    [ToolboxData("<{0}:WebLegend runat=server></{0}:WebLegend>")]
    public class WebLegend : TreeView
    {
        [Bindable(true)]
        [Category("WebMapControlID")]
        [DefaultValue("")]
        [Localizable(true)]
        public string WebMapID
        {
            get
            {
                String s = (String)ViewState["WebMapControlID"];
                return s;
            }

            set
            {
                ViewState["WebMapControlID"] = value;

                CreateLegend();
            }
        }

        
     
        protected void AddLegItems(TreeNode pTN, LegendItem li)
        {
            if (li.LegendItemVisible == false) return;

            TreeNode tn = new TreeNode(li.LegendText);

            //tn.SelectAction = TreeNodeSelectAction.None;
            tn.PopulateOnDemand = false;
            //tn.Value = li.LegendText;
            //tn.NavigateUrl = "javascript:ManualSelect('" + "ciccio" + "')";
            //tn.Text = "<span onclick='return false;'>" + li.LegendText + "</span>";
            tn.Text = li.LegendText;
            
            if (li.LegendSymbolMode == SymbolMode.Checkbox)
            {
                tn.ShowCheckBox = true;
                tn.Checked = li.Checked;
            }

            if (li.LegendSymbolMode == SymbolMode.Symbol)
            {
                tn.ImageUrl = "ImageHandler.ashx?ID="+LegItemOnCache(ref li);
            }


            if (pTN == null)
            {
                Nodes.Add(tn);
            }
            else
            {
                pTN.ChildNodes.Add(tn);
            }


            if (li.LegendItems != null)
            {
                //foreach (LegendItem l in li.LegendItems)
                for(int i = li.LegendItems.Count()-1;i>=0;i--)
                {
                    LegendItem l = (LegendItem)li.LegendItems.ElementAt(i);
                    AddLegItems(tn, l);
                }
            }


        }

        protected void CreateLegend()
        {
            WebMap wm = (WebMap)FindControl(WebMapID);
            if (wm == null) return;

            GDIMap m = wm.ControlMap;

            if (m == null) return;

            Nodes.Clear();

            for (int i = m.Layers.Count - 1; i >= 0; i--)
            {
                LegendItem l = (LegendItem)m.Layers[i];

                AddLegItems(null, l);
            }



            WebMapClient c = wm.Back;

            if (c!=null)
            {
                c.List(this);
            }

            foreach (TreeNode tn in Nodes)
            {
                tn.ExpandAll();
            }

        }

        private string LegItemOnCache(ref LegendItem li)
        {

            Size sz = li.GetLegendSymbolSize();

            Bitmap b = new Bitmap(sz.Width+1, sz.Height+1);
            Graphics g = Graphics.FromImage(b);

            li.LegendSymbol_Painted(g,new Rectangle(0,0,sz.Width,sz.Height));

            string name = Guid.NewGuid().ToString();

            MemoryStream MS = new MemoryStream();
            b.Save(MS, System.Drawing.Imaging.ImageFormat.Png);
            byte[] buffer = MS.ToArray();
            HttpContext.Current.Cache.Insert(name, buffer, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15));

            g.Dispose();
            b.Dispose();

            return name;
        }

        protected override void OnSelectedNodeChanged(EventArgs e)
        {
            base.OnSelectedNodeChanged(e);
            
            string[] keys = SelectedNode.ValuePath.Split(PathSeparator);

            WebMap wm = (WebMap)FindControl(WebMapID);
            GDIMap m = wm.ControlMap;

            m.Layers.SelectedLayer = null;
            for (int i = m.Layers.Count - 1; i >= 0; i--)
            {
                LegendItem liR = (LegendItem)m.Layers[i];

                LegendItem li = GetLegendItem(keys, liR);

                if (li != null)
                {
                    li.IsSelected = true;
                    m.Layers.SelectedLayer = m.Layers[i];
                    break;
                }
            }

            //GDIMapXXX
            //bool found = false;
            //foreach (LegendItem liR in m.Legend.RootNodes)
            //{
            //    if (liR.LegendText == keys[0])
            //    {
            //        string[] k = keysPop(keys);

            //        LegendItem li = GetLegendItem(k, liR);

            //        if (li != null)
            //        {
            //            li.IsSelected = true;
            //            found = true;
            //            break;
            //        }

            //    }
            //}

            //if (found == false)
            //{

            //}


        }

        protected override void OnTreeNodeCheckChanged(TreeNodeEventArgs e)
        {

            string[] keys = e.Node.ValuePath.Split(PathSeparator);

            WebMap wm = (WebMap)FindControl(WebMapID);
            GDIMap m = wm.ControlMap;
            WebMapClient wmc =wm.Back;

            bool found = false;

            for (int i = m.Layers.Count - 1; i >= 0; i--)
            {
                    LegendItem liR = (LegendItem)m.Layers[i];

                    LegendItem li = GetLegendItem(keys, liR);
                    
                    if (li != null)
                    {
                        li.Checked = e.Node.Checked;
                        found = true;
                        break;
                    }
            }

            if (found == false)
            {
                wmc.Check(keys, e.Node.Checked);
            }

            base.OnTreeNodeCheckChanged(e);
        }
        

        LegendItem GetLegendItem(string[] keys, LegendItem lip)
        {

            if (keys.Count() == 0) return null;

            if (keys.Count() == 1 && lip.LegendText == keys[0]) return lip;

            foreach (LegendItem li in lip.LegendItems)
            {
                if (li.LegendText == keys[0])
                {
                    if (keys.Count() == 1)
                        return li;
                    else
                    {
                        string[] k = keysPop(keys);
                        
                        return GetLegendItem(k, li);
                    }
                }
            }
                        

            return null;
        }

        string[] keysPop(string[] keys)
        {
            string[] k = new string[keys.Count() - 1];

            Array.Copy(keys, 1, k, 0, k.Count());

            return k;
        }
      
        //AutoPostBack


//1 function TreeNodeCheckChanged(event, control) {
// 2     // Valid for IE and Firefox/Safari/Chrome.
// 3     var obj = window.event ? window.event.srcElement : event.target;
// 4     var source = window.event ? window.event.srcElement.id : event.target.id;
// 5     source = source.replace(control.id + "t", control.id + "n");
// 6     var checkbox = document.getElementById(source);
// 7     if (checkbox != null && obj.tagName == "INPUT" && obj.type == "checkbox") {
// 8         __doPostBack(checkbox.id, "");
// 9     }
//10 }  


        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    TreeView1.Attributes.Add("onclick", "TreeNodeCheckChanged(event, this)");
        //}



        //autpostback fine




    }
}

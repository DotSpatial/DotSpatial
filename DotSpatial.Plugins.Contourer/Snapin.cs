using System;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Data.Properties;
using DotSpatial.Symbology;

namespace Contourer
{
    public class Snapin : Extension
    {
        public override void Activate()
        {
            AddMenuItems(App.HeaderControl);
            base.Activate();
        }

        private void AddMenuItems(IHeaderControl header)
        {
            SimpleActionItem contourerItem = new SimpleActionItem("Contour...", new EventHandler(menu_Click)) { Key = "kC" };
            header.Add(contourerItem);
        }

        private void menu_Click(object sender, EventArgs e)
        {
            FormContour Frm = new FormContour();

            Frm.layers = App.Map.GetRasterLayers();

            if (Frm.layers.GetLength(0) <= 0)
            {
                MessageBox.Show("No raster layer found!");
                return;
            }

            if (Frm.ShowDialog() == DialogResult.OK)
            {
                IMapFeatureLayer fl = App.Map.Layers.Add(Frm.Contours);
                fl.LegendText = Frm.LayerName + " - Contours";

                int numlevs = Frm.lev.GetLength(0);

                switch (Frm.contourtype)
                {
                    case (Contour.ContourType.Line):
                        {
                            LineScheme ls = new LineScheme();
                            ls.Categories.Clear();

                            for (int i = 0; i < Frm.color.GetLength(0); i++)
                            {
                                LineCategory lc = new LineCategory(Frm.color[i], 2.0);

                                lc.FilterExpression = "[Value] = " + Frm.lev[i].ToString();
                                lc.LegendText = Frm.lev[i].ToString();

                                ls.AddCategory(lc);
                            }

                            fl.Symbology = ls;
                        }
                        break;

                    case (Contour.ContourType.Polygon):
                        {
                            PolygonScheme ps = new PolygonScheme();
                            ps.Categories.Clear();

                            for (int i = 0; i < Frm.color.GetLength(0); i++)
                            {
                                PolygonCategory pc = new PolygonCategory(Frm.color[i], Color.Transparent, 0);
                                pc.FilterExpression = "[Lev] = " + i.ToString();
                                pc.LegendText = Frm.lev[i].ToString() + " - " + Frm.lev[i + 1].ToString();

                                ps.AddCategory(pc);
                            }

                            fl.Symbology = ps;
                        }
                        break;
                }
            }
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;

namespace DotSpatial.Plugins.Contourer
{
    public partial class FormContour : Form
    {
        public IMapRasterLayer[] Layers;

        public string LayerName = "";
        public double Min;
        public double Max;
        public double Eve;
        public double[] Lev;
        public Color[] Color;
        public Contour.ContourType Contourtype;

        public FeatureSet Contours;

        public FormContour()
        {
            InitializeComponent();
        }

        private Contour.ContourType GetSelectedType()
        {
            return comboBoxType.SelectedIndex == 0 ? Contour.ContourType.Line : Contour.ContourType.Polygon;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LayerName = comboBoxLayerList.Text;

            Max = (double)numericUpDownMax.Value;
            Min = (double)numericUpDownMin.Value;
            Eve = (double)numericUpDownEvery.Value;

            Contourtype = GetSelectedType();

            Lev = Contour.CreateLevels(Min, Max, Eve);

            Contours = Contour.Execute(Layers[comboBoxLayerList.SelectedIndex].DataSet as Raster, Contourtype, "Value", Lev);
            Contours.Projection = Layers[comboBoxLayerList.SelectedIndex].Projection;

            int numLev = Lev.GetLength(0);

            switch (Contourtype)
            {
                case (Contour.ContourType.Line):
                    {
                        Color = new Color[numLev];
                        for (int i = 0; i < numLev; i++)
                        {
                            Color[i] = tomPaletteEditor1.GetColor(Lev[i]);
                        }
                    }
                    break;
                case (Contour.ContourType.Polygon):
                    {
                        Color = new Color[numLev - 1];
                        for (int i = 0; i < numLev - 1; i++)
                        {
                            Color[i] = tomPaletteEditor1.GetColor(Lev[i] + (Eve / 2));
                        }
                    }
                    break;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormContour_Load(object sender, EventArgs e)
        {
            if (Layers.Length == 0)
            {
                MessageBox.Show("Please add a raster layer.");
                Close();
                return;
            }

            foreach (IMapRasterLayer l in Layers)
            {
                comboBoxLayerList.Items.Add(l.LegendText);
            }

            comboBoxLayerList.SelectedIndex = 0;

            comboBoxType.Items.Add("Line");
            comboBoxType.Items.Add("Polygons");

            comboBoxType.SelectedIndex = 1;
        }

        private void ComputeMinMaxEvery()
        {
            double min, max, every;

            Raster rst = Layers[comboBoxLayerList.SelectedIndex].DataSet as Raster;

            Contour.ContourType contType = GetSelectedType();

            Contour.CreateMinMaxEvery(rst, contType, out min, out  max, out every); 

            numericUpDownMin.Value = (decimal)min;
            numericUpDownMax.Value = (decimal)max;

            numericUpDownEvery.Value = (decimal)every;

            tomPaletteEditor1.ClearItems();

            if (contType == Contour.ContourType.Line)
            {
                tomPaletteEditor1.AddItem(min, System.Drawing.Color.Chartreuse);
                tomPaletteEditor1.AddItem(max, System.Drawing.Color.Magenta);
            }

            if (contType == Contour.ContourType.Polygon)
            {
                tomPaletteEditor1.AddItem(min + every / 2, System.Drawing.Color.Chartreuse);
                tomPaletteEditor1.AddItem(max - every / 2, System.Drawing.Color.Magenta);
            }

            tomPaletteEditor1.Invalidate();
        }

        private void comboBoxLayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComputeMinMaxEvery();
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComputeMinMaxEvery();
        }

        private void numericUpDownMin_ValueChanged(object sender, EventArgs e)
        {
            if (tomPaletteEditor1.Items.Count > 0)
            {
                tomPaletteEditor1.Items[0].Value = Convert.ToDouble(numericUpDownMin.Value);
            }
        }

        private void numericUpDownMax_ValueChanged(object sender, EventArgs e)
        {
            if (tomPaletteEditor1.Items.Count > 0)
            {
                tomPaletteEditor1.Items[tomPaletteEditor1.Items.Count - 1].Value = Convert.ToDouble(numericUpDownMax.Value);
            }
        }
    }
}
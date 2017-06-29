using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace Contourer
{
    public partial class FormContour : Form
    {
        public IMapRasterLayer[] layers;

        public string LayerName = "";
        public double min;
        public double max;
        public double eve;
        public double[] lev;
        public Color[] color;
        public Contour.ContourType contourtype;

        public DotSpatial.Data.FeatureSet Contours;

        public FormContour()
        {
            InitializeComponent();
        }

        private Contour.ContourType GetSelectedType()
        {
            Contour.ContourType ContType = new Contour.ContourType();

            if (comboBoxType.SelectedIndex == 0)
                ContType = Contour.ContourType.Line;
            else
                ContType = Contour.ContourType.Polygon;

            return ContType;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LayerName = comboBoxLayerList.Text;

            max = (double)numericUpDownMax.Value;
            min = (double)numericUpDownMin.Value;
            eve = (double)numericUpDownEvery.Value;

            contourtype = GetSelectedType();

            lev = Contour.CreateLevels(min, max, eve);

            Contours = Contour.Execute(layers[comboBoxLayerList.SelectedIndex].DataSet as DotSpatial.Data.Raster, contourtype, "Value", lev);
            Contours.Projection = layers[comboBoxLayerList.SelectedIndex].Projection;

            int NumLev = lev.GetLength(0);

            switch (contourtype)
            {
                case (Contour.ContourType.Line):
                    {
                        color = new Color[NumLev];
                        for (int i = 0; i < NumLev; i++)
                        {
                            color[i] = tomPaletteEditor1.GetColor(lev[i]);
                        }
                    }
                    break;
                case (Contour.ContourType.Polygon):
                    {
                        color = new Color[NumLev - 1];
                        for (int i = 0; i < NumLev - 1; i++)
                        {
                            color[i] = tomPaletteEditor1.GetColor(lev[i] + (eve / 2));
                        }
                    }
                    break;
            }
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void FormContour_Load(object sender, EventArgs e)
        {
            if (layers.Length == 0)
            {
                MessageBox.Show("Please add a raster layer.");
                this.Close();
                return;
            }

            foreach (IMapRasterLayer l in layers)
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

            DotSpatial.Data.Raster rst = layers[comboBoxLayerList.SelectedIndex].DataSet as DotSpatial.Data.Raster;

            Contour.ContourType ContType = new Contour.ContourType();

            if (comboBoxType.SelectedIndex == 0)
                ContType = Contour.ContourType.Line;
            else
                ContType = Contour.ContourType.Polygon;

            Contour.CreateMinMaxEvery(rst, ContType, out min, out  max, out every); ;

            numericUpDownMin.Value = (decimal)min;
            numericUpDownMax.Value = (decimal)max;

            numericUpDownEvery.Value = (decimal)every;

            tomPaletteEditor1.ClearItems();

            if (ContType == Contour.ContourType.Line)
            {
                tomPaletteEditor1.AddItem(min, Color.Chartreuse);
                tomPaletteEditor1.AddItem(max, Color.Magenta);
                //tomPaletteEditor1.AddItem((max + min) / 2, Color.Yellow);
            }

            if (ContType == Contour.ContourType.Polygon)
            {
                tomPaletteEditor1.AddItem(min + every / 2, Color.Chartreuse);
                tomPaletteEditor1.AddItem(max - every / 2, Color.Magenta);
                //tomPaletteEditor1.AddItem((max + min) / 2, Color.Yellow);
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
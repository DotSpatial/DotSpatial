using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DotSpatial.Data;
using System.Reflection;





namespace DotSpatial.Symbology.Forms
{
    public partial class ExpressionDialog : UserControl
    {
        public string sComplexEmpty = "def Main():\n   \n \n  return \"no value\"";

        public event EventHandler ExpressionTextChanged;

        IFeatureLayer _ActiveLayer = null;
        IDataTable _DataTable = null; // CGX AERO GLZ
        private string _Expression = string.Empty;

        public bool IsComplexExpression(string Expression)
        {
            if (!string.IsNullOrEmpty(Expression))
            {
                /*if (Expression == sComplexEmpty)
                {
                    return false;
                }*/

                if (Expression.StartsWith("def Main():"))
                {
                    return true;
                }
                else
                {
                    return false;
            }
            }
            return false;
        }
        public void FillExpression(string value)
        {
            if (IsComplexExpression(value))
            {
                // TB_Simple.Text = "";
                TB_Advanced.Text = value;
                TabControl.SelectedTab = TabAdvanced;
            }
            else
            {
                TB_Simple.Text = value;
                //TB_Advanced.Text = sComplexEmpty;
                TabControl.SelectedTab = TabSimple;
            }
            _Expression = value;
        }
        public string Expression
        {
            get
            {
                UpdateExpression();
                return _Expression;
            }
            set
            {
                FillExpression(value);
                _Expression = value;
            }
        }

        public ExpressionDialog()
        {
            InitializeComponent();


        }

        public IFeatureLayer ActiveLayer
        {
            set
            {
                _ActiveLayer = value;
                _DataTable = _ActiveLayer.DataSet.DataTable;
                FillListFields();
            }

        }

        private void FillListFields()
        {
            try
            {
                if (_DataTable != null) // CGX AERO GLZ
                {
                    listViewFields.Items.Clear();
                    foreach (DataColumn dc in _DataTable.Columns)
                        listViewFields.Items.Add("[" + dc.ColumnName + "]");
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        private void B_Preview_Click(object sender, EventArgs e)
        {
            Preview();
        }

        private void UpdateExpression()
        {
            _Expression = TB_Simple.Text;
            if (TabControl.SelectedTab == TabAdvanced)
                _Expression = TB_Advanced.Text;

        }

        private void Preview()
        {
            try
            {
                if ((_ActiveLayer as FeatureLayer).DataSet.Features.Count > 0)
                {

                    string sResult = string.Empty;


                    if (IsComplexExpression(Expression))
                    {
                        Expression = TB_Advanced.Text;
                        sResult = Compute(Expression);
                        sResult = Script.EvaluateWithDialog(sResult);
                    }
                    else
                    {
                        Expression = TB_Simple.Text;
                        sResult = Compute(Expression);
                    }

                    PanelPreview.Refresh();

                    richTextBoxViewer.Text = sResult;
                }
                else
                {
                    richTextBoxViewer.Text = "Unabled to verify the script, no feature found";
            }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }


        private void listViewFields_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listViewFields.SelectedIndices.Count > 0)
                {
                    string Text = listViewFields.Items[listViewFields.SelectedIndices[0]].Text;
                    if (TabControl.SelectedTab == TabSimple)
                        TB_Simple.SelectedText += Text;
                    if (TabControl.SelectedTab == TabAdvanced)
                        TB_Advanced.SelectedText += Text;
                }
                UpdateExpression();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        private void B_Import_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Filter = "Script Files (*.py)|*.py";
                if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TB_Advanced.Clear();
                    StreamReader SR = new StreamReader(OFD.FileName);
                    TB_Advanced.Text = SR.ReadToEnd();
                    SR.Close();

                    this.B_Preview_Click(sender, e);
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        private void B_Export_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "Script Files (*.py)|*.py";
                if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Création du fichier script
                    string sScriptPath = SFD.FileName;
                    bool bContinue = true;
                    if (File.Exists(sScriptPath))
                    {
                        DialogResult DR = MessageBox.Show("File already exists.\nOverwrite ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (DR == System.Windows.Forms.DialogResult.Yes)
                            File.Delete(sScriptPath);
                        if (DR == System.Windows.Forms.DialogResult.No)
                            bContinue = false;
                    }

                    if (bContinue)
                    {
                        StreamWriter SW = new StreamWriter(sScriptPath);
                        SW.Write(TB_Advanced.Text);
                        SW.Close();
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }



        private List<String> GetFields(string sExpression)
        {

            List<String> ListFields = new List<string>();
            string temp = sExpression;

            int iIndex = 0;
            while (iIndex < temp.Length)
            {
                int start = temp.IndexOf("[", iIndex);
                int end = temp.IndexOf("]", iIndex);

                if ((start != -1) && (end != -1))
                {
                    string sResult = temp.Substring(start + 1, (end - start) - 1);
                    ListFields.Add(sResult);
                    iIndex = end + 1;
                }
                else
                {
                    iIndex++;
            }
            }

            return ListFields;
        }

        private string Compute(string sExpresion)
        {
            string TextTmp = string.Empty;
            try
            {

                List<String> ListFields = GetFields(sExpresion);


                if (_ActiveLayer is FeatureLayer)
                {
                    FeatureLayer fl = _ActiveLayer as FeatureLayer;

                    if (fl.DataSet.Features.Count > 0)
                    {
                        IFeature f = fl.DataSet.Features[0];
                        // Récupération pour de la chaîne de remplacement
                        string sCompute = ReplaceFieldsByValue(GetFields(sExpresion), GetLayersFields(f), fl);

                        TextTmp += sCompute + Environment.NewLine;

                    }

                }


            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }


            return TextTmp;
        }

        private Dictionary<string, Object> GetLayersFields(IFeature f)
        {
            Dictionary<string, Object> featureFields = new Dictionary<string, Object>();
            try
            {

                if (f is IFeature)
                {
                    IFeature feature = (IFeature)f;
                    for (int i = 0; i < feature.ParentFeatureSet.DataTable.Columns.Count; i++)
                    {
                        try
                        {
                            IDataTable DT = f.ParentFeatureSet.DataTable; // CGX AERO GLZ
                            DataColumn Column = DT.Columns[i];
                            if (Column != null)
                            {
                                if (DT.Columns.Contains(Column.ColumnName))
                                {
                                    object Row = feature.DataRow[Column.ColumnName];
                                    if (Row != DBNull.Value)
                                        featureFields.Add(Column.ColumnName, Row);
                                }
                            }
                        }
                        catch (Exception ex)
                        { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }



            return featureFields;
        }

        private string ReplaceFieldsByValue(List<String> ListFields, Dictionary<string, Object> featureFields, FeatureLayer fl)
        {
            string sCompute = Expression;

            try
            {
                foreach (string field in ListFields)
                {
                    foreach (string key in featureFields.Keys)
                    {
                        if (key == field)
                        {

                            if (TabControl.SelectedTab == TabSimple)
                                sCompute = sCompute.Replace("[" + field + "]", featureFields[key].ToString());
                            if (TabControl.SelectedTab == TabAdvanced)
                                sCompute = sCompute.Replace("[" + field + "]", GetFormattedField(fl, field, featureFields[key]));
                        }
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return sCompute;
        }

        private string GetFormattedField(FeatureLayer fl, string sFieldName, object oValue)
        {
            string sResult = string.Empty;

            try
            {

                DataColumn Column = fl.DataSet.DataTable.Columns[sFieldName];
                if (Column.DataType == typeof(string))
                    sResult = "\"" + oValue.ToString() + "\"";

            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return sResult;
        }

        private void TB_Simple_TextChanged(object sender, EventArgs e)
        {

            if (ExpressionTextChanged != null) ExpressionTextChanged(this, new EventArgs());


        }

        private void TB_Advanced_TextChanged(object sender, EventArgs e)
        {
            if (ExpressionTextChanged != null) ExpressionTextChanged(this, new EventArgs());
        }

        private void TB_Simple_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter;
        }

        private void TB_Advanced_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter;
        }

        private void richTextBoxViewer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter;
        }

        private void PanelPreview_Paint(object sender, PaintEventArgs e)
        {
            string sResult = string.Empty;
            if (IsComplexExpression(Expression))
            {
                Expression = TB_Advanced.Text;
                sResult = Compute(Expression);
                sResult = DotSpatial.Python.Script.EvaluateWithDialog(sResult);
            }
            else
            {
                Expression = TB_Simple.Text;
                sResult = Compute(Expression);
            }
            if (string.IsNullOrEmpty(sResult)) { return; }


            var g = e.Graphics;
            var p = sender as Panel;

            var format = new StringFormat { Alignment = StringAlignment.Near };
            Font textFont = new System.Drawing.Font("Arial", 10);
            RectangleF currentView = new RectangleF(PanelPreview.DisplayRectangle.X + 10, PanelPreview.DisplayRectangle.Y + 10, PanelPreview.DisplayRectangle.Width, PanelPreview.DisplayRectangle.Height);

            RectangleF labelBounds = GetComputedLabelBounds(g, sResult, textFont, format, currentView);

            string[] sSplitted = sResult.Split('\n');
            PointF pos = new PointF(labelBounds.X, labelBounds.Y);
            foreach (string sSplit in sSplitted)
            {
                using (var gp2 = new GraphicsPath())
                {
                    float size = GetSizeFromTag(sSplit, textFont.SizeInPoints);
                    FontStyle fontStyle = GetFontFromTag(sSplit);
                    Font newFont = new Font(textFont.FontFamily, size, fontStyle);
                    Color textColor = GetColorFromTag(sSplit);
                    string sText = GetTextWithoutTag(sSplit);

                    SizeF stringfSize = g.MeasureString(sText, newFont);

                    if (sSplit.Contains("<title>") && sSplit.Contains("</title>"))
                    {
                        PointF centerPos = new PointF((pos.X + labelBounds.Width / 2) - (stringfSize.Width / 2), pos.Y);
                        labelBounds = new RectangleF(labelBounds.X, labelBounds.Y + (stringfSize.Height / 2), labelBounds.Width, labelBounds.Height - (stringfSize.Height / 2));
                        //e.Graphics.FillRectangle(new SolidBrush(PanelPreview.BackColor), new Rectangle((int)centerPos.X, (int)centerPos.Y, (int)stringfSize.Width, (int)stringfSize.Height));
                        e.Graphics.DrawString(sText, newFont, new SolidBrush(textColor), centerPos, format);
                        e.Graphics.SetClip(new Rectangle((int)centerPos.X, (int)centerPos.Y, (int)stringfSize.Width, (int)stringfSize.Height), CombineMode.Exclude);

                        // Draw a limit above the text if needed
                        DrawUpperScore(g, sSplit, centerPos, newFont);
                        DrawUnderScore(g, sSplit, centerPos, newFont);
                    }
                    else
                    {
                        e.Graphics.DrawString(sText, newFont, new SolidBrush(textColor), pos, format);

                        // Draw a limit above the text if needed
                        DrawUpperScore(g, sSplit, pos, newFont);
                        DrawUnderScore(g, sSplit, pos, newFont);
                    }

                    pos.Y += stringfSize.Height;

                    gp2.Dispose();
                }
            }
            e.Graphics.DrawRectangle(new Pen(Color.Black), (int)labelBounds.X, (int)labelBounds.Y, (int)labelBounds.Width, (int)labelBounds.Height);

        }

        #region Draw

        /// <summary>
        /// Draw a upper score on label
        /// </summary>
        private void DrawUpperScore(Graphics g, string labelText, PointF pos, Font newFont)
        {
            SizeF stringfSize = g.MeasureString(GetTextWithoutTag(labelText), newFont);
            float fPenSize = -9999F;

            if (labelText.Contains("<U>") && labelText.Contains("</U>"))
            {
                fPenSize = 1.0F;
            }

            if (labelText.Contains("<U=") && labelText.Contains("</U>"))
            {
                Regex regex = new Regex("<U=(.*?)>");
                var v = regex.Match(labelText);
                string s = v.Groups[1].ToString();
                if (!float.TryParse(s, out fPenSize))
                    fPenSize = 1.0F;
            }

            if (fPenSize != -9999F)
            {
                g.DrawLine(new Pen(Color.Black, fPenSize), new PointF(pos.X + 2, pos.Y), new PointF(pos.X + (stringfSize.Width - 2), pos.Y));
            }
        }

        /// <summary>
        /// Draw a upper score on label
        /// </summary>
        private void DrawUnderScore(Graphics g, string labelText, PointF pos, Font newFont)
        {
            SizeF stringfSize = g.MeasureString(GetTextWithoutTag(labelText), newFont);
            float fPenSize = -9999F;

            if (labelText.Contains("<u>") && labelText.Contains("</u>"))
            {
                fPenSize = 1.0F;
            }

            if (labelText.Contains("<u=") && labelText.Contains("</u>"))
            {
                Regex regex = new Regex("<u=(.*?)>");
                var v = regex.Match(labelText);
                string s = v.Groups[1].ToString();
                if (!float.TryParse(s, out fPenSize))
                    fPenSize = 1.0F;
            }

            if (fPenSize != -9999F)
            {
                g.DrawLine(new Pen(Color.Black, fPenSize), new PointF(pos.X + 2, pos.Y + (stringfSize.Height - 2)), new PointF(pos.X + (stringfSize.Width - 2), pos.Y + (stringfSize.Height - 2)));
            }
        }

        private RectangleF GetComputedLabelBounds(Graphics g, string labelText, Font textFont, StringFormat format, RectangleF labelBounds)
        {
            RectangleF newLabelBounds = new RectangleF(labelBounds.Location, labelBounds.Size);
            float fWidth = 0.0F;
            float fHeight = 0.0F;
            PointF pos = new PointF(labelBounds.X, labelBounds.Y);

            string[] sSplitted = labelText.Split('\n');
            foreach (string sSplit in sSplitted)
            {
                using (var gp2 = new GraphicsPath())
                {
                    float size = GetSizeFromTag(sSplit, textFont.SizeInPoints);
                    Font newFont = new Font(textFont.FontFamily, size);
                    string sText = GetTextWithoutTag(sSplit);

                    SizeF stringfSize = g.MeasureString(sText, newFont);

                    if (fWidth < stringfSize.Width) { fWidth = stringfSize.Width; }
                    fHeight += stringfSize.Height;

                    pos.Y += stringfSize.Height;
                }
            }

            newLabelBounds.Size = new SizeF(fWidth + 3, fHeight);

            return newLabelBounds;
        }

        private FontStyle GetFontFromTag(string sText)
        {
            FontStyle fontStyle = FontStyle.Regular;
            string sToDraw = sText;

            if (sToDraw.Contains("<b>") && sToDraw.Contains("</b>")) { fontStyle |= FontStyle.Bold; }
            if (sToDraw.Contains("<i>") && sToDraw.Contains("</i>")) { fontStyle |= FontStyle.Italic; }

            // if (sToDraw.Contains("<u>") && sToDraw.Contains("</u>")) { fontStyle |= FontStyle.Underline; }
            return fontStyle;
        }

        private float GetSizeFromTag(string sText, float fOldSize)
        {
            float fNewSize = fOldSize;
            string sToDraw = sText;

            if (sToDraw.Contains("<size=") && sToDraw.Contains("</size>"))
            {
                Regex regex = new Regex("<size=(.*?)>");
                var v = regex.Match(sText);
                string s = v.Groups[1].ToString();
                if (!float.TryParse(s, out fNewSize))
                    fNewSize = fOldSize;
            }

            return fNewSize;
        }

        private Color GetColorFromTag(string sText)
        {
            string sToDraw = sText;
            string sColor = "black";

            if (sToDraw.Contains("<color=") && sToDraw.Contains("</color>"))
            {
                Regex regex = new Regex("<color=(.*?)>");
                var v = regex.Match(sText);
                sColor = v.Groups[1].ToString();
            }

            return Color.FromName(sColor);
        }

        private string GetTextWithoutTag(string sTextWithTag)
        {
            string sTextWithoutTag = sTextWithTag;
            sTextWithoutTag = sTextWithoutTag.Replace("<b>", string.Empty);
            sTextWithoutTag = sTextWithoutTag.Replace("</b>", string.Empty);
            sTextWithoutTag = sTextWithoutTag.Replace("<i>", string.Empty);
            sTextWithoutTag = sTextWithoutTag.Replace("</i>", string.Empty);
            sTextWithoutTag = sTextWithoutTag.Replace("<title>", string.Empty);
            sTextWithoutTag = sTextWithoutTag.Replace("</title>", string.Empty);

            if (sTextWithTag.Contains("<size="))
            {
                Regex regex = new Regex("<size=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<size=" + s + ">", string.Empty);
                sTextWithoutTag = sTextWithoutTag.Replace("</size>", string.Empty);
            }

            if (sTextWithTag.Contains("<color="))
            {
                Regex regex = new Regex("<color=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<color=" + s + ">", string.Empty);
                sTextWithoutTag = sTextWithoutTag.Replace("</color>", string.Empty);
            }

            sTextWithoutTag = sTextWithoutTag.Replace("<u>", string.Empty);
            if (sTextWithTag.Contains("<u="))
            {
                Regex regex = new Regex("<u=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<u=" + s + ">", string.Empty);
            }

            sTextWithoutTag = sTextWithoutTag.Replace("</u>", string.Empty);

            sTextWithoutTag = sTextWithoutTag.Replace("<U>", string.Empty);
            if (sTextWithTag.Contains("<U="))
            {
                Regex regex = new Regex("<U=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<U=" + s + ">", string.Empty);
            }

            sTextWithoutTag = sTextWithoutTag.Replace("</U>", string.Empty);

            return sTextWithoutTag;
        }

        /// <summary>
        /// Draw shadows under texts
        /// </summary>
        private void DrawShadow(Graphics g, GraphicsPath gp, ILabelSymbolizer symb)
        {
            // Draws the drop shadow
            if (symb.DropShadowEnabled && symb.DropShadowColor != Color.Transparent)
            {
                var shadowBrush = new SolidBrush(symb.DropShadowColor);
                var gpTrans = new Matrix();
                gpTrans.Translate(symb.DropShadowPixelOffset.X, symb.DropShadowPixelOffset.Y);
                gp.Transform(gpTrans);
                g.FillPath(shadowBrush, gp);
                gpTrans = new Matrix();
                gpTrans.Translate(-symb.DropShadowPixelOffset.X, -symb.DropShadowPixelOffset.Y);
                gp.Transform(gpTrans);
                gpTrans.Dispose();
            }
        }

        /// <summary>
        /// Draw halo surrounding the texts
        /// </summary>
        private void DrawHalo(Graphics g, GraphicsPath gp, ILabelSymbolizer symb)
        {
            if (symb.HaloEnabled && symb.HaloColor != Color.Transparent)
            {
                using (var haloPen = new Pen(symb.HaloColor) { Width = 2, Alignment = PenAlignment.Outset })
                {
                    g.DrawPath(haloPen, gp);
                }
            }
        }

        #endregion

    }
}

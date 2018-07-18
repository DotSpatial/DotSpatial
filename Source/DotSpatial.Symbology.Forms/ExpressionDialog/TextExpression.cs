using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotSpatial.Symbology.Forms
{
    public class Bullet
    {
        public enum style { None, Left, Right, Both };
    }

    public class TextExpression
    {
        /// <summary>
        /// Return fontstyle from tag
        /// </summary>
        static public FontStyle GetFontFromTag(string sText)
        {
            FontStyle fontStyle = FontStyle.Regular;
            string sToDraw = sText;

            if (sToDraw.Contains("<b>") && sToDraw.Contains("</b>")) { fontStyle |= FontStyle.Bold; }
            if (sToDraw.Contains("<i>") && sToDraw.Contains("</i>")) { fontStyle |= FontStyle.Italic; }
            //if (sToDraw.Contains("<u>") && sToDraw.Contains("</u>")) { fontStyle |= FontStyle.Underline; }

            return fontStyle;
        }

        /// <summary>
        /// Return fontstyle from tag
        /// </summary>
        static public bool GetFontFamilyFromTag(string sText, out FontFamily fontFamily)
        {
            fontFamily = new FontFamily("Arial");
            string sToDraw = sText;

            if (sToDraw.Contains("<font=") && sToDraw.Contains("</font>"))
            {
                Regex regex = new Regex("<font=(.*?)>");
                var v = regex.Match(sText);
                string s = v.Groups[1].ToString();
                fontFamily = new FontFamily(s);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get size from tag (<size=15> return 15)
        /// </summary>
        static public float GetSizeFromTag(string sText, float fOldSize)
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

        /// <summary>
        /// Get color from tag (<color=red> return Color.Red)
        /// </summary>
        static public bool GetColorFromTag(string sText, out Color cTaggedColor)
        {
            cTaggedColor = Color.FromName("Black");
            string sToDraw = sText;
            string sColor = "black";

            if (sToDraw.Contains("<color=") && sToDraw.Contains("</color>"))
            {
                Regex regex = new Regex("<color=(.*?)>");
                var v = regex.Match(sText);
                sColor = v.Groups[1].ToString();
                cTaggedColor = Color.FromName(sColor);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get color from tag (<color=red> return Color.Red)
        /// </summary>
        static public bool GetBulletStyle(string sText, out Bullet.style bulletStyle)
        {
            bulletStyle = Bullet.style.None;
            string sToDraw = sText;

            if (sToDraw.Contains("<bullet=") && sToDraw.Contains("</bullet>"))
            {
                Regex regex = new Regex("<bullet=(.*?)>");
                var v = regex.Match(sText);
                string sValue = v.Groups[1].ToString();

                if (sValue == Bullet.style.None.ToString()) bulletStyle = Bullet.style.None;
                if (sValue == Bullet.style.Left.ToString()) bulletStyle = Bullet.style.Left;
                if (sValue == Bullet.style.Right.ToString()) bulletStyle = Bullet.style.Right;
                if (sValue == Bullet.style.Both.ToString()) bulletStyle = Bullet.style.Both;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove all tags (<b>, <u>, ...) from input text
        /// </summary>
        static public string GetTextWithoutTag(string sTextWithTag)
        {
            string sTextWithoutTag = sTextWithTag;
            sTextWithoutTag = sTextWithoutTag.Replace("<b>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("</b>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("<i>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("</i>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("<title>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("</title>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("<morse>", "");
            sTextWithoutTag = sTextWithoutTag.Replace("</morse>", "");

            if (sTextWithTag.Contains("<font="))
            {
                Regex regex = new Regex("<font=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<font=" + s + ">", "");
                sTextWithoutTag = sTextWithoutTag.Replace("</font>", "");
            }

            if (sTextWithTag.Contains("<size="))
            {
                Regex regex = new Regex("<size=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<size=" + s + ">", "");
                sTextWithoutTag = sTextWithoutTag.Replace("</size>", "");
            }

            if (sTextWithTag.Contains("<color="))
            {
                Regex regex = new Regex("<color=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<color=" + s + ">", "");
                sTextWithoutTag = sTextWithoutTag.Replace("</color>", "");
            }

            sTextWithoutTag = sTextWithoutTag.Replace("<u>", "");
            if (sTextWithTag.Contains("<u="))
            {
                Regex regex = new Regex("<u=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<u=" + s + ">", "");
            }
            sTextWithoutTag = sTextWithoutTag.Replace("</u>", "");

            sTextWithoutTag = sTextWithoutTag.Replace("<U>", "");
            if (sTextWithTag.Contains("<U="))
            {
                Regex regex = new Regex("<U=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<U=" + s + ">", "");
            }
            sTextWithoutTag = sTextWithoutTag.Replace("</U>", "");

            if (sTextWithTag.Contains("<bullet="))
            {
                Regex regex = new Regex("<bullet=(.*?)>");
                var v = regex.Match(sTextWithTag);
                string s = v.Groups[1].ToString();
                sTextWithoutTag = sTextWithoutTag.Replace("<bullet=" + s + ">", "");
            }
            sTextWithoutTag = sTextWithoutTag.Replace("</bullet>", "");

            return sTextWithoutTag;
        }

        /// <summary>
        /// Draw shadows under texts
        /// </summary>
        static public void DrawShadow(Graphics g, GraphicsPath gp, ILabelSymbolizer symb)
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
        static public void DrawHalo(Graphics g, GraphicsPath gp, ILabelSymbolizer symb)
        {
            if (symb.HaloEnabled && symb.HaloColor != Color.Transparent)
            {
                using (var haloPen = new Pen(symb.HaloColor) { Width = 2, Alignment = PenAlignment.Outset })
                {
                    g.DrawPath(haloPen, gp);
                }
            }
        }

        /// <summary>
        /// Draw a upper score on label
        /// </summary>
        static public void DrawUpperScore(Graphics g, string labelText, PointF pos, Color color, Font newFont, float fRapport)
        {
            SizeF stringfSize = g.MeasureString(GetTextWithoutTag(labelText), newFont);
            float fPenSize = -9999F;

            if (!stringfSize.IsEmpty)
            {
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
                    stringfSize = new SizeF(stringfSize.Width * fRapport, stringfSize.Height * fRapport);
                    g.DrawLine(new Pen(color, fPenSize), new PointF(pos.X + 2, pos.Y), new PointF(pos.X + (stringfSize.Width - 2), pos.Y));
                }
            }
        }

        /// <summary>
        /// Draw a upper score on label
        /// </summary>
        static public void DrawUnderScore(Graphics g, string labelText, PointF pos, Color color, Font newFont, float fRapport)
        {
            SizeF stringfSize = g.MeasureString(GetTextWithoutTag(labelText), newFont);
            float fPenSize = -9999F;

            if (!stringfSize.IsEmpty)
            {
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
                    stringfSize = new SizeF(stringfSize.Width * fRapport, stringfSize.Height * fRapport);
                    g.DrawLine(new Pen(color, fPenSize), new PointF(pos.X + 2, pos.Y + (stringfSize.Height - 2)), new PointF(pos.X + (stringfSize.Width - 2), pos.Y + (stringfSize.Height - 2)));
                }
            }
        }

        /// <summary>
        /// Compute label bound considering the size of the texts, the font and the format
        /// </summary>
        static public RectangleF GetComputedLabelBounds(Graphics g, string labelText, Font textFont, StringFormat format, RectangleF labelBounds, float fRapport)
        {
            RectangleF newLabelBounds = new RectangleF(labelBounds.Location, labelBounds.Size);
            float fWidth = 0.0F;
            float fHeight = 0.0F;

            string[] sSplitted = labelText.Split('\n');
            foreach (string sSplit in sSplitted)
            {
                float size = TextExpression.GetSizeFromTag(sSplit, textFont.SizeInPoints);
                FontFamily fontFamily = textFont.FontFamily;
                FontFamily customFontFamily = null;
                if (TextExpression.GetFontFamilyFromTag(sSplit, out customFontFamily))
                {
                    fontFamily = customFontFamily;
                }
                Font newFont = new Font(fontFamily, size);
                string sText = TextExpression.GetTextWithoutTag(sSplit);
                SizeF stringfSize = g.MeasureString(sText, newFont);

                if (fWidth < stringfSize.Width) { fWidth = stringfSize.Width; }
                if (!IsMorse(sSplit))
                {
                    fHeight += stringfSize.Height;
                }
                else
                {
                    if (fWidth < stringfSize.Width) { fWidth = stringfSize.Width; }
                    float fHeightUsed = stringfSize.Height;
                    float fHeightUsedMorse = 0.0F;

                    float fLargerWidth = 0.0F;
                    Font newFontMorse = new Font(new FontFamily("CGXAERO-MorseCode"), TextExpression.GetSizeFromTag(sSplit, textFont.SizeInPoints));
                    foreach (char c in sText)
                    {
                        SizeF fSizeMorse = g.MeasureString(c.ToString(), newFontMorse);
                        fHeightUsedMorse += fSizeMorse.Height;
                        if (fLargerWidth < fSizeMorse.Width)
                        {
                            fLargerWidth = fSizeMorse.Width;
                        }
                    }
                    float fWidthMorse = stringfSize.Width + fLargerWidth + 1;
                    if (fWidth < fWidthMorse) { fWidth = fWidthMorse; }

                    float fLargerPos = fHeightUsed;
                    if (fHeightUsedMorse > fHeightUsed)
                    {
                        fLargerPos = fHeightUsedMorse;
                    }

                    fHeight += fLargerPos;
                }
            }
            newLabelBounds.Size = new SizeF(fWidth * fRapport + 10, fHeight * fRapport + 2);

            return newLabelBounds;
        }

        /// <summary>
        /// Compute label bound considering the size of the texts, the font and the format
        /// </summary>
        static public RectangleF GetComputedLabelBoundsWithTitle(Graphics g, string labelText, Font textFont, StringFormat format, RectangleF labelBounds, float fRapport)
        {
            RectangleF newLabelBounds = new RectangleF(labelBounds.Location, labelBounds.Size);
            float fWidth = 0.0F;
            float fHeight = 0.0F;
            PointF pos = new PointF(labelBounds.X, labelBounds.Y);
            bool bTitled = false, bFirstLine = true;


            string[] sSplitted = labelText.Split('\n');
            foreach (string sSplit in sSplitted)
            {
                float size = TextExpression.GetSizeFromTag(sSplit, textFont.SizeInPoints);
                FontFamily fontFamily = textFont.FontFamily;
                FontFamily customFontFamily = null;
                if (TextExpression.GetFontFamilyFromTag(sSplit, out customFontFamily))
                {
                    fontFamily = customFontFamily;
                }
                Font newFont = new Font(fontFamily, size);
                string sText = TextExpression.GetTextWithoutTag(sSplit);

                SizeF stringfSize = g.MeasureString(sText, newFont);

                if (!IsMorse(sSplit))
                {
                    if (fWidth < stringfSize.Width) { fWidth = stringfSize.Width; }
                    fHeight += stringfSize.Height;

                    if ((!bTitled) && (bFirstLine))
                    {
                        if (sSplit.Contains("<title>") && sSplit.Contains("</title>"))
                        {
                            pos.Y = pos.Y + ((stringfSize.Height / 2) * fRapport);
                            fHeight = fHeight - (stringfSize.Height / 2);
                        }
                        bTitled = true;
                    }
                }
                else
                {
                    fWidth += stringfSize.Width;
                    float fHeightUsed = stringfSize.Height;
                    float fHeightUsedMorse = 0.0F;

                    float fLargerWidth = 0.0F;
                    Font newFontMorse = new Font(new FontFamily("CGXAERO-MorseCode"), TextExpression.GetSizeFromTag(sSplit, textFont.SizeInPoints));
                    foreach (char c in sText)
                    {
                        SizeF fSizeMorse = g.MeasureString(c.ToString(), newFontMorse);
                        fHeightUsedMorse += fSizeMorse.Height;
                        if (fLargerWidth < fSizeMorse.Width)
                        {
                            fLargerWidth = fSizeMorse.Width;
                        }
                    }
                    fWidth += fLargerWidth + 1;

                    float fLargerPos = fHeightUsed;
                    if (fHeightUsedMorse > fHeightUsed)
                    {
                        fLargerPos = fHeightUsedMorse;
                    }

                    fHeight += fLargerPos;
                }
                bFirstLine = false;
            }
            newLabelBounds.Location = new PointF(pos.X, pos.Y);
            newLabelBounds.Size = new SizeF(fWidth * fRapport + 10, fHeight * fRapport + 2);

            return newLabelBounds;
        }

        /// <summary>
        /// Ratote the graphics
        /// </summary>
        static public void RotateAt(Graphics gr, float cx, float cy, float angle)
        {
            gr.ResetTransform();
            gr.TranslateTransform(-cx, -cy, MatrixOrder.Append);
            gr.RotateTransform(angle, MatrixOrder.Append);
            gr.TranslateTransform(cx, cy, MatrixOrder.Append);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="labelBounds"></param>
        /// <param name="symb"></param>
        static public void DrawBackcolor(Graphics g, RectangleF labelBounds, ILabelSymbolizer symb)
        {
            using (var gp = new GraphicsPath())
            {
                // Draws the text outline
                if (symb.BackColorEnabled && symb.BackColor != Color.Transparent)
                {
                    var backBrush = new SolidBrush(symb.BackColor);
                    if (symb.FontColor == Color.Transparent)
                    {
                        using (var backgroundGP = new GraphicsPath())
                        {
                            backgroundGP.AddRectangle(labelBounds);
                            backgroundGP.FillMode = FillMode.Alternate;
                            backgroundGP.AddPath(gp, true);
                            g.FillPath(backBrush, backgroundGP);
                        }
                    }
                    else
                    {
                        float fOpacity = symb.BackColor.GetOpacity();
                        SolidBrush sbBrush = new SolidBrush(Color.FromArgb((int)(fOpacity * 255), symb.BackColor));
                        g.FillRectangle(sbBrush, labelBounds);
                    }
                }
            }
        }


        /// <summary>
        /// Only used for the Morse
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sText"></param>
        /// <param name="fFontSize"></param>
        /// <returns></returns>
        public static float GetLargerMorseWidth(Graphics g, string sText, float fFontSize)
        {
            Font newFontMorse = new Font(new FontFamily("CGXAERO-MorseCode"), fFontSize);
            SizeF fSizeMorse = g.MeasureString(sText, newFontMorse);
            float fMaxMorseWidth = 0.0F;
            foreach (char c in sText)
            {
                float fCurrentMorseWidth = g.MeasureString(c.ToString(), newFontMorse).Width;
                if (fMaxMorseWidth < fCurrentMorseWidth)
                {
                    fMaxMorseWidth = fCurrentMorseWidth;
                }
            }

            return fMaxMorseWidth;
        }

        /// <summary>
        /// 
        /// </summary>
        static public bool IsMorse(string sText)
        {
            return (sText.Contains("<morse>") && sText.Contains("</morse>"));
        }

        /// <summary>
        /// 
        /// </summary>
        static public bool IsTitle(string sText)
        {
            return (sText.Contains("<title>") && sText.Contains("</title>"));
        }

        static public void DrawMorseText(Graphics g, GraphicsPath gp2, ref PointF pos, string sSplit, Font newFont, FontStyle fontStyle, StringAlignment stringAlign, RectangleF newLabelBounds)
        {
            string sText = TextExpression.GetTextWithoutTag(sSplit);
            SizeF stringfSize = g.MeasureString(sText, newFont);
            var format = new StringFormat { Alignment = StringAlignment.Near };

            PointF posText = new PointF(pos.X, pos.Y);

            float fHeightUsed = stringfSize.Height;
            float fHeightUsedMorse = 0.0F;
            float fWidthUsed = stringfSize.Width;
            Font newFontMorse = new Font(new FontFamily("CGXAERO-MorseCode"), TextExpression.GetSizeFromTag(sSplit, newFont.SizeInPoints));
            SizeF fSizeMorse = g.MeasureString(sText, newFontMorse);
            float fMorsePos = pos.Y;

            fWidthUsed += TextExpression.GetLargerMorseWidth(g, sText, TextExpression.GetSizeFromTag(sSplit, newFont.SizeInPoints));
            if (stringAlign == StringAlignment.Center) { posText = new PointF((pos.X + newLabelBounds.Width / 2) - (fWidthUsed / 2) + 1, pos.Y); }
            if (stringAlign == StringAlignment.Far) { posText = new PointF((pos.X + newLabelBounds.Width) - (fWidthUsed + 3), pos.Y); }

            PointF posMorse = new PointF(posText.X + stringfSize.Width, posText.Y);
            foreach (char c in sText)
            {
                //gp2.AddString(c.ToString(), new FontFamily("CGXAERO-MorseCode"), (int)fontStyle, newFont.Size * g.DpiY / 72F, posMorse, format);
                g.DrawString(c.ToString(), newFontMorse, new SolidBrush(Color.Black), posMorse, format);
                posMorse = new PointF(posMorse.X, posMorse.Y + fSizeMorse.Height);
                fHeightUsedMorse += fSizeMorse.Height;
            }

            float fLargerHighOffset = fHeightUsed;
            if (fHeightUsedMorse > fHeightUsed)
            {
                posText = new PointF(posText.X, posText.Y + (fHeightUsedMorse / 2) - stringfSize.Height / 2);
                fLargerHighOffset = fHeightUsedMorse;
            }


            //gp2.AddString(sText + " ", newFont.FontFamily, (int)fontStyle, newFont.Size * g.DpiY / 72F, posText, format);
            g.DrawString(sText + " ", newFont, new SolidBrush(Color.Black), posText, format);

            pos.Y += fLargerHighOffset;
        }

        public static void DrawBullet(Graphics g, Color color, Bullet.style bulletStyle, SizeF stringfSize, PointF textPosition)
        {
            RectangleF rectF = new RectangleF(textPosition, stringfSize);

            PointF[] bullet = new PointF[7];
            bullet[0] = new PointF(rectF.X, rectF.Y);
            bullet[1] = new PointF(rectF.X + rectF.Width, rectF.Y);
            bullet[2] = new PointF(rectF.X + rectF.Width + (rectF.Height / 2), rectF.Y + (rectF.Height / 2));
            bullet[3] = new PointF(rectF.X + rectF.Width, rectF.Y + rectF.Height);
            bullet[4] = new PointF(rectF.X, rectF.Y + rectF.Height);
            bullet[5] = new PointF(rectF.X - (rectF.Height / 2), rectF.Y + (rectF.Height / 2));
            bullet[6] = new PointF(rectF.X, rectF.Y);

            if ((bulletStyle == Bullet.style.Left) || (bulletStyle == Bullet.style.None)) bullet[2] = bullet[1];
            if ((bulletStyle == Bullet.style.Right) || (bulletStyle == Bullet.style.None)) bullet[5] = bullet[4];

            //g.FillPolygon(new SolidBrush(Color.White), bullet);
            g.DrawLines(new Pen(color), bullet);
        }
    }
}

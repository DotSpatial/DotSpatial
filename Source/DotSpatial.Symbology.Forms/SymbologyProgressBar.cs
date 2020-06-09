// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SymbologyProgressBar.
    /// </summary>
    [ToolboxItem(false)]
    public class SymbologyProgressBar : ProgressBar, IProgressHandler
    {
        #region Fields

        private string _message;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyProgressBar"/> class.
        /// </summary>
        public SymbologyProgressBar()
        {
            ShowMessage = true;
            SetStyle(ControlStyles.UserPaint, true);
            FontColor = Color.Black;
        }

        #endregion

        private delegate void UpdateProg(string key, int percent, string message);

        #region Properties

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the color of the message text.")]
        public Color FontColor { get; set; }

        /// <summary>
        /// Gets or sets the progress message.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to draw the status message on the progress bar.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets a value indicating whether to draw the current message on the bar.")]
        public bool ShowMessage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread.
        /// </summary>
        /// <param name="key">A string message with just a description of what is happening, but no percent completion information.</param>
        /// <param name="percent">The integer percent from 0 to 100.</param>
        /// <param name="message">A message.</param>
        public void Progress(string key, int percent, string message)
        {
            if (InvokeRequired)
            {
                UpdateProg prg = UpdateProgress;
                BeginInvoke(prg, key, percent, message);
            }
            else
            {
                UpdateProgress(key, percent, message);
            }
        }

        /// <summary>
        /// Controls the drawing of this bar.
        /// </summary>
        /// <param name="e">The PaintEventArgs for this paint action.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SetClip(e.ClipRectangle);
                using (Brush b = new SolidBrush(BackColor))
                {
                    g.FillRectangle(b, ClientRectangle);
                }

                RectangleF r = new RectangleF(0f, 0f, Width - 1, Height - 1);
                using (GraphicsPath gp = new GraphicsPath())
                {
                    using (LinearGradientBrush lgb = new LinearGradientBrush(r, Color.LightGreen, Color.Green, LinearGradientMode.Vertical))
                    {
                        gp.AddRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), 4);
                        int w = Value * (Width - 1) / 100;
                        RectangleF backup = e.Graphics.ClipBounds;
                        g.SetClip(new Rectangle(0, 0, w, Height), CombineMode.Intersect);
                        g.FillPath(lgb, gp);
                        g.SetClip(backup, CombineMode.Replace);
                    }

                    g.DrawPath(Pens.Gray, gp);
                }

                if (!ShowMessage) return;

                StringFormat fmt = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using (Brush fontBrush = new SolidBrush(FontColor))
                {
                    g.DrawString(_message, Font, fontBrush, r, fmt);
                }
            }

            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
        }

        /// <summary>
        /// Prevent flicker.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // no code here
        }

        private void UpdateProgress(string key, int percent, string message)
        {
            Value = percent;
            _message = message;
        }

        #endregion
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Forms.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
#if !PocketPC

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a collection of rendering performance and quality settings.
    /// </summary>
    /// <remarks>
    /// 	<para>This class is used to control the quality of all painting operations in
    ///     GIS.NET. Settings are biased either towards rendering performance, quality, or a
    ///     compromise between the two. The <strong>HighPerformance</strong> static member is
    ///     used to paint quickly at the cost of quality; the <strong>HighQuality</strong>
    ///     member paints better-looking results but painting operations require more time. The
    ///     <strong>Balanced</strong> member provides minimal quality improvements while
    ///     preserving moderate rendering speed.</para>
    /// </remarks>
    [TypeConverter("DotSpatial.Positioning.Design.GraphicsSettingsConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=3ed3cdf4fdda3400")]
#endif
    public sealed class GraphicsSettings
    {
        private CompositingQuality _compositingQuality; //Gets or sets the rendering quality of composited images drawn to this Graphics.
        private InterpolationMode _interpolationMode; //Gets or sets the interpolation mode associated with this Graphics.
        private PixelOffsetMode _pixelOffsetMode; //Gets or set a value specifying how pixels are offset during rendering of this Graphics.
        private SmoothingMode _smoothingMode; //Gets or sets the rendering quality for this Graphics.
        private TextRenderingHint _textRenderingHint; //Gets or sets the rendering mode for text associated with this Graphics.
        private int _textContrast; //Gets or sets the gamma correction value for rendering text.

        #region Fields

        /// <summary>
        /// Represents graphics settings balanced between quality and rendering performance.
        /// </summary>
        /// <value>A <strong>GraphicsSettings</strong> object.</value>
        /// <remarks>
        /// 	<para>When this setting is used, painting operations will enable a few
        ///     quality-improving features while still allowing for faster rendering performance.
        ///     This quality setting is recommended for "draft" quality, where a more responsive
        ///     map is preferable to quality.</para>
        /// </remarks>
        public static readonly GraphicsSettings Balanced = new GraphicsSettings(CompositingQuality.AssumeLinear,
                                                                                InterpolationMode.Low, PixelOffsetMode.HighSpeed,
                                                                                SmoothingMode.HighQuality, TextRenderingHint.AntiAlias, 4);
        /// <summary>
        /// Represents graphics settings which maximize quality.
        /// </summary>
        /// <value>A <strong>GraphicsSettings</strong> object.</value>
        /// <remarks>
        /// This is the default setting used by the GIS.NET rendering engine. All possible
        /// smoothing features are enabled, including anti-aliasing, bicubic image interpolation,
        /// and ClearText. With this setting, the smallest geographic features and even most text
        /// are readable. This setting is recommended for production use, as well as
        /// printing.
        /// </remarks>
        public static readonly GraphicsSettings HighQuality = new GraphicsSettings(CompositingQuality.HighSpeed,
                                                                                InterpolationMode.HighQualityBicubic, PixelOffsetMode.HighQuality,
                                                                                SmoothingMode.HighQuality, TextRenderingHint.AntiAliasGridFit, 4);
        /// <summary>
        /// Represents graphics settings which maximize rendering performance.
        /// </summary>
        /// <value>A <strong>GraphicsSettings</strong> object.</value>
        /// <remarks>
        /// When this setting is used, all quality enhancement features are disabled. The
        /// resulting map will render more quickly, but the resulting quality will be hardly
        /// suitable for production use. This setting is best suited for situations where panning
        /// and zooming performance must have all possible speed.
        /// </remarks>
        public static readonly GraphicsSettings HighPerformance = new GraphicsSettings(CompositingQuality.HighSpeed,
                                                                                InterpolationMode.Low, PixelOffsetMode.None,
                                                                                SmoothingMode.None, TextRenderingHint.SingleBitPerPixel, 4);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        internal GraphicsSettings()
        { }

        /// <summary>
        /// Creates a new instance using the specified settings.
        /// </summary>
        /// <param name="compositingQuality"></param>
        /// <param name="interpolationMode"></param>
        /// <param name="pixelOffsetMode"></param>
        /// <param name="smoothingMode"></param>
        /// <param name="textRenderingHint"></param>
        /// <param name="textContrast"></param>
        internal GraphicsSettings(
                CompositingQuality compositingQuality,
                InterpolationMode interpolationMode,
                PixelOffsetMode pixelOffsetMode,
                SmoothingMode smoothingMode,
                TextRenderingHint textRenderingHint,
                int textContrast)
        {
            _compositingQuality = compositingQuality;
            _interpolationMode = interpolationMode;
            _pixelOffsetMode = pixelOffsetMode;
            _smoothingMode = smoothingMode;
            _textContrast = textContrast;
            _textRenderingHint = textRenderingHint;
        }

        #endregion

        #region Public members

        /// <summary>
        /// Controls the technique used to merge bitmap images.
        /// </summary>
        public CompositingQuality CompositingQuality
        {
            get { return _compositingQuality; }
            set { _compositingQuality = value; }
        }

        /// <summary>
        /// Controls the method used to calculate color values between pixels.
        /// </summary>
        public InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set { _interpolationMode = value; }
        }

        /// <summary>
        /// Controls the amount of shift for each pixel to improve anti-aliasing.
        /// </summary>
        public PixelOffsetMode PixelOffsetMode
        {
            get { return _pixelOffsetMode; }
            set { _pixelOffsetMode = value; }
        }

        /// <summary>
        /// Controls the technique used to blend edges.
        /// </summary>
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }

        /// <summary>
        /// Controls the technique used to smoothen the edges of text.
        /// </summary>
        public TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }
            set { _textRenderingHint = value; }
        }

        /// <summary>
        /// Controls the amount of gamma correction applied to text.
        /// </summary>
        public int TextContrast
        {
            get { return _textContrast; }
            set { _textContrast = value; }
        }

        /// <summary>
        /// Applies the graphics settings to the specified grahpics container.
        /// </summary>
        /// <param name="graphics">A <strong>Graphics</strong> object.</param>
        public void Apply(Graphics graphics)
        {
            graphics.CompositingQuality = _compositingQuality;
            graphics.InterpolationMode = _interpolationMode;
            graphics.PixelOffsetMode = _pixelOffsetMode;
            graphics.SmoothingMode = _smoothingMode;
            graphics.TextRenderingHint = _textRenderingHint;
            graphics.TextContrast = _textContrast;
        }

        #endregion
    }
}

#endif
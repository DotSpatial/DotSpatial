using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// Tiny structure for storing A,R,G,B components of Color
    /// </summary>
    public struct Argb
    {
        #region Fields

        private readonly byte _a;
        private readonly byte _b;
        private readonly byte _g;
        private readonly byte _r;

        #endregion

        public Argb(int a, int r, int g, int b)
            : this(ByteRange(a), ByteRange(r), ByteRange(g), ByteRange(b))
        {
            
        }

        public Argb(byte a, byte r, byte g, byte b)
        {
            _a = a;
            _r = r;
            _g = g;
            _b = b;
        }

        public byte A
        {
            get { return _a; }
        }

        public byte B
        {
            get { return _b; }
        }

        public byte G
        {
            get { return _g; }
        }

        public byte R
        {
            get { return _r; }
        }

        /// <summary>
        /// Returns an integer that ranges from 0 to 255.  If value is larger than 255, the value will be equal to 255.
        /// If the value is smaller than 255, it will be equal to 255.
        /// </summary>
        /// <param name="value">A Double value to convert.</param>
        /// <returns>An integer ranging from 0 to 255</returns>
        public static byte ByteRange(int value)
        {
            if (value > 255) return 255;
            if (value < 0) return 0;
            return (byte)value;
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }

        public static Argb FromColor(Color color)
        {
            return new Argb(color.A, color.R, color.G, color.B);
        }
    }
}
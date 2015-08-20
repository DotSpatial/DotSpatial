using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DotSpatial.Plugins.WebMap
{
    public class ServiceProvider
    {
        public ServiceProvider(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
        }

        #region Public Properties

        public string Name { get; private set; }
        public virtual bool NeedConfigure { get; protected set; }
        public Func<bool> Configure { get; protected set; } 

        #endregion

        #region Public Methods

        public virtual Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            return null;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

        protected static Bitmap ExceptionToBitmap(Exception ex, int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawString(ex.Message, new Font(FontFamily.GenericSansSerif, 14), new SolidBrush(Color.Black),
                        new RectangleF(0, 0, width, height));
                }

                using (var m = new MemoryStream())
                {
                    bitmap.Save(m, ImageFormat.Png);
                    return new Bitmap(m);
                }
            }
        }
    }
}
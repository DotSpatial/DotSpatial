// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.WebMap
{
    /// <summary>
    /// The base implementation for service providers.
    /// </summary>
    public class ServiceProvider
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProvider"/> class.
        /// </summary>
        /// <param name="name">Name of the service provider.</param>
        public ServiceProvider(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets.
        /// </summary>
        public Func<bool> Configure { get; protected set; }

        /// <summary>
        /// Gets the name of the service provider.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether configuration is needed.
        /// </summary>
        public virtual bool NeedConfigure { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a bitmap.
        /// </summary>
        /// <param name="x">The tile number in x direction.</param>
        /// <param name="y">The tile number in y direction.</param>
        /// <param name="envelope">The envelope for which to get the tiles.</param>
        /// <param name="zoom">The zoom level for which to get the tile.</param>
        /// <returns>Null on error, otherwise the resulting bitmap.</returns>
        public virtual Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            return null;
        }

        /// <summary>
        /// Returns the name of the service provider.
        /// </summary>
        /// <returns>The service provider name.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Takes the given exception an prints it to bitmap.
        /// </summary>
        /// <param name="ex">Exeption that should be printed.</param>
        /// <param name="width">Width of the bitmap.</param>
        /// <param name="height">Height of the bitmap.</param>
        /// <returns>The bitmap with the exception.</returns>
        protected static Bitmap ExceptionToBitmap(Exception ex, int width, int height)
        {
            using var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawString(ex.Message, new Font(FontFamily.GenericSansSerif, 14), new SolidBrush(Color.Black), new RectangleF(0, 0, width, height));
            }

            using var m = new MemoryStream();
            bitmap.Save(m, ImageFormat.Png);
            return new Bitmap(m);
        }

        #endregion
    }
}
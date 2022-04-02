// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using DotSpatial.Plugins.WebMap.Tiling;
using Microsoft.VisualBasic;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.WebMap
{
    /// <summary>
    /// This can be used to work with services other than the ones already defined.
    /// </summary>
    public class OtherServiceProvider : ServiceProvider
    {
        #region Fields

        private string _url;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OtherServiceProvider"/> class.
        /// </summary>
        /// <param name="name">Name of the service provider.</param>
        /// <param name="url">Url of the service provider.</param>
        public OtherServiceProvider(string name, string url)
            : base(name)
        {
            _url = url;
            Configure = () =>
                {
                    var dialogDefault = string.IsNullOrWhiteSpace(_url) ? "http://tiles.virtualearth.net/tiles/h{key}.jpeg?g=461&mkt=en-us&n=z" : _url;
                    var guiUrl = Interaction.InputBox("Please provide the Url for the service.", DefaultResponse: dialogDefault);
                    if (!string.IsNullOrWhiteSpace(guiUrl))
                    {
                        _url = guiUrl;
                        return true;
                    }

                    return false;
                };
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public override bool NeedConfigure => string.IsNullOrWhiteSpace(_url);

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            try
            {
                var url = _url;
                if (url == null)
                {
                    return null;
                }

                if (url.Contains("{key}"))
                {
                    var quadKey = TileCalculator.TileXyToBingQuadKey(x, y, zoom);
                    url = url.Replace("{key}", quadKey);
                }
                else
                {
                    url = url.Replace("{zoom}", zoom.ToString(CultureInfo.InvariantCulture));
                    url = url.Replace("{x}", x.ToString(CultureInfo.InvariantCulture));
                    url = url.Replace("{y}", y.ToString(CultureInfo.InvariantCulture));
                }

                using var client = new WebClient();
                var stream = client.OpenRead(url);
                if (stream != null)
                {
                    var bitmap = new Bitmap(stream);
                    stream.Flush();
                    stream.Close();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException || ex is TimeoutException)
                {
                    return ExceptionToBitmap(ex, 256, 256);
                }

                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        #endregion
    }
}
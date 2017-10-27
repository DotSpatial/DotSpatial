// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using BruTile;
using BruTile.Web;
using BruTile.Tms;
using DotSpatial.Plugins.WebMap.Helper;

namespace DotSpatial.Plugins.WebMap.WMS
{
    /// <summary>
    /// A tile source for wms services.
    /// </summary>
    public class WmsTileSource : TileSource
    {
        #region  Constructors

        private WmsTileSource(ITileProvider tileProvider, ITileSchema tileSchema)
            : base(tileProvider, tileSchema)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new WmsTileSource
        /// </summary>
        /// <param name="info">WmsInfo with the data needed for creation.</param>
        /// <returns>The created WmsTileSource</returns>
        public static WmsTileSource Create(WmsInfo info)
        {
            var schema = new TileSchema
            {
                Format = "image/png",
                Srs = info.Crs,
            };
            const int tileWidth = 256;
            const int tileHeight = 256;

            var request = new WmsRequest(
                new Uri(info.WmsCapabilities.Capability.Request.GetMap.DCPType[0].Http.Get.OnlineResource.Href),
                schema,
                tileWidth,
                tileHeight,
                new List<string> { info.Layer.Name },
                info.Style == null ? null : new List<string> { info.Style },
                info.CustomParameters,
                info.WmsCapabilities.Version.VersionString);
            return new WmsTileSource(new HttpTileProvider(request, fetchTile: d => RequestHelper.FetchImage(d, info.Credentials)), schema);
        }

        #endregion
    }
}
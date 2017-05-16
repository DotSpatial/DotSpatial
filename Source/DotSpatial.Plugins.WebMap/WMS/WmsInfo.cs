// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Net;
using BruTile.Wms;
using DotSpatial.Projections;

namespace DotSpatial.Plugins.WebMap.WMS
{
    /// <summary>
    /// Wms info.
    /// </summary>
    public class WmsInfo
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WmsInfo"/> class.
        /// </summary>
        /// <param name="serverUrl">Url of the WMS server.</param>
        /// <param name="wmsCapabilities">The WmsCapabilities.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="customParameters">The custom parameters.</param>
        /// <param name="crs">The Crs.</param>
        /// <param name="projectionInfo">The CrsProjectionInfo.</param>
        /// <param name="style">The style.</param>
        /// <param name="credentials">Credentials for authentication.</param>
        public WmsInfo(string serverUrl, WmsCapabilities wmsCapabilities, Layer layer, Dictionary<string, string> customParameters, string crs, ProjectionInfo projectionInfo, string style, NetworkCredential credentials)
        {
            Credentials = credentials;
            CustomParameters = customParameters;
            Crs = crs;
            CrsProjectionInfo = projectionInfo;
            Style = style;
            Layer = layer;
            WmsCapabilities = wmsCapabilities;
            ServerUrl = serverUrl;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the credentials for authentication.
        /// </summary>
        public NetworkCredential Credentials { get; private set; }

        /// <summary>
        /// Gets the Crs.
        /// </summary>
        public string Crs { get; private set; }

        /// <summary>
        /// Gets the CrsProjectionInfo.
        /// </summary>
        public ProjectionInfo CrsProjectionInfo { get; private set; }

        /// <summary>
        /// Gets the custom parameters.
        /// </summary>
        public Dictionary<string, string> CustomParameters { get; private set; }

        /// <summary>
        /// Gets the layer.
        /// </summary>
        public Layer Layer { get; private set; }

        /// <summary>
        /// Gets the server Url.
        /// </summary>
        public string ServerUrl { get; private set; }

        /// <summary>
        /// Gets the style.
        /// </summary>
        public string Style { get; private set; }

        /// <summary>
        /// Gets the WmsCapabilities.
        /// </summary>
        public WmsCapabilities WmsCapabilities { get; private set; }

        #endregion
    }
}
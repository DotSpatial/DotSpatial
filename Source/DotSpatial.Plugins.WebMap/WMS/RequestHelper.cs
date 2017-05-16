// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Net;

namespace DotSpatial.Plugins.WebMap.WMS
{
    /// <summary>
    /// Helper class for fetching images.
    /// </summary>
    public static class RequestHelper
    {
        #region Methods

        /// <summary>
        /// Fetches the image from the given uri.
        /// </summary>
        /// <param name="uri">Uri to get the image from.</param>
        /// <param name="credentials">Credentials needed for login.</param>
        /// <returns>byte array width the gotten image data.</returns>
        public static byte[] FetchImage(Uri uri, ICredentials credentials)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Credentials = credentials;
            return BruTile.Web.RequestHelper.FetchImage(webRequest);
        }

        #endregion
    }
}
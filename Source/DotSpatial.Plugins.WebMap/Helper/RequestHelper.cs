// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using BruTile;
using BruTile.Extensions;

namespace DotSpatial.Plugins.WebMap.Helper
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

            return FetchImage(webRequest);
        }

        public static byte[] FetchImage(HttpWebRequest webRequest)
        {
            using (var webResponse = webRequest.GetSyncResponse(10000))
            {
                if (webResponse == null)
                {
                    throw new WebException("An error occurred while fetching tile", null);
                }

                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    return Utilities.ReadFully(responseStream);
                }
            }
        }

        #endregion
    }
}
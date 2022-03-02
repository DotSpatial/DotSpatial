// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// DataProviderEventArgs.
    /// </summary>
    public class DataProviderEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderEventArgs"/> class.
        /// </summary>
        /// <param name="providers">Specifies a list of IDataProviders.</param>
        public DataProviderEventArgs(IEnumerable<IDataProvider> providers)
        {
            Providers = providers;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of providers for this event.
        /// </summary>
        public IEnumerable<IDataProvider> Providers { get; protected set; }

        #endregion
    }
}
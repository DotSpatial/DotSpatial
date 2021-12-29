// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LayerProviders.
    /// </summary>
    public class LayerProviders : EventArgs
    {
        #region Fields

        private List<ILayerProvider> _providers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerProviders"/> class.
        /// </summary>
        /// <param name="providers">Specifies a list of IDataProviders.</param>
        public LayerProviders(List<ILayerProvider> providers)
        {
            _providers = providers;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of providers for this event.
        /// </summary>
        public virtual List<ILayerProvider> Providers
        {
            get
            {
                return _providers;
            }

            protected set
            {
                _providers = value;
            }
        }

        #endregion
    }
}
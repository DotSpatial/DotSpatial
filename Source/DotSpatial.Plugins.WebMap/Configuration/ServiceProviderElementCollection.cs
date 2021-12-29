// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Configuration;

namespace DotSpatial.Plugins.WebMap.Configuration
{
    /// <summary>
    /// A collection for ServiceProviderElements.
    /// </summary>
    public class ServiceProviderElementCollection : ConfigurationElementCollection
    {
        #region Methods

        /// <summary>
        /// Creates a new ServiceProviderElement.
        /// </summary>
        /// <returns>The created ServiceProviderElement.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceProviderElement();
        }

        /// <summary>
        /// Gets the key of the given element.
        /// </summary>
        /// <param name="element">Element to get the key from.</param>
        /// <returns>The key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceProviderElement)element).Key;
        }

        #endregion
    }
}
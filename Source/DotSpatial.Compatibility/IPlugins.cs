// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Interface for manipulating plugins of the IPlugin type.
    /// This differs from "CondensedPlugins" in that the Item ([]) property returns IPlugin
    /// instead of IPluginDetails. The original "Plugins" (this one) cannot be changed without breaking
    /// the backward compatibility of the interface.
    /// </summary>
    public interface IPlugins : IEnumerable
    {
        #region Properties

        /// <summary>
        /// Gets the number of available plugins.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets or Sets the default folder where plugins are loaded from
        /// </summary>
        string PluginFolder { get; set; }

        #endregion

        /// <summary>
        /// Gets an IPlugin object from the list of all loaded plugins
        /// </summary>
        /// <param name="index">0-based index into the list of plugins</param>
        IPlugin this[int index] { get; }

        #region Methods

        /// <summary>
        /// Adds any compatible plugins from a directory(recursive into subdirs)
        /// </summary>
        /// <param name="path">path to the directory</param>
        /// <returns>true on success, false otherwise</returns>
        bool AddFromDir(string path);

        /// <summary>
        /// Add a plugin from a file
        /// </summary>
        /// <param name="path">path to the plugin</param>
        /// <returns>true on success, false on failure</returns>
        bool AddFromFile(string path);

        /// <summary>
        /// Sends a broadcast message to all loaded plugins
        /// </summary>
        /// <param name="message">The message that should be sent</param>
        void BroadcastMessage(string message);

        /// <summary>
        /// clears all plugins from the list of available plugins, but doesn't unload loaded plugins
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns the key belonging to a plugin with the given name. An empty string is returned if the name is not found.
        /// </summary>
        /// <param name="pluginName">The name of the plugin</param>
        /// <returns>The key belonging to a plugin with the given name. An empty string is returned if the name is not found.</returns>
        string GetPluginKey(string pluginName);

        /// <summary>
        /// Loads a plugin from an instance of an object
        /// </summary>
        /// <param name="plugin">the Plugin object to load</param>
        /// <param name="pluginKey">The Key by which this plugin can be identified at a later time</param>
        /// <param name="settingsString">A string that contains any settings that should be passed to the plugin after it is loaded into the system</param>
        /// <returns>true on success, false otherwise</returns>
        bool LoadFromObject(IPlugin plugin, string pluginKey, string settingsString);

        /// <summary>
        /// Loads a plugin from an instance of an object
        /// </summary>
        /// <param name="plugin">the Plugin object to load</param>
        /// <param name="pluginKey">The Key by which this plugin can be identified at a later time</param>
        /// <returns>true on success, false otherwise</returns>
        bool LoadFromObject(IPlugin plugin, string pluginKey);

        /// <summary>
        /// Checks to see if a plugin is currently loaded (running)
        /// </summary>
        /// <param name="key">Unique key identifying the plugin</param>
        /// <returns>true if loaded, false otherwise</returns>
        bool PluginIsLoaded(string key);

        /// <summary>
        /// Removes a plugin from the list of available plugins and unloads the plugin if loaded
        /// </summary>
        /// <param name="indexOrKey">0-based integer index or string key for the plugin to remove</param>
        void Remove(object indexOrKey);

        /// <summary>
        /// Shows the dialog for loading/starting/stopping plugins
        /// </summary>
        void ShowPluginDialog();

        /// <summary>
        /// Starts (loads) a specified plugin
        /// </summary>
        /// <param name="key">Identifying key for the plugin to start</param>
        /// <returns>true on success, false otherwise</returns>
        bool StartPlugin(string key);

        /// <summary>
        /// Stops (unloads) a specified plugin
        /// </summary>
        /// <param name="key">Identifying key for the plugin to stop</param>
        void StopPlugin(string key);

        #endregion
    }
}
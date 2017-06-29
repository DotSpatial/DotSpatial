// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:50:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        /// <summary>
        /// number of available plugins
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets an IPlugin object from the list of all loaded plugins
        /// <param name="index">0-based index into the list of plugins</param>
        /// </summary>
        IPlugin this[int index] { get; }

        /// <summary>
        /// Gets or Sets the default folder where plugins are loaded from
        /// </summary>
        string PluginFolder { get; set; }

        /// <summary>
        /// clears all plugins from the list of available plugins, but doesn't unload loaded plugins
        /// </summary>
        void Clear();

        /// <summary>
        /// Add a plugin from a file
        /// </summary>
        /// <param name="path">path to the plugin</param>
        /// <returns>true on success, false on failure</returns>
        bool AddFromFile(string path);

        /// <summary>
        /// Adds any compatible plugins from a directory(recursive into subdirs)
        /// </summary>
        /// <param name="path">path to the directory</param>
        /// <returns>true on success, false otherwise</returns>
        bool AddFromDir(string path);

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

        /// <summary>
        /// Removes a plugin from the list of available plugins and unloads the plugin if loaded
        /// </summary>
        /// <param name="indexOrKey">0-based integer index or string key for the plugin to remove</param>
        void Remove(object indexOrKey);

        /// <summary>
        /// Checks to see if a plugin is currently loaded (running)
        /// </summary>
        /// <param name="key">Unique key identifying the plugin</param>
        /// <returns>true if loaded, false otherwise</returns>
        bool PluginIsLoaded(string key);

        /// <summary>
        /// Shows the dialog for loading/starting/stopping plugins
        /// </summary>
        void ShowPluginDialog();

        /// <summary>
        /// Sends a broadcast message to all loaded plugins
        /// </summary>
        /// <param name="message">The message that should be sent</param>
        void BroadcastMessage(string message);

        /// <summary>
        /// Returns the key belonging to a plugin with the given name. An empty string is returned if the name is not found.
        /// </summary>
        /// <param name="pluginName">The name of the plugin</param>
        string GetPluginKey(string pluginName);
    }
}
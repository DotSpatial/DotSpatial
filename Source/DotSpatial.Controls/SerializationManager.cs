// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using DotSpatial.Data.Properties;
using DotSpatial.Extensions;
using DotSpatial.Serialization;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Used for opening and saving project files.
    /// </summary>
    public class SerializationManager
    {
        #region Fields

        private static readonly ResourceManager Resources = new ResourceManager("DotSpatial.Controls.MessageStrings", Assembly.GetExecutingAssembly());

        private readonly AppManager _applicationManager;

        private readonly ProjectChangeTracker _changeTracker;

        private readonly Dictionary<string, object> _customSettings;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationManager"/> class.
        /// </summary>
        /// <param name="applicationManager">The application manager.</param>
        public SerializationManager(AppManager applicationManager)
        {
            _applicationManager = applicationManager;
            _customSettings = new Dictionary<string, object>();
            _changeTracker = new ProjectChangeTracker(_applicationManager.Map);
            _changeTracker.MapPropertyChanged += MapPropertyChanged;

            _applicationManager.SatisfyImportsExtensionsActivated += AppSatisfyImportsExtensionsActivated;

            SaveProjectFileProviders = new List<ISaveProjectFileProvider>(0);
            OpenProjectFileProviders = new List<IOpenProjectFileProvider>(0);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the appmanager is being deserialized. allowing a plugin to deal with any custom settings that were deserialized.
        /// </summary>
        public event EventHandler<SerializingEventArgs> Deserializing;

        /// <summary>
        /// Occurs when the dirty (unsaved) state of the current project is changed. This occurs when layers are added or removed to the map,
        /// when layer symbology is changed or when the map view extents are changed.
        /// </summary>
        public event EventHandler IsDirtyChanged;

        /// <summary>
        /// Occurs when a new project is created using the New() method. allowing a plugin to perform initialization  associated with the
        /// creation of a new project
        /// </summary>
        public event EventHandler<SerializingEventArgs> NewProjectCreated;

        /// <summary>
        /// Occurs when the appmanager is being serialized, allowing a plugin to view and modify custom settings that will be stored with the appmanager.
        /// </summary>
        public event EventHandler<SerializingEventArgs> Serializing;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current project directory.
        /// </summary>
        /// <value>
        /// The current project directory.
        /// </value>
        public string CurrentProjectDirectory { get; protected set; }

        /// <summary>
        /// Gets or sets the current project file.
        /// </summary>
        /// <value>
        /// The current project file.
        /// </value>
        public string CurrentProjectFile { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are some unsaved changes in the current project.
        /// </summary>
        public bool IsDirty { get; protected set; }

        /// <summary>
        /// Gets the open dialog filter format.
        /// </summary>
        public string OpenDialogFilterFormat => "{0} |*.map.xml;*.dspx|{1} (*.dspx)|*.dspx" + AggregateProviderExtensions(OpenProjectFileProviders);

        /// <summary>
        /// Gets the filter text for an open project file dialog.
        /// </summary>
        public string OpenDialogFilterText => string.Format(OpenDialogFilterFormat, Resources.GetString("SupportedFiles"), Resources.GetString("ProjectFile"));

        /// <summary>
        /// Gets the open project file providers.
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IOpenProjectFileProvider> OpenProjectFileProviders { get; private set; }

        /// <summary>
        /// Gets the save dialog filter format.
        /// </summary>
        public string SaveDialogFilterFormat => "{0} (*.dspx)|*.dspx" + AggregateProviderExtensions(SaveProjectFileProviders);

        /// <summary>
        /// Gets the filter text for a save project as dialog.
        /// </summary>
        public string SaveDialogFilterText => string.Format(SaveDialogFilterFormat, Resources.GetString("ProjectFile"));

        /// <summary>
        /// Gets the save project file providers.
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<ISaveProjectFileProvider> SaveProjectFileProviders { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the custom setting.
        /// </summary>
        /// <typeparam name="T">The generic type of the output.</typeparam>
        /// <param name="uniqueName">Name of the unique key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The found custom settings.</returns>
        public T GetCustomSetting<T>(string uniqueName, T defaultValue)
        {
            object result;
            if (_customSettings.TryGetValue(uniqueName, out result)) return (T)result;
            return defaultValue;
        }

        /// <summary>
        /// Determines whether a uniqueName is already in use.
        /// </summary>
        /// <param name="uniqueName">Key to test.</param>
        /// <returns>
        ///   <c>true</c> if uniqueName is already in use; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCustomSettingPresent(string uniqueName)
        {
            return _customSettings.ContainsKey(uniqueName);
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        public void New()
        {
            _applicationManager.Map.ClearLayers();
            SetCurrentProjectDirectory(string.Empty);

            IsDirty = false;
            OnIsDirtyChanged();

            _changeTracker.Map = _applicationManager.Map;

            OnNewProject(new SerializingEventArgs());
        }

        /// <summary>
        /// Triggers the Deserializing event.
        /// </summary>
        /// <param name="ea">The event arguments.</param>
        public virtual void OnDeserializing(SerializingEventArgs ea)
        {
            Deserializing?.Invoke(this, ea);
        }

        /// <summary>
        /// Triggers the IsDirtyChanged event.
        /// </summary>
        public virtual void OnIsDirtyChanged()
        {
            IsDirtyChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Triggers the NewProjectCreated event.
        /// </summary>
        /// <param name="ea">The event arguments.</param>
        public virtual void OnNewProject(SerializingEventArgs ea)
        {
            NewProjectCreated?.Invoke(this, ea);
        }

        /// <summary>
        /// Triggers the Serializing event.
        /// </summary>
        /// <param name="ea">The event arguments.</param>
        public virtual void OnSerializing(SerializingEventArgs ea)
        {
            Serializing?.Invoke(this, ea);
        }

        /// <summary>
        /// Deserializes the map from a file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void OpenProject(string fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName), "fileName is null or empty.");
            Contract.Requires(_applicationManager.Map != null);

            _applicationManager.Map.ClearLayers();
            SetCurrentProjectDirectory(fileName);

            string extension = Path.GetExtension(fileName);

            bool isProviderPresent = false;
            foreach (var provider in OpenProjectFileProviders)
            {
                if (string.Equals(provider.Extension, extension, StringComparison.OrdinalIgnoreCase))
                {
                    provider.Open(fileName);
                    isProviderPresent = true;
                }
            }

            if (!isProviderPresent)
            {
                OpenProjectFile(fileName);
            }

            AddFileToRecentFiles(fileName);

            _changeTracker.Map = _applicationManager.Map;
            IsDirty = false;
            OnIsDirtyChanged();

            OnDeserializing(new SerializingEventArgs());
        }

        /// <summary>
        /// This method re-sets the map projection and re-projects all map layers to the projection that is specified in the project file.
        /// </summary>
        public void ResetMapProjection()
        {
            var dspxProjectionEsriString = _applicationManager?.Map?.Projection.ToEsriString();
            if (string.IsNullOrEmpty(dspxProjectionEsriString)) return;

            _applicationManager.Map.MapFrame.ReprojectMapFrame(dspxProjectionEsriString);
            _applicationManager.Map.MapFrame.ResetExtents();
        }

        /// <summary>
        /// Serializes portions of the map to file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveProject(string fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName), "fileName is null or empty.");
            Contract.Requires(_applicationManager.Map != null);

            SetCurrentProjectDirectory(fileName);

            OnSerializing(new SerializingEventArgs());

            var graph = CreateObjectGraph(_applicationManager, new string[0]);

            XmlSerializer s = new XmlSerializer();
            string xml = s.Serialize(graph);

            bool isProviderPresent = false;
            string extension = Path.GetExtension(fileName);

            foreach (ISaveProjectFileProvider provider in SaveProjectFileProviders)
            {
                if (string.Equals(provider.Extension, extension, StringComparison.OrdinalIgnoreCase))
                {
                    provider.Save(fileName, xml);
                    isProviderPresent = true;
                }
            }

            if (!isProviderPresent)
            {
                switch (extension)
                {
                    case ".dspx":
                        File.WriteAllText(fileName, xml);
                        break;
                    default:
                        throw new NotImplementedException("There isn't an implementation for saving that file type.");
                }
            }

            AddFileToRecentFiles(fileName);

            IsDirty = false;
            OnIsDirtyChanged();
        }

        /// <summary>
        /// Sets the custom setting.
        /// </summary>
        /// <param name="uniqueName">Name of the unique key.</param>
        /// <param name="value">The value.</param>
        public void SetCustomSetting(string uniqueName, object value)
        {
            // We avoid throwing an exception is the value is already set to allow settings to be persisted and available
            // even when the originating plugin is not running.
            _customSettings[uniqueName] = value;
        }

        /// <summary>
        /// Starts tracking changes in the current project.
        /// </summary>
        public void StartTrackingChanges()
        {
            _changeTracker.Map = _applicationManager.Map;
        }

        private static void AddFileToRecentFiles(string fileName)
        {
            if (Settings.Default.RecentFiles.Contains(fileName))
            {
                Settings.Default.RecentFiles.Remove(fileName);
            }

            if (Settings.Default.RecentFiles.Count >= Settings.Default.MaximumNumberOfRecentFiles) Settings.Default.RecentFiles.RemoveAt(Settings.Default.RecentFiles.Count - 1);

            // insert value at the top of the list
            Settings.Default.RecentFiles.Insert(0, fileName);

            Settings.Default.Save();
        }

        private static string AggregateProviderExtensions(IEnumerable<IProjectFileProvider> providers)
        {
            return providers.Aggregate(new StringBuilder(), (sb, p) => sb.AppendFormat("|{1} (*{0})|*{0}", p.Extension, p.FileTypeDescription), s => s.ToString());
        }

        private void AppSatisfyImportsExtensionsActivated(object sender, EventArgs e)
        {
            _changeTracker.Map = _applicationManager.Map;
        }

        private void AssignLayerSymbologies(IMapFrame mapFrame)
        {
            foreach (ILayer layer in mapFrame.GetAllLayers())
            {
                IMapLineLayer lineLayer = layer as IMapLineLayer;
                ILineScheme original = lineLayer?.Symbology;
                if (original != null)
                {
                    var newScheme = original.Clone() as ILineScheme;
                    original.CopyProperties(newScheme);
                    original.ResumeEvents();
                }

                // to correctly draw categories:
                IMapFeatureLayer featureLayer = layer as IMapFeatureLayer;
                if (featureLayer?.Symbology.NumCategories > 1)
                {
                    featureLayer.DataSet.FillAttributes();
                    featureLayer.ApplyScheme(featureLayer.Symbology);
                }
            }
        }

        private void AssignParentGroups(IGroup parentGroup, IMapFrame parentMapFrame)
        {
            // this method will assign the parent groups.
            // it needs to be applied after opening project so that none of
            // the parent groups are NULL.
            foreach (ILayer child in parentGroup.GetLayers())
            {
                var childGroup = child as IGroup;
                if (childGroup != null)
                {
                    AssignParentGroups(childGroup, parentMapFrame);
                }

                child.SetParentItem(parentGroup);
                child.MapFrame = parentMapFrame;
            }
        }

        private object[] CreateObjectGraph(AppManager appManager, string[] activePlugins)
        {
            // to improve backwards compatibility the objects may need to remain in the same order.
            var graph = new object[] { activePlugins, appManager.Map, _customSettings };
            return graph;
        }

        private void MapPropertyChanged(object sender, EventArgs e)
        {
            IsDirty = true;
            OnIsDirtyChanged();
        }

        private void OpenProjectFile(string fileName)
        {
            var graph = CreateObjectGraph(_applicationManager, null);
            XmlDeserializer d = new XmlDeserializer();

            // why does deserialize take an object and then insist on creating new objects?
            // it decides to create new objects for anything nest more than one level deep.
            // I have tweaked it a little for arrays.
            d.Deserialize(graph, File.ReadAllText(fileName));

            ResetMapProjection();

            _applicationManager.Map.Invalidate();

            // temporary fix by Jiri to properly assign the parent groups
            if (_applicationManager.Map.MapFrame != null)
            {
                AssignParentGroups(_applicationManager.Map.MapFrame, _applicationManager.Map.MapFrame);
                AssignLayerSymbologies(_applicationManager.Map.MapFrame);
            }

            // end temporary fix
        }

        private void SetCurrentProjectDirectory(string fileName)
        {
            // we set the working directory to the location of the project file. All filenames will be relative to this path.
            if (string.IsNullOrWhiteSpace(fileName))
            {
                CurrentProjectFile = CurrentProjectDirectory = string.Empty;
            }
            else
            {
                var path = Path.GetDirectoryName(fileName);
                if (path != null)
                {
                    Directory.SetCurrentDirectory(path);
                    CurrentProjectFile = fileName;
                    CurrentProjectDirectory = Path.GetDirectoryName(fileName);
                }
                else
                {
                    throw new ArgumentNullException(nameof(path), string.Format(MessageStrings.ProjectDirectoryMayNotBeARootDirectory, fileName));
                }
            }
        }

        #endregion
    }
}
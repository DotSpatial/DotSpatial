// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for a predefined symbolizer which can be stored in a xml symbolizer file.
    /// </summary>
    public interface ICustomSymbolizer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the string group for this predefined symbolizer.
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the string name for this predefined symbolizer.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer for this predifined symbolizer.
        /// </summary>
        IFeatureSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets the symbolizer type of this predefined symbolizer.
        /// </summary>
        SymbolizerType SymbolType { get; }

        /// <summary>
        /// Gets or sets the unique name for this predefined symbolizer.
        /// </summary>
        string UniqueName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Jiri's code to load from XML.
        /// </summary>
        /// <param name="xmlDataSource">The xml Data source to load the symbology from.</param>
        /// <param name="uniqueName">A Unique name for the symbology item.</param>
        void LoadFromXml(string xmlDataSource, string uniqueName);

        /// <summary>
        /// Jiri's code to load from XML.
        /// </summary>
        /// <param name="xmlDataSource">The xml Data source to load the symbology from.</param>
        /// <param name="group">The organizational group or category.</param>
        /// <param name="name">The string name within the specified group or category.</param>
        void LoadFromXml(string xmlDataSource, string group, string name);

        /// <summary>
        /// Jiri's code to save to XML.
        /// </summary>
        /// <param name="xmlDataSource">The xml data source to load the symbology from.</param>
        void SaveToXml(string xmlDataSource);

        #endregion
    }
}
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:20:27 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Project
    /// </summary>
    public interface IProject
    {
        #region Methods

        /// <summary>
        /// Loads a project file.
        /// </summary>
        /// <param name="fileName">Filename of the project to load.</param>
        /// <returns>Returns true if successful.</returns>
        bool Load(string fileName);

        /// <summary>
        /// Loads a project file into the current project as a subgroup.
        /// </summary>
        /// <param name="fileName">Filename of the project to load.</param>
        void LoadIntoCurrentProject(string fileName);

        /// <summary>
        /// Saves the current project.
        /// </summary>
        /// <param name="fileName">Filename to save the project as.</param>
        /// <returns>Returns true if successful.</returns>
        bool Save(string fileName);

        /// <summary>
        /// Saves the current configuration to a configuration file.
        /// </summary>
        /// <param name="fileName">The fileName to save the configuration as.</param>
        /// <returns>Returns true if successful.</returns>
        bool SaveConfig(string fileName);

        #endregion

        #region Properties

        /// <summary>
        /// Returns the fileName of the configuration file that was used to load the current DotSpatial project.
        /// </summary>
        string ConfigFileName { get; }

        /// <summary>
        /// Returns whether the configuration file specified by the project has been loaded.
        /// </summary>
        bool ConfigLoaded { get; }

        /// <summary>
        /// Returns the fileName of the current project.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Return or set the current map units, in the format
        /// "Meters", "Feet", etc
        /// </summary>
        string MapUnits { get; set; }

        /// <summary>
        /// Return or set the current alternate display units, in the format
        /// "Meters", "Feet", etc
        /// </summary>
        string MapUnitsAlternate { get; set; }

        /// <summary>
        /// Gets or sets the project modified flag.
        /// </summary>
        bool Modified { get; set; }

        /// <summary>
        /// Return or set the current project projection, in the format
        /// "+proj=tmerc +ellps=WGS84 etc etc +datum=WGS84"
        /// </summary>
        string ProjectProjection { get; set; }

        /// <summary>
        /// Returns an ArrayList of the recent projects (the full path to the projects)
        /// </summary>
        ArrayList RecentProjects { get; }

        /// <summary>
        /// Gets or sets the option to save shape-level formatting.
        /// </summary>
        bool SaveShapeSettings { get; set; }

        #endregion
    }
}
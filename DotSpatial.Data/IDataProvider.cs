// ********************************************************************************************************
// Product Name: DotSpatial.PluginInterfaces.dll Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.PluginInterfaces.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel.Composition;

namespace DotSpatial.Data
{
    /// <summary>
    /// IDataProvider is what you implement to expand the data handling methods of DotSpatial
    /// </summary>
    [InheritedExport]
    public interface IDataProvider
    {
        #region Methods

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="name">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        IDataSet Open(string name);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimited
        /// by the | symbol.  Each will appear in DotSpatial's open file dialog filter, preceded by the name provided
        /// on this object.
        /// </summary>
        string DialogReadFilter
        {
            get;
        }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceded by the name provided on this object.
        /// In addition, the same extension mapping will be used in order to pair a string driver code to the
        /// extension.
        /// </summary>
        string DialogWriteFilter
        {
            get;
        }

        /// <summary>
        /// Gets a preferably short name that identifies this data provider.  Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets or sets the progress handler to use.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }

        /// <summary>
        /// This provides a basic description of what your provider does.
        /// </summary>
        string Description
        {
            get;
        }

        #endregion
    }
}
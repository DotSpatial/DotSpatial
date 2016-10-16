// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 4:50:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// DataProviders
    /// </summary>
    public class DataProviderEventArgs : EventArgs
    {
        #region Private Variables

        private IEnumerable<IDataProvider> _providers;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DataProviders
        /// </summary>
        /// <param name="providers">Specifies a list of IDataProviders</param>
        public DataProviderEventArgs(IEnumerable<IDataProvider> providers)
        {
            _providers = providers;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of providers for this event.
        /// </summary>
        public virtual IEnumerable<IDataProvider> Providers
        {
            get { return _providers; }
            protected set { _providers = value; }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Helper Functions

        #endregion
    }
}
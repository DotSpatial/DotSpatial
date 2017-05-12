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
    /// DataProviderEventArgs
    /// </summary>
    public class DataProviderEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderEventArgs"/> class.
        /// </summary>
        /// <param name="providers">Specifies a list of IDataProviders</param>
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
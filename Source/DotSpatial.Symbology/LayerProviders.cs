// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/08/2008 10:58:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DataProviders
    /// </summary>
    public class LayerProviders : EventArgs
    {
        #region Private Variables

        private List<ILayerProvider> _providers;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DataProviders
        /// </summary>
        /// <param name="providers">Specifies a list of IDataProviders</param>
        public LayerProviders(List<ILayerProvider> providers)
        {
            _providers = providers;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of providers for this event.
        /// </summary>
        public virtual List<ILayerProvider> Providers
        {
            get { return _providers; }
            protected set { _providers = value; }
        }

        #endregion
    }
}
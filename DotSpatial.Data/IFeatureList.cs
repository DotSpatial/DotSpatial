// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/12/2009 3:28:34 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// IFeatureList
    /// </summary>
    public interface IFeatureList : IList<IFeature>
    {
        #region Events

        /// <summary>
        /// Occurs when a new feature is added to the list
        /// </summary>
        event EventHandler<FeatureEventArgs> FeatureAdded;

        /// <summary>
        /// Occurs when a feature is removed from the list.
        /// </summary>
        event EventHandler<FeatureEventArgs> FeatureRemoved;

        #endregion

        #region Methods

        /// <summary>
        /// Resumes events
        /// </summary>
        void ResumeEvents();

        /// <summary>
        /// Temporarilly disables events
        /// </summary>
        void SuspendEvents();

        /// <summary>
        /// This is a re-expression of the features using a strong typed
        /// list.  This may be the inner list or a copy depending on
        /// implementation.
        /// </summary>
        /// <returns>The features as a List of IFeature.</returns>
        List<IFeature> ToList();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether or not the events have been suspended
        /// </summary>
        bool EventsSuspended
        {
            get;
        }

        /// <summary>
        /// If this is false, then features will be added to the list without copying over attribute Table information.
        /// This will allow the attributes to be loaded in a more on-demand later.
        /// </summary>
        bool IncludeAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the parent featureset for this list.
        /// </summary>
        IFeatureSet Parent
        {
            get;
        }

        #endregion
    }
}
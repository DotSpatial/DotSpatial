// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// A very generic interface that is implemented by any dataset, regardless of what kinds of data that it has.
    /// </summary>
    public interface IDataSet : IDisposable, IDisposeLock, IReproject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the extent for the dataset. Usages to Envelope were replaced
        /// as they required an explicit using to DotSpatial.Topology which is not
        /// as intuitive. Extent.ToEnvelope() and new Extent(myEnvelope) convert them.
        /// </summary>
        Extent Extent { get; set; }

        /// <summary>
        /// Gets or sets the absolute path and fileName for this dataset. This is null if the DataSet has no file.
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// Gets or sets the relative path to the underlying file. This is only meant to be used for serialization.
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Gets a value indicating whether the dispose method has been called on this dataset.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets or sets a string name identifying this dataset.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the progress handler to use for internal actions taken by this dataset.
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This closes the data set. Many times this will simply do nothing, but
        /// in some cases this may close an open connection to a data source.
        /// </summary>
        void Close();

        #endregion
    }
}
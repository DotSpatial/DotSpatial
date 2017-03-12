// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

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
        /// Gets or sets the extent for the dataset.  Usages to Envelope were replaced
        /// as they required an explicit using to DotSpatial.Topology which is not
        /// as intuitive.  Extent.ToEnvelope() and new Extent(myEnvelope) convert them.
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
        /// True if the dispose method has been called on this dataset.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets or sets a string name identifying this dataset
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the progress handler to use for internal actions taken by this dataset.
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This closes the data set.  Many times this will simply do nothing, but
        /// in some cases this may close an open connection to a data source.
        /// </summary>
        void Close();

        #endregion
    }
}
// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
        /// <summary>
        /// Gets or sets the extent for the dataset.  Usages to Envelope were replaced
        /// as they required an explicit using to DotSpatial.Topology which is not
        /// as intuitive.  Extent.ToEnvelope() and new Extent(myEnvelope) convert them.
        /// </summary>
        Extent Extent { get; set; }

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

        /// <summary>
        /// Gets or sets the space time support for this dataset.
        /// </summary>
        SpaceTimeSupport SpaceTimeSupport { get; set; }

        /// <summary>
        /// Gets or sets a string that describes as clearly as possible what type of
        /// elements are contained in this dataset.
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// This closes the data set.  Many times this will simply do nothing, but
        /// in some cases this may close an open connection to a data source.
        /// </summary>
        void Close();
    }
}
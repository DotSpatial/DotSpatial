// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    /// <typeparam name="T">Type of the comparer.</typeparam>
    public class Compare<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Compare{T}"/> class.
        /// </summary>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
        public Compare(IComparer<T> inComparer)
        {
            Comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the System.Collections.Generic.IComparer&lt;T&gt; being referenced by this event
        /// </summary>
        public IComparer<T> Comparer { get; protected set; }

        #endregion
    }
}
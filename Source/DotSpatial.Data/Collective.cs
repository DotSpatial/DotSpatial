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
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class Collective<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Collective{T}"/> class.
        /// </summary>
        /// <param name="inCollection">The IEnumerable&lt;T&gt; specified during the event"/></param>
        public Collective(IEnumerable<T> inCollection)
        {
            Collection = inCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list item being referenced by this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        #endregion
    }
}
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

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    /// <typeparam name="T">Type of the comparison.</typeparam>
    public class ComparisonArgs<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonArgs{T}"/> class.
        /// </summary>
        /// <param name="inComparison">The System.Comparison&lt;T&gt; being used by this action </param>
        public ComparisonArgs(Comparison<T> inComparison)
        {
            Comparison = inComparison;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets System.Comparison being referenced by this event.
        /// </summary>
        public Comparison<T> Comparison { get; set; }

        #endregion
    }
}
﻿// -----------------------------------------------------------------------
using System.ComponentModel.Composition;
using DotSpatial.Data;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Used to display status information.
    /// </summary>
    [InheritedExport]
    public interface IStatusControl : IProgressHandler
    {
        #region Methods

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel">The panel.</param>
        void Add(StatusPanel panel);

        /// <summary>
        /// Removes the specified panel.
        /// </summary>
        /// <param name="panel">The panel.</param>
        void Remove(StatusPanel panel);

        #endregion
    }
}
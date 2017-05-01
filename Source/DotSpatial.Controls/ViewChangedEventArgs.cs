// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Initial Developer of this Original Code is  Peter Hammond/Jia Liang Liu
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                        |   Date             |         Comments
//-----------------------------|--------------------|-----------------------------------------------
// Peter Hammond/Jia Liang Liu |  02/20/2010        |  view change event argument
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Arguments of the ViewChangedEvent.
    /// </summary>
    public class ViewChangedEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldView">The view that existed before the change.</param>
        /// <param name="newView">The view that exists after the change.</param>
        public ViewChangedEventArgs(Rectangle oldView, Rectangle newView)
        {
            OldView = oldView;
            NewView = newView;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the view after the change.
        /// </summary>
        public Rectangle NewView { get; }

        /// <summary>
        /// Gets the view before the change.
        /// </summary>
        public Rectangle OldView { get; }

        #endregion
    }
}
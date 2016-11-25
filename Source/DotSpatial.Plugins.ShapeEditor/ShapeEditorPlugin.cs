// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 6:44:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Controls;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// Initializes a new instance of the ShapeEditorPlugin class.
    /// </summary>
    public class ShapeEditorPlugin : Extension
    {
        private ButtonHandler _myHandler;

        /// <inheritdoc/>
        public override void Activate()
        {
            _myHandler = new ButtonHandler(this.App) { Map = App.Map };
            base.Activate();
        }

        /// <inheritdoc/>
        public override void Deactivate()
        {
            if (App.HeaderControl != null) { App.HeaderControl.RemoveAll(); }

            if (_myHandler != null) { _myHandler.Dispose(); }
            base.Deactivate();
        }
    }
}
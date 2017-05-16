// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Controls;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// Initializes a new instance of the ShapeEditorPlugin class.
    /// </summary>
    public class ShapeEditorPlugin : Extension
    {
        #region Fields

        private ButtonHandler _myHandler;

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override void Activate()
        {
            _myHandler = new ButtonHandler(App)
                         {
                             Map = App.Map
                         };
            base.Activate();
        }

        /// <inheritdoc/>
        public override void Deactivate()
        {
            App.HeaderControl?.RemoveAll();
            _myHandler?.Dispose();
            base.Deactivate();
        }

        #endregion
    }
}
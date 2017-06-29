// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/5/2008 2:12:37 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Common implementation of IMapFunction interface.
    /// </summary>
    public class MapFunction : IMapFunction
    {
        #region Events

        /// <summary>
        /// Occurs when the function is activated
        /// </summary>
        public event EventHandler FunctionActivated;

        /// <summary>
        /// Occurs when the function is deactivated.
        /// </summary>
        public event EventHandler FunctionDeactivated;

        /// <summary>
        /// Occurs during a mouse down event
        /// </summary>
        public event EventHandler<GeoMouseArgs> MouseDown;

        /// <summary>
        /// Occurs during a mouse move event
        /// </summary>
        public event EventHandler<GeoMouseArgs> MouseMove;

        /// <summary>
        /// Occurs during a mouse up event
        /// </summary>
        public event EventHandler<GeoMouseArgs> MouseUp;

        /// <summary>
        /// Occurs during a mousewheel event
        /// </summary>
        public event EventHandler<GeoMouseArgs> MouseWheel;

        /// <summary>
        /// Occurs during a double click event
        /// </summary>
        public event EventHandler<GeoMouseArgs> MouseDoubleClick;

        /// <summary>
        /// Occurs during a key up event
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp;

        #endregion

        #region Private Variables

        private bool _enabled;
        private IMap _map;
        private string _name;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the map function.
        /// </summary>
        public MapFunction()
        {
            // By default, the flags are set to deactivate when anything comes on other than an "Always On" function.
            YieldStyle = YieldStyles.Keyboard | YieldStyles.LeftButton | YieldStyles.RightButton | YieldStyles.Scroll;
        }

        /// <summary>
        /// Combines the constructor with an automatic call to the init method.  If you use this constructor
        /// overload, then it is not necessary to also call the init method.  The init method is supported
        /// because constructors cannot be specified through an interface.
        /// </summary>
        /// <param name="inMap">Any valid IMap interface</param>
        public MapFunction(IMap inMap)
        {
            _map = inMap;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs this tool to
        /// </summary>
        /// <param name="e"></param>
        public void DoKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        /// <summary>
        /// Forces this tool to execute whatever behavior should occur during a double click even on the panel
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseDoubleClick(GeoMouseArgs e)
        {
            OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseDown event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseDown(GeoMouseArgs e)
        {
            OnMouseDown(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseMove event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseMove(GeoMouseArgs e)
        {
            OnMouseMove(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseUp event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseUp(GeoMouseArgs e)
        {
            OnMouseUp(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseWheel event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseWheel(GeoMouseArgs e)
        {
            OnMouseWheel(e);
        }

        /// <inheritdoc />
        public void DoKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        /// <summary>
        /// This is the method that is called by the drawPanel.  The graphics coordinates are
        /// in pixels relative to the image being edited.
        /// </summary>
        public void Draw(MapDrawArgs args)
        {
            //if (OnBeforeDrawing(args) == true) return; // handled
            OnDraw(args);
        }

        /// <summary>
        /// Organizes the map that this tool will work with.
        /// </summary>
        /// <param name="inMap"></param>
        public void Init(IMap inMap)
        {
        }

        /// <summary>
        /// Cancels whatever drawing was being done by the tool and resets the cursor to an arrow.
        /// </summary>
        public virtual void Cancel()
        {
        }

        /// <summary>
        /// Gets an available name given the base name.
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        public string GetAvailableName(string baseName)
        {
            string newName = baseName;
            int i = 1;
            if (_map != null)
            {
                if (_map.MapFunctions != null)
                {
                    string name = newName;
                    bool found = _map.MapFunctions.Any(function => function.Name == name);
                    while (found)
                    {
                        newName = baseName + i;
                        i++;
                        string name1 = newName;
                        found = _map.MapFunctions.Any(function => function.Name == name1);
                    }
                }
            }
            return newName;
        }

        /// <summary>
        /// This allows sub-classes to customize the drawing that occurs.  All drawing is done
        /// in the image coordinate space, where 0, 0 is the upper left corner of the image.
        /// </summary>
        /// <param name="e">A PaintEventArgs where the graphics object is already in image coordinates</param>
        protected virtual void OnDraw(MapDrawArgs e)
        {
        }

        /// <summary>
        /// Allows for inheriting tools to control KeyUp.
        /// </summary>
        /// <param name="e">A KeyEventArgs parameter</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            var h = KeyUp;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseDoubleClick.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseDoubleClick(GeoMouseArgs e)
        {
            var h = MouseDoubleClick;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseDown.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseDown(GeoMouseArgs e)
        {
            var h = MouseDown;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseMove.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseMove(GeoMouseArgs e)
        {
            var h = MouseMove;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseUp.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseUp(GeoMouseArgs e)
        {
            var h = MouseUp;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseWheel.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseWheel(GeoMouseArgs e)
        {
            var h = MouseWheel;
            if (h != null) h(this, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Describes a button image
        /// </summary>
        [Category("Appearance"), Description("This controls is the image that will be used for buttons that activate this tool.")]
        public Image ButtonImage { get; set; }

        /// <summary>
        /// This controls the cursor that this tool uses, unless the action has been cancelled by attempting
        /// to use the tool outside the bounds of the image.
        /// </summary>
        [Category("Appearance"), Description(
            "This controls the cursor that this tool uses, unless the action has been cancelled by " +
            "attempting to use the tool outside the bounds of the image."
            )]
        public Bitmap CursorBitmap { get; set; }

        /// <summary>
        /// Gets or sets a boolean that is true if this tool should be handed drawing instructions
        /// from the screen.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            protected set // Externally you can only Activate or Deactivate, not directly enable/disable
            { _enabled = value; }
        }

        /// <summary>
        /// Gets or sets the basic map that this tool interacts with.  This can alternately be set using
        /// the Init method.
        /// </summary>
        public IMap Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// Gets or sets the name that attempts to identify this plugin uniquely.  If the
        /// name is already in the tools list, this will modify the name stored here.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = GetAvailableName(value); }
        }

        /// <summary>
        /// If this is false, then the typical contents from the map's back buffer are drawn first,
        /// followed by the contents of this tool.
        /// </summary>
        public virtual bool PreventBackBuffer { get; protected set; }

        /// <inheritdoc/>
        public YieldStyles YieldStyle { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Forces activation
        /// </summary>
        public void Activate()
        {
            OnActivate();
        }

        /// <summary>
        /// Deactivate is like when someone clicks on a different button.  It may not
        /// involve the whole plugin being unloaded.
        /// </summary>
        public void Deactivate()
        {
            OnDeactivate();
        }

        /// <summary>
        /// Here, the entire plugin is unloading, so if there are any residual states
        /// that are not taken care of, this should remove them.
        /// </summary>
        public void Unload()
        {
            OnUnload();
        }

        /// <summary>
        /// This is fired when enabled is set to true, and firing this will set enabled to true
        /// </summary>
        protected virtual void OnActivate()
        {
            _enabled = true;

            var h = FunctionActivated;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// this is fired when enabled is set to false, and firing this will set enabled to false.
        /// </summary>
        protected virtual void OnDeactivate()
        {
            _enabled = false;

            var h = FunctionDeactivated;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when a key is pressed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        /// <summary>
        /// This occurs when the entire plugin is being unloaded.
        /// </summary>
        protected virtual void OnUnload()
        {
        }

        #endregion
    }
}
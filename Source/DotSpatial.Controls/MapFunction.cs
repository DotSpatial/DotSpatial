// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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
        #region Fields

        private string _name;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunction"/> class.
        /// </summary>
        public MapFunction()
        {
            // By default, the flags are set to deactivate when anything comes on other than an "Always On" function.
            YieldStyle = YieldStyles.Keyboard | YieldStyles.LeftButton | YieldStyles.RightButton | YieldStyles.Scroll;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunction"/> class.
        /// </summary>
        /// <param name="inMap">The map the map function should work on.</param>
        public MapFunction(IMap inMap)
        {
            Map = inMap;
        }

        #endregion

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
        /// Occurs during a key up event
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp;

        /// <summary>
        /// Occurs during a double click event
        /// </summary>
        public event EventHandler<GeoMouseArgs> MouseDoubleClick;

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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the button image.
        /// </summary>
        [Category("Appearance")]
        [Description("This controls is the image that will be used for buttons that activate this tool.")]
        public Image ButtonImage { get; set; }

        /// <summary>
        /// Gets or sets the cursor bitmap. This controls the cursor that this tool uses, unless the action has been cancelled by attempting
        /// to use the tool outside the bounds of the image.
        /// </summary>
        [Category("Appearance")]
        [Description("This controls the cursor that this tool uses, unless the action has been cancelled by attempting to use the tool outside the bounds of the image.")]
        public Bitmap CursorBitmap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this tool should be handed drawing instructions from the screen.
        /// </summary>
        public bool Enabled { get; protected set; }

        /// <summary>
        /// Gets or sets the basic map that this tool interacts with. This can alternately be set using
        /// the Init method.
        /// </summary>
        public IMap Map { get; set; }

        /// <summary>
        /// Gets or sets the name that attempts to identify this plugin uniquely. If the
        /// name is already in the tools list, this will modify the name stored here.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = GetAvailableName(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the typical contents from the map's back buffer should be drawn first,
        /// followed by the contents of this tool.
        /// </summary>
        public virtual bool PreventBackBuffer { get; protected set; }

        /// <inheritdoc/>
        public YieldStyles YieldStyle { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Forces activation
        /// </summary>
        public void Activate()
        {
            OnActivate();
        }

        /// <summary>
        /// Cancels whatever drawing was being done by the tool and resets the cursor to an arrow.
        /// </summary>
        public virtual void Cancel()
        {
        }

        /// <summary>
        /// Deactivate is like when someone clicks on a different button. It may not
        /// involve the whole plugin being unloaded.
        /// </summary>
        public void Deactivate()
        {
            OnDeactivate();
        }

        /// <inheritdoc />
        public void DoKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the KeyUp event.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void DoKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        /// <summary>
        /// Forces this tool to execute whatever behavior should occur during a double click even on the panel.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void DoMouseDoubleClick(GeoMouseArgs e)
        {
            OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseDown event.
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseDown(GeoMouseArgs e)
        {
            OnMouseDown(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseMove event.
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseMove(GeoMouseArgs e)
        {
            OnMouseMove(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseUp event.
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseUp(GeoMouseArgs e)
        {
            OnMouseUp(e);
        }

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseWheel event.
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        public void DoMouseWheel(GeoMouseArgs e)
        {
            OnMouseWheel(e);
        }

        /// <summary>
        /// This is the method that is called by the drawPanel. The graphics coordinates are
        /// in pixels relative to the image being edited.
        /// </summary>
        /// <param name="args">The map draw args.</param>
        public void Draw(MapDrawArgs args)
        {
            OnDraw(args);
        }

        /// <summary>
        /// Gets an available name given the base name.
        /// </summary>
        /// <param name="baseName">The name that should be used for the function.</param>
        /// <returns>The first available name that is based on the given base name.</returns>
        public string GetAvailableName(string baseName)
        {
            string newName = baseName;
            int i = 1;
            if (Map?.MapFunctions != null)
            {
                string name = newName;
                bool found = Map.MapFunctions.Any(function => function.Name == name);
                while (found)
                {
                    newName = baseName + i;
                    i++;
                    string name1 = newName;
                    found = Map.MapFunctions.Any(function => function.Name == name1);
                }
            }

            return newName;
        }

        /// <summary>
        /// Organizes the map that this tool will work with.
        /// </summary>
        /// <param name="inMap">The map the tool will work with.</param>
        public void Init(IMap inMap)
        {
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
            Enabled = true;
            FunctionActivated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// this is fired when enabled is set to false, and firing this will set enabled to false.
        /// </summary>
        protected virtual void OnDeactivate()
        {
            Enabled = false;
            FunctionDeactivated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This allows sub-classes to customize the drawing that occurs. All drawing is done
        /// in the image coordinate space, where 0, 0 is the upper left corner of the image.
        /// </summary>
        /// <param name="e">A PaintEventArgs where the graphics object is already in image coordinates</param>
        protected virtual void OnDraw(MapDrawArgs e)
        {
        }

        /// <summary>
        /// Occurs when a key is pressed
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        /// <summary>
        /// Allows for inheriting tools to control KeyUp.
        /// </summary>
        /// <param name="e">A KeyEventArgs parameter</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseDoubleClick.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseDoubleClick(GeoMouseArgs e)
        {
            MouseDoubleClick?.Invoke(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseDown.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseDown(GeoMouseArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseMove.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseMove(GeoMouseArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseUp.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseUp(GeoMouseArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        /// <summary>
        /// Allows for inheriting tools to control OnMouseWheel.
        /// </summary>
        /// <param name="e">A GeoMouseArgs parameter</param>
        protected virtual void OnMouseWheel(GeoMouseArgs e)
        {
            MouseWheel?.Invoke(this, e);
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
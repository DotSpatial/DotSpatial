// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  An open source drawing pad that is super simple, but extendable
// ********************************************************************************************************
//
// The Original Code is from SketchPad.exe
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/8/2008 1:01:26 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This parses the input values and changes the background color to salmon
    /// if the value won't work as a degree.
    /// </summary>
    internal class AngleTextBox : ValidTextBox
    {
        #region Fields

        private int _angle;
        private int _maxAngle;
        private int _minAngle;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleTextBox"/> class.
        /// </summary>
        public AngleTextBox()
        {
            _minAngle = -360;
            _maxAngle = 360;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs continuously as someone drags the angle control around
        /// </summary>
        public event EventHandler AngleChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer angle for this textbox.
        /// </summary>
        public int Angle
        {
            get
            {
                return _angle;
            }

            set
            {
                _angle = value;
                Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the maximum valid angle. An angle above the maximum will
        /// register an error and show the error color.
        /// </summary>
        public int MaxAngle
        {
            get
            {
                return _maxAngle;
            }

            set
            {
                _maxAngle = value;
                NormalToolTipText = "Enter an integer degree between " + _minAngle + " and " + _maxAngle + ".";
            }
        }

        /// <summary>
        /// Gets or sets the minimum valid angle. An angle below the minimum will
        /// register an error and show the error color.
        /// </summary>
        public int MinAngle
        {
            get
            {
                return _minAngle;
            }

            set
            {
                _minAngle = value;
                NormalToolTipText = "Enter an integer degree between " + _minAngle + " and " + _maxAngle + ".";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the AngleChanged event
        /// </summary>
        /// <param name="e">The event args.</param>
        protected void OnAngleChanged(EventArgs e)
        {
            AngleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Fires the TextChanged method and also determines whether or not the text is a valid integer.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (int.TryParse(Text, out _angle) == false)
            {
                ErrorMessage = "The text entered into the " + MessageName + " could not be parsed into an integer.";
            }
            else
            {
                if (_angle > 360 || _angle < -360) _angle = _angle % 360;
                if (_angle < _minAngle || _angle > _maxAngle)
                {
                    ErrorMessage = "The value entered into the " + MessageName + " was outside the valid range from " + _minAngle + " to " + _maxAngle;
                }
                else
                {
                    ClearError();
                }

                OnAngleChanged(EventArgs.Empty);
            }

            base.OnTextChanged(e);
        }

        #endregion
    }
}
// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// This handles the methodology of progress messaging in one place to make it easier to update.
    /// </summary>
    public class TimedProgressMeter
    {
        #region Fields
        private readonly Timer _timer;
        private double _endValue;
        private int _oldProg; // the previous progress level
        private int _prog; // the current progress level
        private double _startValue;
        private int _stepPercent = 1;
        private double _value;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedProgressMeter"/> class that shows progress against an expected time.
        /// </summary>
        /// <param name="progressHandler">An IProgressHandler that will actually handle the status messages sent by this meter.</param>
        /// <param name="baseMessage">A base message to use as the basic status for this progress handler.</param>
        /// <param name="estimatedTime">The time the process might take.</param>
        public TimedProgressMeter(IProgressHandler progressHandler, string baseMessage, TimeSpan estimatedTime)
        {
            ProgressHandler = progressHandler;
            Key = baseMessage;
            _endValue = estimatedTime.TotalSeconds * 1000;

            _timer = new Timer
            {
                Interval = Convert.ToInt32(estimatedTime.TotalSeconds * 10) // Attempt to have a tick once during each estimated percentile
            };

            _timer.Tick += TimerTick;
            _timer.Start(); // Timers should be on another thread...
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedProgressMeter"/> class, but doesn't support the IProgressHandler unless one is specified.
        /// </summary>
        public TimedProgressMeter()
            : this(null, "Calculating values.", 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedProgressMeter"/> class.
        /// A progress meter can't actually do anything without a progressHandler, which actually displays the status.
        /// </summary>
        /// <param name="progressHandler">An IProgressHandler that will actually handle the status messages sent by this meter.</param>
        public TimedProgressMeter(IProgressHandler progressHandler)
            : this(progressHandler, "Calculating values.", 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedProgressMeter"/> class that simply keeps track of progress and is capable of sending progress messages.
        /// This assumes a MaxValue of 100 unless it is changed later.
        /// </summary>
        /// <param name="progressHandler">Any valid IProgressHandler that will display progress messages.</param>
        /// <param name="baseMessage">A base message to use as the basic status for this progress handler.</param>
        public TimedProgressMeter(IProgressHandler progressHandler, string baseMessage)
            : this(progressHandler, baseMessage, 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedProgressMeter"/> class that simply keeps track of progress and is capable of sending progress messages.
        /// </summary>
        /// <param name="progressHandler">Any valid implementation if IProgressHandler that will handle the progress function.</param>
        /// <param name="baseMessage">The message without any progress information.</param>
        /// <param name="endValue">Percent shoudl show a range between the MinValue and MaxValue. MinValue is assumed to be 0.</param>
        public TimedProgressMeter(IProgressHandler progressHandler, string baseMessage, object endValue)
        {
            _endValue = Convert.ToDouble(endValue);
            ProgressHandler = progressHandler;
            Reset();
            Key = baseMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current integer progress level from 0 to 100. If a new update is less than or equal to the previous
        /// value, then no progress will be displayed by the ProgressMeter. Values less than 0 are set to 0. Values greater than
        /// 100 are set to 100.
        /// </summary>
        public int CurrentPercent
        {
            get
            {
                return _prog;
            }

            set
            {
                int val = value;
                if (val < 0) val = 0;
                if (val > 100)
                {
                    _timer?.Stop();
                    val = 100;
                }

                _prog = val;
                if (_prog >= _oldProg + _stepPercent)
                {
                    SendProgress();
                    _oldProg = _prog;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current value relative to the specified MaxValue in order to update the progress.
        /// Setting this will also update OldProgress if there is an integer change in the percentage, and send
        /// a progress message to the IProgressHandler interface.
        /// </summary>
        public object CurrentValue
        {
            get
            {
                return _value;
            }

            set
            {
                _value = Convert.ToDouble(value);
                if (_startValue < _endValue)
                {
                    CurrentPercent = Convert.ToInt32(Math.Round(100 * (_value - _startValue) / (_endValue - _startValue)));
                }
                else
                {
                    CurrentPercent = Convert.ToInt32(Math.Round(100 * (_startValue - _value) / (_startValue - _endValue)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that defines when the meter should show as 100% complete.
        /// EndValue can be less than StartValue, but values closer to EndValue
        /// will show as being closer to 100%.
        /// </summary>
        public object EndValue
        {
            get
            {
                return _endValue;
            }

            set
            {
                _endValue = Convert.ToDouble(value);
            }
        }

        /// <summary>
        /// Gets or sets the string that does not include any mention of progress percentage, but specifies what is occuring.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the previous integer progress level from 0 to 100. If a new update is less than or equal to the previous
        /// value, then no progress will be displayed by the ProgressMeter. Values less than 0 are set to 0. Values greater than
        /// 100 are set to 100.
        /// </summary>
        public int PreviousPercent
        {
            get
            {
                return _oldProg;
            }

            set
            {
                int val = value;
                if (val < 0) val = 0;
                if (val > 100) val = 100;
                _oldProg = val;
            }
        }

        /// <summary>
        /// Gets or sets the progress handler for this meter.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the progress meter should send messages to the IProgressHandler.
        /// By default Silent is false, but setting this to true will disable the messaging portion.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Gets or sets the minimum value defines when the meter should show as 0% complete.
        /// </summary>
        public object StartValue
        {
            get
            {
                return _startValue;
            }

            set
            {
                _startValue = Convert.ToDouble(value);
            }
        }

        /// <summary>
        /// Gets or sets an integer value that is 1 by default. Ordinarilly this will send a progress message only when the integer progress
        /// has changed by 1 percentage point. For example, if StepPercent were set to 5, then a progress update would only
        /// be sent out at 5%, 10% and so on. This helps reduce overhead in cases where showing status messages is actually
        /// the majority of the processing time for the function.
        /// </summary>
        public int StepPercent
        {
            get
            {
                return _stepPercent;
            }

            set
            {
                _stepPercent = value;
                if (_stepPercent < 1) _stepPercent = 1;
                if (_stepPercent > 100) _stepPercent = 100;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the progress meter to the 0 value. This sets the status message to "Ready.".
        /// </summary>
        public void Reset()
        {
            _prog = 0;
            _oldProg = 0;
            Key = "Ready.";
            _timer?.Stop();
            if (Silent) return;
            if (ProgressHandler == null) return;
            ProgressHandler.Reset();
            Application.DoEvents(); // Allow the form to update a status bar if necessary.
        }

        /// <summary>
        /// Sends a progress message to the IProgressHandler interface with the current message and progres.
        /// </summary>
        public void SendProgress()
        {
            if (Silent) return;
            if (ProgressHandler == null) return;
            ProgressHandler.Progress(_prog, Key + ", " + _prog + "% Complete.");
            Application.DoEvents(); // Allow the form to update a status bar if necessary.
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CurrentPercent++;
        }

        #endregion
    }
}
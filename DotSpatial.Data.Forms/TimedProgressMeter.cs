// ********************************************************************************************************
// Product Name: DotSpatial.Data.Forms.dll
// Description:  A dynamically loaded extension with a toolkit.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during refactoring 2010.
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// This handles the methodology of progress messaging in one place to make it easier to update.
    /// </summary>
    public class TimedProgressMeter
    {
        private readonly TimeSpan _timeSpan;
        private readonly Timer _timer;
        private string _baseMessage;
        private double _endValue = 100;
        private int _oldProg; // the previous progress level
        private int _prog; // the current progress level
        private IProgressHandler _progressHandler;
        private bool _silent;
        private double _startValue;
        private int _stepPercent = 1;
        private double _value;

        #region Constructors

        /// <summary>
        /// Constructs a Time-Based progress meter that shows progress against an expected time.
        /// </summary>
        /// <param name="progressHandler"></param>
        /// <param name="baseMessage"></param>
        /// <param name="estimatedTime"></param>
        public TimedProgressMeter(IProgressHandler progressHandler, string baseMessage, TimeSpan estimatedTime)
        {
            _progressHandler = progressHandler;
            _baseMessage = baseMessage;
            _timeSpan = estimatedTime;
            _endValue = estimatedTime.TotalSeconds * 1000;

            _timer = new Timer();
            _timer.Interval = Convert.ToInt32(_timeSpan.TotalSeconds * 10); // Attempt to have a tick once during each estimated percentile
            //_timer.Interval = 100;
            _timer.Tick += TimerTick;
            _timer.Start(); // Timers should be on another thread...
        }

        /// <summary>
        /// Intializes a new progress meter, but doesn't support the IProgressHandler unless one is specified.
        /// </summary>
        public TimedProgressMeter()
            : this(null, "Calculating values.", 100)
        {
        }

        /// <summary>
        /// A progress meter can't actually do anything without a progressHandler, which actually displays the status.
        /// </summary>
        /// <param name="progressHandler">An IProgressHandler that will actually handle the status messages sent by this meter.</param>
        public TimedProgressMeter(IProgressHandler progressHandler)
            : this(progressHandler, "Calculating values.", 100)
        {
        }

        /// <summary>
        /// A progress meter that simply keeps track of progress and is capable of sending progress messages.
        /// This assumes a MaxValue of 100 unless it is changed later.
        /// </summary>
        /// <param name="progressHandler">Any valid IProgressHandler that will display progress messages</param>
        /// <param name="baseMessage">A base message to use as the basic status for this progress handler.</param>
        public TimedProgressMeter(IProgressHandler progressHandler, string baseMessage)
            : this(progressHandler, baseMessage, 100)
        {
        }

        /// <summary>
        /// A progress meter that simply keeps track of progress and is capable of sending progress messages.
        /// </summary>
        /// <param name="progressHandler">Any valid implementation if IProgressHandler that will handle the progress function</param>
        /// <param name="baseMessage">The message without any progress information.</param>
        /// <param name="endValue">Percent shoudl show a range between the MinValue and MaxValue.  MinValue is assumed to be 0.</param>
        public TimedProgressMeter(IProgressHandler progressHandler, string baseMessage, object endValue)
        {
            _endValue = Convert.ToDouble(endValue);
            _progressHandler = progressHandler;
            Reset();
            _baseMessage = baseMessage;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CurrentPercent++;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the progress meter to the 0 value.  This sets the status message to "Ready.".
        /// </summary>
        public void Reset()
        {
            _prog = 0;
            _oldProg = 0;
            _baseMessage = "Ready.";
            if (_timer != null) _timer.Stop();
            if (_silent) return;
            if (_progressHandler == null) return;
            _progressHandler.Progress(_baseMessage, _prog, _baseMessage);
            Application.DoEvents(); // Allow the form to update a status bar if necessary.
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string that does not include any mention of progress percentage, but specifies what is occuring.
        /// </summary>
        public string Key
        {
            get { return _baseMessage; }
            set { _baseMessage = value; }
        }

        /// <summary>
        /// Gets or sets the current integer progress level from 0 to 100.  If a new update is less than or equal to the previous
        /// value, then no progress will be displayed by the ProgressMeter.  Values less than 0 are set to 0.  Values greater than
        /// 100 are set to 100.
        /// </summary>
        public int CurrentPercent
        {
            get { return _prog; }
            set
            {
                int val = value;
                if (val < 0) val = 0;
                if (val > 100)
                {
                    if (_timer != null) _timer.Stop();
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
        /// The value that defines when the meter should show as 100% complete.
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
        /// Gets or sets whether the progress meter should send messages to the IProgressHandler.
        /// By default Silent is false, but setting this to true will disable the messaging portion.
        /// </summary>
        public bool Silent
        {
            get { return _silent; }
            set { _silent = value; }
        }

        /// <summary>
        /// The minimum value defines when the meter should show as 0% complete.
        /// </summary>
        public object StartValue
        {
            get { return _startValue; }
            set
            {
                _startValue = Convert.ToDouble(value);
            }
        }

        /// <summary>
        /// An integer value that is 1 by default.  Ordinarilly this will send a progress message only when the integer progress
        /// has changed by 1 percentage point.  For example, if StepPercent were set to 5, then a progress update would only
        /// be sent out at 5%, 10% and so on.  This helps reduce overhead in cases where showing status messages is actually
        /// the majority of the processing time for the function.
        /// </summary>
        public int StepPercent
        {
            get { return _stepPercent; }
            set
            {
                _stepPercent = value;
                if (_stepPercent < 1) _stepPercent = 1;
                if (_stepPercent > 100) _stepPercent = 100;
            }
        }

        /// <summary>
        /// Gets or sets the previous integer progress level from 0 to 100.  If a new update is less than or equal to the previous
        /// value, then no progress will be displayed by the ProgressMeter.  Values less than 0 are set to 0.  Values greater than
        /// 100 are set to 100.
        /// </summary>
        public int PreviousPercent
        {
            get { return _oldProg; }
            set
            {
                int val = value;
                if (val < 0) val = 0;
                if (val > 100) val = 100;
                _oldProg = val;
            }
        }

        /// <summary>
        /// Gets or sets the progress handler for this meter
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        /// <summary>
        /// Sends a progress message to the IProgressHandler interface with the current message and progres
        /// </summary>
        public void SendProgress()
        {
            if (_silent) return;
            if (_progressHandler == null) return;
            _progressHandler.Progress(_baseMessage, _prog, _baseMessage + ", " + _prog + "% Complete.");
            Application.DoEvents(); // Allow the form to update a status bar if necessary.
        }

        #endregion
    }
}
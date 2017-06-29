// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
/* Source...
 *
% KALMANF - updates a system state vector estimate based upon an
%           observation, using a discrete Kalman filter.
%
% Version 1.0, June 30, 2004
%
% This tutorial function was written by Michael C. Kleder
%
% INTRODUCTION
%
% Many people have heard of Kalman filtering, but regard the topic
% as mysterious. While it's true that deriving the Kalman filter and
% proving mathematically that it is "optimal" under a variety of
% circumstances can be rather intense, applying the filter to
% a basic linear system is actually very easy. This Matlab file is
% intended to demonstrate that.
%
% An excellent paper on Kalman filtering at the introductory level,
% without detailing the mathematical underpinnings, is:
% "An Introduction to the Kalman Filter"
% Greg Welch and Gary Bishop, University of North Carolina
% http://www.cs.unc.edu/~welch/kalman/kalmanIntro.html
%
% PURPOSE:
%
% The purpose of each iteration of a Kalman filter is to update
% the estimate of the state vector of a system (and the covariance
% of that vector) based upon the information in a new observation.
% The version of the Kalman filter in this function assumes that
% observations occur at fixed discrete time intervals. Also, this
% function assumes a linear system, meaning that the time evolution
% of the state vector can be calculated by means of a state transition
% matrix.
%
% USAGE:
%
% s = kalmanf(s)
%
% "s" is a "system" struct containing various fields used as input
% and output. The state estimate "x" and its covariance "P" are
% updated by the function. The other fields describe the mechanics
% of the system and are left unchanged. A calling routine may change
% these other fields as needed if state dynamics are time-dependent;
% otherwise, they should be left alone after initial values are set.
% The exceptions are the observation vectro "z" and the input control
% (or forcing function) "u." If there is an input function, then
% "u" should be set to some nonzero value by the calling routine.
%
% SYSTEM DYNAMICS:
%
% The system evolves according to the following difference equations,
% where quantities are further defined below:
%
% x = Ax + Bu + w  meaning the state vector x evolves during one time
%                  step by premultiplying by the "state transition
%                  matrix" A. There is optionally (if nonzero) an input
%                  vector u which affects the state linearly, and this
%                  linear effect on the state is represented by
%                  premultiplying by the "input matrix" B. There is also
%                  gaussian process noise w.
% z = Hx + v       meaning the observation vector z is a linear function
%                  of the state vector, and this linear relationship is
%                  represented by premultiplication by "observation
%                  matrix" H. There is also gaussian measurement
%                  noise v.
% where w ~ N(0, Q) meaning w is gaussian noise with covariance Q
%       v ~ N(0, R) meaning v is gaussian noise with covariance R
%
% VECTOR VARIABLES:
%
% s.x = state vector estimate. In the input struct, this is the
%       "a priori" state estimate (prior to the addition of the
%       information from the new observation). In the output struct,
%       this is the "a posteriori" state estimate (after the new
%       measurement information is included).
% s.z = observation vector
% s.u = input control vector, optional (defaults to zero).
%
% MATRIX VARIABLES:
%
% s.A = state transition matrix (defaults to identity).
% s.P = covariance of the state vector estimate. In the input struct,
%       this is "a priori," and in the output it is "a posteriori."
%       (required unless autoinitializing as described below).
% s.B = input matrix, optional (defaults to zero).
% s.Q = process noise covariance (defaults to zero).
% s.R = measurement noise covariance (required).
% s.H = observation matrix (defaults to identity).
%
% NORMAL OPERATION:
%
% (1) define all state definition fields: A, B, H, Q, R
% (2) define intial state estimate: x, P
% (3) obtain observation and control vectors: z, u
% (4) call the filter to obtain updated state estimate: x, P
% (5) return to step (3) and repeat
%
% INITIALIZATION:
%
% If an initial state estimate is unavailable, it can be obtained
% from the first observation as follows, provided that there are the
% same number of observable variables as state variables. This "auto-
% intitialization" is done automatically if s.x is absent or NaN.
%
% x = inv(H)*z
% P = inv(H)*R*inv(H')
%
% This is mathematically equivalent to setting the initial state estimate
% covariance to infinity.
%
% SCALAR EXAMPLE (Automobile Voltimeter):
%
% % Define the system as a constant of 12 volts:
% clear s
% s.x = 12;
% s.A = 1;
% % Define a process noise (stdev) of 2 volts as the car operates:
% s.Q = 2^2; % variance, hence stdev^2
% % Define the voltimeter to measure the voltage itself:
% s.H = 1;
% % Define a measurement error (stdev) of 2 volts:
% s.R = 2^2; % variance, hence stdev^2
% % Do not define any system input (control) functions:
% s.B = 0;
% s.u = 0;
% % Do not specify an initial state:
% s.x = nan;
% s.P = nan;
% % Generate random voltages and watch the filter operate.
% tru=[]; % truth voltage
% for t=1:20
%    tru(end+1) = randn*2+12;
%    s(end).z = tru(end) + randn*2; % create a measurement
%    s(end+1)=kalmanf(s(end)); % perform a Kalman filter iteration
% end
% figure
% hold on
% grid on
% % plot measurement data:
% hz=plot([s(1:end-1).z],'r.');
% % plot a-posteriori state estimates:
% hk=plot([s(2:end).x],'b-');
% ht=plot(tru,'g-');
% legend([hz hk ht],'observations','Kalman output','true voltage', 0)
% title('Automobile Voltimeter Example')
% hold off

function s = kalmanf(s)

% set defaults for absent fields:
if ~isfield(s,'x'); s.x=nan*z; end
if ~isfield(s,'P'); s.P=nan; end
if ~isfield(s,'z'); error('Observation vector missing'); end
if ~isfield(s,'u'); s.u=0; end
if ~isfield(s,'A'); s.A=eye(length(x)); end
if ~isfield(s,'B'); s.B=0; end
if ~isfield(s,'Q'); s.Q=zeros(length(x)); end
if ~isfield(s,'R'); error('Observation covariance missing'); end
if ~isfield(s,'H'); s.H=eye(length(x)); end

if isnan(s.x)
   % initialize state estimate from first observation
   if diff(size(s.H))
      error('Observation matrix must be square and invertible for state autointialization.');
   end
   s.x = inv(s.H)*s.z;
   s.P = inv(s.H)*s.R*inv(s.H');
else

   % This is the code which implements the discrete Kalman filter:

   % Prediction for state vector and covariance:
   s.x = s.A*s.x + s.B*s.u;
   s.P = s.A * s.P * s.A' + s.Q;

   % Compute Kalman gain factor:
   K = s.P*s.H'*inv(s.H*s.P*s.H'+s.R);

   % Correction based on observation:
   s.x = s.x + K*(s.z-s.H*s.x);
   s.P = s.P - K*s.H*s.P;

   % Notice that the desired result, which is an improved estimate
   % of the sytem state vector x and its covariance P, was obtained
   % in only five lines of code, once the system was defined. (That's
   % how simple the discrete Kalman filter is to use.) Later,
   % we'll discuss how to deal with nonlinear systems.

end

return

 */

using System;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents the state of a KalmanFilter
    /// </summary>
    internal struct KalmanSystemState
    {
        // Observations applied
        /// <summary>
        ///
        /// </summary>
        private int _interval;
        /// <summary>
        ///
        /// </summary>
        private DateTime _lastObservation;
        /// <summary>
        ///
        /// </summary>
        private TimeSpan _delay;

        // GPS Observation
        /// <summary>
        ///
        /// </summary>
        private double _deviceError;

        /// <summary>
        ///
        /// </summary>
        private double _horizontalDOP;
        /// <summary>
        ///
        /// </summary>
        private double _verticalDOP;

        /// <summary>
        ///
        /// </summary>
        private double _errorState;

        /// <summary>
        ///
        /// </summary>
        private readonly Ellipsoid _ellipsoid;

        // Vectors
        /// <summary>
        ///
        /// </summary>
        private CartesianPoint _x; // state vector estimate.
        /// <summary>
        ///
        /// </summary>
        private CartesianPoint _z; // observation vector
        /// <summary>
        ///
        /// </summary>
        private readonly CartesianPoint _u; // input control vector

        // Martrices
        /// <summary>
        ///
        /// </summary>
        private readonly SquareMatrix3D _a; // state transition matrix.
        /// <summary>
        ///
        /// </summary>
        private SquareMatrix3D _p; // covariance of the state vector estimate.
        /// <summary>
        ///
        /// </summary>
        private readonly SquareMatrix3D _b; // input matrix.
        /// <summary>
        ///
        /// </summary>
        private SquareMatrix3D _q; // process noise covariance.
        /// <summary>
        ///
        /// </summary>
        private SquareMatrix3D _r; // measurement noise covariance.
        /// <summary>
        ///
        /// </summary>
        private readonly SquareMatrix3D _h; // observation matrix.

        /// <summary>
        /// Initializes a new instance of the <see cref="KalmanSystemState"/> struct.
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="meanDOP">The mean DOP.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        internal KalmanSystemState(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP, Ellipsoid ellipsoid)
            : this(
                gpsPosition, deviceError, meanDOP, meanDOP, ellipsoid,
                CartesianPoint.Empty, CartesianPoint.Invalid, gpsPosition.ToCartesianPoint(),
                null, null, null,
                null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KalmanSystemState"/> struct.
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        internal KalmanSystemState(
    Position3D gpsPosition, Distance deviceError,
    DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP,
    Ellipsoid ellipsoid)
            : this(
                gpsPosition, deviceError, verticalDOP, horizontalDOP, ellipsoid,
                CartesianPoint.Empty, CartesianPoint.Invalid, gpsPosition.ToCartesianPoint(),
                null, null, null,
                null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KalmanSystemState"/> struct.
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="u">The u.</param>
        /// <param name="x">The x.</param>
        /// <param name="z">The z.</param>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <param name="h">The h.</param>
        /// <param name="p">The p.</param>
        /// <param name="q">The q.</param>
        /// <param name="r">The r.</param>
        internal KalmanSystemState(
    Position3D gpsPosition, Distance deviceError,
    DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP,
    Ellipsoid ellipsoid,
    CartesianPoint u, CartesianPoint x, CartesianPoint z,
    SquareMatrix3D a, SquareMatrix3D b, SquareMatrix3D h,
    SquareMatrix3D p, SquareMatrix3D q, SquareMatrix3D r)
        {
            _deviceError = deviceError.IsEmpty ? DilutionOfPrecision.CurrentAverageDevicePrecision.Value : deviceError.Value;
            _horizontalDOP = horizontalDOP.IsEmpty ? DilutionOfPrecision.Good.Value : horizontalDOP.Value;
            _verticalDOP = verticalDOP.IsEmpty ? DilutionOfPrecision.Good.Value : verticalDOP.Value;
            _ellipsoid = ellipsoid ?? Ellipsoid.Default;

            double hCovariance = _deviceError * _horizontalDOP;
            double vCovariance = _deviceError * _verticalDOP;

            _u = u.IsInvalid ? CartesianPoint.Empty : u;
            _x = x;
            _z = z.IsInvalid ? CartesianPoint.Empty : z;

            _a = a ?? new SquareMatrix3D();
            _b = b ?? new SquareMatrix3D();
            _h = h ?? new SquareMatrix3D();
            _p = p ?? new SquareMatrix3D();
            _q = q ?? SquareMatrix3D.Default(0);
            _r = r ?? new SquareMatrix3D(
                          hCovariance, 0, 0,
                          0, hCovariance, 0,
                          0, 0, vCovariance);

            _interval = 0;

            _errorState = Math.Sqrt(Math.Pow(hCovariance, 2) + Math.Pow(vCovariance, 2));

            _delay = TimeSpan.MaxValue;
            _lastObservation = DateTime.MinValue;

            if (!gpsPosition.IsEmpty)
                Initialize(gpsPosition);
        }

        /// <summary>
        /// Gets the delay.
        /// </summary>
        public TimeSpan Delay
        {
            get { return _delay; }
        }

        /// <summary>
        /// Gets the current error.
        /// </summary>
        public Distance CurrentError
        {
            get
            {
                return Distance.FromMeters(
                    Math.Sqrt(
                        Math.Pow(_p.Elements[0], 2) +
                        Math.Pow(_p.Elements[4], 2) +
                        Math.Pow(_p.Elements[8], 2)
                        ));
            }
        }

        /// <summary>
        /// Gets the state of the error.
        /// </summary>
        public Distance ErrorState
        {
            get { return Distance.FromMeters(_errorState); }
        }

        /// <summary>
        /// Gets the device error.
        /// </summary>
        public Distance DeviceError
        {
            get { return Distance.FromMeters(_deviceError); }
        }

        /// <summary>
        /// Gets the horizontal dilution of precision.
        /// </summary>
        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return new DilutionOfPrecision((float)_horizontalDOP); }
        }

        /// <summary>
        /// Gets the vertical dilution of precision.
        /// </summary>
        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return new DilutionOfPrecision((float)_verticalDOP); }
        }

        /// <summary>
        /// The position reported by the GPS device
        /// </summary>
        /// <returns></returns>
        public Position3D ObservedLocation()
        {
            return _z.ToPosition3D(_ellipsoid);
        }

        /// <summary>
        /// Determines if the Kalman state has an initial observation
        /// </summary>
        public bool IsInitialized { get { return !_x.IsInvalid; } }

        /// <summary>
        /// Returns the number of intervals that have been applied to the Kalman state
        /// </summary>
        /// <remarks>The number of intervals is the number of observations accumulated in the state
        /// interval. The greater the number of observations, the more precise the filter
        /// becomes.</remarks>
        public int IntervalCount { get { return _interval; } }

        /// <summary>
        /// The corrected position
        /// </summary>
        /// <returns></returns>
        public Position3D CorrectedLocation()
        {
            return _x.ToPosition3D(_ellipsoid);
        }

        /// <summary>
        /// Initializes the state to the supplied observation.
        /// </summary>
        /// <param name="z">The z.</param>
        public void Initialize(Position3D z)
        {
            SquareMatrix3D hi = SquareMatrix3D.Invert(_h);

            _z = z.ToCartesianPoint(_ellipsoid);

            //s.x = inv(s.H)*s.z;
            _x = hi * _z;

            //s.P = inv(s.H)*s.R*inv(s.H');
            _p = hi * _r * SquareMatrix3D.Invert(SquareMatrix3D.Transpose(_h));

            _lastObservation = DateTime.Now;
            _delay = TimeSpan.Zero;
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="currentDOP">The current DOP.</param>
        /// <param name="z">The z.</param>
        public void UpdateState(DilutionOfPrecision currentDOP, Position3D z)
        {
            UpdateState(Distance.FromMeters(_deviceError), currentDOP, currentDOP, Azimuth.Empty, Speed.AtRest, z);
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="z">The z.</param>
        public void UpdateState(Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed, Position3D z)
        {
            if (_x.IsInvalid)
            {
                Initialize(z);
                return;
            }

            // More insanity
            double fail = horizontalDOP.Value * verticalDOP.Value * deviceError.Value;
            if (fail == 0 || double.IsNaN(fail) || double.IsInfinity(fail))
            {
                throw new ArgumentException(
                    "Covariance values are invalid. Parameters deviceError, horizontalDOP and verticalDOP must be greater than zero.");
            }

            _deviceError = deviceError.Value;
            _horizontalDOP = horizontalDOP.Value;
            _verticalDOP = verticalDOP.Value;

            double hCovariance = _deviceError * _horizontalDOP;
            double vCovariance = _deviceError * _verticalDOP;

            // Setup the observation covariance (measurement error)
            _r = new SquareMatrix3D(
                hCovariance, 0, 0,
                0, hCovariance, 0,
                0, 0, vCovariance);

            #region Process Noise Estimation

            // Get the translation of the last correction
            CartesianPoint subX = _x.ToPosition3D(_ellipsoid)
                .TranslateTo(bearing, speed.ToDistance(_delay), _ellipsoid)
                .ToCartesianPoint();

            // Get the vector of the translation and the last observation
            //CartesianPoint w = (subX - this.z);
            CartesianPoint w =
                new CartesianPoint(
                    Distance.FromMeters(subX.X.Value - _z.X.Value),   // Values are in meters
                    Distance.FromMeters(subX.Y.Value - _z.Y.Value),   // Values are in meters
                    Distance.FromMeters(subX.Z.Value - _z.Z.Value));  // Values are in meters

            // Setup the noise covariance (process error)
            _q = new SquareMatrix3D(
                Math.Abs(w.X.Value), 0, 0,
                0, Math.Abs(w.Y.Value), 0,
                0, 0, Math.Abs(w.Z.Value));

            #endregion Process Noise Estimation

            // Update the observation state
            _z = z.ToCartesianPoint(_ellipsoid);

            #region State vector prediction and covariance

            //s.x = s.A*s.x + s.B*s.u;
            //this.x = this.A * this.x + this.B * this.u;
            CartesianPoint ax = _a.TransformVector(_x);
            CartesianPoint bu = _b.TransformVector(_u);
            _x =
                new CartesianPoint(
                    Distance.FromMeters(ax.X.Value + bu.X.Value),
                    Distance.FromMeters(ax.Y.Value + bu.Y.Value),
                    Distance.FromMeters(ax.Z.Value + bu.Z.Value));

            //s.P = s.A * s.P * s.A' + s.Q;
            _p = _a * _p * SquareMatrix3D.Transpose(_a) + _q;

            #endregion State vector prediction and covariance

            #region Kalman gain factor

            //K = s.P*s.H'*inv(s.H*s.P*s.H'+s.R);
            SquareMatrix3D ht = SquareMatrix3D.Transpose(_h);
            SquareMatrix3D k = _p * ht * SquareMatrix3D.Invert(_h * _p * ht + _r);

            #endregion Kalman gain factor

            #region Observational correction

            //s.x = s.x + K*(s.z-s.H*s.x);
            //this.x = this.x + K * (this.z - this.H * this.x);
            CartesianPoint hx = _h.TransformVector(_x);
            CartesianPoint zHx = new CartesianPoint(
                Distance.FromMeters(_z.X.Value - hx.X.Value),
                Distance.FromMeters(_z.Y.Value - hx.Y.Value),
                Distance.FromMeters(_z.Z.Value - hx.Z.Value));
            CartesianPoint kzHx = k.TransformVector(zHx);
            _x =
                new CartesianPoint(
                    Distance.FromMeters(_x.X.Value + kzHx.X.Value),
                    Distance.FromMeters(_x.Y.Value + kzHx.Y.Value),
                    Distance.FromMeters(_x.Z.Value + kzHx.Z.Value));

            //s.P = s.P - K*s.H*s.P;
            _p = _p - k * _h * _p;

            #endregion Observational correction

            // Bump the state count
            _interval++;

            // Calculate the average error for the system stste.
            _errorState = (_errorState + Math.Sqrt(Math.Pow(hCovariance, 2) + Math.Pow(vCovariance, 2))) * .5f;

            // Calculate the interval between samples
            DateTime now = DateTime.Now;
            _delay = now.Subtract(_lastObservation);
            _lastObservation = now;
        }
    }

    /// <summary>
    /// A class that employs a Kalman Filter algorithm to reduce error in GPS
    /// precision.
    /// </summary>
    public sealed class KalmanFilter : PrecisionFilter
    {
        #region Private Members

        // The current state of the Kalman filter
        /// <summary>
        ///
        /// </summary>
        private KalmanSystemState _currentState;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Kalman Filter
        /// </summary>
        public KalmanFilter()
            : this(
                new Position3D(Position.Empty),
                DilutionOfPrecision.CurrentAverageDevicePrecision,
                DilutionOfPrecision.Good,
                DilutionOfPrecision.Good,
                Ellipsoid.Default)
        { }

        /// <summary>
        /// Kalman Filter with parameters
        /// </summary>
        /// <param name="initialObservation">The initial observation.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        public KalmanFilter(
    Position3D initialObservation,
    Distance deviceError,
    DilutionOfPrecision horizontalDOP,
    DilutionOfPrecision verticalDOP,
    Ellipsoid ellipsoid)
        {
            _currentState = new KalmanSystemState(
                initialObservation,
                deviceError,
                horizontalDOP,
                verticalDOP,
                ellipsoid);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// The filtered location
        /// </summary>
        public override Position3D FilteredLocation
        {
            get { return _currentState.CorrectedLocation(); }
        }

        /// <summary>
        /// The observed location
        /// </summary>
        public override Position3D ObservedLocation
        {
            get { return _currentState.ObservedLocation(); }
        }

        /// <summary>
        /// Represents the latency between the current obervation and the filter state
        /// </summary>
        public override TimeSpan Delay
        {
            get { return _currentState.Delay; }
        }

        /// <summary>
        /// Returns a value indicationg whether or not the filter has been initialized.
        /// </summary>
        public override bool IsInitialized
        {
            get { return _currentState.IsInitialized; }
        }

        /// <summary>
        /// Returns the DilutionOfPrecision used in the most recent
        /// Kalman filter calculation.
        /// </summary>
        public DilutionOfPrecision CurrentMeanDilutionOfPrecision
        {
            get { return _currentState.HorizontalDilutionOfPrecision; }
        }

        /// <summary>
        /// Returns the device error margin used in the most recent
        /// Kalman filter calculation.
        /// </summary>
        public Distance CurrentDeviceError
        {
            get { return _currentState.DeviceError; }
        }

        /// <summary>
        /// Retrns the of position samples that have been evaluated by
        /// the current Kalman state.
        /// </summary>
        /// <remarks>The results of a Kalman filter are cumulative. Each
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position.</remarks>
        public int CurrentStateSampleCount
        {
            get { return _currentState.IntervalCount; }
        }

        /// <summary>
        /// Returns the accumulated average error accounted for the Kalman system.
        /// </summary>
        /// <remarks>The results of a Kalman filter are cumulative. Each
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position.</remarks>
        public Distance ErrorState
        {
            get { return _currentState.ErrorState; }
        }

        /// <summary>
        /// Returns the current precision estimate for the Kalman state
        /// </summary>
        /// <remarks>The results of a Kalman filter are cumulative. Each
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position.</remarks>
        public Distance CurrentErrorEstimate
        {
            get { return _currentState.CurrentError; }
        }

        #endregion Public Properties

        #region Position Initializers

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <remarks>The results of a Kalman filter are cumulative. Each
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position.</remarks>
        public override void Initialize(Position gpsPosition)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">Distance of the error</param>
        public void Initialize(Position gpsPosition, Distance deviceError)
        {
            Initialize(gpsPosition, deviceError, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="meanDOP">The mean dilution of precision</param>
        public void Initialize(Position gpsPosition, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, meanDOP, meanDOP, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">Distance of the error</param>
        /// <param name="meanDOP">The mean dilution of precision</param>
        public void Initialize(Position gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">Distance of the error</param>
        /// <param name="meanDOP">The mean dilution of precision</param>
        /// <param name="ellipsoid">The ellipsoid</param>
        public void Initialize(Position gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP, Ellipsoid ellipsoid)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, ellipsoid);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">Distance of the error</param>
        /// <param name="horizontalDOP">The horizontal dilution of precision</param>
        /// <param name="verticalDOP">The vertical dilution of precision</param>
        /// <param name="ellipsoid">The ellipsoid</param>
        public void Initialize(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Ellipsoid ellipsoid)
        {
            _currentState = new KalmanSystemState(new Position3D(gpsPosition), deviceError, horizontalDOP, verticalDOP, ellipsoid);
        }

        #endregion Position Initializers

        #region Position3D Initializers

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <remarks>The results of a Kalman filter are cumulative. Each
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position.</remarks>
        public override void Initialize(Position3D gpsPosition)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">A distance measure of device error</param>
        public void Initialize(Position3D gpsPosition, Distance deviceError)
        {
            Initialize(gpsPosition, deviceError, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="meanDOP">The mean dilution of precision</param>
        public void Initialize(Position3D gpsPosition, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, meanDOP, meanDOP, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">A distance measure of device error</param>
        /// <param name="meanDOP">The mean dilution of precision</param>
        public void Initialize(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, Ellipsoid.Default);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">A distance measure of device error</param>
        /// <param name="meanDOP">The mean dilution of precision</param>
        /// <param name="ellipsoid">The ellipsoid</param>
        public void Initialize(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP, Ellipsoid ellipsoid)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, ellipsoid);
        }

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition">The position at which tfilter is to begin opperating.</param>
        /// <param name="deviceError">A distance measure of device error</param>
        /// <param name="horizontalDOP">The horizontal dilution of precision</param>
        /// <param name="verticalDOP">The vertical dilution of precision</param>
        /// <param name="ellipsoid">The ellipsoid</param>
        public void Initialize(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Ellipsoid ellipsoid)
        {
            double fail = horizontalDOP.Value * verticalDOP.Value * deviceError.Value;
            if (fail == 0 || double.IsNaN(fail) || double.IsInfinity(fail))
            {
                throw new ArgumentException(
                    "Parameters deviceError, horizontalDOP and verticalDOP must be greater than zero.");
            }

            _currentState = new KalmanSystemState(gpsPosition, deviceError, horizontalDOP, verticalDOP, ellipsoid);
        }

        #endregion Position3D Initializers

        #region Filters

        /// <summary>
        /// Filter the Position
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <returns></returns>
        /// <inheritdocs/>
        public override Position Filter(Position gpsPosition)
        {
            return Filter(gpsPosition, _currentState.DeviceError, _currentState.HorizontalDilutionOfPrecision, _currentState.VerticalDilutionOfPrecision, Azimuth.Empty, Speed.AtRest);
        }

        /// <summary>
        /// Returns the position
        /// </summary>
        /// <param name="gpsPosition">The gps Position</param>
        /// <param name="currentDOP">The current dilution of precision</param>
        /// <returns>A Position sturcture</returns>
        public Position Filter(Position gpsPosition, DilutionOfPrecision currentDOP)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, Azimuth.Empty, Speed.AtRest);
        }

        /// <summary>
        /// Returns the position
        /// </summary>
        /// <param name="gpsPosition">The gps Position</param>
        /// <param name="currentDOP">The current dilution of precision</param>
        /// <param name="bearing">the directional azimuth</param>
        /// <param name="speed">the magnitude of the velocity</param>
        /// <returns>A Position sturcture</returns>
        public Position Filter(Position gpsPosition, DilutionOfPrecision currentDOP, Azimuth bearing, Speed speed)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, bearing, speed);
        }

        /// <summary>
        /// Return filtered position from specified parameters
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        /// <inheritdocs/>
        public override Position Filter(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            Position3D pos3D = Filter(new Position3D(gpsPosition), deviceError, horizontalDOP, verticalDOP, bearing, speed);
            return new Position(pos3D.Latitude, pos3D.Longitude);
        }

        /// <summary>
        /// Returns the 3D position
        /// </summary>
        /// <param name="gpsPosition">The gps Position</param>
        /// <returns>A Position3D sturcture</returns>
        public override Position3D Filter(Position3D gpsPosition)
        {
            return Filter(gpsPosition, _currentState.DeviceError, _currentState.HorizontalDilutionOfPrecision, _currentState.VerticalDilutionOfPrecision, Azimuth.Empty, Speed.AtRest);
        }

        /// <summary>
        /// Returns the 3D position
        /// </summary>
        /// <param name="gpsPosition">The gps Position</param>
        /// <param name="currentDOP">The current dilution of precision</param>
        /// <returns>A Position3D sturcture</returns>
        public Position3D Filter(Position3D gpsPosition, DilutionOfPrecision currentDOP)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, Azimuth.Empty, Speed.AtRest);
        }

        /// <summary>
        /// Returns the 3D position
        /// </summary>
        /// <param name="gpsPosition">The gps Position</param>
        /// <param name="currentDOP">The current dilution of precision</param>
        /// <param name="bearing">the directional azimuth</param>
        /// <param name="speed">the magnitude of the velocity</param>
        /// <returns>A Position3D sturcture</returns>
        public Position3D Filter(Position3D gpsPosition, DilutionOfPrecision currentDOP, Azimuth bearing, Speed speed)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, bearing, Speed.AtRest);
        }

        /// <summary>
        /// Return a filtered Position3D from the specified parameters
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public override Position3D Filter(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            double fail = horizontalDOP.Value * verticalDOP.Value * deviceError.Value;
            if (fail == 0 || double.IsNaN(fail) || double.IsInfinity(fail))
            {
                throw new ArgumentException(
                    "Parameters deviceError, horizontalDOP and verticalDOP must be greater than zero.");
            }

            _currentState.UpdateState(deviceError, horizontalDOP, verticalDOP, bearing, speed, gpsPosition);
            return _currentState.CorrectedLocation();
        }

        #endregion Filters
    }
}
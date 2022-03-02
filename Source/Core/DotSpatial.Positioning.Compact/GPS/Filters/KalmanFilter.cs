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
% where w ~ N(0,Q) meaning w is gaussian noise with covariance Q
%       v ~ N(0,R) meaning v is gaussian noise with covariance R
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
% (1) define all state definition fields: A,B,H,Q,R
% (2) define intial state estimate: x,P
% (3) obtain observation and control vectors: z,u
% (4) call the filter to obtain updated state estimate: x,P
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
% legend([hz hk ht],'observations','Kalman output','true voltage',0)
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
   
   % Note that the desired result, which is an improved estimate
   % of the sytem state vector x and its covariance P, was obtained
   % in only five lines of code, once the system was defined. (That's
   % how simple the discrete Kalman filter is to use.) Later,
   % we'll discuss how to deal with nonlinear systems.

end

return

 */
using System;
using System.Collections.Generic;

namespace DotSpatial.Positioning.Gps.Filters
{
    /// <summary>
    /// Represents the state of a KalmanFilter
    /// </summary>
    internal struct KalmanSystemState
    {
        // Observations applied
        private int _interval;
        private DateTime _lastObservation;
        private TimeSpan _delay;

        // GPS Observation
        private double _deviceError;

        private double _horizontalDOP;
        private double _verticalDOP;

        private double _errorState;

        private Ellipsoid _ellipsoid;

        // Vectors
        private CartesianPoint x; // state vector estimate. 
        private CartesianPoint z; // observation vector
        private CartesianPoint u; // input control vector

        // Martrices
        private SquareMatrix3D A; // state transition matrix.
        private SquareMatrix3D P; // covariance of the state vector estimate.
        private SquareMatrix3D B; // input matrix.
        private SquareMatrix3D Q; // process noise covariance.
        private SquareMatrix3D R; // measurement noise covariance.
        private SquareMatrix3D H; // observation matrix.

        internal KalmanSystemState(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP, Ellipsoid ellipsoid)
            : this(
                gpsPosition, deviceError, meanDOP, meanDOP, ellipsoid,
                CartesianPoint.Empty, CartesianPoint.Invalid, gpsPosition.ToCartesianPoint(),
                null, null, null,
                null, null, null)
        { }

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

        internal KalmanSystemState(
            Position3D gpsPosition, Distance deviceError,
            DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP,
            Ellipsoid ellipsoid,
            CartesianPoint u, CartesianPoint x, CartesianPoint z,
            SquareMatrix3D A, SquareMatrix3D B, SquareMatrix3D H,
            SquareMatrix3D P, SquareMatrix3D Q, SquareMatrix3D R)
        {
            this._deviceError = deviceError.IsEmpty ? DilutionOfPrecision.CurrentAverageDevicePrecision.Value : deviceError.Value;
            this._horizontalDOP = horizontalDOP.IsEmpty ? DilutionOfPrecision.Good.Value : horizontalDOP.Value;
            this._verticalDOP = verticalDOP.IsEmpty ? DilutionOfPrecision.Good.Value : verticalDOP.Value;
            this._ellipsoid = ellipsoid == null ? Ellipsoid.Default : ellipsoid;

            double hCovariance = this._deviceError * this._horizontalDOP;
            double vCovariance = this._deviceError * this._verticalDOP;

            this.u = u.IsInvalid ? CartesianPoint.Empty : u;
            this.x = x;
            this.z = z.IsInvalid ? CartesianPoint.Empty : z;

            this.A = A == null ? new SquareMatrix3D() : A;
            this.B = B == null ? new SquareMatrix3D() : B;
            this.H = H == null ? new SquareMatrix3D() : H;
            this.P = P == null ? new SquareMatrix3D() : P;
            this.Q = Q == null ? SquareMatrix3D.Default(0) : Q;
            this.R = R == null ? new SquareMatrix3D(
                hCovariance, 0, 0,
                0, hCovariance, 0,
                0, 0, vCovariance) : R;

            this._interval = 0;

            this._errorState = Math.Sqrt(Math.Pow(hCovariance, 2) + Math.Pow(vCovariance, 2));

            this._delay = TimeSpan.MaxValue;
            this._lastObservation = DateTime.MinValue;

            if (!gpsPosition.IsEmpty)
                Initialize(gpsPosition);
        }

        public TimeSpan Delay
        {
            get { return _delay; }
        }

        public Distance CurrentError
        {
            get
            {
                return Distance.FromMeters(
                    Math.Sqrt(
                        Math.Pow(this.P.Elements[0],2) +
                        Math.Pow(this.P.Elements[4],2) +
                        Math.Pow(this.P.Elements[8],2)
                        ));
            }
        }

        public Distance ErrorState
        {
            get { return Distance.FromMeters(_errorState); }
        }

        public Distance DeviceError
        {
            get { return Distance.FromMeters(_deviceError); }
        }

        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return new DilutionOfPrecision((float)_horizontalDOP); }
        }

        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return new DilutionOfPrecision((float)_verticalDOP); }
        }

        /// <summary>
        /// The position reported by the GPS device
        /// </summary>
        public Position3D ObservedLocation()
        {
            return z.ToPosition3D(_ellipsoid);
        }

        /// <summary>
        /// Determines if the Kalman state has an initial observation
        /// </summary>
        public bool IsInitialized { get { return !x.IsInvalid; } }
        /// <summary>
        /// Returns the number of intervals that have been applied to the Kalman state
        /// </summary>
        /// <remarks>
        /// The number of intervals is the number of observations accumulated in the state
        /// interval. The greater the number of observations, the more precise the filter 
        /// becomes.
        /// </remarks>
        public int IntervalCount { get { return _interval; } }

        /// <summary>
        /// The corrected position 
        /// </summary>
        public Position3D CorrectedLocation()
        {
            return x.ToPosition3D(_ellipsoid);
        }

        /// <summary>
        /// Initializes the state to the supplied observation.
        /// </summary>
        /// <param name="z"></param>
        public void Initialize(Position3D z)
        {
            SquareMatrix3D Hi = SquareMatrix3D.Invert(this.H);

            this.z = z.ToCartesianPoint(_ellipsoid);

            //s.x = inv(s.H)*s.z;
            this.x = Hi * this.z;

            //s.P = inv(s.H)*s.R*inv(s.H'); 
            this.P = Hi * this.R * SquareMatrix3D.Invert(SquareMatrix3D.Transpose(this.H));
        
            _lastObservation = DateTime.Now;
            _delay = TimeSpan.Zero;
        }

        public void UpdateState(DilutionOfPrecision currentDOP, Position3D z)
        {
            UpdateState(Distance.FromMeters(_deviceError), currentDOP, currentDOP, Azimuth.Empty, Speed.AtRest, z);
        }

        public void UpdateState(Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed, Position3D z)
        {
            if (this.x.IsInvalid)
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

            this._deviceError = deviceError.Value;
            this._horizontalDOP = horizontalDOP.Value;
            this._verticalDOP = verticalDOP.Value;

            double hCovariance = this._deviceError * this._horizontalDOP;
            double vCovariance = this._deviceError * this._verticalDOP;

            // Setup the observation covariance (measurement error)
            this.R = new SquareMatrix3D(
                hCovariance, 0, 0,
                0, hCovariance, 0,
                0, 0, vCovariance);

            #region Process Noise Estimation

            // Get the translation of the last correction
            CartesianPoint subX = this.x.ToPosition3D(_ellipsoid)
                .TranslateTo(bearing, speed.ToDistance(_delay),_ellipsoid)
                .ToCartesianPoint();

            // Get the vector of the translation and the last observation
            //CartesianPoint w = (subX - this.z);
            CartesianPoint w =
                new CartesianPoint(
                    Distance.FromMeters(subX.X.Value - this.z.X.Value),   // Values are in meters
                    Distance.FromMeters(subX.Y.Value - this.z.Y.Value),   // Values are in meters
                    Distance.FromMeters(subX.Z.Value - this.z.Z.Value));  // Values are in meters

            // Setup the noise covariance (process error)
            this.Q = new SquareMatrix3D(
                Math.Abs(w.X.Value), 0, 0,
                0, Math.Abs(w.Y.Value), 0,
                0, 0, Math.Abs(w.Z.Value));

            #endregion

            // Update the observation state
            this.z = z.ToCartesianPoint(_ellipsoid);

            #region State vector prediction and covariance

            //s.x = s.A*s.x + s.B*s.u;
            //this.x = this.A * this.x + this.B * this.u;
            CartesianPoint Ax = this.A.TransformVector(x);
            CartesianPoint Bu = this.B.TransformVector(u);
            this.x =
                new CartesianPoint(
                    Distance.FromMeters(Ax.X.Value + Bu.X.Value),
                    Distance.FromMeters(Ax.Y.Value + Bu.Y.Value), 
                    Distance.FromMeters(Ax.Z.Value + Bu.Z.Value));

            //s.P = s.A * s.P * s.A' + s.Q;
            this.P = this.A * this.P * SquareMatrix3D.Transpose(this.A) + this.Q;

            #endregion

            #region Kalman gain factor

            //K = s.P*s.H'*inv(s.H*s.P*s.H'+s.R);
            SquareMatrix3D Ht = SquareMatrix3D.Transpose(this.H);
            SquareMatrix3D K = this.P * Ht * SquareMatrix3D.Invert(this.H * this.P * Ht + this.R);

            #endregion

            #region Observational correction

            //s.x = s.x + K*(s.z-s.H*s.x);
            //this.x = this.x + K * (this.z - this.H * this.x);
            CartesianPoint Hx = this.H.TransformVector(x);
            CartesianPoint zHx = new CartesianPoint(
                Distance.FromMeters(this.z.X.Value - Hx.X.Value),
                Distance.FromMeters(this.z.Y.Value - Hx.Y.Value),
                Distance.FromMeters(this.z.Z.Value - Hx.Z.Value));
            CartesianPoint kzHx = K.TransformVector(zHx);
            this.x = 
                new CartesianPoint(
                    Distance.FromMeters(this.x.X.Value + kzHx.X.Value),
                    Distance.FromMeters(this.x.Y.Value + kzHx.Y.Value),
                    Distance.FromMeters(this.x.Z.Value + kzHx.Z.Value));

            //s.P = s.P - K*s.H*s.P;
            this.P = this.P - K * this.H * this.P;

            #endregion

            // Bump the state count
            this._interval++;

            // Calculate the average error for the system stste.
            this._errorState = (this._errorState + Math.Sqrt(Math.Pow(hCovariance, 2) + Math.Pow(vCovariance, 2))) * .5f;

            // Calculate the interval between samples
            DateTime now = DateTime.Now;
            this._delay = now.Subtract(_lastObservation);
            this._lastObservation = now;
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
        private KalmanSystemState _currentState;

        #endregion

        #region Constructors

        public KalmanFilter()
            : this(
                new Position3D(Position.Empty),
                DilutionOfPrecision.CurrentAverageDevicePrecision,
                DilutionOfPrecision.Good,
                DilutionOfPrecision.Good,
                Ellipsoid.Default)
        { }

        public KalmanFilter(
            Position3D initialObservation, 
            Distance deviceError, 
            DilutionOfPrecision horizontalDOP, 
            DilutionOfPrecision verticalDOP,
            Ellipsoid ellipsoid)
        {
            this._currentState = new KalmanSystemState(
                initialObservation, 
                deviceError, 
                horizontalDOP, 
                verticalDOP,
                ellipsoid);
        }

        #endregion

        #region Public Properties

        public override Position3D FilteredLocation
        {
            get { return _currentState.CorrectedLocation(); }
        }

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
        /// <remarks> The results of a Kalman filter are cumulative. Each 
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position. </remarks>
        public int CurrentStateSampleCount
        {
            get { return _currentState.IntervalCount; }
        }

        /// <summary>
        /// Returns the accumulated average error accounted for the Kalman system.
        /// </summary>
        /// <remarks> The results of a Kalman filter are cumulative. Each 
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position. </remarks>
        public Distance ErrorState
        {
            get { return _currentState.ErrorState; }
        }

        /// <summary>
        /// Returns the current precision estimate for the Kalman state
        /// </summary>
        /// <remarks> The results of a Kalman filter are cumulative. Each 
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position. </remarks>
        public Distance CurrentErrorEstimate
        {
            get { return _currentState.CurrentError; }
        }

        #endregion

        #region Position Initializers

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition"> The position at which tfilter is to begin opperating. </param>
        /// <remarks> The results of a Kalman filter are cumulative. Each 
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position. </remarks>
        public override void Initialize(Position gpsPosition)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        public void Initialize(Position gpsPosition, Distance deviceError)
        {
            Initialize(gpsPosition, deviceError, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        public void Initialize(Position gpsPosition, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, meanDOP, meanDOP, Ellipsoid.Default);
        }

        public void Initialize(Position gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, Ellipsoid.Default);
        }

        public void Initialize(Position gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP, Ellipsoid ellipsoid)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, ellipsoid);
        }

        public void Initialize(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Ellipsoid ellipsoid)
        {
            _currentState = new KalmanSystemState(new Position3D(gpsPosition), deviceError, horizontalDOP, verticalDOP, ellipsoid);
        }

        #endregion

        #region Position3D Initializers

        /// <summary>
        /// Initializes the Kalman Filter using an initial observation (position)
        /// </summary>
        /// <param name="gpsPosition"> The position at which tfilter is to begin opperating. </param>
        /// <remarks> The results of a Kalman filter are cumulative. Each 
        /// position processed changes the state of the filter, thus making
        /// the results more accurate with each subsequent position. </remarks>
        public override void Initialize(Position3D gpsPosition)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        public void Initialize(Position3D gpsPosition, Distance deviceError)
        {
            Initialize(gpsPosition, deviceError, DilutionOfPrecision.Good, Ellipsoid.Default);
        }

        public void Initialize(Position3D gpsPosition, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, DilutionOfPrecision.CurrentAverageDevicePrecision, meanDOP, meanDOP, Ellipsoid.Default);
        }

        public void Initialize(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, Ellipsoid.Default);
        }

        public void Initialize(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision meanDOP, Ellipsoid ellipsoid)
        {
            Initialize(gpsPosition, deviceError, meanDOP, meanDOP, ellipsoid);
        }

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

        #endregion

        #region Filters

        public override Position Filter(Position gpsPosition)
        {
            return Filter(gpsPosition, _currentState.DeviceError, _currentState.HorizontalDilutionOfPrecision, _currentState.VerticalDilutionOfPrecision, Azimuth.Empty, Speed.AtRest);
        }

        public Position Filter(Position gpsPosition, DilutionOfPrecision currentDOP)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, Azimuth.Empty, Speed.AtRest);
        }

        public Position Filter(Position gpsPosition, DilutionOfPrecision currentDOP, Azimuth bearing, Speed speed)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, bearing, speed);
        }

        public override Position Filter(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed)
        {
            Position3D pos3d = Filter(new Position3D(gpsPosition), deviceError, horizontalDOP, verticalDOP, bearing, speed);
            return new Position(pos3d.Latitude, pos3d.Longitude);
        }

        public override Position3D Filter(Position3D gpsPosition)
        {
            return Filter(gpsPosition, _currentState.DeviceError, _currentState.HorizontalDilutionOfPrecision, _currentState.VerticalDilutionOfPrecision, Azimuth.Empty, Speed.AtRest);
        }

        public Position3D Filter(Position3D gpsPosition, DilutionOfPrecision currentDOP)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, Azimuth.Empty, Speed.AtRest);
        }

        public Position3D Filter(Position3D gpsPosition, DilutionOfPrecision currentDOP, Azimuth bearing, Speed speed)
        {
            return Filter(gpsPosition, _currentState.DeviceError, currentDOP, currentDOP, bearing, Speed.AtRest);
        }

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

        #endregion
    }
}

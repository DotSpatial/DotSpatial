using System;
using System.Threading;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
	/// <summary>Represents a specific location on Earth's surface.</summary>
	/// <remarks>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (its properties can only be changed via constructors).</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.PositionConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Position : IFormattable, IEquatable<Position>, ICloneable<Position>, IXmlSerializable
    {
        private Latitude _Latitude;
        private Longitude _Longitude;
                
        #region Constants

        // Accuracy is set to 1.0E-12, the smallest value allowed by a Latitude or Longitude
        private const double TargetAccuracy = 1.0E-12;

        #endregion

        #region Fields

        /// <summary>Represents the location at 0°, 0°.</summary>
		public static readonly Position Empty = new Position(Latitude.Empty, Longitude.Empty);
		/// <summary>Represents the smallest possible location of 90°S, 180°W.</summary>
		public static readonly Position Minimum = new Position(Latitude.Minimum, Longitude.Minimum);
        /// <summary>Represents the largest possible location of 90°N, 180°E.</summary>
        public static readonly Position Maximum = new Position(Latitude.Maximum, Longitude.Maximum);
        /// <summary>Represents the single point at the top of Earth: 90°N, 0°E.</summary>
        public static readonly Position NorthPole = new Position(Latitude.Maximum, Longitude.Empty);
        /// <summary>Represents the single point at the bottom of Earth: 90°S, 0°E.</summary>
        public static readonly Position SouthPole = new Position(Latitude.Minimum, Longitude.Empty);
        /// <summary>Represents an invalid or unspecified value.</summary>
        public static readonly Position Invalid = new Position(Latitude.Invalid, Longitude.Invalid);

        #endregion

        #region  Constructors

        /// <summary>
        /// Creates a new instance from the specified longitude and latitude.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
		public Position(Longitude longitude, Latitude latitude) 			
		{
			_Latitude = latitude;
			_Longitude = longitude;
		}

        /// <summary>
        /// Creates a new instance from the specified latitude and longitude.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public Position(Latitude latitude, Longitude longitude)
		{			
			_Latitude = latitude;
			_Longitude = longitude;
        }

		/// <summary>
		/// Creates a new instance by parsing latitude and longitude from a single string.
		/// </summary>
		/// <param name="value">A <strong>String</strong> containing both a latitude and longitude to parse.</param>
		public Position(string value) 
			: this(value, CultureInfo.CurrentCulture)
		{}

        /// <summary>
        /// Creates a new instance by interpreting the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">A <strong>String</strong> describing a latitude in the current culture.</param>
        /// <param name="longitude">A <strong>String</strong> describing a longitude in the current culture.</param>
        /// <remarks>Latitude and longitude values are parsed using the current local culture.  For better support
        /// of international cultures, add a CultureInfo parameter.</remarks>
		public Position(string latitude, string longitude) 
			: this(latitude, longitude, CultureInfo.CurrentCulture)
		{}

        /// <summary>
        /// Creates a new instance by interpreting the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">A <strong>String</strong> describing a latitude in the current culture.</param>
        /// <param name="longitude">A <strong>String</strong> describing a longitude in the current culture.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> describing the specific culture</param>
        /// <remarks>Latitude and longitude values are parsed using the current local culture.  For better support
        /// of international cultures, a CultureInfo parameter should be specified to indicate how numbers should
        /// be parsed.</remarks>
        public Position(string latitude, string longitude, CultureInfo culture)
            : this(Latitude.Parse(latitude, culture),
            Longitude.Parse(longitude, culture))
        { }

        /// <summary>
        /// Creates a new instance by converting the specified string using the specific culture.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        public Position(string value, CultureInfo culture)
        {
            // Empty values mean "Empty"
            if (value.Length == 0)
            {
                _Latitude = Latitude.Empty;
                _Longitude = Longitude.Empty;
                return;
            }
            else if (value == "Empty")
            {
                _Latitude = Latitude.Empty;
                _Longitude = Longitude.Empty;
                return;
            }

            // Try to parse the value as latitude/longitude
            Position Result = ParseAsLatLong(value, culture);
            if (!Result.IsInvalid) 
            {
                _Latitude = Result.Latitude.Normalize();
                _Longitude = Result.Longitude.Normalize();
                return;
            }

            // Raise an exception
#if PocketPC
                throw new ArgumentException(Properties.Resources.Position_InvalidFormat);
#else
            throw new ArgumentException(Properties.Resources.Position_InvalidFormat, "value");
#endif

        }

        /// <summary>
        /// Creates a copy of the specified object.
        /// </summary>
        /// <param name="position"></param>
        public Position(Position position)
            : this(position.Latitude, position.Longitude)
        {}
        
        /// <summary>
        /// Creates a new position by deserializing the specified XML content.
        /// </summary>
        /// <param name="reader"></param>
        public Position(XmlReader reader)
        {
            // Initialize all fields
            _Latitude = Latitude.Invalid;
            _Longitude = Longitude.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

		#endregion

        #region Public Properties

        /// <summary>Represents the vertical North/South portion of the location.</summary>
        public Latitude Latitude
        {
            get { return _Latitude; }
        }

        /// <summary>Represents the horizontal East/West portion of the location.</summary>
        public Longitude Longitude
        {
            get { return _Longitude; }
        }

        /// <summary>Indicates if the position has no value.</summary>
        public bool IsEmpty
        {
            get
            {
                return _Latitude.IsEmpty && _Longitude.IsEmpty;
            }
        }

        /// <summary>Indicates if the position has an invalid or unspecified value.</summary>
        public bool IsInvalid
        {
            get
            {
                return _Latitude.IsInvalid || _Longitude.IsInvalid;
            }
        }

        /// <summary>Indicates whether the position has been normalized and is within the 
        /// allowed bounds of -90° and 90° latitude and -180° and 180° longitude.</summary>
        public bool IsNormalized
        {
            get
            {
                return _Latitude.IsNormalized && _Longitude.IsNormalized;
            }
        }

        #endregion

        #region Public Methods


        /// <overloads>Outputs the current instance as a formatted string.</overloads>
        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <returns></returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the current instance into an Earth-centered, Earth-fixed (ECEF) Cartesian point.
        /// </summary>
        /// <returns></returns>
        public CartesianPoint ToCartesianPoint()
        {
            return ToCartesianPoint(Ellipsoid.Wgs1984, Distance.Empty);
        }

        public CartesianPoint ToCartesianPoint(Ellipsoid ellipsoid, Distance altitude)
        {
            //	% LLA2ECEF - convert latitude, longitude, and altitude to
            //	%            earth-centered, earth-fixed (ECEF) cartesian
            //	% 
            //	% USAGE:
            //	% [x,y,z] = lla2ecef(lat,lon,alt)
            //	% 
            //	% x = ECEF X-coordinate (m)
            //	% y = ECEF Y-coordinate (m)
            //	% z = ECEF Z-coordinate (m)
            //	% lat = geodetic latitude (radians)
            //	% lon = longitude (radians)
            //	% alt = height above WGS84 ellipsoid (m)
            //	% 
            //	% Notes: This function assumes the WGS84 model.
            //	%        Latitude is customary geodetic (not geocentric).
            //	% 
            //	% Source: "Department of Defense World Geodetic System 1984"
            //	%         Page 4-4
            //	%         National Imagery and Mapping Agency
            //	%         Last updated June, 2004
            //	%         NIMA TR8350.2
            //	% 
            //	% Michael Kleder, July 2005
            //
            //	function [x,y,z]=lla2ecef(lat,lon,alt)
            double lat = Latitude.ToRadians().Value;
            double lon = Longitude.ToRadians().Value;
            // Altitude is assumed at 100 meters
            double alt = altitude.ToMeters().Value;

            //
            //	% WGS84 ellipsoid constants:
            //	a = 6378137;
            double a = ellipsoid.EquatorialRadius.ToMeters().Value;
            //	e = 8.1819190842622e-2;
            double e = ellipsoid.Eccentricity;
            //
            //	% intermediate calculation
            //	% (prime vertical radius of curvature)
            //	N = a ./ sqrt(1 - e^2 .* sin(lat).^2);
            double N = a / Math.Sqrt(1.0 - Math.Pow(e, 2) * Math.Pow(Math.Sin(lat), 2));
            //
            //	% results:
            //	x = (N+alt) .* cos(lat) .* cos(lon);
            double x = (N + alt) * Math.Cos(lat) * Math.Cos(lon);
            //	y = (N+alt) .* cos(lat) .* sin(lon);
            double y = (N + alt) * Math.Cos(lat) * Math.Sin(lon);
            //	z = ((1-e^2) .* N + alt) .* sin(lat);
            double z = ((1.0 - Math.Pow(e, 2)) * N + alt) * Math.Sin(lat);
            //
            //	return

            return new CartesianPoint(
                new Distance(x, DistanceUnit.Meters),
                new Distance(y, DistanceUnit.Meters),
                new Distance(z, DistanceUnit.Meters));

        }

        public Position Normalize()
        {
            return new Position(_Latitude.Normalize(), _Longitude.Normalize());
        }

        /// <summary>
        /// Calculates the direction of travel to the specified destination.
        /// </summary>
        /// <param name="destination">A <strong>Position</strong> object to which the bearing is calculated.</param>
        /// <returns>An <strong>Azimuth</strong> object representing the calculated distance.</returns>
        public Azimuth BearingTo(Position destination)
        {
            return BearingTo(destination, Ellipsoid.Wgs1984);
        }

        /// <summary>
        /// Calculates the direction of travel to the specified destination using the specified interpretation of Earth's shape.
        /// </summary>
        /// <param name="destination">A <strong>Position</strong> object to which the bearing is calculated.</param>
        /// <param name="ellipsoid">An <strong>Ellipsoid</strong> object used to fine-tune bearing calculations.</param>
        /// <returns>An <strong>Azimuth</strong> object representing the calculated distance.</returns>
        public Azimuth BearingTo(Position destination, Ellipsoid ellipsoid)
        {
            // From: http://www.mathworks.com/matlabcentral/files/8607/vdist.m

            /*
             function varargout = vdist(lat1,lon1,lat2,lon2)
            % VDIST - Using the WGS-84 Earth ellipsoid, compute the distance between
            %         two points within a few millimeters of accuracy, compute forward
            %         azimuth, and compute backward azimuth, all using a vectorized
            %         version of Vincenty's algorithm.
            %
            % s = vdist(lat1,lon1,lat2,lon2)
            % [s,a12] = vdist(lat1,lon1,lat2,lon2)
            % [s,a12,a21] = vdist(lat1,lon1,lat2,lon2)
            %
            % s = distance in meters (inputs may be scalars, vectors, or matrices)
            % a12 = azimuth in degrees from first point to second point (forward)
            % a21 = azimuth in degrees from second point to first point (backward)
            %       (Azimuths are in degrees clockwise from north.)
            % lat1 = GEODETIC latitude of first point (degrees)
            % lon1 = longitude of first point (degrees)
            % lat2, lon2 = second point (degrees)
            %
            %  Original algorithm source:
            %  T. Vincenty, "Direct and Inverse Solutions of Geodesics on the Ellipsoid
            %  with Application of Nested Equations", Survey Review, vol. 23, no. 176,
            %  April 1975, pp 88-93.
            %  Available at: http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
            %
            % Notes: (1) lat1,lon1,lat2,lon2 can be any (identical) size/shape. Outputs
            %            will have the same size and shape.
            %        (2) Error correcting code, convergence failure traps, antipodal
            %            corrections, polar error corrections, WGS84 ellipsoid
            %            parameters, testing, and comments: Michael Kleder, 2004.
            %        (3) Azimuth implementation (including quadrant abiguity
            %            resolution) and code vectorization, Michael Kleder, Sep 2005.
            %        (4) Vectorization is convergence sensitive; that is, quantities
            %            which have already converged to within tolerance are not
            %            recomputed during subsequent iterations (while other
            %            quantities are still converging).
            %        (5) Vincenty describes his distance algorithm as precise to within
            %            0.01 millimeters, subject to the ellipsoidal model.
            %        (6) For distance calculations, essentially antipodal points are
            %            treated as exactly antipodal, potentially reducing accuracy
            %            slightly.
            %        (7) Distance failures for points exactly at the poles are
            %            eliminated by moving the points by 0.6 millimeters.
            %        (8) The Vincenty distance algorithm was transcribed verbatim by
            %            Peter Cederholm, August 12, 2003. It was modified and
            %            translated to English by Michael Kleder.
            %            Mr. Cederholm's website is http://www.plan.aau.dk/~pce/
            %        (9) Distances agree with the Mapping Toolbox, version 2.2 (R14SP3)
            %            with a max relative difference of about 5e-9, except when the
            %            two points are nearly antipodal, and except when one point is
            %            near the equator and the two longitudes are nearly 180 degrees
            %            apart. This function (vdist) is more accurate in such cases.
            %            For example, note this difference (as of this writing):
            %            >>vdist(0.2,305,15,125)
            %            18322827.0131551
            %            >>distance(0.2,305,15,125,[6378137 0.08181919])
            %            0
            %       (10) Azimuths FROM the north pole (either forward starting at the
            %            north pole or backward when ending at the north pole) are set
            %            to 180 degrees by convention. Azimuths FROM the south pole are
            %            set to 0 degrees by convention.
            %       (11) Azimuths agree with the Mapping Toolbox, version 2.2 (R14SP3)
            %            to within about a hundred-thousandth of a degree, except when
            %            traversing to or from a pole, where the convention for this
            %            function is described in (10), and except in the cases noted
            %            above in (9).
            %       (12) No warranties; use at your own risk.

            % reshape inputs
            keepsize = size(lat1);
            lat1=lat1(:);
            lon1=lon1(:);
            lat2=lat2(:);
            lon2=lon2(:);
            % Input check:
            if any(abs(lat1)>90 | abs(lat2)>90)
                error('Input latitudes must be between -90 and 90 degrees, inclusive.')
            end
            % Supply WGS84 earth ellipsoid axis lengths in meters:
            a = 6378137; % definitionally
            b = 6356752.31424518; % computed from WGS84 earth flattening coefficient
            % preserve true input latitudes:
            lat1tr = lat1;
            lat2tr = lat2;
            % convert inputs in degrees to radians:
            lat1 = lat1 * 0.0174532925199433;
            lon1 = lon1 * 0.0174532925199433;
            lat2 = lat2 * 0.0174532925199433;
            lon2 = lon2 * 0.0174532925199433;
            % correct for errors at exact poles by adjusting 0.6 millimeters:
            kidx = abs(pi/2-abs(lat1)) < 1e-10;
            if any(kidx);
                lat1(kidx) = sign(lat1(kidx))*(pi/2-(1e-10));
            end
            kidx = abs(pi/2-abs(lat2)) < 1e-10;
            if any(kidx)
                lat2(kidx) = sign(lat2(kidx))*(pi/2-(1e-10));
            end
            f = (a-b)/a;
            U1 = atan((1-f)*tan(lat1));
            U2 = atan((1-f)*tan(lat2));
            lon1 = mod(lon1,2*pi);
            lon2 = mod(lon2,2*pi);
            L = abs(lon2-lon1);
            kidx = L > pi;
            if any(kidx)
                L(kidx) = 2*pi - L(kidx);
            end
            lambda = L;
            lambdaold = 0*lat1;
            itercount = 0;
            notdone = logical(1+0*lat1);
            alpha = 0*lat1;
            sigma = 0*lat1;
            cos2sigmam = 0*lat1;
            C = 0*lat1;
            warninggiven = logical(0);
            while any(notdone)  % force at least one execution
                %disp(['lambda(21752) = ' num2str(lambda(21752),20)]);
                itercount = itercount+1;
                if itercount > 50
                    if ~warninggiven
                        warning(['Essentially antipodal points encountered. ' ...
                            'Precision may be reduced slightly.']);
                    end
                    lambda(notdone) = pi;
                    break
                end
                lambdaold(notdone) = lambda(notdone);
                sinsigma(notdone) = sqrt((cos(U2(notdone)).*sin(lambda(notdone)))...
                    .^2+(cos(U1(notdone)).*sin(U2(notdone))-sin(U1(notdone)).*...
                    cos(U2(notdone)).*cos(lambda(notdone))).^2);
                cossigma(notdone) = sin(U1(notdone)).*sin(U2(notdone))+...
                    cos(U1(notdone)).*cos(U2(notdone)).*cos(lambda(notdone));
                % eliminate rare imaginary portions at limit of numerical precision:
                sinsigma(notdone)=real(sinsigma(notdone));
                cossigma(notdone)=real(cossigma(notdone));
                sigma(notdone) = atan2(sinsigma(notdone),cossigma(notdone));
                alpha(notdone) = asin(cos(U1(notdone)).*cos(U2(notdone)).*...
                    sin(lambda(notdone))./sin(sigma(notdone)));
                cos2sigmam(notdone) = cos(sigma(notdone))-2*sin(U1(notdone)).*...
                    sin(U2(notdone))./cos(alpha(notdone)).^2;
                C(notdone) = f/16*cos(alpha(notdone)).^2.*(4+f*(4-3*...
                    cos(alpha(notdone)).^2));
                lambda(notdone) = L(notdone)+(1-C(notdone)).*f.*sin(alpha(notdone))...
                    .*(sigma(notdone)+C(notdone).*sin(sigma(notdone)).*...
                    (cos2sigmam(notdone)+C(notdone).*cos(sigma(notdone)).*...
                    (-1+2.*cos2sigmam(notdone).^2)));
                %disp(['then, lambda(21752) = ' num2str(lambda(21752),20)]);
                % correct for convergence failure in the case of essentially antipodal
                % points
                if any(lambda(notdone) > pi)
                    warning(['Essentially antipodal points encountered. ' ...
                        'Precision may be reduced slightly.']);
                    warninggiven = logical(1);
                    lambdaold(lambda>pi) = pi;
                    lambda(lambda>pi) = pi;
                end
                notdone = abs(lambda-lambdaold) > 1e-12;
            end
            u2 = cos(alpha).^2.*(a^2-b^2)/b^2;
            A = 1+u2./16384.*(4096+u2.*(-768+u2.*(320-175.*u2)));
            B = u2./1024.*(256+u2.*(-128+u2.*(74-47.*u2)));
            deltasigma = B.*sin(sigma).*(cos2sigmam+B./4.*(cos(sigma).*(-1+2.*...
                cos2sigmam.^2)-B./6.*cos2sigmam.*(-3+4.*sin(sigma).^2).*(-3+4*...
                cos2sigmam.^2)));
            varargout{1} = reshape(b.*A.*(sigma-deltasigma),keepsize);
            if nargout > 1
                % From point #1 to point #2
                % correct sign of lambda for azimuth calcs:
                lambda = abs(lambda);
                kidx=sign(sin(lon2-lon1)) .* sign(sin(lambda)) < 0;
                lambda(kidx) = -lambda(kidx);
                numer = cos(U2).*sin(lambda);
                denom = cos(U1).*sin(U2)-sin(U1).*cos(U2).*cos(lambda);
                a12 = atan2(numer,denom);
                kidx = a12<0;
                a12(kidx)=a12(kidx)+2*pi;
                % from poles:
                a12(lat1tr <= -90) = 0;
                a12(lat1tr >= 90 ) = pi;
                varargout{2} = reshape(a12 * 57.2957795130823,keepsize); % to degrees
            end
            if nargout > 2
                a21=NaN*lat1;
                % From point #2 to point #1
                % correct sign of lambda for azimuth calcs:
                lambda = abs(lambda);
                kidx=sign(sin(lon1-lon2)) .* sign(sin(lambda)) < 0;
                lambda(kidx)=-lambda(kidx);
                numer = cos(U1).*sin(lambda);
                denom = sin(U1).*cos(U2)-cos(U1).*sin(U2).*cos(lambda);
                a21 = atan2(numer,denom);
                kidx=a21<0;
                a21(kidx)= a21(kidx)+2*pi;
                % backwards from poles:
                a21(lat2tr >= 90) = pi;
                a21(lat2tr <= -90) = 0;
                varargout{3} = reshape(a21 * 57.2957795130823,keepsize); % to degrees
            end
            return

             * 
             * 
             */

            // If positions are equivalent, return zero
            if (Equals(destination))
                return Azimuth.Empty;

            #region Newer code

            double goodlambda = 0;
            double goodalpha = 0;
            double goodsigma = 0;
            double goodcos2sigmam = 0;

            //            % reshape inputs
            //keepsize = size(lat1);
            //lat1=lat1(:);
            //lon1=lon1(:);
            //lat2=lat2(:);
            //lon2=lon2(:);

            // ?

            //% Input check:
            //if any(abs(lat1)>90 | abs(lat2)>90)
            //    error('Input latitudes must be between -90 and 90 degrees, inclusive.')
            //end

            // The -90 to 90 check is handled by Normalize

            //% Supply WGS84 earth ellipsoid axis lengths in meters:
            //a = 6378137; % definitionally
            //b = 6356752.31424518; % computed from WGS84 earth flattening coefficient

            double a = ellipsoid.EquatorialRadiusMeters;
            double b = ellipsoid.PolarRadiusMeters;

            //% preserve true input latitudes:
            //lat1tr = lat1;
            //lat2tr = lat2;

            double lat1tr = _Latitude.DecimalDegrees;

            /* FxCop says that "lat2tr" is only assigned to, but never used.
             * 
             
            double lat2tr = destination.Latitude.DecimalDegrees;
             
             */

            //% convert inputs in degrees to radians:
            //lat1 = lat1 * 0.0174532925199433;
            //lon1 = lon1 * 0.0174532925199433;
            //lat2 = lat2 * 0.0174532925199433;
            //lon2 = lon2 * 0.0174532925199433;

            // Convert inputs into radians
            double lat1 = this.Latitude.Normalize().ToRadians().Value;
            double lon1 = this.Longitude.Normalize().ToRadians().Value;
            double lat2 = destination.Latitude.Normalize().ToRadians().Value;
            double lon2 = destination.Longitude.Normalize().ToRadians().Value;

            //% correct for errors at exact poles by adjusting 0.6 millimeters:
            //kidx = abs(pi/2-abs(lat1)) < 1e-10;
            //if any(kidx);
            //    lat1(kidx) = sign(lat1(kidx))*(pi/2-(1e-10));
            //end

            // Correct for errors at exact poles by adjusting 0.6mm
            if (Math.Abs(Math.PI * 0.5 - Math.Abs(lat1)) < 1E-10)
            {
                lat1 = Math.Sign(lat1) * (Math.PI * 0.5 - 1E-10);
            }

            //kidx = abs(pi/2-abs(lat2)) < 1e-10;
            //if any(kidx)
            //    lat2(kidx) = sign(lat2(kidx))*(pi/2-(1e-10));
            //end

            if (Math.Abs(Math.PI * 0.5 - Math.Abs(lat2)) < 1E-10)
            {
                lat2 = Math.Sign(lat2) * (Math.PI * 0.5 - 1E-10);
            }


            //f = (a-b)/a;

            double f = ellipsoid.Flattening;

            //U1 = atan((1-f)*tan(lat1));

            double U1 = Math.Atan((1 - f) * Math.Tan(lat1));

            //U2 = atan((1-f)*tan(lat2));

            double U2 = Math.Atan((1 - f) * Math.Tan(lat2));

            //lon1 = mod(lon1,2*pi);

            lon1 = lon1 % (2 * Math.PI);

            //lon2 = mod(lon2,2*pi);

            lon2 = lon2 % (2 * Math.PI);

            //L = abs(lon2-lon1);

            double L = Math.Abs(lon2 - lon1);

            //kidx = L > pi;
            //if any(kidx)
            //    L(kidx) = 2*pi - L(kidx);
            //end

            if (L > Math.PI)
            {
                L = 2.0 * Math.PI - L;
            }

            //lambda = L;

            double lambda = L;

            //lambdaold = 0*lat1;

            double lambdaold = 0;

            //itercount = 0;

            int itercount = 0;

            //notdone = logical(1+0*lat1);

            bool notdone = true;

            //alpha = 0*lat1;

            double alpha = 0;

            //sigma = 0*lat1;

            double sigma = 0;

            //cos2sigmam = 0*lat1;

            double cos2sigmam = 0;

            //C = 0*lat1;

            double C = 0;

            //warninggiven = logical(0);

            //bool warninggiven = false;

            //while any(notdone)  % force at least one execution

            while (notdone)
            {

                //    %disp(['lambda(21752) = ' num2str(lambda(21752),20)]);
                //    itercount = itercount+1;

                itercount++;

                //    if itercount > 50

                if (itercount > 50)
                {

                    //        if ~warninggiven

                    //if (!warninggiven)
                    //{
                    //    //            warning(['Essentially antipodal points encountered. ' ...
                    //    //                'Precision may be reduced slightly.']);

                    //    warninggiven = true;
                    //    throw new WarningException("Distance calculation accuracy may be reduced because the two endpoints are antipodal.");
                    //}

                    //        end
                    //        lambda(notdone) = pi;

                    lambda = Math.PI;

                    //        break

                    break;

                    //    end

                }

                //    lambdaold(notdone) = lambda(notdone);

                lambdaold = lambda;

                //    sinsigma(notdone) = sqrt((cos(U2(notdone)).*sin(lambda(notdone)))...
                //        .^2+(cos(U1(notdone)).*sin(U2(notdone))-sin(U1(notdone)).*...
                //        cos(U2(notdone)).*cos(lambda(notdone))).^2);

                double sinsigma = Math.Sqrt(Math.Pow((Math.Cos(U2) * Math.Sin(lambda))
                        , 2) + Math.Pow((Math.Cos(U1) * Math.Sin(U2) - Math.Sin(U1) *
                        Math.Cos(U2) * Math.Cos(lambda)), 2));

                //    cossigma(notdone) = sin(U1(notdone)).*sin(U2(notdone))+...
                //        cos(U1(notdone)).*cos(U2(notdone)).*cos(lambda(notdone));

                double cossigma = Math.Sin(U1) * Math.Sin(U2) +
                    Math.Cos(U1) * Math.Cos(U2) * Math.Cos(lambda);

                //    % eliminate rare imaginary portions at limit of numerical precision:
                //    sinsigma(notdone)=real(sinsigma(notdone));
                //    cossigma(notdone)=real(cossigma(notdone));

                // Eliminate rare imaginary portions at limit of numerical precision:
                // ?

                //    sigma(notdone) = atan2(sinsigma(notdone),cossigma(notdone));

                sigma = Math.Atan2(sinsigma, cossigma);

                //    alpha(notdone) = asin(cos(U1(notdone)).*cos(U2(notdone)).*...
                //        sin(lambda(notdone))./sin(sigma(notdone)));

                alpha = Math.Asin(Math.Cos(U1) * Math.Cos(U2) *
                    Math.Sin(lambda) / Math.Sin(sigma));

                //    cos2sigmam(notdone) = cos(sigma(notdone))-2*sin(U1(notdone)).*...
                //        sin(U2(notdone))./cos(alpha(notdone)).^2;

                cos2sigmam = Math.Cos(sigma) - 2.0 * Math.Sin(U1) *
                    Math.Sin(U2) / Math.Pow(Math.Cos(alpha), 2);

                //    C(notdone) = f/16*cos(alpha(notdone)).^2.*(4+f*(4-3*...
                //        cos(alpha(notdone)).^2));

                C = f / 16 * Math.Pow(Math.Cos(alpha), 2) * (4 + f * (4 - 3 *
                    Math.Pow(Math.Cos(alpha), 2)));

                //    lambda(notdone) = L(notdone)+(1-C(notdone)).*f.*sin(alpha(notdone))...
                //        .*(sigma(notdone)+C(notdone).*sin(sigma(notdone)).*...
                //        (cos2sigmam(notdone)+C(notdone).*cos(sigma(notdone)).*...
                //        (-1+2.*cos2sigmam(notdone).^2)));

                lambda = L + (1 - C) * f * Math.Sin(alpha)
                            * (sigma + C * Math.Sin(sigma) *
                            (cos2sigmam + C * Math.Cos(sigma) *
                            (-1 + 2 * Math.Pow(cos2sigmam, 2))));

                //    %disp(['then, lambda(21752) = ' num2str(lambda(21752),20)]);
                //    % correct for convergence failure in the case of essentially antipodal
                //    % points

                // Correct for convergence failure in the case of essentially antipodal points

                //    if any(lambda(notdone) > pi)

                if (lambda > Math.PI)
                {

                    //        if ~warninggiven

                    //if (!warninggiven)
                    //{
                    //    //            warning(['Essentially antipodal points encountered. ' ...
                    //    //                'Precision may be reduced slightly.']);

                    //    warninggiven = true;
                    //    throw new WarningException("Distance calculation accuracy may be reduced because the two endpoints are antipodal.");
                    //}

                    //        end

                    //        lambdaold(lambda>pi) = pi;

                    lambdaold = Math.PI;

                    //        lambda(lambda>pi) = pi;

                    lambda = Math.PI;

                    //    end

                }

                //    notdone = abs(lambda-lambdaold) > 1e-12;

                notdone = Math.Abs(lambda - lambdaold) > TargetAccuracy;

                //end

                // NOTE: In some cases "alpha" would return a "NaN".  If values are healthy, 
                // remember them so we get a good distance calc.
                if (!double.IsNaN(alpha))
                {
                    goodlambda = lambda;
                    goodalpha = alpha;
                    goodsigma = sigma;
                    goodcos2sigmam = cos2sigmam;
                }
            }


            //u2 = cos(alpha).^2.*(a^2-b^2)/b^2;

            double u2 = Math.Pow(Math.Cos(goodalpha), 2) * (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2);

            //A = 1+u2./16384.*(4096+u2.*(-768+u2.*(320-175.*u2)));

            double A = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));

            //B = u2./1024.*(256+u2.*(-128+u2.*(74-47.*u2)));

            double B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));

            //deltasigma = B.*sin(sigma).*(cos2sigmam+B./4.*(cos(sigma).*(-1+2.*...
            //    cos2sigmam.^2)-B./6.*cos2sigmam.*(-3+4.*sin(sigma).^2).*(-3+4*...
            //    cos2sigmam.^2)));

            double deltasigma = B * Math.Sin(goodsigma) * (goodcos2sigmam + B / 4 * (Math.Cos(goodsigma) * (-1 + 2 *
                Math.Pow(goodcos2sigmam, 2)) - B / 6 * goodcos2sigmam * (-3 + 4 * Math.Pow(Math.Sin(goodsigma), 2)) * (-3 + 4 *
                Math.Pow(goodcos2sigmam, 2))));

            //varargout{1} = reshape(b.*A.*(sigma-deltasigma),keepsize);

            /* FxCop says that this variable "double s" is only assigned to, but never used.
             * 

            double s = b * A * (goodsigma - deltasigma);
             
             */

            // Return the Distance in meters
            //return new Distance(s, DistanceUnit.Meters).ToLocalUnitType();

            //if nargout > 1
            //    % From point #1 to point #2
            //    % correct sign of lambda for azimuth calcs:

            //    lambda = abs(lambda);

            goodlambda = Math.Abs(goodlambda);

            //    kidx=sign(sin(lon2-lon1)) .* sign(sin(lambda)) < 0;

            bool kidx = Math.Sign(Math.Sin(lon2 - lon1)) * Math.Sign(Math.Sin(goodlambda)) < 0;

            //    lambda(kidx) = -lambda(kidx);

            if (kidx)
                goodlambda = -goodlambda;

            //    numer = cos(U2).*sin(lambda);

            double numer = Math.Cos(U2) * Math.Sin(goodlambda);

            //    denom = cos(U1).*sin(U2)-sin(U1).*cos(U2).*cos(lambda);

            double denom = Math.Cos(U1) * Math.Sin(U2) - Math.Sin(U1) * Math.Cos(U2) * Math.Cos(goodlambda);

            //    a12 = atan2(numer,denom);

            double a12 = Math.Atan2(numer, denom);

            //    kidx = a12<0;

            kidx = a12 < 0;

            //    a12(kidx)=a12(kidx)+2*pi;

            if (kidx)
                a12 = a12 + 2 * Math.PI;

            //    % from poles:
            //    a12(lat1tr <= -90) = 0;

            if (lat1tr <= -90.0)
                a12 = 0;

            //    a12(lat1tr >= 90 ) = pi;

            if (lat1tr >= 90)
                a12 = Math.PI;

            //    varargout{2} = reshape(a12 * 57.2957795130823,keepsize); % to degrees

            // Convert to degrees
            return Azimuth.FromRadians(a12);

            //end
            //if nargout > 2
            //    a21=NaN*lat1;
            //    % From point #2 to point #1
            //    % correct sign of lambda for azimuth calcs:
            //    lambda = abs(lambda);
            //    kidx=sign(sin(lon1-lon2)) .* sign(sin(lambda)) < 0;
            //    lambda(kidx)=-lambda(kidx);
            //    numer = cos(U1).*sin(lambda);
            //    denom = sin(U1).*cos(U2)-cos(U1).*sin(U2).*cos(lambda);
            //    a21 = atan2(numer,denom);
            //    kidx=a21<0;
            //    a21(kidx)= a21(kidx)+2*pi;
            //    % backwards from poles:
            //    a21(lat2tr >= 90) = pi;
            //    a21(lat2tr <= -90) = 0;
            //    varargout{3} = reshape(a21 * 57.2957795130823,keepsize); % to degrees
            //end
            //return

            #endregion


            #region Unused Code (Commented Out)

            /*
            double lonrad = pLongitude.ToRadians().Value;
			double latrad = pLatitude.ToRadians().Value;
			double destlonrad = destination.Longitude.ToRadians().Value;
			double destlatrad = destination.Latitude.ToRadians().Value;
			
			double y = Math.Sin(lonrad - destlonrad) * Math.Cos(destlatrad);
			double x = Math.Cos(latrad) * Math.Sin(destlatrad) 
				     - Math.Sin(latrad) * Math.Cos(destlatrad) * Math.Cos(lonrad - destlonrad);

			double rad = Math.Atan2(-y, x);
			return Azimuth.FromRadians(rad).Normalize();
             */

            //			try
            ////			{
            //				//Dim AdjustedDestination As Position = destination.ToEllipsoid(Ellipsoid.Type)
            //
            //				double y = -Math.Sin(Longitude.ToRadians().Value - destination.Longitude.ToRadians().Value) 
            //					* Math.Cos(destination.Latitude.ToRadians().Value);
            //				double x = Math.Cos(Latitude.ToRadians().Value) * Math.Sin(destination.Latitude.ToRadians().Value) 
            //					- Math.Sin(Latitude.ToRadians().Value) * Math.Cos(destination.Latitude.ToRadians().Value) 
            //					* Math.Cos(Longitude.ToRadians().Value - destination.Longitude.ToRadians().Value);
            //
            //			//Console.WriteLine(String.Format("X: {0}, Y: {1}
            //
            ////			atan2( -sin(long1-long2).cos(lat2),
            ////cos(lat1).sin(lat2) - sin(lat1).cos(lat2).cos(long1-long2) ) 
            //
            //
            //				return new Azimuth(((Math.Atan2(y, x) * 180.0 / Math.PI) + 360) % 360); //+ 1 / 7200.0)
            //			}
            //			catch
            //			{
            //				throw new GpsException("Error while calculating initial bearing.");
            //			}
            // Test Data
            //
            // Name: Denver Oklahoma City 
            // Latitude: 39 ° 45 ' 0.00000 '' 35 ° 26 ' 0.00000 '' 
            // Longitude: 105 ° 0 ' 0.00000 '' 97 ° 28 ' 0.00000 '' 
            // Forward Azimuth:  236 ° 35 ' 21.15 ''  
            // Reverse Azimuth:  51 ° 59 ' 10.32 '' 
            // Datumal Distance:  819373.914 meters  

            //			'' Converted from JavaScript: http://www.movable-type.co.uk/scripts/LatLong.html
            //			''
            //			''	LatLong.bearing = function(p1, p2) {
            //			''  var y = Math.sin(p1.long-p2.long) * Math.cos(p2.lat);
            //			''  var x = Math.cos(p1.lat)*Math.sin(p2.lat) -
            //			''		  Math.sin(p1.lat)*Math.cos(p2.lat)*Math.cos(p1.long-p2.long);
            //			''  return(Math.atan2(-y, x)); // -y 'cos Williams treats W as +ve!
            //			'Try
            //			'	Dim AdjustedDestination As Position = destination				' destination.ToDatum(Datum.Type)

            //			'	Dim StartLatRad As Double = Latitude.ToRadians().Value
            //			'	Dim StartLonRad As Double = Longitude.ToRadians().Value
            //			'	Dim DestLatRad As Double = AdjustedDestination.Latitude.ToRadians().Value
            //			'	Dim DestLonRad As Double = AdjustedDestination.Longitude.ToRadians().Value

            //			'	Dim y As Double = -Math.Sin(StartLonRad - DestLonRad) * Math.Cos(DestLatRad)
            //			'	Dim x As Double = Math.Cos(StartLatRad) * Math.Sin(DestLatRad) _
            //			'		 - Math.Sin(StartLatRad) * Math.Cos(DestLatRad) * Math.Cos(StartLonRad - DestLonRad)
            //			'	Dim NewBearingRads As New Radian(Math.Atan2(y, x))
            //			'	'tc1=mod(atan2(sin(lon1-lon2)*cos(lat2),
            //			'	'		   cos(lat1)*sin(lat2)-sin(lat1)*cos(lat2)*cos(lon1-lon2)), 2*pi)

            //			'	Dim NewBearing As Double = (NewBearingRads.ToAngle.DecimalDegrees + 360) Mod 360				 '* 180.0 / Math.PI '+ 360) Mod 360 + 1 / 7200.0

            //			'	Return New Azimuth(NewBearing)				' Azimuth(double.Parse()
            //			'Catch ex As Exception
            //			'	Throw New GeoException("Error while calculating initial bearing.", ex)
            //			'End Try
            //			'' Test Data
            //			''
            //			'' Name: Denver Oklahoma City 
            //			'' Latitude: 39 ° 45 ' 0.00000 '' 35 ° 26 ' 0.00000 '' 
            //			'' Longitude: 105 ° 0 ' 0.00000 '' 97 ° 28 ' 0.00000 '' 
            //			'' Forward Azimuth:  236 ° 35 ' 21.15 ''  
            //			'' Reverse Azimuth:  51 ° 59 ' 10.32 '' 
            //		   '' Datumal Distance:  819373.914 meters  
            #endregion
        }

        // Returns the speed (as the crow flies) required to reach the given destination in the given time
        /// <summary>
        /// Returns the minimum speed required to travel from the current location to the
        /// specified destination within the specified period of time.
        /// </summary>
        public Speed SpeedTo(Position destination, TimeSpan time)
        {
            try
            {
                // Make sure the position is in the same datum as we are
                //Dim AdjustedDestination As Position = destination.ToDatum(Datum)
                // Now calculate the distance
                double TravelDistance = DistanceTo(destination).ToMeters().Value;
                // Perform the calculation
                return new Speed(TravelDistance / time.TotalSeconds, SpeedUnit.MetersPerSecond);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Indicates if the current instance is North of the specified position.
        /// </summary>
        /// <param name="value">A <strong>Position</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more North than the specified instance.</returns>
        public bool IsNorthOf(Position value)
        {
            return Latitude.IsGreaterThan(value.Latitude);
        }

        /// <summary>
        /// Indicates if the current instance is South of the specified position.
        /// </summary>
        /// <param name="value">A <strong>Position</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more South than the specified instance.</returns>
        public bool IsSouthOf(Position value)
        {
            return Latitude.IsLessThan(value.Latitude);
        }

        /// <summary>
        /// Indicates if the current instance is East of the specified position.
        /// </summary>
        /// <param name="value">A <strong>Position</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more East than the specified instance.</returns>
        public bool IsEastOf(Position value)
        {
            return Longitude.IsGreaterThan(value.Longitude);
        }

        /// <summary>
        /// Indicates if the current instance is West of the specified position.
        /// </summary>
        /// <param name="value">A <strong>Position</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more West than the specified instance.</returns>
        public bool IsWestOf(Position value)
        {
            return Longitude.IsLessThan(value.Longitude);
        }

        /// <summary>
        /// Returns the minimum time required to travel to the given destination at the
        /// specified constant speed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The TimeTo method expects a value for Speed greater than zero.</exception>
        public TimeSpan TimeTo(Position destination, Speed speed)
        {
            if (speed.Value <= 0)
                throw new ArgumentOutOfRangeException("The TimeTo method expects a value for Speed greater than zero.");

            // Perform the calculation
            return TimeSpan.FromSeconds(DistanceTo(destination).ToMeters().Value
                / speed.ToMetersPerSecond().Value);
        }

        /// <overloads>Calculates the great circle distance between any two points on 
        /// Earth.</overloads>
        /// <summary>
        /// Returns the distance over land from the given starting point to the specified
        /// destination.
        /// </summary>
        /// <returns>
        /// A <strong>Distance</strong> object containing the calculated distance in
        /// kilometers.
        /// </returns>
        /// <param name="destination">The ending point of a segment.</param>
        /// <remarks>
        /// This method uses trigonometry to calculate the Great Circle (over Earth's curved
        /// surface) distance between any two points on Earth. The distance is returned in
        /// kilometers but can be converted to any other unit type using methods in the
        /// <see cref="Distance">Distance</see>
        /// class.
        /// </remarks>
        public Distance DistanceTo(Position destination)
        {
            return DistanceTo(destination, Ellipsoid.Wgs1984);
        }

        /// <overloads>Calculates the great circle distance between any two points on 
        /// Earth.</overloads>
        /// <summary>
        /// Returns the distance over land from the given starting point to the specified
        /// destination.
        /// </summary>
        /// <returns>
        /// A <strong>Distance</strong> object containing the calculated distance in
        /// kilometers.
        /// </returns>
        /// <param name="destination">The ending point of a segment.</param>
        /// <param name="isApproximated"></param>
        /// <remarks>
        /// This method uses a high-speed formula to determine the Great Circle distance from one
        /// point to another.  This method is typically used in situations where hundreds of distance
        /// measurements must be made in a short period of time.  The <strong>DistanceTo</strong> method
        /// produces accuracy to one millimeter, but its formula is about a hundred times slower than this 
        /// method.  
        /// <see cref="Distance">Distance</see>
        /// class.
        /// </remarks>
        public Distance DistanceTo(Position destination, bool isApproximated)
        {
            return DistanceTo(destination, Ellipsoid.Wgs1984, isApproximated);
        }

        /// <overloads>Calculates the great circle distance between any two points on 
        /// Earth.</overloads>
        /// <summary>
        /// Returns the distance over land from the given starting point to the specified
        /// destination.
        /// </summary>
        /// <returns>
        /// A <strong>Distance</strong> object containing the calculated distance in
        /// kilometers.
        /// </returns>
        /// <param name="destination">The ending point of a segment.</param>
        /// <param name="ellipsoid">The model of the Earth to use for the distance calculation.</param>
        /// <param name="isApproximated"></param>
        /// <remarks>
        /// This method uses a high-speed formula to determine the Great Circle distance from one
        /// point to another.  This method is typically used in situations where hundreds of distance
        /// measurements must be made in a short period of time.  The <strong>DistanceTo</strong> method
        /// produces accuracy to one millimeter, but its formula is about a hundred times slower than this 
        /// method.  
        /// <see cref="Distance">Distance</see>
        /// class.
        /// </remarks>
        public Distance DistanceTo(Position destination, Ellipsoid ellipsoid, bool isApproximated)
        {
            //// Make sure the destination isn't null
            //if (destination == null)
            //    throw new ArgumentNullException("destination", "The Position.DistanceTo method requires a non-null destination parameter.");

            // If they want the high-speed formula, use it
            if (!isApproximated)
                return DistanceTo(destination);

            // The ellipsoid cannot be null
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The Position.DistanceTo method requires a non-null ellipsoid parameter.");

            //Dim AdjustedDestination As Position = destination.ToDatum(Datum)
            // USING THE FORMULA FROM: 
            //$lat1 = deg2rad(28.5333);
            double lat1 = Latitude.ToRadians().Value;
            //$lat2 = deg2rad(31.1000);
            double lat2 = destination.Latitude.ToRadians().Value;
            //$long1 = deg2rad(-81.3667);
            double long1 = Longitude.ToRadians().Value;
            //$long2 = deg2rad(121.3667);
            double long2 = destination.Longitude.ToRadians().Value;
            //$dlat = abs($lat2 - $lat1);
            double dlat = Math.Abs(lat2 - lat1);
            //$dlong = abs($long2 - $long1);
            double dlong = Math.Abs(long2 - long1);
            //$l = ($lat1 + $lat2) / 2;
            double l = (lat1 + lat2) * 0.5;
            //$a = 6378;
            double a = ellipsoid.EquatorialRadius.ToKilometers().Value;
            //$b = 6357;
            double b = ellipsoid.PolarRadius.ToKilometers().Value;
            //$e = sqrt(1 - ($b * $b)/($a * $a));
            double e = Math.Sqrt(1 - (b * b) / (a * a));
            //$r1 = ($a * (1 - ($e * $e))) / pow((1 - ($e * $e) * (sin($l) * sin($l))), 3/2);
            double r1 = (a * (1 - (e * e))) / Math.Pow((1 - (e * e) * (Math.Sin(l) * Math.Sin(l))), 3 * 0.5);
            //$r2 = $a / sqrt(1 - ($e * $e) * (sin($l) * sin($l)));
            double r2 = a / Math.Sqrt(1 - (e * e) * (Math.Sin(l) * Math.Sin(l)));
            //$ravg = ($r1 * ($dlat / ($dlat + $dlong))) + ($r2 * ($dlong / ($dlat + $dlong)));
            double ravg = (r1 * (dlat / (dlat + dlong))) + (r2 * (dlong / (dlat + dlong)));
            //$sinlat = sin($dlat / 2);
            double sinlat = Math.Sin(dlat * 0.5);
            //$sinlon = sin($dlong / 2);
            double sinlon = Math.Sin(dlong * 0.5);
            //$a = pow($sinlat, 2) + cos($lat1) * cos($lat2) * pow($sinlon, 2);
            a = Math.Pow(sinlat, 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(sinlon, 2);
            //$c = 2 * asin(min(1, sqrt($a)));
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            //$d = $ravg * $c; 
            double d = ravg * c;
            // If it's NaN, return zero
            if (double.IsNaN(d))
            {
                d = 0.0;
            }
            // Return a new distance
            return new Distance(d, DistanceUnit.Kilometers).ToLocalUnitType();
        }

        /// <overloads>Calculates the great circle distance between any two points on 
        /// Earth using a specific model of Earth's shape.</overloads>
        /// <summary>
        /// Returns the distance over land from the given starting point to the specified
        /// destination.
        /// </summary>
        /// <returns>
        /// A <strong>Distance</strong> object containing the calculated distance in
        /// kilometers.
        /// </returns>
        /// <param name="destination">The ending point of a segment.</param>
        /// <param name="ellipsoid">The model of the Earth to use for the distance calculation.</param>
        /// <remarks>
        /// This method uses trigonometry to calculate the Great Circle (over Earth's curved
        /// surface) distance between any two points on Earth. The distance is returned in
        /// kilometers but can be converted to any other unit type using methods in the
        /// <see cref="Distance">Distance</see>
        /// class.
        /// </remarks>
        public Distance DistanceTo(Position destination, Ellipsoid ellipsoid)
        {
            // From: http://www.mathworks.com/matlabcentral/files/8607/vdist.m

            /*
             function varargout = vdist(lat1,lon1,lat2,lon2)
% VDIST - Using the WGS-84 Earth ellipsoid, compute the distance between
%         two points within a few millimeters of accuracy, compute forward
%         azimuth, and compute backward azimuth, all using a vectorized
%         version of Vincenty's algorithm.
%
% s = vdist(lat1,lon1,lat2,lon2)
% [s,a12] = vdist(lat1,lon1,lat2,lon2)
% [s,a12,a21] = vdist(lat1,lon1,lat2,lon2)
%
% s = distance in meters (inputs may be scalars, vectors, or matrices)
% a12 = azimuth in degrees from first point to second point (forward)
% a21 = azimuth in degrees from second point to first point (backward)
%       (Azimuths are in degrees clockwise from north.)
% lat1 = GEODETIC latitude of first point (degrees)
% lon1 = longitude of first point (degrees)
% lat2, lon2 = second point (degrees)
%
%  Original algorithm source:
%  T. Vincenty, "Direct and Inverse Solutions of Geodesics on the Ellipsoid
%  with Application of Nested Equations", Survey Review, vol. 23, no. 176,
%  April 1975, pp 88-93.
%  Available at: http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
%
% Notes: (1) lat1,lon1,lat2,lon2 can be any (identical) size/shape. Outputs
%            will have the same size and shape.
%        (2) Error correcting code, convergence failure traps, antipodal
%            corrections, polar error corrections, WGS84 ellipsoid
%            parameters, testing, and comments: Michael Kleder, 2004.
%        (3) Azimuth implementation (including quadrant abiguity
%            resolution) and code vectorization, Michael Kleder, Sep 2005.
%        (4) Vectorization is convergence sensitive; that is, quantities
%            which have already converged to within tolerance are not
%            recomputed during subsequent iterations (while other
%            quantities are still converging).
%        (5) Vincenty describes his distance algorithm as precise to within
%            0.01 millimeters, subject to the ellipsoidal model.
%        (6) For distance calculations, essentially antipodal points are
%            treated as exactly antipodal, potentially reducing accuracy
%            slightly.
%        (7) Distance failures for points exactly at the poles are
%            eliminated by moving the points by 0.6 millimeters.
%        (8) The Vincenty distance algorithm was transcribed verbatim by
%            Peter Cederholm, August 12, 2003. It was modified and
%            translated to English by Michael Kleder.
%            Mr. Cederholm's website is http://www.plan.aau.dk/~pce/
%        (9) Distances agree with the Mapping Toolbox, version 2.2 (R14SP3)
%            with a max relative difference of about 5e-9, except when the
%            two points are nearly antipodal, and except when one point is
%            near the equator and the two longitudes are nearly 180 degrees
%            apart. This function (vdist) is more accurate in such cases.
%            For example, note this difference (as of this writing):
%            >>vdist(0.2,305,15,125)
%            18322827.0131551
%            >>distance(0.2,305,15,125,[6378137 0.08181919])
%            0
%       (10) Azimuths FROM the north pole (either forward starting at the
%            north pole or backward when ending at the north pole) are set
%            to 180 degrees by convention. Azimuths FROM the south pole are
%            set to 0 degrees by convention.
%       (11) Azimuths agree with the Mapping Toolbox, version 2.2 (R14SP3)
%            to within about a hundred-thousandth of a degree, except when
%            traversing to or from a pole, where the convention for this
%            function is described in (10), and except in the cases noted
%            above in (9).
%       (12) No warranties; use at your own risk.

% reshape inputs
keepsize = size(lat1);
lat1=lat1(:);
lon1=lon1(:);
lat2=lat2(:);
lon2=lon2(:);
% Input check:
if any(abs(lat1)>90 | abs(lat2)>90)
    error('Input latitudes must be between -90 and 90 degrees, inclusive.')
end
% Supply WGS84 earth ellipsoid axis lengths in meters:
a = 6378137; % definitionally
b = 6356752.31424518; % computed from WGS84 earth flattening coefficient
% preserve true input latitudes:
lat1tr = lat1;
lat2tr = lat2;
% convert inputs in degrees to radians:
lat1 = lat1 * 0.0174532925199433;
lon1 = lon1 * 0.0174532925199433;
lat2 = lat2 * 0.0174532925199433;
lon2 = lon2 * 0.0174532925199433;
% correct for errors at exact poles by adjusting 0.6 millimeters:
kidx = abs(pi/2-abs(lat1)) < 1e-10;
if any(kidx);
    lat1(kidx) = sign(lat1(kidx))*(pi/2-(1e-10));
end
kidx = abs(pi/2-abs(lat2)) < 1e-10;
if any(kidx)
    lat2(kidx) = sign(lat2(kidx))*(pi/2-(1e-10));
end
f = (a-b)/a;
U1 = atan((1-f)*tan(lat1));
U2 = atan((1-f)*tan(lat2));
lon1 = mod(lon1,2*pi);
lon2 = mod(lon2,2*pi);
L = abs(lon2-lon1);
kidx = L > pi;
if any(kidx)
    L(kidx) = 2*pi - L(kidx);
end
lambda = L;
lambdaold = 0*lat1;
itercount = 0;
notdone = logical(1+0*lat1);
alpha = 0*lat1;
sigma = 0*lat1;
cos2sigmam = 0*lat1;
C = 0*lat1;
warninggiven = logical(0);
while any(notdone)  % force at least one execution
    %disp(['lambda(21752) = ' num2str(lambda(21752),20)]);
    itercount = itercount+1;
    if itercount > 50
        if ~warninggiven
            warning(['Essentially antipodal points encountered. ' ...
                'Precision may be reduced slightly.']);
        end
        lambda(notdone) = pi;
        break
    end
    lambdaold(notdone) = lambda(notdone);
    sinsigma(notdone) = sqrt((cos(U2(notdone)).*sin(lambda(notdone)))...
        .^2+(cos(U1(notdone)).*sin(U2(notdone))-sin(U1(notdone)).*...
        cos(U2(notdone)).*cos(lambda(notdone))).^2);
    cossigma(notdone) = sin(U1(notdone)).*sin(U2(notdone))+...
        cos(U1(notdone)).*cos(U2(notdone)).*cos(lambda(notdone));
    % eliminate rare imaginary portions at limit of numerical precision:
    sinsigma(notdone)=real(sinsigma(notdone));
    cossigma(notdone)=real(cossigma(notdone));
    sigma(notdone) = atan2(sinsigma(notdone),cossigma(notdone));
    alpha(notdone) = asin(cos(U1(notdone)).*cos(U2(notdone)).*...
        sin(lambda(notdone))./sin(sigma(notdone)));
    cos2sigmam(notdone) = cos(sigma(notdone))-2*sin(U1(notdone)).*...
        sin(U2(notdone))./cos(alpha(notdone)).^2;
    C(notdone) = f/16*cos(alpha(notdone)).^2.*(4+f*(4-3*...
        cos(alpha(notdone)).^2));
    lambda(notdone) = L(notdone)+(1-C(notdone)).*f.*sin(alpha(notdone))...
        .*(sigma(notdone)+C(notdone).*sin(sigma(notdone)).*...
        (cos2sigmam(notdone)+C(notdone).*cos(sigma(notdone)).*...
        (-1+2.*cos2sigmam(notdone).^2)));
    %disp(['then, lambda(21752) = ' num2str(lambda(21752),20)]);
    % correct for convergence failure in the case of essentially antipodal
    % points
    if any(lambda(notdone) > pi)
        warning(['Essentially antipodal points encountered. ' ...
            'Precision may be reduced slightly.']);
        warninggiven = logical(1);
        lambdaold(lambda>pi) = pi;
        lambda(lambda>pi) = pi;
    end
    notdone = abs(lambda-lambdaold) > 1e-12;
end
u2 = cos(alpha).^2.*(a^2-b^2)/b^2;
A = 1+u2./16384.*(4096+u2.*(-768+u2.*(320-175.*u2)));
B = u2./1024.*(256+u2.*(-128+u2.*(74-47.*u2)));
deltasigma = B.*sin(sigma).*(cos2sigmam+B./4.*(cos(sigma).*(-1+2.*...
    cos2sigmam.^2)-B./6.*cos2sigmam.*(-3+4.*sin(sigma).^2).*(-3+4*...
    cos2sigmam.^2)));
varargout{1} = reshape(b.*A.*(sigma-deltasigma),keepsize);
if nargout > 1
    % From point #1 to point #2
    % correct sign of lambda for azimuth calcs:
    lambda = abs(lambda);
    kidx=sign(sin(lon2-lon1)) .* sign(sin(lambda)) < 0;
    lambda(kidx) = -lambda(kidx);
    numer = cos(U2).*sin(lambda);
    denom = cos(U1).*sin(U2)-sin(U1).*cos(U2).*cos(lambda);
    a12 = atan2(numer,denom);
    kidx = a12<0;
    a12(kidx)=a12(kidx)+2*pi;
    % from poles:
    a12(lat1tr <= -90) = 0;
    a12(lat1tr >= 90 ) = pi;
    varargout{2} = reshape(a12 * 57.2957795130823,keepsize); % to degrees
end
if nargout > 2
    a21=NaN*lat1;
    % From point #2 to point #1
    % correct sign of lambda for azimuth calcs:
    lambda = abs(lambda);
    kidx=sign(sin(lon1-lon2)) .* sign(sin(lambda)) < 0;
    lambda(kidx)=-lambda(kidx);
    numer = cos(U1).*sin(lambda);
    denom = sin(U1).*cos(U2)-cos(U1).*sin(U2).*cos(lambda);
    a21 = atan2(numer,denom);
    kidx=a21<0;
    a21(kidx)= a21(kidx)+2*pi;
    % backwards from poles:
    a21(lat2tr >= 90) = pi;
    a21(lat2tr <= -90) = 0;
    varargout{3} = reshape(a21 * 57.2957795130823,keepsize); % to degrees
end
return

             * 
             * 
             */

            // If positions are equivalent, return zero
            if (Equals(destination))
                return Distance.Empty;

            #region Newer code

            double goodalpha = 0;
            double goodsigma = 0;
            double goodcos2sigmam = 0;

            //            % reshape inputs
            //keepsize = size(lat1);
            //lat1=lat1(:);
            //lon1=lon1(:);
            //lat2=lat2(:);
            //lon2=lon2(:);

            // ?

            //% Input check:
            //if any(abs(lat1)>90 | abs(lat2)>90)
            //    error('Input latitudes must be between -90 and 90 degrees, inclusive.')
            //end

            // The -90 to 90 check is handled by Normalize

            //% Supply WGS84 earth ellipsoid axis lengths in meters:
            //a = 6378137; % definitionally
            //b = 6356752.31424518; % computed from WGS84 earth flattening coefficient

            double a = ellipsoid.EquatorialRadiusMeters;
            double b = ellipsoid.PolarRadiusMeters;

            //% preserve true input latitudes:
            //lat1tr = lat1;
            //lat2tr = lat2;

            //double lat1tr = pLatitude.DecimalDegrees;
            //double lat2tr = destination.Latitude.DecimalDegrees;

            //% convert inputs in degrees to radians:
            //lat1 = lat1 * 0.0174532925199433;
            //lon1 = lon1 * 0.0174532925199433;
            //lat2 = lat2 * 0.0174532925199433;
            //lon2 = lon2 * 0.0174532925199433;

            // Convert inputs into radians
            double lat1 = this.Latitude.Normalize().ToRadians().Value;
            double lon1 = this.Longitude.Normalize().ToRadians().Value;
            double lat2 = destination.Latitude.Normalize().ToRadians().Value;
            double lon2 = destination.Longitude.Normalize().ToRadians().Value;

            //% correct for errors at exact poles by adjusting 0.6 millimeters:
            //kidx = abs(pi/2-abs(lat1)) < 1e-10;
            //if any(kidx);
            //    lat1(kidx) = sign(lat1(kidx))*(pi/2-(1e-10));
            //end

            // Correct for errors at exact poles by adjusting 0.6mm
            if (Math.Abs(Math.PI * 0.5 - Math.Abs(lat1)) < 1E-10)
            {
                lat1 = Math.Sign(lat1) * (Math.PI * 0.5 - 1E-10);
            }

            //kidx = abs(pi/2-abs(lat2)) < 1e-10;
            //if any(kidx)
            //    lat2(kidx) = sign(lat2(kidx))*(pi/2-(1e-10));
            //end

            if (Math.Abs(Math.PI * 0.5 - Math.Abs(lat2)) < 1E-10)
            {
                lat2 = Math.Sign(lat2) * (Math.PI * 0.5 - 1E-10);
            }


            //f = (a-b)/a;

            double f = ellipsoid.Flattening;

            //U1 = atan((1-f)*tan(lat1));

            double U1 = Math.Atan((1 - f) * Math.Tan(lat1));

            //U2 = atan((1-f)*tan(lat2));

            double U2 = Math.Atan((1 - f) * Math.Tan(lat2));

            //lon1 = mod(lon1,2*pi);

            lon1 = lon1 % (2 * Math.PI);

            //lon2 = mod(lon2,2*pi);

            lon2 = lon2 % (2 * Math.PI);

            //L = abs(lon2-lon1);

            double L = Math.Abs(lon2 - lon1);

            //kidx = L > pi;
            //if any(kidx)
            //    L(kidx) = 2*pi - L(kidx);
            //end

            if (L > Math.PI)
            {
                L = 2.0 * Math.PI - L;
            }

            //lambda = L;

            double lambda = L;

            //lambdaold = 0*lat1;

            double lambdaold = 0;

            //itercount = 0;

            int itercount = 0;

            //notdone = logical(1+0*lat1);

            bool notdone = true;

            //alpha = 0*lat1;

            double alpha = 0;

            //sigma = 0*lat1;

            double sigma = 0;

            //cos2sigmam = 0*lat1;

            double cos2sigmam = 0;

            //C = 0*lat1;

            double C = 0;

            //warninggiven = logical(0);

            //bool warninggiven = false;

            //while any(notdone)  % force at least one execution

            while (notdone)
            {

                //    %disp(['lambda(21752) = ' num2str(lambda(21752),20)]);
                //    itercount = itercount+1;

                itercount++;

                //    if itercount > 50

                if (itercount > 50)
                {

                    //        if ~warninggiven

                    //if (!warninggiven)
                    //{
                    //    //            warning(['Essentially antipodal points encountered. ' ...
                    //    //                'Precision may be reduced slightly.']);

                    //    warninggiven = true;
                    //    throw new WarningException("Distance calculation accuracy may be reduced because the two endpoints are antipodal.");
                    //}

                    //        end
                    //        lambda(notdone) = pi;

                    lambda = Math.PI;

                    //        break

                    break;

                    //    end

                }

                //    lambdaold(notdone) = lambda(notdone);

                lambdaold = lambda;

                //    sinsigma(notdone) = sqrt((cos(U2(notdone)).*sin(lambda(notdone)))...
                //        .^2+(cos(U1(notdone)).*sin(U2(notdone))-sin(U1(notdone)).*...
                //        cos(U2(notdone)).*cos(lambda(notdone))).^2);

                double sinsigma = Math.Sqrt(Math.Pow((Math.Cos(U2) * Math.Sin(lambda))
                        , 2) + Math.Pow((Math.Cos(U1) * Math.Sin(U2) - Math.Sin(U1) *
                        Math.Cos(U2) * Math.Cos(lambda)), 2));

                //    cossigma(notdone) = sin(U1(notdone)).*sin(U2(notdone))+...
                //        cos(U1(notdone)).*cos(U2(notdone)).*cos(lambda(notdone));

                double cossigma = Math.Sin(U1) * Math.Sin(U2) +
                    Math.Cos(U1) * Math.Cos(U2) * Math.Cos(lambda);

                //    % eliminate rare imaginary portions at limit of numerical precision:
                //    sinsigma(notdone)=real(sinsigma(notdone));
                //    cossigma(notdone)=real(cossigma(notdone));

                // Eliminate rare imaginary portions at limit of numerical precision:
                // ?

                //    sigma(notdone) = atan2(sinsigma(notdone),cossigma(notdone));

                sigma = Math.Atan2(sinsigma, cossigma);

                //    alpha(notdone) = asin(cos(U1(notdone)).*cos(U2(notdone)).*...
                //        sin(lambda(notdone))./sin(sigma(notdone)));

                alpha = Math.Asin(Math.Cos(U1) * Math.Cos(U2) *
                    Math.Sin(lambda) / Math.Sin(sigma));

                //    cos2sigmam(notdone) = cos(sigma(notdone))-2*sin(U1(notdone)).*...
                //        sin(U2(notdone))./cos(alpha(notdone)).^2;

                cos2sigmam = Math.Cos(sigma) - 2.0 * Math.Sin(U1) *
                    Math.Sin(U2) / Math.Pow(Math.Cos(alpha), 2);

                //    C(notdone) = f/16*cos(alpha(notdone)).^2.*(4+f*(4-3*...
                //        cos(alpha(notdone)).^2));

                C = f / 16 * Math.Pow(Math.Cos(alpha), 2) * (4 + f * (4 - 3 *
                    Math.Pow(Math.Cos(alpha), 2)));

                //    lambda(notdone) = L(notdone)+(1-C(notdone)).*f.*sin(alpha(notdone))...
                //        .*(sigma(notdone)+C(notdone).*sin(sigma(notdone)).*...
                //        (cos2sigmam(notdone)+C(notdone).*cos(sigma(notdone)).*...
                //        (-1+2.*cos2sigmam(notdone).^2)));

                lambda = L + (1 - C) * f * Math.Sin(alpha)
                            * (sigma + C * Math.Sin(sigma) *
                            (cos2sigmam + C * Math.Cos(sigma) *
                            (-1 + 2 * Math.Pow(cos2sigmam, 2))));

                //    %disp(['then, lambda(21752) = ' num2str(lambda(21752),20)]);
                //    % correct for convergence failure in the case of essentially antipodal
                //    % points

                // Correct for convergence failure in the case of essentially antipodal points

                //    if any(lambda(notdone) > pi)

                if (lambda > Math.PI)
                {

                    //        if ~warninggiven

                    //if (!warninggiven)
                    //{
                    //    //            warning(['Essentially antipodal points encountered. ' ...
                    //    //                'Precision may be reduced slightly.']);

                    //    warninggiven = true;
                    //    throw new WarningException("Distance calculation accuracy may be reduced because the two endpoints are antipodal.");
                    //}

                    //        end

                    //        lambdaold(lambda>pi) = pi;

                    lambdaold = Math.PI;

                    //        lambda(lambda>pi) = pi;

                    lambda = Math.PI;

                    //    end

                }

                //    notdone = abs(lambda-lambdaold) > 1e-12;

                notdone = Math.Abs(lambda - lambdaold) > TargetAccuracy;

                //end

                // NOTE: In some cases "alpha" would return a "NaN".  If values are healthy, 
                // remember them so we get a good distance calc.
                if (!double.IsNaN(alpha))
                {
                    goodalpha = alpha;
                    goodsigma = sigma;
                    goodcos2sigmam = cos2sigmam;
                }

                // Allow other threads some breathing room
                Thread.Sleep(0);
            }


            //u2 = cos(alpha).^2.*(a^2-b^2)/b^2;

            double u2 = Math.Pow(Math.Cos(goodalpha), 2) * (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2);

            //A = 1+u2./16384.*(4096+u2.*(-768+u2.*(320-175.*u2)));

            double A = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));

            //B = u2./1024.*(256+u2.*(-128+u2.*(74-47.*u2)));

            double B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));

            //deltasigma = B.*sin(sigma).*(cos2sigmam+B./4.*(cos(sigma).*(-1+2.*...
            //    cos2sigmam.^2)-B./6.*cos2sigmam.*(-3+4.*sin(sigma).^2).*(-3+4*...
            //    cos2sigmam.^2)));

            double deltasigma = B * Math.Sin(goodsigma) * (goodcos2sigmam + B / 4 * (Math.Cos(goodsigma) * (-1 + 2 *
                Math.Pow(goodcos2sigmam, 2)) - B / 6 * goodcos2sigmam * (-3 + 4 * Math.Pow(Math.Sin(goodsigma), 2)) * (-3 + 4 *
                Math.Pow(goodcos2sigmam, 2))));

            //varargout{1} = reshape(b.*A.*(sigma-deltasigma),keepsize);
            double s = b * A * (goodsigma - deltasigma);

            // Return the Distance in meters
            return new Distance(s, DistanceUnit.Meters).ToLocalUnitType();

            //if nargout > 1
            //    % From point #1 to point #2
            //    % correct sign of lambda for azimuth calcs:
            //    lambda = abs(lambda);
            //    kidx=sign(sin(lon2-lon1)) .* sign(sin(lambda)) < 0;
            //    lambda(kidx) = -lambda(kidx);
            //    numer = cos(U2).*sin(lambda);
            //    denom = cos(U1).*sin(U2)-sin(U1).*cos(U2).*cos(lambda);
            //    a12 = atan2(numer,denom);
            //    kidx = a12<0;
            //    a12(kidx)=a12(kidx)+2*pi;
            //    % from poles:
            //    a12(lat1tr <= -90) = 0;
            //    a12(lat1tr >= 90 ) = pi;
            //    varargout{2} = reshape(a12 * 57.2957795130823,keepsize); % to degrees
            //end
            //if nargout > 2
            //    a21=NaN*lat1;
            //    % From point #2 to point #1
            //    % correct sign of lambda for azimuth calcs:
            //    lambda = abs(lambda);
            //    kidx=sign(sin(lon1-lon2)) .* sign(sin(lambda)) < 0;
            //    lambda(kidx)=-lambda(kidx);
            //    numer = cos(U1).*sin(lambda);
            //    denom = sin(U1).*cos(U2)-cos(U1).*sin(U2).*cos(lambda);
            //    a21 = atan2(numer,denom);
            //    kidx=a21<0;
            //    a21(kidx)= a21(kidx)+2*pi;
            //    % backwards from poles:
            //    a21(lat2tr >= 90) = pi;
            //    a21(lat2tr <= -90) = 0;
            //    varargout{3} = reshape(a21 * 57.2957795130823,keepsize); % to degrees
            //end
            //return

            #endregion


            #region Unused Code (Commented Out)

            /*
            //Dim AdjustedDestination As Position = destination.ToDatum(Datum)
            // USING THE FORMULA FROM: 
            //$lat1 = deg2rad(28.5333);
            double lat1 = Latitude.ToRadians().Value;
            //$lat2 = deg2rad(31.1000);
            double lat2 = destination.Latitude.ToRadians().Value;
            //$long1 = deg2rad(-81.3667);
            double long1 = Longitude.ToRadians().Value;
            //$long2 = deg2rad(121.3667);
            double long2 = destination.Longitude.ToRadians().Value;
            //$dlat = abs($lat2 - $lat1);
            double dlat = Math.Abs(lat2 - lat1);
            //$dlong = abs($long2 - $long1);
            double dlong = Math.Abs(long2 - long1);
            //$l = ($lat1 + $lat2) / 2;
            double l = (lat1 + lat2) * 0.5;
            //$a = 6378;
            double a = ellipsoid.EquatorialRadius.ToKilometers().Value;
            //$b = 6357;
            double b = ellipsoid.PolarRadius.ToKilometers().Value;
            //$e = sqrt(1 - ($b * $b)/($a * $a));
            double e = Math.Sqrt(1 - (b * b) / (a * a));
            //$r1 = ($a * (1 - ($e * $e))) / pow((1 - ($e * $e) * (sin($l) * sin($l))), 3/2);
            double r1 = (a * (1 - (e * e))) / Math.Pow((1 - (e * e) * (Math.Sin(l) * Math.Sin(l))), 3 * 0.5);
            //$r2 = $a / sqrt(1 - ($e * $e) * (sin($l) * sin($l)));
            double r2 = a / Math.Sqrt(1 - (e * e) * (Math.Sin(l) * Math.Sin(l)));
            //$ravg = ($r1 * ($dlat / ($dlat + $dlong))) + ($r2 * ($dlong / ($dlat + $dlong)));
            double ravg = (r1 * (dlat / (dlat + dlong))) + (r2 * (dlong / (dlat + dlong)));
            //$sinlat = sin($dlat / 2);
            double sinlat = Math.Sin(dlat * 0.5);
            //$sinlon = sin($dlong / 2);
            double sinlon = Math.Sin(dlong * 0.5);
            //$a = pow($sinlat, 2) + cos($lat1) * cos($lat2) * pow($sinlon, 2);
            a = Math.Pow(sinlat, 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(sinlon, 2);
            //$c = 2 * asin(min(1, sqrt($a)));
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            //$d = $ravg * $c; 
            double d = ravg * c;
            // If it's NaN, return zero
            if (double.IsNaN(d))
            {
                d = 0.0;
            }
            // Return a new distance
            return new Distance(d, DistanceUnit.Kilometers).ToLocalUnitType();
             **/

            #endregion
        }

        /// <summary>
        /// Returns the remaining travel distance if traveling for a certain speed for a certain period of time.
        /// </summary>
        /// <param name="destination">A <strong>Position</strong> marking the destination location.</param>
        /// <param name="speed">A <strong>Speed</strong> travelled from the current instance.</param>
        /// <param name="time">A <strong>TimeSpan</strong> representing the time already elapsed during transit to the destination.</param>
        /// <returns>A <strong>Distance</strong> measuring the remaining distance to travel.</returns>
        public Distance DistanceTo(Position destination, Speed speed, TimeSpan time)
        {
            double TraveledDistance = speed.ToMetersPerSecond().Value * time.TotalSeconds;
            // Get the total distance (without travelling)
            double TotalDistance = DistanceTo(destination).ToMeters().Value;
            // Return the distance travelled
            return new Distance(TotalDistance - TraveledDistance, DistanceUnit.Meters).ToLocalUnitType();
        }

        /// <summary>
        /// Calculates the intersection of two lines created by the current instance, another point, and a direction of travel from each point.
        /// </summary>
        /// <param name="firstBearing">An <strong>Angle</strong> specifying a travel direction from the current instance.</param>
        /// <param name="secondPosition">A <strong>Position</strong> specifying the start of the second line of intersection.</param>
        /// <param name="secondBearing">An <strong>Angle</strong> specifying a travel direction from the second position.</param>
        /// <returns>A <strong>Position</strong> representing the point of intersection, if one exists.</returns>
        /// <remarks>This method is typically used to determine the point where two objects in motion would meet.  </remarks>
        public Position IntersectionOf(Azimuth firstBearing, Position secondPosition, Azimuth secondBearing)
        {
            return IntersectionOf(firstBearing.DecimalDegrees, secondPosition, secondBearing.DecimalDegrees);
        }

        public Position IntersectionOf(Angle firstBearing, Position secondPosition, Angle secondBearing)
        {
            return IntersectionOf(firstBearing.DecimalDegrees, secondPosition, secondBearing.DecimalDegrees);
        }

        public Position IntersectionOf(double firstBearing, Position secondPosition, double secondBearing)
        {
            //Dim AdjustedDestination As Position = secondPosition.ToDatum(Datum)

            double tol = 0.000000000000001;
            //			  // in1 - line 1, point 1 latitude index
            //  // in2 - line 1, point 1 longitude index
            //  // in3 - line 1, bearing index
            //  // in4 - line 2, point 1 latitude index 
            //  // in5 - line 2, point 1 longitude index
            //  // in6 - line 2, bearing index
            //  // in7 - intersection point latitude index
            //  // in8 - intersection point longitude index

            //var j = form.units.selectedIndex;
            //var units = form.units.options[j].value;

            //double latddd1 = Latitude.DecimalDegrees;
            double latrad1 = _Latitude.ToRadians().Value;
            //double londdd1 = Longitude.DecimalDegrees;
            double lonrad1 = _Longitude.ToRadians().Value;

            //double latddd2 = secondPosition.Latitude.DecimalDegrees;
            double latrad2 = secondPosition.Latitude.ToRadians().Value;
            //double londdd2 = secondPosition.Longitude.DecimalDegrees;
            double lonrad2 = secondPosition.Longitude.ToRadians().Value;

            double crs13 = Radian.FromDegrees(firstBearing).Value;
            double crs23 = Radian.FromDegrees(secondBearing).Value;

            //  var latddd1 = dms2ddd(form.degreevalue[in1].value, form.minutevalue[in1].value, form.secondvalue[in1].value);  // convert latitude of point to decimal degrees then radians
            //  var latrad1 = deg2rad(latddd1);
            //  var londdd1 = dms2ddd(form.degreevalue[in2].value, form.minutevalue[in2].value, form.secondvalue[in2].value);  // likewise for longitude
            //  var lonrad1 = deg2rad(londdd1);
            //  var latddd2 = dms2ddd(form.degreevalue[in4].value, form.minutevalue[in4].value, form.secondvalue[in4].value);  // convert latitude of point to decimal degrees then radians
            //  var latrad2 = deg2rad(latddd2);
            //  var londdd2 = dms2ddd(form.degreevalue[in5].value, form.minutevalue[in5].value, form.secondvalue[in5].value);  // likewise for longitude
            //  var lonrad2 = deg2rad(londdd2);

            //  var crs13 = dms2ddd(form.degreevalue[in3].value, form.minutevalue[in3].value, form.secondvalue[in3].value);  //convert bearing line 1 to decimal degrees
            //  crs13 = deg2rad(crs13);			 //convert to radians
            //  var crs23 = dms2ddd(form.degreevalue[in6].value, form.minutevalue[in6].value, form.secondvalue[in6].value);  //convert bearing line 2 to decimal degrees
            //  crs23 = deg2rad(crs23);			 //convert to radians

            double w = lonrad2 - lonrad1;
            double v = latrad1 - latrad2;
            double s = 2 * Math.Asin(Math.Sqrt((Math.Sin(v * 0.5) * Math.Sin(v * 0.5)) + (Math.Cos(latrad1) * Math.Cos(latrad2) * Math.Sin(w * 0.5) * Math.Sin(w * 0.5)))); // //distance between start points

            //  var w = lonrad2 - lonrad1; 
            //  var v = latrad1 - latrad2;
            //  var s = 2 * Math.asin(Math.sqrt((Math.sin(v * 0.5) * Math.sin(v * 0.5)) + (Math.cos(latrad1) * Math.cos(latrad2) * Math.sin(w * 0.5) * Math.sin(w * 0.5))));	 //distance between start points

            double crs12 = 0;
            if (Math.Sin(lonrad1 - lonrad2) < 0)
            {
                crs12 = Math.Acos((Math.Sin(latrad2) - Math.Sin(latrad1) * Math.Cos(s)) / (Math.Sin(s) * Math.Cos(latrad1)));
            }
            else
            {
                crs12 = 2 * Math.PI - Math.Acos((Math.Sin(latrad2) - Math.Sin(latrad1) * Math.Cos(s)) / (Math.Sin(s) * Math.Cos(latrad1)));
            }

            //// calculate course 1 to 2
            //  if (Math.sin(lonrad1 - lonrad2) < 0){
            //	  var crs12 = Math.acos((Math.sin(latrad2) - Math.sin(latrad1) * Math.cos(s)) / (Math.sin(s) * Math.cos(latrad1)));
            //	}
            //   else {
            //	  var crs12 = 2 * pi - Math.acos((Math.sin(latrad2) - Math.sin(latrad1) * Math.cos(s)) / (Math.sin(s) * Math.cos(latrad1)));
            //	}

            double crs21 = 0;
            if (Math.Sin(lonrad2 - lonrad1) < 0)
            {
                crs21 = Math.Acos((Math.Sin(latrad1) - Math.Sin(latrad2) * Math.Cos(s)) / (Math.Sin(s) * Math.Cos(latrad2)));
            }
            else
            {
                crs21 = 2 * Math.PI - Math.Acos((Math.Sin(latrad1) - Math.Sin(latrad2) * Math.Cos(s)) / (Math.Sin(s) * Math.Cos(latrad2)));
            }

            //// calculate course 2 to 1
            //   if (Math.sin(lonrad2 - lonrad1) < 0){
            //	  var crs21 = Math.acos((Math.sin(latrad1) - Math.sin(latrad2) * Math.cos(s)) / (Math.sin(s) * Math.cos(latrad2)));
            //	}
            //   else {
            //	  var crs21 = 2 * pi - Math.acos((Math.sin(latrad1) - Math.sin(latrad2) * Math.cos(s)) / (Math.sin(s) * Math.cos(latrad2)));
            //	}

            double ang1 = (crs13 - crs12 + Math.PI) % (2 * Math.PI) - Math.PI;
            double ang2 = (crs21 - crs23 + Math.PI) % (2 * Math.PI) - Math.PI;

            // var ang1 = mod(crs13 - crs12 + pi, 2 * pi) - pi;
            // var ang2 = mod(crs21 - crs23 + pi, 2 * pi) - pi;

            if (Math.Sin(ang1) * Math.Sin(ang2) <= Math.Sqrt(tol))
            {
                // NO EXCEPTION IS THROWN.  RETURN NULL
                return Position.Empty;
                //throw new GeoException("No intersection exists between these two points and the given bearings.");
            }
            else
            {
                ang1 = Math.Abs(ang1);
                ang2 = Math.Abs(ang2);
                double ang3 = Math.Acos(Math.Sin(ang1) * Math.Sin(ang2) * Math.Cos(s) - Math.Cos(ang1) * Math.Cos(ang2));
                double dst13 = Math.Asin(Math.Sin(ang2) * Math.Sin(s) / Math.Sin(ang3));
                double latrad3 = Math.Asin(Math.Sin(latrad1) * Math.Cos(dst13) + Math.Cos(latrad1) * Math.Sin(dst13) * Math.Cos(crs13));
                double lonrad3 = lonrad1 + Math.Asin(Math.Sin(crs13) * Math.Sin(dst13) / Math.Cos(latrad3));
                lonrad3 = ((lonrad3 + Math.PI) % (2 * Math.PI)) - Math.PI;

                Latitude NewLatitude = new Latitude(latrad3 * 180 / Math.PI);
                Longitude NewLongitude = new Longitude(lonrad3 * 180 / Math.PI);
                // Return the new position
                return new Position(NewLatitude, NewLongitude);
            }

            // if (Math.sin(ang1) * Math.sin(ang2) <= Math.sqrt(tol)) {
            //		  alert('No Intersection Exists');
            //		  return true;
            //	 }
            //else {
            //var	ang1 = Math.abs(ang1);
            //var	ang2 = Math.abs(ang2);
            //var	ang3 = Math.acos(Math.sin(ang1) * Math.sin(ang2) * Math.cos(s) - Math.cos(ang1) * Math.cos(ang2));
            //var	dst13 = Math.asin(Math.sin(ang2) * Math.sin(s) / Math.sin(ang3));
            //var	latrad3 = Math.asin(Math.sin(latrad1) * Math.cos(dst13) + Math.cos(latrad1) * Math.sin(dst13) * Math.cos(crs13));
            //var	lonrad3 = lonrad1 + Math.asin(Math.sin(crs13) * Math.sin(dst13) / Math.cos(latrad3));
            //	   lonrad3 = mod(lonrad3 + pi, 2 * pi) - pi;
            //  ddd2dms(form, in8, rad2deg(lonrad3));
            //  ddd2dms(form, in7, rad2deg(latrad3));
            //}
        }

        /// <summary>
        /// Calculates a position relative to the current instance based upon the given bearing and distance.
        /// </summary>
        /// <param name="bearing">An <strong>Angle</strong> object specifying a direction to shift.</param>
        /// <param name="distance">A <strong>Distance</strong> object specifying the distance to shift.</param>
        /// <returns></returns>
        public Position TranslateTo(Angle bearing, Distance distance)
        {
            return TranslateTo(bearing.DecimalDegrees, distance, Ellipsoid.Wgs1984);
        }

        public Position TranslateTo(Angle bearing, Distance distance, Ellipsoid ellipsoid)
        {
            return TranslateTo(bearing.DecimalDegrees, distance, ellipsoid);
        }

        /// <summary>
        /// Calculates a position relative to the current instance based upon the given bearing and distance.
        /// </summary>
        /// <param name="bearing">An <strong>Azimuth</strong> object specifying a direction to shift.</param>
        /// <param name="distance">A <strong>Distance</strong> object specifying the distance to shift.</param>
        /// <returns>A <strong>Position</strong> representing the calculated position.</returns>
        /// <remarks>This function is designed to calculate positions for any location on Earth, with
        /// the exception of coordinates which lie at the poles (e.g. 90°N or 90°S).</remarks>
        public Position TranslateTo(Azimuth bearing, Distance distance)
        {
            return TranslateTo(bearing.DecimalDegrees, distance, Ellipsoid.Wgs1984);
        }

        /// <summary>
        /// Calculates a position relative to the current instance based upon the given bearing and distance.
        /// </summary>
        /// <param name="bearing">An <strong>Azimuth</strong> object specifying a direction to shift.</param>
        /// <param name="distance">A <strong>Distance</strong> object specifying the distance to shift.</param>
        /// <param name="ellipsoid">The model of the Earth to use for the translation calculation.</param>
        /// <returns>A <strong>Position</strong> representing the calculated position.</returns>
        /// <remarks>This function is designed to calculate positions for any location on Earth, with
        /// the exception of coordinates which lie at the poles (e.g. 90°N or 90°S).</remarks>
        public Position TranslateTo(Azimuth bearing, Distance distance, Ellipsoid ellipsoid)
        {
            return TranslateTo(bearing.DecimalDegrees, distance, ellipsoid);
        }

        /// <summary>
        /// Calculates a position relative to the current instance based upon the given bearing and distance.
        /// </summary>
        /// <param name="bearing">A <strong>Double</strong> specifying a direction to shift.</param>
        /// <param name="distance">A <strong>Distance</strong> object specifying the distance to shift.</param>
        /// <returns>A <strong>Position</strong> representing the calculated position.</returns>
        /// <remarks>This function is designed to calculate positions for any location on Earth, with
        /// the exception of coordinates which lie at the poles (e.g. 90°N or 90°S).</remarks>
        public Position TranslateTo(double bearing, Distance distance)
        {
            return TranslateTo(bearing, distance, Ellipsoid.Wgs1984);
        }

        /// <summary>
        /// Calculates a position relative to the current instance based upon the given bearing and distance.
        /// </summary>
        /// <param name="bearing">A <strong>Double</strong> specifying a direction to shift.</param>
        /// <param name="distance">A <strong>Distance</strong> object specifying the distance to shift.</param>
        /// <param name="ellipsoid">The model of the Earth to use for the translation calculation.</param>
        /// <returns>A <strong>Position</strong> representing the calculated position.</returns>
        /// <remarks>This function is designed to calculate positions for any location on Earth, with
        /// the exception of coordinates which lie at the poles (e.g. 90°N or 90°S).</remarks>
        public Position TranslateTo(double bearing, Distance distance, Ellipsoid ellipsoid)
        {
            // Taken from: http://www.koders.com/java/fid72A4E7D3DA8195D118CF926431263DDB8C45C5A1.aspx
            /*
             * Vincenty's Inverse Algorythm.
             * 
             * NOTE: MATLAB had no formula which took the ellipsoid into accout -- only a
             * formula which used Earth's average radius.  This is why the formulae have
             * two separate sources.  -- Jon
             * 
             *      
             *  
             * // Flattening
                    f  = (semiMajorAxis-semiMinorAxis) / semiMajorAxis;
             * 
             * // 1 - flattening
                    fo = 1.0 - f;
             * 
             * // Flattening squared
                    f2 = f*f;
             * // Flattening cubed
                    f3 = f*f2;
             * // Flattening ^ 4
                    f4 = f*f3;
                    eccentricitySquared = f * (2.0-f);

             *          * Solution of the geodetic direct problem after T.Vincenty.
                     * Modified Rainsford's method with Helmert's elliptical terms.
                     * Effective in any azimuth and at any distance short of antipodal.
                     *
                     * Latitudes and longitudes in radians positive North and East.
                     * Forward azimuths at both points returned in radians from North.
                     *
                     * Programmed for CDC-6600 by LCDR L.Pfeifer NGS ROCKVILLE MD 18FEB75
                     * Modified for IBM SYSTEM 360 by John G.Gergen NGS ROCKVILLE MD 7507
                     * Ported from Fortran to Java by Daniele Franzoni.
                     *
                     * Source: ftp://ftp.ngs.noaa.gov/pub/pcsoft/for_inv.3d/source/forward.for
                     *         subroutine DIRECT1
            // Protect internal variables from changes
                    final double lat1     = this.lat1;
                    final double long1    = this.long1;
                    final double azimuth  = this.azimuth;
                    final double distance = this.distance;
                    double TU  = fo*Math.sin(lat1) / Math.cos(lat1);
                    double SF  = Math.sin(azimuth);
                    double CF  = Math.cos(azimuth);
                    double BAZ = (CF!=0) ? Math.atan2(TU,CF)*2.0 : 0;
                    double CU  = 1/Math.sqrt(TU*TU + 1.0);
                    double SU  = TU*CU;
                    double SA  = CU*SF;
                    double C2A = 1.0 - SA*SA;
                    double X   = Math.sqrt((1.0/fo/fo-1)*C2A+1.0) + 1.0;
                    X   = (X-2.0)/X;
                    double C   = 1.0-X;
                    C   = (X*X/4.0+1.0)/C;
                    double D   = (0.375*X*X-1.0)*X;
                    TU   = distance / fo / semiMajorAxis / C;
                    double Y   = TU;
                    double SY, CY, CZ, E;
                    do {
                        SY = Math.sin(Y);
                        CY = Math.cos(Y);
                        CZ = Math.cos(BAZ+Y);
                        E  = CZ*CZ*2.0-1.0;
                        C  = Y;
                        X  = E*CY;
                        Y  = E+E-1.0;
                        Y  = (((SY*SY*4.0-3.0)*Y*CZ*D/6.0+X)*D/4.0-CZ)*SY*D+TU;
                    } while (Math.abs(Y-C) > TOLERANCE_1);
                    BAZ  = CU*CY*CF - SU*SY;
                    C    = fo*Math.sqrt(SA*SA+BAZ*BAZ);
                    D    = SU*CY + CU*SY*CF;
                    lat2 = Math.atan2(D,C);
                    C    = CU*CY-SU*SY*CF;
                    X    = Math.atan2(SY*SF,C);
                    C    = ((-3.0*C2A+4.0)*f+4.0)*C2A*f/16.0;
                    D    = ((E*CY*C+CZ)*SY*C+Y)*SA;
                    long2 = long1+X - (1.0-C)*D*f;
                    long2 = castToAngleRange(long2);
                    destinationValid = true;

             */

            double f = ellipsoid.Flattening;
            double fo = 1.0 - f;
            double semiMajorAxis = ellipsoid.SemiMajorAxisMeters;

            //final double lat1     = this.lat1;
            double lat1 = _Latitude.ToRadians().Value;

            //final double long1    = this.long1;
            double long1 = _Longitude.ToRadians().Value;

            //final double azimuth  = this.azimuth;
            double azimuth = bearing / 180.0 * Math.PI;

            //final double distance = this.distance;
            double dist = distance.ToMeters().Value;

            //double TU  = fo*Math.sin(lat1) / Math.cos(lat1);
            double TU = fo * Math.Sin(lat1) / Math.Cos(lat1);

            //double SF  = Math.sin(azimuth);
            double SF = Math.Sin(azimuth);

            //double CF  = Math.cos(azimuth);
            double CF = Math.Cos(azimuth);

            //double BAZ = (CF!=0) ? Math.atan2(TU,CF)*2.0 : 0;
            double BAZ = (CF != 0) ? Math.Atan2(TU, CF) * 2.0 : 0;

            //double CU  = 1/Math.sqrt(TU*TU + 1.0);
            double CU = 1.0 / Math.Sqrt(TU * TU + 1.0);

            //double SU  = TU*CU;
            double SU = TU * CU;

            //double SA  = CU*SF;
            double SA = CU * SF;

            //double C2A = 1.0 - SA*SA;
            double C2A = 1.0 - SA * SA;

            //double X   = Math.sqrt((1.0/fo/fo-1)*C2A+1.0) + 1.0;
            double X = Math.Sqrt((1.0 / fo / fo - 1) * C2A + 1.0) + 1.0;

            //X   = (X-2.0)/X;
            X = (X - 2.0) / X;

            //double C   = 1.0-X;
            double C = 1.0 - X;

            //C   = (X*X/4.0+1.0)/C;
            C = (X * X / 4.0 + 1.0) / C;

            //double D   = (0.375*X*X-1.0)*X;
            double D = (0.375 * X * X - 1.0) * X;

            //TU   = distance / fo / semiMajorAxis / C;
            TU = dist / fo / semiMajorAxis / C;

            //double Y   = TU;
            double Y = TU;

            //double SY, CY, CZ, E;
            double SY, CY, CZ, E;
            int Iterations = 0;

            //do {
            do
            {
                //    SY = Math.sin(Y);
                SY = Math.Sin(Y);
                //    CY = Math.cos(Y);
                CY = Math.Cos(Y);
                //    CZ = Math.cos(BAZ+Y);
                CZ = Math.Cos(BAZ + Y);
                //    E  = CZ*CZ*2.0-1.0;
                E = CZ * CZ * 2.0 - 1.0;
                //    C  = Y;
                C = Y;
                //    X  = E*CY;
                X = E * CY;
                //    Y  = E+E-1.0;
                Y = E + E - 1.0;
                //    Y  = (((SY*SY*4.0-3.0)*Y*CZ*D/6.0+X)*D/4.0-CZ)*SY*D+TU;
                Y = (((SY * SY * 4.0 - 3.0) * Y * CZ * D / 6.0 + X) * D / 4.0 - CZ) * SY * D + TU;
                //} while (Math.abs(Y-C) > TOLERANCE_1);
                Iterations++;

            } while (Iterations < 30 && Math.Abs(Y - C) > TargetAccuracy);

            //BAZ  = CU*CY*CF - SU*SY;
            BAZ = CU * CY * CF - SU * SY;

            //C    = fo*Math.sqrt(SA*SA+BAZ*BAZ);
            C = fo * Math.Sqrt(SA * SA + BAZ * BAZ);

            //D    = SU*CY + CU*SY*CF;
            D = SU * CY + CU * SY * CF;

            //lat2 = Math.atan2(D,C);
            double lat2 = Math.Atan2(D, C);

            //C    = CU*CY-SU*SY*CF;
            C = CU * CY - SU * SY * CF;

            //X    = Math.atan2(SY*SF,C);
            X = Math.Atan2(SY * SF, C);

            //C    = ((-3.0*C2A+4.0)*f+4.0)*C2A*f/16.0;
            C = ((-3.0 * C2A + 4.0) * f + 4.0) * C2A * f / 16.0;

            //D    = ((E*CY*C+CZ)*SY*C+Y)*SA;
            D = ((E * CY * C + CZ) * SY * C + Y) * SA;

            //long2 = long1+X - (1.0-C)*D*f;
            double long2 = long1 + X - (1.0 - C) * D * f;

            //long2 = castToAngleRange(long2);
            long2 = long2 - (2 * Math.PI) * Math.Floor(long2 / (2 * Math.PI) + 0.5);

            //destinationValid = true;

            // Return a new position, rounded to 10 digits of accuracy

            return new Position(
                Latitude.FromRadians(lat2).Round(10),
                Longitude.FromRadians(long2).Round(10));
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is Position)
                return Equals((Position)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return _Latitude.GetHashCode() ^ _Longitude.GetHashCode();
        }

        /// <summary>
        /// Outputs the current instance as a string using the default format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a random location using the specified random number seed.
        /// </summary>
        /// <returns></returns>
        public static Position Random()
        {
            return Random(new Random());
        }

        /// <summary>
        /// Returns a random location.
        /// </summary>
        /// <returns></returns>
        public static Position Random(Random generator)
        {
            return new Position(Longitude.Random(generator), Latitude.Random(generator));
        }

        /// <summary>
        /// Returns a random location within the specified geographic rectangle.
        /// </summary>
        /// <returns></returns>
        public static Position Random(Latitude southernmost, Longitude westernmost, Latitude northernmost, Longitude easternmost)
        {
            return Random(new Random(), southernmost, westernmost, northernmost, easternmost);
        }

        /// <summary>
        /// Returns a random location within the specified geographic rectangle.
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to generate random values.</param>
        /// <param name="easternmost">A <strong>Longitude</strong> specifying the eastern-most allowed longitude.</param>
        /// <param name="northernmost">A <strong>Latitude</strong> specifying the northern-most allowed latitude.</param>
        /// <param name="southernmost">A <strong>Latitude</strong> specifying the southern-most allowed latitude.</param>
        /// <param name="westernmost">A <strong>Longitude</strong> specifying the western-most allowed longitude.</param>
        /// <returns></returns>
        public static Position Random(Random generator, Latitude southernmost, Longitude westernmost, Latitude northernmost, Longitude easternmost)
        {
            return new Position(Longitude.Random(generator, easternmost, westernmost),
                                Latitude.Random(generator, northernmost, southernmost));
        }

        /// <overloads>Returns the direction of travel from one position to another.</overloads>
        public static Azimuth BearingTo(Position start, Position destination)
        {
            return start.BearingTo(destination);
        }

        /// <overloads>
        /// Returns the position shifted by the specified bearing and distance as new
        /// Position object.
        /// </overloads>
        /// <summary>Returns a new instance shifted by the specified direction and 
        /// distance.</summary>
        /// <returns>A new <strong>Position</strong> object adjusted by the specified 
        /// amount.</returns>
        /// <remarks>
        /// 	<para>This method is typically used to create an destination point relative to an
        ///  existing location. For example, this method could be used to create a point ten
        ///  miles northeast of the current location.</para>
        /// 	<para><em>NOTE: The trigonometric formula used for this method is subject to errors
        ///  when the distance to translate falls below a quarter mile (approximately 433
        ///  meters).</em></para>
        /// </remarks>
        /// <example>
        /// 	<code lang="VB" title="[New Example]" description="This example creates a destination point ten miles northwest of the 
        ///                current location.">
        /// ' Create a distance of ten miles
        /// Dim TravelDistance As New Distance(10, DistanceUnit.StatuteMiles)
        /// ' Calculate the point
        /// Dim DestinationPoint As Position
        /// DestinationPoint = Position.CurrentPosition.TranslateTo(Azimuth.Northwest, 
        /// TravelDistance)
        ///     </code>
        /// </example>
        public static Position TranslateTo(Position start, Angle bearing, Distance distance)
        {
            try
            {
                return start.TranslateTo(bearing, distance);
            }
            catch
            {
                throw;
            }
        }

        /// <overloads>Calculates a position which marks the intersection of two 
        /// vectors.</overloads>
        /// <summary>Calculates the point (if any) at which two imaginary lines 
        /// intersect.</summary>
        /// <returns>A <strong>Position</strong> object specifying the intersection 
        /// point.</returns>
        /// <remarks>
        /// This method uses trigonometry to calculate the point at which two lines intersect
        /// on Earth's surface. This method is typically used to see where two objects in motion
        /// would meet given their current directions of travel.  This method does not take the speed 
        /// of each object into account.
        /// <img src="IntersectionOf.jpg"/>
        /// </remarks>
        /// <param name="firstPosition">A <strong>Position</strong> specifying a position which marks the start of a line.</param>
        /// <param name="firstBearing">An <strong>Angle</strong> specifying a direction from the first Position.</param>
        /// <param name="secondPosition">A <strong>Position</strong> specifying the second position, marking the start of a second line.</param>
        /// <param name="secondBearing">An <strong>Angle</strong> specifying a direction from the second Position.</param>
        public static Position IntersectionOf(Position firstPosition, Angle firstBearing, Position secondPosition, Angle secondBearing)
        {
            try
            {
                return firstPosition.IntersectionOf(firstBearing, secondPosition, secondBearing);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the remaining travel distance if traveling for a certain speed for a certain period of time.
        /// </summary>
        /// <param name="start">A <strong>Position</strong> marking the starting location from which to calculate.</param>
        /// <param name="destination">A <strong>Position</strong> marking the destination location.</param>
        /// <param name="speed">A <strong>Speed</strong> travelled from the current instance.</param>
        /// <param name="time">A <strong>TimeSpan</strong> representing the time already elapsed during transit to the destination.</param>
        /// <returns>A <strong>Distance</strong> measuring the remaining distance to travel.</returns>
        public static Distance DistanceTo(Position start, Position destination, Speed speed, TimeSpan time)
        {
            return start.DistanceTo(destination, speed, time);
        }


        /// <overloads>Calculates the great circle distance between any two points on 
        /// Earth.</overloads>
        /// <summary>
        /// Returns the distance over land from the given starting point to the specified
        /// destination.
        /// </summary>
        /// <returns>
        /// A <strong>Distance</strong> object containing the calculated distance in
        /// kilometers.
        /// </returns>
        /// <param name="start">A beginning point from which to calculate distance.</param>
        /// <param name="destination">The ending point of a segment.</param>
        /// <remarks>
        /// This method uses trigonometry to calculate the Great Circle (over Earth's curved
        /// surface) distance between any two points on Earth. The distance is returned in
        /// kilometers but can be converted to any other unit type using methods in the
        /// <see cref="Distance">Distance</see>
        /// class.
        /// </remarks>
        public static Distance DistanceTo(Position start, Position destination)
        {
            return start.DistanceTo(destination);
        }

        /// <overloads>
        /// Calculates the time required to arrive at a destination when traveling at the
        /// specified speed.
        /// </overloads>
        /// <summary>
        /// Returns the minimum amount of time required to reach the specified destination at
        /// the specified speed.
        /// </summary>
        public static TimeSpan TimeTo(Position start, Position destination, Speed speed)
        {
            return start.TimeTo(destination, speed);
        }

        /// <overloads>
        /// Calculates the minimum speed required to arrive at a destination in the given
        /// time.
        /// </overloads>
        /// <summary>
        /// Returns the minimum speed required to travel over land from the given starting
        /// point to the specified destination within the specified period of time.
        /// </summary>
        /// <returns>
        /// A <strong>Speed</strong> object containing the required minimum travel
        /// speed.
        /// </returns>
        /// <remarks>
        /// This method is typically used to compare the current speed with the minimum
        /// required speed. For example, if the current rate of travel is 30MPH and the minimum
        /// speed is 60MPH, it can be derived that the speed must be doubled to arrive at the
        /// destination on time. Of course, care must be taken when making any suggestion to
        /// increase driving speed.
        /// </remarks>
        /// <param name="start">The beginning point from which calculations are based.</param>
        /// <param name="destination">The ending point to which speed is calculated.</param>
        /// <param name="time">The amount of time allowed to reach the destination.</param>
        public static Speed SpeedTo(Position start, Position destination, TimeSpan time)
        {
            try
            {
                return start.SpeedTo(destination, time);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Converts a string-based positional measurement into a Position 
        /// object.</summary>
        /// <param name="value">A <strong>String</strong> containing both latitude and longitude in the form of a string.</param>
        /// <remarks>
        /// This powerful method will analyze a string containing latitude and longitude and
        /// create a Position object matching the specified values.  The latitude and longitude
        /// must be separated by a non-space delimiter such as a comma.</remarks>
        public static Position Parse(string value)
        {
            return new Position(value, CultureInfo.CurrentCulture);
        }

        public static Position Parse(string value, CultureInfo culture)
        {
            return new Position(value, culture);
        }

        public static Position ParseAsLatLong(string value, CultureInfo culture)
        {
            try
            {
                // Take the value and add spaces after N, S, E, or W
                value = value.Replace("N", "N ").Replace("S", "S ").Replace("E", "E ").Replace("W", "W ")
                    .Replace("n", "N ").Replace("s", "S ").Replace("e", "E ").Replace("w", "W ")
                    // And remove duplicate spaces
                    .Replace("  ", " ").Replace(" , ", ",").Trim();

                // How many words are there?
                string[] Values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());

                // Just one?
                if (Values.Length == 1)
                {
                    // Yep.  Try to split using a space
                    Values = value.Split(' ');
                }

                // Parse out latitude and longitude
                bool IsLatitudeHandled = false;
                bool IsLongitudeHandled = false;
                Latitude pLatitude = Latitude.Empty;
                Longitude pLongitude = Longitude.Empty;

                for (int index = 0; index < Values.Length; index++)
                {
                    string Word = Values[index];

                    // Is this a latitude?
                    if (Word.IndexOf("N") != -1 || Word.IndexOf("n") != -1 || Word.IndexOf("S") != -1 || Word.IndexOf("s") != -1)
                    {
                        // Do we already have a latitude?
                        if (IsLatitudeHandled)
                        {
                            return Invalid;
                        }
                        pLatitude = Latitude.Parse(Word, culture);
                        IsLatitudeHandled = true;
                    }
                    // Is this a longitude?
                    else if (Word.IndexOf("E") != -1 || Word.IndexOf("e") != -1 || Word.IndexOf("W") != -1 || Word.IndexOf("w") != -1)
                    {
                        // Do we already have a longitude?
                        if (IsLongitudeHandled)
                        {
                            return Invalid;
                        }
                        pLongitude = Longitude.Parse(Word, culture);
                        IsLongitudeHandled = true;
                    }
                    else
                    {
                        // Take a guess
                        if (IsLongitudeHandled)
                        {
                            pLatitude = Latitude.Parse(Word, culture);
                            IsLatitudeHandled = true;
                        }
                        else if (IsLatitudeHandled)
                        {
                            pLongitude = Longitude.Parse(Word, culture);
                            IsLongitudeHandled = true;
                        }
                    }
                }

                if (IsLongitudeHandled && IsLatitudeHandled)
                {
                    return new Position(pLatitude, pLongitude);
                }
                else
                {
                    // Zero?
                    if (Values.Length > 1 && Values[0].Trim() == Values[1].Trim())
                    {
                        return new Position(Latitude.Parse(Values[0]), Longitude.Parse(Values[1]));
                    }

                    // Try to interpret as doubles (?)
                    // No.  The coordinate information is ambiguous!  Which is the lat
                    // and which is the long?
                    return Invalid;
                }
            }
            catch
            {
                return Invalid;
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        public static Position operator +(Position left, Position right)
        {
            return left.Add(right);
        }

        public static Position operator -(Position left, Position right)
        {
            return left.Subtract(right);
        }

        public static Position operator *(Position left, Position right)
        {
            return left.Multiply(right);
        }

        public static Position operator /(Position left, Position right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Adds the specified latitude and longitude from the current latitude and longitude.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Position Add(Position position)
        {
            return new Position(_Latitude.Add(position.Latitude.DecimalDegrees),
                _Longitude.Add(position.Longitude.DecimalDegrees));
        }

        /// <summary>
        /// Subtracts the specified latitude and longitude from the current latitude and longitude.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Position Subtract(Position position)
        {
            return new Position(_Latitude.Subtract(position.Latitude.DecimalDegrees),
                _Longitude.Subtract(position.Longitude.DecimalDegrees));
        }

        /// <summary>
        /// Multiplies the specified latitude and longitude from the current latitude and longitude.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Position Multiply(Position position)
        {
            return new Position(_Latitude.Multiply(position.Latitude.DecimalDegrees),
                _Longitude.Multiply(position.Longitude.DecimalDegrees));
        }

        /// <summary>
        /// Divides the specified latitude and longitude from the current latitude and longitude.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Position Divide(Position position)
        {
            return new Position(_Latitude.Divide(position.Latitude.DecimalDegrees), _Longitude.Divide(position.Longitude.DecimalDegrees));
        }

        #endregion

        #region Conversions

        public static explicit operator Position(string value)
        {
            return Position.Parse(value, CultureInfo.CurrentCulture);
        }

        public static explicit operator string(Position value)
        {
            return value.ToString();
        }

        #endregion

        #region ICloneable<Position> Members

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public Position Clone()
        {
            return new Position(_Longitude, _Latitude);
        }

        #endregion

        #region IEquatable<Position> Members

        /// <summary>
        /// Compares the current instance to the specified position.
        /// </summary>
        /// <param name="other">A <strong>Position</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are identical.</returns>
        /// <remarks>The two objects are compared at up to four digits of precision.</remarks>
        public bool Equals(Position other)
        {
            //if (other == null) return false;
            return _Latitude.Equals(other.Latitude) && _Longitude.Equals(other.Longitude);
        }

        /// <summary>
        /// Compares the current instance to the specified position using the specified numeric precision.
        /// </summary>
        /// <param name="other">A <strong>Position</strong> object to compare with.</param>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of fractional digits to compare.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are identical.</returns>
        /// <remarks>This method is typically used when positions do not mark the same location unless they are
        /// extremely close to one another.  Conversely, a low or even negative value for <strong>Precision</strong>
        /// allows positions to be considered equal even when they do not precisely match.</remarks>
        public bool Equals(Position other, int decimals)
        {
            // Compare latitude and longitude
            return (_Latitude.Equals(other.Latitude, decimals)
                && _Longitude.Equals(other.Longitude, decimals));
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and culture information.
        /// </summary>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            // Output as latitude and longitude
            return Latitude.ToString(format, culture) 
                + culture.TextInfo.ListSeparator
                + Longitude.ToString(format, culture);
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            /* The position class uses the GML 3.0 specification for XML.
             * 
             * <gml:pos>X Y</gml:pos>
             *
             */
            writer.WriteStartElement(Xml.GmlXmlPrefix, "pos", Xml.GmlXmlNamespace);
            writer.WriteString(_Longitude.DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(_Latitude.DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            /* The position class uses the GML 3.0 specification for XML.
             * 
             * <gml:pos>X Y</gml:pos>
             *
             * ... but it is also helpful to be able to READ older versions
             * of GML, such as this one for GML 2.0:
             * 
             * <gml:coord>
             *      <gml:X>double</gml:X>
             *      <gml:Y>double</gml:Y>  // optional
             *      <gml:Z>double</gml:Z>  // optional
             * </gml:coord>
             * 
             */

            _Latitude = Latitude.Invalid;
            _Longitude = Longitude.Invalid;

            // Move to the <gml:pos> or <gml:coord> element
            if (!reader.IsStartElement("pos", Xml.GmlXmlNamespace)
                && !reader.IsStartElement("coord", Xml.GmlXmlNamespace))
                reader.ReadStartElement();

            switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
            {
                case "pos":
                    // Read the "X Y" string, then split by the space between them
                    string[] Values = reader.ReadElementContentAsString().Split(' ');
                    // Deserialize the longitude
                    _Longitude = new Longitude(double.Parse(Values[0], CultureInfo.InvariantCulture));
                    // Deserialize the latitude
                    if (Values.Length > 1)
                        _Latitude = new Latitude(double.Parse(Values[1], CultureInfo.InvariantCulture));
                    break;
                case "coordinates":
                    // Read the "X Y" string, then split by the space between them
                    string[] CoordSets = reader.ReadElementContentAsString().Split(' ');
                    string[] Coords = CoordSets[0].Split(',');
                    // Deserialize the longitude
                    _Longitude = new Longitude(double.Parse(Coords[0], CultureInfo.InvariantCulture));
                    // Deserialize the latitude
                    if (Coords.Length > 1)
                        _Latitude = new Latitude(double.Parse(Coords[1], CultureInfo.InvariantCulture));
                    break;
                case "coord":
                    // Read the <gml:coord> start tag
                    reader.ReadStartElement();
                    // Now read up to 3 elements: X, and optionally Y or Z
                    for (int index = 0; index < 3; index++)
                    {
                        /* According to the GML specification, a "gml:x" tag is lower-case.  However,
                         * FWTools outputs tags in uppercase "gml:X".  As a result, make this 
                         * test case-insensitive.
                         */

                        switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "x":
                                _Longitude = new Longitude(reader.ReadElementContentAsDouble());
                                break;
                            case "y":
                                _Latitude = new Latitude(reader.ReadElementContentAsDouble());
                                break;
                            case "z":
                                // Skip Z
                                reader.Skip();
                                break;
                        }

                        // If we're at an end element, stop
                        if (reader.NodeType == XmlNodeType.EndElement)
                            break;
                    }
                    // Read the </gml:coord> end tag
                    reader.ReadEndElement();
                    break;
            }
        }

        #endregion
    }
}

namespace DotSpatial.Positioning.Projections
{
    /// <summary>Indicates a vertical slice of the Earth used as a starting point for UTM positions.</summary>
    public enum ZoneLetter
    {
        Unknown = -1,
        Z = 0,
        C,
        D,
        E,
        F,
        G,
        H,
        J,
        K,
        L,
        M,
        N,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X
    }
}
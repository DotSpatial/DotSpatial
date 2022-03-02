using System;

namespace DotSpatial.Positioning
{
    /// <summary>Represents information about an angle when an angle-related event is raised.</summary>
	/// <remarks>
	/// This class is used for events which use an <strong>Angle</strong> as a
	/// parameter.
	/// </remarks>
	/// <example>
	///     This example demonstrates how to use <strong>AngleEventArgs</strong> when raising
	///     an event. 
	///     <code lang="VB">
	/// ' Declare a new event
	/// Dim MyAngleEvent As AngleEventHandler
	///  
	/// Sub Main()
	///     ' Create an angle of 90°
	///     Dim MyAngle As New Angle(90);
	///     ' Raise our custom Event
	///     RaiseEvent MyAngleEvent(Me, New AngleEventArgs(MyAngle));
	/// End Sub
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a new event
	/// AngleEventHandler MyAngleEvent;
	///  
	/// void Main()
	/// {
	///     // Create an angle of 90°
	///     Angle MyAngle = new Angle(90);
	///     // Raise our custom event
	///     if(MyAngleEvent != null)
	///         MyAngleEvent(this, new AngleEventArgs(MyAngle));
	/// }
	///     </code>
	/// </example>
	/// <seealso cref="Angle">Angle Class</seealso>
	/// <seealso cref="AngleEventHandler">AngleEventHandler Delegate</seealso>
	public sealed class AngleEventArgs : EventArgs
	{
		private Angle _Angle;

		/// <summary>
		/// Creates a new instance containing the specified Angle object.
		/// </summary>

		public AngleEventArgs(Angle angle)
		{
			_Angle = angle;
		}

		/// <summary>
		/// Represents information about an angular measurement when an angle-related event is raised.
		/// </summary>
		/// <value>An <strong>Angle</strong> object containing a property which has changed.</value>
		/// <remarks>This class is used by the <see cref="Angle">Angle</see> class to provide notification when hours, minutes, or seconds properties have changed.</remarks>
		/// <seealso cref="Angle">Angle Class</seealso>
		public Angle Angle
		{
			get
			{
				return _Angle;
			}
		}
	}

    	/// <summary>Represents information about a area when an area-related event is raised.</summary>
	/// <example>
	///     This example demonstrates how to use the <strong>AreaEventArgs</strong> class when
	///     raising an event.
	///     <code lang="VB">
	/// ' Declare a new event
	/// Dim MyAreaEvent As AreaEventHandler
	///  
	/// Sub Main()
	///     ' Create a Area of 125 kilometers
	///     Dim MyArea As New Area(125, AreaUnit.SquareKilometers)
	///     ' Raise our custom event
	///     RaiseEvent MyAreaEvent(Me, New AreaEventArgs(MyArea))
	/// End Sub
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a new event
	/// AreaEventHandler MyAreaEvent;
	///  
	/// void Main()
	/// {
	///     // Create a Area of 125 kilometers
	///     Area MyArea = new Area(125, AreaUnit.SquareKilometers);
	///     // Raise our custom event
	///     if(MyAreaEvent != null)
	///         MyAreaEvent(this, New AreaEventArgs(MyArea));
	/// }
	///     </code>
	/// </example>
	/// <seealso cref="Area">Area Class</seealso>
	/// <seealso cref="AreaEventHandler">AreaEventHandler Delegate</seealso>
    public sealed class AreaEventArgs : EventArgs
	{
		private Area _Area;

		/// <summary>
		/// Creates a new instance containing the specified Area object.
		/// </summary>

		public AreaEventArgs(Area value)
		{
			_Area = value;
		}

		/// <summary>
		/// Represents information about a Area measurement when an Area-related event is raised.
		/// </summary>
		/// <value>A <strong>Area</strong> object containing a property which has changed.</value>
		/// <remarks>This class is used by the <see cref="Area">Area</see> class to provide notification 
		/// when hours, minutes, or seconds properties have changed.</remarks>
		/// <seealso cref="Area">Area Class</seealso>
		public Area Area
		{
			get
			{
				return _Area;
			}
		}
	}
    
    /// <summary>Represents information about an angle when an angle-related event is raised.</summary>
	/// <remarks>
	/// This class is used for events which use an <strong>Azimuth</strong> as a
	/// parameter.
	/// </remarks>
	/// <example>
	///     This example demonstrates how to use <strong>AzimuthEventArgs</strong> when raising
	///     an event. 
	///     <code lang="VB">
	/// ' Declare a new event
	/// Dim MyAzimuthEvent As AzimuthEventHandler
	///  
	/// Sub Main()
	///     ' Create an angle of 90°
	///     Dim MyAzimuth As New Azimuth(90);
	///     ' Raise our custom Event
	///     RaiseEvent MyAzimuthEvent(Me, New AzimuthEventArgs(MyAzimuth));
	/// End Sub
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a new event
	/// AzimuthEventHandler MyAzimuthEvent;
	///  
	/// void Main()
	/// {
	///     // Create an angle of 90°
	///     Azimuth MyAzimuth = new Azimuth(90);
	///     // Raise our custom event
	///     if(MyAzimuthEvent != null)
	///         MyAzimuthEvent(this, new AzimuthEventArgs(MyAzimuth));
	/// }
	///     </code>
	/// </example>
	/// <seealso cref="Azimuth">Azimuth Class</seealso>
	/// <seealso cref="AzimuthEventHandler">AzimuthEventHandler Delegate</seealso>
	public sealed class AzimuthEventArgs : EventArgs
	{
		private Azimuth _Azimuth;

		/// <summary>
		/// Creates a new instance containing the specified Azimuth object.
		/// </summary>

		public AzimuthEventArgs(Azimuth angle)
		{
			_Azimuth = angle;
		}

		/// <summary>
		/// Represents information about an angular measurement when an angle-related event is raised.
		/// </summary>
		/// <value>An <strong>Azimuth</strong> object containing a property which has changed.</value>
		/// <remarks>This class is used by the <see cref="Azimuth">Azimuth</see> class to provide notification when hours, minutes, or seconds properties have changed.</remarks>
		/// <seealso cref="Azimuth">Azimuth Class</seealso>
		public Azimuth Azimuth
		{
			get
			{
				return _Azimuth;
			}
		}
	}

	public sealed class CancelableEventArgs : EventArgs
	{
		private bool _Canceled;		

		public CancelableEventArgs(bool isCanceled)
		{
			_Canceled = isCanceled;
		}

        /// <summary>
        /// Controls whether an operation is to be aborted.
        /// </summary>
        /// <remarks>This property, when set to True will signal that a pending operation should not execute.  For example,
        /// an event which is raised immediately before connecting would check this property to determine whether to 
        /// continue connecting, or exit.</remarks>
		public bool Canceled
		{
			get
			{
				return _Canceled;
			}
            set
            {
                _Canceled = value;
            }
		}
	}

       /// <summary>Represents information about a distance when an distance-related event is raised.</summary>
    /// <remarks>This class is typically used for events in the <see cref="Distance">Distance</see> class to 
    /// provide notification when hours, minutes, decimal minutes or seconds properties have changed.</remarks>
    /// <example>This example demonstrates how to use this class when raising an event.
    /// <code lang="VB">
    /// ' Declare a new event
    /// Dim MyDistanceEvent As DistanceEventHandler
    /// ' Create a distance of 125 kilometers
    /// Dim MyDistance As New Distance(125, DistanceUnit.Kilometers)
    /// 
    /// Sub Main()
    ///   ' Raise our custom event
    ///   RaiseEvent MyDistanceEvent(Me, New DistanceEventArgs(MyDistance))
    /// End Sub
    /// </code>
    /// <code lang="C#">
    /// // Declare a new event
    /// DistanceEventHandler MyDistanceEvent;
    /// // Create a distance of 125 kilometers
    /// Distance MyDistance = new Distance(125, DistanceUnit.Kilometers);
    /// 
    /// void Main()
    /// {
    ///   // Raise our custom event
    ///   MyDistanceEvent(this, New DistanceEventArgs(MyDistance));
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="Distance">Distance Class</seealso>
    /// <seealso cref="DistanceEventHandler">DistanceEventHandler Delegate</seealso>
    public sealed class DistanceEventArgs : EventArgs
    {
        private Distance _Distance;

        /// <summary>
        /// Creates a new instance containing the specified Distance object.
        /// </summary>

        ////[CLSCompliant(false)]
        public DistanceEventArgs(Distance value)
        {
            _Distance = value;
        }

        /// <summary>
        /// Represents information about a distance measurement when an distance-related event is raised.
        /// </summary>
        /// <value>A <strong>Distance</strong> object containing a property which has changed.</value>
        /// <remarks>This class is used by the <see cref="Distance">Distance</see> class to provide notification 
        /// when hours, minutes, or seconds properties have changed.</remarks>
        /// <seealso cref="Distance">Distance Class</seealso>
        ////[CLSCompliant(false)]
        public Distance Distance
        {
            get
            {
                return _Distance;
            }
        }
    }

    	/// <summary>Represents information about an angle when an angle-related event is raised.</summary>
	/// <remarks>
	/// This class is used for events which use an <strong>Elevation</strong> as a
	/// parameter.
	/// </remarks>
	/// <example>
	///     This example demonstrates how to use <strong>ElevationEventArgs</strong> when raising
	///     an event. 
	///     <code lang="VB">
	/// ' Declare a new event
	/// Dim MyElevationEvent As ElevationEventHandler
	///  
	/// Sub Main()
	///     ' Create an angle of 90°
	///     Dim MyElevation As New Elevation(90);
	///     ' Raise our custom Event
	///     RaiseEvent MyElevationEvent(Me, New ElevationEventArgs(MyElevation));
	/// End Sub
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a new event
	/// ElevationEventHandler MyElevationEvent;
	///  
	/// void Main()
	/// {
	///     // Create an angle of 90°
	///     Elevation MyElevation = new Elevation(90);
	///     // Raise our custom event
	///     if(MyElevationEvent != null)
	///         MyElevationEvent(this, new ElevationEventArgs(MyElevation));
	/// }
	///     </code>
	/// </example>
	/// <seealso cref="Elevation">Elevation Class</seealso>
	/// <seealso cref="ElevationEventHandler">ElevationEventHandler Delegate</seealso>
	public sealed class ElevationEventArgs : EventArgs
	{
		private Elevation _Elevation;

		/// <summary>
		/// Creates a new instance containing the specified Elevation object.
		/// </summary>

		public ElevationEventArgs(Elevation angle)
		{
			_Elevation = angle;
		}

		/// <summary>
		/// Represents information about an angular measurement when an angle-related event is raised.
		/// </summary>
		/// <value>An <strong>Elevation</strong> object containing a property which has changed.</value>
		/// <remarks>This class is used by the <see cref="Elevation">Elevation</see> class to provide notification when hours, minutes, or seconds properties have changed.</remarks>
		/// <seealso cref="Elevation">Elevation Class</seealso>
		public Elevation Elevation
		{
			get
			{
				return _Elevation;
			}
		}
	}

    
    //public sealed class GeographicRectangleEventArgs : EventArgs
    //{
    //    private GeographicRectangle _Rectangle;

    //    public GeographicRectangleEventArgs(GeographicRectangle rectangle)
    //    {
    //        _Rectangle = rectangle;
    //    }

    //    public GeographicRectangle GeographicRectangle
    //    {
    //        get
    //        {
    //            return _Rectangle;
    //        }
    //    }
    //}

    	/// <summary>Represents information about an angle when an angle-related event is raised.</summary>
	/// <remarks>
	/// This class is used for events which use an <strong>Latitude</strong> as a
	/// parameter.
	/// </remarks>
	/// <example>
	///     This example demonstrates how to use <strong>LatitudeEventArgs</strong> when raising
	///     an event. 
	///     <code lang="VB">
	/// ' Declare a new event
	/// Dim MyLatitudeEvent As LatitudeEventHandler
	///  
	/// Sub Main()
	///     ' Create an angle of 90°
	///     Dim MyLatitude As New Latitude(90);
	///     ' Raise our custom Event
	///     RaiseEvent MyLatitudeEvent(Me, New LatitudeEventArgs(MyLatitude));
	/// End Sub
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a new event
	/// LatitudeEventHandler MyLatitudeEvent;
	///  
	/// void Main()
	/// {
	///     // Create an angle of 90°
	///     Latitude MyLatitude = new Latitude(90);
	///     // Raise our custom event
	///     if(MyLatitudeEvent != null)
	///         MyLatitudeEvent(this, new LatitudeEventArgs(MyLatitude));
	/// }
	///     </code>
	/// </example>
	/// <seealso cref="Latitude">Latitude Class</seealso>
	/// <seealso cref="LatitudeEventHandler">LatitudeEventHandler Delegate</seealso>
    public sealed class LatitudeEventArgs : EventArgs
	{
		private Latitude _Latitude;

		/// <summary>
		/// Creates a new instance containing the specified Latitude object.
		/// </summary>

		public LatitudeEventArgs(Latitude angle)
		{
			_Latitude = angle;
		}

		/// <summary>
		/// Represents information about an angular measurement when an angle-related event is raised.
		/// </summary>
		/// <value>An <strong>Latitude</strong> object containing a property which has changed.</value>
		/// <remarks>This class is used by the <see cref="Latitude">Latitude</see> class to provide notification when hours, minutes, or seconds properties have changed.</remarks>
		/// <seealso cref="Latitude">Latitude Class</seealso>
		public Latitude Latitude
		{
			get
			{
				return _Latitude;
			}
		}
	}

    	/// <summary>Represents information about an angle when an angle-related event is raised.</summary>
	/// <remarks>
	/// This class is used for events which use an <strong>Longitude</strong> as a
	/// parameter.
	/// </remarks>
	/// <example>
	///     This example demonstrates how to use <strong>LongitudeEventArgs</strong> when raising
	///     an event. 
	///     <code lang="VB">
	/// ' Declare a new event
	/// Dim MyLongitudeEvent As LongitudeEventHandler
	///  
	/// Sub Main()
	///     ' Create an angle of 90°
	///     Dim MyLongitude As New Longitude(90);
	///     ' Raise our custom Event
	///     RaiseEvent MyLongitudeEvent(Me, New LongitudeEventArgs(MyLongitude));
	/// End Sub
	///     </code>
	/// 	<code lang="CS">
	/// // Declare a new event
	/// LongitudeEventHandler MyLongitudeEvent;
	///  
	/// void Main()
	/// {
	///     // Create an angle of 90°
	///     Longitude MyLongitude = new Longitude(90);
	///     // Raise our custom event
	///     if(MyLongitudeEvent != null)
	///         MyLongitudeEvent(this, new LongitudeEventArgs(MyLongitude));
	/// }
	///     </code>
	/// </example>
	/// <seealso cref="Longitude">Longitude Class</seealso>
	/// <seealso cref="LongitudeEventHandler">LongitudeEventHandler Delegate</seealso>
    public sealed class LongitudeEventArgs : EventArgs
	{
		private Longitude _Longitude;

		/// <summary>
		/// Creates a new instance containing the specified Longitude object.
		/// </summary>

		public LongitudeEventArgs(Longitude longitude)
		{
			_Longitude = longitude;
		}

		/// <summary>
		/// Represents information about an angular measurement when an angle-related event is raised.
		/// </summary>
		/// <value>An <strong>Longitude</strong> object containing a property which has changed.</value>
		/// <remarks>This class is used by the <see cref="Longitude">Longitude</see> class to provide notification when hours, minutes, or seconds properties have changed.</remarks>
		/// <seealso cref="Longitude">Longitude Class</seealso>
		public Longitude Longitude
		{
			get
			{
				return _Longitude;
			}
		}
	}

    /*
    public sealed class PolarCoordinateEventArgs : EventArgs
    {
        private PolarCoordinate _PolarCoordinate;

        public PolarCoordinateEventArgs(PolarCoordinate polarCoordinate)
            : base()
        {
            _PolarCoordinate = polarCoordinate;
        }

        public PolarCoordinate PolarCoordinate
        {
            get
            {
                return _PolarCoordinate;
            }
        }
    }

    public sealed class PolarCoordinateOrientationEventArgs : EventArgs
    {
        private PolarCoordinateOrientation _PolarCoordinateOrientation;

        public PolarCoordinateOrientationEventArgs(PolarCoordinateOrientation polarCoordinateOrientation)
            : base()
        {
            _PolarCoordinateOrientation = polarCoordinateOrientation;
        }

        public PolarCoordinateOrientation PolarCoordinateOrientation
        {
            get
            {
                return _PolarCoordinateOrientation;
            }
        }
    }
     */

    public sealed class PositionEventArgs : EventArgs
	{
		private Position _Position;

		public PositionEventArgs(Position position) 
            : base()
		{
			_Position = position;
		}

		public Position Position
		{
			get
			{
				return _Position;
			}
		}
	}
    
    public sealed class RadianEventArgs : EventArgs
	{
		private Radian _Radians;

		public RadianEventArgs(Radian radian)
		{
			_Radians = radian;
		}

		public Radian Radians
		{
			get
			{
				return _Radians;
			}
		}
	}

    
    public sealed class SpeedEventArgs : EventArgs
    {

        private Speed _Speed;

        public SpeedEventArgs(Speed speed)
        {
            _Speed = speed;
        }

        public Speed Speed 
        {
            get
            {
                return _Speed;
            }
        }
    }

    //public sealed class TimeoutExceptionEventArgs : EventArgs
    //{
    //    private TimeoutException _Exception;

    //    public TimeoutExceptionEventArgs(TimeoutException exception)
    //    {
    //        _Exception = exception;
    //    }

    //    public TimeoutException TimeoutException
    //    {
    //        get
    //        {
    //            return _Exception;
    //        }
    //    }
    //}


	public sealed class TimeSpanEventArgs : EventArgs
	{
		private TimeSpan _TimeSpan;

		/// <summary>
		/// Creates a new instance containing the specified TimeSpan object.
		/// </summary>
		/// <param name="timeSpan">A <strong>TimeSpan</strong> object describing a length of time.</param>
		/// <remarks></remarks>
		/// <seealso cref="TimeSpan">TimeSpan Property</seealso>
		/// <seealso cref="System.TimeSpan">TimeSpan Structure</seealso>
		public TimeSpanEventArgs(TimeSpan timeSpan)
		{
			_TimeSpan = timeSpan;
		}

		/// <summary>
		/// Indicates a length of time which is the target of the event.
		/// </summary>
		/// <value>A <strong>TimeSpan</strong> object describing a length of time.</value>
		/// <remarks></remarks>
		/// <seealso cref="System.TimeSpan">TimeSpan Structure</seealso>
		public TimeSpan TimeSpan
		{
			get
			{
				return _TimeSpan;
			}
		}
	}

    ///// <summary>
    ///// Represents information about a byte array when a byte array-related event is
    ///// raised.
    ///// </summary>
    ///// <remarks>This class is used for events which use a byte array as a parameter.</remarks>
    ///// <example>
    /////     This example demonstrates how to use <strong>ByteArrayEventArgs</strong> when
    /////     raising an event.
    /////     <code lang="VB" title="[New Example]">
    ///// ' Declare a new event
    ///// Dim MyByteArrayEvent As ByteArrayEventHandler
    /////  
    ///// Sub Main()
    /////     ' Create a byte array
    /////     Dim MyArray As New Byte(50);
    /////     ' Raise our custom Event
    /////     RaiseEvent MyByteArrayEvent(Me, New ByteArrayEventArgs(MyByteArray));
    ///// End Sub
    /////     </code>
    ///// 	<code lang="CS" title="[New Example]">
    ///// // Declare a new event
    ///// ByteArrayEventHandler MyByteArrayEvent;
    /////  
    ///// void Main()
    ///// {
    /////     // Create a byte array
    /////     byte[] MyByteArray = new byte(90);
    /////     // Raise our custom event
    /////     if(MyByteArrayEvent != null)
    /////         MyByteArrayEvent(this, new ByteArrayEventArgs(MyByteArray));
    ///// }
    /////     </code>
    ///// </example>
    //public sealed class ByteArrayEventArgs : EventArgs
    //{

    //    private byte[] _Bytes;

    //    public ByteArrayEventArgs(byte[] bytes)
    //    {
    //        _Bytes = bytes;
    //    }

    //    public byte[] Bytes
    //    {
    //        get
    //        {
    //            return _Bytes;
    //        }
    //    }
    //}

    //public sealed class ArrayEventArgs : EventArgs
    //{

    //    private Array _Array;

    //    public static new readonly ArrayEventArgs Empty = new ArrayEventArgs(null);

    //    public ArrayEventArgs(Array array)
    //    {
    //        _Array = array;
    //    }

    //    public Array Array
    //    {
    //        get
    //        {
    //            return _Array;
    //        }
    //    }
    //}

	/// <summary>
	/// Represents information about the date and time reported by the GPS device.
	/// </summary>
#if !PocketPC || DesignTime
#if Framework20
    //[Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    //[Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    //[Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
#endif
	public sealed class DateTimeEventArgs : EventArgs
	{
		private DateTime _DateTime;
		private bool _SystemClockUpdated;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="dateTime">A DateTime object containing a date and time reported by the GPS device.</param>
		public DateTimeEventArgs(System.DateTime dateTime)
		{
			_DateTime = dateTime;
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="dateTime">A DateTime object containing a date and time reported by the GPS device.</param>
		/// <param name="systemClockUpdated">Indicates whether the system clock was updated to match this DateTime.</param>
		public DateTimeEventArgs(System.DateTime dateTime, bool systemClockUpdated)
		{
			_DateTime = dateTime;
			_SystemClockUpdated = systemClockUpdated;
		}

		/// <summary>
		/// A date and time value in UTC time (not adjusted for the local time zone).
		/// </summary>
		/// <value>A DateTime object containing a date and time reported by the GPS device.</value>
		/// <remarks>This date and time value is not adjusted to the local time zone.  Use the 
		/// <see cref="System.DateTime.ToLocalTime">ToLocalTime</see> method to adjust to local time.</remarks>
		/// <seealso cref="System.DateTime">DateTime Class</seealso>
		/// <seealso cref="System.DateTime.ToLocalTime">ToLocalTime Method (DateTime Class)</seealso>
		public System.DateTime DateTime
		{
			get
			{
				return _DateTime;
			}
		}

		/// <summary>
		/// Indicates whether the system clock updated to match the <see cref="DateTime"/>.
		/// </summary>
		/// <value>
		/// <see langword="true">True</see> if the system clock was updated; otherwise, <see langword="false"/>.
		/// The default is <see langword="false"/>.
		/// </value>
		public bool SystemClockUpdated
		{
			get
			{
				return _SystemClockUpdated;
			}
		}
	}

    /// <summary>
    /// Represents information about an exception when an error-related event is raised.
    /// </summary>
    /// <remarks>This object is used throughout the GPS.NET framework to provide notification when
    /// either of two situations occur:
    /// 
    /// <list>
    /// <item>An exception is thrown which cannot be trapped via a Try..Catch block (such as from a separate thread)</item>
    /// <item>An exception is thrown which can be recovered from and should not halt the current operation.</item>
    /// </list>
    /// Most frequently, this class is used when a parsing exception occurs via the Parse method or during automatic
    /// data collection.</remarks>
    /// <example>This example demonstrates how to use this class when raising an event.
    /// <code lang="VB">
    /// ' Create a new exception
    /// Dim MyException As New ApplicationException("The error was successfully created.")
    /// ' Declare a new event
    /// Dim MyErrorEvent As ExceptionEventHandler
    /// 
    /// Sub Main()
    ///   ' Raise our custom event
    ///   RaiseEvent MyErrorEvent(Me, New ExceptionEventArgs(MyException))
    /// End Sub
    /// </code>
    /// <code lang="C#">
    /// // Create a new exception
    /// ApplicationException MyException = new ApplicationException("The error was successfully created.")
    /// // Declare a new event
    /// ExceptionEventHandler MyErrorEvent;
    /// 
    /// void Main()
    /// {
    ///   // Raise our custom event
    ///   MySatelliteEvent(this, New ExceptionEventArgs(MyException));
    /// }
    /// </code>
    /// </example>
#if !PocketPC || DesignTime
#if Framework20
    //[Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    //[Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    //[Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
#endif
    public class ExceptionEventArgs : EventArgs
    {
        private Exception _Exception;

        /// <summary>
        /// Creates a new instance containing the specified exception object.
        /// </summary>
        /// <param name="exception">An <strong>Exception</strong> object or derivitive describing the error.</param>
        public ExceptionEventArgs(Exception exception)
        {
            _Exception = exception;
        }

        /// <summary>
        /// Indicates information about the error and its location within a module.
        /// </summary>
        /// <value>An <strong>ApplicationException</strong> object or derivitive describing the error.</value>
        public Exception Exception
        {
            get
            {
                return _Exception;
            }
        }
    }

#if !PocketPC || DesignTime
#if Framework20
    //[Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    //[Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    //[Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
#endif
    public class ProgressEventArgs : EventArgs
    {
        private int _Current;
        private int _Total;

        public new static readonly ProgressEventArgs Empty = new ProgressEventArgs(0, 0);

        public ProgressEventArgs(int total)
        {
            _Total = total;
        }

        public ProgressEventArgs(int current, int total)
        {
            _Current = current;
            _Total = total;
        }

        public int Total
        {
            get
            {
                return _Total;
            }
        }

        public int Current
        {
            get
            {
                return _Current;
            }
        }
    }
}
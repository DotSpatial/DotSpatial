using System;

namespace DotSpatial.Positioning.Gps
{
	/// <summary>
	/// Represents information about the method used to obtain a fix when a fix-related event is raised.
	/// </summary>
	/// <remarks>This object is used primarily by the <see cref="Receiver.FixMethodChanged">FixMethodChanged</see>
	/// event of the <see cref="Receiver">Receiver</see> class to provide notification when 
	/// updated fix method information has been received from the GPS device.</remarks>
	/// <example>This example demonstrates how to use this class when raising an event.
	/// <code lang="VB">
	/// ' Declare a new event
	/// Dim MyFixMethodEvent As EventHandler
	/// ' Create a FixMethod signifying a 3-D fix (both position and altitude)
	/// Dim MyFixMethod As FixMethod = FixMethod.Fix3D
	/// 
	/// Sub Main()
	///   ' Raise our custom event
	///   RaiseEvent MyFixMethodEvent(Me, New FixMethodEventArgs(MyFixMethod))
	/// End Sub
	/// </code>
	/// <code lang="C#">
	/// // Declare a new event
	/// EventHandler MyFixMethodEvent;
	/// // Create a FixMethod signifying a 3-D fix (both position and altitude)
	/// FixMethod MyFixMethod = FixMethod.Fix3D;
	/// 
	/// void Main()
	/// {
	///   // Raise our custom event
	///   MyFixMethodEvent(this, New FixMethodEventArgs(MyFixMethod));
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="Receiver.FixMethod">FixMethod Property (Receiver Class)</seealso>
	/// <seealso cref="Receiver">Receiver Class</seealso>
	public sealed class FixMethodEventArgs : EventArgs
	{

		private FixMethod _FixMethod;

		/// <summary>
		/// Creates a new instance with the specified fix method.
		/// </summary>
		/// <param name="fixMethod">A value from the <strong>FixMethod</strong> enumeration.</param>
		/// <remarks></remarks>
		public FixMethodEventArgs(FixMethod fixMethod)
		{
			_FixMethod = fixMethod;
		}

		/// <summary>
		/// Indicates if the GPS device has no fix, a 2-D fix, or a 3-D fix.
		/// </summary>
		/// <value>A value from the <strong>FixMethod</strong> enumeration.</value>
		/// <remarks>A device is considered to have a "2-D" fix if there are three satellites
		/// involved in the fix.  A 2-D fix means that position information is available, but not 
		/// altitude.  If at least four satellites are fixed, both position and altitude are known,
		/// and the fix is considered 3-D.</remarks>
		/// <seealso cref="Receiver.FixMethod">FixMethod Property (Receiver Class)</seealso>
		public FixMethod FixMethod
		{
			get
			{
				return _FixMethod;
			}
		}
	}

	/// <summary>
	/// Represents information about the likelihood that a fix will be sustained (or obtained) when a when a fix-related event is raised.
	/// </summary>
	/// <remarks>This object is used primarily by the <see cref="Receiver.FixLikelihoodChanged">FixLikelihoodChanged</see>
	/// event of the <see cref="Receiver">Receiver</see> class to provide notification when 
	/// combined satellite radio signal strength has changed.</remarks>
	/// <example>This example demonstrates how to use this class when raising an event.
	/// <code lang="VB">
	/// ' Declare a new event
	/// Dim MyFixLikelihoodEvent As FixLikelihoodEventHandler
	/// ' Create a FixLikelihood signifying a 3-D fix (both position and altitude)
	/// Dim MyFixLikelihood As FixLikelihood = FixLikelihood.Fix3D
	/// 
	/// Sub Main()
	///   ' Raise our custom event
	///   RaiseEvent MyFixLikelihoodEvent(Me, New FixLikelihoodEventArgs(MyFixLikelihood))
	/// End Sub
	/// </code>
	/// <code lang="C#">
	/// // Declare a new event
	/// FixLikelihoodEventHandler MyFixLikelihoodEvent;
	/// // Create a FixLikelihood signifying a 3-D fix (both position and altitude)
	/// FixLikelihood MyFixLikelihood = FixLikelihood.Fix3D;
	/// 
	/// void Main()
	/// {
	///   // Raise our custom event
	///   MyFixLikelihoodEvent(this, New FixLikelihoodEventArgs(MyFixLikelihood));
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="Receiver.FixLikelihood">FixLikelihood Property (Receiver Class)</seealso>
	/// <seealso cref="Receiver">Receiver Class</seealso>
	public sealed class FixLikelihoodEventArgs : EventArgs
	{
		private FixLikelihood _FixLikelihood;

		/// <summary>
		/// Creates a new instance with the specified fix Likelihood.
		/// </summary>
		/// <param name="fixLikelihood">A value from the <strong>FixLikelihood</strong> enumeration.</param>
		/// <remarks></remarks>
		public FixLikelihoodEventArgs(FixLikelihood fixLikelihood)
		{
			_FixLikelihood = fixLikelihood;
		}

		/// <summary>
		/// Indicates if the GPS device has no fix, a 2-D fix, or a 3-D fix.
		/// </summary>
		/// <value>A value from the <strong>FixLikelihood</strong> enumeration.</value>
		/// <remarks>A device is considered to have a "2-D" fix if there are three satellites
		/// involved in the fix.  A 2-D fix means that position information is available, but not 
		/// altitude.  If at least four satellites are fixed, both position and altitude are known,
		/// and the fix is considered 3-D.</remarks>
		/// <seealso cref="Receiver.FixLikelihood">FixLikelihood Property (Receiver Class)</seealso>
		public FixLikelihood FixLikelihood
		{
			get
			{
				return _FixLikelihood;
			}
		}
	}

	/// <summary>
	/// Represents information about whether the fix method is chosen automatically when a fix-related event is raised.
	/// </summary>
	/// <remarks>This object is used primarily by the <see cref="Receiver.FixModeChanged">FixModeChanged</see>
	/// event of the <see cref="Receiver">Receiver</see> class to provide notification when 
	/// the receiver switches fix modes from <strong>Automatic</strong> to <strong>Manual</strong> or vice-versa.</remarks>
	/// <example>This example demonstrates how to use this class when raising an event.
	/// <code lang="VB">
	/// ' Declare a new event
	/// Dim MyFixModeEvent As EventHandler
	/// ' Create a FixMode signifying that the fix method is being chosen automatically
	/// Dim MyFixMode As FixMode = FixMode.Automatic 
	/// 
	/// Sub Main()
	///   ' Raise our custom event
	///   RaiseEvent MyFixModeEvent(Me, New FixModeEventArgs(MyFixMode))
	/// End Sub
	/// </code>
	/// <code lang="C#">
	/// // Declare a new event
	/// EventHandler MyFixModeEvent;
	/// // Create a FixMode signifying that the fix method is being chosen automatically
	/// FixMode MyFixMode = FixMode.Automatic;
	/// 
	/// void Main()
	/// {
	///   // Raise our custom event
	///   MyFixModeEvent(this, New FixModeEventArgs(MyFixMode));
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="Receiver.FixMethod">FixMethod Property (Receiver Class)</seealso>
	/// <seealso cref="Receiver">Receiver Class</seealso>
	public sealed class FixModeEventArgs : EventArgs
	{
		private FixMode _FixMode;

		/// <summary>
		/// Creates a new instance with the specified fix method.
		/// </summary>
		/// <param name="fixMode">A value from the <strong>FixMode</strong> enumeration.</param>
		/// <remarks></remarks>
		public FixModeEventArgs(FixMode fixMode)
		{
			_FixMode = fixMode;
		}

		/// <summary>
		/// Indicates if the GPS device is choosing the <see cref="FixMethod">fix method</see> automatically.
		/// </summary>
		/// <value>A value from the <strong>FixMode</strong> enumeration.</value>
		/// <remarks>A vast majority of GPS devices calculate the fix mode automatically.  This is because
		/// the process of determining the fix mode is a simple process: if there are three fixed satellites,
		/// a 2-D fix is obtained; if there are four or more fixed satellites, a 3-D fix is in progress. Any
		/// less than three satellites means that no fix is possible.
		/// </remarks>
		/// <seealso cref="Receiver.FixMode">FixMode Property (Receiver Class)</seealso>
		public FixMode FixMode
		{
			get
			{
				return _FixMode;
			}
		}
	}

	/// <summary>
	/// Represents information about the type of fix obtained when fix-related event is raised.
	/// </summary>
	/// <remarks>This object is used primarily by the <see cref="Receiver.FixQualityChanged">FixQualityChanged</see>
	/// event of the <see cref="Receiver">Receiver</see> class to provide notification when 
	/// the receiver switches fix modes from <strong>Automatic</strong> to <strong>Manual</strong> or vice-versa.</remarks>
	/// <example>This example demonstrates how to use this class when raising an event.
	/// <code lang="VB">
	/// ' Declare a new event
	/// Dim MyFixQualityEvent As EventHandler
	/// ' Create a FixQuality signifying that a differential GPS fix is obtained
	/// Dim MyFixQuality As FixQuality = FixQuality.DifferentialGpsFix 
	/// 
	/// Sub Main()
	///   ' Raise our custom event
	///   RaiseEvent MyFixQualityEvent(Me, New FixQualityEventArgs(MyFixQuality))
	/// End Sub
	/// </code>
	/// <code lang="C#">
	/// // Declare a new event
	/// EventHandler MyFixQualityEvent;
	/// // Create a FixQuality signifying that a differential GPS fix is obtained
	/// FixQuality MyFixQuality = FixQuality.DifferentialGpsFix;
	/// 
	/// void Main()
	/// {
	///   // Raise our custom event
	///   MyFixQualityEvent(this, New FixQualityEventArgs(MyFixQuality));
	/// }
	/// </code>
	/// </example>
	/// <seealso cref="Receiver.FixMethod">FixMethod Property (Receiver Class)</seealso>
	/// <seealso cref="Receiver">Receiver Class</seealso>
	public sealed class FixQualityEventArgs : EventArgs
	{
		private FixQuality _FixQuality;

		/// <summary>
		/// Creates a new instance with the specified fix quality measurement.
		/// </summary>
		/// <param name="fixQuality">A value from the <strong>FixQuality</strong> enumeration.</param>
		/// <remarks></remarks>
		public FixQualityEventArgs(FixQuality fixQuality)
		{
			_FixQuality = fixQuality;
		}

		/// <summary>
		/// Indicates whether the current fix involves satellites and/or DGPS/WAAS ground stations.
		/// </summary>
		/// <value>A value from the <strong>FixQuality</strong> enumeration.</value>
		/// <remarks></remarks>
		public FixQuality FixQuality
		{
			get
			{
				return _FixQuality;
			}
		}
	}

}

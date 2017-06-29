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
using System;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents information about the method used to obtain a fix when a fix-related event is raised.
    /// </summary>
    /// <example>This example demonstrates how to use this class when raising an event.
    ///   <code lang="VB">
    /// ' Declare a new event
    /// Dim MyFixMethodEvent As EventHandler
    /// ' Create a FixMethod signifying a 3-D fix (both position and altitude)
    /// Dim MyFixMethod As FixMethod = FixMethod.Fix3D
    /// Sub Main()
    /// ' Raise our custom event
    /// RaiseEvent MyFixMethodEvent(Me, New FixMethodEventArgs(MyFixMethod))
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Declare a new event
    /// EventHandler MyFixMethodEvent;
    /// // Create a FixMethod signifying a 3-D fix (both position and altitude)
    /// FixMethod MyFixMethod = FixMethod.Fix3D;
    /// void Main()
    /// {
    /// // Raise our custom event
    /// MyFixMethodEvent(this, New FixMethodEventArgs(MyFixMethod));
    /// }
    ///   </code>
    ///   </example>
    /// <remarks>This object is used primarily by the FixMethodChanged
    /// event of the Receiver class to provide notification when
    /// updated fix method information has been received from the GPS device.</remarks>
    public sealed class FixMethodEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly FixMethod _fixMethod;

        /// <summary>
        /// Creates a new instance with the specified fix method.
        /// </summary>
        /// <param name="fixMethod">The fix method.</param>
        public FixMethodEventArgs(FixMethod fixMethod)
        {
            _fixMethod = fixMethod;
        }

        /// <summary>
        /// Indicates if the GPS device has no fix, a 2-D fix, or a 3-D fix.
        /// </summary>
        /// <value>A value from the <strong>FixMethod</strong> enumeration.</value>
        /// <seealso cref="FixMethod">FixMethod Property (Receiver Class)</seealso>
        /// <remarks>A device is considered to have a "2-D" fix if there are three satellites
        /// involved in the fix.  A 2-D fix means that position information is available, but not
        /// altitude.  If at least four satellites are fixed, both position and altitude are known,
        /// and the fix is considered 3-D.</remarks>
        public FixMethod FixMethod
        {
            get
            {
                return _fixMethod;
            }
        }
    }

    /// <summary>
    /// Represents information about the likelihood that a fix will be sustained (or obtained) when a when a fix-related event is raised.
    /// </summary>
    /// <example>This example demonstrates how to use this class when raising an event.
    ///   <code lang="VB">
    /// ' Declare a new event
    /// Dim MyFixLikelihoodEvent As FixLikelihoodEventHandler
    /// ' Create a FixLikelihood signifying a 3-D fix (both position and altitude)
    /// Dim MyFixLikelihood As FixLikelihood = FixLikelihood.Fix3D
    /// Sub Main()
    /// ' Raise our custom event
    /// RaiseEvent MyFixLikelihoodEvent(Me, New FixLikelihoodEventArgs(MyFixLikelihood))
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Declare a new event
    /// FixLikelihoodEventHandler MyFixLikelihoodEvent;
    /// // Create a FixLikelihood signifying a 3-D fix (both position and altitude)
    /// FixLikelihood MyFixLikelihood = FixLikelihood.Fix3D;
    /// void Main()
    /// {
    /// // Raise our custom event
    /// MyFixLikelihoodEvent(this, New FixLikelihoodEventArgs(MyFixLikelihood));
    /// }
    ///   </code>
    ///   </example>
    /// <remarks>This object is used primarily by the FixLikelihoodChanged
    /// event of the Receiver class to provide notification when
    /// combined satellite radio signal strength has changed.</remarks>
    public sealed class FixLikelihoodEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly FixLikelihood _fixLikelihood;

        /// <summary>
        /// Creates a new instance with the specified fix Likelihood.
        /// </summary>
        /// <param name="fixLikelihood">The fix likelihood.</param>
        public FixLikelihoodEventArgs(FixLikelihood fixLikelihood)
        {
            _fixLikelihood = fixLikelihood;
        }

        /// <summary>
        /// Indicates if the GPS device has no fix, a 2-D fix, or a 3-D fix.
        /// </summary>
        /// <value>A value from the <strong>FixLikelihood</strong> enumeration.</value>
        /// <remarks>A device is considered to have a "2-D" fix if there are three satellites
        /// involved in the fix.  A 2-D fix means that position information is available, but not
        /// altitude.  If at least four satellites are fixed, both position and altitude are known,
        /// and the fix is considered 3-D.</remarks>
        public FixLikelihood FixLikelihood
        {
            get
            {
                return _fixLikelihood;
            }
        }
    }

    /// <summary>
    /// Represents information about whether the fix method is chosen automatically when a fix-related event is raised.
    /// </summary>
    /// <example>This example demonstrates how to use this class when raising an event.
    ///   <code lang="VB">
    /// ' Declare a new event
    /// Dim MyFixModeEvent As EventHandler
    /// ' Create a FixMode signifying that the fix method is being chosen automatically
    /// Dim MyFixMode As FixMode = FixMode.Automatic
    /// Sub Main()
    /// ' Raise our custom event
    /// RaiseEvent MyFixModeEvent(Me, New FixModeEventArgs(MyFixMode))
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Declare a new event
    /// EventHandler MyFixModeEvent;
    /// // Create a FixMode signifying that the fix method is being chosen automatically
    /// FixMode MyFixMode = FixMode.Automatic;
    /// void Main()
    /// {
    /// // Raise our custom event
    /// MyFixModeEvent(this, New FixModeEventArgs(MyFixMode));
    /// }
    ///   </code>
    ///   </example>
    /// <remarks>This object is used primarily by the FixModeChanged
    /// event of the Receiver class to provide notification when
    /// the receiver switches fix modes from <strong>Automatic</strong> to <strong>Manual</strong> or vice-versa.</remarks>
    public sealed class FixModeEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly FixMode _fixMode;

        /// <summary>
        /// Creates a new instance with the specified fix method.
        /// </summary>
        /// <param name="fixMode">The fix mode.</param>
        public FixModeEventArgs(FixMode fixMode)
        {
            _fixMode = fixMode;
        }

        /// <summary>
        /// Indicates if the GPS device is choosing the <see cref="FixMethod">fix method</see> automatically.
        /// </summary>
        /// <value>A value from the <strong>FixMode</strong> enumeration.</value>
        /// <seealso cref="FixMode">FixMode Property (Receiver Class)</seealso>
        /// <remarks>A vast majority of GPS devices calculate the fix mode automatically.  This is because
        /// the process of determining the fix mode is a simple process: if there are three fixed satellites,
        /// a 2-D fix is obtained; if there are four or more fixed satellites, a 3-D fix is in progress. Any
        /// less than three satellites means that no fix is possible.</remarks>
        public FixMode FixMode
        {
            get
            {
                return _fixMode;
            }
        }
    }

    /// <summary>
    /// Represents information about the type of fix obtained when fix-related event is raised.
    /// </summary>
    /// <example>This example demonstrates how to use this class when raising an event.
    ///   <code lang="VB">
    /// ' Declare a new event
    /// Dim MyFixQualityEvent As EventHandler
    /// ' Create a FixQuality signifying that a differential GPS fix is obtained
    /// Dim MyFixQuality As FixQuality = FixQuality.DifferentialGpsFix
    /// Sub Main()
    /// ' Raise our custom event
    /// RaiseEvent MyFixQualityEvent(Me, New FixQualityEventArgs(MyFixQuality))
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Declare a new event
    /// EventHandler MyFixQualityEvent;
    /// // Create a FixQuality signifying that a differential GPS fix is obtained
    /// FixQuality MyFixQuality = FixQuality.DifferentialGpsFix;
    /// void Main()
    /// {
    /// // Raise our custom event
    /// MyFixQualityEvent(this, New FixQualityEventArgs(MyFixQuality));
    /// }
    ///   </code>
    ///   </example>
    /// <remarks>This object is used primarily by the FixQualityChanged
    /// event of the Receiver class to provide notification when
    /// the receiver switches fix modes from <strong>Automatic</strong> to <strong>Manual</strong> or vice-versa.</remarks>
    public sealed class FixQualityEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly FixQuality _fixQuality;

        /// <summary>
        /// Creates a new instance with the specified fix quality measurement.
        /// </summary>
        /// <param name="fixQuality">The fix quality.</param>
        public FixQualityEventArgs(FixQuality fixQuality)
        {
            _fixQuality = fixQuality;
        }

        /// <summary>
        /// Indicates whether the current fix involves satellites and/or DGPS/WAAS ground stations.
        /// </summary>
        /// <value>A value from the <strong>FixQuality</strong> enumeration.</value>
        public FixQuality FixQuality
        {
            get
            {
                return _fixQuality;
            }
        }
    }
}
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
namespace DotSpatial.Positioning
{
    /// <summary>
    /// Indicates which devices are being used to obtain a fix, other than the GPS device
    /// itself.
    /// </summary>
    /// <example>
    ///   <code lang="VB">
    /// ' Declare a new receiver
    /// Private WithEvents MyReceiver As New Receiver()
    /// ' Raised when the fix quality has changed
    /// Private Sub OnFixQualityChanged(ByVal sender As Object, ByVal e As FixQualityEventArgs)
    /// ' What is the new fix quality°
    /// Select Case e.FixQuality
    /// Case FixQuality.NoFix
    /// ' No fix is obtained
    /// Debug.WriteLine("No fix is currently obtained.")
    /// Case FixQuality.GpsFix
    /// ' A fix is present
    /// Debug.WriteLine("A fix has been obtained using only GPS satellites.")
    /// Case FixQuality.DifferentialGpsFix
    /// ' A differential fix is present
    /// Debug.WriteLine("A fix has been obtained using GPS satellites and correction information from DGPS/WAAS ground stations.")
    /// Case FixQuality.Estimated
    /// ' A fix is being estimated (not an actual fix)
    /// Debug.WriteLine("A fix is currently being estimated. ")
    /// End Select
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Declare a new receiver
    /// Receiver MyReceiver = new Receiver();
    /// // Raised when the fix quality has changed
    /// void OnFixQualityChanged(Object sender, FixQualityEventArgs e)
    /// {
    /// // What is the new fix quality°
    /// switch (e.FixQuality)
    /// {
    /// case FixQuality.NoFix:
    /// // No fix is obtained
    /// Debug.WriteLine("No fix is currently obtained.");
    /// case FixQuality.GpsFix:
    /// // A fix is present
    /// Debug.WriteLine("A fix has been obtained using only GPS satellites.");
    /// case FixQuality.DifferentialGpsFix:
    /// // A differential fix is present
    /// Debug.WriteLine("A fix has been obtained using GPS satellites and correction information from DGPS/WAAS ground stations.");
    /// case FixQuality.Estimated:
    /// // A fix is being estimated (not an actual fix)
    /// Debug.WriteLine("A fix is currently being estimated. ");
    /// }
    /// }
    ///   </code>
    ///   </example>
    /// <remarks>This enumeration is typically used by the</remarks>
    public enum FixQuality
    {
        /// <summary>
        /// Not enough information is available to specify the current fix quality.
        /// </summary>
        Unknown,
        /// <summary>No fix is currently obtained.</summary>
        NoFix,
        /// <summary>A fix is currently obtained using GPS satellites only.</summary>
        GpsFix,
        /// <summary>A fix is obtained using both GPS satellites and DGPS/WAAS ground
        /// stations.  Position error is as low as 0.5-5 meters.</summary>
        DifferentialGpsFix,
        /// <summary>
        /// A PPS or pulse-per-second fix.  PPS signals very accurately indicate the start of a second.
        /// </summary>
        PulsePerSecond,
        /// <summary>
        /// Used for surveying.  A fix is obtained with the assistance of a reference station.  Position error is as low as 1-5 centimeters.
        /// </summary>
        FixedRealTimeKinematic,
        /// <summary>
        /// Used for surveying.  A fix is obtained with the assistance of a reference station.  Position error is as low as 20cm to 1 meter.
        /// </summary>
        FloatRealTimeKinematic,
        /// <summary>
        /// The fix is being estimated.
        /// </summary>
        Estimated,
        /// <summary>
        /// The fix is being input manually.
        /// </summary>
        ManualInput,
        /// <summary>
        /// The fix is being simulated.
        /// </summary>
        Simulated
    }

    /// <summary>
    /// Indicates whether a fix is present and if altitude can be calculated along with
    /// latitude and longitude.
    /// </summary>
    /// <example>
    /// This example demonstrates how to use the FixMethod enumeration to tell if altitude
    /// measurements can be made.  Notice: Some devices have built-in altimeters.  These devices
    /// can report altitude even when there is no fix present.  To support such devices, avoid
    /// using this property entirely and use any non-zero altitude measurement.
    ///   <code lang="VB">
    /// Private WithEvents MyReceiver As New Receiver()
    /// ' Raised whenever the fix method has changed
    /// Private Sub OnFixMethodChanged(ByVal sender As Object, ByVal e As FixMethodEventArgs)
    /// ' What is the new fix method°
    /// Select Case e.FixMethod
    /// Case FixMethod.NoFix
    /// ' We have neither position or altitude
    /// Debug.WriteLine("Your position is: Not Yet Available")
    /// Debug.WriteLine("Your altitude is: Not Yet Available")
    /// Case FixMethod.Fix2D
    /// ' We have a position but no altitude
    /// Debug.WriteLine("Your position is: " &amp; MyReceiver.Position.ToString)
    /// Debug.WriteLine("Your altitude is: Not Yet Available")
    /// Case FixMethod.Fix3D
    /// ' We have both position and altitude
    /// Debug.WriteLine("Your position is: " &amp; MyReceiver.Position.ToString)
    /// Debug.WriteLine("Your altitude is: " &amp; MyReceiver.Altitude.ToString)
    /// End Select
    /// End Sub
    ///   </code>
    ///   <code lang="C#">
    /// // Declare a new receiver
    /// Private WithEvents MyReceiver As New Receiver()
    /// // Raised whenever the fix method has changed
    /// void OnFixMethodChanged(Object sender, FixMethodEventArgs e)
    /// {
    /// // What is the new fix method°
    /// switch (e.FixMethod)
    /// {
    /// case FixMethod.NoFix:
    /// // We have neither position or altitude
    /// Debug.WriteLine("Your position is: Not Yet Available");
    /// Debug.WriteLine("Your altitude is: Not Yet Available");
    /// case FixMethod.Fix2D:
    /// // We have a position but no altitude
    /// Debug.WriteLine("Your position is: " &amp; MyReceiver.Position.ToString());
    /// Debug.WriteLine("Your altitude is: Not Yet Available");
    /// case FixMethod.Fix3D:
    /// // We have both position and altitude
    /// Debug.WriteLine("Your position is: " &amp; MyReceiver.Position.ToString());
    /// Debug.WriteLine("Your altitude is: " &amp; MyReceiver.Altitude.ToString());
    /// }
    /// }
    ///   </code>
    ///   </example>
    /// <remarks>This enumeration is used by the
    /// class to indicate if altitude measurements are possible. Altitude measurements are
    /// possible whenever there are four or more satellites involved in a fix.</remarks>
    public enum FixMethod
    {
        /// <summary>The GPS device does not have a fix on the current position.</summary>
        NoFix,
        /// <summary>The GPS device is reporting latitude and longitude.</summary>
        Fix2D,
        /// <summary>The GPS device is reporting latitude, longitude, and altitude.</summary>
        Fix3D,
        /// <summary>
        /// The fix method is not yet known.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Indicates whether a satellite fix is currently active.
    /// </summary>
    public enum FixStatus
    {
        /// <summary>
        /// The satellite fix is untested
        /// </summary>
        Unknown,
        /// <summary>
        /// The satellite fix is inactive
        /// </summary>
        NoFix,
        /// <summary>
        /// The satellite fix is active
        /// </summary>
        Fix,
    }

    /// <summary>
    /// Indicates the likelihood that a GPS satellite fix will be obtained or
    /// sustained.
    /// </summary>
    /// <example>
    /// This example uses the FixLikelihood enumeration to make a judgement call on the
    /// stability of the fix.
    ///   <code lang="VB">
    /// ' Declare a new receiver
    /// Private WithEvents MyReceiver As New Receiver()
    /// Sub Main()
    /// ' Start listening for messages
    /// MyReceiver.Start
    /// End Sub
    /// Sub OnFixLikelihoodChanged(ByVal sender As Object, ByVal e As FixLikelihoodEventArgs) Handles MyReceiver.FixLikelihoodChanged
    /// ' Do we have a fix currently°
    /// If MyReceiver.IsFixObtained Then
    /// ' Yes.  What's the likelihood that the fix will be sustained°
    /// Select Case MyReceiver.FixLikelihood
    /// Case FixLikelihood.Unlikely
    /// Debug.WriteLine("The current fix is about to be lost!")
    /// Case FixLikelihood.Possible
    /// Debug.WriteLine("The current fix is unstable.  Find a more open view of the sky soon.")
    /// Case FixLikelihood.Likely
    /// Debug.WriteLine("The current fix is nearly stable.")
    /// Case FixLikelihood.Certain
    /// Debug.WriteLine("The current fix is stable.")
    /// End Select
    /// Else
    /// ' No. What's the likelihood that a fix will be obtained°
    /// Select Case MyReceiver.FixLikelihood
    /// Case FixLikelihood.Unlikely
    /// Debug.WriteLine("A fix is not possible.  Find a more open view of the sky.")
    /// Case FixLikelihood.Possible
    /// Debug.WriteLine("A fix is possible, but satellite signals are still mostly obscured.")
    /// Case FixLikelihood.Likely
    /// Debug.WriteLine("A fix should occur within the next several seconds.")
    /// Case FixLikelihood.Certain
    /// Debug.WriteLine("A fix will occur in a few seconds.")
    /// End Select
    /// End If
    /// End Sub
    ///   </code>
    ///   <code lang="CS">
    /// ' Declare a new receiver
    /// Private Receiver MyReceiver = new Receiver();
    /// void Main()
    /// {
    /// // Start listening for messages
    /// MyReceiver.Start();
    /// }
    /// void OnFixLikelihoodChanged(Object sender, FixLikelihoodEventArgs e)
    /// {
    /// // Do we have a fix currently°
    /// if (MyReceiver.IsFixObtained)
    /// {
    /// // Yes.  What's the likelihood that the fix will be sustained°
    /// switch (MyReceiver.FixLikelihood)
    /// {
    /// case FixLikelihood.Unlikely:
    /// Debug.WriteLine("The current fix is about to be lost!");
    /// break;
    /// case FixLikelihood.Possible:
    /// Debug.WriteLine("The current fix is unstable.  Find a more open view of the sky soon.");
    /// break;
    /// case FixLikelihood.Likely:
    /// Debug.WriteLine("The current fix is nearly stable.");
    /// break;
    /// case FixLikelihood.Certain:
    /// Debug.WriteLine("The current fix is stable.");
    /// break;
    /// }
    /// }
    /// else
    /// {
    /// // No. What's the likelihood that a fix will be obtained°
    /// switch (MyReceiver.FixLikelihood)
    /// {
    /// case FixLikelihood.Unlikely:
    /// Debug.WriteLine("A fix is not possible.  Find a more open view of the sky.");
    /// break;
    /// case FixLikelihood.Possible:
    /// Debug.WriteLine("A fix is possible, but satellite signals are still mostly obscured.");
    /// break;
    /// case FixLikelihood.Likely:
    /// Debug.WriteLine("A fix should occur within the next several seconds.");
    /// break;
    /// case FixLikelihood.Certain:
    /// Debug.WriteLine("A fix will occur in a few seconds.");
    /// break;
    /// }
    /// }
    /// }
    ///   </code>
    ///   </example>
    public enum FixLikelihood
    {
        /// <summary>Indicates that a fix would probably be lost if a fix is acquired.</summary>
        /// <remarks>When this value is returned, nearly all of the available GPS satellite signals are being obscured by buildings, trees, or other solid objects.  The device should be moved into a more open view of the sky.</remarks>
        Unlikely = 40,
        /// <summary>
        /// Indicates that a fix would probably be lost after a short period of time if a fix
        /// is acquired.
        /// </summary>
        /// <remarks>When this value is returned, a few satellite signals are available, but the combined signals are too weak to maintain a fix for long.  The device should be moved into a more open view of the sky.</remarks>
        Possible = 80,
        /// <summary>
        /// Indicates that a fix would probably last for a longer period of time if a fix is
        /// acquired.
        /// </summary>
        /// <remarks>When this value is returned, at least three satellite signals are being received clearly.  If conditions stay the same or improve, a fix is imminent.</remarks>
        Likely = 105,
        /// <summary>
        /// Indicates that a fix is very likely to be sustained given the current satellite
        /// signal strengths.
        /// </summary>
        /// <remarks>When this value is returned, several satellite signals are being received and are strong enough to maintain a fix.</remarks>
        Certain = 135
    }

    /// <summary>
    /// Indicates if the GPS device is automatically deciding between a 2-D and a 3-D
    /// fix.
    /// </summary>
    /// <remarks>This enumeration is used by the
    /// class. A vast majority of GPS devices use a setting of Automatic because there is no
    /// complicated math behind figuring out if a 2-D fix or 3-D fix is present. If there are
    /// three satellites involved in a fix, only the latitude and longitude can be calculated,
    /// so the fix is 2-D. If more than three involved, the fix is 3-D.</remarks>
    public enum FixMode
    {
        /// <summary>Typical value.  The GPS device is automatically deciding between a two- and three-dimensional fix.</summary>
        Automatic,
        /// <summary>Rare value.  The user must specify whether a two- or three-dimensional fix is to be used.</summary>
        Manual,
        /// <summary>
        /// The fix mode is not yet known.
        /// </summary>
        Unknown
    }
}
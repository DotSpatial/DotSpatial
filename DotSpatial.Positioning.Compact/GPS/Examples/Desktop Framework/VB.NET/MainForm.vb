Imports DotSpatial.Positioning
Imports DotSpatial.Positioning.Gps
Imports DotSpatial.Positioning.Gps.IO
Imports DotSpatial.Positioning.Gps.Nmea
Imports System.Threading

Public Class MainForm

    ' In case no GPS device can be found, we can use a GPS device emulator.
    ' The VirtualDevice class encapsulates all GPS simulation features.

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Hook into GPS.NET's device detection events.  These events will report on
        ' any GPS devices which have been found, along with any problems encountered and reasons
        ' why a particular device could NOT be detected.
        AddHandler Devices.DeviceDetectionAttempted, AddressOf Devices_DeviceDetectionAttempted
        AddHandler Devices.DeviceDetectionAttemptFailed, AddressOf Devices_DeviceDetectionAttemptFailed
        AddHandler Devices.DeviceDetectionStarted, AddressOf Devices_DeviceDetectionStarted
        AddHandler Devices.DeviceDetectionCompleted, AddressOf Devices_DeviceDetectionCompleted
        AddHandler Devices.DeviceDetectionCanceled, AddressOf Devices_DeviceDetectionCanceled
        AddHandler Devices.DeviceDetected, AddressOf Devices_DeviceDetected

        ' Hook up event handlers for application-level and AppDomain-level exceptions so 
        ' they can be reported to the user
        AddHandler Application.ThreadException, AddressOf Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException
    End Sub

#Region "  GPS Device Detection Events  "

    Sub Devices_DeviceDetectionCanceled(ByVal sender As Object, ByVal e As EventArgs)
        BeginInvoke(New EventHandler(AddressOf DeviceDetectionCanceled), sender, e)
    End Sub

    Private Sub DeviceDetectionCanceled(ByVal sender As Object, ByVal e As EventArgs)
        statusLabel.Text = "Device detection canceled!"
        detectButton.Enabled = True
        cancelDetectButton.Enabled = False
    End Sub

    Sub Devices_DeviceDetected(ByVal sender As Object, ByVal e As DeviceEventArgs)
        BeginInvoke(New EventHandler(Of DeviceEventArgs)(AddressOf DeviceDetected), sender, e)
    End Sub

    Private Sub DeviceDetected(ByVal sender As Object, ByVal e As DeviceEventArgs)
        Dim item As ListViewItem
        For Each item In devicesListView.Items
            If Object.ReferenceEquals(item.Tag, e.Device) Then
                item.SubItems(1).Text = "GPS DETECTED"
                item.ImageIndex = 0
            End If
        Next
        devicesListView.Refresh()
    End Sub

    Sub Devices_DeviceDetectionCompleted(ByVal sender As Object, ByVal e As EventArgs)
        BeginInvoke(New EventHandler(AddressOf DeviceDetectionCompleted), sender, e)
    End Sub

    Private Sub DeviceDetectionCompleted(ByVal sender As Object, ByVal e As EventArgs)
        statusLabel.Text = "Device detection complete."
        detectButton.Enabled = True
        cancelDetectButton.Enabled = False
    End Sub

    Sub Devices_DeviceDetectionStarted(ByVal sender As Object, ByVal e As EventArgs)
        BeginInvoke(New EventHandler(AddressOf DeviceDetectionStarted), sender, e)
    End Sub

    Private Sub DeviceDetectionStarted(ByVal sender As Object, ByVal e As EventArgs)
        statusLabel.Text = "Detecting GPS devices..."
        devicesListView.Items.Clear()
        detectButton.Enabled = False
        cancelDetectButton.Enabled = True
    End Sub

    Sub Devices_DeviceDetectionAttemptFailed(ByVal sender As Object, ByVal e As DeviceDetectionExceptionEventArgs)
        BeginInvoke(New EventHandler(Of DeviceDetectionExceptionEventArgs)(AddressOf DeviceDetectionAttemptFailed), sender, e)
    End Sub

    Private Sub DeviceDetectionAttemptFailed(ByVal sender As Object, ByVal e As DeviceDetectionExceptionEventArgs)
        Dim item As ListViewItem
        For Each item In devicesListView.Items
            If Object.ReferenceEquals(item.Tag, e.Device) Then
                item.SubItems(1).Text = e.Exception.Message
                item.ToolTipText = e.Exception.Message
                item.ImageIndex = 1
            End If
        Next item
        devicesListView.Refresh()
    End Sub

    Sub Devices_DeviceDetectionAttempted(ByVal sender As Object, ByVal e As DeviceEventArgs)
        BeginInvoke(New EventHandler(Of DeviceEventArgs)(AddressOf DeviceDetectionAttempted), sender, e)
    End Sub

    Private Sub DeviceDetectionAttempted(ByVal sender As Object, ByVal e As DeviceEventArgs)
        undetectButton.Enabled = True

        For Each existingItem As ListViewItem In devicesListView.Items
            If Object.ReferenceEquals(existingItem.Tag, e.Device) Then
                existingItem.SubItems(1).Text = "Detecting..."
                Return
            End If
        Next

        Dim item As New ListViewItem
        item.Text = e.Device.Name
        item.ImageIndex = 2
        item.Tag = e.Device
        item.SubItems.Add(New ListViewItem.ListViewSubItem(item, "Detecting..."))
        devicesListView.Items.Add(item)
        devicesListView.Refresh()
    End Sub

#End Region

#Region "  NmeaInterpreter Events  "

    ' In GPS.NET 3.0, events may be raised from a thread other than the form's own thread.  As a result,
    ' we must use Invoke or BeginInvoke to "marshal" the code to the Form's thread.

    Private Sub nmeaInterpreter1_SpeedChanged(ByVal sender As Object, ByVal e As SpeedEventArgs) Handles nmeaInterpreter1.SpeedChanged
        BeginInvoke(New EventHandler(Of SpeedEventArgs)(AddressOf SpeedChanged), sender, e)
    End Sub

    Private Sub SpeedChanged(ByVal sender As Object, ByVal e As SpeedEventArgs)
        speedTextBox.Text = e.Speed.ToString()
        speedLabel.Text = speedTextBox.Text
    End Sub

    Private Sub nmeaInterpreter1_BearingChanged(ByVal sender As Object, ByVal e As AzimuthEventArgs) Handles nmeaInterpreter1.BearingChanged
        BeginInvoke(New EventHandler(Of AzimuthEventArgs)(AddressOf BearingChanged), sender, e)
    End Sub

    Private Sub BearingChanged(ByVal sender As Object, ByVal e As AzimuthEventArgs)
        bearingTextBox.Text = e.Azimuth.ToString()
        bearingLabel.Text = bearingTextBox.Text
    End Sub

    Private Sub nmeaInterpreter1_AltitudeChanged(ByVal sender As Object, ByVal e As DistanceEventArgs) Handles nmeaInterpreter1.AltitudeChanged
        BeginInvoke(New EventHandler(Of DistanceEventArgs)(AddressOf AltitudeChanged), sender, e)
    End Sub

    Private Sub AltitudeChanged(ByVal sender As Object, ByVal e As DistanceEventArgs)
        altitudeTextBox.Text = e.Distance.ToString()
        altitudeLabel.Text = altitudeTextBox.Text
    End Sub

    Private Sub nmeaInterpreter1_Paused(ByVal sender As Object, ByVal e As EventArgs) Handles nmeaInterpreter1.Paused
        BeginInvoke(New EventHandler(AddressOf Paused), sender, e)
    End Sub

    Private Sub Paused(ByVal sender As Object, ByVal e As EventArgs)
        pauseButton.Enabled = False
        resumeButton.Enabled = True
        statusLabel.Text = "Paused."
    End Sub

    Private Sub nmeaInterpreter1_Resumed(ByVal sender As Object, ByVal e As EventArgs) Handles nmeaInterpreter1.Resumed
        BeginInvoke(New EventHandler(AddressOf Resumed), sender, e)
    End Sub

    Private Sub Resumed(ByVal sender As Object, ByVal e As EventArgs)
        pauseButton.Enabled = True
        resumeButton.Enabled = False
    End Sub

    Private Sub nmeaInterpreter1_Starting(ByVal sender As Object, ByVal e As DeviceEventArgs) Handles nmeaInterpreter1.Starting
        BeginInvoke(New EventHandler(Of DeviceEventArgs)(AddressOf Starting), sender, e)
    End Sub

    Private Sub Starting(ByVal sender As Object, ByVal e As DeviceEventArgs)
        statusLabel.Text = "Connecting to " + e.Device.Name + "..."
    End Sub

    Private Sub nmeaInterpreter1_Started(ByVal sender As Object, ByVal e As EventArgs) Handles nmeaInterpreter1.Started
        BeginInvoke(New EventHandler(AddressOf Started), sender, e)
    End Sub

    Private Sub Started(ByVal sender As Object, ByVal e As EventArgs)
        statusLabel.Text = "Connected!  Waiting for data..."
        sentenceListBox.Items.Clear()
        startButton.Enabled = False
        stopButton.Enabled = True
        pauseButton.Enabled = True
        resumeButton.Enabled = False

        positionLabel.Text = Position.Empty.ToString()
        speedLabel.Text = Speed.Empty.ToString()
        bearingLabel.Text = Azimuth.Empty.ToString()
        altitudeLabel.Text = Distance.Empty.ToString()
    End Sub

    Private Sub nmeaInterpreter1_Stopping(ByVal sender As Object, ByVal e As EventArgs)
        BeginInvoke(New EventHandler(AddressOf Stopping), sender, e)
    End Sub

    Private Sub Stopping(ByVal sender As Object, ByVal e As EventArgs)
        statusLabel.Text = "Stopping GPS device..."
    End Sub

    Private Sub nmeaInterpreter1_Stopped(ByVal sender As Object, ByVal e As EventArgs)
        BeginInvoke(New EventHandler(AddressOf Stopped), sender, e)
    End Sub

    Private Sub Stopped(ByVal sender As Object, ByVal e As EventArgs)
        statusLabel.Text = "Stopped."
        startButton.Enabled = True
        stopButton.Enabled = False
        pauseButton.Enabled = False
        resumeButton.Enabled = False
    End Sub

    Private Sub nmeaInterpreter1_SatellitesChanged(ByVal sender As Object, ByVal e As SatelliteListEventArgs)
        BeginInvoke(New EventHandler(Of SatelliteListEventArgs)(AddressOf SatellitesChanged), sender, e)
    End Sub

    Private Sub SatellitesChanged(ByVal sender As Object, ByVal e As SatelliteListEventArgs)
        Dim satellite As Satellite

        ' Update each satellite
        For Each satellite In e.Satellites
            Dim isSatelliteNew As Boolean = True

            ' Look through the existing list for matches
            Dim viewItem As ListViewItem
            For Each viewItem In satellitesListView.Items
                ' Each item's Tag property houses a Satellite object
                Dim existing As Satellite = CType(viewItem.Tag, Satellite)

                ' Do the ID's match?
                If existing.PseudorandomNumber.Equals(satellite.PseudorandomNumber) Then
                    ' Yes. Update the listview columns
                    viewItem.SubItems(2).Text = satellite.Azimuth.ToString()
                    viewItem.SubItems(3).Text = satellite.Elevation.ToString()
                    viewItem.SubItems(4).Text = satellite.SignalToNoiseRatio.ToString()
                    isSatelliteNew = False
                End If
            Next

            ' If no existing satellite was found, then add a new one
            If isSatelliteNew Then
                ' Add a new column for this satellite
                Dim newItem As New ListViewItem(satellite.PseudorandomNumber.ToString())
                newItem.SubItems.Add(satellite.Name)
                newItem.SubItems.Add(satellite.Azimuth.ToString())
                newItem.SubItems.Add(satellite.Elevation.ToString())
                newItem.SubItems.Add(satellite.SignalToNoiseRatio.ToString())
                newItem.Tag = satellite
                satellitesListView.Items.Add(newItem)
            End If
        Next
    End Sub

    Private Sub nmeaInterpreter1_PositionChanged(ByVal sender As Object, ByVal e As PositionEventArgs) Handles nmeaInterpreter1.PositionChanged
        BeginInvoke(New EventHandler(Of PositionEventArgs)(AddressOf PositionChanged), sender, e)
    End Sub

    Private Sub PositionChanged(ByVal sender As Object, ByVal e As PositionEventArgs)
        positionTextBox.Text = e.Position.ToString()
        positionLabel.Text = positionTextBox.Text
    End Sub

    Private Sub nmeaInterpreter1_DateTimeChanged(ByVal sender As Object, ByVal e As DateTimeEventArgs) Handles nmeaInterpreter1.DateTimeChanged
        BeginInvoke(New EventHandler(Of DateTimeEventArgs)(AddressOf DateTimeChanged), sender, e)
    End Sub

    Private Sub DateTimeChanged(ByVal sender As Object, ByVal e As DateTimeEventArgs)
        dateTimeTextBox.Text = e.DateTime.ToShortDateString() + " " + e.DateTime.ToLongTimeString()
        utcDateTimeTextBox.Text = e.DateTime.ToUniversalTime().ToString("R")
    End Sub

    Private Sub nmeaInterpreter1_SentenceReceived(ByVal sender As Object, ByVal e As NmeaSentenceEventArgs) Handles nmeaInterpreter1.SentenceReceived
        BeginInvoke(New EventHandler(Of NmeaSentenceEventArgs)(AddressOf SentenceReceived), sender, e)
    End Sub

    Private Sub SentenceReceived(ByVal sender As Object, ByVal e As NmeaSentenceEventArgs)
        If sentenceListBox.Items.Count >= 100 Then
            sentenceListBox.Items.RemoveAt(0)
        End If

        sentenceListBox.Items.Add(e.Sentence.ToString())
        sentenceListBox.SelectedIndex = sentenceListBox.Items.Count - 1

        statusLabel.Text = "Receiving GPS data."
    End Sub

#End Region

#Region "  Button Events  "

    Private Sub detectButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles detectButton.Click
        Devices.BeginDetection()
    End Sub

    Private Sub cancelDetectButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cancelDetectButton.Click
        Devices.CancelDetection(True)
    End Sub

    Private Sub undetectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles undetectButton.Click
        Devices.Undetect()
        devicesListView.Items.Clear()
        undetectButton.Enabled = False
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        Try
            nmeaInterpreter1.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Cannot connect to GPS")
        End Try
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        nmeaInterpreter1.Stop()
    End Sub

    Private Sub pauseButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pauseButton.Click
        nmeaInterpreter1.Pause()
    End Sub

    Private Sub resumeButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles resumeButton.Click
        nmeaInterpreter1.Resume()
    End Sub

#End Region

#Region "  Context Menu Events  "

    Private Sub deviceContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles deviceContextMenu.Opening
        e.Cancel = devicesListView.SelectedItems.Count = 0
    End Sub

    Private Sub redetectMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles redetectMenuItem.Click
        Dim device As Device = DirectCast(devicesListView.SelectedItems(0).Tag, Device)
        device.Undetect()
        device.BeginDetection()
    End Sub

    Private Sub resetMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles resetMenuItem.Click
        Dim device As Device = DirectCast(devicesListView.SelectedItems(0).Tag, Device)
        device.Reset()
    End Sub

#End Region

#Region "  Other Form Control Events  "

    Private Sub devicesListView_ItemSelectionChanged(ByVal sender As Object, ByVal e As ListViewItemSelectionChangedEventArgs) Handles devicesListView.ItemSelectionChanged
        propertyGrid1.SelectedObject = e.Item.Tag
    End Sub

    Private Sub serialCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles serialCheckBox.CheckedChanged
        Devices.AllowSerialConnections = serialCheckBox.Checked
        exhaustiveCheckBox.Enabled = serialCheckBox.Checked
    End Sub

    Private Sub exhaustiveCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exhaustiveCheckBox.CheckedChanged
        Devices.AllowExhaustiveSerialPortScanning = exhaustiveCheckBox.Checked
    End Sub

    Private Sub bluetoothCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles bluetoothCheckBox.CheckedChanged
        Devices.AllowBluetoothConnections = bluetoothCheckBox.Checked
    End Sub

    Private Sub firstDeviceCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles firstDeviceCheckBox.CheckedChanged
        Devices.IsOnlyFirstDeviceDetected = firstDeviceCheckBox.Checked
    End Sub

    Private Sub clockSynchronizationCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clockSynchronizationCheckBox.CheckedChanged
        Devices.IsClockSynchronizationEnabled = clockSynchronizationCheckBox.Checked
    End Sub

#End Region

#Region "  Unhandled Exception Events  "

	Private Sub CurrentDomain_UnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
		Dim ex As Exception = DirectCast(e.ExceptionObject, Exception)
		NotifyOfUnhandledException(ex)
	End Sub

	Private Sub Application_ThreadException(ByVal sender As Object, ByVal e As ThreadExceptionEventArgs)
		NotifyOfUnhandledException(e.Exception)
	End Sub

	''' <summary>
	''' Logs an unhandled exception and displays a message box alerting the user to the error.
	''' </summary>
	''' <param name="exception">The unhandled exception.</param>
	Private Sub NotifyOfUnhandledException(ByVal exception As Exception)
		Try
			' Log the exception (and all of its inner exceptions)
			Dim innerException As Exception = exception
			While (Not (innerException) Is Nothing)
				Trace.TraceError(innerException.ToString)
				innerException = innerException.InnerException
			End While

			' Stop the interpreter
			nmeaInterpreter1.Stop()
		Finally
			' Display the error to the user
			MessageBox.Show( _
				"An unexpected error has occurred." & vbLf & vbLf & exception.GetType.ToString & ": " & exception.Message, _
				Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

#End Region
End Class

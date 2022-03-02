<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.startButton = New System.Windows.Forms.Button()
        Me.bearingTextBox = New System.Windows.Forms.TextBox()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.devicesListView = New System.Windows.Forms.ListView()
        Me.columnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.deviceContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.redetectMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.resetMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.propertyGrid1 = New System.Windows.Forms.PropertyGrid()
        Me.label5 = New System.Windows.Forms.Label()
        Me.devicesTab = New System.Windows.Forms.TabPage()
        Me.dataTab = New System.Windows.Forms.TabPage()
        Me.Speedometer2 = New DotSpatial.Positioning.Gps.Controls.Speedometer()
        Me.SatelliteViewer2 = New DotSpatial.Positioning.Gps.Controls.SatelliteViewer()
        Me.SatelliteSignalBar1 = New DotSpatial.Positioning.Gps.Controls.SatelliteSignalBar()
        Me.Compass2 = New DotSpatial.Positioning.Gps.Controls.Compass()
        Me.Clock1 = New DotSpatial.Positioning.Gps.Controls.Clock()
        Me.Altimeter2 = New DotSpatial.Positioning.Gps.Controls.Altimeter()
        Me.altitudeTextBox = New System.Windows.Forms.TextBox()
        Me.speedTextBox = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.dateTimeTextBox = New System.Windows.Forms.TextBox()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.positionTextBox = New System.Windows.Forms.TextBox()
        Me.utcDateTimeTextBox = New System.Windows.Forms.TextBox()
        Me.label6 = New System.Windows.Forms.Label()
        Me.rawDataTab = New System.Windows.Forms.TabPage()
        Me.sentenceListBox = New System.Windows.Forms.ListBox()
        Me.satellitesTab = New System.Windows.Forms.TabPage()
        Me.satellitesListView = New System.Windows.Forms.ListView()
        Me.columnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.bluetoothCheckBox = New System.Windows.Forms.CheckBox()
        Me.nmeaInterpreter1 = New DotSpatial.Positioning.Gps.Nmea.NmeaInterpreter()
        Me.serialCheckBox = New System.Windows.Forms.CheckBox()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.statusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.resumeButton = New System.Windows.Forms.Button()
        Me.pauseButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.positionLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.speedLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.bearingLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.altitudeLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.exhaustiveCheckBox = New System.Windows.Forms.CheckBox()
        Me.cancelDetectButton = New System.Windows.Forms.Button()
        Me.detectButton = New System.Windows.Forms.Button()
        Me.undetectButton = New System.Windows.Forms.Button()
        Me.firstDeviceCheckBox = New System.Windows.Forms.CheckBox()
        Me.clockSynchronizationCheckBox = New System.Windows.Forms.CheckBox()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.deviceContextMenu.SuspendLayout()
        Me.devicesTab.SuspendLayout()
        Me.dataTab.SuspendLayout()
        Me.rawDataTab.SuspendLayout()
        Me.satellitesTab.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.statusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.startButton.Location = New System.Drawing.Point(569, 142)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(98, 23)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'bearingTextBox
        '
        Me.bearingTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bearingTextBox.Location = New System.Drawing.Point(131, 156)
        Me.bearingTextBox.Name = "bearingTextBox"
        Me.bearingTextBox.Size = New System.Drawing.Size(403, 20)
        Me.bearingTextBox.TabIndex = 13
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.devicesListView)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.propertyGrid1)
        Me.splitContainer1.Size = New System.Drawing.Size(529, 381)
        Me.splitContainer1.SplitterDistance = 298
        Me.splitContainer1.TabIndex = 10
        '
        'devicesListView
        '
        Me.devicesListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeader1, Me.columnHeader2})
        Me.devicesListView.ContextMenuStrip = Me.deviceContextMenu
        Me.devicesListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.devicesListView.LargeImageList = Me.imageList1
        Me.devicesListView.Location = New System.Drawing.Point(0, 0)
        Me.devicesListView.Name = "devicesListView"
        Me.devicesListView.Size = New System.Drawing.Size(298, 381)
        Me.devicesListView.SmallImageList = Me.imageList1
        Me.devicesListView.TabIndex = 0
        Me.devicesListView.UseCompatibleStateImageBehavior = False
        Me.devicesListView.View = System.Windows.Forms.View.Details
        '
        'columnHeader1
        '
        Me.columnHeader1.Text = "Device Name"
        Me.columnHeader1.Width = 148
        '
        'columnHeader2
        '
        Me.columnHeader2.Text = "Status"
        Me.columnHeader2.Width = 641
        '
        'deviceContextMenu
        '
        Me.deviceContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.redetectMenuItem, Me.resetMenuItem})
        Me.deviceContextMenu.Name = "deviceContextMenu"
        Me.deviceContextMenu.Size = New System.Drawing.Size(121, 48)
        '
        'redetectMenuItem
        '
        Me.redetectMenuItem.Name = "redetectMenuItem"
        Me.redetectMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.redetectMenuItem.Text = "Redetect"
        '
        'resetMenuItem
        '
        Me.resetMenuItem.Name = "resetMenuItem"
        Me.resetMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.resetMenuItem.Text = "Reset"
        '
        'imageList1
        '
        Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.imageList1.Images.SetKeyName(0, "Gps.png")
        Me.imageList1.Images.SetKeyName(1, "GpsRemove.png")
        Me.imageList1.Images.SetKeyName(2, "Configuration Tools.png")
        '
        'propertyGrid1
        '
        Me.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.propertyGrid1.Location = New System.Drawing.Point(0, 0)
        Me.propertyGrid1.Name = "propertyGrid1"
        Me.propertyGrid1.Size = New System.Drawing.Size(227, 381)
        Me.propertyGrid1.TabIndex = 0
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(20, 159)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(46, 13)
        Me.label5.TabIndex = 12
        Me.label5.Text = "Bearing:"
        '
        'devicesTab
        '
        Me.devicesTab.Controls.Add(Me.splitContainer1)
        Me.devicesTab.Location = New System.Drawing.Point(4, 22)
        Me.devicesTab.Name = "devicesTab"
        Me.devicesTab.Padding = New System.Windows.Forms.Padding(3)
        Me.devicesTab.Size = New System.Drawing.Size(535, 387)
        Me.devicesTab.TabIndex = 0
        Me.devicesTab.Text = "Devices"
        Me.devicesTab.UseVisualStyleBackColor = True
        '
        'dataTab
        '
        Me.dataTab.Controls.Add(Me.Speedometer2)
        Me.dataTab.Controls.Add(Me.SatelliteViewer2)
        Me.dataTab.Controls.Add(Me.SatelliteSignalBar1)
        Me.dataTab.Controls.Add(Me.Compass2)
        Me.dataTab.Controls.Add(Me.Clock1)
        Me.dataTab.Controls.Add(Me.Altimeter2)
        Me.dataTab.Controls.Add(Me.bearingTextBox)
        Me.dataTab.Controls.Add(Me.label5)
        Me.dataTab.Controls.Add(Me.altitudeTextBox)
        Me.dataTab.Controls.Add(Me.speedTextBox)
        Me.dataTab.Controls.Add(Me.label3)
        Me.dataTab.Controls.Add(Me.label1)
        Me.dataTab.Controls.Add(Me.dateTimeTextBox)
        Me.dataTab.Controls.Add(Me.label4)
        Me.dataTab.Controls.Add(Me.label2)
        Me.dataTab.Controls.Add(Me.positionTextBox)
        Me.dataTab.Controls.Add(Me.utcDateTimeTextBox)
        Me.dataTab.Controls.Add(Me.label6)
        Me.dataTab.Location = New System.Drawing.Point(4, 22)
        Me.dataTab.Name = "dataTab"
        Me.dataTab.Padding = New System.Windows.Forms.Padding(3)
        Me.dataTab.Size = New System.Drawing.Size(535, 387)
        Me.dataTab.TabIndex = 1
        Me.dataTab.Text = "Real-Time Data"
        Me.dataTab.UseVisualStyleBackColor = True
        '
        'Speedometer2
        '
        Me.Speedometer2.CenterR = 0.0!
        Me.Speedometer2.Effect = DotSpatial.Positioning.Drawing.PolarControlEffect.Glass
        Me.Speedometer2.Height = 100
        Me.Speedometer2.IsPaintingOnSeparateThread = True
        Me.Speedometer2.IsUsingRealTimeData = True
        Me.Speedometer2.Location = New System.Drawing.Point(426, 239)
        Me.Speedometer2.MaximumR = 100.0!
        Me.Speedometer2.Name = "Speedometer2"
        Me.Speedometer2.Size = New System.Drawing.Size(100, 100)
        Me.Speedometer2.TabIndex = 19
        Me.Speedometer2.Text = "Speedometer2"
        Me.Speedometer2.Width = 100
        '
        'SatelliteViewer2
        '
        Me.SatelliteViewer2.CenterR = 0.0!
        Me.SatelliteViewer2.Effect = DotSpatial.Positioning.Drawing.PolarControlEffect.Glass
        Me.SatelliteViewer2.FixColor = System.Drawing.Color.LightGreen
        Me.SatelliteViewer2.Height = 100
        Me.SatelliteViewer2.IsPaintingOnSeparateThread = True
        Me.SatelliteViewer2.Location = New System.Drawing.Point(320, 239)
        Me.SatelliteViewer2.MaximumR = 90.0!
        Me.SatelliteViewer2.Name = "SatelliteViewer2"
        Me.SatelliteViewer2.Size = New System.Drawing.Size(100, 100)
        Me.SatelliteViewer2.TabIndex = 18
        Me.SatelliteViewer2.Text = "SatelliteViewer2"
        Me.SatelliteViewer2.Width = 100
        '
        'SatelliteSignalBar1
        '
        Me.SatelliteSignalBar1.BackColor = System.Drawing.Color.DarkGray
        Me.SatelliteSignalBar1.Height = 50
        Me.SatelliteSignalBar1.IsPaintingOnSeparateThread = True
        Me.SatelliteSignalBar1.Location = New System.Drawing.Point(320, 183)
        Me.SatelliteSignalBar1.Name = "SatelliteSignalBar1"
        Me.SatelliteSignalBar1.Size = New System.Drawing.Size(100, 50)
        Me.SatelliteSignalBar1.TabIndex = 17
        Me.SatelliteSignalBar1.Text = "SatelliteSignalBar1"
        Me.SatelliteSignalBar1.Width = 100
        '
        'Compass2
        '
        Me.Compass2.CenterR = 0.0!
        Me.Compass2.Effect = DotSpatial.Positioning.Drawing.PolarControlEffect.Glass
        Me.Compass2.Height = 100
        Me.Compass2.IsPaintingOnSeparateThread = True
        Me.Compass2.IsUsingRealTimeData = True
        Me.Compass2.Location = New System.Drawing.Point(214, 239)
        Me.Compass2.MaximumR = 100.0!
        Me.Compass2.Name = "Compass2"
        Me.Compass2.Size = New System.Drawing.Size(100, 100)
        Me.Compass2.TabIndex = 16
        Me.Compass2.Text = "Compass2"
        Me.Compass2.Width = 100
        '
        'Clock1
        '
        Me.Clock1.CenterR = 0.0!
        Me.Clock1.DisplayMode = DotSpatial.Positioning.Gps.Controls.ClockDisplayMode.SatelliteDerivedTime
        Me.Clock1.Effect = DotSpatial.Positioning.Drawing.PolarControlEffect.Glass
        Me.Clock1.Height = 100
        Me.Clock1.IsPaintingOnSeparateThread = True
        Me.Clock1.Location = New System.Drawing.Point(108, 239)
        Me.Clock1.MaximumR = 100.0!
        Me.Clock1.Name = "Clock1"
        Me.Clock1.Size = New System.Drawing.Size(100, 100)
        Me.Clock1.TabIndex = 15
        Me.Clock1.Text = "Clock1"
        Me.Clock1.UpdateInterval = System.TimeSpan.Parse("00:00:00.1000000")
        Me.Clock1.Value = New Date(1, 1, 1, 8, 0, 0, 0)
        Me.Clock1.ValueColor = System.Drawing.Color.Black
        Me.Clock1.Width = 100
        '
        'Altimeter2
        '
        Me.Altimeter2.CenterR = 0.0!
        Me.Altimeter2.Effect = DotSpatial.Positioning.Drawing.PolarControlEffect.Glass
        Me.Altimeter2.Height = 100
        Me.Altimeter2.IsPaintingOnSeparateThread = True
        Me.Altimeter2.IsUsingRealTimeData = True
        Me.Altimeter2.Location = New System.Drawing.Point(2, 239)
        Me.Altimeter2.MaximumR = 100.0!
        Me.Altimeter2.Name = "Altimeter2"
        Me.Altimeter2.Size = New System.Drawing.Size(100, 100)
        Me.Altimeter2.TabIndex = 14
        Me.Altimeter2.Text = "Altimeter2"
        Me.Altimeter2.Width = 100
        '
        'altitudeTextBox
        '
        Me.altitudeTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.altitudeTextBox.Location = New System.Drawing.Point(131, 102)
        Me.altitudeTextBox.Name = "altitudeTextBox"
        Me.altitudeTextBox.Size = New System.Drawing.Size(403, 20)
        Me.altitudeTextBox.TabIndex = 11
        '
        'speedTextBox
        '
        Me.speedTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.speedTextBox.Location = New System.Drawing.Point(131, 128)
        Me.speedTextBox.Name = "speedTextBox"
        Me.speedTextBox.Size = New System.Drawing.Size(403, 20)
        Me.speedTextBox.TabIndex = 7
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(20, 105)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(45, 13)
        Me.label3.TabIndex = 10
        Me.label3.Text = "Altitude:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(20, 19)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(61, 13)
        Me.label1.TabIndex = 0
        Me.label1.Text = "Date/Time:"
        '
        'dateTimeTextBox
        '
        Me.dateTimeTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dateTimeTextBox.Location = New System.Drawing.Point(131, 16)
        Me.dateTimeTextBox.Name = "dateTimeTextBox"
        Me.dateTimeTextBox.Size = New System.Drawing.Size(403, 20)
        Me.dateTimeTextBox.TabIndex = 1
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(20, 131)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(41, 13)
        Me.label4.TabIndex = 6
        Me.label4.Text = "Speed:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(20, 47)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(86, 13)
        Me.label2.TabIndex = 2
        Me.label2.Text = "UTC Date/Time:"
        '
        'positionTextBox
        '
        Me.positionTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.positionTextBox.Location = New System.Drawing.Point(131, 73)
        Me.positionTextBox.Name = "positionTextBox"
        Me.positionTextBox.Size = New System.Drawing.Size(403, 20)
        Me.positionTextBox.TabIndex = 5
        '
        'utcDateTimeTextBox
        '
        Me.utcDateTimeTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.utcDateTimeTextBox.Location = New System.Drawing.Point(131, 44)
        Me.utcDateTimeTextBox.Name = "utcDateTimeTextBox"
        Me.utcDateTimeTextBox.Size = New System.Drawing.Size(403, 20)
        Me.utcDateTimeTextBox.TabIndex = 3
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(20, 76)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(47, 13)
        Me.label6.TabIndex = 4
        Me.label6.Text = "Position:"
        '
        'rawDataTab
        '
        Me.rawDataTab.Controls.Add(Me.sentenceListBox)
        Me.rawDataTab.Location = New System.Drawing.Point(4, 22)
        Me.rawDataTab.Name = "rawDataTab"
        Me.rawDataTab.Size = New System.Drawing.Size(535, 387)
        Me.rawDataTab.TabIndex = 2
        Me.rawDataTab.Text = "Raw Data"
        Me.rawDataTab.UseVisualStyleBackColor = True
        '
        'sentenceListBox
        '
        Me.sentenceListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.sentenceListBox.FormattingEnabled = True
        Me.sentenceListBox.Location = New System.Drawing.Point(0, 0)
        Me.sentenceListBox.Name = "sentenceListBox"
        Me.sentenceListBox.Size = New System.Drawing.Size(539, 454)
        Me.sentenceListBox.TabIndex = 4
        '
        'satellitesTab
        '
        Me.satellitesTab.Controls.Add(Me.satellitesListView)
        Me.satellitesTab.Location = New System.Drawing.Point(4, 22)
        Me.satellitesTab.Name = "satellitesTab"
        Me.satellitesTab.Size = New System.Drawing.Size(535, 387)
        Me.satellitesTab.TabIndex = 3
        Me.satellitesTab.Text = "Satelllites"
        Me.satellitesTab.UseVisualStyleBackColor = True
        '
        'satellitesListView
        '
        Me.satellitesListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeader3, Me.columnHeader4, Me.columnHeader5, Me.columnHeader6, Me.columnHeader7})
        Me.satellitesListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.satellitesListView.Location = New System.Drawing.Point(0, 0)
        Me.satellitesListView.Name = "satellitesListView"
        Me.satellitesListView.Size = New System.Drawing.Size(539, 454)
        Me.satellitesListView.TabIndex = 0
        Me.satellitesListView.UseCompatibleStateImageBehavior = False
        Me.satellitesListView.View = System.Windows.Forms.View.Details
        '
        'columnHeader3
        '
        Me.columnHeader3.Text = "Satellite ID"
        Me.columnHeader3.Width = 74
        '
        'columnHeader4
        '
        Me.columnHeader4.Text = "Name"
        Me.columnHeader4.Width = 257
        '
        'columnHeader5
        '
        Me.columnHeader5.Text = "Azimuth"
        '
        'columnHeader6
        '
        Me.columnHeader6.Text = "Elevation"
        '
        'columnHeader7
        '
        Me.columnHeader7.Text = "Signal Strength"
        Me.columnHeader7.Width = 95
        '
        'bluetoothCheckBox
        '
        Me.bluetoothCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bluetoothCheckBox.Checked = True
        Me.bluetoothCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.bluetoothCheckBox.Location = New System.Drawing.Point(561, 317)
        Me.bluetoothCheckBox.Name = "bluetoothCheckBox"
        Me.bluetoothCheckBox.Size = New System.Drawing.Size(144, 17)
        Me.bluetoothCheckBox.TabIndex = 9
        Me.bluetoothCheckBox.Text = "Allow Bluetooth"
        Me.bluetoothCheckBox.UseVisualStyleBackColor = True
        '
        'nmeaInterpreter1
        '
        Me.nmeaInterpreter1.IsFilterEnabled = False
        '
        'serialCheckBox
        '
        Me.serialCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.serialCheckBox.Checked = True
        Me.serialCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.serialCheckBox.Location = New System.Drawing.Point(561, 271)
        Me.serialCheckBox.Name = "serialCheckBox"
        Me.serialCheckBox.Size = New System.Drawing.Size(144, 17)
        Me.serialCheckBox.TabIndex = 7
        Me.serialCheckBox.Text = "Allow Serial"
        Me.serialCheckBox.UseVisualStyleBackColor = True
        '
        'tabControl1
        '
        Me.tabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabControl1.Controls.Add(Me.devicesTab)
        Me.tabControl1.Controls.Add(Me.dataTab)
        Me.tabControl1.Controls.Add(Me.rawDataTab)
        Me.tabControl1.Controls.Add(Me.satellitesTab)
        Me.tabControl1.Location = New System.Drawing.Point(12, 6)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(543, 413)
        Me.tabControl1.TabIndex = 11
        '
        'statusLabel
        '
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(653, 17)
        Me.statusLabel.Spring = True
        Me.statusLabel.Text = "Idle."
        Me.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'resumeButton
        '
        Me.resumeButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.resumeButton.Enabled = False
        Me.resumeButton.Location = New System.Drawing.Point(569, 243)
        Me.resumeButton.Name = "resumeButton"
        Me.resumeButton.Size = New System.Drawing.Size(98, 23)
        Me.resumeButton.TabIndex = 3
        Me.resumeButton.Text = "Resume"
        Me.resumeButton.UseVisualStyleBackColor = True
        '
        'pauseButton
        '
        Me.pauseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pauseButton.Enabled = False
        Me.pauseButton.Location = New System.Drawing.Point(569, 213)
        Me.pauseButton.Name = "pauseButton"
        Me.pauseButton.Size = New System.Drawing.Size(98, 23)
        Me.pauseButton.TabIndex = 2
        Me.pauseButton.Text = "Pause"
        Me.pauseButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(569, 171)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(98, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'statusStrip1
        '
        Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statusLabel, Me.positionLabel, Me.speedLabel, Me.bearingLabel, Me.altitudeLabel})
        Me.statusStrip1.Location = New System.Drawing.Point(0, 439)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.ShowItemToolTips = True
        Me.statusStrip1.Size = New System.Drawing.Size(684, 22)
        Me.statusStrip1.TabIndex = 12
        Me.statusStrip1.Text = "statusStrip1"
        '
        'positionLabel
        '
        Me.positionLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.positionLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.positionLabel.Name = "positionLabel"
        Me.positionLabel.Size = New System.Drawing.Size(4, 17)
        Me.positionLabel.ToolTipText = "Current Position"
        '
        'speedLabel
        '
        Me.speedLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.speedLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.speedLabel.Name = "speedLabel"
        Me.speedLabel.Size = New System.Drawing.Size(4, 17)
        Me.speedLabel.ToolTipText = "Current Speed"
        '
        'bearingLabel
        '
        Me.bearingLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.bearingLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.bearingLabel.Name = "bearingLabel"
        Me.bearingLabel.Size = New System.Drawing.Size(4, 17)
        Me.bearingLabel.ToolTipText = "Current Bearing"
        '
        'altitudeLabel
        '
        Me.altitudeLabel.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.altitudeLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.altitudeLabel.Name = "altitudeLabel"
        Me.altitudeLabel.Size = New System.Drawing.Size(4, 17)
        Me.altitudeLabel.ToolTipText = "Current Altitude"
        '
        'exhaustiveCheckBox
        '
        Me.exhaustiveCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.exhaustiveCheckBox.Location = New System.Drawing.Point(561, 294)
        Me.exhaustiveCheckBox.Name = "exhaustiveCheckBox"
        Me.exhaustiveCheckBox.Size = New System.Drawing.Size(144, 17)
        Me.exhaustiveCheckBox.TabIndex = 8
        Me.exhaustiveCheckBox.Text = "Exhaustive Scan"
        Me.exhaustiveCheckBox.UseVisualStyleBackColor = True
        '
        'cancelDetectButton
        '
        Me.cancelDetectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cancelDetectButton.Enabled = False
        Me.cancelDetectButton.Location = New System.Drawing.Point(569, 57)
        Me.cancelDetectButton.Name = "cancelDetectButton"
        Me.cancelDetectButton.Size = New System.Drawing.Size(98, 23)
        Me.cancelDetectButton.TabIndex = 5
        Me.cancelDetectButton.Text = "Cancel"
        Me.cancelDetectButton.UseVisualStyleBackColor = True
        '
        'detectButton
        '
        Me.detectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.detectButton.Location = New System.Drawing.Point(569, 28)
        Me.detectButton.Name = "detectButton"
        Me.detectButton.Size = New System.Drawing.Size(98, 23)
        Me.detectButton.TabIndex = 4
        Me.detectButton.Text = "Detect"
        Me.detectButton.UseVisualStyleBackColor = True
        '
        'undetectButton
        '
        Me.undetectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.undetectButton.Enabled = False
        Me.undetectButton.Location = New System.Drawing.Point(569, 86)
        Me.undetectButton.Name = "undetectButton"
        Me.undetectButton.Size = New System.Drawing.Size(98, 23)
        Me.undetectButton.TabIndex = 6
        Me.undetectButton.Text = "Undetect"
        Me.undetectButton.UseVisualStyleBackColor = True
        '
        'firstDeviceCheckBox
        '
        Me.firstDeviceCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.firstDeviceCheckBox.Location = New System.Drawing.Point(561, 340)
        Me.firstDeviceCheckBox.Name = "firstDeviceCheckBox"
        Me.firstDeviceCheckBox.Size = New System.Drawing.Size(144, 17)
        Me.firstDeviceCheckBox.TabIndex = 10
        Me.firstDeviceCheckBox.Text = "Stop after first device"
        Me.firstDeviceCheckBox.UseVisualStyleBackColor = True
        '
        'clockSynchronizationCheckBox
        '
        Me.clockSynchronizationCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clockSynchronizationCheckBox.Location = New System.Drawing.Point(561, 363)
        Me.clockSynchronizationCheckBox.Name = "clockSynchronizationCheckBox"
        Me.clockSynchronizationCheckBox.Size = New System.Drawing.Size(144, 17)
        Me.clockSynchronizationCheckBox.TabIndex = 11
        Me.clockSynchronizationCheckBox.Text = "Sync system clock"
        Me.clockSynchronizationCheckBox.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 461)
        Me.Controls.Add(Me.undetectButton)
        Me.Controls.Add(Me.clockSynchronizationCheckBox)
        Me.Controls.Add(Me.firstDeviceCheckBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.bluetoothCheckBox)
        Me.Controls.Add(Me.serialCheckBox)
        Me.Controls.Add(Me.resumeButton)
        Me.Controls.Add(Me.pauseButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.cancelDetectButton)
        Me.Controls.Add(Me.exhaustiveCheckBox)
        Me.Controls.Add(Me.detectButton)
        Me.Name = "MainForm"
        Me.Text = "GPS.NET 3.0 Diagnostics"
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        Me.deviceContextMenu.ResumeLayout(False)
        Me.devicesTab.ResumeLayout(False)
        Me.dataTab.ResumeLayout(False)
        Me.dataTab.PerformLayout()
        Me.rawDataTab.ResumeLayout(False)
        Me.satellitesTab.ResumeLayout(False)
        Me.tabControl1.ResumeLayout(False)
        Me.statusStrip1.ResumeLayout(False)
        Me.statusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents speedometer1 As DotSpatial.Positioning.Gps.Controls.Speedometer
    Private WithEvents satelliteViewer1 As DotSpatial.Positioning.Gps.Controls.SatelliteViewer
    Private WithEvents altimeter1 As DotSpatial.Positioning.Gps.Controls.Altimeter
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents compass1 As DotSpatial.Positioning.Gps.Controls.Compass
    Private WithEvents bearingTextBox As System.Windows.Forms.TextBox
    Private WithEvents splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents devicesListView As System.Windows.Forms.ListView
    Private WithEvents columnHeader1 As System.Windows.Forms.ColumnHeader
    Private WithEvents columnHeader2 As System.Windows.Forms.ColumnHeader
    Private WithEvents imageList1 As System.Windows.Forms.ImageList
    Private WithEvents propertyGrid1 As System.Windows.Forms.PropertyGrid
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents devicesTab As System.Windows.Forms.TabPage
    Private WithEvents dataTab As System.Windows.Forms.TabPage
    Private WithEvents altitudeTextBox As System.Windows.Forms.TextBox
    Private WithEvents speedTextBox As System.Windows.Forms.TextBox
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents dateTimeTextBox As System.Windows.Forms.TextBox
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents positionTextBox As System.Windows.Forms.TextBox
    Private WithEvents utcDateTimeTextBox As System.Windows.Forms.TextBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents rawDataTab As System.Windows.Forms.TabPage
    Private WithEvents sentenceListBox As System.Windows.Forms.ListBox
    Private WithEvents satellitesTab As System.Windows.Forms.TabPage
    Private WithEvents satellitesListView As System.Windows.Forms.ListView
    Private WithEvents columnHeader3 As System.Windows.Forms.ColumnHeader
    Private WithEvents columnHeader4 As System.Windows.Forms.ColumnHeader
    Private WithEvents columnHeader5 As System.Windows.Forms.ColumnHeader
    Private WithEvents columnHeader6 As System.Windows.Forms.ColumnHeader
    Private WithEvents columnHeader7 As System.Windows.Forms.ColumnHeader
    Private WithEvents bluetoothCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents nmeaInterpreter1 As DotSpatial.Positioning.Gps.Nmea.NmeaInterpreter
	Private WithEvents serialCheckBox As System.Windows.Forms.CheckBox
	Private WithEvents tabControl1 As System.Windows.Forms.TabControl
	Private WithEvents statusLabel As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents resumeButton As System.Windows.Forms.Button
	Private WithEvents pauseButton As System.Windows.Forms.Button
	Private WithEvents stopButton As System.Windows.Forms.Button
	Private WithEvents statusStrip1 As System.Windows.Forms.StatusStrip
	Private WithEvents exhaustiveCheckBox As System.Windows.Forms.CheckBox
	Private WithEvents cancelDetectButton As System.Windows.Forms.Button
	Private WithEvents detectButton As System.Windows.Forms.Button
	Private WithEvents undetectButton As System.Windows.Forms.Button
	Private WithEvents deviceContextMenu As System.Windows.Forms.ContextMenuStrip
	Friend WithEvents redetectMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents resetMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents firstDeviceCheckBox As System.Windows.Forms.CheckBox
	Private WithEvents positionLabel As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents speedLabel As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents bearingLabel As System.Windows.Forms.ToolStripStatusLabel
	Private WithEvents altitudeLabel As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents clockSynchronizationCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Altimeter2 As DotSpatial.Positioning.Gps.Controls.Altimeter
    Friend WithEvents Speedometer2 As DotSpatial.Positioning.Gps.Controls.Speedometer
    Friend WithEvents SatelliteViewer2 As DotSpatial.Positioning.Gps.Controls.SatelliteViewer
    Friend WithEvents SatelliteSignalBar1 As DotSpatial.Positioning.Gps.Controls.SatelliteSignalBar
    Friend WithEvents Compass2 As DotSpatial.Positioning.Gps.Controls.Compass
    Friend WithEvents Clock1 As DotSpatial.Positioning.Gps.Controls.Clock

End Class

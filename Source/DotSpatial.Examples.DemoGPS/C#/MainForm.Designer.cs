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
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using DotSpatial.Positioning;
using DotSpatial.Positioning.Forms;

namespace DemoGPS
{
    /// <summary>
    /// The Main form for the project.
    /// </summary>
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            nmeaInterpreter1.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.resumeButton = new System.Windows.Forms.Button();
            this.sentenceListBox = new System.Windows.Forms.ListBox();
            this.altitudeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.speedTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.positionTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.utcDateTimeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.detectButton = new System.Windows.Forms.Button();
            this.cancelDetectButton = new System.Windows.Forms.Button();
            this.devicesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deviceContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.redetectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.positionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.speedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.bearingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.altitudeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.devicesTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.dataTab = new System.Windows.Forms.TabPage();
            this.speedometer1 = new DotSpatial.Positioning.Forms.Speedometer();
            this.satelliteSignalBar1 = new DotSpatial.Positioning.Forms.SatelliteSignalBar();
            this.satelliteViewer1 = new DotSpatial.Positioning.Forms.SatelliteViewer();
            this.compass1 = new DotSpatial.Positioning.Forms.Compass();
            this.clock1 = new DotSpatial.Positioning.Forms.Clock();
            this.altimeter1 = new DotSpatial.Positioning.Forms.Altimeter();
            this.bearingTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rawDataTab = new System.Windows.Forms.TabPage();
            this.satellitesTab = new System.Windows.Forms.TabPage();
            this.satellitesListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serialCheckBox = new System.Windows.Forms.CheckBox();
            this.bluetoothCheckBox = new System.Windows.Forms.CheckBox();
            this.nmeaInterpreter1 = new DotSpatial.Positioning.NmeaInterpreter();
            this.exhaustiveCheckBox = new System.Windows.Forms.CheckBox();
            this.undetectButton = new System.Windows.Forms.Button();
            this.firstDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this.clockSynchronizationCheckBox = new System.Windows.Forms.CheckBox();
            this.deviceContextMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.devicesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.dataTab.SuspendLayout();
            this.rawDataTab.SuspendLayout();
            this.satellitesTab.SuspendLayout();
            this.SuspendLayout();
            //
            // startButton
            //
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(589, 142);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(98, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // stopButton
            //
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(589, 171);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(98, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //
            // pauseButton
            //
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(589, 213);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(98, 23);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            //
            // resumeButton
            //
            this.resumeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resumeButton.Enabled = false;
            this.resumeButton.Location = new System.Drawing.Point(589, 243);
            this.resumeButton.Name = "resumeButton";
            this.resumeButton.Size = new System.Drawing.Size(98, 23);
            this.resumeButton.TabIndex = 3;
            this.resumeButton.Text = "Resume";
            this.resumeButton.UseVisualStyleBackColor = true;
            this.resumeButton.Click += new System.EventHandler(this.resumeButton_Click);
            //
            // sentenceListBox
            //
            this.sentenceListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sentenceListBox.FormattingEnabled = true;
            this.sentenceListBox.Location = new System.Drawing.Point(0, 0);
            this.sentenceListBox.Name = "sentenceListBox";
            this.sentenceListBox.Size = new System.Drawing.Size(548, 448);
            this.sentenceListBox.TabIndex = 4;
            //
            // altitudeTextBox
            //
            this.altitudeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.altitudeTextBox.Location = new System.Drawing.Point(131, 102);
            this.altitudeTextBox.Name = "altitudeTextBox";
            this.altitudeTextBox.Size = new System.Drawing.Size(373, 22);
            this.altitudeTextBox.TabIndex = 11;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Altitude:";
            //
            // speedTextBox
            //
            this.speedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.speedTextBox.Location = new System.Drawing.Point(131, 128);
            this.speedTextBox.Name = "speedTextBox";
            this.speedTextBox.Size = new System.Drawing.Size(373, 22);
            this.speedTextBox.TabIndex = 7;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Speed:";
            //
            // positionTextBox
            //
            this.positionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionTextBox.Location = new System.Drawing.Point(131, 73);
            this.positionTextBox.Name = "positionTextBox";
            this.positionTextBox.Size = new System.Drawing.Size(373, 22);
            this.positionTextBox.TabIndex = 5;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Position:";
            //
            // utcDateTimeTextBox
            //
            this.utcDateTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.utcDateTimeTextBox.Location = new System.Drawing.Point(131, 44);
            this.utcDateTimeTextBox.Name = "utcDateTimeTextBox";
            this.utcDateTimeTextBox.Size = new System.Drawing.Size(373, 22);
            this.utcDateTimeTextBox.TabIndex = 3;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "UTC Date/Time:";
            //
            // dateTimeTextBox
            //
            this.dateTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeTextBox.Location = new System.Drawing.Point(131, 16);
            this.dateTimeTextBox.Name = "dateTimeTextBox";
            this.dateTimeTextBox.Size = new System.Drawing.Size(373, 22);
            this.dateTimeTextBox.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date/Time:";
            //
            // detectButton
            //
            this.detectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.detectButton.Location = new System.Drawing.Point(589, 34);
            this.detectButton.Name = "detectButton";
            this.detectButton.Size = new System.Drawing.Size(98, 23);
            this.detectButton.TabIndex = 4;
            this.detectButton.Text = "Detect";
            this.detectButton.UseVisualStyleBackColor = true;
            this.detectButton.Click += new System.EventHandler(DetectButton_Click);
            //
            // cancelDetectButton
            //
            this.cancelDetectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelDetectButton.Enabled = false;
            this.cancelDetectButton.Location = new System.Drawing.Point(589, 63);
            this.cancelDetectButton.Name = "cancelDetectButton";
            this.cancelDetectButton.Size = new System.Drawing.Size(98, 23);
            this.cancelDetectButton.TabIndex = 5;
            this.cancelDetectButton.Text = "Cancel";
            this.cancelDetectButton.UseVisualStyleBackColor = true;
            this.cancelDetectButton.Click += new System.EventHandler(CancelDetectButton_Click);
            //
            // devicesListView
            //
            this.devicesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.devicesListView.ContextMenuStrip = this.deviceContextMenu;
            this.devicesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.devicesListView.Location = new System.Drawing.Point(0, 0);
            this.devicesListView.Name = "devicesListView";
            this.devicesListView.Size = new System.Drawing.Size(305, 442);
            this.devicesListView.TabIndex = 0;
            this.devicesListView.UseCompatibleStateImageBehavior = false;
            this.devicesListView.View = System.Windows.Forms.View.Details;
            this.devicesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.devicesListView_ItemSelectionChanged);
            //
            // columnHeader1
            //
            this.columnHeader1.Text = "Device Name";
            this.columnHeader1.Width = 148;
            //
            // columnHeader2
            //
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 641;
            //
            // deviceContextMenu
            //
            this.deviceContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redetectMenuItem,
            this.resetMenuItem});
            this.deviceContextMenu.Name = "deviceContextMenu";
            this.deviceContextMenu.Size = new System.Drawing.Size(121, 48);
            this.deviceContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.deviceContextMenu_Opening);
            //
            // redetectMenuItem
            //
            this.redetectMenuItem.Name = "redetectMenuItem";
            this.redetectMenuItem.Size = new System.Drawing.Size(120, 22);
            this.redetectMenuItem.Text = "Redetect";
            this.redetectMenuItem.Click += new System.EventHandler(this.redetectMenuItem_Click);
            //
            // resetMenuItem
            //
            this.resetMenuItem.Name = "resetMenuItem";
            this.resetMenuItem.Size = new System.Drawing.Size(120, 22);
            this.resetMenuItem.Text = "Reset";
            this.resetMenuItem.Click += new System.EventHandler(this.resetMenuItem_Click);
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.positionLabel,
            this.speedLabel,
            this.bearingLabel,
            this.altitudeLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 500);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(709, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            //
            // statusLabel
            //
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(678, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Idle.";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // positionLabel
            //
            this.positionLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.positionLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(4, 17);
            this.positionLabel.ToolTipText = "Current Position";
            //
            // speedLabel
            //
            this.speedLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.speedLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(4, 17);
            this.speedLabel.ToolTipText = "Current Speed";
            //
            // bearingLabel
            //
            this.bearingLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.bearingLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.bearingLabel.Name = "bearingLabel";
            this.bearingLabel.Size = new System.Drawing.Size(4, 17);
            this.bearingLabel.ToolTipText = "Current Bearing";
            //
            // altitudeLabel
            //
            this.altitudeLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.altitudeLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.altitudeLabel.Name = "altitudeLabel";
            this.altitudeLabel.Size = new System.Drawing.Size(4, 17);
            this.altitudeLabel.ToolTipText = "Current Altitude";
            //
            // tabControl1
            //
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.devicesTab);
            this.tabControl1.Controls.Add(this.dataTab);
            this.tabControl1.Controls.Add(this.rawDataTab);
            this.tabControl1.Controls.Add(this.satellitesTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(556, 474);
            this.tabControl1.TabIndex = 11;
            //
            // devicesTab
            //
            this.devicesTab.Controls.Add(this.splitContainer1);
            this.devicesTab.Location = new System.Drawing.Point(4, 22);
            this.devicesTab.Name = "devicesTab";
            this.devicesTab.Padding = new System.Windows.Forms.Padding(3);
            this.devicesTab.Size = new System.Drawing.Size(548, 448);
            this.devicesTab.TabIndex = 0;
            this.devicesTab.Text = "Devices";
            this.devicesTab.UseVisualStyleBackColor = true;
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.devicesListView);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(542, 442);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 10;
            //
            // propertyGrid1
            //
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(233, 442);
            this.propertyGrid1.TabIndex = 0;
            //
            // dataTab
            //
            this.dataTab.Controls.Add(this.speedometer1);
            this.dataTab.Controls.Add(this.satelliteSignalBar1);
            this.dataTab.Controls.Add(this.satelliteViewer1);
            this.dataTab.Controls.Add(this.compass1);
            this.dataTab.Controls.Add(this.clock1);
            this.dataTab.Controls.Add(this.altimeter1);
            this.dataTab.Controls.Add(this.bearingTextBox);
            this.dataTab.Controls.Add(this.label5);
            this.dataTab.Controls.Add(this.altitudeTextBox);
            this.dataTab.Controls.Add(this.speedTextBox);
            this.dataTab.Controls.Add(this.label3);
            this.dataTab.Controls.Add(this.label1);
            this.dataTab.Controls.Add(this.dateTimeTextBox);
            this.dataTab.Controls.Add(this.label4);
            this.dataTab.Controls.Add(this.label2);
            this.dataTab.Controls.Add(this.positionTextBox);
            this.dataTab.Controls.Add(this.utcDateTimeTextBox);
            this.dataTab.Controls.Add(this.label6);
            this.dataTab.Location = new System.Drawing.Point(4, 22);
            this.dataTab.Name = "dataTab";
            this.dataTab.Padding = new System.Windows.Forms.Padding(3);
            this.dataTab.Size = new System.Drawing.Size(548, 448);
            this.dataTab.TabIndex = 1;
            this.dataTab.Text = "Real-Time Data";
            this.dataTab.UseVisualStyleBackColor = true;
            //
            // speedometer1
            //
            this.speedometer1.CenterR = 0F;
            this.speedometer1.Effect = DotSpatial.Positioning.Forms.PolarControlEffect.Glass;
            this.speedometer1.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.speedometer1.Height = 100;
            this.speedometer1.IsPaintingOnSeparateThread = true;
            this.speedometer1.IsUsingRealTimeData = true;
            this.speedometer1.Location = new System.Drawing.Point(428, 242);
            this.speedometer1.MaximumR = 100F;
            this.speedometer1.Name = "speedometer1";
            this.speedometer1.Size = new System.Drawing.Size(100, 100);
            this.speedometer1.TabIndex = 19;
            this.speedometer1.Text = "speedometer1";
            this.speedometer1.Width = 100;
            //
            // satelliteSignalBar1
            //
            this.satelliteSignalBar1.BackColor = System.Drawing.Color.DarkGray;
            this.satelliteSignalBar1.Height = 45;
            this.satelliteSignalBar1.IsPaintingOnSeparateThread = true;
            this.satelliteSignalBar1.Location = new System.Drawing.Point(322, 191);
            this.satelliteSignalBar1.Name = "satelliteSignalBar1";
            this.satelliteSignalBar1.Size = new System.Drawing.Size(100, 45);
            this.satelliteSignalBar1.TabIndex = 18;
            this.satelliteSignalBar1.Text = "satelliteSignalBar1";
            this.satelliteSignalBar1.Width = 100;
            //
            // satelliteViewer1
            //
            this.satelliteViewer1.CenterR = 0F;
            this.satelliteViewer1.Effect = DotSpatial.Positioning.Forms.PolarControlEffect.Glass;
            this.satelliteViewer1.FixColor = System.Drawing.Color.LightGreen;
            this.satelliteViewer1.Height = 100;
            this.satelliteViewer1.IsPaintingOnSeparateThread = true;
            this.satelliteViewer1.Location = new System.Drawing.Point(322, 242);
            this.satelliteViewer1.MaximumR = 90F;
            this.satelliteViewer1.Name = "satelliteViewer1";
            this.satelliteViewer1.Size = new System.Drawing.Size(100, 100);
            this.satelliteViewer1.TabIndex = 17;
            this.satelliteViewer1.Text = "satelliteViewer1";
            this.satelliteViewer1.Width = 100;
            //
            // compass1
            //
            this.compass1.CenterR = 0F;
            this.compass1.Effect = DotSpatial.Positioning.Forms.PolarControlEffect.Glass;
            this.compass1.Height = 100;
            this.compass1.IsPaintingOnSeparateThread = true;
            this.compass1.IsUsingRealTimeData = true;
            this.compass1.Location = new System.Drawing.Point(216, 242);
            this.compass1.MaximumR = 100F;
            this.compass1.Name = "compass1";
            this.compass1.Size = new System.Drawing.Size(100, 100);
            this.compass1.TabIndex = 16;
            this.compass1.Text = "compass1";
            this.compass1.Width = 100;
            //
            // clock1
            //
            this.clock1.CenterR = 0F;
            this.clock1.DisplayMode = DotSpatial.Positioning.Forms.ClockDisplayMode.SatelliteDerivedTime;
            this.clock1.Effect = DotSpatial.Positioning.Forms.PolarControlEffect.Glass;
            this.clock1.Height = 100;
            this.clock1.IsPaintingOnSeparateThread = true;
            this.clock1.Location = new System.Drawing.Point(110, 242);
            this.clock1.MaximumR = 100F;
            this.clock1.Name = "clock1";
            this.clock1.Size = new System.Drawing.Size(100, 100);
            this.clock1.TabIndex = 15;
            this.clock1.Text = "clock1";
            this.clock1.UpdateInterval = System.TimeSpan.Parse("00:00:00.1000000");
            this.clock1.Value = new System.DateTime(1, 1, 1, 8, 0, 0, 0);
            this.clock1.ValueColor = System.Drawing.Color.Black;
            this.clock1.Width = 100;
            //
            // altimeter1
            //
            this.altimeter1.CenterR = 0F;
            this.altimeter1.Effect = DotSpatial.Positioning.Forms.PolarControlEffect.Glass;
            this.altimeter1.Height = 100;
            this.altimeter1.IsPaintingOnSeparateThread = true;
            this.altimeter1.IsUsingRealTimeData = true;
            this.altimeter1.Location = new System.Drawing.Point(4, 239);
            this.altimeter1.MaximumR = 100F;
            this.altimeter1.Name = "altimeter1";
            this.altimeter1.Size = new System.Drawing.Size(100, 100);
            this.altimeter1.TabIndex = 14;
            this.altimeter1.Text = "altimeter1";
            this.altimeter1.Width = 100;
            //
            // bearingTextBox
            //
            this.bearingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bearingTextBox.Location = new System.Drawing.Point(131, 156);
            this.bearingTextBox.Name = "bearingTextBox";
            this.bearingTextBox.Size = new System.Drawing.Size(373, 22);
            this.bearingTextBox.TabIndex = 13;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Bearing:";
            //
            // rawDataTab
            //
            this.rawDataTab.Controls.Add(this.sentenceListBox);
            this.rawDataTab.Location = new System.Drawing.Point(4, 22);
            this.rawDataTab.Name = "rawDataTab";
            this.rawDataTab.Size = new System.Drawing.Size(548, 448);
            this.rawDataTab.TabIndex = 2;
            this.rawDataTab.Text = "Raw Data";
            this.rawDataTab.UseVisualStyleBackColor = true;
            //
            // satellitesTab
            //
            this.satellitesTab.Controls.Add(this.satellitesListView);
            this.satellitesTab.Location = new System.Drawing.Point(4, 22);
            this.satellitesTab.Name = "satellitesTab";
            this.satellitesTab.Size = new System.Drawing.Size(548, 448);
            this.satellitesTab.TabIndex = 3;
            this.satellitesTab.Text = "Satelllites";
            this.satellitesTab.UseVisualStyleBackColor = true;
            //
            // satellitesListView
            //
            this.satellitesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.satellitesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.satellitesListView.Location = new System.Drawing.Point(0, 0);
            this.satellitesListView.Name = "satellitesListView";
            this.satellitesListView.Size = new System.Drawing.Size(548, 448);
            this.satellitesListView.TabIndex = 0;
            this.satellitesListView.UseCompatibleStateImageBehavior = false;
            this.satellitesListView.View = System.Windows.Forms.View.Details;
            //
            // columnHeader3
            //
            this.columnHeader3.Text = "Satellite ID";
            this.columnHeader3.Width = 74;
            //
            // columnHeader4
            //
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 257;
            //
            // columnHeader5
            //
            this.columnHeader5.Text = "Azimuth";
            //
            // columnHeader6
            //
            this.columnHeader6.Text = "Elevation";
            //
            // columnHeader7
            //
            this.columnHeader7.Text = "Signal Strength";
            this.columnHeader7.Width = 95;
            //
            // serialCheckBox
            //
            this.serialCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.serialCheckBox.Checked = true;
            this.serialCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.serialCheckBox.Location = new System.Drawing.Point(574, 336);
            this.serialCheckBox.Name = "serialCheckBox";
            this.serialCheckBox.Size = new System.Drawing.Size(151, 17);
            this.serialCheckBox.TabIndex = 7;
            this.serialCheckBox.Text = "Allow Serial";
            this.serialCheckBox.UseVisualStyleBackColor = true;
            this.serialCheckBox.CheckedChanged += new System.EventHandler(this.serialCheckBox_CheckedChanged);
            //
            // bluetoothCheckBox
            //
            this.bluetoothCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bluetoothCheckBox.Checked = true;
            this.bluetoothCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bluetoothCheckBox.Location = new System.Drawing.Point(574, 382);
            this.bluetoothCheckBox.Name = "bluetoothCheckBox";
            this.bluetoothCheckBox.Size = new System.Drawing.Size(151, 17);
            this.bluetoothCheckBox.TabIndex = 9;
            this.bluetoothCheckBox.Text = "Allow Bluetooth";
            this.bluetoothCheckBox.UseVisualStyleBackColor = true;
            this.bluetoothCheckBox.CheckedChanged += new System.EventHandler(this.bluetoothCheckBox_CheckedChanged);
            //
            // nmeaInterpreter1
            //
            this.nmeaInterpreter1.IsFilterEnabled = false;
            this.nmeaInterpreter1.AltitudeChanged += new System.EventHandler<DotSpatial.Positioning.DistanceEventArgs>(this.nmeaInterpreter1_AltitudeChanged);
            this.nmeaInterpreter1.BearingChanged += new System.EventHandler<DotSpatial.Positioning.AzimuthEventArgs>(this.nmeaInterpreter1_BearingChanged);
            this.nmeaInterpreter1.DateTimeChanged += new System.EventHandler<DotSpatial.Positioning.DateTimeEventArgs>(this.nmeaInterpreter1_DateTimeChanged);
            this.nmeaInterpreter1.PositionChanged += new System.EventHandler<DotSpatial.Positioning.PositionEventArgs>(this.nmeaInterpreter1_PositionChanged);
            this.nmeaInterpreter1.SpeedChanged += new System.EventHandler<DotSpatial.Positioning.SpeedEventArgs>(this.nmeaInterpreter1_SpeedChanged);
            this.nmeaInterpreter1.SatellitesChanged += new System.EventHandler<DotSpatial.Positioning.SatelliteListEventArgs>(this.nmeaInterpreter1_SatellitesChanged);
            this.nmeaInterpreter1.Starting += new System.EventHandler<DotSpatial.Positioning.DeviceEventArgs>(this.nmeaInterpreter1_Starting);
            this.nmeaInterpreter1.Started += new System.EventHandler(this.nmeaInterpreter1_Started);
            this.nmeaInterpreter1.Stopping += new System.EventHandler(this.nmeaInterpreter1_Stopping);
            this.nmeaInterpreter1.Stopped += new System.EventHandler(this.nmeaInterpreter1_Stopped);
            this.nmeaInterpreter1.Paused += new System.EventHandler(this.nmeaInterpreter1_Paused);
            this.nmeaInterpreter1.Resumed += new System.EventHandler(this.nmeaInterpreter1_Resumed);
            //
            // exhaustiveCheckBox
            //
            this.exhaustiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exhaustiveCheckBox.Location = new System.Drawing.Point(574, 359);
            this.exhaustiveCheckBox.Name = "exhaustiveCheckBox";
            this.exhaustiveCheckBox.Size = new System.Drawing.Size(151, 17);
            this.exhaustiveCheckBox.TabIndex = 8;
            this.exhaustiveCheckBox.Text = "Exhaustive Scan";
            this.exhaustiveCheckBox.UseVisualStyleBackColor = true;
            this.exhaustiveCheckBox.CheckedChanged += new System.EventHandler(this.exhaustiveCheckBox_CheckedChanged);
            //
            // undetectButton
            //
            this.undetectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.undetectButton.Enabled = false;
            this.undetectButton.Location = new System.Drawing.Point(589, 92);
            this.undetectButton.Name = "undetectButton";
            this.undetectButton.Size = new System.Drawing.Size(98, 23);
            this.undetectButton.TabIndex = 6;
            this.undetectButton.Text = "Undetect";
            this.undetectButton.UseVisualStyleBackColor = true;
            this.undetectButton.Click += new System.EventHandler(this.undetectButton_Click);
            //
            // firstDeviceCheckBox
            //
            this.firstDeviceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.firstDeviceCheckBox.Location = new System.Drawing.Point(574, 405);
            this.firstDeviceCheckBox.Name = "firstDeviceCheckBox";
            this.firstDeviceCheckBox.Size = new System.Drawing.Size(151, 17);
            this.firstDeviceCheckBox.TabIndex = 10;
            this.firstDeviceCheckBox.Text = "Stop after first device";
            this.firstDeviceCheckBox.UseVisualStyleBackColor = true;
            this.firstDeviceCheckBox.CheckedChanged += new System.EventHandler(this.firstDeviceCheckBox_CheckedChanged);
            //
            // clockSynchronizationCheckBox
            //
            this.clockSynchronizationCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clockSynchronizationCheckBox.Location = new System.Drawing.Point(574, 462);
            this.clockSynchronizationCheckBox.Name = "clockSynchronizationCheckBox";
            this.clockSynchronizationCheckBox.Size = new System.Drawing.Size(151, 17);
            this.clockSynchronizationCheckBox.TabIndex = 11;
            this.clockSynchronizationCheckBox.Text = "Sync system clock";
            this.clockSynchronizationCheckBox.UseVisualStyleBackColor = true;
            this.clockSynchronizationCheckBox.CheckedChanged += new System.EventHandler(this.clockSynchronizationCheckBox_CheckedChanged);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 522);
            this.Controls.Add(this.clockSynchronizationCheckBox);
            this.Controls.Add(this.undetectButton);
            this.Controls.Add(this.firstDeviceCheckBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.serialCheckBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.exhaustiveCheckBox);
            this.Controls.Add(this.cancelDetectButton);
            this.Controls.Add(this.bluetoothCheckBox);
            this.Controls.Add(this.detectButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.resumeButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.stopButton);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "GPS.NET 3.0 Diagnostics";
            this.deviceContextMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.devicesTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.dataTab.ResumeLayout(false);
            this.dataTab.PerformLayout();
            this.rawDataTab.ResumeLayout(false);
            this.satellitesTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private NmeaInterpreter nmeaInterpreter1;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button resumeButton;
        private System.Windows.Forms.ListBox sentenceListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox altitudeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox speedTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox positionTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox utcDateTimeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox dateTimeTextBox;
        private System.Windows.Forms.Button detectButton;
        private System.Windows.Forms.Button cancelDetectButton;
        private System.Windows.Forms.ListView devicesListView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage devicesTab;
        private System.Windows.Forms.TabPage dataTab;
        private System.Windows.Forms.TabPage rawDataTab;
        private System.Windows.Forms.TabPage satellitesTab;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.CheckBox serialCheckBox;
        private System.Windows.Forms.CheckBox bluetoothCheckBox;
        private System.Windows.Forms.TextBox bearingTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView satellitesListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ContextMenuStrip deviceContextMenu;
        private System.Windows.Forms.ToolStripMenuItem redetectMenuItem;
        private System.Windows.Forms.CheckBox exhaustiveCheckBox;
        private System.Windows.Forms.ToolStripMenuItem resetMenuItem;
        private System.Windows.Forms.Button undetectButton;
        private System.Windows.Forms.CheckBox firstDeviceCheckBox;
        private System.Windows.Forms.ToolStripStatusLabel speedLabel;
        private System.Windows.Forms.ToolStripStatusLabel positionLabel;
        private System.Windows.Forms.ToolStripStatusLabel bearingLabel;
        private System.Windows.Forms.ToolStripStatusLabel altitudeLabel;
        private System.Windows.Forms.CheckBox clockSynchronizationCheckBox;
        private Clock clock1;
        private Altimeter altimeter1;
        private Compass compass1;
        private SatelliteViewer satelliteViewer1;
        private SatelliteSignalBar satelliteSignalBar1;
        private Speedometer speedometer1;
    }
}
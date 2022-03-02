﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Positioning;
using DotSpatial.Positioning.Gps;
using DotSpatial.Positioning.Gps.Emulators;
using DotSpatial.Positioning.Gps.Nmea;
using DotSpatial.Positioning.Gps.IO;

namespace Diagnostics
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            /* Hook into GPS.NET's device detection events.  These events will report on
             * any GPS devices which have been found, along with any problems encountered and reasons
             * why a particular device could NOT be detected.
             */
            Devices.DeviceDetectionAttempted += new EventHandler<DeviceEventArgs>(Devices_DeviceDetectionAttempted);
            Devices.DeviceDetectionAttemptFailed += new EventHandler<DeviceDetectionExceptionEventArgs>(Devices_DeviceDetectionAttemptFailed);
            Devices.DeviceDetectionStarted += new EventHandler(Devices_DeviceDetectionStarted);
            Devices.DeviceDetectionCompleted += new EventHandler(Devices_DeviceDetectionCompleted);
            Devices.DeviceDetectionCanceled += new EventHandler(Devices_DeviceDetectionCanceled);
            Devices.DeviceDetected += new EventHandler<DeviceEventArgs>(Devices_DeviceDetected);

            /* Hook up event handlers for application-level and AppDomain-level exceptions so 
             * they can be reported to the user
             */
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        #region GPS Device Detection Events

        void Devices_DeviceDetectionCanceled(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Device detection canceled!";
                detectButton.Enabled = true;
                cancelDetectButton.Enabled = false;
            }));
        }

        void Devices_DeviceDetected(object sender, DeviceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                foreach (ListViewItem item in devicesListView.Items)
                {
                    if (object.ReferenceEquals(item.Tag, e.Device))
                    {
                        item.SubItems[1].Text = "GPS DETECTED";
                        item.ImageIndex = 0;
                    }
                }
                devicesListView.Refresh();
            }));
        }

        void Devices_DeviceDetectionCompleted(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Device detection complete.";
                detectButton.Enabled = true;
                cancelDetectButton.Enabled = false;
            }));
        }

        void Devices_DeviceDetectionStarted(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Detecting GPS devices...";
                devicesListView.Items.Clear();
                detectButton.Enabled = false;
                cancelDetectButton.Enabled = true;
            }));
        }

        void Devices_DeviceDetectionAttemptFailed(object sender, DeviceDetectionExceptionEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                foreach (ListViewItem item in devicesListView.Items)
                {
                    if (object.ReferenceEquals(item.Tag, e.Device))
                    {
                        item.SubItems[1].Text = e.Exception.Message;
                        item.ToolTipText = e.Exception.Message;
                        item.ImageIndex = 1;
                    }
                }
                devicesListView.Refresh();
            }));
        }

        void Devices_DeviceDetectionAttempted(object sender, DeviceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                undetectButton.Enabled = true;

                foreach (ListViewItem existingItem in devicesListView.Items)
                {
                    if (object.ReferenceEquals(existingItem.Tag, e.Device))
                    {
                        existingItem.SubItems[1].Text = "Detecting...";
                        return;
                    }
                }

                ListViewItem item = new ListViewItem();
                item.Text = e.Device.Name;
                item.ImageIndex = 2;
                item.Tag = e.Device;
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "Detecting..."));
                devicesListView.Items.Add(item);
                devicesListView.Refresh();
            }));
        }

        #endregion

        #region NmeaInterpreter Events

        private void nmeaInterpreter1_SpeedChanged(object sender, SpeedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                speedTextBox.Text = e.Speed.ToString();
                speedLabel.Text = speedTextBox.Text;
            }));

        }

        private void nmeaInterpreter1_BearingChanged(object sender, AzimuthEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                bearingTextBox.Text = e.Azimuth.ToString();
                bearingLabel.Text = bearingTextBox.Text;
            }));
        }

        private void nmeaInterpreter1_AltitudeChanged(object sender, DistanceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                altitudeTextBox.Text = e.Distance.ToString();
                altitudeLabel.Text = altitudeTextBox.Text;
            }));

        }


        private void nmeaInterpreter1_Paused(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                pauseButton.Enabled = false;
                resumeButton.Enabled = true;
                statusLabel.Text = "Paused.";
            }));
        }

        private void nmeaInterpreter1_Resumed(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                pauseButton.Enabled = true;
                resumeButton.Enabled = false;
            }));
        }

        private void nmeaInterpreter1_Starting(object sender, DeviceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Connecting to " + e.Device.Name + "...";
            }));
        }

        private void nmeaInterpreter1_Started(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Connected!  Waiting for data...";
                sentenceListBox.Items.Clear();
                startButton.Enabled = false;
                stopButton.Enabled = true;
                pauseButton.Enabled = true;
                resumeButton.Enabled = false;

                positionLabel.Text = Position.Empty.ToString();
                speedLabel.Text = Speed.Empty.ToString();
                bearingLabel.Text = Azimuth.Empty.ToString();
                altitudeLabel.Text = Distance.Empty.ToString();
            }));
        }

        private void nmeaInterpreter1_Stopping(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Stopping GPS device...";
            }));
        }

        private void nmeaInterpreter1_Stopped(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                statusLabel.Text = "Stopped.";
                startButton.Enabled = true;
                stopButton.Enabled = false;
                pauseButton.Enabled = false;
                resumeButton.Enabled = false;
            }));
        }


        private void nmeaInterpreter1_SatellitesChanged(object sender, SatelliteListEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                foreach (Satellite satellite in e.Satellites)
                {
                    bool isSatelliteNew = true;

                    // Look for an existing satellite
                    foreach (ListViewItem viewItem in satellitesListView.Items)
                    {
                        Satellite existing = (Satellite)viewItem.Tag;
                        if (existing.PseudorandomNumber.Equals(satellite.PseudorandomNumber))
                        {
                            // Update shiz
                            viewItem.SubItems[2].Text = satellite.Azimuth.ToString();
                            viewItem.SubItems[3].Text = satellite.Elevation.ToString();
                            viewItem.SubItems[4].Text = satellite.SignalToNoiseRatio.ToString();
                            isSatelliteNew = false;
                        }
                    }

                    // If no existing satellite was found, then add a new one
                    if (isSatelliteNew)
                    {
                        ListViewItem newItem = new ListViewItem(satellite.PseudorandomNumber.ToString());
                        newItem.SubItems.Add(satellite.Name);
                        newItem.SubItems.Add(satellite.Azimuth.ToString());
                        newItem.SubItems.Add(satellite.Elevation.ToString());
                        newItem.SubItems.Add(satellite.SignalToNoiseRatio.ToString());
                        newItem.Tag = satellite;
                        satellitesListView.Items.Add(newItem);
                    }
                }
            }));
        }

        private void nmeaInterpreter1_PositionChanged(object sender, PositionEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                positionTextBox.Text = e.Position.ToString();
                positionLabel.Text = positionTextBox.Text;
            }));
        }

        private void nmeaInterpreter1_DateTimeChanged(object sender, DateTimeEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                dateTimeTextBox.Text = e.DateTime.ToShortDateString() + " " + e.DateTime.ToLongTimeString();
                utcDateTimeTextBox.Text = e.DateTime.ToUniversalTime().ToString("R");
            }));
        }

        private void nmeaInterpreter1_SentenceReceived(object sender, DotSpatial.Positioning.Gps.Nmea.NmeaSentenceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                if (sentenceListBox.Items.Count >= 100)
                sentenceListBox.Items.RemoveAt(0);

                sentenceListBox.Items.Add(e.Sentence.ToString());
                sentenceListBox.SelectedIndex = sentenceListBox.Items.Count - 1;

                statusLabel.Text = "Receiving GPS data.";
            }));
        }

        #endregion

        #region Button Events

        private void detectButton_Click(object sender, EventArgs e)
        {
            Devices.BeginDetection();
        }

        private void cancelDetectButton_Click(object sender, EventArgs e)
        {
            Devices.CancelDetection(true);
        }

        private void undetectButton_Click(object sender, EventArgs e)
        {
            Devices.Undetect();
            devicesListView.Items.Clear();
            undetectButton.Enabled = false;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                nmeaInterpreter1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot connect to GPS");
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            nmeaInterpreter1.Stop();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            nmeaInterpreter1.Pause();
        }

        private void resumeButton_Click(object sender, EventArgs e)
        {
            nmeaInterpreter1.Resume();
        }

        #endregion

        #region Context Menu Events

        private void deviceContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = devicesListView.SelectedItems.Count == 0;
        }

        private void redetectMenuItem_Click(object sender, EventArgs e)
        {
            Device device = (Device)devicesListView.SelectedItems[0].Tag;
            device.Undetect();
            device.BeginDetection();
        }

        private void resetMenuItem_Click(object sender, EventArgs e)
        {
            Device device = (Device)devicesListView.SelectedItems[0].Tag;
            device.Reset();
        }

        #endregion

        #region Other Form Control Events

        private void devicesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Item.Tag;
        }

        private void serialCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Devices.AllowSerialConnections = serialCheckBox.Checked;
            exhaustiveCheckBox.Enabled = serialCheckBox.Checked;
        }

        private void exhaustiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Devices.AllowExhaustiveSerialPortScanning = exhaustiveCheckBox.Checked;
        }

        private void bluetoothCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Devices.AllowBluetoothConnections = bluetoothCheckBox.Checked;
        }

        private void firstDeviceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Devices.IsOnlyFirstDeviceDetected = firstDeviceCheckBox.Checked;
        }

        private void clockSynchronizationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Devices.IsClockSynchronizationEnabled = clockSynchronizationCheckBox.Checked;
        }

        #endregion

        #region Unhandled Exception Events

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            NotifyOfUnhandledException(ex);
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            NotifyOfUnhandledException(e.Exception);
        }

        /// <summary>
        /// Logs an unhandled exception and displays a message box alerting the user to the error.
        /// </summary>
        /// <param name="exception">The unhandled exception.</param>
        private void NotifyOfUnhandledException(Exception exception)
        {
            try
            {
                // Log the exception (and all of its inner exceptions)
                Exception innerException = exception;
                while (innerException != null)
                {
                    Trace.TraceError(innerException.ToString());
                    innerException = innerException.InnerException;
                }

                // Stop the interpreter
                nmeaInterpreter1.Stop();
            }
            finally
            {
                // Display the error to the user
                MessageBox.Show(
                    "An unexpected error has occurred.\n\n" + exception.GetType() + ": " + exception.Message,
                    Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
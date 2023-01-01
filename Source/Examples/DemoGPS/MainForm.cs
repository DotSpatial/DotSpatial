// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Positioning;

namespace Demo.GPS
{
    /// <summary>
    /// The Main form for the project.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Main form for the GPS diagnostics demo application
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            /* Hook into GPS.NET's device detection events.  These events will report on
             * any GPS devices which have been found, along with any problems encountered and reasons
             * why a particular device could NOT be detected.
             */
            Devices.DeviceDetectionAttempted += DevicesDeviceDetectionAttempted;
            Devices.DeviceDetectionAttemptFailed += DevicesDeviceDetectionAttemptFailed;
            Devices.DeviceDetectionStarted += DevicesDeviceDetectionStarted;
            Devices.DeviceDetectionCompleted += DevicesDeviceDetectionCompleted;
            Devices.DeviceDetectionCanceled += DevicesDeviceDetectionCanceled;
            Devices.DeviceDetected += DevicesDeviceDetected;

            /* Hook up event handlers for application-level and AppDomain-level exceptions so
             * they can be reported to the user
             */
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        #region GPS Device Detection Events

        private void DevicesDeviceDetectionCanceled(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  statusLabel.Text = "Device detection canceled!";
                                                  detectButton.Enabled = true;
                                                  cancelDetectButton.Enabled = false;
                                              }));
        }

        private void DevicesDeviceDetected(object sender, DeviceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  foreach (ListViewItem item in devicesListView.Items)
                                                  {
                                                      if (ReferenceEquals(item.Tag, e.Device))
                                                      {
                                                          item.SubItems[1].Text = "GPS DETECTED";
                                                          item.ImageIndex = 0;
                                                      }
                                                  }

                                                  devicesListView.Refresh();
                                              }));
        }

        private void DevicesDeviceDetectionCompleted(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  statusLabel.Text = "Device detection complete.";
                                                  detectButton.Enabled = true;
                                                  cancelDetectButton.Enabled = false;
                                              }));
        }

        private void DevicesDeviceDetectionStarted(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  statusLabel.Text = "Detecting GPS devices...";
                                                  devicesListView.Items.Clear();
                                                  detectButton.Enabled = false;
                                                  cancelDetectButton.Enabled = true;
                                              }));
        }

        private void DevicesDeviceDetectionAttemptFailed(object sender, DeviceDetectionExceptionEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  foreach (ListViewItem item in devicesListView.Items)
                                                  {
                                                      if (ReferenceEquals(item.Tag, e.Device))
                                                      {
                                                          item.SubItems[1].Text = e.Exception.Message;
                                                          item.ToolTipText = e.Exception.Message;
                                                          item.ImageIndex = 1;
                                                      }
                                                  }

                                                  devicesListView.Refresh();
                                              }));
        }

        private void DevicesDeviceDetectionAttempted(object sender, DeviceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  undetectButton.Enabled = true;

                                                  foreach (ListViewItem existingItem in devicesListView.Items)
                                                  {
                                                      if (ReferenceEquals(existingItem.Tag, e.Device))
                                                      {
                                                          existingItem.SubItems[1].Text = "Detecting...";
                                                          return;
                                                      }
                                                  }

                                                  ListViewItem item = new() { Text = e.Device.Name, ImageIndex = 2, Tag = e.Device };
                                                  item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "Detecting..."));
                                                  devicesListView.Items.Add(item);
                                                  devicesListView.Refresh();
                                              }));
        }

        #endregion

        #region NmeaInterpreter Events

        private void NmeaInterpreter1SpeedChanged(object sender, SpeedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  speedTextBox.Text = e.Speed.ToString();
                                                  speedLabel.Text = speedTextBox.Text;
                                              }));
        }

        private void NmeaInterpreter1BearingChanged(object sender, AzimuthEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  bearingTextBox.Text = e.Azimuth.ToString();
                                                  bearingLabel.Text = bearingTextBox.Text;
                                              }));
        }

        private void NmeaInterpreter1AltitudeChanged(object sender, DistanceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  altitudeTextBox.Text = e.Distance.ToString();
                                                  altitudeLabel.Text = altitudeTextBox.Text;
                                              }));
        }

        private void NmeaInterpreter1Paused(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  pauseButton.Enabled = false;
                                                  resumeButton.Enabled = true;
                                                  statusLabel.Text = "Paused.";
                                              }));
        }

        private void NmeaInterpreter1Resumed(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  pauseButton.Enabled = true;
                                                  resumeButton.Enabled = false;
                                              }));
        }

        private void NmeaInterpreter1Starting(object sender, DeviceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  statusLabel.Text = "Connecting to " + e.Device.Name + "...";
                                              }));
        }

        private void NmeaInterpreter1Started(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
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

        private void NmeaInterpreter1Stopping(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  statusLabel.Text = "Stopping GPS device...";
                                              }));
        }

        private void NmeaInterpreter1Stopped(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  statusLabel.Text = "Stopped.";
                                                  startButton.Enabled = true;
                                                  stopButton.Enabled = false;
                                                  pauseButton.Enabled = false;
                                                  resumeButton.Enabled = false;
                                              }));
        }

        private void NmeaInterpreter1SatellitesChanged(object sender, SatelliteListEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
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
                                                          ListViewItem newItem = new(satellite.PseudorandomNumber.ToString());
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

        private void NmeaInterpreter1PositionChanged(object sender, PositionEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  positionTextBox.Text = e.Position.ToString();
                                                  positionLabel.Text = positionTextBox.Text;
                                              }));
        }

        private void NmeaInterpreter1DateTimeChanged(object sender, DateTimeEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  dateTimeTextBox.Text = e.DateTime.ToShortDateString() + " " + e.DateTime.ToLongTimeString();
                                                  utcDateTimeTextBox.Text = e.DateTime.ToUniversalTime().ToString("R");
                                              }));
        }

        private void NmeaInterpreter1SentenceReceived(object sender, NmeaSentenceEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  if (sentenceListBox.Items.Count >= 100)
                                                  {
                                                      sentenceListBox.Items.RemoveAt(0);
                                                  }

                                                  sentenceListBox.Items.Add(e.Sentence.ToString());
                                                  sentenceListBox.SelectedIndex = sentenceListBox.Items.Count - 1;

                                                  statusLabel.Text = "Receiving GPS data.";
                                              }));
        }

        #endregion

        #region Button Events

        private void DetectButtonClick(object sender, EventArgs e)
        {
            Devices.BeginDetection();
        }

        private void CancelDetectButtonClick(object sender, EventArgs e)
        {
            Devices.CancelDetection(true);
        }

        private void UndetectButtonClick(object sender, EventArgs e)
        {
            Devices.Undetect();
            devicesListView.Items.Clear();
            undetectButton.Enabled = false;
        }

        private void StartButtonClick(object sender, EventArgs e)
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

        private void StopButtonClick(object sender, EventArgs e)
        {
            nmeaInterpreter1.Stop();
        }

        private void PauseButtonClick(object sender, EventArgs e)
        {
            nmeaInterpreter1.Pause();
        }

        private void ResumeButtonClick(object sender, EventArgs e)
        {
            nmeaInterpreter1.Resume();
        }

        #endregion

        #region Context Menu Events

        private void DeviceContextMenuOpening(object sender, CancelEventArgs e)
        {
            e.Cancel = devicesListView.SelectedItems.Count == 0;
        }

        private void RedetectMenuItemClick(object sender, EventArgs e)
        {
            Device device = (Device)devicesListView.SelectedItems[0].Tag;
            device.Undetect();
            device.BeginDetection();
        }

        private void ResetMenuItemClick(object sender, EventArgs e)
        {
            Device device = (Device)devicesListView.SelectedItems[0].Tag;
            device.Reset();
        }

        #endregion

        #region Other Form Control Events

        private void DevicesListViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Item.Tag;
        }

        private void SerialCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Devices.AllowSerialConnections = serialCheckBox.Checked;
            exhaustiveCheckBox.Enabled = serialCheckBox.Checked;
        }

        private void ExhaustiveCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Devices.AllowExhaustiveSerialPortScanning = exhaustiveCheckBox.Checked;
        }

        private void BluetoothCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Devices.AllowBluetoothConnections = bluetoothCheckBox.Checked;
        }

        private void FirstDeviceCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Devices.IsOnlyFirstDeviceDetected = firstDeviceCheckBox.Checked;
        }

        private void ClockSynchronizationCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Devices.IsClockSynchronizationEnabled = clockSynchronizationCheckBox.Checked;
        }

        #endregion

        #region Unhandled Exception Events

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            NotifyOfUnhandledException(ex);
        }

        private void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
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
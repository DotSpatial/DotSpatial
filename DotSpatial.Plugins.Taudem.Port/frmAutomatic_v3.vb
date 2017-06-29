'********************************************************************************************************
' FILENAME:      frmAutomatic.vb
' DESCRIPTION:  Allows a user to quickly set a base DEM file and find the watershed
'   delineations on it. It has further options for using an outlet point
'   shape file or a stream flow grid, as well as allowing intermediate
'   files to be displayed in MapWindow.
' NOTES: This form is called from mwTauDemBASINSWrap.vb
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License")
'you may not use this file except in compliance with the License. You may obtain a copy of the License at
'http://www.mozilla.org/MPL/
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and
'limitations under the License.
'
'
'Last Update:   January 2011, CWG
' Change Log:
' Date          Changed By      Notes
'08/28/2005     ARA             Added Headers
'10/18/05       ARA             Wrapping up and added mozilla comments
'05/26/06       ARA             Copied code from original frmAutomatic and reset change logs on all functions
'January 2011   CWG             Adapted extensively to use TauDEM V5
'********************************************************************************************************

Imports System.Runtime.InteropServices

Imports System.Windows.Forms
Imports DotSpatial.Controls
Imports DotSpatial.Data

Public Class frmAutomatic_v3
    Inherits System.Windows.Forms.Form


    Public Property App As AppManager
        Get
            Return _app
        End Get
        Set(ByVal Value As AppManager)
            _app = Value
        End Set
    End Property
    Private _app As AppManager


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents btnRunAll As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ttip As System.Windows.Forms.ToolTip
    Friend WithEvents grpbxSetupPreprocess As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectMask As System.Windows.Forms.Button
    Friend WithEvents btnDrawMask As System.Windows.Forms.Button
    Friend WithEvents chkbxMask As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxBurnStream As System.Windows.Forms.CheckBox
    Friend WithEvents lblSelDem As System.Windows.Forms.Label
    Friend WithEvents grpbxThresh As System.Windows.Forms.GroupBox
    Friend WithEvents cmbxThreshConvUnits As System.Windows.Forms.ComboBox
    Friend WithEvents txtbxThreshConv As System.Windows.Forms.TextBox
    Friend WithEvents txtNumCells As System.Windows.Forms.Label
    Friend WithEvents txtbxThreshold As System.Windows.Forms.TextBox
    Friend WithEvents grpbxOutletDef As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectOutlets As System.Windows.Forms.Button
    Friend WithEvents btnDrawOutlets As System.Windows.Forms.Button
    Friend WithEvents btnBrowseOutlets As System.Windows.Forms.Button
    Friend WithEvents cmbxOutlets As System.Windows.Forms.ComboBox
    Friend WithEvents chkbxUseOutlet As System.Windows.Forms.CheckBox
    Friend WithEvents btnRunPreproc As System.Windows.Forms.Button
    Friend WithEvents btnRunThreshDelin As System.Windows.Forms.Button
    Friend WithEvents btnAdvanced As System.Windows.Forms.Button
    Friend WithEvents lblMaskSelected As System.Windows.Forms.Label
    Friend WithEvents lblOutletSelected As System.Windows.Forms.Label
    Friend WithEvents btnSnapTo As System.Windows.Forms.Button
    Friend WithEvents txtbxSnapThresh As System.Windows.Forms.TextBox
    Friend WithEvents lblSnapThresh As System.Windows.Forms.Label
    Friend WithEvents lblElevUnits As System.Windows.Forms.Label
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents cmbxElevUnits As System.Windows.Forms.ComboBox
    Friend WithEvents rdobtnUseFileMask As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnUseExtents As System.Windows.Forms.RadioButton
    Friend WithEvents btnSetExtents As System.Windows.Forms.Button
    Friend WithEvents btnBrowseMask As System.Windows.Forms.Button
    Friend WithEvents cmbxMask As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseStream As System.Windows.Forms.Button
    Friend WithEvents cmbxStream As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseDem As System.Windows.Forms.Button
    Friend WithEvents cmbxSelDem As System.Windows.Forms.ComboBox
    Friend WithEvents lblOutlets As System.Windows.Forms.Label
    Friend WithEvents lblPreproc As System.Windows.Forms.Label
    Friend WithEvents lblDelin As System.Windows.Forms.Label
    Friend WithEvents btnLoadPre As System.Windows.Forms.Button
    Friend WithEvents lblPreOut As System.Windows.Forms.Label
    Friend WithEvents btnLoadDelin As System.Windows.Forms.Button
    Friend WithEvents lblDelinOut As System.Windows.Forms.Label
    Friend WithEvents btnRunOutletFinish As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAutomatic_v3))
        Me.btnRunAll = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.ttip = New System.Windows.Forms.ToolTip(Me.components)
        Me.grpbxSetupPreprocess = New System.Windows.Forms.GroupBox
        Me.lblPreOut = New System.Windows.Forms.Label
        Me.btnLoadPre = New System.Windows.Forms.Button
        Me.lblElevUnits = New System.Windows.Forms.Label
        Me.cmbxElevUnits = New System.Windows.Forms.ComboBox
        Me.rdobtnUseFileMask = New System.Windows.Forms.RadioButton
        Me.rdobtnUseExtents = New System.Windows.Forms.RadioButton
        Me.btnSetExtents = New System.Windows.Forms.Button
        Me.btnRunPreproc = New System.Windows.Forms.Button
        Me.lblMaskSelected = New System.Windows.Forms.Label
        Me.chkbxBurnStream = New System.Windows.Forms.CheckBox
        Me.btnSelectMask = New System.Windows.Forms.Button
        Me.btnDrawMask = New System.Windows.Forms.Button
        Me.btnBrowseMask = New System.Windows.Forms.Button
        Me.cmbxMask = New System.Windows.Forms.ComboBox
        Me.chkbxMask = New System.Windows.Forms.CheckBox
        Me.btnBrowseStream = New System.Windows.Forms.Button
        Me.cmbxStream = New System.Windows.Forms.ComboBox
        Me.btnBrowseDem = New System.Windows.Forms.Button
        Me.cmbxSelDem = New System.Windows.Forms.ComboBox
        Me.lblSelDem = New System.Windows.Forms.Label
        Me.lblPreproc = New System.Windows.Forms.Label
        Me.grpbxThresh = New System.Windows.Forms.GroupBox
        Me.lblDelinOut = New System.Windows.Forms.Label
        Me.btnLoadDelin = New System.Windows.Forms.Button
        Me.btnRunThreshDelin = New System.Windows.Forms.Button
        Me.cmbxThreshConvUnits = New System.Windows.Forms.ComboBox
        Me.txtbxThreshConv = New System.Windows.Forms.TextBox
        Me.txtNumCells = New System.Windows.Forms.Label
        Me.txtbxThreshold = New System.Windows.Forms.TextBox
        Me.lblDelin = New System.Windows.Forms.Label
        Me.grpbxOutletDef = New System.Windows.Forms.GroupBox
        Me.txtbxSnapThresh = New System.Windows.Forms.TextBox
        Me.btnSnapTo = New System.Windows.Forms.Button
        Me.lblOutletSelected = New System.Windows.Forms.Label
        Me.btnRunOutletFinish = New System.Windows.Forms.Button
        Me.btnSelectOutlets = New System.Windows.Forms.Button
        Me.btnDrawOutlets = New System.Windows.Forms.Button
        Me.btnBrowseOutlets = New System.Windows.Forms.Button
        Me.cmbxOutlets = New System.Windows.Forms.ComboBox
        Me.chkbxUseOutlet = New System.Windows.Forms.CheckBox
        Me.lblSnapThresh = New System.Windows.Forms.Label
        Me.lblOutlets = New System.Windows.Forms.Label
        Me.btnAdvanced = New System.Windows.Forms.Button
        Me.btnHelp = New System.Windows.Forms.Button
        Me.numProcesses = New System.Windows.Forms.TextBox
        Me.lblNumProc = New System.Windows.Forms.Label
        Me.showTaudemOutput = New System.Windows.Forms.CheckBox
        Me.grpbxSetupPreprocess.SuspendLayout()
        Me.grpbxThresh.SuspendLayout()
        Me.grpbxOutletDef.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRunAll
        '
        Me.btnRunAll.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnRunAll.Location = New System.Drawing.Point(358, 523)
        Me.btnRunAll.Name = "btnRunAll"
        Me.btnRunAll.Size = New System.Drawing.Size(75, 23)
        Me.btnRunAll.TabIndex = 28
        Me.btnRunAll.Text = "Run All"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(277, 523)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 27
        Me.btnCancel.Text = "Close"
        '
        'ttip
        '
        Me.ttip.AutomaticDelay = 100
        Me.ttip.AutoPopDelay = 5000
        Me.ttip.InitialDelay = 100
        Me.ttip.ReshowDelay = 20
        '
        'grpbxSetupPreprocess
        '
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblPreOut)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnLoadPre)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblElevUnits)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxElevUnits)
        Me.grpbxSetupPreprocess.Controls.Add(Me.rdobtnUseFileMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.rdobtnUseExtents)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnSetExtents)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnRunPreproc)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblMaskSelected)
        Me.grpbxSetupPreprocess.Controls.Add(Me.chkbxBurnStream)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnSelectMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnDrawMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnBrowseMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.chkbxMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnBrowseStream)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxStream)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnBrowseDem)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxSelDem)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblSelDem)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblPreproc)
        Me.grpbxSetupPreprocess.Location = New System.Drawing.Point(5, 3)
        Me.grpbxSetupPreprocess.Name = "grpbxSetupPreprocess"
        Me.grpbxSetupPreprocess.Size = New System.Drawing.Size(431, 272)
        Me.grpbxSetupPreprocess.TabIndex = 28
        Me.grpbxSetupPreprocess.TabStop = False
        Me.grpbxSetupPreprocess.Text = "Setup and Preprocessing"
        '
        'lblPreOut
        '
        Me.lblPreOut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPreOut.Location = New System.Drawing.Point(194, 237)
        Me.lblPreOut.Name = "lblPreOut"
        Me.lblPreOut.Size = New System.Drawing.Size(149, 25)
        Me.lblPreOut.TabIndex = 35
        Me.lblPreOut.Text = "Intermediate Files Loaded"
        Me.lblPreOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPreOut.Visible = False
        '
        'btnLoadPre
        '
        Me.btnLoadPre.Location = New System.Drawing.Point(14, 237)
        Me.btnLoadPre.Name = "btnLoadPre"
        Me.btnLoadPre.Size = New System.Drawing.Size(171, 26)
        Me.btnLoadPre.TabIndex = 34
        Me.btnLoadPre.Text = "Use Existing Intermediate Files"
        Me.btnLoadPre.UseVisualStyleBackColor = True
        '
        'lblElevUnits
        '
        Me.lblElevUnits.Location = New System.Drawing.Point(3, 15)
        Me.lblElevUnits.Name = "lblElevUnits"
        Me.lblElevUnits.Size = New System.Drawing.Size(84, 16)
        Me.lblElevUnits.TabIndex = 33
        Me.lblElevUnits.Text = "Elevation Units"
        '
        'cmbxElevUnits
        '
        Me.cmbxElevUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxElevUnits.ItemHeight = 13
        Me.cmbxElevUnits.Items.AddRange(New Object() {"Meters", "Centimeters", "Feet"})
        Me.cmbxElevUnits.Location = New System.Drawing.Point(6, 34)
        Me.cmbxElevUnits.Name = "cmbxElevUnits"
        Me.cmbxElevUnits.Size = New System.Drawing.Size(81, 21)
        Me.cmbxElevUnits.TabIndex = 32
        '
        'rdobtnUseFileMask
        '
        Me.rdobtnUseFileMask.AutoSize = True
        Me.rdobtnUseFileMask.Location = New System.Drawing.Point(14, 158)
        Me.rdobtnUseFileMask.Name = "rdobtnUseFileMask"
        Me.rdobtnUseFileMask.Size = New System.Drawing.Size(169, 17)
        Me.rdobtnUseFileMask.TabIndex = 31
        Me.rdobtnUseFileMask.Text = "Use Grid or Shapefile for Mask"
        Me.rdobtnUseFileMask.UseVisualStyleBackColor = True
        '
        'rdobtnUseExtents
        '
        Me.rdobtnUseExtents.AutoSize = True
        Me.rdobtnUseExtents.Checked = True
        Me.rdobtnUseExtents.Location = New System.Drawing.Point(14, 135)
        Me.rdobtnUseExtents.Name = "rdobtnUseExtents"
        Me.rdobtnUseExtents.Size = New System.Drawing.Size(189, 17)
        Me.rdobtnUseExtents.TabIndex = 30
        Me.rdobtnUseExtents.TabStop = True
        Me.rdobtnUseExtents.Text = "Use Current View Extents for Mask"
        Me.rdobtnUseExtents.UseVisualStyleBackColor = True
        '
        'btnSetExtents
        '
        Me.btnSetExtents.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSetExtents.Location = New System.Drawing.Point(347, 132)
        Me.btnSetExtents.Name = "btnSetExtents"
        Me.btnSetExtents.Size = New System.Drawing.Size(75, 23)
        Me.btnSetExtents.TabIndex = 29
        Me.btnSetExtents.Text = "Set Extents"
        Me.btnSetExtents.UseVisualStyleBackColor = True
        '
        'btnRunPreproc
        '
        Me.btnRunPreproc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunPreproc.Location = New System.Drawing.Point(350, 239)
        Me.btnRunPreproc.Name = "btnRunPreproc"
        Me.btnRunPreproc.Size = New System.Drawing.Size(75, 23)
        Me.btnRunPreproc.TabIndex = 12
        Me.btnRunPreproc.Text = "Run"
        Me.btnRunPreproc.UseVisualStyleBackColor = True
        '
        'lblMaskSelected
        '
        Me.lblMaskSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMaskSelected.Location = New System.Drawing.Point(191, 206)
        Me.lblMaskSelected.Name = "lblMaskSelected"
        Me.lblMaskSelected.Size = New System.Drawing.Size(152, 23)
        Me.lblMaskSelected.TabIndex = 28
        Me.lblMaskSelected.Text = "0 Selected"
        Me.lblMaskSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkbxBurnStream
        '
        Me.chkbxBurnStream.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxBurnStream.Location = New System.Drawing.Point(6, 61)
        Me.chkbxBurnStream.Name = "chkbxBurnStream"
        Me.chkbxBurnStream.Size = New System.Drawing.Size(416, 24)
        Me.chkbxBurnStream.TabIndex = 3
        Me.chkbxBurnStream.Text = " Burn-in Existing Stream Polyline"
        '
        'btnSelectMask
        '
        Me.btnSelectMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectMask.Enabled = False
        Me.btnSelectMask.Location = New System.Drawing.Point(101, 206)
        Me.btnSelectMask.Name = "btnSelectMask"
        Me.btnSelectMask.Size = New System.Drawing.Size(84, 23)
        Me.btnSelectMask.TabIndex = 10
        Me.btnSelectMask.Text = "Select Mask"
        Me.btnSelectMask.UseVisualStyleBackColor = True
        '
        'btnDrawMask
        '
        Me.btnDrawMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDrawMask.Enabled = False
        Me.btnDrawMask.Location = New System.Drawing.Point(15, 206)
        Me.btnDrawMask.Name = "btnDrawMask"
        Me.btnDrawMask.Size = New System.Drawing.Size(83, 23)
        Me.btnDrawMask.TabIndex = 9
        Me.btnDrawMask.Text = "Draw Mask"
        Me.btnDrawMask.UseVisualStyleBackColor = True
        '
        'btnBrowseMask
        '
        Me.btnBrowseMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseMask.Image = CType(resources.GetObject("btnBrowseMask.Image"), System.Drawing.Image)
        Me.btnBrowseMask.Location = New System.Drawing.Point(398, 175)
        Me.btnBrowseMask.Name = "btnBrowseMask"
        Me.btnBrowseMask.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseMask.TabIndex = 8
        '
        'cmbxMask
        '
        Me.cmbxMask.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxMask.Enabled = False
        Me.cmbxMask.Location = New System.Drawing.Point(14, 177)
        Me.cmbxMask.Name = "cmbxMask"
        Me.cmbxMask.Size = New System.Drawing.Size(378, 21)
        Me.cmbxMask.TabIndex = 7
        '
        'chkbxMask
        '
        Me.chkbxMask.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxMask.Location = New System.Drawing.Point(6, 111)
        Me.chkbxMask.Name = "chkbxMask"
        Me.chkbxMask.Size = New System.Drawing.Size(416, 24)
        Me.chkbxMask.TabIndex = 6
        Me.chkbxMask.Text = "Use a Focusing Mask"
        '
        'btnBrowseStream
        '
        Me.btnBrowseStream.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseStream.Image = CType(resources.GetObject("btnBrowseStream.Image"), System.Drawing.Image)
        Me.btnBrowseStream.Location = New System.Drawing.Point(398, 84)
        Me.btnBrowseStream.Name = "btnBrowseStream"
        Me.btnBrowseStream.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseStream.TabIndex = 5
        '
        'cmbxStream
        '
        Me.cmbxStream.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxStream.Items.AddRange(New Object() {"test"})
        Me.cmbxStream.Location = New System.Drawing.Point(14, 86)
        Me.cmbxStream.Name = "cmbxStream"
        Me.cmbxStream.Size = New System.Drawing.Size(378, 21)
        Me.cmbxStream.TabIndex = 4
        '
        'btnBrowseDem
        '
        Me.btnBrowseDem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseDem.Image = CType(resources.GetObject("btnBrowseDem.Image"), System.Drawing.Image)
        Me.btnBrowseDem.Location = New System.Drawing.Point(398, 32)
        Me.btnBrowseDem.Name = "btnBrowseDem"
        Me.btnBrowseDem.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseDem.TabIndex = 2
        '
        'cmbxSelDem
        '
        Me.cmbxSelDem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxSelDem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxSelDem.ItemHeight = 13
        Me.cmbxSelDem.Items.AddRange(New Object() {"Select a Grid"})
        Me.cmbxSelDem.Location = New System.Drawing.Point(102, 34)
        Me.cmbxSelDem.Name = "cmbxSelDem"
        Me.cmbxSelDem.Size = New System.Drawing.Size(290, 21)
        Me.cmbxSelDem.TabIndex = 1
        '
        'lblSelDem
        '
        Me.lblSelDem.Location = New System.Drawing.Point(94, 15)
        Me.lblSelDem.Name = "lblSelDem"
        Me.lblSelDem.Size = New System.Drawing.Size(184, 16)
        Me.lblSelDem.TabIndex = 27
        Me.lblSelDem.Text = "Base Elevation Data (DEM) Layer:"
        '
        'lblPreproc
        '
        Me.lblPreproc.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreproc.Location = New System.Drawing.Point(6, 16)
        Me.lblPreproc.Name = "lblPreproc"
        Me.lblPreproc.Size = New System.Drawing.Size(419, 253)
        Me.lblPreproc.TabIndex = 33
        Me.lblPreproc.Text = "Setup and Preprocessing Steps Currently Running"
        Me.lblPreproc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpbxThresh
        '
        Me.grpbxThresh.Controls.Add(Me.lblDelinOut)
        Me.grpbxThresh.Controls.Add(Me.btnLoadDelin)
        Me.grpbxThresh.Controls.Add(Me.btnRunThreshDelin)
        Me.grpbxThresh.Controls.Add(Me.cmbxThreshConvUnits)
        Me.grpbxThresh.Controls.Add(Me.txtbxThreshConv)
        Me.grpbxThresh.Controls.Add(Me.txtNumCells)
        Me.grpbxThresh.Controls.Add(Me.txtbxThreshold)
        Me.grpbxThresh.Controls.Add(Me.lblDelin)
        Me.grpbxThresh.Location = New System.Drawing.Point(5, 281)
        Me.grpbxThresh.Name = "grpbxThresh"
        Me.grpbxThresh.Size = New System.Drawing.Size(431, 81)
        Me.grpbxThresh.TabIndex = 30
        Me.grpbxThresh.TabStop = False
        Me.grpbxThresh.Text = "Network Delineation by Threshold Method"
        '
        'lblDelinOut
        '
        Me.lblDelinOut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDelinOut.Location = New System.Drawing.Point(194, 48)
        Me.lblDelinOut.Name = "lblDelinOut"
        Me.lblDelinOut.Size = New System.Drawing.Size(149, 25)
        Me.lblDelinOut.TabIndex = 36
        Me.lblDelinOut.Text = "Intermediate Files Loaded"
        Me.lblDelinOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblDelinOut.Visible = False
        '
        'btnLoadDelin
        '
        Me.btnLoadDelin.Location = New System.Drawing.Point(14, 47)
        Me.btnLoadDelin.Name = "btnLoadDelin"
        Me.btnLoadDelin.Size = New System.Drawing.Size(171, 26)
        Me.btnLoadDelin.TabIndex = 35
        Me.btnLoadDelin.Text = "Use Existing Intermediate Files"
        Me.btnLoadDelin.UseVisualStyleBackColor = True
        '
        'btnRunThreshDelin
        '
        Me.btnRunThreshDelin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunThreshDelin.Location = New System.Drawing.Point(350, 50)
        Me.btnRunThreshDelin.Name = "btnRunThreshDelin"
        Me.btnRunThreshDelin.Size = New System.Drawing.Size(75, 23)
        Me.btnRunThreshDelin.TabIndex = 17
        Me.btnRunThreshDelin.Text = "Run"
        Me.btnRunThreshDelin.UseVisualStyleBackColor = True
        '
        'cmbxThreshConvUnits
        '
        Me.cmbxThreshConvUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxThreshConvUnits.ItemHeight = 13
        Me.cmbxThreshConvUnits.Items.AddRange(New Object() {"sq. mi", "acres", "hectares", "sq. km", "sq. m", "sq. ft"})
        Me.cmbxThreshConvUnits.Location = New System.Drawing.Point(353, 20)
        Me.cmbxThreshConvUnits.Name = "cmbxThreshConvUnits"
        Me.cmbxThreshConvUnits.Size = New System.Drawing.Size(72, 21)
        Me.cmbxThreshConvUnits.TabIndex = 15
        '
        'txtbxThreshConv
        '
        Me.txtbxThreshConv.Location = New System.Drawing.Point(209, 20)
        Me.txtbxThreshConv.MaxLength = 10
        Me.txtbxThreshConv.Name = "txtbxThreshConv"
        Me.txtbxThreshConv.Size = New System.Drawing.Size(138, 20)
        Me.txtbxThreshConv.TabIndex = 14
        '
        'txtNumCells
        '
        Me.txtNumCells.AutoSize = True
        Me.txtNumCells.Location = New System.Drawing.Point(144, 23)
        Me.txtNumCells.Name = "txtNumCells"
        Me.txtNumCells.Size = New System.Drawing.Size(51, 13)
        Me.txtNumCells.TabIndex = 26
        Me.txtNumCells.Text = "# of Cells"
        '
        'txtbxThreshold
        '
        Me.txtbxThreshold.Location = New System.Drawing.Point(14, 20)
        Me.txtbxThreshold.MaxLength = 10
        Me.txtbxThreshold.Name = "txtbxThreshold"
        Me.txtbxThreshold.Size = New System.Drawing.Size(127, 20)
        Me.txtbxThreshold.TabIndex = 13
        '
        'lblDelin
        '
        Me.lblDelin.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelin.Location = New System.Drawing.Point(7, 16)
        Me.lblDelin.Name = "lblDelin"
        Me.lblDelin.Size = New System.Drawing.Size(418, 57)
        Me.lblDelin.TabIndex = 27
        Me.lblDelin.Text = "Network Delineation Steps Running"
        Me.lblDelin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpbxOutletDef
        '
        Me.grpbxOutletDef.Controls.Add(Me.txtbxSnapThresh)
        Me.grpbxOutletDef.Controls.Add(Me.btnSnapTo)
        Me.grpbxOutletDef.Controls.Add(Me.lblOutletSelected)
        Me.grpbxOutletDef.Controls.Add(Me.btnRunOutletFinish)
        Me.grpbxOutletDef.Controls.Add(Me.btnSelectOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.btnDrawOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.btnBrowseOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.cmbxOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.chkbxUseOutlet)
        Me.grpbxOutletDef.Controls.Add(Me.lblSnapThresh)
        Me.grpbxOutletDef.Controls.Add(Me.lblOutlets)
        Me.grpbxOutletDef.Location = New System.Drawing.Point(5, 364)
        Me.grpbxOutletDef.Name = "grpbxOutletDef"
        Me.grpbxOutletDef.Size = New System.Drawing.Size(431, 126)
        Me.grpbxOutletDef.TabIndex = 31
        Me.grpbxOutletDef.TabStop = False
        Me.grpbxOutletDef.Text = "Custom Outlet/Inlet Definition and Delineation Completion"
        '
        'txtbxSnapThresh
        '
        Me.txtbxSnapThresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxSnapThresh.Location = New System.Drawing.Point(185, 98)
        Me.txtbxSnapThresh.Name = "txtbxSnapThresh"
        Me.txtbxSnapThresh.Size = New System.Drawing.Size(85, 20)
        Me.txtbxSnapThresh.TabIndex = 24
        Me.txtbxSnapThresh.Text = "300"
        '
        'btnSnapTo
        '
        Me.btnSnapTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSnapTo.Location = New System.Drawing.Point(15, 96)
        Me.btnSnapTo.Name = "btnSnapTo"
        Me.btnSnapTo.Size = New System.Drawing.Size(84, 23)
        Me.btnSnapTo.TabIndex = 23
        Me.btnSnapTo.Text = "Snap Preview"
        Me.btnSnapTo.UseVisualStyleBackColor = True
        '
        'lblOutletSelected
        '
        Me.lblOutletSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOutletSelected.Location = New System.Drawing.Point(276, 69)
        Me.lblOutletSelected.Name = "lblOutletSelected"
        Me.lblOutletSelected.Size = New System.Drawing.Size(146, 23)
        Me.lblOutletSelected.TabIndex = 29
        Me.lblOutletSelected.Text = "0 Selected"
        Me.lblOutletSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnRunOutletFinish
        '
        Me.btnRunOutletFinish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunOutletFinish.Location = New System.Drawing.Point(350, 96)
        Me.btnRunOutletFinish.Name = "btnRunOutletFinish"
        Me.btnRunOutletFinish.Size = New System.Drawing.Size(75, 23)
        Me.btnRunOutletFinish.TabIndex = 25
        Me.btnRunOutletFinish.Text = "Run"
        Me.btnRunOutletFinish.UseVisualStyleBackColor = True
        '
        'btnSelectOutlets
        '
        Me.btnSelectOutlets.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectOutlets.Location = New System.Drawing.Point(145, 69)
        Me.btnSelectOutlets.Name = "btnSelectOutlets"
        Me.btnSelectOutlets.Size = New System.Drawing.Size(125, 23)
        Me.btnSelectOutlets.TabIndex = 22
        Me.btnSelectOutlets.Text = "Select Outlets/Inlets"
        Me.btnSelectOutlets.UseVisualStyleBackColor = True
        '
        'btnDrawOutlets
        '
        Me.btnDrawOutlets.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDrawOutlets.Location = New System.Drawing.Point(14, 69)
        Me.btnDrawOutlets.Name = "btnDrawOutlets"
        Me.btnDrawOutlets.Size = New System.Drawing.Size(125, 23)
        Me.btnDrawOutlets.TabIndex = 21
        Me.btnDrawOutlets.Text = "Draw Outlets/Inlets"
        Me.btnDrawOutlets.UseVisualStyleBackColor = True
        '
        'btnBrowseOutlets
        '
        Me.btnBrowseOutlets.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseOutlets.Image = CType(resources.GetObject("btnBrowseOutlets.Image"), System.Drawing.Image)
        Me.btnBrowseOutlets.Location = New System.Drawing.Point(398, 41)
        Me.btnBrowseOutlets.Name = "btnBrowseOutlets"
        Me.btnBrowseOutlets.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseOutlets.TabIndex = 20
        '
        'cmbxOutlets
        '
        Me.cmbxOutlets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxOutlets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxOutlets.Location = New System.Drawing.Point(15, 43)
        Me.cmbxOutlets.Name = "cmbxOutlets"
        Me.cmbxOutlets.Size = New System.Drawing.Size(378, 21)
        Me.cmbxOutlets.TabIndex = 19
        '
        'chkbxUseOutlet
        '
        Me.chkbxUseOutlet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxUseOutlet.Location = New System.Drawing.Point(6, 19)
        Me.chkbxUseOutlet.Name = "chkbxUseOutlet"
        Me.chkbxUseOutlet.Size = New System.Drawing.Size(416, 24)
        Me.chkbxUseOutlet.TabIndex = 18
        Me.chkbxUseOutlet.Text = "Use a Custom Outlets/Inlets Layer"
        '
        'lblSnapThresh
        '
        Me.lblSnapThresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSnapThresh.Location = New System.Drawing.Point(102, 94)
        Me.lblSnapThresh.Name = "lblSnapThresh"
        Me.lblSnapThresh.Size = New System.Drawing.Size(89, 26)
        Me.lblSnapThresh.TabIndex = 32
        Me.lblSnapThresh.Text = "Snap Threshold"
        Me.lblSnapThresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOutlets
        '
        Me.lblOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutlets.Location = New System.Drawing.Point(5, 16)
        Me.lblOutlets.Name = "lblOutlets"
        Me.lblOutlets.Size = New System.Drawing.Size(420, 103)
        Me.lblOutlets.TabIndex = 0
        Me.lblOutlets.Text = "Outlets and Sub-basin Delineation Steps Currently Running"
        Me.lblOutlets.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnAdvanced
        '
        Me.btnAdvanced.Location = New System.Drawing.Point(5, 523)
        Me.btnAdvanced.Name = "btnAdvanced"
        Me.btnAdvanced.Size = New System.Drawing.Size(112, 23)
        Me.btnAdvanced.TabIndex = 26
        Me.btnAdvanced.Text = "Advanced Settings"
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(196, 523)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 32
        Me.btnHelp.Text = "Help"
        Me.btnHelp.Visible = False
        '
        'numProcesses
        '
        Me.numProcesses.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numProcesses.Location = New System.Drawing.Point(123, 496)
        Me.numProcesses.Name = "numProcesses"
        Me.numProcesses.Size = New System.Drawing.Size(40, 20)
        Me.numProcesses.TabIndex = 33
        '
        'lblNumProc
        '
        Me.lblNumProc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNumProc.Location = New System.Drawing.Point(8, 492)
        Me.lblNumProc.Name = "lblNumProc"
        Me.lblNumProc.Size = New System.Drawing.Size(109, 26)
        Me.lblNumProc.TabIndex = 34
        Me.lblNumProc.Text = "Number of processes"
        Me.lblNumProc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'showTaudemOutput
        '
        Me.showTaudemOutput.Location = New System.Drawing.Point(196, 498)
        Me.showTaudemOutput.Name = "showTaudemOutput"
        Me.showTaudemOutput.Size = New System.Drawing.Size(136, 17)
        Me.showTaudemOutput.TabIndex = 35
        Me.showTaudemOutput.Text = "Show TauDEM output"
        Me.showTaudemOutput.UseVisualStyleBackColor = True
        '
        'frmAutomatic_v3
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(440, 549)
        Me.Controls.Add(Me.showTaudemOutput)
        Me.Controls.Add(Me.lblNumProc)
        Me.Controls.Add(Me.numProcesses)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnAdvanced)
        Me.Controls.Add(Me.grpbxOutletDef)
        Me.Controls.Add(Me.grpbxThresh)
        Me.Controls.Add(Me.grpbxSetupPreprocess)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnRunAll)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmAutomatic_v3"
        Me.ShowIcon = False
        Me.Text = "Automatic Watershed Delineation"
        Me.grpbxSetupPreprocess.ResumeLayout(False)
        Me.grpbxSetupPreprocess.PerformLayout()
        Me.grpbxThresh.ResumeLayout(False)
        Me.grpbxThresh.PerformLayout()
        Me.grpbxOutletDef.ResumeLayout(False)
        Me.grpbxOutletDef.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Private showTaudemOutput As System.Windows.Forms.CheckBox
    Friend numProcesses As System.Windows.Forms.TextBox
    Friend lblNumProc As System.Windows.Forms.Label

#End Region

#Region " Copied file type code "
    Private Enum LayerType
        dem = 1
        fel
        p
        sd8
        ang
        slp
        ad8
        sca
        gord
        plen
        tlen
        src
        ord
        w
        ReachShapefile
        SordFromDropan
        AtanB
        dist
        WatershedShapefile
        Outlets
        sllAccum
        fdr
        fdrn
        dsca
        cla
        q
        sord
        tri
        ann
        net
        wshed
        dep
        dg
        di
        tla
        tc
        tdep
        cs
        ctpt
        tsup
        racc
        dmax
        mask
        outletPreview
        mergeWShed
        ss ' = 46
        ssa
        sa
        ass
        rz
        dfs
        dd
        du
        slpd
    End Enum

    Private Function GetFileDescription(ByVal Index As LayerType) As String
        'Returns the description of the raster to be loaded.  Used for the label in the legend.
        Dim Desc As String
        '    Dim LType As LayerType
        '    LType = LoadMapTypes(Index)
        Desc = ""
        Select Case Index
            Case LayerType.dem
                Desc = Caption("dem")
            Case LayerType.fel
                Desc = Caption("fel")
            Case LayerType.p
                Desc = Caption("p")
            Case LayerType.sd8
                Desc = Caption("sd8")
            Case LayerType.ang
                Desc = Caption("ang")
            Case LayerType.slp
                Desc = Caption("slp")
            Case LayerType.ad8
                Desc = Caption("ad8")
            Case LayerType.sca
                Desc = Caption("sca")
            Case LayerType.gord
                Desc = Caption("gord")
            Case LayerType.plen
                Desc = Caption("plen")
            Case LayerType.tlen
                Desc = Caption("tlen")
            Case LayerType.src
                Desc = Caption("src")
            Case LayerType.ord
                Desc = Caption("ord")
            Case LayerType.w
                Desc = Caption("w")
            Case LayerType.ReachShapefile
                Desc = Caption("net")
            Case LayerType.SordFromDropan
                Desc = Caption("SordFromDropan")
            Case LayerType.AtanB
                Desc = Caption("atanb")
            Case LayerType.dist
                Desc = Caption("dist")
            Case LayerType.WatershedShapefile
                Desc = Caption("wshed")
            Case LayerType.Outlets
                Desc = Caption("outletshpfile")
            Case LayerType.sllAccum
                Desc = Caption("sllaccum")
            Case LayerType.fdr
                Desc = Caption("fdr")
            Case LayerType.fdrn
                Desc = Caption("fdrn")
            Case LayerType.dsca
                Desc = Caption("dsca")
            Case LayerType.cla
                Desc = "CLA Grid"
            Case LayerType.q
                Desc = Caption("q")
            Case LayerType.sord
                Desc = "SORD Grid"
            Case LayerType.tri
                Desc = Caption("tri")
            Case LayerType.ann
                Desc = Caption("add")
            Case LayerType.net
                Desc = Caption("net")
            Case LayerType.wshed
                Desc = Caption("wshed")
            Case LayerType.dep
                Desc = Caption("dep")
            Case LayerType.dg
                Desc = Caption("dg")
            Case LayerType.di
                Desc = Caption("di")
            Case LayerType.tla
                Desc = Caption("tla")
            Case LayerType.tc
                Desc = Caption("tc")
            Case LayerType.tdep
                Desc = Caption("tedp")
            Case LayerType.cs
                Desc = Caption("cs")
            Case LayerType.ctpt
                Desc = Caption("ctpt")
            Case LayerType.tsup
                Desc = Caption("tsup")
            Case LayerType.racc
                Desc = Caption("racc")
            Case LayerType.dmax
                Desc = Caption("dmax")
            Case LayerType.mask
                Desc = Caption("mask")
            Case LayerType.outletPreview
                Desc = Caption("outletPreview")
            Case LayerType.mergeWShed
                Desc = Caption("mergeWShed")
            Case LayerType.ss
                Desc = Caption("ss")
            Case LayerType.ssa
                Desc = Caption("ssa")
            Case LayerType.sa
                Desc = Caption("sa")
            Case LayerType.ass
                Desc = Caption("ass")
            Case LayerType.rz
                Desc = Caption("rz")
            Case LayerType.dfs
                Desc = Caption("dfs")
            Case LayerType.dd
                Desc = Caption("dd")
            Case LayerType.du
                Desc = Caption("du")
            Case LayerType.slpd
                Desc = Caption("slpd")
        End Select
        GetFileDescription = Desc
    End Function

    'ARA 8/27/05 Copied from modIODefs.bas
    Public Function Caption(ByVal IO As String) As String
        Select Case IO
            Case "ad8"
                Caption = "D8 Contributing Area Grid (ad8)"
            Case "ang"
                Caption = "Dinf Flow direction Grid (ang)"
            Case "ass"
                Caption = "Avalanche Source Site (ass)"
            Case "atanb"
                Caption = "Wetness Index Grid (atanb)"
            Case "coord"
                Caption = "NetWork Coordinates(nnncoord.dat)"
            Case "cs"
                Caption = "Concentration in supply grid (cs)"
            Case "ctpt"
                Caption = "Concentration Grid (ctpt)"
            Case "dd"
                Caption = "Drop to Stream Grid (dd)"
            Case "decaymult"
                Caption = "Decay Multiplier Grid "
            Case "dem"
                Caption = "Base DEM grid"
            Case "dfs"
                Caption = "Distance from Source Grid (dfs)"
            Case "dep"
                Caption = "Upslope Dependence Grid (dep)"
            Case "dg"
                Caption = "Disturbance Indicator Grid (dg)"
            Case "di"
                Caption = "Downslope influence Grid (di)"
            Case "dist"
                Caption = "Distance to Stream Grid (dist)"
            Case "dmax"
                Caption = "Maximum Downslope Grid (dmax)"
            Case "dsca"
                Caption = "Decayed Specific Catchment Area Grid (dsca)"
            Case "du"
                Caption = "Distance Up Grid (du)"
            Case "fdr"
                Caption = "Flow Path Grid (fdr)"
            Case "fdrn"
                Caption = "Verified Flow Path Grid (fdrn)"
            Case "fel"
                Caption = "Pit Filled Elevation Grid (fel)"
            Case "gord"
                Caption = "Strahler Network Order Grid (gord)"
            Case "mask"
                Caption = "Focus Mask"
            Case "net"
                Caption = "Stream Reach Shapefile (net)"
            Case "outletshpfile"
                Caption = "Outlets/Inlets ShapeFile"
            Case "ord"
                Caption = "Stream Order Grid (ord)"
            Case "p"
                Caption = "D8 Flow Direction Grid (p)"
            Case "plen"
                Caption = "Longest Upslope length Grid (plen)"
            Case "q"
                Caption = "Weighted Accumulation Grid (q)"
            Case "racc"
                Caption = "Reverse Accmulation Grid (racc)"
            Case "rz"
                Caption = "Avalanch Runout Zone (rz)"
            Case "sca"
                Caption = "Dinf Specific Catchment Area Grid  (sca)"
            Case "sd8"
                Caption = "D8 Slope Grid (sd8)"
            Case "slp"
                Caption = "DInf Slope Grid (slp)"
            Case "slpd"
                Caption = "Slope Average Down Grid (slpd)"
            Case "src"
                Caption = "Stream Raster Grid (src)"
            Case "ss"
                Caption = "Stream Source Grid (ss)"
            Case "ssa"
                Caption = "Accumulated Stream Source Grid (ssa)"
            Case "sa"
                Caption = "Slope Area Grid (sa)"
            Case "tc"
                Caption = "Tansport Capacity Grid (tc)"
            Case "tdep"
                Caption = "Deposition Grid"
            Case "tla"
                Caption = "Transport Limited Accumulation Grid (tla)"
            Case "tlen"
                Caption = "Total Upslope Length Grid (tlen)"
            Case "tree"
                Caption = "Network Tree(nnnntree.dat)"
            Case "tsup"
                Caption = "Supply Grid (tsup)"
            Case "w"
                Caption = "Watershed Grid (w)"
            Case "weights"
                Caption = "Weight grid "
            Case "wshed"
                Caption = "Watershed Shapefile"
            Case "mask"
                Caption = "Focus Area Mask"
            Case "outletPreview"
                Caption = "Outlets/Inlets Snap Preview"
            Case "mergeWShed"
                Caption = "Outlet Merged Watershed"
            Case Else
                Caption = IO & " Layer"
        End Select
    End Function
#End Region

#Region "Class Variables"
    Private ProgressHandler As TaudemPlugin
    Private frmSettings As New frmAdvancedOptions_v3

    ' debug tick timer items
    Private os As System.IO.StreamWriter
    Private tickb, ticka, tickd As Long
    Private doTicks As Boolean = False
    Private timepath As String = System.IO.Path.Combine(System.IO.Path.GetTempPath, "taudem_timing.txt")
#End Region

#Region "Helper Functions"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: Initialize
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function called to class by setting the wrapper
    '   link used for adding and removing layers from MapWindow. It also
    '   Does the initial filling, selecting, and enabling of list boxes
    '
    ' INPUTS:   wrapper      Used to set the class variable myWrapper
    '
    ' OUTPUTS:  None
    '
    ' NOTES: This has to be called before any layers are added or removed
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 07/03/2006    ARA             Runs form cleanup on initialize to fix any strange errors of locked state
    ' 08/09/2006    ARA             Removed edge contam tooltip as it moved to options form
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub Initialize(ByVal wrapper As TaudemPlugin)
        App = wrapper.App
        ProgressHandler = wrapper

        If doTicks Then
            os = New System.IO.StreamWriter(timepath)
        End If
        cmbxElevUnits.SelectedIndex = 0
        fillCombos()

        LoadFromChoices()
        txtbxThreshold.Text = lastThresh
        ttip.SetToolTip(lblSelDem, "Select a projected Digital Elevation Model grid to delineate from.")
        ttip.SetToolTip(cmbxSelDem, "Select a projected Digital Elevation Model grid to delineate from.")
        ttip.SetToolTip(cmbxElevUnits, "Select the elevation units of the projected grid for correct calculations.")
        ttip.SetToolTip(lblElevUnits, "Select the elevation units of the projected grid for correct calculations.")
        ttip.SetToolTip(chkbxUseOutlet, "Use this option to limit output to only basins" + vbNewLine + "connected to outlet points and exclude basins" + vbNewLine + "connected to inlet points in a point shapefile")
        ttip.SetToolTip(cmbxOutlets, "Use this option to limit output to only basins" + vbNewLine + "connected to outlet points and exclude basins" + vbNewLine + "connected to inlet points in a point shapefile")
        ttip.SetToolTip(chkbxBurnStream, "Use this option to burn in a stream network to" + vbNewLine + "shape portions of the taudem created streams" + vbNewLine + "using a canyon burn-in that lowers elevation" + vbNewLine + "under the stream polyline.")
        ttip.SetToolTip(cmbxStream, "Use this option to burn in a stream network to" + vbNewLine + "shape portions of the taudem created streams" + vbNewLine + "using a canyon burn-in that lowers elevation" + vbNewLine + "under the stream polyline.")
        ttip.SetToolTip(chkbxMask, "Use this option to limit the areas to be delineated to" + vbNewLine + "the current zoom extents, selected polygons" + vbNewLine + "from a shapefile, or the non-nodata values of" + vbNewLine + "a masking grid.")
        ttip.SetToolTip(btnSetExtents, "Click this to zoom and set extents to use for the focusing mask.")
        ttip.SetToolTip(btnLoadPre, "Click this to load pre-existing intermediate pre-processing" + vbNewLine + "files generated from a previous run of the AWD on the base" + vbNewLine + "grid. This allows the skipping of the time-consuming" + vbNewLine + "pre-processing steps.")

        runFormCleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: fillCombos
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Subroutine used to fill the three comboboxes
    '   with existing layers in MapWindow which match the type
    '   needed (grid for Base Dem and Flow grid, point shape for
    '   the outlets file). Also used after adding new layers in
    '   order to repopulate the comboboxes correctly.
    '
    ' INPUTS: None
    '
    ' OUTPUTS: None
    '
    ' NOTES: The user is capable of browsing and selecting shape
    '   files of a non-point format, but this function will not
    '   add them to the outlets combobox as the layertype will
    '   not be the same.
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 09/05/2006    ARA             Fixed to use getgridobject instead of opening the grid object for projection check
    ' 10/02/2006    ARA             Fixed bug where layer index handles get messed up after removed layer. Have to use OCX.getlayerhandle
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub fillCombos()

        cmbxSelDem.Items.Clear()
        cmbxSelDem.Items.Add("Select a DEM Grid")
        cmbxMask.Items.Clear()
        cmbxMask.Items.Add("Select a Mask Grid or Polygon Shapefile or Use Extents")
        cmbxStream.Items.Clear()
        cmbxStream.Items.Add("Select a Stream Polyline Shapefile")
        cmbxOutlets.Items.Clear()
        cmbxOutlets.Items.Add("Select a Point Shapefile, then Select or Draw Outlets/Inlets")

        For Each layer As IMapLayer In App.Map.Layers

            ' names may only be a file name without extension, which causes ambiguity
            Dim item As String = layer.LegendText  'IO.Path.GetFileName(layer.FileName)

            If Not TryCast(layer, MapPointLayer) Is Nothing Then
                cmbxOutlets.Items.Add(item)

            ElseIf Not TryCast(layer, MapLineLayer) Is Nothing Then
                cmbxStream.Items.Add(item)

            ElseIf Not TryCast(layer, MapPolygonLayer) Is Nothing Then
                cmbxMask.Items.Add(item)

            ElseIf Not TryCast(layer, MapRasterLayer) Is Nothing Then

                ' This looks like a hacky hack
                ' see if the number is > 999 ???
                ' probably if the coordinate system is the lat long system
                ' -180 +180 X
                ' -90 +90 for Y
                ' then they think it is not a DEM
                Dim raster As MapRasterLayer = layer
                Dim index = raster.DataSet.CellToProj(1, 1)
                Dim projCheck As Double = System.Math.Abs(System.Math.Floor(index.Y))
                Dim strProj As String = projCheck.ToString()

                If strProj.Length > 3 Then
                    cmbxSelDem.Items.Add(item)
                    cmbxMask.Items.Add(item)
                End If

            End If
        Next

        loadCombosToLast()
    End Sub


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: SaveToChoices
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub called to set the appropriate tdbchoices properties
    '   based on the checkboxes selected
    '
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 08/08/2006    ARA             Moved edge cont checkbox to options form
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub SaveToChoices()
        tdbChoiceList.useOutlets = chkbxUseOutlet.Checked
        tdbChoiceList.useMaskFileOrExtents = chkbxMask.Checked
        tdbChoiceList.useExtentMask = rdobtnUseExtents.Checked
        tdbChoiceList.useBurnIn = chkbxBurnStream.Checked
        Try
            tdbChoiceList.snapThresh = System.Convert.ToDouble(txtbxSnapThresh.Text)
        Catch e As Exception
            tdbChoiceList.snapThresh = 300
        End Try
        tdbChoiceList.ShowTaudemOutput = showTaudemOutput.Checked

    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: LoadFromChoices
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub called to set the appropriate checkboxes based on the
    '   values initialized into the tdbChoiceList from the config file
    '
    ' INPUTS:   wrapper      Used to set the class variable myWrapper
    '
    ' OUTPUTS:  None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 08/08/2006    ARA             Moved edge cont checkbox to options form
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub LoadFromChoices()
        chkbxUseOutlet.Checked = tdbChoiceList.useOutlets
        chkbxBurnStream.Checked = tdbChoiceList.useBurnIn
        chkbxMask.Checked = tdbChoiceList.useMaskFileOrExtents
        rdobtnUseExtents.Checked = tdbChoiceList.useExtentMask
        rdobtnUseFileMask.Checked = Not tdbChoiceList.useExtentMask
        txtbxSnapThresh.Text = tdbChoiceList.snapThresh.ToString()
        numProcesses.Text = tdbChoiceList.numProcesses.ToString()
        showTaudemOutput.Checked = tdbChoiceList.ShowTaudemOutput
        loadCombosToLast()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: loadCombosToLast
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to load the combo boxes from their respective last set values
    '  So that after a fillCombos, the values won't have changed.
    '
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub loadCombosToLast()
        If Not lastDem = "" Then
            cmbxSelDem.SelectedIndex = cmbxSelDem.Items.IndexOf(lastDem)
            If cmbxSelDem.SelectedIndex = -1 Then
                cmbxSelDem.SelectedIndex = 0
            End If
        Else
            cmbxSelDem.SelectedIndex = 0
        End If

        If Not lastThresh = "" Then
            txtbxThreshold.Text = lastThresh
        End If
        If lastConvUnit = -1 Then
            cmbxThreshConvUnits.SelectedIndex = 0
        Else
            cmbxThreshConvUnits.SelectedIndex = lastConvUnit
        End If


        If lastMask = "" Then
            If cmbxMask.Items.Count > 0 Then
                cmbxMask.SelectedIndex = 0
            End If
        Else
            cmbxMask.SelectedIndex = cmbxMask.Items.IndexOf(lastMask)
            If cmbxMask.SelectedIndex = -1 Then
                cmbxMask.SelectedIndex = 0
            End If
        End If

        If lastStream = "" Then
            If cmbxStream.Items.Count > 0 Then
                cmbxStream.SelectedIndex = 0
            End If
        Else
            cmbxStream.SelectedIndex = cmbxStream.Items.IndexOf(lastStream)
            If cmbxStream.SelectedIndex = -1 Then
                cmbxStream.SelectedIndex = 0
            End If
        End If

        If lastOutlet = "" Then
            If cmbxOutlets.Items.Count > 0 Then
                cmbxOutlets.SelectedIndex = 0
            End If
        Else
            cmbxOutlets.SelectedIndex = cmbxOutlets.Items.IndexOf(lastOutlet)
            If cmbxOutlets.SelectedIndex = -1 Then
                cmbxOutlets.SelectedIndex = 0
            End If
        End If

    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getPathByName
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will cycle the existing layers in MapWindow and
    '   return the layer filename/path of a layer with a matching name
    '
    ' INPUTS: strName : A name string to look for on the layers
    '
    ' OUTPUTS: A string containing the name of the layer with the path
    '
    ' NOTES: 
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function getPathByName(ByVal layerName As String) As String
        Dim layer As IMapLayer = App.Map.Layers.Where(Function(p) p.LegendText = layerName).First()

        If layer IsNot Nothing Then
            Return DotSpatial.Plugins.Taudem.TaudemHelpers.GetFileName(layer.DataSet)
        End If

        Return Nothing
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getNameByPath
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will cycle the existing layers in MapWindow and
    '   return the layer name of a layer with a matching filename path
    '
    ' INPUTS: strPath : A file path string to look for on the layers
    '
    ' OUTPUTS: A string containing the name of the layer with the path
    '
    ' NOTES: It is conceivable that you could have two layers with the
    '   same path, which would make this take only the first one it hits.
    '   Could be a problem.
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function getNameByPath(ByVal path As String) As String
        For Each layer As IMapLayer In App.Map.Layers
            If DotSpatial.Plugins.Taudem.TaudemHelpers.GetFileName(layer.DataSet) = path Then
                Return layer.LegendText
            End If
        Next
        Return String.Empty
    End Function



    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: LayerAlreadyInLegend
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will cycle the existing layers in MapWindow and
    '   return true if the inputed path is found as an existing
    '   layer.
    '
    ' INPUTS: strPath : A file path string to look for on the layers
    '
    ' OUTPUTS: Boolean true if path found, false otherwise.
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function LayerAlreadyInLegend(ByVal path As String) As Boolean
        Return getNameByPath(path) <> String.Empty
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getNumCellsByDEMAndMask
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will find the number of cells of a grid and if a mask is used
    '   determine the number of cells under the mask. For calculating default threshold
    '   value and limits
    '
    ' INPUTS: head : Header of the grid to find the cell count of for default grid
    '
    ' OUTPUTS: Number of cells as an area
    '
    ' NOTES: If using mask, cells are calculated by area of shapes or by the number of
    '  non-nodata value cells if a grid
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 07/24/2006    ARA             Added header. don't recall when created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function GetNumCellsByDEMAndMask(ByVal bounds As RasterBounds) As Int32
        Dim numCells As Int32
        'Dim maskCells As Int32
        'Dim strMask As String

        numCells = bounds.NumColumns * bounds.NumRows

        If chkbxMask.Checked Then
            '    If rdobtnUseExtents.Checked Then
            '        maskCells = ((App.Map.View.Extents.xMax - App.Map.View.Extents.xMin) \ r.CellWidth) * ((App.Map.View.Extents.yMax - App.Map.View.Extents.yMin) \ r.CellHeight)
            '        If numCells > maskCells Then
            '            numCells = maskCells
            '        End If
            '    Else
            '        If cmbxMask.SelectedIndex > 0 Then
            '            strMask = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
            '            If IO.Path.GetExtension(strMask) = ".shp" Then
            '                If myWrapper.maskShapesIdx.Count > 0 Then
            '                    maskCells = 0
            '                    Dim sf As New MapWinGIS.Shapefile
            '                    sf.Open(strMask)
            '                    For i As Integer = 0 To myWrapper.maskShapesIdx.Count - 1
            '                        maskCells = maskCells + MapWinGeoProc.Utils.Area(sf.Shape(myWrapper.maskShapesIdx(i))) / (r.CellWidth * r.CellHeight)
            '                    Next
            '                    sf.Close()
            '                    If numCells > maskCells Then
            '                        numCells = maskCells
            '                    End If
            '                End If
            '            Else
            '                Dim g As New MapWinGIS.Grid
            '                Dim maskHead As MapWinGIS.GridHeader
            '                g.Open(strMask)
            '                maskHead = g.Header
            '                maskCells = 0
            '                For row As Integer = 0 To maskHead.NumberRows - 1
            '                    For col As Integer = 0 To maskHead.NumberCols - 1
            '                        If g.Value(col, row) <> maskHead.NodataValue Then
            '                            maskCells = maskCells + 1
            '                        End If
            '                    Next
            '                Next
            '                g.Close()
            '                If numCells > maskCells Then
            '                    numCells = maskCells
            '                End If
            '            End If
            '        End If
            '    End If
        End If
        Return numCells
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateCellThreshAndSet
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to validate the cell threshold entered, raise or lower
    '  it to the minimum or maximum if below or above, and calculate and set the
    '  equivalent converted threshold according to the selected type.
    '
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  boolean true on success
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/05/2006    ARA             Added square miles
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateCellThreshAndSet() As Boolean
        Dim convVal As Double

        If cmbxSelDem.SelectedIndex > 0 Then
            Dim strGrid As String = getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex).ToString())

            Dim r As IRaster = Raster.Open(strGrid)

            Dim numCells As Integer = GetNumCellsByDEMAndMask(r.Bounds)
            Dim minVal, maxVal, currVal, defaultVal As Integer
            maxVal = CInt(numCells / 2)
            minVal = CInt(maxVal * 0.001)
            defaultVal = CInt(maxVal * 0.02)

            ttip.SetToolTip(txtbxThreshold, "Min: " + minVal.ToString() + " cells  Max: " + maxVal.ToString() + " cells")
            ttip.SetToolTip(txtNumCells, "This value is the threshold of grid cells feeding into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString() + "  Max: " + maxVal.ToString())
            ttip.SetToolTip(grpbxThresh, "This value is the threshold of grid cells feeding into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString() + "  Max: " + maxVal.ToString())

            If txtbxThreshold.Text <> "" Then
                Try
                    currVal = System.Convert.ToInt32(txtbxThreshold.Text)
                Catch e As Exception
                    currVal = defaultVal
                End Try

                If currVal > maxVal Then
                    currVal = maxVal
                End If
                If currVal < minVal Then
                    currVal = minVal
                End If
                txtbxThreshold.Text = currVal.ToString()
                tdbChoiceList.Threshold = currVal
            Else
                If lastThresh = "" Then
                    txtbxThreshold.Text = defaultVal.ToString()
                    tdbChoiceList.Threshold = defaultVal
                Else
                    Try
                        tdbChoiceList.Threshold = CSng(lastThresh)
                        txtbxThreshold.Text = lastThresh
                    Catch e As Exception
                        tdbChoiceList.Threshold = defaultVal
                        txtbxThreshold.Text = defaultVal.ToString()
                    End Try
                End If
            End If

            convVal = r.Bounds.CellWidth * r.Bounds.CellHeight * System.Convert.ToInt32(txtbxThreshold.Text)
            If r.Projection.Unit.Name = "Meter" Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then
                    txtbxThreshConv.Text = (convVal / 2589988.110336).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                    txtbxThreshConv.Text = (convVal / 4046.8564224).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                    txtbxThreshConv.Text = (convVal / 10000).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                    txtbxThreshConv.Text = (convVal / 1000000).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                    txtbxThreshConv.Text = convVal.ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                    txtbxThreshConv.Text = (convVal * 10.763910417).ToString("F4")
                End If
            ElseIf r.Projection.Unit.Name = "Foot" Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then
                    txtbxThreshConv.Text = (convVal / 27878400).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                    txtbxThreshConv.Text = (convVal / 43560).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                    txtbxThreshConv.Text = (convVal / 107639.104167097).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                    txtbxThreshConv.Text = (convVal / 10763910.416709721).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                    txtbxThreshConv.Text = (convVal * 0.09290304).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                    txtbxThreshConv.Text = convVal.ToString("F4")
                End If
            End If
            If txtbxThreshold.Text <> lastThresh Then
                threshDelinHasRan = False
            End If
            lastThresh = txtbxThreshold.Text
            r.Close()
            Return True

        Else
            txtbxThreshold.Text = ""
            tdbChoiceList.Threshold = 0
            Return False
        End If

    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateConvThreshAndSet
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to validate the converted threshold entered, raise or lower
    '  it to the minimum or maximum if below or above, and calculate and set the
    '  equivalent threshold in cells according to the selected type.
    '
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  boolean true on success
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/05/2006    ARA             Added square miles
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateConvThreshAndSet() As Boolean
        If cmbxSelDem.SelectedIndex > 0 Then
            Dim strGrid As String = getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex))

            Dim r As IRaster = Raster.Open(strGrid)

            Dim numCells As Integer = GetNumCellsByDEMAndMask(r.Bounds)
            Dim convVal As Double = r.CellWidth * r.CellHeight * numCells
            Dim minVal, maxVal, currVal, defaultVal As Double

            If r.Projection.Unit.Name = "Meter" Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then 'acres
                    maxVal = (convVal / 2) / 2589988.110336
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. mi  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square miles flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then 'sq km
                    maxVal = (convVal / 2) / 4046.8564224
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " acres  Max: " + maxVal.ToString("F4") + " acres")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in acres flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then 'hectare
                    maxVal = (convVal / 2) / 10000
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " hectares  Max: " + maxVal.ToString("F4") + " hectares")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in hectares flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then 'sq m
                    maxVal = (convVal / 2) / 1000000
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. km  Max: " + maxVal.ToString("F4") + " sq. km")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square kilometers flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then 'sq ft
                    maxVal = convVal / 2
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. m  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square meters flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then 'sq mi
                    maxVal = (convVal / 2) * 10.763910417
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. ft  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square feet flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                End If
            ElseIf r.Projection.Unit.Name = "Foot" Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then 'acres
                    maxVal = (convVal / 2) / 27878400
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. mi  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square miles flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then 'sq km
                    maxVal = (convVal / 2) / 43560
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " acres  Max: " + maxVal.ToString("F4") + " acres")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in acres flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then 'hectare
                    maxVal = (convVal / 2) / 107639.1041670972
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " hectares  Max: " + maxVal.ToString("F4") + " hectares")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in hectares flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then 'sq m
                    maxVal = (convVal / 2) / 10763910.416709721
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. km  Max: " + maxVal.ToString("F4") + " sq. km")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square kilometers flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then 'sq ft
                    maxVal = (convVal / 2) * 0.09290304
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. m  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square meters flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then 'sq ft
                    maxVal = convVal / 2
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. ft  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square feet flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                End If
            End If


            If txtbxThreshConv.Text <> "" Then
                Try
                    currVal = System.Convert.ToDouble(txtbxThreshConv.Text)
                Catch e As Exception
                    currVal = defaultVal
                End Try

                If currVal > maxVal Then
                    currVal = maxVal
                End If
                If currVal < minVal Then
                    currVal = minVal
                End If
                txtbxThreshConv.Text = currVal.ToString()

                If r.Projection.Unit.Name = "Meter" Then
                    If cmbxThreshConvUnits.SelectedIndex = 0 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 2589988.110336) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 4046.8564224) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 10000) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 1000000) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32(currVal / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal / 10.763910417) / (r.CellWidth * r.CellHeight)))
                    End If
                ElseIf r.Projection.Unit.Name = "Foot" Then
                    If cmbxThreshConvUnits.SelectedIndex = 0 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 27878400) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 43560) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 107639.1041670972) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 10763910.416709721) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal / 0.09290304) / (r.CellWidth * r.CellHeight)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32(currVal / (r.CellWidth * r.CellHeight)))
                    End If
                End If
            Else
                validateCellThreshAndSet()
            End If
            ' CWG added 9 October 2010: fixes bug 0000924
            ' since cell threshold may have changed, save it to avoid going back to earlier value
            lastThresh = txtbxThreshold.Text
            Return True
        Else
            txtbxThreshConv.Text = ""
            Return False
        End If
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: setMaskSelected
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A public sub called from the interface layer when finished
    '  selecting or drawing Masks, which is used to populate the selected mask
    '  list and highlight the masks which were selected.
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/03/2006    ARA             Modified to select all on drawing
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub setMaskSelected()
        'Dim sf As New MapWinGIS.Shapefile
        'If g_DrawingMask Then
        '    sf.Open(currDrawPath)
        '    App.Map.View.SelectedShapes.ClearSelectedShapes()
        '    For i As Integer = 0 To sf.NumShapes - 1
        '        App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
        '    Next
        '    sf.Close()
        'End If

        'If g_SelectingMask Or g_DrawingMask Then
        '    myWrapper.maskShapesIdx.Clear()
        '    For i As Integer = 0 To App.Map.View.SelectedShapes.NumSelected - 1
        '        myWrapper.maskShapesIdx.Add(App.Map.View.SelectedShapes(i).ShapeIndex)
        '    Next
        'End If

        'If g_SelectingMask Or g_DrawingMask Then
        '    App.Map.View.SelectedShapes.ClearSelectedShapes()
        '    For i As Integer = 0 To myWrapper.maskShapesIdx.Count - 1
        '        App.Map.View.SelectedShapes.AddByIndex(myWrapper.maskShapesIdx.Item(i), Drawing.Color.Yellow)
        '    Next
        'End If
        'lblMaskSelected.Text = myWrapper.maskShapesIdx.Count.ToString + " selected"
        'App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
        'If cmbxSelDem.SelectedIndex > 0 Then
        '    validateCellThreshAndSet()
        '    validateConvThreshAndSet()
        'End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: setOutletsSelected
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A public sub called from the interface layer when finished
    '  selecting or drawing outlets, which is used to populate the selected outlets
    '  list and highlight the outlets which were selected.
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/03/2006    ARA             Modified to select all on drawing
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Public Sub setOutletsSelected()
    '    Dim i As Integer
    '    Dim sf As New MapWinGIS.Shapefile
    '    If g_DrawingOutletsOrInlets Then
    '        sf.Open(currDrawPath)
    '        App.Map.View.SelectedShapes.ClearSelectedShapes()
    '        For i = 0 To sf.NumShapes - 1
    '            App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '        Next
    '        sf.Close()
    '    End If

    '    If g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
    '        myWrapper.outletShapesIdx.Clear()
    '        For i = 0 To App.Map.View.SelectedShapes.NumSelected - 1
    '            myWrapper.outletShapesIdx.Add(App.Map.View.SelectedShapes(i).ShapeIndex)
    '        Next
    '    End If

    '    If g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
    '        sf.Open(currDrawPath)

    '        Dim inletfieldnum As Integer = -1
    '        For z As Integer = 0 To sf.NumFields - 1
    '            If sf.Field(z).Name = "INLET" Then
    '                inletfieldnum = z
    '                Exit For
    '            End If
    '        Next

    '        App.Map.View.SelectedShapes.ClearSelectedShapes()
    '        For i = 0 To myWrapper.outletShapesIdx.Count - 1
    '            If sf.CellValue(inletfieldnum, myWrapper.outletShapesIdx(i)) = 0 Then
    '                App.Map.View.SelectedShapes.AddByIndex(myWrapper.outletShapesIdx.Item(i), Drawing.Color.Yellow)
    '            Else
    '                App.Map.View.SelectedShapes.AddByIndex(myWrapper.outletShapesIdx.Item(i), Drawing.Color.Goldenrod)
    '            End If
    '        Next
    '        sf.Close()
    '    End If

    '    lblOutletSelected.Text = myWrapper.outletShapesIdx.Count.ToString + " selected"
    '    App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
    'End Sub

    Private Function AddMap(ByVal fname As String, ByVal gtype As LayerType) As Boolean
        Dim Message As String
        '3/16/2005 - dpa - Updated add map functionality for MapWindow 3.1 plugin wrapper.
        If (tdbFileList.getext(fname) = "") Then
            Message = fname & "\sta.adf"
        Else
            Message = fname
        End If
        Message = Message & "|" & GetFileDescription(gtype) & "|" & CLng(gtype)
        App.ProgressHandler.Progress("Add", 0, Message)
        Return True
    End Function

    Public Function RemoveLayer(ByVal fname As String, Optional ByVal toPrompt As Boolean = True) As Boolean
        App.ProgressHandler.Progress("Remove", 0, fname)
        RemoveLayer = True
    End Function

    Private Sub closingCleanup()
        If cmbxSelDem.SelectedIndex > 0 Then
            lastDem = cmbxSelDem.Items.Item(cmbxSelDem.SelectedIndex).ToString()
        Else
            lastDem = ""
        End If
        If cmbxMask.SelectedIndex > 0 Then
            lastMask = cmbxMask.Items(cmbxMask.SelectedIndex).ToString()
        Else
            lastMask = ""
        End If
        If cmbxStream.SelectedIndex > 0 Then
            lastStream = cmbxStream.Items.Item(cmbxStream.SelectedIndex).ToString()
        Else
            lastStream = ""
        End If
        If cmbxOutlets.SelectedIndex > 0 Then
            lastOutlet = cmbxOutlets.Items.Item(cmbxOutlets.SelectedIndex).ToString()
        Else
            lastOutlet = ""
        End If
        lastThresh = txtbxThreshold.Text
        SaveToChoices()
        tdbChoiceList.SaveConfig()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runFormInit
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to initialize the AWD v3 form and the tdbChoicelist
    '
    ' INPUTS: None
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub runFormInit()
        Cursor = Windows.Forms.Cursors.WaitCursor
        btnRunAll.Enabled = False
        btnCancel.Enabled = False
        btnAdvanced.Enabled = False
        btnHelp.Enabled = False
        grpbxSetupPreprocess.Enabled = False
        grpbxThresh.Enabled = False
        grpbxOutletDef.Enabled = False
        SaveToChoices()
        tdbChoiceList.SaveConfig()
        Me.Refresh()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runFormCleanup
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to clean up the AWD v3 form
    '
    ' INPUTS: None
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub runFormCleanup()
        App.ProgressHandler.Progress("Status", 0, "")
        Cursor = Windows.Forms.Cursors.Default
        btnRunAll.Enabled = True
        btnCancel.Enabled = True
        btnAdvanced.Enabled = True
        btnHelp.Enabled = True
        lblPreproc.Visible = False
        lblDelin.Visible = False
        lblOutlets.Visible = False
        grpbxSetupPreprocess.Enabled = True
        grpbxThresh.Enabled = True
        grpbxOutletDef.Enabled = True
        Me.Refresh()
    End Sub

#End Region

#Region "Event Handlers"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshold_KeyDown
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture enter key being pressed
    '   and send a tab command to go to the next tabindex item
    '
    ' INPUTS: e is used for keycode eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshold_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbxThreshold.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Return Then
            Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshold_KeyPress
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to stop any non number or control
    '   values from being added to the textbox
    '
    ' INPUTS: e is used for keychar eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshold_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxThreshold.KeyPress
        If Char.IsNumber(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False Then
            e.Handled = True
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshold_Leave
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture leaving the threshold textbox and
    '  call the validate and reset sub
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshold_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxThreshold.Leave
        validateCellThreshAndSet()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshConv_KeyDown
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture enter key being pressed
    '   and send a tab command to go to the next tabindex item
    '
    ' INPUTS: e is used for keycode eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshConv_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbxThreshConv.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Return Then
            Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshConv_KeyPress
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to stop any non number or control
    '   values from being added to the textbox
    '
    ' INPUTS: e is used for keychar eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshConv_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxThreshConv.KeyPress
        If Char.IsNumber(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshConv_Leave
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture leaving the converted threshold textbox and
    '  call the validate and reset sub
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshConv_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxThreshConv.Leave
        validateConvThreshAndSet()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxSnapThresh_KeyPress
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to stop any non number or control
    '   values from being added to the textbox
    '
    ' INPUTS: e is used for keychar eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/01/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxSnapThresh_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxSnapThresh.KeyPress
        If Char.IsNumber(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxSnapThresh_KeyDown
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture enter key being pressed
    '   and send a tab command to go to the next tabindex item
    '
    ' INPUTS: e is used for keycode eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/01/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxSnapThresh_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbxSnapThresh.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Return Then
            Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtbxSnapThresh_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxSnapThresh.TextChanged
        threshDelinHasRan = False
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxSelDem_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the select dem list being changed
    '   merely sets the last dem to the one selected.
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxSelDem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxSelDem.SelectedIndexChanged
        Dim tmpPath, tmpLastDem As String
        If cmbxSelDem.SelectedIndex > 0 Then
            If cmbxSelDem.Items.Item(cmbxSelDem.SelectedIndex) <> lastDem Then
                tmpPath = getPathByName(cmbxSelDem.Items.Item(cmbxSelDem.SelectedIndex))
                tdbFileList.dem = tmpPath
            End If

            tmpLastDem = lastDem
            lastDem = cmbxSelDem.Items(cmbxSelDem.SelectedIndex)

            If tmpLastDem <> lastDem Then
                preProcHasRan = False
                threshDelinHasRan = False

                If getPathByName(lastDem).ToLower.Contains("ned") Then
                    cmbxElevUnits.SelectedIndex = 1
                Else
                    cmbxElevUnits.SelectedIndex = 0
                End If

                validateCellThreshAndSet()
                validateConvThreshAndSet()
            End If
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxStream_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the last stream on new selection
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 07/24/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxStream_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxStream.SelectedIndexChanged
        If cmbxStream.SelectedIndex > 0 Then
            lastStream = cmbxStream.Items(cmbxStream.SelectedIndex)
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxOutlets_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: ensure the currently selected outlet will always be snapped correctly
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub cmbxOutlets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxOutlets.SelectedIndexChanged
    '    
    '    If cmbxOutlets.SelectedIndex > 0 Then
    '        Dim sf As New MapWinGIS.Shapefile
    '        Dim currOutlet, strpath As String
    '        currOutlet = cmbxOutlets.Items(cmbxOutlets.SelectedIndex)
    '        strpath = getPathByName(currOutlet)
    '        App.Map.Layers.CurrentLayer = getIndexByPath(strpath)
    '        If currOutlet = lastOutlet Then
    '            If myWrapper.outletShapesIdx.Count > 0 Then
    '                App.Map.View.SelectedShapes.ClearSelectedShapes()
    '                For i As Integer = 0 To myWrapper.outletShapesIdx.Count - 1
    '                    App.Map.View.SelectedShapes.AddByIndex(myWrapper.outletShapesIdx(i), Drawing.Color.Yellow)
    '                Next
    '            Else
    '                sf.Open(strpath)
    '                App.Map.View.SelectedShapes.ClearSelectedShapes()
    '                For i As Integer = 0 To sf.NumShapes - 1
    '                    App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '                Next
    '                sf.Close()
    '            End If
    '        Else
    '            lastOutlet = currOutlet
    '            sf.Open(strpath)
    '            App.Map.View.SelectedShapes.ClearSelectedShapes()
    '            For i As Integer = 0 To sf.NumShapes - 1
    '                App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '            Next
    '            sf.Close()
    '        End If
    '        g_SelectingOutlets = True
    '        setOutletsSelected()
    '        g_SelectingOutlets = False
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxThreshConvUnits_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the select threshold conversion units which
    '  triggers the cell validate to reset the converted threshold textbox
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxThreshConvUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxThreshConvUnits.SelectedIndexChanged
        validateCellThreshAndSet()
        validateConvThreshAndSet()
        lastConvUnit = cmbxThreshConvUnits.SelectedIndex
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxMask_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the threshold values when masks change
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/19/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub cmbxMask_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxMask.SelectedIndexChanged
    '    If cmbxMask.SelectedIndex > 0 Then
    '        Dim sf As New MapWinGIS.Shapefile
    '        Dim currMask, strpath As String
    '        currMask = cmbxMask.Items(cmbxMask.SelectedIndex)
    '        strpath = getPathByName(currMask)
    '        App.Map.Layers.CurrentLayer = getIndexByPath(strpath)
    '        If currMask = lastMask Then
    '            If IO.Path.GetExtension(strpath) = ".shp" Then
    '                If myWrapper.maskShapesIdx.Count > 0 Then
    '                    App.Map.View.SelectedShapes.ClearSelectedShapes()
    '                    For i As Integer = 0 To myWrapper.maskShapesIdx.Count - 1
    '                        App.Map.View.SelectedShapes.AddByIndex(myWrapper.maskShapesIdx(i), Drawing.Color.Yellow)
    '                    Next
    '                Else
    '                    sf.Open(strpath)
    '                    App.Map.View.SelectedShapes.ClearSelectedShapes()
    '                    For i As Integer = 0 To sf.NumShapes - 1
    '                        App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '                    Next
    '                    sf.Close()
    '                End If
    '                g_SelectingMask = True
    '                setMaskSelected()
    '                g_SelectingMask = False
    '            End If
    '        Else
    '            lastMask = currMask
    '            If IO.Path.GetExtension(strpath) = ".shp" Then
    '                sf.Open(strpath)
    '                App.Map.View.SelectedShapes.ClearSelectedShapes()
    '                For i As Integer = 0 To sf.NumShapes - 1
    '                    App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '                Next
    '                sf.Close()
    '                g_SelectingMask = True
    '                setMaskSelected()
    '                g_SelectingMask = False
    '            End If
    '        End If

    '        'txtbxThreshold.Text = ""
    '        'lastThresh = ""
    '        validateCellThreshAndSet()
    '        validateConvThreshAndSet()
    '    Else
    '        lastMask = ""
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: chkbxMask_CheckedChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the threshold values when masks change
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/19/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub chkbxMask_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxMask.CheckedChanged
    '    If cmbxSelDem.SelectedIndex > 0 Then
    '        'txtbxThreshold.Text = ""
    '        'lastThresh = ""
    '        validateCellThreshAndSet()
    '        validateConvThreshAndSet()
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: rdobtnUseExtents_CheckedChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the threshold values when masks change and enable
    '  the associated components for opening a mask file
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/20/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub rdobtnUseExtents_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnUseExtents.CheckedChanged
        cmbxMask.Enabled = Not rdobtnUseExtents.Checked
        btnDrawMask.Enabled = Not rdobtnUseExtents.Checked
        btnSelectMask.Enabled = Not rdobtnUseExtents.Checked
        lblMaskSelected.Enabled = Not rdobtnUseExtents.Checked
        btnSetExtents.Enabled = rdobtnUseExtents.Checked
        If cmbxSelDem.SelectedIndex > 0 Then
            validateCellThreshAndSet()
            validateConvThreshAndSet()
        End If
    End Sub

    Private Sub chkbxBurnStream_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxBurnStream.Click
        If tdbChoiceList.useBurnIn <> chkbxBurnStream.Checked Then
            preProcHasRan = False
        End If
    End Sub

    Private Sub chkbxMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxMask.Click
        If tdbChoiceList.useMaskFileOrExtents <> chkbxMask.Checked Then
            preProcHasRan = False
        End If
    End Sub

    Private Sub rdobtnUseExtents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnUseExtents.Click
        If tdbChoiceList.useExtentMask <> rdobtnUseExtents.Checked Then
            preProcHasRan = False
        End If
    End Sub

    Private Sub rdobtnUseFileMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnUseFileMask.Click
        If tdbChoiceList.useExtentMask <> rdobtnUseExtents.Checked Then
            preProcHasRan = False
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnAdvanced_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will display the advancedOptions form to let users select custom
    '   output display and turn off special stream and watershed calculation
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    ' 08/09/2006    ARA             Save Config on leaving.
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnAdvanced_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdvanced.Click
        frmSettings.BringToFront()
        frmSettings.ShowDialog()
        SaveToChoices()
        tdbChoiceList.SaveConfig()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseDem_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for grid data
    '   and allow the user to select a base dem grid file. Once
    '   selected, the file path will be loaded as a layer in
    '   MapWindow, then refill the comboboxes to add it to the right
    '   lists.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnBrowseDem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDem.Click

    '    Dim strPath As String
    '    Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog
    '    fdiagOpen.Filter = App.DataManager.RasterReadFilter
    '    fdiagOpen.FilterIndex = 1
    '    If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        preProcHasRan = False
    '        threshDelinHasRan = False
    '        strPath = fdiagOpen.FileName

    '        If LayerAlreadyInLegend(strPath) Then
    '            MsgBox("That grid layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '            Exit Sub
    '        End If

    '        Dim fstream As New System.IO.FileStream(strPath, IO.FileMode.Open)
    '        If fstream.Length > g_MaxFileSize Then
    '            MsgBox("The grid selected, size " & fstream.Length.ToString() & " is above the size limit supported by Automatic Watershed Delineation, which is " & g_MaxFileSize.ToString() & ". Please clip the grid to smaller portions or resample to a lower resolution.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '            Exit Sub
    '        End If
    '        fstream.Close()

    '        tdbFileList.CheckGridIsTif(strPath)

    '        If CheckGridProjection(strPath) Then
    '            AddMap(strPath, LayerType.dem)
    '            fillCombos()
    '            If (cmbxSelDem.Items.IndexOf(getNameByPath(strPath)) = -1) Then
    '                cmbxSelDem.Items.Add(getNameByPath(strPath))
    '            End If
    '            cmbxSelDem.SelectedIndex = cmbxSelDem.Items.IndexOf(getNameByPath(strPath))

    '            ' Save full path of base DEM:
    '            frmLoadOutput.selectedDem = strPath
    '            frmLoadDelinOutput.selectedDem = strPath
    '        Else
    '            MsgBox("The Grid selected was unprojected. Please reproject the grid using GIS Tools before adding.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If
    '    End If
    'End Sub

    '' Added by Paul Meems, 22-Aug-2011 
    'Private Function CheckGridProjection(ByVal strPath As String) As Boolean
    '    Dim g As New MapWinGIS.Grid
    '    Dim projCheck As Double
    '    g.Open(strPath, , True)
    '    g.CellToProj(10, 10, projCheck, projCheck)
    '    g.Close()


    '    projCheck = System.Math.Abs(System.Math.Floor(projCheck))

    '    Return (projCheck.ToString.Length > 3)
    'End Function


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseOutlets_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for shapefile data
    '   and allow the user to select an outlets point shape file. Once
    '   selected, the file path will be loaded as a layer in
    '   MapWindow, then refill the comboboxes to add it to add it
    '   to the list if it was a point shape type
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnBrowseOutlets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOutlets.Click
    '    Dim strPath As String
    '    Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog

    '    fdiagOpen.Filter = App.DataManager.

    '    fdiagOpen.FilterIndex = 1

    '    If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        strPath = fdiagOpen.FileName
    '        chkbxUseOutlet.Checked = True
    '        If Not LayerAlreadyInLegend(strPath) Then
    '            Dim sf As New MapWinGIS.Shapefile
    '            sf.Open(strPath)
    '            If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINT Or _
    '                sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTM Or _
    '                sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTZ Or _
    '                sf.ShapefileType = MapWinGIS.ShpfileType.SHP_MULTIPOINT _
    '            Then
    '                ' Paul Meems, 10-Aug-11: Add projection to prevent projection warnings when adding:
    '                If sf.Projection = String.Empty Then
    '                    sf.Projection = App.Map.Project.ProjectProjection
    '                End If
    '                AddMap(strPath, LayerType.Outlets)
    '                lastOutlet = getNameByPath(strPath)
    '                fillCombos()
    '                currDrawPath = strPath
    '            Else
    '                MsgBox("The shape file selected was not a point shape file and thus was not added.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '            End If
    '            sf.Close()
    '        Else
    '            MsgBox("That point layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseStream_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for shape file data
    '   and allow the user to select a stream flow polyline shape file.
    '   Once selected, the file will be loaded as a layer in
    '   MapWindow, then refill the comboboxes to add it to the right
    '   list.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnBrowseStream_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseStream.Click
    '    Dim strPath As String
    '    Dim lyr As MapWindow.Interfaces.Layer
    '    Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog

    '    fdiagOpen.Filter = App.DataManager.VectorReadFilter
    '    fdiagOpen.FilterIndex = 1

    '    If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        chkbxBurnStream.Checked = True
    '        strPath = fdiagOpen.FileName
    '        If Not LayerAlreadyInLegend(strPath) Then
    '            Dim sf As New MapWinGIS.Shapefile
    '            sf.Open(strPath)
    '            If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINE Or _
    '                sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or _
    '                sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEZ _
    '            Then
    '                ' Paul Meems, 11-Aug-11 Add projection to prevent projection warnings when adding:
    '                If Not App.Map.Project.ProjectProjection = String.Empty Then
    '                    sf.Projection = App.Map.Project.ProjectProjection
    '                End If

    '                lyr = App.Map.Layers.Add(sf, "Stream Burn-in (" + System.IO.Path.GetFileName(strPath) + ")")
    '                lyr.Color = System.Drawing.Color.Blue
    '                fillCombos()
    '                lastStream = getNameByPath(strPath)
    '                cmbxStream.SelectedIndex = cmbxStream.Items.IndexOf(lastStream)
    '            Else
    '                sf.Close()
    '                MsgBox("The shape file selected was not a polyline shape file and thus was not added.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '            End If
    '        Else
    '            MsgBox("That polyline shape layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseMask_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for grid and shape file data
    '   and allow the user to select a mask grid or polygon shapefile
    '   Once selected, the file will be loaded as a layer in
    '   MapWindow, then refill the comboboxes to add it to the right
    '   list.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnBrowseMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMask.Click
    '    Dim strPath As String
    '    Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog

    '    fdiagOpen.Filter = "All Supported Grid and Shapefile Formats|sta.adf;*.bgd;*.asc;*.tif;????cel0.ddf;*.arc;*.aux;*.pix;*.dhm;*.dt0;*.dt1;*.ecw;*.bil;*.sid;*.shp|" + App.DataManager.RasterReadFilter + "|" + App.DataManager.VectorReadFilter
    '    fdiagOpen.FilterIndex = 1

    '    If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '        strPath = fdiagOpen.FileName
    '        chkbxMask.Checked = True
    '        rdobtnUseFileMask.Checked = True
    '        If Not LayerAlreadyInLegend(strPath) Then
    '            If System.IO.Path.GetExtension(strPath) = ".shp" Then
    '                Dim sf As New MapWinGIS.Shapefile
    '                sf.Open(strPath)
    '                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or _
    '                                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or _
    '                                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ Then
    '                    AddMap(strPath, LayerType.mask)
    '                    fillCombos()
    '                    lastMask = getNameByPath(strPath)
    '                    cmbxMask.SelectedIndex = cmbxMask.Items.IndexOf(lastMask)
    '                    preProcHasRan = False
    '                    threshDelinHasRan = False
    '                    
    '                Else
    '                    MsgBox("The shapefile selected was not a polygon shapefile. Only polygon shapefiles can be used for a mask.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '                End If
    '                sf.Close()
    '            Else
    '                Dim fstream As New System.IO.FileStream(strPath, IO.FileMode.Open)
    '                If fstream.Length > g_MaxFileSize Then
    '                    MsgBox("The grid selected was above the size limit supported by Automatic Watershed Delineation. Please clip the grid to smaller portions or resample to a lower resolution.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '                    Exit Sub
    '                End If
    '                fstream.Close()

    '                If CheckGridProjection(strPath) Then
    '                    AddMap(strPath, LayerType.mask)
    '                    fillCombos()
    '                    lastMask = getNameByPath(strPath)
    '                    cmbxMask.SelectedIndex = cmbxMask.Items.IndexOf(lastMask)
    '                Else
    '                    MsgBox("The Grid selected was unprojected. Please reproject the grid using GIS Tools before adding.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '                End If
    '            End If
    '        Else
    '            MsgBox("That layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If

    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnDrawMask_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid draw layer, prompt to create one if one
    '  doesn't exist, then open the frmDrawSelectShape_v3 form and set the interface
    '  to drawing mask mode so that the interface can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnDrawMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrawMask.Click
    '    Dim fileSave As New System.Windows.Forms.SaveFileDialog
    '    Dim strShape As String

    '    If cmbxMask.SelectedIndex > 0 Then
    '        strShape = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
    '    Else
    '        strShape = ""
    '    End If
    '    myWrapper.maskShapesIdx.Clear()
    '    If strShape = "" Or System.IO.Path.GetExtension(strShape) <> ".shp" Then
    '        If MsgBox("There is not a shapefile Mask selected which can be drawn on, would you like to create a new shapefile mask?", MsgBoxStyle.YesNo, "Create new Mask?") = MsgBoxResult.Yes Then
    '            If cmbxSelDem.SelectedIndex > 0 Then
    '                fileSave.Filter = tdbFileList.GetShapefileFilter()
    '                fileSave.FilterIndex = 1
    '                If fileSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '                    chkbxMask.Checked = True

    '                    strShape = fileSave.FileName
    '                    MapWinGeoProc.DataManagement.DeleteShapefile(strShape)
    '                    Dim sf As New MapWinGIS.Shapefile
    '                    sf.CreateNew(strShape, MapWinGIS.ShpfileType.SHP_POLYGON)
    '                    sf.SaveAs(strShape)
    '                    ' Paul Meems, 22-Aug-11 Add projection to prevent projection warnings when adding:
    '                    If Not App.Map.Project.ProjectProjection = String.Empty Then
    '                        sf.Projection = App.Map.Project.ProjectProjection
    '                    End If

    '                    Dim idField As New MapWinGIS.Field
    '                    Dim idFieldNum As Integer

    '                    sf.StartEditingTable()
    '                    idField.Name = "MWShapeID"
    '                    idField.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                    idFieldNum = sf.NumFields
    '                    sf.EditInsertField(idField, idFieldNum)
    '                    sf.StopEditingTable()

    '                    ' Paul Meems, 22-Aug-11 Added extra check if projection was not already set:
    '                    If sf.Projection = String.Empty Then
    '                        Dim g As New MapWinGIS.Grid
    '                        g.Open(getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex)))
    '                        sf.Projection = g.Header.Projection
    '                        g.Close()

    '                    End If

    '                    sf.Close()
    '                    AddMap(strShape, LayerType.mask)
    '                    lastMask = getNameByPath(strShape)
    '                    fillCombos()
    '                    currDrawPath = strShape
    '                    App.Map.Layers.CurrentLayer = getIndexByPath(currDrawPath)
    '                    g_DrawingMask = True
    '                    App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
    '                    frmDrawSelect = New frmDrawSelectShape_v3
    '                    frmDrawSelect.Initialize(Me, "Left-click to draw a vertex. Right click to finish drawing.", False)
    '                    preProcHasRan = False
    '                    threshDelinHasRan = False
    '                    
    '                End If
    '            Else
    '                MsgBox("There is no Base DEM selected. Without the Base DEM, you cannot draw a mask on the new shapefile layer. Please select a Base DEM first.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '            End If
    '        End If
    '    Else
    '        chkbxMask.Checked = True
    '        currDrawPath = strShape
    '        App.Map.Layers.CurrentLayer = getIndexByPath(currDrawPath)
    '        g_DrawingMask = True
    '        App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
    '        frmDrawSelect = New frmDrawSelectShape_v3
    '        frmDrawSelect.Initialize(Me, "Left-click to draw a vertex. Right click to finish drawing.", False)
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSelectMask_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid select layer, then open the
    ' frmDrawSelectShape_v3 form and set the interface to selecting mask mode
    ' so that the interface and Done button can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnSelectMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectMask.Click
    '    Dim strShape As String
    '    Dim sf As New MapWinGIS.Shapefile
    '    preProcHasRan = False
    '    threshDelinHasRan = False
    '    
    '    If cmbxMask.SelectedIndex > 0 Then
    '        strShape = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
    '    Else
    '        strShape = ""
    '    End If

    '    If System.IO.Path.GetExtension(strShape) = ".shp" Then
    '        sf.Open(strShape)
    '        If sf.NumShapes > 0 Then
    '            chkbxMask.Checked = True
    '            currSelectPath = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
    '            App.Map.Layers.CurrentLayer = getIndexByPath(currSelectPath)
    '            App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
    '            g_SelectingMask = True
    '            frmDrawSelect = New frmDrawSelectShape_v3
    '            frmDrawSelect.Initialize(Me, "Hold Control and click on each to Select multiple shapes.", False)
    '        Else
    '            MsgBox("No shapes could be found in the shapefile selected. Please use Draw Mask to draw a Mask or select a new shapefile.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If
    '        sf.Close()
    '    Else
    '        MsgBox("There is no shapefile Mask selected to choose shapes from. Please select a Mask from the Mask drop down list or use the browse button beside the list to add one.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSetExtents_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for selected Base DEM and if found, allow users to zoom
    '  to select interface
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/15/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnSetExtents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetExtents.Click
    '    If cmbxSelDem.SelectedIndex > 0 Then
    '        App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn
    '        chkbxMask.Checked = True
    '        rdobtnUseExtents.Checked = True
    '        frmDrawSelect = New frmDrawSelectShape_v3
    '        frmDrawSelect.Initialize(Me, "Zoom to the extents you wish to use for a mask.", False)
    '        preProcHasRan = False
    '        threshDelinHasRan = False
    '        
    '    Else
    '        MsgBox("There is no Base DEM selected to choose the extents from. Please select one from the Base DEM drop down list or open a new one using the browse button next to it.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '    End If

    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnDrawOutlets_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid draw layer, prompt to create one if one
    '  doesn't exist, then open the frmDrawSelectShape_v3 form and set the interface
    '  to drawing mask mode so that the interface can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    ' 07/06/2006    ARA             Removes snap preview and makes sure draw
    '                                layer is visible
    ' 26/1/11       CWG             MWShapeID field in outlest file renames to ID
    '                                for TauDEM V5
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnDrawOutlets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrawOutlets.Click

    '    Dim g As New MapWinGIS.Grid
    '    Dim fileSave As New System.Windows.Forms.SaveFileDialog
    '    Dim strShape As String

    '    If cmbxOutlets.SelectedIndex > 0 Then
    '        chkbxUseOutlet.Checked = True
    '        strShape = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
    '        RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_clip_snap.shp")
    '        RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_snap.shp")
    '        currDrawPath = strShape
    '        App.Map.Layers.CurrentLayer = getIndexByPath(currDrawPath)
    '        App.Map.Layers.Item(App.Map.Layers.CurrentLayer).Visible = True
    '        g_DrawingOutletsOrInlets = True
    '        App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
    '        frmDrawSelect = New frmDrawSelectShape_v3
    '        frmDrawSelect.Initialize(Me, "Click to place outlets or inlets on or near a stream reach.", True)
    '    Else
    '        If MsgBox("There is no outlets/inlets shapefile selected which can be drawn on, would you like to create a new outlets/inlets shapefile?", MsgBoxStyle.YesNo, "Create new Outlets/Inlets File?") = MsgBoxResult.Yes Then
    '            chkbxUseOutlet.Checked = True
    '            If cmbxSelDem.SelectedIndex > 0 Then
    '                fileSave.Filter = tdbFileList.GetShapefileFilter()
    '                fileSave.FilterIndex = 1
    '                If fileSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '                    strShape = fileSave.FileName
    '                    MapWinGeoProc.DataManagement.DeleteShapefile(strShape)
    '                    Dim sf As New MapWinGIS.Shapefile
    '                    sf.CreateNew(strShape, MapWinGIS.ShpfileType.SHP_POINT)
    '                    ' Paul Meems, 10-Aug-11 Add projection to prevent projection warnings when adding:
    '                    If Not App.Map.Project.ProjectProjection = String.Empty Then
    '                        sf.Projection = App.Map.Project.ProjectProjection
    '                    End If

    '                    sf.SaveAs(strShape)
    '                    Dim idField As New MapWinGIS.Field
    '                    Dim idFieldNum As Integer

    '                    sf.StartEditingTable()
    '                    ' CWG 26/1/11 field that was called MWShapeID has to be called ID for Taudem V5
    '                    idField.Name = "ID"
    '                    idField.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                    idFieldNum = sf.NumFields
    '                    sf.EditInsertField(idField, idFieldNum)
    '                    sf.StopEditingTable()

    '                    ' Paul Meems, 10-Aug-11 Added extra check if projection was not already set:
    '                    If sf.Projection = String.Empty Then
    '                        g.Open(getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex)))
    '                        sf.Projection = g.Header.Projection
    '                        g.Close()

    '                    End If

    '                    sf.Close()
    '                    RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_clip_snap.shp")
    '                    RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_snap.shp")
    '                    AddMap(strShape, LayerType.Outlets)
    '                    lastOutlet = getNameByPath(strShape)
    '                    fillCombos()
    '                    currDrawPath = strShape
    '                    App.Map.Layers.CurrentLayer = getIndexByPath(currDrawPath)
    '                    g_DrawingOutletsOrInlets = True
    '                    App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
    '                    frmDrawSelect = New frmDrawSelectShape_v3
    '                    frmDrawSelect.Initialize(Me, "Click to place outlets or inlets on or near a stream reach.", True)
    '                End If
    '            Else
    '                MsgBox("There is no Base DEM selected. Without the Base DEM, you cannot draw outlets or inlets on the new shapefile layer. Please select a Base DEM first.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '            End If
    '        End If
    '    End If
    '    myWrapper.outletShapesIdx.Clear()
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSelectOutlets_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid select layer, then open the
    ' frmDrawSelectShape_v3 form and set the interface to selecting mask mode
    ' so that the interface and Done button can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    ' 07/06/2006    ARA             Removes snap preview and makes sure draw
    '                                layer is visible
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnSelectOutlets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectOutlets.Click
    '    Dim sf As New MapWinGIS.Shapefile

    '    If cmbxOutlets.SelectedIndex > 0 Then
    '        chkbxUseOutlet.Checked = True
    '        currSelectPath = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
    '        App.Map.Layers.CurrentLayer = getIndexByPath(currSelectPath)
    '        RemoveLayer(IO.Path.GetDirectoryName(currSelectPath) + "\" + IO.Path.GetFileNameWithoutExtension(currSelectPath) + "_clip_snap.shp")
    '        RemoveLayer(IO.Path.GetDirectoryName(currSelectPath) + "\" + IO.Path.GetFileNameWithoutExtension(currSelectPath) + "_snap.shp")
    '        App.Map.Layers.Item(App.Map.Layers.CurrentLayer).Visible = True
    '        sf.Open(currSelectPath)
    '        If sf.NumShapes > 0 Then
    '            App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
    '            currSelectPath = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
    '            g_SelectingOutlets = True
    '            frmDrawSelect = New frmDrawSelectShape_v3
    '            frmDrawSelect.Initialize(Me, "Hold Control and Click to Select Outlets/Inlets near or on reaches.", False)
    '        Else
    '            MsgBox("No shapes could be found in the shapefile selected. Please use Draw Outlets/Inlets to draw new outlets/inlets or select a new shapefile.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If
    '        sf.Close()
    '    Else
    '        MsgBox("There is no shapefile of outlets and inlets selected to choose outlets/inlets from. Please select an Outlets/Inlets shapefile from the Outlets/Inlets drop down list or use the browse button beside the list to add one.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSnapTo_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will clip and snap the outlets if the stream network has been
    '  delineated, in order to give the users a preview of where their outlets will
    '  actually be delineated from.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 06/22/2006    ARA             Created
    ' 07/06/2006    ARA             Updated to run thresh delin if hasn't ran
    '                                rather than giving error
    ' 08/08/2006    ARA             Modified to exit on fail of runThreshDelin
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Private Sub btnSnapTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSnapTo.Click
    '    If cmbxOutlets.SelectedIndex > 0 Then
    '        If myWrapper.outletShapesIdx.Count > 0 Then
    '            If Not threshDelinHasRan Then
    '                'MsgBox("Snapping outlets cannot be done until the stream network have been delineated. Please click the Run button in the Delineation by Threshold Method section before trying to snap points.")
    '                If Not runDelinByThresh() Then
    '                    Exit Sub
    '                End If
    '            End If
    '            runFormInit()
    '            tdbFileList.outletshpfile = getPathByName(cmbxOutlets.Items.Item(cmbxOutlets.SelectedIndex))
    '            'tmpOutPath = tdbFileList.outletshpfile
    '            App.Map.Layers.Item(getIndexByPath(tdbFileList.outletshpfile)).Visible = False
    '            RemoveLayer(IO.Path.GetDirectoryName(tdbFileList.outletshpfile) + "\" + IO.Path.GetFileNameWithoutExtension(tdbFileList.outletshpfile) + "_clip_snap.shp")
    '            RemoveLayer(IO.Path.GetDirectoryName(tdbFileList.outletshpfile) + "\" + IO.Path.GetFileNameWithoutExtension(tdbFileList.outletshpfile) + "_snap.shp")
    '            runClipSelectedOutlets()
    '            runAutoSnap()
    '            AddMap(tdbFileList.outletshpfile, LayerType.outletPreview)
    '            'lastOutlet = getNameByPath(tdbFileList.outletshpfile)
    '            fillCombos()

    '            'Select all shapes in new snap file
    '            'App.Map.View.SelectedShapes.ClearSelectedShapes()
    '            'Dim sf As New MapWinGIS.Shapefile
    '            'sf.Open(tdbFileList.outletshpfile)
    '            'For i As Integer = 0 To sf.NumShapes - 1
    '            'App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '            'Next
    '            'sf.Close()
    '            'g_SelectingOutlets = True
    '            'setOutletsSelected()
    '            'g_SelectingOutlets = False
    '            'tdbFileList.outletshpfile = tmpOutPath

    '            
    '            runFormCleanup()
    '        Else
    '            MsgBox("There are no outlets/inlets currently selected. Please click the Select Outlets/Inlets button to select outlets and inlets to snap.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '        End If
    '    Else
    '        MsgBox("There is no outlets/inlets layer selected. Please select an outlets/inlets shapefile from the Outlets/Inlets dropdown list or add a new one using the button beside it.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
    '    End If
    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunPreproc_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the preprocessing run button being clicked
    '   which activates the runPreprocessing sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunPreproc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunPreproc.Click
        runPreprocessing()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunThreshDelin_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the thresh delin run button being clicked
    '   which activates the runDelinByThresh sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunThreshDelin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunThreshDelin.Click
        runDelinByThresh()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunOutletFinish_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the outlet and finish run button being clicked
    '   which activates the runOutletsAndFinish sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunOutletFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunOutletFinish.Click
        runOutletsAndFinish()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunAll_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the run all button being clicked
    '   which activates the runAll sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunAll.Click
        If runAll() Then
            closingCleanup()
            Me.Close()
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnCancel_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the cancel button which cleans and
    ' closes the form
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        closingCleanup()
        Me.Close()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnLoadPre_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the Load Preprocessing button which
    '  is used to load paths of previously generated pre-processing outputs,
    '  including the pit fill, d8, d8 slope, dinf, and dinf slope files. If
    '  all the paths were specified, then the function sets the choicelist items
    '  used for these
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnLoadPre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadPre.Click
        If cmbxSelDem.SelectedIndex = 0 Then
            MsgBox("You need to select a DEM grid from the Base Elevation Grid drop-down list. If no layers are available to select, you can use the browse button beside the list to open an existing DEM.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        Else
            frmLoadOutput.ShowDialog()
            If frmLoadOutput.fillPath <> "" And frmLoadOutput.sd8Path <> "" And frmLoadOutput.d8Path <> "" Then
                If tdbChoiceList.useDinf And frmLoadOutput.dinfSlopePath <> "" And frmLoadOutput.dInfPath <> "" Then
                    lblPreOut.Visible = True
                    tdbChoiceList.FillGridPath = frmLoadOutput.fillPath
                    tdbChoiceList.D8SlopePath = frmLoadOutput.sd8Path
                    tdbChoiceList.D8Path = frmLoadOutput.d8Path
                    tdbChoiceList.DInfPath = frmLoadOutput.dInfPath
                    tdbChoiceList.DInfSlopePath = frmLoadOutput.dinfSlopePath
                Else
                    lblPreOut.Visible = True
                    tdbChoiceList.FillGridPath = frmLoadOutput.fillPath
                    tdbChoiceList.D8SlopePath = frmLoadOutput.sd8Path
                    tdbChoiceList.D8Path = frmLoadOutput.d8Path
                    tdbChoiceList.DInfPath = ""
                    tdbChoiceList.DInfSlopePath = ""
                End If
            Else
                lblPreOut.Visible = False
                tdbChoiceList.FillGridPath = ""
                tdbChoiceList.D8SlopePath = ""
                tdbChoiceList.D8Path = ""
                tdbChoiceList.DInfPath = ""
                tdbChoiceList.DInfSlopePath = ""
            End If

            ' Paul Meems, 23-Aug-2011, Added:
            tdbChoiceList.MaskedDem = frmLoadOutput.maskedDem
        End If
    End Sub

    Private Sub btnLoadDelin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDelin.Click
        If cmbxSelDem.SelectedIndex = 0 Then
            MsgBox("You need to select a DEM grid from the Base Elevation Grid drop-down list. If no layers are available to select, you can use the browse button beside the list to open an existing DEM.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        Else
            frmLoadDelinOutput.ShowDialog()
            If frmLoadDelinOutput.ad8Path <> "" And frmLoadDelinOutput.gordPath <> "" And frmLoadDelinOutput.plenPath <> "" And frmLoadDelinOutput.tlenPath <> "" And frmLoadDelinOutput.srcPath <> "" And frmLoadDelinOutput.ordPath <> "" And frmLoadDelinOutput.coordPath <> "" And frmLoadDelinOutput.treePath <> "" And frmLoadDelinOutput.netPath <> "" And frmLoadDelinOutput.wPath <> "" Then
                If tdbChoiceList.useDinf And frmLoadDelinOutput.scaPath <> "" Then
                    lblDelinOut.Visible = True
                    tdbChoiceList.Ad8Path = frmLoadDelinOutput.ad8Path
                    tdbChoiceList.ScaPath = frmLoadDelinOutput.scaPath
                    tdbChoiceList.GordPath = frmLoadDelinOutput.gordPath
                    tdbChoiceList.PlenPath = frmLoadDelinOutput.plenPath
                    tdbChoiceList.TlenPath = frmLoadDelinOutput.tlenPath
                    tdbChoiceList.SrcPath = frmLoadDelinOutput.srcPath
                    tdbChoiceList.OrdPath = frmLoadDelinOutput.ordPath
                    tdbChoiceList.CoordPath = frmLoadDelinOutput.coordPath
                    tdbChoiceList.TreePath = frmLoadDelinOutput.treePath
                    tdbChoiceList.NetPath = frmLoadDelinOutput.netPath
                    tdbChoiceList.WPath = frmLoadDelinOutput.wPath
                Else
                    lblDelinOut.Visible = True
                    tdbChoiceList.Ad8Path = frmLoadDelinOutput.ad8Path
                    tdbChoiceList.ScaPath = ""
                    tdbChoiceList.GordPath = frmLoadDelinOutput.gordPath
                    tdbChoiceList.PlenPath = frmLoadDelinOutput.plenPath
                    tdbChoiceList.TlenPath = frmLoadDelinOutput.tlenPath
                    tdbChoiceList.SrcPath = frmLoadDelinOutput.srcPath
                    tdbChoiceList.OrdPath = frmLoadDelinOutput.ordPath
                    tdbChoiceList.CoordPath = frmLoadDelinOutput.coordPath
                    tdbChoiceList.TreePath = frmLoadDelinOutput.treePath
                    tdbChoiceList.NetPath = frmLoadDelinOutput.netPath
                    tdbChoiceList.WPath = frmLoadDelinOutput.wPath
                End If
            Else
                lblDelinOut.Visible = False
                tdbChoiceList.Ad8Path = ""
                tdbChoiceList.ScaPath = ""
                tdbChoiceList.GordPath = ""
                tdbChoiceList.PlenPath = ""
                tdbChoiceList.TlenPath = ""
                tdbChoiceList.SrcPath = ""
                tdbChoiceList.OrdPath = ""
                tdbChoiceList.CoordPath = ""
                tdbChoiceList.TreePath = ""
                tdbChoiceList.NetPath = ""
                tdbChoiceList.WPath = ""
            End If
        End If
    End Sub
#End Region


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runPreprocessing
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run the preprocessing, mask, and pit fill steps
    '
    ' INPUTS: None
    '
    ' OUTPUTS: Boolean true if completed
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runPreprocessing() As Boolean
        runFormInit()
        lblPreproc.BringToFront()
        lblPreproc.Visible = True
        Me.Refresh()
        preProcHasRan = False
        Try
            If Not validatePreprocessing() Then
                runFormCleanup()
                Return False
            End If
            App.ProgressHandler.Progress("Status", 0, "Preparing Grid")
            g_BaseDEM = getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex))
            tdbFileList.FormFileNames(g_BaseDEM, tdbChoiceList.OutputPath, True)
            ' CWG 30/1/11 Changed for TauDEM V5
            ' g_Taudem.SetBASEDEM(tdbFileList.dem)

            tdbChoiceList.numProcesses = ProcessNum()

            If tdbChoiceList.FillGridPath <> "" And tdbChoiceList.D8SlopePath <> "" And tdbChoiceList.D8Path <> "" Then
                tdbFileList.fel = tdbChoiceList.FillGridPath
                tdbFileList.sd8 = tdbChoiceList.D8SlopePath
                tdbFileList.p = tdbChoiceList.D8Path
            Else
                If Not runMask() Then runFormCleanup() : Return False
                If Not runPitFill() Then runFormCleanup() : Return False
                If Not runD8() Then runFormCleanup() : Return False

                'hack:
                If tdbChoiceList.useDinf Then
                    '    If Not runDinf() Then runFormCleanup() : Return False
                End If
            End If
        Catch e As Exception
            runFormCleanup()
            preProcHasRan = False
            Return False
            MsgBox(e.Message, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End Try

        runFormCleanup()
        preProcHasRan = True
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validatePreprocessing
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to check that the necessary files are selected if using the option
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed, false if not validated
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validatePreprocessing() As Boolean
        If cmbxSelDem.SelectedIndex = 0 Then
            MsgBox("You need to select a DEM grid from the Base Elevation Grid drop-down list. If no layers are available to select, you can use the browse button beside the list to open an existing DEM.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            Return False
        End If

        If chkbxBurnStream.Checked Then
            If cmbxStream.SelectedIndex = 0 Then
                MsgBox("You need to select a stream polyline to burn-in from the Burn-in drop-down list. If no layers are available, then you can use the browse button beside the list to open an existing polyline shapefile.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                Return False
            End If
        End If

        If chkbxMask.Checked Then
            If Not rdobtnUseExtents.Checked Then
                If cmbxMask.SelectedIndex = 0 Then
                    MsgBox("You need to select a layer from the Mask drop-down list. If no layers are available, then you can use the browse button beside the list to open an existing polygon shapefile or focus mask grid.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    Return False
                Else
                    Dim strpath As String
                    strpath = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
                    If IO.Path.GetExtension(strpath) = ".shp" Then
                        If ProgressHandler.maskShapesIdx.Count = 0 Then
                            MsgBox("You must select at least one polygon from the mask using Select Mask.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                            Return False
                        End If
                    End If
                End If
            End If
        End If
        validatePreprocessing = True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runDelinByThresh
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run the steps all the way to the stream shape.
    '  Stops there because steps may be rerun based on outlets
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runDelinByThresh() As Boolean
        runFormInit()
        threshDelinHasRan = False

        Try
            If Not validatePreprocessing() Or Not validateDelinByThresh() Then
                runFormCleanup()
                Return False
            End If

            'If the preProcessing hasn't been done, run it first and only go on if it succeeded
            If Not preProcHasRan Then
                If Not runPreprocessing() Then
                    runFormCleanup()
                    Return False
                End If
            End If
            runFormInit()
            lblDelin.BringToFront()
            lblDelin.Visible = True
            Me.Refresh()
            'Setting outlets to false because the output of runDelinByThresh should always
            ' be the stream delineation without outlets. This lets the users see the stream
            ' delineated so they can place their points accurately. UseOutlets will be
            ' reset and the steps rerun in runOutletsAndFinish if the use outlets checkbox
            ' is checked.
            tdbChoiceList.useOutlets = False
            tdbChoiceList.numProcesses = ProcessNum()

            If tdbChoiceList.FillGridPath <> "" And tdbChoiceList.D8SlopePath <> "" And tdbChoiceList.D8Path <> "" Then
                'MapWinGeoProc.DataManagement.DeleteShapefile(tdbFileList.net)
                MapWinGeoProc.DataManagement.CopyShapefile(tdbChoiceList.NetPath, tdbFileList.net)
            Else
                If Not runAreaD8() Then runFormCleanup() : Return False
                If tdbChoiceList.useDinf Then
                    If Not runAreaDinf() Then runFormCleanup() : Return False
                End If
                If Not runDefineStreamGrids() Then runFormCleanup() : Return False

                If cmbxStream.SelectedIndex > 0 Then
                    'turn off the burn in layer to better view the result
                    Dim layer As IMapLayer = App.Map.Layers.Where(Function(p) p.LegendText = cmbxStream.Items(cmbxStream.SelectedIndex)).First()
                    layer.IsVisible = False
                End If
            End If
        Catch e As Exception
            runFormCleanup()
            threshDelinHasRan = False

            MsgBox(e.Message, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            Return False
        End Try

        runFormCleanup()
        threshDelinHasRan = True
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateDelinByThresh
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to check that the threshold is validated
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed, false if not validated
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateDelinByThresh() As Boolean
        Return validateCellThreshAndSet()
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runOutletsAndFinish
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to run the last steps needed, based on whether using outlets or
    '  not. If not using, it will only run the last steps. If outlets are used, it will
    '  autosnap the outlet points to the stream network generated before, then rerun
    '  all steps from the D8 accumulation in order to generate the outlet-defined network
    '  and watershed
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runOutletsAndFinish() As Boolean
        runFormInit()
        runOutletsAndFinish = False
        Try
            If Not validatePreprocessing() Or Not validateDelinByThresh() Or Not validateOutlets() Then
                runFormCleanup()
                Return False
            End If

            'If the Delin portion hasn't been done, run it first and only go on if it succeeded
            If Not threshDelinHasRan Then
                If Not runDelinByThresh() Then
                    runFormCleanup()
                    'If Not stopClose Then
                    '    MsgBox("An error occured while delineating. Please check that your data is in the same projection and overlaps correctly and that your threshold was set to a valid value.")
                    'End If
                    Return False
                End If
            End If

            runFormInit()
            lblOutlets.BringToFront()
            lblOutlets.Visible = True
            tdbChoiceList.numProcesses = ProcessNum()
            Me.Refresh()
            'If using outlets, have to rerun many of the steps as outlets change the output of them
            If tdbChoiceList.useOutlets Then
                'tdbFileList.outletshpfile = getPathByName(cmbxOutlets.Items.Item(cmbxOutlets.SelectedIndex))

                'If Not runClipSelectedOutlets() Then runFormCleanup() : Return False
                'If Not runAutoSnap() Then runFormCleanup() : Return False
                'If Not runAreaD8() Then runFormCleanup() : Return False
                'If tdbChoiceList.useDinf Then
                '    If Not runAreaDinf() Then runFormCleanup() : Return False
                'End If
                'If Not runDefineStreamGrids() Then runFormCleanup() : Return False
            Else
                If tdbChoiceList.FillGridPath <> "" And tdbChoiceList.D8SlopePath <> "" And tdbChoiceList.D8Path <> "" Then
                    tdbFileList.ad8 = tdbChoiceList.Ad8Path
                    tdbFileList.sca = tdbChoiceList.ScaPath
                    tdbFileList.gord = tdbChoiceList.GordPath
                    tdbFileList.plen = tdbChoiceList.PlenPath
                    tdbFileList.tlen = tdbChoiceList.TlenPath
                    tdbFileList.src = tdbChoiceList.SrcPath
                    tdbFileList.ord = tdbChoiceList.OrdPath
                    tdbFileList.coord = tdbChoiceList.CoordPath
                    tdbFileList.tree = tdbChoiceList.TreePath
                    tdbFileList.w = tdbChoiceList.WPath
                End If
            End If

            If Not runWshedToShape() Then runFormCleanup() : Return False
            If Not runApplyStreamAttributes() Then runFormCleanup() : Return False
            'hack
            ' If Not runApplyWatershedAttributes() Then runFormCleanup() : Return False
            'If Not runBuildJoinedBasins() Then runFormCleanup() : Return False
            'If Not runApplyJoinBasinAttributes() Then runFormCleanup() : Return False
        Catch e As Exception
            runFormCleanup()

            MsgBox(e.Message, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End Try
        runFormCleanup()
        runOutletsAndFinish = True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateOutlets
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to check that the outlets parameters are set correctly
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed, false if not validated
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateOutlets() As Boolean
        If chkbxUseOutlet.Checked Then
            If cmbxOutlets.SelectedIndex > 0 Then
                If ProgressHandler.outletShapesIdx.Count = 0 Then
                    MsgBox("There are no outlets/inlets currently selected. Please click the Select Outlets/Inlets button to select outlets and inlets.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    Return False
                End If
            Else
                MsgBox("There is no outlets/Inlets shapefile selected. Please select an outlets/inlets shapefile from the Outlets/Inlets drop down list or add a new one using the button beside it.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                Return False
            End If
            Dim filename As String = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
            CheckIDField(filename)
        End If

        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runAll
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub executed by the run all button, which merely calls the
    '  runOutletsAndFinish.
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: originally, there was going to be special handling for
    '  running all in regards to snapping, but it was made so it always snaps
    '  so this is semi redundanta
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 13/7/2011     CWG             Made public to support testing
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function runAll()
        runAll = runOutletsAndFinish()
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runMask
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run the masking algorithms to limit delineation to
    '  the areas under masks
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runMask() As Boolean
        'Dim maskedPath, strmask As String
        'Dim sf As New MapWinGIS.Shapefile
        'Dim g As New MapWinGIS.Grid
        'Dim gMasked As New MapWinGIS.Grid
        'Dim u As New MapWinGIS.Utils

        If doTicks Then
            tickb = Now().Ticks
        End If
        '    If tdbChoiceList.useMaskFileOrExtents Then
        '        If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
        '            maskedPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".tif") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_masked.tif"
        '        Else
        '            maskedPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_masked.tif"
        '        End If
        '        RemoveLayer(maskedPath)

        '        If rdobtnUseExtents.Checked Then 'mask by extents
        '            If MapWinGeoProc.Hydrology.Mask(tdbFileList.dem, App.Map.View.Extents, maskedPath, App.ProgressHandler) = 0 Then
        '                tdbFileList.dem = maskedPath
        '            End If
        '        Else
        '            If cmbxMask.SelectedIndex > 0 Then
        '                strmask = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
        '                If IO.Path.GetExtension(strmask) = ".shp" Then 'Mask by selected shapes
        '                    If myWrapper.maskShapesIdx.Count > 0 Then
        '                        If MapWinGeoProc.Hydrology.Mask(tdbFileList.dem, strmask, myWrapper.maskShapesIdx, maskedPath, App.ProgressHandler) = 0 Then
        '                            tdbFileList.dem = maskedPath
        '                        End If
        '                    End If
        '                Else 'Mask by grid
        '                    If MapWinGeoProc.Hydrology.Mask(tdbFileList.dem, strmask, maskedPath, App.ProgressHandler) = 0 Then
        '                        tdbFileList.dem = maskedPath
        '                    End If
        '                End If
        '            End If
        '        End If
        '    End If
        '    If doTicks Then
        '        ticka = Now().Ticks
        '        tickd = ticka - tickb
        '        os.WriteLine(tickd.ToString + " - Mask ")
        '    End If
        runMask = True
    End Function

    Private Function runBurn(ByVal strDEM As String) As String

        '    Dim strBurn, strBurnResult As String
        runBurn = strDEM
        If tdbChoiceList.useBurnIn Then
            '        strBurn = getPathByName(cmbxStream.Items.Item(cmbxStream.SelectedIndex))
            '        If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
            '            strBurnResult = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".tif") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_burn.tif"
            '        Else
            '            strBurnResult = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_burn.tif"
            '        End If
            '        If MapWinGeoProc.Hydrology.CanyonBurnin(strBurn, strDEM, strBurnResult, App.ProgressHandler) = 0 Then
            '            runBurn = strBurnResult
            '        Else
            '            MsgBox("An error occured while burning in the stream polyline.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            '        End If
        End If
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runPitFill
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function split off from Begin to make TauDem calls
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: boolean true if succeeds, false if not
    ''
    '' NOTES: None
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '' 07/17/2006    ARA             Changed to use the new fill method
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runPitFill() As Boolean
        Dim usefdrfile As Long = 0
        Dim i As Integer
        Dim inRam As Boolean = True
        Dim strToFill As String
        Dim burnFirst As Boolean

        If tdbChoiceList.usefdrfile Then usefdrfile = 1
        If tdbChoiceList.DiskBased = True Then inRam = False

        RemoveLayer(tdbFileList.fel, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        burnFirst = True
        strToFill = tdbFileList.dem
        If burnFirst Then
            strToFill = runBurn(tdbFileList.dem)
            If strToFill = "" Then
                App.ProgressHandler.Progress("Status", 0, "")
                Cursor = Windows.Forms.Cursors.Default
                Return False
            End If
        End If

        App.ProgressHandler.Progress("Status", 0, "Pit Fill")
        Try
            MapWinGeoProc.Hydrology.Fill(strToFill, tdbFileList.fel, App.ProgressHandler)
        Catch ex As Exception
            MsgBox("An error occured while filling the grid: " + ex.Message, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            App.ProgressHandler.Progress("Status", 0, "")
            Cursor = Windows.Forms.Cursors.Default
            Return False
        End Try

        If Not burnFirst Then
            tdbFileList.fel = runBurn(tdbFileList.fel)
            If tdbFileList.fel = "" Then
                App.ProgressHandler.Progress("Status", 0, "")
                Cursor = Windows.Forms.Cursors.Default
                Return False
            End If
        End If

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Pit Fill ")
        End If

        If tdbChoiceList.AddPitfillLayer And i = 0 Then
            AddMap(tdbFileList.fel, LayerType.fel)
        End If
        Return True
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runD8
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function split off from Begin to make TauDem calls
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: boolean true if succeeds, false if not
    ''
    '' NOTES: None
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '' 07/18/2006    ARA             Changed to call geoproc.hydrology instead of taudem
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runD8() As Boolean
        RemoveLayer(tdbFileList.sd8, False)
        RemoveLayer(tdbFileList.p, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        Dim result = MapWinGeoProc.Hydrology.D8(tdbFileList.fel, tdbFileList.p, tdbFileList.sd8, tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput, ProgressHandler.App.ProgressHandler)


        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - D8 ")
        End If
        If tdbChoiceList.AddD8Layer And result Then
            AddMap(tdbFileList.sd8, LayerType.sd8)
            AddMap(tdbFileList.p, LayerType.p)
        End If

        Return True
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runAreaD8
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function split off from Begin to make TauDem calls
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: boolean true if succeeds, false if not
    ''
    '' NOTES: None
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '' 07/19/2006    ARA             Changed to call geoproc.hydrology instead of taudem
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAreaD8() As Boolean
        Dim i As Integer

        RemoveLayer(tdbFileList.ad8, False)

        If doTicks Then
            tickb = Now().Ticks
        End If
        i = MapWinGeoProc.Hydrology.AreaD8(tdbFileList.p, tdbFileList.outletshpfile, tdbFileList.ad8, tdbChoiceList.useOutlets, tdbChoiceList.EdgeContCheck, tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput, App.ProgressHandler)
        If i <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Area D8 ")
        End If
        If tdbChoiceList.AddD8AreaLayer Then
            AddMap(tdbFileList.ad8, LayerType.ad8)
        End If
        Return True
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runDinf
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function to run Dinf
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: boolean true if succeeds, false if not
    ''
    '' NOTES: None
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 08/23/2006    ARA             Created
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runDinf() As Boolean
        Dim i As Integer

        RemoveLayer(tdbFileList.slp, False)
        RemoveLayer(tdbFileList.ang, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        i = MapWinGeoProc.Hydrology.DInf(tdbFileList.fel, tdbFileList.ang, tdbFileList.slp, tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput, App.ProgressHandler)
        If i <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Dinf ")
        End If
        If tdbChoiceList.AddD8Layer And i = 0 Then
            AddMap(tdbFileList.slp, LayerType.slp)
            AddMap(tdbFileList.ang, LayerType.ang)
        End If

        Return True
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runAreaDinf
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function to run AreaDinf
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: boolean true if succeeds, false if not
    ''
    '' NOTES: None
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 08/23/2006    ARA             Created
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAreaDinf() As Boolean
        RemoveLayer(tdbFileList.sca, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        Dim i As Integer
        ' i = MapWinGeoProc.Hydrology.AreaDInf(tdbFileList.ang, tdbFileList.outletshpfile, tdbFileList.sca, tdbChoiceList.useOutlets, tdbChoiceList.EdgeContCheck, tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput, myWrapper)
        If i <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Area Dinf ")
        End If
        If tdbChoiceList.AddD8AreaLayer Then
            AddMap(tdbFileList.sca, LayerType.sca)
        End If
        Return True
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runClipSelectedOutlets
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: Will clip the selected outlets into a new shape file if the
    ''  number selected isn't already equal to the total number in the file (which
    ''  is the default state). This prevents running this unnecessarily.
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: Returns true on completion.
    ''
    '' NOTES: Will create a clip file named as the point path with _clip.shp at end
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 06/18/2006    ARA             Created
    '' 08/08/2006    ARA             Updated to copy attributes when 'clipping'
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runClipSelectedOutlets() As Boolean
        Dim strClipShapePath As String

        If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
            strClipShapePath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".tif") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_clip.shp"
        Else
            strClipShapePath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_clip.shp"
        End If

        RemoveLayer(strClipShapePath)

        'If MapWinGeoProc.Utils.ExtractSelectedPoints(tdbFileList.outletshpfile, strClipShapePath, myWrapper.outletShapesIdx, App.ProgressHandler) Then
        '    tdbFileList.outletshpfile = strClipShapePath
        'End If

        runClipSelectedOutlets = True
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runAutoSnap
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function which will cycle a set of given points and
    ''  snap them to the nearest position on the given polyline file if they
    ''  meet the criteria of being under the tdbChoiceList.snapthresh distance
    ''  from the segment and if the new position isn't within 100 m or ft of
    ''  another snapped point (to prevent a nasty runtime error which is triggered
    ''  by taudem.
    ''
    '' INPUTS: strPolylinePath: String path to the polyline to snap to
    ''         strPointPath: String path to the points file to snap
    ''
    '' OUTPUTS: Returns true on completion.
    ''
    '' NOTES: Will create a snap file named as the point path with _snap.shp at end
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 05/30/2006    ARA             Created
    '' 08/07/2006    ARA             Moved code out to geoproc.utils.SnapPointsToLines
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAutoSnap() As Boolean
        Dim newPointPath As String
        If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
            newPointPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".tif") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_snap.shp"
        Else
            newPointPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_snap.shp"
        End If

        RemoveLayer(newPointPath)
        Dim g = Raster.Open(tdbFileList.p)
        Dim dx As Double = g.CellWidth
        g.Close()
        runAutoSnap = False
        'runAutoSnap = MapWinGeoProc.Utils.SnapPointsToLines(tdbFileList.outletshpfile, tdbFileList.net, tdbChoiceList.snapThresh, dx / 2, newPointPath, True, App.ProgressHandler)
        If runAutoSnap Then tdbFileList.outletshpfile = newPointPath
    End Function

    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' TITLE: runDefineStreamGrids
    '' AUTHOR: Allen Richard Anselmo
    '' DESCRIPTION: A function which calls the geoproc.hydrology function to
    ''  generate stream network associated grids
    ''
    '' INPUTS: None
    ''
    '' OUTPUTS: boolean true if succeeds, false if not
    ''
    '' NOTES: None
    ''
    '' Change Log:
    '' Date          Changed By      Notes
    '' 07/20/2006    ARA             Created
    '' 27/1/2011     CWG             Stream net and subbasin grids added as parameters
    ''                               to DelinStreamGrids
    ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runDefineStreamGrids() As Boolean
        Dim i As Integer

        RemoveLayer(tdbFileList.gord, False)
        RemoveLayer(tdbFileList.plen, False)
        RemoveLayer(tdbFileList.tlen, False)
        RemoveLayer(tdbFileList.src, False)
        RemoveLayer(tdbFileList.ord, False)
        RemoveLayer(tdbFileList.net, False)
        RemoveLayer(tdbFileList.w, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        i = MapWinGeoProc.Hydrology.DelinStreamGrids(tdbFileList.dem, tdbFileList.fel, tdbFileList.p, tdbFileList.sd8, tdbFileList.ad8, tdbFileList.ang, tdbFileList.outletshpfile, tdbFileList.gord, tdbFileList.plen, tdbFileList.tlen, tdbFileList.src, tdbFileList.ord, tdbFileList.tree, tdbFileList.coord, tdbFileList.net, tdbFileList.w, tdbChoiceList.Threshold, tdbChoiceList.useOutlets, tdbChoiceList.EdgeContCheck, tdbChoiceList.useDinf, tdbChoiceList.numProcesses, tdbChoiceList.ShowTaudemOutput, App.ProgressHandler)
        If i <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Source Definition ")
        End If
        If tdbChoiceList.AddGridNetLayer And i = 0 Then
            AddMap(tdbFileList.gord, LayerType.gord)
            AddMap(tdbFileList.plen, LayerType.plen)
            AddMap(tdbFileList.tlen, LayerType.tlen)
        End If
        If tdbChoiceList.AddRiverRasterLayer Then
            AddMap(tdbFileList.src, LayerType.src)
        End If
        If tdbChoiceList.AddOrderGridLayer Then
            AddMap(tdbFileList.ord, LayerType.ord)
        End If
        If tdbChoiceList.AddWShedGridLayer Then
            AddMap(tdbFileList.w, LayerType.w)
        End If
        If tdbChoiceList.AddStreamShapeLayer Then
            AddMap(tdbFileList.net, LayerType.ReachShapefile) '15 is a reach shapefile
        End If

        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runWshedToShape
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function split off from Begin to make TauDem calls
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if successful, false if failed
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runWshedToShape() As Boolean
        If doTicks Then
            tickb = Now().Ticks
        End If

        RemoveLayer(tdbFileList.wshed, False)

        'Dim d8path As Object = tdbFileList.p
        Dim outputShapefile = tdbFileList.wshed
        Dim inputGrid = tdbFileList.w

        Dim ms As New ManhattanShapes(inputGrid)
        Dim ValueToUse As Double = 32
        Dim sf As FeatureSet = ms.GridToShapeManhattan()
        sf.SaveAs(outputShapefile, True)

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Watershed to Shapefile ")
        End If
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: ApplyStreamAttributes
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A subroutine to calculate and apply the new stream net
    '   attributes
    '
    ' INPUTS: strStreamFile : A string to the filename for the stream net
    '                         shapefile
    '         strDemFile : A string to the filename of the dem grid
    '
    ' OUTPUTS: Boolean true on completion
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runApplyStreamAttributes() As Boolean

        RemoveLayer(tdbFileList.net, False)
        If doTicks Then
            tickb = Now().Ticks
        End If
        App.ProgressHandler.Progress("Status", 0, "Calculating Stream Parameters")
        If tdbChoiceList.CalcSpecialStreamFields Then
            runApplyStreamAttributes = MapWinGeoProc.Hydrology.ApplyStreamAttributes(tdbFileList.net, tdbFileList.dem, tdbFileList.wshed, cmbxElevUnits.SelectedIndex, App.ProgressHandler)
        Else
            runApplyStreamAttributes = True
        End If

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Stream Special Attributes ")
            tickb = Now().Ticks
        End If

        ' Add after Applying the attributes so their attr tables will be up to date
        '
        If tdbChoiceList.AddStreamShapeLayer Then
            AddMap(tdbFileList.net, LayerType.ReachShapefile)
        End If

    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: ApplyWatershedAttributes
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A subroutine to calculate and apply the new watershed
    '   attributes
    '
    ' INPUTS: strShedFile : A string to the filename for the final shed
    '                        shed shapefile
    '         strSlopeFile : A string to the filename for the slope grid
    '         strStreamFile : A string to the filename for the stream net
    '                         shapefile
    '
    ' OUTPUTS: boolean true on completion
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 02/20/2006    CM              Added precision and moved begineditings
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runApplyWatershedAttributes() As Boolean

        If doTicks Then
            tickb = Now().Ticks
        End If

        runApplyWatershedAttributes = MapWinGeoProc.Hydrology.ApplyWatershedLinkAttributes(tdbFileList.wshed, tdbFileList.net, App.ProgressHandler)

        If tdbChoiceList.CalcSpecialWshedFields Then
            runApplyWatershedAttributes = MapWinGeoProc.Hydrology.ApplyWatershedAreaAttributes(tdbFileList.wshed, App.ProgressHandler)
            runApplyWatershedAttributes = MapWinGeoProc.Hydrology.ApplyWatershedSlopeAttribute(tdbFileList.w, tdbFileList.wshed, tdbFileList.sd8, cmbxElevUnits.SelectedIndex, App.ProgressHandler)
        Else
            runApplyWatershedAttributes = True
        End If

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Watershed Special Attributes ")

            ' Paul Meems, 24-Aug-2011: Why close it?:
            ' os.Close()
        End If

        If tdbChoiceList.AddWShedShapeLayer Then
            AddMap(tdbFileList.wshed, LayerType.WatershedShapefile)
        End If
    End Function

    Private Function runBuildJoinedBasins() As Boolean
        If doTicks Then
            tickb = Now().Ticks
        End If

        RemoveLayer(tdbFileList.mergewshed)
        runBuildJoinedBasins = MapWinGeoProc.Hydrology.BuildJoinedBasins(tdbFileList.wshed, tdbFileList.outletshpfile, tdbFileList.mergewshed, App.ProgressHandler)

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Watershed to Joined Shed ")
        End If
    End Function

    Private Function runApplyJoinBasinAttributes() As Boolean
        runApplyJoinBasinAttributes = False

        If tdbChoiceList.calcSpecialMergeWshedFields Then
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyJoinBasinAreaAttributes(tdbFileList.mergewshed, cmbxElevUnits.SelectedIndex, App.ProgressHandler)
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyWatershedSlopeAttribute(tdbFileList.w, tdbFileList.mergewshed, tdbFileList.sd8, cmbxElevUnits.SelectedIndex, App.ProgressHandler)
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyWatershedElevationAttribute(tdbFileList.w, tdbFileList.mergewshed, tdbFileList.fel, App.ProgressHandler)
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyJoinBasinStreamAttributes(tdbFileList.net, tdbFileList.w, tdbFileList.mergewshed, App.ProgressHandler)
        Else
            runApplyJoinBasinAttributes = True
        End If

        If tdbChoiceList.AddMergedWShedShapeLayer Then
            AddMap(tdbFileList.mergewshed, LayerType.mergeWShed)
        End If
    End Function
    '#End Region

    Private Sub CheckIDField(ByVal shapefilename As String)
        'Dim shpfile As New MapWinGIS.Shapefile
        'If Not shpfile.Open(shapefilename) Then
        '    MsgBox("Cannot open shapefile " & shapefilename)
        '    Exit Sub
        'End If

        'Dim theField As Integer
        'Dim Field As MapWinGIS.Field
        'Dim FieldName As String
        'Dim theFieldIndex As Integer
        'Dim numFields As Integer = shpfile.NumFields
        'theFieldIndex = -1
        'For theField = 0 To numFields - 1
        '    Field = shpfile.Field(theField)
        '    FieldName = Field.Name
        '    If LCase(FieldName) = "id" Then
        '        theFieldIndex = theField
        '        Exit For
        '    End If
        'Next theField

        'Dim f As MapWinGIS.Field
        'Dim IDIndex As Integer
        'Dim i As Integer
        'If theFieldIndex < 0 Then
        '    If MsgBox("The outlets shapefile has no ID field.  Would you like to create one automatically?", MsgBoxStyle.YesNo, "Shapefile needs ID Field") = MsgBoxResult.Yes Then
        '        f = New MapWinGIS.Field
        '        f.Type = MapWinGIS.FieldType.INTEGER_FIELD
        '        f.Name = "ID"
        '        If Not shpfile.StartEditingTable() Then
        '            MsgBox("Cannot edit shapefile " & shapefilename)
        '            shpfile.Close()
        '            Exit Sub
        '        End If
        '        shpfile.EditInsertField(f, shpfile.NumFields)
        '        IDIndex = shpfile.NumFields - 1
        '        'put numbers in the new field
        '        For i = 0 To shpfile.NumShapes - 1
        '            shpfile.EditCellValue(IDIndex, i, i)
        '        Next i
        '        shpfile.StopEditingTable(True)
        '    End If
        'End If
        'shpfile.Close()
    End Sub

    Public ReadOnly Property ProcessNum() As Integer
        Get
            If (Me.numProcesses.Text Is Nothing) OrElse (Me.numProcesses.Text = "") Then
                ProcessNum = tdbChoiceList.numProcesses
            Else
                Try
                    ProcessNum = Int32.Parse(Me.numProcesses.Text)
                Catch
                    MsgBox("Cannot parse " & Me.numProcesses.Text & " as integer")
                    ProcessNum = tdbChoiceList.numProcesses
                End Try
            End If
        End Get
    End Property

    Private Sub frmAutomatic_v3_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' Paul Meems, 24-aug-2011
        ' Close the timer file if it is being used and display it:
        If doTicks AndAlso Not (os Is Nothing) Then
            os.Flush()
            os.Close()
            os.Dispose()

            Process.Start(timepath)
        End If
    End Sub

End Class

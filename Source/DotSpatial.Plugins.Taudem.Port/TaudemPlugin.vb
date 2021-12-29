'********************************************************************************************************
'Filename:      mwTaudemBASINSWrapper.vb
'Description:   Plugin wrapper for the modified taudem plugin to be used by BASINS
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
'you may not use this file except in compliance with the License. You may obtain a copy of the License at
'http://www.mozilla.org/MPL/
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and
'limitations under the License.
'
'The Original Code is MapWindow Open Source.
'
'This class implements the MapWindow.Interfaces.Iplugin interface and all of its elements.  It defines this
'assembly as a MapWindow plug-in and provides communication between MapWindow and the mwTaudem COM dll in
'order to form a wrapper around the regular TauDEM plugin to provide functionality requested for BASINS
'
'Contributor(s): (Open source contributors should list themselves and their modifications here).
'Last Update:   10/18/05, ARA
'08/23/05 ARA       Modified from normal taudem wrapper
'10/18/05 ARA       Wrapping up and added mozilla comments
'16/10/09 CWG       Modified MapMouseDown section for marking outlets/inlets/reservoirs
'30/1/11  CWG       Replaced TKTAUDEMLib.ItkCallback with MapWinGIS.ICallback
'                   Adapted for Taudem V5
'********************************************************************************************************

Option Explicit On

Imports DotSpatial.Controls.Header
Imports System.IO
Imports System.Reflection

Public Class TaudemPlugin
    Inherits Controls.Extension

    Private lstDrawPoints As New ArrayList
    Public maskShapesIdx As New ArrayList
    Public outletShapesIdx As New ArrayList

    'For drawing niftiness
    Private ReversibleDrawn As New ArrayList
    Private LastStartPtX As Integer = -1
    Private LastStartPtY As Integer = -1
    Private LastEndX As Integer = -1
    Private LastEndY As Integer = -1
    Private StartPtX As Integer = -1
    Private StartPtY As Integer = -1
    Private EraseLast As Boolean = False
    Private mycolor As New Drawing.Color

    'Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
    '    'dpa 4/22/2005 Save the base grid file name to the MW project
    '    g_BaseDEM = SettingsString
    '    g_Taudem.SetBASEDEM(g_BaseDEM)
    'End Sub

    'Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
    '    'dpa 4/22/2005 Save the base grid file name to the MW project
    '    SettingsString = g_BaseDEM
    'End Sub


#Region "Start and Stop Functions"
    Public Overrides Sub Activate()

        'g_StatusBarItem = App.Map.StatusBar.AddPanel(App.Map.StatusBar.NumPanels - 2)
        'g_StatusBarItem.MinWidth = 10
        'g_StatusBarItem.AutoSize = True

        tdbChoiceList = New tdbChoices_v3
        tdbFileList = New tdbFileTypes_v3
        tdbChoiceList.SetDefaultTDchoices()
        tdbChoiceList.ConfigFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "awd.cfg")
        tdbChoiceList.LoadConfig()

        App.HeaderControl.Add(New SimpleActionItem("Watershed Delineation", AddressOf AutoWatershedDelineation_Click))
        'Dim nil As Object
        'nil = Nothing
        'With App.Map.Menus
        '    'Taudem_BASINS main menu
        '    .AddMenu("btdmWatershedDelin", nil, "Watershed Delineation")
        '    .AddMenu("btdmAutomatic", "btdmWatershedDelin", nil, "Automatic")
        '    .AddMenu("btdmBreak0", "btdmWatershedDelin", nil, "-")

        '    'Taudem Advanced Functions
        '    .AddMenu("btdmTaudem", "btdmWatershedDelin", nil, "Advanced TauDEM Functions")
        '    .AddMenu("btdmSelectBaseDEM", "btdmTaudem", nil, "Select Base DEM Grid...")
        '    .AddMenu("btdmDoAllGrid", "btdmTaudem", nil, "Do All DEM Processing")
        '    .AddMenu("btdmGridSpecific", "btdmTaudem", nil, "DEM Processing Functions")
        '    .AddMenu("btdmBreak1", "btdmTaudem", nil, "-")
        '    .AddMenu("btdmOutlets", "btdmTaudem", nil, "Select Outlets Shapefile...")
        '    .AddMenu("btdmMoveOutlets", "btdmTaudem", nil, "Move Outlets to Streams")
        '    .AddMenu("btdmDoAllNetwork", "btdmTaudem", nil, "Do All Network and Watershed Steps")
        '    .AddMenu("btdmNetworkSpecific", "btdmTaudem", nil, "Network and Watershed Processing Functions")
        '    .AddMenu("btdmBreak2", "btdmTaudem", nil, "-")
        '    .AddMenu("btdmAncillary", "btdmTaudem", nil, "Ancillary Functions")
        '    .AddMenu("btdmBreak3", "btdmTaudem", nil, "-")
        '    .AddMenu("btdmHelp", "btdmTaudem", nil, "TauDEM Help")
        '    .AddMenu("btdmAbout", "btdmTaudem", nil, "About TauDEM")

        '    'Grid Menu Items
        '    .AddMenu("btdmFillPits", "btdmGridSpecific", nil, "Fill Pits")
        '    .AddMenu("btdmD8", "btdmGridSpecific", nil, "D8 Flow Directions")
        '    .AddMenu("btdmDinf", "btdmGridSpecific", nil, "Dinf Flow Directions")
        '    .AddMenu("btdmAreaD8", "btdmGridSpecific", nil, "D8 Contributing Area")
        '    .AddMenu("btdmAreaDinf", "btdmGridSpecific", nil, "Dinf Contributing Area")
        '    .AddMenu("btdmGridNet", "btdmGridSpecific", nil, "Grid Network Order and Flow Path Lengths")

        '    'Network Delineation Menu Items
        '    .AddMenu("btdmPK", "btdmNetworkSpecific", nil, "Peuker Douglas")
        '    .AddMenu("btdmD8FlowPathExtremeUp", "btdmNetworkSpecific", nil, "D8 Extreme Upslope Value")
        '    .AddMenu("btdmSlopeArea", "btdmNetworkSpecific", nil, "Slope Area Combination")
        '    .AddMenu("btdmLengthArea", "btdmNetworkSpecific", nil, "Length Area Stream Source")
        '    .AddMenu("btdmDropAnalysis", "btdmNetworkSpecific", nil, "Stream Drop Analysis")
        '    .AddMenu("btdmThreshold", "btdmNetworkSpecific", nil, "Stream Definition by Threshold")
        '    .AddMenu("btdmDropAnalysisStreamDef", "btdmNetworkSpecific", nil, "Stream Definition with Drop Analysis")
        '    .AddMenu("btdmPKStreamDef", "btdmNetworkSpecific", nil, "Peuker Douglas Stream Definition")
        '    .AddMenu("btdmSlopeAreaStreamDef", "btdmNetworkSpecific", nil, "Slope Area Stream Definition")
        '    .AddMenu("btdmStreamNet", "btdmNetworkSpecific", nil, "Stream Reach and Watershed")
        '    .AddMenu("btdmWatershedToPoly", "btdmNetworkSpecific", nil, "Watershed Grid to Shapefile")
        '    .AddMenu("btdmNetworkBreak1", "btdmNetworkSpecific", nil, "-")


        '    'Ancillary Functions
        '    .AddMenu("btdmAvalanche", "btdmAncillary", nil, "D-Infinity Avalanche Runout")
        '    .AddMenu("btdmCLimAccum", "btdmAncillary", nil, "D-Infinity Concentration Limited Accumulation")
        '    .AddMenu("btdmADinfDecay", "btdmAncillary", nil, "D-Infinity Decaying Accumulation")
        '    .AddMenu("btdmDinfDistDown", "btdmAncillary", nil, "D-Infinity Distance Down")
        '    .AddMenu("btdmDinfDistUp", "btdmAncillary", nil, "D-Infinity Distance Up")
        '    .AddMenu("btdmRevAccum", "btdmAncillary", nil, "D-Infinity Reverse Accumulation")
        '    .AddMenu("btdmTLAccum", "btdmAncillary", nil, "D-Infinity Transport Limited Accumulation")
        '    .AddMenu("btdmDependence", "btdmAncillary", nil, "D-Infinity Upslope Dependence")
        '    .AddMenu("btdmDistance", "btdmAncillary", nil, "D8 Distance to Streams")
        '    .AddMenu("btdmSlopeAveDown", "btdmAncillary", nil, "Slope Average Down")
        '    .AddMenu("btdmSlopeOverArea", "btdmAncillary", nil, "Slope/Area Ratio")
        '    .AddMenu("btdmAncilBreak1", "btdmAncillary", nil, "-")

        'End With

        MyBase.Activate()

    End Sub

    Public Overrides Sub Deactivate()

        If g_AutoForm IsNot Nothing AndAlso Not g_AutoForm.IsDisposed Then g_AutoForm.Close()

        'todo
        '            App.Map.StatusBar.RemovePanel(g_StatusBarItem)

        App.HeaderControl.RemoveAll()

        MyBase.Deactivate()
    End Sub


#End Region

#Region "Used Functions"
    Private Sub DisplayMessage(ByVal message As String)
        App.ProgressHandler.Progress(0, message)
    End Sub
    'Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
    '    'This sub fires when a menu item is clicked in MapWindow.  Here we check if the menu item
    '    'is one of the TauDEM related menus, and then take action accordingly. If we handle a menu
    '    'click, then we set "handled=true".  This tells MapWindow not to pass the event on to any
    '    'other plug-ins, or to the base application (in the case that the menu being captured is a
    '    'default menu such as "mnuPrint".  Note that we are using a naming convention for taudem
    '    'related menu items, where the name of the menu is the corresponding taudem function
    '    'preceded by "btdm".  It is assumed that this will help avoid naming conflicts with other
    '    'plugins.  - dpa 3/18/2005

    '    If ItemName.StartsWith("btdm") Then
    '        'We're going to run a taudem function.  Let's just reset the callback events guy just in case it was lost.
    '        'dpa 4/22/2005
    '        g_Taudem.Initialize(Me, tdbChoiceList.numProcesses)
    '    End If

    '    Select Case ItemName

    '        Case "btdmAutomatic"
    '            If g_AutoForm Is Nothing Or g_AutoForm.IsDisposed Then
    '                g_AutoForm = New frmAutomatic_v3
    '                Dim tempPtr As System.IntPtr = New System.IntPtr(g_handle)
    '                Dim mapFrm As System.Windows.Forms.Form = System.Windows.Forms.Control.FromHandle(tempPtr)
    '                mapFrm.AddOwnedForm(g_AutoForm)
    '            End If
    '            g_AutoForm.WindowState = Windows.Forms.FormWindowState.Normal
    '            g_AutoForm.Initialize(Me)
    '            g_AutoForm.Show()

    '            'Taudem Main Menu
    '        Case "btdmtaudem"  ' this occurs every time a button is clicked on TauDEM - put at top to ease debugging
    '            Exit Select

    '        Case "btdmSelectBaseDEM"

    '            DisplayMessage("TauDEM: Waiting for a base DEM.")
    '            g_Taudem.SelectDEM()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDoAllGrid"
    '            DisplayMessage("TauDEM: Doing all grid processing functions.")
    '            g_Taudem.doAll()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmOutlets"
    '            DisplayMessage("TauDEM: Waiting for an outlet shapefile.")
    '            g_Taudem.Outlets()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDoAllNetwork"
    '            DisplayMessage("TauDEM: Doing all network and watershed delineation functions.")
    '            g_Taudem.DoAllwsDelineate()
    '            App.Map.Refresh()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmHelp"
    '            g_Taudem.tdHelp()
    '            Handled = True

    '        Case "btdmAbout"
    '            'Dim f As New frmAbout_v3
    '            'f.ShowDialog()

    '            'Grid Menu Items
    '        Case "btdmFillPits"
    '            DisplayMessage("TauDEM: Filling pits.")
    '            g_Taudem.FillPits()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmD8"
    '            DisplayMessage("TauDEM: Computing D8 flow directions.")
    '            g_Taudem.D8()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDinf"
    '            DisplayMessage("TauDEM: Computing D-infinity flow directions.")
    '            g_Taudem.Dinf()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmAreaD8"
    '            DisplayMessage("TauDEM: Computing D8 area.")
    '            g_Taudem.AreaD8()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmAreaDinf"
    '            DisplayMessage("TauDEM: Computing D-infinity areas.")
    '            g_Taudem.AreaDinf()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmGridNet"
    '            DisplayMessage("TauDEM: Computing grid network order and flow path lengths.")
    '            g_Taudem.Gridnet()
    '            Handled = True
    '            DisplayMessage("")

    '            'Network Delineation Menu Items
    '        Case "btdmThreshold"
    '            DisplayMessage("TauDEM: Computing stream definitiion by threshold.")
    '            g_Taudem.Threshold()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmPK"
    '            DisplayMessage("TauDEM: Computing Peuker Douglas.")
    '            g_Taudem.PK()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmPKStreamDef"
    '            DisplayMessage("TauDEM: Peuker Douglas Stream Definition.")
    '            g_Taudem.PKStreamDef()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmSlopeAreaStreamDef"
    '            DisplayMessage("TauDEM: Slope Area Stream Definition.")
    '            g_Taudem.SlopeAreaStreamDef()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDropAnalysisStreamDef"
    '            DisplayMessage("TauDEM: Drop Analysis Stream Definition.")
    '            g_Taudem.DropAnalysisStreamDef()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmLengthArea"
    '            DisplayMessage("TauDEM: Computing length area.")
    '            g_Taudem.LengthArea()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmSlopeArea"
    '            DisplayMessage("TauDEM: Computing slope area.")
    '            g_Taudem.SlopeArea()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDropAnalysis"
    '            DisplayMessage("TauDEM: Computing drop analysis.")
    '            g_Taudem.DropAnalysis()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmD8FlowPathExtremeUp"
    '            DisplayMessage("TauDEM: Computing D8 extreme upslope value.")
    '            g_Taudem.D8FlowPathExtremeUp()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmStreamNet"
    '            DisplayMessage("TauDEM: Computing stream order grid and network files.")
    '            g_Taudem.StreamNet()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmWatershedToPoly"
    '            DisplayMessage("TauDEM: Converting watershed to grid to shapefile.")
    '            g_Taudem.WaterShedtoPoly()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmMoveOutlets"
    '            DisplayMessage("TauDEM: Moving Outlets to Streams.")
    '            g_Taudem.MoveOutlets()
    '            Handled = True
    '            DisplayMessage("")

    '            'Ancillary Functions
    '        Case "btdmSlopeOverArea"
    '            DisplayMessage("TauDEM: Computing slope/area ratio.")
    '            g_Taudem.SlopeOverArea()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmSlopeAveDown"
    '            DisplayMessage("TauDEM: Computing slope average down.")
    '            g_Taudem.SlopeAverageDown()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDistance"
    '            DisplayMessage("TauDEM: Computing distance to streams.")
    '            g_Taudem.Distance()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmAvalanche"
    '            DisplayMessage("TauDEM: Computing avalanche Runout.")
    '            g_Taudem.Avalanche()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmADinfDecay"
    '            DisplayMessage("TauDEM: Computing decaying accumulation.")
    '            g_Taudem.ADinfDecay()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDinfDistDown"
    '            DisplayMessage("TauDEM: Computing distance down.")
    '            g_Taudem.DinfDistDown()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDinfDistUp"
    '            DisplayMessage("TauDEM: Computing distance up.")
    '            g_Taudem.DinfDistUp()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmCLimAccum"
    '            DisplayMessage("TauDEM: Computing concentration limited accumulation.")
    '            g_Taudem.CLAccum()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmDependence"
    '            DisplayMessage("TauDEM: Computing upslope dependence.")
    '            g_Taudem.Dependence()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmTLAccum"
    '            DisplayMessage("TauDEM: Computing transport limited accumulation.")
    '            g_Taudem.TLAccum()
    '            Handled = True
    '            DisplayMessage("")

    '        Case "btdmRevAccum"
    '            DisplayMessage("TauDEM: Computing reverse accumulation.")
    '            g_Taudem.RevAccum()
    '            Handled = True
    '            DisplayMessage("")

    '    End Select
    'End Sub
#End Region

#Region "SWAT compatibility Stuff"
    'Public Sub InitializeAWDPaths(ByVal DEMPath As String, ByVal OutletsPath As String, ByVal RelativeOutputPath As String)
    '    Progress("Add", 0, DEMPath + "|Base DEM|1")
    '    lastDem = "Base DEM (" + IO.Path.GetFileName(DEMPath) + ") "

    '    If OutletsPath <> "" Then
    '        Progress("Add", 0, OutletsPath + "|Outlets Shapefile|20")
    '        lastOutlet = "Outlets Shapefile (" + IO.Path.GetFileName(OutletsPath) + ") "
    '        Dim sf As New MapWinGIS.Shapefile
    '        sf.Open(OutletsPath)
    '        App.Map.View.SelectedShapes.ClearSelectedShapes()
    '        For i As Integer = 0 To sf.NumShapes - 1
    '            App.Map.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
    '            outletShapesIdx.Add(i)
    '        Next
    '        g_AutoForm.lblOutletSelected.Text = outletShapesIdx.Count.ToString + " selected"
    '        sf.Close()
    '        tdbChoiceList.useOutlets = True
    '    End If

    '    tdbChoiceList.OutputPath = RelativeOutputPath
    'End Sub

    Public Sub InitializeAWDSettings(ByVal useDinf As Boolean, ByVal useEdgeCheck As Boolean, ByVal calcStreamFields As Boolean, ByVal calcWatershedFields As Boolean, ByVal calcMergeShedFields As Boolean, ByVal displayPitFill As Boolean, ByVal displayD8 As Boolean, ByVal displayAreaD8 As Boolean, ByVal displayDinf As Boolean, ByVal displayAreaDinf As Boolean, ByVal displayStrahlOrd As Boolean, ByVal displayNetRaster As Boolean, ByVal displayStreamOrd As Boolean, ByVal displayWatershedGrid As Boolean, ByVal displayStreamShapefile As Boolean, ByVal displayWatershedShapefile As Boolean, ByVal displayMergeWatershed As Boolean)
        tdbChoiceList.useDinf = useDinf
        tdbChoiceList.EdgeContCheck = useEdgeCheck
        tdbChoiceList.CalcSpecialStreamFields = calcStreamFields
        tdbChoiceList.CalcSpecialWshedFields = calcWatershedFields
        tdbChoiceList.calcSpecialMergeWshedFields = calcMergeShedFields
        tdbChoiceList.AddPitfillLayer = displayPitFill
        tdbChoiceList.AddD8Layer = displayD8
        tdbChoiceList.AddD8AreaLayer = displayAreaD8
        tdbChoiceList.AddDinfLayer = displayDinf
        tdbChoiceList.AddDinfAreaLayer = displayAreaDinf
        tdbChoiceList.AddGridNetLayer = displayStrahlOrd
        tdbChoiceList.AddRiverRasterLayer = displayNetRaster
        tdbChoiceList.AddOrderGridLayer = displayStreamOrd
        tdbChoiceList.AddWShedGridLayer = displayWatershedGrid
        tdbChoiceList.AddStreamShapeLayer = displayStreamShapefile
        tdbChoiceList.AddWShedShapeLayer = displayWatershedShapefile
        tdbChoiceList.AddMergedWShedShapeLayer = displayMergeWatershed
    End Sub

    Public Function HasBeenDelineated(ByVal demPath As String) As Boolean
        tdbFileList.FormFileNames(demPath, "", False)
        If IO.File.Exists(tdbFileList.fel) And IO.File.Exists(tdbFileList.mergewshed) And IO.File.Exists(tdbFileList.net) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function HasBeenDelineated(ByVal demPath As String, ByVal outputDirectory As String) As Boolean
        tdbFileList.FormFileNames(demPath, outputDirectory, False)
        If IO.File.Exists(tdbFileList.fel) And IO.File.Exists(tdbFileList.mergewshed) And IO.File.Exists(tdbFileList.net) Then
            Return True
        Else
            Return False
        End If
    End Function

    'Public ReadOnly Property CurrentDEMPath() As String
    '    Get
    '        Return g_AutoForm.getPathByName(lastDem)
    '    End Get
    'End Property

    Public ReadOnly Property CurrentDEMName() As String
        Get
            Return lastDem
        End Get
    End Property

    Public ReadOnly Property CurrentOutletPath() As String
        Get
            Return lastOutlet
        End Get
    End Property

    'Public ReadOnly Property CurrentOutletName() As String
    '    Get
    '        Return g_AutoForm.getPathByName(lastOutlet)
    '    End Get
    'End Property



    Public ReadOnly Property AutoForm() As frmAutomatic_v3
        Get
            Return g_AutoForm
        End Get
    End Property

    Public Property AutoFormIcon() As Drawing.Icon
        Get
            If Not g_AutoForm Is Nothing Then
                Return g_AutoForm.Icon
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Drawing.Icon)
            If Not g_AutoForm Is Nothing Then
                g_AutoForm.Icon = value
                g_AutoForm.ShowIcon = (Not value Is Nothing)
            End If
        End Set
    End Property

    Public ReadOnly Property FileList() As tdbFileTypes_v3
        Get
            Return tdbFileList
        End Get
    End Property
#End Region

#Region "Drawing and Selecting Events and Functions"

    'Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
    '    Dim CurrentLayerGood As Boolean = False
    '    Dim currPoint As New MapWinGIS.Point
    '    Dim locx, locy As Double

    '    If App.Map.Layers.NumLayers > 0 Then
    '        CurrentLayerGood = App.Map.Layers.Item(App.Map.Layers.CurrentLayer).FileName = currDrawPath
    '    End If

    '    If App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection Then
    '    ElseIf App.Map.View.CursorMode = MapWinGIS.tkCursorMode.cmNone Then
    '        If g_DrawingMask And CurrentLayerGood Then
    '            If Button = 2 Then
    '                AddPolyToShapefile()
    '                ClearTempLines()
    '                frmDrawSelect.disableDone(False)
    '            Else
    '                ' Get actual location and store it in a point which is added to point list
    '                App.Map.View.PixelToProj(x, y, locx, locy)
    '                currPoint.x = locx
    '                currPoint.y = locy
    '                lstDrawPoints.Add(currPoint)
    '                frmDrawSelect.disableDone(True)
    '                Dim mydraw As MapWindow.Interfaces.Draw = App.Map.View.Draw
    '                mydraw.DrawPoint(locx, locy, 3, Drawing.Color.Red)
    '                If (LastStartPtX = -1) Then
    '                    LastStartPtX = System.Windows.Forms.Control.MousePosition.X
    '                    LastStartPtY = System.Windows.Forms.Control.MousePosition.Y
    '                    StartPtX = LastStartPtX
    '                    StartPtY = LastStartPtY
    '                    EraseLast = False
    '                Else
    '                    'Reverse the one to the start place
    '                    System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
    '                    'Permanently draw line (already drawn, don't erase -- just move it)
    '                    ReversibleDrawn.Add(LastStartPtX)
    '                    ReversibleDrawn.Add(LastStartPtY)
    '                    ReversibleDrawn.Add(System.Windows.Forms.Control.MousePosition.X)
    '                    ReversibleDrawn.Add(System.Windows.Forms.Control.MousePosition.Y)

    '                    'Update for next loop
    '                    LastStartPtX = System.Windows.Forms.Control.MousePosition.X
    '                    LastStartPtY = System.Windows.Forms.Control.MousePosition.Y

    '                    EraseLast = False
    '                End If
    '            End If
    '        End If

    '        ' this section revised by cwg 16/10/09 to avoid memory exception when
    '        ' stoppping editing shapefile, and later error when outlet has null value
    '        ' for INLET field
    '        ' - explicit pointIndex and shapeIndex variables rather than pointer
    '        '   to count of object being added to
    '        ' - INLET and RES fields always added, so all values set explicitly
    '        If g_DrawingOutletsOrInlets And CurrentLayerGood Then
    '            ' Get actual location and store it in a point which is added to point list
    '            App.Map.View.PixelToProj(x, y, locx, locy)
    '            currPoint.x = locx
    '            currPoint.y = locy

    '            Dim tempPt As New MapWinGIS.Shape
    '            tempPt.Create(MapWinGIS.ShpfileType.SHP_POINT)
    '            Dim pointIndex As Integer = tempPt.numPoints
    '            tempPt.InsertPoint(currPoint, pointIndex)
    '            Dim sf As MapWinGIS.Shapefile = App.Map.Layers.Item(App.Map.Layers.CurrentLayer).GetObject()
    '            Dim shapeIndex As Integer = sf.NumShapes

    '            If sf.StartEditingShapes() Then
    '                ' Make sure we have ID, INLET, RES and PTSOURCE fields
    '                Dim idfieldnum As Integer = -1
    '                For i As Integer = 0 To sf.NumFields - 1
    '                    If sf.Field(i).Name.ToUpper() = "ID" Then
    '                        idfieldnum = i
    '                        Exit For
    '                    End If
    '                Next i
    '                If idfieldnum = -1 Then
    '                    Dim idField As New MapWinGIS.Field
    '                    idField.Name = "ID"
    '                    idField.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                    idfieldnum = sf.NumFields
    '                    sf.EditInsertField(idField, idfieldnum)
    '                End If

    '                Dim inletfieldnum As Integer = -1
    '                For i As Integer = 0 To sf.NumFields - 1
    '                    If sf.Field(i).Name.ToUpper() = "INLET" Then
    '                        inletfieldnum = i
    '                        Exit For
    '                    End If
    '                Next i
    '                If inletfieldnum = -1 Then
    '                    Dim inletField As New MapWinGIS.Field
    '                    inletField.Name = "INLET"
    '                    inletField.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                    inletfieldnum = sf.NumFields
    '                    sf.EditInsertField(inletField, inletfieldnum)
    '                End If

    '                Dim resfieldnum As Integer = -1
    '                For i As Integer = 0 To sf.NumFields - 1
    '                    If sf.Field(i).Name.ToUpper() = "RES" Then
    '                        resfieldnum = i
    '                        Exit For
    '                    End If
    '                Next
    '                If resfieldnum = -1 Then
    '                    Dim resField As New MapWinGIS.Field
    '                    resField.Name = "RES"
    '                    resField.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                    resfieldnum = sf.NumFields
    '                    sf.EditInsertField(resField, resfieldnum)
    '                End If

    '                Dim srcfieldnum As Integer = -1
    '                For i As Integer = 0 To sf.NumFields - 1
    '                    If sf.Field(i).Name.ToUpper() = "PTSOURCE" Then
    '                        srcfieldnum = i
    '                        Exit For
    '                    End If
    '                Next
    '                If srcfieldnum = -1 Then
    '                    Dim srcField As New MapWinGIS.Field
    '                    srcField.Name = "PTSOURCE"
    '                    srcField.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                    srcfieldnum = sf.NumFields
    '                    sf.EditInsertField(srcField, srcfieldnum)
    '                End If

    '                sf.EditInsertShape(tempPt, shapeIndex)
    '                outletShapesIdx.Add(shapeIndex)

    '                If g_DrawingInlets Then
    '                    sf.EditCellValue(inletfieldnum, shapeIndex, 1)
    '                Else
    '                    sf.EditCellValue(inletfieldnum, shapeIndex, 0)
    '                End If

    '                If g_DrawingReservoir Then
    '                    sf.EditCellValue(resfieldnum, shapeIndex, 1)
    '                Else
    '                    sf.EditCellValue(resfieldnum, shapeIndex, 0)
    '                End If

    '                If g_DrawingPointSource Then
    '                    sf.EditCellValue(srcfieldnum, shapeIndex, 1)
    '                Else
    '                    sf.EditCellValue(srcfieldnum, shapeIndex, 0)
    '                End If

    '                ReNumberMWShapeIDs(sf)

    '                sf.StopEditingShapes()
    '                App.Map.Refresh()
    '            End If


    '        End If
    '    End If
    'End Sub

    'Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
    '    'mycolor = System.Drawing.Color.FromArgb(245, 230, 200)
    '    mycolor = Drawing.Color.White
    '    If g_DrawingMask Then
    '        If EraseLast Then
    '            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(LastStartPtX, LastStartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
    '            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
    '        End If

    '        If Not LastStartPtX = -1 Then
    '            LastEndX = System.Windows.Forms.Control.MousePosition.X
    '            LastEndY = System.Windows.Forms.Control.MousePosition.Y
    '            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(LastStartPtX, LastStartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
    '            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
    '            EraseLast = True
    '        End If
    '    End If
    'End Sub

    'Private Sub ClearTempLines()
    '    If (EraseLast) Then
    '        System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(LastStartPtX, LastStartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
    '        System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)

    '        LastStartPtX = -1
    '        LastStartPtY = -1
    '    End If

    '    For i As Integer = 0 To ReversibleDrawn.Count - 1 Step 4
    '        System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(ReversibleDrawn(i), ReversibleDrawn(i + 1)), New System.Drawing.Point(ReversibleDrawn(i + 2), ReversibleDrawn(i + 3)), mycolor)
    '    Next
    '    EraseLast = False
    '    ReversibleDrawn.Clear()
    '    lstDrawPoints.Clear()

    '    Dim mydraw As MapWindow.Interfaces.Draw = App.Map.View.Draw
    '    mydraw.ClearDrawings()
    'End Sub

    'Private Sub AddPolyToShapefile()
    '    ' Only try to add if the edit points has something in it
    '    If lstDrawPoints.Count > 1 Then
    '        Dim i As Integer
    '        ' Create poly and set it's type to polyline IMPORTANT
    '        Dim tempPoly As New MapWinGIS.Shape
    '        tempPoly.Create(MapWinGIS.ShpfileType.SHP_POLYGON)

    '        ' Loop the points, inserting them into new poly
    '        For i = 0 To lstDrawPoints.Count - 1
    '            tempPoly.InsertPoint(lstDrawPoints(i), tempPoly.numPoints)
    '        Next
    '        'Add the first point to complete the poly
    '        tempPoly.InsertPoint(lstDrawPoints(0), tempPoly.numPoints)

    '        Dim sf As MapWinGIS.Shapefile = App.Map.Layers.Item(App.Map.Layers.CurrentLayer).GetObject

    '        If sf.StartEditingShapes() Then
    '            maskShapesIdx.Add(sf.NumShapes)
    '            sf.EditInsertShape(tempPoly, sf.NumShapes)
    '            sf.StopEditingShapes()
    '            App.Map.Refresh()
    '        End If
    '    End If
    'End Sub

    'Private Sub ReNumberMWShapeIDs(ByRef sf As MapWinGIS.Shapefile)
    '    ' CWG 26/1/11 field that was called MWShapeID has to be called ID for Taudem V5
    '    Dim idfieldnum As Integer = -1
    '    For i As Integer = 0 To sf.NumFields - 1
    '        If sf.Field(i).Name.ToUpper() = "ID" Then
    '            idfieldnum = i
    '            Exit For
    '        End If
    '    Next

    '    If idfieldnum <> -1 Then
    '        For i As Integer = 0 To sf.NumShapes - 1
    '            sf.EditCellValue(idfieldnum, i, i)
    '        Next
    '    End If
    'End Sub

    '<CLSCompliant(False)> _
    'Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    '    Dim CurrentLayerGood As Boolean
    '    If g_SelectingMask Or g_DrawingMask Or g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
    '        If App.Map.Layers.NumLayers > 0 Then
    '            CurrentLayerGood = App.Map.Layers.Item(App.Map.Layers.CurrentLayer).FileName = currSelectPath
    '        End If

    '        If Not CurrentLayerGood Then
    '            App.Map.View.SelectedShapes.ClearSelectedShapes()
    '            App.Map.Layers.CurrentLayer = g_AutoForm.getIndexByPath(currSelectPath)
    '        End If
    '    End If
    'End Sub
#End Region

#Region "Helper Functions"

    '' CWG 30/1/11 changed to use MapWinGIS.ICallback instead of Taudem callback
    'Public Sub Progress(ByVal KeyOfSender As String, ByVal Percent As Integer, ByVal Message As String) Implements MapWinGIS.ICallback.Progress
    '    'This is where we receive the progress events sent from mwTaudem.dll through the g_mwEvents object.
    '    'We use this to get instructions from mwTaudem.dll (add/remove layers) and also for progress events.
    '    'dpa 4/22/2005 - updated to remove layers by handle not index.

    '    Dim FileType As Integer
    '    Dim newlayer As MapWindow.Interfaces.Layer
    '    Select Case KeyOfSender
    '        Case "Remove"
    '            'mwTaudem.dll wants to remove a layer from the map
    '            For Each layer As MapWindow.Interfaces.Layer In App.Map.Layers
    '                If LCase(layer.FileName) = LCase(Message) Then
    '                    App.Map.Layers.Remove(layer.Handle)
    '                End If
    '            Next
    '        Case "Add"
    '            'mwTaudem.dll wants to add a layer to the map
    '            Dim TDGroup As Integer = GetTDGroup()
    '            Dim Temp() As String = Split(Message, "|")
    '            Dim FileName As String = Temp(0)
    '            FileType = CInt(Temp(2))
    '            If FileType = 1 Then
    '                'Make a note that this is the base DEM
    '                g_BaseDEM = FileName
    '            End If

    '            Dim LegendName As String
    '            LegendName = System.IO.Path.GetFileName(FileName)
    '            If (LegendName = "sta.adf") Then
    '                LegendName = System.IO.Path.GetDirectoryName(FileName)
    '                LegendName = System.IO.Path.GetFileName(LegendName)
    '            End If

    '            Dim FileDesc As String = Temp(1) & " (" & LegendName & ") " ' & FileType
    '            If System.IO.File.Exists(FileName) Then
    '                If Not InStr(tdbFileList.GetGridFilter(), System.IO.Path.GetExtension(FileName)) = 0 Then
    '                    Dim tmpGrid As New MapWinGIS.Grid
    '                    tmpGrid.Open(FileName)
    '                    ' Paul Meems, 11-Aug-11 Added extra check if projection was not already set:
    '                    If tmpGrid.Header.Projection = String.Empty AndAlso App.Map.Project.ProjectProjection <> String.Empty Then
    '                        tmpGrid.AssignNewProjection(App.Map.Project.ProjectProjection)
    '                    End If

    '                    Dim CS As MapWinGIS.GridColorScheme = GetColorScheme(FileType, CDbl(tmpGrid.Minimum), CDbl(tmpGrid.Maximum))
    '                    newlayer = App.Map.Layers.Add(tmpGrid, CS, FileDesc)
    '                Else
    '                    newlayer = App.Map.Layers.Add(FileName, FileDesc)
    '                End If
    '                newlayer.MoveTo(App.Map.Layers.NumLayers, TDGroup)

    '                If Not newlayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
    '                    Dim shapeFile As MapWinGIS.Shapefile = CType(newlayer.GetObject(), MapWinGIS.Shapefile)
    '                    If FileType = 19 Then 'watershed shapefile
    '                        shapeFile.DefaultDrawingOptions.LineColor = MapWinUtility.Colors.ColorToUInteger(System.Drawing.Color.Red)
    '                        shapeFile.DefaultDrawingOptions.LineWidth = 3
    '                        newlayer.DrawFill = False
    '                    End If
    '                    If FileType = 15 Then 'stream shapefile
    '                        shapeFile.DefaultDrawingOptions.LineColor = MapWinUtility.Colors.ColorToUInteger(System.Drawing.Color.Blue)
    '                        shapeFile.DefaultDrawingOptions.LineWidth = 2
    '                    End If
    '                    If FileType = 43 Then 'mask shapefile
    '                        shapeFile.DefaultDrawingOptions.LineColor = MapWinUtility.Colors.ColorToUInteger(System.Drawing.Color.LightGreen)
    '                        shapeFile.DefaultDrawingOptions.LineWidth = 3
    '                        newlayer.DrawFill = False
    '                    End If
    '                    If FileType = 20 Or FileType = 44 Then 'outletshape
    '                        Dim cats As MapWinGIS.ShapefileCategoriesClass = New MapWinGIS.ShapefileCategoriesClass()
    '                        Dim outletCat As New MapWinGIS.ShapefileCategoryClass()
    '                        outletCat = cats.Add("Outlets")
    '                        outletCat.DrawingOptions.PointShape = MapWinGIS.tkPointShapeType.ptShapeRegular
    '                        outletCat.DrawingOptions.PointSidesCount = 3
    '                        outletCat.DrawingOptions.PointSize = 10
    '                        outletCat.DrawingOptions.FillColor = MapWinUtility.Colors.ColorToUInteger(Drawing.Color.Aqua)
    '                        outletCat.DrawingOptions.LineVisible = False
    '                        outletCat.Expression = "[INLET] = 0 AND [RES] = 0"
    '                        outletCat.DrawingOptions.Visible = True
    '                        Dim resCat As New MapWinGIS.ShapefileCategoryClass()
    '                        resCat = cats.Add("Reservoirs")
    '                        resCat.DrawingOptions.PointShape = MapWinGIS.tkPointShapeType.ptShapeCircle
    '                        resCat.DrawingOptions.PointSize = 10
    '                        resCat.DrawingOptions.FillColor = MapWinUtility.Colors.ColorToUInteger(Drawing.Color.Aqua)
    '                        resCat.DrawingOptions.LineVisible = False
    '                        resCat.Expression = "[INLET] = 0 AND [RES] = 1"
    '                        resCat.DrawingOptions.Visible = True
    '                        Dim inletCat As New MapWinGIS.ShapefileCategoryClass()
    '                        inletCat = cats.Add("Inlets")
    '                        inletCat.DrawingOptions.PointShape = MapWinGIS.tkPointShapeType.ptShapeRegular
    '                        inletCat.DrawingOptions.PointSidesCount = 3
    '                        inletCat.DrawingOptions.PointRotation = 90
    '                        inletCat.DrawingOptions.PointSize = 10
    '                        inletCat.DrawingOptions.FillColor = MapWinUtility.Colors.ColorToUInteger(Drawing.Color.Blue)
    '                        inletCat.DrawingOptions.LineVisible = False
    '                        inletCat.Expression = "[INLET] = 1 AND [PTSOURCE] = 0"
    '                        inletCat.DrawingOptions.Visible = True
    '                        Dim ptsourceCat As New MapWinGIS.ShapefileCategoryClass()
    '                        ptsourceCat = cats.Add("Point sources")
    '                        ptsourceCat.DrawingOptions.PointShape = MapWinGIS.tkPointShapeType.ptShapeRegular
    '                        ptsourceCat.DrawingOptions.PointSidesCount = 3
    '                        ptsourceCat.DrawingOptions.PointRotation = 30
    '                        ptsourceCat.DrawingOptions.PointSize = 10
    '                        ptsourceCat.DrawingOptions.FillColor = MapWinUtility.Colors.ColorToUInteger(Drawing.Color.Blue)
    '                        ptsourceCat.DrawingOptions.LineVisible = False
    '                        ptsourceCat.Expression = "[INLET] = 1 AND [PTSOURCE] = 1"
    '                        ptsourceCat.DrawingOptions.Visible = True
    '                        Dim tmpout As MapWinGIS.Shapefile = CType(newlayer.GetObject(), MapWinGIS.Shapefile)
    '                        tmpout.Categories = cats

    '                        ' Check we have all the columns
    '                        Dim idfieldnum As Integer = -1
    '                        Dim inletfieldnum As Integer = -1
    '                        Dim resfieldnum As Integer = -1
    '                        Dim ptsourcefieldnum As Integer = -1
    '                        For z As Integer = 0 To tmpout.NumFields - 1
    '                            Dim col As String = tmpout.Field(z).Name.ToUpper()
    '                            If col = "ID" Then
    '                                idfieldnum = z
    '                            ElseIf col = "INLET" Then
    '                                inletfieldnum = z
    '                            ElseIf col = "RES" Then
    '                                resfieldnum = z
    '                            ElseIf col = "PTSOURCE" Then
    '                                ptsourcefieldnum = z
    '                            End If
    '                        Next z
    '                        Dim OK As Boolean = True
    '                        If idfieldnum < 0 OrElse inletfieldnum < 0 OrElse resfieldnum < 0 OrElse ptsourcefieldnum < 0 Then
    '                            OK = tmpout.StartEditingTable()
    '                            If OK Then
    '                                If idfieldnum < 0 Then
    '                                    Dim idfield As New MapWinGIS.FieldClass()
    '                                    idfield.Name = "ID"
    '                                    idfield.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                                    OK = tmpout.EditInsertField(idfield, idfieldnum)
    '                                    If OK Then
    '                                        For s As Integer = 0 To tmpout.NumShapes - 1
    '                                            tmpout.EditCellValue(idfieldnum, s, s)
    '                                        Next
    '                                    End If
    '                                End If
    '                                If inletfieldnum < 0 Then
    '                                    Dim inletfield As New MapWinGIS.FieldClass()
    '                                    inletfield.Name = "INLET"
    '                                    inletfield.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                                    OK = tmpout.EditInsertField(inletfield, inletfieldnum)
    '                                    If OK Then
    '                                        For s As Integer = 0 To tmpout.NumShapes - 1
    '                                            tmpout.EditCellValue(inletfieldnum, s, 0)
    '                                        Next
    '                                    End If
    '                                End If
    '                                If resfieldnum < 0 Then
    '                                    Dim resfield As New MapWinGIS.FieldClass()
    '                                    resfield.Name = "res"
    '                                    resfield.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                                    OK = tmpout.EditInsertField(resfield, resfieldnum)
    '                                    If OK Then
    '                                        For s As Integer = 0 To tmpout.NumShapes - 1
    '                                            tmpout.EditCellValue(resfieldnum, s, 0)
    '                                        Next
    '                                    End If
    '                                End If
    '                                If ptsourcefieldnum < 0 Then
    '                                    Dim ptsourcefield As New MapWinGIS.FieldClass()
    '                                    ptsourcefield.Name = "ptsource"
    '                                    ptsourcefield.Type = MapWinGIS.FieldType.INTEGER_FIELD
    '                                    OK = tmpout.EditInsertField(ptsourcefield, ptsourcefieldnum)
    '                                    If OK Then
    '                                        For s As Integer = 0 To tmpout.NumShapes - 1
    '                                            tmpout.EditCellValue(ptsourcefieldnum, s, 0)
    '                                        Next
    '                                    End If
    '                                End If
    '                            End If
    '                            If tmpout.EditingTable Then
    '                                tmpout.StopEditingTable()
    '                            End If
    '                        End If
    '                        If OK Then
    '                            cats.ApplyExpressions()
    '                        End If

    '                    End If
    '                    If FileType = 45 Then
    '                        newlayer.LineOrPointSize = 3
    '                        newlayer.OutlineColor = Drawing.Color.Cyan
    '                        newlayer.Color = Drawing.Color.DarkCyan
    '                        newlayer.DrawFill = True
    '                        newlayer.ShapeLayerFillTransparency = 0.75
    '                    End If
    '                Else
    '                    ' fix problem with types 12, 46: stream source grid
    '                    ' and 14: watershed grid
    '                    If FileType = 12 OrElse FileType = 14 OrElse FileType = 46 Then
    '                        newlayer.ImageTransparentColor = Drawing.Color.Black
    '                    End If
    '                End If
    '            End If
    '        Case "GetTOC"
    '            'mwTaudem.dll wants us to build a string of filenames and layer names for it to choose from
    '            Dim FilesAndNames As String = ""
    '            Dim j As Integer
    '            For i As Integer = 0 To App.Map.Layers.NumLayers - 1
    '                Dim LyrHandle As Integer = App.Map.Layers.GetHandle(i)
    '                If App.Map.Layers(LyrHandle).LayerType = MapWindow.Interfaces.eLayerType.Grid Then
    '                    j += 1
    '                    If j > 1 Then FilesAndNames = FilesAndNames & "|" 'add a separator
    '                    FilesAndNames = FilesAndNames & App.Map.Layers(LyrHandle).Name & "," & App.Map.Layers(LyrHandle).FileName
    '                End If
    '            Next
    '            g_Taudem.MapTableOfContents = FilesAndNames
    '        Case Else
    '            'mwTaudem.dll has given us some other message that we will display as a status bar message
    '            App.Map.StatusBar.ProgressBarValue = Percent
    '            g_StatusBarItem.Text = Message
    '            System.Windows.Forms.Application.DoEvents()
    '    End Select
    'End Sub

    'Private Function GetColorScheme(ByVal FileType As Integer, ByVal Min As Double, ByVal MAx As Double) As MapWinGIS.GridColorScheme
    '    'This function generates a coloring scheme based on the Taudem filetype - dpa 4/23/2005
    '    Dim CS As New MapWinGIS.GridColorScheme
    '    Dim BR As New MapWinGIS.GridColorBreak
    '    Dim R1 As Integer, R2 As Integer, G1 As Integer, G2 As Integer, B1 As Integer, B2 As Integer
    '    Dim HighVal As Integer, StepVal As Integer, MidVal As Integer, i As Integer
    '    Try
    '        Select Case FileType
    '            Case 1 'Base DEM
    '                CS.UsePredefined(Min, MAx, MapWinGIS.PredefinedColorScheme.SummerMountains)
    '                Return CS
    '            Case 2 'Pit filled DEM
    '                CS.UsePredefined(Min, MAx, MapWinGIS.PredefinedColorScheme.SummerMountains)
    '                Return CS
    '            Case 3 ' D8 Slope - eight unique colors
    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 1 : BR.HighValue = 1
    '                BR.LowColor = System.Convert.ToUInt32(RGB(46, 139, 87))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(46, 139, 87))
    '                BR.Caption = "East"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 2 : BR.HighValue = 2
    '                BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    '                BR.Caption = "Northeast"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 3 : BR.HighValue = 3
    '                BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    '                BR.Caption = "North"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 4 : BR.HighValue = 4
    '                BR.LowColor = System.Convert.ToUInt32(RGB(218, 165, 32))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(218, 165, 32))
    '                BR.Caption = "Northwest"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 5 : BR.HighValue = 5
    '                BR.LowColor = System.Convert.ToUInt32(RGB(222, 184, 135))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(222, 184, 135))
    '                BR.Caption = "West"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 6 : BR.HighValue = 6
    '                BR.LowColor = System.Convert.ToUInt32(RGB(255, 0, 0))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(255, 0, 0))
    '                BR.Caption = "Southwest"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 7 : BR.HighValue = 7
    '                BR.LowColor = System.Convert.ToUInt32(RGB(153, 50, 204))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(153, 50, 204))
    '                BR.Caption = "South"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 8 : BR.HighValue = 8
    '                BR.LowColor = System.Convert.ToUInt32(RGB(123, 104, 238))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(123, 104, 238))
    '                BR.Caption = "Southeast"
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)
    '                Return CS

    '            Case 4, 6 'D8 slope, Dinf slope
    '                BR = New MapWinGIS.GridColorBreak ' white to green
    '                BR.LowValue = 0 : BR.HighValue = 1
    '                R1 = 255 : G1 = 255 : B1 = 255 : R2 = 0 : G2 = 255 : B2 = 0
    '                BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak 'green to dark green
    '                BR.LowValue = 1 : BR.HighValue = MAx
    '                R1 = 0 : G1 = 255 : B1 = 0 : R2 = 0 : G2 = 100 : B2 = 0
    '                BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                CS.InsertBreak(BR)
    '                Return CS

    '            Case 5 'Dinf flow dir - white to brown
    '                BR.LowValue = Min
    '                BR.HighValue = MAx
    '                R1 = 255 : G1 = 255 : B1 = 255 : R2 = 139 : G2 = 69 : B2 = 19
    '                BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                CS.InsertBreak(BR)
    '                Return CS

    '            Case 7, 8, 47 'D8 area, Dinf area, ssa - shading from white to red
    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = Min : BR.HighValue = MAx
    '                BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(255, 0, 0))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                BR.GradientModel = MapWinGIS.GradientModel.Logorithmic
    '                CS.InsertBreak(BR)
    '                Return CS

    '            Case 9, 13 'grid order stream order - shading from yellow to green to blue
    '                'yellow = RGB(255, 255, 0)
    '                'green = RGB(0,255,0)
    '                'blue = RGB(0,0,255)
    '                ' CWG
    '                HighVal = CInt(MAx)
    '                If HighVal > 1 Then
    '                    MidVal = CInt(HighVal / 2)
    '                    StepVal = CInt(255 / MidVal)
    '                Else
    '                    MidVal = 0
    '                    StepVal = 1
    '                End If
    '                ' CWG Make first break black (background)
    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 1 : BR.HighValue = 1
    '                BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 0))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 0))
    '                BR.Caption = 1
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)
    '                For i = 2 To MidVal
    '                    'color breaks from yellow to green
    '                    R1 = 255 - StepVal * (i - 1) : G1 = 255 : B1 = 0
    '                    '  Guard against invalid colors DGT 10/1/05
    '                    If (R1 < 0) Then R1 = 0
    '                    If (R1 > 255) Then R1 = 255
    '                    BR = New MapWinGIS.GridColorBreak
    '                    BR.LowValue = i : BR.HighValue = i
    '                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                    BR.HighColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                    BR.Caption = i
    '                    BR.ColoringType = MapWinGIS.ColoringType.Random
    '                    CS.InsertBreak(BR)
    '                Next
    '                For i = MidVal + 1 To HighVal
    '                    'color breaks from green to blue
    '                    R1 = 0 : G1 = 255 - StepVal * (i - MidVal - 1) : B1 = 0 + StepVal * (i - MidVal - 1)
    '                    '   Guard against invalid colors   DGT 10/1/05
    '                    If (G1 < 0) Then G1 = 0
    '                    If (B1 < 0) Then B1 = 0
    '                    If (G1 > 255) Then G1 = 255
    '                    If (B1 > 255) Then B1 = 255
    '                    BR = New MapWinGIS.GridColorBreak
    '                    BR.LowValue = i : BR.HighValue = i
    '                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                    BR.HighColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                    BR.Caption = i
    '                    BR.ColoringType = MapWinGIS.ColoringType.Random
    '                    CS.InsertBreak(BR)
    '                Next
    '                CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                Return CS

    '            Case 10, 11 'Longest upslope path and length

    '                'yellow to green
    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = Min : BR.HighValue = MAx / 10
    '                BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(0, 255, 0))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                CS.InsertBreak(BR)

    '                'green to blue
    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = MAx / 10 : BR.HighValue = MAx
    '                BR.LowColor = System.Convert.ToUInt32(RGB(0, 255, 0))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                CS.InsertBreak(BR)
    '                Return CS

    '            Case 12, 46 ' Stream raster src and ss white for 0 and blue elsewhere
    '                BR = New MapWinGIS.GridColorBreak
    '                CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                BR.LowValue = 0 : BR.HighValue = 0
    '                BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)

    '                BR = New MapWinGIS.GridColorBreak
    '                BR.LowValue = 1 : BR.HighValue = 1
    '                BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    '                BR.ColoringType = MapWinGIS.ColoringType.Random
    '                CS.InsertBreak(BR)
    '                Return CS

    '            Case 14  ' Watershed grid.  White for no data random colors elsewhere.
    '                ' Taudem V5 numbers from 0
    '                HighVal = CInt(MAx)
    '                For i = 0 To HighVal
    '                    R1 = Int(Rnd() * 255) : G1 = Int(Rnd() * 255) : B1 = Int(Rnd() * 255)
    '                    BR = New MapWinGIS.GridColorBreak
    '                    BR.LowValue = i : BR.HighValue = i
    '                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                    BR.HighColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                    BR.Caption = i
    '                    BR.ColoringType = MapWinGIS.ColoringType.Random
    '                    CS.InsertBreak(BR)
    '                Next
    '                CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                Return CS
    '            Case 18, 52, 53 'distance grid: gradient white down to black
    '                BR.HighColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                BR.LowColor = System.Convert.ToUInt32(RGB(1, 1, 1))
    '                BR.HighValue = MAx
    '                BR.LowValue = Min
    '                BR.ColoringType = 1 ' no hillshade
    '                CS.InsertBreak(BR)
    '                Return CS
    '            Case 43 ' mask grid
    '                BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 0))
    '                BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 0))
    '                BR.HighValue = MAx
    '                BR.LowValue = Min
    '                BR.ColoringType = 1 ' no hillshade
    '                CS.InsertBreak(BR)
    '                CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
    '                Return CS
    '            Case 48 ' Slope Area
    '                CS.UsePredefined(Min, MAx, MapWinGIS.PredefinedColorScheme.ValleyFires)
    '                Return CS
    '            Case Else
    '                BR.LowValue = Min
    '                BR.HighValue = MAx
    '                R1 = Int(Rnd() * 255) : R2 = Int(Rnd() * 255) : G1 = Int(Rnd() * 255) : G2 = Int(Rnd() * 255) : B1 = Int(Rnd() * 255) : B2 = Int(Rnd() * 255)
    '                BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
    '                BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
    '                BR.ColoringType = MapWinGIS.ColoringType.Gradient
    '                CS.InsertBreak(BR)
    '                Return CS

    '        End Select
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    '    Return Nothing
    'End Function

    'Private Function GetTDGroup() As Integer
    '    'Returns the handle for the Taudem group.  It's looking for the first group named "Terrain Analysis"
    '    'dpa 4/22/05
    '    Dim i As Integer
    '    For i = 0 To App.Map.Layers.Groups.Count - 1
    '        If App.Map.Layers.Groups.ItemByPosition(i).Text = "Terrain Analysis" Then
    '            Return App.Map.Layers.Groups.ItemByPosition(i).Handle
    '        End If
    '    Next
    '    'if we get here then the group wasn't found, so we can add it and return the handle.
    '    Return App.Map.Layers.Groups.Add("Terrain Analysis")
    'End Function
#End Region


    Private Sub AutoWatershedDelineation_Click(ByVal sender As Object, ByVal e As EventArgs)
        If g_AutoForm Is Nothing Or g_AutoForm.IsDisposed Then
            g_AutoForm = New frmAutomatic_v3
        End If
        g_AutoForm.WindowState = System.Windows.Forms.FormWindowState.Normal
        g_AutoForm.Initialize(Me)
        g_AutoForm.Show()
    End Sub

End Class


' The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
' you may not use this file except in compliance with the License. You may obtain a copy of the License at 
' http://www.mozilla.org/MPL/ 
' Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
' ANY KIND, either express or implied. See the License for the specific terms governing rights and 
' limitations under the License. 
'
'Part of code for MWTauDEM library supporting TauDEM (http://hydrology.usu.edu/taudem/taudem5.0/)
'for MapWindow (http://www.mapwindow.org)

Module Globals_v3

    'Public g_Taudem As New mwTauDem.mwTaudemInterface
    Public g_BaseDEM As String
    Public g_AutoForm As New frmAutomatic_v3
    ' CWG 30/1/11 Changed for TauDEM V5
    ' Public g_TaudemLib As New TKTAUDEMLib.TauDEM
    'Public g_StatusBarItem As MapWindow.Interfaces.StatusBarItem
    Public g_MaxFileSize As Long = 4000000000 '4 GB for Taudem V5; was 62914560 (60 meg)
    Public g_DrawingMask As Boolean = False
    Public g_SelectingMask As Boolean = False
    Public g_DrawingOutletsOrInlets As Boolean = False
    Public g_DrawingInlets As Boolean = False
    Public g_DrawingReservoir As Boolean = False
    Public g_DrawingPointSource As Boolean = False
    Public g_SelectingOutlets As Boolean = False
    'Public frmDrawSelect As New frmDrawSelectShape_v3
    Public frmLoadOutput As New frmLoadOutputs
    Public frmLoadDelinOutput As New frmLoadDelinOutputs

    'Flags for running delin steps efficiently.
    Public preProcHasRan As Boolean = False
    Public threshDelinHasRan As Boolean = False

    'Variables for previous state 
    Public lastDem As String
    Public lastOutlet As String
    Public lastStream As String
    Public lastMask As String
    Public lastThresh As String
    Public lastConvUnit As Integer = -1
    Public currSelectPath As String
    Public currDrawPath As String

    ' Actual type objects
    Public tdbFileList As tdbFileTypes_v3
    Public tdbChoiceList As tdbChoices_v3
End Module

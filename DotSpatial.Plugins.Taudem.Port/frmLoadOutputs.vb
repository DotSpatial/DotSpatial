' The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
' you may not use this file except in compliance with the License. You may obtain a copy of the License at 
' http://www.mozilla.org/MPL/ 
' Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
' ANY KIND, either express or implied. See the License for the specific terms governing rights and 
' limitations under the License. 
'
'Part of code for MWTauDEM library supporting TauDEM (http://hydrology.usu.edu/taudem/taudem5.0/)
'for MapWindow (http://www.mapwindow.org)

Public Class frmLoadOutputs
    Public fillPath As String
    Public sd8Path As String
    Public d8Path As String
    Public dInfPath As String
    Public dinfSlopePath As String
    Public selectedDem As String
    Public maskedDem As String

    Private Function GetDefaultFilename(ByVal append As String) As String
        Dim selectedDemBase As String = System.IO.Path.GetFileNameWithoutExtension(selectedDem)

        Dim suggestedFilename As String = selectedDemBase & append & ".tif"

        ' Check if the file exists:
        If Not System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(selectedDem), suggestedFilename)) Then
            suggestedFilename = String.Empty
        End If

        Return suggestedFilename
    End Function

    Private Function GetFilenameFromUser(ByVal append As String) As String
        'fdgOpen.Filter = tdbFileList.GetGridFilter()
        fdgOpen.FilterIndex = 1
        fdgOpen.InitialDirectory = System.IO.Path.GetDirectoryName(selectedDem)
        fdgOpen.CheckFileExists = True

        'Default filename:
        fdgOpen.FileName = GetDefaultFilename(append)
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return fdgOpen.FileName
        End If

        Return String.Empty

    End Function
    Private Sub btnBrowseFill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseFill.Click
        txtbxFill.Text = GetFilenameFromUser("fel")
    End Sub

    Private Sub btnBrowseD8Slope_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseD8Slope.Click
        txtbxD8Slope.Text = GetFilenameFromUser("sd8")
    End Sub

    Private Sub btnBrowseD8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseD8.Click
        txtbxD8.Text = GetFilenameFromUser("p")
    End Sub

    Private Sub btnBrowseDinfSlope_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDinfSlope.Click
        txtbxDinfSlope.Text = GetFilenameFromUser("slp")
    End Sub

    Private Sub btnBrowseDinf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDinf.Click
        txtbxDinf.Text = GetFilenameFromUser("ang")
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ' Paul Meems, 23-Aug-2011, Added:
        fillPath = String.Empty
        d8Path = String.Empty
        dinfSlopePath = String.Empty
        dInfPath = String.Empty
        maskedDem = String.Empty

        If txtbxFill.Text <> "" AndAlso IO.File.Exists(txtbxFill.Text) Then
            fillPath = txtbxFill.Text
        End If

        If txtbxD8Slope.Text <> "" AndAlso IO.File.Exists(txtbxD8Slope.Text) Then
            sd8Path = txtbxD8Slope.Text
        End If

        If txtbxD8.Text <> "" AndAlso IO.File.Exists(txtbxD8.Text) Then
            d8Path = txtbxD8.Text
        End If

        If txtbxDinfSlope.Text <> "" AndAlso IO.File.Exists(txtbxDinfSlope.Text) Then
            dinfSlopePath = txtbxDinfSlope.Text
        End If

        If txtbxDinf.Text <> "" AndAlso IO.File.Exists(txtbxDinf.Text) Then
            dInfPath = txtbxDinf.Text
        End If

        If txtbxMasked.Text <> "" AndAlso IO.File.Exists(txtbxMasked.Text) Then
            maskedDem = txtbxMasked.Text
        End If

        ' Paul Meems, 23-aug-2011. Commented the following lines.
        ' It is OK to not have all intermediate files. The missing files will be created and the existing ones will be skipped:
        'If fillPath = "" Or sd8Path = "" Or d8Path = "" Or (tdbChoiceList.useDinf And dinfSlopePath = "") Or (tdbChoiceList.useDinf And dInfPath = "") Then
        '    MsgBox("There must be a path selected for each of the pre-existing preprocessing intermediate grids.", MsgBoxStyle.OkOnly, "Load Intermediate Files")
        '    fillPath = ""
        '    sd8Path = ""
        '    d8Path = ""
        '    dinfSlopePath = ""
        '    dInfPath = ""
        '    txtbxFill.Text = ""
        '    txtbxD8Slope.Text = ""
        '    txtbxD8.Text = ""
        '    txtbxDinfSlope.Text = ""
        '    txtbxDinf.Text = ""
        '    Me.DialogResult = Windows.Forms.DialogResult.Cancel
        'End If
    End Sub

    Private Sub frmLoadOutputs_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible = True Then
            fillPath = ""
            sd8Path = ""
            d8Path = ""
            dInfPath = ""
            dinfSlopePath = ""
            If tdbChoiceList.useDinf Then
                txtbxDinfSlope.Enabled = True
                txtbxDinf.Enabled = True
                btnBrowseDinf.Enabled = True
                btnBrowseDinfSlope.Enabled = True
            Else
                txtbxDinfSlope.Enabled = False
                txtbxDinf.Enabled = False
                btnBrowseDinf.Enabled = False
                btnBrowseDinfSlope.Enabled = False
            End If
        End If
    End Sub

    Private Sub btnBrowseMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMask.Click
        txtbxMasked.Text = GetFilenameFromUser("_masked")
    End Sub
End Class
Imports System
Imports System.Reflection
Imports System.Resources
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports DotSpatial.Controls
Imports DotSpatial.Controls.Header
Imports DotSpatial.Plugins.ScriptRunner.My.Resources

Public Class ScriptRunnerPlugin
    Inherits Extension

    Public Overrides Sub Activate()
        App.HeaderControl.Add(New SimpleActionItem("Script Editor", New EventHandler(AddressOf btnSample_Click)) With {
                                 .SmallImage = Images.script_code_16x16,
                                 .LargeImage = Images.script_code
                                 })

        MyBase.Activate()
    End Sub

    Public Overrides Sub Deactivate()
        App.HeaderControl.RemoveAll()
    End Sub

    Private Sub btnSample_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim script As frmScript = New frmScript(App)
        script.Show()
    End Sub

End Class
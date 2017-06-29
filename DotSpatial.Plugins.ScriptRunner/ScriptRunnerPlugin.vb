Imports System
Imports System.Resources
Imports System.Windows.Forms

Imports System.Reflection
Imports System.Runtime.InteropServices

Imports DotSpatial.Controls
Imports DotSpatial.Controls.Header

Namespace MapWindowDS.ScriptRunner

    Public Class ScriptRunnerPlugin
        Inherits Extension

        Public Overrides Sub Activate()
            App.HeaderControl.Add(New SimpleActionItem("Script Editor", New EventHandler(AddressOf btnSample_Click)) With {
                                 .SmallImage = My.Resources.Images.script_code_16x16,
                                 .LargeImage = My.Resources.Images.script_code
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


End Namespace



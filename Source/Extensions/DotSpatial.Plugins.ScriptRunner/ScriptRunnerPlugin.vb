Imports DotSpatial.Controls
Imports DotSpatial.Controls.Header

Public Class ScriptRunnerPlugin
    Inherits Extension

    Public Overrides Sub Activate()
        App.HeaderControl.Add(New SimpleActionItem("Script Editor", New EventHandler(AddressOf BtnSample_Click)) With {
                                 .SmallImage = My.Resources.Resources.script_code_16x16,
                                 .LargeImage = My.Resources.Resources.script_code
                                 })

        MyBase.Activate()
    End Sub

    Public Overrides Sub Deactivate()
        App.HeaderControl.RemoveAll()
    End Sub

    Private Sub BtnSample_Click(sender As Object, e As EventArgs)
        Dim script As New FrmScript(App)
        script.Show()
    End Sub

End Class
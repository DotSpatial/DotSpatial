Option Strict On
Option Explicit On

Imports System.Globalization
Imports DotSpatial.Data

Public Class manhattanCustomGridRow
    Private grid As IRaster
    Private rowNum As Integer

    Public Sub New(ByVal AGrid As IRaster, ByVal rowNum As Integer)
        grid = AGrid
        Me.rowNum = rowNum
    End Sub

    Public Function Row(ByVal col As Integer) As Integer
        Return System.Convert.ToInt32(grid.Value(rowNum, col), CultureInfo.InvariantCulture)
    End Function
End Class
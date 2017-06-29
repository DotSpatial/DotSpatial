Option Strict On
Option Explicit On

Public Class manhattanCustomData
    Public polygons As List(Of manhattanPolygon)
    ''' <summary>
    ''' area is number of cells, since grid considered to be unit squares
    ''' </summary>
    Public area As Integer

    Public Sub New(ByVal polygons As List(Of manhattanPolygon), ByVal area As Integer)
        Me.polygons = polygons
        Me.area = area
    End Sub
End Class
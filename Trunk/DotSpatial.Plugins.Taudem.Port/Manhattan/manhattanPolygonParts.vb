Option Strict On
Option Explicit On

Imports GeoAPI.Geometries

Namespace Manhattan

    Public Class manhattanPolygonParts

        Public partindex As Integer
        Public points As New Dictionary(Of Integer, Coordinate)

    End Class
End NameSpace
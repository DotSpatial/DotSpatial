Option Strict On
Option Explicit On

Imports NetTopologySuite.Geometries

Namespace Manhattan

    Public Class ManhattanPolygonParts

        Public PartIndex As Integer
        Public Points As New Dictionary(Of Integer, Coordinate)

    End Class
End NameSpace
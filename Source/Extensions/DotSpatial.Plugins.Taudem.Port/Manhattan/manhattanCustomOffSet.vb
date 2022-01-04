Option Strict On
Option Explicit On

Imports NetTopologySuite.Geometries

Namespace Manhattan

    ''' <summary>
    ''' Stores the values from a grid header used to convert link positions to grid points
    ''' </summary>
    Public Class ManhattanCustomOffSet

        ''' <summary>
        ''' Point at top left edge of grid
        ''' </summary>
        Private ReadOnly origin As Coordinate

        ''' <summary>
        ''' E-W distance between grid points
        ''' </summary>
        Private ReadOnly dX As Double

        ''' <summary>
        ''' N-S distance between grid points
        ''' </summary>
        Private ReadOnly dY As Double

        ''' <summary>
        ''' Area of cell
        ''' </summary>
        Private ReadOnly unitArea As Double

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="origin"></param>
        ''' <param name="dX"></param>
        ''' <param name="dY"></param>
        Public Sub New(ByVal origin As Coordinate, ByVal dX As Double, ByVal dY As Double)
            Me.origin = origin
            Me.dX = dX
            Me.dY = dY
            Me.unitArea = CDbl(Me.dX * Me.dY)
        End Sub

        ''' <summary>
        ''' Generate a point from a link's start position
        ''' </summary>
        ''' <param name="l">link</param>
        ''' <returns>point</returns>
        Private Function LinkToPoint(ByVal l As manhattanCustomLink) As Coordinate
            Dim p As New Coordinate()
            Dim x, y As Double
            l.start(x, y)
            p.X = origin.X + (dX * x)
            p.Y = origin.Y - (dY * y)
            Return p
        End Function

        ''' <summary>
        ''' Converts a count c of unit boxes to an area
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns>area</returns>
        Public Function Area(ByVal c As Integer) As Double
            Return c * unitArea
        End Function

        ''' <summary>
        ''' Adds a chain of links as a part to a shape
        ''' </summary>
        ''' <param name="l">chain</param>
        ''' <param name="simplePolygon"></param>
        ''' <param name="partindex"></param>
        ''' <param name="pointindex"></param>
        ''' <returns>true iff no eror</returns>
        Private Function AddChainToShape(ByVal l As List(Of manhattanCustomLink), ByVal simplePolygon As Dictionary(Of Integer, ManhattanPolygonParts), ByRef partindex As Integer, ByRef pointindex As Integer) As Boolean
            If simplePolygon.ContainsKey(partindex) Then
                If simplePolygon(partindex).points.ContainsKey(pointindex) Then
                    Return False
                End If
            Else
                Dim lmpc As New ManhattanPolygonParts With {
                    .partindex = partindex
                }
                simplePolygon.Add(partindex, lmpc)
            End If
            partindex += 1
            manhattanPolygon.rotate(l)
            Dim l0 As manhattanCustomLink = l(0)
            Dim p0 As Coordinate = LinkToPoint(l0)

            If Not simplePolygon.ContainsKey(partindex) Then
                Dim c2 As New Coordinate(p0.X, p0.Y)
                Dim lmpc As New ManhattanPolygonParts With {
                    .partindex = partindex
                }
                simplePolygon.Add(partindex, lmpc)
                simplePolygon(partindex).points.Add(pointindex, c2)
            Else
                If simplePolygon(partindex).points.ContainsKey(pointindex) Then
                    Return False
                End If
            End If

            pointindex += 1
            Dim lastDir As manhattanCustomLink.manhattanDirection = l0.dir
            For i As Integer = 1 To l.Count - 1
                Dim nextLink As manhattanCustomLink = l(i)
                If nextLink.dir <> lastDir Then
                    ' next link has a new direction, so include its start point
                    'If (Not Shape.InsertPoint(linkToPoint(nextLink), pointindex)) Then
                    '    Return False
                    'End If
                    If Not simplePolygon(partindex).points.ContainsKey(pointindex) Then
                        Dim c4 As New Coordinate(LinkToPoint(nextLink).X, LinkToPoint(nextLink).Y)
                        simplePolygon(partindex).points.Add(pointindex, c4)
                    Else
                        If simplePolygon(partindex).points.ContainsKey(pointindex) Then
                            Return False
                        End If
                    End If
                    pointindex += 1
                End If
                lastDir = nextLink.dir
            Next i
            ' close the polygon
            If simplePolygon.ContainsKey(partindex) Then
                Dim c4 As New Coordinate(p0.X, p0.Y)
                simplePolygon(partindex).points.Add(pointindex, c4)
            Else
                If simplePolygon(partindex).points.ContainsKey(pointindex) Then
                    Return False
                End If
            End If
            'If (Shape.InsertPoint(p0, pointindex)) Then
            '    Return False
            'End If
            pointindex += 1
            Return True
        End Function

        ''' <summary>
        ''' Makes a shape from a list of polygons; each polygon becames a part of the shape.
        ''' </summary>
        ''' <param name="polygons"></param>
        ''' <returns>null if error, else shape</returns>
        Public Function MakeShape(ByVal polygons As List(Of manhattanPolygon)) As Dictionary(Of Integer, ManhattanPolygonParts)
            Dim simplePolygon As New Dictionary(Of Integer, ManhattanPolygonParts)

            Dim pointindex As Integer = 0
            Dim partindex As Integer = 0
            For i As Integer = 0 To polygons.Count - 1
                If (Not AddChainToShape(polygons(i).perimeter, simplePolygon, partindex, pointindex)) Then
                    Return Nothing
                End If
            Next i

            Return simplePolygon
        End Function
    End Class
End NameSpace
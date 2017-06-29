Option Strict On
Option Explicit On


''' <summary>
''' Bounds of a rectangle that includes the polygon
''' </summary>
Public Class manhattanCustomBounds
    ''' <summary>
    ''' Minimum horizontal coordinate of bounding rectangle
    ''' </summary>
    Public xmin As Double

    ''' <summary>
    ''' Maximum horizontal coordinate of bounding rectangle
    ''' </summary>
    Public xmax As Double

    ''' <summary>
    ''' Minimum vertical coordinate of bounding rectangle
    ''' </summary>
    Public ymin As Double

    ''' <summary>
    ''' Maximum vertical coordinate of bounding rectangle
    ''' </summary>
    Public ymax As Double

    ''' <summary>
    ''' Constructs a bounding rectangle
    ''' </summary>
    ''' <param name="xmin"></param>
    ''' <param name="xmax"></param>
    ''' <param name="ymin"></param>
    ''' <param name="ymax"></param>
    Public Sub New(ByVal xmin As Double, ByVal xmax As Double, ByVal ymin As Double, ByVal ymax As Double)
        Me.xmin = xmin
        Me.xmax = xmax
        Me.ymin = ymin
        Me.ymax = ymax
    End Sub

    ''' <summary>
    ''' Returns true if no overlap in bounds of this and b
    ''' </summary>
    ''' <param name="b"></param>
    ''' <returns></returns>
    Public Function disjoint(ByVal b As manhattanCustomBounds) As Boolean
        Return ((Me.xmin > b.xmax) OrElse (b.xmin > Me.xmax) OrElse (Me.ymin > b.ymax) OrElse (b.ymin > Me.ymax))
    End Function
End Class

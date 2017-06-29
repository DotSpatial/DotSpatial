Option Strict On
Option Explicit On

''' <summary>
''' Links are unit length directed lines, positioned in a cartesian grid
''' using integer coordinates.
''' Up and down links are positioned by their bottom ends.
''' Left and right links are positioned by their left ends.
''' This means links complement (cancel out when in sequence)
''' if they have the same position but opposite directions.
''' </summary>
Public Class manhattanCustomLink
    ''' <summary>
    ''' Directions in a cartesian grid.
    ''' </summary>
    Public Enum manhattanDirection
        ''' <summary>
        ''' up
        ''' </summary>
        up
        ''' <summary>
        ''' right
        ''' </summary>
        right
        ''' <summary>
        ''' down
        ''' </summary>
        down
        ''' <summary>
        ''' left
        ''' </summary>
        left
    End Enum

    ''' <summary>
    ''' Horizontal coordinate of end of link
    ''' </summary>
    Public x As Double
    ''' <summary>
    ''' Vertical coordinate of end of link
    ''' </summary>
    Public y As Double
    ''' <summary>
    ''' MyLink.Direction of link
    ''' </summary>
    Public dir As manhattanCustomLink.manhattanDirection

    ''' 
    ''' <summary>
    ''' Generator
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="dir"></param>
    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal dir As manhattanCustomLink.manhattanDirection)
        Me.x = x
        Me.y = y
        Me.dir = dir
    End Sub

    ''' <summary>
    ''' Returns the reverse of dir.
    ''' </summary>
    ''' <param name="dir"></param>
    ''' <returns></returns>
    Public Shared Function reverse(ByVal dir As manhattanCustomLink.manhattanDirection) As manhattanCustomLink.manhattanDirection
        Select Case dir
            Case manhattanCustomLink.manhattanDirection.up
                Return manhattanCustomLink.manhattanDirection.down
            Case manhattanCustomLink.manhattanDirection.right
                Return manhattanCustomLink.manhattanDirection.left
            Case manhattanCustomLink.manhattanDirection.down
                Return manhattanCustomLink.manhattanDirection.up
            Case manhattanCustomLink.manhattanDirection.left
                Return manhattanCustomLink.manhattanDirection.right
            Case Else
                Return manhattanCustomLink.manhattanDirection.right
        End Select
    End Function

    ''' <summary>
    ''' Returns a character indicating the direction
    ''' </summary>
    ''' <returns></returns>
    Public Function dc() As Char
        Select Case Me.dir
            Case manhattanCustomLink.manhattanDirection.up
                Return "u"c
            Case manhattanCustomLink.manhattanDirection.down
                Return "d"c
            Case manhattanCustomLink.manhattanDirection.left
                Return "l"c
            Case Else
                Return "r"c
        End Select
    End Function

    ''' <summary>
    ''' Sets (x,y) to the start point of this link.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    Public Sub start(ByRef x As Double, ByRef y As Double)
        'Public Sub start(<System.Runtime.InteropServices.Out()> ByRef x As Double, <System.Runtime.InteropServices.Out()> ByRef y As Double)
        Select Case Me.dir
            Case manhattanCustomLink.manhattanDirection.up, manhattanCustomLink.manhattanDirection.right
                x = Me.x
                y = Me.y
            Case manhattanCustomLink.manhattanDirection.down
                x = Me.x
                y = Me.y + 1 'dx
            Case Else
                x = Me.x + 1 'dx
                y = Me.y
        End Select
    End Sub

    ''' <summary>
    ''' Sets (x,y) to the finish point of this link.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    Public Sub finish(<System.Runtime.InteropServices.Out()> ByRef x As Double, <System.Runtime.InteropServices.Out()> ByRef y As Double)
        Select Case Me.dir
            Case manhattanCustomLink.manhattanDirection.up
                x = Me.x
                y = Me.y + 1
            Case manhattanCustomLink.manhattanDirection.right
                x = Me.x + 1
                y = Me.y
            Case Else
                x = Me.x
                y = Me.y
        End Select
    End Sub

    ''' <summary>
    ''' Returns true if the link l complements this link.
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns></returns>
    Public Function complements(ByVal l As manhattanCustomLink) As Boolean
        Return ((l.x = Me.x) AndAlso (l.y = Me.y) AndAlso (l.dir = reverse(Me.dir))) 'f1
    End Function

    ''' <summary>
    ''' Returns true of the finish of l1 is the same point as the start of l2.
    ''' </summary>
    ''' <param name="l1"></param>
    ''' <param name="l2"></param>
    ''' <returns></returns>
    Public Shared Function connected(ByVal l1 As manhattanCustomLink, ByVal l2 As manhattanCustomLink) As Boolean
        Dim x1, y1, x2, y2 As Double
        l1.finish(x1, y1)
        l2.start(x2, y2) 'dx, dy
        Return ((x1 = x2) AndAlso (y1 = y2))
    End Function
End Class
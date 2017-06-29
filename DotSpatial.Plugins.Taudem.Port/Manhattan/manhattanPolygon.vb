Option Strict On
Option Explicit On


Imports System.Globalization
Imports DotSpatial.Plugins.Taudem.Port.manhattanCustomLink

''' <summary>
''' A polygon is stored as a chain of links forming its perimeter, plus its bounds
''' </summary>
Public Class manhattanPolygon
    ''' <summary>
    ''' Perimeter as chain of links
    ''' </summary>
    Public perimeter As List(Of manhattanCustomLink)
    ''' <summary>
    ''' Bounding rectangle
    ''' </summary>
    Public bounds As manhattanCustomBounds

    ''' <summary>
    ''' Constructs a polygon
    ''' </summary>
    ''' <param name="perimeter"></param>
    ''' <param name="bounds"></param>
    Public Sub New(ByVal perimeter As List(Of manhattanCustomLink), ByVal bounds As manhattanCustomBounds)
        Me.perimeter = perimeter
        Me.bounds = bounds
    End Sub

    ''' <summary>
    ''' Makes a polygon forming the (clockwise) perimeter of a rectangle
    ''' with bottom left corner (x,y), 1 unit high, and width units wide
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="width"></param>
    ''' <returns></returns>
    Public Shared Function box(ByVal x As Double, ByVal y As Double, ByVal width As Integer) As manhattanPolygon
        Dim perimeter As New List(Of manhattanCustomLink)()
        perimeter.Add(New manhattanCustomLink(x, y, manhattanDirection.up))
        For i As Integer = 0 To width - 1
            perimeter.Add(New manhattanCustomLink(x + i, y + 1, manhattanDirection.right))
        Next i
        perimeter.Add(New manhattanCustomLink(x + width, y, manhattanDirection.down))
        For i As Integer = width - 1 To 0 Step -1
            perimeter.Add(New manhattanCustomLink(x + i, y, manhattanDirection.left))
        Next i
        Dim bounds As New manhattanCustomBounds(x, x + width, y, y + 1)
        Dim res As New manhattanPolygon(perimeter, bounds)
        Return res
    End Function

    ''' <summary>
    ''' Makes a single polygon from two polygons p1 and p2 with complementary
    ''' links at index i1 of perimeter of p1 and i2 of perimeter of p2.
    ''' Also removes any adjacent complementary links in the result.
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <param name="i1"></param>
    ''' <param name="p2"></param>
    ''' <param name="i2"></param>
    Public Shared Function merge(ByVal p1 As manhattanPolygon, ByVal i1 As Integer, ByVal p2 As manhattanPolygon, ByVal i2 As Integer) As manhattanPolygon
        Dim l1 As List(Of manhattanCustomLink) = p1.perimeter
        Dim l2 As List(Of manhattanCustomLink) = p2.perimeter
        Dim l As List(Of manhattanCustomLink) = l1.GetRange(0, i1)
        l.AddRange(l2.GetRange(i2 + 1, l2.Count - (i2 + 1)))
        l.AddRange(l2.GetRange(0, i2))
        l.AddRange(l1.GetRange(i1 + 1, l1.Count - (i1 + 1)))
        ' Can get complementary pairs at the joins, so we remove them.
        ' Remove pairs at the rightmost join first, so indexes at
        ' leftmost are not affected.
        ' The guards are the conditions that the rightmost and leftmost
        ' portions of l1 respectively are not empty, i.e that there were joins
        ' between portions of different lists.  They also ensure the precondition
        ' of removePairs(l,i): that i and i+1 are indexes of l.
        If i1 + 1 < l1.Count Then
            removePairs(l, i1 + l2.Count - 2)
        End If
        If i1 > 0 Then
            removePairs(l, i1 - 1)
        End If
        Dim b1 As manhattanCustomBounds = p1.bounds
        Dim b2 As manhattanCustomBounds = p2.bounds
        Dim bounds As New manhattanCustomBounds(Math.Min(b1.xmin, b2.xmin), Math.Max(b1.xmax, b2.xmax), Math.Min(b1.ymin, b2.ymin), Math.Max(b1.ymax, b2.ymax))
        Dim res As New manhattanPolygon(l, bounds)
        Return res
    End Function

    ''' <summary>
    ''' Returns true if p1 and p2 are not disjoint and have complementary links
    ''' at indexes i1 and i2 of their perimeters
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <param name="i1"></param>
    ''' <param name="p2"></param>
    ''' <param name="i2"></param>
    ''' <returns></returns>
    Public Shared Function canMerge(ByVal p1 As manhattanPolygon, <System.Runtime.InteropServices.Out()> ByRef i1 As Integer, ByVal p2 As manhattanPolygon, <System.Runtime.InteropServices.Out()> ByRef i2 As Integer) As Boolean
        If p1.bounds.disjoint(p2.bounds) Then
            i1 = -1 ' for compiler
            i2 = -1 ' for compiler
            Return False
        End If
        Dim l1 As List(Of manhattanCustomLink) = p1.perimeter
        Dim l2 As List(Of manhattanCustomLink) = p2.perimeter
        For i1 = 0 To l1.Count - 1
            Dim link As manhattanCustomLink = l1(i1)
            Dim dir As manhattanDirection = link.dir
            'todo: maybe change this
            ' only horizontal links need be considered
            If (dir = manhattanDirection.left) OrElse (dir = manhattanDirection.right) Then ' OrElse (dir = Direction.up) OrElse (dir = Direction.down)
                i2 = l2.FindIndex(New PredicateWrapper(Of manhattanCustomLink, manhattanCustomLink)(link, AddressOf MYLinkMatch))
                If i2 >= 0 Then
                    Return True
                End If
            End If
        Next i1
        i2 = -1 ' for compiler
        Return False
    End Function

    Private Shared Function MYLinkMatch(ByVal item As manhattanCustomLink, ByVal argument As manhattanCustomLink) As Boolean
        Return item.complements(argument) '(item.dir = reverse(argument.dir)) AndAlso ((item.x = argument.x) AndAlso (item.y = argument.y))
    End Function

    ''' <summary>
    ''' If links at indexes i and i+1 are complementary, removes them.
    ''' Recurses on links originally at i-1 and i+2 if i &gt; 0 and i &lt; l.Count - 2
    ''' Precondition: 0 &lt;= i &lt; l.Count - 1, ie i and i+1 are indexes of l
    ''' </summary>
    ''' <param name="l"></param>
    ''' <param name="i"></param>
    Public Shared Sub removePairs(ByVal l As List(Of manhattanCustomLink), ByVal i As Integer)
        If l(i).complements(l(i + 1)) Then
            l.RemoveRange(i, 2)
            If (i > 0) AndAlso (i < l.Count) Then
                removePairs(l, i - 1)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Removes the first and last links if they complement, and repeats.
    ''' Precondition: l.Count &gt; 1
    ''' </summary>
    ''' <param name="l"></param>
    Public Shared Sub removeFirstLast(ByVal l As List(Of manhattanCustomLink))
        Dim last As Integer = l.Count - 1
        If l(0).complements(l(last)) Then
            l.RemoveAt(last)
            l.RemoveAt(0)
            If l.Count > 1 Then
                removeFirstLast(l)
            End If
        End If
    End Sub

    ''' <summary>
    ''' If a chain of links representing a closed polygon
    ''' has the first and last links the same direction,
    ''' this function puts the head of the chain to the back, and repeats.
    ''' This does not affect the polygon, but will reduce by one
    ''' the number of points representing it.
    ''' </summary>
    ''' <param name="l">Chain of links representing a closed polygon</param>
    Public Shared Sub rotate(ByVal l As List(Of manhattanCustomLink))
        Dim head As manhattanCustomLink = l(0)
        If head.dir = l(l.Count - 1).dir Then
            l.RemoveAt(0)
            l.Add(head)
            rotate(l)
        End If
    End Sub

    ''' <summary>
    ''' List l has a link at first complemented by a link at last,
    ''' where first less than last.  This removes first to last inclusive
    ''' and makes a hole which is the list from first+1 to last-1 inclusive.
    ''' If the complementary links are adjacent the hole will be null.
    ''' Precondition: 0 &lt;= first &lt; last &lt;= l.Count - 1
    ''' </summary>
    ''' <param name="l"></param>
    ''' <param name="first"></param>
    ''' <param name="last"></param>
    ''' <param name="hole"></param>
    Public Shared Sub makeHole(ByVal l As List(Of manhattanCustomLink), ByVal first As Integer, ByVal last As Integer, <System.Runtime.InteropServices.Out()> ByRef hole As List(Of manhattanCustomLink))
        If first + 1 < last Then
            hole = l.GetRange(first + 1, last - (first + 1))
            'holes often have complementary initial and final links
            removeFirstLast(hole)
        Else
            hole = Nothing
        End If
        l.RemoveRange(first, last + 1 - first)
    End Sub

    ''' <summary>
    ''' Returns true if it finds a link complemented by a later one.
    ''' Then first is set to the index of the first link, and last to
    ''' the index of the complementing one.
    ''' </summary>
    ''' <param name="l"></param>
    ''' <param name="first"></param>
    ''' <param name="last"></param>
    ''' <returns></returns>
    Public Shared Function hasHole(ByVal l As List(Of manhattanCustomLink), <System.Runtime.InteropServices.Out()> ByRef first As Integer, <System.Runtime.InteropServices.Out()> ByRef last As Integer) As Boolean
        For first = 0 To l.Count - 2
            Dim link As manhattanCustomLink = l(first)
            Dim dir As manhattanDirection = link.dir
            ' only horizontal links need be considered
            If (dir = manhattanDirection.left) OrElse (dir = manhattanDirection.right) Then
                last = l.FindIndex(first + 1, New PredicateWrapper(Of manhattanCustomLink, manhattanCustomLink)(link, AddressOf MYLinkMatch))
                If last >= 0 Then
                    Return True
                End If

            End If
        Next first
        last = -1 ' for compiler
        Return False
    End Function

    ''' <summary>
    ''' Generate a string for display of a polygon l, in the the form of
    ''' a start point plus a string of direction letters.
    ''' This function is intended for debugging.  It also checks the 
    ''' polygon is connected and closed.
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns></returns>
    Public Shared Function makeString(ByVal l As List(Of manhattanCustomLink)) As String
        If l.Count < 4 Then
            Return "Length is " & l.Count.ToString(CultureInfo.CurrentUICulture)
        End If
        Dim current As manhattanCustomLink = l(0)
        Dim [next] As manhattanCustomLink
        Dim x, y As Double
        current.start(x, y)
        Dim res As String = "(" & x.ToString(CultureInfo.CurrentUICulture) & "," & y.ToString(CultureInfo.CurrentUICulture) & ") " & current.dc()
        For i As Integer = 1 To l.Count - 1
            [next] = l(i)
            If manhattanCustomLink.connected(current, [next]) Then
                current = [next]
                res &= current.dc()
            Else
                res &= " (" & current.x.ToString(CultureInfo.CurrentUICulture) & "," & current.y.ToString(CultureInfo.CurrentUICulture) & "," & current.dc() & ") not connected to (" & [next].x.ToString(CultureInfo.CurrentUICulture) & "," & [next].y.ToString(CultureInfo.CurrentUICulture) & "," & [next].dc() & ")"
                Return res
            End If
        Next i
        [next] = l(0)
        If (Not manhattanCustomLink.connected(current, [next])) Then
            res &= " (" & current.x.ToString(CultureInfo.CurrentUICulture) & "," & current.y.ToString(CultureInfo.CurrentUICulture) & "," & current.dc() & ") not connected to start (" & [next].x.ToString(CultureInfo.CurrentUICulture) & "," & [next].y.ToString(CultureInfo.CurrentUICulture) & "," & [next].dc() & ")"
        End If
        Return res
    End Function

    Private Shared Sub predLinkComp(Optional ByVal p1 As Object = Nothing, Optional ByVal p2 As Object = Nothing)
        Throw New NotImplementedException
    End Sub

End Class

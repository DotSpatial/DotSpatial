Option Strict On
Option Explicit On

Imports Microsoft.VisualBasic

Imports System
Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Globalization
Imports DotSpatial.Data
Imports DotSpatial.Topology

''' <summary>
''' <para>ManhattanShapes are polygons with only vertical or horizontal lines in their perimeters.
''' Lists of ManhattanShapes are the parts of a shape.
''' Each collection of parts is associated with a particular integer index,
''' representing a particular grid value.</para>
''' <para>The algorithm to make the polygons from a grid of cells,
''' each polygon indexed by the grid value it belongs to is: </para>
''' <para>1. Make the basic (horizontal) boxes for each index.  
''' These boxes are unit height and integer width, and located by row and column number,
''' so that the next two steps only require integer arithmetic.</para>
''' <para>2. Merge the boxes for each index</para>
''' <para>3. Make the holes for each index</para>
''' <para>4. Convert the polygons into lists of points, the format for a shape in a shapefile</para>
''' </summary>
Public Class ManhattanShapes
    Private gridFilePath As String
    Private ShapesTable As Dictionary(Of Integer, manhattanCustomData)
    ' ''' <summary>
    ' ''' Constructor, making an empty dictionary
    ' ''' </summary>
    Public Sub New(ByVal GridFileName As String)
        gridFilePath = GridFileName
        ShapesTable = New Dictionary(Of Integer, manhattanCustomData)()
    End Sub

    ''' <summary>
    ''' Adds a part p with index indx.
    ''' </summary>
    ''' <param name="indx"></param>
    ''' <param name="p"></param>
    ''' <param name="area"></param>
    Private Sub addShape(ByVal indx As Integer, ByVal p As manhattanPolygon, ByVal area As Integer)
        If ShapesTable.ContainsKey(indx) Then
            ShapesTable(indx).polygons.Add(p)
            ShapesTable(indx).area += area
        Else
            Dim lp As New List(Of manhattanPolygon)()
            lp.Add(p)
            ShapesTable.Add(indx, New manhattanCustomData(lp, area))
        End If
    End Sub

    ''' <summary>
    ''' For each index in the dictionary, merges its parts if possible.
    ''' </summary>
    Private Sub mergeShapes()
        Dim keys As New List(Of Integer)()
        keys.AddRange(ShapesTable.Keys)
        For Each i As Integer In keys
            ShapesTable(i).polygons = mergeMyPolygons(ShapesTable(i).polygons)
        Next i
    End Sub

    ''' <summary>
    ''' Merges the polygons in a list todo of polygons.
    ''' Two polygons can be merged if they are not disjoint and contain complementary links.
    ''' </summary>
    ''' <param name="todo"></param>
    ''' <returns></returns>
    Private Shared Function mergeMyPolygons(ByVal todo As List(Of manhattanPolygon)) As List(Of manhattanPolygon)
        Dim done As New List(Of manhattanPolygon)()
        Do While todo.Count > 0
            Dim p0 As manhattanPolygon = todo(0)
            todo.RemoveAt(0)
            Dim i0 As Integer
            Dim i1 As Integer
            Dim changed As Boolean = False
            Dim count As Integer = todo.Count
            For i As Integer = 0 To count - 1
                If manhattanPolygon.canMerge(p0, i0, todo(i), i1) Then
                    Dim p As manhattanPolygon = manhattanPolygon.merge(p0, i0, todo(i), i1)
                    todo.RemoveAt(i)
                    todo.Add(p)
                    changed = True
                    Exit For
                End If
            Next i
            If (Not changed) Then
                done.Add(p0)
            End If
        Loop
        Return done
    End Function

    ''' <summary>
    ''' For each index in the dictionary, separates out any holes.
    ''' </summary>
    Private Sub makeHoles()
        Dim keys As New List(Of Integer)()
        keys.AddRange(ShapesTable.Keys)
        For Each i As Integer In keys
            ShapesTable(i).polygons = makeHoles(New List(Of manhattanPolygon)(), ShapesTable(i).polygons)
        Next i
    End Sub

    ''' <summary>
    ''' Separates out the holes in a list of polygons.
    ''' There may be more than one hole in a polygon, and holes may be split into
    ''' several holes.  A polygon contains a hole if it contains two non-adjacent
    ''' complementary links.
    ''' </summary>
    ''' <param name="done">polygons with no holes</param>
    ''' <param name="todo">polygons which may have holes</param>
    ''' <returns></returns>
    Private Shared Function makeHoles(ByVal done As List(Of manhattanPolygon), ByVal todo As List(Of manhattanPolygon)) As List(Of manhattanPolygon)
        Do While todo.Count > 0
            Dim first, last As Integer
            Dim [next] As manhattanPolygon = todo(0)
            If manhattanPolygon.hasHole([next].perimeter, first, last) Then
                'Debug.WriteLine(Link.makeString(next));
                'Debug.WriteLine("has hole from " + first.ToString() + " to " + last.ToString());
                Dim hole As List(Of manhattanCustomLink) = Nothing
                manhattanPolygon.makeHole([next].perimeter, first, last, hole)
                If hole IsNot Nothing Then
                    ' Holes are never merged, so bounds are of no interest
                    Dim bounds As New manhattanCustomBounds(0, 0, 0, 0)
                    Dim p As New manhattanPolygon(hole, bounds)
                    todo.Add(p)
                End If
                If [next].perimeter.Count = 0 Then
                    ' degenerate case: all of next was a hole; the empty polygon can be removed
                    todo.RemoveAt(0)
                End If
            Else
                done.Add([next])
                todo.RemoveAt(0)
            End If
        Loop
        Return done
    End Function

    ''' <summary>
    ''' Finish by merging shapes and making holes
    ''' </summary>
    Public Sub FinishShapes()
        mergeShapes()
        makeHoles()
    End Sub

    ''' <summary>
    ''' Generate a string for all the polygons.  For each grid value:
    ''' 1.  A line stating its value
    ''' 2.  A set of lines, one for each polygon for that value.
    ''' </summary>
    ''' <returns></returns>
    Public Function makeString() As String
        Dim res As String = ""
        For Each kvp As KeyValuePair(Of Integer, manhattanCustomData) In ShapesTable
            Dim indx As Integer = kvp.Key
            Dim lp As List(Of manhattanPolygon) = kvp.Value.polygons
            res &= "Index " & indx.ToString(CultureInfo.CurrentUICulture) + ControlChars.Lf
            For i As Integer = 0 To lp.Count - 1
                res &= manhattanPolygon.makeString(lp(i).perimeter)
                res &= ControlChars.Lf
            Next i
        Next kvp
        Return res
    End Function

    'Private Delegate Function Row(ByVal index As Integer) As Integer

    'Private Class ArrayRow
    '    Private row_Renamed() As Integer

    '    Public Sub New(ByVal row() As Integer)
    '        Me.row_Renamed = row
    '    End Sub

    '    Public Function Row(ByVal index As Integer) As Integer
    '        Return row_Renamed(index)
    '    End Function
    'End Class


    ' ''' <summary>
    ' ''' Adds boxes from array row number rowNum
    ' ''' </summary>
    ' ''' <param name="row"></param>
    ' ''' <param name="rowNum"></param>
    ' ''' <param name="noData"></param>
    'Public Sub addArrayRow(ByVal row() As Integer, ByVal rowNum As Integer, ByVal noData As Double, ByVal valueToUse As Double)
    '    Dim ar As New ArrayRow(row)
    '    addRow(New Row(AddressOf ar.Row), rowNum, row.Length, noData, valueToUse)
    'End Sub

    ' ''' <summary>
    ' ''' Adds boxes from grid row number rowNum of length length
    ' ''' </summary>
    ' ''' <param name="rowNum"></param>
    ' ''' <param name="length"></param>
    'Public Sub addMyGridRow(ByVal rowNum As Integer, ByVal length As Integer, ByVal NoDataValue As Double, ByVal valueToUse As Double)
    '    Dim gr As New manhattanCustomGridRow(grid, rowNum)
    '    addRow(New Row(AddressOf gr.Row), rowNum, length, NoDataValue, valueToUse)
    'End Sub

    ' ''' <summary>
    ' ''' row behaves like an array, indexed from 0 to length - 1
    ' ''' This creates boxes, where boxes are made from adjacent cells
    ' ''' of the array with the same values, and adds them as parts.
    ' ''' Nodata values are ignored.
    ' ''' </summary>
    ' ''' <param name="row"></param>
    ' ''' <param name="rowNum"></param>
    ' ''' <param name="length"></param>
    ' ''' <param name="noData"></param>
    'Private Sub addRow(ByVal row As Row, ByVal rowNum As Integer, ByVal length As Integer, ByVal noData As Double, ByVal valueToUse As Double)
    '    Dim col As Integer = 0
    '    Dim width As Integer = 1
    '    Dim last As Integer = row(0)
    '    Dim bound As Integer = length - 1
    '    Do While col < bound
    '        Dim [next] As Integer = row(col + 1)
    '        If [next] = last Then
    '            width += 1
    '        Else
    '            If last <> noData AndAlso last = valueToUse Then
    '                Me.addShape(last, manhattanPolygon.box(col + 1 - width, rowNum, width), width)
    '            End If
    '            last = [next]
    '            width = 1
    '        End If
    '        col += 1
    '    Loop
    '    If last <> noData AndAlso last = valueToUse Then
    '        Me.addShape(last, manhattanPolygon.box(col + 1 - width, rowNum, width), width)
    '    End If
    'End Sub

    'Private Sub addRow(ByVal row As Row, ByVal rowNum As Integer, ByVal length As Integer, ByVal noData As Double)
    '    Dim width As Integer = 1
    '    Dim last As Integer = row(0)
    '    Dim bound As Integer = length - 1
    '    Dim col As Integer
    '    For col = 0 To bound
    '        Dim NextValue As Integer = row(col + 1)
    '        If NextValue = last Then
    '            width += 1
    '        Else
    '            If last <> noData Then
    '                Me.addShape(last, manhattanPolygon.box(col + 1 - width, rowNum, width), width)
    '            End If
    '            last = NextValue
    '            width = 1
    '        End If
    '        col += 1
    '    Next
    '    If last <> noData Then
    '        Me.addShape(last, manhattanPolygon.box(col + 1 - width, rowNum, width), width)
    '    End If
    'End Sub


    ''' <summary>
    ''' Converts grid to a shapefile, replacing any existing shapefile.
    ''' Assumed to be an integer grid.
    ''' Attribute "BasinID" is populated with the grid values
    ''' Attribute "AREA" is populated with the area of each polygon.
    ''' </summary>
    ''' <returns>null if any errors, else FeatureSet</returns>
    Public Function GridToShapeManhattan() As FeatureSet
        Return GridToShapeManhattan("BasinID", "AREA")
    End Function

    ''' <summary>
    ''' Converts grid to a shapefile, replacing any existing shapefile.
    ''' Assumed to be an integer grid.
    ''' Attribute GridValueFieldName is populated with the grid values
    ''' Attribute "AREA" is populated with the area of each polygon.
    ''' </summary>
    ''' <returns>null if any errors, else FeatureSet</returns>
    Public Function GridToShapeManhattan(ByVal GridValueFieldName As String, ByVal AreaFieldName As String) As FeatureSet
        Dim grid As IRaster = Raster.OpenFile(gridFilePath)

        '// origin (taken as column zero and row zero) is at the top left of the grid
        Dim startC As Coordinate = grid.CellToProj(0, 0)
        startC.X = startC.X - (grid.CellWidth / 2)
        startC.Y = startC.Y + (grid.CellHeight / 2)
        Dim MainOffset As New manhattanCustomOffSet(startC, grid.CellWidth, grid.CellHeight)

        Dim NoDataValue As Double = grid.NoDataValue
        Dim numRows As Integer = grid.EndRow
        Dim numCols As Integer = grid.EndColumn - 1

        Dim width As Integer
        Dim GridValue As Integer
        Dim Col As Integer
        For Row As Integer = 0 To numRows - 1
            GridValue = System.Convert.ToInt32(grid.Value(Row, 0), CultureInfo.InvariantCulture)
            width = 1
            For Col = 1 To numCols
                Dim NextGridValue As Integer = System.Convert.ToInt32(grid.Value(Row, Col), CultureInfo.InvariantCulture)
                If NextGridValue = GridValue Then
                    width += 1
                Else
                    If GridValue <> NoDataValue Then
                        Me.addShape(GridValue, manhattanPolygon.box(Col - width, Row, width), width)
                    End If
                    GridValue = NextGridValue
                    width = 1
                End If
            Next Col
            If GridValue <> NoDataValue Then
                Me.addShape(GridValue, manhattanPolygon.box(Col - width, Row, width), width)
            End If
        Next Row

        Me.FinishShapes()
        Dim PolygonFeatureSet As New FeatureSet(FeatureType.Polygon)
        PolygonFeatureSet.Projection = grid.Projection
        grid.Close()

        addFieldToFS(PolygonFeatureSet, GridValueFieldName, GetType(Integer))
        addFieldToFS(PolygonFeatureSet, AreaFieldName, GetType(Double))

        For Each GridValue In Me.ShapesTable.Keys
            Dim simplePolygon As Dictionary(Of Integer, manhattanPolygonParts) = MainOffset.makeShape(ShapesTable(GridValue).polygons)
            If simplePolygon Is Nothing Then
                Continue For
            Else
                For Each s As KeyValuePair(Of Integer, manhattanPolygonParts) In simplePolygon
                    Dim COORDS As New List(Of Coordinate)
                    For Each S1 As KeyValuePair(Of Integer, Coordinate) In s.Value.points
                        COORDS.Add(S1.Value)
                    Next
                    If COORDS.Count > 0 Then
                        Dim newPolygon As New Polygon(New LinearRing(COORDS))
                        Dim newFeature As New Feature(newPolygon)
                        Dim addedFEAT As IFeature = PolygonFeatureSet.AddFeature(newFeature)
                        addedFEAT.DataRow(0) = GridValue
                        addedFEAT.DataRow(1) = newFeature.Area
                        addedFEAT.DataRow.AcceptChanges()
                    End If
                Next
            End If
        Next GridValue
        Return PolygonFeatureSet
    End Function

    Public Shared Sub addFieldToFS(ByVal polygonSF As FeatureSet, ByVal fieldName As String, ByVal dataType As System.Type)
        Dim exists As Boolean = False
        For Each c As System.Data.DataColumn In polygonSF.DataTable.Columns
            If c.ColumnName.ToLower = fieldName.ToLower Then
                exists = True
            End If
        Next
        If Not exists Then
            Dim newCol As New System.Data.DataColumn(fieldName, dataType)
            polygonSF.DataTable.Columns.Add(newCol)
            polygonSF.DataTable.AcceptChanges()
        End If
    End Sub

End Class

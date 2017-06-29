'********************************************************************************************************
' FILENAME:      tdbChoices.vb
' DESCRIPTION:  A class to hold and manipulate options associated
'   with the taudemBasins functionality
' NOTES: This form is called from mwTauDemBASINSWrap.vb
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
'you may not use this file except in compliance with the License. You may obtain a copy of the License at
'http://www.mozilla.org/MPL/
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and
'limitations under the License.
'
'
'Last Update:   10/18/05, ARA
' Change Log:
' Date          Changed By      Notes
'10/18/05       ARA             Added mozilla comments
'01/18/06       ARA             Defaulted maskthresh to 0 to fix stupid bug
'February 11    CWG             numProcesses added
'********************************************************************************************************

Imports System.Xml

Public Class tdbChoices_v3
    Public useBurnIn As Boolean
    Public usefdrfile As Boolean
    Public useOutlets As Boolean
    Public EdgeContCheck As Boolean
    Public useExtentMask As Boolean
    Public useMaskFileOrExtents As Boolean
    Public useNetworkOutlets As Boolean
    Public useDinf As Boolean
    
    Public numProcesses As Integer
    
    Public ShowTaudemOutput As Boolean

    Public AddPitfillLayer As Boolean
    Public AddD8Layer As Boolean
    Public AddDinfLayer As Boolean
    Public AddD8AreaLayer As Boolean
    Public AddDinfAreaLayer As Boolean
    Public AddGridNetLayer As Boolean
    Public AddRiverRasterLayer As Boolean
    Public AddOrderGridLayer As Boolean
    Public AddWShedGridLayer As Boolean
    Public AddStreamShapeLayer As Boolean
    Public AddWShedShapeLayer As Boolean
    Public AddMergedWShedShapeLayer As Boolean

    Public OutputPath As String

    Public CalcSpecialStreamFields As Boolean
    Public CalcSpecialWshedFields As Boolean
    Public calcSpecialMergeWshedFields As Boolean

    Public Threshold As Single
    Public snapThresh As Double

    Public useScaMask As Boolean
    Public Order As Integer
    Public useMask As Boolean
    Public MaskThreshold As Single
    Public DiskBased As Boolean
    Public UseWeightsFile As Boolean
    Public useTrace As Boolean
    Public ConfigFileName As String

    Public FillGridPath As String
    Public D8SlopePath As String
    Public D8Path As String
    Public DInfSlopePath As String
    Public DInfPath As String
    Public Ad8Path As String
    Public ScaPath As String
    Public GordPath As String
    Public PlenPath As String
    Public TlenPath As String
    Public SrcPath As String
    Public OrdPath As String
    Public CoordPath As String
    Public TreePath As String
    Public NetPath As String
    Public WPath As String
    Public MaskedDem As String

    Private m_Doc As New XmlDocument

    Public Sub SetDefaultTDchoices()
        useOutlets = False
        useBurnIn = False
        usefdrfile = False
        useMaskFileOrExtents = False
        useMask = False
        useExtentMask = True
        useNetworkOutlets = True
        EdgeContCheck = False
        Threshold = 100
        snapThresh = 300
        useDinf = False
        
        numProcesses = 8
        
        ShowTaudemOutput = False

        AddPitfillLayer = False
        AddD8Layer = False
        AddDinfLayer = False
        AddD8AreaLayer = False
        AddDinfAreaLayer = False
        AddGridNetLayer = False
        AddRiverRasterLayer = False
        AddOrderGridLayer = False
        AddWShedGridLayer = False
        AddStreamShapeLayer = True
        AddWShedShapeLayer = True
        AddMergedWShedShapeLayer = True

        CalcSpecialStreamFields = True
        CalcSpecialWshedFields = True
        calcSpecialMergeWshedFields = True

        useScaMask = True
        Order = 1
        useMask = False
        MaskThreshold = 0
        DiskBased = False
        UseWeightsFile = False
        useTrace = False

        FillGridPath = ""
        D8SlopePath = ""
        D8Path = ""
        DInfPath = ""
        DInfSlopePath = ""
        Ad8Path = ""
        ScaPath = ""
        GordPath = ""
        PlenPath = ""
        TlenPath = ""
        SrcPath = ""
        OrdPath = ""
        CoordPath = ""
        TreePath = ""
        NetPath = ""
        WPath = ""

        ConfigFileName = ""
    End Sub

#Region "Save Config"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: SaveConfig
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will save the AWD config file
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 01/23/2006    ARA             added header
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function SaveConfig() As Boolean
        Dim Root As XmlElement
        Dim Path As String
        Dim Reader As System.IO.StreamReader
        Dim Ver As String
        Dim tStr As String

        Try
            Ver = 1.0 'App.VersionString()
            Path = System.IO.Path.GetTempFileName()
            Reader = New System.IO.StreamReader(Path)
            tStr = Reader.ReadToEnd()
            m_Doc.LoadXml("<taudem_options type='configurationfile' version='" + Ver + "'>" + tStr + "</taudem_options>")
            Root = m_Doc.DocumentElement

            'Add the ChoiceInfo
            AddChoiceInfo(Root)

            'close
            Reader.Close()
            m_Doc.Save(ConfigFileName)
            System.IO.File.Delete(Path)

            Return True
        Catch ex As System.Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: AddChoiceInfo
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will save the AWD config file information to a given parent
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 01/23/2006    ARA             removed dinf saves and added header
    ' 01/24/2006    ARA             Added misc option max_grid_file_size
    ' 03/09/2006    ARA             Added fdr burn/canyon burn distinction
    ' 30/3/11       CWG             numProcesses added
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub AddChoiceInfo(ByVal Parent As XmlElement)
        Dim xmlOptionNode As XmlNode

        Dim mainOptionsXML As XmlElement = m_Doc.CreateElement("Main_Options")
        Dim useBurn As XmlAttribute = m_Doc.CreateAttribute("Use_BurnInStream_File")
        Dim useMsk As XmlAttribute = m_Doc.CreateAttribute("Use_Mask")
        Dim useMskExtents As XmlAttribute = m_Doc.CreateAttribute("Use_ExtentsMask")
        Dim useNetOutlets As XmlAttribute = m_Doc.CreateAttribute("Use_NetworkOutlets")
        Dim useOutlet As XmlAttribute = m_Doc.CreateAttribute("Use_Outlets_File")
        Dim useDinfin As XmlAttribute = m_Doc.CreateAttribute("Use_Dinfinity")
        Dim edgecont As XmlAttribute = m_Doc.CreateAttribute("Use_Edge_Contamination_Check")

        Dim layerOptionsXML As XmlElement = m_Doc.CreateElement("Layer_Options")
        Dim Pit As XmlAttribute = m_Doc.CreateAttribute("Add_Pit_Fill_Layer")
        Dim D8 As XmlAttribute = m_Doc.CreateAttribute("Add_D8_Layer")
        Dim D8Area As XmlAttribute = m_Doc.CreateAttribute("Add_D8_Area_Layer")
        Dim DInf As XmlAttribute = m_Doc.CreateAttribute("Add_DInf_Layer")
        Dim DInfArea As XmlAttribute = m_Doc.CreateAttribute("Add_DInf_Area_Layer")
        Dim GridNet As XmlAttribute = m_Doc.CreateAttribute("Add_Grid_Network_Layer")
        Dim River As XmlAttribute = m_Doc.CreateAttribute("Add_River_Raster_Layer")
        Dim Order As XmlAttribute = m_Doc.CreateAttribute("Add_Order_Raster_Layer")
        Dim WshedGrid As XmlAttribute = m_Doc.CreateAttribute("Add_Watershed_Grid_Layer")
        Dim StreamShape As XmlAttribute = m_Doc.CreateAttribute("Add_Stream_Shapefile_Layer")
        Dim WshedShape As XmlAttribute = m_Doc.CreateAttribute("Add_Watershed_Shapefile_Layer")
        Dim MergeWshedShape As XmlAttribute = m_Doc.CreateAttribute("Add_Merged_Watershed_Shapefile_Layer")

        Dim shapeFieldCalcOptionsXML As XmlElement = m_Doc.CreateElement("Calculate_Special_Shapefile_Field_Options")
        Dim calcStream As XmlAttribute = m_Doc.CreateAttribute("Calculate_Special_Stream_Fields")
        Dim calcWshed As XmlAttribute = m_Doc.CreateAttribute("Calculate_Special_Watershed_Fields")
        Dim calcMergeShed As XmlAttribute = m_Doc.CreateAttribute("Calculate_Special_Merge_Watershed_Fields")

        Dim miscOptionsXML As XmlElement = m_Doc.CreateElement("Misc_Options")
        Dim maxFileSize As XmlAttribute = m_Doc.CreateAttribute("Max_Grid_File_Size")
        Dim relOutputPath As XmlAttribute = m_Doc.CreateAttribute("Relative_Output_Path")
        Dim numProc As XmlAttribute = m_Doc.CreateAttribute("Maximum_Process_Count")
        Dim showOutput As XmlAttribute = m_Doc.CreateAttribute("Show_TauDEM_output")

        'Set the attributes
        useBurn.InnerText = useBurnIn.ToString()
        useOutlet.InnerText = useOutlets.ToString()
        useNetOutlets.InnerText = useNetworkOutlets.ToString()
        useMsk.InnerText = useMaskFileOrExtents.ToString()
        useMskExtents.InnerText = useExtentMask.ToString()
        useDinfin.InnerText = useDinf.ToString()
        edgecont.InnerText = EdgeContCheck.ToString()

        Pit.InnerText = AddPitfillLayer.ToString()
        D8.InnerText = AddD8Layer.ToString()
        D8Area.InnerText = AddD8AreaLayer.ToString()
        DInf.InnerText = AddDinfLayer.ToString()
        DInfArea.InnerText = AddDinfLayer.ToString()
        GridNet.InnerText = AddGridNetLayer.ToString()
        River.InnerText = AddRiverRasterLayer.ToString()
        Order.InnerText = AddOrderGridLayer.ToString()
        WshedGrid.InnerText = AddWShedGridLayer.ToString()
        StreamShape.InnerText = AddStreamShapeLayer.ToString()
        WshedShape.InnerText = AddWShedShapeLayer.ToString()
        MergeWshedShape.InnerText = AddMergedWShedShapeLayer.ToString()

        calcStream.InnerText = CalcSpecialStreamFields.ToString()
        calcWshed.InnerText = CalcSpecialWshedFields.ToString()
        calcMergeShed.InnerText = calcSpecialMergeWshedFields.ToString()

        maxFileSize.InnerText = g_MaxFileSize.ToString()
        relOutputPath.InnerText = OutputPath
        numProc.InnerText = numProcesses.ToString()
        showOutput.InnerText = ShowTaudemOutput.ToString()

        'Assign by group
        With mainOptionsXML
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(useBurn)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(useMsk)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(useMskextents)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(useDinfin)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(edgecont)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(useOutlet)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(useNetOutlets)
            End With
        End With
        Parent.AppendChild(mainOptionsXML)

        With layerOptionsXML
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(Pit)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(D8)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(D8Area)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(DInf)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(DInfArea)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(GridNet)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(River)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(Order)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(WshedGrid)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(StreamShape)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(WshedShape)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(MergeWshedShape)
            End With
        End With
        Parent.AppendChild(layerOptionsXML)

        With shapeFieldCalcOptionsXML
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(calcStream)
            End With

            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(calcWshed)
            End With

            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(calcMergeShed)
            End With
        End With
        Parent.AppendChild(shapeFieldCalcOptionsXML)

        With miscOptionsXML
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(maxFileSize)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
            	.Append(relOutputPath)
            End With
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
            	.Append(numProc)
            End With

            ' Paul Meems, 10-Aug-11: Save Show Output as well
            xmlOptionNode = m_Doc.CreateNode(XmlNodeType.Element, "Option", "")
            With .AppendChild(xmlOptionNode).Attributes
                .Append(showOutput)
            End With
        End With
        Parent.AppendChild(miscOptionsXML)
    End Sub
#End Region


#Region "Load Config"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: LoadConfig
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will load the AWD config file
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log:
    ' Date          Changed By      Notes
    ' 01/23/2006    ARA             removed dinf messages and added header
    ' 01/24/2006    ARA             Updated for mix option Max_Grid_File_Size
    ' 01/27/2006    ARA             Creates default Basins config if not found
    ' 03/09/2006    ARA             Added canyon/fdr burn distinction
    ' 30/3/11       CWG             numProcesses added
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub LoadConfig()
        Dim Doc As New XmlDocument
        Dim Root As XmlElement
        Dim currNodes As XmlNodeList

        Try
            If System.IO.File.Exists(ConfigFileName) Then
                Doc = New XmlDocument
                Doc.Load(ConfigFileName)
                Root = Doc.DocumentElement

                With Root.Item("Main_Options")
                    currNodes = .SelectNodes("Option")
                    If Not currNodes Is Nothing And currNodes.Count > 0 Then
                        For i As Integer = 0 To currNodes.Count - 1
                            Select Case currNodes(i).Attributes(0).Name
                                Case "Use_BurnInStream_File"
                                    useBurnIn = currNodes(i).Attributes(0).InnerText

                                Case "Use_Mask"
                                    useMaskFileOrExtents = currNodes(i).Attributes(0).InnerText

                                Case "Use_ExtentsMask"
                                    useExtentMask = currNodes(i).Attributes(0).InnerText

                                Case "Use_Dinfinity"
                                    useDinf = currNodes(i).Attributes(0).InnerText

                                Case "Use_Edge_Contamination_Check"
                                    EdgeContCheck = currNodes(i).Attributes(0).InnerText

                                Case "Use_Outlets_File"
                                    useOutlets = currNodes(i).Attributes(0).InnerText

                                Case "Use_NetworkOutlets"
                                    useNetworkOutlets = currNodes(i).Attributes(0).InnerText
                            End Select
                        Next
                    End If
 

                End With

                With Root.Item("Layer_Options")
                    currNodes = .SelectNodes("Option")
                    If Not currNodes Is Nothing And currNodes.Count > 0 Then
                        For i As Integer = 0 To currNodes.Count - 1
                            Select Case currNodes(i).Attributes(0).Name
                                Case "Add_Pit_Fill_Layer"
                                    AddPitfillLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_D8_Layer"
                                    AddD8Layer = currNodes(i).Attributes(0).InnerText

                                Case "Add_D8_Area_Layer"
                                    AddD8AreaLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_DInf_Layer"
                                    AddDinfLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_DInf_Area_Layer"
                                    AddDinfAreaLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_Grid_Network_Layer"
                                    AddGridNetLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_River_Raster_Layer"
                                    AddRiverRasterLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_Order_Raster_Layer"
                                    AddOrderGridLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_Watershed_Grid_Layer"
                                    AddWShedGridLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_Stream_Shapefile_Layer"
                                    AddStreamShapeLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_Watershed_Shapefile_Layer"
                                    AddWShedShapeLayer = currNodes(i).Attributes(0).InnerText

                                Case "Add_Merged_Watershed_Shapefile_Layer"
                                    AddMergedWShedShapeLayer = currNodes(i).Attributes(0).InnerText
                            End Select
                        Next
                    End If

                End With

                With Root.Item("Calculate_Special_Shapefile_Field_Options")
                    currNodes = .SelectNodes("Option")
                    If Not currNodes Is Nothing And currNodes.Count > 0 Then
                        For i As Integer = 0 To currNodes.Count - 1
                            Select Case currNodes(i).Attributes(0).Name
                                Case "Calculate_Special_Stream_Fields"
                                    CalcSpecialStreamFields = currNodes(i).Attributes(0).InnerText

                                Case "Calculate_Special_Watershed_Fields"
                                    CalcSpecialWshedFields = currNodes(i).Attributes(0).InnerText

                                Case "Calculate_Special_Merge_Watershed_Fields"
                                    calcSpecialMergeWshedFields = currNodes(i).Attributes(0).InnerText
                            End Select
                        Next
                    End If
                End With

                With Root.Item("Misc_Options")
                    currNodes = .SelectNodes("Option")
                    If Not currNodes Is Nothing And currNodes.Count > 0 Then
                        For i As Integer = 0 To currNodes.Count - 1
                            Select Case currNodes(i).Attributes(0).Name
                                Case "Max_Grid_File_Size"
                                    g_MaxFileSize = currNodes(i).Attributes(0).InnerText

                                Case "Relative_Output_Path"
                                    OutputPath = currNodes(i).Attributes(0).InnerText
                                    
                                Case "Maximum_Process_Count"
                                    numProcesses = currNodes(i).Attributes(0).InnerText
                                Case "Show_TauDEM_output"
                                    ShowTaudemOutput = CBool(currNodes(i).Attributes(0).InnerText)

                            End Select
                        Next
                    End If
                End With
            Else
                SaveConfig()
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

#End Region
End Class

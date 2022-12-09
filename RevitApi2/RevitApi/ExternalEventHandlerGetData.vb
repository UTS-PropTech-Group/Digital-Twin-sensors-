Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB.Plumbing
Imports System.Net
Imports System.Web.Script.Serialization
Imports Autodesk.Revit.DB.View

Public Class ExternalEventHandlerGetData
    Implements Autodesk.Revit.UI.IExternalEventHandler
    Public Property UserControl0 As UserControl1
    Public Property Document As Document
    Private counter As Integer = 0
    Private Property Temperature_String As String
    Private Property pressure_String As String
    Private Property humidity_String As String
    Private Property ultrav_String As String
    Private Property light_String As String
    Private Property particle_String As String
    Private uidoc As Autodesk.Revit.UI.UIDocument
    'Private date_list As New List(Of Date)
    ' Public Color_dictionary As New Dictionary(Of Double, Autodesk.Revit.DB.Color)
    ' Private pause As Boolean = False
    'Private current_index As Integer = 0
    Public Sub Execute(app As Autodesk.Revit.UI.UIApplication) Implements Autodesk.Revit.UI.IExternalEventHandler.Execute

        '  Document = app.ActiveUIDocument.Document

        Try
            UserControl0.app = app
            ' Select some elements in Revit before invoking this command
            ' Get the handle of current document.
            uidoc = app.ActiveUIDocument
            ' Get the element selection of current document.
            Dim selection As Autodesk.Revit.UI.Selection.Selection
            Dim selectedIds As ICollection(Of Autodesk.Revit.DB.ElementId)

            '*********
            'Dim filter As New ElementClassFilter(GetType(FamilyInstance))

            'Dim collector As New FilteredElementCollector(uidoc.Document)
            'collector.WherePasses(filter)


            'Dim query As System.Collections.Generic.IEnumerable(Of Autodesk.Revit.DB.Element)
            'query = From element In collector
            '        Where element.LookupParameter("Comment").AsString = "ssd_42C322"
            '        Select element

            '' Cast found elements to family instances, 
            '' this cast to FamilyInstance is safe because ElementClassFilter for FamilyInstance was used
            'Dim familyInstances As List(Of FamilyInstance) = query.Cast(Of FamilyInstance)().ToList()

            ''********
            Dim doc As Document = app.ActiveUIDocument.Document
            'For Each familyIns In familyInstances
            '    Dim id As Autodesk.Revit.DB.ElementId = familyIns.Id
            '    Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
            '    ogs.SetProjectionLineColor(New Color(0, 255, 0))
            '    Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
            '        t.Start()
            '        doc.ActiveView.SetElementOverrides(id, ogs)
            '        t.Commit()
            '    End Using
            'Next

            '' Dim uidoc As Autodesk.Revit.UI.UIDocument = app.ActiveUIDocument


            '********
            If UserControl0.Request_Type = "LastStatus" Then
                selection = uidoc.Selection
                selectedIds = uidoc.Selection.GetElementIds()
                If 0 = selectedIds.Count Then
                    ' If no elements selected.
                    TaskDialog.Show("Revit", "You haven't selected any elements.")
                ElseIf 1 = selectedIds.Count Then
                    ' Dim info = "Ids of selected elements in the document are: "
                    'For Each id0 As Autodesk.Revit.DB.ElementId In selectedIds
                    '    info += vbLf & vbTab & id0.IntegerValue
                    Dim elements As Autodesk.Revit.DB.Element = uidoc.Document.GetElement(selectedIds(0))
                    Dim SensorID = elements.GetParameters("Comments").First.AsValueString
                    'Next
                    CurlRequest(SensorID, UserControl0.Sdate, UserControl0.Edate)
                    ' TaskDialog.Show("Revit", info)
                    UserControl0.TextBox_Temperature.Text = UserControl0.Temperature_array.Last.value
                    UserControl0.TextBox_Pressure.Text = UserControl0.pressure_array.Last.value
                    UserControl0.TextBox_Humidity.Text = UserControl0.humidity_array.Last.value
                    UserControl0.TextBox_Light.Text = UserControl0.light_array.Last.value
                    UserControl0.TextBox_UltraV.Text = UserControl0.ultrav_array.Last.value
                    UserControl0.TextBox_Particle10.Text = UserControl0.particle_array.Last.apm10
                    UserControl0.TextBox_Particle25.Text = UserControl0.particle_array.Last.apm25
                Else
                    TaskDialog.Show("Revit", "You have selected multiplle elements.")
                End If
            ElseIf UserControl0.Request_Type = "FindSensors" Then
                selection = uidoc.Selection
                selectedIds = uidoc.Selection.GetElementIds()
                Dim elements As Autodesk.Revit.DB.Element
                Dim newSensor As SensorItem
                For Each elementID In selectedIds
                    counter += 1
                    elements = uidoc.Document.GetElement(elementID)

                    Dim test_param = elements.LookupParameter("Comments")
                    If IsNothing(test_param) Then
                        Dim asdjsd As Integer = 3532
                    Else
                        Dim SensorID = elements.GetParameters("Comments").First.AsValueString
                        If Not IsNothing(SensorID) AndAlso SensorID.Contains("ssd") Then
                            newSensor = New SensorItem
                            newSensor.sensorid = SensorID
                            newSensor.element = elements
                            UserControl0.sensor_list.Add(newSensor)
                        End If
                    End If
                Next
                Dim sensor_names As String
                For Each item In UserControl0.sensor_list
                    sensor_names += vbCrLf & item.sensorid
                Next
                TaskDialog.Show("Revit", sensor_names)
            ElseIf UserControl0.Request_Type = "HeatMap" Then

                For Each item In UserControl0.sensor_list
                    Reset_element_graphic(item)
                    CurlRequest_HeatMap(item, UserControl0.Sdate, UserControl0.Edate, UserControl0.Combobox_Type.Text)
                Next
                FindMinMax_ColorCode(UserControl0.Combobox_Type.Text)

            ElseIf UserControl0.Request_Type = "Play" Then

                Play()
            ElseIf UserControl0.Request_Type = "IFC" Then
                Dim IFCExportOptions0 As New IFCExportOptions
                'Dim myIFCExportConfiguration As BIM.IFC.Export.UI.IFCExportConfiguration = BIM.IFC.Export.UI.IFCExportConfiguration.CreateDefaultConfiguration()
                'myIFCExportConfiguration.
                'Dim ExportViewId As Autodesk.Revit.DB.ElementId = Nothing
                'myIFCExportConfiguration.UpdateOptions(IFCExportOptions0, ExportViewId)

                'For Each item In UserControl0.sensor_list
                '    CurlRequest(item.sensorid, UserControl0.Sdate, UserControl0.Edate)

                '    'IFCExportOptions0.AddOption("SensorParamater", item.sensorid & "*" & Temperature_String)
                '    'IFCExportOptions0.AddOption("SensorParamater", item.sensorid & "*" & pressure_String)
                '    'IFCExportOptions0.AddOption("SensorParamater", item.sensorid & "*" & humidity_String)
                '    'IFCExportOptions0.AddOption("SensorParamater", item.sensorid & "*" & light_String)
                '    'IFCExportOptions0.AddOption("SensorParamater", item.sensorid & "*" & ultrav_String)
                '    'IFCExportOptions0.AddOption("SensorParamater", item.sensorid & "*" & particle_String)
                '    'IFCExportOptions0.AddOption("SensorParamater", "am1374")
                '    'IFCExportOptions0.AddOption("SensorParamater", "am1374")
                '    'IFCExportOptions0.AddOption("SensorParamater", "am1374")
                '    'IFCExportOptions0.AddOption("SensorParamater", "am1374")
                '    'IFCExportOptions0.AddOption("SensorParamater", "am1374")
                '    'IFCExportOptions0.AddOption("SensorParamater", "am1374")
                '    'Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Parameter")
                '    '    t.Start()
                '    '    item.element.LookupParameter("Temperature").Set(Temperature_String)
                '    '    item.element.LookupParameter("Pressure").Set(pressure_String)
                '    '    item.element.LookupParameter("Humidity").Set(humidity_String)
                '    '    item.element.LookupParameter("Light").Set(light_String)
                '    '    item.element.LookupParameter("UltraV").Set(ultrav_String)
                '    '    item.element.LookupParameter("ParticleAvg").Set(particle_String)
                '    '    t.Commit()
                '    'End Using

                'Next
                Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Parameter")
                    t.Start()
                    doc.Export(UserControl0.FolderPath, "Revit-IFC-Export", IFCExportOptions0)
                    t.Commit()
                End Using

            End If




        Catch e As Exception
            message = e.Message
        End Try


    End Sub
    Public Sub Reset_element_graphic(item As SensorItem)
        Dim doc As Document = uidoc.Document
        Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
        ' ogs.SetSurfaceBackgroundPatternColor(New Color(0, 255, 0))
        '   ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)

        ' ogs.SetSurfaceTransparency(50)
        Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
            t.Start()
            doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
            t.Commit()
        End Using
    End Sub
    Public Sub CurlRequest_HeatMap(item As SensorItem, SDate As String, EDate As String, type As String)
        Try

            Dim myReq As HttpWebRequest
            Dim myResp As HttpWebResponse
            Dim Serializer As New JavaScriptSerializer
            Serializer.MaxJsonLength = Integer.MaxValue
            Dim myreader As System.IO.StreamReader
            Dim myText As String
            '**************************temperature*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/" & type)

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & item.sensorid)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd


            Select Case type
                Case "temperature"
                    item.Temperature_array = Serializer.Deserialize(Of SensorData())(myText)
                Case "pressure"
                    item.pressure_array = Serializer.Deserialize(Of SensorData())(myText)
                Case "humidity"
                    item.humidity_array = Serializer.Deserialize(Of SensorData())(myText)
                Case "light"
                    item.light_array = Serializer.Deserialize(Of SensorData())(myText)
                Case "ultrav"
                    item.ultrav_array = Serializer.Deserialize(Of SensorData())(myText)
                Case "particle10"
                    item.particle_array = Serializer.Deserialize(Of ParticleData())(myText)
                    For Each item0 In item.particle_array
                        item0.value = item0.apm10
                        item0.ts = item0.d.AddHours(item0.h)
                    Next
                Case "particle2.5"
                    item.particle_array = Serializer.Deserialize(Of ParticleData())(myText)
                    For Each item0 In item.particle_array
                        item0.value = item0.apm25
                        item0.ts = item0.d.AddHours(item0.h)
                    Next
            End Select






        Catch ex As Exception

        End Try
    End Sub
    Public Sub CurlRequest(SensorID As String, SDate As String, EDate As String)
        Try


            Dim myReq As HttpWebRequest
            Dim myResp As HttpWebResponse
            Dim Serializer As New JavaScriptSerializer
            Serializer.MaxJsonLength = Integer.MaxValue
            Dim myreader As System.IO.StreamReader
            Dim myText As String
            '**************************temperature*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/temperature")

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & SensorID)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd
            Temperature_String = myText
            UserControl0.Temperature_array = Serializer.Deserialize(Of SensorData())(myText)

            '**************************temperature*****************************
            '**************************pressure*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/pressure")

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & SensorID)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd
            pressure_String = myText
            UserControl0.pressure_array = Serializer.Deserialize(Of SensorData())(myText)

            '**************************pressure*****************************
            '**************************humidity*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/humidity")

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & SensorID)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd
            humidity_String = myText
            UserControl0.humidity_array = Serializer.Deserialize(Of SensorData())(myText)

            '**************************humidity*****************************
            '**************************light*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/light")

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & SensorID)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd
            light_String = myText
            UserControl0.light_array = Serializer.Deserialize(Of SensorData())(myText)

            '**************************light*****************************
            '**************************ultrav*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/ultrav")

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & SensorID)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd
            ultrav_String = myText
            UserControl0.ultrav_array = Serializer.Deserialize(Of SensorData())(myText)

            '**************************ultrav*****************************
            '**************************particleavg*****************************
            myReq = HttpWebRequest.Create("https://www.hibouconnect.com/tapi/particleavg")

            myReq.Method = "GET"
            ' myReq.ContentType = "application/json"
            myReq.Headers.Add("code: 083-083-068-156-105-114")
            myReq.Headers.Add("api-key:d70c04a537f617ef5ac3ff369fdd0f94")
            myReq.Headers.Add("app: hibou5775")
            myReq.Headers.Add("sensor:" & SensorID)
            myReq.Headers.Add("from: " & SDate)
            myReq.Headers.Add("to: " & EDate)

            'Dim myData As String = "{""version"":""1.1"",""method"":""room_verify"",""params"":{""account"":{""type"":""room"",""value"":""0""}}}"
            '  myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
            myResp = myReq.GetResponse
            myreader = New System.IO.StreamReader(myResp.GetResponseStream)

            myText = myreader.ReadToEnd
            particle_String = myText
            UserControl0.particle_array = Serializer.Deserialize(Of ParticleData())(myText)
            For Each item In UserControl0.particle_array
                'item.value = (item.apm10 + item.apm25 + item.apm1) / 3
                item.ts = item.d.AddHours(item.h)
            Next

            '**************************particleavg*****************************
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FindMinMax_ColorCode(type As String)
        Dim temp_array As New List(Of SensorData)
        Dim temp_particle_array As New List(Of ParticleData)
        Dim min As Double
        Dim max As Double
        Select Case type
            Case "temperature"
                For Each item In UserControl0.sensor_list
                    temp_array.AddRange(item.Temperature_array)
                Next
                min = temp_array.Min(Function(obj) obj.value)
                max = temp_array.Max(Function(obj) obj.value)
            Case "pressure"
                For Each item In UserControl0.sensor_list
                    temp_array.AddRange(item.pressure_array)
                Next
                min = temp_array.Min(Function(obj) obj.value)
                max = temp_array.Max(Function(obj) obj.value)
            Case "humidity"
                For Each item In UserControl0.sensor_list
                    temp_array.AddRange(item.humidity_array)
                Next
                min = temp_array.Min(Function(obj) obj.value)
                max = temp_array.Max(Function(obj) obj.value)
            Case "light"
                For Each item In UserControl0.sensor_list
                    temp_array.AddRange(item.light_array)
                Next
                min = temp_array.Min(Function(obj) obj.value)
                max = temp_array.Max(Function(obj) obj.value)
            Case "ultrav"
                For Each item In UserControl0.sensor_list
                    temp_array.AddRange(item.ultrav_array)
                Next
                min = temp_array.Min(Function(obj) obj.value)
                max = temp_array.Max(Function(obj) obj.value)
            Case "particle10"
                For Each item In UserControl0.sensor_list
                    temp_particle_array.AddRange(item.particle_array)
                Next
                min = temp_particle_array.Min(Function(obj) obj.apm10)
                max = temp_particle_array.Max(Function(obj) obj.apm10)
            Case "particle2.5"
                For Each item In UserControl0.sensor_list
                    temp_particle_array.AddRange(item.particle_array)
                Next
                min = temp_particle_array.Min(Function(obj) obj.apm25)
                max = temp_particle_array.Max(Function(obj) obj.apm25)
        End Select
        UserControl0.Label_Min.Content = Math.Round(min, 2)
        UserControl0.Label_Max.Content = Math.Round(max, 2)
        'Dim Color_dictionary As New Dictionary(Of Double, Autodesk.Revit.DB.Color)
        UserControl0.Color_dictionary.Clear()
        UserControl0.Color_dictionary.Add((max - min) * 0 / 5 + min, New Color(255, 0, 255))
        UserControl0.Color_dictionary.Add((max - min) * 1 / 5 + min, New Color(0, 0, 255))
        UserControl0.Color_dictionary.Add((max - min) * 2 / 5 + min, New Color(0, 255, 255))
        UserControl0.Color_dictionary.Add((max - min) * 3 / 5 + min, New Color(0, 255, 0))
        UserControl0.Color_dictionary.Add((max - min) * 4 / 5 + min, New Color(255, 255, 0))
        UserControl0.Color_dictionary.Add((max - min) * 5 / 5 + min, New Color(255, 0, 0))


    End Sub
    Public Sub Play()
        Try
            Select Case UserControl0.Combobox_Type.Text
                Case "temperature"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.Temperature_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As SensorData
                            sensor_data = item.Temperature_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
                Case "pressure"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.pressure_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As SensorData
                            sensor_data = item.pressure_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
                Case "humidity"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.humidity_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As SensorData
                            sensor_data = item.humidity_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
                Case "light"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.light_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As SensorData
                            sensor_data = item.light_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
                Case "ultrav"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.ultrav_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As SensorData
                            sensor_data = item.ultrav_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
                Case "particle10"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.particle_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As ParticleData
                            sensor_data = item.particle_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
                Case "particle2.5"
                    Dim doc As Document = uidoc.Document
                    For Each item In UserControl0.sensor_list
                        If item.particle_array.Count <> 0 Then
                            Dim FillPatternElement As FillPatternElement
                            FillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, "<Solid fill>")
                            Dim ogs As Autodesk.Revit.DB.OverrideGraphicSettings = New Autodesk.Revit.DB.OverrideGraphicSettings()
                            Dim sensor_data As ParticleData
                            sensor_data = item.particle_array.First(Function(obj) obj.ts > UserControl0.date_list(UserControl0.current_index))
                            ogs.SetSurfaceBackgroundPatternColor(Color_Code(item, sensor_data))
                            ogs.SetSurfaceBackgroundPatternId(FillPatternElement.Id)
                            ogs.SetSurfaceTransparency(50)
                            Using t As Autodesk.Revit.DB.Transaction = New Autodesk.Revit.DB.Transaction(doc, "Set Element Override")
                                t.Start()
                                doc.ActiveView.SetElementOverrides(item.element.Id, ogs)
                                t.Commit()
                            End Using
                        End If
                    Next
            End Select



        Catch ex As Exception
            'MessageBox.Show(ex.ToString)
        End Try
    End Sub
    Private Function Color_Code(item As SensorItem, sensor_data As Object) As Color
        Try


            Dim Value1 As Double
            Dim Value2 As Double
            Dim color1 As Color
            Dim color2 As Color
            Dim newcolor As Color
            Value1 = UserControl0.Color_dictionary.Last(Function(obj) obj.Key < sensor_data.value).Key
            Value2 = UserControl0.Color_dictionary.First(Function(obj) obj.Key > sensor_data.value).Key
            color1 = UserControl0.Color_dictionary.Last(Function(obj) obj.Key < sensor_data.value).Value
            color2 = UserControl0.Color_dictionary.First(Function(obj) obj.Key > sensor_data.value).Value
            Dim temp = Math.Round((sensor_data.value - Value1) / (Value2 - Value1), 2)
            Dim r As Integer = Math.Round(CInt(color1.Red) + temp * (CInt(color2.Red) - CInt(color1.Red)))
            Dim g As Integer = Math.Round(CInt(color1.Green) + temp * (CInt(color2.Green) - CInt(color1.Green)))
            Dim b As Integer = Math.Round(CInt(color1.Blue) + temp * (CInt(color2.Blue) - CInt(color1.Blue)))
            newcolor = New Color(r, g, b)
            Return newcolor


        Catch ex As Exception

        End Try
    End Function
    Public Function GetName() As String Implements Autodesk.Revit.UI.IExternalEventHandler.GetName
        Return "Get Sensor Data"
    End Function
End Class

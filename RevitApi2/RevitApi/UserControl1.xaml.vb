Imports System.Windows.Controls
Imports System.Windows
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Microsoft.Win32
Imports System.Collections.ObjectModel
Imports System.ComponentModel


Public Class UserControl1
    Inherits Page
    Implements IDockablePaneProvider
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Property ExEventGetSensorData As Autodesk.Revit.UI.ExternalEvent

    Public Property Temperature As String
    Public Property Pressure As String
    Public Property Light As String
    Public Property UltraV As String
    Public Property Particle As String
    Public Property Humidity As String
    Public Property sensor_list As New List(Of SensorItem)
    Public Property Temperature_array As SensorData()
    Public Property pressure_array As SensorData()
    Public Property humidity_array As SensorData()
    Public Property ultrav_array As SensorData()
    Public Property light_array As SensorData()
    Public Property particle_array As ParticleData()
    Public Property Sdate As String
    Public Property Edate As String
    Public Property Request_Type As String
    Public pause As Boolean = False
    Public current_index As Integer = 0
    Public date_list As New List(Of Date)
    Public Color_dictionary As New Dictionary(Of Double, Autodesk.Revit.DB.Color)
    Public Property app As Autodesk.Revit.UI.UIApplication
    Public Sub New(ExEventGetSensorData As Autodesk.Revit.UI.ExternalEvent)

        ' This call is required by the designer.
        InitializeComponent()
        Me.ExEventGetSensorData = ExEventGetSensorData
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub UserControl1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim MyAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim Directory As String = MyAssembly.Location
        Directory = Directory.Substring(0, Directory.LastIndexOf("\"))

        Dim b As New System.Windows.Media.ImageBrush(New Windows.Media.Imaging.BitmapImage(New Uri(Directory + "\legend.JPG")))
        Legend.Background = b
        Dim c As New System.Windows.Media.ImageBrush(New Windows.Media.Imaging.BitmapImage(New Uri(Directory + "\Play.png")))
        Button_HM_Play.Background = c
        Dim d As New System.Windows.Media.ImageBrush(New Windows.Media.Imaging.BitmapImage(New Uri(Directory + "\Pause.png")))
        Button_HM_Pause.Background = d
        Dim g As New System.Windows.Media.ImageBrush(New Windows.Media.Imaging.BitmapImage(New Uri(Directory + "\Remove.png")))
        Button_HM_Reset.Background = g
    End Sub

    Public Sub SetupDockablePane(data As DockablePaneProviderData) Implements IDockablePaneProvider.SetupDockablePane
        data.FrameworkElement = CType(Me, FrameworkElement)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Request_Type = "LastStatus"
        Sdate = DateTime.Now.ToString("yyyy-MM-dd")
        Edate = DateTime.Now.ToString("yyyy-MM-dd")
        ExEventGetSensorData.Raise()

        'TextBox_Temperature.Text = Temperature_array.Last.value
        'TextBox_Pressure.Text = pressure_array.Last.value
        'TextBox_Humidity.Text = humidity_array.Last.value
        'TextBox_Light.Text = light_array.Last.value
        'TextBox_UltraV.Text = ultrav_array.Last.value
        'TextBox_ParticleAvg.Text = particle_array.Last.value
    End Sub

    Private Sub Button_HM_Get_Click(sender As Object, e As RoutedEventArgs) Handles Button_HM_Get.Click
        current_index = 0
        pause = True
        ' sensor_list.Clear()
        Request_Type = "HeatMap"
        Dim date1 As Date = Datepicker1.SelectedDate
        Dim date2 As Date = Datepicker2.SelectedDate
        Sdate = date1.ToString("yyyy-MM-dd")
        Edate = date2.ToString("yyyy-MM-dd")
        ExEventGetSensorData.Raise()

        date_list.Clear()
        Dim temp As Boolean = False
        Dim date11 As Date = Datepicker1.SelectedDate
        Dim date22 As Date = Datepicker2.SelectedDate
        Do While temp = False
            Select Case Combobox_Interval.Text
                Case "Daily"
                    date_list.Add(date11)
                    date11 = date11.AddDays(1)
                    If date11 > date22 Then
                        temp = True
                    End If
                Case "Hourly"
                    date_list.Add(date11)
                    date11 = date11.AddHours(1)
                    If date11 > date22 Then
                        temp = True
                    End If
                Case "30min"
                    date_list.Add(date11)
                    date11 = date11.AddMinutes(30)
                    If date11 > date22 Then
                        temp = True
                    End If
                Case "15min"
                    date_list.Add(date11)
                    date11 = date11.AddMinutes(15)
                    If date11 > date22 Then
                        temp = True
                    End If
                Case "5min"
                    date_list.Add(date11)
                    date11 = date11.AddMinutes(5)
                    If date11 > date22 Then
                        temp = True
                    End If
                Case "minutely"
                    date_list.Add(date11)
                    date11 = date11.AddMinutes(1)
                    If date11 > date22 Then
                        temp = True
                    End If
            End Select
        Loop

    End Sub

    Private Sub Button_Find_Sensors_Click(sender As Object, e As RoutedEventArgs) Handles Button_Find_Sensors.Click
        sensor_list.Clear()
        Request_Type = "FindSensors"
        ExEventGetSensorData.Raise()
    End Sub

    Private Async Sub Button_HM_Play_Click(sender As Object, e As RoutedEventArgs) Handles Button_HM_Play.Click
        pause = False
        Request_Type = "Play"
        ' ExEventGetSensorData.Raise()


        Do While pause = False
            TextBox_Current_Date.Text = date_list(current_index).ToString("yyyy-MM-dd HH:mm")
            ExEventGetSensorData.Raise()

            Await Task.Run(Sub()
                               Task.Delay(New TimeSpan(0, 0, 1)).Wait()
                           End Sub)

            current_index += 1
            If current_index > date_list.Count - 1 Then
                pause = True
            End If
        Loop


    End Sub

    Private Sub Button_HM_Pause_Click(sender As Object, e As RoutedEventArgs) Handles Button_HM_Pause.Click
        pause = True
    End Sub

    Private Sub Button_HM_Reset_Click(sender As Object, e As RoutedEventArgs) Handles Button_HM_Reset.Click
        pause = True
        current_index = 0
    End Sub
    Public FolderPath As String
    Private Sub Button_IFC_Click(sender As Object, e As RoutedEventArgs) Handles Button_IFC.Click
        Dim OFD As New System.Windows.Forms.FolderBrowserDialog
        Dim Result As Nullable(Of Boolean) = OFD.ShowDialog()

        If (Result = True) Then
            FolderPath = OFD.SelectedPath
            If OFD.SelectedPath = "" Then
                MessageBox.Show("Select folder")
                Exit Sub
            End If
        Else
            MessageBox.Show("Select folder")
            Exit Sub
        End If
        Sdate = New Date(2022, 9, 1)
        Edate = DateTime.Now.ToString("yyyy-MM-dd")
        Request_Type = "IFC"
        ExEventGetSensorData.Raise()
    End Sub

    Private Sub Button_HM_Next_Click(sender As Object, e As RoutedEventArgs) Handles Button_HM_Next.Click
        Try
            pause = True
            current_index += 1
            If current_index <= date_list.Count - 1 Then
                Request_Type = "Play"
                TextBox_Current_Date.Text = date_list(current_index).ToString("yyyy-MM-dd HH:mm")
                ExEventGetSensorData.Raise()
            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub Button_HM_previous_Click(sender As Object, e As RoutedEventArgs) Handles Button_HM_previous.Click
        Try
            pause = True
            current_index -= 1
            If current_index >= 0 Then
                Request_Type = "Play"
                TextBox_Current_Date.Text = date_list(current_index).ToString("yyyy-MM-dd HH:mm")
                ExEventGetSensorData.Raise()
            End If

        Catch ex As Exception

        End Try

    End Sub
End Class

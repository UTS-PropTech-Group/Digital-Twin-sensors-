#Region "Imported Namespaces"
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.Reflection
#End Region

Class AdskApplication
    Implements IExternalApplication
    Private m_DockableWindow As UserControl1 = Nothing
    ''' <summary>
    ''' This method is called when Revit starts up before a 
    ''' document or default template is actually loaded.
    ''' </summary>
    ''' <param name="app">An object passed to the external 
    ''' application which contains the controlled application.</param>
    ''' <returns>Return the status of the external application. 
    ''' A result of Succeeded means that the external application started successfully. 
    ''' Cancelled can be used to signify a problem. If so, Revit informs the user that 
    ''' the external application failed to load and releases the internal reference.
    ''' </returns>
    Public Function OnStartup(
      ByVal app As UIControlledApplication) _
        As Result Implements IExternalApplication.OnStartup

        '    'TODO: Add your code here

        '    'Must return some code

        app.CreateRibbonTab("RevitApi")
        Dim RibbonPanel = app.CreateRibbonPanel("RevitApi", "RevitApi")
        Dim Path As String = Assembly.GetExecutingAssembly().Location
        Dim PushRegisterButton As New PushButtonData("Register", "Register", Path, "RevitApi.RegisterDockableWindow")
        Dim MyAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim Directory As String = MyAssembly.Location
        Directory = Directory.Substring(0, Directory.LastIndexOf("\"))
        PushRegisterButton.LargeImage = New Windows.Media.Imaging.BitmapImage(New Uri(Directory + "\Register.png"))
        PushRegisterButton.AvailabilityClassName = "RevitApi.AvailabilityNoOpenDocument"
        Dim PushShowButton = New PushButtonData("Show", "Show", Path, "RevitApi.AdskCommand")
        PushShowButton.LargeImage = New Windows.Media.Imaging.BitmapImage(New Uri(Directory + "\Show.png"))
        Dim RibbonItem2 = RibbonPanel.AddItem(PushShowButton)
        Dim RibbonItem1 = RibbonPanel.AddItem(PushRegisterButton)

        Return Result.Succeeded
    End Function

    ''' <summary>
    ''' This method is called when Revit is about to exit.
    ''' All documents are closed before this method is called.
    ''' </summary>
    ''' <param name="app">An object passed to the external 
    ''' application which contains the controlled application.</param>
    ''' <returns>Return the status of the external application. 
    ''' A result of Succeeded means that the external application successfully shutdown. 
    ''' Cancelled can be used to signify that the user cancelled the external operation 
    ''' at some point. If false is returned then the Revit user should be warned of the 
    ''' failure of the external application to shut down correctly.</returns>
    Public Function OnShutdown(
      ByVal app As UIControlledApplication) _
    As Result Implements IExternalApplication.OnShutdown

        'TODO: Add your code here

        'Must return some code
        Return Result.Succeeded
    End Function
End Class

Public Class AvailabilityNoOpenDocument
    Implements Autodesk.Revit.UI.IExternalCommandAvailability
    Public Function IsCommandAvailable(applicationData As Autodesk.Revit.UI.UIApplication, selectedCategories As CategorySet) As Boolean Implements Autodesk.Revit.UI.IExternalCommandAvailability.IsCommandAvailable
        If IsNothing(applicationData.ActiveUIDocument) Then
            Return True
        Else
            Return False
        End If
    End Function
End Class

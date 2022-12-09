Imports System.Reflection
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Public Class Class1
    Implements IExternalApplication

    Public Function OnStartup(application As UIControlledApplication) As Result Implements IExternalApplication.OnStartup

        Return Result.Succeeded
    End Function

    Public Function OnShutdown(application As UIControlledApplication) As Result Implements IExternalApplication.OnShutdown
        Return Result.Succeeded
    End Function
End Class
'Public Class AvailabilityNoOpenDocument
'    Implements Autodesk.Revit.UI.IExternalCommandAvailability
'    Public Function IsCommandAvailable(applicationData As Autodesk.Revit.UI.UIApplication, selectedCategories As CategorySet) As Boolean Implements IExternalCommandAvailability.IsCommandAvailable
'        If IsNothing(applicationData.ActiveUIDocument) Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function
'End Class
'<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
'Public Class RegisterDockableWindow
'    Implements IExternalCommand
'    Private m_DockableWindow As UserControl1 = Nothing

'    Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
'        Try
'            Dim DockablePaneDataProvider As DockablePaneProviderData = New DockablePaneProviderData()

'            m_DockableWindow = New UserControl1()

'            DockablePaneDataProvider.FrameworkElement = CType(m_DockableWindow, Windows.FrameworkElement)
'            DockablePaneDataProvider.InitialState = New Autodesk.Revit.UI.DockablePaneState()
'            DockablePaneDataProvider.InitialState.DockPosition = Autodesk.Revit.UI.DockPosition.Tabbed
'            DockablePaneDataProvider.InitialState.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
'            Dim DpId As Autodesk.Revit.UI.DockablePaneId = New Autodesk.Revit.UI.DockablePaneId(New Guid("{D7E587F0-C21A-49A4-8677-E0DAA325ECF8}"))
'            commandData.Application.RegisterDockablePane(DpId, "Revit Auto-draw", m_DockableWindow)
'            AddHandler commandData.Application.ViewActivated, AddressOf ViewActivated
'            Return Result.Succeeded
'        Catch ex As Exception
'            Windows.MessageBox.Show(ex.Message)
'            Return Result.Failed
'        End Try
'    End Function
'    Public Sub ViewActivated(sender As Object, e As Events.ViewActivatedEventArgs)

'    End Sub
'End Class
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)>
Public Class ShowDockablePane
    Implements IExternalCommand

    Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
        Dim DpId As Autodesk.Revit.UI.DockablePaneId = New Autodesk.Revit.UI.DockablePaneId(New Guid("{D7E587F0-C21A-49A4-8677-E0DAA325ECF8}"))
        Dim DockablePane As Autodesk.Revit.UI.DockablePane = commandData.Application.GetDockablePane(DpId)
        DockablePane.Show()
        Return Result.Succeeded
    End Function
End Class
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)>
Public Class HideDockablePane
    Implements IExternalCommand

    Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
        Dim DpId As Autodesk.Revit.UI.DockablePaneId = New Autodesk.Revit.UI.DockablePaneId(New Guid("{D7E587F0-C21A-49A4-8677-E0DAA325ECF8}"))
        Dim DockablePane As Autodesk.Revit.UI.DockablePane = commandData.Application.GetDockablePane(DpId)
        DockablePane.Hide()
        Return Result.Succeeded
    End Function
End Class


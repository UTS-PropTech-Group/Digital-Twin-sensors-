#Region "Imported Namespaces"
Imports System
Imports System.Collections.Generic
Imports Autodesk.Revit.ApplicationServices
Imports Autodesk.Revit.Attributes
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.UI.Selection
#End Region
<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
Public Class RegisterDockableWindow
    Implements IExternalCommand

    Private m_DockableWindow As UserControl1 = Nothing

    Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
        Try
            Dim DockablePaneDataProvider As DockablePaneProviderData = New DockablePaneProviderData()
            Dim ExternalEventHandlerDraw As New ExternalEventHandlerGetData()
            m_DockableWindow = New UserControl1(Autodesk.Revit.UI.ExternalEvent.Create(ExternalEventHandlerDraw))
            ExternalEventHandlerDraw.UserControl0 = m_DockableWindow
            DockablePaneDataProvider.FrameworkElement = CType(m_DockableWindow, Windows.FrameworkElement)
            DockablePaneDataProvider.InitialState = New Autodesk.Revit.UI.DockablePaneState()
            DockablePaneDataProvider.InitialState.DockPosition = Autodesk.Revit.UI.DockPosition.Tabbed
            DockablePaneDataProvider.InitialState.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
            Dim DpId As Autodesk.Revit.UI.DockablePaneId = New Autodesk.Revit.UI.DockablePaneId(New Guid("{D7E587F0-C21A-49A4-8677-E0DAA325ECF8}"))
            commandData.Application.RegisterDockablePane(DpId, "Revit Auto-draw", m_DockableWindow)
            AddHandler commandData.Application.ViewActivated, AddressOf ViewActivated
            Return Result.Succeeded
        Catch ex As Exception
            Windows.MessageBox.Show(ex.Message)
            Return Result.Failed
        End Try
    End Function
    Public Sub ViewActivated(sender As Object, e As Events.ViewActivatedEventArgs)

    End Sub
End Class
<Transaction(TransactionMode.ReadOnly)>
Public Class AdskCommand
    Implements IExternalCommand
    Private m_DockableWindow As UserControl1 = Nothing
    ''' <summary>
    ''' The one and only method required by the IExternalCommand interface, the main entry point for every external command.
    ''' </summary>
    ''' <param name="commandData">Input argument providing access to the Revit application, its documents and their properties.</param>
    ''' <param name="message">Return argument to display a message to the user in case of error if Result is not Succeeded.</param>
    ''' <param name="elements">Return argument to highlight elements on the graphics screen if Result is not Succeeded.</param>
    ''' <returns>Cancelled, Failed or Succeeded Result code.</returns>
    Public Function Execute(
      ByVal commandData As ExternalCommandData,
      ByRef message As String,
      ByVal elements As ElementSet) _
    As Result Implements IExternalCommand.Execute

        'Dim uiapp As Autodesk.Revit.UI.UIApplication = commandData.Application
        'Dim uidoc As Autodesk.Revit.UI.UIDocument = uiapp.ActiveUIDocument
        'Dim app As Application = uiapp.Application
        'Dim doc As Document = uidoc.Document

        'Dim sel As Autodesk.Revit.UI.Selection.Selection = uidoc.Selection

        '**************************************


        Dim DpId As Autodesk.Revit.UI.DockablePaneId = New Autodesk.Revit.UI.DockablePaneId(New Guid("{D7E587F0-C21A-49A4-8677-E0DAA325ECF8}"))
        Dim DockablePane As Autodesk.Revit.UI.DockablePane = commandData.Application.GetDockablePane(DpId)
        DockablePane.Show()


        '**************************************




        '  Return Result.Succeeded

        'Using tx As New Autodesk.Revit.DB.Transaction(doc)
        '    tx.Start("RevitApi")
        '    tx.Commit()
        'End Using

        'Must return some code
        Return Result.Succeeded
    End Function
End Class

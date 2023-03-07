Imports DMD
Imports DMD.Sistema

Public Class DataEventArgs
    Inherits System.EventArgs

    Public Data As String

    Public Sub New()
        Me.Data = ""
    End Sub

    Public Sub New(ByVal data As String)
        Me.Data = data
    End Sub

End Class

Public Class WebPageAnalizer
    Implements IDisposable, IRPCCallHandler

    Public Event DataReady(ByVal sender As Object, ByVal e As DataEventArgs)

    Private m_Stopping As Boolean
    Private m_AR As DMD.Sistema.AsyncState
    'Private m_WC As  System.Windows.Forms.WebBrowser

    Public Sub New()
        ' Me.m_WC = New System.Windows.Forms.WebBrowser
        ' Me.m_WC.ScriptErrorsSuppressed = True
        'AddHandler Me.m_WC.Navigating, AddressOf handleNavigating
        'AddHandler Me.m_WC.DocumentCompleted, AddressOf OnDataReady
        Me.m_Stopping = False
    End Sub

    Public Sub Analize(ByVal url As String)
        Me.Cancel()
        Dim file As String = FileSystem.GetTempFileName
        My.Computer.Network.DownloadFile(url, file)
        Dim text As String = System.IO.File.ReadAllText(file)
        System.IO.File.Delete(file)
        Dim e As New DataEventArgs
        e.Data = text
        Me.OnDataReady(e)
    End Sub

    'Public ReadOnly Property WebControl As System.Windows.Forms.WebBrowser
    '    Get
    '        Return Me.m_WC
    '    End Get
    'End Property


    Public Sub Dispose() Implements IDisposable.Dispose
        If (Me.m_AR IsNot Nothing) Then Me.m_AR.Cancel() : Me.m_AR = Nothing
    End Sub

    Protected Overridable Sub OnDataReady(ByVal e As DataEventArgs)
        'Dim webBrowserForPrinting As WebBrowser = CType(sender, WebBrowser)

        '' Print the document now that it is fully loaded.
        'webBrowserForPrinting.Print()
        'MessageBox.Show("print")

        ' Dispose the WebBrowser now that the task is complete. 
        'webBrowserForPrinting.Dispose()
        RaiseEvent DataReady(Me, e)
    End Sub

    Public Overridable Sub Cancel()
        'If (Me.m_Thread IsNot Nothing) Then
        Me.m_Stopping = True
        If (Me.m_AR IsNot Nothing) Then Me.m_AR.Cancel() : Me.m_AR = Nothing
        Me.m_Stopping = False
        'End If
    End Sub

    'Private Sub handleNavigating(ByVal sender As Object, ByVal e As WebBrowserNavigatingEventArgs)
    '    e.Cancel = Me.m_Stopping
    'End Sub

    Private Sub OnAsyncComplete(res As AsyncResult) Implements IRPCCallHandler.OnAsyncComplete
        Me.m_AR = Nothing
        Dim html As String = res.getResponse
        'Dim doc As New System.Xml.XmlDocument
        'doc.LoadXml(html)
        Me.OnDataReady(New DataEventArgs(html))
    End Sub

    Private Sub OnAsyncError(res As AsyncResult) Implements IRPCCallHandler.OnAsyncError
        Me.m_AR = Nothing
    End Sub
End Class

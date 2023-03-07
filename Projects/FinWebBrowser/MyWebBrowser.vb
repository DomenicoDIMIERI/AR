Imports System.ComponentModel
Imports System.Windows.Forms

Public Class MyWebBrowser
    Inherits System.Windows.Forms.WebBrowser
    ' Implements INotifyPropertyChanged


    ' Private Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged 'Add a event to notify new window opening.  

    Public Event NewWindowOpening(ByVal sender As Object, ByVal e As OpenNewWindowEventArgs)

    Private ww As SHDocVw.WebBrowser_V1

    Public Sub New()
        Me.ww = Nothing
        Me.DocumentText = String.Empty
        Me.ww = CType(Me.ActiveXInstance, SHDocVw.WebBrowser_V1)
        AddHandler Me.ww.NewWindow, AddressOf NewWindowH
    End Sub

    Private Sub NewWindowH(URL As String,
                          Flags As Integer,
                          TargetFrameName As String,
                          ByRef PostData As Object,
                          Headers As String,
                          ByRef Processed As Boolean)
        Processed = True
        System.Threading.SynchronizationContext.Current.Post(AddressOf NavigateOnNewWindow, URL)
    End Sub

    Private Sub NavigateOnNewWindow(NewWindowUrl As Object)
        'Me.ww.Navigate(NewWindowUrl.ToString())
        Dim e As New OpenNewWindowEventArgs(CStr(NewWindowUrl))
        RaiseEvent NewWindowOpening(Me, e)
    End Sub

    'Protected Overrides Sub OnNavigating(e As WebBrowserNavigatingEventArgs)
    '    If ww Is Nothing Then

    '    End If

    '    MyBase.OnNavigating(e)
    'End Sub

    'Protected Overrides Sub OnNewWindow(e As CancelEventArgs)
    '    Dim myElement As HtmlElement = Me.Document.ActiveElement
    '    Dim target As String = myElement.GetAttribute("href")

    '    e.Cancel = True

    '    MyBase.OnNewWindow(e)

    '    Dim e1 As New OpenNewWindowEventArgs(target, Me.Url.Authority)
    '    RaiseEvent NewWindowOpening(Me, e1)
    'End Sub

   

    ''Get the new url in NewWindow3 event  

    'Private Sub ww_NewWindow3(ByRef ppDisp As Object, ByRef Cancel As Boolean, ByVal dwFlags As UInteger, ByVal bstrUrlContext As String, ByVal bstrUrl As String)
    '    Cancel = True

    '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(bstrUrl))  'Event notify  

    '    Dim e As New OpenNewWindowEventArgs(bstrUrl)
    '    RaiseEvent NewWindowOpening(Me, e)
    'End Sub

    ''Private Sub ww_NewWindow2(ByRef ppDisp As Object, ByRef Cancel As Boolean)
    ''    Cancel = True

    ''    Dim myElement As HtmlElement = Me.Document.ActiveElement
    ''    Dim target As String = myElement.GetAttribute("href")

    ''    Dim e As New OpenNewWindowEventArgs(target)
    ''    RaiseEvent NewWindowOpening(Me, e)
    ''End Sub






End Class

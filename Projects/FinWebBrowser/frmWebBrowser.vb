Imports System.Windows.Forms
Imports System.Net

Public Class frmWebBrowser

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub mnuEsci_Click(sender As Object, e As EventArgs) Handles mnuEsci.Click
        Me.Close()
    End Sub

    Private Sub txtWebAddress_Click(sender As Object, e As EventArgs) Handles txtWebAddress.Click

    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Me.NavigateTo(Me.txtWebAddress.Text)
    End Sub

    Private Shared Function CombineURL(ByVal uri1 As String, ByVal uri2 As String) As String
        uri1 = uri1.TrimEnd("/"c)
        uri2 = uri2.TrimStart("/"c)
        Return String.Format("{0}/{1}", uri1, uri2)
    End Function



    Private Sub WebBrowser1_NewWindowOpening(sender As Object, e As OpenNewWindowEventArgs) Handles WebBrowser1.NewWindowOpening
        Dim frmWB As frmWebBrowser
        frmWB = New frmWebBrowser()
        frmWB.NavigateTo(e.URL)
        frmWB.Visible = True

    End Sub

    Private Sub txtWebAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtWebAddress.KeyPress

    End Sub

    Private Sub txtWebAddress_KeyUp(sender As Object, e As KeyEventArgs) Handles txtWebAddress.KeyUp
        If (e.KeyCode = 13) Then
            Me.NavigateTo(Me.txtWebAddress.Text)
        End If
    End Sub

    Public Sub NavigateTo(ByVal address As String)
        Me.txtWebAddress.Text = address
        Me.WebBrowser1.Navigate(address)
    End Sub

    Private Sub mnuShowCode_Click(sender As Object, e As EventArgs) Handles mnuShowCode.Click
        Dim frm As New frmTextViewer
        frm.SetText(Me.WebBrowser1.Document.Body.InnerHtml)
        frm.Show()

        Dim items As System.Windows.Forms.HtmlWindowCollection = Me.WebBrowser1.Document.Window.Frames
        For i As Integer = 0 To items.Count - 1
            Me.analizaFrames(items(i), 0)
        Next
    End Sub

    Private Sub analizaFrames(ByVal frame As System.Windows.Forms.HtmlWindow, ByVal level As Integer)
        Dim space As String = ""
        For i As Integer = 1 To level
            space &= "   "
        Next
        Debug.Print(space & "<frame name=" & frame.Name & ">" & vbNewLine)
        Dim inputs As System.Windows.Forms.HtmlElementCollection = frame.Document.GetElementsByTagName("INPUT")
        For i As Integer = 0 To inputs.Count - 1
            Debug.Print(space & "<input id=" & inputs(i).Id & " name=" & inputs(i).Name & " />" & vbNewLine)
        Next
        Dim iframes As System.Windows.Forms.HtmlWindowCollection = frame.Document.Window.Frames
        For i As Integer = 0 To iframes.Count - 1
            Me.analizaFrames(iframes(i), level + 1)
        Next
        Debug.Print(space & "</frame>" & vbNewLine)
    End Sub
End Class
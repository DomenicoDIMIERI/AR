Imports DMD.Databases

Partial Class WebSite

    Public Class CCurrentPage
        Inherits VisitedPage

        ' Private m_LogTime As New StatsItem
        Private m_Unhautorized As Boolean = False

        Public Sub New()


        End Sub



        ''' <summary>
        ''' Incrementa di 1 il numero di visite alla pagina e restituisce il valore aggiornato 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function IncreaseThisPageVisits() As Integer
            Return VisitedPages.IncreasePageVisits(Me.PageName)
        End Function


        Public Sub SaveLog()
            ' Dim posted As String = Me.PostedData
            ' Dim getted As String = Me.QueryString
            Me.Save(True)
        End Sub

        ''' <summary>
        ''' Metodo che indica l'inizio di una richiesta al sito
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub StartExecution()

        End Sub

        ''' <summary>
        ''' Metodo che indica la fine dell'esecuzione della pagina
        ''' </summary>
        ''' <param name="statusCode"></param>
        ''' <param name="statusDescription"></param>
        ''' <remarks></remarks>
        Public Sub EndExecution(ByVal statusCode As String, ByVal statusDescription As String)
            If Me.m_Unhautorized Then Exit Sub

            Me.StatusCode = statusCode
            Me.StatusDescription = statusDescription
            Me.ExecTime = (Now - Me.Data).TotalMilliseconds / 1000 ' Me.m_LogTime.ExecTime

            'Me.SaveLog()
        End Sub

        ''' <summary>
        ''' Metodo che indica alla pagina corrente che subito dopo verrà invocato il metodo Server.TransferTo
        ''' </summary>
        ''' <param name="url"></param>
        ''' <remarks></remarks>
        Public Sub TransferredTo(ByVal url As String)
            If Me.m_Unhautorized Then Exit Sub

            Me.StatusCode = "200"
            Me.StatusDescription = "Esecuzione trasferita a """ & url & """ "

            Me.SaveLog()
        End Sub


        Public Sub NotifyUnhautorized(msg As String)
            Me.m_Unhautorized = True
            Me.StatusCode = "200.403"
            Me.StatusDescription = msg

            Me.ExecTime = (Now - Me.Data).TotalMilliseconds / 1000 ' Me.m_LogTime.ExecTime
            Me.Save(True)

            Me.SetChanged(False)
        End Sub

        Friend Sub Initialize(ByVal page As System.Web.UI.Page)
            Me.Data = Now
            ' Me.m_LogTime.LastRun = Me.Data

            Me.SetSession(WebSite.Instance.CurrentSession)
            Me.SetUser(DMD.Sistema.Users.CurrentUser)

            Me.Secure = (UCase(page.Request.ServerVariables("HTTPS")) = "ON")
            Me.PageName = page.Request.ServerVariables("SCRIPT_NAME")
            Me.Referrer = page.Request.ServerVariables("HTTP_REFERER")
            Me.QueryString = page.Request.QueryString.ToString
            'Me.PostedData = Request.Form.ToString 
        End Sub

    End Class


End Class

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    Public NotInheritable Class CVisitedPagesClass
        Inherits CGeneralClass(Of VisitedPage)

        Friend Sub New()
            MyBase.New("modVisitedPages", GetType(VisitedPagesCursor), 0)
        End Sub

        Public Function ChangeQueryString(ByVal qs As String, ByVal keyName As String, ByVal keyValue As String) As String
            Dim items() As String
            Dim iName As String
            Dim i, p As Integer
            Dim trovato As Boolean

            trovato = False
            items = Split(qs, "&")
            For i = LBound(items) To UBound(items)
                p = InStr(items(i), "=")
                If (p > 0) Then
                    iName = Trim(Left(items(i), p - 1))
                    If LCase(iName) = LCase(Trim(keyName)) Then
                        trovato = True
                        items(i) = keyName & "=" & keyValue
                        Exit For
                    End If
                End If
            Next

            If Not trovato Then
                ReDim Preserve items(1 + UBound(items))
                items(UBound(items)) = keyName & "=" & keyValue
            End If

            Return Join(items, "&")
        End Function

        Public Function GetQueryStringValue(ByVal qs As String, ByVal keyName As String) As String
            Dim items() As String
            Dim iName, retVal As String
            Dim i, p As Integer
            Dim trovato As Boolean

            trovato = False
            items = Split(qs, "&")
            retVal = ""
            For i = LBound(items) To UBound(items)
                p = InStr(items(i), "=")
                If (p > 0) Then
                    iName = Trim(Left(items(i), p - 1))
                    If LCase(iName) = LCase(Trim(keyName)) Then
                        trovato = True
                        retVal = Mid(items(i), p + 1)
                        Exit For
                    End If
                End If
            Next

            Return retVal
        End Function


        Public Function CountPageVisits(ByVal pageURL As String) As Integer
            Dim dbRis As System.Data.IDataReader
            Dim cnt As Integer
            'Lanciamo la query per recuperare il numero di visite alla pagina specificata
            dbRis = APPConn.ExecuteReader("SELECT [visite] FROM [TabContatore] WHERE [pagina]=" & DBUtils.DBString(pageURL) & "")
            If dbRis.Read = False Then
                cnt = 0
            Else
                cnt = Formats.ToInteger(dbRis("visite"))
            End If
            dbRis.Dispose()
            dbRis = Nothing
            Return cnt
        End Function


        ''' <summary>
        ''' Restituisce il numero di visite effettuate alla pagina corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CountThisPageVisits() As Integer
            Return CountPageVisits(WebSite.ASP_Request.ServerVariables("SCRIPT_NAME"))
        End Function

        ''' <summary>
        ''' Incrementa di 1 il numero di visite alla pagina e restituisce il valore aggiornato 
        ''' </summary>
        ''' <param name="pageURL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IncreasePageVisits(ByVal pageURL As String) As Integer
            Dim dbSQL As String
            Dim cnt As Integer
            ' Incrementiamo il numero di visite.
            ' Risolve potenziali problemi dovuti alla sincronizzazione 
            cnt = CountPageVisits(pageURL)
            If (cnt = 0) Then
                dbSQL = "INSERT INTO [TabContatore] ([pagina], [visite]) VALUES (" & DBUtils.DBString(pageURL) & ", 1)"
                cnt = 1
            Else
                cnt = cnt + 1
                dbSQL = "UPDATE [TabContatore] SET [visite]=" & cnt & " WHERE [pagina] = " & DBUtils.DBString(pageURL) & ""
            End If

            LOGConn.ExecuteCommand(dbSQL)

            Return cnt
        End Function

    End Class


    Private Shared m_VisitedPages As CVisitedPagesClass = Nothing

    Public Shared ReadOnly Property VisitedPages As CVisitedPagesClass
        Get
            If (m_VisitedPages Is Nothing) Then m_VisitedPages = New CVisitedPagesClass
            Return m_VisitedPages
        End Get
    End Property


End Class
 
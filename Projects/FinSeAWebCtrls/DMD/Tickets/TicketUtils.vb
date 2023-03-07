Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Partial Class Utils

        Public NotInheritable Class CTicketUtilsClass
            Friend Sub New()
            End Sub

            Public Function CreateElencoCategorie(ByVal selValue As String) As String
                selValue = Trim(selValue & "")
                Dim dbSQL As String = "SELECT [Categoria] FROM [tbl_SupportTicketsCat] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " GROUP BY [Categoria] ORDER BY [Categoria] ASC"
                Dim dbRis As System.Data.IDataReader = Office.Database.ExecuteReader(dbSQL)
                Dim html As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                t1 = False
                While dbRis.Read
                    Dim cat As String = Formats.ToString(dbRis("Categoria"))
                    t = cat = selValue
                    t1 = t1 OrElse t
                    If (cat <> "") Then html.Append("<option value=""" & Strings.HtmlEncode(cat) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(cat) & "</option>")
                End While
                dbRis.Dispose()
                If (selValue <> "" AndAlso Not t1) Then html.Append("<option value=""" & Strings.HtmlEncode(selValue) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue) & "</option>")
                Return html.ToString
            End Function

            Public Function CreateElencoSottocategoria(ByVal categoria As String, ByVal selValue As String) As String
                categoria = Trim(categoria & "")
                selValue = Trim(selValue & "")

                Dim dbSQL As String
                If (categoria = "") Then
                    dbSQL = "SELECT [Sottocategoria] FROM [tbl_SupportTicketsCat] WHERE ([Categoria]='' Or [Categoria] Is NULL) AND [Stato]=" & ObjectStatus.OBJECT_VALID & " GROUP BY [Sottocategoria] ORDER BY [Sottocategoria] ASC"
                Else
                    dbSQL = "SELECT [Sottocategoria] FROM [tbl_SupportTicketsCat] WHERE [Categoria]=" & DBUtils.DBString(categoria) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " GROUP BY [Sottocategoria] ORDER BY [Sottocategoria] ASC"
                End If
                Dim dbRis As System.Data.IDataReader = Office.Database.ExecuteReader(dbSQL)
                Dim html As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                t1 = False
                While dbRis.Read
                    Dim cat As String = Formats.ToString(dbRis("Sottocategoria"))
                    t = cat = selValue
                    t1 = t1 OrElse t
                    If (cat <> "") Then html.Append("<option value=""" & Strings.HtmlEncode(cat) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(cat) & "</option>")
                End While
                dbRis.Dispose()
                If (selValue <> "" AndAlso Not t1) Then html.Append("<option value=""" & Strings.HtmlEncode(selValue) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue) & "</option>")
                Return html.ToString
            End Function


        End Class

        Private Shared m_TicketUtils As CTicketUtilsClass = Nothing

        Public Shared ReadOnly Property TicketUtils As CTicketUtilsClass
            Get
                If (m_TicketUtils Is Nothing) Then m_TicketUtils = New CTicketUtilsClass
                Return m_TicketUtils
            End Get
        End Property

    End Class
End Namespace
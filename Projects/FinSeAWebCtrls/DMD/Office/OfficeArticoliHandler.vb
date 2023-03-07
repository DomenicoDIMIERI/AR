Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class OfficeArticoliHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New ArticoloCursor
        End Function



        Public Function Find(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Return XML.Utils.Serializer.Serialize(Office.Articoli.Find(text))
        End Function

        Public Function GetNomiMarche(ByVal renderer As Object) As String
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                WebSite.ASP_Server.ScriptTimeout = 15

                Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "_q", "")))
                If (Len(text) < 3) Then Return ""

                Dim dbSQL As String = Strings.JoinW("SELECT [Marca] FROM [tbl_OfficeArticoli] WHERE [Stato]=1 AND [Marca] LIKE '", Replace(text, "'", "''"), "%' GROUP BY [Marca]")
                dbRis = Office.Database.ExecuteReader(dbSQL)

                Dim ret As New System.Text.StringBuilder
                ret.Append("<list>")
                While dbRis.Read
                    Dim nome As String = Formats.ToString(dbRis("Marca"))
                    ret.Append("<item>")
                    ret.Append("<text>")
                    ret.Append(Strings.HtmlEncode(nome))
                    ret.Append("</text>")
                    ret.Append("<value>")
                    ret.Append(Strings.HtmlEncode(nome))
                    ret.Append("</value>")
                    ret.Append("</item>")
                End While
                ret.Append("</list>")

                Return ret.ToString
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetNomiModelli(ByVal renderer As Object) As String
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                WebSite.ASP_Server.ScriptTimeout = 15

                Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "_q", "")))
                If (Len(text) < 3) Then Return ""

                Dim nomeMarca As String = Trim(RPC.n2str(GetParameter(renderer, "_m", "")))
                Dim dbSQL As String = Strings.JoinW("SELECT [Modello] FROM [tbl_OfficeArticoli] WHERE [Marca]=", DBUtils.DBString(nomeMarca), " AND [Stato]=1 AND [Modello] LIKE '", Replace(text, "'", "''"), "%' GROUP BY [Modello]")

                dbRis = Office.Database.ExecuteReader(dbSQL)

                Dim ret As New System.Text.StringBuilder
                ret.Append("<list>")
                While dbRis.Read
                    Dim nome As String = Formats.ToString(dbRis("Modello"))
                    ret.Append("<item>")
                    ret.Append("<text>")
                    ret.Append(Strings.HtmlEncode(nome))
                    ret.Append("</text>")
                    ret.Append("<value>")
                    ret.Append(Strings.HtmlEncode(nome))
                    ret.Append("</value>")
                    ret.Append("</item>")
                End While
                ret.Append("</list>")

                Return ret.ToString
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function
    End Class


End Namespace
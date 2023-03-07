Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class OggettiInventariatiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New OggettoInventariatoCursor
        End Function


        Public Function Find(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Return XML.Utils.Serializer.Serialize(Office.OggettiInventariati.Find(text))
        End Function

        Public Function GetNomeReparto(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "_q", "")))
            If (Len(text) < 3) Then Return ""

            Dim dbSQL As String = "SELECT [CodiceReparto] FROM [tbl_OfficeOggettiInventariati] WHERE [Stato]=1 AND [CodiceReparto] LIKE '" & Replace(text, "'", "''") & "%' GROUP BY [CodiceReparto]"
            Dim dbRis As System.Data.IDataReader = Office.Database.ExecuteReader(dbSQL)

            Dim ret As String
            ret = "<list>"
            While dbRis.Read
                Dim nome As String = Formats.ToString(dbRis("CodiceReparto"))
                ret &= "<item>"
                ret &= "<text>" & Strings.HtmlEncode(nome) & "</text>"
                ret &= "<value>" & Strings.HtmlEncode(nome) & "</value>"
                ret &= "</item>"
            End While
            dbRis.Dispose()
            ret = ret & "</list>"
            Return ret
        End Function

        Public Function GetNomeScaffale(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "_q", "")))
            If (Len(text) < 3) Then Return ""

            Dim dbSQL As String = "SELECT [CodiceScaffale] FROM [tbl_OfficeOggettiInventariati] WHERE [Stato]=1 AND [CodiceScaffale] LIKE '" & Replace(text, "'", "''") & "%' GROUP BY [CodiceScaffale]"
            Dim dbRis As System.Data.IDataReader = Office.Database.ExecuteReader(dbSQL)

            Dim ret As String
            ret = "<list>"
            While dbRis.Read
                Dim nome As String = Formats.ToString(dbRis("CodiceScaffale"))
                ret &= "<item>"
                ret &= "<text>" & Strings.HtmlEncode(nome) & "</text>"
                ret &= "<value>" & Strings.HtmlEncode(nome) & "</value>"
                ret &= "</item>"
            End While
            dbRis.Dispose()
            ret = ret & "</list>"
            Return ret
        End Function
    End Class


End Namespace
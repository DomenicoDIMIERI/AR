Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Namespace Forms

    Public Class NazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CNazioniCursor
            ret.Nome.SortOrder = SortEnum.SORT_ASC
            Return ret
        End Function


        Public Function GetNomeByCodCatasto(ByVal renderer As Object) As String
            Dim cod As String = RPC.str2n(Me.GetParameter(renderer, "cod", vbNullString))
            Dim cursor As New CNazioniCursor
            Dim ret As String = vbNullString
            cod = Trim(cod)
            If (cod = vbNullString) Then Return vbNullString
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.CodiceCatasto.Value = cod
            If Not cursor.EOF Then ret = cursor.Item.Nome
            cursor.Dispose()
            Return ret
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NumeroAbitanti", "Numero Abitanti", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("NomeAbitanti", "Nome Abitanti", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("SantoPatrono", "Santo Patrono", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("GiornoFestivo", "Giorno Festivo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Prefisso", "Prefisso", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CodiceCatasto", "Codice Catasto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CodiceISTAT", "Codice ISTAT", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Sigla", "Sigla", TypeCode.String, True))
            Return ret
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Luoghi.Nazioni.GetItemById(id)
        End Function

        Public Function GetItemByName(ByVal renderer As Object) As String
            Dim value As String = Trim(RPC.n2str(GetParameter(renderer, "name", "")))
            If (value = "") Then Return ""
            Dim item As CNazione = Luoghi.Nazioni.GetItemByName(value)
            If (item Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(item)
        End Function


    End Class


End Namespace
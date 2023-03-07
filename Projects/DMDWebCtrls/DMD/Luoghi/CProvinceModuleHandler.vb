Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Namespace Forms
 

    Public Class CProvinceModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CProvinceCursor
            ret.Nome.SortOrder = SortEnum.SORT_ASC
            Return ret
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Luoghi.Province.GetItemById(id)
        End Function

        Public Function GetItemByName(ByVal renderer As Object) As String
            Dim value As String = Trim(RPC.n2str(GetParameter(renderer, "name", "")))
            If (value = "") Then Return ""
            Dim item As CProvincia = Luoghi.Province.GetItemByName(value)
            If (item Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(item)
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Sigla", "Sigla", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Regione", "Regione", TypeCode.String, False))
            'ret.Add(New ExportableColumnInfo("NomeAbitanti", "NomeAbitanti", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NumeroAbitanti", "NumeroAbitanti", TypeCode.Int32, True))
            'ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            Return ret
        End Function

        Public Function CreateElencoNomiProvince(ByVal renderer As Object) As String
            Dim selValue As String = RPC.n2str(GetParameter(renderer, "s", ""))
            Return Strings.HtmlEncode(LuoghiUtils.CreateElencoNomiProvince(selValue))
        End Function

        Public Function CreateElencoSigleProvince(ByVal renderer As Object) As String
            Dim selValue As String = RPC.n2str(GetParameter(renderer, "s", ""))
            Return Strings.HtmlEncode(LuoghiUtils.CreateElencoSigleProvince(selValue))
        End Function

    End Class

   

  
End Namespace
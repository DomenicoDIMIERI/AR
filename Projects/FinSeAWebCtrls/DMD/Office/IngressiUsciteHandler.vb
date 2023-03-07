Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Office



Namespace Forms


    Public Class IngressiUsciteHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New MarcatureIngressoUscitaCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.Marcature.GetItemById(id)
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As New CCollection(Of ExportableColumnInfo)
            ret.Add(New ExportableColumnInfo("colOperatore", "Operatore", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("colUfficio", "Ufficio", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("colReparto", "Reparto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("colData", "Data", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("colIO", "Ingresso/Uscita", TypeCode.String, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(renderer As Object, item As Object, key As String) As Object
            Dim io As MarcaturaIngressoUscita = item
            Select Case key
                Case "colOperatore" : Return io.NomeOperatore
                Case "colUfficio" : Return io.NomePuntoOperativo
                Case "colReparto" : Return io.NomeReparto
                Case "colData" : Return io.Data
                Case "colIO" : Return CStr(IIf(io.Operazione = TipoMarcaturaIO.INGRESSO, "Ingresso", "Uscita"))
                Case Else : Return MyBase.GetColumnValue(renderer, item, key)
            End Select
        End Function

        Protected Overrides Sub SetColumnValue(renderer As Object, item As Object, key As String, value As Object)
            Dim io As MarcaturaIngressoUscita = item
            Select Case key
                Case "colOperatore" : io.Operatore = Sistema.Users.GetItemByName(CStr(value))
                Case "colUfficio" : io.PuntoOperativo = Anagrafica.Uffici.GetItemByName(CStr(value))
                Case "colReparto" : io.NomeReparto = CStr(value)
                Case "colData" : io.Data = CDate(value)
                Case "colIO" : io.Operazione = IIf(LCase(Trim(CStr(value))) = "ingresso", TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                Case Else : MyBase.SetColumnValue(renderer, item, key, value)
            End Select
        End Sub

    End Class



End Namespace
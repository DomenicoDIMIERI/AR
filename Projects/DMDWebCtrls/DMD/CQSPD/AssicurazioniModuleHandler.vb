Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class AssicurazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(Assicurazioni.Module, ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CAssicurazioniCursor
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String))
            ret.Add(New ExportableColumnInfo("Descrizione", "Descrizione", TypeCode.String))
            ret.Add(New ExportableColumnInfo("MeseScattoEta", "Mese scatto età", TypeCode.Int32))
            ret.Add(New ExportableColumnInfo("MeseScattoAnzianita", "Mese scatto anzianità", TypeCode.Int32))
            Return ret
        End Function


    End Class
End Namespace
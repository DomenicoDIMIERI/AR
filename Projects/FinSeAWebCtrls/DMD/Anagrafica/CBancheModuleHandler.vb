Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils


Imports DMD.Web

Namespace Forms

    Public Class CBancheModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CBancheCursor
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Descrizione", "Istituto", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("Filiale", "Filiale", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("ABI", "ABI", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CAB", "CAB", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Indirizzo", "Indirizzo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DataApertura", "DataApertura", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("DataChiusura", "DataChiusura", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("Attiva", "Attiva", TypeCode.Boolean, True))
            Return ret
        End Function

    End Class

End Namespace
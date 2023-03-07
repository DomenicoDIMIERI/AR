Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms

 
    Public Class FontiContattoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CFontiCursor
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Tipo", "Tipo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IDCampagna", "IDCampagna", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IDAnnuncio", "IDAnnuncio", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IDKeyWord", "IDKeyword", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DataInizio", "DataInizio", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("DataFine", "DataFine", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("Attiva", "Attiva", TypeCode.Boolean, True))
            Return ret
        End Function

    End Class


End Namespace
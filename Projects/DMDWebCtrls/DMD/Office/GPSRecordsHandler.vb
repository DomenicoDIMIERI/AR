Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
 
    Public Class GPSRecordsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New GPSRecordCursor
        End Function

        Public Function GetPosizioniDispositivo(ByVal renderer As Object) As MethodResults
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "id", ""))
            Dim daIstante As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "daIstante", ""))
            Dim aIstante As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "aIstante", ""))
            Return New MethodResults(Office.GPSRecords.GetPosizioniDispositivo(Office.Dispositivi.GetItemById(id), daIstante, aIstante))
        End Function
    End Class


End Namespace
Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms


    Public Class GruppiConsulenzeHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CQSPDStudiDiFattibilitaCursor
        End Function


        Public Function GetStudiDiFattibilitaByPersona(ByVal renderer As Object) As String
            Dim idPersona As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim items As CCollection(Of CQSPDStudioDiFattibilita) = DMD.CQSPD.StudiDiFattibilita.GetStudiDiFattibilitaByPersona(idPersona)
            Return XML.Utils.Serializer.Serialize(items)
        End Function

        Public Function GetUltimoStudioDiFattibilita(ByVal renderer As Object) As String
            Dim idPersona As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim s As CQSPDStudioDiFattibilita = CQSPD.StudiDiFattibilita.GetUltimoStudioDiFattibilita(idPersona)
            If (s Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(s)
        End Function

        Public Function GetSoluzioniXStudioDiFattibilita(ByVal renderer As Object) As String
            Dim gid As Integer = RPC.n2int(GetParameter(renderer, "gid", "0"))
            Dim g As CQSPDStudioDiFattibilita = CQSPD.StudiDiFattibilita.GetItemById(gid)
            Return XML.Utils.Serializer.Serialize(g.Proposte)
        End Function

        Public Function GetStudiDiFattibilitaByRichiesta(ByVal renderer As Object) As String
            Dim idRichiesta As Integer = RPC.n2int(GetParameter(renderer, "rid", ""))
            Dim items As CCollection(Of CQSPDStudioDiFattibilita) = DMD.CQSPD.StudiDiFattibilita.GetStudiDiFattibilitaByRichiesta(idRichiesta)
            Return XML.Utils.Serializer.Serialize(items)
        End Function
    End Class


End Namespace
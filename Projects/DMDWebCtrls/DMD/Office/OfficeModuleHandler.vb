Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms


    Public Class OfficeModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Function GetSituazionePersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim ret As New OfficeSituazionePersona(pid)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class

End Namespace
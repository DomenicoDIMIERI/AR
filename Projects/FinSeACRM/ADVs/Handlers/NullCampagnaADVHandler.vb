Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV


    Public Class NullCampagnaADVHandler
        Inherits HandlerTipoCampagna

        Public Overrides Function GetHandledType() As TipoCampagnaPubblicitaria
            Return TipoCampagnaPubblicitaria.NonImpostato
        End Function

        Public Overrides Function IsBanned(res As CRisultatoCampagna) As Boolean
            Return False
        End Function

        Public Overrides Function IsBlocked(res As CRisultatoCampagna) As Boolean
            Return False
        End Function

        Public Overrides Function GetNomeMezzoSpedizione() As String
            Return ""
        End Function

        Public Overrides Function PrepareResults(campagna As CCampagnaPubblicitaria, item As CPersona) As CCollection(Of CRisultatoCampagna)
            Return New CCollection(Of CRisultatoCampagna)
        End Function

        Public Overrides Sub Send(item As CRisultatoCampagna)

        End Sub


        Public Overrides Function SupportaConfermaLettura() As Boolean
            Return False
        End Function

        Public Overrides Function SupportaConfermaRecapito() As Boolean
            Return False
        End Function

        Public Overrides Sub UpdateStatus(res As CRisultatoCampagna)

        End Sub

        Protected Overrides Sub ParseAddress(str As String, ByRef nome As String, ByRef address As String)
            address = str
            nome = str
        End Sub

        Protected Overrides Function IsValidAddress(ByRef address As String) As Boolean
            Return True
        End Function
    End Class

End Class
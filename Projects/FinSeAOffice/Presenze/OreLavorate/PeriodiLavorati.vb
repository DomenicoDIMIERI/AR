Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CPeriodiLavorati
        Inherits CGeneralClass(Of PeriodoLavorato)

        Friend Sub New()
            MyBase.New("modOfficePeriodiLavorati", GetType(PeriodoLavoratoCursor), 0)
        End Sub


    End Class
End Namespace

Partial Class Office

    Private Shared m_PeriodiLavorati As CPeriodiLavorati = Nothing

    Public Shared ReadOnly Property PeriodiLavorati As CPeriodiLavorati
        Get
            If (m_PeriodiLavorati Is Nothing) Then m_PeriodiLavorati = New CPeriodiLavorati
            Return m_PeriodiLavorati
        End Get
    End Property

End Class
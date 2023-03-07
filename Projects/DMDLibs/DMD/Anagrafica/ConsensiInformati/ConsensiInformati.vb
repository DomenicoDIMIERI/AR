Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals


    Public NotInheritable Class CConsensiInformatiClass
        Inherits CGeneralClass(Of ConsensoInformato)

        Friend Sub New()
            MyBase.New("modConsensiInformati", GetType(ConsensoInformatoCursor), 0)
        End Sub

        Public Function GetConsensiByPersona(ByVal persona As CPersona) As ConsensoInformatoColleciton
            Return New ConsensoInformatoColleciton(persona)
        End Function

        Public Function GetConsensiByPersona(ByVal idPersona As Integer) As ConsensoInformatoColleciton
            Return Me.GetConsensiByPersona(Anagrafica.Persone.GetItemById(idPersona))
        End Function

    End Class

End Namespace


Partial Public Class Anagrafica



    Private Shared m_ConsensiInformati As CConsensiInformatiClass = Nothing

    Public Shared ReadOnly Property ConsensiInformati As CConsensiInformatiClass
        Get
            If (m_ConsensiInformati Is Nothing) Then m_ConsensiInformati = New CConsensiInformatiClass
            Return m_ConsensiInformati
        End Get
    End Property



End Class
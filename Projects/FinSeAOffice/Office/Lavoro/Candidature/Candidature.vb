Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    Public NotInheritable Class CCandidatureClass
        Inherits CGeneralClass(Of Candidatura)

        Friend Sub New()
            MyBase.New("modOfficeCandidature", GetType(CandidaturaCursor))
        End Sub

    End Class

    Private Shared m_Candidature As CCandidatureClass = Nothing

    Public Shared ReadOnly Property Candidature As CCandidatureClass
        Get
            If (m_Candidature Is Nothing) Then m_Candidature = New CCandidatureClass
            Return m_Candidature
        End Get
    End Property


End Class
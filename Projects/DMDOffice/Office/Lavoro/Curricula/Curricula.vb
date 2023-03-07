Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    Public NotInheritable Class CCurriculaClass
        Inherits CGeneralClass(Of Curriculum)

        Friend Sub New()
            MyBase.New("modOfficeCurricula", GetType(CurriculumCursor))
        End Sub

    End Class

    Private Shared m_Curricula As CCurriculaClass = Nothing

    Public Shared ReadOnly Property Curricula As CCurriculaClass
        Get
            If (m_Curricula Is Nothing) Then m_Curricula = New CCurriculaClass
            Return m_Curricula
        End Get
    End Property


End Class
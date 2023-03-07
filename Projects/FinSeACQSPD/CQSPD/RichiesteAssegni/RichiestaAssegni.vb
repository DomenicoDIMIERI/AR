Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class CQSPD

    Public NotInheritable Class CRichiesteAssegniClass
        Inherits CGeneralClass(Of CRichiestaAssegni)

        Friend Sub New()
            MyBase.New("RichiesteAssegni", GetType(CRichiestaAssegniCursor))
        End Sub


    End Class

    Private Shared m_RichiestaAssegni As CRichiesteAssegniClass = Nothing

    Public Shared ReadOnly Property RichiesteAssegni As CRichiesteAssegniClass
        Get
            If (m_RichiestaAssegni Is Nothing) Then m_RichiestaAssegni = New CRichiesteAssegniClass
            Return m_RichiestaAssegni
        End Get
    End Property


End Class
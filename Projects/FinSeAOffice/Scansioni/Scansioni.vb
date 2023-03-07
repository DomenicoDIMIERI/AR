Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CScansioniClass
        Inherits CGeneralClass(Of Scansione)

        Friend Sub New()
            MyBase.New("modOfficeScansioni", GetType(ScansioneCursor), 0)
        End Sub

         
    End Class

End Namespace

Partial Class Office

 

    Private Shared m_Scansioni As CScansioniClass = Nothing

    Public Shared ReadOnly Property Scansioni As CScansioniClass
        Get
            If (m_Scansioni Is Nothing) Then m_Scansioni = New CScansioniClass
            Return m_Scansioni
        End Get
    End Property

End Class
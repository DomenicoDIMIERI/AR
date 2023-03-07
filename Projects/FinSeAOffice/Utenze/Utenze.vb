Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD
Imports DMD.Office
Imports DMD.Internals

Namespace Internals

    Public Class CUtenzeClass
        Inherits CGeneralClass(Of Utenza)

        Public Sub New()
            MyBase.New("modUtenze", GetType(UtenzeCursor), 0)
        End Sub

    End Class


End Namespace

Partial Class Office

    Private Shared m_Utenze As CUtenzeClass = Nothing

    Public Shared ReadOnly Property Utenze As CUtenzeClass
        Get
            If (m_Utenze Is Nothing) Then
                m_Utenze = New CUtenzeClass
            End If
            Return m_Utenze
        End Get
    End Property
End Class

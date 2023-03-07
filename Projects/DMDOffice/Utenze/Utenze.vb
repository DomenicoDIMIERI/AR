Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Office

Namespace Internals

    Public Class UtenzeClass
        Inherits CGeneralClass(Of Utenza)

        Public Sub New()
            MyBase.New("modOfficeUtenze", GetType(UtenzeCursor), 0)
        End Sub

    End Class


End Namespace

Partial Class Office

    Private Shared m_Utenze As UtenzeClass = Nothing

    Public Shared ReadOnly Property Utenze As UtenzeClass
        Get
            If m_Utenze Is Nothing Then m_Utenze = New UtenzeClass
            Return m_Utenze
        End Get
    End Property


End Class
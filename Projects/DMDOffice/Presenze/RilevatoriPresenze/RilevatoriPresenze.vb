Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.Office

Namespace Internals

    Public NotInheritable Class CRilevatoriPresenzeClass
        Inherits CGeneralClass(Of RilevatorePresenze)

        Private m_Drivers As DriversRilevatoriPresenze

        Friend Sub New()
            MyBase.New("modOfficeRilevatoriPresenze", GetType(RilevatoriPresenzeCursor), -1)
        End Sub

        Public ReadOnly Property Drivers As DriversRilevatoriPresenze
            Get
                If (Me.m_Drivers Is Nothing) Then Me.m_Drivers = New DriversRilevatoriPresenze
                Return Me.m_Drivers
            End Get
        End Property

    End Class

End Namespace

Partial Class Office


    Private Shared m_RilevatoriPresenze As CRilevatoriPresenzeClass = Nothing

    Public Shared ReadOnly Property RilevatoriPresenze As CRilevatoriPresenzeClass
        Get
            If (m_RilevatoriPresenze Is Nothing) Then m_RilevatoriPresenze = New CRilevatoriPresenzeClass
            Return m_RilevatoriPresenze
        End Get
    End Property

End Class
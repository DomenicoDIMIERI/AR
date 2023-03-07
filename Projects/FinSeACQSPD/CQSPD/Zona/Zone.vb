Imports DMD.Databases

Imports DMD.Sistema

Partial Public Class CQSPD

    Public NotInheritable Class CZoneClass
        Inherits CGeneralClass(Of CZona)

        Friend Sub New()
            MyBase.New("modZoneGeografiche", GetType(CZonaCursor), -1)
        End Sub
 

    End Class

    Private Shared m_Zone As CZoneClass = Nothing

    Private Shared ReadOnly Property Zone As CZoneClass
        Get
            If (m_Zone Is Nothing) Then m_Zone = New CZoneClass
            Return m_Zone
        End Get
    End Property
End Class
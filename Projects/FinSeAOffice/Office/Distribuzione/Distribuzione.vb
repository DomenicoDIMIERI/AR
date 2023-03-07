Imports FinSeA.Anagrafica
Imports FinSeA.Databases
Imports FinSeA.Sistema

Partial Class Office


    Public NotInheritable Class CDistribuzioneClass
        Inherits CGeneralClass(Of DistribuzioneMateriale)

        Friend Sub New()
            MyBase.New("modOfficeDistribuzioneMat", GetType(DistribuzioneMaterialeCursor))
        End Sub


    End Class

    Private Shared m_Distribuzione As CDistribuzioneClass = Nothing

    Public Shared ReadOnly Property Distribuzione As CDistribuzioneClass
        Get
            If (m_Distribuzione Is Nothing) Then m_Distribuzione = New CDistribuzioneClass
            Return m_Distribuzione
        End Get
    End Property

End Class
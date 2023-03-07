Imports FinSeA.Anagrafica
Imports FinSeA.Databases
Imports FinSeA.Sistema
Imports FinSeA.Luoghi
Imports FinSeA.GPS

Partial Class Office

    ''' <summary>
    ''' Rappresenta una riga della tabella del materiale distribuito
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class RigheDistribuzioneCollection
        Inherits CCollection(Of RigaDistribuzione)

        Private m_Distribuzione As DistribuzioneMateriale
        
        Public Sub New()
            Me.m_Distribuzione = Nothing
        End Sub

        Public Sub New(ByVal distrib As DistribuzioneMateriale)
            Me.New()
            Me.Load(distrib)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Distribuzione IsNot Nothing) Then DirectCast(newValue, RigaDistribuzione).SetDistribuzione(Me.m_Distribuzione)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Distribuzione IsNot Nothing) Then DirectCast(value, RigaDistribuzione).SetDistribuzione(Me.m_Distribuzione)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Sub Load(ByVal distrib As DistribuzioneMateriale)
            If (distrib Is Nothing) Then Throw New ArgumentNullException("distrib")
            Me.Clear()
            Me.m_Distribuzione = distrib
            If (GetID(distrib) <> 0) Then
                'TO DO
            End If
        End Sub

    End Class


End Class
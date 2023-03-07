Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office



    <Serializable> _
    Public Class CommissioneEventArgs
        Inherits System.EventArgs

        Private m_Commissione As Commissione

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal comm As Commissione)
            Me.New()
            Me.m_Commissione = comm
        End Sub

        Public ReadOnly Property Commissione As Commissione
            Get
                Return Me.m_Commissione
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class
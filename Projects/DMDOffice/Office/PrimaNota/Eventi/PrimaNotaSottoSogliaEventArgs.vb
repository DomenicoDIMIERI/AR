Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office

     
        Public Class PrimaNotaSottoSogliaEventArgs
            Inherits System.EventArgs

            Private m_Ufficio As CUfficio
            Private m_Soglia As Decimal

        Public Sub New(ByVal ufficio As CUfficio, ByVal soglia As Decimal)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Ufficio = ufficio
            Me.m_Soglia = soglia
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property Ufficio As CUfficio
            Get
                Return Me.m_Ufficio
            End Get
        End Property

        Public ReadOnly Property Soglia As Decimal
                Get
                    Return Me.m_Soglia
                End Get
            End Property

        End Class
 

End Class
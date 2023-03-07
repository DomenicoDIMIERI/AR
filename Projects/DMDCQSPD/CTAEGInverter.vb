Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

       
      ''' <summary>
    ''' Calcola il valore del netto ricavo in funzione del TAEG
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTAEGInverter
        Private m_Rata As Decimal
        Private m_Durata As Integer
        Private m_TAEG As Double

        Public Sub New(Optional ByVal rata As Decimal = 0, Optional ByVal durata As Integer = 120, Optional ByVal taeg As Double = 0)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Rata = rata
            Me.m_Durata = 120
            Me.m_TAEG = taeg
        End Sub

        Public Property Rata As Decimal
            Get
                Return Me.m_Rata
            End Get
            Set(value As Decimal)
                Me.m_Rata = value
            End Set
        End Property

        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Me.m_Durata = value
            End Set
        End Property

        Public Property TAEG As Double
            Get
                Return Me.m_TAEG
            End Get
            Set(value As Double)
                Me.m_TAEG = value
            End Set
        End Property

        Public Function Calc() As Double
            Dim k As Integer
            Dim s As Double = 0
            For k = 1 To Me.Durata
                s = s + (1 + Me.TAEG / 100) ^ (-k / 12)
            Next
            Return Me.Rata * s
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class
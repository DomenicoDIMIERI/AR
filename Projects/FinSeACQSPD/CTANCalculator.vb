Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

       
    Public Class CTANCalculator
        Public max_iterations As Integer = 100

        Private m_Rata As Decimal
        Private m_Importo As Decimal 'ML - Interessi
        Private m_Durata As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Rata = 0
            Me.m_Durata = 120
            Me.m_Importo = 0
        End Sub

        Public Sub New(ByVal rata As Decimal, ByVal durata As Integer, ByVal importo As Decimal)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Rata = rata
            Me.m_Durata = durata
            Me.m_Importo = importo
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

        Public Property Importo As Decimal
            Get
                Return Me.m_Importo
            End Get
            Set(value As Decimal)
                Me.m_Importo = value
            End Set
        End Property

        Public Function Calc() As Double
            Dim c1C3, c1C4, c1C5 As Double
            c1C3 = Me.Rata
            c1C4 = Me.Importo
            c1C5 = Me.Durata
            If (c1C3 * c1C4 * c1C5 = 0) Then
                Return 0
            Else
                Return Me.Rate(c1C5, c1C3 * -1, c1C4, 0, 0, 0.1) * 12
            End If
        End Function

        Private Function Rate(ByVal nper As Double, ByVal pmt As Double, ByVal pv As Double, ByVal fv As Double, ByVal tipo As Boolean, ByVal guess As Double) As Double
            'If (Not isFinite(guess) Or Not isFinite(tipo) Or Not isFinite(fv) Or Not isFinite(pv) Or Not isFinite(pmt) Or Not isFinite(nper)) Then
            '    Rate = NaN 
            'End If
            Dim type2 As Integer
            Dim wanted_precision As Double
            Dim current_diff As Double
            Dim x, next_x, y, z As Double
            Dim iterations_done As Integer

            If tipo Then
                type2 = 1
            Else
                type2 = 0
            End If
            wanted_precision = 0.00000001
            current_diff = 999999999999999999 ' Number.MAX_VALUE 
            If (guess = 0) Then
                x = 0.1
            Else
                x = guess
            End If
            iterations_done = 0
            While (current_diff > wanted_precision) And (iterations_done < max_iterations)
                If (x = 0) Then
                    next_x = x - (pv + pmt * nper + fv) / (pv * nper + pmt * (nper * (nper - 1) + 2 * type2 * nper) / 2)
                Else
                    y = (1 + x) ^ (nper - 1)
                    z = y * (1 + x)
                    next_x = x * (1 - (x * pv * z + pmt * (1 + x * type2) * (z - 1) + x * fv) / (x * x * nper * pv * y - pmt * (z - 1) + x * pmt * (1 + x * type2) * nper * y))
                End If
                iterations_done = iterations_done + 1
                current_diff = Math.Abs(next_x - x)
                x = next_x
            End While
            If (guess = 0) And (Math.Abs(x) < wanted_precision) Then x = 0
            If (current_diff >= wanted_precision) Then
                Return 0 'Number.NaN;
            Else
                Return x
            End If
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class
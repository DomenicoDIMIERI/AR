Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CTEGFunEvaluator
        Inherits FunEvaluator

        Public Durata As Integer
        Public Quota As Decimal
        Public NettoRicavo As Decimal
        Public SpeseAssicurative As Decimal
        Public Imposte As Decimal

        Public Overrides Function EvalFunction(x As Double) As Double
            Dim s As Integer
            Dim ret As Double = 0
            For s = 1 To Me.Durata Step 1
                ret = ret + (CDbl(1) + x) ^ (-s / CDbl(12))
            Next
            ret = CDbl(ret) * Quota - (NettoRicavo + SpeseAssicurative + Imposte)
            Return ret
        End Function

    End Class


End Class
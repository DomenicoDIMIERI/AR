Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    Public Class IncomingCallEventArgs
        Inherits System.EventArgs

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class
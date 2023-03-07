Public Class SimpleTimeZone
    Inherits TimeZone

    Private _p1 As Integer
    Private _p2 As String

    Sub New(p1 As Integer, p2 As String)
        DMD.DMDObject.IncreaseCounter(Me)
        _p1 = p1
        _p2 = p2
    End Sub

    Sub setRawOffset(p1 As Integer)
        Throw New NotImplementedException
    End Sub

    Sub setID(p1 As String)
        Throw New NotImplementedException
    End Sub

    Function getID() As Object
        Throw New NotImplementedException
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
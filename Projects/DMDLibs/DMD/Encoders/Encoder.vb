Imports System
Imports System.Text
Imports System.IO

Partial Public Class Encoding

    Public MustInherit Class Encoder
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public MustOverride Function Decode(ByVal s() As Byte) As Byte()
        Public MustOverride Function Encode(ByVal s() As Byte) As Byte()

        Public Overridable Function Decode(ByVal s As String) As String
            Return System.Text.Encoding.Default.GetString(Me.Decode(System.Text.Encoding.Default.GetBytes(s)))
        End Function

        Public Overridable Function Encode(ByVal s As String) As String
            Return System.Text.Encoding.Default.GetString(Me.Encode(System.Text.Encoding.Default.GetBytes(s)))
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class
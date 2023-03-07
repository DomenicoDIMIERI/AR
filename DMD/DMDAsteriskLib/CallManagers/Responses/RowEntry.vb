Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Responses

    Public Class RowEntry
        Private m_Command As String
        Private m_Params As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal row As String)
            Me.New
            Me.Parse(row)
        End Sub

        Friend Overridable Sub Parse(ByVal row As String)
            Dim p As Integer = InStr(row, ":")
            If (p > 0) Then
                Me.m_Command = Left(row, p - 1)
                Me.m_Params = Mid(row, p + 2)
            Else
                Me.m_Command = ""
                Me.m_Params = row
            End If
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property Command As String
            Get
                Return Me.m_Command
            End Get
        End Property

        Public ReadOnly Property Params As String
            Get
                Return Me.m_Params
            End Get
        End Property

    End Class

End Namespace
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk

Namespace CallManagers.Responses

    Public Class GetVarResponse
        Inherits ActionResponse

        Private m_varname As Integer
        Private m_Value As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

        Public ReadOnly Property VarName As Integer
            Get
                Return Me.m_varname
            End Get
        End Property

        Public ReadOnly Property Value As String
            Get
                Return Me.m_Value
            End Get
        End Property

        Protected Overrides Sub ParseRow(row As RowEntry)
            Select Case UCase(row.Command)
                Case "VARNAME" : Me.m_varname = row.Params
                Case "VALUE" : Me.m_Value = row.Params
                Case Else : MyBase.ParseRow(row)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString() & "(Varname: " & Me.m_varname & ", Value: " & Me.m_Value & ")"
        End Function

    End Class

End Namespace
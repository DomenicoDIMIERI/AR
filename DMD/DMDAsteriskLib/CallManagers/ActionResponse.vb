Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class ActionResponse
        Private m_Action As Action
        Private m_Response As String
        Private m_Message As String
        Private m_ActionID As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal action As Action)
            Me.New
            Me.m_Action = action
        End Sub

        Public ReadOnly Property Action As Action
            Get
                Return Me.m_Action
            End Get
        End Property

        Public ReadOnly Property Response As String
            Get
                Return Me.m_Response
            End Get
        End Property

        Public ReadOnly Property Message As String
            Get
                Return Me.m_Message
            End Get
        End Property

        Public ReadOnly Property ActionID As String
            Get
                Return Me.m_ActionID
            End Get
        End Property

        Public Overridable Function IsSuccess() As Boolean
            Return Me.m_Response = "Success"
        End Function

        Public Overrides Function ToString() As String
            Dim ret As String = ""
            ret &= "Response: " & Me.Response & vbCrLf
            ret &= "Message: " & Me.Message & vbCrLf
            Return ret
        End Function

        Protected Friend Overridable Sub Parse(ByVal rows() As String)
            For Each r As String In rows
                Dim item As New RowEntry(r)
                Me.ParseRow(item)
            Next
        End Sub

        Protected Overridable Sub ParseRow(ByVal row As RowEntry)
            Select Case UCase(row.Command)
                Case "RESPONSE" : Me.m_Response = row.Params
                Case "MESSAGE" : Me.m_Message = row.Params
                Case "ACTIONID" : Me.m_ActionID = row.Params
                Case Else : Debug.Print("Unsupported response: " & row.Params)
            End Select
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
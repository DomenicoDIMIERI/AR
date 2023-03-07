Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk

Namespace CallManagers.Responses

    Public Class ExtensionStateResponse
        Inherits ActionResponse

        'Private m_ActionID As Integer
        Private m_Exten As String
        Private m_Context As String
        Private m_Hint As String
        Private m_Status As ast_extension_states

        Public Sub New()
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

        'Public ReadOnly Property ActionID As Integer
        '    Get
        '        Return Me.m_ActionID
        '    End Get
        'End Property

        Public ReadOnly Property Exten As String
            Get
                Return Me.m_Exten
            End Get
        End Property

        Public ReadOnly Property Context As String
            Get
                Return Me.m_Context
            End Get
        End Property

        Public ReadOnly Property Hint As String
            Get
                Return Me.m_Hint
            End Get
        End Property

        Public ReadOnly Property Status As ast_extension_states
            Get
                Return Me.m_Status
            End Get
        End Property

        Protected Overrides Sub ParseRow(row As RowEntry)
            Select Case UCase(row.Command)
                'Case "ACTIONID" : Me.m_ActionID = CInt(row.Params)
                Case "EXTEN" : Me.m_Exten = row.Params
                Case "CONTEXT" : Me.m_Context = row.Params
                Case "HINT" : Me.m_Hint = row.Params
                Case "STATUS" : Me.m_Status = CInt(row.Params)
                Case Else : MyBase.ParseRow(row)
            End Select
        End Sub

        'Public Overrides Function ToString() As String
        '    Return MyBase.ToString() & "(Context: " & Me.m_Context & ", Exten: " & Me.m_Exten & ", ActionID: " & Me.m_ActionID & ", Hint: " & Me.m_Hint & ", Status: " & Me.GetStatus(Me.m_Status) & ")"
        'End Function

        Private Function GetStatus(ByVal s As ast_extension_states) As String
            Dim ret As String = ""
            For Each f As ast_extension_states In [Enum].GetValues(GetType(ast_extension_states))
                If (s And f) = f Then
                    If ret <> "" Then ret &= ", "
                    ret &= [Enum].GetName(GetType(ast_extension_states), f)
                End If
            Next
            Return s
        End Function

    End Class

End Namespace
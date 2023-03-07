Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk

Namespace CallManagers.Responses

    Public Class MailboxCountResponse
        Inherits ActionResponse

        Private m_MailBox As String
        Private m_NewMessages As Integer
        Private m_OldMessages As Integer

        Public Sub New()
            Me.m_MailBox = ""
            Me.m_NewMessages = 0
            Me.m_OldMessages = 0
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

        Public ReadOnly Property MailBox As String
            Get
                Return Me.m_MailBox
            End Get
        End Property

        Public ReadOnly Property NewMessages As Integer
            Get
                Return Me.m_NewMessages
            End Get
        End Property

        Public ReadOnly Property OldMessages As Integer
            Get
                Return Me.m_OldMessages
            End Get
        End Property

        Protected Overrides Sub ParseRow(row As RowEntry)
            Select Case UCase(row.Command)
                Case "MAILBOX" : Me.m_MailBox = row.Params
                Case "NEWMESSAGES" : Me.m_NewMessages = CInt(row.Params)
                Case "OLDMESSAGES" : Me.m_NewMessages = CInt(row.Params)
                Case Else : MyBase.ParseRow(row)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString() & "(MailBox: " & Me.MailBox & ", Newmessages: " & Me.NewMessages & ", Oldmessages: " & Me.OldMessages & ")"
        End Function


    End Class

End Namespace
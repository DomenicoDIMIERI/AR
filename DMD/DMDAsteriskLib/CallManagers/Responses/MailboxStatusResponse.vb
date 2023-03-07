Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk

Namespace CallManagers.Responses

    Public Class MailboxStatusResponse
        Inherits ActionResponse

        Private m_MailBox As String
        Private m_Waiting As Integer

        Public Sub New()
            Me.m_MailBox = ""
            Me.m_Waiting = 0
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

        Public ReadOnly Property Waiting As Integer
            Get
                Return Me.m_Waiting
            End Get
        End Property

        Public ReadOnly Property MailBox As String
            Get
                Return Me.m_MailBox
            End Get
        End Property
         
        Protected Overrides Sub ParseRow(row As RowEntry)
            Select Case UCase(row.Command)
                Case "MAILBOX" : Me.m_MailBox = row.Params
                Case "WAITING" : Me.m_Waiting = CInt(row.Params)
                Case Else : MyBase.ParseRow(row)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString() & "(MailBox: " & Me.MailBox & ", Waiting: " & Me.Waiting & ")"
        End Function


    End Class

End Namespace
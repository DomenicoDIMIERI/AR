Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' Checks a voicemail account for status.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MailboxStatus
        Inherits Action

        Private m_Mailbox As String
        Private m_ActionID As Nullable(Of Integer)

        Public Sub New()
            MyBase.New("MailboxStatus")
        End Sub

        Public Sub New(ByVal mailbox As String)
            Me.New()
            Me.m_Mailbox = mailbox
        End Sub

        Public Sub New(ByVal mailbox As String, ByVal actionID As Integer)
            Me.New()
            Me.m_Mailbox = mailbox
            Me.m_ActionID = actionID
        End Sub

        Public ReadOnly Property Mailbox As String
            Get
                Return Me.m_Mailbox
            End Get
        End Property

        Public ReadOnly Property ActionID As Nullable(Of Integer)
            Get
                Return Me.m_ActionID
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            If (Me.m_ActionID.HasValue) Then
                Return MyBase.GetCommandText() & "Mailbox: " & Me.m_Mailbox & vbCrLf & "Actionid: " & Me.m_ActionID
            Else
                Return MyBase.GetCommandText() & "Mailbox: " & Me.m_Mailbox & vbCrLf
            End If
        End Function
         

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New MailboxStatusResponse(Me)
        End Function

        Public Overrides ReadOnly Property RequiredPrivilages As ActionPrivilageFlag
            Get
                Return ActionPrivilageFlag.call Or ActionPrivilageFlag.all
            End Get
        End Property

    End Class

End Namespace
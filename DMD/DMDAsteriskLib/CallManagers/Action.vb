Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class Action
        Private m_Owner As AsteriskCallManager
        Private m_ActionName As String
        Private m_Response As ActionResponse

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_ActionName = ""
            Me.m_Response = Nothing
        End Sub

        Public Sub New(ByVal actionName As String)
            Me.New
            Me.m_ActionName = Trim(actionName)
            Me.m_Response = Nothing
        End Sub

        Public ReadOnly Property ActionName As String
            Get
                Return Me.m_ActionName
            End Get
        End Property

        Public Overridable ReadOnly Property RequiredPrivilages As ActionPrivilageFlag
            Get
                Return ActionPrivilageFlag.none
            End Get
        End Property

        Protected Overridable Function GetCommandText() As String
            Dim ret As String = ""
            ret &= "Action: " & Me.m_ActionName & vbCrLf
            Return ret
        End Function

        Protected Friend Overridable Sub Send(ByVal manager As AsteriskCallManager)
            manager.Send(Me.GetCommandText)
        End Sub

        Public ReadOnly Property Response As ActionResponse
            Get
                Return Me.m_Response
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.GetCommandText
        End Function

        Public Overridable Function IsAsync() As Boolean
            Return False
        End Function

        Protected Overridable Function AllocateResponse(ByVal rows() As String) As ActionResponse
            Return New ActionResponse(Me)
        End Function


        Protected Friend Sub ParseResponse(ByVal rows() As String)
            Me.m_Response = Me.AllocateResponse(rows)
            Me.m_Response.Parse(rows)
        End Sub

        Public Overridable Function RequiresAuthentication() As Boolean
            Return True
        End Function

        Public ReadOnly Property Owner As AsteriskCallManager
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal owner As AsteriskCallManager)
            Me.m_Owner = owner
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' Redirect (transfer) a call
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Redirect
        Inherits AsyncAction

        Private m_Channel As String
        Private m_Exten As String
        Private m_Context As String
        Private m_Priority As Integer
        Private m_ExtraChannel As String
        Private m_ExtraExten As String
        Private m_ExtraContext As String
        Private m_ExtraPriority As Nullable(Of Integer)


        Public Sub New()
            MyBase.New("Redirect")
            Me.m_Channel = ""
            Me.m_Exten = ""
            Me.m_Context = ""
            Me.m_Priority = 1
            Me.m_ExtraChannel = ""
            Me.m_ExtraExten = ""
            Me.m_ExtraContext = ""
            Me.m_ExtraPriority = Nothing
        End Sub

        Public Sub New(ByVal channel As String, ByVal exten As String, ByVal context As String, ByVal priority As Integer, ByVal extraChannel As String)
            Me.New()
            Me.m_Channel = channel
            Me.m_ExtraChannel = extraChannel
            Me.m_Exten = exten
            Me.m_Context = context
            Me.m_Priority = priority
        End Sub

        Public Sub New(ByVal channel As String, ByVal exten As String, ByVal context As String, ByVal priority As Integer, ByVal extraChannel As String, ByVal extraExten As String, ByVal extraContext As String, ByVal extraPriority As Integer)
            Me.New(channel, exten, context, priority, extraChannel)
            Me.m_ExtraExten = extraExten
            Me.m_ExtraContext = extraContext
            Me.m_ExtraPriority = extraPriority
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

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

        Public ReadOnly Property Priority As Integer
            Get
                Return Me.m_Priority
            End Get
        End Property

        Public ReadOnly Property ExtraChannel As String
            Get
                Return Me.m_ExtraChannel
            End Get
        End Property

        Public ReadOnly Property ExtraExten As String
            Get
                Return Me.m_ExtraExten
            End Get
        End Property

        Public ReadOnly Property ExtraContext As String
            Get
                Return Me.m_ExtraContext
            End Get
        End Property

        Public ReadOnly Property ExtraPriority As Nullable(Of Integer)
            Get
                Return Me.m_ExtraPriority
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText()
            ret &= "Channel: " & Me.Channel & vbCrLf
            ret &= "ExtraChannel: " & Me.ExtraChannel & vbCrLf
            ret &= "Exten: " & Me.Exten & vbCrLf
            ret &= "Context: " & Me.Context & vbCrLf
            ret &= "Priority: " & Me.Priority & vbCrLf
            If (Me.ExtraExten <> "") Then ret &= "ExtraExten: " & Me.ExtraExten & vbCrLf
            If (Me.ExtraContext <> "") Then ret &= "ExtraContext: " & Me.ExtraContext & vbCrLf
            If (Me.ExtraPriority.HasValue) Then ret &= "ExtraPriority: " & Me.ExtraPriority & vbCrLf
            Return ret
        End Function

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New RedirectResponse(Me)
        End Function


    End Class

End Namespace
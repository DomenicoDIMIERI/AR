Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions


    ''' <summary>
    ''' Contol Event Flow: Enable/Disable sending of events to this manager client.
    ''' </summary>
    ''' <remarks>Will not work for built-in variables like LANGUAGE !</remarks>
    Public Class GetVar
        Inherits Action

        Private m_Channel As String
        Private m_Variable As String
        Private m_ActionID As Nullable(Of Integer)


        Public Sub New()
            MyBase.New("GetVar")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal channel As String, ByVal variable As String)
            Me.New()
            Me.m_Channel = channel
            Me.m_Variable = variable
        End Sub


        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal channel As String, ByVal variable As String, ByVal actionID As Integer)
            Me.New()
            Me.m_Channel = channel
            Me.m_Variable = variable
            Me.m_ActionID = actionID
        End Sub


        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

        Public ReadOnly Property Variable As String
            Get
                Return Me.m_Variable
            End Get
        End Property

        Public ReadOnly Property ActionID As Nullable(Of Integer)
            Get
                Return Me.m_ActionID
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            If (Me.ActionID.HasValue) Then
                Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf & "Variable: " & Me.Variable & vbCrLf & "ActionID: " & Me.ActionID & vbCrLf
            Else
                Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf & "Variable: " & Me.Variable & vbCrLf
            End If
        End Function

         


        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New GetVarResponse(Me)
        End Function

    End Class

End Namespace
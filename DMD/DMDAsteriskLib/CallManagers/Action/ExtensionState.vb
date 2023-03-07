Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions


    ''' <summary>
    ''' Contol Event Flow: Enable/Disable sending of events to this manager client.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ExtensionState
        Inherits Action

        Private m_Context As String
        Private m_Exten As String
        Private m_ActionID As Integer


        Public Sub New()
            MyBase.New("ExtensionState")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal exten As String, ByVal context As String, ByVal actionID As Integer)
            Me.New()
            Me.m_Context = context
            Me.m_Exten = exten
            Me.m_ActionID = actionID
        End Sub
 

        Public ReadOnly Property Context As String
            Get
                Return Me.m_Context
            End Get
        End Property

        Public ReadOnly Property Exten As String
            Get
                Return Me.m_Exten
            End Get
        End Property

        Public ReadOnly Property ActionID As Integer
            Get
                Return Me.m_ActionID
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "Context: " & Me.Context & vbCrLf & "Exten: " & Me.Exten & vbCrLf & "ActionID: " & Me.ActionID & vbCrLf
        End Function

         

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New ExtensionStateResponse(Me)
        End Function

    End Class

End Namespace
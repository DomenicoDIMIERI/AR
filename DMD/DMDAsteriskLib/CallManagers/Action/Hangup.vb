Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions


    ''' <summary>
    ''' Contol Event Flow: Enable/Disable sending of events to this manager client.
    ''' </summary>
    ''' <remarks>Will not work for built-in variables like LANGUAGE !</remarks>
    Public Class Hangup
        Inherits Action

        Private m_Channel As String
        

        Public Sub New()
            MyBase.New("Hangup")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal channel As String)
            Me.New()
            Me.m_Channel = channel
        End Sub
 

        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property
         
        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf
        End Function

         

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New HangupResponse(Me)
        End Function

    End Class

End Namespace
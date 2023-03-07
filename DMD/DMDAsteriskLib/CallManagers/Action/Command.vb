Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' Changes the file name of a recording occuring on a channel
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Command
        Inherits Action

        Private m_Command As String
        
        Public Sub New()
            MyBase.New("Command")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <param name="command">[in] Comando da inviare al CLI</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal command As String)
            Me.New()
            Me.m_Command = Trim(command)
        End Sub

        Public ReadOnly Property Command As String
            Get
                Return Me.m_Command
            End Get
        End Property
         

        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "command: " & Me.Command & vbCrLf
        End Function


        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New CommandResponse(Me)
        End Function

    End Class

End Namespace
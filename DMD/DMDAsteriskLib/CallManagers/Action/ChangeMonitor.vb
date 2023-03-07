Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' Changes the file name of a recording occuring on a channel
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeMonitor
        Inherits Action

        Private m_Channel As String
        Private m_File As String

        Public Sub New()
            MyBase.New("ChangeMonitor")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <param name="channel">[in] Which channel to hangup, e.g. SIP/123-1c20</param>
        ''' <param name="file">[in] File name</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal channel As String, ByVal file As String)
            Me.New()
            Me.m_Channel = Trim(channel)
            Me.m_File = Trim(file)
        End Sub

        ''' <summary>
        ''' Which channel to hangup, e.g. SIP/123-1c20
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

        ''' <summary>
        ''' File name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property File As String
            Get
                Return Me.m_File
            End Get
        End Property

    

        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf & "File: " & Me.File & vbCrLf
        End Function


        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New ChangeMonitorResponse(Me)
        End Function

    End Class

End Namespace
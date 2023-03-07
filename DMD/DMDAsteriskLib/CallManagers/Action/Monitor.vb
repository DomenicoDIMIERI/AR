Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class Monitor
        Inherits Action

        Private m_Channel As String
        Private m_File As String
        Private m_Format As String
        Private m_Mix As Integer

        Public Sub New()
            MyBase.New("Monitor")
        End Sub

        Public Sub New(ByVal channel As String, ByVal file As String, ByVal format As String, ByVal mix As Integer)
            Me.New()
            Me.m_Channel = channel
            Me.m_File = file
            Me.m_Format = format
            Me.m_Mix = mix
        End Sub

        Public Sub New(ByVal channel As String, ByVal file As String, ByVal mix As Integer)
            Me.New()
            Me.m_Channel = channel
            Me.m_File = file
            Me.m_Format = ""
            Me.m_Mix = mix
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

        Public ReadOnly Property File As String
            Get
                Return Me.m_File
            End Get
        End Property

        Public ReadOnly Property Format As String
            Get
                Return Me.m_Format
            End Get
        End Property

        Public ReadOnly Property Mix As Integer
            Get
                Return Me.m_Mix
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            If (Me.m_Format = "") Then
                Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf & "File: " & Me.File & vbCrLf & "Mix: " & Me.Mix
            Else
                Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf & "File: " & Me.File & vbCrLf & "Format: " & Me.Format & vbCrLf & "Mix: " & Me.Mix
            End If
        End Function

        

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New MonitorResponse(Me)
        End Function
         

    End Class

End Namespace
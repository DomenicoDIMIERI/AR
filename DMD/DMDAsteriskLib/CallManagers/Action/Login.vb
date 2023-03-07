Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' Effettua il login nel manager
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Login
        Inherits Action

        Private m_Username As String
        Private m_Secret As String

        Public Sub New()
            MyBase.New("Login")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal userName As String, ByVal secret As String)
            Me.New()
            Me.m_Username = userName
            Me.m_Secret = secret
        End Sub

        Public ReadOnly Property UserName As String
            Get
                Return Me.m_Username
            End Get
        End Property

        Public ReadOnly Property Secret As String
            Get
                Return Me.m_Secret
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "Username: " & Me.UserName & vbCrLf & "Secret: " & Me.Secret & vbCrLf
        End Function
         

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New LoginResponse(Me)
        End Function

        Public Overrides Function RequiresAuthentication() As Boolean
            Return False
        End Function
    End Class

End Namespace
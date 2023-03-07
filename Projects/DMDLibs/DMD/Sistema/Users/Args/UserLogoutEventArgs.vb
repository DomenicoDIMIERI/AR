Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Sistema


Public Class UserLogoutEventArgs
        Inherits UserLogEventArgs

        Private m_Method As LogOutMethods

        Public Sub New()
        End Sub

        Public Sub New(ByVal user As CUser, ByVal method As LogOutMethods, Optional ByVal params As String = vbNullString)
            MyBase.New(user, params)
            Me.m_Method = method
        End Sub

        Public ReadOnly Property Method As LogOutMethods
            Get
                Return Me.m_Method
            End Get
        End Property

    End Class

 
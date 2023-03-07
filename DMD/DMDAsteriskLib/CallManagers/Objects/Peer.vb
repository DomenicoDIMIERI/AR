Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class Peer
        Inherits AsteriskObject
        Private m_Name As String
        Private m_Status As String
        Private m_LastUpdated As Nullable(Of Date)

        Public Sub New()
            Me.m_Name = ""
            Me.m_Status = ""
            Me.m_LastUpdated = Nothing
        End Sub

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                Me.m_Name = Trim(value)
            End Set
        End Property

        Public Property Status As String
            Get
                Return Me.m_Status
            End Get
            Set(value As String)
                Me.m_Status = Trim(value)
            End Set
        End Property

        Public Property LastUpdated As Nullable(Of Date)
            Get
                Return Me.m_LastUpdated
            End Get
            Set(value As Nullable(Of Date))
                Me.m_LastUpdated = value
            End Set
        End Property




    End Class

End Namespace
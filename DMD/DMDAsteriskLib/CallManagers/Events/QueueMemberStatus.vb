Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class QueueMemberStatus
        Inherits AsteriskEvent
 

        Public Sub New()
            MyBase.New("QueueMemberStatus")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Queue As String
            Get
                Return Me.GetAttribute("Queue")
            End Get
        End Property

        Public ReadOnly Property Location As String
            Get
                Return Me.GetAttribute("Location")
            End Get
        End Property

        Public ReadOnly Property MemberName As String
            Get
                Return Me.GetAttribute("MemberName")
            End Get
        End Property

        Public ReadOnly Property Membership As String
            Get
                Return Me.GetAttribute("Membership")
            End Get
        End Property

        Public ReadOnly Property Penalty As Integer
            Get
                Return Me.GetAttribute("Penalty", 0)
            End Get
        End Property

        Public ReadOnly Property CallsTaken As Integer
            Get
                Return Me.GetAttribute("CallsTaken", 0)
            End Get
        End Property

        Public ReadOnly Property LastCall As Integer
            Get
                Return Me.GetAttribute("LastCall", 0)
            End Get
        End Property

        Public ReadOnly Property Status As QueueStatusFlags
            Get
                Return Me.GetAttribute("Status", 0)
            End Get
        End Property

        Public ReadOnly Property Paused As Integer
            Get
                Return Me.GetAttribute("Paused", 0)
            End Get
        End Property

        '        As far as I know Possible values are:
        '/*! Device is valid but channel didn't know state */
        '    define AST_DEVICE_UNKNOWN 0
        '/*! Device is not used */
        '    define AST_DEVICE_NOT_INUSE 1
        '/*! Device is in use */
        '    define AST_DEVICE_INUSE 2
        '/*! Device is busy */
        '    define AST_DEVICE_BUSY 3
        '/*! Device is invalid */
        '    define AST_DEVICE_INVALID 4
        '/*! Device is unavailable */
        '    define AST_DEVICE_UNAVAILABLE 5
        '/*! Device is ringing */
        '    define AST_DEVICE_RINGING 6
        '/*! Device is ringing *and* in use */
        '    define AST_DEVICE_RINGINUSE 7
        '/*! Device is on hold */
        '    define AST_DEVICE_ONHOLD 8
         
    End Class

End Namespace
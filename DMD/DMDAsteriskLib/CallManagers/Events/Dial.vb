Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Raised when a dial action has started.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Dial
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Dial")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub
         
        Public ReadOnly Property Source As String
            Get
                Return Me.GetAttribute("Source")
            End Get
        End Property

        Public ReadOnly Property Destination As String
            Get
                Return Me.GetAttribute("Destination")
            End Get
        End Property

        Public ReadOnly Property DestinationContext As String
            Get
                Return Me.GetAttribute("DestinationContext")
            End Get
        End Property

        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property

        Public ReadOnly Property CallerIDName As String
            Get
                Return Me.GetAttribute("CallerIDName")
            End Get
        End Property

        Public ReadOnly Property SrcUniqueID As String
            Get
                Return Me.GetAttribute("SrcUniqueID")
            End Get
        End Property

        Public ReadOnly Property DestUniqueID As String
            Get
                Return Me.GetAttribute("DestUniqueID")
            End Get
        End Property
          
    End Class

End Namespace
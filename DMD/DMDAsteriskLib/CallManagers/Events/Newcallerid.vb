Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class Newcallerid
        Inherits AsteriskEvent
         
        Public Sub New()
            MyBase.New("Newcallerid")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UniqueID")
            End Get
        End Property

        Public ReadOnly Property CallerIDName As String
            Get
                Return Me.GetAttribute("CallerIDName")
            End Get
        End Property

        Public ReadOnly Property CIDCallingPres As String
            Get
                Return Me.GetAttribute("CID-CallingPres")
            End Get
        End Property
         
    End Class

End Namespace
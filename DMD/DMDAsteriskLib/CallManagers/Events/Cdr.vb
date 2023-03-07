Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Cdr
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Cdr")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property AccountCode As String
            Get
                Return Me.GetAttribute("AccountCode")
            End Get
        End Property

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

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property DestinationChannel As String
            Get
                Return Me.GetAttribute("DestinationChannel")
            End Get
        End Property

        Public ReadOnly Property LastApplication As String
            Get
                Return Me.GetAttribute("LastApplication")
            End Get
        End Property

        Public ReadOnly Property LastData As String
            Get
                Return Me.GetAttribute("LastData")
            End Get
        End Property

        Public ReadOnly Property StartTime As String
            Get
                Return Me.GetAttribute("StartTime")
            End Get
        End Property

        Public ReadOnly Property AnswerTime As String
            Get
                Return Me.GetAttribute("AnswerTime")
            End Get
        End Property

        Public ReadOnly Property EndTime As String
            Get
                Return Me.GetAttribute("EndTime")
            End Get
        End Property

        Public ReadOnly Property Duration As String
            Get
                Return Me.GetAttribute("Duration")
            End Get
        End Property

        Public ReadOnly Property BillableSeconds As String
            Get
                Return Me.GetAttribute("BillableSeconds")
            End Get
        End Property

        Public ReadOnly Property Disposition As String
            Get
                Return Me.GetAttribute("Disposition")
            End Get
        End Property

        Public ReadOnly Property AMAFlags As String
            Get
                Return Me.GetAttribute("AMAFlags")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UniqueID")
            End Get
        End Property

        Public ReadOnly Property UserField As String
            Get
                Return Me.GetAttribute("UserField")
            End Get
        End Property
          
    End Class

End Namespace
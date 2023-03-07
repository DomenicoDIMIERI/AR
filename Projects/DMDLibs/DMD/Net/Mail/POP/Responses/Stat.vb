Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the results from the execution 
    ''' of a pop3 STAT command.
    ''' </summary>
    Public NotInheritable Class Stat

        Private _messageCount As Integer

        ''' <summary>
        ''' Gets or sets the message count.
        ''' </summary>
        ''' <value>The message count.</value>
        Public Property MessageCount As Integer
            Get
                Return Me._messageCount
            End Get
            Set(value As Integer)
                Me._messageCount = value
            End Set
        End Property

        Private _octets As Long

        ''' <summary>
        ''' Gets or sets the octets.
        ''' </summary>
        ''' <value>The octets.</value>
        Public Property Octets As Long
            Get
                Return _octets
            End Get
            Set(value As Long)
                Me._octets = value
            End Set
        End Property


        ''' <summary>
        ''' Initializes a new instance of the <see cref="Stat"/> class.
        ''' </summary>
        ''' <param name="messageCount">The message count.</param>
        ''' <param name="octets">The octets.</param>
        Public Sub New(ByVal messageCount As Integer, ByVal octets As Long)
            DMD.DMDObject.IncreaseCounter(Me)
            If (messageCount < 0) Then Throw New ArgumentOutOfRangeException("messageCount")
            If (octets < 0) Then Throw New ArgumentOutOfRangeException("octets")
            Me._messageCount = messageCount
            Me._octets = octets
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace

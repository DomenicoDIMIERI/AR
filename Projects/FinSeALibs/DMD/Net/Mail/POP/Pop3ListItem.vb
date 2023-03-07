Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents an item returned from the 
    ''' Pop3 LIST command.
    ''' </summary>
    Public Class Pop3ListItem

        Private _octets As Long
        Private _messageNumber As Integer

        ''' <summary>
        ''' Gets or sets the message number.
        ''' </summary>
        ''' <value>The message number.</value>
        Public Property MessageId As Integer
            Get
                Return Me._messageNumber
            End Get
            Set(value As Integer)
                Me._messageNumber = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the octets.
        ''' </summary>
        ''' <value>The octets.</value>
        Public Property Octets As Long
            Get
                Return Me._octets
            End Get
            Set(value As Long)
                Me._octets = value
            End Set
        End Property


        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3ListItem"/> class.
        ''' </summary>
        ''' <param name="messageNumber">The message number.</param>
        ''' <param name="octets">The octets.</param>
        Public Sub New(ByVal messageNumber As Integer, ByVal octets As Long)
            DMD.DMDObject.IncreaseCounter(Me)
            If (messageNumber < 0) Then Throw New ArgumentOutOfRangeException("messageNumber")
            If (octets < 1) Then Throw New ArgumentOutOfRangeException("octets")
            Me._messageNumber = messageNumber
            Me._octets = octets
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
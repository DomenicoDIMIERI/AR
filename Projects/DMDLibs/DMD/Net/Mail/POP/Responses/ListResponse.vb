Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the response message 
    ''' returned from both a single line and multi line 
    ''' Pop3 LIST Command.
    ''' </summary>
    Friend NotInheritable Class ListResponse
        Inherits Pop3Response

        Private _items As System.Collections.Generic.List(Of Pop3ListItem)

        ''' <summary>
        ''' Gets or sets the items.
        ''' </summary>
        ''' <value>The items.</value>
        Public Property Items As System.Collections.Generic.List(Of Pop3ListItem)
            Get
                Return Me._items
            End Get
            Set(value As System.Collections.Generic.List(Of Pop3ListItem))
                Me._items = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the message number.
        ''' </summary>
        ''' <value>The message number.</value>
        Public ReadOnly Property MessageNumber As Integer
            Get
                Return Me._items(0).MessageId
            End Get
        End Property

        ''' <summary>
        ''' Gets number of octets.
        ''' </summary>
        ''' <value>The number of octets.</value>
        Public ReadOnly Property Octets As Long
            Get
                Return Me._items(0).Octets
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ListResponse"/> class.
        ''' </summary>
        ''' <param name="response">The response.</param>
        ''' <param name="items">The items.</param>
        Public Sub New(ByVal response As Pop3Response, ByVal items As System.Collections.Generic.List(Of Pop3ListItem))
            MyBase.New(response.ResponseContents, response.HostMessage, response.StatusIndicator)
            If (items Is Nothing) Then Throw New ArgumentNullException("items")
            Me._items = items
        End Sub

    End Class

End Namespace


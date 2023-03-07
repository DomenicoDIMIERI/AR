Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class Office


    ''' <summary>
    ''' Collezione delle risposte per un ticket
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public NotInheritable Class CTicketAnswaresCollection
        Inherits CCollection(Of CTicketAnsware)

        <NonSerialized> Private m_Ticket As CTicket

        Public Sub New()
            Me.m_Ticket = Nothing
        End Sub

        Public Sub New(ByVal ticket As CTicket)
            Me.New
            Me.Load(ticket)
        End Sub

        Public ReadOnly Property Ticket As CTicket
            Get
                Return Me.m_Ticket
            End Get
        End Property

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Ticket IsNot Nothing) Then DirectCast(newValue, CTicketAnsware).SetTicket(Me.m_Ticket)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Ticket IsNot Nothing) Then DirectCast(value, CTicketAnsware).SetTicket(Me.m_Ticket)
            MyBase.OnInsert(index, value)
        End Sub

        Friend Sub Load(ByVal value As CTicket)
            If (value Is Nothing) Then Throw New ArgumentNullException("ticket")
            Me.Clear()
            Me.m_Ticket = value
            If (GetID(value) = 0) Then Return
            Dim cursor As New CTicketAnswareCursor
            cursor.IDTicket.Value = GetID(value)
            cursor.IgnoreRights = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose() : cursor = Nothing
            Me.Sort()
        End Sub


    End Class


End Class
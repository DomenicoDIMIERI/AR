Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class Office


    ''' <summary>
    ''' Cursore sulla tabella delle risposte ai messaggi
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public NotInheritable Class CTicketAnswareCursor
        Inherits DBObjectCursor(Of CTicketAnsware)

        Private m_IDTicket As New CCursorField(Of Integer)("IDTicket")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_StatoTicket As New CCursorField(Of TicketStatus)("StatoTicket")
        Private m_Messaggio As New CCursorFieldObj(Of String)("Messaggio")

        Public Sub New()

        End Sub

        Public ReadOnly Property IDTicket As CCursorField(Of Integer)
            Get
                Return Me.m_IDTicket
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property StatoTicket As CCursorField(Of TicketStatus)
            Get
                Return Me.m_StatoTicket
            End Get
        End Property

        Public ReadOnly Property Messaggio As CCursorFieldObj(Of String)
            Get
                Return Me.m_Messaggio
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SupportTicketsAnswares"
        End Function




    End Class


End Class
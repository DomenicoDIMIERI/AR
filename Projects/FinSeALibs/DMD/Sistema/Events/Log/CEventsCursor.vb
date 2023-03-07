Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulla tabella degli eventi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CEventsCursor
        Inherits DBObjectCursorBase(Of CEventLog)

        Private m_Data As CCursorField(Of Date)      'Data e ora in cui si è verificato l'evento
        Private m_Source As CCursorFieldObj(Of String)     'Nome del modulo che ha generato l'evento
        Private m_UserID As CCursorField(Of Integer) 'ID dell'utente nel cui contesto si è verificato l'evento
        Private m_UserName As CCursorFieldObj(Of String) 'Utente nel cui contesto si è verificato l'evento	
        Private m_EventName As CCursorFieldObj(Of String) 'Nome dell'evento
        Private m_Description As CCursorFieldObj(Of String) 'Descrizione dell'evento
        Private m_Parameters As CCursorFieldObj(Of String) 'Parametri dell'evento

        Public Sub New()
            Me.m_Data = New CCursorField(Of Date)("Data")
            Me.m_Source = New CCursorFieldObj(Of String)("Source")
            Me.m_UserID = New CCursorField(Of Integer)("User")
            Me.m_UserName = New CCursorFieldObj(Of String)("UserName")
            Me.m_EventName = New CCursorFieldObj(Of String)("EventName")
            Me.m_Description = New CCursorFieldObj(Of String)("Description")
            Me.m_Parameters = New CCursorFieldObj(Of String)("Parameters")
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CEventLog
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return LOGConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EventsLog"
        End Function

        'Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
        '    Dim col As CCursorFieldsCollection
        '    col = MyBase.GetCursorFields
        '    col.Add(m_Data)
        '    col.Add(m_Source)
        '    col.Add(m_UserID)
        '    col.Add(m_UserName)
        '    col.Add(m_EventName)
        '    col.Add(m_Description)
        '    col.Add(m_Parameters)
        '    Return col
        'End Function

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property Source As CCursorFieldObj(Of String)
            Get
                Return Me.m_Source
            End Get
        End Property

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property EventName As CCursorFieldObj(Of String)
            Get
                Return Me.m_EventName
            End Get
        End Property

        Public ReadOnly Property Description As CCursorFieldObj(Of String)
            Get
                Return Me.m_Description
            End Get
        End Property

        Public ReadOnly Property Parameters As CCursorFieldObj(Of String)
            Get
                Return Me.m_Parameters
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Sistema.Events.Module
        End Function
    End Class

End Class
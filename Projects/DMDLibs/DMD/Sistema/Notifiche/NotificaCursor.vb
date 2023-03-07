Imports DMD.Databases

Partial Public Class Sistema


    ''' <summary>
    ''' Cursore sulla tabella delle notifiche 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NotificaCursor
        Inherits DBObjectCursorPO(Of Notifica)

        'Private m_Data As New CCursorField(Of Date)("Data")
        Private m_Data As New CCursorFieldDBDate("DataStr")
        Private m_Context As New CCursorFieldObj(Of String)("Context")
        Private m_SourceName As New CCursorFieldObj(Of String)("SourceName")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_TargetID As New CCursorField(Of Integer)("TargetID")
        Private m_TargetName As New CCursorFieldObj(Of String)("TargetName")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_DataConsegna As New CCursorField(Of Date)("DataConsegna")
        Private m_DataLettura As New CCursorField(Of Date)("DataLettura")
        Private m_StatoNotifica As New CCursorField(Of StatoNotifica)("StatoNotifica")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")

        Public Sub New()
        End Sub

        'Public ReadOnly Property Data As CCursorField(Of Date)
        '    Get
        '        Return Me.m_Data
        '    End Get
        'End Property

        Public ReadOnly Property Data As CCursorFieldDBDate
            Get
                Return Me.m_Data
            End Get
        End Property


        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Context As CCursorFieldObj(Of String)
            Get
                Return Me.m_Context
            End Get
        End Property

        Public ReadOnly Property SourceName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceName
            End Get
        End Property

        Public ReadOnly Property SourceID As CCursorField(Of Integer)
            Get
                Return Me.m_SourceID
            End Get
        End Property

        Public ReadOnly Property TargetID As CCursorField(Of Integer)
            Get
                Return Me.m_TargetID
            End Get
        End Property

        Public ReadOnly Property TargetName As CCursorFieldObj(Of String)
            Get
                Return Me.m_TargetName
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property DataConsegna As CCursorField(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
        End Property

        Public ReadOnly Property DataLettura As CCursorField(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
        End Property

        Public ReadOnly Property StatoNotifica As CCursorField(Of StatoNotifica)
            Get
                Return Me.m_StatoNotifica
            End Get
        End Property


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Notifiche.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SYSNotify"
        End Function

    End Class


End Class
Imports DMD.Databases

Partial Public Class Sistema

     

    Public Class AzioneRegistrataCursor
        Inherits DBObjectCursor(Of AzioneRegistrata)

        Private m_Priorita As New CCursorField(Of Integer)("Priorita")
        Private m_Description As New CCursorFieldObj(Of String)("Description")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_SourceType As New CCursorFieldObj(Of String)("SourceType")
        Private m_ActionType As New CCursorFieldObj(Of String)("ActionType")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")

        Public Sub New()
        End Sub

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property


        Public ReadOnly Property Priorita As CCursorField(Of Integer)
            Get
                Return Me.m_Priorita
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property Description As CCursorFieldObj(Of String)
            Get
                Return Me.m_Description
            End Get
        End Property


        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property SourceName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceType
            End Get
        End Property

        Public ReadOnly Property ActionType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ActionType
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SYSNotifyAct"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Notifiche.Database
        End Function
    End Class

 

End Class
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CMergePersonaCursor
        Inherits DBObjectCursor(Of CMergePersona)

        Private m_IDPersona1 As New CCursorField(Of Integer)("IDPersona1")
        Private m_NomePersona1 As New CCursorFieldObj(Of String)("NomePersona1")

        Private m_IDPersona2 As New CCursorField(Of Integer)("IDPersona2")
        Private m_NomePersona2 As New CCursorFieldObj(Of String)("NomePersona2")

        Private m_DataOperazione As New CCursorField(Of Date)("DataOperazione")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDPersona1 As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona1
            End Get
        End Property

        Public ReadOnly Property NomePersona1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona1
            End Get
        End Property

        Public ReadOnly Property IDPersona2 As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona2
            End Get
        End Property

        Public ReadOnly Property NomePersona2 As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona2
            End Get
        End Property


        Public ReadOnly Property DataOperazione As CCursorField(Of Date)
            Get
                Return Me.m_DataOperazione
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

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.MergePersone.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MergePersone"
        End Function

    End Class


End Class
Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases

Partial Public Class Sistema


    Public Class CAnnotazioniCursor
        Inherits DBObjectCursor(Of CAnnotazione)

        Private m_OwnerID As CCursorField(Of Integer) 'ID della persona associata
        Private m_OwnerType As CCursorFieldObj(Of String)
        Private m_Valore As CCursorFieldObj(Of String)
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_SourceName As New CCursorFieldObj(Of String)("SourceName")
        Private m_SourceParam As New CCursorFieldObj(Of String)("SourceParam")
        ' Private m_IDProduttore As New CCursorField(Of Integer)("IDProduttore")

        Public Sub New()
            Me.m_OwnerID = New CCursorField(Of Integer)("OwnerID")
            Me.m_OwnerType = New CCursorFieldObj(Of String)("OwnerType")
            Me.m_Valore = New CCursorFieldObj(Of String)("Valore")
        End Sub


        'Public ReadOnly Property IDProduttore As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDProduttore
        '    End Get
        'End Property

        Public ReadOnly Property OwnerID As CCursorField(Of Integer)
            Get
                Return Me.m_OwnerID
            End Get
        End Property

        Public ReadOnly Property OwnerType As CCursorFieldObj(Of String)
            Get
                Return Me.m_OwnerType
            End Get
        End Property

        Public ReadOnly Property Valore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Valore
            End Get
        End Property

        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property TipoContesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
            End Get
        End Property

        Public ReadOnly Property SourceName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceName
            End Get
        End Property

        Public ReadOnly Property SourceParam As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceParam
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Dim ret As New CAnnotazione
            'ret.Produttore = Anagrafica.Aziende.AziendaPrincipale 
            Return ret
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Annotazioni"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Sistema.Annotazioni.Database
        End Function
    End Class

End Class


Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Cursore sulla tabella delle postazioni di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ValoreRegistroCursor
        Inherits DBObjectCursorPO(Of ValoreRegistroContatore)

        Private m_IDPostazione As New CCursorField(Of Integer)("IDPostazione")
        Private m_NomePostazione As New CCursorFieldObj(Of String)("NomePostazione")
        Private m_DataRegistrazione As New CCursorField(Of DateTime)("DataRegistrazione")
        Private m_NomeRegistro As New CCursorFieldObj(Of String)("NomeRegistro")
        Private m_ValoreRegistro As New CCursorField(Of Double)("ValoreRegistro")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDPostazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDPostazione
            End Get
        End Property

        Public ReadOnly Property NomePostazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePostazione
            End Get
        End Property

        Public ReadOnly Property DataRegistrazione As CCursorField(Of DateTime)
            Get
                Return Me.m_DataRegistrazione
            End Get
        End Property

        Public ReadOnly Property NomeRegistro As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRegistro
            End Get
        End Property

        Public ReadOnly Property ValoreRegistro As CCursorField(Of Double)
            Get
                Return Me.m_ValoreRegistro
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New ValoreRegistroContatore
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PostazoniRegistri"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function



    End Class


End Class
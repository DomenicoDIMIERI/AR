Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public Class CRegioniCursor
        Inherits DBObjectCursorPO(Of CRegione)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_NumeroAbitanti As New CCursorField(Of Integer)("NumeroAbitanti")
        Private m_Sigla As New CCursorFieldObj(Of String)("Sigla")
        Private m_Nazione As New CCursorFieldObj(Of String)("Nazione")
        Private m_NomeAbitanti As New CCursorFieldObj(Of String)("NomeAbitanti")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property NumeroAbitanti As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroAbitanti
            End Get
        End Property

        Public ReadOnly Property Sigla As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sigla
            End Get
        End Property

        Public ReadOnly Property Nazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nazione
            End Get
        End Property

        Public ReadOnly Property NomeAbitanti As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAbitanti
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CRegione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Regioni"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Luoghi.Regioni.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class

End Class
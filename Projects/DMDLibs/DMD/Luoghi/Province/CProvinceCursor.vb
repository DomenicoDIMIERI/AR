Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public Class CProvinceCursor
        Inherits DBObjectCursorPO(Of CProvincia)

        Private m_Nome As CCursorFieldObj(Of String)
        Private m_NumeroAbitanti As CCursorField(Of Integer)
        Private m_Sigla As CCursorFieldObj(Of String)
        Private m_Regione As CCursorFieldObj(Of String)

        Public Sub New()
            Me.m_Nome = New CCursorFieldObj(Of String)("Nome")
            Me.m_NumeroAbitanti = New CCursorField(Of Integer)("NumeroResidente")
            Me.m_Sigla = New CCursorFieldObj(Of String)("Sigla")
            Me.m_Regione = New CCursorFieldObj(Of String)("Regione")
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

        Public ReadOnly Property Regione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Regione
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProvincia
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Province"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Luoghi.Province.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class


End Class
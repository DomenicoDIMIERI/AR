Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls
     
    Public Class BlackListAddressCursor
        Inherits DBObjectCursorBase(Of BlackListAddress)

        Private m_TipoRegola As New CCursorField(Of BlackListType)("Tipo")
        Private m_TipoContatto As New CCursorFieldObj(Of String)("TipoContatto")
        Private m_ValoreContatto As New CCursorFieldObj(Of String)("Valore")
        Private m_DataBlocco As New CCursorField(Of Date)("DataBlocco")
        Private m_IDBloccatoDa As New CCursorField(Of Integer)("IDBloccatoDa")
        Private m_NomeBloccatoDa As New CCursorFieldObj(Of String)("NomeBloccatoDa")
        Private m_MotivoBlocco As New CCursorFieldObj(Of String)("MotivoBlocco")

        Public Sub New()
        End Sub

        Public ReadOnly Property TipoRegola As CCursorField(Of BlackListType)
            Get
                Return Me.m_TipoRegola
            End Get
        End Property

        Public ReadOnly Property ValoreContatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_ValoreContatto
            End Get
        End Property

        Public ReadOnly Property TipoContatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContatto
            End Get
        End Property

        Public ReadOnly Property DataBlocco As CCursorField(Of Date)
            Get
                Return Me.m_DataBlocco
            End Get
        End Property

        Public ReadOnly Property IDBloccatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDBloccatoDa
            End Get
        End Property

        Public ReadOnly Property NomeBloccatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeBloccatoDa
            End Get
        End Property

        Public ReadOnly Property MotivoBlocco As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoBlocco
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CustomerCalls.BlackListAdresses.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_BlackListAddress"
        End Function
    End Class



End Class
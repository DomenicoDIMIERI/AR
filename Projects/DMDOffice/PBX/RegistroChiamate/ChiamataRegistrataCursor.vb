Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella chiamate registrate
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChiamataRegistrataCursor
        Inherits DBObjectCursorPO(Of ChiamataRegistrata)

        Private m_IDChiamata As New CCursorFieldObj(Of String)("IDChiamata")
        Private m_StatoChiamata As New CCursorField(Of StatoChiamataRegistrata)("StatoChiamata")
        Private m_EsitoChiamataEx As New CCursorFieldObj(Of String)("EsitoChiamataEx")
        Private m_EsitoChiamata As New CCursorField(Of EsitoChiamataRegistrata)("EsitoChiamata")
        Private m_StatoChiamataEx As New CCursorFieldObj(Of String)("StatoChiamataEx")

        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataRisposta As New CCursorField(Of Date)("DataRisposta")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")

        Private m_IDPBX As New CCursorField(Of Integer)("IDPBX")
        Private m_NomePBX As New CCursorFieldObj(Of String)("NomePBX")

        Private m_IDChiamante As New CCursorField(Of Integer)("IDChiamante")
        Private m_NomeChiamante As New CCursorFieldObj(Of String)("NomeChiamante")

        Private m_IDChiamato As New CCursorField(Of Integer)("IDChiamato")
        Private m_NomeChiamato As New CCursorFieldObj(Of String)("NomeChiamato")

        Private m_DaNumero As New CCursorFieldObj(Of String)("DaNumero")
        Private m_ANumero As New CCursorFieldObj(Of String)("ANumero")

        Private m_NomeCanale As New CCursorFieldObj(Of String)("NomeCanale")
        Private m_Qualita As New CCursorField(Of QualitaChiamata)("Qualita")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDChiamata As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDChiamata
            End Get
        End Property

        Public ReadOnly Property StatoChiamata As CCursorField(Of StatoChiamataRegistrata)
            Get
                Return Me.m_StatoChiamata
            End Get
        End Property

        Public ReadOnly Property EsitoChiamataEx As CCursorFieldObj(Of String)
            Get
                Return Me.m_EsitoChiamataEx
            End Get
        End Property

        Public ReadOnly Property EsitoChiamata As CCursorField(Of EsitoChiamataRegistrata)
            Get
                Return Me.m_EsitoChiamata
            End Get
        End Property

        Public ReadOnly Property StatoChiamataEx As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatoChiamataEx
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataRisposta As CCursorField(Of Date)
            Get
                Return Me.m_DataRisposta
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property IDPBX As CCursorField(Of Integer)
            Get
                Return Me.m_IDPBX
            End Get
        End Property

        Public ReadOnly Property NomePBX As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePBX
            End Get
        End Property

        Public ReadOnly Property IDChiamante As CCursorField(Of Integer)
            Get
                Return Me.m_IDChiamante
            End Get
        End Property

        Public ReadOnly Property NomeChiamante As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeChiamante
            End Get
        End Property

        Public ReadOnly Property IDChiamato As CCursorField(Of Integer)
            Get
                Return Me.m_IDChiamato
            End Get
        End Property

        Public ReadOnly Property NomeChiamato As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeChiamato
            End Get
        End Property

        Public ReadOnly Property DaNumero As CCursorFieldObj(Of String)
            Get
                Return Me.m_DaNumero
            End Get
        End Property

        Public ReadOnly Property ANumero As CCursorFieldObj(Of String)
            Get
                Return Me.m_ANumero
            End Get
        End Property

        Public ReadOnly Property NomeCanale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCanale
            End Get
        End Property

        Public ReadOnly Property Qualita As CCursorField(Of QualitaChiamata)
            Get
                Return Me.m_Qualita
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.ChiamateRegistrate.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRegCall"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.PBXs.Database
        End Function
    End Class



End Class
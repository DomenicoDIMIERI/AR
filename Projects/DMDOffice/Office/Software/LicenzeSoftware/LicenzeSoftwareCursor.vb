Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella delle licenze software
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LicenzeSoftwareCursor
        Inherits DBObjectCursorPO(Of LicenzaSoftware)

        Private m_IDSoftware As New CCursorField(Of Integer)("IDSoftware")
        Private m_NomeSoftware As New CCursorFieldObj(Of String)("NomeSoftware")
        Private m_IDDispositivo As New CCursorField(Of Integer)("IDDispositivo")
        Private m_NomeDispositivo As New CCursorFieldObj(Of String)("NomeDispositivo")
        Private m_CodiceLicenza As New CCursorFieldObj(Of String)("CodiceLicenza")
        Private m_DataAcquisto As New CCursorField(Of Date)("DataAcquisto")
        Private m_DataInstallazione As New CCursorField(Of Date)("DataInstallazione")
        Private m_DataDismissione As New CCursorField(Of Date)("DataDismissione")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")
        Private m_ScaricatoDa As New CCursorFieldObj(Of String)("ScaricatoDa")
        Private m_StatoUtilizzo As New CCursorField(Of StatoLicenzaSoftware)("StatoUtilizzo")
        Private m_Flags As New CCursorField(Of FlagsLicenzaSoftware)("Flags")
        Private m_IDProprietario As New CCursorField(Of Integer)("IDProprietario")
        Private m_NomeProprietario As New CCursorFieldObj(Of String)("NomeProprietario")
        Private m_IDDocumentoAcquisto As New CCursorField(Of Integer)("IDDocumentoAcquisto")
        Private m_NumeroDocumentoAcquisto As New CCursorFieldObj(Of String)("NumeroDocumentoAcquisto")



        Public Sub New()
        End Sub

        Public ReadOnly Property IDSoftware As CCursorField(Of Integer)
            Get
                Return Me.m_IDSoftware
            End Get
        End Property

        Public ReadOnly Property NomeSoftware As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeSoftware
            End Get
        End Property

        Public ReadOnly Property IDDispositivo As CCursorField(Of Integer)
            Get
                Return Me.m_IDDispositivo
            End Get
        End Property

        Public ReadOnly Property NomeDispositivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDispositivo
            End Get
        End Property

        Public ReadOnly Property CodiceLicenza As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceLicenza
            End Get
        End Property

        Public ReadOnly Property DataAcquisto As CCursorField(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
        End Property

        Public ReadOnly Property DataInstallazione As CCursorField(Of Date)
            Get
                Return Me.m_DataInstallazione
            End Get
        End Property

        Public ReadOnly Property DataDismissione As CCursorField(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
        End Property

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property

        Public ReadOnly Property ScaricatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_ScaricatoDa
            End Get
        End Property

        Public ReadOnly Property StatoUtilizzo As CCursorField(Of StatoLicenzaSoftware)
            Get
                Return Me.m_StatoUtilizzo
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of FlagsLicenzaSoftware)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDProprietario As CCursorField(Of Integer)
            Get
                Return Me.m_IDProprietario
            End Get
        End Property

        Public ReadOnly Property NomeProprietario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProprietario
            End Get
        End Property

        Public ReadOnly Property IDDocumentoAcquisto As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumentoAcquisto
            End Get
        End Property

        Public ReadOnly Property NumeroDocumentoAcquisto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroDocumentoAcquisto
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.LicenzeSoftware.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeLicenzeSoftware"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class
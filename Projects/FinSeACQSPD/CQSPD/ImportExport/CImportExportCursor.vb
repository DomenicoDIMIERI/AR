Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    Public Class CImportExportCursor
        Inherits DBObjectCursorPO(Of CImportExport)

        Private m_Esportazione As New CCursorField(Of Boolean)("Esportazione")
        Private m_DataEsportazione As New CCursorField(Of Date)("DataEsportazione")
        Private m_IDEsportataDa As New CCursorField(Of Integer)("IDEsportataDa")
        Private m_NomeEsportataDa As New CCursorFieldObj(Of String)("NomeEsportataDa")
        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")
        Private m_IDPresaInCaricoDa As New CCursorField(Of Integer)("IDPresaInCaricoDa")
        Private m_NomePresaInCaricoDa As New CCursorFieldObj(Of String)("NomePresaInCaricoDa")
        Private m_IDPersonaEsportata As New CCursorField(Of Integer)("IDPersonaEsportata")
        Private m_NomePersonaEsportata As New CCursorFieldObj(Of String)("NomePersonaEsportata")
        Private m_IDPersonaImportata As New CCursorField(Of Integer)("IDPersonaImportata")
        Private m_NomePersonaImportata As New CCursorFieldObj(Of String)("NomePersonaImportata")
        Private m_IDFinestraLavorazioneEsportata As New CCursorField(Of Integer)("IDFinestraLavorazioneEsportata")
        Private m_IDFinestraLavorazioneImportata As New CCursorField(Of Integer)("IDFinestraLavorazioneImportata")
        Private m_Flags As New CCursorField(Of FlagsEsportazione)("Flags")
        Private m_StatoRemoto As New CCursorField(Of StatoEsportazione)("StatoRemoto")
        Private m_DettaglioStatoRemoto As New CCursorFieldObj(Of String)("DettaglioStatoRemoto")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_SharedKey As New CCursorFieldObj(Of String)("SharedKey")
        Private m_DataUltimoAggiornamento As New CCursorField(Of Date)("DataUltimoAggiornamento")
        Private m_DataEsportazioneOk As New CCursorField(Of Date)("DataEsportazioneOk")
        Private m_StatoConferma As New CCursorField(Of StatoConfermaEsportazione)("StatoConferma")
        Private m_IDOperatoreConferma As New CCursorField(Of Integer)("IDOperatoreConferma")
        Private m_NomeOperatoreConferma As New CCursorFieldObj(Of String)("NomeOperatoreConferma")


        Public Sub New()

        End Sub

        Public ReadOnly Property IDOperatoreConferma As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreConferma
            End Get
        End Property

        Public ReadOnly Property NomeOperatoreConferma As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatoreConferma
            End Get
        End Property

        Public ReadOnly Property StatoConferma As CCursorField(Of StatoConfermaEsportazione)
            Get
                Return Me.m_StatoConferma
            End Get
        End Property

        Public ReadOnly Property DataEsportazioneOk As CCursorField(Of Date)
            Get
                Return Me.m_DataEsportazioneOk
            End Get
        End Property

        Public ReadOnly Property Esportazione As CCursorField(Of Boolean)
            Get
                Return Me.m_Esportazione
            End Get
        End Property

        Public ReadOnly Property DataEsportazione As CCursorField(Of Date)
            Get
                Return Me.m_DataEsportazione
            End Get
        End Property

        Public ReadOnly Property IDEsportataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDEsportataDa
            End Get
        End Property

        Public ReadOnly Property NomeEsportataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeEsportataDa
            End Get
        End Property

        Public ReadOnly Property DataPresaInCarico As CCursorField(Of Date)
            Get
                Return Me.m_DataPresaInCarico
            End Get
        End Property

        Public ReadOnly Property IDPresaInCaricoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDPresaInCaricoDa
            End Get
        End Property

        Public ReadOnly Property NomePresaInCaricoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePresaInCaricoDa
            End Get
        End Property

        Public ReadOnly Property IDPersonaEsportata As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersonaEsportata
            End Get
        End Property

        Public ReadOnly Property NomePersonaEsportata As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersonaEsportata
            End Get
        End Property

        Public ReadOnly Property IDPersonaImportata As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersonaImportata
            End Get
        End Property

        Public ReadOnly Property NomePersonaImportata As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersonaImportata
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazioneEsportata As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazioneEsportata
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazioneImportata As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazioneImportata
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of FlagsEsportazione)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property StatoRemoto As CCursorField(Of StatoEsportazione)
            Get
                Return Me.m_StatoRemoto
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoRemoto As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoRemoto
            End Get
        End Property

        Public ReadOnly Property SourceID As CCursorField(Of Integer)
            Get
                Return Me.m_SourceID
            End Get
        End Property

        Public ReadOnly Property SharedKey As CCursorFieldObj(Of String)
            Get
                Return Me.m_SharedKey
            End Get
        End Property

        Public ReadOnly Property DataUltimoAggiornamento As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimoAggiornamento
            End Get
        End Property


        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CImportExport
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDImportExport"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return CQSPD.ImportExport.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class
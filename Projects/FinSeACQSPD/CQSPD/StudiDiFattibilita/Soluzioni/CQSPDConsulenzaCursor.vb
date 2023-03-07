Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle consulenze
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CQSPDConsulenzaCursor
        Inherits DBObjectCursorPO(Of CQSPDConsulenza)

        Private m_IDStudioDiFattibilita As New CCursorField(Of Integer)("IDGruppo")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IDRichiesta As New CCursorField(Of Integer)("IDRichiesta")
        'Private m_IDOffertaCorrente As Integer          'ID dell'offerta corrente
        Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_NomeConsulente As New CCursorFieldObj(Of String)("NomeConsulente")
        Private m_DataConsulenza As New CCursorField(Of Date)("DataConsulenza")
        Private m_DataConferma As New CCursorField(Of Date)("DataConferma")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Flags As New CCursorField(Of ConsulenzeFlags)("Flags")
        Private m_StatoConsulenza As New CCursorField(Of StatiConsulenza)("StatoConsulenza")
        Private m_IDOffertaCQS As New CCursorField(Of Integer)("IDOffertaCQS")
        Private m_IDOffertaPD As New CCursorField(Of Integer)("IDOffertaPD")
        Private m_DataProposta As New CCursorField(Of Date)("DataProposta")
        Private m_IDPropostaDa As New CCursorField(Of Integer)("IDPropostaDa")
        Private m_IDConfermataDa As New CCursorField(Of Integer)("IDConfermataDa")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_Durata As New CCursorField(Of Double)("Durata")
        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_IDInseritoDa As New CCursorField(Of Integer)("IDInseritoDa")
        Private m_IDRichiestaApprovazione As New CCursorField(Of Integer)("IDRichiestaApprovazione")
        Private m_MotivoAnnullamento As New CCursorFieldObj(Of String)("MotivoAnnullamento")
        Private m_DettaglioAnnullamento As New CCursorFieldObj(Of String)("DettaglioAnnullamento")
        Private m_IDAnnullataDa As New CCursorField(Of Integer)("IDAnnullataDa")
        Private m_NomeAnnullataDa As New CCursorFieldObj(Of String)("NomeAnnullataDa")
        Private m_DataAnnullamento As New CCursorField(Of Date)("DataAnnullamento")
        Private m_IDFinestraLavorazione As New CCursorField(Of Integer)("IDFinestraLavorazione")
        Private m_IDUltimaVerifica As New CCursorField(Of Integer)("IDUltimaVerifica")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDUltimaVerifica As CCursorField(Of Integer)
            Get
                Return Me.m_IDUltimaVerifica
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazione
            End Get
        End Property

        Public ReadOnly Property IDAnnullataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDAnnullataDa
            End Get
        End Property

        Public ReadOnly Property NomeAnnullataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAnnullataDa
            End Get
        End Property

        Public ReadOnly Property DataAnnullamento As CCursorField(Of Date)
            Get
                Return Me.m_DataAnnullamento
            End Get
        End Property

        Public ReadOnly Property MotivoAnnullamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoAnnullamento
            End Get
        End Property

        Public ReadOnly Property DettaglioAnnullamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioAnnullamento
            End Get
        End Property


        Public ReadOnly Property IDRichiestaApprovazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaApprovazione
            End Get
        End Property

        Public ReadOnly Property IDInseritoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDInseritoDa
            End Get
        End Property


        Public ReadOnly Property IDStudioDiFattibilita As CCursorField(Of Integer)
            Get
                Return Me.m_IDStudioDiFattibilita
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Double)
            Get
                Return Me.m_Durata
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

        Public ReadOnly Property DataProposta As CCursorField(Of Date)
            Get
                Return Me.m_DataProposta
            End Get
        End Property

        Public ReadOnly Property IDPropostaDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDPropostaDa
            End Get
        End Property

        Public ReadOnly Property IDConfermataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDConfermataDa
            End Get
        End Property

        Public ReadOnly Property IDOffertaCQS As CCursorField(Of Integer)
            Get
                Return Me.m_IDOffertaCQS
            End Get
        End Property

        Public ReadOnly Property IDOffertaPD As CCursorField(Of Integer)
            Get
                Return Me.m_IDOffertaPD
            End Get
        End Property

        Public ReadOnly Property StatoConsulenza As CCursorField(Of StatiConsulenza)
            Get
                Return Me.m_StatoConsulenza
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property IDRichiesta As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiesta
            End Get
        End Property

        Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulente
            End Get
        End Property

        Public ReadOnly Property NomeConsulente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConsulente
            End Get
        End Property

        Public ReadOnly Property DataConsulenza As CCursorField(Of Date)
            Get
                Return Me.m_DataConsulenza
            End Get
        End Property

        Public ReadOnly Property DataConferma As CCursorField(Of Date)
            Get
                Return Me.m_DataConferma
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ConsulenzeFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CQSPDConsulenza
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConsulenze"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Consulenze.Module
        End Function


    End Class


End Class

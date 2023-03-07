Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable>
    Public Class CQSPDValutazioneAziendaCursor
        Inherits DBObjectCursor(Of CQSPDValutazioneAzienda)

        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_NomeAzienda As New CCursorFieldObj(Of String)("NomeAzienda")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")
        Private m_NomeFonte As New CCursorFieldObj(Of String)("NomeFonte")
        Private m_CapitaleSociale As New CCursorField(Of Decimal)("CapitaleSociale")
        Private m_NumeroDipendenti As New CCursorField(Of Integer)("NumeroDipendenti")
        Private m_FatturatoAnnuo As New CCursorField(Of Decimal)("FatturatoAnnuo")
        Private m_RapportoTFR_VN As New CCursorField(Of Double)("RapportoTFR_VN")
        Private m_Rating As New CCursorField(Of Integer)("Rating")
        Private m_DataRevisione As New CCursorField(Of Date)("DataRevisione")
        Private m_DataScadenzaRevisione As New CCursorField(Of Date)("DataScadenzaRevisioneione")
        Private m_StatoAzienda As New CCursorFieldObj(Of String)("StatoAzienda")
        Private m_DettaglioStatoAzienda As New CCursorFieldObj(Of String)("DettaglioStatoAzienda")
        Private m_GiorniAnticipoEstinzione As New CCursorField(Of Integer)("GiorniAnticipoEstinzione")
        Private m_Flags As New CCursorField(Of CQSPDValutazioneAziendaFlags)("Flags")

        Public Sub New()
        End Sub

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

        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
            End Get
        End Property

        Public ReadOnly Property NomeAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAzienda
            End Get
        End Property

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property

        Public ReadOnly Property IDFonte As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonte
            End Get
        End Property

        Public ReadOnly Property NomeFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFonte
            End Get
        End Property

        Public ReadOnly Property CapitaleSociale As CCursorField(Of Decimal)
            Get
                Return Me.m_CapitaleSociale
            End Get
        End Property

        Public ReadOnly Property NumeroDipendenti As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroDipendenti
            End Get
        End Property

        Public ReadOnly Property FatturatoAnnuo As CCursorField(Of Decimal)
            Get
                Return Me.m_FatturatoAnnuo
            End Get
        End Property

        Public ReadOnly Property RapportoTFR_VN As CCursorField(Of Double)
            Get
                Return Me.m_RapportoTFR_VN
            End Get
        End Property

        Public ReadOnly Property Rating As CCursorField(Of Integer)
            Get
                Return Me.m_Rating
            End Get
        End Property

        Public ReadOnly Property DataRevisione As CCursorField(Of Date)
            Get
                Return Me.m_DataRevisione
            End Get
        End Property

        Public ReadOnly Property DataScadenzaRevisione As CCursorField(Of Date)
            Get
                Return Me.m_DataScadenzaRevisione
            End Get
        End Property

        Public ReadOnly Property StatoAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatoAzienda
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoAzienda
            End Get
        End Property

        Public ReadOnly Property GiorniAnticipoEstinzione As CCursorField(Of Integer)
            Get
                Return Me.m_GiorniAnticipoEstinzione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of CQSPDValutazioneAziendaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.ValutazioniAzienda.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDValutazioniAzienda"
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class

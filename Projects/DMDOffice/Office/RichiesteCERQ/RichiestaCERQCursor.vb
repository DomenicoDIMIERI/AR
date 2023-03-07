Imports DMD.Databases

Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella delle richieste di conteggi / quote
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RichiestaCERQCursor
        Inherits DBObjectCursorPO(Of RichiestaCERQ)

        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_TipoRichiesta As New CCursorFieldObj(Of String)("TipoRichiesta")
        Private m_IDAmministrazione As New CCursorField(Of Integer)("IDAmministrazione")
        Private m_NomeAmministrazione As New CCursorFieldObj(Of String)("NomeAmministrazione")
        Private m_RichiestaAMezzo As New CCursorFieldObj(Of String)("RichiestaAMezzo")
        Private m_RichiestaAIndirizzo As New CCursorFieldObj(Of String)("RichiestaAIndirizzo")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_StatoOperazione As New CCursorField(Of StatoRichiestaCERQ)("StatoOperazione")
        Private m_DataPrevista As New CCursorField(Of Date)("DataPrevista")
        Private m_DataEffettiva As New CCursorField(Of Date)("DataEffettiva")
        Private m_IDOperatoreEffettivo As New CCursorField(Of Integer)("IDOperatoreEffettivo")
        Private m_NomeOperatoreEffettivo As New CCursorFieldObj(Of String)("NomeOperatoreEffettivo")
        Private m_IDCommissione As New CCursorField(Of Integer)("IDCommissione")
        Private m_ContextType As New CCursorFieldObj(Of String)("ContextType")
        Private m_ContextID As New CCursorField(Of Integer)("ContextID")
        Private m_IDAziendaRichiedente As New CCursorField(Of Integer)("IDAziendaRichiedente")
        Private m_NomeAziendaRichiedente As New CCursorFieldObj(Of String)("NomeAziendaRichiedente")
        Private m_IDDocumentoProdotto As New CCursorField(Of Integer)("IDOggettoProdotto")
        Private m_TipoDocumentoProdotto As New CCursorFieldObj(Of String)("TipoOggettoProdotto")

        Public Sub New()
        End Sub

        Public ReadOnly Property NomeAziendaRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAziendaRichiedente
            End Get
        End Property

        Public ReadOnly Property IDAziendaRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDAziendaRichiedente
            End Get
        End Property

        Public ReadOnly Property ContextType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContextType
            End Get
        End Property

        Public ReadOnly Property ContextID As CCursorField(Of Integer)
            Get
                Return Me.m_ContextID
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

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

        Public ReadOnly Property TipoRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoRichiesta
            End Get
        End Property

        Public ReadOnly Property IDAmministrazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDAmministrazione
            End Get
        End Property

        Public ReadOnly Property NomeAmministrazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAmministrazione
            End Get
        End Property

        Public ReadOnly Property RichiestaAMezzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_RichiestaAMezzo
            End Get
        End Property

        Public ReadOnly Property RichiestaAIndirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_RichiestaAIndirizzo
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property StatoOperazione As CCursorField(Of StatoRichiestaCERQ)
            Get
                Return Me.m_StatoOperazione
            End Get
        End Property

        Public ReadOnly Property DataPrevista As CCursorField(Of Date)
            Get
                Return Me.m_DataPrevista
            End Get
        End Property

        Public ReadOnly Property DataEffettiva As CCursorField(Of Date)
            Get
                Return Me.m_DataEffettiva
            End Get
        End Property

        Public ReadOnly Property IDOperatoreEffettivo As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreEffettivo
            End Get
        End Property

        Public ReadOnly Property NomeOperatoreEffettivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatoreEffettivo
            End Get
        End Property

        Public ReadOnly Property IDCommissione As CCursorField(Of Integer)
            Get
                Return Me.m_IDCommissione
            End Get
        End Property

        Public ReadOnly Property IDDocumentoProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumentoProdotto
            End Get
        End Property

        Public ReadOnly Property TipoDocumentoProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoDocumentoProdotto
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return RichiesteCERQ.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDRichCERQ"
        End Function

        Public Overrides Function Add() As Object
            Dim ret As RichiestaCERQ = MyBase.Add()
            ret.Data = Now
            ret.Operatore = Users.CurrentUser
            ret.AziendaRichiedente = Anagrafica.Aziende.AziendaPrincipale 
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

    End Class



End Class
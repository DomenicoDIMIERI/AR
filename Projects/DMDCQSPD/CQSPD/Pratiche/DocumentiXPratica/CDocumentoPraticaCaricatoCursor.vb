Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella dei documenti caricati per una pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumentoPraticaCaricatoCursor
        Inherits DBObjectCursor(Of CDocumentoPraticaCaricato)

        Private m_IDDocumento As CCursorField(Of Integer) '[INT] ID del tipo di documento (CDocumentoPerPratica)
        Private m_IDPratica As CCursorField(Of Integer) 'ID della pratica
        Private m_DataCaricamento As CCursorField(Of Date) '[Date] Data ed ora di caricamento
        Private m_IDOperatoreCaricamento As CCursorField(Of Integer) '[INT] ID dell'operatore che ha caricato il documento
        Private m_NomeOperatoreCaricamento As CCursorFieldObj(Of String)
        Private m_DataInizioSpedizione As CCursorField(Of Date) '[Date] Data di inizio spedizione
        Private m_IDOperatoreSpedizione As CCursorField(Of Integer) '[INT] ID dell'operatore che ha preso in carico la spedizione
        Private m_NomeOperatoreSpedizione As CCursorFieldObj(Of String)
        Private m_DataConsegna As CCursorField(Of Date) '[Date] Data di consegna
        Private m_IDOperatoreConsegna As CCursorField(Of Integer) '[INT] ID dell'operatore che consegnato il documento
        Private m_NomeOperatoreConsegna As CCursorFieldObj(Of String)
        Private m_Firmato As CCursorField(Of Boolean) '[BOOL] Se vero indica che il documento è stato firmato
        Private m_StatoConsegna As CCursorField(Of Integer) '[INT] 0 in preaccettazione, 1 in gestione, 2 spedito, 3 fermo, 4 in consegna, 5 consegnato, 255 errore
        Private m_Note As CCursorFieldObj(Of String) '[TEXT]
        Private m_Progressivo As New CCursorField(Of Integer)("Progressivo")
        Private m_Verificato As New CCursorField(Of Boolean)("Verificato")

        Public Sub New()
            Me.m_IDDocumento = New CCursorField(Of Integer)("Documento")
            Me.m_IDPratica = New CCursorField(Of Integer)("Pratica")
            Me.m_DataCaricamento = New CCursorField(Of Date)("DataCaricamento")
            Me.m_IDOperatoreCaricamento = New CCursorField(Of Integer)("IDOpCaricamento")
            Me.m_NomeOperatoreCaricamento = New CCursorFieldObj(Of String)("NmOpCaricamento")
            Me.m_DataInizioSpedizione = New CCursorField(Of Date)("DataInizioSpedizione")
            Me.m_IDOperatoreSpedizione = New CCursorField(Of Integer)("IDOpSpedizione")
            Me.m_NomeOperatoreSpedizione = New CCursorFieldObj(Of String)("NmOpSpedizione")
            Me.m_DataConsegna = New CCursorField(Of Date)("DataConsegna")
            Me.m_IDOperatoreConsegna = New CCursorField(Of Integer)("IDOpConsegna")
            Me.m_NomeOperatoreConsegna = New CCursorFieldObj(Of String)("NmOpConsegna")
            Me.m_Firmato = New CCursorField(Of Boolean)("Firmato")
            Me.m_StatoConsegna = New CCursorField(Of Integer)("StatoConsegna")
            Me.m_Note = New CCursorFieldObj(Of String)("Notes")
        End Sub

        Public ReadOnly Property Verificato As CCursorField(Of Boolean)
            Get
                Return Me.m_Verificato
            End Get
        End Property

        Public ReadOnly Property Progressivo As CCursorField(Of Integer)
            Get
                Return Me.m_Progressivo
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property IDDocumento As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumento
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
            End Get
        End Property

        Public ReadOnly Property DataCaricamento As CCursorField(Of Date)
            Get
                Return Me.m_DataCaricamento
            End Get
        End Property

        Public ReadOnly Property IDOperatoreCaricamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreCaricamento
            End Get
        End Property

        Public ReadOnly Property NomeOperatoreCaricamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatoreCaricamento
            End Get
        End Property

        Public ReadOnly Property DataInizioSpedizione As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioSpedizione
            End Get
        End Property

        Public ReadOnly Property IDOperatoreSpedizione As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreSpedizione
            End Get
        End Property

        Public ReadOnly Property NomeOperatoreSpedizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatoreSpedizione
            End Get
        End Property

        Public ReadOnly Property DataConsegna As CCursorField(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
        End Property

        Public ReadOnly Property IDOperatoreConsegna As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreConsegna
            End Get
        End Property

        Public ReadOnly Property NomeOperatoreConsegna As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatoreConsegna
            End Get
        End Property

        Public ReadOnly Property Firmato As CCursorField(Of Boolean)
            Get
                Return Me.m_Firmato
            End Get
        End Property

        Public ReadOnly Property StatoConsegna As CCursorField(Of Integer)
            Get
                Return Me.m_StatoConsegna
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CDocumentoPraticaCaricato
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_DocXPrat"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class


End Class
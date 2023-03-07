Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office

Partial Public Class CQSPD

    ''' <summary>
    ''' Rappresenta il caricamento di un Documento per una Pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumentoPraticaCaricato
        Inherits DBObject

        Private m_IDDocumento As Integer '[INT] ID del tipo di documento (CDocumentoPerPratica)
        Private m_Documento As CDocumentoXGruppoProdotti  '[CDocumentoPerPratica] Documento 
        Private m_IDPratica As Integer 'ID della pratica
        Private m_Pratica As CRapportino 'Pratica
        Private m_DataCaricamento As Nullable(Of Date) '[Date] Data ed ora di caricamento
        Private m_IDOperatoreCaricamento As Integer '[INT] ID dell'operatore che ha caricato il documento
        Private m_OperatoreCaricamento As CUser '[CUser] Operatore che ha caricato il documento
        Private m_NomeOperatoreCaricamento As String
        Private m_DataInizioSpedizione As Nullable(Of Date) '[Date] Data di inizio spedizione
        Private m_IDOperatoreSpedizione As Integer '[INT] ID dell'operatore che ha preso in carico la spedizione
        Private m_OperatoreSpedizione As CUser '[CUser] Operatore che ha preso in carico la spedizione
        Private m_NomeOperatoreSpedizione As String
        Private m_DataConsegna As Nullable(Of Date) '[Date] Data di consegna
        Private m_IDOperatoreConsegna As Integer '[INT] ID dell'operatore che consegnato il documento
        Private m_OperatoreConsegna As CUser '[CUser] Operatore che ha consegnato il documento
        Private m_NomeOperatoreConsegna As String
        Private m_Firmato As Boolean '[BOOL] Se vero indica che il documento è stato firmato
        Private m_StatoConsegna As Integer '[INT] 0 in preaccettazione, 1 in gestione, 2 spedito, 3 fermo, 4 in consegna, 5 consegnato, 255 errore
        Private m_Note As String '[TEXT]
        Private m_Progressivo As Integer
        Private m_Verificato As Boolean
        Private m_IDDocumentoCaricato As Integer
        Private m_DocumentoCaricato As CAttachment

        Public Sub New()
            Me.m_IDDocumento = 0
            Me.m_Documento = Nothing
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing
            Me.m_DataCaricamento = Nothing
            Me.m_IDOperatoreCaricamento = 0
            Me.m_OperatoreCaricamento = Nothing
            Me.m_NomeOperatoreCaricamento = ""
            Me.m_DataInizioSpedizione = Nothing
            Me.m_IDOperatoreSpedizione = 0
            Me.m_OperatoreSpedizione = Nothing
            Me.m_NomeOperatoreSpedizione = ""
            Me.m_DataConsegna = Nothing
            Me.m_IDOperatoreConsegna = 0
            Me.m_OperatoreConsegna = Nothing
            Me.m_NomeOperatoreConsegna = ""
            Me.m_Firmato = False
            Me.m_StatoConsegna = 0
            Me.m_Note = ""
            Me.m_Progressivo = 0
            Me.m_Verificato = False
            Me.m_IDDocumentoCaricato = 0
            Me.m_DocumentoCaricato = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property IDDocumentoCaricato As Integer
            Get
                Return GetID(Me.m_DocumentoCaricato, Me.m_IDDocumentoCaricato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumentoCaricato
                If (oldValue = value) Then Exit Property
                Me.m_DocumentoCaricato = Nothing
                Me.m_IDDocumentoCaricato = value
                Me.DoChanged("IDDocumentoCaricato", value, oldValue)
            End Set
        End Property

        Public Property DocumentoCaricato As CAttachment
            Get
                If (Me.m_DocumentoCaricato Is Nothing) Then Me.m_DocumentoCaricato = Attachments.GetItemById(Me.m_IDDocumentoCaricato)
                Return Me.m_DocumentoCaricato
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_DocumentoCaricato
                If (oldValue Is value) Then Exit Property
                Me.m_DocumentoCaricato = value
                Me.m_IDDocumentoCaricato = GetID(value)
                Me.DoChanged("DocumentoCaricato", value, oldValue)
            End Set
        End Property

        Public Property Verificato As Boolean
            Get
                Return Me.m_Verificato
            End Get
            Set(value As Boolean)
                If (Me.m_Verificato = value) Then Exit Property
                Me.m_Verificato = value
                Me.DoChanged("Verificato", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDDocumento As Integer
            Get
                Return GetID(Me.m_Documento, Me.m_IDDocumento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumento
                If (oldValue = value) Then Exit Property
                Me.m_IDDocumento = value
                Me.m_Documento = Nothing
                Me.DoChanged("IDDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Documento As CDocumentoXGruppoProdotti
            Get
                If (Me.m_Documento Is Nothing) Then Me.m_Documento = CQSPD.VincoliProdotto.GetItemById(Me.m_IDDocumento)
                Return Me.m_Documento
            End Get
            Set(value As CDocumentoXGruppoProdotti)
                Dim oldValue As CDocumentoXGruppoProdotti = Me.Documento
                If (oldValue = value) Then Exit Property
                Me.m_Documento = value
                Me.m_IDDocumento = GetID(value)
                Me.DoChanged("Documento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica per cui è stato caricato il documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPratica
                If oldValue = value Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
                Me.DoChanged("IDPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica per cui è stato caricato il documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pratica As CRapportino
            Get
                If (Me.m_Pratica Is Nothing) Then Me.m_Pratica = CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.Pratica
                If (oldValue = value) Then Exit Property
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
                Me.DoChanged("Pratica", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPratica(ByVal value As CRapportino)
            Me.m_Pratica = value
            Me.m_IDPratica = GetID(value)
        End Sub

        Protected Friend Sub SetProgressivo(ByVal value As Integer)
            Me.m_Progressivo = value
        End Sub

        Public Property Progressivo As Integer
            Get
                Return Me.m_Progressivo
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Progressivo
                If (oldValue = value) Then Exit Property
                Me.m_Progressivo = value
                Me.DoChanged("Progressivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di caricamento del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataCaricamento As Nullable(Of Date)
            Get
                Return Me.m_DataCaricamento
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataCaricamento
                If (oldValue = value) Then Exit Property
                Me.m_DataCaricamento = value
                Me.DoChanged("DataCaricamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha caricato il documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatoreCaricamento As Integer
            Get
                Return GetID(Me.m_OperatoreCaricamento, Me.m_IDOperatoreCaricamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreCaricamento
                If oldValue = value Then Exit Property
                Me.m_IDOperatoreCaricamento = value
                Me.m_OperatoreCaricamento = Nothing
                Me.DoChanged("IDOperatoreCaricamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha caricato il documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OperatoreCaricamento As CUser
            Get
                If (Me.m_OperatoreCaricamento Is Nothing) Then Me.m_OperatoreCaricamento = Sistema.Users.GetItemById(Me.m_IDOperatoreCaricamento)
                Return Me.m_OperatoreCaricamento
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreCaricamento
                If (oldValue = value) Then Exit Property
                Me.m_OperatoreCaricamento = value
                Me.m_IDOperatoreCaricamento = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomeOperatoreCaricamento = value.Nominativo
                Me.DoChanged("OperatoreCaricamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha caricato il documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatoreCaricamento As String
            Get
                Return Me.m_NomeOperatoreCaricamento
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatoreCaricamento
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatoreCaricamento = value
                Me.DoChanged("NomeOperatoreCaricamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Se il documento è stato spedito restituisce o imposta la data di spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioSpedizione As Nullable(Of Date)
            Get
                Return Me.m_DataInizioSpedizione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizioSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_DataInizioSpedizione = value
                Me.DoChanged("DataInizioSpedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Se il documento è stato spedito restituisce o imposta l'operatore che ha effettuato la spedizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatoreSpedizione As Integer
            Get
                Return GetID(Me.m_OperatoreSpedizione, Me.m_IDOperatoreSpedizione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreSpedizione
                If oldValue = value Then Exit Property
                Me.m_IDOperatoreSpedizione = value
                Me.m_OperatoreSpedizione = Nothing
                Me.DoChanged("IDOperatoreSpedizione", value, oldValue)
            End Set
        End Property


        Public Property OperatoreSpedizione As CUser
            Get
                If (Me.m_OperatoreSpedizione Is Nothing) Then Me.m_OperatoreSpedizione = Sistema.Users.GetItemById(Me.m_IDOperatoreSpedizione)
                Return Me.m_OperatoreSpedizione
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreSpedizione
                Me.m_OperatoreSpedizione = value
                Me.m_IDOperatoreSpedizione = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatoreSpedizione = value.Nominativo
                Me.DoChanged("OperatoreSpedizione", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatoreSpedizione As String
            Get
                Return Me.m_NomeOperatoreSpedizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatoreSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatoreSpedizione = value
                Me.DoChanged("NomeOperatoreSpedizione", value, oldValue)
            End Set
        End Property

        Public Property DataConsegna As Nullable(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConsegna
                If (oldValue = value) Then Exit Property
                Me.m_DataConsegna = value
                Me.DoChanged("DataConsegna", value, oldValue)
            End Set
        End Property

        Public Property IDOperatoreConsegna As Integer
            Get
                Return GetID(Me.m_OperatoreConsegna, Me.m_IDOperatoreConsegna)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreConsegna
                If oldValue = value Then Exit Property
                Me.m_IDOperatoreConsegna = value
                Me.m_OperatoreConsegna = Nothing
                Me.DoChanged("IDOperatoreConsegna", value, oldValue)
            End Set
        End Property

        Public Property OperatoreConsegna As CUser
            Get
                If (Me.m_OperatoreConsegna Is Nothing) Then Me.m_OperatoreConsegna = Sistema.Users.GetItemById(Me.m_IDOperatoreConsegna)
                Return Me.m_OperatoreConsegna
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreConsegna
                If (oldValue = value) Then Exit Property
                Me.m_OperatoreConsegna = value
                Me.m_IDOperatoreConsegna = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomeOperatoreConsegna = value.Nominativo
                Me.DoChanged("OperatoreConsegna", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatoreConsegna As String
            Get
                Return Me.m_NomeOperatoreConsegna
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatoreConsegna
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatoreConsegna = value
                Me.DoChanged("NomeOperatoreConsegna", value, oldValue)
            End Set
        End Property

        Public Property Firmato As Boolean
            Get
                Return Me.m_Firmato
            End Get
            Set(value As Boolean)
                If (Me.m_Firmato = value) Then Exit Property
                Me.m_Firmato = value
                Me.DoChanged("Firmato", value, Not value)
            End Set
        End Property

        Public Property StatoConsegna As Integer
            Get
                Return Me.m_StatoConsegna
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_StatoConsegna
                If (oldValue = value) Then Exit Property
                Me.m_StatoConsegna = value
                Me.DoChanged("StatoConsegna", value, oldValue)
            End Set
        End Property

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_DocXPrat"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDDocumento = reader.Read("Documento", Me.m_IDDocumento)
            Me.m_IDPratica = reader.Read("Pratica", Me.m_IDPratica)
            Me.m_DataCaricamento = reader.Read("DataCaricamento", Me.m_DataCaricamento)
            Me.m_IDOperatoreCaricamento = reader.Read("IDOpCaricamento", Me.m_IDOperatoreCaricamento)
            Me.m_NomeOperatoreCaricamento = reader.Read("NmOpCaricamento", Me.m_NomeOperatoreCaricamento)
            Me.m_DataInizioSpedizione = reader.Read("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            Me.m_IDOperatoreSpedizione = reader.Read("IDOpSpedizione", Me.m_IDOperatoreSpedizione)
            Me.m_NomeOperatoreSpedizione = reader.Read("NmOpSpedizione", Me.m_NomeOperatoreSpedizione)
            Me.m_DataConsegna = reader.Read("DataConsegna", Me.m_DataConsegna)
            Me.m_IDOperatoreConsegna = reader.Read("IDOpConsegna", Me.m_IDOperatoreConsegna)
            Me.m_NomeOperatoreConsegna = reader.Read("NmOpConsegna", Me.m_NomeOperatoreConsegna)
            Me.m_Firmato = reader.Read("Firmato", Me.m_Firmato)
            Me.m_StatoConsegna = reader.Read("StatoConsegna", Me.m_StatoConsegna)
            Me.m_Note = reader.Read("Notes", Me.m_Note)
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Me.m_Verificato = reader.Read("Verificato", Me.m_Verificato)
            Me.m_IDDocumentoCaricato = reader.Read("IDDocumentoCaricato", Me.m_IDDocumentoCaricato)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Documento", Me.IDDocumento)
            writer.Write("Pratica", Me.IDPratica)
            writer.Write("DataCaricamento", Me.m_DataCaricamento)
            writer.Write("IDOpCaricamento", Me.IDOperatoreCaricamento)
            writer.Write("NmOpCaricamento", Me.m_NomeOperatoreCaricamento)
            writer.Write("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            writer.Write("IDOpSpedizione", Me.IDOperatoreSpedizione)
            writer.Write("NmOpSpedizione", Me.m_NomeOperatoreSpedizione)
            writer.Write("DataConsegna", Me.m_DataConsegna)
            writer.Write("IDOpConsegna", Me.IDOperatoreConsegna)
            writer.Write("NmOpConsegna", Me.m_NomeOperatoreConsegna)
            writer.Write("Firmato", Me.m_Firmato)
            writer.Write("StatoConsegna", Me.m_StatoConsegna)
            writer.Write("Notes", Me.m_Note)
            writer.Write("Progressivo", Me.m_Progressivo)
            writer.Write("Verificato", Me.m_Verificato)
            writer.Write("IDDocumentoCaricato", Me.IDDocumentoCaricato)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Note
        End Function

        '---------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("DataCaricamento", Me.m_DataCaricamento)
            writer.WriteAttribute("IDOperatoreCaricamento", Me.IDOperatoreCaricamento)
            writer.WriteAttribute("NomeOperatoreCaricamento", Me.m_NomeOperatoreCaricamento)
            writer.WriteAttribute("DataInizioSpedizione", Me.m_DataInizioSpedizione)
            writer.WriteAttribute("IDOperatoreSpedizione", Me.IDOperatoreSpedizione)
            writer.WriteAttribute("NomeOperatoreSpedizione", Me.m_NomeOperatoreSpedizione)
            writer.WriteAttribute("DataConsegna", Me.m_DataConsegna)
            writer.WriteAttribute("IDOperatoreConsegna", Me.IDOperatoreConsegna)
            writer.WriteAttribute("NomeOperatoreConsegna", Me.m_NomeOperatoreConsegna)
            writer.WriteAttribute("Firmato", Me.m_Firmato)
            writer.WriteAttribute("StatoConsegna", Me.m_StatoConsegna)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            writer.WriteAttribute("Verificato", Me.m_Verificato)
            writer.WriteAttribute("IDDocumentoCaricato", Me.IDDocumentoCaricato)
            writer.WriteAttribute("IDDocumento", Me.IDDocumento)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
            writer.WriteTag("Documento", Me.Documento)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "DataCaricamento" : Me.m_DataCaricamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatoreCaricamento" : Me.m_IDOperatoreCaricamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatoreCaricamento" : Me.m_NomeOperatoreCaricamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizioSpedizione" : Me.m_DataInizioSpedizione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatoreSpedizione" : Me.m_IDOperatoreSpedizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatoreSpedizione" : Me.m_NomeOperatoreSpedizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataConsegna" : Me.m_DataConsegna = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatoreConsegna" : Me.m_IDOperatoreConsegna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatoreConsegna" : Me.m_NomeOperatoreConsegna = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Firmato" : Me.m_Firmato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "StatoConsegna" : Me.m_StatoConsegna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Progressivo" : Me.m_Progressivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Verificato" : Me.m_Verificato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDDocumentoCaricato" : Me.m_IDDocumentoCaricato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDocumento" : Me.m_IDDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Documento" : Me.m_Documento = XML.Utils.Serializer.ToObject(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class
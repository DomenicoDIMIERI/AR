Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office

Partial Public Class CQSPD

       
    ''' <summary>
    ''' Azione da effettuare
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DocumentoXProdottoDisposition As Integer
        ''' <summary>
        ''' Costituisce semplicemente una verifica
        ''' </summary>
        ''' <remarks></remarks>
        VERIFICA = 0
        ''' <summary>
        ''' Carica a sistema
        ''' </summary>
        ''' <remarks></remarks>
        CARICA = 1
        ''' <summary>
        ''' Stampa
        ''' </summary>
        ''' <remarks></remarks>
        STAMPA = 2
        ''' <summary>
        ''' Firma
        ''' </summary>
        ''' <remarks></remarks>
        FIRMA = 3
        ''' <summary>
        ''' Spedisci
        ''' </summary>
        ''' <remarks></remarks>
        SPEDISCI = 4
        ''' <summary>
        ''' Archivia
        ''' </summary>
        ''' <remarks></remarks>
        ARCHIVIA = 5
        ''' <summary>
        ''' Distruggi
        ''' </summary>
        ''' <remarks></remarks>
        DISTRUGGI = 6

        ''' <summary>
        ''' Il vincolo indica che la pratica richiede l'approvazione di un supervisore
        ''' </summary>
        ''' <remarks></remarks>
        APPROVA = 7
    End Enum

    ''' <summary>
    ''' Flags per i vincoli sui prodotti
    ''' </summary>
    ''' <remarks></remarks>
    <Flags> _
    Public Enum VincoliProdottoFlags As Integer
        ''' <summary>
        ''' Nessun flag
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' Se vero indica che il vincolo è visibile nella scheda amministrazione
        ''' </summary>
        ''' <remarks></remarks>
        Visible = 1
    End Enum

    ''' <summary>
    ''' Rappresenta un documento caricabile per un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumentoXGruppoProdotti
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String                                    'Nome del vincolo
        Private m_Descrizione As String                             'Descrizione del vincolo
        Private m_Progressivo As Integer                            'Ordine di verifica del vincolo
        Private m_Espressione As String                             'Espressione che viene eseguita nel contesto client per veificare il vincolo

        Private m_IDGruppoProdotti As Integer                       'ID del gruppo prodotti a cui si applica il vincolo
        Private m_GruppoProdotti As CGruppoProdotti                 'Gruppo prodotti a cui si applica il vincolo
        Private m_IDDocumento As Integer                            'ID del documento caricato
        Private m_Documento As CDocumento                           'Documento caricato
        Private m_Disposizione As DocumentoXProdottoDisposition     'Azione che deve essere eseguita affinchè il vincolo sia verificato
        Private m_Richiesto As Boolean                              'Vero se il verificarsi del vincolo è obbligatorio
        Private m_IDStatoPratica As Integer                         'ID dello stato pratica a cui si applica il vincolo
        Private m_StatoPratica As CStatoPratica                     'Stato pratica a cui si applica il vincolo
        Private m_Flags As VincoliProdottoFlags                     'Flags

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_Progressivo = 0
            Me.m_Espressione = ""

            Me.m_IDGruppoProdotti = 0
            Me.m_GruppoProdotti = Nothing
            Me.m_IDDocumento = 0
            Me.m_Documento = Nothing
            Me.m_Disposizione = DocumentoXProdottoDisposition.VERIFICA
            Me.m_Richiesto = False
            Me.m_IDStatoPratica = 0
            Me.m_StatoPratica = Nothing
            Me.m_Flags = VincoliProdottoFlags.Visible
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.VincoliProdotto.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As VincoliProdottoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As VincoliProdottoFlags)
                Dim oldValue As VincoliProdottoFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del vincolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione del vincolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ordine di priorità del vincolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        Protected Friend Sub SetProgressivo(ByVal value As Integer)
            Me.m_Progressivo = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il codice che viene eseguito nel cliente per verificare il vincolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Espressione As String
            Get
                Return Me.m_Espressione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Espressione
                If (oldValue = value) Then Exit Property
                Me.m_Espressione = value
                Me.DoChanged("Espressione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'azione da compiere per il documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Disposizione As DocumentoXProdottoDisposition
            Get
                Return Me.m_Disposizione
            End Get
            Set(value As DocumentoXProdottoDisposition)
                Dim oldValue As DocumentoXProdottoDisposition = Me.m_Disposizione
                If (oldValue = value) Then Exit Property
                Me.m_Disposizione = value
                Me.DoChanged("Disposizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del prodotto associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDGruppoProdotti As Integer
            Get
                Return GetID(Me.m_GruppoProdotti, Me.m_IDGruppoProdotti)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDGruppoProdotti
                If oldValue = value Then Exit Property
                Me.m_IDGruppoProdotti = value
                Me.m_GruppoProdotti = Nothing
                Me.DoChanged("IDGruppoProdotti", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il gruppo prodotti associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GruppoProdotti As CGruppoProdotti
            Get
                If Me.m_GruppoProdotti Is Nothing Then Me.m_GruppoProdotti = CQSPD.GruppiProdotto.GetItemById(Me.m_IDGruppoProdotti)
                Return Me.m_GruppoProdotti
            End Get
            Set(value As CGruppoProdotti)
                Dim oldValue As CGruppoProdotti = Me.m_GruppoProdotti
                If (oldValue = value) Then Exit Property
                Me.m_GruppoProdotti = value
                Me.m_IDGruppoProdotti = GetID(value)
                Me.DoChanged("GruppoProdotti", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetGruppoProdotti(ByVal value As CGruppoProdotti)
            Me.m_GruppoProdotti = value
            Me.m_IDGruppoProdotti = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento associato
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
                If oldValue = value Then Exit Property
                Me.m_IDDocumento = value
                Me.m_Documento = Nothing
                Me.DoChanged("IDDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il documento associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Documento As CDocumento
            Get
                If (Me.m_Documento Is Nothing) Then Me.m_Documento = GDE.GetItemById(Me.m_IDDocumento)
                Return Me.m_Documento
            End Get
            Set(value As CDocumento)
                Dim oldValue As CDocumento = Me.m_Documento
                If (oldValue = value) Then Exit Property
                Me.m_Documento = value
                Me.m_IDDocumento = GetID(value)
                Me.DoChanged("Documento", value, oldValue)
            End Set
        End Property

        'Private Function GetStatoIndex(ByVal stato As StatoPraticaEnum) As Integer
        '    Select Case stato
        '        Case StatoPraticaEnum.STATO_CONTATTO : Return 0
        '        Case StatoPraticaEnum.STATO_RICHIESTADELIBERA : Return 1
        '        Case StatoPraticaEnum.STATO_DELIBERATA : Return 2
        '        Case StatoPraticaEnum.STATO_LIQUIDATA : Return 3
        '        Case StatoPraticaEnum.STATO_ARCHIVIATA : Return 4
        '        Case StatoPraticaEnum.STATO_ANNULLATA : Return 5
        '        Case Else : Throw New ArgumentOutOfRangeException("Stato non valido: " & [Enum].GetName(GetType(StatoPraticaEnum), stato))
        '    End Select
        'End Function

        'Public Property Richiesto(ByVal stato As StatoPraticaEnum) As Boolean
        '    Get
        '        Return Me.m_RichiestoPerStati(Me.GetStatoIndex(stato))
        '    End Get
        '    Set(value As Boolean)
        '        Me.m_RichiestoPerStati(Me.GetStatoIndex(stato)) = value
        '    End Set
        'End Property

        'Private Function GetStrRichiesto() As String
        '    Dim str As String = vbNullString
        '    If (Me.Richiesto(StatoPraticaEnum.STATO_CONTATTO)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_CONTATTO, ",")
        '    If (Me.Richiesto(StatoPraticaEnum.STATO_RICHIESTADELIBERA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_RICHIESTADELIBERA, ",")
        '    If (Me.Richiesto(StatoPraticaEnum.STATO_DELIBERATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_DELIBERATA, ",")
        '    If (Me.Richiesto(StatoPraticaEnum.STATO_LIQUIDATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_LIQUIDATA, ",")
        '    If (Me.Richiesto(StatoPraticaEnum.STATO_ARCHIVIATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_ARCHIVIATA, ",")
        '    If (Me.Richiesto(StatoPraticaEnum.STATO_ANNULLATA)) Then str = Strings.Combine(str, StatoPraticaEnum.STATO_ANNULLATA, ",")
        '    Return str
        'End Function

        'Private Sub FromStrRichiesto(ByVal text As String)
        '    For i As Integer = 0 To UBound(Me.m_RichiestoPerStati)
        '        Me.m_RichiestoPerStati(i) = False
        '    Next
        '    text = Trim(text)
        '    If (text <> "") Then
        '        Dim str() As String = Split(text, ",")
        '        For i As Integer = 0 To UBound(str)
        '            Me.m_RichiestoPerStati(Me.GetStatoIndex(Formats.ToInteger(str(i)))) = True
        '        Next
        '    End If
        'End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se questa azione è necessaria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Richiesto As Boolean
            Get
                Return Me.m_Richiesto
            End Get
            Set(value As Boolean)
                If (Me.m_Richiesto = value) Then Exit Property
                Me.m_Richiesto = value
                Me.DoChanged("Richiesto", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato a cui si applica la regola.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStatoPratica As Integer
            Get
                Return GetID(Me.m_StatoPratica, Me.m_IDStatoPratica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoPratica
                If (oldValue = value) Then Exit Property
                Me.m_IDStatoPratica = value
                Me.m_StatoPratica = Nothing
                Me.DoChanged("IDStatoPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato pratica a cui si applica la regola (se NULL la regola si applica a tutti gli stati)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoPratica As CStatoPratica
            Get
                If (Me.m_StatoPratica Is Nothing) Then Me.m_StatoPratica = CQSPD.StatiPratica.GetItemById(Me.m_IDStatoPratica)
                Return Me.m_StatoPratica
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_StatoPratica
                If (oldValue Is value) Then Exit Property
                Me.m_StatoPratica = value
                Me.m_IDStatoPratica = GetID(value)
                Me.DoChanged("StatoPratica", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DocumentiXGruppoProdotti"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDGruppoProdotti = reader.Read("GruppoProdotti", Me.m_IDGruppoProdotti)
            Me.m_IDDocumento = reader.Read("Documento", Me.m_IDDocumento)
            Me.m_Disposizione = reader.Read("Disposizione", Me.m_Disposizione)
            Me.m_Richiesto = reader.Read("Richiesto", Me.m_Richiesto)
            Me.m_IDStatoPratica = reader.Read("IDStatoPratica", Me.m_IDStatoPratica)
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Espressione = reader.Read("Espressione", Me.m_Espressione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("GruppoProdotti", Me.IDGruppoProdotti)
            writer.Write("Documento", Me.IDDocumento)
            writer.Write("Disposizione", Me.m_Disposizione)
            writer.Write("Richiesto", Me.m_Richiesto)
            writer.Write("IDStatoPratica", Me.IDStatoPratica)
            writer.Write("Progressivo", Me.m_Progressivo)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Espressione", Me.m_Espressione)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDGruppoProdotti", Me.IDGruppoProdotti)
            writer.WriteAttribute("IDDocumento", Me.IDDocumento)
            writer.WriteAttribute("Disposizione", Me.m_Disposizione)
            writer.WriteAttribute("Richiesto", Me.m_Richiesto)
            writer.WriteAttribute("IDStatoPratica", Me.IDStatoPratica)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Espressione", Me.m_Espressione)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDGruppoProdotti" : Me.m_IDGruppoProdotti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDocumento" : Me.m_IDDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Disposizione" : Me.m_Disposizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Richiesto" : Me.m_Richiesto = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDStatoPratica" : Me.m_IDStatoPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Progressivo" : Me.m_Progressivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Espressione" : Me.m_Espressione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.VincoliProdotto.UpdateCached(Me)
            If Me.GruppoProdotti IsNot Nothing Then
                SyncLock Me.GruppoProdotti
                    Dim item As CDocumentoXGruppoProdotti = Me.GruppoProdotti.Documenti.GetItemById(GetID(Me))
                    Dim i As Integer = -1
                    If (item IsNot Nothing) Then i = Me.GruppoProdotti.Documenti.IndexOf(item)
                    If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                        If (i < 0) Then
                            Me.GruppoProdotti.Documenti.Add(Me)
                        Else
                            Me.GruppoProdotti.Documenti(i) = Me
                        End If
                        Me.GruppoProdotti.Documenti.Sort()
                    Else
                        If (i >= 0) Then Me.GruppoProdotti.Documenti.RemoveAt(i)
                    End If
                End SyncLock
            End If
        End Sub

        Public Function CompareTo(obj As CDocumentoXGruppoProdotti) As Integer
            Dim ret As Integer = Arrays.Compare(Me.m_Progressivo, obj.m_Progressivo)
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, obj.m_Nome)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function


    End Class


End Class
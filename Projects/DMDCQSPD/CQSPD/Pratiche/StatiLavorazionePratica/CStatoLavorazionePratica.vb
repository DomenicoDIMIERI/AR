Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Rappresenta uno stato di lavorazione di una pratica cioè un passaggio di stato
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CStatoLavorazionePratica
        Inherits DBObject
        Implements IComparable

        Private m_MacroStato As Nullable(Of StatoPraticaEnum) 'Macrostato
        Private m_IDPratica As Integer                  'ID della pratica che ha subito il passaggio
        Private m_Pratica As CRapportino                'Pratica che ha subito il passaggio
        Private m_IDFromStato As Integer                'ID dello stato precedente
        Private m_FromStato As CStatoLavorazionePratica 'Stato precedente
        Private m_RegolaApplicata As CStatoPratRule     'Regola applicata
        Private m_IDRegolaApplicata As Integer          'ID della regola applicata
        Private m_IDStatoPratica As Integer             'ID dello stato successivo
        Private m_DescrizioneStato As String            'Descrizione dello stato attuale
        Private m_StatoPratica As CStatoPratica         'Stato successivo
        Private m_Data As Nullable(Of Date)             'Data ed ora del passaggio di stato
        Private m_IDOperatore As Integer                'ID dell'operatore che ha effettuato il passaggio
        Private m_Operatore As CUser                    'Operatore che ha effettuato il passaggio
        Private m_NomeOperatore As String               'Nome dell'operatore che ha effettuato il passaggio
        Private m_Note As String                        'Annotazioni sul passaggio
        Private m_Params As String                      'Parametri del passaggio di stato
        Private m_Forzato As Boolean                    'Vero se il passaggio è stato forzato a dispetto dei vincoli
        Private m_Attachment As String                  '
        Private m_IDOfferta As Integer                  'ID dell'offerta corrente al momento del passaggio
        Private m_Offerta As COffertaCQS                'Offerta corrente al momento del passaggio

        Public Sub New()
            Me.m_MacroStato = Nothing
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing
            Me.m_IDFromStato = 0
            Me.m_FromStato = Nothing
            Me.m_IDStatoPratica = 0
            Me.m_StatoPratica = Nothing
            Me.m_Data = Nothing
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = vbNullString
            Me.m_Note = vbNullString
            Me.m_Params = vbNullString
            Me.m_Forzato = False
            Me.m_Attachment = vbNullString
            Me.m_IDOfferta = 0
            Me.m_Offerta = Nothing
            Me.m_DescrizioneStato = vbNullString
            Me.m_RegolaApplicata = Nothing
            Me.m_IDRegolaApplicata = 0
        End Sub

        Public Sub New(ByVal macroStato As StatoPraticaEnum)
            Me.New()
            Me.m_MacroStato = macroStato
        End Sub

        Public Property IDRegolaApplicata As Integer
            Get
                Return GetID(Me.m_RegolaApplicata, Me.m_IDRegolaApplicata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRegolaApplicata
                If (oldValue = value) Then Exit Property
                Me.m_IDRegolaApplicata = value
                Me.m_RegolaApplicata = Nothing
                Me.DoChanged("IDRegolaApplicata", value, oldValue)
            End Set
        End Property

        Public Property RegolaApplicata As CStatoPratRule
            Get
                If (Me.m_RegolaApplicata Is Nothing) Then Me.m_RegolaApplicata = DMD.CQSPD.StatiPratRules.GetItemById(Me.m_IDRegolaApplicata)
                Return Me.m_RegolaApplicata
            End Get
            Set(value As CStatoPratRule)
                Dim oldValue As CStatoPratRule = Me.m_RegolaApplicata
                If (oldValue Is value) Then Exit Property
                Me.m_RegolaApplicata = value
                Me.m_IDRegolaApplicata = GetID(value)
                Me.DoChanged("RegolaApplicata", value, oldValue)
            End Set
        End Property

        Public Property DescrizioneStato As String
            Get
                Return Me.m_DescrizioneStato
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DescrizioneStato
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneStato = value
                Me.DoChanged("DescrizioneStato", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return StatiLavorazionePratica.Module
        End Function

        Public Property IDFromStato As Integer
            Get
                Return GetID(Me.m_FromStato, Me.m_IDFromStato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFromStato
                If (oldValue = value) Then Exit Property
                Me.m_IDFromStato = value
                Me.m_FromStato = Nothing
                Me.DoChanged("IDFromStato", value, oldValue)
            End Set
        End Property

        Public Property FromStato As CStatoLavorazionePratica
            Get
                If (Me.m_FromStato Is Nothing) Then Me.m_FromStato = DMD.CQSPD.StatiLavorazionePratica.GetItemById(Me.m_IDFromStato)
                Return Me.m_FromStato
            End Get
            Set(value As CStatoLavorazionePratica)
                Dim oldValue As CStatoLavorazionePratica = Me.m_FromStato
                If (oldValue Is value) Then Exit Property
                Me.m_FromStato = value
                Me.m_IDFromStato = GetID(value)
                Me.DoChanged("FromStato", value, oldValue)
            End Set
        End Property

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

        Public Property StatoPratica As CStatoPratica
            Get
                If (Me.m_StatoPratica Is Nothing) Then Me.m_StatoPratica = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDStatoPratica)
                Return Me.m_StatoPratica
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_StatoPratica
                If (oldValue Is value) Then Exit Property
                Me.m_StatoPratica = value
                Me.m_IDStatoPratica = GetID(value)
                If (value IsNot Nothing) Then Me.m_DescrizioneStato = value.Nome
                Me.DoChanged("StatoPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore numerico che indica lo stato della pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MacroStato As Nullable(Of StatoPraticaEnum)
            Get
                Return Me.m_MacroStato
            End Get
            Set(value As Nullable(Of StatoPraticaEnum))
                Dim oldValue As Nullable(Of StatoPraticaEnum) = Me.m_MacroStato
                If (oldValue = value) Then Exit Property
                Me.m_MacroStato = value
                Me.DoChanged("MacroStato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica associata
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
        ''' Restituisce o imposta l'oggetto pratica a cui è associato lo stato di lavorazione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pratica As CRapportino
            Get
                If Me.m_Pratica Is Nothing Then Me.m_Pratica = DMD.CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
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

        ''' <summary>
        ''' Restituisce o imposta la data in cui la pratica è passata nello stato di lavorazione indicato da questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Nullable(Of Date)
            Get
                Return Me.m_Data
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_Data
                If (oldValue = value) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha messo la pratica nello stato di lavorazione rappresentato da questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If oldValue = value Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un oggetto CUser che rappresenta l'utente che ha messo la pratica nello stato di lavorazione rappresentato da questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If Me.m_Operatore Is Nothing Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue = value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha messo la pratica nello stato di lavorazione rappresentato da questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituise o imposta una stringa usata come campo note
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta una stringa usata come campo parametri
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Params As String
            Get
                Return Me.m_Params
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Params
                If (oldValue = value) Then Exit Property
                Me.m_Params = value
                Me.DoChanged("Params", value, oldValue)
            End Set
        End Property

        Public Property Attachment As String
            Get
                Return Me.m_Attachment
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Attachment
                If (oldValue = value) Then Exit Property
                Me.m_Attachment = value
                Me.DoChanged("Attachment", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se lo stato è stato forzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Forzato As Boolean
            Get
                Return Me.m_Forzato
            End Get
            Set(value As Boolean)
                If (Me.m_Forzato = value) Then Exit Property
                Me.m_Forzato = value
                Me.DoChanged("Forzato", value, Not value)
            End Set
        End Property

        Public Property IDOfferta As Integer
            Get
                Return GetID(Me.m_Offerta, Me.m_IDOfferta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOfferta
                If (value = oldValue) Then Exit Property
                Me.m_IDOfferta = value
                Me.m_Offerta = Nothing
                Me.DoChanged("IDOfferta", value, oldValue)
            End Set
        End Property

        Public Property Offerta As COffertaCQS
            Get
                If (Me.m_Offerta Is Nothing) Then Me.m_Offerta = DMD.CQSPD.Offerte.GetItemById(Me.m_IDOfferta)
                Return Me.m_Offerta
            End Get
            Set(value As COffertaCQS)
                If (value Is Me.m_Offerta) Then Exit Property
                Me.m_Offerta = value
                Me.m_IDOfferta = GetID(value)
                Me.DoChanged("Offerta", value)
            End Set
        End Property

        Protected Friend Overridable Sub SetOfferta(ByVal value As COffertaCQS)
            Me.m_Offerta = value
            Me.m_IDOfferta = GetID(value)
        End Sub

        '------------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("MacroStato", Me.m_MacroStato)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("Params", Me.m_Params)
            writer.WriteAttribute("Attachment", Me.m_Attachment)
            writer.WriteAttribute("Forzato", Me.m_Forzato)
            writer.WriteAttribute("IDOfferta", Me.IDOfferta)
            writer.WriteAttribute("IDFromStato", Me.IDFromStato)
            writer.WriteAttribute("IDToStato", Me.IDStatoPratica)
            writer.WriteAttribute("DescrizioneStato", Me.m_DescrizioneStato)
            writer.WriteAttribute("IDRegolaApplicata", Me.IDRegolaApplicata)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
            ' writer.WriteTag("StatoPratica", Me.StatoPratica)
            'writer.WriteTag("OffertaCorrente", Me.Offerta)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "MacroStato" : Me.m_MacroStato = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Params" : Me.m_Params = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attachment" : Me.m_Attachment = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Forzato" : Me.m_Forzato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDOfferta" : Me.m_IDOfferta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDFromStato" : Me.m_IDFromStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDToStato" : Me.m_IDStatoPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DescrizioneStato" : Me.m_DescrizioneStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRegolaApplicata" : Me.m_IDRegolaApplicata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoPratica" : Me.m_StatoPratica = fieldValue
                    'Case "OffertaCorrente" : Me.m_Offerta = fieldValue
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheSTL"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_MacroStato = reader.GetValue(Of Integer)("MacroStato", 0)
            Me.m_IDPratica = reader.Read("IDPratica", Me.m_IDPratica)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_Params = reader.Read("Parameters", Me.m_Params)
            'Me.m_Attachment = reader.Read("Attachment", Me.m_Attachment)
            Me.m_Forzato = reader.Read("Forzato", Me.m_Forzato)
            Me.m_IDOfferta = reader.Read("IDOfferta", Me.m_IDOfferta)
            Me.m_IDFromStato = reader.Read("IDFromStato", Me.m_IDFromStato)
            Me.m_IDStatoPratica = reader.Read("IDToStato", Me.m_IDStatoPratica)
            Me.m_DescrizioneStato = reader.Read("DescrizioneStato", Me.m_DescrizioneStato)
            Me.m_IDRegolaApplicata = reader.Read("IDRegolaApplicata", Me.m_IDRegolaApplicata)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("MacroStato", Me.m_MacroStato)
            writer.Write("IDPratica", Me.IDPratica)
            writer.Write("Data", Me.m_Data)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("Note", Me.m_Note)
            writer.Write("Parameters", Me.m_Params)
            writer.Write("Forzato", Me.m_Forzato)
            'writer.Write("Attachment", Me.m_Attachment)
            writer.Write("IDOfferta", Me.IDOfferta)
            writer.Write("IDFromStato", Me.IDFromStato)
            writer.Write("IDToStato", Me.IDStatoPratica)
            writer.Write("DescrizioneStato", Me.m_DescrizioneStato)
            writer.Write("IDRegolaApplicata", Me.IDRegolaApplicata)
            'writer.Write("DataStr", DBUtils.ToDBDateStr(Me.m_Data))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_DescrizioneStato
        End Function

        Protected Friend Sub SetPratica(ByVal value As CRapportino)
            Me.m_Pratica = value
            Me.m_IDPratica = GetID(value)
        End Sub

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function CompareTo(ByVal item As CStatoLavorazionePratica)
            Dim ret As Integer = DateUtils.Compare(Me.Data, item.Data)
            If (ret = 0) Then ret = Arrays.Compare(Me.ID, item.ID)
            Return ret
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            With DirectCast(value, CStatoLavorazionePratica)
                Me.m_MacroStato = .m_MacroStato
                'Me.m_IDPratica = .m_IDPratica
                'Me.m_Pratica = .m_Pratica
                Me.m_IDFromStato = .m_IDFromStato
                Me.m_FromStato = .m_FromStato
                Me.m_RegolaApplicata = .m_RegolaApplicata
                Me.m_IDRegolaApplicata = .m_IDRegolaApplicata
                Me.m_IDStatoPratica = .m_IDStatoPratica
                Me.m_DescrizioneStato = .m_DescrizioneStato
                Me.m_StatoPratica = .m_StatoPratica
                Me.m_Data = .m_Data
                Me.m_IDOperatore = .m_IDOperatore
                Me.m_Operatore = .m_Operatore
                Me.m_NomeOperatore = .m_NomeOperatore
                Me.m_Note = .m_Note
                Me.m_Params = .m_Params
                Me.m_Forzato = .m_Forzato
                Me.m_Attachment = .m_Attachment
                Me.m_IDOfferta = .m_IDOfferta
                Me.m_Offerta = .m_Offerta
            End With
            MyBase.InitializeFrom(value)
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            If Not (TypeOf (obj) Is CStatoLavorazionePratica) Then Return False
            Dim tmp As CStatoLavorazionePratica = obj
            Return Me.ID = tmp.ID
        End Function

    End Class

End Class
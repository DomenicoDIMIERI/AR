Imports FinSeA.Databases
Imports FinSeA.Sistema
Imports FinSeA.Office

Partial Public Class CQSPD

    ''' <summary>
    ''' Flags relativi ai vincoli sui gruppi prodotto
    ''' </summary>
    ''' <remarks></remarks>
    <Flags> _
    Public Enum VincoloGruppoProdottiFlags As Integer
        None = 0

        ''' <summary>
        ''' Il vincolo è attivo
        ''' </summary>
        ''' <remarks></remarks>
        Attivo = 1

        ''' <summary>
        ''' Il non verificarsi del vincolo richiede l'approvazione
        ''' </summary>
        ''' <remarks></remarks>
        RichiedeApprovazione = 2


    End Enum
       
  
    ''' <summary>
    ''' Rappresenta un vincolo sui prodotti.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VincoloXGruppoProdotti
        Inherits DBObject

        Private m_Nome As String                        'Nome del vincolo 
        Private m_Descrizione As String                 'Descrizione del vincolo
        Private m_IDGruppoProdotti As Integer           'ID del gruppo prodotti a cui si applica il vincolo
        Private m_GruppoProdotti As CGruppoProdotti     'Gruppo prodotti a cui si applica il vincolo
        Private m_Priorita As Integer                   'Priorità del vincolo (i vincoli vengono valutati per priorità crescende)
        Private m_Flags As VincoloGruppoProdottiFlags   'Flags
        Private m_IDMotivoApprovazione As Integer               'ID del motivo sconto predefinito
        Private m_MotivoApprovazione As CMotivoScontoPratica    'Motivo sconto predefinito
        
        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_IDGruppoProdotti = 0
            Me.m_GruppoProdotti = Nothing
            Me.m_Priorita = 0
            Me.m_Flags = VincoloGruppoProdottiFlags.None
            Me.m_IDMotivoApprovazione = 0
            Me.m_MotivoApprovazione = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.VincoliProdotto.Module
        End Function

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
            Return "(" & Me.IDGruppoProdotti & ", " & Me.IDDocumento & ")"
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
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As FinSeA.XML.XMLWriter)
            writer.WriteAttribute("IDGruppoProdotti", Me.IDGruppoProdotti)
            writer.WriteAttribute("IDDocumento", Me.IDDocumento)
            writer.WriteAttribute("Disposizione", Me.m_Disposizione)
            writer.WriteAttribute("Richiesto", Me.m_Richiesto)
            writer.WriteAttribute("IDStatoPratica", Me.IDStatoPratica)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            MyBase.XMLSerialize(writer)
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
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub
    End Class


End Class
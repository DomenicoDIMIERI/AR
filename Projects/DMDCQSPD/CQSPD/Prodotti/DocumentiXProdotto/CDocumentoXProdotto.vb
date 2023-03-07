Imports FinSeA.Databases
Imports FinSeA.Sistema
Imports FinSeA.Office

Partial Public Class CQSPD

       
  
    ''' <summary>
    ''' Rappresenta un documento caricabile per un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumentoXProdotto
        Inherits DBObject

        Private m_IDProdotto As Integer
        Private m_Prodotto As CCQSPDProdotto
        Private m_Descrizione As String
        Private m_IDDocumento As Integer
        Private m_Documento As CDocumento
        'Private m_RichiestoPerStati() As Boolean
        Private m_Disposizione As DocumentoXProdottoDisposition
        Private m_Richiesto As Boolean
        Private m_IDStatoPratica As Integer
        Private m_StatoPratica As CStatoPratica
        Private m_Progressivo As Integer

        Public Sub New()
            Me.m_Descrizione = ""
            Me.m_IDProdotto = 0
            Me.m_Prodotto = Nothing
            Me.m_IDDocumento = 0
            Me.m_Documento = Nothing
            'ReDim Me.m_RichiestoPerStati(7)
            Me.m_Disposizione = DocumentoXProdottoDisposition.VERIFICA
            Me.m_Richiesto = False
            Me.m_IDStatoPratica = 0
            Me.m_StatoPratica = Nothing
            Me.m_Progressivo = 0
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
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
        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_IDProdotto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProdotto
                If oldValue = value Then Exit Property
                Me.m_IDProdotto = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il prodotto associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If Me.m_Prodotto Is Nothing Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_IDProdotto)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.m_Prodotto
                If (oldValue = value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_IDProdotto = GetID(value)
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

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
            Return "(" & Me.IDProdotto & ", " & Me.IDDocumento & ")"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DocumentiXProdotto"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDProdotto = reader.Read("Prodotto", Me.m_IDProdotto)
            Me.m_IDDocumento = reader.Read("Documento", Me.m_IDDocumento)
            Me.m_Disposizione = reader.Read("Disposizione", Me.m_Disposizione)
            Me.m_Richiesto = reader.Read("Richiesto", Me.m_Richiesto)
            Me.m_IDStatoPratica = reader.Read("IDStatoPratica", Me.m_IDStatoPratica)
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Prodotto", Me.IDProdotto)
            writer.Write("Documento", Me.IDDocumento)
            writer.Write("Disposizione", Me.m_Disposizione)
            writer.Write("Richiesto", Me.m_Richiesto)
            writer.Write("IDStatoPratica", Me.IDStatoPratica)
            writer.Write("Progressivo", Me.m_Progressivo)
            writer.Write("Descrizione", Me.m_Descrizione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As FinSeA.XML.XMLWriter)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
            writer.WriteAttribute("IDDocumento", Me.IDDocumento)
            writer.WriteAttribute("Disposizione", Me.m_Disposizione)
            writer.WriteAttribute("Richiesto", Me.m_Richiesto)
            writer.WriteAttribute("IDStatoPratica", Me.IDStatoPratica)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDProdotto" : Me.m_IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
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
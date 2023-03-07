Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

    <Serializable> _
    Public Class CQSPDConvenzione
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String
        Private m_IDProdotto As Integer
        Private m_Prodotto As CCQSPDProdotto
        Private m_NomeProdotto As String
        Private m_Attiva As Boolean
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_MinimoCaricabile As Nullable(Of Decimal)
        Private m_MinimoCaricabileP As Nullable(Of Double)
        Private m_IDAmministrazione As Integer
        Private m_Amministrazione As CAzienda
        Private m_NomeAmministrazione As String
        Private m_TipoRapporto As String
        Private m_Aziende As AziendaXConvenzioneCollection

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_IDProdotto = 0
            Me.m_Prodotto = Nothing
            Me.m_NomeProdotto = vbNullString
            Me.m_Attiva = True
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_MinimoCaricabile = Nothing
            Me.m_MinimoCaricabileP = Nothing
            Me.m_IDAmministrazione = 0
            Me.m_Amministrazione = Nothing
            Me.m_NomeAmministrazione = ""
            Me.m_TipoRapporto = ""
            Me.m_Aziende = Nothing
        End Sub

        Public ReadOnly Property Aziende As AziendaXConvenzioneCollection
            Get
                If (Me.m_Aziende Is Nothing) Then Me.m_Aziende = New AziendaXConvenzioneCollection(Me)
                Return Me.m_Aziende
            End Get
        End Property

        Friend Sub InvalidateAziende()
            Me.m_Aziende = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'amministrazione 
        ''' </summary>
        ''' <returns></returns>
        Public Property IDAmministrazione As Integer
            Get
                Return GetID(Me.m_Amministrazione, Me.m_IDAmministrazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAmministrazione
                If (oldValue = value) Then Return
                Me.m_IDAmministrazione = value
                Me.m_Amministrazione = Nothing
                Me.DoChanged("IDAmministrazione", value, oldValue)
            End Set
        End Property

        Public Property Amministrazione As CAzienda
            Get
                If (Me.m_Amministrazione Is Nothing) Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_IDAmministrazione)
                Return Me.m_Amministrazione
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.Amministrazione
                If (oldValue Is value) Then Return
                Me.m_Amministrazione = value
                Me.m_IDAmministrazione = GetID(value)
                Me.m_NomeAmministrazione = "" : If (value IsNot Nothing) Then Me.m_NomeAmministrazione = value.Nominativo
                Me.DoChanged("Amministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'amministrazione
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeAmministrazione As String
            Get
                Return Me.m_NomeAmministrazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAmministrazione
                If (oldValue = value) Then Return
                Me.m_NomeAmministrazione = value
                Me.DoChanged("NomeAmministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo rapporto per cui è valida la convenzione.
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoRapporto As String
            Get
                Return Me.m_TipoRapporto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoRapporto
                If (oldValue = value) Then Return
                Me.m_TipoRapporto = value
                Me.DoChanged("TipoRapporto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome della convenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del prodotto in convenzione
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
                If (oldValue = value) Then Exit Property
                Me.m_IDProdotto = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il prodotto in convenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_IDProdotto)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.m_Prodotto
                If (oldValue Is value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_IDProdotto = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeProdotto = value.Nome
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del prodotto in convenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeProdotto As String
            Get
                Return Me.m_NomeProdotto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeProdotto
                If (oldValue = value) Then Exit Property
                Me.m_NomeProdotto = value
                Me.DoChanged("NomeProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la convenzione è attiva oppure no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attiva As Boolean
            Get
                Return Me.m_Attiva
            End Get
            Set(value As Boolean)
                If (value = Me.m_Attiva) Then Exit Property
                Me.m_Attiva = value
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio della convenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine della convenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore minimo caricabile (in euro)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MinimoCaricabile As Nullable(Of Decimal)
            Get
                Return Me.m_MinimoCaricabile
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_MinimoCaricabile
                If (oldValue = value) Then Exit Property
                Me.m_MinimoCaricabile = value
                Me.DoChanged("MinimoCaricabile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il minimo caricabile (in % rispetto al montante lordo)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MinimoCaricabileP As Nullable(Of Double)
            Get
                Return Me.m_MinimoCaricabileP
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_MinimoCaricabile
                If (oldValue = value) Then Exit Property
                Me.m_MinimoCaricabile = value
                Me.DoChanged("MinimoCaricabile", value, oldValue)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return Me.IsValid(DateUtils.Now)
        End Function

        Public Function IsValid(ByVal at As DateTime) As Boolean
            Return DateUtils.CheckBetween(at, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Convenzioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConvenzioni"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Attiva = reader.Read("Attiva", Me.m_Attiva)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_IDProdotto = reader.Read("IDProdotto", Me.m_IDProdotto)
            Me.m_MinimoCaricabile = reader.Read("MinimoCaricabile", Me.m_MinimoCaricabile)
            Me.m_MinimoCaricabileP = reader.Read("SogliaMinimaP", Me.m_MinimoCaricabileP)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_NomeProdotto = reader.Read("NomeProdotto", Me.m_NomeProdotto)
            Me.m_IDAmministrazione = reader.Read("IDAmministrazione", Me.m_IDAmministrazione)
            Me.m_NomeAmministrazione = reader.Read("NomeAmministrazione", Me.m_NomeAmministrazione)
            Me.m_TipoRapporto = reader.Read("TipoRapporto", Me.m_TipoRapporto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Attiva", Me.m_Attiva)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("IDProdotto", Me.IDProdotto)
            writer.Write("MinimoCaricabile", Me.m_MinimoCaricabile)
            writer.Write("SogliaMinimaP", Me.m_MinimoCaricabileP)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("NomeProdotto", Me.m_NomeProdotto)
            writer.Write("IDAmministrazione", Me.IDAmministrazione)
            writer.Write("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.Write("TipoRapporto", Me.m_TipoRapporto)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Attiva", Me.m_Attiva)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
            writer.WriteAttribute("MinimoCaricabile", Me.m_MinimoCaricabile)
            writer.WriteAttribute("MinimoCaricabileP", Me.m_MinimoCaricabileP)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("NomeProdotto", Me.m_NomeProdotto)
            writer.WriteAttribute("IDAmministrazione", Me.IDAmministrazione)
            writer.WriteAttribute("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.WriteAttribute("TipoRapporto", Me.m_TipoRapporto)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Aziende", Me.Aziende())
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Attiva" : Me.m_Attiva = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDProdotto" : Me.m_IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MinimoCaricabile" : Me.m_MinimoCaricabile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MinimoCaricabileP" : Me.m_MinimoCaricabileP = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeProdotto" : Me.m_NomeProdotto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAmministrazione" : Me.m_IDAmministrazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAmministrazione" : Me.m_NomeAmministrazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoRapporto" : Me.m_TipoRapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Aziende" : Me.m_Aziende = fieldValue : Me.m_Aziende.SetConvenzione(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function CompareTo(ByVal obj As CQSPDConvenzione) As Integer
            Dim a1 As Integer = IIf(Me.Attiva, 1, 0)
            Dim a2 As Integer = IIf(obj.Attiva, 1, 0)
            Dim ret As Integer = a1 - a2
            If (ret = 0) Then ret = Strings.Compare(Me.Nome, obj.Nome, CompareMethod.Text)
            Return ret
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Convenzioni.UpdateCached(Me)
        End Sub

    End Class


End Class

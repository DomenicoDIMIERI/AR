Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Relazione tra un finanziamento in corso ed un oggetto che lo estingue (pratica, consulenza, ecc)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class EstinzioneXEstintore
        Inherits DBObject

        Private m_Selezionata As Boolean
        Private m_Estintore As IEstintore
        Private m_TipoEstintore As String
        Private m_IDEstintore As Integer
        Private m_DataEstinzione As Date?

        Private m_IDEstinzione As Integer
        Private m_Estinzione As CEstinzione
        Private m_Parametro As String
        Private m_Correzione As Decimal

        Private m_Tipo As TipoEstinzione
        Private m_IDCessionario As Integer
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_NomeAgenzia As String
        Private m_Rata As Decimal?
        Private m_DataDecorrenza As Date?
        Private m_DataFine As Date?
        Private m_Durata As Integer?
        Private m_TAN As Double?
        Private m_TAEG As Double?
        Private m_TotaleDaEstinguere As Decimal?

        Private m_NumeroQuoteInsolute As Integer
        Private m_NumeroQuoteResidue As Integer
        Private m_NumeroQuoteScadute As Integer

        Public Sub New()
            Me.m_Selezionata = False
            Me.m_Estintore = Nothing
            Me.m_TipoEstintore = ""
            Me.m_IDEstintore = 0
            Me.m_Parametro = ""

            Me.m_IDEstinzione = 0
            Me.m_Estinzione = Nothing
            Me.m_Correzione = 0

            Me.m_Tipo = TipoEstinzione.ESTINZIONE_NO
            Me.m_IDCessionario = 0
            Me.m_Cessionario = Nothing
            Me.m_NomeCessionario = ""
            Me.m_NomeAgenzia = ""
            Me.m_Rata = Nothing
            Me.m_DataDecorrenza = Nothing
            Me.m_DataFine = Nothing
            Me.m_Durata = Nothing
            Me.m_TAN = Nothing
            Me.m_TAEG = Nothing
            Me.m_TotaleDaEstinguere = Nothing

            Me.m_DataEstinzione = Nothing
            Me.m_NumeroQuoteInsolute = 0
            Me.m_NumeroQuoteResidue = 0
            Me.m_NumeroQuoteScadute = 0
        End Sub

        Public Property Tipo As TipoEstinzione
            Get
                Return Me.m_Tipo
            End Get
            Set(value As TipoEstinzione)
                Dim oldValue As TipoEstinzione = Me.m_Tipo
                If (oldValue = value) Then Return
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public Property IDCessionario As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_IDCessionario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCessionario
                If (oldValue = value) Then Return
                Me.m_IDCessionario = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("IDCessionario", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = CQSPD.Cessionari.GetItemById(Me.m_IDCessionario)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue Is value) Then Return
                Me.m_Cessionario = value
                Me.m_IDCessionario = GetID(value)
                Me.m_NomeCessionario = "" : If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCessionario
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

        Public Property NomeAgenzia As String
            Get
                Return Me.m_NomeAgenzia
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAgenzia
                If (oldValue = value) Then Return
                value = Strings.Trim(value)
                Me.m_NomeAgenzia = value
                Me.DoChanged("NomeAgenzia", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il valore della rata mensile al momento del calcolo
        ''' </summary>
        ''' <returns></returns>
        Public Property Rata As Decimal?
            Get
                Return Me.m_Rata
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_Rata
                If (oldValue = value) Then Return
                Me.m_Rata = value
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultima rata del prestito in corso
        ''' </summary>
        ''' <returns></returns>
        Public Property DataFine As DateTime?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata del prestito in corso
        ''' </summary>
        ''' <returns></returns>
        Public Property Durata As Integer?
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_Durata
                If (oldValue = value) Then Return
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        Public Property TAN As Double?
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAN
                If (oldValue = value) Then Return
                Me.m_TAN = value
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

        Public Property TAEG As Double?
            Get
                Return Me.m_TAEG
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAEG
                If (oldValue = value) Then Return
                Me.m_TAEG = value
                Me.DoChanged("TAEG", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'oggetto è selezionato per estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Selezionata As Boolean
            Get
                Return Me.m_Selezionata
            End Get
            Set(value As Boolean)
                If (value = Me.m_Selezionata) Then Exit Property
                Me.m_Selezionata = value
                Me.DoChanged("Selezionata", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore correttivo da aggiungere al calcolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Correzione As Decimal
            Get
                Return Me.m_Correzione
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Correzione
                If (oldValue = value) Then Exit Property
                Me.m_Correzione = value
                Me.DoChanged("Correzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un parametro aggiuntivo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Parametro As String
            Get
                Return Me.m_Parametro
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Parametro
                If (oldValue = value) Then Exit Property
                Me.m_Parametro = value
                Me.DoChanged("Parametro", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto che estingue l'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estintore As IEstintore
            Get
                If (Me.m_Estintore Is Nothing) Then Me.m_Estintore = Types.GetItemByTypeAndId(Me.m_TipoEstintore, Me.m_IDEstintore)
                Return Me.m_Estintore
            End Get
            Set(value As IEstintore)
                Dim oldValue As IEstintore = Me.m_Estintore
                If (oldValue Is value) Then Exit Property
                Me.SetEstintore(value)
                Me.DoChanged("Estintore", value, oldValue)
            End Set
        End Property

        Friend Sub SetEstintore(ByVal value As Object)
            Me.m_Estintore = value
            Me.m_IDEstintore = GetID(value)
            If (value Is Nothing) Then
                Me.m_TipoEstintore = ""
            Else
                Me.m_TipoEstintore = TypeName(value)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto che estingue l'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDEstintore As Integer
            Get
                Return GetID(Me.m_Estintore, Me.m_IDEstintore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEstintore
                If (oldValue = value) Then Exit Property
                Me.m_IDEstintore = value
                Me.m_Estintore = Nothing
                Me.DoChanged("IDEstintore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto che estingue l'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoEstintore As String
            Get
                Return Me.m_TipoEstintore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoEstintore
                If (oldValue = value) Then Exit Property
                Me.m_TipoEstintore = value
                Me.DoChanged("TipoEstintore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore dell'estinzione (se si desidera forzare il valore occorre usare questa proprietà)
        ''' </summary>
        ''' <returns></returns>
        Public Property TotaleDaEstinguere As Decimal?
            Get
                Return Me.m_TotaleDaEstinguere
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_TotaleDaEstinguere
                If (oldValue = value) Then Return
                Me.m_TotaleDaEstinguere = value
                Me.DoChanged("TotaleDaEstinguere", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di decorrenza per cui è fissato il calcolo dell'estinzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataDecorrenza As Date?
            Get
                Return Me.m_DataDecorrenza
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataDecorrenza
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataDecorrenza = value
                Me.DoChanged("DataDecorrenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote insolute alla data di decorrenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteInsolute As Integer
            Get
                Return Me.m_NumeroQuoteInsolute
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroQuoteInsolute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroQuoteInsolute = value
                Me.DoChanged("NumeroQuoteInsolute", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote scadute alla data di decorrenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteScadute As Integer
            Get
                Return Me.m_NumeroQuoteScadute
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroQuoteScadute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroQuoteScadute = value
                Me.DoChanged("NumeroQuoteScadute", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote rimanenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteResidue As Integer
            Get
                Return Me.m_NumeroQuoteResidue
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroQuoteResidue
                If (oldValue = value) Then Exit Property
                Me.m_NumeroQuoteResidue = value
                Me.DoChanged("NumeroQuoteResidue", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'altro prestito da estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDEstinzione As Integer
            Get
                Return GetID(Me.m_Estinzione, Me.m_IDEstinzione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEstinzione
                If (oldValue = value) Then Exit Property
                Me.m_IDEstinzione = value
                Me.m_Estinzione = Nothing
                Me.DoChanged("IDEstinzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'altro prestito da estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estinzione As CEstinzione
            Get
                If (Me.m_Estinzione Is Nothing) Then Me.m_Estinzione = DMD.CQSPD.Estinzioni.GetItemById(Me.m_IDEstinzione)
                Return Me.m_Estinzione
            End Get
            Set(value As CEstinzione)
                Dim oldValue As CEstinzione = Me.m_Estinzione
                If (oldValue Is value) Then Exit Property
                Me.m_Estinzione = value
                Me.m_IDEstinzione = GetID(value)
                Me.DoChanged("Estinzione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetEstinzione(ByVal value As CEstinzione)
            Me.m_Estinzione = value
            Me.m_IDEstinzione = GetID(value)
        End Sub

        ''' <summary>
        ''' Aggiorna i valori copiandoli dagli oggetti Estinzione e Estintore associati
        ''' </summary>
        Public Sub AggiornaValori()
            Dim est As CEstinzione = Me.Estinzione
            If (est IsNot Nothing) Then
                If (est.Scadenza.HasValue = False AndAlso est.DataInizio.HasValue AndAlso est.Durata.HasValue) Then
                    est.Scadenza = DateUtils.GetLastMonthDay(DateUtils.DateAdd(DateInterval.Month, est.Durata.Value, est.DataInizio.Value))
                End If
                If (est.DataInizio.HasValue = False AndAlso est.Scadenza.HasValue AndAlso est.Durata.HasValue) Then
                    est.DataInizio = DateUtils.GetLastMonthDay(DateUtils.DateAdd(DateInterval.Month, -est.Durata.Value, est.Scadenza.Value))
                End If
                If (est.Istituto Is Nothing AndAlso est.NomeIstituto <> "") Then
                    est.Istituto = CQSPD.Cessionari.GetItemByName(est.NomeIstituto)
                End If
                est.Save()
                Me.Tipo = est.Tipo
                Me.Cessionario = est.Istituto
                Me.NomeCessionario = est.NomeIstituto
                Me.NomeAgenzia = est.NomeAgenzia
                Me.Rata = est.Rata
                Me.DataDecorrenza = est.DataInizio
                Me.DataFine = est.Scadenza
                Me.Durata = est.Durata
                Me.TAN = est.TAN
                Me.TAEG = est.TAEG
            End If
            'If (Me.Estintore IsNot Nothing) Then
            '    Me.DataEstinzione = Me.Estintore.DataDecorrenza
            'End If

            If (Me.DataEstinzione.HasValue AndAlso Me.DataFine.HasValue) Then Me.m_NumeroQuoteResidue = Math.Max(0, DateUtils.DateDiff(DateInterval.Month, Me.DataEstinzione.Value, Me.DataFine.Value) + 1)
            If (Me.Durata.HasValue AndAlso Me.m_NumeroQuoteResidue > Me.Durata.Value) Then
                Me.m_NumeroQuoteResidue = -1 'Math.Min(Me.m_NumeroQuoteResidue, Me.Durata.Value)
                'Else
                '    Me.m_NumeroQuoteResidue = Math.Min(Me.m_NumeroQuoteResidue, Me.Durata.Value)
            End If

        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data di estinzione (corrispondente in genere alla decorrenza dell'estintore)
        ''' </summary>
        ''' <returns></returns>
        Public Property DataEstinzione As DateTime?
            Get
                Return Me.m_DataEstinzione
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataEstinzione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataEstinzione = value
                Me.DoChanged("DataEstinzione", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property TotaleDaRimborsare As Decimal
            Get
                If (Me.m_TotaleDaEstinguere.HasValue) Then Return Me.m_TotaleDaEstinguere.Value

                'Return Me.Estinzione.TotaleDaRimborsare(Me.m_NumeroQuoteScadute, Me.m_NumeroQuoteInsolute) + Me.m_Correzione
                Dim c As New CEstinzioneCalculator()
                c.Rata = Me.Rata '(this.getEstinzione().getRata());
                'c.PenaleEstinzione = (this.getEstinzione().getPenaleEstinzione());
                c.TAN = Me.TAN '(this.getEstinzione().getTAN());

                c.SpeseAccessorie = Me.Correzione '(this.m_Correzione);
                c.NumeroRateInsolute = Me.NumeroQuoteInsolute '(this.m_NumeroQuoteInsolute);
                c.Durata = Me.NumeroQuoteResidue '(this.m_NumeroQuoteResidue);

                Return c.TotaleDaRimborsare
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_EstinzioniXEstintore"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDEstintore = reader.Read("IDEstintore", Me.m_IDEstintore)
            Me.m_TipoEstintore = reader.Read("TipoEstintore", Me.m_TipoEstintore)
            Me.m_IDEstinzione = reader.Read("IDEstinzione", Me.m_IDEstinzione)
            Me.m_DataDecorrenza = reader.Read("DataDecorrenza", Me.m_DataDecorrenza)
            Me.m_NumeroQuoteInsolute = reader.Read("NQI", Me.m_NumeroQuoteInsolute)
            Me.m_NumeroQuoteScadute = reader.Read("NQS", Me.m_NumeroQuoteScadute)
            Me.m_NumeroQuoteResidue = reader.Read("NQR", Me.m_NumeroQuoteResidue)
            Me.m_Parametro = reader.Read("Parametro", Me.m_Parametro)
            Me.m_Correzione = reader.Read("Correzione", Me.m_Correzione)
            Me.m_Selezionata = reader.Read("Selezionata", Me.m_Selezionata)
            Me.m_DataEstinzione = reader.Read("DataEstinzione", Me.m_DataEstinzione)
            Me.m_Rata = reader.Read("Rata", Me.m_Rata)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_TAN = reader.Read("TAN", Me.m_TAN)
            Me.m_TAEG = reader.Read("TAEG", Me.m_TAEG)
            Me.m_TotaleDaEstinguere = reader.Read("TotaleDaEstinguere", Me.m_TotaleDaEstinguere)
            Me.m_IDCessionario = reader.Read("IDCessionario", Me.m_IDCessionario)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_NomeAgenzia = reader.Read("NomeAgenzia", Me.m_NomeAgenzia)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDEstintore", Me.IDEstintore)
            writer.Write("TipoEstintore", Me.m_TipoEstintore)
            writer.Write("IDEstinzione", Me.IDEstinzione)
            writer.Write("DataDecorrenza", Me.m_DataDecorrenza)
            writer.Write("NQI", Me.m_NumeroQuoteInsolute)
            writer.Write("NQS", Me.m_NumeroQuoteScadute)
            writer.Write("Parametro", Me.m_Parametro)
            writer.Write("Correzione", Me.m_Correzione)
            writer.Write("NQR", Me.m_NumeroQuoteResidue)
            writer.Write("Selezionata", Me.m_Selezionata)
            writer.Write("DataEstinzione", Me.m_DataEstinzione)
            writer.Write("Rata", Me.m_Rata)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("TAEG", Me.m_TAEG)
            writer.Write("TotaleDaEstinguere", Me.m_TotaleDaEstinguere)
            writer.Write("IDCessionario", Me.IDCessionario)
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("NomeAgenzia", Me.m_NomeAgenzia)
            writer.Write("Tipo", Me.m_Tipo)
            Return MyBase.SaveToRecordset(writer)
        End Function


        '------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDEstintore", Me.IDEstintore)
            writer.WriteAttribute("TipoEstintore", Me.m_TipoEstintore)
            writer.WriteAttribute("IDEstinzione", Me.IDEstinzione)
            writer.WriteAttribute("DataDecorrenza", Me.m_DataDecorrenza)
            writer.WriteAttribute("NQI", Me.m_NumeroQuoteInsolute)
            writer.WriteAttribute("NQS", Me.m_NumeroQuoteScadute)
            writer.WriteAttribute("NQR", Me.m_NumeroQuoteResidue)
            writer.WriteAttribute("Parametro", Me.m_Parametro)
            writer.WriteAttribute("Correzione", Me.m_Correzione)
            writer.WriteAttribute("Selezionata", Me.m_Selezionata)
            writer.WriteAttribute("DataEstinzione", Me.m_DataEstinzione)
            writer.WriteAttribute("Rata", Me.m_Rata)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("TAN", Me.m_TAN)
            writer.WriteAttribute("TAEG", Me.m_TAEG)
            writer.WriteAttribute("TotaleDaEstinguere", Me.m_TotaleDaEstinguere)
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("NomeAgenzia", Me.m_NomeAgenzia)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDEstintore" : Me.m_IDEstintore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoEstintore" : Me.m_TipoEstintore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDEstinzione" : Me.m_IDEstinzione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataDecorrenza" : Me.m_DataDecorrenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NQI" : Me.m_NumeroQuoteInsolute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NQS" : Me.m_NumeroQuoteScadute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NQR" : Me.m_NumeroQuoteResidue = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Parametro" : Me.m_Parametro = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Correzione" : Me.m_Correzione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Selezionata" : Me.m_Selezionata = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataEstinzione" : Me.m_DataEstinzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Rata" : Me.m_Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG" : Me.TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TotaleDaEstinguere" : Me.m_TotaleDaEstinguere = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDCessionario" : Me.m_IDCessionario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeAgenzia" : Me.m_NomeAgenzia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing '    Return Consulenze.Module
        End Function

    End Class

End Class

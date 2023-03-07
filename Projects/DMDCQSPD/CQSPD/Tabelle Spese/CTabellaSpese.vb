Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    <Flags>
    Public Enum TabellaSpeseFlags As Integer
        None = 0
        Attiva = 1
    End Enum




    ''' <summary>
    ''' SPESE DEFINITE PER UNO O PIU' PRODOTTI
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CTabellaSpese
        Inherits DBObject

        Private m_Flags As TabellaSpeseFlags
        Private m_CessionarioID As Integer
        <NonSerialized> Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_Nome As String
        Private m_Descrizione As String
        Private m_SpeseFisse() As Decimal
        Private m_SpeseVariabili() As Double
        'Private m_Interessi() As Double
        Private m_Rivalsa() As Decimal
        Private m_SpeseProduttore() As Decimal
        Private m_SpeseAmministrazione() As Decimal
        Private m_SpesePerRata() As Decimal
        Private m_ImpostaSostitutiva() As Double
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        'Private m_Running As Double
        Private m_Rappel() As Double
        Private m_Visible As Boolean

        Public Sub New()
            Me.m_CessionarioID = 0
            Me.m_Cessionario = Nothing
            Me.m_NomeCessionario = ""
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            ReDim m_SpeseFisse(10)
            ReDim m_SpeseVariabili(10)
            '        ReDim m_Interessi(10)
            ReDim m_Rivalsa(10)
            ReDim m_SpeseProduttore(10)
            ReDim m_SpeseAmministrazione(10)
            ReDim m_SpesePerRata(10)
            ReDim m_ImpostaSostitutiva(10)
            ReDim m_Rappel(10)
            For i As Integer = 0 To 10
                Me.m_SpeseFisse(i) = 0
                Me.m_SpeseVariabili(i) = 0
                'm_Interessi(i) = 0
                Me.m_Rivalsa(i) = 0
                Me.m_SpeseProduttore(i) = 0
                Me.m_SpeseAmministrazione(i) = 0
                Me.m_SpesePerRata(i) = 0
                Me.m_ImpostaSostitutiva(i) = 0
                Me.m_Rappel(i) = 0
            Next
            'Me.m_Running = 0
            Me.m_Visible = True
            Me.m_Flags = TabellaSpeseFlags.Attiva
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleSpese.Module
        End Function

        Public Property Attiva As Boolean
            Get
                Return TestFlag(Me.m_Flags, TabellaSpeseFlags.Attiva)
            End Get
            Set(value As Boolean)
                If (Me.Attiva = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TabellaSpeseFlags.Attiva, value)
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta i flags impostati per la tabella spese
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As TabellaSpeseFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As TabellaSpeseFlags)
                Dim oldValue As TabellaSpeseFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la tabella spese è visibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visible As Boolean
            Get
                Return Me.m_Visible
            End Get
            Set(value As Boolean)
                If (Me.m_Visible = value) Then Exit Property
                Me.m_Visible = value
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

        'Public Property Running As Double
        '    Get
        '        Return Me.m_Running
        '    End Get
        '    Set(value As Double)
        '        If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Running")
        '        Dim oldValue As Double = Me.m_Running
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Running = value
        '        Me.DoChanged("Running", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Rappresenta la provvigione percentuale sul montante lordo che il cessionario paga all'agenzia (non visibile)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rappel(ByVal index As Integer) As Double
            Get
                Return Me.m_Rappel(index)
            End Get
            Set(value As Double)
                If (value < 0) Or (value > 100) Then Throw New ArgumentOutOfRangeException("Rappel")
                Dim oldValue As Double = Me.m_Rappel(index)
                If (oldValue = value) Then Exit Property
                Me.m_Rappel(index) = value
                Me.DoChanged("Running", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Nome della tabella spese
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Descrizione della tabella spese
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' ID del cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CessionarioID As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_CessionarioID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.CessionarioID
                If oldValue = value Then Exit Property
                Me.m_CessionarioID = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("CessionarioID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = DMD.CQSPD.Cessionari.GetItemById(Me.m_CessionarioID)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_CessionarioID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Nome del cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCessionario
                If (oldValue = value) Then Exit Property
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Data di inizio di validità
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
        ''' Data di fine validità
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

        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return Me.Attiva AndAlso DateUtils.CheckBetween(atDate, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Function IsValid() As Boolean
            Return Me.IsValid(DateUtils.Now)
        End Function

        ''' <summary>
        ''' Restituisce o imposta le spese pagate dall'agenzia in funzione della durata del finanziamento
        ''' </summary>
        ''' <param name="durata"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpeseProduttore(ByVal durata As Integer) As Decimal
            Get
                Return Me.m_SpeseProduttore(Fix(durata / 12))
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("SpeseProduttore")
                Dim oldValue As Decimal = Me.m_SpeseProduttore(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpeseProduttore(Fix(durata / 12)) = value
                Me.DoChanged("SpeseProduttore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta le spese pagate all'amministrazione in base alla durata del finanziamento
        ''' </summary>
        ''' <param name="durata"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpeseAmministrazione(ByVal durata As Integer) As Decimal
            Get
                Return Me.m_SpeseAmministrazione(Fix(durata / 12))
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("SpeseAmministrazione")
                Dim oldValue As Decimal = Me.m_SpeseAmministrazione(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpeseAmministrazione(Fix(durata / 12)) = value
                Me.DoChanged("SpeseAmministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore dell'imposta sostituiva in base alla durata del finanziamento
        ''' </summary>
        ''' <param name="durata"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImpostaSostitutiva(ByVal durata As Integer) As Double
            Get
                Return Me.m_ImpostaSostitutiva(Fix(durata / 12))
            End Get
            Set(value As Double)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("ImpostaSostitutiva")
                Dim oldValue As Double = Me.m_ImpostaSostitutiva(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_ImpostaSostitutiva(Fix(durata / 12)) = value
                Me.DoChanged("ImpostaSostitutiva", value, oldValue)
            End Set
        End Property

        'Public Property Coefficiente(ByVal durata As Integer) As double
        '    Get
        '        Return Me.m_Durata(Fix(durata / 12))
        '    End Get
        '    Set(value As Double)
        '        Me.m_Durata(Fix(durata / 12)) = value
        '    End Set
        'End Property

        Public Property Rivalsa(ByVal durata As Integer) As Decimal
            Get
                Return Me.m_Rivalsa(Fix(durata / 12))
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Rivalsa")
                Dim oldValue As Decimal = Me.m_Rivalsa(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Rivalsa(Fix(durata / 12)) = value
                Me.DoChanged("", value, oldValue)
            End Set
        End Property

        Public Property SpesePerRata(ByVal durata As Integer) As Decimal
            Get
                Return Me.m_SpesePerRata(Fix(durata / 12))
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("SpesePerRata")
                Dim oldValue As Decimal = Me.m_SpesePerRata(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpesePerRata(Fix(durata / 12)) = value
                Me.DoChanged("SpesePerRata", value, oldValue)
            End Set
        End Property

        Public Property SpeseFisse(ByVal durata As Integer) As Double
            Get
                Return Me.m_SpeseFisse(Fix(durata / 12))
            End Get
            Set(value As Double)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("SpeseFisse")
                Dim oldValue As Double = Me.m_SpeseFisse(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpeseFisse(Fix(durata / 12)) = value
                Me.DoChanged("SpeseFisse", value, oldValue)
            End Set
        End Property

        Public Property SpeseVariabili(ByVal durata As Integer) As Double
            Get
                Return Me.m_SpeseVariabili(Fix(durata / 12))
            End Get
            Set(value As Double)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("SpeseVariabili")
                Dim oldValue As Double = Me.m_SpeseVariabili(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpeseVariabili(Fix(durata / 12)) = value
                Me.DoChanged("SpeseVariabili", value, oldValue)
            End Set
        End Property

        
        ''' <summary>
        ''' Calcola le spese per l'oggerta selezionata
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <remarks></remarks>
        Public Sub Calcola(ByVal offerta As COffertaCQS)
            If (offerta Is Nothing) Then Throw New ArgumentNullException("offerta")
            'offerta.Rivalsa = m_Rivalsa(offerta.Durata)
            'offerta.AltreSpese = 0
            'offerta.AltreSpese = offerta.AltreSpese + m_SpeseFisse(offerta.Durata)
            'offerta.AltreSpese = offerta.AltreSpese + m_SpeseCoefficiente(offerta.Durata) * offerta.MontanteLordo
            'offerta.m_Interessi(10)
            'offerta.AltreSpese = offerta.AltreSpese + m_SpeseProduttore(offerta.Durata)
            'offerta.AltreSpese = offerta.AltreSpese + m_SpeseAmministrazione(offerta.Durata)
            'offerta.AltreSpese = offerta.AltreSpese + m_SpesePerRata(offerta.Durata) * offerta.Durata
            'offerta.AltreSpese = offerta.AltreSpese + m_ImpostaSostitutiva(offerta.Durata) * offerta.CapitaleFinanziato
            offerta.Rappel = offerta.MontanteLordo * Me.Rappel(offerta.Durata) / 100
            offerta.Rivalsa = Me.Rivalsa(offerta.Durata)
            'Calcolo degli oneri erariali
            offerta.OneriErariali = Me.SpeseProduttore(offerta.Durata) + Me.SpeseAmministrazione(offerta.Durata)
            'If Prodotto.Scomposizione Is Nothing Then
            offerta.Imposte = 0
            'Else
            '	m_Imposte = Prodotto.Scomposizione.ImposteFisse
            'End If

            offerta.AltreSpese = 0
            offerta.AltreSpese += Me.SpeseFisse(offerta.Durata)
            offerta.AltreSpese += (offerta.MontanteLordo * Me.SpeseVariabili(offerta.Durata) / 100)

            offerta.SpeseConvenzioni = 0
            offerta.SpeseConvenzioni += offerta.Durata * Me.SpesePerRata(offerta.Durata)

            'Imposta
            offerta.ImpostaSostitutiva = (offerta.MontanteLordo - offerta.Interessi) * Me.ImpostaSostitutiva(offerta.Durata) / 100
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TabellaSpese"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_CessionarioID = reader.Read("Cessionario", Me.m_CessionarioID)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            For i As Integer = 2 To 10
                Me.m_Rivalsa(i) = reader.Read("durata" & (i * 12), Me.m_Rivalsa(i))
                Me.m_SpeseFisse(i) = reader.Read("SpeseFisse" & (i * 12), Me.m_SpeseFisse(i))
                Me.m_SpeseVariabili(i) = reader.Read("SpeseCoefficiente" & (i * 12), Me.m_SpeseVariabili(i))
                'm_Interessi(i) = dbRis ( "Interessi" & (i * 12))
                Me.m_SpeseProduttore(i) = reader.Read("SpeseProduttore" & (i * 12), Me.m_SpeseProduttore(i))
                Me.m_SpeseAmministrazione(i) = reader.Read("SpeseAmministrazione" & (i * 12), Me.m_SpeseAmministrazione(i))
                Me.m_SpesePerRata(i) = reader.Read("SpesePerRata" & (i * 12), Me.m_SpesePerRata(i))
                Me.m_ImpostaSostitutiva(i) = reader.Read("ImpostaSostitutiva" & (i * 12), Me.m_ImpostaSostitutiva(i))
                'Me.m_Running = reader.Read("Running", Me.m_Running)
                Me.m_Rappel(i) = reader.Read("Rappel" & (i * 12), Me.m_Rappel(i))
            Next
            Me.m_Visible = reader.Read("Visible", Me.m_Visible)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Cessionario", Me.CessionarioID)
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            For i As Integer = 2 To 10
                writer.Write("Durata" & (i * 12), Me.m_Rivalsa(i))
                writer.Write("SpeseFisse" & (i * 12), Me.m_SpeseFisse(i))
                writer.Write("SpeseCoefficiente" & (i * 12), Me.m_SpeseVariabili(i))
                'dbRis("Interessi" & (i * 12)) = m_Interessi(i)
                writer.Write("SpeseProduttore" & (i * 12), Me.m_SpeseProduttore(i))
                writer.Write("SpeseAmministrazione" & (i * 12), Me.m_SpeseAmministrazione(i))
                writer.Write("SpesePerRata" & (i * 12), Me.m_SpesePerRata(i))
                writer.Write("ImpostaSostitutiva" & (i * 12), Me.m_ImpostaSostitutiva(i))
                writer.Write("Rappel" & (i * 12), Me.m_Rappel(i))
            Next
            'writer.Write("Running", Me.m_Running)
            writer.Write("Visible", Me.m_Visible)
            writer.Write("Flags", Me.m_Flags)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("CessionarioID", Me.CessionarioID)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("SpeseFisse", Me.m_SpeseFisse)
            writer.WriteTag("SpeseCoefficiente", Me.m_SpeseVariabili)
            writer.WriteTag("Rivalsa", Me.m_Rivalsa)
            writer.WriteTag("SpeseProduttore", Me.m_SpeseProduttore)
            writer.WriteTag("SpeseAmministrazione", Me.m_SpeseAmministrazione)
            writer.WriteTag("SpesePerRata", Me.m_SpesePerRata)
            writer.WriteTag("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            writer.WriteTag("Rappel", Me.m_Rappel)
            'writer.WriteTag("Running", Me.m_Running)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "CessionarioID" : Me.m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SpeseFisse" : Me.m_SpeseFisse = Arrays.Convert(Of Decimal)(fieldValue)
                Case "SpeseCoefficiente" : Me.m_SpeseVariabili = Arrays.Convert(Of Double)(fieldValue)
                Case "Rivalsa" : Me.m_Rivalsa = Arrays.Convert(Of Decimal)(fieldValue)
                Case "SpeseProduttore" : Me.m_SpeseProduttore = Arrays.Convert(Of Decimal)(fieldValue)
                Case "SpeseAmministrazione" : Me.m_SpeseAmministrazione = Arrays.Convert(Of Decimal)(fieldValue)
                Case "SpesePerRata" : Me.m_SpesePerRata = Arrays.Convert(Of Decimal)(fieldValue)
                Case "ImpostaSostitutiva" : Me.m_ImpostaSostitutiva = Arrays.Convert(Of Double)(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                    'Case "Running" : Me.m_Running = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Rappel" : Me.m_Rappel = Arrays.Convert(Of Double)(fieldValue)
                Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.TabelleSpese.UpdateCached(Me)
        End Sub


    End Class

End Class

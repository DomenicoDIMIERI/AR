Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Tipologie di estinzione
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoEstinzione As Integer
        ESTINZIONE_NO = 0    'Sconosciuto
        ESTINZIONE_CESSIONEDELQUINTO = 1    'Cessione del quinto
        ESTINZIONE_PRESTITODELEGA = 2    'Delegazione di pagamento
        ESTINZIONE_PRESTITOPERSONALE = 3    'Prestito personale
        ESTINZIONE_PIGNORAMENTO = 4    'Pignoramento
        ESTINZIONE_MUTUO = 5
        ESTINZIONE_PROTESTI = 6
        ESTINZIONE_ASSICURAZIONE = 7
        ESTINZIONE_ALIMENTI = 8
        ESTINZIONE_CQP = 9
        ESTINZIONE_PICCOLO_PRESTITO = 10
    End Enum

    <Serializable> _
    Public Class CAltroPrestito
        Inherits DBObject

        Private m_Tipo As TipoEstinzione '[INT]      Un valore intero che indica la tipologia di estinzione
        Private m_IDIstituto As Integer '[INT]      ID dell'istituto con cui il cliente ha stipulato il contratto da estinguere
        Private m_Istituto As CCQSPDCessionarioClass '[Object]   Oggetto istituto con cui il cliente ha stipulato il contratto
        Private m_NomeIstituto As String '[TEXT]     Nome dell'istituto
        Private m_DataInizio As Nullable(Of Date) '[Date]     Data di inizio del prestito
        Private m_Scadenza As Nullable(Of Date) '[Date]     Data di scadenza del prestito
        Private m_Rata As Nullable(Of Decimal) '[Double]   
        Private m_Durata As Integer '[INT]      Durata in numero di rate
        Private m_TAN As Double '[Double]    
        Private m_NumeroRateInsolute As Integer '[int] Numero di rate insolute
        Private m_Penale As Double '[Double] Percentuale da corrispondere come penale per estinzione anticipata
        Private m_NumeroRatePagate As Integer
        Private m_Calculated As Boolean
        Private m_AbbuonoInteressi As Decimal
        Private m_PenaleEstinzione As Double
        Private m_SpeseAccessorie As Decimal
        Private m_IDPersona As Integer
        Private m_Persona As CPersona
        Private m_NomePersona As String

        Public Sub New()
            Me.m_Tipo = TipoEstinzione.ESTINZIONE_NO
            Me.m_IDIstituto = 0
            Me.m_Istituto = Nothing
            Me.m_NomeIstituto = ""
            Me.m_DataInizio = Nothing
            Me.m_Scadenza = Nothing
            Me.m_Rata = 0
            Me.m_Durata = 0
            Me.m_TAN = 0
            Me.m_NumeroRatePagate = 0
            Me.m_Calculated = False
            Me.m_AbbuonoInteressi = 0
            Me.m_PenaleEstinzione = 1
            Me.m_SpeseAccessorie = 0
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona a cui appartiene l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        Public Sub Validate()
            If Me.m_Calculated = False Then Me.Calculate()
        End Sub

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub

        Public Overrides Function GetModule() As CModule
            Return AltriPrestiti.Module
        End Function

       
        Public Property PenaleEstinzione As Double
            Get
                Return Me.m_PenaleEstinzione
            End Get
            Set(value As Double)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("La penale di estinzione non può assumere un valore negativo")
                Dim oldValue As Double = Me.m_PenaleEstinzione
                If oldValue = value Then Exit Property
                Me.m_PenaleEstinzione = value
                Me.Invalidate()
                Me.DoChanged("PenaleEstinzione", value, oldValue)
            End Set
        End Property

        Public Property SpeseAccessorie As Decimal
            Get
                Return Me.m_SpeseAccessorie
            End Get
            Set(value As Decimal)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Le spese accessorie non possono assumere un valore negativo")
                Dim oldValue As Decimal = Me.m_SpeseAccessorie
                If oldValue = value Then Exit Property
                Me.m_SpeseAccessorie = value
                Me.Invalidate()
                Me.DoChanged("SpeseAccessorie", value, oldValue)
            End Set
        End Property

        Public Property NumeroRatePagate As Integer
            Get
                Return Me.m_NumeroRatePagate
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate pagate non può essere negativo")
                Dim oldValue As Integer = Me.m_NumeroRatePagate
                If oldValue = value Then Exit Property
                Me.m_NumeroRatePagate = value
                Me.Invalidate()
                Me.DoChanged("NumeroRatePagate", value, oldValue)
            End Set
        End Property

        Public Property NumeroRateResidue As Integer
            Get
                Return Me.Durata - Me.NumeroRatePagate
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate residue non può essere negativo")
                Dim oldValue As Integer = Me.NumeroRateResidue
                If oldValue = value Then Exit Property
                Me.m_NumeroRatePagate = Me.Durata - value
                Me.Invalidate()
                Me.DoChanged("NumeroRateResidue", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di rate già maturate ma non  ancora pagate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroRateInsolute As Integer
            Get
                Return Me.m_NumeroRateInsolute
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate insolute non può essere negativo")
                Dim oldValue As Integer = Me.m_NumeroRateInsolute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroRateInsolute = value
                Me.DoChanged("NumeroRateInsolute", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As TipoEstinzione
            Get
                Return Me.m_Tipo
            End Get
            Set(value As TipoEstinzione)
                Dim oldValue As TipoEstinzione = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property TipoEx As String
            Get
                Return CQSPD.Estinzioni.FormatTipo(Me.Tipo)
            End Get
        End Property

        Public Property IDIstituto As Integer
            Get
                Return GetID(Me.m_Istituto, Me.m_IDIstituto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDIstituto
                If oldValue = value Then Exit Property
                Me.m_IDIstituto = value
                Me.m_Istituto = Nothing
                Me.DoChanged("IDIstituto", value, oldValue)
            End Set
        End Property

        Public Property Istituto As CCQSPDCessionarioClass
            Get
                If (Me.m_Istituto Is Nothing) Then Me.m_Istituto = CQSPD.Cessionari.GetItemById(Me.m_IDIstituto)
                Return Me.m_Istituto
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Istituto
                If (oldValue = value) Then Exit Property
                Me.m_Istituto = value
                Me.m_IDIstituto = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomeIstituto = value.Nome
                Me.DoChanged("Istituto", value, oldValue)
            End Set
        End Property

        Public Property NomeIstituto As String
            Get
                Return Me.m_NomeIstituto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeIstituto
                If (oldValue = value) Then Exit Property
                Me.m_NomeIstituto = value
                Me.DoChanged("NomeIstituto", value, oldValue)
            End Set
        End Property

        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If oldValue = value Then Exit Property
                Me.m_DataInizio = value
                Me.Invalidate()
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property Scadenza As Nullable(Of Date)
            Get
                Return Me.m_Scadenza
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_Scadenza
                If (oldValue = value) Then Exit Property
                If Me.m_Scadenza = value Then Exit Property
                Me.m_Scadenza = value
                Me.Invalidate()
                Me.DoChanged("Scadenza", value, oldValue)
            End Set
        End Property

        Public Property Rata As Nullable(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Rata
                If oldValue = value Then Exit Property
                Me.m_Rata = value
                Me.Invalidate()
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        Public Property Durata As Integer
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Durata
                If oldValue = value Then Exit Property
                Me.m_Durata = value
                Me.Invalidate()
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        Public Property TAN As Double
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_TAN
                If oldValue = value Then Exit Property
                Me.m_TAN = value
                Me.Invalidate()
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

       

        Public Property DebitoIniziale As Nullable(Of Decimal)
            Get
                If Me.m_Rata.HasValue Then Return Me.m_Rata.Value * Me.m_Durata
                Return Nothing
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.DebitoIniziale
                If (oldValue = value) Then Exit Property
                If Me.m_Durata <= 0 Then Me.m_Durata = 1
                Me.m_Rata = value / Me.m_Durata
                Me.DoChanged("DebitoIniziale", value, oldValue)
            End Set
        End Property

        Public Property DebitoResiduo As Nullable(Of Decimal)
            Get
                If (Me.DebitoIniziale.HasValue AndAlso Me.m_Rata.HasValue) Then
                    Return Me.DebitoIniziale.Value - Me.m_Rata.Value * Me.m_NumeroRatePagate
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.DebitoResiduo
                If (oldValue = value) Then Exit Property
                Me.m_NumeroRatePagate = (Me.DebitoIniziale - value) / Me.m_Rata.Value
                Me.DoChanged("DebitoResiduo", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property AbbuonoInteressi As Decimal
            Get
                Me.Validate()
                Return Me.m_AbbuonoInteressi
            End Get
        End Property

        Public ReadOnly Property CapitaleDaRimborsare As Decimal
            Get
                If Me.DebitoResiduo.HasValue Then
                    Return Me.DebitoResiduo.Value - Me.AbbuonoInteressi
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property ValorePenale As Decimal
            Get
                Return Me.CapitaleDaRimborsare * Me.m_PenaleEstinzione / 100
            End Get
        End Property

        Public ReadOnly Property TotaleDaRimborsare As Decimal
            Get
                Return Me.CapitaleDaRimborsare + Me.ValorePenale + Me.SpeseAccessorie + Me.ValoreQuoteInsolute
            End Get
        End Property

        Public ReadOnly Property ValoreQuoteInsolute As Decimal
            Get
                Me.Validate()
                If (Me.m_Rata.HasValue) Then
                    Return Me.m_NumeroRateInsolute * Me.m_Rata.Value
                Else
                    Return 0
                End If
            End Get
        End Property


        Public Function Calculate() As Boolean
            'Dim Dk As Decimal   'Debito residuo per k=1..n-1
            Dim Ik As Decimal 'Interesse in ciascun periodo k=1...n
            Dim k As Integer
            Dim i As Double

            'Me.m_DebitoIniziale = Me.m_Rata.Value * Me.m_Durata
            'Me.m_DebitoResiduo = Me.m_DebitoIniziale - Me.m_Quota * Me.m_NumeroRatePagate

            i = (Me.m_TAN / 100) / 12

            Me.m_AbbuonoInteressi = 0
            For k = Me.m_NumeroRatePagate + 1 To Me.m_Durata
                Ik = Me.m_Rata.Value * (1 - 1 / ((1 + i) ^ (Me.m_Durata - k + 1)))
                Me.m_AbbuonoInteressi = Me.m_AbbuonoInteressi + Ik
            Next

            Me.m_Calculated = True

            Return True
        End Function

        '---------------------------------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_Tipo", Me.m_Tipo)
            writer.WriteAttribute("m_IDIstituto", Me.IDIstituto)
            writer.WriteAttribute("m_NomeIstituto", Me.m_NomeIstituto)
            writer.WriteAttribute("m_DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("m_Scadenza", Me.m_Scadenza)
            writer.WriteAttribute("m_Rata", Me.m_Rata)
            writer.WriteAttribute("m_Durata", Me.m_Durata)
            writer.WriteAttribute("m_TAN", Me.m_TAN)
            writer.WriteAttribute("NumeroRatePagate", Me.m_NumeroRatePagate)
            writer.WriteAttribute("NumeroRateInsolute", Me.m_NumeroRateInsolute)
            writer.WriteAttribute("Calculated", Me.m_Calculated)
            writer.WriteAttribute("AbbuonoInteressi", Me.m_AbbuonoInteressi)
            writer.WriteAttribute("PenaleEstinzione", Me.m_PenaleEstinzione)
            writer.WriteAttribute("SpeseAccessorie", Me.m_SpeseAccessorie)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            MyBase.XMLSerialize(writer)
        End Sub


        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_IDIstituto" : Me.m_IDIstituto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_NomeIstituto" : Me.m_NomeIstituto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Scadenza" : m_Scadenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Rata" : Me.m_Rata = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "m_Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "NumeroRateInsolute" : Me.m_NumeroRateInsolute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroRatePagate" : Me.m_NumeroRatePagate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Calculated" : Me.m_Calculated = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "AbbuonoInteressi" : Me.m_AbbuonoInteressi = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "PenaleEstinzione" : Me.m_PenaleEstinzione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpeseAccessorie" : Me.m_SpeseAccessorie = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDAltriPrestiti"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Tipo", Me.m_Tipo)
            reader.Read("Istituto", Me.m_IDIstituto)
            reader.Read("NomeIstituto", Me.m_NomeIstituto)
            reader.Read("DataInizio", Me.m_DataInizio)
            reader.Read("Scadenza", Me.m_Scadenza)
            reader.Read("Rata", Me.m_Rata)
            reader.Read("Durata", Me.m_Durata)
            reader.Read("TAN", Me.m_TAN)
            reader.Read("NumeroRatePagate", Me.m_NumeroRatePagate)
            reader.Read("NumeroRateResidue", Me.m_NumeroRateInsolute)
            reader.Read("Calculated", Me.m_Calculated)
            reader.Read("AbbuonoInteressi", Me.m_AbbuonoInteressi)
            reader.Read("PenaleEstinzione", Me.m_PenaleEstinzione)
            reader.Read("SpeseAccessorie", Me.m_SpeseAccessorie)
            reader.Read("IDPersona", Me.m_IDPersona)
            reader.Read("NomePersona", Me.m_NomePersona)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Istituto", Me.IDIstituto)
            writer.Write("NomeIstituto", Me.m_NomeIstituto)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("Scadenza", Me.m_Scadenza)
            writer.Write("Rata", Me.m_Rata)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("NumeroRatePagate", Me.m_NumeroRatePagate)
            writer.Write("NumeroRateResidue", Me.m_NumeroRateInsolute)
            writer.Write("Calculated", Me.m_Calculated)
            writer.Write("AbbuonoInteressi", Me.m_AbbuonoInteressi)
            writer.Write("PenaleEstinzione", Me.m_PenaleEstinzione)
            writer.Write("SpeseAccessorie", Me.m_SpeseAccessorie)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return CQSPD.AltriPrestiti.FormatTipo(Me.Tipo) & ", Rata: " & Formats.FormatValuta(Me.Rata) & ", Rate residue: " & Me.NumeroRateResidue
        End Function

        Public Function InCorso() As Boolean
            Return Calendar.CheckBetween(Now, Me.DataInizio, Me.Scadenza)
        End Function

        Friend Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
            Me.m_NomePersona = value.Nominativo
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            If (Me.m_DataInizio.HasValue AndAlso Me.m_Durata > 0) Then
                Me.ProgrammaRicontatto()
            End If
            MyBase.OnCreate(e)
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            If (Me.m_DataInizio.HasValue AndAlso Me.m_Durata > 0) Then
                Me.ProgrammaRicontatto()
            End If
            MyBase.OnModified(e)
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            Me.AnnullaRicontatto()
            MyBase.OnDelete(e)
        End Sub

        ''' <summary>
        ''' Programma il ricontatto opportuno
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ProgrammaRicontatto()
            ' If (Me.InCorso) Then
            Dim dataRinnovo As Date
            Dim percRinnovo As Single
            Dim giorniAnticipo As Integer
            Dim numMesi As Integer
            Dim motivo As String
            Dim nomeLista As String

            Dim r As ListaRicontattoItem = Anagrafica.ListeRicontatto.Items.GetRicontattoBySource(Me)
            If (r IsNot Nothing AndAlso r.StatoRicontatto = StatoRicontatto.EFFETTUATO) Then Exit Sub
            If (Me.GetPraticheEstinte.Count > 0) Then
                If (r IsNot Nothing) Then
                    r.StatoRicontatto = StatoRicontatto.ANNULLATO
                    r.Save()
                End If
            Else
                Select Case Me.Tipo
                    Case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO, TipoEstinzione.ESTINZIONE_PRESTITODELEGA, TipoEstinzione.ESTINZIONE_CQP
                        nomeLista = "Altri Prestiti Rinnovabili"
                        percRinnovo = Me.GetModule.Settings.GetValueDouble("Rinnovo_PercDurata", 40)
                        giorniAnticipo = Me.GetModule.Settings.GetValueInt("Rinnovo_GiorniAnticipo", 30)
                        numMesi = Math.Floor(Me.Durata * percRinnovo / 100)
                        dataRinnovo = Calendar.DateAdd(DateInterval.Month, numMesi, Me.DataInizio.Value)
                        dataRinnovo = Calendar.DateAdd(DateInterval.Day, -giorniAnticipo, dataRinnovo)
                        motivo = "Finanziamento rinnovabile: raggiunto il " & Formats.FormatInteger(percRinnovo) & "% del prestito " & Me.ToString
                        If (r IsNot Nothing) Then
                            r.Note = motivo
                            r.NomeLista = nomeLista
                            r.DataPrevista = dataRinnovo
                            r.Save()
                        Else
                            r = Ricontatti.ProgrammaRicontatto(Me.Persona, dataRinnovo, motivo, TypeName(Me), GetID(Me), nomeLista, Me.Persona.PuntoOperativo, Users.CurrentUser)
                        End If
                    Case Else
                        If (r IsNot Nothing) Then
                            r.StatoRicontatto = StatoRicontatto.ANNULLATO
                            r.Save()
                        End If
                End Select
            End If
        End Sub

        Public Sub AnnullaRicontatto()
            Dim r As CRicontatto = Ricontatti.GetRicontattoBySource(Me)
            If (r IsNot Nothing) Then
                r.StatoRicontatto = StatoRicontatto.ANNULLATO
                r.Save()
            End If
        End Sub

        ''' <summary>
        ''' Ottiene l'elenco delle estinzioni di questo oggetto. L'elenco non contiene le estinzioni relative alle pratiche annullate
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPraticheEstinte() As CCollection(Of CEstinzione)
            Dim cursor As New CEstinzioniCursor
            Dim ret As New CCollection(Of CEstinzione)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            'cursor.IDAltroPrestito.Value = GetID(Me)
            While Not cursor.EOF
                If (cursor.Item.Pratica.StatoAttuale.MacroStato.HasValue) Then
                    Select Case cursor.Item.Pratica.StatoAttuale.MacroStato
                        Case StatoPraticaEnum.STATO_ANNULLATA
                        Case Else
                            ret.Add(cursor.Item)
                    End Select
                End If
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

    End Class

     

  
 
End Class
Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Enum TabellaFinanziariaFlags As Integer
        None = 0

        ''' <summary>
        ''' Se vero indica che il rappel è calcolato in base alla tariffa e non alla tabella spese
        ''' </summary>
        ''' <remarks></remarks>
        ForzaRappel = 1

        ''' <summary>
        ''' Se vero indica che l'utilizzo di questa tabella finanziaria è sottoposto all'approvazione di un supervisore
        ''' </summary>
        ''' <remarks></remarks>
        RichiedeApprovazione = 2

        ''' <summary>
        ''' Se vero indica che la riduzione provvigionale (per estinzioni o rinnovi) viene detratta dal rappel e non dalla provvigione di UpFront
        ''' </summary>
        ''' <remarks></remarks>
        ScontaRiduzioniSuRappel = 4

        ''' <summary>
        ''' Visualizza il riquadro provvigione TAN
        ''' </summary>
        ShowProvvigioneTAN = 1024
    End Enum

    ''' <summary>
    ''' Definisce la modalità con cui il sistema calcola le provvigioni
    ''' </summary>
    Public Enum TipoCalcoloProvvigioni As Integer
        ''' <summary>
        ''' Le commissioni vengono calcolate come prodotto tra il montante lordo e la provvigione definita nella tabella finanziaria
        ''' </summary>
        MONTANTE_LORDO = 0

        ''' <summary>
        ''' Le commissioni vengono calcolate come prodotto tra la percentuale e la differenza tra il montante lordo e il valore da estinguere
        ''' </summary>
        DELTA_MONTANTE_ESTINZIONI = 1

        ''' <summary>
        ''' Le commissioni vengono calcolate sulla base di un formula 
        ''' </summary>
        FUNZIONE = 2048
    End Enum

    ''' <summary>
    ''' TABELLE FINANZIARIE ASSOCIABILI AD UN SINGOLO PRODOTTO
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CTabellaFinanziaria
        Inherits DBObject

        Private m_Nome As String                                    'Nome della tabella
        Private m_CessionarioID As Integer                          'ID del cessionario
        Private m_Cessionario As CCQSPDCessionarioClass             'Cessionario
        Private m_NomeCessionario As String                         'Nome del cessionario
        Private m_Descrizione As String                             'Nome della tabella visualizzato agli utenti finali
        Private m_Coeff() As Double?                                'Array delle percentuali di coefficiente base (per durata)
        Private m_Commiss() As Double?                              'Array delle percentuali di commissione (per durata)
        Private m_TANVariabile As Boolean                           'Se vero indica che la tabella è a TAN variabile
        Private m_TAN As Double?                                    'Valore del TAN (in caso di tabella con TAN fisso)
        Private m_DataInizio As Nullable(Of Date)                   'Data di inizio validità della tabella
        Private m_DataFine As Nullable(Of Date)                     'Data di fine validità della tabella
        Private m_Visible As Boolean                                'La tabella è visibile nell'elenco delle tabelle quando si fa un preventivo
        Private m_ProvvMax As Double?                               'Percentuale di UpFront massimo
        Private m_ProvvMaxConRinnovi As Double?                     'Percentuale di riduzione sul provvigionale in caso di estinzioni (interni)
        Private m_ProvvMaxConEstinzioni As Double?                  'Percentuale di riduzione sul provvigionale in caso di estinzioni (esterne)
        Private m_ProvvAggVisib As Double                           'Provvigione aggiuntiva visibile
        Private m_Sconto As Double?                                 'Percentuale dello sconto (rappel)
        Private m_ProvvTANR As Double                                'Percentuale provv. TAN in caso di rinnovi
        Private m_ProvvTANE As Double                                'Percentuale provv. TAN in caso di estinzioni
        Private m_Flags As TabellaFinanziariaFlags                  'Flags
        Private m_Attributi As CKeyCollection                       'Collezione di attributi aggiuntivi
        Private m_UpFrontMax As Decimal?                            'Valore in euro che indica il limite massimo dell'upfront pagato dal cessionario all'agenzia
        Private m_TipoCalcoloProvvigioni As TipoCalcoloProvvigioni  'Valore che indica come calcolare le provvigioni
        Private m_TipoCalcoloProvvTAN As TipoCalcoloProvvigioni     'Valore che indica come calcolare le provvigioni tan (provvigioni ScontoVisibile)
        Private m_FormulaProvvigioni As String                      'Formula usata per il calcolo delle provvigioni quando in TipoCalcoloProvvigioni è impostato il valore TipoCalcoloProvvigioni.FUNZIONI
        Private m_ScontoVisibile As Double                          'Percentuale dello sconto (rappel) che è resa visibile agli utenti


        Public Sub New()
            Me.m_Nome = ""
            Me.m_CessionarioID = 0
            Me.m_Cessionario = Nothing
            Me.m_NomeCessionario = ""
            Me.m_Descrizione = ""
            Me.m_Sconto = Nothing
            ReDim m_Coeff(10)
            ReDim m_Commiss(10)
            Me.m_TANVariabile = False
            Me.m_TAN = Nothing
            Me.m_ProvvMax = Nothing
            Me.m_ProvvMaxConEstinzioni = Nothing
            Me.m_ProvvMaxConRinnovi = Nothing
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Visible = True
            Me.m_Sconto = Nothing
            Me.m_Flags = TabellaFinanziariaFlags.None
            Me.m_Attributi = New CKeyCollection
            Me.m_UpFrontMax = Nothing
            Me.m_TipoCalcoloProvvigioni = TipoCalcoloProvvigioni.MONTANTE_LORDO
            Me.m_FormulaProvvigioni = vbNullString
            Me.m_ScontoVisibile = 0
            Me.m_ProvvAggVisib = 0
            Me.m_ProvvTANR = 0.0
            Me.m_ProvvTANE = 0.0
            Me.m_TipoCalcoloProvvTAN = TipoCalcoloProvvigioni.MONTANTE_LORDO
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la provvigione tan in caso di rinnovi
        ''' </summary>
        ''' <returns></returns>
        Public Property ProvvTANR As Double
            Get
                Return Me.m_ProvvTANR
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ProvvTANR
                If (oldValue = value) Then Return
                Me.m_ProvvTANR = value
                Me.DoChanged("ProvvTANR", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione tan in caso di estinzioni
        ''' </summary>
        ''' <returns></returns>
        Public Property ProvvTANE As Double
            Get
                Return Me.m_ProvvTANE
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ProvvTANE
                If (oldValue = value) Then Return
                Me.m_ProvvTANE = value
                Me.DoChanged("ProvvTANE", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Valore che determina come vengono calcolate le provvigioni TAN
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoCalcoloProvvTAN As TipoCalcoloProvvigioni
            Get
                Return Me.m_TipoCalcoloProvvTAN
            End Get
            Set(value As TipoCalcoloProvvigioni)
                Dim oldValue As TipoCalcoloProvvigioni = Me.m_TipoCalcoloProvvTAN
                If (oldValue = value) Then Return
                Me.m_TipoCalcoloProvvTAN = value
                Me.DoChanged("TipoCalcoloProvvTAN", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione aggiuntiva visibile
        ''' </summary>
        ''' <returns></returns>
        Public Property ProvvAggVisib As Double
            Get
                Return Me.m_ProvvAggVisib
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ProvvAggVisib
                If (oldValue = value) Then Return
                Me.m_ProvvAggVisib = value
                Me.DoChanged("ProvvAggVisib", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica come calcolare le provvigioni
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoCalcoloProvvigioni As TipoCalcoloProvvigioni
            Get
                Return Me.m_TipoCalcoloProvvigioni
            End Get
            Set(value As TipoCalcoloProvvigioni)
                Dim oldValue As TipoCalcoloProvvigioni = Me.m_TipoCalcoloProvvigioni
                If (oldValue = value) Then Return
                Me.m_TipoCalcoloProvvigioni = value
                Me.DoChanged("TipoCalcoloProvvigioni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Nel caso in TipoCalcoloProvvigioni sia impostato il tipo "Formula" questo campo rappresenta la formula da valutare
        ''' </summary>
        ''' <returns></returns>
        Public Property FormulaProvvigioni As String
            Get
                Return Me.m_FormulaProvvigioni
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FormulaProvvigioni
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_FormulaProvvigioni = value
                Me.DoChanged("FormulaProvvigioni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il massimo UpFront caricabile con questa tabella finanziaria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UpFrontMax As Decimal?
            Get
                Return Me.m_UpFrontMax
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_UpFrontMax
                If (oldValue = value) Then Exit Property
                Me.m_UpFrontMax = value
                Me.DoChanged("UpFrontMax", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                Return Me.m_Attributi
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleFinanziarie.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'utilizzo della tabella finanziaria è sottoposto all'approvazione di un supervisore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiedeApprovazione As Boolean
            Get
                Return TestFlag(Me.m_Flags, TabellaFinanziariaFlags.RichiedeApprovazione)
            End Get
            Set(value As Boolean)
                If (Me.RichiedeApprovazione = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TabellaFinanziariaFlags.RichiedeApprovazione, value)
                Me.DoChanged("RichiedeApprovazione", value, Not value)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta lo sconto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sconto As Double?
            Get
                Return Me.m_Sconto
            End Get
            Set(value As Double?)
                Dim oldValue As Double = Me.m_Sconto
                If (oldValue = value) Then Exit Property
                Me.m_Sconto = value
                Me.DoChanged("Sconto", value, oldValue)
            End Set
        End Property

        Public Property Flags As TabellaFinanziariaFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As TabellaFinanziariaFlags)
                Dim oldValue As TabellaFinanziariaFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione massima caricabile per il prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvMax As Double?
            Get
                Return Me.m_ProvvMax
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_ProvvMax
                If (oldValue = value) Then Exit Property
                Me.m_ProvvMax = value
                Me.DoChanged("ProvvMax", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione massima in caso di rinnovo (estinzione di pratica dello stesso cessionario)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvMaxConRinnovi As Double?
            Get
                Return Me.m_ProvvMaxConRinnovi
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_ProvvMaxConRinnovi
                If (oldValue = value) Then Exit Property
                Me.m_ProvvMaxConRinnovi = value
                Me.DoChanged("ProvvMaxConRinnovi", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione massima in caso di estinzione (estinzione di pratica da cessionario diverso)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvMaxConEstinzioni As Double?
            Get
                Return Me.m_ProvvMaxConEstinzioni
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_ProvvMaxConEstinzioni
                If (oldValue = value) Then Exit Property
                Me.m_ProvvMaxConEstinzioni = value
                Me.DoChanged("ProvvMaxConEstinzioni", value, oldValue)
            End Set
        End Property

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

        Public Property IDCessionario As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_CessionarioID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCessionario
                If oldValue = value Then Exit Property
                Me.m_CessionarioID = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("IDCessionario", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = DMD.CQSPD.Cessionari.GetItemById(Me.m_CessionarioID)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.m_Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_CessionarioID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

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

        Public Property CoefficienteBase(ByVal durata As Integer) As Double?
            Get
                Return Me.m_Coeff(Fix(durata / 12))
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Coeff(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Coeff(Fix(durata / 12)) = value
                Me.DoChanged("CoefficienteBase", value, oldValue)
            End Set
        End Property

        Public Property CommissioniBancarie(ByVal durata As Integer) As Double?
            Get
                Return Me.m_Commiss(Fix(durata / 12))
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Commiss(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Commiss(Fix(durata / 12)) = value
                Me.DoChanged("CommissioniBancarie", value, oldValue)
            End Set
        End Property

        Public Property Interessi(ByVal durata As Integer) As Double?
            Get
                If (Me.CoefficienteBase(durata).HasValue AndAlso Me.CommissioniBancarie(durata).HasValue) Then
                    Return Me.CoefficienteBase(durata) - Me.CommissioniBancarie(durata)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Double?)
                If (value.HasValue) Then
                    If Me.CoefficienteBase(durata).HasValue Then
                        Me.CommissioniBancarie(durata) = Me.CoefficienteBase(durata) - value
                    ElseIf Me.CommissioniBancarie(durata).HasValue Then
                        Me.CoefficienteBase(durata) = Me.CommissioniBancarie(durata) + value
                    Else
                        Me.CoefficienteBase(durata) = value
                    End If
                Else
                    'Me.CommissioniBancarie(durata) = Me.CoefficienteBase(durata) - value
                End If
            End Set
        End Property

        Public Property TANVariabile As Boolean
            Get
                Return Me.m_TANVariabile
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.m_TANVariabile
                If (oldValue = value) Then Exit Property
                Me.m_TANVariabile = value
                Me.DoChanged("TANVariabile", value, oldValue)
            End Set
        End Property

        Public Property TAN As Double?
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAN
                If (oldValue = value) Then Exit Property
                Me.m_TAN = value
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

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
        ''' Restituisce vero se la tabella è valida alla data odierna
        ''' </summary>
        ''' <returns></returns>
        Public Function IsValid() As Boolean
            Return Me.IsValid(Calendar.Now)
        End Function

        ''' <summary>
        ''' Restituisce vero se la tabella è valida alla data specificata
        ''' </summary>
        ''' <param name="atDate"></param>
        ''' <returns></returns>
        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return Me.Stato = ObjectStatus.OBJECT_VALID AndAlso Calendar.CheckBetween(atDate, Me.m_DataInizio, Me.m_DataFine)
        End Function

        ''' <summary>
        ''' Restituisce o imposta la percentuale visibile dello sconto (Rappel)
        ''' </summary>
        ''' <returns></returns>
        Public Property ScontoVisibile As Double
            Get
                Return Me.m_ScontoVisibile
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ScontoVisibile
                If (value = oldValue) Then Return
                Me.m_ScontoVisibile = value
                Me.DoChanged("ScontoVisibile", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_TblFin"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_CessionarioID = reader.Read("Cessionario", Me.m_CessionarioID)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            For i As Integer = 1 To 10
                Me.m_Coeff(i) = reader.Read("c" & (i * 12), Me.m_Coeff(i))
                Me.m_Commiss(i) = reader.Read("i" & (i * 12), Me.m_Commiss(i))
            Next
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_TAN = reader.Read("TAN", Me.m_TAN)
            Me.m_TANVariabile = reader.Read("TANVariabile", Me.m_TANVariabile)
            Me.m_Visible = reader.Read("Visible", Me.m_Visible)
            Me.m_ProvvMax = reader.Read("ProvvMax", Me.m_ProvvMax)
            Me.m_ProvvMaxConRinnovi = reader.Read("ProvvMaxRinn", Me.m_ProvvMaxConRinnovi)
            Me.m_ProvvMaxConEstinzioni = reader.Read("ProvvMaxEst", Me.m_ProvvMaxConEstinzioni)
            Me.m_Sconto = reader.Read("Sconto", Me.m_Sconto)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_UpFrontMax = reader.Read("UpFrontMax", Me.m_UpFrontMax)
            Try
                Dim attributiString As String = ""
                attributiString = reader.Read("Attributi", attributiString)
                If (attributiString <> "") Then
                    Me.m_Attributi = XML.Utils.Serializer.Deserialize(attributiString)
                Else
                    Me.m_Attributi = New CKeyCollection
                End If
            Catch ex As Exception
                Me.m_Attributi = New CKeyCollection
            End Try
            Me.m_TipoCalcoloProvvigioni = reader.Read("TipoCalcoloProvvigioni", Me.m_TipoCalcoloProvvigioni)
            Me.m_FormulaProvvigioni = reader.Read("FormulaProvvigioni", Me.m_FormulaProvvigioni)
            Me.m_ScontoVisibile = reader.Read("ScontoVisibile", Me.m_ScontoVisibile)
            Me.m_ProvvAggVisib = reader.Read("ProvvAggVisib", Me.m_ProvvAggVisib)
            Me.m_TipoCalcoloProvvTAN = reader.Read("TipoCalcoloProvvTAN", Me.m_TipoCalcoloProvvTAN)
            Me.m_ProvvTANR = reader.Read("ProvvTANR", Me.m_ProvvTANR)
            Me.m_ProvvTANE = reader.Read("ProvvTANE", Me.m_ProvvTANE)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Cessionario", Me.IDCessionario)
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            For i As Integer = 1 To 10
                writer.Write("c" & (i * 12), Me.m_Coeff(i))
                writer.Write("i" & (i * 12), Me.m_Commiss(i))
            Next
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("TANVariabile", Me.m_TANVariabile)
            writer.Write("Visible", Me.m_Visible)
            writer.Write("ProvvMax", Me.m_ProvvMax)
            writer.Write("ProvvMaxRinn", Me.m_ProvvMaxConRinnovi)
            writer.Write("ProvvMaxEst", Me.m_ProvvMaxConEstinzioni)
            writer.Write("Sconto", Me.m_Sconto)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("UpFrontMax", Me.m_UpFrontMax)
            writer.Write("TipoCalcoloProvvigioni", Me.m_TipoCalcoloProvvigioni)
            writer.Write("FormulaProvvigioni", Me.m_FormulaProvvigioni)
            writer.Write("ScontoVisibile", Me.m_ScontoVisibile)
            writer.Write("ProvvAggVisib", Me.m_ProvvAggVisib)
            writer.Write("TipoCalcoloProvvTAN", Me.m_TipoCalcoloProvvTAN)
            writer.Write("ProvvTANR", Me.m_ProvvTANR)
            writer.Write("ProvvTANE", Me.m_ProvvTANE)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("TAN", Me.m_TAN)
            writer.WriteAttribute("TANVariabile", Me.m_TANVariabile)
            writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("ProvvMax", Me.m_ProvvMax)
            writer.WriteAttribute("ProvvMaxRinn", Me.m_ProvvMaxConRinnovi)
            writer.WriteAttribute("ProvvMaxEst", Me.m_ProvvMaxConEstinzioni)
            writer.WriteAttribute("Sconto", Me.m_Sconto)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("UpFrontMax", Me.m_UpFrontMax)
            writer.WriteAttribute("TipoCalcoloProvvigioni", Me.m_TipoCalcoloProvvigioni)
            writer.WriteAttribute("ScontoVisibile", Me.m_ScontoVisibile)
            writer.WriteAttribute("ProvvAggVisib", Me.m_ProvvAggVisib)
            writer.WriteAttribute("TipoCalcoloProvvTAN", Me.m_TipoCalcoloProvvTAN)
            writer.WriteAttribute("ProvvTANR", Me.m_ProvvTANR)
            writer.WriteAttribute("ProvvTANE", Me.m_ProvvTANE)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Coefficienti", Me.m_Coeff)
            writer.WriteTag("Commissioni", Me.m_Commiss)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("FormulaProvvigioni", Me.m_FormulaProvvigioni)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCessionario" : Me.m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Coefficienti" : Me.m_Coeff = Arrays.Convert(Of Double?)(fieldValue)
                Case "Commissioni" : Me.m_Commiss = Arrays.Convert(Of Double?)(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TANVariabile" : Me.m_TANVariabile = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ProvvMax" : Me.m_ProvvMax = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvMaxRinn" : Me.m_ProvvMaxConRinnovi = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvMaxEst" : Me.m_ProvvMaxConEstinzioni = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Sconto" : Me.m_Sconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UpFrontMax" : Me.m_UpFrontMax = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case "TipoCalcoloProvvigioni" : Me.m_TipoCalcoloProvvigioni = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FormulaProvvigioni" : Me.m_FormulaProvvigioni = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ScontoVisibile" : Me.m_ScontoVisibile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvAggVisib" : Me.m_ProvvAggVisib = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TipoCalcoloProvvTAN" : Me.m_TipoCalcoloProvvTAN = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ProvvTANR" : Me.m_ProvvTANR = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvTANE" : Me.m_ProvvTANE = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            CQSPD.TabelleFinanziarie.UpdateCached(Me)
            Return ret
        End Function


        Public Function CalcolaProvvigioni(ByVal o As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Decimal
            Dim ret As Decimal? = Nothing
            Dim ml As Decimal? = o.MontanteLordo
            Dim sumEst As Decimal = 0


            Select Case Me.TipoCalcoloProvvigioni
                Case TipoCalcoloProvvigioni.MONTANTE_LORDO

                Case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI
                Case TipoCalcoloProvvigioni.FUNZIONE
                Case Else
                    Throw New NotSupportedException("TipoCalcoloProvvigioni")
            End Select

            Return ret
        End Function

        Public Function CalcolaProvvigioniTAN(ByVal o As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore)) As Decimal
            Dim ret As Decimal? = Nothing
            Dim ml As Decimal? = o.MontanteLordo
            Dim sumEst As Decimal = 0


            Select Case Me.TipoCalcoloProvvigioni
                Case TipoCalcoloProvvigioni.MONTANTE_LORDO

                Case TipoCalcoloProvvigioni.DELTA_MONTANTE_ESTINZIONI
                Case TipoCalcoloProvvigioni.FUNZIONE
                Case Else
                    Throw New NotSupportedException("TipoCalcoloProvvigioni")
            End Select

            Return ret
        End Function

    End Class


End Class
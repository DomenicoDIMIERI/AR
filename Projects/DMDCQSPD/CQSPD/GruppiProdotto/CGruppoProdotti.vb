Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Rappresenta un gruppo di prodotti
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CGruppoProdotti
        Inherits DBObject

        Private m_NomeCessionario As String
        Private m_CessionarioID As Integer
        <NonSerialized> _
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_TipoContrattoID As String
        'Private m_TipoContratto As CQSPDTipoContratto
        Private m_TipoRapportoID As String
        'Private m_TipoRapporto As TipoRapporto
        Private m_costoPrestitalia As Decimal
        Private m_TipoCostoPrestitalia As Integer
        Private m_CostoAmministrazione As Decimal
        Private m_TipoCostoAmministrazione As Integer
        Private m_TipoCalcoloTEG As TipoCalcoloTEG
        Private m_Scomposizione As Integer
        'Private m_IDProduttore As Integer
        'Private m_Produttore As  pro
        Private m_ImpostaSostitutiva As Double
        Private m_ProvvigioneMassima As Double
        Private m_InBando As Boolean
        ' Private m_Assicurazioni
        'Private m_Convenzioni
        Private m_CostoPerRata As Decimal
        Private m_Rappel As Double
        Private m_Costo As Decimal
        'Private m_CriteriAssuntiviID
        'Private m_CriteriAssuntivi
        'Private m_DocumentiPerPratica As CDocumentoPerPraticaGrpCollection
        Private m_Descrizione As String
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_Visible As Boolean
        <NonSerialized> Private m_StatoIniziale As CStatoPratica
        Private m_IDStatoIniziale As Integer
        <NonSerialized> Private m_Documenti As CDocumentiXGruppoProdottiCollection

        Private m_SpeseCoefficiente() As Double
        Private m_Interessi() As Double
        Private m_Durata() As Double
        Private m_SpeseFisse() As Decimal
        Private m_Rivalsa() As Decimal

        <NonSerialized> _
        Private m_Lock As New Object

        <NonSerialized> Private m_Provvigioni As CCQSPDTipoProvvigioneCollection

        Public Sub New()
            Me.m_Descrizione = "Nuovo gruppo prodotti"
            Me.m_NomeCessionario = ""
            Me.m_CessionarioID = 0
            Me.m_Cessionario = Nothing
            Me.m_TipoContrattoID = "C"
            '            m_TipoContratto = Nothing
            Me.m_TipoRapportoID = ""
            '           m_TipoRapporto = Nothing
            Me.m_costoPrestitalia = 0
            Me.m_TipoCostoPrestitalia = 0
            Me.m_CostoAmministrazione = 0
            Me.m_TipoCostoAmministrazione = 0
            Me.m_TipoCalcoloTEG = 0
            Me.m_Scomposizione = 0
            'm_IDProduttore = 0
            'm_Produttore = Nothing
            ReDim m_Durata(10)
            Me.m_ImpostaSostitutiva = 0
            Me.m_ProvvigioneMassima = 0
            Me.m_InBando = False
            'm_Documenti = Nothing
            ReDim m_SpeseFisse(10)
            ReDim m_SpeseCoefficiente(10)
            Me.m_CostoPerRata = 0
            ReDim m_Interessi(10)
            Me.m_Rappel = 0
            Me.m_Costo = 0
            ' m_CriteriAssuntivi = Nothing
            ReDim m_Rivalsa(10)
            'm_DocumentiPerPratica = Nothing
            Me.m_Descrizione = ""
            Me.m_Visible = True
            Me.m_StatoIniziale = Nothing
            Me.m_IDStatoIniziale = 0
            Me.m_Documenti = Nothing
            Me.m_Provvigioni = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce la collezione delle provvigioni definite per il gruppo prodotti
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Provvigioni As CCQSPDTipoProvvigioneCollection
            Get
                If (Me.m_Provvigioni Is Nothing) Then Me.m_Provvigioni = New CCQSPDTipoProvvigioneCollection(Me)
                Return Me.m_Provvigioni
            End Get
        End Property

        Friend Sub InvalidateTipiProvvigione()
            Me.m_Provvigioni = Nothing
        End Sub

        Public ReadOnly Property Documenti As CDocumentiXGruppoProdottiCollection
            Get
                SyncLock Me.m_Lock
                    If (Me.m_Documenti Is Nothing) Then Me.m_Documenti = New CDocumentiXGruppoProdottiCollection(Me)
                    Return Me.m_Documenti
                End SyncLock
            End Get
        End Property

        Public Property StatoIniziale As CStatoPratica
            Get
                If (Me.m_StatoIniziale Is Nothing) Then Me.m_StatoIniziale = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDStatoIniziale)
                Return Me.m_StatoIniziale
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_StatoIniziale
                If (oldValue Is value) Then Exit Property
                Me.m_StatoIniziale = value
                Me.m_IDStatoIniziale = GetID(value)
                Me.DoChanged("StatoIniziale", value, oldValue)
            End Set
        End Property

        Public Property IDStatoIniziale As Integer
            Get
                Return GetID(Me.m_StatoIniziale, Me.m_IDStatoIniziale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoIniziale
                If (oldValue = value) Then Exit Property
                Me.m_IDStatoIniziale = value
                Me.m_StatoIniziale = Nothing
                Me.DoChanged("IDStatoIniziale", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.GruppiProdotto.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il gruppo prodotti è visibile
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

        Public Property TipoContrattoID As String
            Get
                Return Me.m_TipoContrattoID
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_TipoContrattoID
                If (oldValue = value) Then Exit Property
                Me.m_TipoContrattoID = value
                Me.DoChanged("TipoContrattoID", value, oldValue)
            End Set
        End Property

        Public Property TipoRapportoID As String
            Get
                Return Me.m_TipoRapportoID
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_TipoRapportoID
                If (oldValue = value) Then Exit Property
                Me.m_TipoRapportoID = value
                Me.DoChanged("TipoRapportoID", value, oldValue)
            End Set
        End Property

        Public Property Rivalsa(ByVal durata As Integer) As Decimal
            Get
                Return Me.m_Rivalsa(Fix(durata / 12))
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Rivalsa(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Rivalsa(Fix(durata / 12)) = value
                Me.DoChanged("Rivalsa", value, oldValue)
            End Set
        End Property

        Public Property Coefficiente(ByVal durata As Integer) As Double
            Get
                Return Me.m_Durata(Fix(durata / 12))
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Durata(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Durata(Fix(durata / 12)) = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        Public Property Interessi(ByVal durata As Integer) As Double
            Get
                Return Me.m_Interessi(Fix(durata / 12))
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Interessi(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Interessi(Fix(durata / 12)) = value
                Me.DoChanged("Interessi", value, oldValue)
            End Set
        End Property

        Public Property Commissioni(ByVal durata As Integer) As Double
            Get
                Return Me.Coefficiente(durata) - Me.Interessi(durata)
            End Get
            Set(value As Double)
                Me.Coefficiente(durata) = Me.Interessi(durata) + value
            End Set
        End Property

        Public Property CostoPerRata() As Decimal
            Get
                Return Me.m_CostoPerRata
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_CostoPerRata
                If (oldValue = value) Then Exit Property
                Me.m_CostoPerRata = value
                Me.DoChanged("CostoPerRata", value, oldValue)
            End Set
        End Property

        Public Property SpeseFisse(ByVal durata As Integer) As Decimal
            Get
                Return Me.m_SpeseFisse(Fix(durata / 12))
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SpeseFisse(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpeseFisse(Fix(durata / 12)) = value
                Me.DoChanged("SpeseFisse", value, oldValue)
            End Set
        End Property

        Public Property SpeseCoefficiente(ByVal durata As Integer) As Double
            Get
                Return Me.m_SpeseCoefficiente(Fix(durata / 12))
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_SpeseCoefficiente(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_SpeseCoefficiente(Fix(durata / 12)) = value
                Me.DoChanged("SpeseCoefficiente", value, oldValue)
            End Set
        End Property

        Public Property ProvvigioneMassima As Double
            Get
                Return Me.m_ProvvigioneMassima
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ProvvigioneMassima
                If (oldValue = value) Then Exit Property
                Me.m_ProvvigioneMassima = value
                Me.DoChanged("ProvvigioneMassima", value, oldValue)
            End Set
        End Property

        Public Property CostoPrestitalia As Decimal
            Get
                Return Me.m_costoPrestitalia
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_costoPrestitalia
                If (oldValue = value) Then Exit Property
                Me.m_costoPrestitalia = value
                Me.DoChanged("CostoPrestitalia", value, oldValue)
            End Set
        End Property

        Public Property CostoAmministrazione As Decimal
            Get
                Return Me.m_CostoAmministrazione
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_CostoAmministrazione
                If (oldValue = value) Then Exit Property
                Me.m_CostoAmministrazione = value
                Me.DoChanged("CostoAmministrazione", value, oldValue)
            End Set
        End Property

        Public Property ImpostaSostitutiva As Double
            Get
                Return Me.m_ImpostaSostitutiva
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ImpostaSostitutiva
                If (oldValue = value) Then Exit Property
                Me.m_ImpostaSostitutiva = value
                Me.DoChanged("ImpostaSostitutiva", value, oldValue)
            End Set
        End Property

        Public Property Costo As Decimal
            Get
                Return Me.m_Costo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Costo
                If (oldValue = value) Then Exit Property
                Me.m_Costo = value
                Me.DoChanged("Costo", value, oldValue)
            End Set
        End Property

        Public Property Rappel As Double
            Get
                Return Me.m_Rappel
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Rappel
                If (oldValue = value) Then Exit Property
                Me.m_Rappel = value
                Me.DoChanged("Rappel", value, oldValue)
            End Set
        End Property

        Public Property TipoCalcoloTeg As TipoCalcoloTEG
            Get
                Return Me.m_TipoCalcoloTEG
            End Get
            Set(value As TipoCalcoloTEG)
                Dim oldValue As TipoCalcoloTEG = Me.m_TipoCalcoloTEG
                If (oldValue = value) Then Exit Property
                Me.m_TipoCalcoloTEG = value
                Me.DoChanged("TipoCalcoloTEG", value, oldValue)
            End Set
        End Property

        Public Function GetMaxTEG(ByVal calculator As COffertaCQS) As Double
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            Dim ret As Double
            Dim montanteLordo As Decimal
            Dim durata As Integer

            montanteLordo = calculator.MontanteLordo
            durata = calculator.Durata

            ret = 0
            Select Case TipoCalcoloTeg
                Case TipoCalcoloTeg.TEG_COSTANTE
                    dbSQL = "SELECT * FROM [tbl_TEG_Costante] WHERE [Tipo]='G' And [Prodotto] = " & ID & ";"
                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If dbRis.Read Then
                        ret = Formats.ToDouble(dbRis("teg" & calculator.Durata))
                    End If
                    dbRis.Dispose()
                    dbRis = Nothing
                Case TipoCalcoloTeg.TEG_FDIML
                    dbSQL = "SELECT * FROM [tbl_TEG_ML] WHERE [Tipo]='G' And [FinoAML] = (SELECT Min([FinoAML]) FROM [tbl_TEG_ML] WHERE [Tipo]='G' And [FinoAML]>" & DBUtils.DBNumber(calculator.MontanteLordo) & " And [Prodotto]=" & ID & ") And [Prodotto]=" & ID & ";"
                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If dbRis.Read Then
                        ret = Formats.ToDouble(dbRis("teg" & calculator.Durata))
                    End If
                    dbRis.Dispose()
                    dbRis = Nothing
                Case TipoCalcoloTeg.TEG_FDIETA
                    dbSQL = "SELECT * FROM [tbl_TEG_Eta] WHERE [Tipo]='G' And [Eta] = (SELECT Min([Eta]) FROM [tbl_TEG_Eta] WHERE [Tipo]='G' And [Eta]>" & calculator.Eta & " And [Prodotto]=" & ID & ") And [Prodotto]=" & ID & ";"
                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If dbRis.Read Then
                        ret = Formats.ToDouble(dbRis("teg" & calculator.Durata))
                    End If
                    dbRis.Dispose()
                    dbRis = Nothing
                Case Else
                    'Response.Write("Parametro TipoCalcoloTeg non valido in CFsbPrevGruppo_Prodotti")
                    'Response.End
                    ret = -1
            End Select

            Return ret
        End Function

        '       	Property Get CriteriAssuntivi
        '	If (m_CriteriAssuntivi Is Nothing) And (m_CriteriAssuntiviID <> 0) Then
        '		Set m_CriteriAssuntivi = GestioneDocumentale.GetDocumentoByID(m_CriteriAssuntiviID)
        '	End If
        '	Set CriteriAssuntivi = m_CriteriAssuntivi
        'End Property
        'Property Set CriteriAssuntivi(value)
        '	Set m_CriteriAssuntivi = value
        '              m_CriteriAssuntiviID = GetObjectID(value)
        'End Property

        'Property Get CriteriAssuntiviID
        '	CriteriAssuntiviID = m_CriteriAssuntiviID
        '      End Property
        'Property Let CriteriAssuntiviID(value)
        '	m_CriteriAssuntiviID = Users.ParseInteger(value)
        '	Set m_CriteriAssuntivi = Nothing
        'End Property

        Public Function GetArrayProdottiValidi() As CCQSPDProdotto()
            Dim cursor As New CProdottiCursor
            Dim ret As New CCollection(Of CCQSPDProdotto)
            cursor.GruppoProdottiID.Value = GetID(Me)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.OnlyValid = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret.ToArray
        End Function

        'Public ReadOnly Property DocumentiPerPratica As CDocumentoPerPraticaGrpCollection
        '    Get
        '        If Me.m_DocumentiPerPratica Is Nothing Then
        '            Me.m_DocumentiPerPratica = New CDocumentoPerPraticaGrpCollection
        '            Me.m_DocumentiPerPratica.Load(Me)
        '        End If
        '        Return Me.m_DocumentiPerPratica
        '    End Get
        'End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_GruppiProdotti"
        End Function

        'Protected Overrides Function SaveToDatabase(dbConn As CDBConnection) As Boolean
        '    Dim ret As Boolean = MyBase.SaveToDatabase(dbConn)
        '    'If Not (Me.m_DocumentiPerPratica Is Nothing) Then dbConn.Save(Me.m_DocumentiPerPratica)
        '    Return ret
        'End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_CessionarioID = reader.Read("Cessionario", Me.m_CessionarioID)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_TipoContrattoID = reader.Read("TipoContratto", Me.m_TipoContrattoID)
            Me.m_TipoRapportoID = reader.Read("TipoRapporto", Me.m_TipoRapportoID)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_costoPrestitalia = reader.Read("CostoPrestitalia", Me.m_costoPrestitalia)
            Me.m_CostoAmministrazione = reader.Read("CostoAmministrazione", Me.m_CostoAmministrazione)
            Me.m_TipoCalcoloTEG = reader.Read("TipoCalcoloTeg", Me.m_TipoCalcoloTEG)
            Me.m_CostoPerRata = reader.Read("CostoPerRata", Me.m_CostoPerRata)
            Me.m_Rappel = reader.Read("Rappel", Me.m_Rappel)
            Me.m_Costo = reader.Read("Costo", Me.m_Costo)
            'm_CriteriAssuntiviID = Formats.ToInteger(dbRis("CriteriAssuntivi"))
            For i As Integer = 2 To 10
                Me.m_Durata(i) = reader.Read("durata" & (i * 12), Me.m_Durata(i))
                Me.m_SpeseFisse(i) = reader.Read("SpeseFisse" & (i * 12), Me.m_SpeseFisse(i))
                Me.m_SpeseCoefficiente(i) = reader.Read("SpeseCoefficiente" & (i * 12), Me.m_SpeseCoefficiente(i))
                Me.m_Interessi(i) = reader.Read("Interessi" & (i * 12), Me.m_Interessi(i))
                Me.m_Rivalsa(i) = reader.Read("rivalsa" & (i * 12), Me.m_Rivalsa(i))
            Next
            Me.m_ProvvigioneMassima = reader.Read("provvigioneMassima", Me.m_ProvvigioneMassima)
            Me.m_ImpostaSostitutiva = reader.Read("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Visible = reader.Read("Visible", Me.m_Visible)
            Me.m_IDStatoIniziale = reader.Read("IDStatoIniziale", Me.m_IDStatoIniziale)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            Dim i As Integer
            writer.Write("Cessionario", GetID(Me.m_Cessionario, Me.m_CessionarioID))
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("TipoContratto", Me.m_TipoContrattoID)
            writer.Write("TipoRapporto", Me.m_TipoRapportoID)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("costoPrestitalia", Me.m_costoPrestitalia)
            writer.Write("CostoAmministrazione", Me.m_CostoAmministrazione)
            writer.Write("TipoCalcoloTeg", Me.m_TipoCalcoloTEG)
            writer.Write("CostoPerRata", Me.m_CostoPerRata)
            writer.Write("Rappel", Me.m_Rappel)
            writer.Write("Costo", Me.m_Costo)
            'dbRis("CriteriAssuntivi") = m_CriteriAssuntiviID
            For i = 2 To 10
                writer.Write("durata" & (i * 12), Me.m_Durata(i))
                writer.Write("SpeseFisse" & (i * 12), Me.m_SpeseFisse(i))
                writer.Write("SpeseCoefficiente" & (i * 12), Me.m_SpeseCoefficiente(i))
                writer.Write("Interessi" & (i * 12), Me.m_Interessi(i))
                writer.Write("Rivalsa" & (i * 12), Me.m_Rivalsa(i))
            Next
            writer.Write("ProvvigioneMassima", Me.m_ProvvigioneMassima)
            writer.Write("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Visible", Me.m_Visible)
            writer.Write("IDStatoIniziale", Me.IDStatoIniziale)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("CessionarioID", Me.CessionarioID)
            writer.WriteAttribute("TipoContrattoID", Me.m_TipoContrattoID)
            writer.WriteAttribute("TipoRapportoID", Me.m_TipoRapportoID)
            writer.WriteAttribute("costoPrestitalia", Me.m_costoPrestitalia)
            writer.WriteAttribute("TipoCostoPrestitalia", Me.m_TipoCostoPrestitalia)
            writer.WriteAttribute("CostoAmministrazione", Me.m_CostoAmministrazione)
            writer.WriteAttribute("TipoCostoAmministrazione", Me.m_TipoCostoAmministrazione)
            writer.WriteAttribute("TipoCalcoloTEG", Me.m_TipoCalcoloTEG)
            writer.WriteAttribute("Scomposizione", Me.m_Scomposizione)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("IDStatoIniziale", Me.IDStatoIniziale)
            writer.WriteAttribute("ImpostaSostitutiva", Me.m_ImpostaSostitutiva)
            writer.WriteAttribute("ProvvigioneMassima", Me.m_ProvvigioneMassima)
            writer.WriteAttribute("InBando", Me.m_InBando)
            writer.WriteAttribute("CostoPerRata", Me.m_CostoPerRata)
            writer.WriteAttribute("Rappel", Me.m_Rappel)
            writer.WriteAttribute("Costo", Me.m_Costo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Provvigioni", Me.Provvigioni)
            writer.WriteTag("Durata", Me.m_Durata)
            writer.WriteTag("SpeseFisse", Me.m_SpeseFisse)
            writer.WriteTag("SpeseCoefficiente", Me.m_SpeseCoefficiente)
            writer.WriteTag("Interessi", Me.m_Interessi)
            writer.WriteTag("Rivalsa", Me.m_Rivalsa)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CessionarioID" : Me.m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContrattoID" : Me.m_TipoContrattoID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoRapportoID" : Me.m_TipoRapportoID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "costoPrestitalia" : Me.m_costoPrestitalia = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TipoCostoPrestitalia" : Me.m_TipoCostoPrestitalia = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CostoAmministrazione" : Me.m_CostoAmministrazione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TipoCostoAmministrazione" : Me.m_TipoCostoAmministrazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCalcoloTEG" : Me.m_TipoCalcoloTEG = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Scomposizione" : Me.m_Scomposizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Durata" : Me.m_Durata = Arrays.Convert(Of Double)(fieldValue)
                Case "ImpostaSostitutiva" : Me.m_ImpostaSostitutiva = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ProvvigioneMassima" : Me.m_ProvvigioneMassima = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InBando" : Me.m_InBando = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "SpeseFisse" : Me.m_SpeseFisse = Arrays.Convert(Of Decimal)(fieldValue)
                Case "SpeseCoefficiente" : Me.m_SpeseCoefficiente = Arrays.Convert(Of Double)(fieldValue)
                Case "CostoPerRata" : Me.m_CostoPerRata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Interessi" : Me.m_Interessi = Arrays.Convert(Of Double)(fieldValue)
                Case "Rappel" : Me.m_Rappel = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Costo" : Me.m_Costo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Rivalsa" : Me.m_Rivalsa = Arrays.Convert(Of Decimal)(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDStatoIniziale" : Me.m_IDStatoIniziale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Provvigioni" : Me.m_Provvigioni = XML.Utils.Serializer.ToObject(fieldValue) : Me.m_Provvigioni.SetGruppoProdotti(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse (Me.m_Documenti IsNot Nothing AndAlso Me.m_Documenti.IsChanged)
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret AndAlso Me.m_Documenti IsNot Nothing) Then Me.m_Documenti.Save(force)
            If (ret AndAlso Me.m_Provvigioni IsNot Nothing) Then Me.m_Provvigioni.Save(force)
            Return ret
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.GruppiProdotto.UpdateCached(Me)
        End Sub

        Public Overrides Sub InitializeFrom(ByVal obj As Object)
            Dim value As CGruppoProdotti = CType(obj, CGruppoProdotti)
            MyBase.InitializeFrom(value)
            Me.m_Durata = Arrays.Clone(Me.m_Durata)
            Me.m_SpeseFisse = Arrays.Clone(Me.m_SpeseFisse)
            Me.m_SpeseCoefficiente = Arrays.Clone(Me.m_SpeseCoefficiente)
            Me.m_Interessi = Arrays.Clone(Me.m_Interessi)
            Me.m_Rivalsa = Arrays.Clone(Me.m_Rivalsa)
            Me.m_Documenti = New CDocumentiXGruppoProdottiCollection() : Me.m_Documenti.SetOwner(Me)
            For Each doc1 As CDocumentoXGruppoProdotti In value.Documenti
                Dim doc As New CDocumentoXGruppoProdotti()
                doc.InitializeFrom(doc1)
                Me.m_Documenti.Add(doc)
            Next
            Me.m_Provvigioni = New CCQSPDTipoProvvigioneCollection() : Me.m_Provvigioni.SetGruppoProdotti(Me)
            For Each provv1 As CCQSPDTipoProvvigione In value.Provvigioni
                Dim provv As New CCQSPDTipoProvvigione()
                provv.InitializeFrom(provv1)
                Me.m_Provvigioni.Add(provv)
            Next
        End Sub


    End Class


End Class
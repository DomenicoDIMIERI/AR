Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    

    <Serializable> _
    Public Class CQSPDStudioDiFattibilita
        Inherits DBObjectPO

        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String
        Private m_Data As Date
        Private m_IDRichiesta As Integer
        Private m_Richiesta As CRichiestaFinanziamento
        Private m_OraInizio As Nullable(Of Date)
        Private m_OraFine As Nullable(Of Date)
        Private m_IDConsulente As Integer
        Private m_Consulente As CConsulentePratica
        Private m_NomeConsulente As String
        Private m_Impiego As CImpiegato
        Private m_SommaTrattenuteVolontarie As Decimal
        Private m_SommaCQS As Decimal
        Private m_SommaPD As Decimal
        Private m_SommaPignoramenti As Decimal
        Private m_GARF As Integer
        Private m_LimiteCumulativo As Decimal
        Private m_RataMaxC As Decimal
        Private m_RataMaxD As Decimal
        Private m_DecorrenzaPratica As Date
        Private m_Proposte As CQSPDSoluzioniXStudioDiFattibilita
        Private m_IDContesto As Integer
        Private m_TipoContesto As String
        
        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_Data = Now
            Me.m_IDRichiesta = 0
            Me.m_Richiesta = Nothing
            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing
            Me.m_NomeConsulente = ""
            Me.m_OraInizio = Nothing
            Me.m_OraFine = Nothing
            Me.m_Impiego = New CImpiegato
            Me.m_SommaTrattenuteVolontarie = 0
            Me.m_SommaCQS = 0
            Me.m_SommaPD = 0
            Me.m_SommaPignoramenti = 0
            Me.m_GARF = 0
            Me.m_LimiteCumulativo = 0
            Me.m_RataMaxC = 0
            Me.m_RataMaxD = 0
            Me.m_DecorrenzaPratica = CQSPD.Pratiche.CalcolaProssimaDecorrenza
            Me.m_Proposte = Nothing
            Me.m_IDContesto = 0
            Me.m_TipoContesto = ""
        End Sub

   

        ''' <summary>
        ''' Restituisce o imposta l'ID del contesto in cui è stata creata la consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDContesto As Integer
            Get
                Return Me.m_IDContesto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDContesto
                If (oldValue = value) Then Exit Property
                Me.m_IDContesto = value
                Me.DoChanged("IDContesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del contesto in cui è stato creato l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContesto As String
            Get
                Return Me.m_TipoContesto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoContesto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContesto = value
                Me.DoChanged("TipoContesto", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Proposte As CQSPDSoluzioniXStudioDiFattibilita
            Get
                If (Me.m_Proposte Is Nothing) Then Me.m_Proposte = New CQSPDSoluzioniXStudioDiFattibilita(Me)
                Return Me.m_Proposte
            End Get
        End Property

        Public Property DecorrenzaPratica As Date
            Get
                Return Me.m_DecorrenzaPratica
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DecorrenzaPratica
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DecorrenzaPratica = value
                Me.DoChanged("DecorrenzaPratica", value, oldValue)
            End Set
        End Property

        Public Property SommaTrattenuteVolontarie As Decimal
            Get
                Return Me.m_SommaTrattenuteVolontarie
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaTrattenuteVolontarie
                If (oldValue = value) Then Exit Property
                Me.m_SommaTrattenuteVolontarie = value
                Me.DoChanged("SommaTrattenuteVolontarie", value, oldValue)
            End Set
        End Property

        Public Property SommaCQS As Decimal
            Get
                Return Me.m_SommaCQS
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaCQS
                If (oldValue = value) Then Exit Property
                Me.m_SommaCQS = value
                Me.DoChanged("SommaCQS", value, oldValue)
            End Set
        End Property

        Public Property SommaPD As Decimal
            Get
                Return Me.m_SommaPD
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaPD
                If (oldValue = value) Then Exit Property
                Me.m_SommaPD = value
                Me.DoChanged("SommaPD", value, oldValue)
            End Set
        End Property

        Public Property SommaPignoramenti As Decimal
            Get
                Return Me.m_SommaPignoramenti
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SommaPignoramenti
                If (oldValue = value) Then Exit Property
                Me.m_SommaPignoramenti = value
                Me.DoChanged("SommaPignoramenti", value, oldValue)
            End Set
        End Property

        Public Property GARF As Integer
            Get
                Return Me.m_GARF
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_GARF
                If (oldValue = value) Then Exit Property
                Me.m_GARF = value
                Me.DoChanged("GARF", value, oldValue)
            End Set
        End Property

        Public Property LimiteCumulativo As Decimal
            Get
                Return Me.m_LimiteCumulativo
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_LimiteCumulativo
                If (oldValue = value) Then Exit Property
                Me.m_LimiteCumulativo = value
                Me.DoChanged("LimiteCumulativo", value, oldValue)
            End Set
        End Property

        Public Property RataMassimaCessione As Decimal
            Get
                Return Me.m_RataMaxC
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_RataMaxC
                If (oldValue = value) Then Exit Property
                Me.m_RataMaxC = value
                Me.DoChanged("RataMessimaCessione", value, oldValue)
            End Set
        End Property

        Public Property RataMassimaDelega As Decimal
            Get
                Return Me.m_RataMaxD
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_RataMaxD
                If (oldValue = value) Then Exit Property
                Me.m_RataMaxD = value
                Me.DoChanged("RataMassimaDelega", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Impiego As CImpiegato
            Get
                Return Me.m_Impiego
            End Get
        End Property


        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersona)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        Public Property IDRichiesta As Integer
            Get
                Return GetID(Me.m_Richiesta, Me.m_IDRichiesta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiesta = value
                Me.m_Richiesta = Nothing
                Me.DoChanged("Richiesta", value, oldValue)
            End Set
        End Property

        Public Property Richiesta As CRichiestaFinanziamento
            Get
                If (Me.m_Richiesta Is Nothing) Then Me.m_Richiesta = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiesta)
                Return Me.m_Richiesta
            End Get
            Set(value As CRichiestaFinanziamento)
                Dim oldValue As CRichiestaFinanziamento = Me.m_Richiesta
                If (oldValue Is value) Then Exit Property
                Me.m_Richiesta = value
                Me.m_IDRichiesta = GetID(value)
                Me.DoChanged("Richiesta", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetRichiesta(ByVal value As CRichiestaFinanziamento)
            Me.m_Richiesta = value
            Me.m_IDRichiesta = GetID(value)
        End Sub

        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulente
                If (oldValue = value) Then Exit Property
                Me.m_IDConsulente = value
                Me.m_Consulente = Nothing
                Me.DoChanged("IDConsulente", value, oldValue)
            End Set
        End Property

        Public Property Consulente As CConsulentePratica
            Get
                If (Me.m_Consulente Is Nothing) Then Me.m_Consulente = CQSPD.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Dim oldValue As CConsulentePratica = Me.m_Consulente
                If (oldValue Is value) Then Exit Property
                Me.m_Consulente = value
                Me.m_IDConsulente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeConsulente = value.Nome
                Me.DoChanged("Consulente", value, oldValue)
            End Set
        End Property

        Public Property NomeConsulente As String
            Get
                Return Me.m_NomeConsulente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeConsulente
                If (oldValue = value) Then Exit Property
                Me.m_NomeConsulente = value
                Me.DoChanged("NomeConsulente", value, oldValue)
            End Set
        End Property

        Public Property OraInizio As Nullable(Of Date)
            Get
                Return Me.m_OraInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraInizio
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_OraInizio = value
                Me.DoChanged("OraInizio", value, oldValue)
            End Set
        End Property

        Public Property OraFine As Nullable(Of Date)
            Get
                Return Me.m_OraFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraFine
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_OraFine = value
                Me.DoChanged("OraFine", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.StudiDiFattibilita.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDGrpConsulenze"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Impiego.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Me.m_Impiego.SetChanged(False)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDRichiesta = reader.Read("IDRichiesta", Me.m_IDRichiesta)
            Me.m_OraInizio = reader.Read("OraInizio", Me.m_OraInizio)
            Me.m_OraFine = reader.Read("OraFine", Me.m_OraFine)
            Me.m_IDConsulente = reader.Read("IDConsulente", Me.m_IDConsulente)
            Me.m_NomeConsulente = reader.Read("NomeConsulente", Me.m_NomeConsulente)

            Me.m_Impiego.IDAzienda = reader.Read("IDAzienda", Me.m_Impiego.IDAzienda)
            Me.m_Impiego.NomeAzienda = reader.Read("NomeAzienda", Me.m_Impiego.NomeAzienda)
            Me.m_Impiego.IDEntePagante = reader.Read("IDEntePagante", Me.m_Impiego.IDEntePagante)
            Me.m_Impiego.NomeEntePagante = reader.Read("NomeEntePagante", Me.m_Impiego.NomeEntePagante)
            Me.m_Impiego.StipendioNetto = reader.Read("StipendioNetto", Me.m_Impiego.StipendioNetto)
            Me.m_Impiego.TFR = reader.Read("TFR", Me.m_Impiego.TFR)
            Me.m_Impiego.TipoRapporto = reader.Read("TipoRapporto", Me.m_Impiego.TipoRapporto)
            Me.m_Impiego.SetChanged(False)

            Me.m_SommaTrattenuteVolontarie = reader.Read("SommaTrattenuteVolontarie", Me.m_SommaTrattenuteVolontarie)
            Me.m_SommaCQS = reader.Read("SommaCQS", Me.m_SommaCQS)
            Me.m_SommaPD = reader.Read("SommaPD", Me.m_SommaPD)
            Me.m_SommaPignoramenti = reader.Read("SommaPignoramenti", Me.m_SommaPignoramenti)
            Me.m_GARF = reader.Read("GARF", Me.m_GARF)
            Me.m_LimiteCumulativo = reader.Read("LimiteCumulativo", Me.m_LimiteCumulativo)
            Me.m_RataMaxC = reader.Read("RataMaxC", Me.m_RataMaxC)
            Me.m_RataMaxD = reader.Read("RataMaxD", Me.m_RataMaxD)
            Me.m_DecorrenzaPratica = reader.Read("DecorrenzaPratica", Me.m_DecorrenzaPratica)

            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)

           
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("Data", Me.m_Data)
            writer.Write("IDRichiesta", Me.IDRichiesta)
            writer.Write("OraInizio", Me.m_OraInizio)
            writer.Write("OraFine", Me.m_OraFine)
            writer.Write("IDConsulente", Me.IDConsulente)
            writer.Write("NomeConsulente", Me.m_NomeConsulente)

            writer.Write("IDAzienda", Me.m_Impiego.IDAzienda)
            writer.Write("NomeAzienda", Me.m_Impiego.NomeAzienda)
            writer.Write("IDEntePagante", Me.m_Impiego.IDEntePagante)
            writer.Write("NomeEntePagante", Me.m_Impiego.NomeEntePagante)
            writer.Write("StipendioNetto", Me.m_Impiego.StipendioNetto)
            writer.Write("TFR", Me.m_Impiego.TFR)
            writer.Write("TipoRapporto", Me.m_Impiego.TipoRapporto)

            writer.Write("SommaTrattenuteVolontarie", Me.m_SommaTrattenuteVolontarie)
            writer.Write("SommaCQS", Me.m_SommaCQS)
            writer.Write("SommaPD", Me.m_SommaPD)
            writer.Write("SommaPignoramenti", Me.m_SommaPignoramenti)
            writer.Write("GARF", Me.m_GARF)
            writer.Write("LimiteCumulativo", Me.m_LimiteCumulativo)
            writer.Write("RataMaxC", Me.m_RataMaxC)
            writer.Write("RataMaxD", Me.m_RataMaxD)
            writer.Write("DecorrenzaPratica", Me.m_DecorrenzaPratica)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("IDRichiesta", Me.IDRichiesta)
            writer.WriteAttribute("OraInizio", Me.m_OraInizio)
            writer.WriteAttribute("OraFine", Me.m_OraFine)
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("NomeConsulente", Me.m_NomeConsulente)
            writer.WriteAttribute("SommaTrattenuteVolontarie", Me.m_SommaTrattenuteVolontarie)
            writer.WriteAttribute("SommaCQS", Me.m_SommaCQS)
            writer.WriteAttribute("SommaPD", Me.m_SommaPD)
            writer.WriteAttribute("SommaPignoramenti", Me.m_SommaPignoramenti)
            writer.WriteAttribute("GARF", Me.m_GARF)
            writer.WriteAttribute("LimiteCumulativo", Me.m_LimiteCumulativo)
            writer.WriteAttribute("RataMaxC", Me.m_RataMaxC)
            writer.WriteAttribute("RataMaxD", Me.m_RataMaxD)
            writer.WriteAttribute("DecorrenzaPratica", Me.m_DecorrenzaPratica)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Impiego", Me.m_Impiego)
            writer.WriteTag("Proposte", Me.m_Proposte)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Dim x As XML.XMLSerializer = XML.Utils.Serializer
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = x.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = x.DeserializeString(fieldValue)
                Case "Data" : Me.m_Data = x.DeserializeDate(fieldValue)
                Case "IDRichiesta" : Me.m_IDRichiesta = x.DeserializeInteger(fieldValue)
                Case "OraInizio" : Me.m_OraInizio = x.DeserializeDate(fieldValue)
                Case "OraFine" : Me.m_OraFine = x.DeserializeDate(fieldValue)
                Case "IDConsulente" : Me.m_IDConsulente = x.DeserializeInteger(fieldValue)
                Case "NomeConsulente" : Me.m_NomeConsulente = x.DeserializeString(fieldValue)
                Case "Impiego" : Me.m_Impiego = fieldValue
                Case "SommaTrattenuteVolontarie" : Me.m_SommaTrattenuteVolontarie = x.DeserializeDouble(fieldValue)
                Case "SommaCQS" : Me.m_SommaCQS = x.DeserializeDouble(fieldValue)
                Case "SommaPD" : Me.m_SommaPD = x.DeserializeDouble(fieldValue)
                Case "SommaPignoramenti" : Me.m_SommaPignoramenti = x.DeserializeDouble(fieldValue)
                Case "GARF" : Me.m_GARF = x.DeserializeDouble(fieldValue)
                Case "LimiteCumulativo" : Me.m_LimiteCumulativo = x.DeserializeDouble(fieldValue)
                Case "RataMaxC" : Me.m_RataMaxC = x.DeserializeDouble(fieldValue)
                Case "RataMaxD" : Me.m_RataMaxD = x.DeserializeDouble(fieldValue)
                Case "DecorrenzaPratica" : Me.m_DecorrenzaPratica = x.DeserializeDate(fieldValue)
                Case "Proposte" : Me.m_Proposte = fieldValue : If (Me.m_Proposte IsNot Nothing) Then Me.m_Proposte.SetStudioDiFattibilita(Me)
                Case "IDContesto" : Me.m_IDContesto = x.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = x.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class

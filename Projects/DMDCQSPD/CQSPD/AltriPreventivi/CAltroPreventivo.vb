Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

      
    ''' <summary>
    ''' Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CAltroPreventivo
        Inherits DBObject

        Private m_IDRichiestaDiFinanziamento As Integer
        <NonSerialized> _
        Private m_RichiestaDiFinanziamento As CRichiestaFinanziamento

        Private m_Data As Nullable(Of Date)
        Private m_IDIstituto As Integer
        <NonSerialized> _
        Private m_Istituto As CCQSPDCessionarioClass
        Private m_NomeIstituto As String
        Private m_IDAgenzia As Integer
        <NonSerialized> _
        Private m_Agenzia As CAzienda
        Private m_NomeAgenzia As String

        Private m_IDAgente As Integer
        <NonSerialized> _
        Private m_Agente As CPersonaFisica
        Private m_NomeAgente As String

        Private m_Rata As Nullable(Of Decimal)
        Private m_Durata As Nullable(Of Integer)
        Private m_MontanteLordo As Nullable(Of Decimal)
        Private m_NettoRicavo As Nullable(Of Decimal)
        Private m_NettoAllaMano As Nullable(Of Decimal)
        Private m_TAN As Nullable(Of Double)
        Private m_TAEG As Nullable(Of Double)

        Private m_Descrizione As String

        Private m_DataAccettazione As Nullable(Of Date)

        Private m_Flags As Integer


        Public Sub New()
            Me.m_IDRichiestaDiFinanziamento = 0
            Me.m_RichiestaDiFinanziamento = Nothing
            Me.m_Data = Nothing
            Me.m_IDIstituto = 0
            Me.m_Istituto = Nothing
            Me.m_NomeIstituto = vbNullString
            Me.m_IDAgenzia = 0
            Me.m_Agenzia = Nothing
            Me.m_NomeAgenzia = vbNullString
            Me.m_IDAgente = 0
            Me.m_Agente = Nothing
            Me.m_NomeAgente = vbNullString
            Me.m_Rata = Nothing
            Me.m_Durata = Nothing
            Me.m_MontanteLordo = Nothing
            Me.m_NettoRicavo = Nothing
            Me.m_TAN = Nothing
            Me.m_TAEG = Nothing
            Me.m_Descrizione = vbNullString
            Me.m_DataAccettazione = Nothing
            Me.m_NettoAllaMano = Nothing
            Me.m_Flags = 0
        End Sub

        ''' <summary>
        ''' Restituisdce o imposta l'ID della richiesta di finanziamento in cui è stato registrato questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiestaDiFinanziamento As Integer
            Get
                Return GetID(Me.m_RichiestaDiFinanziamento, Me.m_IDRichiestaDiFinanziamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaDiFinanziamento
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaDiFinanziamento = value
                Me.m_RichiestaDiFinanziamento = Nothing
                Me.DoChanged("IDRichiestaDiFinanziamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la richiesta di finanziamento in cui è stato registrato questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaDiFinanziamento As CRichiestaFinanziamento
            Get
                If (Me.m_RichiestaDiFinanziamento Is Nothing) Then Me.m_RichiestaDiFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiestaDiFinanziamento)
                Return Me.m_RichiestaDiFinanziamento
            End Get
            Set(value As CRichiestaFinanziamento)
                Dim oldValue As CRichiestaFinanziamento = Me.m_RichiestaDiFinanziamento
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaDiFinanziamento = value
                Me.m_IDRichiestaDiFinanziamento = GetID(value)
                Me.DoChanged("RichiestaDiFinanziamento", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetRichiestaDiFinanziamento(ByVal value As CRichiestaFinanziamento)
            Me.m_RichiestaDiFinanziamento = value
            Me.m_IDRichiestaDiFinanziamento = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data di rilascio del preventivo
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
        ''' Restituisce o imposta L'ID dell'istituto cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDIstituto As Integer
            Get
                Return GetID(Me.m_Istituto, Me.m_IDIstituto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDIstituto
                If (oldValue = value) Then Exit Property
                Me.m_IDIstituto = value
                Me.m_Istituto = Nothing
                Me.DoChanged("IDIstituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'istituto cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Istituto As CCQSPDCessionarioClass
            Get
                If (Me.m_Istituto Is Nothing) Then Me.m_Istituto = CQSPD.Cessionari.GetItemById(Me.m_IDIstituto)
                Return Me.m_Istituto
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.m_Istituto
                If (oldValue Is value) Then Exit Property
                Me.m_Istituto = value
                Me.m_IDIstituto = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeIstituto = value.Nome
                Me.DoChanged("Istituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'istituto cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeIstituto As String
            Get
                Return Me.m_NomeIstituto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeIstituto
                If (oldValue = value) Then Exit Property
                Me.m_NomeIstituto = value
                Me.DoChanged("NomeIstituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'agenzia che ha richiesto il conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAgenzia As Integer
            Get
                Return GetID(Me.m_Agenzia, Me.m_IDAgenzia)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAgenzia
                If (oldValue = value) Then Exit Property
                Me.m_IDAgenzia = value
                Me.m_Agenzia = Nothing
                Me.DoChanged("IDAgenzia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusce o imposta l'agenzia  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Agenzia As CAzienda
            Get
                If (Me.m_Agenzia Is Nothing) Then Me.m_Agenzia = Anagrafica.Aziende.GetItemById(Me.m_IDAgenzia)
                Return Me.m_Agenzia
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Agenzia
                If (oldValue Is value) Then Exit Property
                Me.m_Agenzia = value
                Me.m_IDAgenzia = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAgenzia = value.Nominativo
                Me.DoChanged("Agenzia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'agenzia richiedente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAgenzia As String
            Get
                Return Me.m_NomeAgenzia
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAgenzia
                If (oldValue = value) Then Exit Property
                Me.m_NomeAgenzia = value
                Me.DoChanged("NomeAgenzia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'agente che ha richiesto il conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAgente As Integer
            Get
                Return GetID(Me.m_Agente, Me.m_IDAgente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAgente
                If (oldValue = value) Then Exit Property
                Me.m_IDAgente = value
                Me.m_Agente = Nothing
                Me.DoChanged("IDAgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Agente As CPersonaFisica
            Get
                If (Me.m_Agente Is Nothing) Then Me.m_Agente = Anagrafica.Persone.GetItemById(Me.m_IDAgente)
                Return Me.m_Agente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Agente
                If (oldValue Is value) Then Exit Property
                Me.m_Agente = value
                Me.m_IDAgente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAgente = value.Nominativo
                Me.DoChanged("Agente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'agente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAgente As String
            Get
                Return Me.m_NomeAgente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAgente
                If (oldValue = value) Then Exit Property
                Me.m_NomeAgente = value
                Me.DoChanged("NomeAgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di evasione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAccettazione As Nullable(Of Date)
            Get
                Return Me.m_DataAccettazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAccettazione
                If (oldValue = value) Then Exit Property
                Me.m_DataAccettazione = value
                Me.DoChanged("DataAccettazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la rata proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rata As Nullable(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Rata
                If (oldValue = value) Then Exit Property
                Me.m_Rata = value
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Durata As Nullable(Of Integer)
            Get
                Return Me.m_Durata
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_Durata
                If (oldValue = value) Then Exit Property
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il netto ricavo proposto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NettoRicavo As Nullable(Of Decimal)
            Get
                Return Me.m_NettoRicavo
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_NettoRicavo
                If (oldValue = value) Then Exit Property
                Me.m_NettoRicavo = value
                Me.DoChanged("NettoRicavo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il netto alla mano
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NettoAllaMano As Nullable(Of Decimal)
            Get
                Return Me.m_NettoAllaMano
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_NettoAllaMano
                If (oldValue = value) Then Exit Property
                Me.m_NettoAllaMano = value
                Me.DoChanged("NettoAllaMano", value, oldValue)
            End Set
        End Property

        Public Property MontanteLordo As Nullable(Of Decimal)
            Get
                Return Me.m_MontanteLordo
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_MontanteLordo
                If (oldValue = value) Then Exit Property
                Me.m_MontanteLordo = value
                Me.DoChanged("MontanteLordo", value, oldValue)
            End Set
        End Property

        Public Property TAN As Nullable(Of Double)
            Get
                Return Me.m_TAN
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_TAN
                If (oldValue = value) Then Exit Property
                Me.m_TAN = value
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

        Public Property TAEG As Nullable(Of Double)
            Get
                Return Me.m_TAEG
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_TAEG
                If (oldValue = value) Then Exit Property
                Me.m_TAEG = value
                Me.DoChanged("TAEG", value, oldValue)
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

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteAltriPrev"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaDiFinanziamento", Me.m_IDRichiestaDiFinanziamento)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDIstituto = reader.Read("IDIstituto", Me.m_IDIstituto)
            Me.m_NomeIstituto = reader.Read("NomeIstituto", Me.m_NomeIstituto)
            Me.m_IDAgenzia = reader.Read("IDAgenzia", Me.m_IDAgenzia)
            Me.m_NomeAgenzia = reader.Read("NomeAgenzia", Me.m_NomeAgenzia)
            Me.m_IDAgente = reader.Read("IDAgente", Me.m_IDAgente)
            Me.m_NomeAgente = reader.Read("NomeAgente", Me.m_NomeAgente)
            Me.m_Rata = reader.Read("Rata", Me.m_Rata)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_MontanteLordo = reader.Read("MontanteLordo", Me.m_MontanteLordo)
            Me.m_NettoRicavo = reader.Read("NettoRicavo", Me.m_NettoRicavo)
            Me.m_TAN = reader.Read("TAN", Me.m_TAN)
            Me.m_TAEG = reader.Read("TAEG", Me.m_TAEG)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_DataAccettazione = reader.Read("DataAccettazione", Me.m_DataAccettazione)
            Me.m_NettoAllaMano = reader.Read("NettoAllaMano", Me.m_NettoAllaMano)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDRichiestaDiFinanziamento", Me.IDRichiestaDiFinanziamento)
            writer.Write("Data", Me.m_Data)
            writer.Write("IDIstituto", Me.IDIstituto)
            writer.Write("NomeIstituto", Me.m_NomeIstituto)
            writer.Write("IDAgenzia", Me.IDAgenzia)
            writer.Write("NomeAgenzia", Me.m_NomeAgenzia)
            writer.Write("IDAgente", Me.IDAgente)
            writer.Write("NomeAgente", Me.m_NomeAgente)
            writer.Write("Rata", Me.m_Rata)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("MontanteLordo", Me.m_MontanteLordo)
            writer.Write("NettoRicavo", Me.m_NettoRicavo)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("TAEG", Me.m_TAEG)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("DataAccettazione", Me.m_DataAccettazione)
            writer.Write("NettoAllaMano", Me.m_NettoAllaMano)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDRichiestaDiFinanziamento", Me.IDRichiestaDiFinanziamento)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("DataAccettazione", Me.m_DataAccettazione)
            writer.WriteAttribute("IDIstituto", Me.IDIstituto)
            writer.WriteAttribute("NomeIstituto", Me.m_NomeIstituto)
            writer.WriteAttribute("IDAgenzia", Me.IDAgenzia)
            writer.WriteAttribute("NomeAgenzia", Me.m_NomeAgenzia)
            writer.WriteAttribute("IDAgente", Me.IDAgente)
            writer.WriteAttribute("NomeAgente", Me.m_NomeAgente)
            writer.WriteAttribute("Rata", Me.m_Rata)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("MontanteLordo", Me.m_MontanteLordo)
            writer.WriteAttribute("NettoRicavo", Me.m_NettoRicavo)
            writer.WriteAttribute("TAN", Me.m_TAN)
            writer.WriteAttribute("TAEG", Me.m_TAEG)
            writer.WriteAttribute("NettoAllaMano", Me.m_NettoAllaMano)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDRichiestaDiFinanziamento" : Me.m_IDRichiestaDiFinanziamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataAccettazione" : Me.m_DataAccettazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDIstituto" : Me.m_IDIstituto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeIstituto" : Me.m_NomeIstituto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAgenzia" : Me.m_IDAgenzia = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAgenzia" : Me.m_NomeAgenzia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAgente" : Me.m_IDAgente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAgente" : Me.m_NomeAgente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Rata" : Me.m_Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MontanteLordo" : Me.m_MontanteLordo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NettoRicavo" : Me.m_NettoRicavo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NettoAllaMano" : Me.m_NettoAllaMano = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG" : Me.m_TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return "Preventivo della concorrenza"
        End Function


    End Class


End Class

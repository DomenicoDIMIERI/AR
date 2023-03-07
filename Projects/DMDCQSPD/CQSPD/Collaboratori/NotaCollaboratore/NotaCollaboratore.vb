Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD


    <Serializable>
    Public Class NotaCollaboratore
        Inherits DBObject
        Implements IComparable

        Private m_IDPersona As Integer
        <NonSerialized> Private m_Persona As CPersonaFisica
        Private m_IDCollaboratore As Integer
        <NonSerialized> Private m_Collaboratore As CCollaboratore
        Private m_IDClienteXCollaboratore As Integer
        <NonSerialized> Private m_ClienteXCollaboratore As ClienteXCollaboratore
        Private m_Data As Date
        Private m_Tipo As String
        Private m_Indirizzo As String
        Private m_Flags As Integer
        Private m_Esito As Integer
        Private m_Parameters As CKeyCollection
        Private m_Scopo As String
        Private m_Nota As String

        Public Sub New()
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing
            Me.m_IDClienteXCollaboratore = 0
            Me.m_ClienteXCollaboratore = Nothing
            Me.m_Data = DateUtils.Now
            Me.m_Tipo = ""
            Me.m_Indirizzo = ""
            Me.m_Flags = 0
            Me.m_Esito = 0
            Me.m_Parameters = Nothing
            Me.m_Scopo = ""
            Me.m_Nota = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo della nota
        ''' </summary>
        ''' <returns></returns>
        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Return
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona fisica nel database principale associata al cliente gestito dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Return
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona fisica nel database principale associata al cliente gestito dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property Persona As CPersonaFisica
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Persona
                If (oldValue Is value) Then Return
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPersona(ByVal value As CPersonaFisica)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del collaboratore che ha in gestione il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property IDCollaboratore As Integer
            Get
                Return GetID(Me.m_Collaboratore, Me.m_IDCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDCollaboratore = value
                Me.m_Collaboratore = Nothing
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il collaboratore che ha in gestione il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue Is value) Then Return
                Me.m_Collaboratore = value
                Me.m_IDCollaboratore = GetID(value)
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCollaboratore(ByVal value As CCollaboratore)
            Me.m_Collaboratore = value
            Me.m_IDCollaboratore = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del ClienteXCollaboratore che ha in gestione il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property IDClienteXCollaboratore As Integer
            Get
                Return GetID(Me.m_ClienteXCollaboratore, Me.m_IDClienteXCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDClienteXCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDClienteXCollaboratore = value
                Me.m_ClienteXCollaboratore = Nothing
                Me.DoChanged("IDClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetClienteXCollaboratore(ByVal value As ClienteXCollaboratore)
            Me.m_ClienteXCollaboratore = value
            Me.m_IDClienteXCollaboratore = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il ClienteXCollaboratore che ha in gestione il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property ClienteXCollaboratore As ClienteXCollaboratore
            Get
                If (Me.m_ClienteXCollaboratore Is Nothing) Then Me.m_ClienteXCollaboratore = CQSPD.Collaboratori.ClientiXCollaboratori.GetItemById(Me.m_IDClienteXCollaboratore)
                Return Me.m_ClienteXCollaboratore
            End Get
            Set(value As ClienteXCollaboratore)
                Dim oldValue As ClienteXCollaboratore = Me.m_ClienteXCollaboratore
                If (oldValue Is value) Then Return
                Me.m_ClienteXCollaboratore = value
                Me.m_IDClienteXCollaboratore = GetID(value)
                Me.DoChanged("ClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data  
        ''' </summary>
        ''' <returns></returns>
        Public Property Data As DateTime?
            Get
                Return Me.m_Data
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_Data
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo del ricontatto fissato dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property Scopo As String
            Get
                Return Me.m_Scopo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Scopo
                If (oldValue = value) Then Return
                Me.m_Scopo = value
                Me.DoChanged("Scopo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei Esito aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Esito As Integer
            Get
                Return Me.m_Esito
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Esito
                If (oldValue = value) Then Return
                Me.m_Esito = value
                Me.DoChanged("Esito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property



        Public Property Indirizzo As String
            Get
                Return Me.m_Indirizzo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Indirizzo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Indirizzo = value
                Me.DoChanged("Indirizzo", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il motivo dell'assegnazione
        ''' </summary>
        ''' <returns></returns>
        Public Property Nota As String
            Get
                Return Me.m_Nota
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nota
                If (oldValue = value) Then Return
                Me.m_Nota = value
                Me.DoChanged("Nota", value, oldValue)
            End Set
        End Property




        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function



        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Collaboratori.GetConnection
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDNoteCliXCollab"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)
            Me.m_IDClienteXCollaboratore = reader.Read("IDCliXCollab", Me.m_IDClienteXCollaboratore)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Indirizzo = reader.Read("Indirizzo", Me.m_Indirizzo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Esito = reader.Read("Esito", Me.m_Esito)
            Me.m_Scopo = reader.Read("Scopo", Me.m_Scopo)
            Me.m_Nota = reader.Read("Nota", Me.m_Nota)
            Dim tmp As String = ""
            tmp = reader.Read("Parameters", tmp)
            If (tmp <> "") Then Me.m_Parameters = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("IDCollaboratore", Me.IDCollaboratore)
            writer.Write("IDCliXCollab", Me.IDClienteXCollaboratore)
            writer.Write("Data", Me.m_Data)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Indirizzo", Me.m_Indirizzo)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Esito", Me.m_Esito)
            writer.Write("Scopo", Me.m_Scopo)
            writer.Write("Nota", Me.m_Nota)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteAttribute("IDCliXCollab", Me.IDClienteXCollaboratore)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Indirizzo", Me.m_Indirizzo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Esito", Me.m_Esito)
            writer.WriteAttribute("Scopo", Me.m_Scopo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.m_Parameters)
            writer.WriteTag("Nota", Me.m_Nota)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCliXCollab" : Me.m_IDClienteXCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Esito" : Me.m_Esito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Scopo" : Me.m_Scopo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parameters" : Me.m_Parameters = CType(XML.Utils.Serializer.ToObject(fieldValue), CKeyCollection)
                Case "Nota" : Me.m_Nota = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(obj As NotaCollaboratore) As Integer
            Return DateUtils.Compare(Me.Data, obj.Data)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(obj, NotaCollaboratore))
        End Function
    End Class

End Class
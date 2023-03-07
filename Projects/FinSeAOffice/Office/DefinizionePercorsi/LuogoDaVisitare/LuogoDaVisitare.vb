Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Rappresenta la definizione di percorso
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class LuogoDaVisitare
        Inherits DBObject
        Implements IComparable

        Private m_Etichetta As String
        Private m_IDPercorso As Integer
        Private m_Percorso As PercorsoDefinito
        Private m_Indirizzo As CIndirizzo
        Private m_IDPersona As Integer
        Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_Progressivo As Integer
        Private m_Priorita As PriorityEnum
        
        Public Sub New()
            Me.m_Etichetta = ""
            Me.m_IDPercorso = 0
            Me.m_Percorso = Nothing
            Me.m_Indirizzo = New CIndirizzo
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_Progressivo = 0
            Me.m_Priorita = PriorityEnum.PRIORITY_NORMAL
        End Sub

     

        ''' <summary>
        ''' Restituisce o imposta un numero che indica l'ordine da seguire in un elenco di luoghi da visitare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' Restituisce o imposta la priorità da dare a questo luogo (in caso non si possano effettuare tutti)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priorita As PriorityEnum
            Get
                Return Me.m_Priorita
            End Get
            Set(value As PriorityEnum)
                Dim oldValue As PriorityEnum = Me.m_Priorita
                If (oldValue = value) Then Exit Property
                Me.m_Priorita = value
                Me.DoChanged("Priorita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una etichetta che identifica il luogo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Etichetta As String
            Get
                Return Me.m_Etichetta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Etichetta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Etichetta = value
                Me.DoChanged("Etichetta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del percorso a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPercorso As Integer
            Get
                Return GetID(Me.m_Percorso, Me.m_IDPercorso)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPercorso
                If (oldValue = value) Then Exit Property
                Me.m_IDPercorso = value
                Me.m_Percorso = Nothing
                Me.DoChanged("IDPercorso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Percorso As PercorsoDefinito
            Get
                If (Me.m_Percorso Is Nothing) Then Me.m_Percorso = Office.PercorsiDefiniti.GetItemById(Me.m_IDPercorso)
                Return Me.m_Percorso
            End Get
            Set(value As PercorsoDefinito)
                Dim oldValue As PercorsoDefinito = Me.m_Percorso
                If (oldValue Is value) Then Exit Property
                Me.m_Percorso = value
                Me.m_IDPercorso = GetID(value)
                Me.DoChanged("Percorso", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPercorso(ByVal value As PercorsoDefinito)
            Me.m_Percorso = value
            Me.m_IDPercorso = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce l'indirizzo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona o dell'azienda da visitare
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
        ''' Restituisce o imposta la persona o l'azienda da visitare
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
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona o dell'azienda da visitare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return PercorsiDefiniti.LuoghiDaVisitare.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDefPercorsiL"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Indirizzo.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Me.m_Indirizzo.SetChanged(False)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)
            Me.m_Etichetta = reader.Read("Etichetta", Me.m_Etichetta)
            Me.m_IDPercorso = reader.Read("IDPercorso", Me.m_IDPercorso)
            Me.m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            Me.m_Indirizzo.ToponimoEVia = reader.Read("Indirizzo_Via", Me.m_Indirizzo.ToponimoEVia)
            Me.m_Indirizzo.Civico = reader.Read("Indirizzo_Civico", Me.m_Indirizzo.Civico)
            Me.m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            Me.m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            Me.m_Indirizzo.Latitude = reader.Read("Lat", Me.m_Indirizzo.Latitude)
            Me.m_Indirizzo.Longitude = reader.Read("Lng", Me.m_Indirizzo.Longitude)
            Me.m_Indirizzo.Altitude = reader.Read("Alt", Me.m_Indirizzo.Altitude)
            Me.m_Indirizzo.SetChanged(False)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Progressivo", Me.m_Progressivo)
            writer.Write("Priorita", Me.m_Priorita)
            writer.Write("Etichetta", Me.m_Etichetta)
            writer.Write("IDPercorso", Me.IDPercorso)
            writer.Write("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            writer.Write("Indirizzo_Via", Me.m_Indirizzo.ToponimoEVia)
            writer.Write("Indirizzo_Civico", Me.m_Indirizzo.Civico)
            writer.Write("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            writer.Write("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Lat", Me.m_Indirizzo.Latitude)
            writer.Write("Lng", Me.m_Indirizzo.Longitude)
            writer.Write("Alt", Me.m_Indirizzo.Altitude)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            writer.WriteAttribute("Etichetta", Me.m_Etichetta)
            writer.WriteAttribute("IDPercorso", Me.IDPercorso)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Indirizzo", Me.m_Indirizzo)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Progressivo" : Me.m_Progressivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Etichetta" : Me.m_Etichetta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPercorso" : Me.IDPercorso = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = fieldValue
                Case "IDPersona" : Me.IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Etichetta
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overridable Function CompareTo(obj As LuogoDaVisitare) As Integer
            Dim ret As Integer = Me.m_Progressivo - obj.m_Progressivo
            If (ret = 0) Then ret = Strings.Compare(Me.m_Etichetta, obj.m_Etichetta, CompareMethod.Text)
            Return ret
        End Function

    End Class


End Class
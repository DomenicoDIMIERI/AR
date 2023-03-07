Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office


    ''' <summary>
    ''' Rappresenta un punto della mappa del territorio attraversato dall'utente per la commissione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class LuogoVisitato
        Inherits DBObject
        Implements IComparable

        Private m_IDOperatore As Integer                'ID dell'operatore
        <NonSerialized> _
        Private m_Operatore As CUser                    'Operatore
        Private m_IDUscita As Integer                   'ID dell'uscita
        <NonSerialized> _
        Private m_Uscita As Uscita                      'Uscita
        Private m_Indirizzo As CIndirizzo               'Indirizzo presso cui l'operatore
        Private m_OraArrivo As Nullable(Of Date)        'Data e ora di arrivo presso l'indirizzo specificato
        Private m_OraPartenza As Nullable(Of Date)          'Data e ora di partenza dall'indirizzo specificato
        Private m_Descrizione As String
        Private m_IDLuogo As Integer
        Private m_Luogo As LuogoDaVisitare
        Private m_IDPersona As Integer
        Private m_Persona As CPersona
        Private m_TipoMateriale As String
        Private m_ConsegnatiAMano As Integer
        Private m_ConsegnatiPostale As Integer
        Private m_ConsegnatiAuto As Integer
        Private m_ConsegnatiAltro As Integer
        Private m_Progressivo As Integer

        Public Sub New()
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_IDUscita = 0
            Me.m_Uscita = Nothing
            Me.m_Indirizzo = New CIndirizzo
            Me.m_OraArrivo = Nothing
            Me.m_OraPartenza = Nothing
            Me.m_Descrizione = vbNullString
            Me.m_IDLuogo = 0
            Me.m_Luogo = Nothing
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_TipoMateriale = ""
            Me.m_ConsegnatiAMano = 0
            Me.m_ConsegnatiPostale = 0
            Me.m_ConsegnatiAuto = 0
            Me.m_ConsegnatiAltro = 0
            Me.m_Progressivo = 0
        End Sub

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

        Protected Friend Sub SetProgressivo(ByVal value As Integer)
            Me.m_Progressivo = value
        End Sub

        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        Public Property IDUscita As Integer
            Get
                Return GetID(Me.m_Uscita, Me.m_IDUscita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUscita
                If (oldValue = value) Then Exit Property
                Me.m_IDUscita = value
                Me.m_Uscita = Nothing
                Me.DoChanged("IDUscita", value, oldValue)
            End Set
        End Property

        Public Property Uscita As Uscita
            Get
                If (Me.m_Uscita Is Nothing) Then Me.m_Uscita = Office.Uscite.GetItemById(Me.m_IDUscita)
                Return Me.m_Uscita
            End Get
            Set(value As Uscita)
                Dim oldValue As Uscita = Me.m_Uscita
                If (oldValue Is value) Then Exit Property
                Me.m_Uscita = value
                Me.m_IDUscita = GetID(value)
                Me.DoChanged("Uscita", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUscita(ByVal value As Office.Uscita)
            Me.m_Uscita = value
            Me.m_IDUscita = GetID(value)
        End Sub


        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Public Property OraArrivo As Nullable(Of Date)
            Get
                Return Me.m_OraArrivo
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraArrivo
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_OraArrivo = value
                Me.DoChanged("OraArrivo", value, oldValue)
            End Set
        End Property

        Public Property OraPartenza As Nullable(Of Date)
            Get
                Return Me.m_OraPartenza
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraPartenza
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_OraPartenza = value
                Me.DoChanged("OraPartenza", value, oldValue)
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

      

        Public ReadOnly Property Perorso As PercorsoDefinito
            Get
                If (Me.Luogo Is Nothing) Then Return Nothing
                Return Me.Luogo.Percorso
            End Get
        End Property


        Public Property IDLuogo As Integer
            Get
                Return GetID(Me.m_Luogo, Me.m_IDLuogo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDLuogo
                If (oldValue = value) Then Exit Property
                Me.m_IDLuogo = value
                Me.m_Luogo = Nothing
                Me.DoChanged("IDLuogo", value, oldValue)
            End Set
        End Property

        Public Property Luogo As LuogoDaVisitare
            Get
                If (Me.m_Luogo Is Nothing) Then Me.m_Luogo = Office.PercorsiDefiniti.LuoghiDaVisitare.GetItemById(Me.m_IDLuogo)
                Return Me.m_Luogo
            End Get
            Set(value As LuogoDaVisitare)
                Dim oldValue As LuogoDaVisitare = Me.m_Luogo
                If (oldValue Is value) Then Exit Property
                Me.m_Luogo = value
                Me.m_IDLuogo = GetID(value)
                Me.DoChanged("Luogo", value, oldValue)
            End Set
        End Property

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
                If (value IsNot Nothing) Then Me.m_Indirizzo.Nome = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Property TipoMateriale As String
            Get
                Return Me.m_TipoMateriale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoMateriale
                If (oldValue = value) Then Exit Property
                Me.m_TipoMateriale = value
                Me.DoChanged("TipoMateriale", value, oldValue)
            End Set
        End Property

        Public Property ConsegnatiAMano As Integer
            Get
                Return Me.m_ConsegnatiAMano
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConsegnatiAMano
                If (oldValue = value) Then Exit Property
                Me.m_ConsegnatiAMano = value
                Me.DoChanged("ConsegnatiAMano", value, oldValue)
            End Set
        End Property

        Public Property ConsegnatiPostale As Integer
            Get
                Return Me.m_ConsegnatiPostale
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConsegnatiPostale
                If (oldValue = value) Then Exit Property
                Me.m_ConsegnatiPostale = value
                Me.DoChanged("ConsegnatiPostale", value, oldValue)
            End Set
        End Property

        Public Property ConsegnatiAuto As Integer
            Get
                Return Me.m_ConsegnatiAuto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConsegnatiAuto
                If (oldValue = value) Then Exit Property
                Me.m_ConsegnatiAuto = value
                Me.DoChanged("ConsegnatiAuto", value, oldValue)
            End Set
        End Property

        Public Property ConsegnatiAltro As Integer
            Get
                Return Me.m_ConsegnatiAltro
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConsegnatiAltro
                If (oldValue = value) Then Exit Property
                Me.m_ConsegnatiAltro = value
                Me.DoChanged("ConsegnatiAltro", value, oldValue)
            End Set
        End Property


        Public ReadOnly Property ConsegnatiTotale As Integer
            Get
                Return Me.m_ConsegnatiAltro + Me.m_ConsegnatiAMano + Me.m_ConsegnatiAuto + Me.m_ConsegnatiPostale
            End Get
        End Property


        Public Overrides Function ToString() As String
            Return Me.m_Indirizzo.Nome
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.LuoghiVisitati.Module
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Indirizzo.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Me.m_Indirizzo.SetChanged(False)
            Return ret
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeLuoghiV"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_IDUscita = reader.Read("IDUscita", Me.m_IDUscita)
            
            Me.m_Indirizzo.Nome = reader.Read("Indirizzo_Etichetta", "")
            Me.m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", "")
            Me.m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", "")
            Me.m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", "")
            Me.m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", "")
            Me.m_Indirizzo.Latitude = reader.Read("Lat", Me.m_Indirizzo.Latitude)
            Me.m_Indirizzo.Longitude = reader.Read("Lng", Me.m_Indirizzo.Longitude)
            Me.m_Indirizzo.Altitude = reader.Read("Alt", Me.m_Indirizzo.Altitude)

            Me.m_Indirizzo.SetChanged(False)
            Me.m_OraArrivo = reader.Read("OraArrivo", Me.m_OraArrivo)
            Me.m_OraPartenza = reader.Read("OraPartenza", Me.m_OraPartenza)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDLuogo = reader.Read("IDLuogo", Me.m_IDLuogo)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_TipoMateriale = reader.Read("TipoMateriale", Me.m_TipoMateriale)
            Me.m_ConsegnatiAMano = reader.Read("ConsegnatiAMano", Me.m_ConsegnatiAMano)
            Me.m_ConsegnatiPostale = reader.Read("ConsegnatiPostale", Me.m_ConsegnatiPostale)
            Me.m_ConsegnatiAuto = reader.Read("ConsegnatiAuto", Me.m_ConsegnatiAuto)
            Me.m_ConsegnatiAltro = reader.Read("ConsegnatiAltro", Me.m_ConsegnatiAltro)
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("IDUscita", Me.IDUscita)
            writer.Write("Indirizzo_Etichetta", Me.m_Indirizzo.Nome)
            writer.Write("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            writer.Write("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            writer.Write("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            writer.Write("Lat", Me.m_Indirizzo.Latitude)
            writer.Write("Lng", Me.m_Indirizzo.Longitude)
            writer.Write("Alt", Me.m_Indirizzo.Altitude)
            writer.Write("OraArrivo", Me.m_OraArrivo)
            writer.Write("OraPartenza", Me.m_OraPartenza)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDLuogo", Me.IDLuogo)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("TipoMateriale", Me.m_TipoMateriale)
            writer.Write("ConsegnatiAMano", Me.m_ConsegnatiAMano)
            writer.Write("ConsegnatiPostale", Me.m_ConsegnatiPostale)
            writer.Write("ConsegnatiAuto", Me.m_ConsegnatiAuto)
            writer.Write("ConsegnatiAltro", Me.m_ConsegnatiAltro)
            writer.Write("Progressivo", Me.m_Progressivo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OraArrivo", Me.m_OraArrivo)
            writer.WriteAttribute("OraPartenza", Me.m_OraPartenza)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("IDLuogo", Me.IDLuogo)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("TipoMateriale", Me.m_TipoMateriale)
            writer.WriteAttribute("ConsegnatiAMano", Me.m_ConsegnatiAMano)
            writer.WriteAttribute("ConsegnatiPostale", Me.m_ConsegnatiPostale)
            writer.WriteAttribute("ConsegnatiAuto", Me.m_ConsegnatiAuto)
            writer.WriteAttribute("ConsegnatiAltro", Me.m_ConsegnatiAltro)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("IDUscita", Me.IDUscita)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Indirizzo", Me.m_Indirizzo)
            writer.WriteTag("Luogo", Me.Luogo)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUscita" : Me.m_IDUscita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = fieldValue
                Case "OraArrivo" : Me.m_OraArrivo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraPartenza" : Me.m_OraPartenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDLuogo" : Me.m_IDLuogo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoMateriale" : Me.m_TipoMateriale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ConsegnatiAMano" : Me.m_ConsegnatiAMano = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConsegnatiPostale" : Me.m_ConsegnatiPostale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConsegnatiAuto" : Me.m_ConsegnatiAuto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConsegnatiAltro" : Me.m_ConsegnatiAltro = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Progressivo" : Me.m_Progressivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Luogo"
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overridable Function CompareTo(obj As LuogoVisitato) As Integer
            Dim ret As Integer = Me.m_Progressivo - obj.m_Progressivo
            If (ret = 0) Then ret = GetID(Me) - GetID(obj)
            Return ret
        End Function

    End Class



End Class
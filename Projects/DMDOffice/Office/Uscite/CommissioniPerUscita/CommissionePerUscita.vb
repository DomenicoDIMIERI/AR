Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office


    ''' <summary>
    ''' Descrive lo svolgimento di una commissione durante un'uscita
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CommissionePerUscita
        Inherits DBObject

        <NonSerialized> _
        Private m_Operatore As CUser                    'Operatore che ha svolto la commissione
        Private m_IDOperatore As Integer                'ID dell'operatore che ha svolto la commissione
        Private m_NomeOperatore As String               'Nome dell'operatore che ha svolto la commissione
        Private m_OraInizio As Nullable(Of Date)        'Data ed ora di uscita (per svolgere la commissione)
        Private m_OraFine As Nullable(Of Date)       'Data ed ora di rientro
        Private m_DistanzaPercorsa As Nullable(Of Double)   'Distanza percorsa

        Private m_IDCommissione As Integer
        Private m_Commissione As Commissione

        Private m_IDUscita As Integer
        <NonSerialized> _
        Private m_Uscita As Uscita
        Private m_DescrizioneEsito As String
        Private m_Luoghi As CCollection(Of LuogoDaVisitare)
        Private m_StatoCommissione As StatoCommissione

        Public Sub New()
            Me.m_Operatore = Nothing
            Me.m_IDOperatore = 0
            Me.m_NomeOperatore = vbNullString
            Me.m_IDCommissione = 0
            Me.m_Commissione = Nothing
            Me.m_IDUscita = 0
            Me.m_Uscita = Nothing
            Me.m_DescrizioneEsito = vbNullString
            Me.m_OraInizio = Nothing
            Me.m_OraFine = Nothing
            Me.m_DistanzaPercorsa = Nothing
            Me.m_Luoghi = New CCollection(Of LuogoDaVisitare) ' = vbNullString
            Me.m_StatoCommissione = StatoCommissione.NonIniziata
        End Sub

        'Public Property Luogo As String
        '    Get
        '        Return Me.m_Luogo
        '    End Get
        '    Set(value As String)
        '        value = Strings.Trim(value)
        '        Dim oldValue As String = Me.m_Luogo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Luogo = value
        '        Me.DoChanged("Luogo", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property Luoghi As CCollection(Of LuogoDaVisitare)
            Get
                Return Me.m_Luoghi
            End Get
        End Property

        Public Property StatoCommissione As StatoCommissione
            Get
                Return Me.m_StatoCommissione
            End Get
            Set(value As StatoCommissione)
                Dim oldValue As StatoCommissione = Me.m_StatoCommissione
                If (oldValue = value) Then Exit Property
                Me.m_StatoCommissione = value
                Me.DoChanged("StatoCommissione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha effettuato la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha effettuate la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        Public Property IDCommissione As Integer
            Get
                Return GetID(Me.m_Commissione, Me.m_IDCommissione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCommissione
                If (oldValue = value) Then Exit Property
                Me.m_IDCommissione = value
                Me.m_Commissione = Nothing
                Me.DoChanged("IDCommissione", value, oldValue)
            End Set
        End Property

        Public Property Commissione As Commissione
            Get
                If (Me.m_Commissione Is Nothing) Then Me.m_Commissione = Office.Commissioni.GetItemById(Me.m_IDCommissione)
                Return Me.m_Commissione
            End Get
            Set(value As Commissione)
                Dim oldValue As Commissione = Me.m_Commissione
                If (oldValue Is value) Then Exit Property
                Me.m_Commissione = value
                Me.m_IDCommissione = GetID(value)
                Me.DoChanged("Commissione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCommissione(ByVal value As Office.Commissione)
            Me.m_Commissione = value
            Me.m_IDCommissione = GetID(value)
        End Sub


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

        Protected Friend Sub SetUscita(ByVal value As Uscita)
            Me.m_Uscita = value
            Me.m_IDUscita = GetID(value)
        End Sub

        Public Property DescrizioneEsito As String
            Get
                Return Me.m_DescrizioneEsito
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DescrizioneEsito
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneEsito = value
                Me.DoChanged("DescrizioneEsito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di uscita per la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OraInizio As Nullable(Of Date)
            Get
                Return Me.m_OraInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraInizio
                If (oldValue = value) Then Exit Property
                Me.m_OraInizio = value
                Me.DoChanged("OraInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata in secondi (differenza tra ora ingresso ed ora uscita)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Durata As Nullable(Of Integer)
            Get
                If (Me.m_OraInizio.HasValue AndAlso Me.m_OraFine.HasValue) Then
                    Return Math.Abs(DateUtils.DateDiff("s", Me.m_OraFine.Value, Me.m_OraInizio.Value))
                Else
                    Return Nothing
                End If
            End Get
        End Property
        ''' <summary>
        ''' Restituisce o imposta la data di rientro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OraFine As Nullable(Of Date)
            Get
                Return Me.m_OraFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraFine
                If (oldValue = value) Then Exit Property
                Me.m_OraFine = value
                Me.DoChanged("OraFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la distanza percorsa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DistanzaPercorsa As Nullable(Of Double)
            Get
                Return Me.m_DistanzaPercorsa
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_DistanzaPercorsa
                If (oldValue = value) Then Exit Property
                Me.m_DistanzaPercorsa = value
                Me.DoChanged("DistanzaPercorsa", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Nothing ' CommissioniPerUscite.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCommissXUscite"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_IDUscita = reader.Read("IDUscita", Me.m_IDUscita)
            Me.m_IDCommissione = reader.Read("IDCommissione", Me.m_IDCommissione)
            Me.m_DescrizioneEsito = reader.Read("DescrizioneEsito", Me.m_DescrizioneEsito)
            Me.m_OraInizio = reader.Read("OraInizio", Me.m_OraInizio)
            Me.m_OraFine = reader.Read("OraFine", Me.m_OraFine)
            Me.m_DistanzaPercorsa = reader.Read("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            'Me.m_Luogo = reader.Read("Luogo", Me.m_Luogo)
            Dim luogo As String = ""
            luogo = reader.Read("Luogo", luogo)
            Try
                Me.m_Luoghi.Clear()
                Me.m_Luoghi.AddRange(XML.Utils.Serializer.Deserialize(luogo))
            Catch ex As Exception
                Dim tmp As New LuogoDaVisitare
                tmp.Indirizzo.Parse(luogo)
                Me.m_Luoghi.Add(tmp)
            End Try
            Me.m_StatoCommissione = reader.Read("StatoCommissione", Me.m_StatoCommissione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("IDUscita", Me.IDUscita)
            writer.Write("IDCommissione", Me.IDCommissione)
            writer.Write("DescrizioneEsito", Me.m_DescrizioneEsito)
            writer.Write("OraInizio", Me.m_OraInizio)
            writer.Write("OraFine", Me.m_OraFine)
            writer.Write("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            writer.Write("StatoCommissione", Me.m_StatoCommissione)
            writer.Write("Luogo", XML.Utils.Serializer.Serialize(Me.Luoghi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("IDUscita", Me.IDUscita)
            writer.WriteAttribute("DescrizioneEsito", Me.m_DescrizioneEsito)
            writer.WriteAttribute("OraInizio", Me.m_OraInizio)
            writer.WriteAttribute("OraFine", Me.m_OraFine)
            writer.WriteAttribute("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            writer.WriteAttribute("StatoCommissione", Me.m_StatoCommissione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Luoghi", Me.Luoghi)
            If (Not writer.Settings.GetValueBool("uscitaserialization", False)) Then writer.WriteTag("Uscita", Me.Uscita)
            writer.WriteTag("IDCommissione", Me.IDCommissione)
            If (Not writer.Settings.GetValueBool("commissioneserialization", False)) Then writer.WriteTag("Commissione", Me.Commissione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDUscita" : Me.m_IDUscita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Uscita" : Me.m_Uscita = fieldValue
                Case "IDCommissione" : Me.m_IDCommissione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Commissione" : Me.m_Commissione = fieldValue
                Case "DescrizioneEsito" : Me.m_DescrizioneEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OraInizio" : Me.m_OraInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraFine" : Me.m_OraFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DistanzaPercorsa" : Me.m_DistanzaPercorsa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Luoghi" : Me.m_Luoghi.Clear() : Me.m_Luoghi.AddRange(fieldValue)
                Case "StatoCommissione" : Me.m_StatoCommissione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


    End Class



End Class
Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Partial Class Office


    Public Enum TipoMarcaturaIO As Integer
        INGRESSO = 0
        USCITA = 1
    End Enum

    <Flags>
    Public Enum MetodoRiconoscimento As Integer
        Sconosciuto = 0
        Password = 1
        ImprontaDigitale = 2
        ImprontaRetina = 4
        RiconoscimentoFacciale = 16
        RiconoscimentoVocale = 32
    End Enum

    '000001	2014-07-09 15:04:53	3	I

    <Serializable>
    Public Class MarcaturaIngressoUscita
        Inherits DBObjectPO
        Implements IComparable

        Private m_IDOperatore As Integer
        <NonSerialized>
        Private m_Operatore As CUser
        Private m_NomeOperatore As String
        Private m_IDDispositivo As Integer
        <NonSerialized>
        Private m_Dispositivo As RilevatorePresenze
        Private m_Data As Date
        Private m_Operazione As TipoMarcaturaIO

        Private m_IDReparto As Integer
        Private m_NomeReparto As String

        Private m_MetodiRiconoscimentoUsati As MetodoRiconoscimento

        Private m_Parametri As CKeyCollection


        Public Sub New()
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
            Me.m_IDDispositivo = 0
            Me.m_Dispositivo = Nothing
            Me.m_Data = Now
            Me.m_Operazione = TipoMarcaturaIO.INGRESSO
            Me.m_IDReparto = 0
            Me.m_NomeReparto = ""
            Me.m_MetodiRiconoscimentoUsati = MetodoRiconoscimento.Sconosciuto
            Me.m_Parametri = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettualo la marcatura
        ''' </summary>
        ''' <returns></returns>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Return
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
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Return
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                Me.m_NomeOperatore = ""
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatore
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID del dispositivo che ha registrato la marcatura
        ''' </summary>
        ''' <returns></returns>
        Public Property IDDispositivo As Integer
            Get
                Return GetID(Me.m_Dispositivo, Me.m_IDDispositivo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDispositivo
                If (oldValue = value) Then Return
                Me.m_IDDispositivo = value
                Me.m_Dispositivo = Nothing
                Me.DoChanged("IDDispositivo", value, oldValue)
            End Set
        End Property


        Public Property Dispositivo As RilevatorePresenze
            Get
                If (Me.m_Dispositivo Is Nothing) Then Me.m_Dispositivo = Office.RilevatoriPresenze.GetItemById(Me.m_IDDispositivo)
                Return Me.m_Dispositivo
            End Get
            Set(value As RilevatorePresenze)
                Dim oldValue As RilevatorePresenze = Me.Dispositivo
                If (oldValue Is value) Then Return
                Me.m_Dispositivo = value
                Me.m_IDDispositivo = GetID(value)
                Me.DoChanged("Dispositivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della marcatura
        ''' </summary>
        ''' <returns></returns>
        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        Public Property Operazione As TipoMarcaturaIO
            Get
                Return Me.m_Operazione
            End Get
            Set(value As TipoMarcaturaIO)
                Dim oldValue As TipoMarcaturaIO = Me.m_Operazione
                If (oldValue = value) Then Return
                Me.m_Operazione = value
                Me.DoChanged("Operazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del reparto 
        ''' </summary>
        ''' <returns></returns>
        Public Property IDReparto As Integer
            Get
                Return Me.m_IDReparto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDReparto
                If (oldValue = value) Then Return
                Me.m_IDReparto = value
                Me.DoChanged("IDReparto", value, oldValue)
            End Set
        End Property

        Public Property NomeReparto As String
            Get
                Return Me.m_NomeReparto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeReparto
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeReparto = value
                Me.DoChanged("NomeReparto", value, oldValue)
            End Set
        End Property

        Public Property MetodiRiconoscimentoUsati As MetodoRiconoscimento
            Get
                Return Me.m_MetodiRiconoscimentoUsati
            End Get
            Set(value As MetodoRiconoscimento)
                Dim oldValue As MetodoRiconoscimento = Me.m_MetodiRiconoscimentoUsati
                If (oldValue = value) Then Return
                Me.m_MetodiRiconoscimentoUsati = value
                Me.DoChanged("MetodiRiconoscimentoUsati", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parametri As CKeyCollection
            Get
                If (Me.m_Parametri Is Nothing) Then Me.m_Parametri = New CKeyCollection
                Return Me.m_Parametri
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Marcature.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeUserIO"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_IDDispositivo = reader.Read("IDDispositivo", Me.m_IDDispositivo)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_Operazione = reader.Read("Operazione", Me.m_Operazione)
            Me.m_IDReparto = reader.Read("IDReparto", Me.m_IDReparto)
            Me.m_NomeReparto = reader.Read("NomeReparto", Me.m_NomeReparto)
            Me.m_MetodiRiconoscimentoUsati = reader.Read("MetodiRiconoscimentoUsati", Me.m_MetodiRiconoscimentoUsati)
            Try
                Me.m_Parametri = XML.Utils.Serializer.Deserialize(reader.Read("Parametri", ""))
            Catch ex As Exception
                Me.m_Parametri = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("IDDispositivo", Me.IDDispositivo)
            writer.Write("Data", Me.m_Data)
            writer.Write("Operazione", Me.m_Operazione)
            writer.Write("IDReparto", Me.IDReparto)
            writer.Write("NomeReparto", Me.m_NomeReparto)
            writer.Write("MetodiRiconoscimentoUsati", Me.m_MetodiRiconoscimentoUsati)
            writer.Write("Parametri", XML.Utils.Serializer.Serialize(Me.Parametri))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("IDDispositivo", Me.IDDispositivo)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("Operazione", Me.m_Operazione)
            writer.WriteAttribute("IDReparto", Me.IDReparto)
            writer.WriteAttribute("NomeReparto", Me.m_NomeReparto)
            writer.WriteAttribute("MetodiRiconoscimentoUsati", Me.m_MetodiRiconoscimentoUsati)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parametri", Me.Parametri)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDispositivo" : Me.m_IDDispositivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Operazione" : Me.m_Operazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDReparto" : Me.m_IDReparto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeReparto" : Me.m_NomeReparto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MetodiRiconoscimentoUsati" : Me.m_MetodiRiconoscimentoUsati = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Parametri" : Me.m_Parametri = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As MarcaturaIngressoUscita) As Integer
            Dim ret As Integer = Calendar.Compare(Me.Data, obj.Data)
            If (ret = 0) Then
                Dim o1 As Integer = IIf(Me.Operazione = TipoMarcaturaIO.INGRESSO, 0, 1)
                Dim o2 As Integer = IIf(obj.Operazione = TipoMarcaturaIO.INGRESSO, 0, 1)
                ret = o1.CompareTo(o2)
            End If
            If (ret = 0) Then ret = Strings.Compare(Me.NomePuntoOperativo, obj.NomePuntoOperativo, CompareMethod.Text)
            If (ret = 0) Then ret = Me.IDOperatore.CompareTo(obj.IDOperatore)
            If (ret = 0) Then ret = Me.ID.CompareTo(obj.ID)
            Return ret
        End Function


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class


End Class
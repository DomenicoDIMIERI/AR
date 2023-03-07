Imports DMD
Imports DMD.CustomerCalls
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Public Enum StatoChiamata As Integer
    None = 0
    Dialling = 1            'Composizione della chiamata in uscita
    Ringing = 2             'Il telefono sta squillando 
    Speaking = 3            'La cornetta é stata alzata e la conversazione é in corso
    Paused = 4              'La chiamata é stata messa in pausa
    Answered = 5            'La cornetta é stata alzata e la conversazione é terminata
    NotAnswered = 6         'La cornetta non é stata alzata e la chiamata é terminata
End Enum

Public Class Chiamata
    Inherits DBObjectBase

    Private m_ServerIP As String
    Private m_ServerName As String
    Private m_Channel As String
    Private m_SourceNumber As String
    Private m_TargetNumber As String
    Private m_StartTime As DateTime?
    Private m_AnswerTime As DateTime?
    Private m_EndTime As DateTime?
    Private m_StatoChiamata As StatoChiamata
    Private m_Direzione As Integer
    Private m_Flags As Integer
    Private m_Parameters As CKeyCollection
    Private m_IDTelefonata As Integer
    Private m_Telefonata As CTelefonata

    Public Sub New()
        Me.m_ServerIP = ""
        Me.m_ServerName = ""
        Me.m_Channel = ""
        Me.m_SourceNumber = ""
        Me.m_TargetNumber = ""
        Me.m_StartTime = Nothing
        Me.m_AnswerTime = Nothing
        Me.m_EndTime = Nothing
        Me.m_StatoChiamata = StatoChiamata.None
        Me.m_Direzione = 0
        Me.m_Flags = 0
        Me.m_Parameters = Nothing
        Me.m_IDTelefonata = 0
        Me.m_Telefonata = Nothing
    End Sub

    Public Property IDTelefonata As Integer
        Get
            Return GetID(Me.m_Telefonata, Me.m_IDTelefonata)
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = Me.IDTelefonata
            If (oldValue = value) Then Return
            Me.m_IDTelefonata = value
            Me.m_Telefonata = Nothing
            Me.DoChanged("IDTelefonata", value, oldValue)
        End Set
    End Property

    Public Property Telefonata As CTelefonata
        Get
            If (Me.m_Telefonata Is Nothing) Then Me.m_Telefonata = CustomerCalls.Telefonate.GetItemById(Me.m_IDTelefonata)
            Return Me.m_Telefonata
        End Get
        Set(value As CTelefonata)
            Dim oldValue As CTelefonata = Me.m_Telefonata
            If (oldValue Is value) Then Return
            Me.m_Telefonata = value
            Me.m_IDTelefonata = GetID(value)
            Me.DoChanged("Telefonata", value, oldValue)
        End Set
    End Property

    Public Property ServerIP As String
        Get
            Return Me.m_ServerIP
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_ServerIP
            If (oldValue = value) Then Return
            Me.m_ServerIP = value
            Me.DoChanged("ServerIP", value, oldValue)
        End Set
    End Property

    Public Property ServerName As String
        Get
            Return Me.m_ServerName
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_ServerName
            If (oldValue = value) Then Return
            Me.m_ServerName = value
            Me.DoChanged("ServerName", value, oldValue)
        End Set
    End Property

    Public Property Channel As String
        Get
            Return Me.m_Channel
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_Channel
            If (value = oldValue) Then Return
            Me.m_Channel = value
            Me.DoChanged("Channel", value, oldValue)
        End Set
    End Property

    Public Property SourceNumber As String
        Get
            Return Me.m_SourceNumber
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_SourceNumber
            If (oldValue = value) Then Return
            Me.m_SourceNumber = value
            Me.DoChanged("SourceNumber", value, oldValue)
        End Set
    End Property

    Public Property TargetNumber As String
        Get
            Return Me.m_TargetNumber
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_TargetNumber
            If (oldValue = value) Then Return
            Me.m_TargetNumber = value
            Me.DoChanged("TargetNumber", value, oldValue)
        End Set
    End Property

    Public Property StartTime As DateTime?
        Get
            Return Me.m_StartTime
        End Get
        Set(value As DateTime?)
            Dim oldValue As DateTime? = Me.m_StartTime
            If (DateUtils.Compare(value, oldValue) = 0) Then Return
            Me.m_StartTime = value
            Me.DoChanged("StartTime", value, oldValue)
        End Set
    End Property

    Public Property AnswerTime As DateTime?
        Get
            Return Me.m_AnswerTime
        End Get
        Set(value As DateTime?)
            Dim oldValue As DateTime? = Me.m_AnswerTime
            If (DateUtils.Compare(value, oldValue) = 0) Then Return
            Me.m_AnswerTime = value
            Me.DoChanged("AnswerTime", value, oldValue)
        End Set
    End Property

    Public Property EndTime As DateTime?
        Get
            Return Me.m_EndTime
        End Get
        Set(value As DateTime?)
            Dim oldValue As DateTime? = Me.m_EndTime
            If (DateUtils.Compare(value, oldValue) = 0) Then Return
            Me.m_EndTime = value
            Me.DoChanged("EndTime", value, oldValue)
        End Set
    End Property

    Public Property StatoChiamata As StatoChiamata
        Get
            Return Me.m_StatoChiamata
        End Get
        Set(value As StatoChiamata)
            Dim oldValue As StatoChiamata = Me.m_StatoChiamata
            If (oldValue = value) Then Return
            Me.m_StatoChiamata = value
            Me.DoChanged("StatoChiamata", value, oldValue)
        End Set
    End Property

    Public Property Direzione As Integer
        Get
            Return Me.m_Direzione
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = Me.m_Direzione
            If (oldValue = value) Then Return
            Me.m_Direzione = value
            Me.DoChanged("Direzione", value, oldValue)
        End Set
    End Property

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

    Public ReadOnly Property Parameters As CKeyCollection
        Get
            If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
            Return Me.m_Parameters
        End Get
    End Property

    Public Overrides Function GetModule() As Sistema.CModule
        Return Nothing
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_Telefonate"
    End Function

    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Me.m_ServerIP = reader.Read("ServerIP", Me.m_ServerIP)
        Me.m_ServerName = reader.Read("ServerName", Me.m_ServerName)
        Me.m_Channel = reader.Read("Channel", Me.m_Channel)
        Me.m_SourceNumber = reader.Read("SourceNumber", Me.m_SourceNumber)
        Me.m_TargetNumber = reader.Read("TargetNumber", Me.m_TargetNumber)
        Me.m_StartTime = reader.Read("StartTime", Me.m_StartTime)
        Me.m_AnswerTime = reader.Read("AnswerTime", Me.m_AnswerTime)
        Me.m_EndTime = reader.Read("EndTime", Me.m_EndTime)
        Me.m_StatoChiamata = reader.Read("StatoChiamata", Me.m_StatoChiamata)
        Me.m_Direzione = reader.Read("Direzione", Me.m_Direzione)
        Me.m_Flags = reader.Read("Flags", Me.m_Flags)
        Me.m_IDTelefonata = reader.Read("IDTelefonata", Me.m_IDTelefonata)
        Dim tmp As String = ""
        tmp = reader.Read("Parameters", tmp)
        If (tmp <> "") Then Me.m_Parameters = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)

        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        writer.Write("ServerIP", Me.m_ServerIP)
        writer.Write("ServerName", Me.m_ServerName)
        writer.Write("Channel", Me.m_Channel)
        writer.Write("SourceNumber", Me.m_SourceNumber)
        writer.Write("TargetNumber", Me.m_TargetNumber)
        writer.Write("StartTime", Me.m_StartTime)
        writer.Write("AnswerTime", Me.m_AnswerTime)
        writer.Write("EndTime", Me.m_EndTime)
        writer.Write("StatoChiamata", Me.m_StatoChiamata)
        writer.Write("Direzione", Me.m_Direzione)
        writer.Write("Flags", Me.m_Flags)
        writer.Write("IDTelefonata", Me.IDTelefonata)
        writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
        Return MyBase.SaveToRecordset(writer)
    End Function

    Protected Overrides Sub XMLSerialize(writer As XMLWriter)
        writer.WriteAttribute("ServerIP", Me.m_ServerIP)
        writer.WriteAttribute("ServerName", Me.m_ServerName)
        writer.WriteAttribute("Channel", Me.m_Channel)
        writer.WriteAttribute("SourceNumber", Me.m_SourceNumber)
        writer.WriteAttribute("TargetNumber", Me.m_TargetNumber)
        writer.WriteAttribute("StartTime", Me.m_StartTime)
        writer.WriteAttribute("AnswerTime", Me.m_AnswerTime)
        writer.WriteAttribute("EndTime", Me.m_EndTime)
        writer.WriteAttribute("StatoChiamata", Me.m_StatoChiamata)
        writer.WriteAttribute("Direzione", Me.m_Direzione)
        writer.WriteAttribute("Flags", Me.m_Flags)
        writer.WriteAttribute("IDTelefonata", Me.IDTelefonata)
        MyBase.XMLSerialize(writer)
        writer.WriteTag("Parameters", Me.Parameters)
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "ServerIP" : Me.m_ServerIP = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "ServerName" : Me.m_ServerName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Channel" : Me.m_Channel = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "SourceNumber" : Me.m_SourceNumber = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "TargetNumber" : Me.m_TargetNumber = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "StartTime" : Me.m_StartTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "AnswerTime" : Me.m_AnswerTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "EndTime" : Me.m_EndTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "StatoChiamata" : Me.m_StatoChiamata = CType(XML.Utils.Serializer.DeserializeInteger(fieldValue), StatoChiamata)
            Case "Direzione" : Me.m_Direzione = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "Flags" : Me.m_Flags = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "IDTelefonata" : Me.m_IDTelefonata = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "Parameters" : Me.m_Parameters = CType(fieldValue, CKeyCollection)
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select

    End Sub

    Protected Overrides Function GetConnection() As CDBConnection
        Return Chiamate.Database
    End Function
End Class

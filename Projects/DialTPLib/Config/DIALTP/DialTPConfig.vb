Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

<Flags>
Public Enum DialtpConfigFlags As Integer
    None = 0

    ''' <summary>
    ''' Se vero indica al programma di mostrare una finestra per le telefonate in ingresso
    ''' </summary>
    ShowInCall = 4
End Enum

<Serializable>
Public NotInheritable Class DialTPConfig
    Inherits DBObjectBase

    Private m_IDPostazione As String        'Stringa che identifica la postazione a cui fa riferimento la configurazione
    Private m_IDMacchina As String          'Stringa che identifica la macchina a cui fa riferimento la configurazione
    Private m_IDUtente As String            'Strings che identifica l'utente collegato alla macchina
    Private m_Attiva As Boolean             'Se vero indica che la configurazione è attiva
    Private m_DataInizio As Date?           'Data di inizio validità della configurazione
    Private m_DataFine As Date?             'Data di fine validità della configurazione
    Private m_AsteriskServers As CCollection(Of AsteriskServer)    'Collezione dei server asterisk
    Private m_Dispositivi As CCollection(Of DispositivoEsterno)     'Collezione dei dispositivi configurati
    Private m_Linee As CCollection(Of LineaEsterna)                 'Configurazione delle linee disponibili per il centralino
    Private m_CartelleMonitorate As CCollection(Of String)  'Elenco delle cartelle monitorate
    Private m_CartelleEscluse As CCollection(Of String)  'Elenco delle cartelle monitorate
    Private m_ServerName As String          'Server da cui vengono scaricate le informazioni
    Private m_UserName As String            'Utente che viene utilizzato per identificarsi sul server delle informazioni
    Private m_UploadServer As String        'Server a cui vengono inviati i files caricati nelle cartelle monitorate
    Private m_NotifyServer As String        'Server a cui vengono inviati gli screenshot
    Private m_Flags As Integer
    Private m_Attributi As CKeyCollection
    Private m_HylafaxConfig As HylafaxServerConfig
    Private m_RemoveLogAfterNDays As Integer
    Private m_MinVersion As String          'Versione minima del programma per cui è applicabile la configurazione
    Private m_MaxVersion As String          'Versione massima del programma per cui è applicabile la configurazione
    Private m_ProxyConfig As ProxyConfig


    Public Sub New()
        Me.m_IDPostazione = ""
        Me.m_IDMacchina = ""
        Me.m_IDUtente = ""
        Me.m_Attiva = True
        Me.m_DataInizio = Nothing
        Me.m_DataFine = Nothing
        Me.m_AsteriskServers = Nothing
        Me.m_Dispositivi = Nothing
        Me.m_Linee = Nothing
        Me.m_CartelleMonitorate = Nothing
        Me.m_CartelleEscluse = Nothing
        Me.m_Flags = DialtpConfigFlags.None
        Me.m_Attributi = Nothing
        Me.m_UploadServer = ""
        Me.m_NotifyServer = ""
        Me.m_ServerName = ""
        Me.m_UserName = ""
        Me.m_HylafaxConfig = Nothing
        Me.m_RemoveLogAfterNDays = 7
        Me.m_MinVersion = ""
        Me.m_MaxVersion = ""
        Me.m_ProxyConfig = Nothing
    End Sub

    ''' <summary>
    ''' Se vero indica al programma di mostrare una finestra per le telefonate in ingresso
    ''' </summary>
    ''' <returns></returns>
    Public Property ShowInCall As Boolean
        Get
            Return TestFlag(Me.m_Flags, DialtpConfigFlags.ShowInCall)
        End Get
        Set(value As Boolean)
            If Me.ShowInCall = value Then Return
            Me.m_Flags = SetFlag(Me.m_Flags, DialtpConfigFlags.ShowInCall, value)
            Me.DoChanged("ShowInCall", value, Not value)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta la versione minima del programma per cui è applicabile la configurazione
    ''' </summary>
    ''' <returns></returns>
    Public Property MinVersion As String
        Get
            Return Me.m_MinVersion
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_MinVersion
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_MinVersion = value
            Me.DoChanged("MinVersion", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta la versione massima del programma per cui è applicabile la configurazione
    ''' </summary>
    ''' <returns></returns>
    Public Property MaxVersion As String
        Get
            Return Me.m_MaxVersion
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_MaxVersion
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_MaxVersion = value
            Me.DoChanged("MaxVersion", value, oldValue)
        End Set
    End Property


    ''' <summary>
    ''' Restituisce o imposta il numero di giorni in cui conservare i log.
    ''' Un valore pari a 0 indica nessun log.
    ''' Un valore negativo indica che i log non verranno mai eliminati
    ''' </summary>
    ''' <returns></returns>
    Public Property RemoveLogAfterNDays As Integer
        Get
            Return Me.m_RemoveLogAfterNDays
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = Me.m_RemoveLogAfterNDays
            If (oldValue = value) Then Return
            Me.m_RemoveLogAfterNDays = value
            Me.DoChanged("RemoveLogAfterNDays", value, oldValue)
        End Set
    End Property


    ''' <summary>
    ''' Server da cui vengono scaricate le informazioni
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ServerName As String
        Get
            Return Me.m_ServerName
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_ServerName
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_ServerName = value
            Me.DoChanged("ServerName", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta l'utente che viene utilizzato per identificarsi sul server delle informazioni
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserName As String
        Get
            Return Me.m_UserName
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_UserName
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_UserName = value
            Me.DoChanged("UserName", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta il server a cui vengono inviati i files delle cartelle monitorate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UploadServer As String
        Get
            Return Me.m_UploadServer
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_UploadServer
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_UploadServer = value
            Me.DoChanged("UploadServer", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta il server delle notifiche
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotifyServer As String
        Get
            Return Me.m_NotifyServer
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_NotifyServer
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_NotifyServer = value
            Me.DoChanged("NotifyServer", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta l'ID della postazione associata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IDPostazione As String
        Get
            Return Me.m_IDPostazione
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_IDPostazione
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_IDPostazione = value
            Me.DoChanged("IDPostazione", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta l'ID della macchina associata
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IDMacchina As String
        Get
            Return Me.m_IDMacchina
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_IDMacchina
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_IDMacchina = value
            Me.DoChanged("IDMacchina", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta l'ID dell'utente associato
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IDUtente As String
        Get
            Return Me.m_IDUtente
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_IDUtente
            value = Strings.Trim(value)
            If (oldValue = value) Then Exit Property
            Me.m_IDUtente = value
            Me.DoChanged("IDUtente", value, oldValue)
        End Set
    End Property

    ''' <summary>
    ''' Restituisce o imposta un valore booleano che indica se la configurazione è attiva
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Attiva As Boolean
        Get
            Return Me.m_Attiva
        End Get
        Set(value As Boolean)
            If (Me.m_Attiva = value) Then Exit Property
            Me.m_Attiva = value
            Me.DoChanged("Attiva", value, Not value)
        End Set
    End Property

    Public Property DataInizio As Date?
        Get
            Return Me.m_DataInizio
        End Get
        Set(value As Date?)
            Dim oldValue As Date? = Me.m_DataInizio
            If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
            Me.m_DataInizio = value
            Me.DoChanged("DataInizio", value, oldValue)
        End Set
    End Property

    Public Property DataFine As Date?
        Get
            Return Me.m_DataFine
        End Get
        Set(value As Date?)
            Dim oldValue As Date? = Me.m_DataFine
            If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
            Me.m_DataFine = value
            Me.DoChanged("DataFine", value, oldValue)
        End Set
    End Property

    Public ReadOnly Property AsteriskServers As CCollection(Of AsteriskServer)
        Get
            If (Me.m_AsteriskServers Is Nothing) Then Me.m_AsteriskServers = New CCollection(Of AsteriskServer)
            Return Me.m_AsteriskServers
        End Get
    End Property

    Public ReadOnly Property Dispositivi As CCollection(Of DispositivoEsterno)
        Get
            If (Me.m_Dispositivi Is Nothing) Then Me.m_Dispositivi = New CCollection(Of DispositivoEsterno)
            Return Me.m_Dispositivi
        End Get
    End Property

    Public ReadOnly Property Linee As CCollection(Of LineaEsterna)
        Get
            If (Me.m_Linee Is Nothing) Then Me.m_Linee = New CCollection(Of LineaEsterna)
            Return Me.m_Linee
        End Get
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

    Public ReadOnly Property Attributi As CKeyCollection
        Get
            If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
            Return Me.m_Attributi
        End Get
    End Property

    Public ReadOnly Property HylafaxConfig As HylafaxServerConfig
        Get
            If (Me.m_HylafaxConfig Is Nothing) Then Me.m_HylafaxConfig = New HylafaxServerConfig
            Return Me.m_HylafaxConfig
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Me.m_IDMacchina & "." & Me.m_IDPostazione & "." & Me.m_IDUtente
    End Function

    ''' <summary>
    ''' Restituisce la collezione delle cartelle che vengono monitorate per la creazione, modifica ed eliminazione di files
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CartelleMonitorate As CCollection(Of String)
        Get
            If (Me.m_CartelleMonitorate Is Nothing) Then Me.m_CartelleMonitorate = New CCollection(Of String)
            Return Me.m_CartelleMonitorate
        End Get
    End Property

    ''' <summary>
    ''' Restituisce la collezione delle cartelle che vengono escluse dal monitoraggio dei files (le cartelle escluse hanno precedenza)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CartelleEscluse As CCollection(Of String)
        Get
            If (Me.m_CartelleEscluse Is Nothing) Then Me.m_CartelleEscluse = New CCollection(Of String)
            Return Me.m_CartelleEscluse
        End Get
    End Property

    Public ReadOnly Property ProxyConfig As ProxyConfig
        Get
            If Me.m_ProxyConfig Is Nothing Then Me.m_ProxyConfig = New ProxyConfig
            Return Me.m_ProxyConfig
        End Get
    End Property

    Protected Overrides Function GetConnection() As CDBConnection
        Return DialTPApp.Database
    End Function

    Public Overrides Function GetModule() As DMD.Sistema.CModule
        Return DialTPApp.Configs.Module
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_DialTPConfig"
    End Function

    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Me.m_IDPostazione = reader.Read("IDPostazione", Me.m_IDPostazione)
        Me.m_IDMacchina = reader.Read("IDMacchina", Me.m_IDMacchina)
        Me.m_IDUtente = reader.Read("IDUtente", Me.m_IDUtente)
        Me.m_Attiva = reader.Read("Attiva", Me.m_Attiva)
        Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
        Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
        Me.m_Flags = reader.Read("Flags", Me.m_Flags)
        Me.m_UploadServer = reader.Read("UploadServer", Me.m_UploadServer)
        Me.m_NotifyServer = reader.Read("NotifyServer", Me.m_NotifyServer)
        Me.m_ServerName = reader.Read("ServerName", Me.m_ServerName)
        Me.m_UserName = reader.Read("UserName", Me.m_UserName)
        Me.m_RemoveLogAfterNDays = reader.Read("RemoveLogAfterNDays", Me.m_RemoveLogAfterNDays)
        Me.m_MinVersion = reader.Read("MinVersion", Me.m_MinVersion)
        Me.m_MaxVersion = reader.Read("MaxVersion", Me.m_MaxVersion)
        Try
            Me.m_AsteriskServers = New CCollection(Of AsteriskServer)
            Me.m_AsteriskServers.AddRange(CType(XML.Utils.Serializer.Deserialize(reader.Read("AsteriskServers", "")), CCollection))
        Catch ex As Exception
            Me.m_AsteriskServers = Nothing
        End Try

        Try
            Me.m_Dispositivi = New CCollection(Of DispositivoEsterno)
            Me.m_Dispositivi.AddRange(CType(XML.Utils.Serializer.Deserialize(reader.Read("Dispositivi", "")), CCollection))
        Catch ex As Exception
            Me.m_Dispositivi = Nothing
        End Try

        Try
            Me.m_Linee = New CCollection(Of LineaEsterna)
            Me.m_Linee.AddRange(CType(XML.Utils.Serializer.Deserialize(reader.Read("Linee", "")), CCollection))
        Catch ex As Exception
            Me.m_Linee = Nothing
        End Try

        Try
            Me.m_CartelleMonitorate = New CCollection(Of String)
            Me.m_CartelleMonitorate.AddRange(CType(XML.Utils.Serializer.Deserialize(reader.Read("CartelleMonitorate", "")), CCollection))
        Catch ex As Exception
            Me.m_CartelleMonitorate = Nothing
        End Try

        Try
            Me.m_CartelleEscluse = New CCollection(Of String)
            Me.m_CartelleEscluse.AddRange(CType(XML.Utils.Serializer.Deserialize(reader.Read("CartelleEscluse", "")), CCollection))
        Catch ex As Exception
            Me.m_CartelleEscluse = Nothing
        End Try

        Try
            Me.m_Attributi = CType(XML.Utils.Serializer.Deserialize(reader.Read("Attributi", "")), CKeyCollection)
        Catch ex As Exception
            Me.m_Attributi = Nothing
        End Try

        Try
            Me.m_HylafaxConfig = CType(XML.Utils.Serializer.Deserialize(reader.Read("HylafaxConfig", "")), HylafaxServerConfig)
        Catch ex As Exception
            Me.m_HylafaxConfig = Nothing
        End Try

        Try
            Me.m_ProxyConfig = CType(XML.Utils.Serializer.Deserialize(reader.Read("ProxyConfig", "")), ProxyConfig)
        Catch ex As Exception
            Me.m_ProxyConfig = Nothing
        End Try

        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        writer.Write("IDPostazione", Me.m_IDPostazione)
        writer.Write("IDMacchina", Me.m_IDMacchina)
        writer.Write("IDUtente", Me.m_IDUtente)
        writer.Write("Attiva", Me.m_Attiva)
        writer.Write("DataInizio", Me.m_DataInizio)
        writer.Write("DataFine", Me.m_DataFine)
        writer.Write("Flags", Me.m_Flags)
        writer.Write("AsteriskServers", XML.Utils.Serializer.Serialize(Me.AsteriskServers))
        writer.Write("Dispositivi", XML.Utils.Serializer.Serialize(Me.Dispositivi))
        writer.Write("Linee", XML.Utils.Serializer.Serialize(Me.Linee))
        writer.Write("CartelleMonitorate", XML.Utils.Serializer.Serialize(Me.CartelleMonitorate))
        writer.Write("CartelleEscluse", XML.Utils.Serializer.Serialize(Me.CartelleEscluse))
        writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
        writer.Write("HylafaxConfig", XML.Utils.Serializer.Serialize(Me.HylafaxConfig))
        writer.Write("UploadServer", Me.m_UploadServer)
        writer.Write("NotifyServer", Me.m_NotifyServer)
        writer.Write("ServerName", Me.m_ServerName)
        writer.Write("UserName", Me.m_UserName)
        writer.Write("RemoveLogAfterNDays", Me.m_RemoveLogAfterNDays)
        writer.Write("MinVersion", Me.m_MinVersion)
        writer.Write("MaxVersion", Me.m_MaxVersion)
        writer.Write("ProxyConfig", XML.Utils.Serializer.Serialize(Me.ProxyConfig))
        Return MyBase.SaveToRecordset(writer)
    End Function

    Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        writer.WriteAttribute("IDPostazione", Me.m_IDPostazione)
        writer.WriteAttribute("IDMacchina", Me.m_IDMacchina)
        writer.WriteAttribute("IDUtente", Me.m_IDUtente)
        writer.WriteAttribute("Attiva", Me.m_Attiva)
        writer.WriteAttribute("DataInizio", Me.m_DataInizio)
        writer.WriteAttribute("DataFine", Me.m_DataFine)
        writer.WriteAttribute("Flags", Me.m_Flags)
        writer.WriteAttribute("UploadServer", Me.m_UploadServer)
        writer.WriteAttribute("NotifyServer", Me.m_NotifyServer)
        writer.WriteAttribute("ServerName", Me.m_ServerName)
        writer.WriteAttribute("UserName", Me.m_UserName)
        writer.WriteAttribute("RemoveLogAfterNDays", Me.m_RemoveLogAfterNDays)
        writer.WriteAttribute("MinVersion", Me.m_MinVersion)
        writer.WriteAttribute("MaxVersion", Me.m_MaxVersion)
        MyBase.XMLSerialize(writer)
        writer.WriteTag("AsteriskServers", Me.AsteriskServers)
        writer.WriteTag("Dispositivi", Me.Dispositivi)
        writer.WriteTag("Linee", Me.Linee)
        writer.WriteTag("CartelleMonitorate", Me.CartelleMonitorate)
        writer.WriteTag("CartelleEscluse", Me.CartelleEscluse)
        writer.WriteTag("HylafaxConfig", Me.HylafaxConfig)
        writer.WriteTag("Attributi", Me.Attributi)
        writer.WriteTag("ProxyConfig", Me.ProxyConfig)
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "IDPostazione" : Me.m_IDPostazione = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "IDMacchina" : Me.m_IDMacchina = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Attiva" : Me.m_Attiva = CBool(XML.Utils.Serializer.DeserializeBoolean(fieldValue))
            Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "Flags" : Me.m_Flags = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "UploadServer" : Me.m_UploadServer = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "NotifyServer" : Me.m_NotifyServer = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "ServerName" : Me.m_ServerName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "RemoveLogAfterNDays" : Me.m_RemoveLogAfterNDays = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "MinVersion" : Me.m_MinVersion = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "MaxVersion" : Me.m_MaxVersion = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "AsteriskServers"
                Me.m_AsteriskServers = New CCollection(Of AsteriskServer)
                Me.m_AsteriskServers.AddRange(CType(fieldValue, CCollection))
            Case "Dispositivi"
                Me.m_Dispositivi = New CCollection(Of DispositivoEsterno)
                Me.m_Dispositivi.AddRange(CType(fieldValue, CCollection))
            Case "Linee"
                Me.m_Linee = New CCollection(Of LineaEsterna)
                Me.m_Linee.AddRange(CType(fieldValue, CCollection))
            Case "CartelleMonitorate"
                Me.m_CartelleMonitorate = New CCollection(Of String)
                Me.m_CartelleMonitorate.AddRange(CType(fieldValue, CCollection))
            Case "CartelleEscluse"
                Me.m_CartelleEscluse = New CCollection(Of String)
                Me.m_CartelleEscluse.AddRange(CType(fieldValue, CCollection))
            Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
            Case "HylafaxConfig" : Me.m_HylafaxConfig = CType(fieldValue, HylafaxServerConfig)
            Case "ProxyConfig" : Me.m_ProxyConfig = CType(fieldValue, ProxyConfig)
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select
    End Sub

    Protected Overrides Sub OnAfterSave(e As SystemEvent)
        MyBase.OnAfterSave(e)
        DialTPApp.Configs.UpdateCached(Me)
    End Sub


End Class

Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Drivers
Imports DMD.Anagrafica

Partial Public Class Sistema

    <Flags> _
    Public Enum FaxModemFlags As Integer
        ''' <summary>
        ''' Nessun flag
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' Il modem è abilitato all'invio
        ''' </summary>
        ''' <remarks></remarks>
        SendEnabled = 1

        ''' <summary>
        ''' Il modem è abilitato alla ricezione
        ''' </summary>
        ''' <remarks></remarks>
        ReceiveEnabled = 2
    End Enum

    ''' <summary>
    ''' Definisce una linea FAX utilizzabile da un driver
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class FaxDriverModem
        Implements DMD.XML.IDMDXMLSerializable

        Private m_Name As String
        Private m_Tipologia As String
        Private m_Numero As String
        Private m_EMailInvio As String
        Private m_EMailRicezione As String
        Private m_ServerName As String
        Private m_ServerPort As Integer
        Private m_Account As String
        Private m_UserName As String
        Private m_Password As String
        Private m_Flags As FaxModemFlags
        <NonSerialized> _
        Private m_PuntiOperativi As CCollection(Of CUfficio)
        Private m_Parametri As CKeyCollection
        <NonSerialized> Private m_Owner As BaseFaxDriver
        Private m_IDPuntoOperativo As Integer
        <NonSerialized> Private m_PuntoOperativo As CUfficio
        <NonSerialized> Private m_NotificaAGruppi As CCollection(Of CGroup)
        <NonSerialized> Private m_EscludiNotificaGruppi As CCollection(Of CGroup)
        Private m_DialPrefix As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Name = ""
            Me.m_Tipologia = ""
            Me.m_Numero = ""
            Me.m_EMailInvio = ""
            Me.m_EMailRicezione = ""
            Me.m_ServerName = ""
            Me.m_ServerPort = 0
            Me.m_Account = ""
            Me.m_UserName = ""
            Me.m_Password = ""
            Me.m_Flags = FaxModemFlags.ReceiveEnabled Or FaxModemFlags.SendEnabled
            Me.m_PuntiOperativi = Nothing
            Me.m_Parametri = Nothing
            Me.m_Owner = Nothing
            Me.m_IDPuntoOperativo = 0
            Me.m_PuntoOperativo = Nothing
            Me.m_PuntiOperativi = Nothing
            Me.m_NotificaAGruppi = Nothing
            Me.m_EscludiNotificaGruppi = Nothing
            Me.m_DialPrefix = ""
        End Sub

        Protected Overridable Sub DoChanged(ByVal pName As String, ByVal newValue As Object, ByVal oldValue As Object)

        End Sub

        ''' <summary>
        ''' Restituisce o imposta un codice da anteporre al numero per eventuale accesso alle linee del centralino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DialPrefix As String
            Get
                Return Me.m_DialPrefix
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DialPrefix
                If (oldValue = value) Then Exit Property
                Me.m_DialPrefix = value
                Me.DoChanged("DialPrefix", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del server a cui si connette il modem
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServerName As String
            Get
                Return Me.m_ServerName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ServerName
                If (oldValue = value) Then Exit Property
                Me.m_ServerName = value
                Me.DoChanged("ServerName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la porta di ascolto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServerPort As Integer
            Get
                Return Me.m_ServerPort
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ServerPort
                If (oldValue = value) Then Exit Property
                Me.m_ServerPort = value
                Me.DoChanged("ServerPort", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del punto operativo predefinito di consegna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPuntoOperativo As Integer
            Get
                Return GetID(Me.m_PuntoOperativo, Me.m_IDPuntoOperativo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPuntoOperativo
                If (oldValue = value) Then Exit Property
                Me.m_IDPuntoOperativo = value
                Me.m_PuntoOperativo = Nothing
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il punto operativo predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PuntoOperativo As CUfficio
            Get
                If Me.m_PuntoOperativo Is Nothing Then Me.m_PuntoOperativo = Anagrafica.Uffici.GetItemById(Me.m_IDPuntoOperativo)
                Return Me.m_PuntoOperativo
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.PuntoOperativo
                If (oldValue Is value) Then Exit Property
                Me.m_IDPuntoOperativo = GetID(value)
                Me.m_PuntoOperativo = value
                Me.DoChanged("PuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il driver che gestisce il modem
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Owner As BaseFaxDriver
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal owner As BaseFaxDriver)
            Me.m_Owner = owner
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del modem
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tipologia del modem (e-mail, analogico, ecc..)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipologia As String
            Get
                Return Me.m_Tipologia
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Tipologia
                If (oldValue = value) Then Exit Property
                Me.m_Tipologia = value
                Me.DoChanged("Tipologia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero geografico utilizzato per la ricezione e l'invio di fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Numero As String
            Get
                Return Me.m_Numero
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Numero
                If (oldValue = value) Then Exit Property
                Me.m_Numero = value
                Me.DoChanged("Numero", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo email a cui inviare i documenti da spedire come fax (per i servizi Fax Out)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMailInvio As String
            Get
                Return Me.m_EMailInvio
            End Get
            Set(value As String)
                value = Formats.ParseEMailAddress(value)
                Dim oldValue As String = Me.m_EMailInvio
                If (oldValue = value) Then Exit Property
                Me.m_EMailInvio = value
                Me.DoChanged("eMailInvio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo eMail di ascolto su cui vengono inviati i fax ricevuti (per i servizi fax in)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMailRicezione As String
            Get
                Return Me.m_EMailRicezione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_EMailRicezione
                If (oldValue = value) Then Exit Property
                Me.m_EMailRicezione = value
                Me.DoChanged("eMailRicezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'account associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Account As String
            Get
                Return Me.m_Account
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Account
                If (value = oldValue) Then Exit Property
                Me.m_Account = value
                Me.DoChanged("Account", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_UserName
                If (oldValue = value) Then Exit Property
                Me.m_UserName = value
                Me.DoChanged("UserName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Password
                If (oldValue = value) Then Exit Property
                Me.m_Password = value
                Me.DoChanged("Password", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As FaxModemFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As FaxModemFlags)
                Dim oldValue As FaxModemFlags = value
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica il modem è abilitato all'invio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SendEnabled As Boolean
            Get
                Return TestFlag(Me.m_Flags, FaxModemFlags.SendEnabled)
            End Get
            Set(value As Boolean)
                If (Me.SendEnabled = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, FaxModemFlags.SendEnabled, value)
                Me.DoChanged("SendEnabled", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il modem è abilitato alla ricezione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReceiveEnabled As Boolean
            Get
                Return TestFlag(Me.m_Flags, FaxModemFlags.ReceiveEnabled)
            End Get
            Set(value As Boolean)
                If (Me.ReceiveEnabled = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, FaxModemFlags.ReceiveEnabled, value)
                Me.DoChanged("ReceiveEnabled", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce un set di parametri aggiuntivi per il modem
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Parametri As CKeyCollection
            Get
                SyncLock Me
                    If (Me.m_Parametri Is Nothing) Then Me.m_Parametri = New CKeyCollection
                    Return Me.m_Parametri
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei punti operativi per cui il fax è visibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PuntiOperativi As CCollection(Of CUfficio)
            Get
                SyncLock Me
                    If (Me.m_PuntiOperativi Is Nothing) Then Me.m_PuntiOperativi = New CCollection(Of CUfficio)
                    Return Me.m_PuntiOperativi
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei gruppi a cui viene notificata la ricezione di un fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NotificaAGruppi As CCollection(Of CGroup)
            Get
                SyncLock Me
                    If (Me.m_NotificaAGruppi Is Nothing) Then Me.m_NotificaAGruppi = New CCollection(Of CGroup)
                    Return Me.m_NotificaAGruppi
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei gruppi a cui viene impedita la notifica di ricezione di un fax (ha precedenza rispetto a NotificaAGruppi)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EscludiNotificaAGruppi As CCollection(Of CGroup)
            Get
                SyncLock Me
                    If (Me.m_EscludiNotificaGruppi Is Nothing) Then Me.m_EscludiNotificaGruppi = New CCollection(Of CGroup)
                    Return Me.m_EscludiNotificaGruppi
                End SyncLock
            End Get
        End Property



        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipologia" : Me.m_Tipologia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Numero" : Me.m_Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EMailInvio" : Me.m_EMailInvio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EMailRicezione" : Me.m_EMailRicezione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Account" : Me.m_Account = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PuntiOperativi" : Me.m_PuntiOperativi = Me.GetColUffici(XML.Utils.Serializer.ToArray(Of Integer)(fieldValue))
                Case "Parametri" : Me.m_Parametri = XML.Utils.Serializer.ToObject(fieldValue)
                Case "IDPuntoOperativo" : Me.m_IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NotificaAGruppi" : Me.m_NotificaAGruppi = Me.GetColGruppi(XML.Utils.Serializer.ToArray(Of Integer)(fieldValue))
                Case "EscludiNotificaGruppi" : Me.m_EscludiNotificaGruppi = Me.GetColGruppi(XML.Utils.Serializer.ToArray(Of Integer)(fieldValue))
                Case "ServerName" : Me.m_ServerName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ServerPort" : Me.m_ServerPort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DialPrefix" : Me.m_DialPrefix = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("Tipologia", Me.m_Tipologia)
            writer.WriteAttribute("Numero", Me.m_Numero)
            writer.WriteAttribute("EMailInvio", Me.m_EMailInvio)
            writer.WriteAttribute("EMailRicezione", Me.m_EMailRicezione)
            writer.WriteAttribute("Account", Me.m_Account)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Password", Me.m_Password)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("ServerName", Me.m_ServerName)
            writer.WriteAttribute("ServerPort", Me.m_ServerPort)
            writer.WriteAttribute("DialPrefix", Me.m_DialPrefix)
            writer.WriteTag("PuntiOperativi", Me.GetArrUffici())
            writer.WriteTag("Parametri", Me.Parametri)
            writer.WriteTag("NotificaAGruppi", Me.GetArrGruppi(Me.NotificaAGruppi))
            writer.WriteTag("EscludiNotificaGruppi", Me.GetArrGruppi(Me.EscludiNotificaAGruppi))
        End Sub

        Private Function GetArrGruppi(ByVal col As CCollection(Of CGroup)) As Integer()
            Dim ret As New System.Collections.ArrayList
            For Each g As CGroup In col
                ret.Add(GetID(g))
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function GetArrUffici() As Integer()
            Dim ret As New System.Collections.ArrayList
            For Each po As CUfficio In Me.PuntiOperativi
                ret.Add(GetID(po))
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function GetColUffici(ByVal arr As Integer()) As CCollection(Of CUfficio)
            Dim ret As New CCollection(Of CUfficio)
            For i As Integer = 0 To Arrays.Len(arr) - 1
                Dim u As CUfficio = Anagrafica.Uffici.GetItemById(arr(i))
                If (u IsNot Nothing) Then ret.Add(u)
            Next
            Return ret
        End Function

        Private Function GetColGruppi(ByVal arr As Integer()) As CCollection(Of CGroup)
            Dim ret As New CCollection(Of CGroup)
            For i As Integer = 0 To Arrays.Len(arr) - 1
                Dim g As CGroup = Sistema.Groups.GetItemById(arr(i))
                If (g IsNot Nothing) Then ret.Add(g)
            Next
            Return ret
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class

Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Drivers
 
Partial Public Class Sistema

    Public MustInherit Class BasicSMSDriver
        Inherits Driver

        Private m_Modems As SMSDriverModems
        Private m_Config As SMSDriverOptions


        Public Sub New()
            Me.m_Modems = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce il valore della proprietà specificata. 
        ''' Tutti i driver di tipo SMS supportano le proprietà
        '''    maxMessageChars               Che restituisce il numero massimo di caratteri che può contenere un messaggio
        '''    supportsReceiveNotify         Che restituisce vero se il driver supporta la conferma di recapito
        '''    supportsMessageConcatenation  Che restituisce vero se più messaggi possono essere concatenati
        ''' </summary>
        ''' <param name="capName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetDriverCAP(capName As String) As Object
            Select Case capName
                Case "maxMessageChars" : Return Me.GetMaxSMSLen
                Case "supportsReceiveNotify" : Return Me.SupportaConfermaDiRecapito()
                Case "supportsMessageConcatenation" : Return Me.SupportaMessaggiConcatenati
                Case Else : Return MyBase.GetDriverCAP(capName)
            End Select
        End Function

        ''' <summary>
        ''' Restituisce un oggetto che descrive la configurazione del driver
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Config As SMSDriverOptions
            Get
                SyncLock Me
                    If (Me.m_Config Is Nothing) Then Me.m_Config = Me.GetDefaultOptions
                    Return Me.m_Config
                End SyncLock
            End Get
        End Property

        Protected Friend Overridable Sub SetConfig(ByVal value As SMSDriverOptions)
            Me.m_Config = value
        End Sub

        Public Overridable Function GetModem(ByVal modemName As String) As SMSDriverModem
            modemName = Strings.Trim(modemName)
            If (modemName <> "") Then
                For Each modem As SMSDriverModem In Me.Modems
                    If modem.Name = modemName Then
                        Return modem
                    End If
                Next
            End If
            If (Me.Modems.Count > 0) Then Return Me.Modems(0)
            Return Nothing
        End Function

        Public ReadOnly Property Modems As SMSDriverModems
            Get
                SyncLock Me
                    If (Me.m_Modems Is Nothing) Then
                        Dim str As String = Me.Config.GetValueString("__MODEMSCONFIG__", "")
                        Try
                            Me.m_Modems = XML.Utils.Serializer.Deserialize(str)
                            Me.m_Modems.SetOwner(Me)
                        Catch ex As Exception
                            Sistema.Events.NotifyUnhandledException(ex)
                            Me.m_Modems = New SMSDriverModems(Me)
                        End Try
                    End If
                    Return Me.m_Modems
                End SyncLock
            End Get
        End Property


        ''' <summary>
        ''' Invia al numero specificato un SMS (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
        ''' </summary>
        ''' <param name="destNumber"></param>
        ''' <param name="testo"></param>
        ''' <returns>Restituisce un ID con cui è possibile recuperare lo stato del messaggio</returns>
        ''' <remarks></remarks>
        Protected Friend MustOverride Function Send(ByVal destNumber As String, ByVal testo As String, ByVal options As SMSDriverOptions) As String

        ''' <summary>
        ''' Invia al numero specificato un SMS (ciascuna pagina è contenuta in una singola immagine) utilizzando il sistema predefinito per l'invio
        ''' </summary>
        ''' <param name="messageID">ID del messaggio restituito dalla funzione Send</param>
        ''' <returns>Restituisce un ID con cui è possibile recuperare lo stato del messaggio</returns>
        ''' <remarks></remarks>
        Protected Friend MustOverride Function GetStatus(ByVal messageID As String) As MessageStatus

        ''' <summary>
        ''' Restituisce le impostazioni predefinite del driver
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function InstantiateNewOptions() As DriverOptions
            Return New SMSDriverOptions
        End Function

        Protected Friend Overridable Function IsValidNumber(ByVal value As String) As Boolean
            value = Formats.ParsePhoneNumber(value)
            Return Len(value) > 3
        End Function

        ''' <summary>
        ''' Quando sottoposto ad override in una classe derivata restituisce vero se il driver supporta la conferma di lettura
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function SupportaConfermaDiRecapito() As Boolean

        ''' <summary>
        ''' Restituisce vero se il sistema supporta la concatenzaione dei messaggi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function SupportaMessaggiConcatenati() As Boolean

        ''' <summary>
        ''' Restituisce la lunghezza massima di un SMS
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend Overridable Function GetMaxSMSLen() As Integer
            Return 160
        End Function

        ''' <summary>
        ''' Restituisce il numero di caratteri conteggiati per l'invio dell'SMS
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend MustOverride Function GetSMSTextLen(ByVal text As String) As Integer

        ''' <summary>
        ''' Restituisce il numero di SMS concatenati necessari per inviare il testo
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend Overridable Function CountRequiredSMS(ByVal text As String) As Integer
            Return Math.Ceiling(Me.GetSMSTextLen(text) / Me.GetMaxSMSLen)
        End Function


        Protected Friend MustOverride Sub VerifySender(ByVal sender As String)


        Public Overridable Sub NotifyDeliveredSMS(ByVal order_id As String, ByVal recipient As String, ByVal status As MessageStatusEnum, ByVal delivery_date As Nullable(Of Date))
            Dim e As New SMSDeliverEventArgs(order_id, recipient, status, delivery_date)
            SMSService.doSMSDelivered(e)
        End Sub

        Public Overridable Sub NotifyReceivedSMS(ByVal modem As SMSDriverModem, ByVal order_id As String, ByVal sender As String, ByVal receiveDate As Date, ByVal message As String)
            Dim e As New SMSReceivedEventArgs(modem, order_id, sender, receiveDate, message)
            SMSService.doSMSReceived(e)
        End Sub

        Public Overridable Sub NotifySMSStatus(ByVal modem As SMSDriverModem, ByVal order_id As String, ByVal status As MessageStatusEnum, ByVal statusEx As String, ByVal time As Date?)
            Dim e As New SMSStatusEventArgs(modem, order_id, status, statusEx, time)
            SMSService.doSMSStatusChanged(e)
        End Sub

        Public Overridable Sub NotifyEvent(ByVal modem As SMSDriverModem, ByVal eventType As String, ByVal eventParameters As Object)
            Dim e As New SMSEventArgs(modem, eventType, eventParameters)
            SMSService.doSMSEvent(e)
        End Sub

        Protected Overrides Function GetDefaultSettingsFileName() As String
            Return Global.System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "Drivers\SMS\" & Me.GetUniqueID & "\config.cfg")
        End Function


        Protected Overrides Sub SetFieldInternal(fielName As String, fieldValue As Object)
            Select Case fielName
                Case "Modems" : Me.m_Modems = fieldValue : Me.m_Modems.SetOwner(Me)
                Case "Configuration" : Me.m_Config = XML.Utils.Serializer.ToObject(fieldValue)
                Case Else : MyBase.SetFieldInternal(fielName, fieldValue)
            End Select
        End Sub


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Modems", Me.Modems)
            writer.WriteTag("Configuration", Me.Config)
        End Sub

        Public Overridable Sub SaveConfiguration()
            Me.Config.SetValueString("__MODEMSCONFIG__", XML.Utils.Serializer.Serialize(Me.Modems))
            Me.SetConfig(Me.Config)
            Dim path As String = Me.GetDefaultSettingsFileName 'Global.System.IO.Path.Combine(WebSite.Instance.AppContext.SystemDataFolder, "TrendooSMS\config.cfg")
            DMD.Sistema.FileSystem.CreateRecursiveFolder(Sistema.FileSystem.GetFolderName(path))
            Me.Config.SaveToFile(path)
            DMD.Sistema.SMSService.UpdateDriver(Me)
        End Sub
    End Class

 
End Class
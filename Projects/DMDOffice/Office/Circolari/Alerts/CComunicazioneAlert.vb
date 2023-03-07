Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Office

    Public Enum StatoAlertComunicazione As Integer
        NONCONSEGNATO = 0
        CONSEGNATO = 1
        LETTO = 2
    End Enum

    <Serializable> _
    Public Class CComunicazioneAlert
        Inherits DBObject

        Private m_IDComunicazione As Integer
        Private m_Comunicazione As CComunicazione
        Private m_TargetUserID As Integer
        Private m_TargetUser As CUser
        Private m_Via As String
        Private m_Param As String
        Private m_StatoComunicazione As StatoAlertComunicazione
        Private m_DataConsegna As Nullable(Of Date)
        Private m_DataLettura As Nullable(Of Date)
        Private m_DettaglioStato As String

        Public Sub New()
            Me.m_IDComunicazione = 0
            Me.m_Comunicazione = Nothing
            Me.m_TargetUserID = 0
            Me.m_TargetUser = Nothing
            Me.m_Via = ""
            Me.m_StatoComunicazione = StatoAlertComunicazione.NONCONSEGNATO
            Me.m_DataConsegna = Nothing
            Me.m_DataLettura = Nothing
            Me.m_DettaglioStato = ""
        End Sub

        Public Property IDComunicazione As Integer
            Get
                Return GetID(Me.m_Comunicazione, Me.m_IDComunicazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDComunicazione
                If oldValue = value Then Exit Property
                Me.m_IDComunicazione = value
                Me.m_Comunicazione = Nothing
                Me.DoChanged("IDComunicazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o impsta l'oggetto comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Comunicazione As CComunicazione
            Get
                If Me.m_Comunicazione Is Nothing Then Me.m_Comunicazione = Comunicazioni.GetItemById(Me.m_IDComunicazione)
                Return Me.m_Comunicazione
            End Get
            Set(value As CComunicazione)
                Dim oldValue As CComunicazione = Me.Comunicazione
                If (oldValue = value) Then Exit Property
                Me.m_Comunicazione = value
                Me.m_IDComunicazione = GetID(value)
                Me.DoChanged("Comunicazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente destinatario della comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetUser As CUser
            Get
                If Me.m_TargetUser Is Nothing Then Me.m_TargetUser = Sistema.Users.GetItemById(Me.m_TargetUserID)
                Return Me.m_TargetUser
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.TargetUser
                If (oldValue = value) Then Exit Property
                Me.m_TargetUser = value
                Me.m_TargetUserID = GetID(value)
                Me.DoChanged("TargetUser", value, oldValue)
            End Set
        End Property

        Public Property TargetUserID As Integer
            Get
                Return GetID(Me.m_TargetUser, Me.m_TargetUserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.TargetUserID
                If oldValue = value Then Exit Property
                Me.m_TargetUser = Nothing
                Me.m_TargetUserID = value
                Me.DoChanged("TargetUserID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituosce o imposta una stringa che descrive lo stato della comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioStato As String
            Get
                Return Me.m_DettaglioStato
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Via
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStato = value
                Me.DoChanged("DettaglioStato ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mezzo attraverso cui è stata inviata la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Via As String
            Get
                Return Me.m_Via
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Via
                If (oldValue = value) Then Exit Property
                Me.m_Via = value
                Me.DoChanged("Via", value, oldValue)
            End Set
        End Property

        Public Property Param As String
            Get
                Return Me.m_Param
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Param
                If (oldValue = value) Then Exit Property
                Me.m_Param = value
                Me.DoChanged("Param", value, oldValue)
            End Set
        End Property

        Public Property StatoComunicazione As StatoAlertComunicazione
            Get
                Return Me.m_StatoComunicazione
            End Get
            Set(value As StatoAlertComunicazione)
                Dim oldValue As StatoAlertComunicazione = Me.m_StatoComunicazione
                If (oldValue = value) Then Exit Property
                Me.m_StatoComunicazione = value
                Me.DoChanged("StatoComunicazione", value, oldValue)
            End Set
        End Property

        Public Property DataConsegna As Nullable(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConsegna
                If (oldValue = value) Then Exit Property
                Me.m_DataConsegna = value
                Me.DoChanged("DataConsegna", value, oldValue)
            End Set
        End Property

        Public Property DataLettura As Nullable(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataLettura
                If (oldValue = value) Then Exit Property
                Me.m_DataLettura = value
                Me.DoChanged("DataLettura", value, oldValue)
            End Set
        End Property

        Private Function ParseTemplate(ByVal body As String) As String
            body = Replace(body, "%%TARGERTUSERNAME%%", Me.TargetUser.Nominativo)
            body = Replace(body, "%%SOURCEUSERNAME%%", Users.CurrentUser.Nominativo)
            body = Replace(body, "%%ID%%", Me.IDComunicazione)
            body = Replace(body, "%%DOCUMENTNAME%%", Me.Comunicazione.ToString)
            body = Replace(body, "%%DOCUMENTLINK%%", Me.Comunicazione.URL)
            body = Replace(body, "%%BASEURL%%", ApplicationContext.BaseURL)
            body = Replace(body, "%%NUMEROCOMUNICAZIONE%%", Me.Comunicazione.Progressivo)
            body = Replace(body, "%%CATEGORIACOMUNICAZIONE%%", Me.Comunicazione.Categoria)
            body = Replace(body, "%%DESCRIZIONECOMUNICAZIONE%%", Me.Comunicazione.Descrizione)
            body = Replace(body, "%%NOTECOMUNICAZIONE%%", Me.Comunicazione.Note)
            Return body
        End Function

        Public Sub Notify()
            Me.Save()
            Select Case LCase(Me.Via)
                Case "e-mail"
                    If Me.Param <> "" Then
                        Dim template As String = ApplicationContext.MapPath(Comunicazioni.Module.Settings.GetValueString("Template"))
                        Dim body As String = Me.ParseTemplate(FileSystem.GetTextFileContents(template))
                        Dim subject As String = Comunicazioni.Module.Settings.GetValueString("NotifySubject")
                        If (subject = "") Then subject = "E' richiesta la presa visione della circolare N° %%NUMEROCOMUNICAZIONE%% - %%DOCUMENTNAME%%" '"Notifica da parte di %%SOURCEUSERNAME%%: Il documento %%DOCUMENTNAME%% è stato aggiornato"
                        subject = Me.ParseTemplate(subject)
                        Dim sender As String = Comunicazioni.Module.Settings.GetValueString("Sender")
                        Dim m As System.Net.Mail.MailMessage = EMailer.PrepareMessage(sender, Me.Param, "", "", subject, body, "COMUNICAZIONI", True)
                        EMailer.SendMessageAsync(m, True)
                    End If
                Case Else
                    Dim text As String = "E' richiesta la presa visione della circolare N° <b>%%NUMEROCOMUNICAZIONE%%</b><br/><span style='color:blue;'>%%DOCUMENTNAME%%</span>"
                    text = Me.ParseTemplate(text)
                    Sistema.Notifiche.ProgramAlert(Me.TargetUser, text, DateUtils.Now, Me, "Comunicazioni/Circolari")
            End Select
        End Sub
         
        Public Overrides Function ToString() As String
            Return Me.IDComunicazione
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ComunicazioniAlert"
        End Function

        Protected Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDComunicazione = reader.Read("IDComunicazione", Me.m_IDComunicazione)
            Me.m_TargetUserID = reader.Read("IDUser", Me.m_TargetUserID)
            Me.m_Via = reader.Read("Via", Me.m_Via)
            Me.m_Param = reader.Read("Param", Me.m_Param)
            Me.m_StatoComunicazione = reader.Read("StatoComunicazione", Me.m_StatoComunicazione)
            Me.m_DataConsegna = reader.Read("DataConsegna", Me.m_DataConsegna)
            Me.m_DataLettura = reader.Read("DataLettura", Me.m_DataLettura)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDComunicazione", Me.IDComunicazione)
            writer.Write("IDUser", Me.TargetUserID)
            writer.Write("Via", Me.m_Via)
            writer.Write("Param", Me.m_Param)
            writer.Write("StatoComunicazione", Me.m_StatoComunicazione)
            writer.Write("DataConsegna", Me.m_DataConsegna)
            writer.Write("DataLettura", Me.m_DataLettura)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDComunicazione", Me.IDComunicazione)
            writer.WriteAttribute("IDUser", Me.TargetUserID)
            writer.WriteAttribute("Via", Me.m_Via)
            writer.WriteAttribute("Param", Me.m_Param)
            writer.WriteAttribute("StatoComunicazione", Me.m_StatoComunicazione)
            writer.WriteAttribute("DataConsegna", Me.m_DataConsegna)
            writer.WriteAttribute("DataLettura", Me.m_DataLettura)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDComunicazione" : Me.m_IDComunicazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUser" : Me.m_TargetUserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Via" : Me.m_Via = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Param" : Me.m_Param = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoComunicazione" : Me.m_StatoComunicazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataConsegna" : Me.m_DataConsegna = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataLettura" : Me.m_DataLettura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class




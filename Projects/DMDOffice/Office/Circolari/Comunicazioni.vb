Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals

Partial Class Office

    Public NotInheritable Class CComunicazioniClass
        Inherits CGeneralClass(Of CComunicazione)

        Private m_Database As CDBConnection
        Private m_Index As CIndexingService = Nothing

        Friend Sub New()
            MyBase.New("Comunicazioni", GetType(CComunicazioniCursor))
        End Sub

        Public Property Database As CDBConnection
            Get
                If (m_Database Is Nothing) Then Return Office.Database
                Return m_Database
            End Get
            Set(value As CDBConnection)
                If (Me.m_Database Is value) Then Return
                Me.m_Database = value
                Me.m_Index = Nothing
            End Set
        End Property

        Public Overrides Function GetConnection() As CDBConnection
            Return Me.Database
        End Function

        Public ReadOnly Property Index As CIndexingService
            Get
                SyncLock Me
                    If (Me.m_Index Is Nothing) Then
                        Dim db As CDBConnection = Me.GetConnection
                        Me.m_Index = New CIndexingService(db)
                        Me.m_Index.Database = db
                        Me.m_Index.WordIndexFolder = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "circolari\wordindex\")
                        DMD.Sistema.FileSystem.CreateRecursiveFolder(Me.m_Index.WordIndexFolder)
                    End If
                    Return Me.m_Index
                End SyncLock
            End Get
        End Property


        ''' <summary>
        ''' Crea un avviso per la ricezione della comunicazione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Notify(ByVal comunicazione As CComunicazione, Optional ByVal via As String = vbNullString) As CCollection(Of CComunicazioneAlert)
            Dim users As CCollection(Of CUser) = comunicazione.GetAffectedUsers
            Dim ret As New CCollection(Of CComunicazioneAlert)

            Dim i As Integer = 0
            via = Trim(via)
            For Each user As CUser In users
                Dim item As New CComunicazioneAlert
                item.Comunicazione = comunicazione
                item.TargetUser = user
                item.Stato = ObjectStatus.OBJECT_VALID
                item.Via = via
                item.Param = user.eMail
#If Not DEBUG Then
            Try
#End If
                item.Notify()
                item.StatoComunicazione = StatoAlertComunicazione.CONSEGNATO
                item.DataConsegna = Now
                item.DettaglioStato = "Consegnato"
                i += 1
#If Not DEBUG Then
            Catch ex As Exception
                item.StatoComunicazione = StatoAlertComunicazione.NONCONSEGNATO
                item.DettaglioStato = Strings.Left(ex.Message, 255)
                '
            End Try
#End If
                item.Save()

                ret.Add(item)
            Next

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il primo numero progressivo disponibile
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNextAvailableProgressivo() As Integer
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            Dim maxID As Integer = 0
            dbSQL = "SELECT Max([Progressivo]) As [NextProgressivo] FROM [tbl_Comunicazioni] WHERE [Stato]=1;"
            dbRis = APPConn.ExecuteReader(dbSQL)
            If dbRis.Read Then maxID = Formats.ToInteger(dbRis("NextProgressivo"))
            dbRis.Dispose()
            dbRis = Nothing
            Return maxID + 1
        End Function


        ' ''' <summary>
        ' ''' Restituisce l'oggetto Configurazione
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared ReadOnly Property Configuration As CComunicazioniConfiguration
        '    Get
        '        If m_Configuration Is Nothing Then
        '            m_Configuration = New CComunicazioniConfiguration
        '            m_Configuration.Load()
        '        End If
        '        Return m_Configuration
        '    End Get
        'End Property



        'Public Class CComunicazioniConfiguration
        '    Inherits DBObjectBase

        '    Private m_SMTPServerName As String 'Nome del server STMP
        '    Private m_SMTPServerPort As Integer 'Porta di ascolto del server SMTP
        '    Private m_SenderEMail As String 'Indirizzo e-mail del mittente
        '    Private m_SenderPassword As String 'Password del mittente per accedere al server SMTP
        '    Private m_TemplateURL As String 'Percorso del modello da utilizzare per l'invio e-mail
        '    Private m_SendList As String 'Elenco degli utenti a cui inviare la comunicazione

        '    Public Sub New()
        '        m_SMTPServerName = ""
        '        m_SMTPServerPort = 0
        '        m_SenderEMail = ""
        '        m_SenderPassword = ""
        '        m_TemplateURL = ""
        '        m_SendList = ""
        '    End Sub

        '    Public Overrides Function GetModule() As CModule
        '        Return Nothing
        '    End Function

        '    Public Property SMTPServerName As String
        '        Get
        '            Return Me.m_SMTPServerName
        '        End Get
        '        Set(value As String)
        '            value = Trim(value)
        '            Dim oldValue As String = Me.m_SMTPServerName
        '            If (oldValue = value) Then Exit Property
        '            Me.m_SMTPServerName = value
        '            Me.DoChanged("SMTPServerName", value, oldValue)
        '        End Set
        '    End Property

        '    Public Property SMTPServerPort As Integer
        '        Get
        '            Return Me.m_SMTPServerPort
        '        End Get
        '        Set(value As Integer)
        '            Dim oldValue As Integer = Me.m_SMTPServerPort
        '            If (oldValue = value) Then Exit Property
        '            Me.m_SMTPServerPort = value
        '            Me.DoChanged("SMTPServerName", value, oldValue)
        '        End Set
        '    End Property

        '    Public Property SenderEMail As String
        '        Get
        '            Return Me.m_SenderEMail
        '        End Get
        '        Set(value As String)
        '            value = Trim(value)
        '            Dim oldValue As String = Me.m_SenderEMail
        '            If (oldValue = value) Then Exit Property
        '            Me.m_SenderEMail = value
        '            Me.DoChanged("SenderEMail", value, oldValue)
        '        End Set
        '    End Property

        '    Public Property SenderPassword As String
        '        Get
        '            Return Me.m_SenderPassword
        '        End Get
        '        Set(value As String)
        '            Dim oldValue As String = Me.m_SenderPassword
        '            If (oldValue = value) Then Exit Property
        '            Me.m_SenderPassword = value
        '            Me.DoChanged("SenderPassword", value, oldValue)
        '        End Set
        '    End Property

        '    Public Property TemplateURL As String
        '        Get
        '            Return Me.m_TemplateURL
        '        End Get
        '        Set(value As String)
        '            value = Trim(value)
        '            Dim oldValue As String = Me.m_TemplateURL
        '            If (oldValue = value) Then Exit Property
        '            Me.m_TemplateURL = value
        '            Me.DoChanged("TemplateURL", value, oldValue)
        '        End Set
        '    End Property

        '    Public Property SendList As String
        '        Get
        '            Return Me.m_SendList
        '        End Get
        '        Set(value As String)
        '            value = Trim(value)
        '            Dim oldValue As String = Me.m_SendList
        '            If (oldValue = value) Then Exit Property
        '            Me.m_SendList = value
        '            Me.DoChanged("SendList", value, oldValue)
        '        End Set
        '    End Property

        '    Public Overrides Function GetTableName() As String
        '        Return "tbl_ComunicazioniConfig"
        '    End Function

        '    Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
        '        reader.Read("SMTPServerName", Me.m_SMTPServerName)
        '        reader.Read("SMTPServerPort", Me.m_SMTPServerPort)
        '        reader.Read("SenderEMail", Me.m_SenderEMail)
        '        reader.Read("SenderPassword", Me.m_SenderPassword)
        '        reader.Read("TemplateURL", Me.m_TemplateURL)
        '        reader.Read("SendList", Me.m_SendList)
        '        Return MyBase.LoadFromRecordset(reader)
        '    End Function

        '    Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
        '        writer.Write("SMTPServerName", Me.m_SMTPServerName)
        '        writer.Write("SMTPServerPort", Me.m_SMTPServerPort)
        '        writer.Write("SenderEMail", Me.m_SenderEMail)
        '        writer.Write("SenderPassword", Me.m_SenderPassword)
        '        writer.Write("TemplateURL", Me.m_TemplateURL)
        '        writer.Write("SendList", Me.m_SendList)
        '        Return MyBase.SaveToRecordset(writer)
        '    End Function

        '    Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
        '        Return Databases.APPConn
        '    End Function

        '    Public Sub Load()
        '        Dim dbRis1 As System.Data.IDataReader
        '        dbRis1 = APPConn.ExecuteReader("SELECT * FROM [tbl_ComunicazioniConfig] ORDER BY [ID] ASC")
        '        Dim reader As New DBReader(APPConn.Tables("tbl_ComunicazioniConfig"), dbRis1)
        '        If reader.Read Then APPConn.Load(Me, reader)
        '        dbRis1.Close()
        '    End Sub



        'End Class

    End Class

    Private Shared m_Comunicazioni As CComunicazioniClass = Nothing

    Public Shared ReadOnly Property Comunicazioni As CComunicazioniClass
        Get
            If (m_Comunicazioni Is Nothing) Then m_Comunicazioni = New CComunicazioniClass
            Return m_Comunicazioni
        End Get
    End Property

End Class
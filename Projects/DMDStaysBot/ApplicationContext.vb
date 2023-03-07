Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Drivers
Imports DMD.Net.Mail
Imports org.apache.pdfbox.pdmodel
Imports org.apache.pdfbox.util
Imports System.Collections.Specialized

Public Class MyApplicationContext
    Implements DMD.Sistema.IApplicationContext

    Const debugBD = "http://localhost:33016/bd/stay.aspx"

    Private Shared m_Settings As New CKeyCollection
    Private m_ID As String = ASPSecurity.GetRandomKey(25)
    Private m_Ufficio As CUfficio
    Private m_CurrentLogin As CLoginHistory

    Public ReadOnly Property Description As String Implements IApplicationContext.Description
        Get
            Return Application.ProductName
        End Get
    End Property

    Public ReadOnly Property InstanceID As String Implements IApplicationContext.InstanceID
        Get
            Return m_ID
        End Get
    End Property

    Public ReadOnly Property RemoteIP As String Implements IApplicationContext.RemoteIP
        Get
            Return "0.0.0.0"
        End Get
    End Property

    Public ReadOnly Property RemotePort As Integer Implements IApplicationContext.RemotePort
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property StartupFloder As String Implements IApplicationContext.StartupFloder
        Get
            Return Application.StartupPath
        End Get
    End Property

    Public ReadOnly Property TmporaryFolder As String Implements IApplicationContext.TmporaryFolder
        Get
            Return System.IO.Path.GetTempPath
        End Get
    End Property

    Public ReadOnly Property SystemDataFolder As String Implements IApplicationContext.SystemDataFolder
        Get
            Return Application.UserAppDataPath
        End Get
    End Property

    Public ReadOnly Property UserDataFolder As String Implements IApplicationContext.UserDataFolder
        Get
            Return Application.UserAppDataPath
        End Get
    End Property

    Private m_CurrentUser As CUser = Nothing

    Public Property CurrentUser As Sistema.CUser Implements DMD.Sistema.IApplicationContext.CurrentUser
        Get
            Return Me.m_CurrentUser
        End Get
        Set(value As Sistema.CUser)
            Me.m_CurrentUser = value
        End Set
    End Property

    Public Function GetEntryAssembly() As Reflection.Assembly Implements IApplicationContext.GetEntryAssembly
        Return System.Reflection.Assembly.GetEntryAssembly
    End Function

    Public ReadOnly Property Settings As CKeyCollection Implements IApplicationContext.Settings
        Get
            Return m_Settings
        End Get
    End Property

    Public Function GetParameter(paramName As String, Optional ByVal defValue As String = vbNullString) As String Implements IApplicationContext.GetParameter
        'Select Case paramName
        '    Case "ID" : Return "27" '"1347956004"
        '    Case "params" : Return "<?xml version=""1.0""?><CRapportiniCursor xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><m_Token></m_Token><m_PageNum>-1</m_PageNum><m_Index>-1</m_Index><m_NumItems>-1</m_NumItems><m_Fields><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Autore</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CQS_PD</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Cessionario</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>T</m_IsSet><m_FieldName>CodiceFiscale</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CognomeCliente</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Commerciale</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CreatoDa</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CreatoIl</m_FieldName><m_DataType>7</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>DaVedere</m_FieldName><m_DataType>11</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>1</m_SortOrder><m_SortPriority>100</m_SortPriority><m_Value>F</m_Value><m_Value1>F</m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>FonteContatto</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>T</m_IsSet><m_FieldName>IDPuntoOperativo</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value>3</m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ID</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>LCase(Trim([NomeCliente] &amp; &#39; &#39; &amp; [CognomeCliente]))</m_FieldName><m_DataType>130</m_DataType><m_Operator>3</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>1</m_SortOrder><m_SortPriority>98</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ModificatoDa</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ModificatoIl</m_FieldName><m_DataType>7</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NatoAComune</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NatoAProvincia</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NatoIl</m_FieldName><m_DataType>7</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NettoRicavo</m_FieldName><m_DataType>6</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeCessionario</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeCliente</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeCommerciale</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeOperatore</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeProduttore</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeProfilo</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomePuntoOperativo</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NumeroRate</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>PartitaIVA</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Prodotto</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Produttore</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteACAP</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteACivico</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteAComune</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteAProvincia</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteAVia</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>StatoPratica</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>1</m_SortOrder><m_SortPriority>99</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>T</m_IsSet><m_FieldName>Stato</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value>1</m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>TipoFonteContatto</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>-1</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField></m_Fields><m_IgnoreRights>F</m_IgnoreRights><m_PageSize>25</m_PageSize><m_Items></m_Items><m_DataInserimento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataInserimento><m_DataRichiestaDelibera><CIntervalloData><m_Tipo></m_Tipo><m_Inizio></m_Inizio><m_Fine></m_Fine></CIntervalloData></m_DataRichiestaDelibera><m_DataDelibera><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataDelibera><m_DataCaricamento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataCaricamento><m_DataLiquidazione><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataLiquidazione><m_DataArchiviazione><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataArchiviazione><m_DataAnnullamento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataAnnullamento><m_DataTrasferimento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataTrasferimento></CRapportiniCursor>"
        '    Case "tn" : Return "CRapportiniCursor"
        '    Case Else : Return vbNullString
        'End Select
        Return ""
    End Function

    Public Function GetParameter(Of T)(paramName As String, Optional defValue As Object = Nothing) As T Implements IApplicationContext.GetParameter
        Return CType(Types.CastTo(Me.GetParameter(paramName), Type.GetTypeCode(GetType(T))), T)
    End Function

    Private m_CurrentSession As Object

    Public Property CurrentSession As Object Implements IApplicationContext.CurrentSession
        Get
            Return Me.m_CurrentSession
        End Get
        Set(value As Object)
            Me.m_CurrentSession = value
        End Set
    End Property

    Public Function MapPath(path As String) As String Implements IApplicationContext.MapPath
        Return path
    End Function

    Public Function UnMapPath(path As String) As String Implements IApplicationContext.UnMapPath
        Return path
    End Function

    Public Function IsUserLogged(user As CUser) As Boolean Implements IApplicationContext.IsUserLogged
        Return GetID(user) <> 0
    End Function

    Public Overridable Function IsDebug() As Boolean Implements IApplicationContext.IsDebug
        Return True
    End Function


    ''' <summary>
    ''' Restituisce o imposta l'azienda utilizzata come azienda principale
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property IDAziendaPrincipale As Integer Implements IApplicationContext.IDAziendaPrincipale
        Get
            Return 0 'Remote.AziendaPrincipale
        End Get
        Set(value As Integer)
            'Remote.AziendaPrincipale = value
        End Set
    End Property

    Public ReadOnly Property BaseURL As String Implements IApplicationContext.BaseURL
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property SupportEMail As String Implements IApplicationContext.SupportEMail
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property Title As String Implements IApplicationContext.Title
        Get
            Return ""
        End Get
    End Property

    Public Sub Start() Implements IApplicationContext.Start
        AddHandler Sistema.Events.UnhandledException, AddressOf UnhandledExceptionHandler

        Sistema.Types.Imports.Add("DMD")
        Sistema.Types.Imports.Add("DMD.Sistema")
        Sistema.Types.Imports.Add("DMD.FileSystem")
        Sistema.Types.Imports.Add("DMD.Databases")
        Sistema.Types.Imports.Add("DMD.Calendar")
        Sistema.Types.Imports.Add("DMD.Collegamenti")
        Sistema.Types.Imports.Add("DMD.Comunicazioni")
        Sistema.Types.Imports.Add("DMD.Appuntamenti")
        Sistema.Types.Imports.Add("DMD.GDE")
        Sistema.Types.Imports.Add("DMD.Luoghi")
        Sistema.Types.Imports.Add("DMD.Anagrafica")
        Sistema.Types.Imports.Add("DMD.CustomerCalls")
        Sistema.Types.Imports.Add("DMD.CQSPD")
        Sistema.Types.Imports.Add("DMD.Forms")
        Sistema.Types.Imports.Add("DMD.Messenger")
        Sistema.Types.Imports.Add("DMD.Web")
        Sistema.Types.Imports.Add("DMD.Web.WebSite")
        Sistema.Types.Imports.Add("DMD.Visite")
        Sistema.Types.Imports.Add("DMD.ADV")
        Sistema.Types.Imports.Add("DMD.Anagrafica+Fonti")
        Sistema.Types.Imports.Add("DMD.Office")
        Sistema.Types.Imports.Add("DMD.Mail")
        Sistema.Types.Imports.Add("DMD.Tickets")
        Sistema.Types.Imports.Add("DMD.WebSite")
        Sistema.Types.Imports.Add("DMD.Drivers")

        'Sistema.Types.NewTypeHandlers.Add(New NewTypeHandlers)

        'DMD.Databases.APPConn.Path = Me.MapPath("/mdb-database/sitedb.mdb")
        'DMD.Databases.APPConn.OpenDB()

        'DMD.Databases.LOGConn.Path = Me.MapPath("/mdb-database/log.mdb")
        'DMD.Databases.LOGConn.OpenDB()

        AddHandler Sistema.EMailer.MessageReceived, AddressOf handleEMailReceived


        ''DMD.Sistema.EMailer.Config.SMTPLimitOutSent = My.Settings.SMTPLimitOutSent
        'DMD.Sistema.EMailer.Config.SMTPUserName = My.Settings.SMTPUserName
        'DMD.Sistema.EMailer.Config.SMTPPassword = My.Settings.SMTPPassword
        'DMD.Sistema.EMailer.Config.SMTPServer = My.Settings.SMTPServer
        'DMD.Sistema.EMailer.Config.SMTPServerPort = My.Settings.SMTPPort
        'DMD.Sistema.EMailer.Config.SMTPUseSSL = My.Settings.SMTPSSL




    End Sub

    Public Sub handleEMailReceived(ByVal sender As Object, ByVal e As MailMessageReceivedEventArgs)
        Dim m As MailMessageEx = e.Message

        Dim fromAddress As String = LCase(m.From.Address)
        Dim subject As String = LCase(m.Subject)



        'Select Case fromAddress
        ' Case "iniziativarinnovabili@prestitalia.it"
        Select Case subject
            Case "iniziativa stay - fin.se.a. srl"
#If Not DEBUG Then
                Try
#End If
                Me.Log("Ricevuta e-mail con titolo Iniziativa STAY - FIN.SE.A. SRL")
                Me.handleStay201705(m)
#If Not DEBUG Then
                Catch ex As Exception
                    Me.LogParseEMailError("Errore nell'elaborazione della mail di Prestitalia - Segnalazione nominativi iniziativa Stay", ex, m)
                End Try
#End If
            Case "richiesta contatto da  campagna dem"
#If Not DEBUG Then
                Try
#End If
                Me.Log("Ricevuta e-mail con titolo DEM")
                Me.handleDEM(m)
#If Not DEBUG Then
                Catch ex As Exception
                    Me.LogParseEMailError("Errore nell'elaborazione della mail di Prestitalia - Segnalazione nominativi iniziativa Stay", ex, m)
                End Try
#End If
            'Case "iniziativa rinnovabili"

            '    Try
            '        'Me.handleEMailIniziativaRinnovabili(m)
            '    Catch ex As Exception
            '        Me.LogParseEMailError("Errore nell'elaborazione della mail di Prestitalia - Iniziativa Rinnovabili", ex, m)
            '    End Try
            Case "segnalazione nominativi iniziativa stay"
#If Not DEBUG Then
                Try
#End If
                Me.Log("Ricevuta e-mail con titolo Segnalazioni Stay")
                Me.handleEMailSegnalazioniStay(m)
#If Not DEBUG Then
                Catch ex As Exception
                    Me.LogParseEMailError("Errore nell'elaborazione della mail di Prestitalia - Segnalazione nominativi iniziativa Stay", ex, m)
                End Try
#End If
        End Select
        'End Select

    End Sub

    Private Sub LogParseEMailError(ByVal descrizione As String, ByVal ex As System.Exception, ByVal m As MailMessageEx)
        Dim test As String = descrizione & vbCrLf & "Errore: " & ex.Message & vbCrLf & "Stack: " & Left(ex.StackTrace.ToString, 64) & vbCrLf & vbCrLf
        test &= "Data: " & Formats.FormatUserDateTime(m.DeliveryDate) & vbCrLf
        test &= "Da: " & m.From.ToString & vbCrLf
        test &= "A: " & m.To.ToString & vbCrLf
        test &= "CC: " & m.CC.ToString & vbCrLf
        test &= "CCn: " & m.Bcc.ToString & vbCrLf
        test &= "Oggetto: " & m.Subject & vbCrLf & vbCrLf
        test &= m.Body & vbCrLf & vbCrLf
        Me.Log(test)
    End Sub

    Private Function GetComuneResidenzaDEM(ByVal value As String) As String
        Dim n As String() = Split(value, " ")
        Dim provincia As String = n(n.Length - 1)
        n(n.Length - 2) = RemoveRNumbers(n(n.Length - 2))
        Dim comune As String = Me.join(n, 0, n.Length - 1)
        Return comune & " (" & provincia & ")"
    End Function

    Private Function GetCAPResidenzaDEM(ByVal value As String) As String
        Dim n As String() = Split(value, " ")
        Dim comuneecap As String = n(n.Length - 2)
        Dim comune As String = RemoveRNumbers(comuneecap)

        Return Mid(comuneecap, comune.Length + 1)
    End Function

    Private Function RemoveRNumbers(ByVal value As String) As String
        Dim digitsNumbers As Char() = {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}
        Return value.TrimEnd(digitsNumbers)
    End Function

    Private Sub handleDEM(ByVal m As MailMessageEx)
        Dim a As AttachmentEx = Nothing
        For Each a1 As AttachmentEx In m.Attachments
            If LCase(System.IO.Path.GetExtension(a1.FileName)) = ".pdf" Then
                a = a1
                Exit For
            End If
        Next
        If (a Is Nothing) Then Return

        Dim doc As PDDocument = Nothing
        Dim fn As String = ""
        Try
            fn = System.IO.Path.GetTempFileName
            a.SaveToFile(fn)
            a.ContentStream.Position = 0
            doc = PDDocument.load(fn)

            Dim lst As java.util.List = doc.getDocumentCatalog().getAllPages

            For i As Integer = 0 To lst.size - 1
                Dim destName As String = System.IO.Path.GetTempFileName & ".PDF"
                Dim c As PDDocument = Me.EstraiPagina(doc, i, destName)

                Dim stripper As New PDFTextStripper()
                Dim text As String = stripper.getText(c)

                Try
                    Me.ParseTestoDEM(destName, text, m.From.Address, m.DeliveryDate)
                Catch ex As Exception
                    Me.Log(ex.Message & vbNewLine & ex.StackTrace)
                    Try
                        c.close()
                    Catch ex1 As Exception

                    End Try
                    c = Nothing
                End Try
            Next

        Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
            Throw
        Finally
            If doc IsNot Nothing Then doc.close()
            If (fn <> "") Then System.IO.File.Delete(fn)
        End Try

    End Sub

    Private Sub handleEMailSegnalazioniStay(ByVal m As MailMessageEx)
        Dim a As AttachmentEx = Nothing
        For Each a1 As AttachmentEx In m.Attachments
            If LCase(System.IO.Path.GetExtension(a1.FileName)) = ".pdf" Then
                a = a1
                Exit For
            End If
        Next
        If (a Is Nothing) Then Return

        Dim doc As PDDocument = Nothing
        Dim fn As String = ""
        Try
            fn = System.IO.Path.GetTempFileName
            a.SaveToFile(fn)
            a.ContentStream.Position = 0
            doc = PDDocument.load(fn)

            Dim lst As java.util.List = doc.getDocumentCatalog().getAllPages

            For i As Integer = 0 To lst.size - 1
                Dim destName As String = System.IO.Path.GetTempFileName & ".PDF"
                Dim c As PDDocument = Me.EstraiPagina(doc, i, destName)

                Dim stripper As New PDFTextStripper()
                Dim text As String = stripper.getText(c)

                Try
                    Me.ParseTestoSegnalazioneStay(destName, text, m.From.Address, m.DeliveryDate)
                Catch ex As Exception
                    Me.Log(ex.Message & vbNewLine & ex.StackTrace)
                    Try
                        c.close()
                    Catch ex1 As Exception

                    End Try
                    c = Nothing
                End Try
            Next

        Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
            Throw
        Finally
            If doc IsNot Nothing Then doc.close()
            If (fn <> "") Then System.IO.File.Delete(fn)
        End Try

    End Sub

    Private Function join(ByVal n() As String, ByVal from As Integer, ByVal len As Integer) As String
        Dim ret As New System.Text.StringBuilder
        For i As Integer = 0 To len - 1
            If (i > 0) Then ret.Append(" ")
            ret.Append(n(i + from))
        Next
        Return ret.ToString
    End Function


    Private Sub ParseTestoDEM(ByVal fileName As String, ByVal testo As String, ByVal sender As String, ByVal data As Date)
        Dim linee As String() = Split(testo, vbCrLf)
        Dim i As Integer = 0
        Dim col As CKeyCollection = Nothing
        Dim n() As String

        While (i < linee.Length)
            Dim linea As String = linee(i)
            Select Case linea
                Case "Data di Nascita"
                    If (col IsNot Nothing) Then
                        Dim uploadedFile As String = Upload(fileName)
                        Me.Log("Carico il PDF: " & uploadedFile)
                        col.Add("attachment", uploadedFile)

                        Me.Log("Carico la segnalazione DEM")
#If DEBUG Then

                        RPC.InvokeMethod(debugBD & "?_a=SegnalazioneDEM", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#Else
                        RPC.InvokeMethod(My.Settings.UploadToPage & "?_a=SegnalazioneDEM", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#End If
                    End If
                    col = New CKeyCollection

                    i += 1
                    If (i < linee.Length) Then
                        linea = linee(i)
                        col.Add("dataSegnalazione", Formats.ParseDate(linea))
                        i += 1
                    End If
                    If (i < linee.Length) Then
                        linea = linee(i)
                        n = Split(linea, " ")
                        col.Add("sesso", n(n.Length - 1))
                        col.Add("codiceFiscale", n(n.Length - 2))
                        col.Add("cognomeEnome", Me.join(n, 0, n.Length - 2))
                        i += 1
                    End If
                Case "Indirizzo di Residenza"
                    i += 1
                    If (i < linee.Length) Then
                        linea = linee(i)
                        col.Add("indirizzo", linea)
                        i += 1
                    End If
                    If (i < linee.Length) Then
                        linea = linee(i)
                        col.Add("numeroCellulare", linea)
                        i += 1
                    End If
                Case "Dati cliente"
                    i += 1
                Case "Recapiti telefonici: cell/tel/Email"
                    i += 1
                Case "Dati contatto"
                    i += 1
                Case "Data di riferimento"
                    i += 1
                Case "Cognome e Nome Codice Fiscale Sesso"
                    i += 1
                Case "Descrizione Segnalante"
                    i += 1
                    If (i < linee.Length) Then
                        linea = linee(i) 'PayClick
                        i += 1
                    End If
                    If (i < linee.Length) Then
                        linea = linee(i) 'DIPENDENTE PUBBLICO/STATALE
                        col.Add("tipoImpiego", linea)
                        i += 1
                    End If

                Case "Amministrazione Riferimento: P.Iva/Cod.Fisc./Ragione Sociale"
                    i += 1
                Case "Note"
                    i += 1
                    If (i < linee.Length) Then
                        linea = linee(i) 'Note
                        col.Add("note", linea)
                        i += 1
                    End If
                    If (i < linee.Length) Then
                        linea = linee(i) 'Data Nascita
                        col.Add("dataNascita", Formats.ParseDate(linea))
                        i += 1
                    End If
                Case "Luogo di Nascita"
                    i += 1
                    If (i < linee.Length) Then
                        linea = linee(i) ' 
                        col.Add("luogoNascita", Me.GetComuneResidenzaDEM(linea))
                        i += 1
                    End If
                    If (i < linee.Length) Then
                        linea = linee(i) '
                        col.Add("luogoResidenza", Me.GetComuneResidenzaDEM(linea))
                        col.Add("capResidenza", Me.GetCAPResidenzaDEM(linea))
                        i += 1
                    End If
                    If (i < linee.Length) Then
                        linea = linee(i) 'Data Nascita
                        col.Add("email", linea)
                        i += 1
                    End If
                Case Else
                    i += 1
            End Select
        End While

        If (col IsNot Nothing) Then
            Dim uploadedFile As String = Upload(fileName)
            Me.Log("Carico il PDF: " & uploadedFile)
            col.Add("attachment", uploadedFile)

            Me.Log("Carico la segnalazione DEM")
#If DEBUG Then

            RPC.InvokeMethod(debugBD & "?_a=SegnalazioneDEM", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#Else
                    RPC.InvokeMethod(My.Settings.UploadToPage & "?_a=SegnalazioneDEM", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#End If
        End If

    End Sub


    Private Sub ParseTestoSegnalazioneStay(ByVal fileName As String, ByVal testo As String, ByVal sender As String, ByVal data As Date)
        Dim linee As String() = Split(testo, vbCrLf)
        Dim i As Integer = 0
        Dim j, k As Integer
        Dim n() As String
        Dim fonte As IFonte = Anagrafica.Fonti.GetItemByName("Lista", "Lista", "Segnalazione Stay Richiesta CE")

        While (i < linee.Length)
            Dim linea As String = linee(i)
            Dim dataInvioConteggio As Date? = Nothing
            Dim numeroPratica As String = ""
            Dim numeroRate As Integer? = Nothing
            Dim importoRata As Decimal? = Nothing
            Dim dataDecorrenza As Date? = Nothing
            Dim dataScadenza As Date? = Nothing
            Dim dataRichiestaConteggio As Date? = Nothing
            Dim importoCE As Decimal? = Nothing
            Dim dataNascita As Date? = Nothing
            Dim codiceFiscale As String = ""
            Dim cognomeEnome As String = ""
            Dim indirizzoDaRichiestaVia As String = ""
            Dim indirizzoDaRichiestaCAP As String = ""
            Dim indirizzoDaRichiestaComune As String = ""
            Dim indirizzoDaRichiestaProvincia As String = ""
            Dim numeroCellulare As String = ""
            Dim numeroFisso As String = ""
            Dim richiedente As String = ""
            Dim codiceAmministrazione As String = ""
            Dim nomeAmministrazione As String = ""
            Dim note As String = ""
            Dim tan As Double? = Nothing
            Dim taeg As Double? = Nothing


            If (linea = "Report iniziativa " & Chr(34) & "Stay" & Chr(34)) Then 'Nuova pagina
                i += 1
                linea = linee(i)
                If (linea = "FIN.SE.A. SRL 343382") Then
                    '17/02/2017             
                    i += 1
                    linea = linee(i)
                    dataInvioConteggio = Formats.ParseDate(linea)

                    i += 1
                    linea = linee(i)
                    '1038665 108 € 260,00 01/08/2013 01/08/2022
                    n = Split(linea, " ")
                    numeroPratica = n(0)
                    numeroRate = Formats.ParseInteger(n(1))
                    'n(2) = €
                    importoRata = Formats.ParseValuta(n(3))
                    dataDecorrenza = Formats.ParseDate(n(4))
                    dataScadenza = Formats.ParseDate(n(5))

                    '10/02/2017 € 13.468,60
                    i += 1
                    linea = linee(i)
                    n = Split(linea)
                    dataRichiestaConteggio = Formats.ParseDate(n(0))
                    'n(1) = €
                    importoCE = Formats.ParseValuta(n(2))

                    'MITRIONE ANGELO MTRNGL45M18A881H 18/08/1945
                    i += 1
                    linea = linee(i)
                    n = Split(linea, " ")
                    dataNascita = Formats.ParseDate(n(n.Length - 1))
                    codiceFiscale = Formats.ParseCodiceFiscale(n(n.Length - 2))
                    For j = 0 To n.Length - 3
                        If (cognomeEnome <> "") Then cognomeEnome = cognomeEnome & " "
                        cognomeEnome &= n(j)
                    Next

                    'VIA PANORAMICA 3 83044 BISACCIA (AV)
                    i += 1
                    linea = linee(i)
                    n = Split(linea)
                    indirizzoDaRichiestaProvincia = n(n.Length - 1) : indirizzoDaRichiestaProvincia = Mid(indirizzoDaRichiestaProvincia, 2, Len(indirizzoDaRichiestaProvincia) - 2)
                    j = n.Length - 2
                    While j > 0 AndAlso Not Luoghi.Comuni.IsValidCAP(n(j))
                        j -= 1
                    End While
                    indirizzoDaRichiestaCAP = n(j)

                    For k = j + 1 To n.Length - 2
                        If (indirizzoDaRichiestaComune <> "") Then indirizzoDaRichiestaComune &= " "
                        indirizzoDaRichiestaComune &= n(k)
                    Next

                    For k = 0 To j - 1
                        If (indirizzoDaRichiestaVia <> "") Then indirizzoDaRichiestaVia &= " "
                        indirizzoDaRichiestaVia &= n(k)
                    Next

                    '339 4503205
                    i += 1
                    linea = linee(i)

                    Dim numeri As String() = Me.ParseNumeri(linea)
                    If (Arrays.Len(numeri) > 1) Then
                        Debug.Print("Numeri: " & linea)
                    End If

                    'For nk As Integer = 0 To Arrays.Len(numeri) - 1
                    If (Arrays.Len(numeri) > 0) Then
                        numeroCellulare = Formats.ParsePhoneNumber(numeri(0))
                    End If

                    If (Arrays.Len(numeri) > 1) Then
                        numeroFisso = Formats.ParsePhoneNumber(numeri(1))
                    End If

                    'Next


                    Do
                        i += 1
                        linea = linee(i)
                    Loop While (i < linee.Length) AndAlso Not (linea = "Richiedente CE")

                    'FINSERVICE DI MONTUORI FRANCESCO 
                    i += 1
                    linea = linee(i)
                    richiedente = linea

                    '02121151001 INPS - NUOVA CONVENZIONE
                    i += 1
                    linea = linee(i)

                    j = InStr(linea, " ")
                    If (j > 0) Then
                        codiceAmministrazione = Trim(Left(linea, j - 1))
                        nomeAmministrazione = Trim(Mid(linea, j + 1))
                    Else
                        codiceAmministrazione = ""
                        nomeAmministrazione = linea
                    End If


                    'Amministrazione Riferimento: P.Iva/Ragione Sociale
                    i += 1
                    linea = linee(i)

                    'Note per finalità commerciali
                    i += 1
                    linea = linee(i)

                    'Cliente da non contattare per finalità commerciali
                    i += 1
                    linea = linee(i)
                    note = linea

                    ''8,61
                    'i += 1
                    'linea = linee(i)
                    'tan = Formats.ParseDouble(linea)

                    'i += 1
                    'linea = linee(i)
                    ''TAN

                    ''10,92
                    'i += 1
                    'linea = linee(i)
                    'taeg = Formats.ParseDouble(linea)

                    'i += 1
                    'linea = linee(i)
                    ''TAEG

                    ''....

                    While (i < linee.Length) AndAlso (linea <> "Report iniziativa " & Chr(34) & "Stay" & Chr(34))
                        linea = linee(i)
                        Select Case linea
                            Case "TAN" : tan = Formats.ParseDouble(linee(i - 1))
                            Case "TAEG" : taeg = Formats.ParseDouble(linee(i - 1))
                        End Select
                        i += 1
                    End While
                    If (linea = "Report iniziativa " & Chr(34) & "Stay" & Chr(34)) Then i -= 1

                    Dim col As New CKeyCollection
                    col.Add("dataInvioConteggio", dataInvioConteggio)
                    col.Add("numeroPratica", numeroPratica)
                    col.Add("numeroRate", numeroRate)
                    col.Add("importoRata", importoRata)
                    col.Add("dataDecorrenza", dataDecorrenza)
                    col.Add("dataScadenza", dataScadenza)
                    col.Add("dataRichiestaConteggio", dataRichiestaConteggio)
                    col.Add("importoCE", importoCE)
                    col.Add("dataNascita", dataNascita)
                    col.Add("codiceFiscale", codiceFiscale)
                    col.Add("cognomeEnome", cognomeEnome)
                    col.Add("indirizzoDaRichiestaVia", indirizzoDaRichiestaVia)
                    col.Add("indirizzoDaRichiestaCAP", indirizzoDaRichiestaCAP)
                    col.Add("indirizzoDaRichiestaComune", indirizzoDaRichiestaComune)
                    col.Add("indirizzoDaRichiestaProvincia", indirizzoDaRichiestaProvincia)
                    col.Add("numeroCellulare", numeroCellulare)
                    col.Add("numeroFisso", numeroFisso)
                    col.Add("richiedente", richiedente)
                    col.Add("codiceAmministrazione", nomeAmministrazione)
                    col.Add("nomeAmministrazione", nomeAmministrazione)
                    col.Add("note", note)
                    col.Add("tan", tan)
                    col.Add("taeg", taeg)

                    Dim uploadedFile As String = Upload(fileName)
                    Me.Log("Carico il PDF: " & uploadedFile)
                    col.Add("attachment", uploadedFile)

                    Me.Log("Carico la segnalazione " & codiceFiscale & " - " & numeroPratica)
#If DEBUG Then

                    RPC.InvokeMethod(debugBD & "?_a=SegnalazioneStay", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#Else
                    RPC.InvokeMethod(My.Settings.UploadToPage & "?_a=SegnalazioneStay", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#End If

                End If
            Else
                i += 1
            End If
        End While

    End Sub

    Private Function ParseNumeri(ByVal linea As String) As String()
        linea = Trim(linea)
        Dim n() As String = Split(linea, " ")
        Dim ret As New System.Collections.ArrayList
        Dim numero As String = ""
        For i As Integer = 0 To Arrays.Len(n) - 1
            numero &= n(i)
            If (Len(n(i)) > 4) Then
                ret.Add(numero)
                numero = ""
            End If
        Next
        Return ret.ToArray(GetType(String))
    End Function

    Public Shared Function Upload(ByVal fileName As String) As String
        Dim info As New System.IO.FileInfo(fileName)
        Dim data As Date = info.CreationTime
        Dim tmpName As String = System.IO.Path.GetTempFileName
        System.IO.File.Copy(fileName, tmpName, True)
#If DEBUG Then
        Dim url As String = "http://localhost:33016/widgets/websvc/Uploader1.aspx?f=" & fileName & ".pdf"  '?p=" & My.Computer.Name & "&u=" & userName & "&f=" & fileName & "&d=" & DMD.Sistema.RPC.date2n(data)
#Else
        Dim url As String = "https://areariservata.DMD.net/widgets/websvc/Uploader1.aspx?f=" & fileName & ".pdf" '?p=" & My.Computer.Name & "&u=" & userName & "&f=" & fileName & "&d=" & DMD.Sistema.RPC.date2n(data)
#End If
        Dim nvc As New NameValueCollection()
        'nvc.Add("id", "TTR")
        nvc.Add("File1", "Upload")
        Dim ret As String = RPC.HttpUploadFile(url, tmpName, "file", "text/pdf", nvc)
        System.IO.File.Delete(tmpName)
        Return ret
    End Function

    Private Function EstraiPagina(ByVal doc As PDDocument, ByVal pageIndex As Integer, ByVal destFileName As String) As PDDocument
        doc.save(destFileName)

        doc = PDDocument.load(destFileName)
        While (doc.getPageCount() > 0 AndAlso pageIndex > 0)
            doc.removePage(0)
            pageIndex -= 1
        End While

        While (doc.getPageCount() >= 2)
            doc.removePage(1)
        End While
        doc.save(destFileName)

        Return doc
        'PDPageContentStream contentStream = New PDPageContentStream(Buffer, page);

        'PDFont font = PDType1Font.HELVETICA_BOLD;
        'contentStream.beginText();
        'contentStream.setNonStrokingColor(Color.White); // !!!!!!
        'contentStream.setFont(font, 6);
        'contentStream.newLineAtOffset(100, 700);
        'contentStream.showText("Empty page");
        'contentStream.endText();
        'contentStream.close();
        '// Close the buffer document, if i comment it out the exception Is gone
        'Buffer.close();
        '// Add the blank page
        'c.addPage(page);


    End Function


    Sub UnhandledExceptionHandler(ByVal e As Exception)
        'Dim remoteIP As String = "<local>"
        'Dim remotePort As Integer = 0
        'Dim userAgent As String = ""
        Dim html As String = "" & vbNewLine
        html &= "------------------------------------------------------" & vbNewLine
        html &= "ERRORE NON GESTITO" & vbNewLine
        html &= e.Message & vbNewLine
        html &= e.StackTrace & vbNewLine
        html &= "------------------------------------------------------" & vbNewLine

        If (e.InnerException IsNot Nothing) Then
            html &= e.InnerException.Message & vbNewLine
            html &= e.InnerException.StackTrace & vbNewLine
        End If

        html &= "------------------------------------------------------" & vbNewLine

        Me.Log(html)
    End Sub



    Public Sub [Stop]() Implements IApplicationContext.Stop

    End Sub

    Public Property CurrentUfficio As CUfficio Implements IApplicationContext.CurrentUfficio
        Get
            Return Me.m_Ufficio
        End Get
        Set(value As CUfficio)
            Me.m_Ufficio = value
        End Set
    End Property

    Public Property CurrentLogin As CLoginHistory Implements IApplicationContext.CurrentLogin
        Get
            Return Me.m_CurrentLogin
        End Get
        Set(value As CLoginHistory)
            Me.m_CurrentLogin = value
        End Set
    End Property

    Public Function GetProperty(name As String) As Object Implements IApplicationContext.GetProperty
        Return ""
    End Function

    Public Sub handleFaxReceived(ByVal sender As Object, ByVal e As FaxReceivedEventArgs)
        Dim job As FaxJob = e.Job
        MsgBox("Nuovo fax ricevuto da " & job.Options.SenderName)
    End Sub

    Public Sub handleSMSReceived(ByVal sender As Object, ByVal e As SMSReceivedEventArgs)

    End Sub

    Public Sub EnterMaintenance() Implements IApplicationContext.EnterMaintenance

    End Sub

    Public Function IsMaintenance() As Boolean Implements IApplicationContext.IsMaintenance
        Return False
    End Function

    Public Sub QuitMaintenance() Implements IApplicationContext.QuitMaintenance

    End Sub

    Public Sub Log(message As String) Implements IApplicationContext.Log
        frmMain.Log(message)
    End Sub




#Region "Stay Maggio 2017"

    Private Sub handleStay201705(ByVal m As MailMessageEx)
        Dim a As AttachmentEx = Nothing
        For Each a1 As AttachmentEx In m.Attachments
            If LCase(System.IO.Path.GetExtension(a1.FileName)) = ".pdf" Then
                a = a1
                Exit For
            End If
        Next
        If (a Is Nothing) Then Return

        Dim doc As PDDocument = Nothing
        Dim fn As String = ""

#If Not DEBUG Then
        Try
#End If
        fn = System.IO.Path.GetTempFileName
        a.SaveToFile(fn)
        a.ContentStream.Position = 0
        doc = PDDocument.load(fn)

        Dim lst As java.util.List = doc.getDocumentCatalog().getAllPages

        For i As Integer = 0 To lst.size - 1
            Dim destName As String = System.IO.Path.GetTempFileName & ".PDF"
            Dim c As PDDocument = Me.EstraiPagina(doc, i, destName)

            Dim stripper As New PDFTextStripper()
            Dim text As String = stripper.getText(c)

#If Not DEBUG Then
            Try
#End If
            Dim files() As String = PDFUtils1.SplitInto2PDF(destName, 0)


            Me.ParseStay201705(text, m.From.Address, m.DeliveryDate, files)

            PDFUtils1.DeleteFile(files(0))
            PDFUtils1.DeleteFile(files(1))

#If Not DEBUG Then
            Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
            Finally
#End If
            Try
                c.close()
            Catch ex1 As Exception

            End Try
            c = Nothing
#If Not DEBUG Then
            End Try
#End If
        Next

#If Not DEBUG Then
        Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
            Throw
        Finally

#End If
        If doc IsNot Nothing Then doc.close()
        If (fn <> "") Then System.IO.File.Delete(fn)

#If Not DEBUG Then
        End Try
#End If

    End Sub

    Private Sub ParseStay201705(ByVal testo As String, ByVal sender As String, ByVal data As Date, ByVal files() As String)
        Const cmdNumeroPratica As String = "Numero pratica"
        Const cmdDurata As String = "Durata"
        Const cmdImportoRata As String = "Importo rata €"
        Const cmdCognome As String = "Cognome"
        Const cmdNome As String = "Nome"
        Const cmdCodiceFiscale As String = "Codice Fiscale"
        Const cmdDataDecorrenza As String = "Data decorrenza"
        Const cmdDataUltimaScadenza As String = "Ultima scadenza"
        Const cmdTan As String = "TAN"
        Const cmdTaeg As String = "TAEG"
        Const cmdDataNascita As String = "Data Nascita"
        Const cmdDataInvio As String = "Data invio"
        Const cmdSaldoResiduo As String = "Saldo residuo €"
        Const cmdRichiedente As String = "Richiedente"
        Const cmdDataRichiesta As String = "Data richiesta"
        Const cmdTel1 As String = "Tel 1"
        Const cmdTel2 As String = "Tel 2"
        Const cmdTel3 As String = "Tel 3"
        Const cmdTel4 As String = "Tel 4"
        Const cmdATCPIVA As String = "ATC PIva"
        Const cmdATCDESC As String = "ATC Descrizione"
        Const cmdIndirizzoOCS As String = "Indirizzo"

        Dim pFile As Integer = 0
        Dim linee As String() = Split(testo, vbCrLf)
        Dim i As Integer = 0
        Dim j As Integer
        Dim fonte As IFonte = Anagrafica.Fonti.GetItemByName("Lista", "Lista", "Segnalazione Stay Richiesta CE")
        Dim statoParser As String = ""

        Dim dataInvioConteggio As Date? = Nothing
        Dim numeroPratica As String = ""
        Dim numeroRate As Integer? = Nothing
        Dim importoRata As Decimal? = Nothing
        Dim dataDecorrenza As Date? = Nothing
        Dim dataScadenza As Date? = Nothing
        Dim dataRichiestaConteggio As Date? = Nothing
        Dim importoCE As Decimal? = Nothing
        Dim dataNascita As Date? = Nothing
        Dim codiceFiscale As String = ""
        Dim cognome As String = ""
        Dim nome As String = ""
        Dim indirizzoDaRichiestaVia As String = ""
        Dim indirizzoDaRichiestaCAP As String = ""
        Dim indirizzoDaRichiestaComune As String = ""
        Dim indirizzoDaRichiestaProvincia As String = ""
        Dim tel1 As String = ""
        Dim tel2 As String = ""
        Dim tel3 As String = ""
        Dim tel4 As String = ""
        Dim richiedente As String = ""
        Dim codiceAmministrazione As String = ""
        Dim nomeAmministrazione As String = ""
        Dim note As String = ""
        Dim tan As Double? = Nothing
        Dim taeg As Double? = Nothing

        While (i < linee.Length)
            Dim linea As String = Trim(linee(i))



            Select Case statoParser
                Case ""
                    Select Case linea
                        Case "DATI PRATICA"
                            statoParser = linea

                    End Select

                    i += 1

                Case "DATI PRATICA"
                    Select Case linea
                        Case "DATI CONTEGGIO"
                            statoParser = linea
                            i += 1
                        Case Else
                            If linea.StartsWith(cmdNumeroPratica) Then numeroPratica = Trim(linea.Substring(cmdNumeroPratica.Length + 1))
                            If linea.StartsWith(cmdDurata) Then numeroRate = Formats.ParseInteger(Trim(linea.Substring(cmdDurata.Length + 1)))
                            If linea.StartsWith(cmdImportoRata) Then importoRata = Formats.ParseValuta(Trim(linea.Substring(cmdImportoRata.Length + 1)))
                            If linea.StartsWith(cmdCognome) Then cognome = Trim(linea.Substring(cmdCognome.Length + 1))
                            If linea.StartsWith(cmdNome) Then nome = Trim(linea.Substring(cmdNome.Length + 1))
                            If linea.StartsWith(cmdCodiceFiscale) Then codiceFiscale = Trim(linea.Substring(cmdCodiceFiscale.Length + 1))
                            If linea.StartsWith(cmdDataDecorrenza) Then dataDecorrenza = Formats.ParseDate(linea.Substring(cmdDataDecorrenza.Length + 1))
                            If linea.StartsWith(cmdDataUltimaScadenza) Then dataScadenza = Formats.ParseDate(linea.Substring(cmdDataUltimaScadenza.Length + 1))
                            If linea.StartsWith(cmdTan) Then tan = Formats.ParseDouble(linea.Substring(cmdTan.Length + 1))
                            If linea.StartsWith(cmdTaeg) Then taeg = Formats.ParseDouble(linea.Substring(cmdTaeg.Length + 1))
                            If linea.StartsWith(cmdDataNascita) Then dataNascita = Formats.ParseDate(linea.Substring(cmdDataNascita.Length + 1))
                            i += 1
                    End Select

                Case "DATI CONTEGGIO"
                    Select Case linea
                        Case "DATI CLIENTE"
                            statoParser = linea
                            i += 1
                        Case Else
                            If linea.StartsWith(cmdDataInvio) Then
                                If (linea.Length > cmdDataInvio.Length + 1) Then
                                    dataInvioConteggio = Formats.ParseDate(linea.Substring(cmdDataInvio.Length + 1))
                                Else
                                    Debug.Print("oops")
                                End If
                            End If

                            If linea.StartsWith(cmdSaldoResiduo) AndAlso linea.Length > cmdSaldoResiduo.Length + 1 Then importoCE = Formats.ParseValuta(linea.Substring(cmdSaldoResiduo.Length + 1))
                            If linea.StartsWith(cmdRichiedente) AndAlso linea.Length > cmdRichiedente.Length + 1 Then
                                richiedente = Trim(linea.Substring(cmdRichiedente.Length + 1))
                                While (i + 1 < linee.Length AndAlso Not linee(i + 1).StartsWith(cmdDataRichiesta))
                                    richiedente &= " " & linee(i + 1)
                                    i += 1
                                End While
                            End If
                            If linea.StartsWith(cmdDataRichiesta) AndAlso linea.Length > cmdDataRichiesta.Length + 1 Then dataRichiestaConteggio = Formats.ParseDate(linea.Substring(cmdDataRichiesta.Length + 1))
                            i += 1
                    End Select

                Case "DATI CLIENTE"
                    Select Case linea
                        Case "NOTE"
                            statoParser = ""
                            i += 1

                            Dim col As New CKeyCollection
                            col.Add("dataInvioConteggio", dataInvioConteggio)
                            col.Add("numeroPratica", numeroPratica)
                            col.Add("numeroRate", numeroRate)
                            col.Add("importoRata", importoRata)
                            col.Add("dataDecorrenza", dataDecorrenza)
                            col.Add("dataScadenza", dataScadenza)
                            col.Add("dataRichiestaConteggio", dataRichiestaConteggio)
                            col.Add("importoCE", importoCE)
                            col.Add("dataNascita", dataNascita)
                            col.Add("codiceFiscale", codiceFiscale)
                            col.Add("cognome", cognome)
                            col.Add("nome", nome)
                            col.Add("indirizzoDaRichiestaVia", indirizzoDaRichiestaVia)
                            col.Add("indirizzoDaRichiestaCAP", indirizzoDaRichiestaCAP)
                            col.Add("indirizzoDaRichiestaComune", indirizzoDaRichiestaComune)
                            col.Add("indirizzoDaRichiestaProvincia", indirizzoDaRichiestaProvincia)
                            col.Add("tel1", tel1)
                            col.Add("tel2", tel2)
                            col.Add("tel3", tel3)
                            col.Add("tel4", tel4)
                            col.Add("richiedente", richiedente)
                            col.Add("codiceAmministrazione", nomeAmministrazione)
                            col.Add("nomeAmministrazione", nomeAmministrazione)
                            col.Add("note", note)
                            col.Add("tan", tan)
                            col.Add("taeg", taeg)

                            Me.Log("Carico il PDF: " & files(pFile))
                            Dim uploadedFile As String = Upload(files(pFile)) : pFile += 1
                            Me.Log("PDF caricato: " & uploadedFile)
                            col.Add("attachment", uploadedFile)

                            Me.Log("Carico la segnalazione " & codiceFiscale & " - " & numeroPratica)
#If DEBUG Then

                            RPC.InvokeMethod(debugBD & "?_a=SegnalazioneStay20705", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#Else
RPC.InvokeMethod(My.Settings.UploadToPage & "?_a=SegnalazioneStay20705", "sender", RPC.str2n(sender), "data", RPC.date2n(data), "col", RPC.str2n(XML.Utils.Serializer.Serialize(col)))
#End If

                        Case Else
                            If linea.StartsWith(cmdTel1) Then tel1 = Trim(linea.Substring(cmdTel1.Length))
                            If linea.StartsWith(cmdTel2) Then tel2 = Trim(linea.Substring(cmdTel2.Length))
                            If linea.StartsWith(cmdTel3) Then tel3 = Trim(linea.Substring(cmdTel3.Length))
                            If linea.StartsWith(cmdTel4) Then tel4 = Trim(linea.Substring(cmdTel4.Length))
                            If linea.StartsWith(cmdATCPIVA) Then codiceAmministrazione = Trim(linea.Substring(cmdATCPIVA.Length + 1))
                            If linea.StartsWith(cmdATCDESC) Then nomeAmministrazione = Trim(linea.Substring(cmdATCDESC.Length + 1))
                            If linea.EndsWith(cmdIndirizzoOCS) AndAlso linee(i + 1) = "OCS" Then
                                indirizzoDaRichiestaVia = linea.Substring(0, linea.Length - cmdIndirizzoOCS.Length)
                                i += 1
                                linea = linee(i)
                                If (linea <> "OCS") Then
                                    Sistema.Events.NotifyUnhandledException(New Exception("Indirizzo OCS non valido (OCS)"))
                                End If
                                i += 1
                                linea = linee(i)
                                j = linea.IndexOf(" ")
                                If (j > 0) Then
                                    indirizzoDaRichiestaCAP = linea.Substring(0, j)
                                    indirizzoDaRichiestaComune = linea.Substring(j + 1)
                                    indirizzoDaRichiestaProvincia = Luoghi.GetProvincia(indirizzoDaRichiestaComune)
                                    indirizzoDaRichiestaComune = Luoghi.GetComune(indirizzoDaRichiestaComune)
                                Else
                                    Sistema.Events.NotifyUnhandledException(New Exception("Indirizzo OCS non valido (CAP)"))
                                End If
                            End If

                            If linea.EndsWith(cmdIndirizzoOCS) AndAlso linee(i + 1) = "CE" Then
                                i += 1
                            End If
                            i += 1

                    End Select


            End Select








        End While

    End Sub

#End Region


End Class

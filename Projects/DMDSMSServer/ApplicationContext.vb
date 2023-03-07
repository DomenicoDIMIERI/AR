Imports Microsoft.VisualBasic
Imports FinSeA
Imports FinSeA.Sistema
Imports FinSeA.Databases
Imports FinSeA.Anagrafica
Imports FinSeA.Drivers

Public Class ApplicationContext
    Implements FinSeA.Sistema.IApplicationContext

    Private Shared m_Settings As New CKeyCollection
    Private Shared m_CurrentUser As CUser
    Private Shared m_CurrentSession As Object
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
            Return Application.StartupPath
        End Get
    End Property

    Public ReadOnly Property UserDataFolder As String Implements IApplicationContext.UserDataFolder
        Get
            Return Application.StartupPath
        End Get
    End Property

    Public Property CurrentUser As Sistema.CUser Implements FinSeA.Sistema.IApplicationContext.CurrentUser
        Get
            If (m_CurrentUser Is Nothing) Then m_CurrentUser = New CUser
            Return m_CurrentUser
        End Get
        Set(value As Sistema.CUser)
            m_CurrentUser = value
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
        Select Case paramName
            Case "ID" : Return "27" '"1347956004"
            Case "params" : Return "<?xml version=""1.0""?><CRapportiniCursor xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><m_Token></m_Token><m_PageNum>-1</m_PageNum><m_Index>-1</m_Index><m_NumItems>-1</m_NumItems><m_Fields><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Autore</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CQS_PD</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Cessionario</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>T</m_IsSet><m_FieldName>CodiceFiscale</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CognomeCliente</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Commerciale</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CreatoDa</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>CreatoIl</m_FieldName><m_DataType>7</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>DaVedere</m_FieldName><m_DataType>11</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>1</m_SortOrder><m_SortPriority>100</m_SortPriority><m_Value>F</m_Value><m_Value1>F</m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>FonteContatto</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>T</m_IsSet><m_FieldName>IDPuntoOperativo</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value>3</m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ID</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>LCase(Trim([NomeCliente] &amp; &#39; &#39; &amp; [CognomeCliente]))</m_FieldName><m_DataType>130</m_DataType><m_Operator>3</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>1</m_SortOrder><m_SortPriority>98</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ModificatoDa</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ModificatoIl</m_FieldName><m_DataType>7</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NatoAComune</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NatoAProvincia</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NatoIl</m_FieldName><m_DataType>7</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NettoRicavo</m_FieldName><m_DataType>6</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeCessionario</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeCliente</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeCommerciale</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeOperatore</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeProduttore</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomeProfilo</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NomePuntoOperativo</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>NumeroRate</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>PartitaIVA</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Prodotto</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>Produttore</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteACAP</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteACivico</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteAComune</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteAProvincia</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>ResidenteAVia</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>StatoPratica</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>1</m_SortOrder><m_SortPriority>99</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>T</m_IsSet><m_FieldName>Stato</m_FieldName><m_DataType>3</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>0</m_SortPriority><m_Value>1</m_Value><m_Value1></m_Value1></CCursorField><CCursorField><m_IsSet>F</m_IsSet><m_FieldName>TipoFonteContatto</m_FieldName><m_DataType>130</m_DataType><m_Operator>0</m_Operator><m_IncludeNulls>F</m_IncludeNulls><m_SortOrder>0</m_SortOrder><m_SortPriority>-1</m_SortPriority><m_Value></m_Value><m_Value1></m_Value1></CCursorField></m_Fields><m_IgnoreRights>F</m_IgnoreRights><m_PageSize>25</m_PageSize><m_Items></m_Items><m_DataInserimento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataInserimento><m_DataRichiestaDelibera><CIntervalloData><m_Tipo></m_Tipo><m_Inizio></m_Inizio><m_Fine></m_Fine></CIntervalloData></m_DataRichiestaDelibera><m_DataDelibera><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataDelibera><m_DataCaricamento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataCaricamento><m_DataLiquidazione><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataLiquidazione><m_DataArchiviazione><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataArchiviazione><m_DataAnnullamento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataAnnullamento><m_DataTrasferimento><CIntervalloData><m_Tipo></m_Tipo><m_Inizio>1901/1/1 0.0.0</m_Inizio><m_Fine>1901/1/1 0.0.0</m_Fine></CIntervalloData></m_DataTrasferimento></CRapportiniCursor>"
            Case "tn" : Return "CRapportiniCursor"
            Case Else : Return vbNullString
        End Select
    End Function

    Public Function GetParameter(Of T)(paramName As String, Optional defValue As Object = Nothing) As T Implements IApplicationContext.GetParameter
        Return Types.CastTo(Me.GetParameter(paramName), Type.GetTypeCode(GetType(T)))
    End Function

    Public Property CurrentSession As Object Implements IApplicationContext.CurrentSession
        Get
            Return m_CurrentSession
        End Get
        Set(value As Object)
            m_CurrentSession = value
        End Set
    End Property

    Public Function MapPath(path As String) As String Implements IApplicationContext.MapPath
        Return path
    End Function

    Public Function UnMapPath(path As String) As String Implements IApplicationContext.UnMapPath
        Return path
    End Function

    Public Function IsUserLogged(user As CUser) As Boolean Implements IApplicationContext.IsUserLogged
        Return GetID(user) = GetID(m_CurrentUser)
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
            Return Anagrafica.Aziende.Module.Settings.GetValueInt("AziendaPrincipale", 0)
        End Get
        Set(value As Integer)
            Anagrafica.Aziende.Module.Settings.SetValueInt("AziendaPrincipale", value)
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
        Sistema.Types.Imports.Add("FinSeA")
        Sistema.Types.Imports.Add("FinSeA.Sistema")
        Sistema.Types.Imports.Add("FinSeA.FileSystem")
        Sistema.Types.Imports.Add("FinSeA.Databases")
        Sistema.Types.Imports.Add("FinSeA.Calendar")
        Sistema.Types.Imports.Add("FinSeA.Collegamenti")
        Sistema.Types.Imports.Add("FinSeA.Comunicazioni")
        Sistema.Types.Imports.Add("FinSeA.Appuntamenti")
        Sistema.Types.Imports.Add("FinSeA.GDE")
        Sistema.Types.Imports.Add("FinSeA.Luoghi")
        Sistema.Types.Imports.Add("FinSeA.Anagrafica")
        Sistema.Types.Imports.Add("FinSeA.CustomerCalls")
        Sistema.Types.Imports.Add("FinSeA.CQSPD")
        Sistema.Types.Imports.Add("FinSeA.Forms")
        Sistema.Types.Imports.Add("FinSeA.Messenger")
        Sistema.Types.Imports.Add("FinSeA.Web")
        Sistema.Types.Imports.Add("FinSeA.Web.WebSite")
        Sistema.Types.Imports.Add("FinSeA.Visite")
        Sistema.Types.Imports.Add("FinSeA.ADV")
        Sistema.Types.Imports.Add("FinSeA.Anagrafica+Fonti")
        Sistema.Types.Imports.Add("FinSeA.Office")
        Sistema.Types.Imports.Add("FinSeA.Mail")
        Sistema.Types.Imports.Add("FinSeA.Tickets")

        'Sistema.Types.NewTypeHandlers.Add(New NewTypeHandlers)

        'FinSeA.Databases.APPConn.Path = "C:\Users\Domenico.DiMieri\Documents\Visual Studio 2012\WebSites\WebSite1\mdb-database\sitedb.mdb"
        'FinSeA.Databases.APPConn.OpenDB()

        'FinSeA.Databases.LOGConn.Path = "C:\Users\Domenico.DiMieri\Documents\Visual Studio 2012\WebSites\WebSite1\mdb-database\log.mdb"
        'FinSeA.Databases.LOGConn.OpenDB()

        'Try
        '    Dim smsDriver As New FinSeA.Drivers.TrendoSMSDriver
        '    smsDriver.UserName = "amministrazione@finsea.net"
        '    smsDriver.Password = "fjFfUqUj"

        '    FinSeA.Sistema.SMSService.InstallDriver(smsDriver)

        '    AddHandler Sistema.SMSService.SMSReceived, AddressOf Me.handleSMSReceived
        'Catch aex As ArgumentException
        '    'Se il driver è già installato non fa niente
        'Catch ex As Exception
        '    FinSeA.Sistema.Events.NotifyUnhandledException(ex)
        '    Throw
        'End Try

        'Try
        '    Dim faxDriver As FinSeA.Sistema.BaseFaxDriver
        '    faxDriver = New FinSeA.Drivers.MessageNetFaxDriver
        '    FinSeA.Sistema.FaxService.InstallDriver(faxDriver)

        '    faxDriver = New HylaFaxDriver '("95.225.118.59")
        '    FinSeA.Sistema.FaxService.InstallDriver(faxDriver)
        '    'With DirectCast(faxDriver, HylaFaxDriver)
        '    '    .Accounts.Add(New CEmailAccount("av.fax@finsea.net", ""))
        '    '    .Accounts.Add(New CEmailAccount("bn.fax@finsea.net", ""))
        '    '    .Accounts.Add(New CEmailAccount("sa.fax@finsea.net", ""))
        '    '    .Accounts.Add(New CEmailAccount("tegg.fax@finsea.net", ""))
        '    '    .Connect()
        '    'End With
        '    faxDriver.Connect()

        '    AddHandler Sistema.FaxService.FaxReceived, AddressOf Me.handleFaxReceived
        'Catch aex As ArgumentException
        '    'Se il driver è già installato non fa niente
        'Catch ex As Exception
        '    FinSeA.Sistema.Events.NotifyUnhandledException(ex)
        '    Throw
        'End Try
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

    End Sub
End Class

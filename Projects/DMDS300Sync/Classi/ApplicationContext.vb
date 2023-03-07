Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Drivers

Public Class MyApplicationContext
    Implements DMD.Sistema.IApplicationContext

    Private Shared mdb As CDBConnection

    Private Shared m_Settings As New CKeyCollection
    'Private Shared m_CurrentUser As CUser
    'Private Shared m_CurrentSession As Object
    Private m_ID As String = ASPSecurity.GetRandomKey(25)
    Private m_Ufficio As CUfficio
    Private m_CurrentLogin As CLoginHistory

    Private Shared Sub CheckDB()
        Dim fn As String = GetDBFileName()
        DMD.Sistema.FileSystem.CreateRecursiveFolder(DMD.Sistema.FileSystem.GetFolderName(fn))
        If Not System.IO.File.Exists(fn) Then
            System.IO.File.Copy(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "templates\anvizdb.mdb"), fn)
        End If
        mdb = New CMdbDBConnection(fn)
        mdb.OpenDB()
    End Sub


    Private Shared Function GetDBFileName() As String
        Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DMDS300Sync\anvizdb.mdb")
    End Function

    Public ReadOnly Property Description As String Implements IApplicationContext.Description
        Get
            Return "Fin.Se.A. S300 Sincronizzatore"
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
            Return My.Application.Info.DirectoryPath
        End Get
    End Property

    Public ReadOnly Property TmporaryFolder As String Implements IApplicationContext.TmporaryFolder
        Get
            Return System.IO.Path.GetTempPath
        End Get
    End Property

    Public ReadOnly Property SystemDataFolder As String Implements IApplicationContext.SystemDataFolder
        Get
            Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DMDS300Sync")
        End Get
    End Property

    Public ReadOnly Property UserDataFolder As String Implements IApplicationContext.UserDataFolder
        Get
            Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DMDS300Sync")
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

        CheckDB()

        DMD.Databases.APPConn = mdb
        DMD.Databases.LOGConn = mdb

        UploadWorker.Start()
        DownloadWorker.Start()


    End Sub

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
        UploadWorker.Stop()
        DownloadWorker.Stop()
        DMD.Databases.APPConn.CloseDB()
        'DMD.Databases.LOGConn = mdb
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
        Try
            SyncLock Me
                Dim text As String = Formats.FormatUserDateTime(Now) & " -> " & message & vbNewLine
                DMD.Sistema.FileSystem.CreateRecursiveFolder(Me.SystemDataFolder)
                System.IO.File.AppendAllText(System.IO.Path.Combine(Me.SystemDataFolder, "dmds300s.log"), text)
            End SyncLock
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Shared m_Instance As MyApplicationContext = Nothing

    Public Shared ReadOnly Property Instance As MyApplicationContext
        Get
            If (m_Instance Is Nothing) Then m_Instance = New MyApplicationContext
            Return m_Instance
        End Get
    End Property


End Class

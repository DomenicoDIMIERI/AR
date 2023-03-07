Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Flags> _
    Public Enum ImportExportSourceFlags As Integer
        None = 0
        CanImport = 1
        CanExport = 2
    End Enum

    ''' <summary>
    ''' Rappresenta una sorgente di importazione/esportazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImportExportSource
        Inherits DBObject

        Private m_Name As String
        Private m_DisplayName As String
        Private m_Flags As ImportExportSourceFlags
        Private m_Password As String
        'Private m_UserName As String
        Private m_RemoteURL As String
        Private m_Attributi As CKeyCollection
        Private m_UserMapping As CImportExportSourceUserMap


        Public Sub New()
            Me.m_Name = ""
            Me.m_Flags = ImportExportSourceFlags.None
            Me.m_Password = ""
            Me.m_RemoteURL = ""
            Me.m_Attributi = Nothing
            Me.m_UserMapping = Nothing
            Me.m_DisplayName = ""
            'Me.m_UserName = ""
        End Sub

        Public Property DisplayName As String
            Get
                Return Me.m_DisplayName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DisplayName
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DisplayName = value
                Me.DoChanged("DisplayName", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property UserMapping As CImportExportSourceUserMap
            Get
                SyncLock Me
                    If (Me.m_UserMapping Is Nothing) Then
                        Dim tmp As Object = Me.Attributi.GetItemByKey("UserMapping")
                        If Not (TypeOf (tmp) Is CImportExportSourceUserMap) Then
                            Me.m_UserMapping = New CImportExportSourceUserMap
                        Else
                            Me.m_UserMapping = tmp
                        End If
                        Me.m_UserMapping.SetOwner(Me)
                        Me.Attributi.SetItemByKey("UserMapping", Me.m_UserMapping)
                    End If
                    Return Me.m_UserMapping
                End SyncLock
            End Get

        End Property

        ''' <summary>
        ''' Restituisce o imposta un nome descrittivo per l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Name
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        Public Property Flags As ImportExportSourceFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ImportExportSourceFlags)
                Dim oldValue As ImportExportSourceFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se questo oggetto è abilitato all'importazione dal sito specificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CanImport As Boolean
            Get
                Return TestFlag(Me.m_Flags, ImportExportSourceFlags.CanImport)
            End Get
            Set(value As Boolean)
                If (Me.CanImport = value) Then Return
                Me.m_Flags = SetFlag(Me.m_Flags, ImportExportSourceFlags.CanImport, value)
                Me.DoChanged("CanImport", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se questo oggetto è abilitato all'esportazione verso il sito remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CanExport As Boolean
            Get
                Return TestFlag(Me.m_Flags, ImportExportSourceFlags.CanExport)
            End Get
            Set(value As Boolean)
                If (Me.CanExport = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, ImportExportSourceFlags.CanExport, value)
                Me.DoChanged("CanExport", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una password che deve essere condivisa tra i siti che esportano ed importano
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
                If (value = oldValue) Then Return
                Me.m_Password = value
                Me.DoChanged("Password", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la url completa che viene chiamata per esportare verso il sito esterno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RemoteURL As String
            Get
                Return Me.m_RemoteURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RemoteURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_RemoteURL = value
                Me.DoChanged("RemoteURL", value, oldValue)
            End Set
        End Property

        '''' <summary>
        '''' Restituisce o imposta il nome utente usato per l'autenticazione con il server remoto
        '''' </summary>
        '''' <returns></returns>
        'Public Property UserName As String
        '    Get
        '        Return Me.m_UserName
        '    End Get
        '    Set(value As String)
        '        Dim oldValue As String = Me.m_UserName
        '        value = Strings.Trim(value)
        '        If (oldValue = value) Then Return
        '        Me.m_UserName = value
        '        Me.DoChanged("UserName", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_DisplayName = reader.Read("DisplayName", Me.m_DisplayName)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Password = reader.Read("Password", Me.m_Password)
            Me.m_RemoteURL = reader.Read("RemoteURL", Me.m_RemoteURL)
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("DisplayName", Me.m_DisplayName)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Password", Me.m_Password)
            writer.Write("RemoteURL", Me.m_RemoteURL)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("DisplayName", Me.m_DisplayName)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Password", Me.m_Password)
            writer.WriteAttribute("RemoteURL", Me.m_RemoteURL)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DisplayName" : Me.m_DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RemoteURL" : Me.m_RemoteURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.ImportExportSources.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDImportExportS"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.ImportExportSources.UpdateCached(Me)
        End Sub

        ''' <summary>
        ''' Esporta l'anagrafica e la finestra di lavorazione corrente verso il sito remoto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Function Esporta(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Me.CanExport = False) Then Throw New PermissionDeniedException("Azione Export non consentita per la sorgente: " & Me.Name)
            Dim tmp As String = RPC.InvokeMethod(Me.RemoteURL & "?_a=Esporta", "persona", RPC.str2n(XML.Utils.Serializer.Serialize(item.PersonaEsportata)), "recapiti", XML.Utils.Serializer.Serialize(item.PersonaEsportata.Recapiti), "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password))
            Return XML.Utils.Serializer.Deserialize(tmp)
        End Function

        ''' <summary>
        ''' Esporta l'anagrafica e la finestra di lavorazione corrente verso il sito remoto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Function ConfermaEsportazione(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Me.CanExport = False) Then Throw New PermissionDeniedException("Azione Export non consentita per la sorgente: " & Me.Name)
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=ConfermaEsportazione", "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function

        ''' <summary>
        ''' Revoca la richiesta di confronto fatta
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Function AnnullaEsportazione(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Me.CanExport = False) Then Throw New PermissionDeniedException("Azione Export non consentita per la sorgente: " & Me.Name)
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=AnnullaEsportazione", "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function

        ''' <summary>
        ''' Nel sistema che ha ricevuto la richiesta di valutazione rifiuta il cliente
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Function RifiutaCliente(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Me.CanImport = False) Then Throw New PermissionDeniedException("Azione Import non consentita per la sorgente: " & Me.Name)
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=RifiutaCliente", "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function


        ''' <summary>
        ''' Importa l'anagrafica e la finestra di lavorazione corrente verso il sito remoto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Function Importa(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Me.CanImport = False) Then Throw New PermissionDeniedException("Azione Import non consentita per la sorgente: " & Me.Name)
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=Importa", "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function

        ''' <summary>
        ''' Prende in carico la finestra importata da un sito esterno
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Function PrendiInCarico(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Me.CanImport = False) Then Throw New PermissionDeniedException("Azione PrendiInCarico non consentita per la sorgente " & Me.Name)
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=PrendiInCarico", "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function

        Protected Friend Overridable Function Sincronizza(ByVal item As CImportExport, ByVal oggetti As CKeyCollection) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Not Me.CanExport OrElse Not Me.CanImport) Then Throw New PermissionDeniedException("La sincronizzazione richiede l'autorizzazione ad Importare e Esportare")
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=Sincronizza", "oggetti", RPC.str2n(XML.Utils.Serializer.Serialize(oggetti)), "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function

        Protected Friend Overridable Function Sollecita(ByVal item As CImportExport) As CImportExport
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (Not Me.CanExport OrElse Not Me.CanImport) Then Throw New PermissionDeniedException("La sollecitazione richiede l'autorizzazione ad Importare e Esportare")
            Return XML.Utils.Serializer.Deserialize(RPC.InvokeMethod(Me.RemoteURL & "?_a=Sollecita", "item", RPC.str2n(XML.Utils.Serializer.Serialize(item)), "un", RPC.str2n(Me.Name), "pwd", RPC.str2n(Me.Password)))
        End Function

    End Class





End Class
Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils


Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms


    Public Class CSystemModuleHandler
        Inherits CBaseModuleHandler


        Public Sub New()
        End Sub

        Public Function GetServerTime(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.SerializeDate(Now)
        End Function



        Public Function GetSysDebugInfo(ByVal renderer As Object) As String
            Dim ret As New WebSiteDebugInfo
            ret.Initialize()
            Return XML.Utils.Serializer.Serialize(ret)
        End Function


        Public Function htmlEncode(ByVal renderer As Object) As String
            Dim text As String
            text = RPC.n2str(Me.GetParameter(renderer, "text", ""))
            Return Strings.HtmlEncode(text)
        End Function

        Public Function getSessionID(ByVal renderer As Object) As String
            Return WebSite.ASP_Session.SessionID
        End Function

        Public Function setScreenSize(ByVal renderer As Object) As String
            WebSite.ASP_Session("window_w") = RPC.n2int(Me.GetParameter(renderer, "w", ""))
            WebSite.ASP_Session("window_h") = RPC.n2int(Me.GetParameter(renderer, "h", ""))
            Return vbNullString
        End Function

        Public Function SaveAttachment(ByVal renderer As Object) As String
            Dim str As String
            Dim item As CAttachment
            Dim itemID As Integer
            str = RPC.n2str(Me.GetParameter(renderer, "data", ""))
            item = XML.Utils.Serializer.Deserialize(str, "CAttachment")
            item.Save()
            itemID = Databases.GetID(item, 0)
            Return RPC.FormatID(itemID)
        End Function

        Public Function deleteAttachment(ByVal renderer As Object) As String
            Dim item As CAttachment
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            Dim cursor As New CAttachmentsCursor
            cursor.ID.Value = itemID
            item = cursor.Item
            cursor.Dispose()
            item.Delete()
            Return vbNullString
        End Function

        Public Function getScadenze(ByVal renderer As Object) As String
            Dim f, t As Date
            Dim items As CCollection(Of ICalendarActivity)
            f = RPC.n2date(Me.GetParameter(renderer, "f", ""))
            t = RPC.n2date(Me.GetParameter(renderer, "t", ""))
            items = Calendar.GetScadenze(f, t)
            If items.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, 0)
            Else
                Return vbNullString
            End If
        End Function

        'Public Function addProvider() As String
        '    Dim pName, className As String
        '    pName = RPC.n2str(Me.GetParameter(renderer, "pName"))
        '    className = RPC.n2str(Me.GetParameter(renderer, "cName"))
        '    Calendar.Providers.RegisterProvider(pName, className)
        '    Return vbNullString
        'End Function

        'Public Function removeProvider() As String
        '    Dim pName As String
        '    pName = RPC.n2str(Me.GetParameter(renderer, "pName"))
        '    Calendar.Providers.UnregisterProvider(pName)
        '    Return vbNullString
        'End Function

        'Public Function ResettaSessioni() As String
        '    DirectCast(ApplicationContext, WebApplicationContext).ResettaCursori()
        '    Return ""
        'End Function

        Public Function CreateNewCursor(ByVal renderer As Object) As String
            Dim tn, params As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim ret As String = vbNullString
#If Not DEBUG Then
        Try
#End If

            tn = RPC.n2str(Me.GetParameter(renderer, "tn", ""))
            params = RPC.n2str(Me.GetParameter(renderer, "params", ""))
            If (tn = "") Then Throw New ArgumentNullException("Tipo cursore")
            If (params = "") Then Throw New ArgumentNullException("Params")
            cursor = XML.Utils.Serializer.Deserialize(params, tn)
            cursor.Open()
            With DirectCast(ApplicationContext, WebApplicationContext)
                .OpenCursor(cursor)
            End With

            ret = XML.Utils.Serializer.Serialize(cursor, 0)
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            Throw
        Finally
#End If
            If (cursor IsNot Nothing) Then
                Dim arr As System.Array = cursor.GetItemsArray
                If (arr IsNot Nothing) Then
                    For Each o As Object In arr
                        If TypeOf (o) Is IDisposable Then DirectCast(o, IDisposable).Dispose()
                    Next
                End If
                cursor.Dispose()
            End If
            cursor = Nothing
#If Not DEBUG Then
        End Try
#End If
            Return ret
        End Function

        'Public Function LoadNextCursorPage(ByVal renderer As Object) As String
        '    Dim tn, token As String
        '    Dim items() As Object = Nothing
        '    Dim idx As Integer
        '    Dim cursor As DBObjectCursorBase = Nothing
        '    Dim ret As String = vbNullString

        '    Try
        '        tn = RPC.n2str(Me.GetParameter(renderer, "tn", ""))
        '        If (tn = "") Then Throw New ArgumentNullException("Tipo cursore")
        '        token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
        '        If (token = "") Then Throw New ArgumentNullException("Token")
        '        idx = RPC.n2int(Me.GetParameter(renderer, "idx", ""))
        '        If (idx < 0) Then Throw New ArgumentOutOfRangeException("Indice")
        '        With DirectCast(ApplicationContext, WebApplicationContext)
        '            cursor = .GetCursor(token) ' DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
        '        End With
        '        If (TypeName(cursor) <> tn) Then Throw New ArgumentException("Il token non identifica il cursore specificato")
        '        cursor.MoveTo(idx)
        '        items = cursor.GetItemsArray
        '        If items IsNot Nothing Then
        '            ret = XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
        '            For Each o As Object In items
        '                If TypeOf (o) Is IDisposable Then DirectCast(o, IDisposable).Dispose()
        '            Next
        '            Erase items
        '            items = Nothing
        '        End If
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw
        '    Finally
        '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '        If items IsNot Nothing Then
        '            For Each o As Object In items
        '                If TypeOf (o) Is IDisposable Then DirectCast(o, IDisposable).Dispose()
        '            Next
        '            Erase items
        '            items = Nothing
        '        End If
        '    End Try

        '    Return ret
        'End Function


        Public Function LoadNextCursorPage(ByVal renderer As Object) As String
            Dim tn, token As String
            Dim idx As Integer
            Dim cursor As DBObjectCursorBase = Nothing
            Dim ret As String = vbNullString

            Try
                tn = RPC.n2str(Me.GetParameter(renderer, "tn", ""))
                If (tn = "") Then Throw New ArgumentNullException("Tipo cursore")
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (token = "") Then Throw New ArgumentNullException("Token")
                idx = RPC.n2int(Me.GetParameter(renderer, "idx", ""))
                If (idx < 0) Then Throw New ArgumentOutOfRangeException("Indice")
                With DirectCast(ApplicationContext, WebApplicationContext)
                    cursor = .GetCursor(token) ' DBObjectCursorBase.Restore(Sistema.ApplicationContext.Settings, token) ' Sistema.CreateInstance(tn)
                End With
                If (TypeName(cursor) <> tn) Then Throw New ArgumentException("Il token non identifica il cursore specificato")
                cursor.MoveTo(idx)
                ret = XML.Utils.Serializer.Serialize(cursor, XMLSerializeMethod.Document)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return ret
        End Function

        'Public Function LoadNextCursorPage1(ByVal renderer As Object) As String
        '    Dim c As String
        '    Dim items() As Object = Nothing
        '    Dim idx As Integer
        '    Dim cursor As DBObjectCursorBase = Nothing
        '    Dim ret As String = vbNullString

        '    Try
        '        c = RPC.n2str(Me.GetParameter(renderer, "c", ""))
        '        cursor = XML.Utils.Serializer.Deserialize(c)
        '        idx = RPC.n2int(Me.GetParameter(renderer, "idx", ""))
        '        If (idx < 0) Then Throw New ArgumentOutOfRangeException("Indice")
        '        cursor.Reset1()
        '        cursor.MoveTo(idx)
        '        items = cursor.GetItemsArray
        '        If items IsNot Nothing Then
        '            ret = XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
        '            For Each o As Object In items
        '                If TypeOf (o) Is IDisposable Then DirectCast(o, IDisposable).Dispose()
        '            Next
        '            Erase items
        '            items = Nothing
        '        End If
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw
        '    Finally
        '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '        If items IsNot Nothing Then
        '            For Each o As Object In items
        '                If TypeOf (o) Is IDisposable Then DirectCast(o, IDisposable).Dispose()
        '            Next
        '            Erase items
        '            items = Nothing
        '        End If
        '    End Try

        '    Return ret
        'End Function

        Public Function LoadNextCursorPage1(ByVal renderer As Object) As String
            Dim c As String
            Dim idx As Integer
            Dim cursor As DBObjectCursorBase = Nothing
            Dim ret As String = vbNullString

            Try
                c = RPC.n2str(Me.GetParameter(renderer, "c", ""))
                cursor = XML.Utils.Serializer.Deserialize(c)
                idx = RPC.n2int(Me.GetParameter(renderer, "idx", ""))
                If (idx < 0) Then Throw New ArgumentOutOfRangeException("Indice")
                cursor.Reset1()
                cursor.MoveTo(idx)
                ret = XML.Utils.Serializer.Serialize(cursor, XMLSerializeMethod.Document)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return ret
        End Function

        Public Function DestroyCursor(ByVal renderer As Object) As String
            Dim tn, token As String
#If Not DEBUG Then
            Try
#End If
            tn = RPC.n2str(Me.GetParameter(renderer, "tn", ""))
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (tn = "") Then Throw New ArgumentNullException("Tipo cursore")
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (token = "") Then Throw New ArgumentNullException("Token")
                'Dim cursor As DBObjectCursorBase

                With DirectCast(ApplicationContext, WebApplicationContext)
                    .DestroyCursor(token)
                End With
            'If (cursor IsNot Nothing) Then Debug.Print("Cursore: " & TypeName(cursor) & "[" & cursor.Token & "].Destroy()")
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally

            End Try
#End If

            Return vbNullString
        End Function

        Public Function ResetCursor(ByVal renderer As Object) As String
            Dim tn, token As String
#If Not DEBUG Then
            Try
#End If
            tn = RPC.n2str(Me.GetParameter(renderer, "tn", ""))
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (tn = "") Then Throw New ArgumentNullException("Tipo cursore")
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (token = "") Then Throw New ArgumentNullException("Token")
            With DirectCast(ApplicationContext, WebApplicationContext)
                .ResetCursor(token)
            End With
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            End Try
#End If

            Return vbNullString
        End Function

        Public Function RestoreCursor(ByVal renderer As Object) As String
            Dim tn, token As String
            Dim cursor As DBObjectCursorBase = Nothing
#If Not DEBUG Then
            Try
#End If
            tn = RPC.n2str(Me.GetParameter(renderer, "tn", ""))
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (tn = "") Then Throw New ArgumentNullException("Tipo cursore")
                token = RPC.n2str(Me.GetParameter(renderer, "tk", ""))
                If (token = "") Then Throw New ArgumentNullException("Token")
                With DirectCast(ApplicationContext, WebApplicationContext)
                    cursor = .RestoreCursor(token)
                End With
                If (TypeName(cursor) <> tn) Then Throw New ArgumentException("Il token non identifica il cursore specificato")
                Dim ret As String = XML.Utils.Serializer.Serialize(cursor, XMLSerializeMethod.Document)
            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                'If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If
        End Function

        Public Function CurrentUserLogOut(ByVal renderer As Object) As String
            Dim id As Integer
            id = GetID(Users.CurrentUser)
            Users.LogOut(Users.CurrentUser, LogOutMethods.LOGOUT_LOGOUT)
            Return RPC.FormatID(id)
        End Function

        Function ChangeUserAvatar(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(Me.GetParameter(renderer, "uid", 0))
            Dim url As String = RPC.n2str(Me.GetParameter(renderer, "url", vbNullString))
            If (uid <> GetID(Users.CurrentUser)) Then
                Throw New PermissionDeniedException(Me.Module, "ChangeUserAvatar")
            End If
            Dim user As CUser = Sistema.Users.GetItemById(uid)
            user.IconURL = url
            user.Save()
            Return vbNullString
        End Function

        Public Function SaveUser(ByVal renderer As Object) As String
            Dim objType As String
            Dim objValue As String
            Dim obj As CUser

            objType = RPC.n2str(Me.GetParameter(renderer, "type", ""))
            objValue = RPC.n2str(Me.GetParameter(renderer, "value", ""))
            obj = XML.Utils.Serializer.Deserialize(objValue, objType)

            If obj.GetModule IsNot Nothing Then
                Dim can As Boolean = False

                If GetID(obj) = 0 Then
                    If obj.GetModule.UserCanDoAction("create") Then
                        can = True
                    End If
                Else
                    If obj.GetModule.UserCanDoAction("edit") Then
                        can = True
                    Else
                        If obj.GetModule.UserCanDoAction("edit_own") AndAlso TypeOf (obj) Is IDBObject Then
                            With DirectCast(obj, IDBObject)
                                can = .CreatoDaId = GetID(Users.CurrentUser)
                            End With
                        End If
                    End If
                End If
                If Not can Then
                    Throw New PermissionDeniedException("Permesso negato: " & obj.GetModule.ModuleName & ".SaveObject")
                End If
            End If

            Dim user As CUser = Sistema.Users.GetItemById(GetID(obj))
            If (user IsNot Nothing) Then
                user.InitializeFrom(obj)
            Else
                user = obj
            End If
            user.Save()
            Return RPC.FormatID(GetID(obj))
        End Function

        Public Function SaveObject(ByVal renderer As Object) As String
            Dim objType As String
            Dim objValue As String
            Dim obj As DBObjectBase
            Dim handler As IModuleHandler
            Dim can As Boolean = True
            Dim moduleName As String = vbNullString
            Dim force As Boolean = RPC.n2bool(GetParameter(renderer, "f", "f"))
            objType = RPC.n2str(Me.GetParameter(renderer, "type", ""))
            objValue = RPC.n2str(Me.GetParameter(renderer, "value", ""))
            obj = XML.Utils.Serializer.Deserialize(objValue, objType)

            If obj.GetModule IsNot Nothing Then
                moduleName = obj.GetModule.ModuleName
                handler = obj.GetModule.CreateHandler(Nothing)
                If (handler IsNot Nothing) Then
                    If GetID(obj) = 0 Then
                        can = handler.CanCreate
                    Else
                        can = handler.CanEdit(obj)
                    End If
                End If
            End If

            If Not can Then
                Throw New PermissionDeniedException("Permesso negato: [" & moduleName & "].SaveObject(" & TypeName(obj) & ")")
            End If

#If Not DEBUG Then
            Try
#End If
            obj.Save(force)

            Dim id As Integer = GetID(obj)


            Return RPC.FormatID(id)
#If Not DEBUG Then
            Catch ex As Exception
                Return RPC.FormatID(0) & ex.ToString
#End If

#If Not DEBUG Then
            Finally
                If TypeOf (obj) Is IDisposable Then DirectCast(obj, IDisposable).Dispose()
                obj = Nothing
            End Try
#End If
        End Function

        Public Function DeleteObject(ByVal renderer As Object) As String
            Dim objType As String
            Dim objValue As String
            Dim obj As DBObjectBase
            Dim handler As IModuleHandler
            Dim can As Boolean = True
            Dim moduleName As String = vbNullString
            Dim force As Boolean = RPC.n2bool(GetParameter(renderer, "f", "F"))
            objType = RPC.n2str(Me.GetParameter(renderer, "type", ""))
            objValue = RPC.n2str(Me.GetParameter(renderer, "value", ""))
            obj = XML.Utils.Serializer.Deserialize(objValue, objType)

            If obj.GetModule IsNot Nothing Then
                handler = obj.GetModule.CreateHandler(Nothing)
                If (handler IsNot Nothing) Then can = handler.CanDelete(obj)
            End If

            If Not can Then Throw New PermissionDeniedException("Permesso negato: [" & moduleName & "].DeleteObject(" & TypeName(obj) & ")")


            Try
                obj.Delete(force)

                Return RPC.FormatID(GetID(obj))
            Catch ex As Exception
                Return RPC.FormatID(0) & ex.ToString
            Finally
                If TypeOf (obj) Is IDisposable Then DirectCast(obj, IDisposable).Dispose()
                obj = Nothing
            End Try
        End Function

        Public Function AddNewNote(ByVal renderer As Object) As String
            Dim oid As Integer
            Dim otp As String
            Dim obj As DBObjectBase
            Dim Text As String
            Dim note As CAnnotazione
            oid = RPC.n2int(Me.GetParameter(renderer, "oid", ""))
            otp = RPC.n2str(Me.GetParameter(renderer, "otp", ""))
            Text = RPC.n2str(Me.GetParameter(renderer, "text", ""))
            obj = Types.CreateInstance(otp)
            DBUtils.SetID(obj, oid)
            'If TypeOf (obj) Is DBObject Then
            '    note = DirectCast(obj, DBObject).Annotazioni.Add(Text)
            'Else
            note = New CAnnotazione
            note.SetOwner(obj)
            note.Valore = Text

            'End If
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Save()

            Return XML.Utils.Serializer.Serialize(note, 0)
        End Function

        Public Function GetAnnotazioni(ByVal renderer As Object) As String
            Dim id As Integer = Me.GetParameter(renderer, "id", 0)
            Dim tp As String = Trim(Me.GetParameter(renderer, "tp", vbNullString))
            Dim cid As Integer = Me.GetParameter(renderer, "cid", 0)
            Dim ctp As String = Trim(Me.GetParameter(renderer, "ctp", vbNullString))
            If (id = 0) Then Throw New ArgumentNullException("id")
            If (tp = vbNullString) Then Throw New ArgumentNullException("tp")

            Dim cursor As New CAnnotazioniCursor
            Dim ret As New CCollection(Of CAnnotazione)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OwnerID.Value = id
            cursor.OwnerType.Value = tp
            cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True

            If (cid <> 0) And (ctp <> vbNullString) Then
                cursor.TipoContesto.Value = ctp
                cursor.IDContesto.Value = cid
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetAnnotazioneById(ByVal renderer As Object) As String
            Dim id As Integer
            Dim cursor As DBObjectCursorBase
            Dim note As CAnnotazione
            id = RPC.n2int(Me.GetParameter(renderer, "id", ""))
            cursor = New CAnnotazioniCursor
            cursor.ID.Value = id
            cursor.IgnoreRights = True
            note = cursor.Item
            cursor.Dispose()
            cursor = Nothing
            If note Is Nothing Then
                Return vbNullString
            Else
                Return XML.Utils.Serializer.Serialize(note, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetObjectAttachments(ByVal renderer As Object) As String
            Dim id As Integer = Me.GetParameter(renderer, "oid", 0)
            Dim tp As String = Trim(Me.GetParameter(renderer, "otype", vbNullString))
            Dim ctid As Integer = Me.GetParameter(renderer, "ctid", 0)
            Dim cttp As String = Trim(Me.GetParameter(renderer, "cttp", vbNullString))
            If (id = 0) Then Throw New ArgumentNullException("id", "0")
            If (tp = vbNullString) Then Throw New ArgumentNullException("tp")

            Dim cursor As New CAttachmentsCursor
            Dim ret As New CCollection(Of CAttachment)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OwnerID.Value = id
            cursor.OwnerType.Value = tp
            cursor.Testo.SortOrder = SortEnum.SORT_ASC
            cursor.IgnoreRights = True
            If (cttp <> vbNullString) Then
                cursor.TipoContesto.Value = cttp
                cursor.IDContesto.Value = ctid
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetEMailConfig(ByVal renderer As Object) As String
            'If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Return XML.Utils.Serializer.Serialize(Sistema.EMailer.Config)
        End Function

        Public Function GetFaxConfig(ByVal renderer As Object) As String
            'If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Return XML.Utils.Serializer.Serialize(Sistema.FaxService.Config)
        End Function

        Public Function GetInstalledFaxDrivers(ByVal renderer As Object) As String
            'If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Return XML.Utils.Serializer.Serialize(Sistema.FaxService.GetInstalledDrivers)
        End Function

        'Public Function GetFaxDriverConfiguration() As String
        '    'If (Not Me.CanList()) Then Throw New PermissionDeniedException(Me.Module, "list")
        '    Dim drvName As String = RPC.n2str(GetParameter(renderer, "drv", ""))
        '    Dim config As FaxDriverOptions = Sistema.FaxService.GetDriverConfiguration(drvName)
        '    Return XML.Utils.Serializer.Serialize(config)
        'End Function

        'Public Function SetFaxDriverConfiguration() As String
        '    If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
        '    Dim drvName As String = RPC.n2str(GetParameter(renderer, "drv", ""))
        '    Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
        '    Dim config As FaxDriverOptions = XML.Utils.Serializer.Deserialize(text)
        '    Sistema.FaxService.SetDriverConfiguration(drvName, config)
        '    Return ""
        'End Function

        'Public Function GetFaxModems() As String
        '    Dim drvName As String = RPC.n2str(GetParameter(renderer, "drv", ""))
        '    Dim modems As CCollection(Of FaxDriverModem) = Sistema.FaxService.GetDriver(drvName).GetModems
        '    Return XML.Utils.Serializer.Serialize(modems)
        'End Function

        'Public Function SetFaxModems() As String
        '    If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
        '    Dim drvName As String = RPC.n2str(GetParameter(renderer, "drv", ""))
        '    Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
        '    Dim modems As CCollection(Of FaxDriverModem) = XML.Utils.Serializer.Deserialize(text)
        '    Sistema.FaxService.GetDriver(drvName).SetModems(modems)
        '    Return ""
        'End Function

        Public Function SaveFaxDriver(ByVal renderer As Object) As String
            If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim drv As BaseFaxDriver = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "drv", "")))
            drv.SaveConfiguration()
            Return ""
        End Function

        Public Function GetSMSConfig(ByVal renderer As Object) As String
            'If (Not Me.CanList()) Then Throw New PermissionDeniedException(Me.Module, "list")
            Return XML.Utils.Serializer.Serialize(Sistema.SMSService.Config)
        End Function

        Public Function GetInstalledSMSDrivers(ByVal renderer As Object) As String
            'If (Not Me.CanList()) Then Throw New PermissionDeniedException(Me.Module, "list")
            Return XML.Utils.Serializer.Serialize(Sistema.SMSService.GetInstalledDrivers)
        End Function

        Public Function GetSMSDriverConfiguration(ByVal renderer As Object) As String
            ' If (Not Me.CanList()) Then Throw New PermissionDeniedException(Me.Module, "list")
            Dim drvName As String = RPC.n2str(GetParameter(renderer, "drv", ""))
            Dim config As SMSDriverOptions = Sistema.SMSService.GetDriverConfiguration(drvName)
            Return XML.Utils.Serializer.Serialize(config)
        End Function

        Public Function SetSMSDriverConfiguration(ByVal renderer As Object) As String
            If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim drvName As String = RPC.n2str(GetParameter(renderer, "drv", ""))
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim config As SMSDriverOptions = XML.Utils.Serializer.Deserialize(text)
            Sistema.SMSService.SetDriverConfiguration(drvName, config)
            Return ""
        End Function

        Public Function SaveSMSDriver(ByVal renderer As Object) As String
            If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim drv As BasicSMSDriver = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "drv", "")))
            drv.SaveConfiguration()
            Return ""
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function


        Public Function CancelPendingAlertsBySource(ByVal renderer As Object) As String
            Dim uid As Nullable(Of Integer) = RPC.n2int(Me.GetParameter(renderer, "user", ""))
            Dim toDate As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "todate", ""))
            Dim sourceType As String = RPC.n2str(Me.GetParameter(renderer, "sourceType", ""))
            Dim sourceID As Integer = RPC.n2int(Me.GetParameter(renderer, "sourceID", ""))
            Dim source As Object = Nothing
            Dim categoria As String = RPC.n2str(GetParameter(renderer, "categoria", ""))
            If (sourceType <> vbNullString) Then
                source = Sistema.Types.CreateInstance(sourceType)
                DBUtils.SetID(source, sourceID)
            End If
            If (uid.HasValue = True) Then
                Dim user As CUser = Sistema.Users.GetItemById(uid.Value)
                Sistema.Notifiche.CancelPendingAlertsBySource(user, toDate, source, categoria)
            Else
                Sistema.Notifiche.CancelPendingAlertsBySource(toDate, source, categoria)
            End If
            Return vbNullString
        End Function

        Public Function GetNotificheNonConsegnate(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(Me.GetParameter(renderer, "user", ""))
            Dim sourceType As String = RPC.n2str(Me.GetParameter(renderer, "sourceType", ""))
            Dim sourceID As Integer = RPC.n2int(Me.GetParameter(renderer, "sourceID", ""))
            Dim source As Object = Nothing
            Dim ret As CCollection(Of Notifica)
            If (sourceType <> vbNullString) Then
                source = Sistema.Types.CreateInstance(sourceType)
                DBUtils.SetID(source, sourceID)
            End If
            If (uid = 0) Then
                ret = Sistema.Notifiche.GetNotificheNonConsegnate(source)
            Else
                ret = Sistema.Notifiche.GetNotificheNonConsegnate(uid, source)
            End If
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Function GetNotificaById(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "id", ""))
            Dim item As Notifica = Sistema.Notifiche.GetItemById(id)
            If (item Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function ProgramAlert(ByVal renderer As Object) As String
            'Dim oid As Integer = RPC.n2int(Me.GetParameter(renderer, "oid"))
            Dim uid As Integer = RPC.n2int(Me.GetParameter(renderer, "user", ""))
            Dim descrizione As String = RPC.n2str(Me.GetParameter(renderer, "descrizione", ""))
            Dim [date] As Date = RPC.n2date(Me.GetParameter(renderer, "date", ""))
            Dim sourceType As String = RPC.n2str(Me.GetParameter(renderer, "sourceType", ""))
            Dim sourceID As Integer = RPC.n2int(Me.GetParameter(renderer, "sourceID", ""))
            Dim categoria As String = RPC.n2str(Me.GetParameter(renderer, "categoria", ""))
            Dim source As Object = Sistema.Types.CreateInstance(sourceType)
            Dim ret As Notifica
            DBUtils.SetID(source, sourceID)
            If (uid = 0) Then
                ret = Sistema.Notifiche.ProgramAlert(descrizione, [date], source, categoria)
            Else
                ret = Sistema.Notifiche.ProgramAlert(Users.GetItemById(uid), descrizione, [date], source, categoria)
            End If
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Function CountPendingAlertsBySource(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(Me.GetParameter(renderer, "user", ""))
            Dim sourceType As String = RPC.n2str(Me.GetParameter(renderer, "sourceType", ""))
            Dim sourceID As Integer = RPC.n2int(Me.GetParameter(renderer, "sourceID", ""))
            Dim source As Object = Nothing
            If (sourceType <> vbNullString) Then
                source = Sistema.Types.CreateInstance(sourceType)
                DBUtils.SetID(source, sourceID)
            End If
            If (uid = 0) Then
                Return XML.Utils.Serializer.SerializeInteger(Sistema.Notifiche.CountPendingAlertsBySource(source))
            Else
                Return XML.Utils.Serializer.SerializeInteger(Sistema.Notifiche.CountPendingAlertsBySource(uid, source))
            End If
        End Function

        Public Function GetPendingAlertsBySource(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(Me.GetParameter(renderer, "user", ""))
            Dim sourceType As String = RPC.n2str(Me.GetParameter(renderer, "sourceType", ""))
            Dim sourceID As Integer = RPC.n2int(Me.GetParameter(renderer, "sourceID", ""))
            Dim source As Object = Nothing
            Dim ret As CCollection(Of Notifica)
            If (sourceType <> vbNullString) Then
                source = Sistema.Types.CreateInstance(sourceType)
                DBUtils.SetID(source, sourceID)
            End If
            If (uid = 0) Then
                ret = Sistema.Notifiche.GetPendingAlertsBySource(source)
            Else
                ret = Sistema.Notifiche.GetPendingAlertsBySource(uid, source)
            End If
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Function LoadSettings(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(Me.GetParameter(renderer, "oid", ""))
            Dim otp As String = RPC.n2str(Me.GetParameter(renderer, "otp", ""))
            Dim items As New System.Collections.ArrayList
            Dim cursor As New CSettingsCursor
            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID

            cursor.OwnerType.Value = otp
            cursor.OwnerID.Value = oid
            While Not cursor.EOF
                items.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetAnnotazioniTipiContestoPerOggetto(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", ""))
            Dim otp As String = RPC.n2str(GetParameter(renderer, "otp", ""))
            Return XML.Utils.Serializer.Serialize(Annotazioni.GetTipiContestoPerOggetto(otp, oid), XMLSerializeMethod.Document)
        End Function

        Public Function GetAttachmentsTipiContestoPerOggetto(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", ""))
            Dim otp As String = RPC.n2str(GetParameter(renderer, "otp", ""))
            Return XML.Utils.Serializer.Serialize(Attachments.GetTipiContestoPerOggetto(otp, oid), XMLSerializeMethod.Document)
        End Function

        Public Function RegisterAzione(ByVal renderer As Object) As String
            Dim sn As String = RPC.n2str(GetParameter(renderer, "sn", ""))
            Dim ac As String = RPC.n2str(GetParameter(renderer, "ac", ""))
            Dim azione As AzioneEseguibile = Types.CreateInstance(ac)
            Dim item As AzioneRegistrata = Sistema.Notifiche.RegisterAzione(sn, azione)
            Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
        End Function

        Public Function UnregisterAzione(ByVal renderer As Object) As String
            Dim sn As String = RPC.n2str(GetParameter(renderer, "sn", ""))
            Dim ac As String = RPC.n2str(GetParameter(renderer, "ac", ""))
            Dim azione As AzioneEseguibile = Types.CreateInstance(ac)
            Dim item As AzioneRegistrata = Sistema.Notifiche.UnregisterAzione(sn, azione)
            Return RPC.FormatID(GetID(item))
        End Function

        Public Function GetRegisteredActions(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(Sistema.Notifiche.RegisteredActions)
        End Function

        Public Function ExecuteNotificaAction(ByVal renderer As Object) As String
            Dim action As String = RPC.n2str(GetParameter(renderer, "action", ""))
            Dim nid As Integer = RPC.n2int(GetParameter(renderer, "nid", ""))
            Dim params As String = RPC.n2str(GetParameter(renderer, "params", ""))
            Dim a As AzioneEseguibile = Sistema.Types.CreateInstance(action)
            Dim notifica As Notifica = Sistema.Notifiche.GetItemById(nid)
            Return XML.Utils.Serializer.Serialize(a.Execute(notifica, params))
        End Function

        Public Function GetRegisteredPropertyPages(ByVal renderer As Object) As String
            Dim typeName As String = RPC.n2str(GetParameter(renderer, "tn", ""))
            Dim items() As String = Sistema.PropertyPages.GetRegisteredPropertyPageNames(typeName)
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.Serialize(items)
            Else
                Return ""
            End If
        End Function

        Public Function GetGroupByName(ByVal renderer As Object) As String
            Dim nm As String = RPC.n2str(GetParameter(renderer, "nm", ""))
            Dim item As CGroup = Sistema.Groups.GetItemByName(nm)
            If (item Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetModuleByName(ByVal renderer As Object) As String
            Dim nm As String = RPC.n2str(GetParameter(renderer, "nm", ""))
            Dim item As CModule = Sistema.Modules.GetItemByName(nm)
            If (item Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
            End If
        End Function



        Private Function GetGC() As Long
            Return GC.GetTotalMemory(False)
        End Function

        Private Sub DebugGC(ByVal text As String, ByRef time As Date, ByRef gc As Long)
            Dim gcEnd As Long = GetGC()
            Dim timeEnd As Date = Now
            Dim msg As String
            msg = text & " -> " & Formats.FormatUserTime(time) & " - Time: " & Formats.FormatUserTime(timeEnd) & " (" & Formats.FormatNumber((timeEnd - time).TotalSeconds, 3) & ") "
            Dim diff As Long = gcEnd - gc
            msg &= " Memory: " & Formats.FormatBytes(gc, 3) & " - " & Formats.FormatBytes(gcEnd, 3) & " (" & Formats.FormatBytes(diff) & ")"
            If (diff > 10000) Then msg &= " (ATTENZIONE)"
            time = timeEnd
            gc = gcEnd
            Debug.Print(msg)
        End Sub

        Public Function GetToken(ByVal renderer As Object) As String
            Dim token As String = RPC.n2str(GetParameter(renderer, "token", ""))
            Dim ret As CSecurityToken = Sistema.ASPSecurity.GetToken(token)
            If (ret Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function CreateToken(ByVal renderer As Object) As String
            Dim name As String = RPC.n2str(GetParameter(renderer, "name", ""))
            Dim value As String = RPC.n2str(GetParameter(renderer, "value", ""))
            Dim expireCount As Integer = RPC.n2int(GetParameter(renderer, "ec", ""))
            Dim expireTime As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "et", ""))
            Dim ret As CSecurityToken = Sistema.ASPSecurity.CreateToken(name, value, expireCount, expireTime)
            If (ret Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function FindTokenOrCreate(ByVal renderer As Object) As String
            Dim name As String = RPC.n2str(GetParameter(renderer, "name", ""))
            Dim value As String = RPC.n2str(GetParameter(renderer, "value", ""))
            Dim expireCount As Integer = RPC.n2int(GetParameter(renderer, "ec", ""))
            Dim expireTime As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "et", ""))
            Dim ret As CSecurityToken = Sistema.ASPSecurity.FindTokenOrCreate(name, value, expireCount, expireTime)
            If (ret Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ret)
        End Function


        Public Function GetBackupConfiguration(ByVal renderer As Object) As String
            'If (Not Me.CanList()) Then Throw New PermissionDeniedException(Me.Module, "list")
            Return XML.Utils.Serializer.Serialize(Sistema.Backups.Configuration)
        End Function

        Public Function SetBackupConfiguration(ByVal renderer As Object) As String
            If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            Dim testo As String = RPC.n2str(GetParameter(renderer, "testo", ""))
            Dim c As Sistema.CBackupsConfiguration = XML.Utils.Serializer.Deserialize(testo)
            c.Save()
            Return ""
        End Function

        Public Function GetPendingQueries(ByVal renderer As Object) As String
            If (Not Me.CanConfigure()) Then Throw New PermissionDeniedException(Me.Module, "configure")
            SyncLock DBUtils.PendingQueries
                Return XML.Utils.Serializer.Serialize(DBUtils.PendingQueries)
            End SyncLock
        End Function

    End Class



End Namespace
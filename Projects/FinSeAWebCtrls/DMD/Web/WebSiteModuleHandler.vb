Imports DMD
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Namespace Forms


    Public Class WebSiteModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Function GetUploadPercentage(ByVal renderer As Object) As String
            Dim uid As String = RPC.n2str(Me.GetParameter(renderer, "uid", ""))
            If Uploader.IsUploading(uid) Then
                Return Formats.ToInteger(Uploader.GetUpload(uid).Percentage)
            Else
                Return ""
            End If
        End Function

        Public Function AddIP(ByVal renderer As Object) As String
            If WebSite.Instance.Module.UserCanDoAction("edit") = False Then
                Throw New PermissionDeniedException
                Return ""
            End If
            Dim params As String = RPC.n2str(Me.GetParameter(renderer, "params", ""))
            Dim item As IPADDRESSinfo = XML.Utils.Serializer.Deserialize(params, "IPADDRESSinfo")
            item.Save()
            WebSite.Instance.AllowedIPs.Add(item)
            Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
        End Function

        Public Function RemoveIP(ByVal renderer As Object) As String
            If WebSite.Instance.Module.UserCanDoAction("edit") = False Then
                Throw New PermissionDeniedException
                Return ""
            End If
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "id", ""))
            Dim item As IPADDRESSinfo = WebSite.Instance.AllowedIPs.GetItemById(id)
            WebSite.Instance.AllowedIPs.Remove(item)
            item.Stato = ObjectStatus.OBJECT_DELETED
            item.Save()
            Return ""
        End Function

        Public Function TestRemoveIP(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "id", ""))
            Dim item As IPADDRESSinfo = WebSite.Instance.AllowedIPs.GetItemById(id)
            Return IIf(WebSite.Instance.AllowedIPs.TestRemove(item), "1", "0")
        End Function

        Public Function TestAddIP(ByVal renderer As Object) As String
            Dim params As String = RPC.n2str(Me.GetParameter(renderer, "params", ""))
            Dim item As IPADDRESSinfo = XML.Utils.Serializer.Deserialize(params, "IPADDRESSinfo")
            Return IIf(WebSite.Instance.AllowedIPs.TestAdd(item), "1", "0")
        End Function

        Public Function TestIPAllowNegate(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            Dim a As Boolean = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            Dim n As Boolean = RPC.n2bool(Me.GetParameter(renderer, "n", ""))
            Return IIf(WebSite.Instance.AllowedIPs.TestAllowNegate(WebSite.Instance.AllowedIPs.GetItemById(id), a, n), "1", "0")
        End Function

        Public Function SetIPAllowNegate(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            Dim a As Boolean = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            Dim n As Boolean = RPC.n2bool(Me.GetParameter(renderer, "n", ""))
            WebSite.Instance.AllowedIPs.SetAllowNegate(WebSite.Instance.AllowedIPs.GetItemById(id), a, n)
            Return ""
        End Function

        Public Function GetServerTime(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.SerializeDate(Calendar.Now)
        End Function

        Function GetMainDBStats(ByVal renderer As Object) As String
            Return "" 'XML.Utils.Serializer.Serialize(APPConn.GetQueryTimes, XMLSerializeMethod.Document)"
        End Function

        Function GetLogDBStats(ByVal renderer As Object) As String
            Return "" 'XML.Utils.Serializer.Serialize(LOGConn.GetQueryTimes, XMLSerializeMethod.Document)
        End Function

        Function GetWebPageStats(ByVal renderer As Object) As String
            Return "" ' XML.Utils.Serializer.Serialize(WebSite.Instance.GetQueryTimes, XMLSerializeMethod.Document)
        End Function

        Public Function GetCurrentSessionID(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.SerializeString(WebSite.ASP_Session.SessionID)
        End Function

        Public Function GetConfiguration(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(WebSite.Instance.Configuration)
        End Function

        Public Function SaveConfiguration(ByVal renderer As Object) As String
            If (Not Me.CanList()) Then Throw New PermissionDeniedException(Me.Module, "edit")
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim config As SiteConfig = XML.Utils.Serializer.Deserialize(text)
            config.Save()
            Return ""
        End Function

        Public Function GetCurrentSessionInfo(ByVal renderer As Object) As String
            Dim ret As CSessionInfo = DirectCast(Sistema.ApplicationContext, DMD.WebSite.WebApplicationContext).GetCurrentSessionInfo
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetSessionInfo(ByVal renderer As Object) As String
            If Not Me.CanList Then Throw New PermissionDeniedException(Me.Module, "list")
            Dim sid As String = RPC.n2str(GetParameter(renderer, "sid", ""))
            Dim ret As CSessionInfo = DirectCast(Sistema.ApplicationContext, DMD.WebSite.WebApplicationContext).GetSessionInfo(sid)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function IsUploading(ByVal renderer As Object) As String
            Dim key As String = RPC.n2str(GetParameter(renderer, "key", ""))
            If (key = "") Then Return False
            Dim ret As Boolean = WebSite.Uploader.IsUploading(key)
            Return IIf(ret, "T", "F")
        End Function

        Public Function getTotalNumberOfUploads(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.SerializeInteger(DMD.WebSite.Uploader.TotalNumberOfUploads)
        End Function

        Public Function getUploadedFiles(ByVal renderer As Object) As String
            Dim ht As CCollection = WebSite.Uploader.UploadedFiles '= RPC.invokeMethod("/widgets/websvc/?_m=WebSite&_a=getUploadedFiles");
            Return XML.Utils.Serializer.Serialize(ht)
        End Function

    End Class


End Namespace
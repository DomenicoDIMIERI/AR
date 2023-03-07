Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CDevicesLogClass
        Inherits CGeneralClass(Of DeviceLog)

        Friend Sub New()
            MyBase.New("modOfficeDevLog", GetType(DeviceLogCursor), 50)
        End Sub

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeDevLog")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeDevLog")
                ret.Description = "Log Dispositivi"
                ret.DisplayName = "Log Dispositivi"
                ret.Parent = DMD.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not DMD.Office.Database.Tables.ContainsKey("tbl_OfficeDevLog") Then
                Dim table As CDBTable = DMD.Office.Database.Tables.Add("tbl_OfficeDevLog")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("IDDevice", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Flags", TypeCode.Int32)
                field = table.Fields.Add("Params", TypeCode.String) : field.MaxLength = 0
                field = table.Fields.Add("StartDate", TypeCode.DateTime)
                field = table.Fields.Add("EndDate", TypeCode.DateTime)
                field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32)
                field = table.Fields.Add("NomePuntoOperativo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                field = table.Fields.Add("Stato", TypeCode.Int32)
                table.Create()
            End If
            Return ret
        End Function

        Public Function GetLastLog(dev As Dispositivo) As DeviceLog
            Dim log As DeviceLog = Nothing

            SyncLock Me.cacheLock
                If (dev Is Nothing) Then Throw New ArgumentNullException("dev")
                For Each o As CacheItem In Me.CachedItems
                    log = o.Item
                    If log.IDDevice = GetID(dev) Then
                        log.SetDevice(dev)
                        Return log
                    End If
                Next
            End SyncLock

            Dim cursor As DeviceLogCursor = Nothing
#If Not DEBUG Then
            Try
#End If
            cursor = New DeviceLogCursor
                cursor.IDDevice.Value = GetID(dev)
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                log = cursor.Item
                If (log IsNot Nothing) Then
                    log.SetDevice(dev)
                    Me.AddToCache(log)
                End If
            Return log
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
            End Try
#End If

        End Function
    End Class

End Namespace

Partial Class Office




    Private Shared m_DevicesLog As CDevicesLogClass = Nothing

    Public Shared ReadOnly Property DevicesLog As CDevicesLogClass
        Get
            If (m_DevicesLog Is Nothing) Then m_DevicesLog = New CDevicesLogClass
            Return m_DevicesLog
        End Get
    End Property

End Class
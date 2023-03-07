Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Web

Partial Class WebSite

    Public Class CSessionInfo
        Implements DMD.XML.IDMDXMLSerializable, IDisposable

        Public ReadOnly lock As New Object

        Public InternalID As Integer
        Public SessionID As String
        Public RemoteIP As String
        Public RemotePort As String
        Public StartTime As Date
        Public RemoteTime As Nullable(Of Date)
        Public ServerTime As Nullable(Of Date)
        Public LastUpdated As DateTime
        Public CurrentUserName As String
        Public CurrentUser As CUser
        Public CurrentUfficio As CUfficio
        Public CurrentSession As CSiteSession
        Public CurrentLogin As CLoginHistory
        Public ForceAbadon As Boolean
        Public OriginalUser As CUser
        Public Parameters As CKeyCollection

        Public Descrizione As String

        Public RemoteOpenedCursors As New CKeyCollection(Of DBObjectCursorBase)

        'Public QueriesInfo As New CKeyCollection(Of StatsItem)
        'Public PagesInfo As New CKeyCollection(Of StatsItem)
        'Public WebServicesInfo As New CKeyCollection(Of StatsItem)
        ''Public userSessions As New CCollection(Of CSiteSession)

        Public Sub New()
            Me.InternalID = 0
            Me.SessionID = ""
            Me.RemoteIP = ""
            Me.RemotePort = ""
            Me.StartTime = Calendar.Now
            Me.RemoteTime = Nothing
            Me.ServerTime = Nothing
            Me.CurrentUserName = ""
            Me.CurrentUser = Nothing
            Me.CurrentUfficio = Nothing
            Me.CurrentSession = Nothing
            Me.CurrentLogin = Nothing
            Me.OriginalUser = Nothing
            Me.Parameters = New CKeyCollection
            Me.Descrizione = ""
            Me.ForceAbadon = False
            Me.LastUpdated = Now
            Me.RemoteOpenedCursors = New CKeyCollection(Of DBObjectCursorBase)
        End Sub

        Public Sub New(ByVal sessionID As String)
            Me.New
            DMD.DMDObject.IncreaseCounter(Me)
            Me.SessionID = sessionID
        End Sub

        Public Sub Reset()
            SyncLock Me.lock
                For Each c As DBObjectCursorBase In Me.RemoteOpenedCursors
                    Try
                        c.Dispose()
                    Catch ex As Exception
                        Sistema.ApplicationContext.Log("Session: " & Me.SessionID & " resetting cursor " & TypeName(c) & " error" & vbCrLf & ex.Message & vbCrLf & ex.StackTrace)
                    End Try
                Next
                Me.RemoteOpenedCursors.Clear()
            End SyncLock
        End Sub


        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "InternalID" : Me.InternalID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SessionID" : Me.SessionID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RemoteIP" : Me.RemoteIP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RemotePort" : Me.RemotePort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RemoteTime" : Me.RemoteTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ServerTime" : Me.ServerTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "CurrentUserID" : Me.CurrentUser = Sistema.Users.GetItemById(XML.Utils.Serializer.DeserializeInteger(fieldValue))
                Case "CurrentUserName" : Me.CurrentUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OriginalUserID" : Me.OriginalUser = Sistema.Users.GetItemById(XML.Utils.Serializer.DeserializeInteger(fieldValue))
                Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StartTime" : Me.StartTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ForceAbadon" : Me.ForceAbadon = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "PagesInfo"
                    'Me.PagesInfo.Clear()
                    'Dim tmp As CKeyCollection = fieldValue
                    'For Each k As String In tmp.Keys
                    '    Me.PagesInfo.Add(k, tmp(k))
                    'Next
                Case "WebServicesInfo"
                    'Me.WebServicesInfo.Clear()
                    'Dim tmp As CKeyCollection = fieldValue
                    'For Each k As String In tmp.Keys
                    '    Me.WebServicesInfo.Add(k, tmp(k))
                    'Next
                Case "QueriesInfo"
                    'Me.QueriesInfo.Clear()
                    'Dim tmp As CKeyCollection = fieldValue
                    'For Each k As String In tmp.Keys
                    '    Me.QueriesInfo.Add(k, tmp(k))
                    'Next
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("InternalID", Me.InternalID)
            writer.WriteAttribute("SessionID", Me.SessionID)
            writer.WriteAttribute("RemoteIP", Me.RemoteIP)
            writer.WriteAttribute("RemotePort", Me.RemotePort)
            writer.WriteAttribute("RemoteTime", Me.RemoteTime)
            writer.WriteAttribute("ServerTime", Me.ServerTime)
            writer.WriteAttribute("CurrentUserID", GetID(Me.CurrentUser))
            writer.WriteAttribute("CurrentUserName", Me.CurrentUserName)
            writer.WriteAttribute("OriginalUserID", GetID(Me.OriginalUser))
            writer.WriteAttribute("Descrizione", Me.Descrizione)
            writer.WriteAttribute("StartTime", Me.StartTime)
            writer.WriteAttribute("ForceAbadon", Me.ForceAbadon)
            'writer.WriteTag("Parameters", Me.Parameters)
            'writer.WriteTag("QueriesInfo", Me.QueriesInfo)
            'writer.WriteTag("PagesInfo", Me.PagesInfo)
            'writer.WriteTag("WebServicesInfo", Me.WebServicesInfo)
        End Sub


        ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            'Me.RemoteIP = vbNullString
            'Me.RemotePort = vbNullString
            'Me.RemoteTime = Nothing
            'Me.ServerTime = Nothing
            'Me.CurrentUserName = vbNullString
            'Me.CurrentUser = Nothing
            'Me.CurrentUfficio = Nothing
            'Me.CurrentSession = Nothing
            'Me.CurrentLogin = Nothing
            'Me.OriginalUser = Nothing
            SyncLock Me.lock
                While (Me.RemoteOpenedCursors.Count > 0)
                    Dim c As DBObjectCursorBase = Me.RemoteOpenedCursors(0)
                    Me.RemoteOpenedCursors.RemoveAt(0)
                    c.Dispose()
                End While
            End SyncLock

            ' Me.RemoteOpenedCursors = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub


        'Public Function BeginPage(ByVal pageName As String) As StatsItem
        'SyncLock Me.PagesInfo
        '    Dim info As StatsItem = Me.PagesInfo.GetItemByKey(pageName)
        '    If (info Is Nothing) Then
        '        info = New StatsItem
        '        info.Name = pageName
        '        Me.PagesInfo.Add(info.Name, info)
        '    End If
        '    info.Begin()
        '    Return New StatsItem(pageName, info.Count, info.LastRun, GC.GetTotalMemory(False))
        'End SyncLock
        'End Function

        'Public Function EndPage(ByVal info As StatsItem) As StatsItem
        '    SyncLock Me.PagesInfo
        '        Dim ret As StatsItem = Me.PagesInfo.GetItemByKey(info.Name)
        '        ret.End(info)
        '        Return ret
        '    End SyncLock
        'End Function

        'Public Function BeginWebService(ByVal pageName As String) As StatsItem
        '    SyncLock Me.WebServicesInfo
        '        Dim info As StatsItem = Me.WebServicesInfo.GetItemByKey(pageName)
        '        If (info Is Nothing) Then
        '            info = New StatsItem
        '            info.Name = pageName
        '            Me.WebServicesInfo.Add(info.Name, info)
        '        End If
        '        info.Begin()
        '        Return New StatsItem(pageName, info.Count, info.LastRun, GC.GetTotalMemory(False))
        '    End SyncLock
        'End Function

        'Public Function EndWebService(ByVal info As StatsItem) As StatsItem
        '    SyncLock Me.WebServicesInfo
        '        Dim ret As StatsItem = Me.WebServicesInfo.GetItemByKey(info.Name)
        '        ret.End(info)
        '        Return ret
        '    End SyncLock
        'End Function


    End Class


End Class

Imports DMD
Imports DMD.Sistema
Imports DMD.CallManagers
Imports DMD.CallManagers.Events

Public Class AsteriskServer
    Implements DMD.XML.IDMDXMLSerializable

    Public Event Connected(ByVal sender As Object, ByVal e As AsteriskEventArgs)
    Public Event Disconnected(ByVal sender As Object, ByVal e As AsteriskEventArgs)
    Public Event ManagerEvent(ByVal sender As Object, ByVal e As AsteriskEvent)


    Private WithEvents m_Timer As System.Timers.Timer

    Public Nome As String
    Public ServerName As String
    Public ServerPort As Integer = 5038
    Public UserName As String = ""
    Public Password As String = ""
    Public Channel As String = ""
    Public CallerID As String = ""
    Private WithEvents m_Manager As DMD.CallManagers.AsteriskCallManager

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_Timer = New System.Timers.Timer
        Me.m_Timer.Interval = 60 * 5 * 1000
        Me.m_Timer.Enabled = False
        Me.m_Manager = Nothing
    End Sub

    Public Sub New(ByVal nome As String, ByVal serverName As String, ByVal channel As String, ByVal userName As String, ByVal password As String)
        Me.New()
        Me.Nome = nome
        Me.ServerName = serverName
        Me.Channel = channel
        Me.UserName = userName
        Me.Password = password
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Nome & " (" & Me.ServerName & ":" & Me.ServerPort & ")"
    End Function



    Public Function GetManager() As DMD.CallManagers.AsteriskCallManager
        Return Me.m_Manager
    End Function

    Public Sub SetManager(ByVal value As DMD.CallManagers.AsteriskCallManager)
        Me.m_Manager = value
    End Sub

    Public Function IsConnected() As Boolean
        Return Me.m_Manager IsNot Nothing AndAlso Me.GetManager.IsConnected
    End Function

    Public Sub Connect()
        If Me.IsConnected Then Exit Sub
        Me.m_Manager = New DMD.CallManagers.AsteriskCallManager(Me.UserName, Me.Password, Me.ServerName, Me.ServerPort)

        Me.m_Manager.Start()
        Me.m_Manager.Login()

        Me.m_Timer.Enabled = True
    End Sub

    Public Sub Disconnect()
        If Not Me.IsConnected Then Return
        Me.m_Timer.Enabled = False

        Me.m_Manager.Stop()
        Me.m_Manager = Nothing

    End Sub


    Private Sub handleAsteriskEvent(sender As Object, e As AsteriskEvent) Handles m_Manager.ManagerEvent
        RaiseEvent ManagerEvent(sender, e)
    End Sub


    Private Sub handleAsteriskConnect(sender As Object, e As AsteriskEventArgs) Handles m_Manager.Connected
        RaiseEvent Connected(sender, e)
    End Sub

    Private Sub handleAsteriskDisconnect(sender As Object, e As AsteriskEventArgs) Handles m_Manager.Disconnected
        RaiseEvent Disconnected(sender, e)
    End Sub

    Private Sub m_Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_Timer.Elapsed
        'If Me.IsConnected Then
        '    Try

        '        SyncLock Me
        '            Me.Disconnect()
        '            Me.Connect()
        '        End SyncLock
        '    Catch ex As Exception
        '        DMD.Sistema.Events.NotifyUnhandledException(ex)
        '    End Try
        'End If

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If (obj Is Me) Then Return True
        If Not (TypeOf (obj) Is AsteriskServer) Then Return False
        With DirectCast(obj, AsteriskServer)
            Return Me.Nome = .Nome AndAlso
                   Me.ServerName = .ServerName AndAlso
                   Me.ServerPort = .ServerPort AndAlso
                   Me.UserName = .UserName AndAlso
                   Me.Password = .Password AndAlso
                   Me.Channel = .Channel AndAlso
                   Me.CallerID = .CallerID
        End With
    End Function

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements DMD.XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "servername" : Me.ServerName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "serverport" : Me.ServerPort = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "username" : Me.UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "password" : Me.Password = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "channel" : Me.Channel = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "callerid" : Me.CallerID = XML.Utils.Serializer.DeserializeString(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As DMD.XML.XMLWriter) Implements DMD.XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("nome", Me.Nome)
        writer.WriteAttribute("servername", Me.ServerName)
        writer.WriteAttribute("serverport", Me.ServerPort)
        writer.WriteAttribute("username", Me.UserName)
        writer.WriteAttribute("password", Me.Password)
        writer.WriteAttribute("channel", Me.Channel)
        writer.WriteAttribute("callerid", Me.CallerID)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    'Private Sub ReadXml(reader As Xml.XmlReader) Implements Xml.Serialization.IXmlSerializable.ReadXml
    '    Me.Nome = reader.GetAttribute("nome")
    '    Me.ServerName = reader.GetAttribute("servername")
    '    Me.ServerPort = CInt(reader.GetAttribute("serverport"))
    '    Me.UserName = reader.GetAttribute("username")
    '    Me.Password = reader.GetAttribute("password")
    '    Me.Channel = reader.GetAttribute("channel")
    '    Me.CallerID = reader.GetAttribute("callerid")
    '    reader.Read()
    'End Sub

    'Public Sub WriteXml(writer As Xml.XmlWriter) Implements Xml.Serialization.IXmlSerializable.WriteXml
    '    writer.WriteAttributeString("nome", Me.Nome)
    '    writer.WriteAttributeString("servername", Me.ServerName)
    '    writer.WriteAttributeString("serverport", CStr(Me.ServerPort))
    '    writer.WriteAttributeString("username", Me.UserName)
    '    writer.WriteAttributeString("password", Me.Password)
    '    writer.WriteAttributeString("channel", Me.Channel)
    '    writer.WriteAttributeString("callerid", Me.CallerID)
    'End Sub
End Class

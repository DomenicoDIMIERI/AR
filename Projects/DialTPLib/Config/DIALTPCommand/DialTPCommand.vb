Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases


''' <summary>
''' Rappresenta un coamndo inviato ad un sistema remoto
''' </summary>
Public NotInheritable Class DialTPCommand
    Inherits DBObjectBase

    Public IDPostazione As String        'Stringa che identifica la postazione a cui fa riferimento la configurazione
    Public IDMacchina As String          'Stringa che identifica la macchina a cui fa riferimento la configurazione
    Public IDUtente As String            'Strings che identifica l'utente collegato alla macchina

    Public Name As String                        'Nome del comando da eseguire
    Public Parameters As New CKeyCollection          'Parametri del comando
    Public ProgramedTime As DateTime?           'Data e ora in cui eseguire il comando
    Public RunTime As DateTime?                   'Data in cui il comando è stato eseguito
    Public Results As New CKeyCollection             'Risultati del comando 


    Public Sub New()
        Me.IDPostazione = ""
        Me.IDMacchina = ""
        Me.IDUtente = ""
        Me.Name = ""
        Me.Parameters = New CKeyCollection
        Me.ProgramedTime = Nothing
        Me.RunTime = Nothing
        Me.Results = New CKeyCollection
    End Sub

    Protected Overrides Function GetConnection() As CDBConnection
        Return DialTPApp.Database
    End Function

    Public Overrides Function GetModule() As DMD.Sistema.CModule
        Return Nothing
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_DialTPCmd"
    End Function

    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Me.Name = reader.Read("IDPostazione", Me.IDPostazione)
        Me.Name = reader.Read("IDMacchina", Me.IDMacchina)
        Me.Name = reader.Read("IDUtente", Me.IDUtente)
        Me.Name = reader.Read("Name", Me.Name)
        Me.Parameters = XML.Utils.Serializer.Deserialize(reader.Read("Parameters", ""))
        Me.ProgramedTime = reader.Read("ProgramedTime", Me.ProgramedTime)
        Me.RunTime = reader.Read("RunTime", Me.RunTime)
        Me.Results = XML.Utils.Serializer.Deserialize(reader.Read("Results", ""))
        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        writer.Write("IDPostazione", Me.IDPostazione)
        writer.Write("IDMacchina", Me.IDMacchina)
        writer.Write("IDUtente", Me.IDUtente)
        writer.Write("Name", Me.Name)
        writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
        writer.Write("ProgramedTime", Me.ProgramedTime)
        writer.Write("RunTime", Me.RunTime)
        writer.Write("Results", XML.Utils.Serializer.Serialize(Me.Results))
        Return MyBase.SaveToRecordset(writer)
    End Function

    Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        writer.WriteAttribute("IDPostazione", Me.IDPostazione)
        writer.WriteAttribute("IDMacchina", Me.IDMacchina)
        writer.WriteAttribute("IDUtente", Me.IDUtente)
        writer.WriteAttribute("Name", Me.Name)
        writer.WriteAttribute("ProgramedTime", Me.ProgramedTime)
        writer.WriteAttribute("RunTime", Me.RunTime)
        MyBase.XMLSerialize(writer)
        writer.WriteTag("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
        writer.WriteTag("Results", XML.Utils.Serializer.Serialize(Me.Results))
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "IDPostazione" : Me.IDPostazione = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "IDMacchina" : Me.IDMacchina = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "IDUtente" : Me.IDUtente = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Name" : Me.Name = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "ProgramedTime" : Me.ProgramedTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "RunTime" : Me.RunTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "Parameters" : Me.Parameters = XML.Utils.Serializer.ToObject(fieldValue)
            Case "Results" : Me.Results = XML.Utils.Serializer.ToObject(fieldValue)
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select
    End Sub

    Public Sub Execute()
        DIALTPLib.Log.LogMessage("DialTPCommand.Execute: " & Me.Name)
        Select Case (Strings.LCase(Strings.Trim(Me.Name)))
            Case "shutdown"
                System.Diagnostics.Process.Start("shutdown", "-s -t 00")
                Me.Results.SetItemByKey("[default]", "ok")
            Case "forceupdate"
                DialTPApp.InstallUpdateSyncWithInfo(True)
                Me.Results.SetItemByKey("[default]", "ok")
            Case "ffmpeg_streammic"
                Dim micName As String = CStr(Me.Parameters.GetItemByKey("micname"))
                Dim udpAddress As String = CStr(Me.Parameters.GetItemByKey("updaddress"))
                'ffmpeg -f dshow -i audio="USB Mic (2- Samson GoMic)" -c:a libmp3lame -f mpegts udp://192.168.0.255:12345
                System.Diagnostics.Process.Start("ffmpeg", "-f dshow -i audio=""" & Replace(micName, Chr(34), "'") & """ -c:a libmp3lame -f mpegts udp://" & udpAddress)
                Me.Results.SetItemByKey("[default]", "ok")
        End Select

    End Sub


End Class

Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    <Serializable> _
    Public Class CEventLog
        Inherits DBObjectBase

        Private m_Data As Date 'Data e ora in cui si è verificato l'evento
        Private m_Source As String 'Nome del modulo che ha generato l'evento
        Private m_UserID As Integer 'ID dell'utente nel cui contesto si è verificato l'evento
        Private m_User As CUser 'Utente nel cui contesto si è verificato l'evento
        Private m_UserName As String 'Nome dell'operatore	
        Private m_EventName As String 'Nome dell'evento
        Private m_Description As String 'Descrizione dell'evento
        Private m_Parameters As String 'Parametri dell'evento

        Public Sub New()
        End Sub

        Public Sub New(ByVal data As Date, ByVal source As String, ByVal user As CUser, ByVal eventName As String, ByVal description As String)
            Me.New(data, source, user, eventName, description, "")
        End Sub

        Public Sub New(ByVal e As EventDescription)
            Me.New(e.Data, e.Module.ModuleName, e.Utente, e.EventName, e.Descrizione, e.Descrittore)
        End Sub

        Public Sub New(ByVal data As Date, ByVal source As String, ByVal user As CUser, ByVal eventName As String, ByVal description As String, ByVal parameters As Object)
            Me.m_Data = data
            Me.m_Source = source
            Me.m_UserID = GetID(user)
            Me.m_User = user
            Me.m_UserName = user.Nominativo
            Me.m_EventName = eventName
            Me.m_Description = description
            If (parameters Is Nothing) Then
                m_Parameters = ""
            ElseIf TypeOf (parameters) Is String Then
                Me.m_Parameters = CStr(parameters)
            ElseIf TypeOf (parameters) Is IEnumerable Then
                Dim tmp As New System.Text.StringBuilder
                For Each item As Object In parameters
                    If tmp.Length > 0 Then tmp.Append(", ")
                    If (item Is Nothing) Then
                        tmp.Append("NULL")
                    Else
                        tmp.Append(item.ToString)
                    End If
                Next
                Me.m_Parameters = "{ " & tmp.ToString & " }"
            Else
                Me.m_Parameters = parameters.ToString
            End If

        End Sub

        Public Overrides Function GetModule() As CModule
            Return Events.Module
        End Function



        ''' <summary>
        ''' Data e ora in cui è stato generato l'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Data As Date
            Get
                Return Me.m_Data
            End Get
        End Property

        ''' <summary>
        ''' Nome della classe che ha generato l'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Source As String
            Get
                Return Me.m_Source
            End Get
        End Property

        ''' <summary>
        ''' Utente nel cui contesto è stato generato l'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
        End Property

        Public ReadOnly Property UserDisplayName As String
            Get
                Return Me.m_UserName
            End Get
        End Property

        ''' <summary>
        ''' Nome dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EventName As String
            Get
                Return Me.m_EventName
            End Get
        End Property

        ''' <summary>
        ''' Descrizione dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Description As String
            Get
                Return Me.m_Description
            End Get
        End Property

        ''' <summary>
        ''' Parametri
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Parameters As String
            Get
                Return Me.m_Parameters
            End Get
        End Property

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_Data", Me.m_Data)
            writer.WriteAttribute("m_Source", Me.m_Source)
            writer.WriteAttribute("m_UserID", Me.m_UserID)
            writer.WriteAttribute("m_UserName", Me.m_UserName)
            writer.WriteAttribute("m_EventName", Me.m_EventName)

            MyBase.XMLSerialize(writer)

            writer.WriteTag("m_Description", Me.m_Description)
            writer.WriteTag("m_Parameters", Me.m_Parameters)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "m_Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Source" : Me.m_Source = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_UserID" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_EventName" : Me.m_EventName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Description" : Me.m_Description = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Parameters" : Me.m_Parameters = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_EventsLog"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.LOGConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_Source = reader.Read("Source", Me.m_Source)
            Me.m_UserID = reader.Read("User", Me.m_UserID)
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_EventName = reader.Read("EventName", Me.m_EventName)
            Me.m_Description = reader.Read("Description", Me.m_Description)
            Me.m_Parameters = reader.Read("Parameters", Me.m_Parameters)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Data", Me.m_Data)
            writer.Write("Source", Me.m_Source)
            writer.Write("User", GetID(Me.m_User, Me.m_UserID))
            writer.Write("UserName", Me.m_UserName)
            writer.Write("EventName", Me.m_EventName)
            writer.Write("Description", Me.m_Description)
            writer.Write("Parameters", Me.m_Parameters)
            Return MyBase.SaveToRecordset(writer)
        End Function


    End Class

End Class
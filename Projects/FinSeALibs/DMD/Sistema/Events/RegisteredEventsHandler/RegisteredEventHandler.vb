Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Registrazione di un handler di un evento
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class RegisteredEventHandler
        Inherits DBObjectBase

        Private m_Active As Boolean
        Private m_ModuleID As Integer
        Private m_ModuleName As String
        Private m_Module As CModule
        Private m_EventName As String
        Private m_Handler As IEventHandler
        Private m_Priority As Integer
        Private m_ClassName As String

        Public Sub New()
            Me.m_Active = True
            Me.m_ModuleID = 0
            Me.m_Module = Nothing
            Me.m_EventName = ""
            Me.m_Handler = Nothing
            Me.m_ModuleName = ""
            Me.m_ClassName = ""
            Me.m_Priority = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'evento è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Active As Boolean
            Get
                Return Me.m_Active
            End Get
            Set(value As Boolean)
                If (value = Me.m_Active) Then Exit Property
                Me.m_Active = value
                Me.DoChanged("Active", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del modulo di cui l'handler gestisce l'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ModuleID As Integer
            Get
                Return GetID(Me.m_Module, Me.m_ModuleID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ModuleID
                If (oldValue = value) Then Exit Property
                Me.m_ModuleID = value
                Me.m_Module = Nothing
                Me.DoChanged("ModuleID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modulo di cui l'handler gestisce l'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property [Module] As CModule
            Get
                If (Me.m_Module Is Nothing) Then Me.m_Module = Modules.GetItemById(Me.m_ModuleID)
                Return Me.m_Module
            End Get
            Set(value As CModule)
                Dim oldValue As CModule = Me.Module
                If (oldValue Is value) Then Exit Property
                Me.m_Module = value
                Me.m_ModuleID = GetID(value)
                Me.m_ModuleName = ""
                If (value IsNot Nothing) Then Me.m_ModuleName = value.ModuleName
                Me.DoChanged("Module", value)
            End Set
        End Property

        Public Property ModuleName As String
            Get
                Return Me.m_ModuleName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ModuleName
                If (oldValue = value) Then Exit Property
                Me.m_ModuleName = value
                Me.DoChanged("ModuleName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'evento gestito da questo handler
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EventName As String
            Get
                Return Me.m_EventName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_EventName
                If (oldValue = value) Then Exit Property
                Me.m_EventName = value
                Me.DoChanged("EventName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'handler
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateHandler() As Object
            If (Me.m_Handler Is Nothing) Then
                Try
                    Me.m_Handler = Sistema.Types.CreateInstance(Me.m_ClassName)
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(New Exception("Non riesco a creare l'handler [" & Me.m_ClassName & "] per l'azione (" & Me.ModuleName & ", " & Me.m_EventName & ")"))
                End Try
            End If
            Return Me.m_Handler
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome della classe gestore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClassName As String
            Get
                Return Me.m_ClassName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ClassName
                If (oldValue = value) Then Exit Property
                Me.m_ClassName = value
                Me.m_Handler = Nothing
                Me.DoChanged("ClassName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un numero che indica la priorità di esecuzione del gestore. Numero maggiori indicano priorità minore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priority As Integer
            Get
                Return Me.m_Priority
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Priority
                If (oldValue = value) Then Exit Property
                Me.m_Priority = value
                Me.DoChanged("Priority", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Sistema.RegisteredEventHandlers.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EventsHandlers"
        End Function

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append("{ ")
            If (Me.Module IsNot Nothing) Then
                ret.Append(Me.Module.ModuleName)
            Else
                ret.Append("Modulo non valido: " & Me.ModuleID)
            End If
            ret.Append(", ")
            ret.Append(Me.m_EventName)
            ret.Append(" }")
            Return ret.ToString
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Active = reader.Read("Active", Me.m_Active)
            Me.m_ModuleID = reader.Read("Module", Me.m_ModuleID)
            Me.m_ModuleName = reader.Read("ModuleName", Me.m_ModuleName)
            Me.m_EventName = reader.Read("EventName", Me.m_EventName)
            Me.m_ClassName = reader.Read("ClassName", Me.m_ClassName)
            Me.m_Priority = reader.Read("Priority", Me.m_Priority)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Active", Me.m_Active)
            writer.Write("Module", Me.ModuleID)
            writer.Write("ModuleName", Me.m_ModuleName)
            writer.Write("EventName", Me.m_EventName)
            writer.Write("ClassName", Me.m_ClassName)
            writer.Write("Priority", Me.m_Priority)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Active", Me.m_Active)
            writer.WriteAttribute("Module", Me.ModuleID)
            writer.WriteAttribute("ModuleName", Me.m_ModuleName)
            writer.WriteAttribute("EventName", Me.m_EventName)
            writer.WriteAttribute("ClassName", Me.m_ClassName)
            writer.WriteAttribute("Priority", Me.m_Priority)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Active" : Me.m_Active = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Module" : Me.m_ModuleID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ModuleName" : Me.m_ModuleName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EventName" : Me.m_EventName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ClassName" : Me.m_ClassName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Priority" : Me.m_Priority = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub



        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            Sistema.RegisteredEventHandlers.UpdateCached(Me)
            Return ret
        End Function


    End Class

End Class
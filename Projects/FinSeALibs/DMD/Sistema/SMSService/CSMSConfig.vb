Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 

Partial Public Class Sistema

    <Serializable> _
    Public Class CSMSConfig
        Inherits DBObjectBase

        Private m_DefaultSenderName As String
        Private m_DefaultSenderNumber As String
        Private m_DefaultDriverName As String

        Public Sub New()
            Me.m_DefaultDriverName = ""
            Me.m_DefaultSenderName = ""
            Me.m_DefaultSenderNumber = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del driver predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultDriverName As String
            Get
                Return Me.m_DefaultDriverName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DefaultDriverName
                If (oldValue = value) Then Exit Property
                Me.m_DefaultDriverName = value
                Me.DoChanged("DefaultDriverName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mittente predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultSenderName As String
            Get
                Return Me.m_DefaultSenderName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DefaultSenderName
                If (oldValue = value) Then Exit Property
                Me.m_DefaultSenderName = value
                Me.DoChanged("DefaultSenderName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del mittente predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultSenderNumbmer As String
            Get
                Return Me.m_DefaultSenderNumber
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DefaultSenderNumber
                If (oldValue = value) Then Exit Property
                Me.m_DefaultSenderNumber = value
                Me.DoChanged("DefaultSenderNumber", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SMSConfig"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_DefaultDriverName = reader.Read("DefaultDriverName", Me.m_DefaultDriverName)
            Me.m_DefaultSenderName = reader.Read("DefaultSenderName", Me.m_DefaultSenderName)
            Me.m_DefaultSenderNumber = reader.Read("DefaultSenderNumber", Me.m_DefaultSenderNumber)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("DefaultDriverName", Me.m_DefaultDriverName)
            writer.Write("DefaultSenderName", Me.m_DefaultSenderName)
            writer.Write("DefaultSenderNumber", Me.m_DefaultSenderNumber)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DefaultDriverName" : Me.m_DefaultDriverName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DefaultSenderName" : Me.m_DefaultSenderName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DefaultSenderNumber" : Me.m_DefaultSenderNumber = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("DefaultDriverName", Me.m_DefaultDriverName)
            writer.WriteAttribute("DefaultSenderName", Me.m_DefaultSenderName)
            writer.WriteAttribute("DefaultSenderNumber", Me.m_DefaultSenderNumber)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Sub Load()
            Dim reader As DBReader
            Dim dbSQL As String = "SELECT * FROM [" & Me.GetTableName & "] ORDER BY [ID] ASC"
            reader = New DBReader(APPConn.Tables(Me.GetTableName), dbSQL)
            If reader.Read Then
                APPConn.Load(Me, reader)
            End If
            reader.Dispose()
        End Sub

        'Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
        '    MyBase.DoChanged(propName, newVal, oldVal)
        '    FaxService.doConfigChanged(New System.EventArgs)
        'End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Sistema.SMSService.SetConfig(Me)
        End Sub


    End Class


End Class
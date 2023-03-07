Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    ''' <summary>
    ''' Rappresenta una stanza della chat
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CChatRoom
        Inherits DBObjectBase

        Private m_Name As String

        Public Sub New()
            Me.m_Name = ""
        End Sub

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Messenger.Rooms.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ChatRooms"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
 
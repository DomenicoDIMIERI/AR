Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    <Flags> _
    Public Enum StickyNoteFlags As Integer
        None = 0
        TopMost = 1
    End Enum

    <Serializable> _
    Public Class StickyNote
        Inherits DBObjectPO

        Private m_Text As String
        Private m_Flags As StickyNoteFlags
        Private m_Attributes As CKeyCollection
        
        Public Sub New()
            Me.m_Text = ""
            Me.m_Flags = StickyNoteFlags.None
            Me.m_Attributes = Nothing
        End Sub



        Public Property Text As String
            Get
                Return Me.m_Text
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Text
                If (oldValue = value) Then Exit Property
                Me.m_Text = value
                Me.DoChanged("Text", value, oldValue)
            End Set
        End Property

        Public Property Flags As StickyNoteFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As StickyNoteFlags)
                Dim oldValue As StickyNoteFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Attributes As CKeyCollection
            Get
                If (Me.m_Attributes Is Nothing) Then Me.m_Attributes = New CKeyCollection
                Return Me.m_Attributes
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return StickyNotes.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeStickyNotes"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Text = reader.Read("Text", Me.m_Text)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Try
                Me.m_Attributes = XML.Utils.Serializer.Deserialize(reader.Read("Attributes", ""))
            Catch ex As Exception
                Me.m_Attributes = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Text", Me.m_Text)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributes", XML.Utils.Serializer.Serialize(Me.Attributes))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Text", Me.m_Text)
            writer.WriteTag("Attributes", Me.Attributes)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Text" : Me.m_Text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attributes" : Me.m_Attributes = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Text
        End Function

    End Class


End Class
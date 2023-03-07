Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports System.Drawing

Partial Class Office


    <Serializable>
    Public Class DocumentTemplate
        Inherits DBObjectPO

        Private m_Name As String
        Private m_SourceFile As String
        Private m_Description As String
        Private m_PageFormatName As String
        Private m_PageFormat As CSize
        Private m_TemplateItems As TemplateItemsCollection
        Private m_ContextType As String

        Public Sub New()
            Me.m_Name = ""
            Me.m_SourceFile = ""
            Me.m_Description = ""
            Me.m_PageFormatName = ""
            Me.m_PageFormat = New CSize(0, 0)
            Me.m_TemplateItems = Nothing
            Me.m_ContextType = ""
        End Sub

        Public Property ContextType As String
            Get
                Return Me.m_ContextType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ContextType
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ContextType = value
                Me.DoChanged("ContextType", value, oldValue)
            End Set
        End Property

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        Public Property SourceFile As String
            Get
                Return Me.m_SourceFile
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SourceFile
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_SourceFile = value
                Me.DoChanged("SourceValue", value, oldValue)
            End Set
        End Property

        Public Property Description As String
            Get
                Return Me.m_Description
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Description
                If (oldValue = value) Then Exit Property
                Me.m_Description = value
                Me.DoChanged("Description", value, oldValue)
            End Set
        End Property

        Public Property PageFormatName As String
            Get
                Return Me.m_PageFormatName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_PageFormatName
                If (oldValue = value) Then Exit Property
                Me.m_PageFormatName = value
                Me.DoChanged("PageFormatName", value, oldValue)
            End Set
        End Property

        Public Property PageFormat As CSize
            Get
                Return Me.m_PageFormat
            End Get
            Set(value As CSize)
                Dim oldValue As CSize = Me.m_PageFormat
                If (Object.ReferenceEquals(oldValue, value)) Then Exit Property
                Me.m_PageFormat = value
                Me.DoChanged("PageFormat", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Items As TemplateItemsCollection
            Get
                If Me.m_TemplateItems Is Nothing Then Me.m_TemplateItems = New TemplateItemsCollection(Me)
                Return Me.m_TemplateItems
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Name
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.Templates.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DocumentiTemplates"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_ContextType = reader.Read("ContextType", Me.m_ContextType)
            Me.m_Description = reader.Read("Description", Me.m_Description)
            Me.m_PageFormatName = reader.Read("PageFormatName", Me.m_PageFormatName)
            Me.m_SourceFile = reader.Read("SourceFile", Me.m_SourceFile)
            Dim w As Single = reader.Read("PageFormatWidth", 0)
            Dim h As Single = reader.Read("PageFormatHeight", 0)
            Me.m_PageFormat = New CSize(w, h)
            Dim txt As String = reader.Read("TemplateItems", "")
            If (txt <> "") Then
                Me.m_TemplateItems = XML.Utils.Serializer.Deserialize(txt)
                Me.m_TemplateItems.SetDocument(Me)
            End If
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("Description", Me.m_Description)
            writer.Write("ContextType", Me.m_ContextType)
            writer.Write("PageFormatName", Me.m_PageFormatName)
            writer.Write("PageFormatWidth", Me.m_PageFormat.Width)
            writer.Write("PageFormatHeight", Me.m_PageFormat.Height)
            writer.Write("SourceFile", Me.m_SourceFile)
            writer.Write("TemplateItems", XML.Utils.Serializer.Serialize(Me.Items))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("Description", Me.m_Description)
            writer.WriteAttribute("ContextType", Me.m_ContextType)
            writer.WriteAttribute("PageFormatName", Me.m_PageFormatName)
            writer.WriteAttribute("SourceFile", Me.m_SourceFile)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("PageFormat", Me.PageFormat)
            writer.WriteTag("TemplateItems", Me.Items)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceFile" : Me.m_SourceFile = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Description" : Me.m_Description = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContextType" : Me.m_ContextType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PageFormatName" : Me.m_PageFormatName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PageFormat" : Me.m_PageFormat = fieldValue
                Case "TemplateItems"
                    Me.m_TemplateItems = New TemplateItemsCollection(Me)
                    For Each item As TemplateItem In fieldValue
                        Me.m_TemplateItems.Add(item)
                    Next
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Sub RenderToPDFFile(ByVal context As Object, ByVal fileName As String)

        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Office.Templates.UpdateCached(Me)
        End Sub

    End Class

End Class
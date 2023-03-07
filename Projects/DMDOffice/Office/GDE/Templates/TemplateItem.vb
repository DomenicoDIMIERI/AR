Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports System.Drawing
Imports DMD.XML

Partial Class Office

    Public Enum TemplateItemTypes
        NoOperation = 0
        TEXTOUT = 1
        DRAWRECT = 2
        FILLRECT = 3
        DRAWELLIPSE = 4
        FILLELLIPSE = 5
        DRAWIMAGE = 6
        NEWPAGE = 7
        DATAFIELD = 8
        EXPRESSION = 9
        GOTOPAGE = 10
    End Enum

    <Serializable>
    Public Class TemplateItem
        Inherits DBObjectBase

        Private m_ItemType As TemplateItemTypes
        Private m_Color As String
        Private m_Bounds As CRectangle
        Private m_Text As String
        Private m_Parameters As CKeyCollection

        Public Sub New()
            Me.m_ItemType = TemplateItemTypes.NoOperation
            Me.m_Color = ""
            Me.m_Bounds = New CRectangle(0, 0, 0, 0)
            Me.m_Text = ""
            Me.m_Parameters = Nothing
        End Sub

        Public Sub New(ByVal type As TemplateItemTypes, ByVal text As String, ByVal bounds As CRectangle)
            Me.New()
            Me.m_ItemType = type
            Me.m_Text = text
            Me.m_Bounds = bounds
            Me.m_Color = "#000000"
        End Sub

        Public Sub New(ByVal type As TemplateItemTypes, ByVal text As String, ByVal bounds As CRectangle, ByVal color As String)
            Me.New()
            Me.m_ItemType = type
            Me.m_Text = text
            Me.m_Bounds = bounds
            Me.m_Color = color
        End Sub

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Property ItemType As TemplateItemTypes
            Get
                Return Me.m_ItemType
            End Get
            Set(value As TemplateItemTypes)
                Dim oldValue As TemplateItemTypes = Me.m_ItemType
                If (oldValue = value) Then Exit Property
                Me.m_ItemType = value
                Me.DoChanged("ItemType", value, oldValue)
            End Set
        End Property

        Public Property Color As String
            Get
                Return Me.m_Color
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Color
                value = Strings.Trim(value)
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_Color = value
                Me.DoChanged("Color", value, oldValue)
            End Set
        End Property

        Public Property Bounds As CRectangle
            Get
                Return Me.m_Bounds
            End Get
            Set(value As CRectangle)
                Dim oldValue As CRectangle = Me.m_Bounds
                If (Object.ReferenceEquals(oldValue, value)) Then Exit Property
                Me.m_Bounds = value
                Me.DoChanged("Bounds", value, oldValue)
            End Set
        End Property

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

        Public Overrides Function ToString() As String
            Return [Enum].GetName(GetType(TemplateItemTypes), Me.m_ItemType) & "/" & Me.m_Bounds.ToString & "/" & Me.m_Text
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DocumentiTemplateItems"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_ItemType = reader.Read("ItemType", Me.m_ItemType)
            Me.m_Color = reader.Read("Color", Me.m_Color)
            Dim x, y, w, h As Single
            x = reader.Read("x", x)
            y = reader.Read("y", y)
            w = reader.Read("w", w)
            h = reader.Read("h", h)
            Me.m_Bounds = New CRectangle(x, y, w, h)
            Me.m_Text = reader.Read("Text", Me.m_Text)
            Dim txt As String = reader.Read("Params", "")
            If (txt <> "") Then Me.m_Parameters = XML.Utils.Serializer.Deserialize(txt)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("ItemType", Me.m_ItemType)
            writer.Write("Color", Me.m_Color)
            writer.Write("x", Me.Bounds.Left)
            writer.Write("y", Me.Bounds.Top)
            writer.Write("w", Me.Bounds.Width)
            writer.Write("h", Me.Bounds.Height)
            writer.Write("Text", Me.m_Text)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("ItemType", Me.m_ItemType)
            writer.WriteAttribute("Color", Me.m_Color)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Bounds", Me.Bounds)
            writer.WriteTag("Params", Me.Parameters)
            writer.WriteTag("Text", Me.m_Text)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "ItemType" : Me.m_ItemType = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Color" : Me.m_Color = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Bounds" : Me.m_Bounds = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Text" : Me.m_Text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Params" : Me.m_Parameters = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

    End Class


End Class
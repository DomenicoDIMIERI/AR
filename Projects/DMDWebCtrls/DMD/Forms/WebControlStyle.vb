Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases


Namespace Forms

    <Serializable> _
    Public Class WebControlStyle
        Implements DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Owner As WebControl
        Public Position As String
        Private m_Left As String
        Private m_Top As String
        Private m_Bottom As String
        Private m_Right As String
        Private m_Width As String
        Private m_Height As String
        Public Background As WebControlBackground
        Public Border As WebControlBorder
        Public Color As String
        Public TextAlign As String
        Public zIndex As String
        Public Visibility As String
        Public Overflow As String
        Public WhiteSpace As String
        Public MinWidth As String
        Public MaxWidth As String
        Public MinHeight As String
        Public MaxHeight As String
        Public Display As String
        Public VerticalAlign As String = ""

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Background = New WebControlBackGround(Me)
            Me.Border = New WebControlBorder(Me)
        End Sub

        Public Sub New(ByVal owner As WebControl)
            Me.New()
            Me.m_Owner = owner
        End Sub

        Public Property Left As String
            Get
                Return Me.m_Left
            End Get
            Set(value As String)
                Me.m_Left = NormalizeSize(value)
            End Set
        End Property

        Public Property Top As String
            Get
                Return Me.m_Top
            End Get
            Set(value As String)
                Me.m_Top = NormalizeSize(value)
            End Set
        End Property

        Public Property Right As String
            Get
                Return Me.m_Right
            End Get
            Set(value As String)
                Me.m_Right = NormalizeSize(value)
            End Set
        End Property

        Public Property Bottom As String
            Get
                Return Me.m_Bottom
            End Get
            Set(value As String)
                Me.m_Bottom = NormalizeSize(value)
            End Set
        End Property

        Public Property Width As String
            Get
                Return Me.m_Width
            End Get
            Set(value As String)
                Me.m_Width = NormalizeSize(value)
            End Set
        End Property

        Public Property Height As String
            Get
                Return Me.m_Height
            End Get
            Set(value As String)
                Me.m_Height = NormalizeSize(value)
            End Set
        End Property

        Private Function NormalizeSize(ByVal value As String) As String
            value = Trim(value)
            If (value = "") Then Return ""
            Dim ch As String = Strings.Right(value, 1)
            If (ch >= "0" AndAlso ch <= "9") Then value = value & "px"
            Return value
        End Function

        Public Overrides Function ToString() As String
            Dim ret As String = vbNullString
            Dim tmp As String
            If (Me.TextAlign <> "") Then ret = Strings.Combine(ret, "text-align:" & Me.TextAlign, ";")
            If (Me.Position <> "") Then ret = Strings.Combine(ret, "position:" & Me.Position, ";")
            If (Me.Left <> "") Then ret = Strings.Combine(ret, "left:" & Me.Left, ";")
            If (Me.Top <> "") Then ret = Strings.Combine(ret, "top:" & Me.Top, ";")
            If (Me.Right <> "") Then ret = Strings.Combine(ret, "right:" & Me.Right, ";")
            If (Me.Bottom <> "") Then ret = Strings.Combine(ret, "bottom:" & Me.Bottom, ";")
            If (Me.Width <> "") Then ret = Strings.Combine(ret, "width:" & Me.Width, ";")
            If (Me.Height <> "") Then ret = Strings.Combine(ret, "height:" & Me.Height, ";")
            If (Me.Overflow <> "") Then ret = Strings.Combine(ret, "overflow:" & Me.Overflow, ";")
            If (Me.WhiteSpace <> "") Then ret = Strings.Combine(ret, "white-space:" & Me.WhiteSpace, ";")
            If (Me.MinWidth <> "") Then ret = Strings.Combine(ret, "min-width:" & Me.MinWidth, ";")
            If (Me.MaxWidth <> "") Then ret = Strings.Combine(ret, "max-width:" & Me.MaxWidth, ";")
            If (Me.MinHeight <> "") Then ret = Strings.Combine(ret, "min-height:" & Me.MinHeight, ";")
            If (Me.MaxHeight <> "") Then ret = Strings.Combine(ret, "max-height:" & Me.MaxHeight, ";")
            If (Me.Display <> "") Then ret = Strings.Combine(ret, "display:" & Me.Display, ";")
            If (Me.VerticalAlign <> "") Then ret = Strings.Combine(ret, "vertical-align:" & Me.VerticalAlign, ";")

            tmp = Me.Border.ToString
            If tmp <> vbNullString Then ret = Strings.Combine(ret, "border:" & tmp, ";")
            tmp = Me.Background.ToString
            If (tmp <> vbNullString) Then ret = Strings.Combine(ret, "background:" & tmp, ";")
            If (Me.Color <> vbNullString) Then ret = Strings.Combine(ret, "color:" & Me.Color, ";")
            If (Me.zIndex <> vbNullString) Then ret = Strings.Combine(ret, "z-index:" & Me.zIndex, ";")
            If (Me.Visibility <> vbNullString) Then ret = Strings.Combine(ret, "visibility:" & Me.Visibility, ";")
            Return ret
        End Function

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Position" : Me.Position = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Left" : Me.Left = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Top" : Me.Top = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Bottom" : Me.Bottom = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Right" : Me.Right = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Width" : Me.Width = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Height" : Me.Height = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Color" : Me.Color = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TextAlign" : Me.TextAlign = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "zIndex" : Me.zIndex = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Visibility" : Me.Visibility = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Overflow" : Me.Overflow = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "WhiteSpace" : Me.WhiteSpace = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MinWidth" : Me.MinWidth = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MaxWidth" : Me.MaxWidth = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MinHeight" : Me.MinHeight = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MaxHeight" : Me.MaxHeight = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Display" : Me.Display = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "VerticalAlign" : Me.VerticalAlign = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Background" : Me.Background = fieldValue
                Case "Border" : Me.Border = fieldValue
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Position", Me.Position)
            writer.WriteAttribute("Left", Me.Left)
            writer.WriteAttribute("Top", Me.Top)
            writer.WriteAttribute("Bottom", Me.Bottom)
            writer.WriteAttribute("Right", Me.Right)
            writer.WriteAttribute("Width", Me.Width)
            writer.WriteAttribute("Height", Me.Height)
            writer.WriteAttribute("Color", Me.Color)
            writer.WriteAttribute("TextAlign", Me.TextAlign)
            writer.WriteAttribute("zIndex", Me.zIndex)
            writer.WriteAttribute("Visibility", Me.Visibility)
            writer.WriteAttribute("Overflow", Me.Overflow)
            writer.WriteAttribute("WhiteSpace", Me.WhiteSpace)
            writer.WriteAttribute("MinWidth", Me.MinWidth)
            writer.WriteAttribute("MaxWidth", Me.MaxWidth)
            writer.WriteAttribute("MinHeight", Me.MinHeight)
            writer.WriteAttribute("MaxHeight", Me.MaxHeight)
            writer.WriteAttribute("Display", Me.Display)
            writer.WriteAttribute("VerticalAlign", Me.VerticalAlign)
            writer.WriteTag("Background", Me.Background)
            writer.WriteTag("Border", Me.Border)
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal owner As WebControl)
            Me.m_Owner = owner
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace
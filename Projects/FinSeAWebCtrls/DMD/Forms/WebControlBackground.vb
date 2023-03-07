Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases


Namespace Forms

    <Serializable> _
    Public Class WebControlBackGround
        Implements DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Owner As WebControlStyle
        Private m_Color As String
        Public Image As String
        Public Repeat As String
        Public Attachment As String
        Public Position As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal owner As WebControlStyle)
            Me.New()
            Me.m_Owner = owner
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String = vbNullString
            If Me.m_Color <> vbNullString Then ret = Strings.Combine(ret, Me.m_Color, " ")
            If Me.Image <> vbNullString Then ret = Strings.Combine(ret, Me.Image, " ")
            If Me.Repeat <> vbNullString Then ret = Strings.Combine(ret, Me.Repeat, " ")
            If Me.Attachment <> vbNullString Then ret = Strings.Combine(ret, Me.Attachment, " ")
            If Me.Position <> vbNullString Then ret = Strings.Combine(ret, Me.Position, " ")
            '#00ff00 url('smiley.gif') no-repeat fixed center; 
            Return ret
        End Function

        Public Property Color As String
            Get
                Return Me.m_Color
            End Get
            Set(value As String)
                Me.m_Color = value
            End Set
        End Property

        'Public Shared Widening Operator CType(ByVal value As String) As WebControlBackground
        '    Dim items() As String = Split(Replace(Trim(value), "  ", " "), " ")
        '    Dim ret As New WebControlBackground
        '    For i = 0 To UBound(items)
        '        If Left(items(i), 1) = "#" Then
        '            ret.Color = items(i)
        '        ElseIf Left(items(i), 1) >= "0" And Left(items(i), 1) <= "9" Then
        '            ret.Size = items(i)
        '        Else
        '            ret.Style = items(i)
        '        End If
        '    Next
        '    Return ret
        'End Operator


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Color" : Me.Color = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Image" : Me.Image = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Repeat" : Me.Repeat = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attachment" : Me.Attachment = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Position" : Me.Position = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Color", Me.Color)
            writer.WriteAttribute("Image", Me.Image)
            writer.WriteAttribute("Repeat", Me.Repeat)
            writer.WriteAttribute("Attachment", Me.Attachment)
            writer.WriteAttribute("Position", Me.Position)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace
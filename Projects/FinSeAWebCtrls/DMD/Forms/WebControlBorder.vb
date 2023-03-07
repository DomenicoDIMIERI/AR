Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases



Namespace Forms

    <Serializable> _
    Public Class WebControlBorder
        Implements DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Owner As WebControlStyle
        Public Color As String
        Public Size As String
        Public Style As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Color = ""
            Me.Size = ""
            Me.Style = ""
        End Sub

        Public Sub New(ByVal text As String)
            Me.New
            Me.Parse(text)
        End Sub

        Public Sub New(ByVal owner As WebControlStyle)
            Me.New
            Me.m_Owner = owner
        End Sub

        Private Sub Parse(ByVal value As String)
            Dim items() As String = Split(Replace(Trim(value), "  ", " "), " ")
            For i = 0 To UBound(items)
                If Left(items(i), 1) = "#" Then
                    Me.Color = items(i)
                ElseIf Left(items(i), 1) >= "0" And Left(items(i), 1) <= "9" Then
                    Me.Size = items(i)
                Else
                    Me.Style = items(i)
                End If
            Next
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String = vbNullString
            If Me.Style <> vbNullString Then ret = Strings.Combine(ret, Me.Style, " ")
            If Me.Size <> vbNullString Then ret = Strings.Combine(ret, Me.Size, " ")
            If Me.Color <> vbNullString Then ret = Strings.Combine(ret, Me.Color, " ")
            Return ret
        End Function

        Public Shared Widening Operator CType(ByVal value As String) As WebControlBorder
            Return New WebControlBorder(value)
        End Operator


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Color" : Me.Color = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Size" : Me.Size = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Style" : Me.Style = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Color", Me.Color)
            writer.WriteAttribute("Size", Me.Size)
            writer.WriteAttribute("Style", Me.Style)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace
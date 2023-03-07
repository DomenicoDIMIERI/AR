Imports DMD
Imports DMD.Sistema

Public Class DispositivoEsterno
    'Implements Xml.Serialization.IXmlSerializable
    Implements DMD.XML.IDMDXMLSerializable

    Public Nome As String
    Public Indirizzo As String
    Public Tipo As String

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Nome = ""
        Me.Indirizzo = ""
        Me.Tipo = ""
    End Sub

    Public Sub New(ByVal nome As String, ByVal indirizzo As String, ByVal tipo As String)
        Me.New
        Me.Nome = nome
        Me.Indirizzo = indirizzo
        Me.Tipo = tipo
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If (Me Is obj) Then Return True
        If Not (TypeOf (obj) Is DispositivoEsterno) Then Return False
        With DirectCast(obj, DispositivoEsterno)
            Return .Nome = Me.Nome AndAlso
                   .Indirizzo = Me.Indirizzo AndAlso
                   .Tipo = Me.Tipo
        End With
    End Function

    Public Overrides Function ToString() As String
        Return Me.Nome & " (" & Me.Tipo & ")"
    End Function


    'Private Function GetSchema() As XML.Schema.XmlSchema Implements XML.Serialization.IXmlSerializable.GetSchema
    '    Return Nothing
    'End Function

    'Private Sub ReadXml(reader As XML.XmlReader) Implements XML.Serialization.IXmlSerializable.ReadXml
    '    Me.Nome = reader.GetAttribute("nome")
    '    Me.Indirizzo = reader.GetAttribute("indirizzo")
    '    Me.Tipo = reader.GetAttribute("tipo")
    '    reader.Read()
    'End Sub

    'Public Sub WriteXml(writer As XML.XMLWriter) Implements XML.Serialization.IXmlSerializable.WriteXml
    '    writer.WriteAttributeString("nome", Me.Nome)
    '    writer.WriteAttributeString("indirizzo", Me.Indirizzo)
    '    writer.WriteAttributeString("tipo", Me.Tipo)
    'End Sub

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Indirizzo" : Me.Indirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Nome", Me.Nome)
        writer.WriteAttribute("Indirizzo", Me.Indirizzo)
        writer.WriteAttribute("Tipo", Me.Tipo)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class

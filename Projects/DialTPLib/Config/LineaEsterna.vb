Imports DMD
Imports DMD.Sistema

Public Class LineaEsterna
    'Implements Xml.Serialization.IXmlSerializable
    Implements DMD.XML.IDMDXMLSerializable, IComparable

    Public Nome As String
    Public Prefisso As String
    Public Ordine As Integer
    Public Server As String

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Nome = ""
        Me.Prefisso = ""
        Me.Ordine = 0
        Me.Server = ""
    End Sub

    Public Sub New(ByVal nome As String, ByVal prefisso As String)
        Me.New
        Me.Nome = Trim(nome)
        Me.Prefisso = Trim(prefisso)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Nome & " (" & Me.Prefisso & ")"
    End Function


    'Private Function GetSchema() As Xml.Schema.XmlSchema Implements Xml.Serialization.IXmlSerializable.GetSchema
    '    Return Nothing
    'End Function

    'Private Sub ReadXml(reader As Xml.XmlReader) Implements Xml.Serialization.IXmlSerializable.ReadXml
    '    Me.Nome = reader.GetAttribute("nome")
    '    Me.Prefisso = reader.GetAttribute("prefisso")
    '    reader.Read()
    'End Sub

    'Public Sub WriteXml(writer As Xml.XmlWriter) Implements Xml.Serialization.IXmlSerializable.WriteXml
    '    writer.WriteAttributeString("nome", Me.Nome)
    '    writer.WriteAttributeString("prefisso", Me.Prefisso)
    'End Sub

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Prefisso" : Me.Prefisso = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Ordine" : Me.Ordine = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "Server" : Me.Server = XML.Utils.Serializer.DeserializeString(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Nome", Me.Nome)
        writer.WriteAttribute("Prefisso", Me.Prefisso)
        writer.WriteAttribute("Ordine", Me.Ordine)
        writer.WriteAttribute("Server", Me.Server)
    End Sub

    Public Function CompareTo(ByVal linea As LineaEsterna) As Integer
        Dim ret As Integer = Me.Ordine.CompareTo(linea.Ordine)
        If (ret = 0) Then ret = Strings.Compare(Me.Nome, linea.Nome)
        If (ret = 0) Then ret = Strings.Compare(Me.Prefisso, linea.Prefisso)
        Return ret
    End Function

    Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
        Return Me.CompareTo(DirectCast(obj, LineaEsterna))
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class

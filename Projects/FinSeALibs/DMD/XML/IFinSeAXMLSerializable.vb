Namespace XML

    Public Interface IDMDXMLSerializable

        Sub XMLSerialize(ByVal writer As XMLWriter) 'As String
        Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)

    End Interface

End Namespace

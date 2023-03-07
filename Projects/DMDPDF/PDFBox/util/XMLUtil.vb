Imports FinSeA.Io
Imports System.IO
Imports System.Xml

Namespace org.apache.pdfbox.util

    '/**
    ' * This class with handle some simple XML operations.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public NotInheritable Class XMLUtil

        '/**
        ' * Utility class, should not be instantiated.
        ' *
        ' */
        Private Sub New()
        End Sub

        '/**
        ' * This will parse an XML stream and create a DOM document.
        ' *
        ' * @param is The stream to get the XML from.
        ' * @return The DOM document.
        ' * @throws IOException It there is an error creating the dom.
        ' */
        Public Shared Function parse(ByVal [is] As InputStream) As System.Xml.XmlDocument
            Try
                'Dim builderFactory As System.Xml.XmlDocumentBuilderFactory = DocumentBuilderFactory.newInstance()
                'Dim builder As System.Xml.XmlDocumentBuilder = builderFactory.newDocumentBuilder()
                'Return builder.parse([is])
                Dim doc As New XmlDocument()
                doc.Load([is])
                Return doc
            Catch e As Exception
                Dim thrown As New IOException(e.Message)
                Throw thrown
            End Try
        End Function

        '/**
        ' * This will get the text value of an element.
        ' *
        ' * @param node The node to get the text value for.
        ' * @return The text of the node.
        ' */
        Public Shared Function getNodeValue(ByVal node As System.Xml.XmlElement) As String
            Dim retval As String = ""
            Dim children As System.Xml.XmlNodeList = node.ChildNodes
            For i As Integer = 0 To children.Count - 1
                Dim [next] As System.Xml.XmlNode = children.Item(i)
                If (TypeOf ([next]) Is System.Xml.XmlText) Then
                    retval = [next].Value 'NodeValue
                End If
            Next
            Return retval
        End Function

    End Class

End Namespace

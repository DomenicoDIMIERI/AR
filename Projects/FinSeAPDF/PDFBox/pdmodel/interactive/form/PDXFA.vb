Imports FinSeA.Io
'imports F javax.xml.parsers.DocumentBuilder;
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
'import org.w3c.dom.Document;
'import org.xml.sax.SAXException;

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This class represents an XML Forms Architecture Data packet.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDXFA
        Implements COSObjectable

        Private xfa As COSBase

        '/**
        ' * Constructor.
        ' *
        ' * @param xfaBase The xfa resource.
        ' */
        Public Sub New(ByVal xfaBase As COSBase)
            xfa = xfaBase
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return xfa
        End Function


        '/**
        ' * Get the XFA content as byte array.
        ' * 
        ' * The XFA is either a stream containing the entire XFA resource
        ' * or an array specifying individual packets that together make
        ' * up the XFA resource.
        ' * 
        ' * A packet is a pair of a string and stream. The string contains
        ' * the name of the XML element and the stream contains the complete
        ' * text of this XML element. Each packet represents a complete XML
        ' * element, with the exception of the first and last packet,
        ' * which specify begin and end tags for the xdp:xdp element.
        ' * [IS0 32000-1:2008: 12.7.8]
        ' * 
        ' * @return the XFA content
        ' * @throws IOException 
        ' */    
        Public Function getBytes() As Byte() ' throws IOException 
            Dim baos As ByteArrayOutputStream = New ByteArrayOutputStream()
            Dim [is] As InputStream = Nothing
            Dim xfaBytes() As Byte = Nothing

            Try
                ' handle the case if the XFA is split into individual parts
                If (TypeOf (Me.getCOSObject()) Is COSArray) Then
                    ReDim xfaBytes(1024 - 1) '= new byte[1024];
                    Dim cosArray As COSArray = Me.getCOSObject()
                    For i As Integer = 1 To cosArray.size() - 1 Step 2
                        Dim cosObj As COSBase = cosArray.getObject(i)
                        If (TypeOf (cosObj) Is COSStream) Then
                            [is] = DirectCast(cosObj, COSStream).getUnfilteredStream()
                            Dim nRead As Integer
                            nRead = [is].read(xfaBytes, 0, xfaBytes.Length)
                            While (nRead > 0)
                                baos.Write(xfaBytes, 0, nRead)
                                nRead = [is].read(xfaBytes, 0, xfaBytes.Length)
                            End While
                            baos.Flush()
                        End If
                    Next
                    ' handle the case if the XFA is represented as a single stream
                ElseIf (TypeOf (xfa.getCOSObject()) Is COSStream) Then
                    ReDim xfaBytes(1024 - 1)
                    [is] = DirectCast(xfa.getCOSObject(), COSStream).getUnfilteredStream()
                    Dim nRead As Integer
                    nRead = [is].read(xfaBytes, 0, xfaBytes.Length)
                    While (nRead > 0)
                        baos.Write(xfaBytes, 0, nRead)
                        nRead = [is].read(xfaBytes, 0, xfaBytes.Length)
                    End While
                    baos.Flush()
                End If
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
                If (baos IsNot Nothing) Then
                    baos.Close()
                End If
            End Try
            Return baos.toByteArray()
        End Function

        '/**
        ' * Get the XFA content as W3C document.
        ' * 
        ' * @see #getBytes()
        ' * 
        ' * @return the XFA content
        ' * 
        ' * @throws ParserConfigurationException parser exception.
        ' * @throws SAXException parser exception.
        ' * @throws IOException if something went wrong when reading the XFA content.
        ' * 
        ' */        
        Public Function getDocument() As System.Xml.XmlDocument ' throws ParserConfigurationException, SAXException, IOException 
            'Dim factory As System.Xml.XmlDocumentBuilderFactory = DocumentBuilderFactory.newInstance()
            'factory.setNamespaceAware(True)
            'Dim builder As System.Xml.XmlDocumentBuilder = factory.newDocumentBuilder()
            Dim doc As New System.Xml.XmlDocument()
            Dim stream As New ByteArrayInputStream(Me.getBytes())
            doc.Load(stream)
            stream.Dispose()
            Return doc
        End Function

    End Class

End Namespace
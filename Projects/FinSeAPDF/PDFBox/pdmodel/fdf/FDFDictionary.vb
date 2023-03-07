Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.filespecification
'import org.w3c.dom.Element;
'import org.w3c.dom.Node;
'import org.w3c.dom.NodeList;

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * Me represents an FDF dictionary that is part of the FDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */
    Public Class FDFDictionary
        Implements COSObjectable

        Private fdf As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            fdf = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fdfDictionary The FDF documents catalog.
        ' */
        Public Sub New(ByVal fdfDictionary As COSDictionary)
            fdf = fdfDictionary
        End Sub

        '/**
        ' * Me will create an FDF dictionary from an XFDF XML document.
        ' *
        ' * @param fdfXML The XML document that contains the XFDF data.
        ' * @throws IOException If there is an error reading from the dom.
        ' */
        Public Sub New(ByVal fdfXML As System.Xml.XmlElement) 'throws IOException
            Me.New()
            Dim nodeList As System.Xml.XmlNodeList = fdfXML.ChildNodes
            For i As Integer = 0 To nodeList.Count - 1
                Dim node As System.Xml.XmlNode = nodeList.Item(i)
                If (TypeOf (node) Is System.Xml.XmlElement) Then
                    Dim child As System.Xml.XmlElement = node
                    If (child.Name.Equals("f")) Then 'Tag Name
                        Dim fs As PDSimpleFileSpecification = New PDSimpleFileSpecification()
                        fs.setFile(child.GetAttribute("href"))
                        setFile(fs)

                    ElseIf (child.Name.Equals("ids")) Then 'TagName
                        Dim ids As COSArray = New COSArray()
                        Dim original As String = child.GetAttribute("original")
                        Dim modified As String = child.GetAttribute("modified")
                        ids.add(COSString.createFromHexString(original))
                        ids.add(COSString.createFromHexString(modified))
                        setID(ids)

                    ElseIf (child.Name.Equals("fields")) Then 'TagName
                        Dim fields As System.Xml.XmlNodeList = child.ChildNodes
                        Dim fieldList As List = New ArrayList()
                        For f As Integer = 0 To fields.Count - 1
                            fieldList.add(New FDFField(fields.Item(f)))
                            Dim currentNode As System.Xml.XmlNode = fields.Item(f)
                            If (TypeOf (currentNode) Is System.Xml.XmlElement) Then
                                If (DirectCast(currentNode, System.Xml.XmlElement).Name.Equals("field")) Then 'NodeName
                                    fieldList.add(New FDFField(fields.Item(f)))
                                End If
                            End If
                        Next
                        setFields(fieldList)
                    ElseIf (child.Name.Equals("annots")) Then 'TagName
                        Dim annots As System.Xml.XmlNodeList = child.ChildNodes
                        Dim annotList As List = New ArrayList()
                        For j As Integer = 0 To annots.Count - 1
                            Dim annotNode As System.Xml.XmlNode = annots.Item(i)
                            If (TypeOf (annotNode) Is System.Xml.XmlElement) Then
                                Dim annot As System.Xml.XmlElement = annotNode
                                If (annot.Name().Equals("text")) Then 'NodeName
                                    annotList.add(New FDFAnnotationText(annot))
                                Else
                                    Throw New IOException("Error: Unknown annotation type '" & annot.Name()) 'NodeName
                                End If
                            End If
                        Next
                        setAnnotations(annotList)
                    End If
                End If
            Next
        End Sub

        '/**
        ' * Me will write Me element as an XML document.
        ' *
        ' * @param output The stream to write the xml to.
        ' *
        ' * @throws IOException If there is an error writing the XML.
        ' */
        Public Sub writeXML(ByVal output As Finsea.Io.Writer) 'throws IOException
            Dim fs As PDFileSpecification = Me.getFile()
            If (fs IsNot Nothing) Then
                output.write("<f href=""" & fs.getFile() & """ />" & vbLf)
            End If
            Dim ids As COSArray = Me.getID()
            If (ids IsNot Nothing) Then
                Dim original As COSString = ids.getObject(0)
                Dim modified As COSString = ids.getObject(1)
                output.write("<ids original=""" & original.getHexString() & """ ")
                output.write("modified=""" & modified.getHexString() & """ />" & vbLf)
            End If
            Dim fields As List = getFields()
            If (fields IsNot Nothing AndAlso fields.size() > 0) Then
                output.write("<fields>" & vbLf)
                For i As Integer = 0 To fields.size() - 1
                    DirectCast(fields.get(i), FDFField).writeXML(output)
                Next
                output.write("</fields>" & vbLf)
            End If
        End Sub

        '/**
        ' * Convert Me standard java object to a COS object.
        ' *
        ' * @return The cos object that matches Me Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return fdf
        End Function

        '/**
        ' * Convert Me standard java object to a COS object.
        ' *
        ' * @return The cos object that matches Me Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return fdf
        End Function

        '/**
        ' * The source file or target file: the PDF document file that
        ' * Me FDF file was exported from or is intended to be imported into.
        ' *
        ' * @return The F entry of the FDF dictionary.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Function getFile() As PDFileSpecification ' throws IOException
            Return PDFileSpecification.createFS(fdf.getDictionaryObject(COSName.F))
        End Function

        '/**
        ' * Me will set the file specification.
        ' *
        ' * @param fs The file specification.
        ' */
        Public Sub setFile(ByVal fs As PDFileSpecification)
            fdf.setItem(COSName.F, fs)
        End Sub

        '/**
        ' * Me is the FDF id.
        ' *
        ' * @return The FDF ID.
        ' */
        Public Function getID() As COSArray
            Return fdf.getDictionaryObject(COSName.ID)
        End Function

        '/**
        ' * Me will set the FDF id.
        ' *
        ' * @param id The new id for the FDF.
        ' */
        Public Sub setID(ByVal id As COSArray)
            fdf.setItem(COSName.ID, id)
        End Sub

        '/**
        ' * Me will get the list of FDF Fields.  Me will return a list of FDFField
        ' * objects.
        ' *
        ' * @return A list of FDF fields.
        ' */
        Public Function getFields() As List
            Dim retval As List = Nothing
            Dim fieldArray As COSArray = fdf.getDictionaryObject(COSName.FIELDS)
            If (fieldArray IsNot Nothing) Then
                Dim fields As List(Of FDFField) = New ArrayList(Of FDFField)
                For i As Integer = 0 To fieldArray.size() - 1
                    fields.add(New FDFField(fieldArray.getObject(i)))
                Next
                retval = New COSArrayList(fields, fieldArray)
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the list of fields.  Me should be a list of FDFField objects.
        ' *
        ' * @param fields The list of fields.
        ' */
        Public Sub setFields(ByVal fields As List)
            fdf.setItem(COSName.FIELDS, COSArrayList.converterToCOSArray(fields))
        End Sub

        '/**
        ' * Me will get the status string to be displayed as the result of an
        ' * action.
        ' *
        ' * @return The status.
        ' */
        Public Function getStatus() As String
            Return fdf.getString(COSName.STATUS)
        End Function

        '/**
        ' * Me will set the status string.
        ' *
        ' * @param status The new status string.
        ' */
        Public Sub setStatus(ByVal status As String)
            fdf.setString(COSName.STATUS, status)
        End Sub

        '/**
        ' * Me will get the list of FDF Pages.  Me will return a list of FDFPage objects.
        ' *
        ' * @return A list of FDF pages.
        ' */
        Public Function getPages() As List
            Dim retval As List = Nothing
            Dim pageArray As COSArray = fdf.getDictionaryObject(COSName.PAGES)
            If (pageArray IsNot Nothing) Then
                Dim pages As List(Of FDFPage) = New ArrayList(Of FDFPage)
                For i As Integer = 0 To pageArray.size() - 1
                    pages.add(New FDFPage(pageArray.get(i)))
                Next
                retval = New COSArrayList(pages, pageArray)
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the list of pages.  Me should be a list of FDFPage objects.
        ' *
        ' *
        ' * @param pages The list of pages.
        ' */
        Public Sub setPages(ByVal pages As List)
            fdf.setItem(COSName.PAGES, COSArrayList.converterToCOSArray(pages))
        End Sub

        '/**
        ' * The encoding to be used for a FDF field.  The default is PDFDocEncoding
        ' * and Me method will never return null.
        ' *
        ' * @return The encoding value.
        ' */
        Public Function getEncoding() As String
            Dim encoding As String = fdf.getNameAsString(COSName.ENCODING)
            If (encoding = "") Then
                encoding = "PDFDocEncoding"
            End If
            Return encoding
        End Function

        ' /**
        '* Me will set the encoding.
        '*
        '* @param encoding The new encoding.
        '*/
        Public Sub setEncoding(ByVal encoding As String)
            fdf.setName(COSName.ENCODING, encoding)
        End Sub

        '/**
        ' * Me will get the list of FDF Annotations.  Me will return a list of FDFAnnotation objects
        ' * or null if the entry is not set.
        ' *
        ' * @return A list of FDF annotations.
        ' *
        ' * @throws IOException If there is an error creating the annotation list.
        ' */
        Public Function getAnnotations() As List  ' throws IOException
            Dim retval As List = Nothing
            Dim annotArray As COSArray = fdf.getDictionaryObject(COSName.ANNOTS)
            If (annotArray IsNot Nothing) Then
                Dim annots As List(Of FDFAnnotation) = New ArrayList(Of FDFAnnotation)
                For i As Integer = 0 To annotArray.size() - 1
                    annots.add(FDFAnnotation.create(annotArray.getObject(i)))
                Next
                retval = New COSArrayList(annots, annotArray)
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the list of annotations.  Me should be a list of FDFAnnotation objects.
        ' *
        ' *
        ' * @param annots The list of annotations.
        ' */
        Public Sub setAnnotations(ByVal annots As List)
            fdf.setItem(COSName.ANNOTS, COSArrayList.converterToCOSArray(annots))
        End Sub

        '/**
        ' * Me will get the incremental updates since the PDF was last opened.
        ' *
        ' * @return The differences entry of the FDF dictionary.
        ' */
        Public Function getDifferences() As COSStream
            Return fdf.getDictionaryObject(COSName.DIFFERENCES)
        End Function

        '/**
        ' * Me will set the differences stream.
        ' *
        ' * @param diff The new differences stream.
        ' */
        Public Sub setDifferences(ByVal diff As COSStream)
            fdf.setItem(COSName.DIFFERENCES, diff)
        End Sub

        '/**
        ' * Me will get the target frame in the browser to open Me document.
        ' *
        ' * @return The target frame.
        ' */
        Public Function getTarget() As String
            Return fdf.getString(COSName.TARGET)
        End Function

        '/**
        ' * Me will set the target frame in the browser to open Me document.
        ' *
        ' * @param target The new target frame.
        ' */
        Public Sub setTarget(ByVal target As String)
            fdf.setString(COSName.TARGET, target)
        End Sub

        '/**
        ' * Me will get the list of embedded FDF entries, or null if the entry is null.
        ' * Me will return a list of PDFileSpecification objects.
        ' *
        ' * @return A list of embedded FDF files.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Function getEmbeddedFDFs() As List ' throws IOException
            Dim retval As List = Nothing
            Dim embeddedArray As COSArray = fdf.getDictionaryObject(COSName.EMBEDDED_FDFS)
            If (embeddedArray IsNot Nothing) Then
                Dim embedded As List(Of PDFileSpecification) = New ArrayList(Of PDFileSpecification)
                For i As Integer = 0 To embeddedArray.size() - 1
                    embedded.add(PDFileSpecification.createFS(embeddedArray.get(i)))
                Next
                retval = New COSArrayList(embedded, embeddedArray)
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the list of embedded FDFs.  Me should be a list of
        ' * PDFileSpecification objects.
        ' *
        ' *
        ' * @param embedded The list of embedded FDFs.
        ' */
        Public Sub setEmbeddedFDFs(ByVal embedded As List)
            fdf.setItem(COSName.EMBEDDED_FDFS, COSArrayList.converterToCOSArray(embedded))
        End Sub

        '/**
        ' * Me will get the java script entry.
        ' *
        ' * @return The java script entry describing javascript commands.
        ' */
        Public Function getJavaScript() As FDFJavaScript
            Dim fs As FDFJavaScript = Nothing
            Dim dic As COSDictionary = fdf.getDictionaryObject(COSName.JAVA_SCRIPT)
            If (dic IsNot Nothing) Then
                fs = New FDFJavaScript(dic)
            End If
            Return fs
        End Function

        '/**
        ' * Me will set the JavaScript entry.
        ' *
        ' * @param js The javascript entries.
        ' */
        Public Sub setJavaScript(ByVal js As FDFJavaScript)
            fdf.setItem(COSName.JAVA_SCRIPT, js)
        End Sub

    End Class

End Namespace

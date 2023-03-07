Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.fdf
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This class represents the acroform of a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.14 $
    ' */
    Public Class PDAcroForm
        Implements COSObjectable

        Private acroForm As COSDictionary
        Private document As PDDocument

        Private fieldCache As Map

        '/**
        ' * Constructor.
        ' *
        ' * @param doc The document that this form is part of.
        ' */
        Public Sub New(ByVal doc As PDDocument)
            document = doc
            acroForm = New COSDictionary()
            Dim fields As COSArray = New COSArray()
            acroForm.setItem(COSName.getPDFName("Fields"), fields)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param doc The document that this form is part of.
        ' * @param form The existing acroForm.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal form As COSDictionary)
            document = doc
            acroForm = form
        End Sub

        '/**
        ' * This will get the document associated with this form.
        ' *
        ' * @return The PDF document.
        ' */
        Public Function getDocument() As PDDocument
            Return document
        End Function

        '/**
        ' * This will get the dictionary that this form wraps.
        ' *
        ' * @return The dictionary for this form.
        ' */
        Public Function getDictionary() As COSDictionary
            Return acroForm
        End Function

        '/**
        ' * This method will import an entire FDF document into the PDF document
        ' * that this acroform is part of.
        ' *
        ' * @param fdf The FDF document to import.
        ' *
        ' * @throws IOException If there is an error doing the import.
        ' */
        Public Sub importFDF(ByVal fdf As FDFDocument)  'throws IOException
            Dim fields As List = fdf.getCatalog().getFDF().getFields()
            If (fields IsNot Nothing) Then
                For i As Integer = 0 To fields.size() - 1
                    Dim fdfField As FDFField = fields.get(i)
                    Dim docField As PDField = getField(fdfField.getPartialFieldName())
                    If (docField IsNot Nothing) Then
                        docField.importFDF(fdfField)
                    End If
                Next
            End If
        End Sub

        '/**
        ' * This will export all FDF form data.
        ' *
        ' * @return An FDF document used to export the document.
        ' * @throws IOException If there is an error when exporting the document.
        ' */
        Public Function exportFDF() As FDFDocument ' throws IOException
            Dim fdf As FDFDocument = New FDFDocument()
            Dim catalog As FDFCatalog = fdf.getCatalog()
            Dim fdfDict As FDFDictionary = New FDFDictionary()
            catalog.setFDF(fdfDict)

            Dim fdfFields As List = New ArrayList()
            Dim fields As List = getFields()
            'Iterator fieldIter = fields.iterator();
            'While (fieldIter.hasNext())
            '{
            'PDField docField = (PDField)fieldIter.next();
            For Each docField As PDField In fields
                addFieldAndChildren(docField, fdfFields)
            Next
            fdfDict.setID(document.getDocument().getDocumentID())
            If (fdfFields.size() > 0) Then
                fdfDict.setFields(fdfFields)
            End If
            Return fdf
        End Function

        Private Sub addFieldAndChildren(ByVal docField As PDField, ByVal fdfFields As List) 'throws IOException
            Dim fieldValue As Object = docField.getValue()
            Dim fdfField As FDFField = New FDFField()
            fdfField.setPartialFieldName(docField.getPartialName())
            fdfField.setValue(fieldValue)
            Dim kids As List = docField.getKids()
            Dim childFDFFields As List = New ArrayList()
            If (kids IsNot Nothing) Then
                For i As Integer = 0 To kids.size() - 1
                    addFieldAndChildren(kids.get(i), childFDFFields)
                Next
                If (childFDFFields.size() > 0) Then
                    fdfField.setKids(childFDFFields)
                End If
            End If
            If (fieldValue IsNot Nothing OrElse childFDFFields.size() > 0) Then
                fdfFields.add(fdfField)
            End If
        End Sub

        '/**
        ' * This will return all of the fields in the document.  The type
        ' * will be a org.apache.pdfbox.pdmodel.field.PDField.
        ' *
        ' * @return A list of all the fields.
        ' * @throws IOException If there is an error while getting the list of fields.
        ' */
        Public Function getFields() As List  ' throws IOException
            Dim retval As List = Nothing
            Dim fields As COSArray = acroForm.getDictionaryObject(COSName.getPDFName("Fields"))

            If (fields IsNot Nothing) Then
                Dim actuals As List = New ArrayList()
                For i As Integer = 0 To fields.size() - 1
                    Dim element As COSDictionary = fields.getObject(i)
                    If (element IsNot Nothing) Then
                        Dim field As PDField = PDFieldFactory.createField(Me, element)
                        If (field IsNot Nothing) Then
                            actuals.add(field)
                        End If
                    End If
                Next
                retval = New COSArrayList(actuals, fields)
            End If
            Return retval
        End Function

        '/**
        ' * Set the fields that are part of this AcroForm.
        ' *
        ' * @param fields The fields that are part of this form.
        ' */
        Public Sub setFields(ByVal fields As List)
            acroForm.setItem("Fields", COSArrayList.converterToCOSArray(fields))
        End Sub

        '/**
        ' * This will tell this form to cache the fields into a Map structure
        ' * for fast access via the getField method.  The default is false.  You would
        ' * want this to be false if you were changing the COSDictionary behind the scenes,
        ' * otherwise setting this to true is acceptable.
        ' *
        ' * @param cache A boolean telling if we should cache the fields.
        ' * @throws IOException If there is an error while caching the fields.
        ' */
        Public Sub setCacheFields(ByVal cache As Boolean)  'throws IOException
            If (cache) Then
                fieldCache = New HashMap()
                Dim fields As List = getFields()
                'Iterator fieldIter = fields.iterator();
                '    While (fieldIter.hasNext())
                '{
                '   PDField next = (PDField)fieldIter.next();
                For Each [next] As PDField In fields
                    fieldCache.put([next].getFullyQualifiedName(), [next])
                Next
            Else
                fieldCache = Nothing
            End If
        End Sub

        '/**
        ' * This will tell if this acro form is caching the fields.
        ' *
        ' * @return true if the fields are being cached.
        ' */
        Public Function isCachingFields() As Boolean
            Return fieldCache IsNot Nothing
        End Function

        '/**
        ' * This will get a field by name, possibly using the cache if setCache is true.
        ' *
        ' * @param name The name of the field to get.
        ' *
        ' * @return The field with that name of null if one was not found.
        ' *
        ' * @throws IOException If there is an error getting the field type.
        ' */
        Public Function getField(ByVal name As String) As PDField ' throws IOException
            Dim retval As PDField = Nothing
            If (fieldCache IsNot Nothing) Then
                retval = fieldCache.get(name)
            Else
                Dim nameSubSection() As String = name.Split("\.")
                Dim fields As COSArray = acroForm.getDictionaryObject(COSName.getPDFName("Fields"))

                For i As Integer = 0 To fields.size() - 1
                    If (retval IsNot Nothing) Then Exit For
                    Dim element As COSDictionary = fields.getObject(i)
                    If (element IsNot Nothing) Then
                        Dim fieldName As COSString = element.getDictionaryObject(COSName.getPDFName("T"))
                        If (fieldName.getString().Equals(name) OrElse fieldName.getString().Equals(nameSubSection(0))) Then
                            Dim root As PDField = PDFieldFactory.createField(Me, element)

                            If (nameSubSection.Length > 1) Then
                                Dim kid As PDField = root.findKid(nameSubSection, 1)
                                If (kid IsNot Nothing) Then
                                    retval = kid
                                Else
                                    retval = root
                                End If
                            Else
                                retval = root
                            End If
                        End If
                    End If
                Next
            End If
            Return retval
        End Function

        '/**
        ' * This will get the default resources for the acro form.
        ' *
        ' * @return The default resources.
        ' */
        Public Function getDefaultResources() As PDResources
            Dim retval As PDResources = Nothing
            Dim dr As COSDictionary = acroForm.getDictionaryObject(COSName.getPDFName("DR"))
            If (dr IsNot Nothing) Then
                retval = New PDResources(dr)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the default resources for the acroform.
        ' *
        ' * @param dr The new default resources.
        ' */
        Public Sub setDefaultResources(ByVal dr As PDResources)
            Dim drDict As COSDictionary = Nothing
            If (dr IsNot Nothing) Then
                drDict = dr.getCOSDictionary()
            End If
            acroForm.setItem(COSName.getPDFName("DR"), drDict)
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return acroForm
        End Function

        '/**
        ' * Get the XFA resource, the XFA resource is only used for PDF 1.5+ forms.
        ' *
        ' * @return The xfa resource or null if it does not exist.
        ' */
        Public Function getXFA() As PDXFA
            Dim xfa As PDXFA = Nothing
            Dim base As COSBase = acroForm.getDictionaryObject("XFA")
            If (base IsNot Nothing) Then
                xfa = New PDXFA(base)
            End If
            Return xfa
        End Function

        '/**
        ' * Set the XFA resource, this is only used for PDF 1.5+ forms.
        ' *
        ' * @param xfa The xfa resource.
        ' */
        Public Sub setXFA(ByVal xfa As PDXFA)
            acroForm.setItem("XFA", xfa)
        End Sub

    End Class

End Namespace

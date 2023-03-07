Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.encryption
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form
Imports System.Net
Imports FinSeA.Exceptions
Imports FinSeA.Net

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This is the in-memory representation of the PDF document.  You need to call
    ' * close() on this object when you are done using it!!
    ' * <p>
    ' * This class implements the {@link Pageable} interface, but since PDFBox
    ' * version 1.3.0 you should be using the {@link PDPageable} adapter instead
    ' * (see <a href="https://issues.apache.org/jira/browse/PDFBOX-788">PDFBOX-788</a>).
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.47 $
    ' */
    Public Class PDDocument
        Implements Pageable

        Private document As COSDocument

        '//cached values
        Private documentInformation As PDDocumentInformation
        Private documentCatalog As PDDocumentCatalog

        '//The encParameters will be cached here.  When the document is decrypted then
        '//the COSDocument will not have an "Encrypt" dictionary anymore and this object
        '//must be used.
        Private encParameters As PDEncryptionDictionary = Nothing

        ''' <summary>
        ''' The security handler used to decrypt / encrypt the document.
        ''' </summary>
        ''' <remarks></remarks>
        Private securityHandler As SecurityHandler = Nothing


        '/**
        ' * This assocates object ids with a page number.  It's used to determine
        ' * the page number for bookmarks (or page numbers for anything else for
        ' * which you have an object id for that matter). 
        ' */
        Private pageMap As Map(Of String, NInteger) = Nothing

        '/**
        ' * This will hold a flag which tells us if we should remove all security
        ' * from this documents.
        ' */
        Private allSecurityToBeRemoved As Boolean = False

        '/**
        ' * Keep tracking customized documentId for the trailer. If null, a new 
        ' * id will be generated for the document. This ID doesn't represent the
        ' * actual documentId from the trailer.
        ' */
        Private documentId As NLong


        '/**
        ' * Constructor, creates a new PDF Document with no pages.  You need to add
        ' * at least one page for the document to be valid.
        ' *
        ' * @throws IOException If there is an error creating this document.
        ' */
        Public Sub New() 'throws IOException

            document = New COSDocument()

            'First we need a trailer
            Dim trailer As COSDictionary = New COSDictionary()
            document.setTrailer(trailer)

            'Next we need the root dictionary.
            Dim rootDictionary As COSDictionary = New COSDictionary()
            trailer.setItem(COSName.ROOT, rootDictionary)
            rootDictionary.setItem(COSName.TYPE, COSName.CATALOG)
            rootDictionary.setItem(COSName.VERSION, COSName.getPDFName("1.4"))

            'next we need the pages tree structure
            Dim pages As COSDictionary = New COSDictionary()
            rootDictionary.setItem(COSName.PAGES, pages)
            pages.setItem(COSName.TYPE, COSName.PAGES)
            Dim kidsArray As COSArray = New COSArray()
            pages.setItem(COSName.KIDS, kidsArray)
            pages.setItem(COSName.COUNT, COSInteger.ZERO)
        End Sub

        Private Sub generatePageMap()
            pageMap = New HashMap(Of String, NInteger)()
            ' these page nodes could be references to pages, 
            ' or references to arrays which have references to pages
            ' or references to arrays which have references to arrays which have references to pages
            ' or ... (I think you get the idea...)
            processListOfPageReferences(getDocumentCatalog().getPages().getKids())
        End Sub

        Private Sub processListOfPageReferences(ByVal pageNodes As List(Of Object))
            Dim numberOfNodes As Integer = pageNodes.size()
            For i As Integer = 0 To numberOfNodes - 1
                Dim pageOrArray As Object = pageNodes.get(i)
                If (TypeOf (pageOrArray) Is PDPage) Then
                    Dim pageArray As COSArray = DirectCast(DirectCast(pageOrArray, PDPage).getParent().getKids(), COSArrayList).toList()
                    parseCatalogObject(pageArray.get(i))
                ElseIf (TypeOf (pageOrArray) Is PDPageNode) Then
                    processListOfPageReferences(DirectCast(pageOrArray, PDPageNode).getKids())
                End If
            Next
        End Sub

        '/**
        ' * This will either add the page passed in, or, if it's a pointer to an array
        ' * of pages, it'll recursivly call itself and process everything in the list.
        ' */
        Private Sub parseCatalogObject(ByVal thePageOrArrayObject As COSObject)
            Dim arrayCountBase As COSBase = thePageOrArrayObject.getItem(COSName.COUNT)
            Dim arrayCount As Integer = -1
            If (TypeOf (arrayCountBase) Is COSInteger) Then
                arrayCount = DirectCast(arrayCountBase, COSInteger).intValue()
            End If

            Dim kidsBase As COSBase = thePageOrArrayObject.getItem(COSName.KIDS)
            Dim kidsCount As Integer = -1
            If (TypeOf (kidsBase) Is COSArray) Then
                kidsCount = DirectCast(kidsBase, COSArray).size()
            End If

            If (arrayCount = -1 OrElse kidsCount = -1) Then
                ' these cases occur when we have a page, not an array of pages
                Dim objStr As String = CStr(thePageOrArrayObject.getObjectNumber().intValue())
                Dim genStr As String = CStr(thePageOrArrayObject.getGenerationNumber().intValue())
                getPageMap().put(objStr & "," & genStr, getPageMap().size() + 1)
            Else
                ' we either have an array of page pointers, or an array of arrays
                If (arrayCount = kidsCount) Then
                    ' process the kids... they're all references to pages
                    Dim kidsArray As COSArray = kidsBase
                    For i As Integer = 0 To kidsArray.size() - 1
                        Dim thisObject As COSObject = kidsArray.get(i)
                        Dim objStr As String = CStr(thisObject.getObjectNumber().intValue())
                        Dim genStr As String = CStr(thisObject.getGenerationNumber().intValue())
                        getPageMap().put(objStr + "," + genStr, getPageMap().size() + 1)
                    Next
                Else
                    ' this object is an array of references to other arrays
                    Dim list As COSArray = Nothing
                    If (TypeOf (kidsBase) Is COSArray) Then
                        list = kidsBase
                    End If
                    If (list IsNot Nothing) Then
                        For arrayCounter As Integer = 0 To arrayCounter < list.size() - 1
                            parseCatalogObject(list.get(arrayCounter))
                        Next
                    End If
                End If
            End If
        End Sub

        '/**
        ' * This will return the Map containing the mapping from object-ids to pagenumbers.
        ' * 
        ' * @return the pageMap
        ' */
        Public Function getPageMap() As Map(Of String, NInteger)
            If (pageMap Is Nothing) Then
                generatePageMap()
            End If
            Return pageMap
        End Function

        '/**
        ' * This will add a page to the document.  This is a convenience method, that
        ' * will add the page to the root of the hierarchy and set the parent of the
        ' * page to the root.
        ' *
        ' * @param page The page to add to the document.
        ' */
        Public Sub addPage(ByVal page As PDPage)
            Dim rootPages As PDPageNode = getDocumentCatalog().getPages()
            rootPages.getKids().add(page)
            page.setParent(rootPages)
            rootPages.updateCount()
        End Sub

        '/**
        ' * Add a signature.
        ' * 
        ' * @param sigObject is the PDSignature model
        ' * @param signatureInterface is a interface which provides signing capabilities
        ' * @throws IOException if there is an error creating required fields
        ' * @throws SignatureException if something went wrong
        ' */
        Public Sub addSignature(ByVal sigObject As interactive.digitalsignature.PDSignature, ByVal signatureInterface As SignatureInterface) 'throws(IOException, SignatureException)
            Dim defaultOptions As SignatureOptions = New SignatureOptions()
            defaultOptions.setPage(1)
            addSignature(sigObject, signatureInterface, defaultOptions)
        End Sub

        '/**
        ' * This will add a signature to the document. 
        ' *
        ' * @param sigObject is the PDSignature model
        ' * @param signatureInterface is a interface which provides signing capabilities
        ' * @param options signature options
        ' * @throws IOException if there is an error creating required fields
        ' * @throws SignatureException if something went wrong
        ' */
        Public Sub addSignature(ByVal sigObject As interactive.digitalsignature.PDSignature, ByVal signatureInterface As SignatureInterface, ByVal options As SignatureOptions) 'throws(IOException, SignatureException)
            ' Reserve content
            ' We need to reserve some space for the signature. Some signatures including
            ' big certificate chain and we need enough space to store it.
            Dim preferedSignatureSize As Integer = options.getPreferedSignatureSize()
            If (preferedSignatureSize > 0) Then
                sigObject.setContents(Array.CreateInstance(GetType(Byte), preferedSignatureSize * 2 + 2))
            Else
                sigObject.setContents(Array.CreateInstance(GetType(Byte), &H2500 * 2 + 2))
            End If

            ' Reserve ByteRange
            sigObject.setByteRange(New Integer() {0, 1000000000, 1000000000, 1000000000})

            getDocument().setSignatureInterface(signatureInterface)

            '// #########################################
            '// # Create SignatureForm for signature    #
            '// # and appending it to the document      #
            '// #########################################

            '// Get the first page
            Dim root As PDDocumentCatalog = getDocumentCatalog()
            Dim rootPages As PDPageNode = root.getPages()
            Dim kids As List(Of PDPage) = New ArrayList(Of PDPage)()
            rootPages.getAllKids(kids)

            Dim size As Integer = rootPages.getCount()
            Dim page As PDPage = Nothing
            If (size = 0) Then
                Throw New SignatureException(SignatureException.INVALID_PAGE_FOR_SIGNATURE, "The PDF file has no pages")
            End If
            If (options.getPage() > size) Then
                page = kids.get(size - 1)
            ElseIf (options.getPage() <= 0) Then
                page = kids.get(0)
            Else
                page = kids.get(options.getPage() - 1)
            End If

            ' Get the AcroForm from the Root-Dictionary and append the annotation
            Dim acroForm As PDAcroForm = root.getAcroForm()
            root.getCOSObject().setNeedToBeUpdate(True)

            If (acroForm Is Nothing) Then
                acroForm = New PDAcroForm(Me)
                root.setAcroForm(acroForm)
            Else
                acroForm.getCOSObject().setNeedToBeUpdate(True)
            End If

            '/*
            ' * For invisible signatures, the annotation has a rectangle array with values [ 0 0 0 0 ]. 
            ' * This annotation is usually attached to the viewed page when the signature is created. 
            ' * Despite not having an appearance, the annotation AP and N dictionaries may be present 
            ' * in some versions of Acrobat. If present, N references the DSBlankXObj (blank) XObject.
            ' */

            '// Create Annotation / Field for signature
            Dim annotations As List(Of PDAnnotation) = page.getAnnotations()

            Dim fields As List(Of PDField) = acroForm.getFields()
            Dim signatureField As PDSignatureField = Nothing
            If (fields Is Nothing) Then
                fields = New ArrayList()
                acroForm.setFields(fields)
            End If
            For Each pdField As PDField In fields
                If (TypeOf (pdField) Is PDSignatureField) Then
                    Dim signature As interactive.digitalsignature.PDSignature = DirectCast(pdField, PDSignatureField).getSignature()
                    If (signature IsNot Nothing AndAlso signature.getDictionary().Equals(sigObject.getDictionary())) Then
                        signatureField = pdField
                    End If
                End If
            Next
            If (signatureField Is Nothing) Then
                signatureField = New PDSignatureField(acroForm)
                signatureField.setSignature(sigObject) ' append the signature object
                signatureField.getWidget().setPage(page) ' backward linking
            End If

            ' Set the AcroForm Fields
            Dim acroFormFields As List(Of PDField) = acroForm.getFields()
            Dim acroFormDict As COSDictionary = acroForm.getDictionary()
            acroFormDict.setDirect(True)
            acroFormDict.setInt(COSName.SIG_FLAGS, 3)

            Dim checkFields As Boolean = False
            For Each field As PDField In acroFormFields
                If (TypeOf (field) Is PDSignatureField) Then
                    If (DirectCast(field, PDSignatureField).getCOSObject().Equals(signatureField.getCOSObject())) Then
                        checkFields = True
                        signatureField.getCOSObject().setNeedToBeUpdate(True)
                        Exit For
                    End If
                End If
            Next
            If (Not checkFields) Then
                acroFormFields.add(signatureField)
            End If

            ' Get the object from the visual signature
            Dim visualSignature As COSDocument = options.getVisualSignature()

            ' Distinction of case for visual and non-visual signature
            If (visualSignature Is Nothing) Then ' non-visual signature
                ' Set rectangle for non-visual signature to 0 0 0 0
                signatureField.getWidget().setRectangle(New PDRectangle()) ' rectangle array [ 0 0 0 0 ]
                ' Clear AcroForm / Set DefaultRessource
                Dim tmp As COSBase = Nothing
                acroFormDict.setItem(COSName.DR, tmp)
                ' Set empty Appearance-Dictionary
                Dim ap As PDAppearanceDictionary = New PDAppearanceDictionary()
                Dim apsStream As COSStream = getDocument().createCOSStream()
                apsStream.createUnfilteredStream()
                Dim aps As PDAppearanceStream = New PDAppearanceStream(apsStream)
                Dim cosObject As COSDictionary = aps.getCOSObject()
                cosObject.setItem(COSName.SUBTYPE, COSName.FORM)
                cosObject.setItem(COSName.BBOX, New PDRectangle())

                ap.setNormalAppearance(aps)
                ap.getDictionary().setDirect(True)
                signatureField.getWidget().setAppearance(ap)
            Else ' visual signature
                ' Obtain visual signature object
                Dim cosObjects As List(Of COSObject) = visualSignature.getObjects()

                Dim annotNotFound As Boolean = True
                Dim sigFieldNotFound As Boolean = True

                For Each cosObject As COSObject In cosObjects
                    If (Not annotNotFound AndAlso Not sigFieldNotFound) Then
                        Exit For
                    End If

                    Dim base As COSBase = cosObject.getObject()
                    If (base IsNot Nothing AndAlso TypeOf (base) Is COSDictionary) Then
                        Dim ft As COSBase = DirectCast(base, COSDictionary).getItem(COSName.FT)
                        Dim type As COSBase = DirectCast(base, COSDictionary).getItem(COSName.TYPE)
                        Dim apDict As COSBase = DirectCast(base, COSDictionary).getItem(COSName.AP)

                        ' Search for signature annotation
                        If (annotNotFound AndAlso COSName.ANNOT.equals(type)) Then
                            Dim cosBaseDict As COSDictionary = base

                            ' Read and set the Rectangle for visual signature
                            Dim rectAry As COSArray = cosBaseDict.getItem(COSName.RECT)
                            Dim rect As PDRectangle = New PDRectangle(rectAry)
                            signatureField.getWidget().setRectangle(rect)
                            annotNotFound = False
                        End If

                        ' Search for Signature-Field
                        If (sigFieldNotFound AndAlso COSName.SIG.equals(ft) AndAlso apDict IsNot Nothing) Then
                            Dim cosBaseDict As COSDictionary = base

                            ' Appearance Dictionary auslesen und setzen
                            Dim ap As PDAppearanceDictionary = New PDAppearanceDictionary(cosBaseDict.getItem(COSName.AP))
                            ap.getDictionary().setDirect(True)
                            signatureField.getWidget().setAppearance(ap)

                            ' AcroForm DefaultRessource auslesen und setzen
                            Dim dr As COSBase = cosBaseDict.getItem(COSName.DR)
                            dr.setDirect(True)
                            dr.setNeedToBeUpdate(True)
                            acroFormDict.setItem(COSName.DR, dr)
                            sigFieldNotFound = False
                        End If
                    End If
                Next

                If (annotNotFound OrElse sigFieldNotFound) Then
                    Throw New SignatureException(SignatureException.VISUAL_SIGNATURE_INVALID, "Could not read all needed objects from template")
                End If
            End If

            ' Get the annotations of the page and append the signature-annotation to it
            If (annotations Is Nothing) Then
                annotations = New COSArrayList()
                page.setAnnotations(annotations)
            End If
            ' take care that page and acroforms do not share the same array (if so, we don't need to add it twice)
            If (Not ((TypeOf (annotations) Is COSArrayList) AndAlso (TypeOf (acroFormFields) Is COSArrayList) AndAlso (DirectCast(annotations, COSArrayList).toList().Equals(DirectCast(acroFormFields, COSArrayList).toList()))) AndAlso Not checkFields) Then
                annotations.add(signatureField.getWidget())
            End If
            page.getCOSObject().setNeedToBeUpdate(True)
        End Sub

        '/**
        ' * This will add a signaturefield to the document.
        ' * @param sigFields are the PDSignatureFields that should be added to the document
        ' * @param signatureInterface is a interface which provides signing capabilities
        ' * @param options signature options
        ' * @throws IOException if there is an error creating required fields
        ' * @throws SignatureException 
        ' */
        Public Sub addSignatureField(ByVal sigFields As List(Of PDSignatureField), ByVal signatureInterface As SignatureInterface, ByVal options As SignatureOptions)  'throws IOException, SignatureException
            Dim catalog As PDDocumentCatalog = getDocumentCatalog()
            catalog.getCOSObject().setNeedToBeUpdate(True)

            Dim acroForm As PDAcroForm = catalog.getAcroForm()
            If (acroForm Is Nothing) Then
                acroForm = New PDAcroForm(Me)
                catalog.setAcroForm(acroForm)
            Else
                acroForm.getCOSObject().setNeedToBeUpdate(True)
            End If

            Dim acroFormDict As COSDictionary = acroForm.getDictionary()
            acroFormDict.setDirect(True)
            acroFormDict.setNeedToBeUpdate(True)
            If (acroFormDict.getInt(COSName.SIG_FLAGS) < 1) Then
                acroFormDict.setInt(COSName.SIG_FLAGS, 1) ' 1 if at least one signature field is available
            End If

            Dim field As List(Of PDField) = acroForm.getFields()

            For Each sigField As PDSignatureField In sigFields
                Dim sigObject As interactive.digitalsignature.PDSignature = sigField.getSignature()
                sigField.getCOSObject().setNeedToBeUpdate(True)

                ' Check if the field already exist
                Dim checkFields As Boolean = False
                For Each obj As Object In field
                    If (TypeOf (obj) Is PDSignatureField) Then
                        If (DirectCast(obj, PDSignatureField).getCOSObject().Equals(sigField.getCOSObject())) Then
                            checkFields = True
                            sigField.getCOSObject().setNeedToBeUpdate(True)
                            Exit For
                        End If
                    End If
                Next

                If (Not checkFields) Then
                    field.add(sigField)
                End If

                '/ Check if we need to add a signature
                If (sigField.getSignature() IsNot Nothing) Then
                    sigField.getCOSObject().setNeedToBeUpdate(True)
                    If (options Is Nothing) Then

                    End If
                    addSignature(sigField.getSignature(), signatureInterface, options)
                End If
            Next
        End Sub


        '/**
        ' * Remove the page from the document.
        ' *
        ' * @param page The page to remove from the document.
        ' *
        ' * @return true if the page was found false otherwise.
        ' */
        Public Function removePage(ByVal page As PDPage) As Boolean
            Dim parent As PDPageNode = page.getParent()
            Dim retval As Boolean = parent.getKids().remove(page)
            If (retval) Then
                'do a recursive updateCount starting at the root of the document
                getDocumentCatalog().getPages().updateCount()
            End If
            Return retval
        End Function

        '/**
        ' * Remove the page from the document.
        ' *
        ' * @param pageNumber 0 based index to page number.
        ' * @return true if the page was found false otherwise.
        ' */
        Public Function removePage(ByVal pageNumber As Integer) As Boolean
            Dim removed As Boolean = False
            Dim allPages As List = getDocumentCatalog().getAllPages()
            If (allPages.size() > pageNumber) Then
                Dim page As PDPage = allPages.get(pageNumber)
                removed = removePage(page)
            End If
            Return removed
        End Function

        '/**
        ' * This will import and copy the contents from another location.  Currently
        ' * the content stream is stored in a scratch file.  The scratch file is
        ' * associated with the document.  If you are adding a page to this document
        ' * from another document and want to copy the contents to this document's
        ' * scratch file then use this method otherwise just use the addPage method.
        ' *
        ' * @param page The page to import.
        ' * @return The page that was imported.
        ' *
        ' * @throws IOException If there is an error copying the page.
        ' */
        Public Function importPage(ByVal page As PDPage) As PDPage  'throws IOException
            Dim importedPage As PDPage = New PDPage(New COSDictionary(page.getCOSDictionary()))
            Dim [is] As InputStream = Nothing
            Dim os As OutputStream = Nothing
            Try
                Dim src As PDStream = page.getContents()
                If (src IsNot Nothing) Then
                    Dim dest As PDStream = New PDStream(document.createCOSStream())
                    importedPage.setContents(dest)
                    os = dest.createOutputStream()

                    Dim buf(10240 - 1) As Byte
                    Dim amountRead As Integer = 0
                    [is] = src.createInputStream()
                    amountRead = [is].read(buf, 0, 10240)
                    While (amountRead > 0)
                        os.Write(buf, 0, amountRead)
                        amountRead = [is].read(buf, 0, 10240)
                    End While
                End If
                addPage(importedPage)
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
                If (os IsNot Nothing) Then
                    os.Close()
                End If
            End Try
            Return importedPage
        End Function

        '/**
        ' * Constructor that uses an existing document.  The COSDocument that
        ' * is passed in must be valid.
        ' *
        ' * @param doc The COSDocument that this document wraps.
        ' */
        Public Sub New(ByVal doc As COSDocument)
            document = doc
        End Sub

        '/**
        ' * This will get the low level document.
        ' *
        ' * @return The document that this layer sits on top of.
        ' */
        Public Function getDocument() As COSDocument
            Return document
        End Function

        '/**
        ' * This will get the document info dictionary.  This is guaranteed to not return null.
        ' *
        ' * @return The documents /Info dictionary
        ' */
        Public Function getDocumentInformation() As PDDocumentInformation
            If (documentInformation Is Nothing) Then
                Dim trailer As COSDictionary = document.getTrailer()
                Dim infoDic As COSDictionary = trailer.getDictionaryObject(COSName.INFO)
                If (infoDic Is Nothing) Then
                    infoDic = New COSDictionary()
                    trailer.setItem(COSName.INFO, infoDic)
                End If
                documentInformation = New PDDocumentInformation(infoDic)
            End If
            Return documentInformation
        End Function

        '/**
        ' * This will set the document information for this document.
        ' *
        ' * @param info The updated document information.
        ' */
        Public Sub setDocumentInformation(ByVal info As PDDocumentInformation)
            documentInformation = info
            document.getTrailer().setItem(COSName.INFO, info.getDictionary())
        End Sub

        '/**
        ' * This will get the document CATALOG.  This is guaranteed to not return null.
        ' *
        ' * @return The documents /Root dictionary
        ' */
        Public Function getDocumentCatalog() As PDDocumentCatalog
            If (documentCatalog Is Nothing) Then
                Dim trailer As COSDictionary = document.getTrailer()
                Dim dictionary As COSBase = trailer.getDictionaryObject(COSName.ROOT)
                If (TypeOf (dictionary) Is COSDictionary) Then
                    documentCatalog = New PDDocumentCatalog(Me, dictionary)
                Else
                    documentCatalog = New PDDocumentCatalog(Me)
                End If
            End If
            Return documentCatalog
        End Function

        '/**
        ' * This will tell if this document is encrypted or not.
        ' *
        ' * @return true If this document is encrypted.
        ' */
        Public Function isEncrypted() As Boolean
            Return document.isEncrypted()
        End Function

        '/**
        ' * This will get the encryption dictionary for this document.  This will still
        ' * return the parameters if the document was decrypted.  If the document was
        ' * never encrypted then this will return null.  As the encryption architecture
        ' * in PDF documents is plugable this returns an abstract class, but the only
        ' * supported subclass at this time is a PDStandardEncryption object.
        ' *
        ' * @return The encryption dictionary(most likely a PDStandardEncryption object)
        ' *
        ' * @throws IOException If there is an error determining which security handler to use.
        ' */
        Public Function getEncryptionDictionary() As PDEncryptionDictionary  ' throws IOException
            If (encParameters Is Nothing) Then
                If (isEncrypted()) Then
                    encParameters = New PDEncryptionDictionary(document.getEncryptionDictionary())
                End If
            End If
            Return encParameters
        End Function

        '/**
        ' * This will set the encryption dictionary for this document.
        ' *
        ' * @param encDictionary The encryption dictionary(most likely a PDStandardEncryption object)
        ' *
        ' * @throws IOException If there is an error determining which security handler to use.
        ' */
        Public Sub setEncryptionDictionary(ByVal encDictionary As PDEncryptionDictionary) 'throws IOException
            encParameters = encDictionary
        End Sub

        '/**
        ' * This will return the last signature.
        ' *
        ' * @return the last signature as <code>PDSignature</code>.
        ' * @throws IOException if no document catalog can be found.
        ' * @deprecated use {@link #getLastSignatureDictionary()} instead.
        ' */
        <Obsolete> _
        Public Function getSignatureDictionary() As interactive.digitalsignature.PDSignature ' throws IOException
            Return getLastSignatureDictionary()
        End Function

        '/**
        ' * This will return the last signature.
        ' *
        ' * @return the last signature as <code>PDSignature</code>.
        ' * @throws IOException if no document catalog can be found.
        ' */
        Public Function getLastSignatureDictionary() As interactive.digitalsignature.PDSignature ' throws IOException
            Dim signatureDictionaries As List(Of interactive.digitalsignature.PDSignature) = getSignatureDictionaries()
            Dim size As Integer = signatureDictionaries.size()
            If (size > 0) Then
                Return signatureDictionaries.get(size - 1)
            End If
            Return Nothing
        End Function

        '/**
        ' * Retrieve all signature fields from the document.
        ' *
        ' * @return a <code>List</code> of <code>PDSignatureField</code>s
        ' * @throws IOException if no document catalog can be found.
        ' */
        Public Function getSignatureFields() As List(Of PDSignatureField)  'throws IOException
            Dim fields As List(Of PDSignatureField) = New LinkedList(Of PDSignatureField)()
            Dim acroForm As PDAcroForm = getDocumentCatalog().getAcroForm()
            If (acroForm IsNot Nothing) Then
                Dim signatureDictionary As List(Of COSDictionary) = document.getSignatureFields(False)
                For Each dict As COSDictionary In signatureDictionary
                    fields.add(New PDSignatureField(acroForm, dict))
                Next
            End If
            Return fields
        End Function

        '/**
        ' * Retrieve all signature dictionaries from the document.
        ' *
        ' * @return a <code>List</code> of <code>PDSignature</code>s
        ' * @throws IOException if no document catalog can be found.
        ' */
        Public Function getSignatureDictionaries() As List(Of interactive.digitalsignature.PDSignature) ' throws IOException
            Dim signatureDictionary As List(Of COSDictionary) = document.getSignatureDictionaries()
            Dim signatures As List(Of interactive.digitalsignature.PDSignature) = New LinkedList(Of interactive.digitalsignature.PDSignature)()
            For Each dict As COSDictionary In signatureDictionary
                signatures.add(New interactive.digitalsignature.PDSignature(dict))
            Next
            Return signatures
        End Function

        '/**
        ' * This will determine if this is the user password.  This only applies when
        ' * the document is encrypted and uses standard encryption.
        ' *
        ' * @param password The plain text user password.
        ' *
        ' * @return true If the password passed in matches the user password used to encrypt the document.
        ' *
        ' * @throws IOException If there is an error determining if it is the user password.
        ' * @throws CryptographyException If there is an error in the encryption algorithms.
        ' *
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Function isUserPassword(ByVal password As String)  'throws IOException, CryptographyException
            Return False
        End Function

        '/**
        ' * This will determine if this is the owner password.  This only applies when
        ' * the document is encrypted and uses standard encryption.
        ' *
        ' * @param password The plain text owner password.
        ' *
        ' * @return true If the password passed in matches the owner password used to encrypt the document.
        ' *
        ' * @throws IOException If there is an error determining if it is the user password.
        ' * @throws CryptographyException If there is an error in the encryption algorithms.
        ' *
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Function isOwnerPassword(ByVal password As String) 'throws IOException, CryptographyException
            Return False
        End Function

        '/**
        ' * This will decrypt a document. This method is provided for compatibility reasons only. User should use
        ' * the new security layer instead and the openProtection method especially.
        ' *
        ' * @param password Either the user or owner password.
        ' *
        ' * @throws CryptographyException If there is an error decrypting the document.
        ' * @throws IOException If there is an error getting the stream data.
        ' * @throws InvalidPasswordException If the password is not a user or owner password.
        ' *
        ' */
        Public Sub decrypt(ByVal password As String) 'throws CryptographyException, IOException, InvalidPasswordException
            Try
                Dim m As StandardDecryptionMaterial = New StandardDecryptionMaterial(password)
                Me.openProtection(m)
                document.dereferenceObjectStreams()
            Catch e As BadSecurityHandlerException
                Throw New CryptographyException(e)
            End Try
        End Sub

        '/**
        ' * This will tell if the document was decrypted with the master password.  This
        ' * entry is invalid if the PDF was not decrypted.
        ' *
        ' * @return true if the pdf was decrypted with the master password.
        ' *
        ' * @deprecated use <code>getCurrentAccessPermission</code> instead
        ' */
        <Obsolete> _
        Public Function wasDecryptedWithOwnerPassword() As Boolean
            Return False
        End Function

        '/**
        ' * This will <b>mark</b> a document to be encrypted.  The actual encryption
        ' * will occur when the document is saved.
        ' * This method is provided for compatibility reasons only. User should use
        ' * the new security layer instead and the openProtection method especially.
        ' *
        ' * @param ownerPassword The owner password to encrypt the document.
        ' * @param userPassword The user password to encrypt the document.
        ' *
        ' * @throws CryptographyException If an error occurs during encryption.
        ' * @throws IOException If there is an error accessing the data.
        ' *
        ' */
        Public Sub encrypt(ByVal ownerPassword As String, ByVal userPassword As String) 'throws(CryptographyException, IOException)
            Try
                Dim policy As StandardProtectionPolicy = New StandardProtectionPolicy(ownerPassword, userPassword, New AccessPermission())
                Me.protect(policy)
            Catch e As BadSecurityHandlerException
                Throw New CryptographyException(e)
            End Try
        End Sub


        '/**
        ' * The owner password that was passed into the encrypt method. You should
        ' * never use this method.  This will not longer be valid once encryption
        ' * has occured.
        ' *
        ' * @return The owner password passed to the encrypt method.
        ' *
        ' * @deprecated Do not rely on this method anymore.
        ' */
        <Obsolete> _
        Public Function getOwnerPasswordForEncryption() As String
            Return Nothing
        End Function

        '/**
        ' * The user password that was passed into the encrypt method.  You should
        ' * never use this method.  This will not longer be valid once encryption
        ' * has occured.
        ' *
        ' * @return The user password passed to the encrypt method.
        ' *
        ' * @deprecated Do not rely on this method anymore.
        ' */
        <Obsolete> _
        Public Function getUserPasswordForEncryption() As String
            Return vbNullString
        End Function

        '/**
        ' * Internal method do determine if the document will be encrypted when it is saved.
        ' *
        ' * @return True if encrypt has been called and the document
        ' *              has not been saved yet.
        ' *
        ' * @deprecated Do not rely on this method anymore. It is the responsibility of
        ' * COSWriter to hold this state
        ' */
        <Obsolete> _
        Public Function willEncryptWhenSaving() As Boolean
            Return False
        End Function

        '/**
        ' * This shoule only be called by the COSWriter after encryption has completed.
        ' *
        ' * @deprecated Do not rely on this method anymore. It is the responsability of
        ' * COSWriter to hold this state.
        ' */
        <Obsolete> _
        Public Sub clearWillEncryptWhenSaving()
            'method is deprecated.
        End Sub

        '/**
        ' * This will load a document from a url.
        ' *
        ' * @param url The url to load the PDF from.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal url As Url) As PDDocument ' throws IOException
            Dim s As Stream = url.openStream()
            Dim ret As PDDocument = load(s)
            s.Dispose()
            Return ret
        End Function

        '/**
        '    * This will load a document from a url. Used for skipping corrupt
        '    * pdf objects
        '    *
        '    * @param url The url to load the PDF from.
        '    * @param force When true, the parser will skip corrupt pdf objects and 
        '    * will continue parsing at the next object in the file
        '    *
        '    * @return The document that was loaded.
        '    *
        '    * @throws IOException If there is an error reading from the stream.
        '    */
        Public Shared Function load(ByVal url As Url, ByVal force As Boolean) As PDDocument 'throws IOException
            Dim s As Stream = url.openStream()
            Dim ret As PDDocument = load(s, force)
            s.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a url.
        ' *
        ' * @param url The url to load the PDF from.
        ' * @param scratchFile A location to store temp PDFBox data for this document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal url As Url, ByVal scratchFile As RandomAccess) As PDDocument 'throws IOException
            Dim s As Stream = url.openStream()
            Dim ret As PDDocument = load(s, scratchFile)
            s.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param filename The name of the file to load.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal filename As String) As PDDocument 'throws IOException
            Dim fs As New FileInputStream(filename)
            Dim ret As PDDocument = load(fs)
            fs.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file. Allows for skipping corrupt pdf
        ' * objects
        ' *
        ' * @param filename The name of the file to load.
        ' * @param force When true, the parser will skip corrupt pdf objects and 
        ' * will continue parsing at the next object in the file
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal filename As String, ByVal force As Boolean) As PDDocument 'throws IOException
            Dim fs As New FileInputStream(filename)
            Dim ret As PDDocument = load(fs, force)
            fs.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param filename The name of the file to load.
        ' * @param scratchFile A location to store temp PDFBox data for this document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal filename As String, ByVal scratchFile As RandomAccess) As PDDocument 'throws IOException
            Dim fs As New FileInputStream(filename)
            Dim ret As PDDocument = load(fs, scratchFile)
            fs.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param file The name of the file to load.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal file As FinSeA.Io.File) As PDDocument ' throws IOException
            Dim fs As New FileInputStream(file)
            Dim ret As PDDocument = load(fs)
            fs.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from a file.
        ' *
        ' * @param file The name of the file to load.
        ' * @param scratchFile A location to store temp PDFBox data for this document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal file As FinSeA.Io.File, ByVal scratchFile As RandomAccess) As PDDocument  'throws IOException
            Dim fs As New FileInputStream(file)
            Dim ret As PDDocument = load(fs, scratchFile)
            fs.Dispose()
            Return ret
        End Function

        '/**
        ' * This will load a document from an input stream.
        ' *
        ' * @param input The stream that contains the document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal input As InputStream) As PDDocument  'throws IOException
            Return load(input, DirectCast(Nothing, RandomAccess))
        End Function

        '/**
        ' * This will load a document from an input stream.
        ' * Allows for skipping corrupt pdf objects
        ' *
        ' * @param input The stream that contains the document.
        ' * @param force When true, the parser will skip corrupt pdf objects and 
        ' * will continue parsing at the next object in the file
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal input As InputStream, ByVal force As Boolean) As PDDocument 'throws IOException
            Return load(input, Nothing, force)
        End Function

        '/**
        ' * This will load a document from an input stream.
        ' *
        ' * @param input The stream that contains the document.
        ' * @param scratchFile A location to store temp PDFBox data for this document.
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal input As InputStream, ByVal scratchFile As RandomAccess) As PDDocument 'throws IOException
            Dim parser As pdfparser.PDFParser = New pdfparser.PDFParser(New BufferedInputStream(input), scratchFile)
            parser.parse()
            Return parser.getPDDocument()
        End Function

        '/**
        ' * This will load a document from an input stream. Allows for skipping corrupt pdf objects
        ' * 
        ' * @param input The stream that contains the document.
        ' * @param scratchFile A location to store temp PDFBox data for this document.
        ' * @param force When true, the parser will skip corrupt pdf objects and 
        ' * will continue parsing at the next object in the file
        ' *
        ' * @return The document that was loaded.
        ' *
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Shared Function load(ByVal input As InputStream, ByVal scratchFile As RandomAccess, ByVal force As Boolean) As PDDocument 'throws IOException
            Dim parser As pdfparser.PDFParser = New pdfparser.PDFParser(New BufferedInputStream(input), scratchFile, force)
            parser.parse()
            Return parser.getPDDocument()
        End Function


        '/**
        ' * Parses PDF with non sequential parser.
        ' *  
        ' * @param file  file to be loaded
        ' * @param scratchFile  location to store temp PDFBox data for this document
        ' *
        ' * @return loaded document
        ' *
        ' * @throws IOException  in case of a file reading or parsing error
        ' */
        Public Shared Function loadNonSeq(ByVal file As FinSeA.Io.File, ByVal scratchFile As RandomAccess) As PDDocument 'throws IOException
            Return loadNonSeq(file, scratchFile, "")
        End Function

        '/**
        ' * Parses PDF with non sequential parser.
        ' *  
        ' * @param file  file to be loaded
        ' * @param scratchFile  location to store temp PDFBox data for this document
        ' * @param password password to be used for decryption
        ' *
        ' * @return loaded document
        ' *
        ' * @throws IOException  in case of a file reading or parsing error
        ' */
        Public Shared Function loadNonSeq(ByVal file As FinSeA.Io.File, ByVal scratchFile As RandomAccess, ByVal password As String) As PDDocument 'throws IOException
            Dim parser As NonSequentialPDFParser = New NonSequentialPDFParser(file, scratchFile, password)
            parser.parse()
            Return parser.getPDDocument()
        End Function

        '/**
        ' * Parses PDF with non sequential parser.
        ' *  
        ' * @param input stream that contains the document.
        ' * @param scratchFile location to store temp PDFBox data for this document
        ' *
        ' * @return loaded document
        ' *
        ' * @throws IOException  in case of a file reading or parsing error
        ' */
        Public Shared Function loadNonSeq(ByVal input As InputStream, ByVal scratchFile As RandomAccess) As PDDocument 'throws IOException
            Return loadNonSeq(input, scratchFile, "")
        End Function

        '/**
        ' * Parses PDF with non sequential parser.
        ' *  
        ' * @param input stream that contains the document.
        ' * @param scratchFile location to store temp PDFBox data for this document
        ' * @param password password to be used for decryption
        ' *
        ' * @return loaded document
        ' *
        ' * @throws IOException  in case of a file reading or parsing error
        ' */
        Public Shared Function loadNonSeq(ByVal input As InputStream, ByVal scratchFile As RandomAccess, ByVal password As String) As PDDocument 'throws IOException
            Dim parser As NonSequentialPDFParser = New NonSequentialPDFParser(input, scratchFile, password)
            parser.parse()
            Return parser.getPDDocument()
        End Function

        '/**
        ' * Save the document to a file.
        ' *
        ' * @param fileName The file to save as.
        ' *
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub save(ByVal fileName As String)  'throws IOException, COSVisitorException
            Dim s As New OutputStream(New FileStream(fileName, FileMode.Create, FileAccess.Write))
            save(s)
            s.Dispose()
        End Sub

        '/**
        ' * Save the document to a file.
        ' *
        ' * @param file The file to save as.
        ' *
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub save(ByVal file As FinSeA.Io.File) 'throws IOException, COSVisitorException
            Dim s As New FileOutputStream(file)
            save(s)
            s.Dispose()
        End Sub

        '/**
        ' * This will save the document to an output stream.
        ' *
        ' * @param output The stream to write to.
        ' *
        ' * @throws IOException If there is an error writing the document.
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub save(ByVal output As OutputStream) 'throws IOException, COSVisitorException
            'update the count in case any pages have been added behind the scenes.
            getDocumentCatalog().getPages().updateCount()
            Dim writer As COSWriter = Nothing
            Try
                writer = New COSWriter(output)
                writer.write(Me)
                writer.close()
            Finally
                If (writer IsNot Nothing) Then
                    writer.close()
                End If
            End Try
        End Sub

        '/** 
        ' * Save the pdf as incremental.
        ' * 
        ' * @param fileName the filename to be used
        ' * @throws IOException if something went wrong
        ' * @throws COSVisitorException  if something went wrong
        ' */
        Public Sub saveIncremental(ByVal fileName As String)  'throws IOException, COSVisitorException
            Dim fi As New InputStream(New FileStream(fileName, FileMode.Open, FileAccess.Read))
            Dim fo As New OutputStream(New FileStream(fileName, FileMode.Create, FileAccess.Write))
            saveIncremental(fi, fo)
            fo.Dispose()
            fi.Dispose()
        End Sub

        '/** 
        ' * Save the pdf as incremental.
        ' * 
        ' * @param input 
        ' * @param output
        ' * @throws IOException if something went wrong
        ' * @throws COSVisitorException  if something went wrong
        ' */
        Public Sub saveIncremental(ByVal input As InputStream, ByVal output As OutputStream) 'throws IOException, COSVisitorException
            'update the count in case any pages have been added behind the scenes.
            getDocumentCatalog().getPages().updateCount()
            Dim writer As COSWriter = Nothing
            Try
                '// Sometimes the original file will be missing a newline at the end
                '// In order to avoid having %%EOF the first object on the same line
                '// as the %%EOF, we put a newline here.  If there's already one at
                '// the end of the file, an extra one won't hurt. PDFBOX-1051
                output.Write(Sistema.Strings.GetBytes(vbCrLf))
                writer = New COSWriter(output, input)
                writer.write(Me)
                writer.close()
            Finally
                If (writer IsNot Nothing) Then
                    writer.close()
                End If
            End Try
        End Sub

        '/**
        ' * This will return the total page count of the PDF document.  Note: This method
        ' * is deprecated in favor of the getNumberOfPages method.  The getNumberOfPages is
        ' * a required interface method of the Pageable interface.  This method will
        ' * be removed in a future version of PDFBox!!
        ' *
        ' * @return The total number of pages in the PDF document.
        ' * @deprecated Use the getNumberOfPages method instead!
        ' */
        <Obsolete> _
        Public Function getPageCount() As Integer
            Return getNumberOfPages()
        End Function

        Public Function getNumberOfPages() As Integer Implements Pageable.getNumberOfPages
            Dim cat As PDDocumentCatalog = getDocumentCatalog()
            Return cat.getPages().getCount()
        End Function

        '/**
        ' * Returns the format of the page at the given index when using a
        ' * default printer job returned by  {@link PrinterJob#getPrinterJob()}.
        ' *
        ' * @deprecated Use the {@link PDPageable} adapter class
        ' * @param pageIndex page index, zero-based
        ' * @return page format
        ' */
        <Obsolete> _
        Public Function getPageFormat(ByVal pageIndex As Integer) As PageFormat Implements Pageable.getPageFormat
            Try
                Dim printerJob As PrinterJob = printerJob.getPrinterJob()
                Return New PDPageable(Me, printerJob).getPageFormat(pageIndex)
            Catch e As PrinterException
                Throw New RuntimeException(e.Message, e)
            End Try
        End Function

        Public Function getPrintable(ByVal pageIndex As Integer) As Printable Implements Pageable.getPrintable
            Return getDocumentCatalog().getAllPages().get(pageIndex)
        End Function

        '/**
        ' * @see PDDocument#print()
        ' *
        ' * @param printJob The printer job.
        ' *
        ' * @throws PrinterException If there is an error while sending the PDF to
        ' * the printer, or you do not have permissions to print this document.
        ' */
        Public Sub print(ByVal printJob As PrinterJob)  'throws PrinterException
            print(printJob, False)
        End Sub

        '/**
        ' * This will send the PDF document to a printer.  The printing functionality
        ' * depends on the org.apache.pdfbox.pdfviewer.PageDrawer functionality.  The PageDrawer
        ' * is a work in progress and some PDFs will print correctly and some will
        ' * not.  This is a convenience method to create the java.awt.print.PrinterJob.
        ' * The PDDocument implements the java.awt.print.Pageable interface and
        ' * PDPage implementes the java.awt.print.Printable interface, so advanced printing
        ' * capabilities can be done by using those interfaces instead of this method.
        ' *
        ' * @throws PrinterException If there is an error while sending the PDF to
        ' * the printer, or you do not have permissions to print this document.
        ' */
        Public Sub print() ' throws PrinterException
            print(PrinterJob.getPrinterJob())
        End Sub

        '/**
        ' * This will send the PDF to the default printer without prompting the user
        ' * for any printer settings.
        ' *
        ' * @see PDDocument#print()
        ' *
        ' * @throws PrinterException If there is an error while printing.
        ' */
        Public Sub silentPrint() 'throws PrinterException
            silentPrint(PrinterJob.getPrinterJob())
        End Sub

        '/**
        ' * This will send the PDF to the default printer without prompting the user
        ' * for any printer settings.
        ' *
        ' * @param printJob A printer job definition.
        ' * @see PDDocument#print()
        ' *
        ' * @throws PrinterException If there is an error while printing.
        ' */
        Public Sub silentPrint(ByVal printJob As PrinterJob)  'throws PrinterException
            print(printJob, True)
        End Sub

        Private Sub print(ByVal job As PrinterJob, ByVal silent As Boolean)  'throws PrinterException 
            If (job Is Nothing) Then
                Throw New PrinterException("The given printer job is null.")
            Else
                job.setPageable(New PDPageable(Me, job))
                If (silent OrElse job.printDialog()) Then
                    job.print()
                End If
            End If
        End Sub

        '/**
        ' * This will close the underlying COSDocument object.
        ' *
        ' * @throws IOException If there is an error releasing resources.
        ' */
        Public Sub close() 'throws IOException
            document.close()
        End Sub


        '/**
        ' * Protects the document with the protection policy pp. The document content will be really encrypted
        ' * when it will be saved. This method only marks the document for encryption.
        ' *
        ' * @see org.apache.pdfbox.pdmodel.encryption.StandardProtectionPolicy
        ' * @see org.apache.pdfbox.pdmodel.encryption.PublicKeyProtectionPolicy
        ' *
        ' * @param pp The protection policy.
        ' *
        ' * @throws BadSecurityHandlerException If there is an error during protection.
        ' */
        Public Sub protect(ByVal pp As ProtectionPolicy)  'throws BadSecurityHandlerException
            Dim handler As SecurityHandler = SecurityHandlersManager.getInstance().getSecurityHandler(pp)
            securityHandler = handler
        End Sub

        '/**
        ' * Tries to decrypt the document in memory using the provided decryption material.
        ' *
        ' *  @see org.apache.pdfbox.pdmodel.encryption.StandardDecryptionMaterial
        ' *  @see org.apache.pdfbox.pdmodel.encryption.PublicKeyDecryptionMaterial
        ' *
        ' * @param pm The decryption material (password or certificate).
        ' *
        ' * @throws BadSecurityHandlerException If there is an error during decryption.
        ' * @throws IOException If there is an error reading cryptographic information.
        ' * @throws CryptographyException If there is an error during decryption.
        ' */
        Public Sub openProtection(ByVal pm As DecryptionMaterial) 'throws(BadSecurityHandlerException, IOException, CryptographyException)
            Dim dict As PDEncryptionDictionary = Me.getEncryptionDictionary()
            If (dict.getFilter() IsNot Nothing) Then
                securityHandler = SecurityHandlersManager.getInstance().getSecurityHandler(dict.getFilter())
                securityHandler.decryptDocument(Me, pm)
                document.dereferenceObjectStreams()
                document.setEncryptionDictionary(Nothing)
            Else
                Throw New RuntimeException("This document does not need to be decrypted")
            End If
        End Sub

        '/**
        ' * Returns the access permissions granted when the document was decrypted.
        ' * If the document was not decrypted this method returns the access permission
        ' * for a document owner (ie can do everything).
        ' * The returned object is in read only mode so that permissions cannot be changed.
        ' * Methods providing access to content should rely on this object to verify if the current
        ' * user is allowed to proceed.
        ' *
        ' * @return the access permissions for the current user on the document.
        ' */
        Public Function getCurrentAccessPermission() As AccessPermission
            If (Me.securityHandler Is Nothing) Then
                Return AccessPermission.getOwnerAccessPermission()
            End If
            Return securityHandler.getCurrentAccessPermission()
        End Function

        '/**
        ' * Get the security handler that is used for document encryption.
        ' *
        ' * @return The handler used to encrypt/decrypt the document.
        ' */
        Public Function getSecurityHandler() As SecurityHandler
            Return securityHandler
        End Function

        '/**
        ' * Sets security handler if none is set already.
        ' * 
        ' * @param secHandler security handler to be assigned to document
        ' * @return  <code>true</code> if security handler was set, <code>false</code>
        ' *          otherwise (a security handler was already set)
        ' */
        Public Function setSecurityHandler(ByVal secHandler As SecurityHandler) As Boolean
            If (securityHandler Is Nothing) Then
                securityHandler = secHandler
                Return True
            End If
            Return False
        End Function

        '/**
        ' * Indicates if all security is removed or not when writing the pdf.
        ' * @return returns true if all security shall be removed otherwise false
        ' */
        Public Function isAllSecurityToBeRemoved() As Boolean
            Return allSecurityToBeRemoved
        End Function

        '/**
        ' * Activates/Deactivates the removal of all security when writing the pdf.
        ' *  
        ' * @param removeAllSecurity remove all security if set to true
        ' */
        Public Sub setAllSecurityToBeRemoved(ByVal removeAllSecurity As Boolean)
            allSecurityToBeRemoved = removeAllSecurity
        End Sub

        Public Function getDocumentId() As NLong
            Return documentId
        End Function

        Public Sub setDocumentId(ByVal docId As NLong)
            documentId = docId
        End Sub

    End Class

End Namespace


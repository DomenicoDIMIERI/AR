Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.outline
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form

Namespace org.apache.pdfbox.util


    '/**
    ' * This class will take a list of pdf documents and merge them, saving the result in a new document.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDFMergerUtility

        Private Const STRUCTURETYPE_DOCUMENT = "Document"

        Private sources As List(Of InputStream)
        Private destinationFileName As String
        Private destinationStream As OutputStream
        Private ignoreAcroFormErrors As Boolean = False

        '/**
        ' * Instantiate a new PDFMergerUtility.
        ' */
        Public Sub New()
            sources = New ArrayList(Of InputStream)()
        End Sub

        '/**
        ' * Get the name of the destination file.
        ' * 
        ' * @return Returns the destination.
        ' */
        Public Function getDestinationFileName() As String
            Return destinationFileName
        End Function

        '/**
        ' * Set the name of the destination file.
        ' * 
        ' * @param destination The destination to set.
        ' */
        Public Sub setDestinationFileName(ByVal destination As String)
            Me.destinationFileName = destination
        End Sub

        '/**
        ' * Get the destination OutputStream.
        ' * 
        ' * @return Returns the destination OutputStream.
        ' */
        Public Function getDestinationStream() As OutputStream
            Return destinationStream
        End Function

        '/**
        ' * Set the destination OutputStream.
        ' * 
        ' * @param destStream The destination to set.
        ' */
        Public Sub setDestinationStream(ByVal destStream As OutputStream)
            destinationStream = destStream
        End Sub

        '/**
        ' * Add a source file to the list of files to merge.
        ' * 
        ' * @param source Full path and file name of source document.
        ' */
        Public Sub addSource(ByVal source As String)
            Try
                sources.add(New FileInputStream(source))
            Catch e As Exception
                Throw New RuntimeException(e.Message, e)
            End Try
        End Sub

        '/**
        ' * Add a source file to the list of files to merge.
        ' * 
        ' * @param source File representing source document
        ' */
        Public Sub addSource(ByVal source As System.IntPtr)
            Try
                sources.add(New FileInputStream(source))
            Catch e As Exception
                Throw New RuntimeException(e.Message, e)
            End Try
        End Sub

        '/**
        ' * Add a source to the list of documents to merge.
        ' * 
        ' * @param source InputStream representing source document
        ' */
        Public Sub addSource(ByVal source As InputStream)
            sources.add(source)
        End Sub

        '/**
        ' * Add a list of sources to the list of documents to merge.
        ' * 
        ' * @param sourcesList List of InputStream objects representing source documents
        ' */
        Public Sub addSources(ByVal sourcesList As List(Of InputStream))
            sources.addAll(sourcesList)
        End Sub

        '/**
        ' * Merge the list of source documents, saving the result in the destination file.
        ' * 
        ' * @throws IOException If there is an error saving the document.
        ' * @throws COSVisitorException If an error occurs while saving the destination file.
        ' */
        Public Sub mergeDocuments() 'throws IOException, COSVisitorException
            Dim destination As PDDocument = Nothing
            Dim sourceFile As InputStream
            Dim source As PDDocument
            If (sources IsNot Nothing AndAlso sources.size() > 0) Then
                Dim tobeclosed As Vector(Of PDDocument) = New Vector(Of PDDocument)()

                Try
                    'Iterator(Of InputStream) sit = sources.iterator();
                    Dim sit As Global.System.Collections.Generic.IEnumerator(Of InputStream) = sources.iterator
                    sit.MoveNext()
                    sourceFile = sit.Current
                    destination = PDDocument.load(sourceFile)

                    While (sit.MoveNext)
                        sourceFile = sit.Current
                        source = PDDocument.load(sourceFile)
                        tobeclosed.add(source)
                        appendDocument(destination, source)
                    End While
                    If (destinationStream Is Nothing) Then
                        destination.save(destinationFileName)
                    Else
                        destination.save(destinationStream)
                    End If
                Finally
                    If (destination IsNot Nothing) Then
                        destination.close()
                    End If
                    For Each doc As PDDocument In tobeclosed
                        doc.close()
                    Next
                End Try
            End If
        End Sub

        '/**
        ' * append all pages from source to destination.
        ' * 
        ' * @param destination the document to receive the pages
        ' * @param source the document originating the new pages
        ' * 
        ' * @throws IOException If there is an error accessing data from either document.
        ' */
        Public Sub appendDocument(ByVal destination As PDDocument, ByVal source As PDDocument)  'throws IOException
            If (destination.isEncrypted()) Then
                Throw New IOException("Error: destination PDF is encrypted, can't append encrypted PDF documents.")
            End If
            If (source.isEncrypted()) Then
                Throw New IOException("Error: source PDF is encrypted, can't append encrypted PDF documents.")
            End If
            Dim destInfo As PDDocumentInformation = destination.getDocumentInformation()
            Dim srcInfo As PDDocumentInformation = source.getDocumentInformation()
            destInfo.getDictionary().mergeInto(srcInfo.getDictionary())

            Dim destCatalog As PDDocumentCatalog = destination.getDocumentCatalog()
            Dim srcCatalog As PDDocumentCatalog = source.getDocumentCatalog()

            ' use the highest version number for the resulting pdf
            Dim destVersion As Single = destination.getDocument().getVersion()
            Dim srcVersion As Single = source.getDocument().getVersion()

            If (destVersion < srcVersion) Then
                destination.getDocument().setVersion(srcVersion)
            End If

            If (destCatalog.getOpenAction() Is Nothing) Then
                destCatalog.setOpenAction(srcCatalog.getOpenAction())
            End If

            ' maybe there are some shared resources for all pages
            Dim srcPages As COSDictionary = srcCatalog.getCOSDictionary().getDictionaryObject(COSName.PAGES)
            Dim srcResources As COSDictionary = srcPages.getDictionaryObject(COSName.RESOURCES)
            Dim destPages As COSDictionary = destCatalog.getCOSDictionary().getDictionaryObject(COSName.PAGES)
            Dim destResources As COSDictionary = destPages.getDictionaryObject(COSName.RESOURCES)
            If (srcResources IsNot Nothing) Then
                If (destResources IsNot Nothing) Then
                    destResources.mergeInto(srcResources)
                Else
                    destPages.setItem(COSName.RESOURCES, srcResources)
                End If
            End If

            Dim cloner As PDFCloneUtility = New PDFCloneUtility(destination)

            Try
                Dim destAcroForm As PDAcroForm = destCatalog.getAcroForm()
                Dim srcAcroForm As PDAcroForm = srcCatalog.getAcroForm()
                If (destAcroForm Is Nothing) Then
                    cloner.cloneForNewDocument(srcAcroForm)
                    destCatalog.setAcroForm(srcAcroForm)
                Else
                    If (srcAcroForm IsNot Nothing) Then
                        mergeAcroForm(cloner, destAcroForm, srcAcroForm)
                    End If
                End If
            Catch e As Exception
                ' if we are not ignoring exceptions, we'll re-throw this
                If (Not ignoreAcroFormErrors) Then
                    Throw e
                End If
            End Try

            Dim destThreads As COSArray = destCatalog.getCOSDictionary().getDictionaryObject(COSName.THREADS)
            Dim srcThreads As COSArray = cloner.cloneForNewDocument(destCatalog.getCOSDictionary().getDictionaryObject(COSName.THREADS))
            If (destThreads Is Nothing) Then
                destCatalog.getCOSDictionary().setItem(COSName.THREADS, srcThreads)
            Else
                destThreads.addAll(srcThreads)
            End If

            Dim destNames As PDDocumentNameDictionary = destCatalog.getNames()
            Dim srcNames As PDDocumentNameDictionary = srcCatalog.getNames()
            If (srcNames IsNot Nothing) Then
                If (destNames Is Nothing) Then
                    destCatalog.getCOSDictionary().setItem(COSName.NAMES, cloner.cloneForNewDocument(srcNames))
                Else
                    cloner.cloneMerge(srcNames, destNames)
                End If
            End If

            Dim destOutline As PDDocumentOutline = destCatalog.getDocumentOutline()
            Dim srcOutline As PDDocumentOutline = srcCatalog.getDocumentOutline()
            If (srcOutline IsNot Nothing) Then
                If (destOutline Is Nothing) Then
                    Dim cloned As PDDocumentOutline = New PDDocumentOutline(cloner.cloneForNewDocument(srcOutline))
                    destCatalog.setDocumentOutline(cloned)
                Else
                    Dim first As PDOutlineItem = srcOutline.getFirstChild()
                    If (first IsNot Nothing) Then
                        Dim clonedFirst As PDOutlineItem = New PDOutlineItem(cloner.cloneForNewDocument(first))
                        destOutline.appendChild(clonedFirst)
                    End If
                End If
            End If

            Dim destPageMode As String = destCatalog.getPageMode()
            Dim srcPageMode As String = srcCatalog.getPageMode()
            If (destPageMode Is Nothing) Then
                destCatalog.setPageMode(srcPageMode)
            End If

            Dim destLabels As COSDictionary = destCatalog.getCOSDictionary().getDictionaryObject(COSName.PAGE_LABELS)
            Dim srcLabels As COSDictionary = srcCatalog.getCOSDictionary().getDictionaryObject(COSName.PAGE_LABELS)
            If (srcLabels IsNot Nothing) Then
                Dim destPageCount As Integer = destination.getNumberOfPages()
                Dim destNums As COSArray = Nothing
                If (destLabels Is Nothing) Then
                    destLabels = New COSDictionary()
                    destNums = New COSArray()
                    destLabels.setItem(COSName.NUMS, destNums)
                    destCatalog.getCOSDictionary().setItem(COSName.PAGE_LABELS, destLabels)
                Else
                    destNums = destLabels.getDictionaryObject(COSName.NUMS)
                End If
                Dim srcNums As COSArray = srcLabels.getDictionaryObject(COSName.NUMS)
                If (srcNums IsNot Nothing) Then
                    For i As Integer = 0 To srcNums.size() - 1 Step 2
                        Dim labelIndex As COSNumber = srcNums.getObject(i)
                        Dim labelIndexValue As Long = labelIndex.intValue()
                        destNums.add(COSInteger.get(labelIndexValue + destPageCount))
                        destNums.add(cloner.cloneForNewDocument(srcNums.getObject(i + 1)))
                    Next
                End If
            End If

            Dim destMetadata As COSStream = destCatalog.getCOSDictionary().getDictionaryObject(COSName.METADATA)
            Dim srcMetadata As COSStream = srcCatalog.getCOSDictionary().getDictionaryObject(COSName.METADATA)
            If (destMetadata Is Nothing AndAlso srcMetadata IsNot Nothing) Then
                Dim newStream As PDStream = New PDStream(destination, srcMetadata.getUnfilteredStream(), False)
                newStream.getStream().mergeInto(srcMetadata)
                newStream.addCompression()
                destCatalog.getCOSDictionary().setItem(COSName.METADATA, newStream)
            End If

            ' merge logical structure hierarchy if logical structure information is available in both source pdf and
            ' destination pdf
            Dim mergeStructTree As Boolean = False
            Dim destParentTreeNextKey As Integer = -1
            Dim destParentTreeDict As COSDictionary = Nothing
            Dim srcParentTreeDict As COSDictionary = Nothing
            Dim destNumbersArray As COSArray = Nothing
            Dim srcNumbersArray As COSArray = Nothing
            Dim destMark As PDMarkInfo = destCatalog.getMarkInfo()
            Dim destStructTree As PDStructureTreeRoot = destCatalog.getStructureTreeRoot()
            Dim srcMark As PDMarkInfo = srcCatalog.getMarkInfo()
            Dim srcStructTree As PDStructureTreeRoot = srcCatalog.getStructureTreeRoot()
            If (destStructTree IsNot Nothing) Then
                Dim destParentTree As PDNumberTreeNode = destStructTree.getParentTree()
                destParentTreeNextKey = destStructTree.getParentTreeNextKey()
                If (destParentTree IsNot Nothing) Then
                    destParentTreeDict = destParentTree.getCOSDictionary()
                    destNumbersArray = destParentTreeDict.getDictionaryObject(COSName.NUMS)
                    If (destNumbersArray IsNot Nothing) Then
                        If (destParentTreeNextKey < 0) Then
                            destParentTreeNextKey = destNumbersArray.size() / 2
                        End If
                        If (destParentTreeNextKey > 0) Then
                            If (srcStructTree IsNot Nothing) Then
                                Dim srcParentTree As PDNumberTreeNode = srcStructTree.getParentTree()
                                If (srcParentTree IsNot Nothing) Then
                                    srcParentTreeDict = srcParentTree.getCOSDictionary()
                                    srcNumbersArray = srcParentTreeDict.getDictionaryObject(COSName.NUMS)
                                    If (srcNumbersArray IsNot Nothing) Then
                                        mergeStructTree = True
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                If (destMark IsNot Nothing AndAlso destMark.isMarked() AndAlso Not mergeStructTree) Then
                    destMark.setMarked(False)
                End If
                If (Not mergeStructTree) Then
                    destCatalog.setStructureTreeRoot(Nothing)
                End If
            End If

            '// finally append the pages
            Dim pages As List(Of PDPage) = srcCatalog.getAllPages()
            Dim pageIter As Global.System.Collections.Generic.IEnumerator(Of PDPage) = pages.iterator 'Iterator(Of PDPage) pageIter = pages.iterator();
            Dim objMapping As HashMap(Of COSDictionary, COSDictionary) = New HashMap(Of COSDictionary, COSDictionary)()
            While (pageIter.MoveNext)
                Dim page As PDPage = pageIter.Current
                Dim newPage As PDPage = New PDPage(cloner.cloneForNewDocument(page.getCOSDictionary()))
                newPage.setCropBox(page.findCropBox())
                newPage.setMediaBox(page.findMediaBox())
                newPage.setRotation(page.findRotation())
                If (mergeStructTree) Then
                    updateStructParentEntries(newPage, destParentTreeNextKey)
                    objMapping.put(page.getCOSDictionary(), newPage.getCOSDictionary())
                    Dim oldAnnots As List(Of PDAnnotation) = page.getAnnotations()
                    Dim newAnnots As List(Of PDAnnotation) = newPage.getAnnotations()
                    For i As Integer = 0 To oldAnnots.size() - 1
                        objMapping.put(oldAnnots.get(i).getDictionary(), newAnnots.get(i).getDictionary())
                    Next
                    ' TODO update mapping for XObjects
                End If
                destination.addPage(newPage)
            End While
            If (mergeStructTree) Then
                updatePageReferences(srcNumbersArray, objMapping)
                For i As Integer = 0 To srcNumbersArray.size() / 2 - 1
                    destNumbersArray.add(COSInteger.get(destParentTreeNextKey + i))
                    destNumbersArray.add(srcNumbersArray.getObject(i * 2 + 1))
                Next
                destParentTreeNextKey += srcNumbersArray.size() / 2
                destParentTreeDict.setItem(COSName.NUMS, destNumbersArray)
                Dim newParentTreeNode As PDNumberTreeNode = New PDNumberTreeNode(destParentTreeDict, GetType(COSBase))
                destStructTree.setParentTree(newParentTreeNode)
                destStructTree.setParentTreeNextKey(destParentTreeNextKey)

                Dim kDictLevel0 As COSDictionary = New COSDictionary()
                Dim newKArray As COSArray = New COSArray()
                Dim destKArray As COSArray = destStructTree.getKArray()
                Dim srcKArray As COSArray = srcStructTree.getKArray()
                If (destKArray IsNot Nothing AndAlso srcKArray IsNot Nothing) Then
                    updateParentEntry(destKArray, kDictLevel0)
                    newKArray.addAll(destKArray)
                    If (mergeStructTree) Then
                        updateParentEntry(srcKArray, kDictLevel0)
                    End If
                    newKArray.addAll(srcKArray)
                End If
                kDictLevel0.setItem(COSName.K, newKArray)
                kDictLevel0.setItem(COSName.P, destStructTree)
                kDictLevel0.setItem(COSName.S, New COSString(STRUCTURETYPE_DOCUMENT))
                destStructTree.setK(kDictLevel0)
            End If
        End Sub

        Private nextFieldNum As Integer = 1

        '/**
        ' * Merge the contents of the source form into the destination form for the destination file.
        ' * 
        ' * @param cloner the object cloner for the destination document
        ' * @param destAcroForm the destination form
        ' * @param srcAcroForm the source form
        ' * @throws IOException If an error occurs while adding the field.
        ' */
        Private Sub mergeAcroForm(ByVal cloner As PDFCloneUtility, ByVal destAcroForm As PDAcroForm, ByVal srcAcroForm As PDAcroForm)
            Dim destFields As List = destAcroForm.getFields()
            Dim srcFields As List = srcAcroForm.getFields()
            If (srcFields IsNot Nothing) Then
                If (destFields Is Nothing) Then
                    destFields = New COSArrayList()
                    destAcroForm.setFields(destFields)
                End If
                Dim srcFieldsIterator As Global.System.Collections.Generic.IEnumerator(Of PDField) = srcFields.iterator()
                While (srcFieldsIterator.MoveNext)
                    Dim srcField As PDField = srcFieldsIterator.Current
                    Dim destField As PDField = PDFieldFactory.createField(destAcroForm, cloner.cloneForNewDocument(srcField.getDictionary()))
                    ' if the form already has a field with this name then we need to rename this field
                    ' to prevent merge conflicts.
                    If (destAcroForm.getField(destField.getFullyQualifiedName()) IsNot Nothing) Then
                        destField.setPartialName("dummyFieldName" & (nextFieldNum))
                        nextFieldNum += 1
                    End If
                    destFields.add(destField)
                End While
            End If
        End Sub

        '/**
        ' * Indicates if acroform errors are ignored or not.
        ' * 
        ' * @return true if acroform errors are ignored
        ' */
        Public Function isIgnoreAcroFormErrors() As Boolean
            Return ignoreAcroFormErrors
        End Function

        '/**
        ' * Set to true to ignore acroform errors.
        ' * 
        ' * @param ignoreAcroFormErrorsValue true if acroform errors should be ignored
        ' */
        Public Sub setIgnoreAcroFormErrors(ByVal ignoreAcroFormErrorsValue As Boolean)
            ignoreAcroFormErrors = ignoreAcroFormErrorsValue
        End Sub

        '/**
        ' * Update the Pg and Obj references to the new (merged) page.
        ' * 
        ' * @param parentTreeEntry
        ' * @param objMapping mapping between old and new references
        ' */
        Private Sub updatePageReferences(ByVal parentTreeEntry As COSDictionary, ByVal objMapping As HashMap(Of COSDictionary, COSDictionary))
            Dim page As COSBase = parentTreeEntry.getDictionaryObject(COSName.PG)
            If (TypeOf (page) Is COSDictionary) Then
                If (objMapping.containsKey(page)) Then
                    parentTreeEntry.setItem(COSName.PG, objMapping.get(page))
                End If
            End If
            Dim obj As COSBase = parentTreeEntry.getDictionaryObject(COSName.OBJ)
            If (TypeOf (obj) Is COSDictionary) Then
                If (objMapping.containsKey(obj)) Then
                    parentTreeEntry.setItem(COSName.OBJ, objMapping.get(obj))
                End If
            End If
            Dim kSubEntry As COSBase = parentTreeEntry.getDictionaryObject(COSName.K)
            If (TypeOf (kSubEntry) Is COSArray) Then
                updatePageReferences(DirectCast(kSubEntry, COSArray), objMapping)
            ElseIf (TypeOf (kSubEntry) Is COSDictionary) Then
                updatePageReferences(DirectCast(kSubEntry, COSDictionary), objMapping)
            End If
        End Sub

        Private Sub updatePageReferences(ByVal parentTreeEntry As COSArray, ByVal objMapping As HashMap(Of COSDictionary, COSDictionary))
            For i As Integer = 0 To parentTreeEntry.size() - 1
                Dim subEntry As COSBase = parentTreeEntry.getObject(i)
                If (TypeOf (subEntry) Is COSArray) Then
                    updatePageReferences(DirectCast(subEntry, COSArray), objMapping)
                ElseIf (TypeOf (subEntry) Is COSDictionary) Then
                    updatePageReferences(DirectCast(subEntry, COSDictionary), objMapping)
                End If
            Next
        End Sub

        '/**
        ' * Update the P reference to the new parent dictionary.
        ' * 
        ' * @param kArray the kids array
        ' * @param newParent the new parent
        ' */
        Private Sub updateParentEntry(ByVal kArray As COSArray, ByVal newParent As COSDictionary)
            For i As Integer = 0 To kArray.size() - 1
                Dim subEntry As COSBase = kArray.getObject(i)
                If (TypeOf (subEntry) Is COSDictionary) Then
                    Dim dictEntry As COSDictionary = subEntry
                    If (dictEntry.getDictionaryObject(COSName.P) IsNot Nothing) Then
                        dictEntry.setItem(COSName.P, newParent)
                    End If
                End If
            Next
        End Sub

        '/**
        ' * Update the StructParents and StructParent values in a PDPage.
        ' * 
        ' * @param page the new page
        ' * @param structParentOffset the offset which should be applied
        ' */
        Private Sub updateStructParentEntries(ByVal page As PDPage, ByVal structParentOffset As Integer) ' throws IOException
            page.setStructParents(page.getStructParents() + structParentOffset)
            Dim annots As List(Of PDAnnotation) = page.getAnnotations()
            Dim newannots As List(Of PDAnnotation) = New ArrayList(Of PDAnnotation)()
            For Each annot As PDAnnotation In annots
                annot.setStructParent(annot.getStructParent() + structParentOffset)
                newannots.add(annot)
            Next
            page.setAnnotations(newannots)
        End Sub

    End Class

End Namespace

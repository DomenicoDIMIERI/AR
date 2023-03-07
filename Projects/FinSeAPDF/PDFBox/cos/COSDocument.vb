Imports FinSeA.Io
Imports System.IO

Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
Imports FinSeA.org.apache.pdfbox.persistence.util

Namespace org.apache.pdfbox.cos

    '/**
    ' * This is the in-memory representation of the PDF document.  You need to call
    ' * close() on this object when you are done using it!!
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.28 $
    ' */
    Public Class COSDocument
        Inherits COSBase
        Implements IDisposable

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(COSDocument.class);

        Private version As Single = 1.4

        '/**
        ' * Maps ObjectKeys to a COSObject. Note that references to these objects
        ' * are also stored in COSDictionary objects that map a name to a specific object.
        ' */
        Private objectPool As Map(Of COSObjectKey, COSObject) = New HashMap(Of COSObjectKey, COSObject)

        '/**
        ' * Maps object and generation id to object byte offsets.
        ' */
        Private xrefTable As New HashMap(Of COSObjectKey, Integer) ' Map<COSObjectKey, Long> xrefTable = new HashMap<COSObjectKey, Long>();

        '/**
        ' * Document trailer dictionary.
        ' */
        Private trailer As COSDictionary

        '/**
        ' * Signature interface.
        ' */
        Private signatureInterface As SignatureInterface

        '/**
        ' * This file will store the streams in order to conserve memory.
        ' */
        Private scratchFile As pdfbox.io.RandomAccess

        Private tmpFile As String = ""

        Private headerString As String = "%PDF-" & version

        Private warnMissingClose As Boolean = True

        ' signal that document is already decrypted, e.g. with {@link NonSequentialPDFParser} */
        Private isDecrypted As Boolean = False

        Private startXref As Integer

        Private closed As Boolean = False

        '/**
        ' * Flag to skip malformed or otherwise unparseable input where possible.
        ' */
        Private forceParsing As Boolean

        '/**
        ' * Constructor that will use the given random access file for storage
        ' * of the PDF streams. The client of this method is responsible for
        ' * deleting the storage if necessary that this file will write to. The
        ' * close method will close the file though.
        ' *
        ' * @param scratchFileValue the random access file to use for storage
        ' * @param forceParsingValue flag to skip malformed or otherwise unparseable
        ' *                     document content where possible
        ' */
        Public Sub New(ByVal scratchFileValue As RandomAccess, ByVal forceParsingValue As Boolean)
            Me.scratchFile = scratchFileValue
            Me.tmpFile = Nothing
            Me.forceParsing = forceParsingValue
        End Sub

        '/**
        ' * Constructor that will use a temporary file in the given directory
        ' * for storage of the PDF streams. The temporary file is automatically
        ' * removed when this document gets closed.
        ' *
        ' * @param scratchDir directory for the temporary file,
        ' *                   or <code>null</code> to use the system default
        ' * @param forceParsingValue flag to skip malformed or otherwise unparseable
        ' *                     document content where possible
        ' * @throws IOException if something went wrong
        ' */
        Public Sub New(ByVal scratchDir As FinSeA.Io.File, ByVal forceParsingValue As Boolean) ' throws IOException 
            Me.tmpFile = FinSeA.Sistema.FileSystem.GetTempFileName(".tmp") ', scratchDir)
            Me.scratchFile = New pdfbox.io.RandomAccessFile(New FinSeA.Io.File(tmpFile), "rw")
            Me.forceParsing = forceParsingValue
        End Sub

        '/**
        ' * Constructor.  Uses memory to store stream.
        ' *
        ' *  @throws IOException If there is an error creating the tmp file.
        ' */
        Public Sub New() 'throws IOException 
            Me.New(New RandomAccessBuffer(), False)
        End Sub

        '/**
        ' * Constructor that will create a create a scratch file in the
        ' * following directory.
        ' *
        ' * @param scratchDir The directory to store a scratch file.
        ' *
        ' * @throws IOException If there is an error creating the tmp file.
        ' */
        Public Sub New(ByVal scratchDir As FinSeA.Io.File) 'throws IOException 
            Me.New(scratchDir, False)
        End Sub

        '/**
        ' * Constructor that will use the following random access file for storage
        ' * of the PDF streams.  The client of this method is responsible for deleting
        ' * the storage if necessary that this file will write to.  The close method
        ' * will close the file though.
        ' *
        ' * @param file The random access file to use for storage.
        ' */
        Public Sub New(ByVal file As RandomAccess)
            Me.New(file, False)
        End Sub

        '/**
        ' * This will get the scratch file for this document.
        ' *
        ' * @return The scratch file.
        ' * 
        ' * 
        ' */
        Public Function getScratchFile() As RandomAccess
            ' TODO the direct access to the scratch file should be removed.
            If (Not Me.closed) Then
                Return Me.scratchFile
            Else
                Debug.Print("Can't access the scratch file as it is already closed!")
                Return Nothing
            End If
        End Function

        '/**
        ' * Create a new COSStream using the underlying scratch file.
        ' * 
        ' * @return the new COSStream
        ' */
        Public Function createCOSStream() As COSStream
            Return New COSStream(getScratchFile())
        End Function

        '/**
        ' * Create a new COSStream using the underlying scratch file.
        ' *
        ' * @param dictionary the corresponding dictionary
        ' * 
        ' * @return the new COSStream
        ' */
        Public Function createCOSStream(ByVal dictionary As COSDictionary) As COSStream
            Return New COSStream(dictionary, getScratchFile())
        End Function

        '/**
        ' * This will get the first dictionary object by type.
        ' *
        ' * @param type The type of the object.
        ' *
        ' * @return This will return an object with the specified type.
        ' * @throws IOException If there is an error getting the object
        ' */
        Public Function getObjectByType(ByVal type As String) As COSObject ' throws IOException 
            Return getObjectByType(COSName.getPDFName(type))
        End Function

        '/**
        ' * This will get the first dictionary object by type.
        ' *
        ' * @param type The type of the object.
        ' *
        ' * @return This will return an object with the specified type.
        ' * @throws IOException If there is an error getting the object
        ' */
        Public Function getObjectByType(ByVal type As COSName) As COSObject 'throws IOException
            For Each [object] As COSObject In objectPool.values()
                Dim realObject As COSBase = [object].getObject()
                If (TypeOf (realObject) Is COSDictionary) Then
                    Try
                        Dim dic As COSDictionary = realObject
                        Dim objectType As COSName = dic.getItem(COSName.TYPE)
                        If (objectType IsNot Nothing AndAlso objectType.equals(type)) Then
                            Return [object]
                        End If
                    Catch e As InvalidCastException
                        Debug.Print(e.ToString)
                    End Try
                End If
            Next
            Return Nothing
        End Function

        '/**
        ' * This will get all dictionary objects by type.
        ' *
        ' * @param type The type of the object.
        ' *
        ' * @return This will return an object with the specified type.
        ' * @throws IOException If there is an error getting the object
        ' */
        Public Function getObjectsByType(ByVal type As String) As List(Of COSObject) 'throws IOException
            Return getObjectsByType(COSName.getPDFName(type))
        End Function

        '/**
        ' * This will get a dictionary object by type.
        ' *
        ' * @param type The type of the object.
        ' *
        ' * @return This will return an object with the specified type.
        ' * @throws IOException If there is an error getting the object
        ' */
        Public Function getObjectsByType(ByVal type As COSName) As List(Of COSObject) 'throws IOException
            Dim retval As New ArrayList(Of COSObject)
            For Each [object] As COSObject In objectPool.values()
                Dim realObject As COSBase = [object].getObject()
                If (TypeOf (realObject) Is COSDictionary) Then
                    Try
                        Dim dic As COSDictionary = realObject
                        Dim objectType As COSName = dic.getItem(COSName.TYPE)
                        If (objectType IsNot Nothing AndAlso objectType.equals(type)) Then
                            retval.add([object])
                        End If
                    Catch e As InvalidCastException
                        Debug.Print(e.ToString)
                    End Try
                End If
            Next
            Return retval
        End Function

        '/**
        ' * This will print contents to stdout.
        ' */
        Public Sub print()
            For Each [object] As COSObject In objectPool.values()
                Debug.Print([object].toString)
            Next
        End Sub

        '/**
        ' * This will set the version of this PDF document.
        ' *
        ' * @param versionValue The version of the PDF document.
        ' */
        Public Sub setVersion(ByVal versionValue As Single)
            ' update header string
            If (versionValue <> version) Then
                headerString = headerString.Replace(Val(version), Val(versionValue))
            End If
            version = versionValue
        End Sub

        '/**
        ' * This will get the version of this PDF document.
        ' *
        ' * @return This documents version.
        ' */
        Public Function getVersion() As Single
            Return version
        End Function

        '/** Signals that the document is decrypted completely.
        ' *  Needed e.g. by {@link NonSequentialPDFParser} to circumvent
        ' *  additional decryption later on. */
        Public Sub setDecrypted()
            isDecrypted = True
        End Sub

        '/**
        ' * This will tell if this is an encrypted document.
        ' *
        ' * @return true If this document is encrypted.
        ' */
        Public Function isEncrypted() As Boolean
            If (isDecrypted) Then
                Return False
            End If
            Dim encrypted As Boolean = False
            If (trailer IsNot Nothing) Then
                encrypted = trailer.getDictionaryObject(COSName.ENCRYPT) IsNot Nothing
            End If
            Return encrypted
        End Function

        '/**
        ' * This will get the encryption dictionary if the document is encrypted or null
        ' * if the document is not encrypted.
        ' *
        ' * @return The encryption dictionary.
        ' */
        Public Function getEncryptionDictionary() As COSDictionary
            Return trailer.getDictionaryObject(COSName.ENCRYPT)
        End Function

        '/**
        ' * This will return the signature interface.
        ' * @return the signature interface 
        ' */
        Public Function getSignatureInterface() As SignatureInterface
            Return signatureInterface
        End Function

        '/**
        ' * This will set the encryption dictionary, this should only be called when
        ' * encrypting the document.
        ' *
        ' * @param encDictionary The encryption dictionary.
        ' */
        Public Sub setEncryptionDictionary(ByVal encDictionary As COSDictionary)
            trailer.setItem(COSName.ENCRYPT, encDictionary)
        End Sub

        '/**
        ' * This will return a list of signature dictionaries as COSDictionary.
        ' *
        ' * @return list of signature dictionaries as COSDictionary
        ' * @throws IOException if no document catalog can be found
        ' */
        Public Function getSignatureDictionaries() As List(Of COSDictionary) 'throws IOException
            Dim signatureFields As List(Of COSDictionary) = getSignatureFields(False)
            Dim signatures As List(Of COSDictionary) = New ArrayList(Of COSDictionary)
            For Each dict As COSDictionary In signatureFields
                Dim dictionaryObject As COSBase = dict.getDictionaryObject(COSName.V)
                If (dictionaryObject IsNot Nothing) Then
                    signatures.Add(dictionaryObject)
                End If
            Next
            Return signatures
        End Function

        '/**
        ' * This will return a list of signature fields.
        ' *
        ' * @return list of signature dictionaries as COSDictionary
        ' * @throws IOException if no document catalog can be found
        ' */
        Public Function getSignatureFields(ByVal onlyEmptyFields As Boolean) As List(Of COSDictionary) 'throws IOException
            Dim documentCatalog As COSObject = getCatalog()
            If (documentCatalog IsNot Nothing) Then
                Dim acroForm As COSDictionary = documentCatalog.getDictionaryObject(COSName.ACRO_FORM)
                If (acroForm IsNot Nothing) Then
                    Dim fields As COSArray = acroForm.getDictionaryObject(COSName.FIELDS)
                    If (fields IsNot Nothing) Then
                        ' Some fields may contain twice references to a single field. 
                        ' This will prevent such double entries.
                        Dim signatures As New HashMap(Of COSObjectKey, COSDictionary)
                        For Each [object] As Object In fields
                            Dim dict As COSObject = [object]
                            If (COSName.SIG.Equals(dict.getItem(COSName.FT))) Then
                                Dim dictionaryObject As COSBase = dict.getDictionaryObject(COSName.V)
                                If (dictionaryObject IsNot Nothing OrElse (dictionaryObject IsNot Nothing AndAlso Not onlyEmptyFields)) Then
                                    signatures.put(New COSObjectKey(dict), dict.getObject())
                                End If
                            End If
                        Next
                        Return New LinkedList(Of COSDictionary)(signatures.values())
                    End If
                End If
            End If
            Return New ArrayList(Of COSDictionary)
        End Function

        '/**
        ' * This will get the document ID.
        ' *
        ' * @return The document id.
        ' */
        Public Function getDocumentID() As COSArray
            Return getTrailer().getDictionaryObject(COSName.ID)
        End Function

        '/**
        ' * This will set the document ID.
        ' *
        ' * @param id The document id.
        ' */
        Public Sub setDocumentID(ByVal id As COSArray)
            getTrailer().setItem(COSName.ID, id)
        End Sub

        '/**
        ' * Set the signature interface to the given value.
        ' * @param sigInterface the signature interface
        ' */
        Public Sub setSignatureInterface(ByVal sigInterface As SignatureInterface)
            signatureInterface = sigInterface
        End Sub

        '/**
        ' * This will get the document catalog.
        ' *
        ' * Maybe this should move to an object at PDFEdit level
        ' *
        ' * @return catalog is the root of all document activities
        ' *
        ' * @throws IOException If no catalog can be found.
        ' */
        Public Function getCatalog() As COSObject 'throws IOException
            Dim catalog As COSObject = getObjectByType(COSName.CATALOG)
            If (catalog IsNot Nothing) Then
                Throw New FormatException("Catalog cannot be found")
            End If
            Return catalog
        End Function

        '/**
        ' * This will get a list of all available objects.
        ' *
        ' * @return A list of all objects.
        ' */
        Public Function getObjects() As List(Of COSObject)
            Return New ArrayList(Of COSObject)(objectPool.Values())
        End Function

        '/**
        ' * This will get the document trailer.
        ' *
        ' * @return the document trailer dict
        ' */
        Public Function getTrailer() As COSDictionary
            Return trailer
        End Function

        '/**
        ' * // MIT added, maybe this should not be supported as trailer is a persistence construct.
        ' * This will set the document trailer.
        ' *
        ' * @param newTrailer the document trailer dictionary
        ' */
        Public Sub setTrailer(ByVal newTrailer As COSDictionary)
            trailer = newTrailer
        End Sub

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object ' throws COSVisitorException
            Return visitor.visitFromDocument(Me)
        End Function

        '/**
        ' * This will close all storage and delete the tmp files.
        ' *
        ' *  @throws IOException If there is an error close resources.
        ' */
        Public Sub close() 'throws IOException
            If (Not closed) Then
                scratchFile.close()
                If (tmpFile IsNot Nothing) Then
                    FinSeA.Sistema.FileSystem.DeleteFile(tmpFile, True)
                End If
                closed = True
            End If
        End Sub

        '/**
        ' * Warn the user in the finalizer if he didn't close the PDF document. The method also
        ' * closes the document just in case, to avoid abandoned temporary files. It's still a good
        ' * idea for the user to close the PDF document at the earliest possible to conserve resources.
        ' * @throws IOException if an error occurs while closing the temporary files
        ' */
        Public Sub Dispose() Implements IDisposable.Dispose
            If (Not Me.closed) Then
                If (Me.warnMissingClose) Then
                    Debug.Print("Warning: You did not close a PDF Document")
                End If
                Me.close()
            End If
        End Sub

        '/**
        ' * Controls whether this instance shall issue a warning if the PDF document wasn't closed
        ' * properly through a call to the {@link #close()} method. If the PDF document is held in
        ' * a cache governed by soft references it is impossible to reliably close the document
        ' * before the warning is raised. By default, the warning is enabled.
        ' * @param warn true enables the warning, false disables it.
        ' */
        Public Sub setWarnMissingClose(ByVal warn As Boolean)
            Me.warnMissingClose = warn
        End Sub

        '/**
        ' * @return Returns the headerString.
        ' */
        Public Function getHeaderString() As String
            Return headerString
        End Function

        '/**
        ' * @param header The headerString to set.
        ' */
        Public Sub setHeaderString(ByVal header As String)
            headerString = header
        End Sub

        '/**
        ' * This method will search the list of objects for types of ObjStm.  If it finds
        ' * them then it will parse out all of the objects from the stream that is contains.
        ' *
        ' * @throws IOException If there is an error parsing the stream.
        ' */
        Public Sub dereferenceObjectStreams() 'throws IOException
            For Each objStream As COSObject In getObjectsByType(COSName.OBJ_STM)
                Dim stream As COSStream = objStream.getObject()
                Dim parser As New PDFObjectStreamParser(stream, Me, forceParsing)
                parser.parse()
                For Each [next] As COSObject In parser.getObjects()
                    Dim key As New COSObjectKey([next])
                    '// xrefTable stores negated objNr of objStream for objects in objStreams
                    If (objectPool.get(key) Is Nothing OrElse objectPool.get(key).getObject() Is Nothing OrElse (xrefTable.containsKey(key) AndAlso xrefTable.get(key) = -objStream.getObjectNumber().longValue())) Then
                        Dim obj As COSObject = getObjectFromPool(key)
                        obj.setObject([next].getObject())
                    End If
                Next
            Next
        End Sub

        '/**
        ' * This will get an object from the pool.
        ' *
        ' * @param key The object key.
        ' *
        ' * @return The object in the pool or a new one if it has not been parsed yet.
        ' *
        ' * @throws IOException If there is an error getting the proxy object.
        ' */
        Public Function getObjectFromPool(ByVal key As COSObjectKey) As COSObject  'throws IOException
            Dim obj As COSObject = Nothing
            If (key IsNot Nothing) Then
                obj = objectPool.get(key)
            End If
            If (obj IsNot Nothing) Then
                ' this was a forward reference, make "proxy" object
                obj = New COSObject(Nothing)
                If (key IsNot Nothing) Then
                    obj.setObjectNumber(COSInteger.get(key.getNumber()))
                    obj.setGenerationNumber(COSInteger.get(key.getGeneration()))
                    objectPool.put(key, obj)
                End If
            End If
            Return obj
        End Function

        '/**
        ' * Removes an object from the object pool.
        ' * @param key the object key
        ' * @return the object that was removed or null if the object was not found
        ' */
        Public Function removeObject(ByVal key As COSObjectKey) As COSObject
            Return objectPool.remove(key)
        End Function

        '/**
        ' * Populate XRef HashMap with given values.
        ' * Each entry maps ObjectKeys to byte offsets in the file.
        ' * @param xrefTableValues  xref table entries to be added
        ' */
        Public Sub addXRefTable(ByVal xrefTableValues As Map(Of COSObjectKey, Integer))
            xrefTable.putAll(xrefTableValues)
        End Sub

        '/**
        ' * Returns the xrefTable which is a mapping of ObjectKeys
        ' * to byte offsets in the file.
        ' * @return mapping of ObjectsKeys to byte offsets
        ' */
        Public Function getXrefTable() As Map(Of COSObjectKey, Integer)
            Return xrefTable
        End Function

        '/**
        ' * This method set the startxref value of the document. This will only 
        ' * be needed for incremental updates.
        ' * 
        ' * @param startXrefValue the value for startXref
        ' */
        Public Sub setStartXref(ByVal startXrefValue As Integer)
            startXref = startXrefValue
        End Sub

        '/**
        ' * Return the startXref Position of the parsed document. This will only be needed for incremental updates.
        ' * 
        ' * @return a long with the old position of the startxref
        ' */
        Public Function getStartXref() As Integer
            Return startXref
        End Function

        '/**
        ' * Determines it the trailer is a XRef stream or not.
        ' * 
        ' * @return true if the trailer is a XRef stream
        ' */
        Public Function isXRefStream() As Boolean
            If (trailer IsNot Nothing) Then
                Return COSName.XREF.equals(trailer.getItem(COSName.TYPE))
            End If
            Return False
        End Function

    End Class

End Namespace

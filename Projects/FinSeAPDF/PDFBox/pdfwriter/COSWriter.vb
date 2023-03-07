Imports FinSeA.Exceptions
Imports System.IO
Imports FinSeA.Io
Imports FinSeA.Security
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.encryption
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
Imports FinSeA.org.apache.pdfbox.persistence.util
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdfwriter


    '/**
    ' * this class acts on a in-memory representation of a pdf document.
    ' *
    ' * todo no support for incremental updates
    ' * todo single xref section only
    ' * todo no linearization
    ' *
    ' * @author Michael Traut
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class COSWriter
        Implements ICOSVisitor

        ''' <summary>
        ''' The dictionary open token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly DICT_OPEN() As Byte = StringUtil.getBytes("<<")

        ''' <summary>
        ''' The dictionary close token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly DICT_CLOSE() As Byte = StringUtil.getBytes(">>")

        ''' <summary>
        ''' space character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly SPACE() As Byte = StringUtil.getBytes(" ")

        ''' <summary>
        ''' The start to a PDF comment.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly COMMENT() As Byte = StringUtil.getBytes("%")

        ''' <summary>
        ''' The output version of the PDF.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly VERSION() As Byte = StringUtil.getBytes("PDF-1.4")

        ''' <summary>
        ''' Garbage bytes used to create the PDF header.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly GARBAGE() As Byte = {&HF6, &HE4, &HFC, &HDF}

        ''' <summary>
        ''' The EOF constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly EOF() As Byte = StringUtil.getBytes("%%EOF")

        ' pdf tokens

        ''' <summary>
        ''' The reference token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly REFERENCE() As Byte = StringUtil.getBytes("R")

        ''' <summary>
        ''' The XREF token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly XREF() As Byte = StringUtil.getBytes("xref")

        ''' <summary>
        ''' The xref free token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly XREF_FREE() As Byte = StringUtil.getBytes("f")

        ''' <summary>
        ''' The xref used token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly XREF_USED() As Byte = StringUtil.getBytes("n")

        ''' <summary>
        ''' The trailer token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TRAILER() As Byte = StringUtil.getBytes("trailer")

        ''' <summary>
        ''' The start xref token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly STARTXREF() As Byte = StringUtil.getBytes("startxref")

        ''' <summary>
        ''' The starting object token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly OBJ() As Byte = StringUtil.getBytes("obj")

        ''' <summary>
        ''' The end object token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ENDOBJ() As Byte = StringUtil.getBytes("endobj")

        ''' <summary>
        ''' The array open token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ARRAY_OPEN() As Byte = StringUtil.getBytes("[")

        ''' <summary>
        '''  The array close token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ARRAY_CLOSE() As Byte = StringUtil.getBytes("]")

        ''' <summary>
        ''' The open stream token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly STREAM() As Byte = StringUtil.getBytes("stream")

        ''' <summary>
        ''' The close stream token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly ENDSTREAM() As Byte = StringUtil.getBytes("endstream")

        Private formatXrefOffset As NumberFormat = New DecimalFormat("0000000000")

        ''' <summary>
        ''' The decimal format for the xref object generation number data.
        ''' </summary>
        ''' <remarks></remarks>
        Private formatXrefGeneration As New DecimalFormat("00000")

        Private formatDecimal As NumberFormat = NumberFormat.getNumberInstance(Locale.US)

        ' the stream where we create the pdf output
        Private output As OutputStream

        ' the stream used to write standard cos data
        Private standardOutput As COSStandardOutputStream

        ' the start position of the x ref section
        Private _startxref As Long = 0

        ' the current object number
        Private number As Long = 0

        ' maps the object to the keys generated in the writer
        ' these are used for indirect references in other objects
        'A hashtable is used on purpose over a hashmap
        'so that null entries will not get added.
        Private objectKeys As Map(Of COSBase, COSObjectKey) = New Hashtable(Of COSBase, COSObjectKey)
        Private keyObject As Map(Of COSObjectKey, COSBase) = New Hashtable(Of COSObjectKey, COSBase)

        ' the list of x ref entries to be made so far
        Private xRefEntries As List(Of COSWriterXRefEntry) = New ArrayList(Of COSWriterXRefEntry)
        Private objectsToWriteSet As HashSet(Of COSBase) = New HashSet(Of COSBase)

        'A list of objects to write.
        Private objectsToWrite As LinkedList(Of COSBase) = New LinkedList(Of COSBase)

        'a list of objects already written
        Private writtenObjects As [Set](Of COSBase) = New HashSet(Of COSBase)
        'An 'actual' is any COSBase that is not a COSObject.
        'need to keep a list of the actuals that are added
        'as well as the objects because there is a problem
        'when adding a COSObject and then later adding
        'the actual for that object, so we will track
        'actuals separately.
        Private actualsAdded As [Set](Of COSBase) = New HashSet(Of COSBase)

        Private currentObjectKey As COSObjectKey = Nothing

        Private document As PDDocument = Nothing

        Private willEncrypt As Boolean = False

        Private incrementalUpdate As Boolean = False

        Private reachedSignature As Boolean = False

        Private signaturePosition() As Integer = Array.CreateInstance(GetType(Integer), 2)

        Private byterangePosition() As Integer = Array.CreateInstance(GetType(Integer), 2)

        Private [in] As InputStream ' FileInputStream

        '/**
        ' * COSWriter constructor comment.
        ' *
        ' * @param os The wrapped output stream.
        ' */
        Public Sub New(ByVal os As OutputStream)
            MyBase.New()
            Me.setOutput(os)
            Me.setStandardOutput(New COSStandardOutputStream(output))
            Me.formatDecimal.setMaximumFractionDigits(10)
            Me.formatDecimal.setGroupingUsed(False)
        End Sub

        '/**
        ' * COSWriter constructor for incremental updates. 
        ' *
        ' * @param os The wrapped output stream.
        ' * @param is input stream
        ' */
        Public Sub New(ByVal os As OutputStream, ByVal [is] As InputStream) 'FileInputStream 
            Me.New(os)
            [in] = [is]
            incrementalUpdate = True
        End Sub

        Private Sub prepareIncrement(ByVal doc As PDDocument)
            Try
                If (doc IsNot Nothing) Then
                    Dim cosDoc As COSDocument = doc.getDocument()

                    Dim xrefTable As Map(Of COSObjectKey, Nullable(Of Long)) = cosDoc.getXrefTable()
                    Dim keySet As [Set](Of COSObjectKey) = xrefTable.keySet()
                    Dim highestNumber As Long = 0
                    For Each cosObjectKey As COSObjectKey In keySet
                        Dim [object] As COSBase = cosDoc.getObjectFromPool(cosObjectKey).getObject()
                        If ([object] IsNot Nothing AndAlso cosObjectKey IsNot Nothing AndAlso Not (TypeOf ([object]) Is COSNumber)) Then
                            objectKeys.put([object], cosObjectKey)
                            keyObject.put(cosObjectKey, [object])
                        End If

                        Dim num As Long = cosObjectKey.getNumber()
                        If (num > highestNumber) Then
                            highestNumber = num
                        End If
                    Next
                    setNumber(highestNumber)
                    ' xrefTable.clear();

                End If
            Catch e As IOException
                Debug.Print(e.ToString) 'e.printStackTrace()
            End Try
        End Sub

        '/**
        ' * add an entry in the x ref table for later dump.
        ' *
        ' * @param entry The new entry to add.
        ' */
        Protected Sub addXRefEntry(ByVal entry As COSWriterXRefEntry)
            getXRefEntries().add(entry)
        End Sub

        '/**
        ' * This will close the stream.
        ' *
        ' * @throws IOException If the underlying stream throws an exception.
        ' */
        Public Sub close() 'throws IOException
            If (getStandardOutput() IsNot Nothing) Then
                getStandardOutput().Close()
            End If
            If (getOutput() IsNot Nothing) Then
                getOutput().Close()
            End If
        End Sub

        '/**
        ' * This will get the current object number.
        ' *
        ' * @return The current object number.
        ' */
        Protected Function getNumber() As Long
            Return number
        End Function

        '/**
        ' * This will get all available object keys.
        ' *
        ' * @return A map of all object keys.
        ' */
        Public Function getObjectKeys() As Map(Of COSBase, COSObjectKey)
            Return objectKeys
        End Function

        '/**
        ' * This will get the output stream.
        ' *
        ' * @return The output stream.
        ' */
        Protected Function getOutput() As OutputStream
            Return output
        End Function

        '/**
        ' * This will get the standard output stream.
        ' *
        ' * @return The standard output stream.
        ' */
        Protected Function getStandardOutput() As COSStandardOutputStream
            Return standardOutput
        End Function

        '/**
        ' * This will get the current start xref.
        ' *
        ' * @return The current start xref.
        ' */
        Protected Function getStartxref() As Long
            Return _startxref
        End Function

        '/**
        ' * This will get the xref entries.
        ' *
        ' * @return All available xref entries.
        ' */
        Protected Function getXRefEntries() As List(Of COSWriterXRefEntry)
            Return xRefEntries
        End Function

        '/**
        ' * This will set the current object number.
        ' *
        ' * @param newNumber The new object number.
        ' */
        Protected Sub setNumber(ByVal newNumber As Long)
            number = newNumber
        End Sub ' 

        '/**
        ' * This will set the output stream.
        ' *
        ' * @param newOutput The new output stream.
        ' */
        Private Sub setOutput(ByVal newOutput As OutputStream)
            output = newOutput
        End Sub

        '/**
        ' * This will set the standard output stream.
        ' *
        ' * @param newStandardOutput The new standard output stream.
        ' */
        Private Sub setStandardOutput(ByVal newStandardOutput As COSStandardOutputStream)
            standardOutput = newStandardOutput
        End Sub

        '/**
        ' * This will set the start xref.
        ' *
        ' * @param newStartxref The new start xref attribute.
        ' */
        Protected Sub setStartxref(ByVal newStartxref As Long)
            _startxref = newStartxref
        End Sub

        '/**
        ' * This will write the body of the document.
        ' *
        ' * @param doc The document to write the body for.
        ' *
        ' * @throws IOException If there is an error writing the data.
        ' * @throws COSVisitorException If there is an error generating the data.
        ' */
        Protected Sub doWriteBody(ByVal doc As COSDocument) ' throws IOException, COSVisitorException
            Dim trailer As COSDictionary = doc.getTrailer()
            Dim root As COSDictionary = trailer.getDictionaryObject(COSName.ROOT)
            Dim info As COSDictionary = trailer.getDictionaryObject(COSName.INFO)
            Dim encrypt As COSDictionary = trailer.getDictionaryObject(COSName.ENCRYPT)
            If (root IsNot Nothing) Then
                addObjectToWrite(root)
            End If
            If (info IsNot Nothing) Then
                addObjectToWrite(info)
            End If

            While (objectsToWrite.size() > 0)
                Dim nextObject As COSBase = objectsToWrite.RemoveFirst()
                objectsToWriteSet.Remove(nextObject)
                doWriteObject(nextObject)
            End While

            willEncrypt = False

            If (encrypt IsNot Nothing) Then
                addObjectToWrite(encrypt)
            End If
            While (objectsToWrite.size() > 0)
                Dim nextObject As COSBase = objectsToWrite.RemoveFirst()
                objectsToWriteSet.Remove(nextObject)
                doWriteObject(nextObject)
            End While
        End Sub

        Private Sub addObjectToWrite(ByVal [object] As COSBase)
            Dim actual As COSBase = [object]
            If (TypeOf (actual) Is COSObject) Then
                actual = DirectCast(actual, COSObject).getObject()
            End If

            If (Not writtenObjects.contains([object]) AndAlso _
                Not objectsToWriteSet.Contains([object]) AndAlso _
                Not actualsAdded.contains(actual)) Then
                Dim cosBase As COSBase = Nothing
                Dim cosObjectKey As COSObjectKey = Nothing
                If (actual IsNot Nothing) Then
                    cosObjectKey = objectKeys.get(actual)
                End If
                If (cosObjectKey IsNot Nothing) Then
                    cosBase = keyObject.get(cosObjectKey)
                End If
                If (actual IsNot Nothing AndAlso objectKeys.containsKey(actual) AndAlso _
                        Not [object].isNeedToBeUpdate() AndAlso (cosBase IsNot Nothing AndAlso _
                        Not cosBase.isNeedToBeUpdate())) Then
                    Return
                End If

                objectsToWrite.add([object])
                objectsToWriteSet.Add([object])
                If (actual IsNot Nothing) Then
                    actualsAdded.add(actual)
                End If
            End If
        End Sub

        '/**
        ' * This will write a COS object.
        ' *
        ' * @param obj The object to write.
        ' *
        ' * @throws COSVisitorException If there is an error visiting objects.
        ' */
        Public Sub doWriteObject(ByVal obj As COSBase) 'throws COSVisitorException
            Try
                writtenObjects.add(obj)
                If (TypeOf (obj) Is COSDictionary) Then
                    Dim dict As COSDictionary = obj
                    Dim item As COSName = dict.getItem(COSName.TYPE)
                    If (COSName.SIG.equals(item) OrElse COSName.DOC_TIME_STAMP.equals(item)) Then
                        reachedSignature = True
                    End If
                End If

                ' find the physical reference
                currentObjectKey = getObjectKey(obj)
                ' add a x ref entry
                addXRefEntry(New COSWriterXRefEntry(getStandardOutput().getPos(), obj, currentObjectKey))
                ' write the object
                getStandardOutput().write(Sistema.Strings.GetBytes(CStr(currentObjectKey.getNumber()), "ISO-8859-1"))
                getStandardOutput().write(SPACE)
                getStandardOutput().write(Sistema.Strings.GetBytes(CStr(currentObjectKey.getGeneration()), "ISO-8859-1"))
                getStandardOutput().write(SPACE)
                getStandardOutput().write(COSWriter.OBJ)
                getStandardOutput().writeEOL()
                obj.accept(Me)
                getStandardOutput().writeEOL()
                getStandardOutput().write(ENDOBJ)
                getStandardOutput().writeEOL()
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Sub

        '/**
        ' * This will write the header to the PDF document.
        ' *
        ' * @param doc The document to get the data from.
        ' *
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Protected Sub doWriteHeader(ByVal doc As COSDocument) ' throws IOException
            getStandardOutput().write(Sistema.Strings.GetBytes(doc.getHeaderString(), "ISO-8859-1"))
            getStandardOutput().writeEOL()
            getStandardOutput().write(COMMENT)
            getStandardOutput().write(GARBAGE)
            getStandardOutput().writeEOL()
        End Sub


        '/**
        ' * This will write the trailer to the PDF document.
        ' *
        ' * @param doc The document to create the trailer for.
        ' *
        ' * @throws IOException If there is an IOError while writing the document.
        ' * @throws COSVisitorException If there is an error while generating the data.
        ' */
        Protected Sub doWriteTrailer(ByVal doc As COSDocument) 'throws IOException, COSVisitorException
            getStandardOutput().write(COSWriter.TRAILER)
            getStandardOutput().writeEOL()

            Dim trailer As COSDictionary = doc.getTrailer()
            'sort xref, needed only if object keys not regenerated
            Collections.sort(Of COSWriterXRefEntry)(getXRefEntries())
            Dim lastEntry As COSWriterXRefEntry = getXRefEntries().get(getXRefEntries().size() - 1)
            trailer.setInt(COSName.SIZE, CInt(lastEntry.getKey().getNumber()) + 1)
            ' Only need to stay, if an incremental update will be performed
            If (Not incrementalUpdate) Then
                trailer.removeItem(COSName.PREV)
            End If
            ' Remove a checksum if present
            trailer.removeItem(COSName.DOC_CHECKSUM)

            trailer.accept(Me)
        End Sub

        '/**
        ' * write the x ref section for the pdf file
        ' *
        ' * currently, the pdf is reconstructed from the scratch, so we write a single section
        ' *
        ' * todo support for incremental writing?
        ' *
        ' * @param doc The document to write the xref from.
        ' *
        ' * @throws IOException If there is an error writing the data to the stream.
        ' */
        Protected Sub doWriteXRef(ByVal doc As COSDocument) 'throws IOException
            If (doc.isXRefStream()) Then
                ' sort xref, needed only if object keys not regenerated
                Collections.sort(Of COSWriterXRefEntry)(getXRefEntries())
                Dim lastEntry As COSWriterXRefEntry = getXRefEntries().get(getXRefEntries().size() - 1)

                ' remember the position where x ref is written
                setStartxref(getStandardOutput().getPos())

                getStandardOutput().write(XREF)
                getStandardOutput().writeEOL()
                ' write start object number and object count for this x ref section
                ' we assume starting from scratch
                writeXrefRange(0, lastEntry.getKey().getNumber() + 1)
                ' write initial start object with ref to first deleted object and magic generation number
                writeXrefEntry(COSWriterXRefEntry.getNullEntry())
                ' write entry for every object
                Dim lastObjectNumber As Long = 0
                'for (Iterator<COSWriterXRefEntry> i = getXRefEntries().iterator(); i.hasNext();)
                For Each entry As COSWriterXRefEntry In getXRefEntries()
                    'Dim entry As COSWriterXRefEntry = i.next()
                    While (lastObjectNumber < entry.getKey().getNumber() - 1)
                        writeXrefEntry(COSWriterXRefEntry.getNullEntry())
                    End While
                    lastObjectNumber = entry.getKey().getNumber()
                    writeXrefEntry(entry)
                Next
            Else
                Dim trailer As COSDictionary = doc.getTrailer()
                trailer.setLong(COSName.PREV, doc.getStartXref())
                addXRefEntry(COSWriterXRefEntry.getNullEntry())

                ' sort xref, needed only if object keys not regenerated
                Collections.sort(Of COSWriterXRefEntry)(getXRefEntries())

                ' remember the position where x ref was written
                setStartxref(getStandardOutput().getPos())

                getStandardOutput().write(XREF)
                getStandardOutput().writeEOL()
                ' write start object number and object count for this x ref section
                ' we assume starting from scratch

                Dim xRefRanges() As NInteger = getXRefRanges(getXRefEntries())
                Dim xRefLength As Integer = xRefRanges.Length
                Dim x As Integer = 0
                Dim j As Integer = 0
                While (x < xRefLength AndAlso (xRefLength Mod 2) = 0)
                    writeXrefRange(xRefRanges(x), xRefRanges(x + 1))

                    For i As Integer = 0 To xRefRanges(x + 1) - 1
                        writeXrefEntry(xRefEntries.get(j))
                        j += 1
                    Next
                    x += 2
                End While
            End If
        End Sub

        Private Sub doWriteXRefInc(ByVal doc As COSDocument, ByVal hybridPrev As Long) 'throws IOException, COSVisitorException
            If (doc.isXRefStream() OrElse hybridPrev <> -1) Then
                ' the file uses XrefStreams, so we need to update
                ' it with an xref stream. We create a new one and fill it
                ' with data available here
                ' first set an entry for the null entry in the xref table
                ' this is probably not necessary
                ' addXRefEntry(COSWriterXRefEntry.getNullEntry());

                ' create a new XRefStrema object
                Dim pdfxRefStream As New PDFXRefStream() 'PDFXRefStream

                ' add all entries from the incremental update.
                Dim xRefEntries2 As List(Of COSWriterXRefEntry) = getXRefEntries()
                For Each cosWriterXRefEntry As COSWriterXRefEntry In xRefEntries2
                    pdfxRefStream.addEntry(cosWriterXRefEntry)
                Next

                Dim trailer As COSDictionary = doc.getTrailer()
                'trailer.setLong(COSName.PREV, hybridPrev == -1 ? prev : hybridPrev);
                trailer.setLong(COSName.PREV, doc.getStartXref())

                pdfxRefStream.addTrailerInfo(trailer)
                ' the size is the highest object number+1. we add one more
                ' for the xref stream object we are going to write
                pdfxRefStream.setSize(getNumber() + 2)

                setStartxref(getStandardOutput().getPos())
                Dim stream2 As COSStream = pdfxRefStream.getStream()
                doWriteObject(stream2)
            End If

            If (Not doc.isXRefStream() OrElse hybridPrev <> -1) Then
                Dim trailer As COSDictionary = doc.getTrailer()
                trailer.setLong(COSName.PREV, doc.getStartXref())
                If (hybridPrev <> -1) Then
                    Dim xrefStm As COSName = COSName.XREF_STM
                    trailer.removeItem(xrefStm)
                    trailer.setLong(xrefStm, getStartxref())
                End If
                addXRefEntry(COSWriterXRefEntry.getNullEntry())

                ' sort xref, needed only if object keys not regenerated
                Collections.sort(Of COSWriterXRefEntry)(getXRefEntries())

                ' remember the position where x ref was written
                setStartxref(getStandardOutput().getPos())

                getStandardOutput().write(XREF)
                getStandardOutput().writeEOL()
                ' write start object number and object count for this x ref section
                ' we assume starting from scratch

                Dim xRefRanges() As NInteger = getXRefRanges(getXRefEntries())
                Dim xRefLength As Integer = xRefRanges.Length
                Dim x As Integer = 0
                Dim j As Integer = 0
                While (x < xRefLength AndAlso (xRefLength Mod 2) = 0)
                    writeXrefRange(xRefRanges(x), xRefRanges(x + 1))

                    For i As Integer = 0 To xRefRanges(x + 1) - 1
                        writeXrefEntry(xRefEntries.get(j))
                        j += 1
                    Next
                    x += 2
                End While
            End If
        End Sub

        Private Sub doWriteSignature(ByVal doc As COSDocument) ' throws IOException, SignatureException
            ' need to calculate the ByteRange
            If (signaturePosition(0) > 0 AndAlso byterangePosition(1) > 0) Then
                Dim left As Integer = CInt(getStandardOutput().getPos()) - signaturePosition(1)
                Dim newByteRange As String = "0 " & signaturePosition(0) & " " & signaturePosition(1) & " " & left & "]"
                Dim leftByterange As Integer = byterangePosition(1) - byterangePosition(0) - newByteRange.Length()
                If (leftByterange < 0) Then
                    Throw New IOException("Can't write new ByteRange, not enough space")
                End If
                getStandardOutput().setPos(byterangePosition(0))
                getStandardOutput().write(Sistema.Strings.GetBytes(newByteRange))
                For i As Integer = 0 To leftByterange - 1
                    getStandardOutput().write(&H20)
                Next

                getStandardOutput().setPos(0)
                ' Begin - extracting document
                Dim filterInputStream As New COSFilterInputStream([in], New Integer() {0, signaturePosition(0), signaturePosition(1), left})
                Dim bytes As New ByteArrayOutputStream()
                Try
                    Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
                    Dim c As Integer
                    c = filterInputStream.Read(buffer)
                    While (c <> -1)
                        bytes.write(buffer, 0, c)
                        c = filterInputStream.Read(buffer)
                    End While
                Finally
                    If (filterInputStream IsNot Nothing) Then
                        filterInputStream.Close()
                    End If
                End Try

                Dim pdfContent() As Byte = bytes.toByteArray()
                ' End - extracting document

                Dim signatureInterface As SignatureInterface = doc.getSignatureInterface()
                Dim sign() As Byte = signatureInterface.sign(New ByteArrayInputStream(pdfContent))
                Dim signature As String = New COSString(sign).getHexString()
                Dim leftSignaturerange As Integer = signaturePosition(1) - signaturePosition(0) - signature.Length()
                If (leftSignaturerange < 0) Then
                    Throw New IOException("Can't write signature, not enough space")
                End If
                getStandardOutput().setPos(signaturePosition(0) + 1)
                getStandardOutput().write(Sistema.Strings.GetBytes(signature))
            End If
        End Sub

        Private Sub writeXrefRange(ByVal x As Long, ByVal y As Long) 'throws IOException
            getStandardOutput().write(Sistema.Strings.GetBytes(CStr(x)))
            getStandardOutput().write(SPACE)
            getStandardOutput().write(Sistema.Strings.GetBytes(CStr(y)))
            getStandardOutput().writeEOL()
        End Sub

        Private Sub writeXrefEntry(ByVal entry As COSWriterXRefEntry) 'throws IOException
            Dim offset As String = formatXrefOffset.format(entry.getOffset())
            Dim generation As String = formatXrefGeneration.format(entry.getKey().getGeneration())
            getStandardOutput().write(Sistema.Strings.GetBytes(offset, "ISO-8859-1"))
            getStandardOutput().write(SPACE)
            getStandardOutput().write(Sistema.Strings.GetBytes(generation, "ISO-8859-1"))
            getStandardOutput().write(SPACE)
            getStandardOutput().write(IIf(entry.isFree(), XREF_FREE, XREF_USED))
            getStandardOutput().writeCRLF()
        End Sub

        '/**
        ' * check the xref entries and write out the ranges.  The format of the
        ' * returned array is exactly the same as the pdf specification.  See section
        ' * 7.5.4 of ISO32000-1:2008, example 1 (page 40) for reference.
        ' * <p>
        ' * example: 0 1 2 5 6 7 8 10
        ' * <p>
        ' * will create a array with follow ranges
        ' * <p>
        ' * 0 3 5 4 10 1
        ' * <p>
        ' * this mean that the element 0 is followed by two other related numbers 
        ' * that represent a cluster of the size 3. 5 is follow by three other
        ' * related numbers and create a cluster of size 4. etc.
        ' * 
        ' * @param xRefEntriesList list with the xRef entries that was written
        ' * @return a integer array with the ranges
        ' */
        Protected Function getXRefRanges(ByVal xRefEntriesList As List(Of COSWriterXRefEntry)) As NInteger()
            Dim nr As Integer = 0
            Dim last As Integer = -2
            Dim count As Integer = 1

            Dim list As New ArrayList(Of NInteger)
            For Each [object] As Object In xRefEntriesList
                nr = DirectCast([object], COSWriterXRefEntry).getKey().getNumber()
                If (nr = last + 1) Then
                    count += 1
                    last = nr
                ElseIf (last = -2) Then
                    last = nr
                Else
                    list.add(last - count + 1)
                    list.add(count)
                    last = nr
                    count = 1
                End If
            Next
            ' If no new entry is found, we need to write out the last result
            If (xRefEntriesList.size() > 0) Then
                list.add(last - count + 1)
                list.add(count)
            End If
            Return list.toArray(Of NInteger)() 'new Integer[list.size()])
        End Function

        '/**
        ' * This will get the object key for the object.
        ' *
        ' * @param obj The object to get the key for.
        ' *
        ' * @return The object key for the object.
        ' */
        Private Function getObjectKey(ByVal obj As COSBase) As COSObjectKey
            Dim actual As COSBase = obj
            If (TypeOf (actual) Is COSObject) Then
                actual = DirectCast(obj, COSObject).getObject()
            End If
            Dim key As COSObjectKey = Nothing
            If (actual IsNot Nothing) Then
                key = objectKeys.get(actual)
            End If
            If (key Is Nothing) Then
                key = objectKeys.get(obj)
            End If
            If (key Is Nothing) Then
                setNumber(getNumber() + 1)
                key = New COSObjectKey(getNumber(), 0)
                objectKeys.put(obj, key)
                If (actual IsNot Nothing) Then
                    objectKeys.put(actual, key)
                End If
            End If
            Return key
        End Function

        '/**
        ' * visitFromArray method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromArray(ByVal obj As COSArray) As Object Implements ICOSVisitor.visitFromArray
            Try
                Dim count As Integer = 0
                getStandardOutput().write(ARRAY_OPEN)
                'for (Iterator(Of COSBase) i = obj.iterator(); i.hasNext();)
                Dim i As Global.System.Collections.Generic.IEnumerator(Of COSBase) = obj.iterator
                i.MoveNext()
                While (i.Current IsNot Nothing) 'For Each current As COSBase In obj
                    Dim current As COSBase = i.Current
                    'i.next();
                    If (TypeOf (current) Is COSDictionary) Then
                        addObjectToWrite(current)
                        writeReference(current)
                    ElseIf (TypeOf (current) Is COSObject) Then
                        Dim subValue As COSBase = DirectCast(current, COSObject).getObject()
                        If (TypeOf (subValue) Is COSDictionary OrElse subValue Is Nothing) Then
                            addObjectToWrite(current)
                            writeReference(current)
                        Else
                            subValue.accept(Me)
                        End If
                    ElseIf (current Is Nothing) Then
                        COSNull.NULL.accept(Me)

                    ElseIf (TypeOf (current) Is COSString) Then
                        Dim copy As New COSString(True)
                        copy.append(DirectCast(current, COSString).getBytes())
                        copy.accept(Me)
                    Else
                        current.accept(Me)
                    End If
                    count += 1
                    If (i.MoveNext()) Then
                        If (count Mod 10 = 0) Then
                            getStandardOutput().writeEOL()
                        Else
                            getStandardOutput().write(SPACE)
                        End If
                    End If
                End While
                getStandardOutput().write(ARRAY_CLOSE)
                getStandardOutput().writeEOL()
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromBoolean method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromBoolean(ByVal obj As COSBoolean) As Object Implements ICOSVisitor.visitFromBoolean

            Try
                obj.writePDF(getStandardOutput())
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromDictionary method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromDictionary(ByVal obj As COSDictionary) As Object Implements ICOSVisitor.visitFromDictionary
            Try
                getStandardOutput().write(DICT_OPEN)
                getStandardOutput().writeEOL()
                'for (Map.Entry(Of COSName, COSBase) entry : obj.entrySet())
                For Each entry As Map.Entry(Of COSName, COSBase) In obj.entrySet
                    Dim value As COSBase = entry.Value
                    If (value IsNot Nothing) Then
                        entry.Key.accept(Me)
                        getStandardOutput().write(SPACE)
                        If (TypeOf (value) Is COSDictionary) Then
                            Dim dict As COSDictionary = value

                            ' write all XObjects as direct objects, this will save some size
                            Dim item As COSBase = dict.getItem(COSName.XOBJECT)
                            If (item IsNot Nothing) Then
                                item.setDirect(True)
                            End If
                            item = dict.getItem(COSName.RESOURCES)
                            If (item IsNot Nothing) Then
                                item.setDirect(True)
                            End If

                            If (dict.isDirect()) Then
                                ' If the object should be written direct, we need
                                ' to pass the dictionary to the visitor again.
                                visitFromDictionary(dict)
                            Else
                                addObjectToWrite(dict)
                                writeReference(dict)
                            End If
                        ElseIf (TypeOf (value) Is COSObject) Then
                            Dim subValue As COSBase = DirectCast(value, COSObject).getObject()
                            If (TypeOf (subValue) Is COSDictionary OrElse subValue Is Nothing) Then
                                addObjectToWrite(value)
                                writeReference(value)
                            Else
                                If (subValue.isDirect()) Then
                                    subValue.accept(Me)
                                Else
                                    addObjectToWrite(subValue)
                                    writeReference(subValue)
                                End If
                            End If
                        Else
                            ' If we reach the pdf signature, we need to determinate the position of the
                            ' content and byterange
                            If (reachedSignature AndAlso COSName.CONTENTS.equals(entry.Key)) Then
                                signaturePosition = Array.CreateInstance(GetType(Integer), 2)
                                signaturePosition(0) = getStandardOutput().getPos()
                                value.accept(Me)
                                signaturePosition(1) = getStandardOutput().getPos()
                            ElseIf (reachedSignature AndAlso COSName.BYTERANGE.equals(entry.Key)) Then
                                byterangePosition = Array.CreateInstance(GetType(Integer), 2)
                                byterangePosition(0) = getStandardOutput().getPos() + 1
                                value.accept(Me)
                                byterangePosition(1) = getStandardOutput().getPos() - 1
                                reachedSignature = False
                            Else
                                value.accept(Me)
                            End If
                        End If
                        getStandardOutput().writeEOL()
                    Else
                        'then we won't write anything, there are a couple cases
                        'were the value of an entry in the COSDictionary will
                        'be a dangling reference that points to nothing
                        'so we will just not write out the entry if that is the case
                    End If
                Next
                getStandardOutput().write(DICT_CLOSE)
                getStandardOutput().writeEOL()
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * The visit from document method.
        ' *
        ' * @param doc The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromDocument(ByVal doc As COSDocument) As Object Implements ICOSVisitor.visitFromDocument
            Try
                If (Not incrementalUpdate) Then
                    doWriteHeader(doc)
                End If
                doWriteBody(doc)

                ' get the previous trailer
                Dim trailer As COSDictionary = doc.getTrailer()
                Dim hybridPrev As Long = -1

                If (trailer IsNot Nothing) Then
                    hybridPrev = trailer.getLong(COSName.XREF_STM)
                End If

                If (incrementalUpdate) Then
                    doWriteXRefInc(doc, hybridPrev)
                Else
                    doWriteXRef(doc)
                End If

                ' the trailer section should only be used for xref tables not for xref streams
                If (Not incrementalUpdate OrElse Not doc.isXRefStream() OrElse hybridPrev <> -1) Then
                    doWriteTrailer(doc)
                End If

                ' write endof
                getStandardOutput().write(STARTXREF)
                getStandardOutput().writeEOL()
                getStandardOutput().write(Sistema.Strings.GetBytes(CStr(getStartxref()), "ISO-8859-1"))
                getStandardOutput().writeEOL()
                getStandardOutput().write(EOF)
                getStandardOutput().writeEOL()

                If (incrementalUpdate) Then
                    doWriteSignature(doc)
                End If

                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            Catch e As SignatureException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromFloat method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromFloat(ByVal obj As COSFloat) As Object Implements ICOSVisitor.visitFromFloat

            Try
                obj.writePDF(getStandardOutput())
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromFloat method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromInt(ByVal obj As COSInteger) As Object Implements ICOSVisitor.visitFromInt
            Try
                obj.writePDF(getStandardOutput())
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromName method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromName(ByVal obj As COSName) As Object Implements ICOSVisitor.visitFromName
            Try
                obj.writePDF(getStandardOutput())
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromNull method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromNull(ByVal obj As COSNull) As Object Implements ICOSVisitor.visitFromNull
            Try
                obj.writePDF(getStandardOutput())
                Return Nothing
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Function

        '/**
        ' * visitFromObjRef method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' */
        Public Sub writeReference(ByVal obj As COSBase) ' throws COSVisitorException
            Try
                Dim key As COSObjectKey = getObjectKey(obj)
                getStandardOutput().write(Sistema.Strings.GetBytes(CStr(key.getNumber()), "ISO-8859-1"))
                getStandardOutput().write(SPACE)
                getStandardOutput().write(Sistema.Strings.GetBytes(CStr(key.getGeneration()), "ISO-8859-1"))
                getStandardOutput().write(SPACE)
                getStandardOutput().write(REFERENCE)
            Catch e As IOException
                Throw New COSVisitorException(e)
            End Try
        End Sub

        '/**
        ' * visitFromStream method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' *
        ' * @return null
        ' */
        Public Function visitFromStream(ByVal obj As COSStream) As Object Implements ICOSVisitor.visitFromStream
            Dim input As InputStream = Nothing
            Try
                If (willEncrypt) Then
                    document.getSecurityHandler().encryptStream(obj, currentObjectKey.getNumber(), currentObjectKey.getGeneration())
                End If

                Dim lengthObject As COSObject = Nothing
                ' check if the length object is required to be direct, like in
                ' a cross reference stream dictionary
                Dim lengthEntry As COSBase = obj.getDictionaryObject(COSName.LENGTH)
                Dim type As String = obj.getNameAsString(COSName.TYPE)
                If (lengthEntry IsNot Nothing AndAlso lengthEntry.isDirect() OrElse "XRef".Equals(type)) Then
                    ' the length might be the non encoded length,
                    ' set the real one as direct object
                    Dim cosInteger As COSInteger = cosInteger.get(obj.getFilteredLength())
                    cosInteger.setDirect(True)
                    obj.setItem(COSName.LENGTH, cosInteger)

                Else
                    ' make the length an implicit indirect object
                    ' set the length of the stream and write stream dictionary
                    lengthObject = New COSObject(Nothing)

                    obj.setItem(COSName.LENGTH, lengthObject)
                End If
                input = obj.getFilteredStream()
                'obj.accept(this);
                ' write the stream content
                visitFromDictionary(obj)
                getStandardOutput().write(STREAM)
                getStandardOutput().writeCRLF()
                Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
                Dim amountRead As Integer = 0
                Dim totalAmountWritten As Integer = 0
                amountRead = input.read(buffer, 0, 1024)
                While (amountRead > 0)
                    getStandardOutput().write(buffer, 0, amountRead)
                    totalAmountWritten += amountRead
                    amountRead = input.read(buffer, 0, 1024)
                End While
                ' set the length as an indirect object
                If (lengthObject IsNot Nothing) Then
                    lengthObject.setObject(COSInteger.get(totalAmountWritten))
                End If
                getStandardOutput().writeCRLF()
                getStandardOutput().write(ENDSTREAM)
                getStandardOutput().writeEOL()
                Return Nothing
            Catch e As Exception
                Throw New COSVisitorException(e)
            Finally
                If (input IsNot Nothing) Then
                    Try
                        input.Close()
                    Catch e As IOException
                        Throw New COSVisitorException(e)
                    End Try
                End If
            End Try
        End Function

        '/**
        ' * visitFromString method comment.
        ' *
        ' * @param obj The object that is being visited.
        ' *
        ' * @return null
        ' *
        ' * @throws COSVisitorException If there is an exception while visiting this object.
        ' */
        Public Function visitFromString(ByVal obj As COSString) As Object Implements ICOSVisitor.visitFromString
            Try
                If (willEncrypt) Then
                    document.getSecurityHandler().decryptString(obj, currentObjectKey.getNumber(), currentObjectKey.getGeneration())
                End If

                obj.writePDF(getStandardOutput())
            Catch e As Exception
                Throw New COSVisitorException(e)
            End Try
            Return Nothing
        End Function

        '/**
        ' * This will write the pdf document.
        ' *
        ' * @param doc The document to write.
        ' *
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub write(ByVal doc As COSDocument)  'throws COSVisitorException
            Dim pdDoc As PDDocument = New PDDocument(doc)
            write(pdDoc)
        End Sub

        '/**
        ' * This will write the pdf document.
        ' *
        ' * @param doc The document to write.
        ' *
        ' * @throws COSVisitorException If an error occurs while generating the data.
        ' */
        Public Sub write(ByVal doc As PDDocument)  'throws COSVisitorException
            Dim idTime As Nullable(Of Long)
            Dim cosDoc As COSDocument
            Dim trailer As COSDictionary

            If (doc.getDocumentId().hasValue = False) Then
                idTime = (New Date()).Ticks
            Else
                idTime = doc.getDocumentId()
            End If

            document = doc
            If (incrementalUpdate) Then
                prepareIncrement(doc)
            End If

            ' if the document says we should remove encryption, then we shouldn't encrypt
            If (doc.isAllSecurityToBeRemoved()) Then
                Me.willEncrypt = False
                ' also need to get rid of the "Encrypt" in the trailer so readers 
                ' don't try to decrypt a document which is not encrypted
                cosDoc = doc.getDocument()
                trailer = cosDoc.getTrailer()
                trailer.removeItem(COSName.ENCRYPT)
            Else
                Dim securityHandler As SecurityHandler = document.getSecurityHandler()
                If (securityHandler IsNot Nothing) Then
                    Try
                        securityHandler.prepareDocumentForEncryption(document)
                        Me.willEncrypt = True
                    Catch e As IOException
                        Throw New COSVisitorException(e)
                    Catch e As CryptographyException
                        Throw New COSVisitorException(e)
                    End Try
                Else
                    Me.willEncrypt = False
                End If
            End If

            cosDoc = document.getDocument()
            trailer = cosDoc.getTrailer()
            Dim idArray As COSArray = trailer.getDictionaryObject(COSName.ID)
            If (idArray Is Nothing OrElse incrementalUpdate) Then
                Try
                    'algorithm says to use time/path/size/values in doc to generate
                    'the id.  We don't have path or size, so do the best we can
                    Dim md As MessageDigest = MessageDigest.getInstance("MD5")
                    md.update(Sistema.Strings.GetBytes(CStr(idTime), "ISO-8859-1"))
                    Dim info As COSDictionary = trailer.getDictionaryObject(COSName.INFO)
                    If (info IsNot Nothing) Then
                        Dim values As Global.System.Collections.Generic.IEnumerator(Of COSBase) = info.getValues().iterator()
                        While (values.MoveNext)
                            md.update(Sistema.Strings.GetBytes(values.Current.ToString(), "ISO-8859-1"))
                        End While
                    End If
                    idArray = New COSArray()
                    Dim id As COSString = New COSString(md.digest())
                    idArray.add(id)
                    idArray.add(id)
                    trailer.setItem(COSName.ID, idArray)
                Catch e As NoSuchAlgorithmException
                    Throw New COSVisitorException(e)
                Catch e As UnsupportedEncodingException
                    Throw New COSVisitorException(e)
                End Try
            End If
            cosDoc.accept(Me)
        End Sub

    End Class

End Namespace
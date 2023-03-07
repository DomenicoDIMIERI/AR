Imports FinSeA.Io
Imports System.IO
Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.persistence.util
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.io


Namespace org.apache.pdfbox.pdfparser


    '/**
    ' * 
    ' * @author <a href="adam@apache.org">Adam Nichols</a>
    ' */
    Public Class ConformingPDFParser
        Inherits BaseParser
        Protected inputFile As RandomAccess
        Dim xrefEntries As List(Of XrefEntry)
        Private currentOffset As Long
        Private doc As ConformingPDDocument = Nothing
        Private throwNonConformingException As Boolean = True
        Private recursivlyRead As Boolean = True

        '/**
        ' * Constructor.
        ' *
        ' * @param input The input stream that contains the PDF document.
        ' *
        ' * @throws IOException If there is an error initializing the stream.
        ' */
        Public Sub New(ByVal inputFile As FinSeA.Io.File) ' throws IOException {
            Me.inputFile = New FinSeA.Io.RandomAccessFile(inputFile, "r")
        End Sub

        '/**
        ' * This will parse the stream and populate the COSDocument object.  This will close
        ' * the stream when it is done parsing.
        ' *
        ' * @throws IOException If there is an error reading from the stream or corrupt data
        ' * is found.
        ' */
        Public Sub parse() 'throws IOException {
            document = New COSDocument()
            doc = New ConformingPDDocument(document)
            currentOffset = inputFile.length() - 1
            Dim xRefTableLocation As Long = parseTrailerInformation()
            currentOffset = xRefTableLocation
            parseXrefTable()
            ' now that we read the xref table and put null references in the doc,
            ' we can deference those objects now.
            Dim oldValue As Boolean = recursivlyRead
            recursivlyRead = False
            Dim keys As List(Of COSObjectKey) = doc.getObjectKeysFromPool()
            For Each key As COSObjectKey In keys
                ' getObject will put it into the document's object pool for us
                getObject(key.getNumber(), key.getGeneration())
            Next
            recursivlyRead = oldValue
        End Sub

        '/**
        ' * This will get the document that was parsed.  parse() must be called before this is called.
        ' * When you are done with this document you must call close() on it to release
        ' * resources.
        ' *
        ' * @return The document that was parsed.
        ' *
        ' * @throws IOException If there is an error getting the document.
        ' */
        Public Function getDocument() As COSDocument ' throws IOException {
            If (document Is Nothing) Then
                Throw New IOException("You must call parse() before calling getDocument()")
            End If
            Return document
        End Function

        '/**
        ' * This will get the PD document that was parsed.  When you are done with
        ' * this document you must call close() on it to release resources.
        ' *
        ' * @return The document at the PD layer.
        ' *
        ' * @throws IOException If there is an error getting the document.
        ' */
        Public Function getPDDocument() As PDDocument ' throws IOException {
            Return doc
        End Function

        Private Function parseXrefTable() As Boolean ' throws IOException {
            Dim currentLine As String = readLine()
            If (throwNonConformingException) Then
                If (Not "xref".Equals(currentLine)) Then
                    Throw New Exception("xref table not found.\nExpected: xref\nFound: " & currentLine)
                End If
            End If

            Dim objectNumber As Integer = readInt()
            Dim entries As Integer = readInt()
            xrefEntries = New ArrayList(Of XrefEntry)(entries)
            For i As Integer = 0 To entries - 1
                xrefEntries.add(New XrefEntry(objectNumber, readInt(), readInt(), readLine()))
                objectNumber += 1
            Next

            Return True
        End Function

        Protected Function parseTrailerInformation() As Long ' throws IOException, NumberFormatException {
            Dim xrefLocation As Long = -1
            consumeWhitespaceBackwards()
            Dim currentLine As String = readLineBackwards()
            If (throwNonConformingException) Then
                If (Not "%%EOF".Equals(currentLine)) Then
                    Throw New Exception("Invalid EOF marker.\nExpected: %%EOF\nFound: " & currentLine)
                End If
            End If
            xrefLocation = readLongBackwards()
            currentLine = readLineBackwards()
            If (throwNonConformingException) Then
                If (Not "startxref".Equals(currentLine)) Then
                    Throw New Exception("Invalid trailer.\nExpected: startxref\nFound: " & currentLine)
                End If
            End If

            document.setTrailer(readDictionaryBackwards())
            consumeWhitespaceBackwards()
            currentLine = readLineBackwards()
            If (throwNonConformingException) Then
                If (Not "trailer".Equals(currentLine)) Then
                    Throw New Exception("Invalid trailer.\nExpected: trailer\nFound: " & currentLine)
                End If
            End If

            Return xrefLocation
        End Function

        Protected Function readByteBackwards() As Byte ' throws IOException {
            inputFile.seek(currentOffset)
            Dim singleByte As Byte = inputFile.read()
            currentOffset -= 1
            Return singleByte
        End Function

        Protected Function readByte() As Byte ' throws IOException {
            inputFile.seek(currentOffset)
            Dim singleByte As Byte = inputFile.read()
            currentOffset += 1
            Return singleByte
        End Function

        Protected Function readBackwardUntilWhitespace() As String ' throws IOException {
            Dim sb As New StringBuilder()
            Dim singleByte As Byte = readByteBackwards()
            While (Not isWhitespace(singleByte))
                sb.Insert(0, Convert.ToChar(singleByte))
                singleByte = readByteBackwards()
            End While
            Return sb.ToString()
        End Function

        '/**
        ' * This will read all bytes (backwards) until a non-whitespace character is
        ' * found.  To save you an extra read, the non-whitespace character is
        ' * returned.  If the current character is not whitespace, this method will
        ' * just return the current char.
        ' * @return the first non-whitespace character found
        ' * @throws IOException if there is an error reading from the file
        ' */
        Protected Function consumeWhitespaceBackwards() As Byte 'throws IOException {
            inputFile.seek(currentOffset)
            Dim singleByte As Byte = inputFile.read()
            If (Not isWhitespace(singleByte)) Then Return singleByte

            ' we have some whitespace, let's consume it
            While (isWhitespace(singleByte))
                singleByte = readByteBackwards()
            End While
            ' readByteBackwards will decrement the currentOffset to point the byte
            ' before the one just read, so we increment it back to the current byte
            currentOffset += 1
            Return singleByte
        End Function

        '/**
        ' * This will read all bytes until a non-whitespace character is
        ' * found.  To save you an extra read, the non-whitespace character is
        ' * returned.  If the current character is not whitespace, this method will
        ' * just return the current char.
        ' * @return the first non-whitespace character found
        ' * @throws IOException if there is an error reading from the file
        ' */
        Protected Function consumeWhitespace() As Byte ' throws IOException {
            inputFile.seek(currentOffset)
            Dim singleByte As Byte = inputFile.read()
            If (Not isWhitespace(singleByte)) Then Return singleByte

            ' we have some whitespace, let's consume it
            While (isWhitespace(singleByte))
                singleByte = readByte()
            End While
            ' readByte() will increment the currentOffset to point the byte
            ' after the one just read, so we decrement it back to the current byte
            currentOffset -= 1
            Return singleByte
        End Function

        '/**
        ' * This will consume any whitespace, read in bytes until whitespace is found
        ' * again and then parse the characters which have been read as a long.  The
        ' * current offset will then point at the first whitespace character which
        ' * preceeds the number.
        ' * @return the parsed number
        ' * @throws IOException if there is an error reading from the file
        ' * @throws NumberFormatException if the bytes read can not be converted to a number
        ' */
        Protected Function readLongBackwards() As Long ' throws IOException, NumberFormatException {
            Dim sb As New StringBuilder()
            consumeWhitespaceBackwards()
            Dim singleByte As Byte = readByteBackwards()
            While (Not isWhitespace(singleByte))
                sb.Insert(0, Convert.ToChar(singleByte))
                singleByte = readByteBackwards()
            End While
            If (sb.Length() = 0) Then
                Throw New Exception("Number not found.  Expected number at offset: " & currentOffset)
            End If
            Return Long.Parse(sb.ToString())
        End Function

        Protected Overrides Function readInt() As Integer ' throws IOException {
            Dim sb As New StringBuilder()
            consumeWhitespace()
            Dim singleByte As Byte = readByte()
            While (Not isWhitespace(singleByte))
                sb.Append(Convert.ToChar(singleByte))
                singleByte = readByte()
            End While
            If (sb.Length() = 0) Then
                Throw New Exception("Number not found.  Expected number at offset: " & currentOffset)
            End If
            Return Integer.Parse(sb.ToString())
        End Function

        '/**
        ' * This will read in a number and return the COS version of the number (be
        ' * it a COSInteger or a COSFloat).
        ' * @return the COSNumber which was read/parsed
        ' * @throws IOException
        ' */
        Protected Function readNumber() As COSNumber ' throws IOException {
            Dim sb As New StringBuilder()
            consumeWhitespace()
            Dim singleByte As Byte = readByte()
            While (Not isWhitespace(singleByte))
                sb.Append(Convert.ToChar(singleByte))
                singleByte = readByte()
            End While
            If (sb.Length() = 0) Then
                Throw New Exception("Number not found.  Expected number at offset: " & currentOffset)
            End If
            Return parseNumber(sb.ToString())
        End Function

        Protected Function parseNumber(ByVal number As String) As COSNumber 'throws IOException {
            If (Sistema.Strings.Metches(number, "^[0-9]+$")) Then
                Return COSInteger.get(number)
            End If
            Return New COSFloat(Single.Parse(number))
        End Function

        Protected Function processCosObject(ByVal [string] As String) As COSBase 'throws IOException {
            If ([string] <> "" AndAlso [string].EndsWith(">")) Then
                ' string of hex codes
                Return COSString.createFromHexString([string].Replace("^<", "").Replace(">$", ""))
            End If
            Return Nothing
        End Function

        Protected Function readObjectBackwards() As COSBase ' throws IOException {
            Dim obj As COSBase = Nothing
            consumeWhitespaceBackwards()
            Dim lastSection As String = readBackwardUntilWhitespace()
            If ("R".Equals(lastSection)) Then
                ' indirect reference
                Dim gen As Long = readLongBackwards()
                Dim number As Long = readLongBackwards()
                ' We just put a placeholder in the pool for now, we'll read the data later
                doc.putObjectInPool(New COSUnread(), number, gen)
                obj = New COSUnread(number, gen, Me)
            ElseIf (">>".Equals(lastSection)) Then
                ' dictionary
                Throw New RuntimeException("Not yet implemented")
            ElseIf (lastSection <> "" AndAlso lastSection.EndsWith("]")) Then
                ' array
                Dim array As New COSArray()
                lastSection = lastSection.Replace("]$", "")
                While (Not lastSection.StartsWith("["))
                    If (Sistema.Strings.Metches(lastSection, "^\s*<.*>\s*$")) Then ' it's a hex string
                        array.add(COSString.createFromHexString(lastSection.Replace("^\\s*<", "").Replace(">\\s*$", "")))
                    End If
                    lastSection = readBackwardUntilWhitespace()
                End While
                lastSection = lastSection.Replace("^\\[", "")
                If (Sistema.Strings.Metches(lastSection, "^\s*<.*>\s*$")) Then ' it's a hex string
                    array.add(COSString.createFromHexString(Sistema.Strings.ReplaceAll(Sistema.Strings.ReplaceAll(lastSection, "^\s*<", ""), ">\s*$", "")))
                End If
                obj = array
            ElseIf (lastSection <> "" AndAlso lastSection.EndsWith(">")) Then
                ' string of hex codes
                obj = processCosObject(lastSection)
            Else
                ' try a number, otherwise fall back on a string
                Try
                    Long.Parse(lastSection)
                    obj = COSNumber.get(lastSection)
                Catch e As FormatException
                    Throw New RuntimeException("Not yet implemented")
                End Try
            End If

            Return obj
        End Function

        Protected Function readNameBackwards() As COSName 'throws IOException {
            Dim name As String = readBackwardUntilWhitespace()
            name = Sistema.Strings.ReplaceAll(name, "^/", "")
            Return COSName.getPDFName(name)
        End Function

        Public Function getObject(ByVal objectNumber As Long, ByVal generation As Long) As COSBase 'throws IOException {
            ' we could optionally, check to see if parse() have been called &
            ' throw an exception here, but I don't think that's really necessary
            Dim entry As XrefEntry = xrefEntries.get(CInt(objectNumber))
            currentOffset = entry.getByteOffset()
            Return readObject(objectNumber, generation)
        End Function

        '/**
        ' * This will read an object from the inputFile at whatever our currentOffset
        ' * is.  If the object and generation are not the expected values and this
        ' * object is set to throw an exception for non-conforming documents, then an
        ' * exception will be thrown.
        ' * @param objectNumber the object number you expect to read
        ' * @param generation the generation you expect this object to be
        ' * @return
        ' */
        Public Function readObject(ByVal objectNumber As Long, ByVal generation As Long) As COSBase 'throws IOException {
            ' when recursivly reading, we always pull the object from the filesystem
            If (document IsNot Nothing AndAlso recursivlyRead) Then
                ' check to see if it is in the document cache before hitting the filesystem
                Dim obj1 As COSBase = doc.getObjectFromPool(objectNumber, generation)
                If (obj1 IsNot Nothing) Then
                    Return obj1
                End If
            End If

            Dim actualObjectNumber As Integer = readInt()
            If (objectNumber <> actualObjectNumber) Then
                If (throwNonConformingException) Then Throw New Exception("Object numer expected was " & objectNumber & " but actual was " & actualObjectNumber)
            End If
            consumeWhitespace()

            Dim actualGeneration As Integer = readInt()
            If (generation <> actualGeneration) Then
                If (throwNonConformingException) Then Throw New Exception("Generation expected was " & generation & " but actual was " & actualGeneration)
            End If
            consumeWhitespace()

            Dim obj As String = readWord()
            If (Not "obj".Equals(obj)) Then
                If (throwNonConformingException) Then Throw New Exception("Expected keyword 'obj' but found " & obj)
            End If

            ' put placeholder object in doc to prevent infinite recursion
            ' e.g. read Root -> dereference object -> read object which has /Parent -> GOTO read Root
            doc.putObjectInPool(New COSObject(Nothing), objectNumber, generation)
            Dim [object] As COSBase = readObject()
            doc.putObjectInPool([object], objectNumber, generation)
            Return [object]
        End Function

        '/**
        ' * This actually reads the object data.
        ' * @return the object which is read
        ' * @throws IOException
        ' */
        Protected Function readObject() As COSBase ' throws IOException {
            consumeWhitespace()
            Dim [string] As String = readWord()
            Dim tempOffset As Long

            If ([string].StartsWith("<<")) Then
                ' this is a dictionary
                Dim dictionary As New COSDictionary()
                Dim atEndOfDictionary As Boolean = False
                ' remove the marker for the beginning of the dictionary
                [string] = Sistema.Strings.ReplaceAll([string], "^<<", "")

                If ("".Equals([string]) OrElse Sistema.Strings.Metches([string], "^\w$")) Then
                    [string] = readWord().Trim()
                    While (Not atEndOfDictionary)
                        Dim name As COSName = COSName.getPDFName([string])
                        Dim [object] As COSBase = readObject()
                        dictionary.setItem(name, [object])

                        Dim singleByte As Byte = consumeWhitespace()
                        If (singleByte = Asc(">"c)) Then
                            readByte() ' get rid of the second '>'
                            atEndOfDictionary = True
                        End If
                        If (Not atEndOfDictionary) Then
                            [string] = readWord().Trim()
                        End If
                    End While
                    Return dictionary
                ElseIf ([string].StartsWith("/")) Then
                    ' it's a dictionary label. i.e. /Type or /Pages or something similar
                    Dim name As COSBase = COSName.getPDFName([string])
                    Return name
                ElseIf ([string].StartsWith("-")) Then
                    ' it's a negitive number
                    Return parseNumber([string])
                ElseIf ([string].Chars(0) >= "0"c AndAlso [string].Chars(0) <= "9"c) Then
                    ' it's a COSInt or COSFloat, or a weak reference (i.e. "3 0 R")
                    ' we'll have to peek ahead a little to see if it's a reference or not
                    tempOffset = Me.currentOffset
                    consumeWhitespace()
                    Dim tempString As String = readWord()
                    If (Sistema.Strings.Metches(tempString, "^[0-9]+$")) Then
                        ' it is an int, might be a weak reference...
                        tempString = readWord()
                    End If
                    If (Not "R".Equals(tempString)) Then
                        ' it's just a number, not a weak reference
                        Me.currentOffset = tempOffset
                        Return parseNumber([string])
                    End If
                Else
                    ' it's just a number, not a weak reference
                    Me.currentOffset = tempOffset
                    Return parseNumber([string])
                End If

                ' it wasn't a number, so we need to parse the weak-reference
                Me.currentOffset = tempOffset
                Dim number As Integer = Integer.Parse([string])
                Dim gen As Integer = readInt()
                Dim r As String = readWord()

                If (Not "R".Equals(r)) Then
                    If (throwNonConformingException) Then
                        Throw New Exception("Expected keyword 'R' but found " & r)
                    End If
                End If

                If (recursivlyRead) Then
                    ' seek to the object, read it, seek back to current location
                    Dim tempLocation As Long = Me.currentOffset
                    Me.currentOffset = Me.xrefEntries.get(number).getByteOffset()
                    Dim returnValue As COSBase = readObject(number, gen)
                    Me.currentOffset = tempLocation
                    Return returnValue
                Else
                    ' Put a COSUnknown there as a placeholder
                    Dim obj As New COSObject(New COSUnread())
                    obj.setObjectNumber(COSInteger.get(number))
                    obj.setGenerationNumber(COSInteger.get(gen))
                    Return obj
                End If
            ElseIf ([string].StartsWith("]")) Then
                ' end of an array, just return null
                If ("]".Equals([string])) Then Return Nothing
                Dim oldLength As Integer = [string].Length()
                Me.currentOffset -= oldLength
                Return Nothing
            ElseIf ([string].StartsWith("[")) Then
                ' array of values
                ' we'll just pay attention to the first part (this is in case there
                ' is no whitespace between the "[" and the first element)
                Dim oldLength As Integer = [string].Length()
                [string] = "["
                Me.currentOffset -= (oldLength - [string].Length() + 1)

                Dim array As New COSArray()
                Dim [object] As COSBase = readObject()
                While ([object] IsNot Nothing)
                    array.add([object])
                    [object] = readObject()
                End While
                Return array
            ElseIf ([string].StartsWith("(")) Then
                ' this is a string (not hex encoded), strip off the '(' and read until ')'
                Dim sb As New StringBuilder([string].Substring(1))
                Dim singleByte As Byte = readByte()
                While (singleByte <> Asc(")"c))
                    sb.Append(Convert.ToChar(singleByte))
                    singleByte = readByte()
                End While
                Return New COSString(sb.ToString())
            Else
                Throw New RuntimeException("Not yet implemented: " & [string] & " loation=" & Me.currentOffset)
            End If
        End Function

        '/**
        ' * This will read the next string from the stream.
        ' * @return The string that was read from the stream.
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Protected Overrides Function readString() As String ' throws IOException {
            consumeWhitespace()
            Dim buffer As New StringBuilder()
            Dim c As Integer = pdfSource.read()
            While (Not isEndOfName(Convert.ToChar(c)) AndAlso Not isClosing(c) AndAlso c <> -1)
                buffer.Append(Convert.ToChar(c))
                c = pdfSource.read()
            End While
            If (c <> -1) Then
                pdfSource.unread(c)
            End If
            Return buffer.ToString()
        End Function

        Protected Function readDictionaryBackwards() As COSDictionary ' throws IOException {
            Dim dict As New COSDictionary()

            ' consume the last two '>' chars which signify the end of the dictionary
            consumeWhitespaceBackwards()
            Dim singleByte As Byte = readByteBackwards()
            If (throwNonConformingException) Then
                If (singleByte <> Asc(">"c)) Then
                    Throw New Exception("")
                End If
            End If
            singleByte = readByteBackwards()
            If (throwNonConformingException) Then
                If (singleByte <> Asc(">"c)) Then
                    Throw New Exception("")
                End If
            End If

            ' check to see if we're at the end of the dictionary
            Dim atEndOfDictionary As Boolean = False
            singleByte = consumeWhitespaceBackwards()
            If (singleByte = Asc("<"c)) Then
                inputFile.seek(currentOffset - 1)
                atEndOfDictionary = (inputFile.read()) = Asc("<"c)
            End If

            Dim backwardsDictionary As New COSDictionary()
            ' while we're not at the end of the dictionary, read in entries
            While (Not atEndOfDictionary)
                Dim [object] As COSBase = readObjectBackwards()
                Dim name As COSName = readNameBackwards()
                backwardsDictionary.setItem(name, [object])

                singleByte = consumeWhitespaceBackwards()
                If (singleByte = Asc("<"c)) Then
                    inputFile.seek(currentOffset - 1)
                    atEndOfDictionary = (inputFile.read()) = Asc("<"c)
                End If
            End While

            ' the dictionaries preserve the order keys were added, as such we shall
            ' add them in the proper order, not the reverse order
            Dim backwardsKeys As ICollection(Of COSName) = backwardsDictionary.keySet() 'Set(of COSName) 
            For i As Integer = backwardsKeys.size() - 1 To 0 Step -1
                dict.setItem(backwardsKeys.toArray()(i), backwardsDictionary.getItem(backwardsKeys.toArray()(i)))
            Next
            ' consume the last two '<' chars
            readByteBackwards()
            readByteBackwards()

            Return dict
        End Function

        '/**
        ' * This will read a line starting with the byte at offset and going 
        ' * backwards until it finds a newline.  This should only be used if we are
        ' * certain that the data will only be text, and not binary data.
        ' * 
        ' * @param offset the location of the file where we should start reading
        ' * @return the string which was read
        ' * @throws IOException if there was an error reading data from the file
        ' */
        Protected Function readLineBackwards() As String  'throws IOException {
            Dim sb = New StringBuilder()
            Dim endOfObject As Boolean = False

            Do
                ' first we read the %%EOF marker
                Dim singleByte As Byte = readByteBackwards()
                If (singleByte = vbLf) Then ''\n') {
                    ' if ther's a preceeding \r, we'll eat that as well
                    inputFile.seek(currentOffset)
                    If (inputFile.read() = vbCr) Then ' '\r')
                        currentOffset -= 1
                    End If
                    endOfObject = True
                ElseIf (singleByte = vbCr) Then ' '\r') {
                    endOfObject = True
                Else
                    sb.Insert(0, singleByte)
                End If
            Loop While (Not endOfObject)

            Return sb.ToString()
        End Function

        '/**
        ' * This will read a line starting with the byte at offset and going
        ' * forward until it finds a newline.  This should only be used if we are
        ' * certain that the data will only be text, and not binary data.
        ' * @param offset the location of the file where we should start reading
        ' * @return the string which was read
        ' * @throws IOException if there was an error reading data from the file
        ' */
        Protected Overrides Function readLine() As String ' throws IOException {
            Dim sb As New StringBuilder()
            Dim endOfLine As Boolean = False

            Do
                ' first we read the %%EOF marker
                Dim singleByte As Byte = readByte()
                If (singleByte = vbLf) Then ''\n') {
                    ' if ther's a preceeding \r, we'll eat that as well
                    inputFile.seek(currentOffset)
                    If (inputFile.read() = vbCr) Then ' '\r')
                        currentOffset += 1
                    End If
                    endOfLine = True
                ElseIf (singleByte = vbCr) Then ''\r') {
                    endOfLine = True
                Else
                    sb.Append(singleByte)
                End If
            Loop While (Not endOfLine)

            Return sb.ToString()
        End Function

        Protected Function readWord() As String 'throws IOException {
            Dim sb As New StringBuilder()
            Dim [stop] As Boolean = True
            Do
                Dim singleByte As Byte = readByte()
                [stop] = Me.isWhitespace(singleByte)

                ' there are some additional characters which indicate the next element/word has begun
                ' ignore the first char we read, b/c the first char is the beginnging of this object, not the next one
                If (Not [stop] AndAlso sb.Length() > 0) Then
                    [stop] = singleByte = Asc("/"c) OrElse singleByte = Asc("["c) OrElse singleByte = Asc("]"c) OrElse (singleByte = Asc(">"c) AndAlso Not ">".Equals(sb.ToString()))
                    If ([stop]) Then ' we're stopping on a non-whitespace char, decrement the
                        Me.currentOffset -= 1 ' counter so we don't miss this character
                    End If
                End If
                If (Not [stop]) Then sb.Append(Convert.ToChar(singleByte))
            Loop While (Not [stop])

            Return sb.ToString()
        End Function

        '/**
        ' * @return the recursivlyRead
        ' */
        Public Function isRecursivlyRead() As Boolean
            Return recursivlyRead
        End Function

        '/**
        ' * @param recursivlyRead the recursivlyRead to set
        ' */
        Public Sub setRecursivlyRead(ByVal recursivlyRead As Boolean)
            Me.recursivlyRead = recursivlyRead
        End Sub

    End Class

End Namespace
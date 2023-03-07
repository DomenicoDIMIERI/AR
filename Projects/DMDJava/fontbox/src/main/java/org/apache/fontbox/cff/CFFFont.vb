Imports System.IO
Imports FinSeA.org.apache.fontbox.cff
Imports FinSeA.org.apache.fontbox.cff.charset
Imports FinSeA.org.apache.fontbox.cff.encoding
Imports FinSeA

Namespace org.apache.fontbox.cff


    '/**
    ' * This class represents a CFF/Type2 Font.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class CFFFont

        Private fontname As String = vbNullString
        Private topDict As Map(Of String, Object) = New LinkedHashMap(Of String, Object)()
        Private privateDict As Map(Of String, Object) = New LinkedHashMap(Of String, Object)()
        Private fontEncoding As CFFEncoding = Nothing
        Private fontCharset As CFFCharset = Nothing
        Private charStringsDict As Map(Of String, Byte()) = New LinkedHashMap(Of String, Byte())()
        Private globalSubrIndex As IndexData = Nothing
        Private localSubrIndex As IndexData = Nothing

        '/**
        ' * The name of the font.
        ' * @return the name of the font
        ' */
        Public Function getName() As String
            Return fontname
        End Function

        '/**
        ' * Sets the name of the font.
        ' * @param name the name of the font
        ' */
        Public Sub setName(ByVal name As String)
            fontname = name
        End Sub

        '/**
        ' * Returns the value for the given name from the dictionary.
        ' * @param name the name of the value
        ' * @return the value of the name if available
        ' */
        Public Function getProperty(ByVal name As String) As Object
            Dim topDictValue As Object = topDict.get(name)
            If (topDictValue IsNot Nothing) Then
                Return topDictValue
            End If
            Dim privateDictValue As Object = privateDict.get(name)
            If (privateDictValue IsNot Nothing) Then
                Return privateDictValue
            End If
            Return Nothing
        End Function

        '/**
        ' * Adds the given key/value pair to the top dictionary. 
        ' * @param name the given key
        ' * @param value the given value
        ' */
        Public Sub addValueToTopDict(ByVal name As String, ByVal value As Object)
            If (value IsNot Nothing) Then
                topDict.put(name, value)
            End If
        End Sub

        '/** 
        ' * Returns the top dictionary.
        ' * @return the dictionary
        ' */
        Public Function getTopDict() As Map(Of String, Object)
            Return topDict
        End Function

        '/**
        ' * Adds the given key/value pair to the private dictionary. 
        ' * @param name the given key
        ' * @param value the given value
        ' */
        Public Sub addValueToPrivateDict(ByVal name As String, ByVal value As Object)
            If (value IsNot Nothing) Then
                privateDict.put(name, value)
            End If
        End Sub

        '/** 
        ' * Returns the private dictionary.
        ' * @return the dictionary
        ' */
        Public Function getPrivateDict() As Map(Of String, Object)
            Return privateDict
        End Function

        '/**
        ' * Get the mapping (code/SID/charname/bytes) for Me font.
        ' * @return mappings for codes < 256 and for codes > = 256
        ' */
        Public Function getMappings() As ICollection(Of Mapping)
            Dim mappings As List(Of Mapping) = New ArrayList(Of Mapping)()
            Dim mappedNames As [Set](Of String) = New HashSet(Of String)()
            For Each entry As CFFEncoding.Entry In fontEncoding.getEntries()
                Dim charName As String = fontCharset.getName(entry.getSID())
                ' Predefined encoding
                If (charName = vbNullString) Then
                    Continue For
                End If
                Dim bytes() As Byte = charStringsDict.get(charName)
                If (bytes Is Nothing) Then
                    Continue For
                End If
                Dim mapping As New Mapping(Me)
                mapping.setCode(entry.getCode())
                mapping.setSID(entry.getSID())
                mapping.setName(charName)
                mapping.setBytes(bytes)
                mappings.add(mapping)
                mappedNames.add(charName)
            Next
            If (TypeOf (fontEncoding) Is CFFParser.EmbeddedEncoding) Then
                Dim embeddedEncoding As CFFParser.EmbeddedEncoding = fontEncoding

                For Each supplement As CFFParser.EmbeddedEncoding.Supplement In embeddedEncoding.getSupplements()
                    Dim charName As String = fontCharset.getName(supplement.getGlyph())
                    If (charName = vbNullString) Then
                        Continue For
                    End If
                    Dim bytes() As Byte = charStringsDict.get(charName)
                    If (bytes Is Nothing) Then
                        Continue For
                    End If
                    Dim mapping As New Mapping(Me)
                    mapping.setCode(supplement.getCode())
                    mapping.setSID(supplement.getGlyph())
                    mapping.setName(charName)
                    mapping.setBytes(bytes)
                    mappings.add(mapping)
                    mappedNames.add(charName)
                Next
            End If
            ' XXX
            Dim code As Integer = 256
            For Each entry As CFFCharset.Entry In fontCharset.getEntries()
                Dim name As String = entry.getName()
                If (mappedNames.Contains(name)) Then
                    Continue For
                End If
                Dim bytes() As Byte = Me.charStringsDict.get(name)
                If (bytes Is Nothing) Then
                    Continue For
                End If
                Dim mapping As New Mapping(Me)
                mapping.setCode(code) : code += 1 'code++
                mapping.setSID(entry.getSID())
                mapping.setName(name)
                mapping.setBytes(bytes)

                mappings.add(mapping)

                mappedNames.add(name)
            Next
            Return mappings
        End Function

        '/**
        ' * Return the Width value of the given Glyph identifier
        ' * 
        ' * @param SID
        ' * @return -1 if the SID is missing from the Font.
        ' * @throws IOException
        ' */
        Public Overridable Function getWidth(ByVal SID As Integer) As Integer ' throws IOException {
            Dim nominalWidth As Integer = 0
            If (privateDict.containsKey("nominalWidthX")) Then nominalWidth = DirectCast(privateDict.get("nominalWidthX"), Number).intValue()
            Dim defaultWidth As Integer = 1000
            If (privateDict.containsKey("defaultWidthX")) Then defaultWidth = DirectCast(privateDict.get("defaultWidthX"), Number).intValue()

            For Each m As Mapping In getMappings()
                If (m.getSID() = SID) Then
                    Dim csr As CharStringRenderer = Nothing
                    If (DirectCast(getProperty("CharstringType"), Number).intValue() = 2) Then
                        Dim lSeq As List(Of Object) = m.toType2Sequence()
                        csr = New CharStringRenderer(False)
                        csr.render(lSeq)
                    Else
                        Dim lSeq As List(Of Object) = m.toType1Sequence()
                        csr = New CharStringRenderer()
                        csr.render(lSeq)
                    End If

                    ' ---- If the CharString has a Width nominalWidthX must be added, 
                    '	    otherwise it is the default width.
                    Return IIf(csr.getWidth() <> 0, csr.getWidth() + nominalWidth, defaultWidth)
                End If
            Next

            ' ---- SID Width not found, return the nodef width
            Return getNotDefWidth(defaultWidth, nominalWidth)
        End Function

        Protected Function getNotDefWidth(ByVal defaultWidth As Integer, ByVal nominalWidth As Integer) As Integer
            Dim csr As CharStringRenderer
            Dim glyphDesc() As Byte = Me.getCharStringsDict().get(".notdef")
            If (DirectCast(getProperty("CharstringType"), Number).intValue() = 2) Then

                Dim parser As New Type2CharStringParser()
                Dim lSeq As List(Of Object) = parser.parse(glyphDesc, getGlobalSubrIndex(), getLocalSubrIndex())
                csr = New CharStringRenderer(False)
                csr.render(lSeq)
            Else
                Dim parser As New Type1CharStringParser()
                Dim lSeq As List(Of Object) = parser.parse(glyphDesc, getLocalSubrIndex())
                csr = New CharStringRenderer()
                csr.render(lSeq)
            End If
            Return IIf(csr.getWidth() <> 0, csr.getWidth() + nominalWidth, defaultWidth)
        End Function

        '/**
        ' * Returns the CFFEncoding of the font.
        ' * @return the encoding
        ' */
        Public Function getEncoding() As CFFEncoding
            Return fontEncoding
        End Function

        '/**
        ' * Sets the CFFEncoding of the font.
        ' * @param encoding the given CFFEncoding
        ' */
        Public Sub setEncoding(ByVal encoding As CFFEncoding)
            fontEncoding = encoding
        End Sub

        '/**
        ' * Returns the CFFCharset of the font.
        ' * @return the charset
        ' */
        Public Function getCharset() As CFFCharset
            Return fontCharset
        End Function

        '/**
        ' * Sets the CFFCharset of the font.
        ' * @param charset the given CFFCharset
        ' */
        Public Sub setCharset(ByVal charset As CFFCharset)
            fontCharset = charset
        End Sub

        '/** 
        ' * Returns the character strings dictionary.
        ' * @return the dictionary
        ' */
        Public Function getCharStringsDict() As Map(Of String, Byte())
            Return charStringsDict
        End Function

        '/**
        ' * Creates a CharStringConverter for Me font.
        ' * @return the new CharStringConverter
        ' */
        Public Function createConverter() As CharStringConverter
            Dim defaultWidthX As Number = getProperty("defaultWidthX")
            Dim nominalWidthX As Number = getProperty("nominalWidthX")
            Return New CharStringConverter(defaultWidthX.intValue(), nominalWidthX.intValue())
        End Function

        '/**
        ' * Creates a CharStringRenderer for Me font.
        ' * @return the new CharStringRenderer
        ' */
        Public Function createRenderer() As CharStringRenderer
            Return New CharStringRenderer()
        End Function

        Public Overrides Function toString() As String
            Return Me.GetType().Name & "[name=" & fontname.ToString & ", topDict=" & topDict.ToString & ", privateDict=" & privateDict.ToString & ", encoding=" & fontEncoding.ToString & ", charset=" & fontCharset.ToString & ", charStringsDict=" & charStringsDict.ToString & "]"
        End Function

        '      	/**
        '* Sets the global subroutine index data.
        '* @param globalSubrIndex the IndexData object containing the global subroutines 
        '*/
        Public Sub setGlobalSubrIndex(ByVal globalSubrIndex As IndexData)
            Me.globalSubrIndex = globalSubrIndex
        End Sub

        '/** 
        ' * Returns the global subroutine index data.
        ' * @return the dictionary
        ' */
        Public Function getGlobalSubrIndex() As IndexData
            Return globalSubrIndex
        End Function

        '/** 
        ' * Returns the local subroutine index data.
        ' * @return the dictionary
        ' */
        Public Function getLocalSubrIndex() As IndexData
            Return localSubrIndex
        End Function

        '/**
        ' * Sets the local subroutine index data.
        ' * @param localSubrIndex the IndexData object containing the local subroutines 
        ' */
        Public Sub setLocalSubrIndex(ByVal localSubrIndex As IndexData)
            Me.localSubrIndex = localSubrIndex
        End Sub

        '/**
        ' * This class is used for the font mapping.
        ' *
        ' */
        Public Class Mapping
            Private m_font As CFFFont

            Private mappedCode As Integer
            Private mappedSID As Integer
            Private mappedName As String
            Private mappedBytes() As Byte

            Public Sub New(ByVal f As CFFFont)
                Me.m_font = f
            End Sub

            '/**
            ' * Converts the mapping into a Type1-sequence.
            ' * @return the Type1-sequence
            ' * @throws IOException if an error occurs during reading
            ' */
            Public Function toType1Sequence() As List(Of Object)
                Dim converter As CharStringConverter = Me.m_font.createConverter()
                Return converter.convert(toType2Sequence())
            End Function

            '/**
            ' * Converts the mapping into a Type2-sequence.
            ' * @return the Type2-sequence
            ' * @throws IOException if an error occurs during reading
            ' */
            Public Function toType2Sequence() As List(Of Object)
                Dim parser As New Type2CharStringParser()
                Return parser.parse(getBytes(), Me.m_font.getGlobalSubrIndex(), Me.m_font.getLocalSubrIndex())
            End Function

            '/**
            ' * Gets the value for the code.
            ' * @return the code
            ' */
            Public Function getCode() As Integer
                Return mappedCode
            End Function

            Friend Sub setCode(ByVal code As Integer)
                mappedCode = code
            End Sub

            '/**
            ' * Gets the value for the SID.
            ' * @return the SID
            ' */
            Public Function getSID() As Integer
                Return mappedSID
            End Function

            Friend Sub setSID(ByVal sid As Integer)
                Me.mappedSID = sid
            End Sub

            '/**
            ' * Gets the value for the name.
            ' * @return the name
            ' */
            Public Function getName() As String
                Return mappedName
            End Function

            Friend Sub setName(ByVal name As String)
                Me.mappedName = name
            End Sub

            '/**
            ' * Gets the value for the bytes.
            ' * @return the bytes
            ' */
            Public Function getBytes() As Byte()
                Return mappedBytes
            End Function

            Friend Sub setBytes(ByVal bytes As Byte())
                Me.mappedBytes = bytes
            End Sub

        End Class

        Function TYPE1_FONT() As Object
            Throw New NotImplementedException
        End Function

    End Class

End Namespace
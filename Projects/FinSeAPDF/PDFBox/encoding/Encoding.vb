Imports FinSeA.Io
Imports FinSeA.Text
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO
Imports System.Text

Namespace org.apache.pdfbox.encoding

    '/**
    ' * This is an interface to a text encoder.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.15 $
    ' */
    Public MustInherit Class Encoding
        Implements COSObjectable

        '/**
        ' * Log instance.
        ' */
        'Private Shared ReadOnly Log As Log = LogFactory.getLog(Encoding.class)

        ''' <summary>
        ''' Identifies a non-mapped character.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly NOTDEF As String = ".notdef"

        ''' <summary>
        ''' This is a mapping from a character code to a character name.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend codeToName As New HashMap(Of NInteger, String)

        ''' <summary>
        ''' This is a mapping from a character name to a character code.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend nameToCode As New HashMap(Of String, Integer)

        Private Shared NAME_TO_CHARACTER As New HashMap(Of String, String)

        Private Shared CHARACTER_TO_NAME As New HashMap(Of String, String)

        Shared Sub New()
            'Loads the official Adobe Glyph List
            loadGlyphList("org/apache/pdfbox/resources/glyphlist.txt")
            'Loads some additional glyph mappings
            loadGlyphList("org/apache/pdfbox/resources/additional_glyphlist.txt")

            ' Load an external glyph list file that user can give as JVM property
            Dim location As String = "" 'My.Settings.g("glyphlist_ext")
            If (location <> "") Then
                If (FinSeA.Sistema.FileSystem.FileExists(location)) Then
                    loadGlyphList(location)
                End If
            End If

            NAME_TO_CHARACTER.put(NOTDEF, "")
            NAME_TO_CHARACTER.put("fi", "fi")
            NAME_TO_CHARACTER.put("fl", "fl")
            NAME_TO_CHARACTER.put("ffi", "ffi")
            NAME_TO_CHARACTER.put("ff", "ff")
            NAME_TO_CHARACTER.put("pi", "pi")

            For Each entry As Map.Entry(Of String, String) In NAME_TO_CHARACTER.entrySet()
                CHARACTER_TO_NAME.put(entry.Value, entry.Key)
            Next
        End Sub

        '/**
        ' * Loads a glyph list from a given location and populates the NAME_TO_CHARACTER hashmap
        ' * for character lookups.
        ' * @param location - The string location of the glyphlist file
        ' */
        Private Shared Sub loadGlyphList(ByVal location As String)
            Dim glyphStream As BufferedReader = Nothing
            Try
                Dim resource As InputStream = ResourceLoader.loadResource(location)
                If (resource Is Nothing) Then
                    Throw New Exception("Glyphlist not found: " & location)
                End If
                glyphStream = New BufferedReader(New InputStreamReader(resource))
                Dim line As String = glyphStream.readLine()
                While (line <> "")
                    line = line.Trim()
                    'lines starting with # are comments which we can ignore.
                    If (Not line.StartsWith("#")) Then
                        Dim semicolonIndex As Integer = line.IndexOf(";")
                        If (semicolonIndex >= 0) Then
                            Dim unicodeValue As String = ""
                            Try
                                Dim characterName As String = line.Substring(0, semicolonIndex)
                                unicodeValue = line.Substring(semicolonIndex + 1) ', line.Length())
                                Dim tokenizer As New StringTokenizer(unicodeValue, " ", False)
                                Dim value As New StringBuilder()
                                While (tokenizer.hasMoreTokens())
                                    Dim characterCode As Integer = CInt("&H" & tokenizer.nextToken())
                                    value.Append(ChrW(characterCode))
                                End While
                                If (NAME_TO_CHARACTER.containsKey(characterName)) Then
                                    LOG.warn("duplicate value for characterName=" & characterName.ToString & "," & value.ToString)
                                Else
                                    NAME_TO_CHARACTER.put(characterName, value.ToString())
                                End If
                            Catch nfe As FormatException
                                LOG.error("malformed unicode value " + unicodeValue, nfe)
                            End Try
                        End If
                    End If
                    line = glyphStream.readLine()
                End While
            Catch io As IOException
                LOG.error("error while reading the glyph list.", io)
            Finally
                If (glyphStream IsNot Nothing) Then
                    Try
                        glyphStream.Close()
                    Catch e As IOException
                        LOG.error("error when closing the glyph list.", e)
                    End Try
                End If
            End Try
        End Sub

        '/**
        ' * Returns an unmodifiable view of the Code2Name mapping.
        ' * @return the Code2Name map
        ' */
        Public Function getCodeToNameMap() As Map(Of NInteger, String)
            Return Collections.unmodifiableMap(codeToName)
        End Function

        '/**
        ' * Returns an unmodifiable view of the Name2Code mapping.
        ' * @return the Name2Code map
        ' */
        Public Function getNameToCodeMap() As Map(Of String, NInteger)
            Return Collections.unmodifiableMap(nameToCode)
        End Function

        '/**
        ' * This will add a character encoding.
        ' *
        ' * @param code The character code that matches the character.
        ' * @param name The name of the character.
        ' */
        Public Sub addCharacterEncoding(ByVal code As Integer, ByVal name As String)
            codeToName.put(code, name)
            nameToCode.put(name, code)
        End Sub

        '/**
        ' * This will get the character code for the name.
        ' *
        ' * @param name The name of the character.
        ' *
        ' * @return The code for the character.
        ' *
        ' * @throws IOException If there is no character code for the name.
        ' */
        Public Function getCode(ByVal name As String) As Integer ' throws IOException
            Dim code As Nullable(Of Integer) = nameToCode.get(name)
            If (code.HasValue = False) Then
                Throw New IOException("No character code for character name '" & name & "'")
            End If
            Return code
        End Function

        '/**
        ' * This will take a character code and get the name from the code.
        ' *
        ' * @param code The character code.
        ' *
        ' * @return The name of the character.
        ' *
        ' * @throws IOException If there is no name for the code.
        ' */
        Public Overridable Function getName(ByVal code As Integer) As String ' throws IOException
            Return codeToName.get(code)
        End Function

        '/**
        ' * This will take a character code and get the name from the code.
        ' *
        ' * @param c The character.
        ' *
        ' * @return The name of the character.
        ' *
        ' * @throws IOException If there is no name for the character.
        ' */
        Public Function getNameFromCharacter(ByVal c As Char) As String 'throws IOException
            Dim name As Nullable(Of Char) = CHARACTER_TO_NAME.get(CStr(c))
            If (name = "") Then
                Throw New IOException("No name for character '" & c & "'")
            End If
            Return name
        End Function

        '/**
        ' * This will get the character from the code.
        ' *
        ' * @param code The character code.
        ' *
        ' * @return The printable character for the code.
        ' *
        ' * @throws IOException If there is not name for the character.
        ' */
        Public Function getCharacter(ByVal code As Integer) As String  'throws IOException
            Dim name As String = getName(code)
            If (name <> "") Then
                Return getCharacter(getName(code))
            End If
            Return ""
        End Function

        '/**
        ' * This will get the character from the name.
        ' *
        ' * @param name The name of the character.
        ' *
        ' * @return The printable character for the code.
        ' */
        Public Function getCharacter(ByVal name As String) As String
            Dim character As String = NAME_TO_CHARACTER.get(name)
            If (character = "") Then
                ' test if we have a suffix and if so remove it
                If (name.IndexOf(".") > 0) Then
                    character = getCharacter(name.Substring(0, name.IndexOf(".")))
                End If
                ' test for Unicode name
                ' (uniXXXX - XXXX must be a multiple of four;
                ' each representing a hexadecimal Unicode code point)
            ElseIf (name.StartsWith("uni")) Then
                Dim nameLength As Integer = name.Length()
                Dim uniStr As New StringBuilder()
                Try
                    For chPos As Integer = 3 To nameLength - 4 Step 4
                        Dim characterCode As Integer = CInt("&H" & name.Substring(chPos, chPos + 4))

                        If (characterCode > &HD7FF AndAlso characterCode < &HE000) Then
                            LOG.warn("Unicode character name with not allowed code area: " & name)
                        Else
                            uniStr.Append(Convert.ToChar(characterCode))
                        End If
                    Next
                    character = uniStr.ToString()
                    NAME_TO_CHARACTER.put(name, character)
                Catch nfe As FormatException
                    LOG.warn("Not a number in Unicode character name: " & name)
                    character = name
                End Try
                ' test for an alternate Unicode name representation 
            ElseIf (name.StartsWith("u")) Then
                Try
                    Dim characterCode As Integer = CInt("&H" & name.Substring(1))
                    If (characterCode > &HD7FF AndAlso characterCode < &HE000) Then
                        LOG.warn("Unicode character name with not allowed code area: " & name)
                    Else
                        character = Val(Chr(characterCode))
                        NAME_TO_CHARACTER.put(name, character)
                    End If
                Catch nfe As Exception
                    LOG.warn("Not a number in Unicode character name: " & name)
                    character = name
                End Try
            ElseIf (nameToCode.containsKey(name)) Then
                Dim code As Integer = nameToCode.get(name)
                character = Chr(code)
            Else
                character = name
            End If
            'End If
            Return character
        End Function

        Public MustOverride Function getCOSObject() As cos.COSBase Implements COSObjectable.getCOSObject
         
    End Class

End Namespace
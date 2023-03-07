Imports System.IO

Namespace org.apache.fontbox.encoding

    '/**
    ' * This is an interface to a text encoder.
    ' *
    ' * @author Ben Litchfield
    ' * @version $Revision: 1.1 $
    ' * 
    ' * 
    ' */
    Public MustInherit Class Encoding

        ''' <summary>
        ''' The number of standard mac glyph names.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NUMBER_OF_MAC_GLYPHS = 258

        ''' <summary>
        ''' The 258 standard mac glyph names a used in 'post' format 1 and 2.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly MAC_GLYPH_NAMES() As String = { _
            ".notdef", ".null", "nonmarkingreturn", "space", "exclam", "quotedbl", _
                "numbersign", "dollar", "percent", "ampersand", "quotesingle", _
                "parenleft", "parenright", "asterisk", "plus", "comma", "hyphen", _
                "period", "slash", "zero", "one", "two", "three", "four", "five", _
                "six", "seven", "eight", "nine", "colon", "semicolon", "less", _
                "equal", "greater", "question", "at", "A", "B", "C", "D", "E", "F", _
                "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", _
                "T", "U", "V", "W", "X", "Y", "Z", "bracketleft", "backslash", _
                "bracketright", "asciicircum", "underscore", "grave", "a", "b", _
                "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", _
                "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "braceleft", _
                "bar", "braceright", "asciitilde", "Adieresis", "Aring", _
                "Ccedilla", "Eacute", "Ntilde", "Odieresis", "Udieresis", "aacute", _
                "agrave", "acircumflex", "adieresis", "atilde", "aring", _
                "ccedilla", "eacute", "egrave", "ecircumflex", "edieresis", _
                "iacute", "igrave", "icircumflex", "idieresis", "ntilde", "oacute", _
                "ograve", "ocircumflex", "odieresis", "otilde", "uacute", "ugrave", _
                "ucircumflex", "udieresis", "dagger", "degree", "cent", "sterling", _
                "section", "bullet", "paragraph", "germandbls", "registered", _
                "copyright", "trademark", "acute", "dieresis", "notequal", "AE", _
                "Oslash", "infinity", "plusminus", "lessequal", "greaterequal", _
                "yen", "mu", "partialdiff", "summation", "product", "pi", _
                "integral", "ordfeminine", "ordmasculine", "Omega", "ae", "oslash", _
                "questiondown", "exclamdown", "logicalnot", "radical", "florin", _
                "approxequal", "Delta", "guillemotleft", "guillemotright", _
                "ellipsis", "nonbreakingspace", "Agrave", "Atilde", "Otilde", "OE", _
                "oe", "endash", "emdash", "quotedblleft", "quotedblright", _
                "quoteleft", "quoteright", "divide", "lozenge", "ydieresis", _
                "Ydieresis", "fraction", "currency", "guilsinglleft", _
                "guilsinglright", "fi", "fl", "daggerdbl", "periodcentered", _
                "quotesinglbase", "quotedblbase", "perthousand", "Acircumflex", _
                "Ecircumflex", "Aacute", "Edieresis", "Egrave", "Iacute", _
                "Icircumflex", "Idieresis", "Igrave", "Oacute", "Ocircumflex", _
                "apple", "Ograve", "Uacute", "Ucircumflex", "Ugrave", "dotlessi", _
                "circumflex", "tilde", "macron", "breve", "dotaccent", "ring", _
                "cedilla", "hungarumlaut", "ogonek", "caron", "Lslash", "lslash", _
                "Scaron", "scaron", "Zcaron", "zcaron", "brokenbar", "Eth", "eth", _
                "Yacute", "yacute", "Thorn", "thorn", "minus", "multiply", _
                "onesuperior", "twosuperior", "threesuperior", "onehalf", _
                "onequarter", "threequarters", "franc", "Gbreve", "gbreve", _
                "Idotaccent", "Scedilla", "scedilla", "Cacute", "cacute", "Ccaron", _
                "ccaron", "dcroat" _
        }

        ''' <summary>
        ''' The indices of the standard mac glyph names.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared MAC_GLYPH_NAMES_INDICES As Map(Of String, NInteger)

        Shared Sub New()
            MAC_GLYPH_NAMES_INDICES = New HashMap(Of String, NInteger)()
            For i As Integer = 0 To Encoding.NUMBER_OF_MAC_GLYPHS
                MAC_GLYPH_NAMES_INDICES.put(Encoding.MAC_GLYPH_NAMES(i), i)
            Next
        End Sub

        ''' <summary>
        ''' Identifies a non-mapped character. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NOTDEF = ".notdef"

        ''' <summary>
        ''' This is a mapping from a character code to a character name.
        ''' </summary>
        ''' <remarks></remarks>
        Protected codeToName As Map(Of NInteger, String) = New HashMap(Of NInteger, String)()

        ''' <summary>
        ''' This is a mapping from a character name to a character code.
        ''' </summary>
        ''' <remarks></remarks>
        Protected nameToCode As Map(Of String, NInteger) = New HashMap(Of String, NInteger)()

        Private Shared NAME_TO_CHARACTER As Map(Of String, String) = New HashMap(Of String, String)()
        Private Shared CHARACTER_TO_NAME As Map(Of String, String) = New HashMap(Of String, String)()

        '/**
        ' * This will add a character encoding.
        ' *
        ' * @param code The character code that matches the character.
        ' * @param name The name of the character.
        ' */
        Protected Sub addCharacterEncoding(ByVal code As Integer, ByVal name As String)
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
        Public Function getCode(ByVal name As String) As Integer
            Dim code As NInteger = nameToCode.get(name)
            If (code.HasValue = False) Then
                Throw New IOException("No character code for character name '" & name & "'")
            End If
            Return code.Value
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
        Public Function getName(ByVal code As Integer) As String
            Dim name As String = codeToName.get(code)
            If (name = vbNullString) Then
                name = NOTDEF
            End If
            Return name
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
        Public Function getNameFromCharacter(ByVal c As Char) As String
            Dim name As String = CHARACTER_TO_NAME.get(c)
            If (name = vbNullString) Then
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
        Public Function getCharacter(ByVal code As Integer) As String
            Return getCharacter(getName(code))
        End Function

        '/**
        ' * This will get the character from the name.
        ' *
        ' * @param name The name of the character.
        ' *
        ' * @return The printable character for the code.
        ' */
        Public Shared Function getCharacter(ByVal name As String) As String
            Dim character As String = NAME_TO_CHARACTER.get(name)
            If (character = vbNullString) Then
                character = name
            End If
            Return character
        End Function

    End Class

End Namespace
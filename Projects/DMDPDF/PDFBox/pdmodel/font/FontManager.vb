Imports System.IO
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' *  This class is used as font manager.
    ' *  @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' *  @version $Revision: 1.0 $
    ' */

    Public NotInheritable Class FontManager


        ' HashMap with all known fonts
        Private Shared envFonts As HashMap(Of String, JFont) = New HashMap(Of String, JFont)
        ' the standard font
        Private Const standardFont As String = "helvetica"
        Private Shared fontMapping As Properties = New Properties()

        Shared Sub New()
            Try
                ResourceLoader.loadProperties("org/apache/pdfbox/resources/FontMapping.properties", fontMapping)
            Catch io As IOException
                Debug.Print(io.ToString)
                Throw New RuntimeException("Error loading font mapping")
            End Try
            loadFonts()
            loadBasefontMapping()
            loadFontMapping()
        End Sub

        Private Sub New()
        End Sub

        '/**
        ' * Get the standard font from the environment, usually Arial or Times New Roman. 
        ' *
        ' * @return The standard font 
        ' * 
        ' */
        Public Shared Function getStandardFont() As JFont
            Return getAwtFont(standardFont)
        End Function

        '/**
        ' * Get the font for the given fontname.
        ' *
        ' * @param font The name of the font.
        ' *
        ' * @return The font we are looking for or a similar font or null if nothing is found.
        ' * 
        ' */
        Public Shared Function getAwtFont(ByVal font As String) As JFont
            Dim fontname As String = normalizeFontname(font)
            If (envFonts.containsKey(fontname)) Then
                Return envFonts.get(fontname)
            End If
            Return Nothing
        End Function

        '/**
        ' * Load all available fonts from the environment.
        ' */
        Private Shared Sub loadFonts()
            Dim allFonts() As JFont = JFont.GetAllFontsAsArray 'java.awt.GraphicsEnvironment.getLocalGraphicsEnvironment().getAllFonts();
            Dim numberOfFonts As Integer = allFonts.length
            For i As Integer = 0 To numberOfFonts - 1
                Dim font As JFont = allFonts(i)
                Dim family As String = normalizeFontname(font.getFamily()) ' )
                Dim psname As String = normalizeFontname(font.getPSName()) ' )
                If (isBoldItalic(font)) Then
                    envFonts.put(family & "bolditalic", font)
                ElseIf (isBold(font)) Then
                    envFonts.put(family & "bold", font)
                ElseIf (isItalic(font)) Then
                    envFonts.put(family & "italic", font)
                Else
                    envFonts.put(family, font)
                End If
                If (Not family.Equals(psname)) Then
                    envFonts.put(normalizeFontname(font.getPSName()), font) '
                End If
            Next
        End Sub

        '/**
        ' * Normalize the fontname.
        ' *
        ' * @param fontname The name of the font.
        ' *
        ' * @return The normalized name of the font.
        ' * 
        ' */
        Private Shared Function normalizeFontname(ByVal fontname As String) As String
            ' Terminate all whitespaces, commas and hyphens
            Dim normalizedFontname As String = fontname.ToLower().Replace(" ", "").Replace(",", "").Replace("-", "")
            ' Terminate trailing characters up to the "& ".
            ' As far as I know, these characters are used in names of embedded fonts
            ' If the embedded font can't be read, we'll try to find it here
            If (normalizedFontname.indexOf("& ") > -1) Then
                normalizedFontname = normalizedFontname.Substring(normalizedFontname.IndexOf("& ") + 1)
            End If
            ' normalize all kinds of fonttypes. There are several possible version which have to be normalized
            ' e.g. Arial,Bold Arial-BoldMT Helevtica-oblique ...
            Dim isBold As Boolean = normalizedFontname.IndexOf("bold") > -1
            Dim isItalic As Boolean = normalizedFontname.IndexOf("italic") > -1 OrElse normalizedFontname.IndexOf("oblique") > -1
            normalizedFontname = normalizedFontname.ToLower().Replace("bold", "").Replace("italic", "").Replace("oblique", "")
            If (isBold) Then
                normalizedFontname &= "bold"
            End If
            If (isItalic) Then
                normalizedFontname &= "italic"
            End If
            Return normalizedFontname
        End Function


        '/**
        ' * Add a font-mapping.
        ' *
        ' * @param font The name of the font.
        ' *
        ' * @param mappedName The name of the mapped font.
        ' * 
        ' */
        Private Shared Function addFontMapping(ByVal font As String, ByVal mappedName As String) As Boolean
            Dim fontname As String = normalizeFontname(font)
            ' is there already a font mapping ?
            If (envFonts.containsKey(fontname)) Then
                Return False
            End If
            Dim mappedFontname As String = normalizeFontname(mappedName)
            ' is the mapped font available ?
            If (Not envFonts.containsKey(mappedFontname)) Then
                Return False
            End If
            envFonts.put(fontname, envFonts.get(mappedFontname))
            Return True
        End Function

        '/**
        ' * Load the mapping for the well knwon font-substitutions.
        ' *
        ' */
        Private Shared Sub loadFontMapping()
            Dim addedMapping As Boolean = True
            ' There could be some recursive mappings in the fontmapping, so that we have to 
            ' read the list until no more additional mapping is added to it 
            While (addedMapping)
                Dim counter As Integer = 0
                'Enumeration(Of Object) keys = fontMapping.keys();
                For Each key As String In fontMapping.keys '   While (keys.hasMoreElements())
                    'String key = (String)keys.nextElement();
                    If (addFontMapping(key, fontMapping.get(key))) Then
                        counter += 1
                    End If
                Next
                If (counter = 0) Then
                    addedMapping = False
                End If
            End While
        End Sub

        '/**
        ' * Mapping for the basefonts.
        ' */
        Private Shared Sub loadBasefontMapping()
            ' use well known substitutions if the environments doesn't provide native fonts for the 14 standard fonts
            ' Times-Roman -> Serif
            If (Not addFontMapping("Times-Roman", "TimesNewRoman")) Then
                addFontMapping("Times-Roman", "Serif")
            End If
            If (Not addFontMapping("Times-Bold", "TimesNewRoman,Bold")) Then
                addFontMapping("Times-Bold", "Serif.bold")
            End If
            If (Not addFontMapping("Times-Italic", "TimesNewRoman,Italic")) Then
                addFontMapping("Times-Italic", "Serif.italic")
            End If
            If (Not addFontMapping("Times-BoldItalic", "TimesNewRoman,Bold,Italic")) Then
                addFontMapping("Times-BoldItalic", "Serif.bolditalic")
            End If
            ' Helvetica -> SansSerif
            If (Not addFontMapping("Helvetica", "Helvetica")) Then
                addFontMapping("Helvetica", "SansSerif")
            End If
            If (Not addFontMapping("Helvetica-Bold", "Helvetica,Bold")) Then
                addFontMapping("Helvetica-Bold", "SansSerif.bold")
            End If
            If (Not addFontMapping("Helvetica-Oblique", "Helvetica,Italic")) Then
                addFontMapping("Helvetica-Oblique", "SansSerif.italic")
            End If
            If (Not addFontMapping("Helvetica-BoldOblique", "Helvetica,Bold,Italic")) Then
                addFontMapping("Helvetica-BoldOblique", "SansSerif.bolditalic")
            End If
            ' Courier -> Monospaced
            If (Not addFontMapping("Courier", "Courier")) Then
                addFontMapping("Courier", "Monospaced")
            End If
            If (Not addFontMapping("Courier-Bold", "Courier,Bold")) Then
                addFontMapping("Courier-Bold", "Monospaced.bold")
            End If
            If (Not addFontMapping("Courier-Oblique", "Courier,Italic")) Then
                addFontMapping("Courier-Oblique", "Monospaced.italic")
            End If
            If (Not addFontMapping("Courier-BoldOblique", "Courier,Bold,Italic")) Then
                addFontMapping("Courier-BoldOblique", "Monospaced.bolditalic")
            End If
            ' some well known (??) substitutions found on fedora linux
            addFontMapping("Symbol", "StandardSymbolsL")
            addFontMapping("ZapfDingbats", "Dingbats")
        End Sub

        '/**
        ' * Try to determine if the font has both a BOLD and an ITALIC-type.
        ' *
        ' * @param name The font.
        ' *
        ' * @return font has BOLD and ITALIC-type or not
        ' */
        Private Shared Function isBoldItalic(ByVal font As JFont)
            Return isBold(font) AndAlso isItalic(font)
        End Function

        '/**
        ' * Try to determine if the font has a BOLD-type.
        ' *
        ' * @param name The font.
        ' *
        ' * @return font has BOLD-type or not
        ' */
        Private Shared Function isBold(ByVal font As JFont) As Boolean
            '{
            '    String name = font.getName().toLowerCase();
            '        If (name.indexOf("bold") > -1) Then
            '    {
            '        return true;
            '    }
            '    String psname = font.getPSName().toLowerCase();
            '            If (psname.IndexOf("bold") > -1) Then
            '    {
            '        return true;
            '    }
            '    return false;
            Return font.IsBold
        End Function

        '/**
        ' * Try to determine if the font has an ITALIC-type.
        ' *
        ' * @param name The font.
        ' *
        ' * @return font has ITALIC-type or not
        ' */
        Private Shared Function isItalic(ByVal font As JFont) As Boolean
            'String name = font.getName().toLowerCase();
            '// oblique is the same as italic
            'if (name.indexOf("italic") > -1 || name.indexOf("oblique") > -1)
            '{
            '    return true;
            '}
            'String psname = font.getPSName().toLowerCase();
            'if (psname.indexOf("italic") > -1 || psname.indexOf("oblique") > -1)
            '{
            '    return true;
            '}
            'return false;
            Return font.isItalic
        End Function
    End Class

End Namespace

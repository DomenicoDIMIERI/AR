Namespace org.apache.pdfbox.util


    '/**
    ' * This class allows a caller to normalize text in various ways. It will load the ICU4J jar file if it is defined on the
    ' * classpath.
    ' * 
    ' * @author <a href="mailto:carrier@digital-evidence.org">Brian Carrier</a>
    ' * 
    ' */
    Public Class TextNormalize

        Private icu4j As ICU4JImpl = Nothing
        Private Shared DIACHASH As HashMap(Of NInteger, String) = New HashMap(Of NInteger, String)()
        Private outputEncoding As String

        Shared Sub New()
            populateDiacHash()
        End Sub

        '/**
        ' * 
        ' * @param encoding The Encoding that the text will eventually be written as (or null)
        ' */
        Public Sub New(ByVal encoding As String)
            findICU4J()
            outputEncoding = encoding
        End Sub

        Private Sub findICU4J()
            ' see if we can load the icu4j classes from the classpath
            Try
                'Me.GetType().getClassLoader().loadClass("com.ibm.icu.text.Bidi")
                'Me.GetType().getClassLoader().loadClass("com.ibm.icu.text.Normalizer")
                icu4j = New ICU4JImpl()
            Catch e As TypeLoadException
                icu4j = Nothing
            End Try
        End Sub

        '/*
        ' * Adds non-decomposing diacritics to the hash with their related combining character. These are values that the
        ' * unicode spec claims are equivalent but are not mapped in the form NFKC normalization method. Determined by going
        ' * through the Combining Diacritical Marks section of the Unicode spec and identifying which characters are not
        ' * mapped to by the normalization.
        ' */
        Private Shared Sub populateDiacHash()
            DIACHASH.put(New NInteger(&H60), ChrW(&H300)) '"\u0300")
            DIACHASH.put(New NInteger(&H2CB), ChrW(&H300)) ' "\u0300")
            DIACHASH.put(New NInteger(&H27), ChrW(&H301)) '"\u0301")
            DIACHASH.put(New NInteger(&H2B9), ChrW(&H301)) '"\u0301")
            DIACHASH.put(New NInteger(&H2CA), ChrW(&H301)) ' "\u0301")
            DIACHASH.put(New NInteger(&H5E), ChrW(&H302)) '"\u0302")
            DIACHASH.put(New NInteger(&H2C6), ChrW(&H302)) '"\u0302")
            DIACHASH.put(New NInteger(&H7E), ChrW(&H303)) '"\u0303")
            DIACHASH.put(New NInteger(&H2C9), ChrW(&H304)) '"\u0304")
            DIACHASH.put(New NInteger(&HB0), ChrW(&H30A)) '"\u030A")
            DIACHASH.put(New NInteger(&H2BA), ChrW(&H30B)) '"\u030B")
            DIACHASH.put(New NInteger(&H2C7), ChrW(&H30C)) '"\u030C")
            DIACHASH.put(New NInteger(&H2C8), ChrW(&H30D)) '"\u030D")
            DIACHASH.put(New NInteger(&H22), ChrW(&H30E)) '"\u030E")
            DIACHASH.put(New NInteger(&H2BB), ChrW(&H312)) '"\u0312")
            DIACHASH.put(New NInteger(&H2BC), ChrW(&H313)) '"\u0313")
            DIACHASH.put(New NInteger(&H486), ChrW(&H313)) ' "\u0313")
            DIACHASH.put(New NInteger(&H55A), ChrW(&H313)) ' "\u0313")
            DIACHASH.put(New NInteger(&H2BD), ChrW(&H314)) ' "\u0314")
            DIACHASH.put(New NInteger(&H485), ChrW(&H314)) '"\u0314")
            DIACHASH.put(New NInteger(&H559), ChrW(&H314)) '"\u0314")
            DIACHASH.put(New NInteger(&H2D4), ChrW(&H31D)) ' "\u031D")
            DIACHASH.put(New NInteger(&H2D5), ChrW(&H31E)) ' "\u031E")
            DIACHASH.put(New NInteger(&H2D6), ChrW(&H31F)) ' "\u031F")
            DIACHASH.put(New NInteger(&H2D7), ChrW(&H320)) '"\u0320")
            DIACHASH.put(New NInteger(&H2B2), ChrW(&H321)) '"\u0321")
            DIACHASH.put(New NInteger(&H2CC), ChrW(&H329)) '"\u0329")
            DIACHASH.put(New NInteger(&H2B7), ChrW(&H32B)) '"\u032B")
            DIACHASH.put(New NInteger(&H2CD), ChrW(&H331)) '"\u0331")
            DIACHASH.put(New NInteger(&H5F), ChrW(&H332)) '"\u0332")
            DIACHASH.put(New NInteger(&H204E), ChrW(&H359)) '"\u0359")
        End Sub

        '/**
        ' * Takes a line of text in presentation order and converts it to logical order. For most text other than Arabic and
        ' * Hebrew, the presentation and logical orders are the same. However, for Arabic and Hebrew, they are different and
        ' * if the text involves both RTL and LTR text then the Unicode BIDI algorithm must be used to determine how to map
        ' * between them.
        ' * 
        ' * @param str Presentation form of line to convert (i.e. left most char is first char)
        ' * @param isRtlDominant true if the PAGE has a dominant right to left ordering
        ' * @return Logical form of string (or original string if ICU4J library is not on classpath)
        ' */
        Public Function makeLineLogicalOrder(ByVal str As String, ByVal isRtlDominant As Boolean) As String
            If (icu4j IsNot Nothing) Then
                Return icu4j.makeLineLogicalOrder(str, isRtlDominant)
            Else
                Return str
            End If
        End Function

        '/**
        ' * Normalize the presentation forms of characters in the string. For example, convert the single "fi" ligature to
        ' * "f" and "i".
        ' * 
        ' * @param str String to normalize
        ' * @return Normalized string (or original string if ICU4J library is not on classpath)
        ' */
        Public Function normalizePres(ByVal str As String) As String
            If (icu4j IsNot Nothing) Then
                Return icu4j.normalizePres(str)
            Else
                Return str
            End If
        End Function


        '/**
        ' * Normalize the diacritic, for example, convert non-combining diacritic characters to their combining counterparts.
        ' * 
        ' * @param str String to normalize
        ' * @return Normalized string (or original string if ICU4J library is not on classpath)
        ' */
        Public Function normalizeDiac(ByVal str As String) As String
            '/*
            ' * Unicode contains special combining forms of the diacritic characters and we want to use these.
            ' */
            If (outputEncoding IsNot Nothing AndAlso outputEncoding.ToUpper().StartsWith("UTF")) Then
                Dim c As New NInteger(Convert.ToInt32(str.Chars(0)))
                ' convert the characters not defined in the Unicode spec
                If (DIACHASH.containsKey(c)) Then
                    Return DIACHASH.get(c)
                ElseIf (icu4j IsNot Nothing) Then
                    Return icu4j.normalizeDiac(str)
                Else
                    Return str
                End If
            Else
                Return str
            End If
        End Function

    End Class

End Namespace

'imports com.ibm.icu.text.Bidi;
'imports com.ibm.icu.text.Normalizer;
Imports FinSeA.Text
Imports System.Text

Namespace org.apache.pdfbox.util


    '/**
    ' * This class is an implementation the the ICU4J class. TextNormalize 
    ' * will call this only if the ICU4J library exists in the classpath.
    ' * @author <a href="mailto:carrier@digital-evidence.org">Brian Carrier</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class ICU4JImpl
        Dim bidi As Bidi

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            bidi = New Bidi()

            '/* We do not use bidi.setInverse() because that uses
            ' * Bidi.REORDER_INVERSE_NUMBERS_AS_L, which caused problems
            ' * in some test files. For example, a file had a line of:
            ' * 0 1 / ARABIC
            ' * and the 0 and 1 were reversed in the end result.  
            ' * REORDER_INVERSE_LIKE_DIRECT is the inverse Bidi mode 
            ' * that more closely reflects the Unicode spec.
            ' */
            bidi.setReorderingMode(bidi.REORDER_INVERSE_LIKE_DIRECT)
        End Sub

        '/**
        ' * Takes a line of text in presentation order and converts it to logical order.
        ' * @see TextNormalize.makeLineLogicalOrder(String, boolean)     
        ' *  
        ' * @param str String to convert
        ' * @param isRtlDominant RTL (right-to-left) will be the dominant text direction
        ' * @return The converted string
        ' */
        Public Function makeLineLogicalOrder(ByVal str As String, ByVal isRtlDominant As Boolean) As String
            bidi.setPara(str, IIf(isRtlDominant, bidi.RTL, bidi.LTR), Nothing)
            '/* Set the mirror flag so that parentheses and other mirror symbols
            '* are properly reversed, when needed.  With this removed, lines
            '* such as (CBA) in the PDF file will come out like )ABC( in logical
            '* order.
            '*/
            Return bidi.writeReordered(bidi.DO_MIRRORING)
        End Function

        '/**
        ' * Normalize presentation forms of characters to the separate parts. 
        ' * @see TextNormalize.normalizePres(String)
        ' * 
        ' * @param str String to normalize
        ' * @return Normalized form
        ' */
        Public Function normalizePres(ByVal str As String) As String
            Dim builder As StringBuilder = Nothing
            Dim p As Integer = 0
            Dim q As Integer = 0
            Dim strLength As Integer = str.Length()
            While (q < strLength)
                '// We only normalize if the codepoint is in a given range.
                '// Otherwise, NFKC converts too many things that would cause
                '// confusion. For example, it converts the micro symbol in
                '// extended Latin to the value in the Greek script. We normalize
                '// the Unicode Alphabetic and Arabic A&B Presentation forms.
                Dim c As Integer = Convert.ToInt16(str.Chars(q))
                If ((&HFB00 <= c AndAlso c <= &HFDFF) OrElse (&HFE70 <= c AndAlso c <= &HFEFF)) Then
                    If (builder Is Nothing) Then
                        builder = New StringBuilder(strLength * 2)
                    End If
                    builder.Append(str.Substring(p, q))
                    '// Some fonts map U+FDF2 differently than the Unicode spec.
                    '// They add an extra U+0627 character to compensate.
                    '// This removes the extra character for those fonts. 
                    If (c = &HFDF2 AndAlso q > 0 AndAlso (Convert.ToInt16(str.Chars(q - 1)) = &H627 OrElse Convert.ToInt16(str.Chars(q - 1)) = &HFE8D)) Then
                        builder.Append(ChrW(&H644) & ChrW(&H644) & ChrW(&H647)) '"\u0644\u0644\u0647")
                    Else
                        '// Trim because some decompositions have an extra space,
                        '// such as U+FC5E
                        builder.Append(Normalizer.normalize(c, Normalizer.NFKC).trim())
                    End If
                    p = q + 1
                End If
                q += 1
            End While
            If (builder Is Nothing) Then
                Return str
            Else
                builder.append(str.Substring(p, q))
                Return builder.toString()
            End If
        End Function

        '/**
        ' * Decomposes Diacritic characters to their combining forms.
        ' * 
        ' * @param str String to be Normalized
        ' * @return A Normalized String
        ' */      
        Public Function normalizeDiac(ByVal str As String) As String
            Dim retStr As New StringBuilder()
            Dim strLength As Integer = str.Length()
            For i As Integer = 0 To strLength - 1
                Dim c As Integer = Convert.ToInt16(str.Chars(i))
                Dim type As Integer = NChar.GetCharType(c)
                If (type = NChar.NON_SPACING_MARK OrElse type = NChar.MODIFIER_SYMBOL OrElse type = NChar.MODIFIER_LETTER) Then
                    '/*
                    ' * Trim because some decompositions have an extra space, such as
                    ' * U+00B4
                    ' */
                    retStr.Append(Normalizer.normalize(c, Normalizer.NFKC).trim())
                Else
                    retStr.Append(str.Chars(i))
                End If
            Next
            Return retStr.ToString()
        End Function

    End Class

End Namespace

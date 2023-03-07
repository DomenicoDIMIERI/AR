Imports FinSeA.org.apache.fontbox.cmap


Namespace org.apache.pdfbox.encoding.conversion

    '/**
    ' *  CJKConverter converts encodings defined in CJKEncodings.
    ' *
    ' *  @author  Pin Xue (http://www.pinxue.net), Holly Lee (holly.lee (at) gmail.com)
    ' *  @version $Revision: 1.0 $
    ' */
    Public Class CJKConverter
        Implements EncodingConverter

        ' The encoding
        Private encodingName As String = ""
        ' The java charset name
        Private charsetName As String = ""

        '/**
        ' *  Constructs a CJKConverter from a PDF encoding name.
        ' *  
        ' *  @param encoding the encoding to be used
        ' */
        Public Sub New(ByVal encoding As String)
            Me.encodingName = encoding
            Me.charsetName = CJKEncodings.getCharset(encoding)
        End Sub

        '/**
        ' *  Convert a string. It occurs when a cmap lookup returned
        ' *  converted bytes successfully, but we still need to convert its
        ' *  encoding. The parameter s is constructs as one byte or a UTF-16BE
        ' *  encoded string.
        ' *
        ' *  Note: pdfbox set string to UTF-16BE charset before calling into
        ' *  Me.
        ' *  
        ' *  {@inheritDoc}
        ' */
        Public Function convertString(ByVal s As String) As String Implements EncodingConverter.convertString
            If (s.Length() = 1) Then Return s
            If (UCase(Me.charsetName) = "UTF-16BE") Then Return s
            Try
                Return Sistema.Strings.GetString(Sistema.Strings.GetBytes(s, "UTF-16BE"), charsetName)
            Catch uee As Exception
                Return s
            End Try
        End Function

        '/**
        ' *  Convert bytes to a string. We just convert bytes within
        ' *  coderange defined in CMap.
        ' *
        ' *  {@inheritDoc}
        ' */
        Public Function convertBytes(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer, ByVal cmap As CMap) As String Implements EncodingConverter.convertBytes
            If (cmap IsNot Nothing) Then
                Try
                    If (cmap.isInCodeSpaceRanges(c, offset, length)) Then
                        Return Sistema.Strings.GetString(c, offset, length, charsetName)
                    Else
                        Return ""
                    End If
                Catch uee As FormatException
                    Return Sistema.Strings.GetString(c, offset, length)
                End Try
            End If
            ' No cmap?
            Return ""
        End Function

    End Class

End Namespace
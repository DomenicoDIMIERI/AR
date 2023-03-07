Imports FinSeA.org.apache.fontbox.cmap


Namespace org.apache.pdfbox.encoding.conversion


    '/**
    ' *  EncodingConverter converts string or characters in one encoding, which is specified in PDF
    ' *  file, to another string with respective java charset. The mapping from
    ' *  PDF encoding name to java charset name is maintained by EncodingConversionManager

    ' *  @author  Pin Xue (http://www.pinxue.net), Holly Lee (holly.lee (at) gmail.com)
    ' *  @version $Revision: 1.0 $
    ' */
    Public Interface EncodingConverter

        '/**
        ' *  Convert a string.
        ' *  
        ' *  @param s the string to be converted
        ' *  @return the converted string
        ' */
        Function convertString(ByVal s As String) As String

        '/**
        ' *  Convert bytes to a string.
        ' *
        ' *  @param c the byte array to be converted
        ' *  @param offset the starting offset of the array
        ' *  @param length the number of bytes
        ' *  @param cmap the cmap to be used for conversion   
        ' *  @return the converted string
        ' */
        Function convertBytes(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer, ByVal cmap As CMap) As String

    End Interface

End Namespace

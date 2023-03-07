Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is a filter for the RunLength Decoder.
    ' *
    ' * From the PDF Reference
    ' * <pre>
    ' * The RunLengthDecode filter decodes data that has been encoded in a simple
    ' * byte-oriented format based on run length. The encoded data is a sequence of
    ' * runs, where each run consists of a length byte followed by 1 to 128 bytes of data. If
    ' * the length byte is in the range 0 to 127, the following length + 1 (1 to 128) bytes
    ' * are copied literally during decompression. If length is in the range 129 to 255, the
    ' * following single byte is to be copied 257 ? length (2 to 128) times during decompression.
    ' * A length value of 128 denotes EOD.
    ' *
    ' * The compression achieved by run-length encoding depends on the input data. In
    ' * the best case (all zeros), a compression of approximately 64:1 is achieved for long
    ' * files. The worst case (the hexadecimal sequence 00 alternating with FF) results in
    ' * an expansion of 127:128.
    ' * </pre>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class RunLengthDecodeFilter
        Implements Filter

        '/**
        '    * Log instance.
        '    */
        'private static final Log log = LogFactory.getLog(RunLengthDecodeFilter.class);

        Private Const RUN_LENGTH_EOD As Integer = 128

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim dupAmount As Integer = -1
            Dim buffer() As Byte
            ReDim buffer(128 - 1)
            dupAmount = compressedData.ReadByte()
            While (dupAmount <> -1 AndAlso dupAmount <> RUN_LENGTH_EOD)
                If (dupAmount <= 127) Then
                    Dim amountToCopy As Integer = dupAmount + 1
                    Dim compressedRead As Integer = 0
                    While (amountToCopy > 0)
                        compressedRead = compressedData.Read(buffer, 0, amountToCopy)
                        result.Write(buffer, 0, compressedRead)
                        amountToCopy -= compressedRead
                    End While
                Else
                    Dim dupByte As Integer = compressedData.ReadByte()
                    For i As Integer = 0 To 257 - dupAmount - 1
                        result.WriteByte(dupByte)
                    Next
                End If
                dupAmount = compressedData.ReadByte()
            End While
        End Sub

        '/**
        '* {@inheritDoc}
        '*/
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Throw New NotImplementedException("RunLengthDecodeFilter.encode is not implemented yet, skipping this stream.")
        End Sub

    End Class

End Namespace

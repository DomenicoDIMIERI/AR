Imports FinSeA.zip
Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the FlateDecode filter.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Marcel Kammer
    ' * @version $Revision: 1.12 $
    ' */
    Public Class FlateFilter
        Implements Filter

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(FlateFilter.class);

        Private Shared BUFFER_SIZE As Integer = 16348

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim baseObj As COSBase = options.getDictionaryObject(COSName.DECODE_PARMS, COSName.DP)
            Dim dict As COSDictionary = Nothing
            If (TypeOf (baseObj) Is COSDictionary) Then
                dict = baseObj
            ElseIf (TypeOf (baseObj) Is COSArray) Then
                Dim [paramArray] As COSArray = baseObj
                If (filterIndex < [paramArray].size()) Then
                    dict = [paramArray].getObject(filterIndex)
                End If
            ElseIf (baseObj IsNot Nothing) Then
                Throw New IOException("Error: Expected COSArray or COSDictionary and not " & baseObj.GetType.Name)
            End If

            Dim predictor As Integer = -1
            Dim colors As Integer = -1
            Dim bitsPerPixel As Integer = -1
            Dim columns As Integer = -1
            Dim bais As ByteArrayInputStream = Nothing '
            Dim baos As ByteArrayOutputStream = Nothing '
            If (dict IsNot Nothing) Then
                predictor = dict.getInt(COSName.PREDICTOR)
                If (predictor > 1) Then
                    colors = dict.getInt(COSName.COLORS)
                    bitsPerPixel = dict.getInt(COSName.BITS_PER_COMPONENT)
                    columns = dict.getInt(COSName.COLUMNS)
                End If
            End If

            Try
                baos = decompress(compressedData)
                ' Decode data using given predictor
                If (predictor = -1 OrElse predictor = 1) Then
                    result.Write(baos.toByteArray)
                Else
                    '/*
                    ' * Reverting back to default values
                    ' */
                    If (colors = -1) Then
                        colors = 1
                    End If
                    If (bitsPerPixel = -1) Then
                        bitsPerPixel = 8
                    End If
                    If (columns = -1) Then
                        columns = 1
                    End If

                    ' Copy data to ByteArrayInputStream for reading
                    bais = New ByteArrayInputStream(baos.toByteArray) '

                    Dim decodedData() As Byte = decodePredictor(predictor, colors, bitsPerPixel, columns, bais)
                    bais.Close()
                    bais = Nothing

                    result.Write(decodedData)
                End If
                result.Flush()
            Catch exception As FormatException
                ' if the stream is corrupt a DataFormatException may occur
                Debug.Print("FlateFilter: stop reading corrupt stream due to a DataFormatException")
                ' re-throw the exception, caller has to handle it
                Dim [io] As New IOException(exception.Message, exception)
                Throw io
            Finally
                If (bais IsNot Nothing) Then
                    bais.Close()
                End If
                If (baos IsNot Nothing) Then
                    baos.Close()
                End If
            End Try
        End Sub

        '// Use Inflater instead of InflateInputStream to avoid an EOFException due to a probably 
        '// missing Z_STREAM_END, see PDFBOX-1232 for details
        Private Function decompress(ByVal [in] As InputStream) As ByteArrayOutputStream
            Dim out As New ByteArrayOutputStream()
            Dim buf() As Byte
            ReDim buf(2048 - 1)
            Dim read As Integer = [in].read(buf, 0, 2048)
            If (read > 0) Then
                Dim inflater As New Inflater()
                inflater.setInput(buf, 0, read)
                Dim res() As Byte
                ReDim res(2048 - 1)
                While (True)
                    Dim resRead As Integer = inflater.inflate(res)
                    If (resRead <> 0) Then
                        out.Write(res, 0, resRead)
                        Continue While
                    End If
                    If (inflater.finished() OrElse inflater.needsDictionary() OrElse [in].available() = 0) Then
                        Exit While
                    End If
                    read = [in].read(buf, 0, 2048)
                    inflater.setInput(buf, 0, read)
                End While
            End If
            out.Close()
            Return out
        End Function

        Private Function decodePredictor(ByVal predictor As Integer, ByVal colors As Integer, ByVal bitsPerComponent As Integer, ByVal columns As Integer, ByVal data As InputStream) As Byte() ' throws IOException
            Dim baos As New ByteArrayOutputStream()
            Dim buffer() As Byte
            ReDim buffer(2048 - 1) '= new byte[2048];
            If (predictor = 1) Then
                ' No prediction
                Dim i As Integer = 0
                i = data.Read(buffer, 0, 2048)
                While (i > 0)
                    baos.Write(buffer, 0, i)
                    i = data.Read(buffer, 0, 2048)
                End While
            Else
                ' calculate sizes
                Dim bitsPerPixel As Integer = colors * bitsPerComponent
                Dim bytesPerPixel As Integer = (bitsPerPixel + 7) / 8
                Dim rowlength As Integer = (columns * bitsPerPixel + 7) / 8
                Dim actline() As Byte
                ReDim actline(rowlength - 1)
                ' Initialize lastline with Zeros according to PNG-specification
                Dim lastline() As Byte
                ReDim lastline(rowlength)

                Dim done As Boolean = False
                Dim linepredictor As Integer = predictor

                While (Not done AndAlso data.available() > 0)
                    ' test for PNG predictor; each value >= 10 (not only 15) indicates usage of PNG predictor
                    If (predictor >= 10) Then
                        ' PNG predictor; each row starts with predictor type (0, 1, 2, 3, 4)
                        linepredictor = data.ReadByte() ' read per line predictor
                        If (linepredictor = -1) Then
                            done = True ' reached EOF
                            Exit While
                        Else
                            linepredictor += 10 'add 10 to tread value 0 as 10, 1 as 11, ...
                        End If
                    End If
                End While

                ' read line
                Dim i As Integer = 0
                Dim offset As Integer = 0
                i = data.Read(actline, offset, rowlength - offset)
                While (offset < rowlength AndAlso (i > 0))
                    offset += i
                    i = data.Read(actline, offset, rowlength - offset)
                End While

                ' Do prediction as specified in PNG-Specification 1.2
                Select Case (linepredictor)
                    Case 2 ' PRED TIFF SUB
                        '/**
                        ' * @TODO decode tiff with bitsPerComponent != 8;
                        ' * e.g. for 4 bpc each nibble must be subtracted separately
                        ' */
                        If (bitsPerComponent <> 8) Then
                            Throw New IOException("TIFF-Predictor with " & bitsPerComponent & " bits per component not supported")
                        End If
                        ' for 8 bits per component it is the same algorithm as PRED SUB of PNG format
                        For p As Integer = 0 To rowlength - 1
                            Dim [sub] = actline(p) And &HFF
                            Dim left As Integer = p - IIf(bytesPerPixel >= 0, actline(p - bytesPerPixel) And &HFF, 0)
                            actline(p) = ([sub] + left)
                        Next
                    Case 10 ' PRED NONE
                        ' do nothing
                    Case 11 ' PRED SUB
                        For p As Integer = 0 To rowlength - 1
                            Dim [sub] As Integer = actline(p)
                            Dim left As Integer = p - IIf(bytesPerPixel >= 0, actline(p - bytesPerPixel), 0)
                            actline(p) = ([sub] + left)
                        Next
                    Case 12 ' PRED UP
                        For p As Integer = 0 To rowlength - 1
                            Dim up As Integer = actline(p) And &HFF
                            Dim prior As Integer = lastline(p) And &HFF
                            actline(p) = ((up + prior) And &HFF)
                        Next
                    Case 13 ' PRED AVG
                        For p As Integer = 0 To rowlength - 1
                            Dim avg As Integer = actline(p) And &HFF
                            Dim left As Integer = p - IIf(bytesPerPixel >= 0, actline(p - bytesPerPixel) And &HFF, 0)
                            Dim up As Integer = lastline(p) And &HFF
                            actline(p) = ((avg + CInt(Math.Floor((left + up) / 2))) And &HFF)
                        Next
                    Case 14 ' PRED PAETH
                        For p As Integer = 0 To rowlength - 1
                            Dim paeth As Integer = actline(p) And &HFF
                            Dim a As Integer = p - IIf(bytesPerPixel >= 0, actline(p - bytesPerPixel) And &HFF, 0) 'left
                            Dim b As Integer = lastline(p) And &HFF ' upper
                            Dim c As Integer = p - IIf(bytesPerPixel >= 0, lastline(p - bytesPerPixel) And &HFF, 0) ' upperleft
                            Dim value As Integer = a + b - c
                            Dim absa As Integer = Math.Abs(value - a)
                            Dim absb As Integer = Math.Abs(value - b)
                            Dim absc As Integer = Math.Abs(value - c)

                            If (absa <= absb AndAlso absa <= absc) Then
                                actline(p) = ((paeth + a) And &HFF)
                            ElseIf (absb <= absc) Then
                                actline(p) = ((paeth + b) And &HFF)
                            Else
                                actline(p) = ((paeth + c) And &HFF)
                            End If
                        Next
                    Case Else
                End Select
                lastline = actline.Clone()
                baos.Write(actline, 0, actline.Length)
            End If
            Return baos.toByteArray
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Dim out As New DeflaterOutputStream(result)
            Dim amountRead As Integer = 0
            Dim mayRead As Integer = rawData.available()
            If (mayRead > 0) Then
                Dim buffer() As Byte
                ReDim buffer(Math.Min(mayRead, BUFFER_SIZE))
                amountRead = rawData.read(buffer, 0, Math.Min(mayRead, BUFFER_SIZE))
                While (amountRead > 0)
                    out.write(buffer, 0, amountRead)
                    amountRead = rawData.read(buffer, 0, Math.Min(mayRead, BUFFER_SIZE))
                End While
            End If
            out.close()
            result.Flush()
        End Sub

    End Class

End Namespace

Imports System.IO
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.Io

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the LZWDecode filter.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.15 $
    ' */
    Public Class LZWFilter
        Implements Filter

        ''' <summary>
        ''' The LZW clear table code.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CLEAR_TABLE As Integer = 256

        ''' <summary>
        ''' The LZW end of data code.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const EOD As Integer = 257

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            'log.debug("decode( )");
            Dim [in] As NBitInputStream = Nothing
            [in] = New NBitInputStream(compressedData)
            [in].setBitsInChunk(9)
            Dim dic As New LZWDictionary()
            Dim firstByte As Byte = 0
            Dim nextCommand As Integer = 0
            nextCommand = [in].read()
            While (nextCommand <> EOD)
                ' log.debug( "decode - nextCommand=" + nextCommand + ", bitsInChunk: " + in.getBitsInChunk());

                If (nextCommand = CLEAR_TABLE) Then
                    [in].setBitsInChunk(9)
                    dic = New LZWDictionary()
                Else
                    Dim data() As Byte = dic.getData(nextCommand)
                    If (data Is Nothing) Then
                        dic.visit(firstByte)
                        data = dic.getData(nextCommand)
                        dic.clear()
                    End If
                    If (data Is Nothing) Then
                        Throw New StreamCorruptedException("Error: data is null")
                    End If
                    dic.visit(data)

                    'log.debug( "decode - dic.getNextCode(): " + dic.getNextCode());

                    If (dic.getNextCode() >= 2047) Then
                        [in].setBitsInChunk(12)
                    ElseIf (dic.getNextCode() >= 1023) Then
                        [in].setBitsInChunk(11)
                    ElseIf (dic.getNextCode() >= 511) Then
                        [in].setBitsInChunk(10)
                    Else
                        [in].setBitsInChunk(9)
                    End If
                    '/**
                    'if( in.getBitsInChunk() != dic.getCodeSize() )
                    '{
                    '    in.unread( nextCommand );
                    '    in.setBitsInChunk( dic.getCodeSize() );
                    '    System.out.print( "Switching " + nextCommand + " to " );
                    '    nextCommand = in.read();
                    '    System.out.println( "" +  nextCommand );
                    '    data = dic.getData( nextCommand );
                    '}**/
                    firstByte = data(0)
                    result.Write(data, 0, 1 + UBound(data))
                End If
                nextCommand = [in].read()
            End While
            result.Flush()
        End Sub


        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            'log.debug("encode( )");
            Dim input As New FinSeA.Io.PushBackInputStream(rawData, 4096)
            Dim dic As New LZWDictionary()
            Dim out As New NBitOutputStream(result)
            out.setBitsInChunk(9) 'initially nine
            out.write(CLEAR_TABLE)
            Dim buffer As New ByteArrayOutputStream()
            Dim byteRead As Integer = 0
            Dim i As Integer = 0
            byteRead = input.read()
            While (byteRead > 0)
                'log.debug( "byteRead = '" + (char)byteRead + "' (0x" + Integer.toHexString(byteRead) + "), i=" + i);
                buffer.Write(byteRead)
                dic.visit(byteRead)
                out.setBitsInChunk(dic.getCodeSize())

                'log.debug( "Getting node '" + new String( buffer.toByteArray() ) + "', buffer.size = " + buffer.size() );
                Dim node As LZWNode = dic.getNode(buffer.toByteArray())
                Dim nextByte As Integer = input.read()
                If (nextByte <> -1) Then
                    'log.debug( "nextByte = '" + (char)nextByte + "' (0x" + Integer.toHexString(nextByte) + ")");
                    Dim [next] As LZWNode = node.getNode(nextByte)
                    If ([next] Is Nothing) Then
                        'log.debug("encode - No next node, writing node and resetting buffer (" +
                        '          " node.getCode: " + node.getCode() + ")" +
                        '          " bitsInChunk: " + out.getBitsInChunk() +
                        '          ")");
                        out.write(node.getCode())
                        buffer.Position = 0
                    End If

                    input.unread(nextByte)
                Else
                    'log.debug("encode - EOF on lookahead: writing node, resetting buffer, and terminating read loop (" +
                    '          " node.getCode: " + node.getCode() + ")" +
                    '          " bitsInChunk: " + out.getBitsInChunk() +
                    '          ")");
                    out.write(node.getCode())
                    buffer.Position = 0
                    Exit While
                End If

                If (dic.getNextCode() = 4096) Then
                    'log.debug("encode - Clearing dictionary and unreading pending buffer data (" +
                    '          " bitsInChunk: " + out.getBitsInChunk() +
                    '          ")");
                    out.write(CLEAR_TABLE)
                    dic = New LZWDictionary()
                    input.unread(buffer.toByteArray())
                    buffer.Position = 0
                End If
                i += 1
                byteRead = input.read()
            End While

            '// Fix the code size based on the fact that we are writing the EOD
            '//
            If (dic.getNextCode() >= 2047) Then
                out.setBitsInChunk(12)
            ElseIf (dic.getNextCode() >= 1023) Then
                out.setBitsInChunk(11)
            ElseIf (dic.getNextCode() >= 511) Then
                out.setBitsInChunk(10)
            Else
                out.setBitsInChunk(9)
            End If

            '//log.debug("encode - Writing EOD (" +
            '//          " bitsInChunk: " + out.getBitsInChunk() +
            '//          ")");
            out.write(EOD)
            out.close()
            result.Flush()
        End Sub

    End Class

End Namespace

'/*
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' *
' *      http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' */

'import java.awt.Graphics;
'import java.awt.image.BufferedImage;
'import java.awt.image.DataBuffer;
'import java.awt.image.DataBufferByte;
'import java.io.IOException;
'import java.io.InputStream;
'import java.io.OutputStream;
'import java.io.SequenceInputStream;
'import java.util.Iterator;

'import javax.imageio.ImageIO;
'import javax.imageio.ImageReader;

'import org.apache.commons.logging.Log;
'import org.apache.commons.logging.LogFactory;
'import org.apache.pdfbox.cos.COSDictionary;
'import org.apache.pdfbox.cos.COSInteger;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.cos.COSStream;

Imports System.IO
Imports FinSeA.Io
Imports System.Drawing
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Drawings

Namespace org.apache.pdfbox.filter

    '/**
    ' * Modeled on the JBIG2Decode filter.
    ' *
    ' * thanks to Timo Boehme <timo.boehme@ontochem.com>
    ' */

    Public Class JBIG2Filter
        Implements Filter

        '/** Log instance. */
        'private static final Log LOG = LogFactory.getLog(JBIG2Filter.class);

        '/**
        ' * Decode JBIG2 data using Java ImageIO library.
        ' *
        ' * {@inheritDoc}
        ' *
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            '/**
            ' *  A working JBIG2 ImageIO plugin is needed to decode JBIG2 encoded streams.
            ' *  The following is known to be working. It can't be bundled with PDFBox because of an incompatible license.
            ' *  http://code.google.com/p/jbig2-imageio/ 
            ' */
            Dim readers As Iterator(Of ImageReader) = ImageIO.getImageReadersByFormatName("JBIG2") 'Iterator<ImageReader> 
            If (Not readers.hasNext()) Then
                Debug.Print("Can't find an ImageIO plugin to decode the JBIG2 encoded datastream.")
                Return
            End If
            Dim reader As ImageReader = readers.next()
            Dim decodeP As COSDictionary = options.getDictionaryObject(COSName.DECODE_PARMS)
            Dim bits As COSInteger = options.getDictionaryObject(COSName.BITS_PER_COMPONENT)
            Dim st As COSStream = Nothing
            If (decodeP IsNot Nothing) Then
                st = decodeP.getDictionaryObject(COSName.JBIG2_GLOBALS)
            End If
            If (st IsNot Nothing) Then
                reader.setInput(ImageIO.createImageInputStream(New SequenceInputStream(st.getFilteredStream(), compressedData)))
            Else
                reader.setInput(ImageIO.createImageInputStream(compressedData))
            End If
            Dim bi As BufferedImage = reader.read(0)
            reader.Dispose()
            If (bi IsNot Nothing) Then
                '// I am assuming since JBIG2 is always black and white 
                '// depending on your renderer this might or might be needed
                If (bi.getColorModel().getPixelSize() <> bits.intValue()) Then
                    If (bits.intValue() <> 1) Then
                        Debug.Print("Do not know how to deal with JBIG2 with more than 1 bit")
                        Return
                    End If
                    Dim packedImage As New BufferedImage(bi.getWidth, bi.getHeight, BufferedImage.TYPE_BYTE_BINARY)
                    Dim graphics As Graphics2D = Graphics2D.FromImage(packedImage)
                    graphics.drawImage(bi, 0, 0, Nothing)
                    graphics.dispose()
                    bi = packedImage
                End If
                Dim dBuf As DataBufferByte = bi.getData().getDataBuffer()
                If (dBuf.getDataType() = DataBuffer.TYPE_BYTE) Then
                    result.Write(dBuf.getData())
                Else
                    Debug.Print("Image data buffer not of type byte but type " & dBuf.getDataType())
                End If
            Else
                Debug.Print("Something went wrong when decoding the JBIG2 encoded datastream.")
            End If
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Throw New NotImplementedException("JBIG2.encode is not implemented yet, skipping this stream.")
        End Sub

    End Class

End Namespace


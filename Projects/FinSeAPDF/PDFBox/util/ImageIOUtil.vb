Imports System.IO
Imports FinSeA.Drawings

Namespace org.apache.pdfbox.util



    '/**
    ' * This class handles some ImageIO operations.
    ' *
    ' * @version $Revision$
    ' * 
    ' */
    Public Class ImageIOUtil

        ''' <summary>
        ''' Default screen resolution: 72dpi.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_SCREEN_RESOLUTION As Integer = 72

        ''' <summary>
        ''' Default compression quality: 1.0f.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_COMPRESSION_QUALITY As Single = 1.0F

        Private Sub New()
            ' Default constructor
        End Sub

        '/**
        ' * Writes a buffered image to a file using the given image format.
        ' * 
        ' * @param image the image to be written
        ' * @param imageFormat the target format (ex. "png")
        ' * @param filename used to construct the filename for the individual images
        ' * @param imageType the image type (see {@link BufferedImage}.TYPE_*)
        ' * @param resolution the resolution in dpi (dots per inch)
        ' * 
        ' * @return true if the images were produced, false if there was an error
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function writeImage(ByVal image As System.Drawing.Image, ByVal imageFormat As String, ByVal filename As String, ByVal imageType As Integer, ByVal resolution As Integer) As Boolean 'throws IOException
            Dim strm As New System.IO.FileStream(filename & "." & imageFormat, FileMode.Create, FileAccess.Write)
            Dim ret As Boolean = writeImage(image, imageFormat, strm, resolution)
            strm.Dispose()
            Return ret
        End Function

        '/**
        ' * Writes a buffered image to a file using the given image format.
        ' * 
        ' * @param image the image to be written
        ' * @param imageFormat the target format (ex. "png")
        ' * @param outputStream the output stream to be used for writing
        ' * 
        ' * @return true if the images were produced, false if there was an error
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function writeImage(ByVal image As System.Drawing.Image, ByVal imageFormat As String, ByVal outputStream As Stream) As Boolean 'throws IOException
            Return writeImage(image, imageFormat, outputStream, DEFAULT_SCREEN_RESOLUTION)
        End Function

        '/**
        ' * Writes a buffered image to a file using the given image format.
        ' * 
        ' * @param image the image to be written
        ' * @param imageFormat the target format (ex. "png")
        ' * @param outputStream the output stream to be used for writing
        ' * @param resolution resolution to be used when writing the image
        ' * 
        ' * @return true if the images were produced, false if there was an error
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function writeImage(ByVal image As System.Drawing.Image, ByVal imageFormat As String, ByVal outputStream As Stream, ByVal resolution As Integer) As Boolean  'throws IOException
            Return writeImage(image, imageFormat, outputStream, resolution, DEFAULT_COMPRESSION_QUALITY)
        End Function

        '/**
        ' * Writes a buffered image to a file using the given image format.
        ' * 
        ' * @param image the image to be written
        ' * @param imageFormat the target format (ex. "png")
        ' * @param outputStream the output stream to be used for writing
        ' * @param resolution resolution to be used when writing the image
        ' * @param quality quality to be used when compressing the image (0 < quality < 1.0f)
        ' * 
        ' * @return true if the images were produced, false if there was an error
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Shared Function writeImage(ByVal image As System.Drawing.Image, ByVal imageFormat As String, ByVal outputStream As Stream, ByVal resolution As Integer, ByVal quality As Single) As Boolean ' throws IOException
            Select Case LCase(Trim(imageFormat))
                Case "bmp" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Bmp)
                Case "emf" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Emf)
                Case "jfif" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Exif)
                Case "gif" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Gif)
                Case "ico" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Icon)
                Case "jpg", "jpeg" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                Case "png" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Png)
                Case "tif", "tiff" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Tiff)
                Case "wmf" : image.Save(outputStream, System.Drawing.Imaging.ImageFormat.Wmf)
                Case Else : Throw New NotSupportedException("Formato non supportato: " & imageFormat)
            End Select
            Return True
            '    Dim bSuccess As Boolean = True
            '    Dim output As Stream = Nothing
            '    Dim imageWriter As StreamWriter = Nothing 'ImageWriter  = null;
            '    Try
            '        output = ImageIO.createImageOutputStream(outputStream)
            '        Dim foundWriter As Boolean = False
            '    Iterator<ImageWriter> writerIter = ImageIO.getImageWritersByFormatName( imageFormat );
            '    while( writerIter.hasNext() && !foundWriter )
            '    {
            '            Try
            '        {
            '            imageWriter = (ImageWriter)writerIter.next();
            '            ImageWriteParam writerParams = imageWriter.getDefaultWriteParam();
            '                If (writerParams.canWriteCompressed()) Then
            '            {
            '                writerParams.setCompressionMode(ImageWriteParam.MODE_EXPLICIT);
            '                // reset the compression type if overwritten by setCompressionMode
            '                if (writerParams.getCompressionType() Is Nothing)
            '                {
            '                    writerParams.setCompressionType(writerParams.getCompressionTypes()(0));
            '                }
            '                writerParams.setCompressionQuality(quality);
            '            }
            '            IIOMetadata meta = createMetadata( image, imageWriter, writerParams, resolution);
            '            imageWriter.setOutput( output );
            '            imageWriter.write( null, new IIOImage( image, null, meta ), writerParams );
            '            foundWriter = true;
            '        }
            '        catch( IIOException io )
            '        {
            '            throw new IOException( io.getMessage() );
            '        }
            '            Finally
            '        {
            '            if( imageWriter IsNot Nothing )
            '            {
            '                imageWriter.dispose();
            '            }
            '        }
            '    }
            '                    If (!foundWriter) Then
            '    {
            '        bSuccess = false;
            '    }
            '}
            'finally
            '{
            '    if( output IsNot Nothing )
            '    {
            '        output.flush();
            '        output.close();
            '    }
            '}
            'return bSuccess;
        End Function

        'Private Shared Function createMetadata(ByVal image As System.Drawing.Image, ByVal imageWriter As Stream, ByVal writerParams As ImageWriteParam, ByVal resolution As Integer) As IIOMetadata
        '    Dim type As ImageTypeSpecifier
        '    If (writerParams.getDestinationType() IsNot Nothing) Then
        '        type = writerParams.getDestinationType()
        '    Else
        '        type = ImageTypeSpecifier.createFromRenderedImage(image)
        '    End If
        '    Dim meta As IIOMetadata = ImageWriter.getDefaultImageMetadata(type, writerParams)
        '    Return IIf(addResolution(meta, resolution), meta, Nothing)
        'End Function

        'Private Const STANDARD_METADATA_FORMAT As String = "javax_imageio_1.0"

        'Private Shared Function addResolution(ByVal meta As IIOMetadata, ByVal resolution As Integer) As Boolean
        '    If (Not meta.isReadOnly() AndAlso meta.isStandardMetadataFormatSupported()) Then
        '        Dim root As IIOMetadataNode = meta.getAsTree(STANDARD_METADATA_FORMAT)
        '        Dim [dim] As IIOMetadataNode = getChildNode(root, "Dimension")
        '        If ([dim] Is Nothing) Then
        '            [dim] = New IIOMetadataNode("Dimension")
        '        root.appendChild(dim)
        '        End If
        '        Dim child As IIOMetadataNode
        '        child = getChildNode([dim], "HorizontalPixelSize")
        '        If (child Is Nothing) Then
        '            child = New IIOMetadataNode("HorizontalPixelSize")
        '            [dim].appendChild(child)
        '        End If
        '        child.setAttribute("value", Double.ToString(resolution / 25.4))
        '        child = getChildNode([dim], "VerticalPixelSize")
        '        If (child Is Nothing) Then
        '            child = New IIOMetadataNode("VerticalPixelSize")
        '            [dim].appendChild(child)
        '        End If
        '        child.setAttribute("value", Double.ToString(resolution / 25.4))
        '        Try
        '            meta.mergeTree(STANDARD_METADATA_FORMAT, root)
        '        Catch e As Exception
        '            Throw New Exception("Cannot update image metadata: " & e.Message(), e)
        '            Return True
        '        End Try
        '    End If
        '    Return False
        'End Function

        'Private Shared Function getChildNode(ByVal n As Node, ByVal name As String) As IIOMetadataNode
        '    Dim nodes As NodeList = n.getChildNodes()
        '    For i As Integer = 0 To nodes.getLength() - 1
        '        Dim child As Node = nodes.item(i)
        '        If (name.Equals(child.getNodeName())) Then
        '            Return child
        '        End If
        '    Next
        '    Return Nothing
        'End Function

        Shared Function getImageReaders(input As ImageInputStream) As Object
            Throw New NotImplementedException
        End Function


    End Class

End Namespace

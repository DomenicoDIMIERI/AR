Namespace Drawings

    Public Class ImageIO

        Shared Function read(fis As Io.InputStream) As BufferedImage
            Throw New NotImplementedException
        End Function

        Shared Function createImageInputStream(byteArrayInputStream As Io.ByteArrayInputStream) As Object
            Throw New NotImplementedException
        End Function

        Shared Function getImageReaders(input As ImageInputStream) As IEnumerator(Of ImageReader)
            Throw New NotImplementedException
        End Function

        Shared Function getImageReadersByFormatName(p1 As String) As IEnumerator(Of ImageReader)
            Throw New NotImplementedException
        End Function

        Shared Function createImageInputStream(sequenceInputStream As Io.SequenceInputStream) As ImageInputStream
            Throw New NotImplementedException
        End Function

        Shared Function createImageInputStream(compressedData As Io.InputStream) As ImageInputStream
            Throw New NotImplementedException
        End Function


    End Class

End Namespace
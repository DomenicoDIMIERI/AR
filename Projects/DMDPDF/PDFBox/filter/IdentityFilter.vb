Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.filter

    '/**
    ' * The IdentityFilter filter just passes the data through without any modifications.
    ' * This is defined in section 7.6.5 of the PDF 1.7 spec and also stated in table
    ' * 26.
    ' * 
    ' * @author adam.nichols
    ' */
    Public Class IdentityFilter
        Implements Filter

        Private Shared BUFFER_SIZE As Integer = 1024

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim buffer() As Byte
            ReDim buffer(BUFFER_SIZE - 1)
            Dim amountRead As Integer
            amountRead = compressedData.read(buffer, 0, BUFFER_SIZE)
            While (amountRead > 0)
                result.Write(buffer, 0, amountRead)
                amountRead = compressedData.read(buffer, 0, BUFFER_SIZE)
            End While
            result.Flush()
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Dim buffer() As Byte
            ReDim buffer(BUFFER_SIZE - 1)
            Dim amountRead As Integer
            amountRead = rawData.read(buffer, 0, BUFFER_SIZE)
            While (amountRead > 0)
                result.Write(buffer, 0, amountRead)
                amountRead = rawData.read(buffer, 0, BUFFER_SIZE)
            End While
            result.Flush()
        End Sub

    End Class

End Namespace
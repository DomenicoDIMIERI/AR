Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Io


Namespace org.apache.pdfbox.filter

    '/**
    ' *
    ' * @author adam.nichols
    ' */
    Public Class CryptFilter
        Implements Filter

        '/**
        '    * {@inheritDoc}
        '    */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim encryptionName As COSName = options.getDictionaryObject(COSName.NAME)
            If (encryptionName Is Nothing OrElse encryptionName.equals(COSName.IDENTITY)) Then
                ' currently the only supported implementation is the Identity crypt filter
                Dim identityFilter As Filter = New IdentityFilter()
                identityFilter.decode(compressedData, result, options, filterIndex)
            Else
                Throw New IOException("Unsupported crypt filter " & encryptionName.getName())
            End If
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Dim encryptionName As COSName = options.getDictionaryObject(COSName.NAME)
            If (encryptionName Is Nothing OrElse encryptionName.equals(COSName.IDENTITY)) Then
                ' currently the only supported implementation is the Identity crypt filter
                Dim identityFilter As Filter = New IdentityFilter()
                identityFilter.encode(rawData, result, options, filterIndex)
            Else
                Throw New IOException("Unsupported crypt filter " & encryptionName.getName())
            End If
        End Sub

    End Class

End Namespace


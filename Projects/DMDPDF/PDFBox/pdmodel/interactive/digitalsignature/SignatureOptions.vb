Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    Public Class SignatureOptions

        Private visualSignature As COSDocument

        Private preferedSignatureSize As Integer

        Private pageNo As Integer

        '/**
        ' * Set the page number.
        ' * 
        ' * @param pageNo the page number
        ' * 
        ' */
        Public Sub setPage(ByVal pageNo As Integer)
            Me.pageNo = pageNo
        End Sub

        '/**
        ' * Get the page number.
        ' * 
        ' * @return the page number
        ' */
        Public Function getPage() As Integer
            Return pageNo
        End Function

        '/**
        ' * Reads the visual signature from the given input stream.
        ' *  
        ' * @param is the input stream containing the visual signature
        ' * 
        ' * @throws IOException when something went wrong during parsing 
        ' */
        Public Sub setVisualSignature(ByVal [is] As InputStream)  'throws IOException
            Dim visParser As VisualSignatureParser = New VisualSignatureParser([is])
            visParser.parse()
            visualSignature = visParser.getDocument()
        End Sub

        '/**
        ' * Reads the visual signature from the given visual signature properties
        ' *  
        ' * @param is the <code>PDVisibleSigProperties</code> object containing the visual signature
        ' * 
        ' * @throws IOException when something went wrong during parsing
        ' * 
        ' * @since 1.8.3
        ' */
        Public Sub setVisualSignature(ByVal visSignatureProperties As PDVisibleSigProperties)  'throws IOException
            setVisualSignature(visSignatureProperties.getVisibleSignature())
        End Sub

        '/**
        ' * Get the visual signature.
        ' * 
        ' * @return the visual signature
        ' */
        Public Function getVisualSignature() As COSDocument
            Return visualSignature
        End Function

        '/**
        ' * Get the preferred size of the signature.
        ' * 
        ' * @return the preferred size
        ' */
        Public Function getPreferedSignatureSize() As Integer
            Return preferedSignatureSize
        End Function

        '/**
        ' * Set the preferred size of the signature.
        ' * 
        ' * @param size the size of the signature
        ' */
        Public Sub setPreferedSignatureSize(ByVal size As Integer)
            If (size > 0) Then
                preferedSignatureSize = size
            End If
        End Sub

    End Class

End Namespace

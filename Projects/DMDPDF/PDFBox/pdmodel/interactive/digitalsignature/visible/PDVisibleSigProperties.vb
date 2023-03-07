Imports System.IO
Imports FinSeA.Io

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

    '/**
    ' * This builder class is in order to create visible signature properties.
    ' * 
    ' * @author <a href="mailto:vakhtang.koroghlishvili@gmail.com"> vakhtang koroghlishvili (gogebashvili) </a>
    ' * 
    ' */
    Public Class PDVisibleSigProperties

        Private _signerName As String
        Private _signerLocation As String
        Private _signatureReason As String
        Private _visualSignEnabled As Boolean
        Private _page As Integer
        Private _preferredSize As Integer

        Private visibleSignature As InputStream
        Private pdVisibleSignature As PDVisibleSignDesigner

        '/**
        ' * start building of visible signature
        ' * 
        ' * @throws IOException
        ' */
        Public Sub buildSignature()  'throws IOException
            Dim builder As New PDVisibleSigBuilder()
            Dim creator As New PDFTemplateCreator(builder)
            setVisibleSignature(creator.buildPDF(getPdVisibleSignature()))
        End Sub

        '/**
        ' * 
        ' * @return - signer name
        ' */
        Public Function getSignerName() As String
            Return _signerName
        End Function

        '/**
        ' * Sets signer name
        ' * @param _signerName
        ' * @return
        ' */
        Public Function signerName(ByVal value As String) As PDVisibleSigProperties
            Me._signerName = value
            Return Me
        End Function

        '/**
        ' * Gets signer locations
        ' * @return - location
        ' */
        Public Function getSignerLocation() As String
            Return _signerLocation
        End Function

        '/**
        ' * Sets location
        ' * @param _signerLocation
        ' * @return
        ' */
        Public Function signerLocation(ByVal value As String) As PDVisibleSigProperties
            Me._signerLocation = value
            Return Me
        End Function

        '/**
        ' * gets reason of signing
        ' * @return 
        ' */
        Public Function getSignatureReason() As String
            Return _signatureReason
        End Function

        '/**
        ' * sets reason of signing
        ' * @param _signatureReason
        ' * @return
        ' */
        Public Function signatureReason(ByVal value As String) As PDVisibleSigProperties
            Me._signatureReason = _signatureReason
            Return Me
        End Function

        '/**
        ' * returns your _page
        ' * @return 
        ' */
        Public Function getPage() As Integer
            Return _page
        End Function

        '/**
        ' * sets _page number
        ' * @param _page
        ' * @return
        ' */
        Public Function page(ByVal value As Integer) As PDVisibleSigProperties
            Me._page = value
            Return Me
        End Function

        '/**
        ' * gets our preferred size
        ' * @return
        ' */
        Public Function getPreferredSize() As Integer
            Return _preferredSize
        End Function

        '/**
        ' * sets our preferred size
        ' * @param _preferredSize
        ' * @return
        ' */
        Public Function preferredSize(ByVal value As Integer) As PDVisibleSigProperties
            Me._preferredSize = value
            Return Me
        End Function

        '/**
        ' * checks if we need to add visible signature
        ' * @return
        ' */
        Public Function isVisualSignEnabled() As Boolean
            Return _visualSignEnabled
        End Function

        '/**
        ' * sets visible signature to be added or not
        ' * @param _visualSignEnabled
        ' * @return
        ' */
        Public Function visualSignEnabled(ByVal value As Boolean) As PDVisibleSigProperties
            Me._visualSignEnabled = value
            Return Me
        End Function

        '/**
        ' * Me method gets visible signature configuration object
        ' * @return
        ' */
        Public Function getPdVisibleSignature() As PDVisibleSignDesigner
            Return pdVisibleSignature
        End Function

        '/**
        ' * Sets visible signature configuration Object
        ' * @param pdVisibleSignature
        ' * @return
        ' */
        Public Function setPdVisibleSignature(ByVal pdVisibleSignature As PDVisibleSignDesigner) As PDVisibleSigProperties
            Me.pdVisibleSignature = pdVisibleSignature
            Return Me
        End Function

        '/**
        ' * returns visible signature configuration object
        ' * @return
        ' */
        Public Function getVisibleSignature() As InputStream
            Return visibleSignature
        End Function

        '/**
        ' * sets configuration object of visible signature
        ' * @param visibleSignature
        ' */
        Public Sub setVisibleSignature(ByVal visibleSignature As InputStream)
            Me.visibleSignature = visibleSignature
        End Sub

    End Class

End Namespace

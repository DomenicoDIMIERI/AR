Imports FinSeA.Sistema
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.pdmodel
'Imports FinSeA.org.bouncycastle.util
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

    '/**
    ' * 
    ' * That class is in order to build  your 
    ' * visible signature design. Because of 
    ' * this is builder, instead of setParam()
    ' * we use param() methods.
    ' * @author <a href="mailto:vakhtang.koroghlishvili@gmail.com"> vakhtang koroghlishvili (gogebashvili) </a>
    ' */
    Public Class PDVisibleSignDesigner

        Private sigImgWidth As NFloat
        Private sigImgHeight As NFloat
        Private _xAxis As Single
        Private _yAxis As Single
        Private _pageHeight As Single
        Private _pageWidth As Single
        Private imgageStream As InputStream
        Private _signatureFieldName As String = "sig" ' default
        Private _formaterRectangleParams() As Byte = {0, 0, 100, 50} ' default
        Private _affineTransformParams() As Byte = {1, 0, 0, 1, 0, 0} ' default
        Private _imageSizeInPercents As Single
        Private document As PDDocument = Nothing



        '/**
        ' * 
        ' * @param originalDocumenStream
        ' * @param imageStream
        ' * @param page-which page are you going to add visible signature
        ' * @throws IOException
        ' */
        Public Sub New(ByVal originalDocumenStream As InputStream, ByVal imageStream As InputStream, ByVal page As Integer) 'throws IOException
            signatureImageStream(imageStream)
            document = PDDocument.load(originalDocumenStream)
            calculatePageSize(document, page)
        End Sub

        '/**
        ' * 
        ' * @param documentPath - path of your pdf document
        ' * @param imageStream - stream of image
        ' * @param page -which page are you going to add visible signature
        ' * @throws IOException
        ' */
        Public Sub New(ByVal documentPath As String, ByVal imageStream As InputStream, ByVal page As Integer) 'throws IOException
            ' set visible singature image Input stream
            signatureImageStream(imageStream)

            ' create PD document
            document = PDDocument.load(documentPath)

            ' calculate height an width of document
            calculatePageSize(document, page)

            document.close()
        End Sub

        '/**
        ' * 
        ' * @param doc - Already created PDDocument of your PDF document
        ' * @param imageStream
        ' * @param page
        ' * @throws IOException - If we can't read, flush, or can't close stream
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal imageStream As InputStream, ByVal page As Integer)  'throws IOException 
            signatureImageStream(imageStream)
            calculatePageSize(doc, page)
        End Sub

        '/**
        ' * Each page of document can be different sizes.
        ' * 
        ' * @param document
        ' * @param page
        ' */
        Private Sub calculatePageSize(ByVal document As PDDocument, ByVal page As Integer)
            If (page < 1) Then
                Throw New ArgumentOutOfRangeException("First page of pdf is 1, not " & page)
            End If

            Dim pages As List(Of PDPage) = document.getDocumentCatalog().getAllPages() 'List<?>
            Dim firstPage As PDPage = pages.get(page - 1)
            Dim mediaBox As PDRectangle = firstPage.findMediaBox()
            Me._pageHeight = mediaBox.getHeight()
            Me._pageWidth = mediaBox.getWidth()

            Dim x As Single = Me._pageWidth
            Dim y As Single = 0
            Me._pageWidth = Me._pageWidth + y
            Dim tPercent As Single = (100 * y / (x + y))
            Me._imageSizeInPercents = 100 - tPercent
        End Sub


        '/**
        ' * 
        ' * @param path  of image location
        ' * @return image Stream
        ' * @throws IOException
        ' */
        Public Function signatureImage(ByVal path As String) As PDVisibleSignDesigner 'throws IOException
            Dim fin As InputStream = New FileInputStream(path)
            Return signatureImageStream(fin)
        End Function

        '/**
        ' * zoom signature image with some percent.
        ' * 
        ' * @param percent- x % increase image with x percent.
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function zoom(ByVal percent As Single) As PDVisibleSignDesigner
            sigImgHeight = sigImgHeight + (sigImgHeight * percent) / 100
            sigImgWidth = sigImgWidth + (sigImgWidth * percent) / 100
            Return Me
        End Function

        '/**
        ' * 
        ' * @param _xAxis - x coordinate 
        ' * @param _yAxis - y coordinate
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function coordinates(ByVal x As Single, ByVal y As Single) As PDVisibleSignDesigner
            xAxis(x)
            yAxis(y)
            Return Me
        End Function

        '/**
        ' * 
        ' * @return _xAxis - gets x coordinates
        ' */
        Public Function getxAxis() As Single
            Return _xAxis
        End Function

        '/**
        ' * 
        ' * @param _xAxis  - x coordinate 
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function xAxis(ByVal value As Single) As PDVisibleSignDesigner
            Me._xAxis = value
            Return Me
        End Function

        '/**
        ' * 
        ' * @return _yAxis
        ' */
        Public Function getyAxis() As Single
            Return _yAxis
        End Function

        '/**
        ' * 
        ' * @param _yAxis
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function yAxis(ByVal value As Single) As PDVisibleSignDesigner
            Me._yAxis = value
            Return Me
        End Function

        '/**
        ' * 
        ' * @return signature image width
        ' */
        Public Function getWidth() As Single
            Return sigImgWidth
        End Function

        '/**
        ' * 
        ' * @param sets signature image width
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function width(ByVal signatureImgWidth As Single) As PDVisibleSignDesigner
            Me.sigImgWidth = signatureImgWidth
            Return Me
        End Function

        '/**
        ' * 
        ' * @return signature image height
        ' */
        Public Function getHeight() As Single
            Return sigImgHeight
        End Function

        '/**
        ' * 
        ' * @param set signature image Height
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function height(ByVal signatureImgHeight As Single) As PDVisibleSignDesigner
            Me.sigImgHeight = signatureImgHeight
            Return Me
        End Function

        '/**
        ' * 
        ' * @return template height
        ' */
        Protected Friend Function getTemplateHeight() As Single
            Return getPageHeight()
        End Function

        '/**
        ' * 
        ' * @param templateHeight
        ' * @return Visible Signature Configuration Object
        ' */
        Private Function pageHeight(ByVal templateHeight As Single) As PDVisibleSignDesigner
            Me._pageHeight = templateHeight
            Return Me
        End Function

        '/**
        ' * 
        ' * @return signature field name
        ' */
        Public Function getSignatureFieldName() As String
            Return _signatureFieldName
        End Function

        '/**
        ' * 
        ' * @param _signatureFieldName
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function signatureFieldName(ByVal value As String) As PDVisibleSignDesigner
            Me._signatureFieldName = value
            Return Me
        End Function

        '/**
        ' * 
        ' * @return image Stream
        ' */
        Public Function getImageStream() As InputStream
            Return imgageStream
        End Function

        '/**
        ' * 
        ' * @param imgageStream- stream of your visible signature image
        ' * @return Visible Signature Configuration Object
        ' * @throws IOException - If we can't read, flush, or close stream of image
        ' */
        Private Function signatureImageStream(ByVal imageStream As InputStream) As PDVisibleSignDesigner 'throws IOException 
            Dim baos As ByteArrayOutputStream = New ByteArrayOutputStream()
            Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
            Dim len As Integer
            len = imageStream.read(buffer)
            While (len > 0)
                baos.Write(buffer, 0, len)
                len = imageStream.read(buffer)
            End While
            baos.Flush()
            baos.Close()

            Dim byteArray() As Byte = baos.toByteArray()
            Dim byteArraySecond() As Byte = Arrays.Clone(byteArray)

            Dim inputForBufferedImage As InputStream = New ByteArrayInputStream(byteArray)
            Dim revertInputStream As InputStream = New ByteArrayInputStream(byteArraySecond)

            If (sigImgHeight.HasValue = False OrElse sigImgWidth.HasValue = False) Then
                calcualteImageSize(inputForBufferedImage)
            End If

            Me.imgageStream = revertInputStream

            Return Me
        End Function

        '/**
        ' * calculates image width and height. sported formats: all
        ' * 
        ' * @param fis - input stream of image
        ' * @throws IOException - if can't read input stream
        ' */
        Private Sub calcualteImageSize(ByVal fis As InputStream)  'throws IOException 
            Dim bimg As BufferedImage = ImageIO.read(fis)
            Dim width As Integer = bimg.getWidth()
            Dim height As Integer = bimg.getHeight()

            sigImgHeight = height
            sigImgWidth = width

        End Sub

        '/**
        ' * 
        ' * @return Affine Transform parameters of for PDF Matrix
        ' */
        Public Function getAffineTransformParams() As Byte()
            Return Me._affineTransformParams
        End Function

        '/**
        ' * 
        ' * @param _affineTransformParams
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function affineTransformParams(ByVal params() As Byte) As PDVisibleSignDesigner
            _affineTransformParams = params
            Return Me
        End Function

        '/**
        ' * 
        ' * @return formatter PDRectanle parameters
        ' */
        Public Function getFormaterRectangleParams() As Byte()
            Return _formaterRectangleParams
        End Function

        '/**
        ' * sets formatter PDRectangle;
        ' * 
        ' * @param _formaterRectangleParams
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function formaterRectangleParams(ByVal params() As Byte) As PDVisibleSignDesigner
            Me._formaterRectangleParams = params
            Return Me
        End Function

        Public Function getPageWidth() As Single
            Return _pageWidth
        End Function

        '/**
        ' * 
        ' * @param sets _pageWidth
        ' * @return Visible Signature Configuration Object
        ' */
        Public Function pageWidth(ByVal value As Single) As PDVisibleSignDesigner
            Me._pageWidth = value
            Return Me
        End Function

        Public Function getPageHeight() As Single
            Return _pageHeight
        End Function

        '/**
        ' * get image size in percents
        ' * @return
        ' */
        Public Function getImageSizeInPercents() As Single
            Return _imageSizeInPercents
        End Function

        '/**
        ' * 
        ' * @param _imageSizeInPercents
        ' */
        Public Sub imageSizeInPercents(ByVal value As Single)
            Me._imageSizeInPercents = value
        End Sub

        '/**
        ' * returns visible signature text
        ' * @return
        ' */
        Public Function getSignatureText() As String
            Throw New NotImplementedException("That method is not yet implemented")
        End Function

        '/**
        ' * 
        ' * @param signatureText - adds the text on visible signature
        ' * @return
        ' */
        Public Function signatureText(ByVal value As String) As PDVisibleSignDesigner
            Throw New NotImplementedException("That method is not yet implemented")
        End Function


    End Class

End Namespace
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

    '/**
    ' * Using that class, we  build pdf template
    ' * @author <a href="mailto:vakhtang.koroghlishvili@gmail.com"> vakhtang koroghlishvili (gogebashvili) </a>
    ' */
    Public Class PDFTemplateCreator

        Private pdfBuilder As PDFTemplateBuilder
        'private static final Log logger = LogFactory.getLog(PDFTemplateCreator.class);

        '/**
        ' * sets PDFBuilder
        ' * 
        ' * @param bookBuilder
        ' */
        Public Sub New(ByVal bookBuilder As PDFTemplateBuilder)
            Me.pdfBuilder = bookBuilder
        End Sub

        '/**
        ' * that method returns object of PDFStructur
        ' * 
        ' * @return PDFStructure
        ' */
        Public Function getPdfStructure() As PDFTemplateStructure
            Return Me.pdfBuilder.getStructure()
        End Function

        '/**
        ' * this method builds pdf  step by step, and finally it returns stream of visible signature
        ' * @param properties
        ' * @return InputStream
        ' * @throws IOException
        ' * @throws COSVisitorException
        ' */

        Public Function buildPDF(ByVal properties As PDVisibleSignDesigner) As InputStream 'throws IOException
            LOG.info("pdf building has been started")
            Dim pdfStructure As PDFTemplateStructure = pdfBuilder.getStructure()

            ' we create array of [Text, ImageB, ImageC, ImageI]
            Me.pdfBuilder.createProcSetArray()

            'create page
            Me.pdfBuilder.createPage(properties)
            Dim page As PDPage = pdfStructure.getPage()

            'create template
            Me.pdfBuilder.createTemplate(page)
            Dim template As PDDocument = pdfStructure.getTemplate()

            'create /AcroForm
            Me.pdfBuilder.createAcroForm(template)
            Dim acroForm As PDAcroForm = pdfStructure.getAcroForm()

            ' AcroForm contains singature fields
            Me.pdfBuilder.createSignatureField(acroForm)
            Dim pdSignatureField As PDSignatureField = pdfStructure.getSignatureField()

            ' create signature
            Me.pdfBuilder.createSignature(pdSignatureField, page, properties.getSignatureFieldName())

            ' that is /AcroForm/DR entry
            Me.pdfBuilder.createAcroFormDictionary(acroForm, pdSignatureField)

            ' create AffineTransform
            Me.pdfBuilder.createAffineTransform(properties.getAffineTransformParams())
            Dim transform As AffineTransform = pdfStructure.getAffineTransform()

            'rectangle, formatter, image. /AcroForm/DR/XObject contains that form
            Me.pdfBuilder.createSignatureRectangle(pdSignatureField, properties)
            Me.pdfBuilder.createFormaterRectangle(properties.getFormaterRectangleParams())
            Dim formater As PDRectangle = pdfStructure.getFormaterRectangle()
            Me.pdfBuilder.createSignatureImage(template, properties.getImageStream())

            ' create form stream, form and  resource. 
            Me.pdfBuilder.createHolderFormStream(template)
            Dim holderFormStream As PDStream = pdfStructure.getHolderFormStream()
            Me.pdfBuilder.createHolderFormResources()
            Dim holderFormResources As PDResources = pdfStructure.getHolderFormResources()
            Me.pdfBuilder.createHolderForm(holderFormResources, holderFormStream, formater)

            ' that is /AP entry the appearance dictionary.
            Me.pdfBuilder.createAppearanceDictionary(pdfStructure.getHolderForm(), pdSignatureField)

            ' inner formstream, form and resource (hlder form containts inner form)
            Me.pdfBuilder.createInnerFormStream(template)
            Me.pdfBuilder.createInnerFormResource()
            Dim innerFormResource As PDResources = pdfStructure.getInnerFormResources()
            Me.pdfBuilder.createInnerForm(innerFormResource, pdfStructure.getInnterFormStream(), formater)
            Dim innerForm As PDXObjectForm = pdfStructure.getInnerForm()

            ' inner form must be in the holder form as we wrote
            Me.pdfBuilder.insertInnerFormToHolerResources(innerForm, holderFormResources)

            '  Image form is in this structure: /AcroForm/DR/FRM0/Resources/XObject/n0
            Me.pdfBuilder.createImageFormStream(template)
            Dim imageFormStream As PDStream = pdfStructure.getImageFormStream()
            Me.pdfBuilder.createImageFormResources()
            Dim imageFormResources As PDResources = pdfStructure.getImageFormResources()
            Me.pdfBuilder.createImageForm(imageFormResources, innerFormResource, imageFormStream, formater, transform, pdfStructure.getJpedImage())

            ' now inject procSetArray
            Me.pdfBuilder.injectProcSetArray(innerForm, page, innerFormResource, imageFormResources, holderFormResources, pdfStructure.getProcSet())

            Dim imgFormName As String = pdfStructure.getImageFormName()
            Dim imgName As String = pdfStructure.getImageName()
            Dim innerFormName As String = pdfStructure.getInnerFormName()

            ' now create Streams of AP
            Me.pdfBuilder.injectAppearanceStreams(holderFormStream, imageFormStream, imageFormStream, imgFormName, imgName, innerFormName, properties)
            Me.pdfBuilder.createVisualSignature(template)
            Me.pdfBuilder.createWidgetDictionary(pdSignatureField, holderFormResources)

            Dim [in] As ByteArrayInputStream = Nothing
            Try
                [in] = pdfStructure.getTemplateAppearanceStream()
            Catch e As COSVisitorException
                LOG.error("COSVisitorException: can't get apereance stream ", e)
            End Try
            LOG.info("stream returning started, size= " + [in].available())

            ' we must close the document
            template.close()

            ' return result of the stream 
            Return [in]
        End Function


    End Class

End Namespace

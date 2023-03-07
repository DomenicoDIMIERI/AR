Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

    '/**
    ' * That's implementation of <b>PDFTemplateBuilder </b>
    ' * @see org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible.PDFTemplateBuilder
    ' * @author <a href="mailto:vakhtang.koroghlishvili@gmail.com"> vakhtang koroghlishvili (gogebashvili) </a>
    ' * 
    ' */
    Public Class PDVisibleSigBuilder
        Implements PDFTemplateBuilder

        Private pdfStructure As PDFTemplateStructure
        'private static final Log LOG = LogFactory.getLog(PDVisibleSigBuilder.class);

        Public Sub createPage(ByVal properties As PDVisibleSignDesigner) Implements PDFTemplateBuilder.createPage
            Dim page As PDPage = New PDPage()
            page.setMediaBox(New PDRectangle(properties.getPageWidth(), properties.getPageHeight()))
            pdfStructure.setPage(page)
            LOG.info("PDF page has been created")
        End Sub

        Public Sub createTemplate(ByVal page As PDPage) Implements PDFTemplateBuilder.createTemplate 'throws IOException
            Dim template As PDDocument = New PDDocument()
            template.addPage(page)
            pdfStructure.setTemplate(template)
        End Sub

        Public Sub New()
            pdfStructure = New PDFTemplateStructure()
            LOG.info("PDF Strucure has been Created")
        End Sub

        Public Sub createAcroForm(ByVal template As PDDocument) Implements PDFTemplateBuilder.createAcroForm
            Dim theAcroForm As PDAcroForm = New PDAcroForm(template)
            template.getDocumentCatalog().setAcroForm(theAcroForm)
            pdfStructure.setAcroForm(theAcroForm)
            LOG.info("Acro form page has been created")
        End Sub

        Public Function getStructure() As PDFTemplateStructure Implements PDFTemplateBuilder.getStructure
            Return pdfStructure
        End Function

        Public Sub createSignatureField(ByVal acroForm As PDAcroForm) Implements PDFTemplateBuilder.createSignatureField
            Dim sf As PDSignatureField = New PDSignatureField(acroForm)
            pdfStructure.setSignatureField(sf)
            LOG.info("Signature field has been created")
        End Sub

        Public Sub createSignature(ByVal pdSignatureField As PDSignatureField, ByVal page As PDPage, ByVal signatureName As String) Implements PDFTemplateBuilder.createSignature
            Dim pdSignature As PDSignature = New PDSignature()
            pdSignatureField.setSignature(pdSignature)
            pdSignatureField.getWidget().setPage(page)
            page.getAnnotations().add(pdSignatureField.getWidget())
            pdSignature.setName(signatureName)
            pdSignature.setByteRange(New Integer() {0, 0, 0, 0})
            pdSignature.setContents(Array.CreateInstance(GetType(Byte), 4096))
            pdfStructure.setPdSignature(pdSignature)
            LOG.info("PDSignatur has been created")
        End Sub

        Public Sub createAcroFormDictionary(ByVal acroForm As PDAcroForm, ByVal signatureField As PDSignatureField) Implements PDFTemplateBuilder.createAcroFormDictionary
            '@SuppressWarnings("unchecked")
            Dim acroFormFields As List(Of PDField) = acroForm.getFields()
            Dim acroFormDict As COSDictionary = acroForm.getDictionary()
            acroFormDict.setDirect(True)
            acroFormDict.setInt(COSName.SIG_FLAGS, 3)
            acroFormFields.add(signatureField)
            acroFormDict.setString(COSName.DA, "/sylfaen 0 Tf 0 g")
            pdfStructure.setAcroFormFields(acroFormFields)
            pdfStructure.setAcroFormDictionary(acroFormDict)
            LOG.info("AcroForm dictionary has been created")
        End Sub

        Public Sub createSignatureRectangle(ByVal signatureField As PDSignatureField, ByVal properties As PDVisibleSignDesigner) Implements PDFTemplateBuilder.createSignatureRectangle
            Dim rect As PDRectangle = New PDRectangle()
            rect.setUpperRightX(properties.getxAxis() + properties.getWidth())
            rect.setUpperRightY(properties.getTemplateHeight() - properties.getyAxis())
            rect.setLowerLeftY(properties.getTemplateHeight() - properties.getyAxis() - properties.getHeight())
            rect.setLowerLeftX(properties.getxAxis())
            signatureField.getWidget().setRectangle(rect)
            pdfStructure.setSignatureRectangle(rect)
            LOG.info("rectangle of signature has been created")
        End Sub

        Public Sub createAffineTransform(ByVal params() As Byte) Implements PDFTemplateBuilder.createAffineTransform
            Dim transform As AffineTransform = New AffineTransform(params(0), params(1), params(2), params(2), params(4), params(5))
            pdfStructure.setAffineTransform(transform)
            LOG.info("Matrix has been added")
        End Sub

        Public Sub createProcSetArray() Implements PDFTemplateBuilder.createProcSetArray
            Dim procSetArr As COSArray = New COSArray()
            procSetArr.add(COSName.getPDFName("PDF"))
            procSetArr.add(COSName.getPDFName("Text"))
            procSetArr.add(COSName.getPDFName("ImageB"))
            procSetArr.add(COSName.getPDFName("ImageC"))
            procSetArr.add(COSName.getPDFName("ImageI"))
            pdfStructure.setProcSet(procSetArr)
            LOG.info("ProcSet array has been created")
        End Sub

        Public Sub createSignatureImage(ByVal template As PDDocument, ByVal inputStream As InputStream) Implements PDFTemplateBuilder.createSignatureImage
            Dim img As PDJpeg = New PDJpeg(template, inputStream)
            pdfStructure.setJpedImage(img)
            LOG.info("Visible Signature Image has been created")
            ' pdfStructure.setTemplate(template)
            inputStream.Close()
        End Sub


        Public Sub createFormaterRectangle(ByVal params() As Byte) Implements PDFTemplateBuilder.createFormaterRectangle
            Dim formrect As PDRectangle = New PDRectangle()
            formrect.setUpperRightX(params(0))
            formrect.setUpperRightY(params(1))
            formrect.setLowerLeftX(params(2))
            formrect.setLowerLeftY(params(2))

            pdfStructure.setFormaterRectangle(formrect)
            LOG.info("Formater rectangle has been created")
        End Sub

        Public Sub createHolderFormStream(ByVal template As PDDocument) Implements PDFTemplateBuilder.createHolderFormStream
            Dim holderForm As PDStream = New PDStream(template)
            pdfStructure.setHolderFormStream(holderForm)
            LOG.info("Holder form Stream has been created")
        End Sub

        Public Sub createHolderFormResources() Implements PDFTemplateBuilder.createHolderFormResources
            Dim holderFormResources As PDResources = New PDResources()
            pdfStructure.setHolderFormResources(holderFormResources)
            LOG.info("Holder form resources have been created")
        End Sub

        Public Sub createHolderForm(ByVal holderFormResources As PDResources, ByVal holderFormStream As PDStream, ByVal formrect As PDRectangle) Implements PDFTemplateBuilder.createHolderForm
            Dim holderForm As PDXObjectForm = New PDXObjectForm(holderFormStream)
            holderForm.setResources(holderFormResources)
            holderForm.setBBox(formrect)
            holderForm.setFormType(1)
            pdfStructure.setHolderForm(holderForm)
            LOG.info("Holder form has been created")
        End Sub

        Public Sub createAppearanceDictionary(ByVal holderForml As PDXObjectForm, ByVal signatureField As PDSignatureField) Implements PDFTemplateBuilder.createAppearanceDictionary
            Dim appearance As PDAppearanceDictionary = New PDAppearanceDictionary()
            appearance.getCOSObject().setDirect(True)

            Dim appearanceStream As PDAppearanceStream = New PDAppearanceStream(holderForml.getCOSStream())

            appearance.setNormalAppearance(appearanceStream)
            signatureField.getWidget().setAppearance(appearance)

            pdfStructure.setAppearanceDictionary(appearance)
            LOG.info("PDF appereance Dictionary has been created")
        End Sub

        Public Sub createInnerFormStream(ByVal template As PDDocument) Implements PDFTemplateBuilder.createInnerFormStream
            Dim innterFormStream As PDStream = New PDStream(template)
            pdfStructure.setInnterFormStream(innterFormStream)
            LOG.info("Strean of another form (inner form - it would be inside holder form) has been created")
        End Sub

        Public Sub createInnerFormResource() Implements PDFTemplateBuilder.createInnerFormResource
            Dim innerFormResources As PDResources = New PDResources()
            pdfStructure.setInnerFormResources(innerFormResources)
            LOG.info("Resources of another form (inner form - it would be inside holder form) have been created")
        End Sub

        Public Sub createInnerForm(ByVal innerFormResources As PDResources, ByVal innerFormStream As PDStream, ByVal formrect As PDRectangle) Implements PDFTemplateBuilder.createInnerForm
            Dim innerForm As PDXObjectForm = New PDXObjectForm(innerFormStream)
            innerForm.setResources(innerFormResources)
            innerForm.setBBox(formrect)
            innerForm.setFormType(1)
            pdfStructure.setInnerForm(innerForm)
            LOG.info("Another form (inner form - it would be inside holder form) have been created")
        End Sub


        Public Sub insertInnerFormToHolerResources(ByVal innerForm As PDXObjectForm, ByVal holderFormResources As PDResources) Implements PDFTemplateBuilder.insertInnerFormToHolerResources
            Dim name As String = holderFormResources.addXObject(innerForm, "FRM")
            pdfStructure.setInnerFormName(name)
            LOG.info("Alerady inserted inner form  inside holder form")
        End Sub

        Public Sub createImageFormStream(ByVal template As PDDocument) Implements PDFTemplateBuilder.createImageFormStream
            Dim imageFormStream As PDStream = New PDStream(template)
            pdfStructure.setImageFormStream(imageFormStream)
            LOG.info("Created image form Stream")
        End Sub

        Public Sub createImageFormResources() Implements PDFTemplateBuilder.createImageFormResources
            Dim imageFormResources As PDResources = New PDResources()
            pdfStructure.setImageFormResources(imageFormResources)
            LOG.info("Created image form Resources")
        End Sub

        Public Sub createImageForm(ByVal imageFormResources As PDResources, ByVal innerFormResource As PDResources, ByVal imageFormStream As PDStream, ByVal formrect As PDRectangle, ByVal affineTransform As AffineTransform, ByVal img As PDJpeg) Implements PDFTemplateBuilder.createImageForm
            '/*
            ' * if you need text on the visible signature 
            ' * 
            ' * PDFont font = PDTrueTypeFont.loadTTF(Me.pdfStructure.getTemplate(), new File("D:\\arial.ttf")); 
            ' * font.setFontEncoding(new WinAnsiEncoding());
            ' * 
            ' * Map<String, PDFont> fonts = new HashMap<String, PDFont>(); fonts.put("arial", font);
            ' */
            Dim imageForm As PDXObjectForm = New PDXObjectForm(imageFormStream)
            imageForm.setBBox(formrect)
            imageForm.setMatrix(affineTransform)
            imageForm.setResources(imageFormResources)
            imageForm.setFormType(1)
            '/*
            ' * imageForm.getResources().addFont(font); 
            ' * imageForm.getResources().setFonts(fonts);
            ' */

            imageFormResources.getCOSObject().setDirect(True)
            Dim imageFormName As String = innerFormResource.addXObject(imageForm, "n")
            Dim imageName As String = imageFormResources.addXObject(img, "img")
            Me.pdfStructure.setImageForm(imageForm)
            Me.pdfStructure.setImageFormName(imageFormName)
            Me.pdfStructure.setImageName(imageName)
            LOG.info("Created image form")
        End Sub

        Public Sub injectProcSetArray(ByVal innerForm As PDXObjectForm, ByVal page As PDPage, ByVal innerFormResources As PDResources, ByVal imageFormResources As PDResources, ByVal holderFormResources As PDResources, ByVal procSet As COSArray) Implements PDFTemplateBuilder.injectProcSetArray
            innerForm.getResources().getCOSDictionary().setItem(COSName.PROC_SET, procSet) '
            page.getCOSDictionary().setItem(COSName.PROC_SET, procSet)
            innerFormResources.getCOSDictionary().setItem(COSName.PROC_SET, procSet)
            imageFormResources.getCOSDictionary().setItem(COSName.PROC_SET, procSet)
            holderFormResources.getCOSDictionary().setItem(COSName.PROC_SET, procSet)
            LOG.info("inserted ProcSet to PDF")
        End Sub

        Public Sub injectAppearanceStreams(ByVal holderFormStream As PDStream, ByVal innterFormStream As PDStream, ByVal imageFormStream As PDStream, ByVal imageObjectName As String, ByVal imageName As String, ByVal innerFormName As String, ByVal properties As PDVisibleSignDesigner) Implements PDFTemplateBuilder.injectAppearanceStreams
            ' 100 means that document width is 100% via the rectangle. if rectangle
            ' is 500px, images 100% is 500px.
            ' String imgFormComment = "q "+imageWidthSize+ " 0 0 50 0 0 cm /" +
            ' imageName + " Do Q\n" + builder.toString();
            Dim imgFormComment As String = "q 100 0 0 50 0 0 cm /" & imageName & " Do Q" & vbLf
            Dim holderFormComment As String = "q 1 0 0 1 0 0 cm /" & innerFormName & " Do Q " & vbLf
            Dim innerFormComment As String = "q 1 0 0 1 0 0 cm /" & imageObjectName & " Do Q" & vbLf

            appendRawCommands(pdfStructure.getHolderFormStream().createOutputStream(), holderFormComment)
            appendRawCommands(pdfStructure.getInnterFormStream().createOutputStream(), innerFormComment)
            appendRawCommands(pdfStructure.getImageFormStream().createOutputStream(), imgFormComment)
            LOG.info("Injected apereance stream to pdf")

        End Sub

        Public Sub appendRawCommands(ByVal os As OutputStream, ByVal commands As String)
            os.Write(Sistema.Strings.GetBytes(commands, "UTF-8"))
            os.Close()
        End Sub

        Public Sub createVisualSignature(ByVal template As PDDocument) Implements PDFTemplateBuilder.createVisualSignature
            Me.pdfStructure.setVisualSignature(template.getDocument())
            LOG.info("Visible signature has been created")
        End Sub

        Public Sub createWidgetDictionary(ByVal signatureField As PDSignatureField, ByVal holderFormResources As PDResources) Implements PDFTemplateBuilder.createWidgetDictionary
            Dim widgetDict As COSDictionary = signatureField.getWidget().getDictionary()
            widgetDict.setNeedToBeUpdate(True)
            widgetDict.setItem(COSName.DR, holderFormResources.getCOSObject())

            pdfStructure.setWidgetDictionary(widgetDict)
            LOG.info("WidgetDictionary has been crated")
        End Sub

        Public Sub closeTemplate(ByVal template As PDDocument) Implements PDFTemplateBuilder.closeTemplate
            template.close()
            Me.pdfStructure.getTemplate().close()
        End Sub

    End Class

End Namespace

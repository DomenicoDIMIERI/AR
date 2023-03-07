Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfwriter
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

    '/**
    ' * Structure of PDF document with visible signature
    ' * 
    ' * @author <a href="mailto:vakhtang.koroghlishvili@gmail.com"> vakhtang koroghlishvili (gogebashvili) </a>
    ' * 
    ' */
    Public Class PDFTemplateStructure

        Private page As PDPage
        Private template As PDDocument
        Private acroForm As PDAcroForm
        Private signatureField As PDSignatureField
        Private pdSignature As PDSignature
        Private acroFormDictionary As COSDictionary
        Private singatureRectangle As PDRectangle
        Private affineTransform As AffineTransform
        Private procSet As COSArray
        Private jpedImage As PDJpeg
        Private formaterRectangle As PDRectangle
        Private holderFormStream As PDStream
        Private holderFormResources As PDResources
        Private holderForm As PDXObjectForm
        Private appearanceDictionary As PDAppearanceDictionary
        Private innterFormStream As PDStream
        Private innerFormResources As PDResources
        Private innerForm As PDXObjectForm
        Private imageFormStream As PDStream
        Private imageFormResources As PDResources
        Private acroFormFields As List(Of PDField)
        Private innerFormName As String
        Private imageFormName As String
        Private imageName As String
        Private visualSignature As COSDocument
        Private imageForm As PDXObjectForm
        Private widgetDictionary As COSDictionary

        '/**
        ' * Returns document page.
        ' * @return
        ' */
        Public Function getPage() As PDPage
            Return page
        End Function

        '/**
        ' * Sets document page
        ' * @param page
        ' */
        Public Sub setPage(ByVal page As PDPage)
            Me.page = page
        End Sub

        '/**
        ' * Gets PDDocument template.
        ' * This represents a digital signature
        ' *  that can be attached to a document
        ' * @return
        ' */
        Public Function getTemplate() As PDDocument
            Return template
        End Function

        '/**
        ' * Wets PDDocument template.
        ' * This represents a digital signature
        ' * that can be attached to a document
        ' * @param template
        ' */
        Public Sub setTemplate(ByVal template As PDDocument)
            Me.template = template
        End Sub

        '/**
        ' * Gets Acroform
        ' * @return
        ' */
        Public Function getAcroForm() As PDAcroForm
            Return acroForm
        End Function

        '/**
        ' * Sets Acroform
        ' * @param acroForm
        ' */
        Public Sub setAcroForm(ByVal acroForm As PDAcroForm)
            Me.acroForm = acroForm
        End Sub

        '/**
        ' * Gets Signature field
        ' * @return
        ' */
        Public Function getSignatureField() As PDSignatureField
            Return signatureField
        End Function

        '/**
        ' * Sets signature field
        ' * @param signatureField
        ' */
        Public Sub setSignatureField(ByVal signatureField As PDSignatureField)
            Me.signatureField = signatureField
        End Sub

        '/**
        ' * Gets PDSignature
        ' * @return
        ' */
        Public Function getPdSignature() As PDSignature
            Return pdSignature
        End Function

        '/**
        ' * Sets PDSignature
        ' * @param pdSignature
        ' */
        Public Sub setPdSignature(ByVal pdSignature As PDSignature)
            Me.pdSignature = pdSignature
        End Sub

        '/**
        ' * Gets Dictionary of AcroForm. Thats <b> /DR </b>
        ' * entry in the AcroForm
        ' * @return
        ' */
        Public Function getAcroFormDictionary() As COSDictionary
            Return acroFormDictionary
        End Function

        '/**
        ' * Acroform have its Dictionary, so we here set
        ' * the Dictionary  which is in this location:
        ' * <b> AcroForm/DR <b>
        ' * @param acroFormDictionary
        ' */
        Public Sub setAcroFormDictionary(ByVal acroFormDictionary As COSDictionary)
            Me.acroFormDictionary = acroFormDictionary
        End Sub

        '/**
        ' * Gets SignatureRectangle
        ' * @return
        ' */
        Public Function getSingatureRectangle() As PDRectangle
            Return singatureRectangle
        End Function

        '/**
        ' * Sets SignatureRectangle
        ' * @param singatureRectangle
        ' */
        Public Sub setSignatureRectangle(ByVal singatureRectangle As PDRectangle)
            Me.singatureRectangle = singatureRectangle
        End Sub

        '/**
        ' * Gets AffineTransform
        ' * @return
        ' */
        Public Function getAffineTransform() As AffineTransform
            Return affineTransform
        End Function

        '/**
        ' * Sets AffineTransform
        ' * @param affineTransform
        ' */
        Public Sub setAffineTransform(ByVal affineTransform As AffineTransform)
            Me.affineTransform = affineTransform
        End Sub

        '/**
        ' * Gets ProcSet Array
        ' * @return
        ' */
        Public Function getProcSet() As COSArray
            Return procSet
        End Function

        '/**
        ' * Sets ProcSet Array
        ' * @param procSet
        ' */
        Public Sub setProcSet(ByVal procSet As COSArray)
            Me.procSet = procSet
        End Sub

        '/**
        ' * Gets the image of visible signature
        ' * @return
        ' */
        Public Function getJpedImage() As PDJpeg
            Return jpedImage
        End Function

        '/**
        ' * Sets the image of visible signature
        ' * @param jpedImage
        ' */
        Public Sub setJpedImage(ByVal jpedImage As PDJpeg)
            Me.jpedImage = jpedImage
        End Sub

        '/**
        ' * Gets formatter rectangle
        ' * @return
        ' */
        Public Function getFormaterRectangle() As PDRectangle
            Return formaterRectangle
        End Function

        '/**
        ' * Sets formatter rectangle
        ' * @param formaterRectangle
        ' */
        Public Sub setFormaterRectangle(ByVal formaterRectangle As PDRectangle)
            Me.formaterRectangle = formaterRectangle
        End Sub

        '/**
        ' * Sets HolderFormStream
        ' * @return
        ' */
        Public Function getHolderFormStream() As PDStream
            Return holderFormStream
        End Function

        '/**
        ' * Sets stream of holder form Stream 
        ' * @param holderFormStream
        ' */
        Public Sub setHolderFormStream(ByVal holderFormStream As PDStream)
            Me.holderFormStream = holderFormStream
        End Sub

        '/**
        ' * Gets Holder form.
        ' * That form is here <b> AcroForm/DR/XObject/{holder form name} </b>
        ' * By default, name stars with FRM. We also add number of form
        ' * to the name.
        ' * @return
        ' */
        Public Function getHolderForm() As PDXObjectForm
            Return holderForm
        End Function

        '/**
        ' * In the structure, form will be contained by XObject in the <b>AcroForm/DR/ </b>
        ' * @param holderForm
        ' */
        Public Sub setHolderForm(ByVal holderForm As PDXObjectForm)
            Me.holderForm = holderForm
        End Sub

        '/**
        ' * Gets Holder form resources
        ' * @return
        ' */
        Public Function getHolderFormResources() As PDResources
            Return holderFormResources
        End Function

        '/**
        ' * Sets holder form resources
        ' * @param holderFormResources
        ' */
        Public Sub setHolderFormResources(ByVal holderFormResources As PDResources)
            Me.holderFormResources = holderFormResources
        End Sub

        '/**
        ' * Gets AppearanceDictionary
        ' * That is <b>/AP</b> entry the appearance dictionary.
        ' * @return
        ' */
        Public Function getAppearanceDictionary() As PDAppearanceDictionary
            Return appearanceDictionary
        End Function

        '/**
        ' * Sets AppearanceDictionary
        ' * That is <b>/AP</b> entry the appearance dictionary.
        ' * @param appearanceDictionary
        ' */
        Public Sub setAppearanceDictionary(ByVal appearanceDictionary As PDAppearanceDictionary)
            Me.appearanceDictionary = appearanceDictionary
        End Sub

        '/**
        ' * Gets Inner form Stream.
        ' * @return
        ' */
        Public Function getInnterFormStream() As PDStream
            Return innterFormStream
        End Function

        '/**
        ' * Sets inner form stream
        ' * @param innterFormStream
        ' */
        Public Sub setInnterFormStream(ByVal innterFormStream As PDStream)
            Me.innterFormStream = innterFormStream
        End Sub

        '/**
        ' * Gets inner form Resource
        ' * @return
        ' */
        Public Function getInnerFormResources() As PDResources
            Return innerFormResources
        End Function

        '/**
        ' * Sets inner form resource
        ' * @param innerFormResources
        ' */
        Public Sub setInnerFormResources(ByVal innerFormResources As PDResources)
            Me.innerFormResources = innerFormResources
        End Sub

        '/**
        ' * Gets inner form that is in this location:
        ' * <b> AcroForm/DR/XObject/{holder form name}/Resources/XObject/{inner name} </b>
        ' * By default inner form name starts with "n". Then we add number of form
        ' * to the name.
        ' * @return
        ' */
        Public Function getInnerForm() As PDXObjectForm
            Return innerForm
        End Function

        '/**
        ' * sets inner form to this location:
        ' * <b> AcroForm/DR/XObject/{holder form name}/Resources/XObject/{destination} </b>
        ' * @param innerForm
        ' */
        Public Sub setInnerForm(ByVal innerForm As PDXObjectForm)
            Me.innerForm = innerForm
        End Sub

        '/**
        ' * Gets name of inner form
        ' * @return
        ' */
        Public Function getInnerFormName() As String
            Return innerFormName
        End Function

        '/**
        ' * Sets inner form name
        ' * @param innerFormName
        ' */
        Public Sub setInnerFormName(ByVal innerFormName As String)
            Me.innerFormName = innerFormName
        End Sub

        '/**
        ' * Gets Image form stream
        ' * @return
        ' */
        Public Function getImageFormStream() As PDStream
            Return imageFormStream
        End Function

        '/**
        ' * Sets image form stream
        ' * @param imageFormStream
        ' */
        Public Sub setImageFormStream(ByVal imageFormStream As PDStream)
            Me.imageFormStream = imageFormStream
        End Sub

        '/**
        ' * Gets image form resources
        ' * @return
        ' */
        Public Function getImageFormResources() As PDResources
            Return imageFormResources
        End Function

        '/**
        ' * Sets image form resource
        ' * @param imageFormResources
        ' */
        Public Sub setImageFormResources(ByVal imageFormResources As PDResources)
            Me.imageFormResources = imageFormResources
        End Sub

        '/**
        ' * Gets Image form. Image form is in this structure: 
        ' * <b>/AcroForm/DR/{holder form}/Resources/XObject /{inner form} </b>
        ' * /Resources/XObject/{image form name}.
        ' * @return
        ' */
        Public Function getImageForm() As PDXObjectForm
            Return imageForm
        End Function

        '/**
        ' * Sets Image form. Image form will be in this structure: 
        ' * <b>/AcroForm/DR/{holder form}/Resources/XObject /{inner form}
        ' * /Resources/XObject/{image form name}.</b> By default we start
        ' *  image form name with "img". Then we  add number of image 
        ' *  form to the form name.
        ' * Sets image form
        ' * @param imageForm
        ' */
        Public Sub setImageForm(ByVal imageForm As PDXObjectForm)
            Me.imageForm = imageForm
        End Sub

        '/**
        ' * Gets image form name
        ' * @return
        ' */
        Public Function getImageFormName() As String
            Return imageFormName
        End Function

        '/**
        ' * Sets image form name
        ' * @param imageFormName
        ' */
        Public Sub setImageFormName(ByVal imageFormName As String)
            Me.imageFormName = imageFormName
        End Sub

        '/**
        ' * Gets visible signature image name
        ' * @return
        ' */
        Public Function getImageName() As String
            Return imageName
        End Function

        '/**
        ' * Sets visible signature image name
        ' * @param imageName
        ' */
        Public Sub setImageName(ByVal imageName As String)
            Me.imageName = imageName
        End Sub

        '/**
        ' * Gets COSDocument of visible Signature.
        ' * @see org.apache.pdfbox.cos.COSDocument
        ' * @return
        ' */
        Public Function getVisualSignature() As COSDocument
            Return visualSignature
        End Function

        '/**
        ' * 
        ' * Sets COSDocument of visible Signature.
        ' * @see org.apache.pdfbox.cos.COSDocument
        ' * @param visualSignature
        ' */
        Public Sub setVisualSignature(ByVal visualSignature As COSDocument)
            Me.visualSignature = visualSignature
        End Sub

        '/**
        ' * Gets acroFormFields
        ' * @return
        ' */
        Public Function getAcroFormFields() As List(Of PDField)
            Return acroFormFields
        End Function

        '/**
        ' * Sets acroFormFields
        ' * @param acroFormFields
        ' */
        Public Sub setAcroFormFields(ByVal acroFormFields As List(Of PDField))
            Me.acroFormFields = acroFormFields
        End Sub

        '/**
        ' * Gets AP of the created template
        ' * @return
        ' * @throws IOException
        ' * @throws COSVisitorException
        ' */
        Public Function getTemplateAppearanceStream() As ByteArrayInputStream  ' throws IOException, COSVisitorException
            Dim visualSignature As COSDocument = getVisualSignature()
            Dim memoryOut As ByteArrayOutputStream = New ByteArrayOutputStream()
            Dim memoryWriter As COSWriter = New COSWriter(memoryOut)
            memoryWriter.write(visualSignature)
            memoryOut.Dispose()
            Dim input As ByteArrayInputStream = New ByteArrayInputStream(memoryOut.toByteArray())
            getTemplate().close()
            Return input
        End Function

        '/**
        ' * Gets Widget Dictionary.
        ' * {@link org.apache.pdfbox.pdmodel.interactive.form.PDField}
        ' * @see org.apache.pdfbox.pdmodel.interactive.form.PDField.getWidget() 
        ' * @return
        ' */
        Public Function getWidgetDictionary() As COSDictionary
            Return widgetDictionary
        End Function

        '/**
        ' * Sets Widget Dictionary.
        ' * {@link org.apache.pdfbox.pdmodel.interactive.form.PDField}
        ' * @see org.apache.pdfbox.pdmodel.interactive.form.PDField.getWidget() 
        ' * @param widgetDictionary
        ' */
        Public Sub setWidgetDictionary(ByVal widgetDictionary As COSDictionary)
            Me.widgetDictionary = widgetDictionary
        End Sub

    End Class

End Namespace

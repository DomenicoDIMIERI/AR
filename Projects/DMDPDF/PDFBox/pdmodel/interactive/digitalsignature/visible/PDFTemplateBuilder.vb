Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature.visible

    '/**
    ' * That class builds visible signature template
    ' * which will be added in our pdf document
    ' * @author Vakhtang koroghlishvili (Gogebashvili)
    ' *
    ' */
    Public Interface PDFTemplateBuilder


        ''' <summary>
        ''' In order to create  Affine Transform, using parameters
        ''' </summary>
        ''' <param name="params"></param>
        ''' <remarks></remarks>
        Sub createAffineTransform(ByVal params() As Byte)

        '/**
        ' * Creates specified size page
        ' * @param properties
        ' */
        Sub createPage(ByVal properties As PDVisibleSignDesigner)

        '/**
        ' * Creates template using page
        ' * @param page
        ' * @throws IOException
        ' */
        Sub createTemplate(ByVal page As PDPage)  'throws IOException;

        '/**
        ' * Creates Acro forms in the template
        ' * @param template
        ' */
        Sub createAcroForm(ByVal template As PDDocument)

        '/**
        ' * Creates signature fields
        ' * @param acroForm
        ' * @throws IOException
        ' */
        Sub createSignatureField(ByVal acroForm As PDAcroForm)  'throws IOException;

        '/**
        ' * Creates PDSignature
        ' * @param pdSignatureField
        ' * @param page
        ' * @param signatureName
        ' * @throws IOException
        ' */
        Sub createSignature(ByVal pdSignatureField As PDSignatureField, ByVal page As PDPage, ByVal signatureName As String)  'throws IOException;

        '/**
        ' * Create AcroForm Dictionary
        ' * @param acroForm
        ' * @param signatureField
        ' * @throws IOException
        ' */
        Sub createAcroFormDictionary(ByVal acroForm As PDAcroForm, ByVal signatureField As PDSignatureField)  'throws IOException;

        '/**
        ' * Creates SingatureRectangle
        ' * @param signatureField
        ' * @param properties
        ' * @throws IOException
        ' */
        Sub createSignatureRectangle(ByVal signatureField As PDSignatureField, ByVal properties As PDVisibleSignDesigner)  'throws IOException;

        '/**
        ' * Creates procSetArray of PDF,Text,ImageB,ImageC,ImageI    
        ' */
        Sub createProcSetArray()

        '/**
        ' * Creates signature image
        ' * @param template
        ' * @param InputStream
        ' * @throws IOException
        ' */
        Sub createSignatureImage(ByVal template As PDDocument, ByVal InputStream As InputStream) 'throws IOException;

        '/**
        ' * 
        ' * @param params
        ' */
        Sub createFormaterRectangle(ByVal params() As Byte)

        '/**
        ' * 
        ' * @param template
        ' */
        Sub createHolderFormStream(ByVal template As PDDocument)

        '/**
        ' * Creates resources of form
        ' */
        Sub createHolderFormResources()

        '/**
        ' * Creates Form
        ' * @param holderFormResources
        ' * @param holderFormStream
        ' * @param formrect
        ' */
        Sub createHolderForm(ByVal holderFormResources As PDResources, ByVal holderFormStream As PDStream, ByVal formrect As PDRectangle)

        '/**
        ' * Creates appearance dictionary
        ' * @param holderForml
        ' * @param signatureField
        ' * @throws IOException
        ' */
        Sub createAppearanceDictionary(ByVal holderForml As PDXObjectForm, ByVal signatureField As PDSignatureField)  'throws IOException;

        '/**
        ' * 
        ' * @param template
        ' */
        Sub createInnerFormStream(ByVal template As PDDocument)


        '/**
        ' * Creates InnerForm
        ' */
        Sub createInnerFormResource()

        '/**
        ' * 
        ' * @param innerFormResources
        ' * @param innerFormStream
        ' * @param formrect
        ' */
        Sub createInnerForm(ByVal innerFormResources As PDResources, ByVal innerFormStream As PDStream, ByVal formrect As PDRectangle)


        '/**
        '	 * 
        '	 * @param innerForm
        '	 * @param holderFormResources
        '	 */
        Sub insertInnerFormToHolerResources(ByVal innerForm As PDXObjectForm, ByVal holderFormResources As PDResources)

        '/**
        ' * 
        ' * @param template
        ' */
        Sub createImageFormStream(ByVal template As PDDocument)

        '/**
        ' * Create resource of image form
        ' */
        Sub createImageFormResources()

        '/**
        ' * Creates Image form
        ' * @param imageFormResources
        ' * @param innerFormResource
        ' * @param imageFormStream
        ' * @param formrect
        ' * @param affineTransform
        ' * @param img
        ' * @throws IOException
        ' */
        Sub createImageForm(ByVal imageFormResources As PDResources, ByVal innerFormResource As PDResources, ByVal imageFormStream As PDStream, ByVal formrect As PDRectangle, ByVal affineTransform As AffineTransform, ByVal img As PDJpeg)  'throws IOException;

        '/**
        ' * Inject procSetArray 
        ' * @param innerForm
        ' * @param page
        ' * @param innerFormResources
        ' * @param imageFormResources
        ' * @param holderFormResources
        ' * @param procSet
        ' */
        Sub injectProcSetArray(ByVal innerForm As PDXObjectForm, ByVal page As PDPage, ByVal innerFormResources As PDResources, ByVal imageFormResources As PDResources, ByVal holderFormResources As PDResources, ByVal procSet As COSArray)

        '/**
        ' * injects appearance streams
        ' * @param holderFormStream
        ' * @param innterFormStream
        ' * @param imageFormStream
        ' * @param imageObjectName
        ' * @param imageName
        ' * @param innerFormName
        ' * @param properties
        ' * @throws IOException
        ' */
        Sub injectAppearanceStreams(ByVal holderFormStream As PDStream, ByVal innterFormStream As PDStream, ByVal imageFormStream As PDStream, ByVal imageObjectName As String, ByVal imageName As String, ByVal innerFormName As String, ByVal properties As PDVisibleSignDesigner)  'throws IOException;

        '/**
        ' * just to create visible signature
        ' * @param template
        ' */
        Sub createVisualSignature(ByVal template As PDDocument)

        '/**
        ' * adds Widget Dictionary
        ' * @param signatureField
        ' * @param holderFormResources
        ' * @throws IOException
        ' */
        Sub createWidgetDictionary(ByVal signatureField As PDSignatureField, ByVal holderFormResources As PDResources)  'throws IOException;

        '/**
        ' * 
        ' * @return - PDF template Structure
        ' */
        Function getStructure() As PDFTemplateStructure

        '/**
        ' * Closes template
        ' * @param template
        ' * @throws IOException
        ' */
        Sub closeTemplate(ByVal template As PDDocument)  'throws IOException;


    End Interface

End Namespace

Imports FinSeA.Drawings
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.edit
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.optionalcontent
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.markedcontent
Imports FinSeA.org.apache.fontbox.util

Namespace org.apache.pdfbox.util


    '/**
    ' * This class allows to import pages as Form XObjects into a PDF file and use them to create
    ' * layers (optional content groups).
    ' *
    ' * @version $Revision$
    ' */
    Public Class LayerUtility

        Private Const DEBUG As Boolean = True

        Private targetDoc As PDDocument
        Private cloner As PDFCloneUtility

        '/**
        ' * Creates a new instance.
        ' * @param document the PDF document to modify
        ' */
        Public Sub New(ByVal document As PDDocument)
            Me.targetDoc = document
            Me.cloner = New PDFCloneUtility(document)
        End Sub

        '/**
        ' * Returns the PDF document we work on.
        ' * @return the PDF document
        ' */
        Public Function getDocument() As PDDocument
            Return Me.targetDoc
        End Function

        '/**
        ' * Some applications may not wrap their page content in a save/restore (q/Q) pair which can
        ' * lead to problems with coordinate system transformations when content is appended. This
        ' * method lets you add a q/Q pair around the existing page's content.
        ' * @param page the page
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Sub wrapInSaveRestore(ByVal page As PDPage) ' throws IOException
            Dim saveGraphicsStateDic As New COSDictionary()
            Dim saveGraphicsStateStream As COSStream = getDocument().getDocument().createCOSStream(saveGraphicsStateDic)
            Dim saveStream As OutputStream = saveGraphicsStateStream.createUnfilteredStream()
            saveStream.Write(Sistema.Strings.GetBytes("q" & vbLf, "ISO-8859-1"))
            saveStream.Flush()

            Dim restoreGraphicsStateStream As COSStream = getDocument().getDocument().createCOSStream(saveGraphicsStateDic)
            Dim restoreStream As OutputStream = restoreGraphicsStateStream.createUnfilteredStream()
            restoreStream.Write(Sistema.Strings.GetBytes("Q" & vbLf, "ISO-8859-1"))
            restoreStream.Flush()

            '//Wrap the existing page's content in a save/restore pair (q/Q) to have a controlled
            '//environment to add additional content.
            Dim pageDictionary As COSDictionary = page.getCOSDictionary()
            Dim contents As COSBase = pageDictionary.getDictionaryObject(COSName.CONTENTS)
            If (TypeOf (contents) Is COSStream) Then
                Dim contentsStream As COSStream = contents

                Dim Array As New COSArray()
                Array.add(saveGraphicsStateStream)
                Array.add(contentsStream)
                Array.add(restoreGraphicsStateStream)

                pageDictionary.setItem(COSName.CONTENTS, Array)
            ElseIf (TypeOf (contents) Is COSArray) Then
                Dim contentsArray As COSArray = contents
                contentsArray.add(0, saveGraphicsStateStream)
                contentsArray.add(restoreGraphicsStateStream)
            Else
                Throw New FormatException("Contents are unknown type: " & contents.GetType().Name)
            End If
        End Sub

        '/**
        ' * Imports a page from some PDF file as a Form XObject so it can be placed on another page
        ' * in the target document.
        ' * @param sourceDoc the source PDF document that contains the page to be copied
        ' * @param pageNumber the page number of the page to be copied
        ' * @return a Form XObject containing the original page's content
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Function importPageAsForm(ByVal sourceDoc As PDDocument, ByVal pageNumber As Integer) As PDXObjectForm  'throws IOException 
            Dim page As PDPage = sourceDoc.getDocumentCatalog().getAllPages().get(pageNumber)
            Return importPageAsForm(sourceDoc, page)
        End Function

        Private ReadOnly PAGE_TO_FORM_FILTER = New CKeyCollection(Of String)({"Group", "LastModified", "Metadata"}, {"Group", "LastModified", "Metadata"})

        '/**
        ' * Imports a page from some PDF file as a Form XObject so it can be placed on another page
        ' * in the target document.
        ' * @param sourceDoc the source PDF document that contains the page to be copied
        ' * @param page the page in the source PDF document to be copied
        ' * @return a Form XObject containing the original page's content
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Function importPageAsForm(ByVal sourceDoc As PDDocument, ByVal page As PDPage) As PDXObjectForm 'throws IOException 
            Dim pageStream As COSStream = page.getContents().getCOSObject()
            Dim newStream As New PDStream(targetDoc, pageStream.getUnfilteredStream(), False)
            Dim form As New PDXObjectForm(newStream)

            'Copy resources
            Dim pageRes As PDResources = page.findResources()
            Dim formRes As PDResources = New PDResources()
            cloner.cloneMerge(pageRes, formRes)
            form.setResources(formRes)

            'Transfer some values from page to form
            transferDict(page.getCOSDictionary(), form.getCOSStream(), PAGE_TO_FORM_FILTER, True)

            Dim matrix As Matrix = form.getMatrix()
            Dim at As AffineTransform
            If (matrix IsNot Nothing) Then
                at = matrix.createAffineTransform()
            Else
                at = New AffineTransform()
            End If
            Dim mediaBox As PDRectangle = page.findMediaBox()
            Dim cropBox As PDRectangle = page.findCropBox()
            Dim viewBox As PDRectangle = IIf(cropBox IsNot Nothing, cropBox, mediaBox)

            'Handle the /Rotation entry on the page dict
            Dim rotation As Integer = getNormalizedRotation(page)

            'Transform to FOP's user space
            'at.scale(1 / viewBox.getWidth(), 1 / viewBox.getHeight());
            at.translate(mediaBox.getLowerLeftX() - viewBox.getLowerLeftX(), mediaBox.getLowerLeftY() - viewBox.getLowerLeftY())
            Select Case (rotation)
                Case 90
                    at.scale(viewBox.getWidth() / viewBox.getHeight(), viewBox.getHeight() / viewBox.getWidth())
                    at.translate(0, viewBox.getWidth())
                    at.rotate(-Math.PI / 2.0)
                Case 180
                    at.translate(viewBox.getWidth(), viewBox.getHeight())
                    at.rotate(-Math.PI)
                Case 270
                    at.scale(viewBox.getWidth() / viewBox.getHeight(), viewBox.getHeight() / viewBox.getWidth())
                    at.translate(viewBox.getHeight(), 0)
                    at.rotate(-Math.PI * 1.5)
                Case Else
                    'no additional transformations necessary
            End Select
            'Compensate for Crop Boxes not starting at 0,0
            at.translate(-viewBox.getLowerLeftX(), -viewBox.getLowerLeftY())
            If (Not at.isIdentity()) Then
                form.setMatrix(at)
            End If
            Dim bbox As New BoundingBox()
            bbox.setLowerLeftX(viewBox.getLowerLeftX())
            bbox.setLowerLeftY(viewBox.getLowerLeftY())
            bbox.setUpperRightX(viewBox.getUpperRightX())
            bbox.setUpperRightY(viewBox.getUpperRightY())
            form.setBBox(New PDRectangle(bbox))
            Return form
        End Function

        '/**
        ' * Places the given form over the existing content of the indicated page (like an overlay).
        ' * The form is enveloped in a marked content section to indicate that it's part of an
        ' * optional content group (OCG), here used as a layer. This optional group is returned and
        ' * can be enabled and disabled through methods on {@link PDOptionalContentProperties}.
        ' * @param targetPage the target page
        ' * @param form the form to place
        ' * @param transform the transformation matrix that controls the placement
        ' * @param layerName the name for the layer/OCG to produce
        ' * @return the optional content group that was generated for the form usage
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Function appendFormAsLayer(ByVal targetPage As PDPage, ByVal form As PDXObjectForm, ByVal transform As AffineTransform, ByVal layerName As String) As PDOptionalContentGroup ' throws IOException
            Dim catalog As PDDocumentCatalog = targetDoc.getDocumentCatalog()
            Dim ocprops As PDOptionalContentProperties = catalog.getOCProperties()
            If (ocprops Is Nothing) Then
                ocprops = New PDOptionalContentProperties()
                catalog.setOCProperties(ocprops)
            End If
            If (ocprops.hasGroup(layerName)) Then
                Throw New ArgumentException("Optional group (layer) already exists: " & layerName)
            End If

            Dim layer As New PDOptionalContentGroup(layerName)
            ocprops.addGroup(layer)

            Dim resources As PDResources = targetPage.findResources()
            Dim props As PDPropertyList = resources.getProperties()
            If (props Is Nothing) Then
                props = New PDPropertyList()
                resources.setProperties(props)
            End If

            'Find first free resource name with the pattern "MC<index>"
            Dim index As Integer = 0
            Dim ocg As PDOptionalContentGroup
            Dim resourceName As COSName
            Do
                resourceName = COSName.getPDFName("MC" + index)
                ocg = props.getOptionalContentGroup(resourceName)
                index += 1
            Loop While (ocg IsNot Nothing)
            'Put mapping for our new layer/OCG
            props.putMapping(resourceName, layer)

            Dim contentStream As New PDPageContentStream(targetDoc, targetPage, True, Not DEBUG)
            contentStream.beginMarkedContentSequence(COSName.OC, resourceName)
            contentStream.drawXObject(form, transform)
            contentStream.endMarkedContentSequence()
            contentStream.close()
            Return layer
        End Function

        Private Sub transferDict(ByVal orgDict As COSDictionary, ByVal targetDict As COSDictionary, ByVal filter As CKeyCollection(Of String), ByVal inclusive As Boolean) 'throws IOException
            For Each entry As Map.Entry(Of COSName, COSBase) In orgDict.entrySet()
                Dim key As COSName = entry.Key
                If (inclusive AndAlso Not filter.Contains(key.getName())) Then
                    Continue For
                ElseIf (Not inclusive AndAlso filter.Contains(key.getName())) Then
                    Continue For
                End If
                targetDict.setItem(key, cloner.cloneForNewDocument(entry.Value))
            Next
        End Sub

        Private Shared Function getNormalizedRotation(ByVal page As PDPage) As Integer
            'Handle the /Rotation entry on the page dict
            Dim rotation As Integer = page.findRotation()
            While (rotation >= 360)
                rotation -= 360
            End While
            If (rotation < 0) Then
                rotation = 0
            End If
            Select Case (rotation)
                Case 90, 180, 270
                    Return rotation
                Case Else : Return 0
            End Select
        End Function

    End Class

End Namespace

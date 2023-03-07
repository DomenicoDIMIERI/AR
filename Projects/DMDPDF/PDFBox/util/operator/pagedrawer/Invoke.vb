Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.util.operator

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class Invoke
        Inherits OperatorProcessor

    
        '/**
        ' * process : Do : Paint the specified XObject (section 4.7).
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If there is an error invoking the sub object.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim drawer As pdfviewer.PageDrawer = context
            Dim page As PDPage = drawer.getPage()
            Dim objectName As COSName = arguments.get(0)
            Dim xobjects As Map(Of String, PDXObject) = drawer.getResources().getXObjects()
            Dim xobject As PDXObject = xobjects.get(objectName.getName())
            If (xobject Is Nothing) Then
                LOG.warn("Can't find the XObject for '" & objectName.getName() & "'")
            ElseIf (TypeOf (xobject) Is PDXObjectImage) Then
                Dim image As PDXObjectImage = xobject
                Try
                    If (image.getImageMask()) Then
                        ' set the current non stroking colorstate, so that it can
                        ' be used to create a stencil masked image
                        image.setStencilColor(drawer.getGraphicsState().getNonStrokingColor())
                    End If
                    Dim awtImage As BufferedImage = image.getRGBImage()
                    If (awtImage Is Nothing) Then
                        LOG.warn("getRGBImage returned NULL")
                        Return 'TODO PKOCH
                    End If
                    Dim imageWidth As Integer = awtImage.getWidth()
                    Dim imageHeight As Integer = awtImage.getHeight()
                    Dim pageHeight As Double = drawer.getPageSize().Height

                    LOG.debug("imageWidth: " & imageWidth & vbTab & vbTab & "timageHeight: " & imageHeight)

                    Dim ctm As Matrix = drawer.getGraphicsState().getCurrentTransformationMatrix()
                    Dim yScaling As Single = ctm.getYScale()
                    Dim angle As Single = Math.acos(ctm.getValue(0, 0) / ctm.getXScale())
                    If (ctm.getValue(0, 1) < 0 AndAlso ctm.getValue(1, 0) > 0) Then
                        angle = (-1) * angle
                    End If
                    ctm.setValue(2, 1, (pageHeight - ctm.getYPosition() - Math.Cos(angle) * yScaling))
                    ctm.setValue(2, 0, (ctm.getXPosition() - Math.Sin(angle) * yScaling))
                    ' because of the moved 0,0-reference, we have to shear in the opposite direction
                    ctm.setValue(0, 1, (-1) * ctm.getValue(0, 1))
                    ctm.setValue(1, 0, (-1) * ctm.getValue(1, 0))
                    Dim ctmAT As AffineTransform = ctm.createAffineTransform()
                    ctmAT.scale(1.0F / imageWidth, 1.0F / imageHeight)
                    drawer.drawImage(awtImage, ctmAT)
                Catch e As Exception
                    'e.printStackTrace()
                    LOG.error(e.Message, e)
                End Try
            ElseIf (TypeOf (xobject) Is PDXObjectForm) Then
                ' save the graphics state
                context.getGraphicsStack().push(context.getGraphicsState().clone())

                Dim form As PDXObjectForm = xobject
                Dim formContentstream As COSStream = form.getCOSStream()
                ' find some optional resources, instead of using the current resources
                Dim pdResources As PDResources = form.getResources()
                ' if there is an optional form matrix, we have to map the form space to the user space
                Dim matrix As Matrix = form.getMatrix()
                If (matrix IsNot Nothing) Then
                    Dim xobjectCTM As Matrix = matrix.multiply(context.getGraphicsState().getCurrentTransformationMatrix())
                    context.getGraphicsState().setCurrentTransformationMatrix(xobjectCTM)
                End If
                getContext().processSubStream(page, pdResources, formContentstream)

                ' restore the graphics state
                context.setGraphicsState(context.getGraphicsStack().pop())
            End If
        End Sub

    End Class

End Namespace

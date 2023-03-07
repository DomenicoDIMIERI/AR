Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class BeginInlineImage
        Inherits OperatorProcessor

        '   /**
        '* process : BI : begin inline image.
        '* @param operator The operator that is being executed.
        '* @param arguments List
        '* @throws IOException If there is an error displaying the inline image.
        '*/
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))
            Dim drawer As pdfviewer.PageDrawer = context
            Dim page As PDPage = drawer.getPage()
            'begin inline image object
            Dim params As ImageParameters = [operator].getImageParameters()
            Dim image As PDInlinedImage = New PDInlinedImage()
            image.setImageParameters(params)
            image.setImageData([operator].getImageData())
            Dim awtImage As BufferedImage = image.createImage(context.getColorSpaces())

            If (awtImage Is Nothing) Then
                LOG.warn("BeginInlineImage.process(): createImage returned NULL")
                Return
            End If
            Dim imageWidth As Integer = awtImage.getWidth()
            Dim imageHeight As Integer = awtImage.getHeight()
            Dim pageHeight As Double = drawer.getPageSize().Height

            Dim ctm As Matrix = drawer.getGraphicsState().getCurrentTransformationMatrix()
            Dim pageRotation As Integer = page.findRotation()

            Dim ctmAT As AffineTransform = ctm.createAffineTransform()
            ctmAT.scale(1.0F / imageWidth, 1.0F / imageHeight)
            Dim rotationMatrix As Matrix = New Matrix()
            rotationMatrix.setFromAffineTransform(ctmAT)
            '// calculate the inverse rotation angle
            '// scaleX = m00 = cos
            '// shearX = m01 = -sin
            '// tan = sin/cos
            Dim angle As Double = Math.atan(ctmAT.getShearX() / ctmAT.getScaleX())
            Dim translationMatrix As Matrix = Nothing
            If (pageRotation = 0 OrElse pageRotation = 180) Then
                translationMatrix = Matrix.getTranslatingInstance((Math.Sin(angle) * ctm.getXScale()), (pageHeight - 2 * ctm.getYPosition() - Math.Cos(angle) * ctm.getYScale()))
            ElseIf (pageRotation = 90 OrElse pageRotation = 270) Then
                translationMatrix = Matrix.getTranslatingInstance((Math.Sin(angle) * ctm.getYScale()), (pageHeight - 2 * ctm.getYPosition()))
            End If
            rotationMatrix = rotationMatrix.multiply(translationMatrix)
            rotationMatrix.setValue(0, 1, (-1) * rotationMatrix.getValue(0, 1))
            rotationMatrix.setValue(1, 0, (-1) * rotationMatrix.getValue(1, 0))
            Dim at As AffineTransform = New AffineTransform( _
                    rotationMatrix.getValue(0, 0), rotationMatrix.getValue(0, 1), _
                    rotationMatrix.getValue(1, 0), rotationMatrix.getValue(1, 1), _
                    rotationMatrix.getValue(2, 0), rotationMatrix.getValue(2, 1) _
                    )
            drawer.drawImage(awtImage, at)
        End Sub

    End Class

End Namespace
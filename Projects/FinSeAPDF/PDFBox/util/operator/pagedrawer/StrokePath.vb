Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.util.operator
Imports FinSeA.Drawings

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class StrokePath
        Inherits OperatorProcessor

    
        '/**
        ' * S stroke the path.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'dwilson 3/19/07 refactor
            Try
                Dim drawer As pdfviewer.PageDrawer = context

                Dim lineWidth As Single = context.getGraphicsState().getLineWidth()
                Dim ctm As Matrix = context.getGraphicsState().getCurrentTransformationMatrix()
                If (ctm IsNot Nothing AndAlso ctm.getXScale() > 0) Then
                    lineWidth = lineWidth * ctm.getXScale()
                End If

                Dim stroke As BasicStroke = drawer.getStroke()
                If (stroke Is Nothing) Then
                    drawer.setStroke(New BasicStroke(lineWidth))
                Else
                    drawer.setStroke(New BasicStroke(lineWidth, stroke.getEndCap(), stroke.getLineJoin(), stroke.getMiterLimit(), stroke.getDashArray(), stroke.getDashPhase()))
                End If
                drawer.strokePath()
            Catch exception As Exception
                LOG.warn(exception.Message, exception)
            End Try
        End Sub


    End Class

End Namespace

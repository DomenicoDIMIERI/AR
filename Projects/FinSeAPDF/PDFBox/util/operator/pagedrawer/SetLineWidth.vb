Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Drawings

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class SetLineWidth
        Inherits org.apache.pdfbox.util.operator.SetLineWidth

        '/**
        ' * w Set line width.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            MyBase.process([operator], arguments)
            Dim lineWidth As Single = context.getGraphicsState().getLineWidth()
            If (lineWidth = 0) Then
                lineWidth = 1
            End If
            Dim drawer As pdfviewer.PageDrawer = context
            Dim stroke As BasicStroke = drawer.getStroke()
            If (stroke Is Nothing) Then
                drawer.setStroke(New BasicStroke(lineWidth))
            Else
                drawer.setStroke(New BasicStroke(lineWidth, stroke.getEndCap(), stroke.getLineJoin(), stroke.getMiterLimit(), stroke.getDashArray(), stroke.getDashPhase()))
            End If
        End Sub

    End Class

End Namespace

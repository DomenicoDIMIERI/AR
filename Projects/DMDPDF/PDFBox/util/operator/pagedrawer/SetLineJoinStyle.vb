Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Drawings
Imports System.IO

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:andreas@lehmi.de>Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetLineJoinStyle
        Inherits FinSeA.org.apache.pdfbox.util.operator.SetLineJoinStyle

        '/**
        ' * Set the line cap style.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            MyBase.process([operator], arguments)
            Dim lineJoinStyle As Integer = context.getGraphicsState().getLineJoin()
            Dim drawer As pdfviewer.PageDrawer = context
            Dim stroke As BasicStroke = drawer.getStroke()
            If (stroke Is Nothing) Then
                drawer.setStroke(New BasicStroke(1, BasicStroke.CAP_SQUARE, lineJoinStyle))
            Else
                drawer.setStroke(New BasicStroke(stroke.getLineWidth(), stroke.getEndCap(), lineJoinStyle, stroke.getMiterLimit(), stroke.getDashArray(), stroke.getDashPhase()))
            End If
        End Sub

    End Class

End Namespace

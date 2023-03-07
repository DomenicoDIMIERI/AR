Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Drawings
Imports System.IO

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetLineMiterLimit
        Inherits org.apache.pdfbox.util.operator.SetLineMiterLimit

        '/**
        ' * Set the line dash pattern.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            MyBase.process([operator], arguments)
            Dim miterLimit As Single = context.getGraphicsState().getMiterLimit()
            Dim drawer As pdfviewer.PageDrawer = context
            Dim stroke As BasicStroke = drawer.getStroke()
            If (stroke Is Nothing) Then
                drawer.setStroke(New BasicStroke(1, BasicStroke.CAP_SQUARE, BasicStroke.JOIN_MITER, miterLimit, Nothing, 0.0F))
            Else
                drawer.setStroke(New BasicStroke(stroke.getLineWidth(), stroke.getEndCap(), stroke.getLineJoin(), miterLimit, Nothing, 0.0F))
            End If
        End Sub


    End Class

End Namespace

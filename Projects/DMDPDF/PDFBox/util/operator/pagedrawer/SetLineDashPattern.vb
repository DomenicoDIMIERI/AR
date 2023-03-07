Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
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
    Public Class SetLineDashPattern
        Inherits FinSeA.org.apache.pdfbox.util.operator.SetLineDashPattern

        '/**
        ' * Set the line dash pattern.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            MyBase.process([operator], arguments)
            Dim lineDashPattern As PDLineDashPattern = context.getGraphicsState().getLineDashPattern()
            Dim drawer As pdfviewer.PageDrawer = context
            Dim stroke As BasicStroke = drawer.getStroke()
            If (stroke Is Nothing) Then
                If (lineDashPattern.isDashPatternEmpty()) Then
                    drawer.setStroke(New BasicStroke(1, BasicStroke.CAP_SQUARE, BasicStroke.JOIN_MITER, 10.0F))
                Else
                    drawer.setStroke(New BasicStroke(1, BasicStroke.CAP_SQUARE, BasicStroke.JOIN_MITER, 10.0F, lineDashPattern.getCOSDashPattern().toFloatArray(), lineDashPattern.getPhaseStart()))
                End If
            Else
                If (lineDashPattern.isDashPatternEmpty()) Then
                    drawer.setStroke(New BasicStroke(stroke.getLineWidth(), stroke.getEndCap(), stroke.getLineJoin(), stroke.getMiterLimit()))
                Else
                    drawer.setStroke(New BasicStroke(stroke.getLineWidth(), stroke.getEndCap(), stroke.getLineJoin(), stroke.getMiterLimit(), lineDashPattern.getCOSDashPattern().toFloatArray(), lineDashPattern.getPhaseStart()))
                End If
            End If
        End Sub

    End Class


End Namespace

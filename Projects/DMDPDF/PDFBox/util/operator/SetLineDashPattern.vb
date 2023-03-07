Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class SetLineDashPattern
        Inherits OperatorProcessor

        '/**
        ' * Set the line dash pattern.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim dashArray As COSArray = arguments.get(0)
            Dim dashPhase As Integer = DirectCast(arguments.get(1), COSNumber).intValue()
            Dim lineDash As PDLineDashPattern = New PDLineDashPattern(dashArray, dashPhase)
            context.getGraphicsState().setLineDashPattern(lineDash)
        End Sub

    End Class

End Namespace

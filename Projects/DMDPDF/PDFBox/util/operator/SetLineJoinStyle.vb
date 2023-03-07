Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:andreas@lehmi.de>Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetLineJoinStyle
        Inherits OperatorProcessor

        '/**
        ' * Set the line cap style.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase)) 'throws IOException
            Dim lineJoinStyle As Integer = DirectCast(arguments.get(0), COSNumber).intValue()
            context.getGraphicsState().setLineJoin(lineJoinStyle)
        End Sub

    End Class

End Namespace

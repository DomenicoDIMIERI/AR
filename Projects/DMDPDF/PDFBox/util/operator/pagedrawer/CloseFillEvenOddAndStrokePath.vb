Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class CloseFillEvenOddAndStrokePath
        Inherits org.apache.pdfbox.util.operator.OperatorProcessor

        '/**
        ' * fill and stroke the path.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))
            ' execute ClosePath
            context.processOperator("h", arguments)
            ' execute FillEvenOddAndStroke
            context.processOperator("B*", arguments)
        End Sub

    End Class

End Namespace

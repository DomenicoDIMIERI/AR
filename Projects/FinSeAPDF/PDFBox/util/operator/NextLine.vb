Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */
    Public Class NextLine
        Inherits OperatorProcessor

        '/**
        '    * process : T* Move to start of next text line.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    *
        '    * @throws IOException If there is an error during processing.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'move to start of next text line
            Dim args As New ArrayList(Of COSBase)()
            args.add(New COSFloat(0.0F))
            ' this must be -leading instead of just leading as written in the
            ' specification (p.369) the acrobat reader seems to implement it the same way
            args.add(New COSFloat(-1 * context.getGraphicsState().getTextState().getLeading()))
            ' use Td instead of repeating code
            context.processOperator("Td", args)
        End Sub

    End Class

End Namespace

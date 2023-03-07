Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */
    Public Class MoveAndShow
        Inherits OperatorProcessor

        '/**
        '    * ' Move to next line and show text.
        '    * @param arguments List
        '    * @param operator The operator that is being executed.
        '    * @throws IOException If there is an error processing the operator.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            ' Move to start of next text line, and show text
            context.processOperator("T*", Nothing)
            context.processOperator("Tj", arguments)
        End Sub

    End Class

End Namespace

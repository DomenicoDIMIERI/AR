Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator


    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.6 $
    ' */

    Public Class SetMoveAndShow
        Inherits OperatorProcessor

        '/**
        '    * " Set word and character spacing, move to next line, and show text.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List.
        '    * @throws IOException If there is an error processing the operator.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'Set word and character spacing, move to next line, and show text
            context.processOperator("Tw", arguments.subList(0, 1))
            context.processOperator("Tc", arguments.subList(1, 2))
            context.processOperator("'", arguments.subList(2, 3))
        End Sub

    End Class

End Namespace

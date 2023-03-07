Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */
    Public Class GRestore
        Inherits OperatorProcessor

        '/**
        '    * process : Q : Restore graphics state.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            context.setGraphicsState(context.getGraphicsStack().pop())
        End Sub
    End Class

End Namespace

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */
    Public Class GSave
        Inherits OperatorProcessor

        '/**
        '    * process : q : Save graphics state.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            context.getGraphicsStack().push(context.getGraphicsState().clone())
        End Sub

    End Class

End Namespace
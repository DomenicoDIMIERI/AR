Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */
    Public Class SetTextLeading
        Inherits OperatorProcessor

        '/**
        '    * TL Set text leading.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim leading As COSNumber = arguments.get(0)
            context.getGraphicsState().getTextState().setLeading(leading.floatValue())
        End Sub

    End Class

End Namespace

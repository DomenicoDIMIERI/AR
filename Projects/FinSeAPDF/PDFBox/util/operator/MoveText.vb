Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */
    Public Class MoveText
        Inherits OperatorProcessor

        '/**
        ' * process : Td : Move text position.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim x As COSNumber = arguments.get(0)
            Dim y As COSNumber = arguments.get(1)
            Dim td As Matrix = New Matrix()
            td.setValue(2, 0, x.floatValue())
            td.setValue(2, 1, y.floatValue())
            context.setTextLineMatrix(td.multiply(context.getTextLineMatrix()))
            context.setTextMatrix(context.getTextLineMatrix().copy())
        End Sub


    End Class

End Namespace

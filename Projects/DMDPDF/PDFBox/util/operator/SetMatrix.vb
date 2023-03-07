Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util


Namespace org.apache.pdfbox.util.operator


    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */

    Public Class SetMatrix
        Inherits OperatorProcessor

        '/**
        ' * Tm Set text matrix and text line matrix.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'Set text matrix and text line matrix
            Dim a As COSNumber = arguments.get(0)
            Dim b As COSNumber = arguments.get(1)
            Dim c As COSNumber = arguments.get(2)
            Dim d As COSNumber = arguments.get(3)
            Dim e As COSNumber = arguments.get(4)
            Dim f As COSNumber = arguments.get(5)

            Dim textMatrix As Matrix = New Matrix()
            textMatrix.setValue(0, 0, a.floatValue())
            textMatrix.setValue(0, 1, b.floatValue())
            textMatrix.setValue(1, 0, c.floatValue())
            textMatrix.setValue(1, 1, d.floatValue())
            textMatrix.setValue(2, 0, e.floatValue())
            textMatrix.setValue(2, 1, f.floatValue())
            context.setTextMatrix(textMatrix)
            context.setTextLineMatrix(textMatrix.copy())
        End Sub

    End Class

End Namespace

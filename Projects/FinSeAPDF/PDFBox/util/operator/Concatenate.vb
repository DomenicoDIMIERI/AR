Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */
    Public Class Concatenate
        Inherits OperatorProcessor

        '/**
        ' * process : cm : Concatenate matrix to current transformation matrix.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If there is an error processing the operator.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'concatenate matrix to current transformation matrix
            Dim a As COSNumber = arguments.get(0)
            Dim b As COSNumber = arguments.get(1)
            Dim c As COSNumber = arguments.get(2)
            Dim d As COSNumber = arguments.get(3)
            Dim e As COSNumber = arguments.get(4)
            Dim f As COSNumber = arguments.get(5)

            Dim newMatrix As Matrix = New Matrix()
            newMatrix.setValue(0, 0, a.floatValue())
            newMatrix.setValue(0, 1, b.floatValue())
            newMatrix.setValue(1, 0, c.floatValue())
            newMatrix.setValue(1, 1, d.floatValue())
            newMatrix.setValue(2, 0, e.floatValue())
            newMatrix.setValue(2, 1, f.floatValue())

            'this line has changed
            context.getGraphicsState().setCurrentTransformationMatrix(newMatrix.multiply(context.getGraphicsState().getCurrentTransformationMatrix()))

        End Sub

    End Class

End Namespace

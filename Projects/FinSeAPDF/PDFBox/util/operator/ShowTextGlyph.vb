Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.6 $
    ' */

    Public Class ShowTextGlyph
        Inherits OperatorProcessor

        '/**
        '    * TJ Show text, allowing individual glyph positioning.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If there is an error processing this operator.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim array As COSArray = arguments.get(0)
            Dim arraySize As Integer = array.size()
            Dim fontsize As Single = context.getGraphicsState().getTextState().getFontSize()
            Dim horizontalScaling As Single = context.getGraphicsState().getTextState().getHorizontalScalingPercent() / 100
            For i As Integer = 0 To arraySize - 1
                Dim [next] As COSBase = array.get(i)
                If (TypeOf ([next]) Is COSNumber) Then
                    Dim adjustment As Single = DirectCast([next], COSNumber).floatValue()
                    Dim adjMatrix As Matrix = New Matrix()
                    adjustment = -(adjustment / 1000) * horizontalScaling * fontsize
                    ' TODO vertical writing mode
                    adjMatrix.setValue(2, 0, adjustment)
                    context.setTextMatrix(adjMatrix.multiply(context.getTextMatrix(), adjMatrix))
                ElseIf (TypeOf ([next]) Is COSString) Then
                    context.processEncodedText(DirectCast([next], COSString).getBytes())
                Else
                    Throw New IOException("Unknown type in array for TJ operation:" & [next].ToString)
                End If
            Next
        End Sub

    End Class

End Namespace

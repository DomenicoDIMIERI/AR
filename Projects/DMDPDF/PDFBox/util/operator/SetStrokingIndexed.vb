Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * 
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetStrokingIndexed
        Inherits OperatorProcessor

        '/**
        ' * scn Set color space for non stroking operations.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim colorInstance As PDColorState = context.getGraphicsState().getStrokingColor()
            Dim colorSpace As PDColorSpace = colorInstance.getColorSpace()

            If (colorSpace IsNot Nothing) Then
                Dim indexed As PDIndexed = colorSpace
                colorSpace = indexed.getBaseColorSpace()
                Dim colorValue As COSInteger = arguments.get(0)
                colorInstance.setColorSpaceValue(indexed.calculateColorValues(colorValue.intValue()))
            End If
        End Sub


    End Class

End Namespace

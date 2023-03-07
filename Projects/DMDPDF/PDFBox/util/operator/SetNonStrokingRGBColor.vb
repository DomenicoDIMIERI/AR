Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Set the non stroking color space.</p>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class SetNonStrokingRGBColor
        Inherits OperatorProcessor

        '/**
        '    * rg Set color space for non stroking operations.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim cs As PDColorSpace = PDDeviceRGB.INSTANCE
            Dim colorInstance As PDColorState = context.getGraphicsState().getNonStrokingColor()
            colorInstance.setColorSpace(cs)
            Dim values(2 - 1) As Single
            For i As Integer = 0 To arguments.size() - 1
                values(i) = DirectCast(arguments.get(i), COSNumber).floatValue()
            Next
            colorInstance.setColorSpaceValue(values)
        End Sub

    End Class

End Namespace

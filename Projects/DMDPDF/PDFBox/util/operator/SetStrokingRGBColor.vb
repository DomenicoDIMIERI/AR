Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Structal modification of the PDFEngine class :
    ' * the long sequence of conditions in processOperator is remplaced by
    ' * this strategy pattern.</p>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class SetStrokingRGBColor
        Inherits OperatorProcessor

        '/**
        '    * RG Set color space for stroking operations.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim color As PDColorState = context.getGraphicsState().getStrokingColor()
            color.setColorSpace(PDDeviceRGB.INSTANCE)
            Dim values(2 - 1) As Single
            For i As Integer = 0 To arguments.size() - 1
                values(i) = DirectCast(arguments.get(i), COSNumber).floatValue()
            Next
            color.setColorSpaceValue(values)
        End Sub


    End Class

End Namespace

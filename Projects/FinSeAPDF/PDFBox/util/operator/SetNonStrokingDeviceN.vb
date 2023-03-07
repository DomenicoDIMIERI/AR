Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util


Namespace org.apache.pdfbox.util.operator

    '/**
    ' * 
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetNonStrokingDeviceN
        Inherits OperatorProcessor

        '/**
        ' * scn Set color space for non stroking operations.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim colorInstance As PDColorState = context.getGraphicsState().getNonStrokingColor()
            Dim colorSpace As PDColorSpace = colorInstance.getColorSpace()

            If (colorSpace IsNot Nothing) Then
                Dim sep As PDDeviceN = colorSpace
                colorSpace = sep.getAlternateColorSpace()
                Dim colorValues As COSArray = sep.calculateColorValues(arguments)
                colorInstance.setColorSpaceValue(colorValues.toFloatArray())
            End If
        End Sub

    End Class

End Namespace
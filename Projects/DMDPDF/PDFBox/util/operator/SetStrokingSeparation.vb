Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * 
    ' * @author <a href="mailto:WilliamstonConsulting@GMail.com">Daniel Wilson</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetStrokingSeparation
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
                Dim sep As PDSeparation = colorSpace
                colorSpace = sep.getAlternateColorSpace()
                Dim values As COSArray = sep.calculateColorValues(arguments.get(0))
                colorInstance.setColorSpaceValue(values.toFloatArray())
            End If
        End Sub

    End Class

End Namespace

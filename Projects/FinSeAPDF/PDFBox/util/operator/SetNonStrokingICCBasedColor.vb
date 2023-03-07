Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * 
    ' * @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetNonStrokingICCBasedColor
        Inherits OperatorProcessor

        '/**
        '    * scn Set color space for non stroking operations.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim colorInstance As PDColorState = context.getGraphicsState().getNonStrokingColor()
            Dim cs As PDColorSpace = colorInstance.getColorSpace()
            Dim numberOfComponents As Integer = cs.getNumberOfComponents()
            Dim values() As Single = Array.CreateInstance(GetType(Single), numberOfComponents)
            For i As Integer = 0 To numberOfComponents - 1
                values(i) = DirectCast(arguments.get(i), COSNumber).floatValue()
            Next
            colorInstance.setColorSpaceValue(values)
        End Sub

    End Class

End Namespace

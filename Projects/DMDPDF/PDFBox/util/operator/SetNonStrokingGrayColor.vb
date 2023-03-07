Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Set the non stroking color space.</p>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class SetNonStrokingGrayColor
        Inherits OperatorProcessor

        '/**
        '    * rg Set color space for non stroking operations.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim cs As PDColorSpace = New PDDeviceGray()
            Dim colorInstance As PDColorState = context.getGraphicsState().getNonStrokingColor()
            colorInstance.setColorSpace(cs)
            Dim values(1 - 1) As Single
            If (arguments.size() >= 1) Then
                values(0) = DirectCast(arguments.get(0), COSNumber).floatValue()
            Else
                Throw New IOException("Error: Expected at least one argument when setting non stroking gray color")
            End If
            colorInstance.setColorSpaceValue(values)
        End Sub

    End Class

End Namespace

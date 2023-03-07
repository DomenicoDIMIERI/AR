Imports System.IO
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
    ' * @version $Revision: 1.1 $
    ' */
    Public Class SetStrokingGrayColor
        Inherits OperatorProcessor

        '/**
        '    * RG Set color space for stroking operations.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim color As PDColorState = context.getGraphicsState().getStrokingColor()
            color.setColorSpace(New PDDeviceGray())
            Dim values(1 - 1) As Single '= new Single(1);
            If (arguments.size() >= 1) Then
                values(0) = DirectCast(arguments.get(0), COSNumber).floatValue()
            Else
                Throw New IOException("Error: Expected at least one argument when setting non stroking gray color")
            End If
            color.setColorSpaceValue(values)
        End Sub


    End Class

End Namespace

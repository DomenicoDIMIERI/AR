Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * Sets the non stroking color values for Lab colorspace.
    ' * 
    ' */
    Public Class SetNonStrokingLabColor
        Inherits OperatorProcessor

        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim colorInstance As PDColorState = context.getGraphicsState().getNonStrokingColor()
            Dim values(2 - 1) As Single
            For i As Integer = 0 To arguments.size() - 1
                values(i) = DirectCast(arguments.get(i), COSNumber).floatValue()
            Next
            colorInstance.setColorSpaceValue(values)
        End Sub

    End Class

End Namespace

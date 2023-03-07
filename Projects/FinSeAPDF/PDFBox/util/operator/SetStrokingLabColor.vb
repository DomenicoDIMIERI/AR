Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * Sets the stroking color values for Lab colorspace.  
    ' * 
    ' */
    Public Class SetStrokingLabColor
        Inherits OperatorProcessor


        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim color As PDColorState = context.getGraphicsState().getStrokingColor()
            Dim values(2 - 1) As Single
            For i As Integer = 0 To arguments.size() - 1
                values(i) = DirectCast(arguments.get(i), COSNumber).floatValue()
            Next
            color.setColorSpaceValue(values)
        End Sub

    End Class

End Namespace

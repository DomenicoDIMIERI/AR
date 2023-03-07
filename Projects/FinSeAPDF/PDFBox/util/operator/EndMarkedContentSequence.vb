Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * EMC : Ends a marked-content sequence begun by BMC or BDC.
    ' * @author koch
    ' * @version $Revision: $
    ' */
    Public Class EndMarkedContentSequence
        Inherits OperatorProcessor


        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            If (TypeOf (Me.context) Is PDFMarkedContentExtractor) Then
                DirectCast(Me.context, PDFMarkedContentExtractor).endMarkedContentSequence()
            End If
        End Sub


    End Class

End Namespace
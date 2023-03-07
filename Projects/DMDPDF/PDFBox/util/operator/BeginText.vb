Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */
    Public Class BeginText
        Inherits OperatorProcessor

        '/**
        '    * process : BT : Begin text object.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            context.setTextMatrix(New Matrix())
            context.setTextLineMatrix(New Matrix())
        End Sub

    End Class

End Namespace

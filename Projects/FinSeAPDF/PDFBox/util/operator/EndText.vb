Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */
    Public Class EndText
        Inherits OperatorProcessor

        '/**
        ' * process : ET : End text object.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            context.setTextMatrix(Nothing)
            context.setTextLineMatrix(Nothing)
        End Sub

    End Class

End Namespace

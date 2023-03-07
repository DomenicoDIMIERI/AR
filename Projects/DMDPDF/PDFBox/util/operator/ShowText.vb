Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO.IOException

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.4 $
    ' */
    Public Class ShowText
        Inherits OperatorProcessor

        '/**
        ' * Tj show Show text.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If there is an error processing this operator.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim [string] As COSString = arguments.get(0)
            context.processEncodedText([string].getBytes())
        End Sub

    End Class

End Namespace

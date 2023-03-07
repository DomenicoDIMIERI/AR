Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator


    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */
    Public Class MoveTextSetLeading
        Inherits OperatorProcessor

        '/**
        ' * process : TD Move text position and set leading.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If there is an error during processing.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of cos.COSBase))
            'move text position and set leading
            Dim y As COSNumber = arguments.get(1)

            Dim args As New ArrayList(Of COSBase)()
            args.add(New COSFloat(-1 * y.floatValue()))
            context.processOperator("TL", args)
            context.processOperator("Td", arguments)

        End Sub
    End Class

End Namespace

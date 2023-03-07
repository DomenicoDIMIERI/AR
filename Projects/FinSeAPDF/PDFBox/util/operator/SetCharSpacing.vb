Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' *
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */
    Public Class SetCharSpacing
        Inherits OperatorProcessor

        '/**
        '    * process : Tc Set character spacing.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'set character spacing
            If (arguments.size() > 0) Then
                'There are some documents which are incorrectly structured, and have
                'a wrong number of arguments to this, so we will assume the last argument
                'in the list
                Dim charSpacing As Object = arguments.get(arguments.size() - 1)
                If (TypeOf (charSpacing) Is COSNumber) Then
                    Dim characterSpacing As COSNumber = charSpacing
                    context.getGraphicsState().getTextState().setCharacterSpacing(characterSpacing.floatValue())
                End If
            End If
        End Sub


    End Class

End Namespace

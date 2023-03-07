Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator


    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.5 $
    ' */

    Public Class SetTextFont
        Inherits OperatorProcessor

        '/**
        '    * Tf selectfont Set text font and size.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'there are some documents that are incorrectly structured and
            'arguments are in the wrong spot, so we will silently ignore them
            'if there are no arguments
            If (arguments.size() >= 2) Then
                'set font and size
                Dim fontName As COSName = arguments.get(0)
                Dim fontSize As Single = DirectCast(arguments.get(1), COSNumber).floatValue()
                context.getGraphicsState().getTextState().setFontSize(fontSize)

                context.getGraphicsState().getTextState().setFont(DirectCast(context.getFonts().get(fontName.getName()), PDFont))
                If (context.getGraphicsState().getTextState().getFont() Is Nothing) Then
                    Throw New System.IO.IOException("Error: Could not find font(" & fontName.toString & ") in map=" & context.getFonts().ToString)
                End If
            End If
        End Sub

    End Class

End Namespace

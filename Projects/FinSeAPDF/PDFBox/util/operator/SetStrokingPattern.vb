Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Set the stroking pattern.</p>
    ' *
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetStrokingPattern
        Inherits OperatorProcessor

        '/**
        '    * Set pattern instead of a color space for stroking operations.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the pattern.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim selectedPattern As COSName
            Dim numberOfArguments As Integer = arguments.size()
            Dim colorValues As COSArray
            If (numberOfArguments = 1) Then
                selectedPattern = arguments.get(0)
            Else
                ' uncolored tiling patterns shall have some additional color values
                ' TODO: pass these values to the colorstate
                colorValues = New COSArray()
                For i As Integer = 0 To numberOfArguments - 2
                    colorValues.add(arguments.get(i))
                Next
                selectedPattern = arguments.get(numberOfArguments - 1)
            End If
            Dim patterns As Map(Of String, PDPatternResources) = getContext().getResources().getPatterns()
            Dim pattern As PDPatternResources = patterns.get(selectedPattern.getName())
            getContext().getGraphicsState().getStrokingColor().setPattern(pattern)
        End Sub

    End Class

End Namespace

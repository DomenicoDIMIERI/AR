Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO.IOException

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Structal modification of the PDFEngine class :
    ' * the long sequence of conditions in processOperator is remplaced by
    ' * this strategy pattern.</p>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class SetTextRenderingMode
        Inherits OperatorProcessor

        '/**
        '    * Tr Set text rendering mode.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim mode As COSNumber = arguments.get(0)
            context.getGraphicsState().getTextState().setRenderingMode(mode.intValue())
        End Sub

    End Class

End Namespace

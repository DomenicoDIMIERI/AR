Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class CloseAndStrokePath
        Inherits OperatorProcessor

        '/**
        '    * s close and stroke the path.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    *
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            context.processOperator("h", arguments)
            context.processOperator("S", arguments)
        End Sub


    End Class

End Namespace

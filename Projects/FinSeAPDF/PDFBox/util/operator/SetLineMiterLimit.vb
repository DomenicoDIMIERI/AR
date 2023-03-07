Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util


Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Structal modification of the PDFEngine class :
    ' * the long sequence of conditions in processOperator is remplaced by
    ' * this strategy pattern.</p>
    ' *
    ' * @author <a href="mailto:andreas@lehmi.de">Andreas Lehmkühler</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetLineMiterLimit
        Inherits OperatorProcessor

        '/**
        '    * w Set miter limit.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim miterLimit As COSNumber = arguments.get(0)
            context.getGraphicsState().setMiterLimit(miterLimit.doubleValue())
        End Sub


    End Class

End Namespace

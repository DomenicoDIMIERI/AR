Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator
    '/**
    ' * <p>Structal modification of the PDFEngine class :
    ' * the long sequence of conditions in processOperator is remplaced by
    ' * this strategy pattern.</p>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class SetGraphicsStateParameters
        Inherits OperatorProcessor

        '/**
        '    * gs Set parameters from graphics state parameter dictionary.
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    * @throws IOException If an error occurs while processing the font.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            'set parameters from graphics state parameter dictionary
            Dim graphicsName As COSName = arguments.get(0)
            Dim gs As PDExtendedGraphicsState = context.getGraphicsStates().get(graphicsName.getName())
            gs.copyIntoGraphicsState(context.getGraphicsState())
        End Sub

    End Class

End Namespace

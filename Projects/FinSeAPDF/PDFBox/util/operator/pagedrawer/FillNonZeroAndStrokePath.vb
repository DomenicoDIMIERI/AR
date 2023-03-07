Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Drawings
Imports System.IO

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class FillNonZeroAndStrokePath
        Inherits FinSeA.org.apache.pdfbox.util.operator.OperatorProcessor

        '/**
        ' * fill and stroke the path.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException If an error occurs while processing the font.
        ' */

        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim drawer As pdfviewer.PageDrawer = context
            Dim currentPath As GeneralPath = drawer.getLinePath().clone()

            context.processOperator("f", arguments)
            drawer.setLinePath(currentPath)

            context.processOperator("S", arguments)
        End Sub

    End Class

End Namespace
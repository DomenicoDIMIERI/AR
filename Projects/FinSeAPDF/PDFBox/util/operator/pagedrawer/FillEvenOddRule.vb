Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.util.operator

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class FillEvenOddRule
        Inherits OperatorProcessor

    
        '/**
        ' * process : f* : fill path using even odd rule.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException if there is an error during execution.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Try
                'dwilson refactoring
                Dim drawer As pdfviewer.PageDrawer = context
                drawer.fillPath(GeneralPath.WIND_EVEN_ODD)
            Catch e As Exception
                LOG.warn(e.Message, e)
            End Try
        End Sub
    End Class
End Namespace

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
    ' * @author <a href="mailto:Daniel.Wilson@BlackLocustSoftware.com">Daniel Wilson</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class ClipEvenOddRule
        Inherits OperatorProcessor

    
        '/**
        ' * process : W* : set clipping path using even odd rule.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' *
        ' * @throws IOException if there is an error during execution.
        ' */
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))
    
            Try
                Dim drawer As pdfviewer.PageDrawer = context
                drawer.setClippingWindingRule(GeneralPath.WIND_EVEN_ODD)
            Catch e As Exception
                LOG.warn(e.Message, e)
            End Try
        End Sub

    End Class

End Namespace
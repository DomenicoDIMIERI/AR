Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.util.operator

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of sh operator for page drawer.
    ' *  See section 4.6.3 of the PDF 1.7 specification.
    ' *
    ' * @author <a href="mailto:Daniel.Wilson@BlackLocustSoftware.com">Daniel Wilson</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SHFill
        Inherits OperatorProcessor

        '   /**
        '* process : sh : shade fill the clipping area.
        '* @param operator The operator that is being executed.
        '* @param arguments List
        '*
        '* @throws IOException if there is an error during execution.
        '*/
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Try
                Dim drawer As pdfviewer.PageDrawer = context
                drawer.shFill(arguments.get(0))
            Catch e As Exception
                LOG.warn(e.Message, e)
            End Try
        End Sub
    End Class

End Namespace

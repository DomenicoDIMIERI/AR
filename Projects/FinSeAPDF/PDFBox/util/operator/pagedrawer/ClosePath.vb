Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.util.operator

Namespace org.apache.pdfbox.util.operator.pagedrawer

    '/**
    ' * Implementation of content stream operator for page drawer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class ClosePath
        Inherits OperatorProcessor

        '   /**
        '* process : h : Close path.
        '* @param operator The operator that is being executed.
        '* @param arguments List
        '* 
        '* @throws IOException if something went wrong during logging
        '*/
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))  'throws IOException
            Dim drawer As pdfviewer.PageDrawer = context
            Try
                drawer.getLinePath().closePath()
            Catch t As System.Exception
                LOG.warn(t.Message, t)
            End Try
        End Sub

    End Class

End Namespace


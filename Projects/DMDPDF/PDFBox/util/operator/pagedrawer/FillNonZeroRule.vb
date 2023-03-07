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
    Public Class FillNonZeroRule
        Inherits OperatorProcessor

        '   /**
        '* process : F/f : fill path using non zero winding rule.
        '* @param operator The operator that is being executed.
        '* @param arguments List
        '*
        '* @throws IOException If there is an error during the processing.
        '*/

        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Try
                'dwilson refactoring
                Dim drawer As pdfviewer.PageDrawer = context
                drawer.fillPath(GeneralPath.WIND_NON_ZERO)
            Catch e As Exception
                LOG.warn(e.Message, e)
            End Try
        End Sub

    End Class

End Namespace

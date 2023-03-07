Imports FinSeA.Drawings
Imports System.Drawing
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
    ' * @version $Revision: 1.2 $
    ' */
    Public Class MoveTo
        Inherits OperatorProcessor

    
        '/**
        ' * process : m : Begin new subpath.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If there is an error processing the operator.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Try
                Dim drawer As pdfviewer.PageDrawer = context
                Dim x As COSNumber = arguments.get(0)
                Dim y As COSNumber = arguments.get(1)
                Dim pos As PointF = drawer.transformedPoint(x.doubleValue(), y.doubleValue())
                drawer.getLinePath().moveTo(pos.X, pos.Y)
            Catch exception As Exception
                LOG.warn(exception.Message, exception)
            End Try
        End Sub

    End Class

End Namespace

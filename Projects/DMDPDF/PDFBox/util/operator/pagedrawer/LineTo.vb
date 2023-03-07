Imports FinSeA.Drawings
Imports System.Drawing
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
    Public Class LineTo
        Inherits OperatorProcessor


        '/**
        ' * process : l : Append straight line segment to path.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim drawer As pdfviewer.PageDrawer = context

            'append straight line segment from the current point to the point.
            Dim x As COSNumber = arguments.get(0)
            Dim y As COSNumber = arguments.get(1)

            Dim pos As PointF = drawer.transformedPoint(x.doubleValue(), y.doubleValue())
            drawer.getLinePath().lineTo(pos.X, pos.Y)
        End Sub

    End Class

End Namespace

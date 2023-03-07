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
    Public Class CurveToReplicateInitialPoint
        Inherits OperatorProcessor


        '/**
        ' * process : v : Append curved segment to path (initial point replicated).
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' */
        Public Overrides Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))
            Dim drawer As pdfviewer.PageDrawer = context

            Dim x2 As COSNumber = arguments.get(0)
            Dim y2 As COSNumber = arguments.get(1)
            Dim x3 As COSNumber = arguments.get(2)
            Dim y3 As COSNumber = arguments.get(3)
            Dim path As GeneralPath = drawer.getLinePath()
            Dim currentPoint As PointF = path.getCurrentPoint()

            Dim point2 As PointF = drawer.transformedPoint(x2.doubleValue(), y2.doubleValue())
            Dim point3 As PointF = drawer.transformedPoint(x3.doubleValue(), y3.doubleValue())

            drawer.getLinePath().curveTo(currentPoint.X, currentPoint.Y, point2.X, point2.Y, point3.X, point3.Y)
        End Sub


    End Class

End Namespace

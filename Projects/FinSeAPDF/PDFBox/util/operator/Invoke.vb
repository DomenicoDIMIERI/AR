Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * Invoke named XObject.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Mario Ivankovits
    ' *
    ' * @version $Revision: 1.9 $
    ' */
    Public Class Invoke
        Inherits OperatorProcessor

        '/**
        '    * process : Do - Invoke a named xobject.
        '    * 
        '    * @param operator The operator that is being executed.
        '    * @param arguments List
        '    *
        '    * @throws IOException If there is an error processing this operator.
        '    */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim name As COSName = arguments.get(0)

            Dim xobjects As Map(Of String, PDXObject) = context.getXObjects()
            Dim xobject As PDXObject = xobjects.get(name.getName())
            If (TypeOf (context) Is PDFMarkedContentExtractor) Then
                DirectCast(context, PDFMarkedContentExtractor).xobject(xobject)
            End If

            If (TypeOf (xobject) Is PDXObjectForm) Then
                Dim form As PDXObjectForm = xobject
                Dim formContentstream As COSStream = form.getCOSStream()
                ' if there is an optional form matrix, we have to map the form space to the user space
                Dim matrix As Matrix = form.getMatrix()
                If (Matrix IsNot Nothing) Then
                    Dim xobjectCTM As Matrix = matrix.multiply(context.getGraphicsState().getCurrentTransformationMatrix())
                    context.getGraphicsState().setCurrentTransformationMatrix(xobjectCTM)
                End If
                ' find some optional resources, instead of using the current resources
                Dim pdResources As PDResources = form.getResources()
                context.processSubStream(context.getCurrentPage(), pdResources, formContentstream)
            End If
        End Sub

    End Class

End Namespace
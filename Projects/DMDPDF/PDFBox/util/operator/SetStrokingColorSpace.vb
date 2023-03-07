Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Structal modification of the PDFEngine class :
    ' * the long sequence of conditions in processOperator is remplaced by
    ' * this strategy pattern.</p>
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class SetStrokingColorSpace
        Inherits OperatorProcessor

        Private Shared EMPTY_FLOAT_ARRAY() As Single = Nothing

        '/**
        ' * CS Set color space for stroking operations.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
			'(PDF 1.1) Set color space for stroking operations
            Dim name As COSName = arguments.get(0)
            Dim cs As PDColorSpace = PDColorSpaceFactory.createColorSpace(name, context.getColorSpaces(), context.getResources().getPatterns())
            Dim color As PDColorState = context.getGraphicsState().getStrokingColor()
            color.setColorSpace(cs)
            Dim numComponents As Integer = cs.getNumberOfComponents()
            Dim values() As Single = EMPTY_FLOAT_ARRAY
            If (numComponents >= 0) Then
                values = Array.CreateInstance(GetType(Single), numComponents)
                For i As Integer = 0 To numComponents - 1
                    values(i) = 0.0F
                Next
                If (TypeOf (cs) Is PDDeviceCMYK) Then
                    values(2) = 1.0F
                End If
            End If
            color.setColorSpaceValue(values)
        End Sub

    End Class

End Namespace
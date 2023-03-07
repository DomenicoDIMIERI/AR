Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * <p>Set the stroking color space.</p>
    ' * 
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetStrokingColor
        Inherits OperatorProcessor

    
        '/**
        ' * SC,SCN Set color space for stroking operations.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim colorSpace As PDColorSpace = context.getGraphicsState().getStrokingColor().getColorSpace()
            If (colorSpace IsNot Nothing) Then
                Dim newOperator As OperatorProcessor = Nothing
                If (TypeOf (colorSpace) Is PDDeviceGray) Then
                    newOperator = New SetStrokingGrayColor()
                ElseIf (TypeOf (colorSpace) Is PDDeviceRGB) Then
                    newOperator = New SetStrokingRGBColor()
                ElseIf (TypeOf (colorSpace) Is PDDeviceCMYK) Then
                    newOperator = New SetStrokingCMYKColor()
                ElseIf (TypeOf (colorSpace) Is PDICCBased) Then
                    newOperator = New SetStrokingICCBasedColor()
                ElseIf (TypeOf (colorSpace) Is PDCalRGB) Then
                    newOperator = New SetStrokingCalRGBColor()
                ElseIf (TypeOf (colorSpace) Is PDSeparation) Then
                    newOperator = New SetStrokingSeparation()
                ElseIf (TypeOf (colorSpace) Is PDDeviceN) Then
                    newOperator = New SetStrokingDeviceN()
                ElseIf (TypeOf (colorSpace) Is PDPattern) Then
                    newOperator = New SetStrokingPattern()
                ElseIf (TypeOf (colorSpace) Is PDIndexed) Then
                    newOperator = New SetStrokingIndexed()
                ElseIf (TypeOf (colorSpace) Is PDLab) Then
                    newOperator = New SetStrokingLabColor()
                End If

                If (newOperator IsNot Nothing) Then
                    newOperator.setContext(getContext())
                    newOperator.process([operator], arguments)
                Else
                    LOG.info("Not supported colorspace " & colorSpace.getName() & " within operator " & [operator].getOperation())
                End If
            Else
                LOG.warn("Colorspace not found in " & Me.GetType.Name & ".process!!")
            End If
        End Sub

    End Class

End Namespace

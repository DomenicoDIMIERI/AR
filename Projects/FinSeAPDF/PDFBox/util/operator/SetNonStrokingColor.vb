Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator


    '/**
    ' * <p>Set the non stroking color space.</p>
    ' * 
    ' * @version $Revision: 1.0 $
    ' */
    Public Class SetNonStrokingColor
        Inherits OperatorProcessor

    
        '/**
        ' * sc,scn Set color space for non stroking operations.
        ' * @param operator The operator that is being executed.
        ' * @param arguments List
        ' * @throws IOException If an error occurs while processing the font.
        ' */
        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim colorSpace As PDColorSpace = context.getGraphicsState().getNonStrokingColor().getColorSpace()
            If (colorSpace IsNot Nothing) Then
                Dim newOperator As OperatorProcessor = Nothing
                If (TypeOf (colorSpace) Is PDDeviceGray) Then
                    newOperator = New SetNonStrokingGrayColor()
                ElseIf (TypeOf (colorSpace) Is PDDeviceRGB) Then
                    newOperator = New SetNonStrokingRGBColor()
                ElseIf (TypeOf (colorSpace) Is PDDeviceCMYK) Then
                    newOperator = New SetNonStrokingCMYKColor()
                ElseIf (TypeOf (colorSpace) Is PDICCBased) Then
                    newOperator = New SetNonStrokingICCBasedColor()
                ElseIf (TypeOf (colorSpace) Is PDCalRGB) Then
                    newOperator = New SetNonStrokingCalRGBColor()
                ElseIf (TypeOf (colorSpace) Is PDSeparation) Then
                    newOperator = New SetNonStrokingSeparation()
                ElseIf (TypeOf (colorSpace) Is PDDeviceN) Then
                    newOperator = New SetNonStrokingDeviceN()
                ElseIf (TypeOf (colorSpace) Is PDPattern) Then
                    newOperator = New SetNonStrokingPattern()
                ElseIf (TypeOf (colorSpace) Is PDIndexed) Then
                    newOperator = New SetNonStrokingIndexed()
                ElseIf (TypeOf (colorSpace) Is PDLab) Then
                    newOperator = New SetNonStrokingLabColor()
                End If
                If (newOperator IsNot Nothing) Then
                    newOperator.setContext(getContext())
                    newOperator.process([operator], arguments)
                Else
                    LOG.warn("Not supported colorspace " & colorSpace.getName() & " within operator " & [operator].getOperation())
                End If
            Else
                LOG.warn("Colorspace not found in " & Me.GetType.Name & ".process!!")
            End If
        End Sub

    End Class

End Namespace

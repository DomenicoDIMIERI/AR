Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * BDC : Begins a marked-content sequence with property list.
    ' *
    ' * @author koch
    ' * @version $Revision$
    ' */
    Public Class BeginMarkedContentSequenceWithProperties
        Inherits OperatorProcessor


        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim tag As COSName = Nothing
            Dim properties As COSDictionary = Nothing
            For Each argument As COSBase In arguments
                If (TypeOf (argument) Is COSName) Then
                    tag = argument
                ElseIf (TypeOf (argument) Is COSDictionary) Then
                    properties = argument
                End If
            Next
            If (TypeOf (Me.context) Is PDFMarkedContentExtractor) Then
                DirectCast(Me.context, PDFMarkedContentExtractor).beginMarkedContentSequence(tag, properties)
            End If
        End Sub

    End Class

End Namespace
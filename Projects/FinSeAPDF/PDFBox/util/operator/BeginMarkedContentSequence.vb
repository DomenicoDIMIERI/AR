Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator
    '/**
    ' * BMC : Begins a marked-content sequence.
    ' * @author koch
    ' * @version $Revision$
    ' *
    ' */
    Public Class BeginMarkedContentSequence
        Inherits OperatorProcessor

        Public Overrides Sub process([operator] As PDFOperator, arguments As List(Of COSBase))
            Dim tag As COSName = Nothing
            For Each argument As COSBase In arguments
                If (TypeOf (argument) Is COSName) Then
                    tag = argument
                End If
            Next
            If (TypeOf (Me.context) Is PDFMarkedContentExtractor) Then
                DirectCast(Me.context, PDFMarkedContentExtractor).beginMarkedContentSequence(tag, Nothing)
            End If
        End Sub

    End Class

End Namespace

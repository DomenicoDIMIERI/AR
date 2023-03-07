Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.markedcontent

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf

    '/**
    ' * An artifact marked content.
    ' *
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' *
    ' */
    Public Class PDArtifactMarkedContent
        Inherits PDMarkedContent

        Public Sub New(ByVal properties As COSDictionary)
            MyBase.New(COSName.ARTIFACT, properties)
        End Sub


        '/**
        ' * Gets the type (Type).
        ' * 
        ' * @return the type
        ' */
        Public Function getContentType() As String
            Return Me.getProperties().getNameAsString(COSName.TYPE)
        End Function

        '/**
        ' * Gets the artifact's bounding box (BBox).
        ' * 
        ' * @return the artifact's bounding box
        ' */
        Public Function getBBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim a As COSArray = Me.getProperties().getDictionaryObject(COSName.BBOX)
            If (a IsNot Nothing) Then
                retval = New PDRectangle(a)
            End If
            Return retval
        End Function

        '/**
        ' * Is the artifact attached to the top edge?
        ' * 
        ' * @return <code>true</code> if the artifact is attached to the top edge,
        ' * <code>false</code> otherwise
        ' */
        Public Function isTopAttached() As Boolean
            Return Me.isAttached("Top")
        End Function

        '/**
        ' * Is the artifact attached to the bottom edge?
        ' * 
        ' * @return <code>true</code> if the artifact is attached to the bottom edge,
        ' * <code>false</code> otherwise
        ' */
        Public Function isBottomAttached() As Boolean
            Return Me.isAttached("Bottom")
        End Function

        '/**
        ' * Is the artifact attached to the left edge?
        ' * 
        ' * @return <code>true</code> if the artifact is attached to the left edge,
        ' * <code>false</code> otherwise
        ' */
        Public Function isLeftAttached() As Boolean
            Return Me.isAttached("Left")
        End Function

        '/**
        ' * Is the artifact attached to the right edge?
        ' * 
        ' * @return <code>true</code> if the artifact is attached to the right edge,
        ' * <code>false</code> otherwise
        ' */
        Public Function isRightAttached() As Boolean
            Return Me.isAttached("Right")
        End Function

        '/**
        ' * Gets the subtype (Subtype).
        ' * 
        ' * @return the subtype
        ' */
        Public Function getSubtype() As String
            Return Me.getProperties().getNameAsString(COSName.SUBTYPE)
        End Function


        '/**
        ' * Is the artifact attached to the given edge?
        ' * 
        ' * @param edge the edge
        ' * @return <code>true</code> if the artifact is attached to the given edge,
        ' * <code>false</code> otherwise
        ' */
        Private Function isAttached(ByVal edge As String) As Boolean
            Dim a As COSArray = Me.getProperties().getDictionaryObject(COSName.ATTACHED)
            If (a IsNot Nothing) Then
                For i As Integer = 0 To a.size() - 1

                    If (edge.Equals(a.getName(i))) Then
                        Return True
                    End If
                Next
            End If
            Return False
        End Function

    End Class

End Namespace
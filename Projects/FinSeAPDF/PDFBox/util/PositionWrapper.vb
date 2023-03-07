Namespace org.apache.pdfbox.util

    '/**
    ' * wrapper of TextPosition that adds flags to track
    ' * status as linestart and paragraph start positions.
    ' * <p>
    ' * This is implemented as a wrapper since the TextPosition
    ' * class doesn't provide complete access to its
    ' * state fields to subclasses.  Also, conceptually TextPosition is
    ' * immutable while these flags need to be set post-creation so
    ' * it makes sense to put these flags in this separate class.
    ' * </p>
    ' * @author m.martinez@ll.mit.edu
    ' *
    ' */
    Public Class PositionWrapper


        Private _isLineStart As Boolean = False
        Private _isParagraphStart As Boolean = False
        Private _isPageBreak As Boolean = False
        Private _isHangingIndent As Boolean = False
        Private _isArticleStart As Boolean = False

        Private _position As TextPosition = Nothing

        '/**
        ' * Returns the underlying TextPosition object.
        ' * @return the text position
        ' */
        Public Function getTextPosition() As TextPosition
            Return _position
        End Function


        Public Function isLineStart() As Boolean
            Return _isLineStart
        End Function

        '/**
        ' * Sets the isLineStart() flag to true.
        ' */
        Public Sub setLineStart()
            Me._isLineStart = True
        End Sub

        Public Function isParagraphStart() As Boolean
            Return _isParagraphStart
        End Function


        '/**
        ' * sets the isParagraphStart() flag to true.
        ' */
        Public Sub setParagraphStart()
            Me._isParagraphStart = True
        End Sub


        Public Function isArticleStart() As Boolean
            Return _isArticleStart
        End Function


        '/**
        ' * Sets the isArticleStart() flag to true.
        ' */
        Public Sub setArticleStart()
            Me._isArticleStart = True
        End Sub


        Public Function isPageBreak() As Boolean
            Return _isPageBreak
        End Function


        '/**
        ' * Sets the isPageBreak() flag to true.
        ' */
        Public Sub setPageBreak()
            Me._isPageBreak = True
        End Sub

        Public Function isHangingIndent() As Boolean
            Return _isHangingIndent
        End Function

        ' /**
        '* Sets the isHangingIndent() flag to true.
        '*/
        Public Sub setHangingIndent()
            Me._isHangingIndent = True
        End Sub

        ' /**
        '* Constructs a PositionWrapper around the specified TextPosition object.
        '* @param position the text position
        '*/
        Public Sub New(ByVal position As TextPosition)
            Me._position = position
        End Sub

    End Class

End Namespace

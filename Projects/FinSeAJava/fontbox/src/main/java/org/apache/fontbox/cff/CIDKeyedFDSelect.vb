Namespace org.apache.fontbox.cff

    Public MustInherit Class CIDKeyedFDSelect

        Protected owner As CFFFontROS = Nothing

        '/**
        ' * Constructor.
        ' * @param _owner the owner of the FDSelect data.
        ' */
        Public Sub New(ByVal _owner As CFFFontROS)
            Me.owner = _owner
        End Sub

        ''' <summary>
        '''  Returns the Font DICT index for the given glyph identifier
        ''' </summary>
        ''' <param name="glyph"></param>
        ''' <returns>return -1 if the glyph isn't define, otherwise the FD index value</returns>
        ''' <remarks></remarks>
        Public MustOverride Function getFd(ByVal glyph As Integer) As Integer

    End Class

End Namespace

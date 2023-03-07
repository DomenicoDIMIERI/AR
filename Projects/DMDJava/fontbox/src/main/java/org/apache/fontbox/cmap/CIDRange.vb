Namespace org.apache.fontbox.cmap

    '/**
    ' * Range of continuous CIDs between two Unicode characters.
    ' */
    Class CIDRange

        Private from As Char

        Private [to] As Char

        Private cid As Integer

        Public Sub New(ByVal from As Char, ByVal [to] As Char, ByVal cid As Integer)
            Me.from = from
            Me.to = [to]
            Me.cid = cid
        End Sub

        '/**
        ' * Maps the given Unicode character to the corresponding CID in Me range.
        ' *
        ' * @param ch Unicode character
        ' * @return corresponding CID, or -1 if the character is out of range
        ' */
        Public Function map(ByVal ch As Char) As Integer
            If (from <= ch AndAlso ch <= [to]) Then
                Return cid + (AscW(ch) - AscW([from]))
            Else
                Return -1
            End If
        End Function

        '/**
        ' * Maps the given CID to the corresponding Unicode character in Me range.
        ' *
        ' * @param code CID
        ' * @return corresponding Unicode character, or -1 if the CID is out of range
        ' */
        Public Function unmap(ByVal code As Integer) As Integer
            If (cid <= code AndAlso code <= cid + (AscW([to]) - AscW(from))) Then
                Return AscW(from) + (code - cid)
            Else
                Return -1
            End If
        End Function

    End Class

End Namespace
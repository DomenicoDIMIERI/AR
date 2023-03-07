Imports System.Drawing

Namespace PDF

    Public Class PDFFontDescriptor
        Public Ascent As Single
        Public Descent As Single
        Public CapHeight As Single
        Public Flags As Integer
        Public FontBBox As System.Drawing.RectangleF
        Public ItalicAngle As Single
        Public StemV As Single
        Public MissingWidth As Single

        Public Sub New(ByVal ascent As Single, ByVal descent As Single, ByVal capHeight As Single, ByVal flags As Integer, ByVal fontBox As RectangleF, ByVal italicAngle As Single, ByVal stemV As Single, ByVal missingWidth As Single)
            Me.Ascent = ascent
            Me.Descent = descent
            Me.CapHeight = capHeight
            Me.Flags = flags
            Me.FontBBox = fontBox
            Me.ItalicAngle = italicAngle
            Me.StemV = stemV
            Me.MissingWidth = missingWidth
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String = vbNullString
            ret &= " /Ascent " & Me.Ascent.ToString("US-EN")
            ret &= " /Descent " & Me.Descent.ToString("US-EN")
            ret &= " /CapHeight " & Me.CapHeight.ToString("US-EN")
            ret &= " /Flags " & Me.Flags.ToString("US-EN")
            ret &= " /FontBBox " & Me.FontBBox.ToString("US-EN")
            ret &= " /ItalicAngle " & Me.ItalicAngle.ToString("US-EN")
            ret &= " /StemV " & Me.StemV.ToString("US-EN")
            ret &= " /MissingWidth " & Me.MissingWidth.ToString("US-EN")
            '    xv = xfont.desc(xk)
            '    xs &= " /" & xk & " " & xv
            'Next
            Return ret
        End Function

    End Class

End Namespace

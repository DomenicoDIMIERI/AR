Namespace Drawings

    Public NotInheritable Class Colors

        Public Shared Function FromWeb(ByVal value As String) As System.Drawing.Color
            Dim r, g, b As Byte
            value = Right("000000" & Replace(value, " ", ""), 6)
            r = CByte(CInt("&H" & Left(value, 2)))
            g = CByte(CInt("&H" & Mid(value, 3, 2)))
            b = CByte(CInt("&H" & Right(value, 2)))
            Return Color.FromArgb(255, r, g, b)
        End Function

    End Class

End Namespace
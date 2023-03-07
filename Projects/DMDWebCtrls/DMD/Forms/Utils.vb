Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.Databases


Namespace Forms

  
    Public NotInheritable Class Utils
        Private Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public NotInheritable Class CFormsUtilsClass
            Friend Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            Public Function ToRGBSTR(ByVal c As System.Drawing.Color) As String
                Return Right("00" & Hex(c.R), 2) & Right("00" & Hex(c.G), 2) & Right("00" & Hex(c.B), 2)
            End Function

            Public Function FromRGBSTR(ByVal str As String) As System.Drawing.Color
                str = LCase(Replace(str, " ", ""))
                If (str = "") Then Return System.Drawing.Color.Empty
                Select Case str
                    Case "aliceblue" : Return System.Drawing.Color.AliceBlue
                    Case "antiquewhite" : Return System.Drawing.Color.AntiqueWhite
                    Case "aqua" : Return System.Drawing.Color.Aqua
                    Case "aquamarine" : Return System.Drawing.Color.Aquamarine
                    Case "azure" : Return System.Drawing.Color.Azure
                    Case "beige" : Return System.Drawing.Color.Beige
                    Case "bisque" : Return System.Drawing.Color.Bisque
                    Case "black" : Return System.Drawing.Color.Black
                    Case "blanchedalmond" : Return System.Drawing.Color.BlanchedAlmond
                    Case "blue" : Return System.Drawing.Color.Blue
                    Case "blueviolet" : Return System.Drawing.Color.BlueViolet
                    Case "brown" : Return System.Drawing.Color.Brown
                    Case "burlywood" : Return System.Drawing.Color.BurlyWood
                    Case "cadetblue" : Return System.Drawing.Color.CadetBlue
                    Case "chartreuse" : Return System.Drawing.Color.Chartreuse
                    Case "chocolate" : Return System.Drawing.Color.Chocolate
                    Case "coral" : Return System.Drawing.Color.Coral
                    Case "cornflowerBlue" : Return System.Drawing.Color.CornflowerBlue
                    Case "cornsilk" : Return System.Drawing.Color.Cornsilk
                    Case "crimson" : Return System.Drawing.Color.Crimson
                    Case "cyan" : Return System.Drawing.Color.Cyan
                    Case "darkblue" : Return System.Drawing.Color.DarkBlue
                    Case "darkcyan" : Return System.Drawing.Color.DarkCyan
                    Case "darkgoldenrod" : Return System.Drawing.Color.DarkGoldenrod
                    Case "darkgray" : Return System.Drawing.Color.DarkGray
                    Case "darkgreen" : Return System.Drawing.Color.DarkGreen
                    Case "darkkhaki" : Return System.Drawing.Color.DarkKhaki
                    Case "darkmagenta" : Return System.Drawing.Color.DarkMagenta
                    Case "darkolivegreen" : Return System.Drawing.Color.DarkOliveGreen
                    Case "darkorange" : Return System.Drawing.Color.DarkOrange
                    Case "darkorchid" : Return System.Drawing.Color.DarkOrchid
                    Case "darkred" : Return System.Drawing.Color.DarkRed
                    Case "darksalmon" : Return System.Drawing.Color.DarkSalmon
                    Case "darkseagreen" : Return System.Drawing.Color.DarkSeaGreen
                    Case "darkslateblue" : Return System.Drawing.Color.DarkSlateBlue
                    Case "darkslategray" : Return System.Drawing.Color.DarkSlateGray
                    Case "darkturquoise" : Return System.Drawing.Color.DarkTurquoise
                    Case "darkviolet" : Return System.Drawing.Color.DarkViolet
                    Case "deeppink" : Return System.Drawing.Color.DeepPink
                    Case "deepskyblue" : Return System.Drawing.Color.DeepSkyBlue
                    Case "dimgray" : Return System.Drawing.Color.DimGray
                    Case "dodgerblue" : Return System.Drawing.Color.DodgerBlue
                    Case "empty" : Return System.Drawing.Color.Empty
                    Case "firebrick" : Return System.Drawing.Color.Firebrick
                    Case "floralwhite" : Return System.Drawing.Color.FloralWhite
                    Case "forestgreen" : Return System.Drawing.Color.ForestGreen
                    Case "fuchsia" : Return System.Drawing.Color.Fuchsia
                    Case "gainsboro" : Return System.Drawing.Color.Gainsboro
                    Case "ghostwhite" : Return System.Drawing.Color.GhostWhite
                    Case "gold" : Return System.Drawing.Color.Gold
                    Case "goldenrod" : Return System.Drawing.Color.Goldenrod
                    Case "gray" : Return System.Drawing.Color.Gray
                    Case "green" : Return System.Drawing.Color.Green
                    Case "greenyellow" : Return System.Drawing.Color.GreenYellow
                    Case "honeydew" : Return System.Drawing.Color.Honeydew
                    Case "hotpink" : Return System.Drawing.Color.HotPink
                    Case "indianred" : Return System.Drawing.Color.IndianRed
                    Case "indigo" : Return System.Drawing.Color.Indigo
                    Case "ivory" : Return System.Drawing.Color.Ivory
                    Case "khaki" : Return System.Drawing.Color.Khaki
                    Case "lavender" : Return System.Drawing.Color.Lavender
                    Case "lavenderblush" : Return System.Drawing.Color.LavenderBlush
                    Case "lawngreen" : Return System.Drawing.Color.LawnGreen
                    Case "lemonchiffon" : Return System.Drawing.Color.LemonChiffon
                    Case "lightblue" : Return System.Drawing.Color.LightBlue
                    Case "lightcoral" : Return System.Drawing.Color.LightCoral
                    Case "lightcyan" : Return System.Drawing.Color.LightCyan
                    Case "lightgoldenrodyellow" : Return System.Drawing.Color.LightGoldenrodYellow
                    Case "lightgray" : Return System.Drawing.Color.LightGray
                    Case "lightgreen" : Return System.Drawing.Color.LightGreen
                    Case "lightpink" : Return System.Drawing.Color.LightPink
                    Case "lightsalmon" : Return System.Drawing.Color.LightSalmon
                    Case "lightseagreen" : Return System.Drawing.Color.LightSeaGreen
                    Case "lightskyblue" : Return System.Drawing.Color.LightSkyBlue
                    Case "lightslategray" : Return System.Drawing.Color.LightSlateGray
                    Case "lightsteelblue" : Return System.Drawing.Color.LightSteelBlue
                    Case "lightyellow" : Return System.Drawing.Color.LightYellow
                    Case "lime" : Return System.Drawing.Color.Lime
                    Case "limegreen" : Return System.Drawing.Color.LimeGreen
                    Case "linen" : Return System.Drawing.Color.Linen
                    Case "magenta" : Return System.Drawing.Color.Magenta
                    Case "maroon" : Return System.Drawing.Color.Maroon
                    Case "mediumaquamarine" : Return System.Drawing.Color.MediumAquamarine
                    Case "mediumblue" : Return System.Drawing.Color.MediumBlue
                    Case "mediumorchid" : Return System.Drawing.Color.MediumOrchid
                    Case "mediumpurple" : Return System.Drawing.Color.MediumPurple
                    Case "mediumseagreen" : Return System.Drawing.Color.MediumSeaGreen
                    Case "mediumslateblue" : Return System.Drawing.Color.MediumSlateBlue
                    Case "mediumspringgreen" : Return System.Drawing.Color.MediumSpringGreen
                    Case "mediumturquoise" : Return System.Drawing.Color.MediumTurquoise
                    Case "mediumvioletRed" : Return System.Drawing.Color.MediumVioletRed
                    Case "midnightblue" : Return System.Drawing.Color.MidnightBlue
                    Case "mintcream" : Return System.Drawing.Color.MintCream
                    Case "mistyrose" : Return System.Drawing.Color.MistyRose
                    Case "moccasin" : Return System.Drawing.Color.Moccasin
                    Case "navajowhite" : Return System.Drawing.Color.NavajoWhite
                    Case "navy" : Return System.Drawing.Color.Navy
                    Case "oldlace" : Return System.Drawing.Color.OldLace
                    Case "olive" : Return System.Drawing.Color.Olive
                    Case "olivedrab" : Return System.Drawing.Color.OliveDrab
                    Case "orange" : Return System.Drawing.Color.Orange
                    Case "orangered" : Return System.Drawing.Color.OrangeRed
                    Case "orchid" : Return System.Drawing.Color.Orchid
                    Case "palegoldenrod" : Return System.Drawing.Color.PaleGoldenrod
                    Case "palegreen" : Return System.Drawing.Color.PaleGreen
                    Case "paleturquoise" : Return System.Drawing.Color.PaleTurquoise
                    Case "palevioletred" : Return System.Drawing.Color.PaleVioletRed
                    Case "papayawhip" : Return System.Drawing.Color.PapayaWhip
                    Case "peachpuff" : Return System.Drawing.Color.PeachPuff
                    Case "peru" : Return System.Drawing.Color.Peru
                    Case "pink" : Return System.Drawing.Color.Pink
                    Case "plum" : Return System.Drawing.Color.Plum
                    Case "powderblue" : Return System.Drawing.Color.PowderBlue
                    Case "purple" : Return System.Drawing.Color.Purple
                    Case "red" : Return System.Drawing.Color.Red
                    Case "rosybrown" : Return System.Drawing.Color.RosyBrown
                    Case "royalblue" : Return System.Drawing.Color.RoyalBlue
                    Case "saddlebrown" : Return System.Drawing.Color.SaddleBrown
                    Case "salmon" : Return System.Drawing.Color.Salmon
                    Case "sandybrown" : Return System.Drawing.Color.SandyBrown
                    Case "seagreen" : Return System.Drawing.Color.SeaGreen
                    Case "seashell" : Return System.Drawing.Color.SeaShell
                    Case "sienna" : Return System.Drawing.Color.Sienna
                    Case "silver" : Return System.Drawing.Color.Silver
                    Case "skyblue" : Return System.Drawing.Color.SkyBlue
                    Case "slateblue" : Return System.Drawing.Color.SlateBlue
                    Case "slategray" : Return System.Drawing.Color.SlateGray
                    Case "snow" : Return System.Drawing.Color.Snow
                    Case "springgreen" : Return System.Drawing.Color.SpringGreen
                    Case "steelblue" : Return System.Drawing.Color.SteelBlue
                    Case "tan" : Return System.Drawing.Color.Tan
                    Case "teal" : Return System.Drawing.Color.Teal
                    Case "thistle" : Return System.Drawing.Color.Thistle
                    Case "tomato" : Return System.Drawing.Color.Tomato
                    Case "transparent" : Return System.Drawing.Color.Transparent
                    Case "turquoise" : Return System.Drawing.Color.Turquoise
                    Case "violet" : Return System.Drawing.Color.Violet
                    Case "wheat" : Return System.Drawing.Color.Wheat
                    Case "white" : Return System.Drawing.Color.White
                    Case "whitesmoke" : Return System.Drawing.Color.WhiteSmoke
                    Case "yellow" : Return System.Drawing.Color.Yellow
                    Case "yellowgreen" : Return System.Drawing.Color.YellowGreen
                    Case Else
                        Try
                            Dim blue As Integer = CInt("&H0" & Right(str, 2)) : str = Left(str, Len(str) - 2)
                            Dim green As Integer = CInt("&H0" & Right(str, 2)) : str = Left(str, Len(str) - 2)
                            Dim red As Integer = CInt("&H0" & Right(str, 2)) : str = Left(str, Len(str) - 2)
                            Return System.Drawing.Color.FromArgb(255, red, green, blue)
                        Catch ex As Exception
                            Throw New ArgumentException("Colore non valido: " & str)
                        End Try
                End Select
            End Function

            'Public Sub WriteCBOOps(ByVal writer As HTMLWriter, ByVal cboName As String, ByVal value As String, ByVal width As String, ByVal height As String)
            '    CreateCBOOps(writer, cboName, value, width, height)
            'End Sub

            'Public Sub CreateCBOOps(ByVal writer As HTMLWriter, ByVal cboName As String, ByVal value As String, ByVal width As String, ByVal height As String)
            '    Dim cbo As New COperatorsComboBox
            '    cbo.Name = cboName
            '    cbo.Width = width
            '    cbo.Height = height
            '    cbo.SelectedValue = value
            '    cbo.CreateHTML(writer)
            'End Sub

            Public Function GetValidControlName(ByVal value As String) As String
                Dim ch As String
                Dim i As Integer
                Dim ret As String
                value = Trim(value)
                ch = Strings.Left(value, 1)
                If (UCase(ch) < "A") Or (UCase(ch) > "Z") Then
                    ret = "_"
                Else
                    ret = ch
                End If
                For i = 2 To Len(value)
                    ch = Mid(value, i, 1)
                    If ((UCase(ch) < "A") Or (UCase(ch) > "Z")) And ((ch < "0") Or (ch > "9")) Then
                        ret &= "_"
                    Else
                        ret &= ch
                    End If
                Next
                Return ret
            End Function

            Public Function GetRandomColor() As System.Drawing.Color
                Dim r, g, b As Byte
                r = Rnd(1) * 255
                g = Rnd(1) * 255
                b = Rnd(1) * 255
                Return System.Drawing.Color.FromArgb(255, r, g, b)
            End Function

            Public Function GenerateRandomColor() As String
                Dim r, g, b As String
                r = Right("00" & Hex(CInt(Rnd(1) * 256) Mod 256), 2)
                g = Right("00" & Hex(CInt(Rnd(1) * 256) Mod 256), 2)
                b = Right("00" & Hex(CInt(Rnd(1) * 256) Mod 256), 2)
                Return r & g & b
            End Function

            Public Function GetTextAlignStyle(ByVal value As TextAlignEnum) As String
                Select Case value
                    Case TextAlignEnum.TEXTALIGN_DEFAULT : Return ""
                    Case TextAlignEnum.TEXTALIGN_LEFT : Return "text-align:left;"
                    Case TextAlignEnum.TEXTALIGN_RIGHT : Return "text-align:right;"
                    Case TextAlignEnum.TEXTALIGN_CENTER : Return "text-align:center;"
                    Case TextAlignEnum.TEXTALIGN_JUSTIFY : Return "text-align:justify;"
                End Select
                Return vbNullString
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class


        Private Shared m_FormUtils As CFormsUtilsClass = Nothing

        Public Shared ReadOnly Property FormsUtils As CFormsUtilsClass
            Get
                If (m_FormUtils Is Nothing) Then m_FormUtils = New CFormsUtilsClass
                Return m_FormUtils
            End Get
        End Property
    End Class

End Namespace
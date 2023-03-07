Imports System.IO
Imports System.Text
Imports FinSeA.Text

Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a formatter for a given Type1 font.  
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class Type1FontFormatter

        Private Sub New()
        End Sub

        '/**
        ' * Read and convert a given CFFFont.
        ' * @param font the given CFFFont
        ' * @return the Type1 font
        ' * @throws IOException if an error occurs during reading the given font
        ' */
        Public Shared Function format(ByVal font As CFFFont) As Byte()
            Dim output As New DataOutput()
            printFont(font, output)
            Return output.getBytes()
        End Function

        Private Shared Sub printFont(ByVal font As CFFFont, ByVal output As DataOutput)
            output.println("%!FontType1-1.0 " & font.getName() & " " & font.getProperty("version"))

            printFontDictionary(font, output)

            For i As Integer = 0 To 8 - 1
                Dim sb As New StringBuilder()

                For j As Integer = 0 To 64 - 1
                    sb.append("0")
                Next

                output.println(sb.toString())
            Next

            output.println("cleartomark")
        End Sub

        Private Shared Sub printFontDictionary(ByVal font As CFFFont, ByVal output As DataOutput)
            output.println("10 dict begin")
            output.println("/FontInfo 10 dict dup begin")
            output.println("/version (" & font.getProperty("version") & ") readonly def")
            output.println("/Notice (" & font.getProperty("Notice") & ") readonly def")
            output.println("/FullName (" & font.getProperty("FullName") & ") readonly def")
            output.println("/FamilyName (" & font.getProperty("FamilyName") & ") readonly def")
            output.println("/Weight (" & font.getProperty("Weight") & ") readonly def")
            output.println("/ItalicAngle " & font.getProperty("ItalicAngle") & " def")
            output.println("/isFixedPitch " & font.getProperty("isFixedPitch") & " def")
            output.println("/UnderlinePosition " & font.getProperty("UnderlinePosition") & " def")
            output.println("/UnderlineThickness " & font.getProperty("UnderlineThickness") & " def")
            output.println("end readonly def")
            output.println("/FontName /" & font.getName() & " def")
            output.println("/PaintType " & font.getProperty("PaintType") & " def")
            output.println("/FontType 1 def")
            Dim matrixFormat As New DecimalFormat("0.########", New DecimalFormatSymbols(Locale.US))
            output.println("/FontMatrix " & formatArray(font.getProperty("FontMatrix"), matrixFormat, False) & " readonly def")
            output.println("/FontBBox " & formatArray(font.getProperty("FontBBox"), False) & " readonly def")
            output.println("/StrokeWidth " & font.getProperty("StrokeWidth") & " def")

            Dim mappings As ICollection(Of CFFFont.Mapping) = font.getMappings()

            output.println("/Encoding 256 array")
            output.println("0 1 255 {1 index exch /.notdef put} for")

            For Each mapping As CFFFont.Mapping In mappings
                output.println("dup " & mapping.getCode() & " /" & mapping.getName() & " put")
            Next

            output.println("readonly def")
            output.println("currentdict end")

            Dim eexecOutput As New DataOutput()

            printEexecFontDictionary(font, eexecOutput)

            output.println("currentfile eexec")

            Dim eexecBytes() As Byte = Type1FontUtil.eexecEncrypt(eexecOutput.getBytes())

            Dim hexString As String = Type1FontUtil.hexEncode(eexecBytes)
            Dim i As Integer = 0
            While (i < hexString.Length())
                Dim hexLine As String = hexString.Substring(i, Math.Min(i + 72, hexString.Length()))

                output.println(hexLine)

                i += hexLine.Length()
            End While
        End Sub

        Private Shared Sub printEexecFontDictionary(ByVal font As CFFFont, ByVal output As DataOutput) 'throws IOException
            output.println("dup /Private 15 dict dup begin")
            output.println("/RD {string currentfile exch readstring pop} executeonly def")
            output.println("/ND {noaccess def} executeonly def")
            output.println("/NP {noaccess put} executeonly def")
            output.println("/BlueValues " & formatArray(font.getProperty("BlueValues"), True) & " ND")
            output.println("/OtherBlues " & formatArray(font.getProperty("OtherBlues"), True) & " ND")
            output.println("/BlueScale " & font.getProperty("BlueScale") & " def")
            output.println("/BlueShift " & font.getProperty("BlueShift") & " def")
            output.println("/BlueFuzz " & font.getProperty("BlueFuzz") & " def")
            output.println("/StdHW " & formatArray(font.getProperty("StdHW"), True) & " ND")
            output.println("/StdVW " & formatArray(font.getProperty("StdVW"), True) & " ND")
            output.println("/ForceBold " & font.getProperty("ForceBold") & " def")
            output.println("/MinFeature {16 16} def")
            output.println("/password 5839 def")

            Dim mappings As ICollection(Of CFFFont.Mapping) = font.getMappings()

            output.println("2 index /CharStrings " & mappings.size() & " dict dup begin")

            Dim formatter As New Type1CharStringFormatter()

            For Each mapping As CFFFont.Mapping In mappings
                Dim type1Bytes() As Byte = formatter.format(mapping.toType1Sequence())

                Dim charstringBytes() As Byte = Type1FontUtil.charstringEncrypt(type1Bytes, 4)

                output.print("/" & mapping.getName() & " " & charstringBytes.Length & " RD ")
                output.write(charstringBytes)
                output.print(" ND")

                output.println()
            Next

            output.println("end")
            output.println("end")

            output.println("readonly put")
            output.println("noaccess put")
            output.println("dup /FontName get exch definefont pop")
            output.println("mark currentfile closefile")
        End Sub

        Private Shared Function formatArray(ByVal [object] As Object, ByVal executable As Boolean) As String
            Return formatArray([object], Nothing, executable)
        End Function

        Private Shared Function formatArray(ByVal [object] As Object, ByVal format As NumberFormat, ByVal executable As Boolean) As String
            Dim sb As New StringBuffer()

            sb.append(IIf(executable, "{", "["))

            If (TypeOf ([object]) Is ICollection) Then
                Dim sep As String = ""

                Dim elements As ICollection = [object]
                For Each element As Object In elements
                    sb.append(formatElement(element, format))
                    sb.append(sep)
                    sep = " "
                Next
            ElseIf (TypeOf ([object]) Is Number) Then
                sb.append(formatElement([object], format))
            End If

            sb.append(IIf(executable, "}", "]"))

            Return sb.ToString()
        End Function

        Private Shared Function formatElement(ByVal [object] As Object, ByVal format As NumberFormat) As String
            If (format IsNot Nothing) Then
                If (TypeOf ([object]) Is NDouble OrElse TypeOf ([object]) Is NFloat) Then
                    Dim number As Number = [object]
                    Return format.format(number.doubleValue())
                ElseIf (TypeOf ([object]) Is NLong OrElse TypeOf ([object]) Is NInteger) Then
                    Dim number As Number = [object]
                    Return format.format(number.longValue())
                End If
            End If
            Return CStr([object])
        End Function

    End Class

End Namespace
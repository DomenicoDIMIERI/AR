Imports DMD.Sistema
Imports DMD.XML

Partial Class Sistema

    Public Class CNumberInfo
        Implements DMD.XML.IDMDXMLSerializable

        Public Zona As String
        Public Nazione As String
        Public PrefissoInternazionale As String
        Public PrefissoLocale As String
        Public Distretto As String
        Public Numero As String

        Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Zona", Me.Zona)
            writer.WriteAttribute("Nazione", Me.Nazione)
            writer.WriteAttribute("PrefissoInternazionale", Me.PrefissoInternazionale)
            writer.WriteAttribute("PrefissoLocale", Me.PrefissoLocale)
            writer.WriteAttribute("Distretto", Me.Distretto)
            writer.WriteAttribute("Numero", Me.Numero)
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Zona" : Me.Zona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nazione" : Me.Nazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PrefissoInternazionale" : Me.PrefissoInternazionale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PrefissoLocale" : Me.PrefissoLocale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Distretto" : Me.Distretto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Numero" : Me.Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String
            ret = Strings.Combine(Me.PrefissoInternazionale, Me.PrefissoLocale, " ")
            ret = Strings.Combine(ret, Me.Numero, " ")
            Return ret
        End Function
    End Class

    Public Class CPhonePrefix
        Public destinazione As String
        Public per As String
        Public da As String
        Public zona As Integer

        Public Sub New(ByVal destinazione As String, ByVal per As String, Optional ByVal da As String = vbNullString, Optional ByVal zona As Integer = 0)
            Me.destinazione = Strings.Trim(destinazione)
            Me.per = Strings.Trim(per)
            Me.da = da
            Me.zona = zona
        End Sub

    End Class



    Public NotInheritable Class CFormatsClass
        Private lockPrefissi As New Object
        Private m_Prefissi As CCollection(Of CPhonePrefix)
        Public ReadOnly _defaultPhoneNumberFormat As String = "ITA"
        Public ReadOnly _validPhoneNumberChars As String = "+0123456789"


        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Prefissi = Nothing
        End Sub

        Public Function FormatBytes(ByVal value As Long, Optional ByVal numDecimals As Integer = 0) As String
            Const KB As Long = 1024
            Const MB As Long = 1048576
            Const GB As Long = 1073741824
            Dim ret As String
            If value < KB Then
                ret = Formats.FormatNumber(value, numDecimals) & " bytes"
            ElseIf value < MB Then
                ret = Formats.FormatNumber(value / KB, numDecimals) & " KB"
            ElseIf value < GB Then
                ret = Formats.FormatNumber(value / MB, numDecimals) & " MB"
            Else
                ret = Formats.FormatNumber(value / GB, numDecimals) & " GB"
            End If
            Return ret
        End Function

        Public Function Format(ByVal value As Object, ByVal formatString As String) As String
            Const separators As String = " /|""'()[]{}+*°<>!_-.:,;"
            Dim i As Integer
            Dim ch, ret, token As String
            Dim status As Integer
            i = 1
            status = 0
            token = ""
            ret = ""
            While (i <= Len(formatString))
                ch = Mid(formatString, i, 1)
                If InStr(separators, ch) > 0 Then
                    Select Case token
                        Case "d" : token = Day(value)
                        Case "dd" : token = Right("00" & Day(value), 2)
                        Case "ddd" : token = WeekdayName(Weekday(value), True)
                        Case "dddd" : token = WeekdayName(Weekday(value), False)
                        Case "M" : token = Month(value)
                        Case "MM" : token = Right("00" & Month(value), 2)
                        Case "MMM" : token = MonthName(Month(value), True)
                        Case "MMMM" : token = MonthName(Month(value), False)
                        Case "yy" : token = Right("00" & Year(value), 2)
                        Case "yyy" : token = Year(value)
                        Case "yyyy" : token = Year(value)
                        Case "h" : token = Hour(value)
                        Case "hh" : token = Right("00" & Hour(value), 2)
                        Case "m" : token = Minute(value)
                        Case "mm" : token = Right("00" & Minute(value), 2)
                        Case "s" : token = Second(value)
                        Case "ss" : token = Right("00" & Second(value), 2)
                        Case Else
                    End Select
                    ret &= token
                    ret &= ch
                    token = ""
                Else
                    token = token & ch
                End If
                i = i + 1
            End While
            ch = ""

            Select Case token
                Case "d" : token = Day(value)
                Case "dd" : token = Right("00" & Day(value), 2)
                Case "ddd" : token = WeekdayName(Weekday(value), True)
                Case "dddd" : token = WeekdayName(Weekday(value), False)
                Case "M" : token = Month(value)
                Case "MM" : token = Right("00" & Month(value), 2)
                Case "MMM" : token = MonthName(Month(value), True)
                Case "MMMM" : token = MonthName(Month(value), False)
                Case "yy" : token = Right("00" & Year(value), 2)
                Case "yyy" : token = Year(value)
                Case "yyyy" : token = Year(value)
                Case "h" : token = Hour(value)
                Case "hh" : token = Right("00" & Hour(value), 2)
                Case "m" : token = Minute(value)
                Case "mm" : token = Right("00" & Minute(value), 2)
                Case "s" : token = Second(value)
                Case "ss" : token = Right("00" & Second(value), 2)
                Case "#" : token = "" & CDbl(value)
                Case Else
            End Select
            ret &= token
            ret &= ch
            token = ""

            Return ret
        End Function

        Public Function TrimInternationalPrefix(value As String) As String
            Dim i As CNumberInfo = Me.GetInfoNumero(value)
            Return Strings.Combine(i.PrefissoLocale, i.Numero, " ")
        End Function



        Public Function GetInfoNumero(value As String) As CNumberInfo
            SyncLock Me.lockPrefissi
                value = Strings.Trim(value)
                If (value.StartsWith("00")) Then value = "+" & value.Substring(2)

                Dim items As CCollection(Of CPhonePrefix) = Me.GetPrefissiInt
                Dim ret As New CNumberInfo

                For Each p As CPhonePrefix In items
                    If value.StartsWith(p.per) Then
                        If (p.zona <> 0) Then
                            ret.PrefissoInternazionale = p.per
                            ret.Zona = p.zona
                            ret.Nazione = p.destinazione
                            value = value.Substring(p.per.Length)
                        Else
                            ret.PrefissoLocale = p.per
                            ret.Distretto = p.destinazione
                            ret.Numero = value.Substring(p.per.Length)
                            value = ""
                        End If
                        If (value = "") Then Exit For
                    End If
                Next
                If (value <> "") Then ret.Numero = value

                Return ret
            End SyncLock
        End Function

        Public Function ParsePartitaIVA(ByVal value As String) As String
            Dim validLetters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Dim validNumbers As String = "0123456789"
            Dim ret As String = ""
            value = UCase(Replace(value, " ", ""))
            Dim i As Integer = 1
            While (i <= Math.Min(2, Len(value)) AndAlso InStr(validLetters, Mid(value, i, 1)) > 0)
                ret &= Mid(value, i, 1)
                i += 1
            End While
            While (i <= Math.Min(13, Len(value)) AndAlso InStr(validNumbers, Mid(value, i, 1)) > 0)
                ret &= Mid(value, i, 1)
                i += 1
            End While
            Return ret
        End Function

        Public Function FormatPartitaIVA(ByVal value As String) As String
            Dim ret As String
            ret = UCase(Replace("" & value, " ", ""))
            Return ret
        End Function



        Public Function FormatUserTime(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return Nothing
            Return FormatDateTime(value, 4)
        End Function



        Public Function FormatUserDateTimeOggi(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return Nothing
            If Calendar.GetDatePart(value) = Calendar.ToDay Then
                Return "Oggi alle " & FormatDateTime(value, 4)
            ElseIf Calendar.GetDatePart(value) = Calendar.YesterDay Then
                Return "Ieri alle " & FormatDateTime(value, 4)
            Else
                Return FormatDateTime(value, 2) & " " & FormatDateTime(value, 4)
            End If
        End Function

        Public Function IsNullOrNothing(ByVal value As Object) As Boolean
            If TypeOf (value) Is DBNull Then Return True
            If TypeOf (value) Is ValueType Then Return False
            Return (value Is Nothing)
        End Function


        Public Function ParseDate(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Date)
            If IsNullOrNothing(value) Then Return Nothing
            If (ignoreErrors) Then
                Try
                    Return ParseDate(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    If (Trim(value) = vbNullString) Then Return Nothing
                    value = Replace(value, ".", ":")
                    Return CDate(value)
                Else
                    Return CDate(value)
                End If
            End If
        End Function

        'Public Function ParseDate(ByVal value As Object, ByVal format As String) As Nullable(Of Date)
        '    If IsNullOrNothing(value) Then Return Nothing

        'End Function

        Public Function ParseUSADate(ByVal value As Object) As Nullable(Of Date)
            If IsNullOrNothing(value) Then Return Nothing
            If (TypeOf (value) Is Date OrElse TypeOf (value) Is Date?) Then Return value
            Dim strValue As String = Trim(value.ToString)
            If (strValue = vbNullString) Then Return Nothing
            Dim s As String() = Split(strValue, "/")
            Return DateSerial(s(2), s(0), s(1))
        End Function

        Public Function FormatUSADate(ByVal value As Object) As String
            Dim d As Date? = Formats.ParseDate(value)
            If (d.HasValue = False) Then Return ""
            With d.Value
                Return .Month & "/" & .Day & "/" & .Year & " " & .Hour & ":" & .Minute & ":" & .Second
            End With
        End Function

        Public Function FormatISODate(ByVal value As Object) As String
            Dim d As Date? = Formats.ParseDate(value)
            If (d.HasValue = False) Then Return ""
            With d.Value
                Return Right("0000" & .Year, 4) & Right("00" & .Month, 2) & Right("00" & .Day, 2) & Right("00" & .Hour, 2) & Right("00" & .Minute, 2) & Right("00" & .Second, 2)
            End With
        End Function

        Public Function ParseISODate(ByVal value As Object) As Date?
            If IsNullOrNothing(value) Then Return Nothing
            If (TypeOf (value) Is Date OrElse TypeOf (value) Is Date?) Then Return value
            Dim strValue As String = Trim(value.ToString)
            If (strValue = vbNullString) Then Return Nothing
            Dim year As Integer = Formats.ToInteger(Mid(value, 1, 4))
            Dim month As Integer = Formats.ToInteger(Mid(value, 5, 2))
            Dim day As Integer = Formats.ToInteger(Mid(value, 7, 2))
            Dim hour As Integer = Formats.ToInteger(Mid(value, 9, 2))
            Dim minute As Integer = Formats.ToInteger(Mid(value, 11, 2))
            Dim second As Integer = Formats.ToInteger(Mid(value, 13, 2))
            Return New Date(year, month, day, hour, minute, second)
        End Function

        Public Function ToDate(ByVal value As Object, Optional ByVal defaultValue As Object = Nothing) As Date
            Dim ret As Nullable(Of Date) = ParseDate(value)
            If ret.HasValue Then Return ret.Value
            Return defaultValue
        End Function

        Public Function FormatValuta(ByVal value As Object, Optional ByVal numDecimals As Integer = 2, Optional ByVal symbol As String = vbNullString) As String
            If IsNullOrNothing(value) Then Return vbNullString
            Return Strings.Trim(FormatNumber(value, numDecimals) & " " & symbol)
        End Function

        Public Function FormatValuta0(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return Nothing
            Return FormatNumber(value, 2)
        End Function

        Public Function ParseDouble(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Double)
            If IsNullOrNothing(value) Then Return Nothing
            If (ignoreErrors) Then
                Try
                    Return ParseDouble(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    Dim tValue As String = Trim(value)
                    If (tValue = vbNullString) Then Return Nothing
                    tValue = Replace(tValue, ".", "")
                    Return CDbl(tValue)
                Else
                    Return CDbl(value)
                End If
            End If
        End Function

        Public Function ParseSingle(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Single)
            If IsNullOrNothing(value) Then Return Nothing
            If (ignoreErrors) Then
                Try
                    Return ParseSingle(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    Dim tValue As String = Trim(value)
                    If (tValue = vbNullString) Then Return Nothing
                    tValue = Replace(tValue, ".", "")
                    Return CSng(tValue)
                Else
                    Return CSng(value)
                End If
            End If
        End Function

        Public Function ParsePercentage(ByVal value As Object) As Nullable(Of Double)
            Return ParseDouble(value)
        End Function

        Public Function ParseValuta(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Decimal)
            If IsNullOrNothing(value) Then Return Nothing
            If (ignoreErrors) Then
                Try
                    Return ParseValuta(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    Dim tValue As String = Trim(value)
                    If (tValue = vbNullString) Then Return Nothing
                    tValue = Replace(tValue, ".", "")
                    Return CDec(tValue)
                Else
                    Return CDec(value)
                End If
            End If
        End Function

        Public Function ToValuta(ByVal value As Object, Optional ByVal defValue As Decimal = 0) As Decimal
            Dim ret As Nullable(Of Decimal) = ParseValuta(value)
            If ret.HasValue Then Return ret.Value
            Return defValue
        End Function

        Public Function FormatFloat(ByVal value As Object, Optional ByVal numDecimals As Integer = 2) As String
            If IsNullOrNothing(value) Then Return Nothing
            Return FormatNumber(value, numDecimals)
        End Function

        Public Function ParseInteger(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Integer)
            If IsNullOrNothing(value) Then Return Nothing
            If (ignoreErrors) Then
                Try
                    Return ParseInteger(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    Dim tValue As String = Trim(value)
                    If tValue = vbNullString Then Return Nothing
                    tValue = Replace(tValue, ".", "")
                    Dim ret As Long
#If DEBUG Then
                    Try
                        ret = CLng(tValue)
                        If (ret > 2147483648) Then ret = ret - 4294967296
                    Catch ex As Exception
                        Throw
                    End Try
#Else
                    ret = CLng(tValue)
                        If (ret > 2147483648) Then ret = ret - 4294967296
#End If
                    Return CInt(ret)
                Else
                    Return CInt(value)
                End If
            End If
        End Function

        Public Function ParseLong(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Long)
            If IsNullOrNothing(value) Then Return Nothing
            If (ignoreErrors) Then
                Try
                    Return ParseLong(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    Dim tValue As String = Trim(value)
                    If tValue = vbNullString Then Return Nothing
                    tValue = Replace(tValue, ".", "")
                    Return CLng(tValue)
                Else
                    Return CLng(value)
                End If
            End If
        End Function

        Public Function ParseBool(ByVal value As Object, Optional ByVal ignoreErrors As Boolean = False) As Nullable(Of Boolean)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is Boolean OrElse TypeOf (value) Is Nullable(Of Boolean) Then Return value
            If (ignoreErrors) Then
                Try
                    Return ParseBool(value, False)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                If TypeOf (value) Is String Then
                    Dim tmp As String = Trim(value)
                    If tmp = "" Then Return Nothing
                    Return CBool(value)
                Else
                    Return CBool(value)
                End If
            End If
        End Function

        Public Function ParseByte(ByVal value As Object) As Nullable(Of Byte)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is Byte OrElse TypeOf (value) Is Nullable(Of Byte) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CByte(value)
            Else
                Return CByte(value)
            End If
        End Function

        Public Function ParseSByte(ByVal value As Object) As Nullable(Of SByte)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is SByte OrElse TypeOf (value) Is Nullable(Of SByte) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CSByte(value)
            Else
                Return CSByte(value)
            End If
        End Function

        Public Function ParseChar(ByVal value As Object) As Nullable(Of Char)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is Char OrElse TypeOf (value) Is Nullable(Of Char) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CChar(value)
            Else
                Return CChar(value)
            End If
        End Function

        Public Function ParseShort(ByVal value As Object) As Nullable(Of Short)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is Short OrElse TypeOf (value) Is Nullable(Of Short) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CShort(value)
            Else
                Return CShort(value)
            End If
        End Function

        Public Function ParseUShort(ByVal value As Object) As Nullable(Of UShort)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is UShort OrElse TypeOf (value) Is Nullable(Of UShort) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CUShort(value)
            Else
                Return CUShort(value)
            End If
        End Function

        Public Function ParseULong(ByVal value As Object) As Nullable(Of ULong)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is ULong OrElse TypeOf (value) Is Nullable(Of ULong) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CULng(value)
            Else
                Return CULng(value)
            End If
        End Function

        Public Function ParseUInteger(ByVal value As Object) As Nullable(Of UInteger)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is UInteger OrElse TypeOf (value) Is Nullable(Of UInteger) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CUInt(value)
            Else
                Return CUInt(value)
            End If
        End Function

        Public Function ParseDecimal(ByVal value As Object) As Nullable(Of Decimal)
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is Decimal OrElse TypeOf (value) Is Nullable(Of Decimal) Then Return value
            If TypeOf (value) Is String Then
                Dim tmp As String = Trim(value)
                If tmp = "" Then Return Nothing
                Return CDec(value)
            Else
                Return CDec(value)
            End If
        End Function


        Public Shadows Function ToString(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return Nothing
            If TypeOf (value) Is String Then Return value
            Return value.ToString
        End Function


        Public Function ToDouble(ByVal value As Object, Optional ByVal defValue As Double = 0) As Double
            Dim ret As Nullable(Of Double) = ParseDouble(value)
            If ret.HasValue Then Return ret.Value
            Return defValue
        End Function


        Public Function FormatUserDate(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return Nothing
            Return Calendar.Format(value, "dd/MM/yyyy")
        End Function

        Public Function FormatUserDate(ByVal value As Object, ByVal format As String) As String
            If IsNullOrNothing(value) Then Return Nothing
            Return Calendar.Format(value, format)
        End Function

        Public Function FormatUserDateTime(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return vbNullString
            Return Calendar.Format(value, "dd/MM/yyyy HH:mm:ss")
        End Function

        Public Function FormatCodiceFiscale(ByVal value As String) As String
            Dim ret As String = Microsoft.VisualBasic.Replace(value, " ", "")
            If (Left(ret, 1) >= "0" And Left(ret, 1) <= "9") Then Return FormatPartitaIVA(ret)
            Return Trim(Microsoft.VisualBasic.Mid(ret, 1, 3) & " " & Microsoft.VisualBasic.Mid(ret, 4, 3) & " " & Microsoft.VisualBasic.Mid(ret, 7, 5) & " " & Microsoft.VisualBasic.Mid(ret, 12, 4) & " " & Microsoft.VisualBasic.Mid(ret, 16, 1))
        End Function

        Public Function FormatNumber(ByVal num As Object, Optional ByVal decimalNum As Integer = 0, Optional ByVal bolLeadingZero As Boolean = True, Optional ByVal bolParens As Boolean = False, Optional ByVal bolCommas As Boolean = True) As String
            num = ParseDouble(num)
            Return Microsoft.VisualBasic.FormatNumber(num, decimalNum, bolLeadingZero, bolParens, bolCommas)
        End Function

        Public Function FormatInteger(ByVal value As Object) As String
            If IsNullOrNothing(value) Then Return vbNullString
            Return FormatNumber(value, 0)
        End Function

        Public Function FormatPercentage(ByVal value As Object, Optional ByVal n As Integer = 2) As String
            If IsNullOrNothing(value) Then Return vbNullString
            Return FormatNumber(value, n, True, False, True)
        End Function


        Public Function ParseCodiceFiscale(ByVal value As String) As String
            Const validChars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Const validNumbers As String = "0123456789"
            Dim ret As String
            value = Strings.UCase(Replace(value, " ", ""))
            ret = ""
            If (InStr(validChars, Mid(value, 1, 1)) > 0 AndAlso InStr(validNumbers, Mid(value, 7, 1)) > 0) Then
                Dim ch As String

                For i As Integer = 1 To Math.Min(6, Len(value))
                    ch = Mid(value, i, 1)
                    If (InStr(validChars, ch) > 0) Then ret &= ch
                Next
                For i As Integer = 7 To Math.Min(8, Len(value))
                    ch = Mid(value, i, 1)
                    If (InStr(validNumbers, ch) > 0) Then ret &= ch
                Next
                For i As Integer = 9 To Math.Min(9, Len(value))
                    ch = Mid(value, i, 1)
                    If (InStr(validChars, ch) > 0) Then ret &= ch
                Next
                For i As Integer = 10 To Math.Min(11, Len(value))
                    ch = Mid(value, i, 1)
                    If (InStr(validNumbers, ch) > 0) Then ret &= ch
                Next
                For i As Integer = 12 To Math.Min(16, Len(value))
                    ch = Mid(value, i, 1)
                    If (InStr(validNumbers, ch) > 0 OrElse InStr(validChars, ch) > 0) Then ret &= ch
                Next
            Else
                ret = ParsePartitaIVA(value)
            End If
            Return ret
        End Function

        Public Function ToBool(ByVal value As Object, Optional ByVal defValue As Boolean = False) As Boolean
            Dim ret As Nullable(Of Boolean) = ParseBool(value)
            If ret.HasValue Then Return ret.Value
            Return defValue
        End Function

        Public Function ToBoolean(ByVal value As Object, Optional ByVal defValue As Boolean = False) As Boolean
            Return ToBool(value, defValue)
        End Function

        Public Function ToInteger(ByVal value As Object, Optional ByVal defValue As Integer = 0) As Integer
            Dim ret As Nullable(Of Integer) = ParseInteger(value)
            If ret.HasValue Then Return ret.Value
            Return defValue
        End Function

        Public Function ToLong(ByVal value As Object, Optional ByVal defValue As Long = 0) As Long
            Dim ret As Nullable(Of Long) = ParseLong(value)
            If ret.HasValue Then Return ret.Value
            Return defValue
        End Function

        '        Public Function ParseInteger(ByVal value As Object) As Nullable(Of Integer)
        '    var ret = null;
        '    if (value != null) {
        '        if (typeof (value) == "number") {
        '            ret = parseInt(value);
        '        } else {
        '            ret = parseInt(Strings.Replace(value.toString(), ",", ""));
        '        }
        '        if (ret > 2147483648) ret = ret - 4294967296;
        '    }
        '    return ret;
        '}
        'Formats.ParseBool = function (value) {
        '    try {
        '        return (value == true);
        '    } catch (ex) {
        '        return null;
        '    }    
        '}
        'Formats.ParseBoolean = function (value) { return Formats.ParseBool(value); }


        Private Sub _AddPrefixInt(ByVal zona As Integer, ByVal nazione As String, ByVal per As String, Optional ByVal da As String = vbNullString)
            Me.m_Prefissi.Add(New CPhonePrefix(nazione, per, da, zona))
        End Sub

        Public Sub _AddPrefix(ByVal destinazione As String, ByVal per As String, Optional ByVal da As String = vbNullString)
            Me.m_Prefissi.Add(New CPhonePrefix(destinazione, per, da))
        End Sub

        Private Function GetPrefissiInt() As CCollection(Of CPhonePrefix)
            If (Me.m_Prefissi Is Nothing) Then Me._InitPhonePrefixes()
            Return Me.m_Prefissi
        End Function

        Public Function GetPrefissi() As CCollection(Of CPhonePrefix)
            SyncLock Me
                Return New CCollection(Of CPhonePrefix)(Me.GetPrefissiInt)
            End SyncLock
        End Function




        Private Sub _InitPhonePrefixes()
            'Zona 1: America settentrionale
            Me.m_Prefissi = New CCollection(Of CPhonePrefix)
            _AddPrefixInt(1, "Stati Uniti d'America", "+1")
            Formats._AddPrefixInt(1, "Canada", "+1")
            'Zona 2: Principalmente Africa
            Formats._AddPrefixInt(2, "Egitto", "+20")
            Formats._AddPrefixInt(2, "riservato al Marocco", "+210")
            Formats._AddPrefixInt(2, "Sudan del Sud", "+211")
            Formats._AddPrefixInt(2, "Marocco", "+212")
            Formats._AddPrefixInt(2, "Algeria", "+213")
            Formats._AddPrefixInt(2, "riservato all'Algeria", "+214")
            Formats._AddPrefixInt(2, "riservato all'Algeria", "+215")
            Formats._AddPrefixInt(2, "Tunisia", "+216")
            Formats._AddPrefixInt(2, "riservato allaTunisia", "+217")
            Formats._AddPrefixInt(2, "Libia", "+218")
            Formats._AddPrefixInt(2, "riservato alla Libia", "+219")
            Formats._AddPrefixInt(2, "Gambia", "+220")
            Formats._AddPrefixInt(2, "Senegal", "+221")
            Formats._AddPrefixInt(2, "Mauritania", "+222")
            Formats._AddPrefixInt(2, "Mali", "+223")
            Formats._AddPrefixInt(2, "Guinea", "+224")
            Formats._AddPrefixInt(2, "Costa d'Avorio", "+225")
            Formats._AddPrefixInt(2, "Burkina Faso", "+226")
            Formats._AddPrefixInt(2, "Niger", "+227")
            Formats._AddPrefixInt(2, "Togo", "+228")
            Formats._AddPrefixInt(2, "Benin", "+229")
            Formats._AddPrefixInt(2, "Mauritius", "+230")
            Formats._AddPrefixInt(2, "Liberia", "+231")
            Formats._AddPrefixInt(2, "Sierra Leone", "+232")
            Formats._AddPrefixInt(2, "Ghana", "+233")
            Formats._AddPrefixInt(2, "Nigeria", "+234")
            Formats._AddPrefixInt(2, "Ciad", "+235")
            Formats._AddPrefixInt(2, "Repubblica Centrafricana", "+236")
            Formats._AddPrefixInt(2, "Camerun", "+237")
            Formats._AddPrefixInt(2, "Capo Verde", "+238")
            Formats._AddPrefixInt(2, "Sao Tome e Principe", "+239")
            Formats._AddPrefixInt(2, "Guinea Equatoriale", "+240")
            Formats._AddPrefixInt(2, "Gabon", "+241")
            Formats._AddPrefixInt(2, "Repubblica del Congo (Brazzaville)", "+242")
            Formats._AddPrefixInt(2, "Repubblica Democratica del Congo (Kinshasa, precedentemente detta Zaire)", "+243")
            Formats._AddPrefixInt(2, "Angola", "+244")
            Formats._AddPrefixInt(2, "Angola", "+244")
            Formats._AddPrefixInt(2, "Guinea-Bissau", "+245")
            Formats._AddPrefixInt(2, "Diego Garcia", "+246")
            Formats._AddPrefixInt(2, "Isola Ascensione", "+247")
            Formats._AddPrefixInt(2, "Seychelles", "+248")
            Formats._AddPrefixInt(2, "Sudan", "+249")
            Formats._AddPrefixInt(2, "Ruanda", "+250")
            Formats._AddPrefixInt(2, "Etiopia", "+251")
            Formats._AddPrefixInt(2, "Somalia", "+252")
            Formats._AddPrefixInt(2, "Gibuti", "+253")
            Formats._AddPrefixInt(2, "Kenia", "+254")
            Formats._AddPrefixInt(2, "Tanzania", "+255")
            Formats._AddPrefixInt(2, "Uganda", "+256")
            Formats._AddPrefixInt(2, "Burundi", "+257")
            Formats._AddPrefixInt(2, "Mozambico", "+258")
            Formats._AddPrefixInt(2, "Zanzibar – mai implementato (vedi +255 Tanzania)", "+259")
            Formats._AddPrefixInt(2, "Zambia", "+260")
            Formats._AddPrefixInt(2, "Madagascar", "+261")
            Formats._AddPrefixInt(2, "Riunione", "+262")
            Formats._AddPrefixInt(2, "Zimbabwe", "+263")
            Formats._AddPrefixInt(2, "Namibia", "+264")
            Formats._AddPrefixInt(2, "Malawi", "+265")
            Formats._AddPrefixInt(2, "Lesotho", "+266")
            Formats._AddPrefixInt(2, "Botswana", "+267")
            Formats._AddPrefixInt(2, "Swaziland", "+268")
            Formats._AddPrefixInt(2, "Comore e Mayotte", "+269")
            Formats._AddPrefixInt(2, "Sudafrica", "+27")
            Formats._AddPrefixInt(2, "Sant'Elena", "+290")
            Formats._AddPrefixInt(2, "Eritrea", "+291")
            Formats._AddPrefixInt(2, "dismesso (era assegnato a San Marino, che ora usa +378)", "+295")
            Formats._AddPrefixInt(2, "Aruba", "+297")
            Formats._AddPrefixInt(2, "Isole Fær Øer", "+298")
            Formats._AddPrefixInt(2, "Groenlandia", "+299")
            'Zona 3: Europa
            Formats._AddPrefixInt(3, "Grecia", "+30")
            Formats._AddPrefixInt(3, "Paesi Bassi", "+31")
            Formats._AddPrefixInt(3, "Belgio", "+32")
            Formats._AddPrefixInt(3, "Francia", "+33")
            Formats._AddPrefixInt(3, "Spagna", "+34")
            Formats._AddPrefixInt(3, "Gibilterra", "+350")
            Formats._AddPrefixInt(3, "Portogallo", "+351")
            Formats._AddPrefixInt(3, "Lussemburgo", "+352")
            Formats._AddPrefixInt(3, "Irlanda", "+353")
            Formats._AddPrefixInt(3, "Islanda", "+354")
            Formats._AddPrefixInt(3, "Albania", "+355")
            Formats._AddPrefixInt(3, "Malta", "+356")
            Formats._AddPrefixInt(3, "Cipro", "+357")
            Formats._AddPrefixInt(3, "Finlandia", "+358")
            Formats._AddPrefixInt(3, "Bulgaria", "+359")
            Formats._AddPrefixInt(3, "Ungheria", "+36")
            Formats._AddPrefixInt(3, "era usato dalla Repubblica Democratica Tedesca. In tali regioni si usa adesso il codice +49 della Germania riunificata", "+37")
            Formats._AddPrefixInt(3, "Lituania", "+370")
            Formats._AddPrefixInt(3, "Lettonia", "+371")
            Formats._AddPrefixInt(3, "Estonia", "+372")
            Formats._AddPrefixInt(3, "Moldavia", "+373")
            Formats._AddPrefixInt(3, "Armenia", "+374")
            Formats._AddPrefixInt(3, "Bielorussia", "+375")
            Formats._AddPrefixInt(3, "Andorra", "+376")
            Formats._AddPrefixInt(3, "Principato di Monaco", "+377")
            Formats._AddPrefixInt(3, "San Marino", "+378")
            Formats._AddPrefixInt(3, "assegnato a Città del Vaticano, ma non attivato (usa +39 06 = Roma)", "+379")
            Formats._AddPrefixInt(3, "era usato dalla Jugoslavia", "+38")
            Formats._AddPrefixInt(3, "Ucraina", "+380")
            Formats._AddPrefixInt(3, "Serbia", "+381")
            Formats._AddPrefixInt(3, "Montenegro", "+382")
            Formats._AddPrefixInt(3, "Croazia", "+385")
            Formats._AddPrefixInt(3, "Slovenia", "+386")
            Formats._AddPrefixInt(3, "Bosnia ed Erzegovina", "+387")
            Formats._AddPrefixInt(3, "Spazio di numerazione telefonica europeo – Servizi Europei", "+388")
            Formats._AddPrefixInt(3, "Macedonia (F.Y.R.O.M.)", "+389")
            Formats._AddPrefixInt(3, "Italia", "+39")

            'Zona 4: Europa
            Formats._AddPrefixInt(4, "Romania", "+40")
            Formats._AddPrefixInt(4, "Svizzera", "+41")
            Formats._AddPrefixInt(4, "era usato dalla Cecoslovacchia", "+42")
            Formats._AddPrefixInt(4, "Repubblica Ceca", "+420")
            Formats._AddPrefixInt(4, "Slovacchia", "+421")
            Formats._AddPrefixInt(4, "Liechtenstein", "+423")
            Formats._AddPrefixInt(4, "Austria", "+43")
            Formats._AddPrefixInt(4, "Regno Unito", "+44")
            Formats._AddPrefixInt(4, "Danimarca", "+45")
            Formats._AddPrefixInt(4, "Svezia", "+46")
            Formats._AddPrefixInt(4, "Norvegia", "+47")
            Formats._AddPrefixInt(4, "Polonia", "+48")
            Formats._AddPrefixInt(4, "Germania", "+49")
            'Zona 5: Messico, America centrale e meridionale, Indie occidentali
            Formats._AddPrefixInt(5, "Isole Falkland", "+500")
            Formats._AddPrefixInt(5, "Belize", "+501")
            Formats._AddPrefixInt(5, "Guatemala", "+502")
            Formats._AddPrefixInt(5, "El Salvador", "+503")
            Formats._AddPrefixInt(5, "Honduras", "+504")
            Formats._AddPrefixInt(5, "Nicaragua", "+505")
            Formats._AddPrefixInt(5, "Costa Rica", "+506")
            Formats._AddPrefixInt(5, "Panamá", "+507")
            Formats._AddPrefixInt(5, "Saint Pierre e Miquelon", "+508")
            Formats._AddPrefixInt(5, "Haiti", "+509")
            Formats._AddPrefixInt(5, "Perù", "+51")
            Formats._AddPrefixInt(5, "Messico", "+52")
            Formats._AddPrefixInt(5, "Cuba", "+53")
            Formats._AddPrefixInt(5, "Argentina", "+54")
            Formats._AddPrefixInt(5, "Brasile", "+55")
            Formats._AddPrefixInt(5, "Cile", "+56")
            Formats._AddPrefixInt(5, "Colombia", "+57")
            Formats._AddPrefixInt(5, "Venezuela", "+58")
            Formats._AddPrefixInt(5, "Guadalupa", "+590")
            Formats._AddPrefixInt(5, "Bolivia", "+591")
            Formats._AddPrefixInt(5, "Guyana", "+592")
            Formats._AddPrefixInt(5, "Ecuador", "+593")
            Formats._AddPrefixInt(5, "Guyana Francese", "+594")
            Formats._AddPrefixInt(5, "Paraguay", "+595")
            Formats._AddPrefixInt(5, "Martinica", "+596")
            Formats._AddPrefixInt(5, "Suriname", "+597")
            Formats._AddPrefixInt(5, "Uruguay", "+598")
            Formats._AddPrefixInt(5, "era usato dalle Antille Olandesi ora divise in", "+599")
            Formats._AddPrefixInt(5, "Sint Eustatius", "+5993")
            Formats._AddPrefixInt(5, "Saba", "+5994")
            Formats._AddPrefixInt(5, "Sint Maarten", "+5995")
            Formats._AddPrefixInt(5, "Bonaire", "+5997")
            Formats._AddPrefixInt(5, "Curaçao", "+5999")
            'Zona 6: Oceano Pacifico meridionale e Oceania
            Formats._AddPrefixInt(6, "Malesia", "+60")
            Formats._AddPrefixInt(6, "Australia", "+61")
            Formats._AddPrefixInt(6, "Indonesia", "+62")
            Formats._AddPrefixInt(6, "Filippine", "+63")
            Formats._AddPrefixInt(6, "Nuova Zelanda", "+64")
            Formats._AddPrefixInt(6, "Singapore", "+65")
            Formats._AddPrefixInt(6, "Thailandia", "+66")
            Formats._AddPrefixInt(6, "Timor Est – era assegnato alle Isole Marianne Settentrionali, che ora usano il prefisso +1", "+670")
            Formats._AddPrefixInt(6, "era assegnato a Guam, che ora usa il prefisso +1", "+671")
            Formats._AddPrefixInt(6, "Territori Australiani Esterni: Antartide, Isola del Natale, Isole Cocos e Isola Norfolk", "+672")
            Formats._AddPrefixInt(6, "Brunei", "+673")
            Formats._AddPrefixInt(6, "Nauru", "+674")
            Formats._AddPrefixInt(6, "Papua Nuova Guinea", "+675")
            Formats._AddPrefixInt(6, "Tonga", "+676")
            Formats._AddPrefixInt(6, "Isole Salomone", "+677")
            Formats._AddPrefixInt(6, "Vanuatu", "+678")
            Formats._AddPrefixInt(6, "Figi", "+679")
            Formats._AddPrefixInt(6, "Palau", "+680")
            Formats._AddPrefixInt(6, "Wallis e Futuna", "+681")
            Formats._AddPrefixInt(6, "Isole Cook", "+682")
            Formats._AddPrefixInt(6, "Niue", "+683")
            Formats._AddPrefixInt(6, "Samoa Americane", "+684")
            Formats._AddPrefixInt(6, "Samoa", "+685")
            Formats._AddPrefixInt(6, "Kiribati, Isola Gilbert", "+686")
            Formats._AddPrefixInt(6, "Nuova Caledonia", "+687")
            Formats._AddPrefixInt(6, "Tuvalu, Isole Ellice", "+688")
            Formats._AddPrefixInt(6, "Polinesia Francese", "+689")
            Formats._AddPrefixInt(6, "Tokelau", "+690")
            Formats._AddPrefixInt(6, "Stati Federati di Micronesia", "+691")
            Formats._AddPrefixInt(6, "Isole Marshall", "+692")
            'Zona 7: Russia e Asia centrale (ex Unione Sovietica)
            Formats._AddPrefixInt(7, "Russia, Kazakistan", "+7")
            'Zona 8: Asia orientale e Servizi Speciali
            Formats._AddPrefixInt(8, "International Freephone", "+800")
            Formats._AddPrefixInt(8, "riservato per Shared Cost Services", "+808")
            Formats._AddPrefixInt(8, "Giappone", "+81")
            Formats._AddPrefixInt(8, "Corea del Sud", "+82")
            Formats._AddPrefixInt(8, "Vietnam", "+84")
            Formats._AddPrefixInt(8, "Corea del Nord", "+850")
            Formats._AddPrefixInt(8, "Hong Kong", "+852")
            Formats._AddPrefixInt(8, "Macao", "+853")
            Formats._AddPrefixInt(8, "Cambogia", "+855")
            Formats._AddPrefixInt(8, "Laos", "+856")
            Formats._AddPrefixInt(8, "Cina", "+86")
            Formats._AddPrefixInt(8, "Servizio Inmarsat 'SNAC'", "+870")
            Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+875")
            Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+876")
            Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+877")
            Formats._AddPrefixInt(8, "Universal Personal Telecommunications services", "+878")
            Formats._AddPrefixInt(8, "riservato per il Servizio Mobile Marittimo", "+879")
            Formats._AddPrefixInt(8, "Bangladesh", "+880")
            Formats._AddPrefixInt(8, "Mobile Satellite System", "+881")
            Formats._AddPrefixInt(8, "International Networks", "+882")
            Formats._AddPrefixInt(8, "Taiwan", "+886")
            'Zona 9: Asia occidentale e meridionale, Medio Oriente
            Formats._AddPrefixInt(9, "Turchia", "+90")
            Formats._AddPrefixInt(9, "India", "+91")
            Formats._AddPrefixInt(9, "Pakistan", "+92")
            Formats._AddPrefixInt(9, "Afghanistan", "+93")
            Formats._AddPrefixInt(9, "Sri Lanka", "+94")
            Formats._AddPrefixInt(9, "Myanmar", "+95")
            Formats._AddPrefixInt(9, "Maldive", "+960")
            Formats._AddPrefixInt(9, "Libano", "+961")
            Formats._AddPrefixInt(9, "Giordania", "+962")
            Formats._AddPrefixInt(9, "Siria", "+963")
            Formats._AddPrefixInt(9, "Iraq", "+964")
            Formats._AddPrefixInt(9, "Kuwait", "+965")
            Formats._AddPrefixInt(9, "Arabia Saudita", "+966")
            Formats._AddPrefixInt(9, "Yemen", "+967")
            Formats._AddPrefixInt(9, "Oman", "+968")
            Formats._AddPrefixInt(9, "era usato dalla Repubblica Democratica dello Yemen, ora unificata con lo Yemen (+967)", "+969")
            Formats._AddPrefixInt(9, "Palestina", "+970")
            Formats._AddPrefixInt(9, "Emirati Arabi Uniti", "+971")
            Formats._AddPrefixInt(9, "Israele", "+972")
            Formats._AddPrefixInt(9, "Bahrain", "+973")
            Formats._AddPrefixInt(9, "Qatar", "+974")
            Formats._AddPrefixInt(9, "Bhutan", "+975")
            Formats._AddPrefixInt(9, "Mongolia", "+976")
            Formats._AddPrefixInt(9, "Nepal", "+977")
            Formats._AddPrefixInt(9, "International Premium Rate Service", "+979")
            Formats._AddPrefixInt(9, "Iran", "+98")
            Formats._AddPrefixInt(9, "International Telecommunications Public Correspondence Service trial (ITPCS)", "+991")
            Formats._AddPrefixInt(9, "Tagikistan", "+992")
            Formats._AddPrefixInt(9, "Turkmenistan", "+993")
            Formats._AddPrefixInt(9, "Azerbaigian", "+994")
            Formats._AddPrefixInt(9, "Georgia", "+995")
            Formats._AddPrefixInt(9, "Kirghizistan", "+996")
            Formats._AddPrefixInt(9, "Uzbekistan", "+998")

            Formats._AddPrefix("Campioni d'Italia", "004191", "")
            Formats._AddPrefix("", "010", "")
            Formats._AddPrefix("", "011", "")
            Formats._AddPrefix("", "0121", "")
            Formats._AddPrefix("", "0122", "")
            Formats._AddPrefix("", "0123", "")
            Formats._AddPrefix("", "0124", "")
            Formats._AddPrefix("", "0125", "")
            Formats._AddPrefix("", "0131", "")
            Formats._AddPrefix("", "0141", "")
            Formats._AddPrefix("", "0142", "")
            Formats._AddPrefix("", "0143", "")
            Formats._AddPrefix("", "0144", "")
            Formats._AddPrefix("", "015", "")
            Formats._AddPrefix("", "0161", "")
            Formats._AddPrefix("", "0163", "")
            Formats._AddPrefix("", "0165", "")
            Formats._AddPrefix("", "0166", "")
            Formats._AddPrefix("", "0171", "")
            Formats._AddPrefix("", "0172", "")
            Formats._AddPrefix("", "0173", "")
            Formats._AddPrefix("", "0174", "")
            Formats._AddPrefix("", "0175", "")
            Formats._AddPrefix("", "0182", "")
            Formats._AddPrefix("", "0183", "")
            Formats._AddPrefix("", "0184", "")
            Formats._AddPrefix("", "0185", "")
            Formats._AddPrefix("", "0187", "")
            Formats._AddPrefix("", "019", "")
            Formats._AddPrefix("", "02", "")
            Formats._AddPrefix("", "030", "")
            Formats._AddPrefix("", "031", "")
            Formats._AddPrefix("", "0321", "")
            Formats._AddPrefix("", "0322", "")
            Formats._AddPrefix("", "0323", "")
            Formats._AddPrefix("", "0324", "")
            Formats._AddPrefix("", "0331", "")
            Formats._AddPrefix("", "0332", "")
            Formats._AddPrefix("", "0341", "")
            Formats._AddPrefix("", "0342", "")
            Formats._AddPrefix("", "0343", "")
            Formats._AddPrefix("", "0344", "")
            Formats._AddPrefix("", "0345", "")
            Formats._AddPrefix("", "0346", "")
            Formats._AddPrefix("", "035", "")
            Formats._AddPrefix("", "0362", "")
            Formats._AddPrefix("", "0363", "")
            Formats._AddPrefix("", "0364", "")
            Formats._AddPrefix("", "0365", "")
            Formats._AddPrefix("", "0371", "")
            Formats._AddPrefix("", "0372", "")
            Formats._AddPrefix("", "0373", "")
            Formats._AddPrefix("", "0374", "")
            Formats._AddPrefix("", "0375", "")
            Formats._AddPrefix("", "0376", "")
            Formats._AddPrefix("", "0377", "")
            Formats._AddPrefix("", "0381", "")
            Formats._AddPrefix("", "0382", "")
            Formats._AddPrefix("", "0383", "")
            Formats._AddPrefix("", "0384", "")
            Formats._AddPrefix("", "0385", "")
            Formats._AddPrefix("", "0386", "")
            Formats._AddPrefix("", "039", "")
            Formats._AddPrefix("", "040", "")
            Formats._AddPrefix("", "041", "")
            Formats._AddPrefix("", "0421", "")
            Formats._AddPrefix("", "0422", "")
            Formats._AddPrefix("", "0423", "")
            Formats._AddPrefix("", "0424", "")
            Formats._AddPrefix("", "0425", "")
            Formats._AddPrefix("", "0426", "")
            Formats._AddPrefix("", "0427", "")
            Formats._AddPrefix("", "0428", "")
            Formats._AddPrefix("", "0429", "")
            Formats._AddPrefix("", "0431", "")
            Formats._AddPrefix("", "0432", "")
            Formats._AddPrefix("", "0433", "")
            Formats._AddPrefix("", "0434", "")
            Formats._AddPrefix("", "0435", "")
            Formats._AddPrefix("", "0436", "")
            Formats._AddPrefix("", "0437", "")
            Formats._AddPrefix("", "0438", "")
            Formats._AddPrefix("", "0439", "")
            Formats._AddPrefix("", "0442", "")
            Formats._AddPrefix("", "0444", "")
            Formats._AddPrefix("", "0445", "")
            Formats._AddPrefix("", "045", "")
            Formats._AddPrefix("", "0461", "")
            Formats._AddPrefix("", "0462", "")
            Formats._AddPrefix("", "0463", "")
            Formats._AddPrefix("", "0464", "")
            Formats._AddPrefix("", "0465", "")
            Formats._AddPrefix("", "0471", "")
            Formats._AddPrefix("", "0472", "")
            Formats._AddPrefix("", "0473", "")
            Formats._AddPrefix("", "0474", "")
            Formats._AddPrefix("", "0481", "")
            Formats._AddPrefix("", "049", "")
            Formats._AddPrefix("", "050", "")
            Formats._AddPrefix("", "051", "")
            Formats._AddPrefix("", "0521", "")
            Formats._AddPrefix("", "0522", "")
            Formats._AddPrefix("", "0523", "")
            Formats._AddPrefix("", "0524", "")
            Formats._AddPrefix("", "0525", "")
            Formats._AddPrefix("", "0532", "")
            Formats._AddPrefix("", "0533", "")
            Formats._AddPrefix("", "0534", "")
            Formats._AddPrefix("", "0535", "")
            Formats._AddPrefix("", "0536", "")
            Formats._AddPrefix("", "0541", "")
            Formats._AddPrefix("", "0542", "")
            Formats._AddPrefix("", "0543", "")
            Formats._AddPrefix("", "0544", "")
            Formats._AddPrefix("", "0545", "")
            Formats._AddPrefix("", "0546", "")
            Formats._AddPrefix("", "0547", "")
            Formats._AddPrefix("", "055", "")
            Formats._AddPrefix("", "0564", "")
            Formats._AddPrefix("", "0565", "")
            Formats._AddPrefix("", "0566", "")
            Formats._AddPrefix("", "0571", "")
            Formats._AddPrefix("", "0572", "")
            Formats._AddPrefix("", "0573", "")
            Formats._AddPrefix("", "0574", "")
            Formats._AddPrefix("", "0575", "")
            Formats._AddPrefix("", "0577", "")
            Formats._AddPrefix("", "0578", "")
            Formats._AddPrefix("", "0583", "")
            Formats._AddPrefix("", "0584", "")
            Formats._AddPrefix("", "0585", "")
            Formats._AddPrefix("", "0586", "")
            Formats._AddPrefix("", "0587", "")
            Formats._AddPrefix("", "0588", "")
            Formats._AddPrefix("", "059", "")
            Formats._AddPrefix("", "06", "")
            Formats._AddPrefix("", "070", "")
            Formats._AddPrefix("", "071", "")
            Formats._AddPrefix("", "0721", "")
            Formats._AddPrefix("", "0722", "")
            Formats._AddPrefix("", "0731", "")
            Formats._AddPrefix("", "0732", "")
            Formats._AddPrefix("", "0733", "")
            Formats._AddPrefix("", "0734", "")
            Formats._AddPrefix("", "0735", "")
            Formats._AddPrefix("", "0736", "")
            Formats._AddPrefix("", "0737", "")
            Formats._AddPrefix("", "0742", "")
            Formats._AddPrefix("", "0743", "")
            Formats._AddPrefix("", "0744", "")
            Formats._AddPrefix("", "0746", "")
            Formats._AddPrefix("", "075", "")
            Formats._AddPrefix("", "0761", "")
            Formats._AddPrefix("", "0763", "")
            Formats._AddPrefix("", "0765", "")
            Formats._AddPrefix("", "0766", "")
            Formats._AddPrefix("", "0771", "")
            Formats._AddPrefix("", "0773", "")
            Formats._AddPrefix("", "0774", "")
            Formats._AddPrefix("", "0775", "")
            Formats._AddPrefix("", "0776", "")
            Formats._AddPrefix("", "0781", "")
            Formats._AddPrefix("", "0782", "")
            Formats._AddPrefix("", "0783", "")
            Formats._AddPrefix("", "0784", "")
            Formats._AddPrefix("", "0785", "")
            Formats._AddPrefix("", "0789", "")
            Formats._AddPrefix("", "079", "")
            Formats._AddPrefix("", "080", "")
            Formats._AddPrefix("", "081", "")
            Formats._AddPrefix("", "0823", "")
            Formats._AddPrefix("", "0824", "")
            Formats._AddPrefix("", "0825", "")
            Formats._AddPrefix("", "0827", "")
            Formats._AddPrefix("", "0828", "")
            Formats._AddPrefix("", "0831", "")
            Formats._AddPrefix("", "0832", "")
            Formats._AddPrefix("", "0833", "")
            Formats._AddPrefix("", "0835", "")
            Formats._AddPrefix("", "0836", "")
            Formats._AddPrefix("", "085", "")
            Formats._AddPrefix("", "0861", "")
            Formats._AddPrefix("", "0862", "")
            Formats._AddPrefix("", "0863", "")
            Formats._AddPrefix("", "0864", "")
            Formats._AddPrefix("", "0865", "")
            Formats._AddPrefix("", "0871", "")
            Formats._AddPrefix("", "0872", "")
            Formats._AddPrefix("", "0873", "")
            Formats._AddPrefix("", "0874", "")
            Formats._AddPrefix("", "0875", "")
            Formats._AddPrefix("", "0881", "")
            Formats._AddPrefix("", "0882", "")
            Formats._AddPrefix("", "0883", "")
            Formats._AddPrefix("", "0884", "")
            Formats._AddPrefix("", "0885", "")
            Formats._AddPrefix("", "089", "")
            Formats._AddPrefix("", "090", "")
            Formats._AddPrefix("", "091", "")
            Formats._AddPrefix("", "0921", "")
            Formats._AddPrefix("", "0922", "")
            Formats._AddPrefix("", "0923", "")
            Formats._AddPrefix("", "0924", "")
            Formats._AddPrefix("", "0925", "")
            Formats._AddPrefix("", "0931", "")
            Formats._AddPrefix("", "0932", "")
            Formats._AddPrefix("", "0933", "")
            Formats._AddPrefix("", "0934", "")
            Formats._AddPrefix("", "0935", "")
            Formats._AddPrefix("", "0941", "")
            Formats._AddPrefix("", "0942", "")
            Formats._AddPrefix("", "095", "")
            Formats._AddPrefix("", "0961", "")
            Formats._AddPrefix("", "0962", "")
            Formats._AddPrefix("", "0963", "")
            Formats._AddPrefix("", "0964", "")
            Formats._AddPrefix("", "0965", "")
            Formats._AddPrefix("", "0966", "")
            Formats._AddPrefix("", "0967", "")
            Formats._AddPrefix("", "0968", "")
            Formats._AddPrefix("", "0971", "")
            Formats._AddPrefix("", "0972", "")
            Formats._AddPrefix("", "0973", "")
            Formats._AddPrefix("", "0974", "")
            Formats._AddPrefix("", "0975", "")
            Formats._AddPrefix("", "0976", "")
            Formats._AddPrefix("", "0981", "")
            Formats._AddPrefix("", "0982", "")
            Formats._AddPrefix("", "0983", "")
            Formats._AddPrefix("", "0984", "")
            Formats._AddPrefix("", "0985", "")
            Formats._AddPrefix("", "099", "")
            Formats._AddPrefix("TIM", "330", "")
            Formats._AddPrefix("TIM", "331", "")
            Formats._AddPrefix("TIM", "333", "")
            Formats._AddPrefix("TIM", "334", "")
            Formats._AddPrefix("TIM", "335", "")
            Formats._AddPrefix("TIM", "336")
            Formats._AddPrefix("TIM", "337")
            Formats._AddPrefix("TIM", "338")
            Formats._AddPrefix("TIM", "339")
            Formats._AddPrefix("TIM", "360")
            Formats._AddPrefix("TIM", "361*")
            Formats._AddPrefix("TIM", "362*")
            Formats._AddPrefix("TIM", "363*")

            Formats._AddPrefix("TIM", "366")
            Formats._AddPrefix("TIM", "368")
            Formats._AddPrefix("Vodafone", "340")
            Formats._AddPrefix("Vodafone", "341*")
            Formats._AddPrefix("Vodafone", "342")
            Formats._AddPrefix("Vodafone", "343*")
            Formats._AddPrefix("Vodafone", "345")
            Formats._AddPrefix("Vodafone", "346")
            Formats._AddPrefix("Vodafone", "347")
            Formats._AddPrefix("Vodafone", "348")
            Formats._AddPrefix("Vodafone", "349")
            Formats._AddPrefix("Vodafone", "383*")
            Formats._AddPrefix("Wind", "320")
            Formats._AddPrefix("Wind", "322*")
            Formats._AddPrefix("Wind", "323*")
            Formats._AddPrefix("Wind", "324")
            Formats._AddPrefix("Wind", "327")
            Formats._AddPrefix("Wind", "328")
            Formats._AddPrefix("Wind", "329")
            Formats._AddPrefix("Wind", "380")
            Formats._AddPrefix("Wind", "383")
            Formats._AddPrefix("Wind", "388")
            Formats._AddPrefix("Wind", "389")
            Formats._AddPrefix("Tre", "390*")
            Formats._AddPrefix("Tre", "391")
            Formats._AddPrefix("Tre", "392")
            Formats._AddPrefix("Tre", "393")
            Formats._AddPrefix("Tre", "397*")
            Formats._AddPrefix("RFI", "313")
            'Formats._AddPrefix("A-Mobile (Wind)", "389")
            Formats._AddPrefix("BIP MOBILE (H3G)", "373")
            Formats._AddPrefix("BT Mobile (Vodafone)", "377")
            Formats._AddPrefix("CoopVoce (TIM)", "331")
            Formats._AddPrefix("CoopVoce (TIM)", "370")
            Formats._AddPrefix("CoopVoce (TIM)", "371")
            Formats._AddPrefix("CoopVoce (TIM)", "372")
            Formats._AddPrefix("CoopVoce (TIM)", "373")
            Formats._AddPrefix("Daily Telecom Mobile (Vodafone)", "377")
            Formats._AddPrefix("Daily Telecom Mobile (Vodafone)", "378")
            Formats._AddPrefix("ERG Mobile (Vodafone)", "375")
            Formats._AddPrefix("ERG Mobile (Vodafone)", "376")
            Formats._AddPrefix("ERG Mobile (Vodafone)", "377")
            '//Fastweb (Tre): 373
            '//Italia Mobile: n.d.
            '//MTV Mobile (TIM): 331, 366
            '//Noverca (TIM): 350-5, 370-7
            '//PlusCom (H3G): 373
            '//PosteMobile (Vodafone): 377-1, 377-2, 377-4, 377-9
            '//Tiscali Mobile (TIM): 370-1
            '//UNOMobile (Vodafone): 377-3
            '//Telogic (H3G): 373
            Me.m_Prefissi.Comparer = New _prefixComparer
            Me.m_Prefissi.Sort()
        End Sub

        Private Class _prefixComparer
            Implements IComparer

            Public Function Compare(ByVal a As CPhonePrefix, ByVal b As CPhonePrefix) As Integer
                If (a.zona <> 0) And (b.zona = 0) Then Return -1
                If (a.zona = 0) And (b.zona <> 0) Then Return 1
                Return (b.per.Length - a.per.Length)
            End Function

            Public Function Compare12(x As Object, y As Object) As Integer Implements IComparer.Compare
                Return Me.Compare(x, y)
            End Function
        End Class


        Public Function ParsePhoneNumber(ByVal text As String) As String
            Dim i As Integer
            Dim ch, ret As String
            text = Strings.Trim(text)

            For i = 1 To Len(text)
                If Char.IsLetter(Mid(text, i, 1)) Then Return text
            Next

            If (Left(text, 2) = "00") Then text = "+" & Mid(text, 3)
            ret = ""
            For i = 1 To Len(text)
                ch = Mid(text, i, 1)
                If (InStr(Formats._validPhoneNumberChars, ch) >= 1) Then ret &= ch
            Next
            Return ret
        End Function

        Public Function FormatPhoneNumber(ByVal text As String) As String
            'Dim i As Integer
            'Dim ret As String
            'Dim p As Integer
            'text = Formats.ParsePhoneNumber(text)
            'If (text = vbNullString) Then Return vbNullString

            'If (Formats._phPrefx Is Nothing) Then Formats._InitPhonePrefixes()
            'p = 1
            'ret = ""
            'For i = 0 To Formats._phPrefx.Count - 1
            '    With Formats._phPrefx(i)
            '        If (Mid(text, p, .per.Length) = .per) Then
            '            ret &= .per & " "
            '            p += .per.Length
            '        End If
            '    End With
            'Next
            'ret &= Mid(text, p)
            'Return ret
            text = Me.ParsePhoneNumber(text)
            Dim i As CNumberInfo = Me.GetInfoNumero(text)
            Return i.ToString
        End Function

        ''' <summary>
        ''' Formatta un intervallo temporale misurato in secondi in una stringa che mostra gg hh mm ss
        ''' </summary>
        ''' <param name="seconds"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FormatDurata(ByVal seconds As Nullable(Of Long)) As String
            If (seconds.HasValue = False) Then Return ""
            Dim g, h, m, s As Integer
            s = seconds Mod 60
            seconds = Math.Floor(seconds.Value / 60)
            m = seconds Mod 60
            seconds = Math.Floor(seconds.Value / 60)
            h = seconds Mod 60
            g = Math.Floor(seconds.Value / 60)

            Dim ret As String = ""
            If (g > 0) Then ret = Strings.Combine(ret, g & " gg", " ")
            If (h > 0) Then ret = Strings.Combine(ret, h & " h", " ")
            If (m > 0) Then ret = Strings.Combine(ret, m & " m", " ")
            If (s > 0) Then ret = Strings.Combine(ret, s & " s", " ")

            Return ret
        End Function

        Public Function IsEMailAddress(ByVal value As String) As Boolean
            Return Me.ParseEMailAddress(value) <> ""
        End Function

        Public Function ParseEMailAddress(ByVal value As String) As String
            Try
                Dim m As New System.Net.Mail.MailAddress(value)
                Return m.Address
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Function FormatEMailAddress(ByVal value As String) As String
            Return Me.ParseEMailAddress(value)
        End Function

        Public Function ParseWebAddress(ByVal value As String) As String
            Dim ret As String = LCase(Replace(value, " ", ""))
            If (ret = "") Then Return ""
            If (Left(ret, 7) = "http://") Then
                Return ret
            ElseIf (Left(ret, 8) = "https://") Then
                Return ret
            ElseIf (Left(ret, 6) = "ftp://") Then
                Return ret
            ElseIf (Left(ret, 7) = "ftps://") Then
                Return ret
            ElseIf (Left(ret, 4) = "ftp.") Then
                Return "ftp://" & ret
            Else
                Return "http://" & ret
            End If
        End Function

        Public Function FormatWebAddress(ByVal value As String) As String
            Dim ret As String = LCase(Replace(value, " ", ""))
            If (ret = "") Then Return ""
            If (Left(ret, 7) = "http://") Then
                Return ret
            ElseIf (Left(ret, 8) = "https://") Then
                Return ret
            ElseIf (Left(ret, 6) = "ftp://") Then
                Return ret
            ElseIf (Left(ret, 7) = "ftps://") Then
                Return ret
            ElseIf (Left(ret, 4) = "ftp.") Then
                Return "ftp://" & ret
            Else
                Return "http://" & ret
            End If
        End Function

#Region "USA"

        Public NotInheritable Class CUSAClass
            Private m_CultureInfo As System.Globalization.CultureInfo

            Friend Sub New(ByVal culture As String)
                Me.m_CultureInfo = New System.Globalization.CultureInfo(culture)
            End Sub

            Public Function ParseSingle(ByVal value As String) As Single
                Return Single.Parse(value, m_CultureInfo)
            End Function

            Public Function FormatSingle(ByVal value As Single) As String
                Return value.ToString(m_CultureInfo)
            End Function

            Public Function ParseDouble(ByVal value As String) As Double?
                value = Trim(value)
                If (value = "") Then Return Nothing
                Return Double.Parse(value, m_CultureInfo)
            End Function

            Public Function FormatDouble(ByVal value As Object) As String
                Try
                    Return CType(value, Double).ToString(m_CultureInfo)
                Catch ex As Exception
                    Return ""
                End Try
            End Function

            Function FormatNumber(ByVal value As Object) As String
                Return Me.FormatDouble(value)
            End Function

            Function FormatNumber(ByVal value As Object, ByVal decimals As Integer) As String
                Try
                    Dim tmp As System.Globalization.CultureInfo = Me.m_CultureInfo.Clone
                    tmp.NumberFormat.NumberDecimalDigits = decimals
                    Return CType(value, Double).ToString(tmp)
                Catch ex As Exception
                    Return ""
                End Try
            End Function

            Function ToDouble(ByVal value As String) As Double
                Try
                    Return Double.Parse(value, Me.m_CultureInfo)
                Catch ex As Exception
                    Return 0
                End Try
            End Function

            Function ParseDate(ByVal str As String) As Date?
                str = Trim(str)
                If (str = "") Then Return Nothing
                Return Date.Parse(str, Me.m_CultureInfo)
            End Function

        End Class

        Private m_USA As CUSAClass = Nothing

        Public ReadOnly Property USA As CUSAClass
            Get
                If (Me.m_USA Is Nothing) Then Me.m_USA = New CUSAClass("en-US")
                Return Me.m_USA
            End Get
        End Property


#End Region

        ''' <summary>
        ''' Restituisce una stringa nel formato YYYYMMDDHHnnssmmm"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTimeStamp() As String
            SyncLock (Me)
                Dim d As Date = Calendar.Now
                Return Right("0000" & d.Year, 4) & Right("00" & d.Month, 2) & Right("00" & d.Day, 2) &
                       Right("00" & d.Hour, 2) & Right("00" & d.Minute, 2) & Right("00" & d.Second, 2) &
                       Right("000" & d.Millisecond, 3)
            End SyncLock
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Private Shared m_Formats As CFormatsClass = Nothing

    Public Shared ReadOnly Property Formats As CFormatsClass
        Get
            If (m_Formats Is Nothing) Then m_Formats = New CFormatsClass
            Return m_Formats
        End Get
    End Property

End Class
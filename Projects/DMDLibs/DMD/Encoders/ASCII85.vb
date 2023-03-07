Imports System
Imports System.Text
Imports System.IO

Partial Public Class Encoding

    Public Class Ascii85
        Inherits Encoder

        ''' <summary>
        ''' Prefix mark that identifies an encoded ASCII85 string, traditionally &lt;~
        ''' </summary>
        ''' <remarks></remarks>
        Public PrefixMark As String = "<~"
        ''' <summary>
        ''' Suffix mark that identifies an encoded ASCII85 string, traditionally ~&gt;
        ''' </summary>
        ''' <remarks></remarks>
        Public SuffixMark As String = "~>"

        ''' <summary>
        ''' Maximum line length for encoded ASCII85 string;
        ''' set to zero for one unbroken line.
        ''' </summary>
        ''' <remarks></remarks>
        Public LineLength As Integer = 75

        ''' <summary>
        ''' Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding
        ''' </summary>
        ''' <remarks></remarks>
        Public Property EnforceMarks As Boolean = True

        Private Const _asciiOffset As Integer = 33
        Private _encodedBlock As Byte()
        Private _decodedBlock As Byte()
        Private _tuple As UInt32 = 0
        Private _linePos As Integer = 0
        Private zMark As Char = CChar("z")

        Private pow85 As UInt32() = {(85 * 85 * 85 * 85), (85 * 85 * 85), (85 * 85), 85, 1}

        Public Sub New()
            _encodedBlock = CType(Array.CreateInstance(GetType(Byte), 5), Byte())
            _decodedBlock = CType(Array.CreateInstance(GetType(Byte), 4), Byte())
        End Sub

        Private Sub Reset()
            Array.Clear(_encodedBlock, 0, _encodedBlock.Length)
            Array.Clear(_decodedBlock, 0, _decodedBlock.Length)
            _tuple = 0
            _linePos = 0


        End Sub

        Public Overrides Function Decode(s() As Byte) As Byte()
            Dim b As String = System.Text.Encoding.Default.GetString(s)
            Return Me.Decode1(b)
        End Function

        ''' <summary>
        ''' Decodes an ASCII85 encoded string into the original binary data
        ''' </summary>
        ''' <param name="s">ASCII85 encoded string</param>
        ''' <returns>byte array of decoded binary data</returns>
        ''' <remarks></remarks>
        Private Function Decode1(s As String) As Byte()
            Reset()
            If (EnforceMarks) Then
                If (Not s.StartsWith(PrefixMark) Or Not s.EndsWith(SuffixMark)) Then
                    Throw New Exception("ASCII85 encoded data should begin with '" & PrefixMark & "' and end with '" + SuffixMark + "'")
                End If

                ' strip prefix and suffix if present
                If (s.StartsWith(PrefixMark)) Then
                    s = s.Substring(PrefixMark.Length)
                End If
                If (s.EndsWith(SuffixMark)) Then
                    s = s.Substring(0, s.Length - SuffixMark.Length)
                End If
            End If

            Using ms As New MemoryStream()

                Dim count As Integer = 0
                Dim processChar As Boolean = False


                For Each c As Char In s
                    Select Case c
                        Case zMark
                            If (count <> 0) Then
                                Throw New Exception("The character 'z' is invalid inside an ASCII85 block.")
                            End If
                            _decodedBlock(0) = 0
                            _decodedBlock(1) = 0
                            _decodedBlock(2) = 0
                            _decodedBlock(3) = 0
                            ms.Write(_decodedBlock, 0, _decodedBlock.Length)
                            processChar = False
                        Case Chr(10), Chr(13), Chr(9), Chr(0), Chr(12), Chr(8)
                            processChar = False
                        Case Else
                            If (c < "!" Or c > "u") Then
                                Throw New Exception("Bad character '" + c + "' found. ASCII85 only allows characters '!' to 'u'.")
                            End If
                            processChar = True
                    End Select

                    If (processChar) Then

                        _tuple += (CType((Asc(c) - _asciiOffset) * pow85(count), UInteger))
                        count += 1
                        If (count = _encodedBlock.Length) Then
                            DecodeBlock()
                            ms.Write(_decodedBlock, 0, _decodedBlock.Length)
                            _tuple = 0
                            count = 0
                        End If
                    End If
                Next

                'if we have some bytes left over at the end..
                If (count <> 0) Then
                    If (count = 1) Then
                        Throw New Exception("The last block of ASCII85 data cannot be a single byte.")
                    End If
                    count -= 1
                    _tuple += pow85(count)
                    DecodeBlock(count)
                    Dim i As Integer
                    While (i < count)
                        ms.WriteByte(_decodedBlock(i))
                        i += 1
                    End While
                End If

                Return ms.ToArray()
            End Using
        End Function

        ''' <summary>
        '''Encodes binary data into a plaintext ASCII85 format string
        ''' </summary>
        ''' <param name="ba">binary data to encode</param>
        ''' <returns>ASCII85 encoded string</returns>
        ''' <remarks></remarks>
        Public Function Encode1(ba As Byte()) As String
            Reset()
            Dim sb As New StringBuilder(CInt(ba.Length * (_encodedBlock.Length / _decodedBlock.Length)))
            _linePos = 0

            If EnforceMarks Then
                AppendString(sb, PrefixMark)
            End If

            Dim count As Integer = 0
            _tuple = 0
            For Each b As Byte In ba
                If count >= _decodedBlock.Length - 1 Then
                    _tuple = _tuple Or b
                    If _tuple = 0 Then
                        AppendChar(sb, zMark)
                    Else
                        EncodeBlock(sb)
                    End If
                    _tuple = 0
                    count = 0

                Else
                    _tuple = _tuple Or (CUInt(b) << 24 - (count * 8))
                    count += 1
                End If
            Next

            ' if we have some bytes left over at the end..
            If count > 0 Then
                EncodeBlock(count + 1, sb)
            End If

            If EnforceMarks Then
                AppendString(sb, SuffixMark)
            End If
            Return sb.ToString()
        End Function

        Private Sub EncodeBlock(sb As StringBuilder)
            EncodeBlock(_encodedBlock.Length, sb)
        End Sub


        Private Sub EncodeBlock(count As Integer, sb As StringBuilder)
            For i As Integer = _encodedBlock.Length - 1 To 0 Step -1
                _encodedBlock(i) = CByte(((_tuple Mod 85) + &H21))
                _tuple = CUInt(_tuple \ 85)
            Next

            For i As Integer = 0 To count - 1
                Me.AppendChar(sb, Chr(_encodedBlock(i)))
            Next
        End Sub

        Private Sub DecodeBlock()
            DecodeBlock(_decodedBlock.Length)
        End Sub


        Private Sub DecodeBlock(bytes As Integer)
            For i As Integer = 0 To bytes - 1
                _decodedBlock(i) = CType((_tuple >> (24 - (i * 8))) And 255, Byte)
            Next
        End Sub

        Private Sub AppendString(sb As StringBuilder, s As String)
            If (LineLength > 0 AndAlso (_linePos + s.Length > LineLength)) Then
                _linePos = 0
                sb.AppendLine()
            Else
                _linePos += s.Length
            End If
            sb.Append(s)
        End Sub

        Private Sub AppendChar(sb As StringBuilder, c As Char)
            sb.Append(c)
            _linePos += 1
            If (LineLength > 0 AndAlso (_linePos >= LineLength)) Then
                _linePos = 0
                sb.AppendLine()
            End If
        End Sub

        Public Overloads Overrides Function Encode(s() As Byte) As Byte()
            Return System.Text.Encoding.Default.GetBytes(Me.Encode1(s))
        End Function
    End Class

End Class
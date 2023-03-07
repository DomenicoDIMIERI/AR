Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Net.Mime

    ''' <summary>
    ''' This class is based on the QuotedPrintable class written by Bill Gearhart
    ''' found at http:'www.aspemporium.com/classes.aspx?cid=6
    ''' </summary>
    Public NotInheritable Class QuotedPrintableEncodingq

        Private Const Equal As String = "="
        Private Const HexPattern As String = "(\=([0-9A-F][0-9A-F]))"

        Public Shared Function Decode(ByVal contents As String) As String
            If (contents Is Nothing) Then Throw New ArgumentNullException("contents")

            Using writer As New StringWriter()
                Using reader As New StringReader(contents)
                    Dim line As String = reader.ReadLine()
                    While (line IsNot Nothing)
                        '/*remove trailing line whitespace that may have
                        ' been added by a mail transfer agent per rule
                        ' #3 of the Quoted Printable section of RFC 1521.*/
                        line.TrimEnd()

                        If (line.EndsWith(Equal)) Then
                            writer.Write(DecodeLine(line))
                            'handle soft line breaks for lines that end with an "="
                        Else
                            writer.WriteLine(DecodeLine(line))
                        End If

                        line = reader.ReadLine()
                    End While
                End Using
                writer.Flush()

                Return writer.ToString()
            End Using
        End Function

        Private Shared Function DecodeLine(ByVal line As String) As String
            If (line Is Nothing) Then Throw New ArgumentNullException("line")
            Dim hexRegex As New Regex(HexPattern, RegexOptions.IgnoreCase)
            Return hexRegex.Replace(line, New MatchEvaluator(AddressOf HexMatchEvaluator))
        End Function

        Private Shared Function HexMatchEvaluator(ByVal m As Match) As String
            Dim dec As Integer = Convert.ToInt32(m.Groups(2).Value, 16)
            Dim character As Char = Convert.ToChar(dec)
            Return character.ToString()
        End Function

    End Class

End Namespace

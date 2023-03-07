Imports FinSeA.Io
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.persistence.util

Namespace org.apache.pdfbox.filter

    '/**
    ' * This is the used for the ASCIIHexDecode filter.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */
    Public Class ASCIIHexFilter
        Implements Filter

        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(ASCIIHexFilter.class);
        '/**
        ' * Whitespace.
        ' *   0  0x00  Null (NUL)
        ' *   9  0x09  Tab (HT)
        ' *  10  0x0A  Line feed (LF)
        ' *  12  0x0C  Form feed (FF)
        ' *  13  0x0D  Carriage return (CR)
        ' *  32  0x20  Space (SP)  
        ' */

        Private Function isWhitespace(ByVal c As Integer) As Boolean
            Return c = 0 OrElse c = 9 OrElse c = 10 OrElse c = 12 OrElse c = 13 OrElse c = 32
        End Function

        Private Function isEOD(ByVal c As Integer) As Boolean
            Return (c = 62) ' '>' - EOD
        End Function

        '/**
        '  * {@inheritDoc}
        '  */
        Public Sub decode(ByVal compressedData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.decode
            Dim value As Integer = 0
            Dim firstByte As Integer = 0
            Dim secondByte As Integer = 0
            firstByte = compressedData.ReadByte()
            While (firstByte <> -1)
                'always after first char
                While (isWhitespace(firstByte))
                    firstByte = compressedData.ReadByte()
                End While
                If (isEOD(firstByte)) Then
                    Exit While
                End If

                If (REVERSE_HEX(firstByte) = -1) Then
                    Debug.Print("Invalid Hex Code; int: " & firstByte & " char: " & Convert.ToChar(firstByte))
                End If
                value = REVERSE_HEX(firstByte) * 16
                secondByte = compressedData.ReadByte()

                If (isEOD(secondByte)) Then
                    ' second value behaves like 0 in case of EOD
                    result.WriteByte(value)
                    Exit While
                End If
                If (secondByte >= 0) Then
                    If (REVERSE_HEX(secondByte) = -1) Then
                        Debug.Print("Invalid Hex Code; int: " & secondByte & " char: " & Convert.ToChar(secondByte))
                    End If
                    value += REVERSE_HEX(secondByte)
                End If
                result.WriteByte(value)
                firstByte = compressedData.ReadByte()
            End While
            result.Flush()
        End Sub

        Private Shared ReadOnly REVERSE_HEX() As Integer = {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1, -1, -1, -1, -1, -1, -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 10, 11, 12, 13, 14, 15}
        'private shared ReadOnly  REVERSE_HEX () as Integer = {
        '  {
        '      -1, //0
        '      -1, //1
        '      -1, //2
        '      -1, //3
        '      -1, //4
        '      -1, //5
        '      -1, //6
        '      -1, //7
        '      -1, //8
        '      -1, //9
        '      -1, //10
        '      -1, //11
        '      -1, //12
        '      -1, //13
        '      -1, //14
        '      -1, //15
        '      -1, //16
        '      -1, //17
        '      -1, //18
        '      -1, //19
        '      -1, //20
        '      -1, //21
        '      -1, //22
        '      -1, //23
        '      -1, //24
        '      -1, //25
        '      -1, //26
        '      -1, //27
        '      -1, //28
        '      -1, //29
        '      -1, //30
        '      -1, //31
        '      -1, //32
        '      -1, //33
        '      -1, //34
        '      -1, //35
        '      -1, //36
        '      -1, //37
        '      -1, //38
        '      -1, //39
        '      -1, //40
        '      -1, //41
        '      -1, //42
        '      -1, //43
        '      -1, //44
        '      -1, //45
        '      -1, //46
        '      -1, //47
        '       0, //48
        '       1, //49
        '       2, //50
        '       3, //51
        '       4, //52
        '       5, //53
        '       6, //54
        '       7, //55
        '       8, //56
        '       9, //57
        '      -1, //58
        '      -1, //59
        '      -1, //60
        '      -1, //61
        '      -1, //62
        '      -1, //63
        '      -1, //64
        '      10, //65
        '      11, //66
        '      12, //67
        '      13, //68
        '      14, //69
        '      15, //70
        '      -1, //71
        '      -1, //72
        '      -1, //73
        '      -1, //74
        '      -1, //75
        '      -1, //76
        '      -1, //77
        '      -1, //78
        '      -1, //79
        '      -1, //80
        '      -1, //81
        '      -1, //82
        '      -1, //83
        '      -1, //84
        '      -1, //85
        '      -1, //86
        '      -1, //87
        '      -1, //88
        '      -1, //89
        '      -1, //90
        '      -1, //91
        '      -1, //92
        '      -1, //93
        '      -1, //94
        '      -1, //95
        '      -1, //96
        '      10, //97
        '      11, //98
        '      12, //99
        '      13, //100
        '      14, //101
        '      15, //102
        '  } 
        '/**
        ' * {@inheritDoc}
        ' */
        Public Sub encode(ByVal rawData As InputStream, ByVal result As OutputStream, ByVal options As COSDictionary, ByVal filterIndex As Integer) Implements Filter.encode
            Dim byteRead As Integer = 0
            byteRead = rawData.ReadByte()
            While (byteRead <> -1)
                Dim value As Integer = (byteRead + 256) Mod 256
                result.Write(COSHEXTable.TABLE(value))
                byteRead = rawData.ReadByte()
            End While
            result.Flush()
        End Sub


    End Class

End Namespace
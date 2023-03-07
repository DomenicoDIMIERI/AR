Imports FinSeA.Io

Imports System.IO

Namespace org.apache.pdfbox.io


    '/**
    ' * This class represents an ASCII85 output stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' *
    ' */
    Public Class ASCII85OutputStream
        Inherits FilterOutputStream

        Private lineBreak As Integer
        Private count As Integer

        Private indata() As Byte
        Private outdata() As Byte

        '/**
        ' * Function produces five ASCII printing characters from
        ' * four bytes of binary data.
        ' */
        Private maxline As Integer
        Private flushed As Boolean
        Private terminator As Char
        Private Shared ReadOnly OFFSET As Integer = AscW("!")
        Private Shared ReadOnly NEWLINE As Integer = AscW(vbLf) ' '\n';
        Private Shared ReadOnly Z As Integer = AscW("z")

        '/**
        ' * Constructor.
        ' *
        ' * @param out The output stream to write to.
        ' */
        Public Sub New(ByVal out As Stream)
            MyBase.New(out)
            lineBreak = 36 * 2
            maxline = 36 * 2
            count = 0
            ReDim indata(4 - 1)
            ReDim outdata(5 - 1)
            flushed = True
            terminator = "~"
        End Sub

        '/**
        ' * This will set the terminating character.
        ' *
        ' * @param term The terminating character.
        ' */
        Public Sub setTerminator(ByVal term As Char)
            Dim t As Integer = Convert.ToInt16(term)
            If (t < 118 OrElse t > 126 OrElse t = Z) Then
                Throw New ArgumentException("Terminator must be 118-126 excluding z")
            End If
            terminator = term
        End Sub

        '/**
        ' * This will get the terminating character.
        ' *
        ' * @return The terminating character.
        ' */
        Public Function getTerminator() As Char
            Return terminator
        End Function

        '/**
        ' * This will set the line length that will be used.
        ' *
        ' * @param l The length of the line to use.
        ' */
        Public Sub setLineLength(ByVal l As Integer)
            If (lineBreak > l) Then
                lineBreak = l
            End If
            maxline = l
        End Sub

        '/**
        ' * This will get the length of the line.
        ' *
        ' * @return The line length attribute.
        ' */
        Public Function getLineLength() As Integer
            Return maxline
        End Function

        '/**
        ' * This will transform the next four ascii bytes.
        ' */
        Private Sub transformASCII85()
            Dim word As Integer = ((((indata(0) << 8) Or (indata(1) And &HFF)) << 16) Or ((indata(2) And &HFF) << 8) Or (indata(3) And &HFF)) And &HFFFFFFFFL

            If (word = 0) Then
                outdata(0) = Convert.ToByte(Z)
                outdata(1) = 0
                Return
            End If
            Dim x As Integer
            x = word / (85L * 85L * 85L * 85L)
            outdata(0) = (x + Convert.ToInt16(OFFSET))
            word -= x * 85L * 85L * 85L * 85L

            x = word / (85L * 85L * 85L)
            outdata(1) = (x + Convert.ToInt16(OFFSET))
            word -= x * 85L * 85L * 85L

            x = word / (85L * 85L)
            outdata(2) = (x + Convert.ToInt16(OFFSET))
            word -= x * 85L * 85L

            x = word / 85L
            outdata(3) = (x + Convert.ToInt16(OFFSET))

            outdata(4) = ((word Mod 85L) + Convert.ToInt16(OFFSET))
        End Sub

        '/**
        ' * This will write a single byte.
        ' *
        ' * @param b The byte to write.
        ' *
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Overrides Sub write(ByVal b As Integer) ' throws IOException
            flushed = False
            indata(count) = b : count += 1
            If (count < 4) Then
                Return
            End If
            transformASCII85()
            For i As Integer = 0 To 5 - 1
                If (outdata(i) = 0) Then
                    Exit For
                End If
                out.write(outdata(i))
                lineBreak -= 1
                If (lineBreak = 0) Then
                    out.write(NEWLINE)
                    lineBreak = maxline
                End If
            Next
            count = 0
        End Sub

        '/**
        ' * This will flush the data to the stream.
        ' *
        ' * @throws IOException If there is an error writing the data to the stream.
        ' */
        Public Overrides Sub flush() 'throws IOException
            If (flushed) Then Return

            If (count > 0) Then
                For i As Integer = count To 4 - 1
                    indata(i) = 0
                Next
                transformASCII85()
                If (outdata(0) = Z) Then
                    For i As Integer = 0 To 5 - 1 'expand 'z',
                        outdata(i) = OFFSET
                    Next
                End If
                For i As Integer = 0 To count + 1 - 1
                    out.write(outdata(i))
                    lineBreak -= 1
                    If (lineBreak = 0) Then
                        out.write(NEWLINE)
                        lineBreak = maxline
                    End If
                Next
            End If
            lineBreak -= 1
            If (lineBreak = 0) Then
                out.write(NEWLINE)
            End If
            Out.Write(Convert.ToByte(terminator))
            out.write(NEWLINE)
            count = 0
            lineBreak = maxline
            flushed = True
            MyBase.Flush()
        End Sub

        '/**
        ' * This will close the stream.
        ' *
        ' * @throws IOException If there is an error closing the wrapped stream.
        ' */
        Public Overrides Sub close() 'throws IOException
            Try
                flush()
                MyBase.Close()
            Finally
                indata = Nothing
                outdata = Nothing
            End Try
        End Sub


    End Class

End Namespace

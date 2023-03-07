Imports FinSeA.Sistema
Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * This class is based on code from Apache Batik a subproject of Apache XMLGraphics. see
    ' * http://xmlgraphics.apache.org/batik/ for further details.
    ' */
    Public Class GlyfSimpleDescript
        Inherits GlyfDescript

        Private endPtsOfContours() As Integer
        Private flags() As Byte
        Private xCoordinates() As Short
        Private yCoordinates() As Short
        Private pointCount As Integer

        '/**
        ' * Constructor.
        ' * 
        ' * @param numberOfContours number of contours
        ' * @param bais the stream to be read
        ' * @throws IOException is thrown if something went wrong
        ' */
        Public Sub New(ByVal numberOfContours As Short, ByVal bais As TTFDataStream)
            MyBase.New(numberOfContours, bais)

            '/*
            ' * https://developer.apple.com/fonts/TTRefMan/RM06/Chap6glyf.html
            ' * "If a glyph has zero contours, it need not have any glyph data." set the pointCount to zero to initialize
            ' * attributes and avoid nullpointer but maybe there shouldn't have GlyphDescript in the GlyphData?
            ' */
            If (numberOfContours = 0) Then
                pointCount = 0
                Return
            End If

            ' Simple glyph description
            endPtsOfContours = Array.CreateInstance(GetType(Integer), numberOfContours)
            endPtsOfContours = bais.readUnsignedShortArray(numberOfContours)

            ' The last end point index reveals the total number of points
            pointCount = endPtsOfContours(numberOfContours - 1) + 1

            flags = Array.CreateInstance(GetType(Byte), pointCount)
            xCoordinates = Array.CreateInstance(GetType(Short), pointCount)
            yCoordinates = Arrays.CreateInstance(Of Short)(pointCount)

            Dim instructionCount As Integer = bais.readUnsignedShort()
            readInstructions(bais, instructionCount)
            readFlags(pointCount, bais)
            readCoords(pointCount, bais)
        End Sub

  
        Public Overrides Function getEndPtOfContours(ByVal i As Integer) As Integer
            Return endPtsOfContours(i)
        End Function

        Public Overrides Function getFlags(ByVal i As Integer) As Byte
            Return flags(i)
        End Function

        Public Overrides Function getXCoordinate(ByVal i As Integer) As Short
            Return xCoordinates(i)
        End Function

        Public Overrides Function getYCoordinate(ByVal i As Integer) As Short
            Return yCoordinates(i)
        End Function

        Public Overrides Function isComposite() As Boolean
            Return False
        End Function

        Public Overrides Function getPointCount() As Integer
            Return pointCount
        End Function

        '/**
        ' * The table is stored as relative values, but we'll store them as absolutes.
        ' */
        Private Sub readCoords(ByVal count As Integer, ByVal bais As TTFDataStream)
            Dim x As Short = 0
            Dim y As Short = 0
            For i As Integer = 0 To count - 1
                If ((flags(i) And X_DUAL) = X_DUAL) Then '!= 0
                    If ((flags(i) And X_SHORT_VECTOR) = X_SHORT_VECTOR) Then '!= 0
                        x += bais.readUnsignedByte()
                    End If
                Else
                    If ((flags(i) And X_SHORT_VECTOR) = X_SHORT_VECTOR) Then '!= 0
                        x += -(bais.readUnsignedByte())
                    Else
                        x += bais.readSignedShort()
                    End If
                End If
                xCoordinates(i) = x
            Next

            For i As Integer = 0 To count - 1
                If ((flags(i) And Y_DUAL) = Y_DUAL) Then
                    If ((flags(i) And Y_SHORT_VECTOR) = Y_SHORT_VECTOR) Then
                        y += bais.readUnsignedByte()
                    End If
                Else
                    If ((flags(i) And Y_SHORT_VECTOR) = Y_SHORT_VECTOR) Then
                        y += -(bais.readUnsignedByte())
                    Else
                        y += bais.readSignedShort()
                    End If
                End If
                yCoordinates(i) = y
            Next
        End Sub

        '/**
        ' * The flags are run-length encoded.
        ' */
        Private Sub readFlags(ByVal flagCount As Integer, ByVal bais As TTFDataStream)
            Try
                For index As Integer = 0 To flagCount - 1
                    flags(index) = bais.readUnsignedByte()
                    If ((flags(index) And REPEAT) = REPEAT) Then
                        Dim repeats As Integer = bais.readUnsignedByte()
                        For i As Integer = 1 To repeats
                            flags(index + i) = flags(index)
                        Next
                        index += repeats
                    End If
                Next
            Catch e As ArgumentOutOfRangeException
                LOG.error("error: array index out of bounds")
            End Try
        End Sub


    End Class

End Namespace
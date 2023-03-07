Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class TTFTable

        Private tag As String
        Private checkSum As Long
        Private offset As Long
        Private length As Long

        '/**
        ' * @return Returns the checkSum.
        ' */
        Public Function getCheckSum() As Long
            Return checkSum
        End Function

        '/**
        ' * @param checkSumValue The checkSum to set.
        ' */
        Public Sub setCheckSum(ByVal checkSumValue As Long)
            Me.checkSum = checkSumValue
        End Sub

        '/**
        ' * @return Returns the length.
        ' */
        Public Function getLength() As Long
            Return length
        End Function

        '/**
        ' * @param lengthValue The length to set.
        ' */
        Public Sub setLength(ByVal lengthValue As Long)
            Me.length = lengthValue
        End Sub

        '/**
        ' * @return Returns the offset.
        ' */
        Public Function getOffset() As Long
            Return offset
        End Function

        '/**
        ' * @param offsetValue The offset to set.
        ' */
        Public Sub setOffset(ByVal offsetValue As Long)
            Me.offset = offsetValue
        End Sub

        '/**
        ' * @return Returns the tag.
        ' */
        Public Function getTag() As String
            Return tag
        End Function

        '/**
        ' * @param tagValue The tag to set.
        ' */
        Public Sub setTag(ByVal tagValue As String)
            Me.tag = tagValue
        End Sub

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overridable Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
        End Sub

    End Class


End Namespace
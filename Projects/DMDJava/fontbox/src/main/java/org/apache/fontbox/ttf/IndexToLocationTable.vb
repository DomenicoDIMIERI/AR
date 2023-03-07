Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class IndexToLocationTable
        Inherits TTFTable

        Private SHORT_OFFSETS As Short = 0
        Private LONG_OFFSETS As Short = 1

        ''' <summary>
        ''' A tag that identifies Me table type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "loca"

        Private offsets() As Long

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim head As HeaderTable = ttf.getHeader()
            Dim maxp As MaximumProfileTable = ttf.getMaximumProfile()
            Dim numGlyphs As Integer = maxp.getNumGlyphs()
            offsets = Array.CreateInstance(GetType(Long), numGlyphs + 1)
            For i As Integer = 0 To numGlyphs + 1 - 1
                If (head.getIndexToLocFormat() = SHORT_OFFSETS) Then
                    offsets(i) = data.readUnsignedShort() * 2
                ElseIf (head.getIndexToLocFormat() = LONG_OFFSETS) Then
                    offsets(i) = data.readUnsignedInt()
                Else
                    Throw New IOException("Error:TTF.loca unknown offset format.")
                End If
            Next
        End Sub

        '/**
        ' * @return Returns the offsets.
        ' */
        Public Function getOffsets() As Long()
            Return offsets
        End Function

        '/**
        ' * @param offsetsValue The offsets to set.
        ' */
        Public Sub setOffsets(ByVal offsetsValue() As Long)
            Me.offsets = offsetsValue
        End Sub

    End Class

End Namespace

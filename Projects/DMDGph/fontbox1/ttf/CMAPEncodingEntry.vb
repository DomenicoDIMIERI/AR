Imports System.IO

Namespace org.fontbox.ttf

    '/**
    ' * An encoding entry for a cmap.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class CMAPEncodingEntry

        Private platformId As Integer
        Private platformEncodingId As Integer
        Private subTableOffset As Long
        Private glyphIdToCharacterCode As Integer()

        '/**
        '    * This will read the required data from the stream.
        '    * 
        '    * @param ttf The font that is being read.
        '    * @param data The stream to read the data from.
        '    * @throws IOException If there is an error reading the data.
        '    */
        Public Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            platformId = data.readUnsignedShort()
            platformEncodingId = data.readUnsignedShort()
            subTableOffset = data.readUnsignedInt()
        End Sub

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Sub initSubtable(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            data.seek(ttf.getCMAP().getOffset() + subTableOffset)
            Dim subtableFormat As Integer = data.readUnsignedShort()
            Dim length As Integer = data.readUnsignedShort()
            Dim version As Integer = data.readUnsignedShort()
            Dim numGlyphs As Integer = ttf.getMaximumProfile().getNumGlyphs()
            If (subtableFormat = 0) Then
                Dim glyphMapping() As Byte = data.read(256)
                glyphIdToCharacterCode = Array.CreateInstance(GetType(Integer), 256)
                For i As Integer = 0 To glyphMapping.Length - 1
                    glyphIdToCharacterCode(i) = (glyphMapping(i) + 256) Mod 256
                Next
            ElseIf (subtableFormat = 2) Then
                Dim subHeaderKeys() As Integer = Array.CreateInstance(GetType(Integer), 256)
                For i As Integer = 0 To 256 - 1
                    subHeaderKeys(i) = data.readUnsignedShort()
                Next
                Dim firstCode As Integer = data.readUnsignedShort()
                Dim entryCount As Integer = data.readUnsignedShort()
                Dim idDelta As Short = data.readSignedShort()
                Dim idRangeOffset As Integer = data.readUnsignedShort()
                'BJL
                'HMM the TTF spec is not very clear about what is suppose to
                'happen here.  If you know please submit a patch or point
                'me to some better documentation.
                Throw New IOException("Not yet implemented:" & subtableFormat.ToString)
            ElseIf (subtableFormat = 4) Then
                Dim segCountX2 As Integer = data.readUnsignedShort()
                Dim segCount As Integer = segCountX2 / 2
                Dim searchRange As Integer = data.readUnsignedShort()
                Dim entrySelector As Integer = data.readUnsignedShort()
                Dim rangeShift As Integer = data.readUnsignedShort()
                Dim endCount() As Integer = data.readUnsignedShortArray(segCount)
                Dim reservedPad As Integer = data.readUnsignedShort()
                Dim startCount() As Integer = data.readUnsignedShortArray(segCount)
                Dim idDelta() As Integer = data.readUnsignedShortArray(segCount)
                Dim idRangeOffset() As Integer = data.readUnsignedShortArray(segCount)

                'this is the final result
                'key=glyphId, value is character codes
                glyphIdToCharacterCode = Array.CreateInstance(GetType(Integer), numGlyphs)

                Dim currentPosition As Long = data.getCurrentPosition()

                For i As Integer = 0 To segCount - 1
                    Dim start As Integer = startCount(i)
                    Dim [end] As Integer = endCount(i)
                    Dim delta As Integer = idDelta(i)
                    Dim rangeOffset As Integer = idRangeOffset(i)
                    If (start <> 65535 AndAlso [end] <> 65535) Then
                        For j As Integer = start To [end]
                            If (rangeOffset = 0) Then
                                glyphIdToCharacterCode(((j + delta) Mod 65536)) = j
                            Else
                                Dim glyphOffset As Long = currentPosition + ((rangeOffset / 2) + (j - start) + (i - segCount)) * 2
                                data.seek(glyphOffset)
                                Dim glyphIndex As Integer = data.readUnsignedShort()
                                If (glyphIndex <> 0) Then
                                    glyphIndex += delta
                                    glyphIndex = glyphIndex Mod 65536
                                    If (glyphIdToCharacterCode(glyphIndex) = 0) Then
                                        glyphIdToCharacterCode(glyphIndex) = j
                                    End If
                                End If

                            End If
                        Next
                    End If
                Next
            ElseIf (subtableFormat = 6) Then
                Dim firstCode As Integer = data.readUnsignedShort()
                Dim entryCount As Integer = data.readUnsignedShort()
                glyphIdToCharacterCode = Array.CreateInstance(GetType(Integer), numGlyphs)
                Dim glyphIdArray() As Integer = data.readUnsignedShortArray(entryCount)
                For i As Integer = 0 To entryCount - 1
                    glyphIdToCharacterCode(glyphIdArray(i)) = firstCode + i
                Next
            Else
                Throw New IOException("Unknown cmap format:" & subtableFormat.ToString)
            End If
        End Sub


        '/**
        ' * @return Returns the glyphIdToCharacterCode.
        ' */
        Public Function getGlyphIdToCharacterCode() As Integer()
            Return glyphIdToCharacterCode
        End Function

        '/**
        ' * @param glyphIdToCharacterCodeValue The glyphIdToCharacterCode to set.
        ' */
        Public Sub setGlyphIdToCharacterCode(ByVal glyphIdToCharacterCodeValue() As Integer)
            Me.glyphIdToCharacterCode = glyphIdToCharacterCodeValue
        End Sub

        '/**
        ' * @return Returns the platformEncodingId.
        ' */
        Public Function getPlatformEncodingId() As Integer
            Return platformEncodingId
        End Function

        '/**
        ' * @param platformEncodingIdValue The platformEncodingId to set.
        ' */
        Public Sub setPlatformEncodingId(ByVal platformEncodingIdValue As Integer)
            Me.platformEncodingId = platformEncodingIdValue
        End Sub

        '/**
        ' * @return Returns the platformId.
        ' */
        Public Function getPlatformId() As Integer
            Return platformId
        End Function

        '/**
        '    * @param platformIdValue The platformId to set.
        '    */
        Public Sub setPlatformId(ByVal platformIdValue As Integer)
            Me.platformId = platformIdValue
        End Sub

    End Class

End Namespace
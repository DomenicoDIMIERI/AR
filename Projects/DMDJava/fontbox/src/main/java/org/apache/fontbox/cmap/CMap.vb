Imports System.IO

Namespace org.apache.fontbox.cmap

    '/**
    ' * This class represents a CMap file.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.3 $
    ' */
    Public Class CMap

        Private wmode As Integer = 0
        Private cmapName As String = vbNullString
        Private cmapVersion As String = vbNullString
        Private cmapType As Integer = -1

        Private registry As String = vbNullString
        Private ordering As String = vbNullString
        Private supplement As Integer = 0

        Private codeSpaceRanges As List(Of CodespaceRange) = New ArrayList(Of CodespaceRange)()
        Private singleByteMappings As Map(Of NInteger, String) = New HashMap(Of NInteger, String)()
        Private doubleByteMappings As Map(Of NInteger, String) = New HashMap(Of NInteger, String)()

        Private cid2charMappings As Map(Of NInteger, String) = New HashMap(Of NInteger, String)()
        Private char2CIDMappings As Map(Of String, NInteger) = New HashMap(Of String, NInteger)()
        Private cidRanges As List(Of CIDRange) = New LinkedList(Of CIDRange)()

        Private Const SPACE = " "
        Private spaceMapping As Integer = -1

        '/**
        ' * Creates a new instance of CMap.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * This will tell if Me cmap has any one byte mappings.
        ' * 
        ' * @return true If there are any one byte mappings, false otherwise.
        ' */
        Public Function hasOneByteMappings() As Boolean
            Return singleByteMappings.size() > 0
        End Function

        '/**
        ' * This will tell if Me cmap has any two byte mappings.
        ' * 
        ' * @return true If there are any two byte mappings, false otherwise.
        ' */
        Public Function hasTwoByteMappings() As Boolean
            Return doubleByteMappings.size() > 0
        End Function

        '/**
        ' * This will tell if Me cmap has any CID mappings.
        ' * 
        ' * @return true If there are any CID mappings, false otherwise.
        ' */
        Public Function hasCIDMappings() As Boolean
            Return Not char2CIDMappings.isEmpty() OrElse Not cidRanges.isEmpty()
        End Function

        '/**
        ' * This will perform a lookup into the map.
        ' *
        ' * @param code The code used to lookup.
        ' * @param offset The offset into the byte array.
        ' * @param length The length of the data we are getting.
        ' *
        ' * @return The string that matches the lookup.
        ' */
        Public Function lookup(ByVal code() As Byte, ByVal offset As Integer, ByVal length As Integer) As String
            Return lookup(getCodeFromArray(code, offset, length), length)
        End Function

        '/**
        ' * This will perform a lookup into the map.
        ' *
        ' * @param code The code used to lookup.
        ' * @param length The length of the data we are getting.
        ' *
        ' * @return The string that matches the lookup.
        ' */
        Public Function lookup(ByVal code As Integer, ByVal length As Integer) As String
            Dim result As String = vbNullString
            If (length = 1) Then
                result = singleByteMappings.get(code)
            ElseIf (length = 2) Then
                result = doubleByteMappings.get(code)
            End If
            Return result
        End Function

        '/**
        ' * This will perform a lookup into the CID map.
        ' *
        ' * @param cid The CID used to lookup.
        ' *
        ' * @return The string that matches the lookup.
        ' */
        Public Function lookupCID(ByVal cid As Integer) As String
            If (cid2charMappings.containsKey(cid)) Then
                Return cid2charMappings.get(cid)
            Else
                For Each range As CIDRange In cidRanges
                    Dim ch As Integer = range.unmap(cid)
                    If (ch <> -1) Then
                        Return Convert.ToChar(ch) 'Character.toString((char) ch)
                    End If
                Next
                Return vbNullString
            End If
        End Function

        '/**
        ' * This will perform a lookup into the CID map.
        ' *
        ' * @param code The code used to lookup.
        ' * @param offset the offset into the array.
        ' * @param length the length of the subarray.
        ' *
        ' * @return The CID that matches the lookup.
        ' */
        Public Function lookupCID(ByVal code() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
            If (isInCodeSpaceRanges(code, offset, length)) Then
                Dim codeAsInt As Integer = getCodeFromArray(code, offset, length)
                If (char2CIDMappings.containsKey(codeAsInt)) Then
                    Return char2CIDMappings.get(codeAsInt)
                Else
                    For Each range As CIDRange In cidRanges
                        Dim ch As Integer = range.map(Convert.ToChar(codeAsInt))
                        If (ch <> -1) Then
                            Return ch
                        End If
                    Next
                    Return -1
                End If
            End If
            Return -1
        End Function

        '/**
        ' * Convert the given part of a byte array to an integer.
        ' * @param data the byte array
        ' * @param offset The offset into the byte array.
        ' * @param length The length of the data we are getting.
        ' * @return the resulting integer
        ' */
        Private Function getCodeFromArray(ByVal data() As Byte, ByVal offset As Integer, ByVal length As Integer) As Integer
            Dim code As Integer = 0
            For i As Integer = 0 To length - 1
                code <<= 8
                code = code Or (data(offset + i) + 256) Mod 256
            Next
            Return code
        End Function

        '/**
        ' * This will add a mapping.
        ' *
        ' * @param src The src to the mapping.
        ' * @param dest The dest to the mapping.
        ' *
        ' * @throws IOException if the src is invalid.
        ' */
        Public Sub addMapping(ByVal src() As Byte, ByVal dest As String)
            Dim srcLength As Integer = src.Length
            Dim intSrc As Integer = getCodeFromArray(src, 0, srcLength)
            If (SPACE.Equals(dest)) Then
                spaceMapping = intSrc
            End If
            If (srcLength = 1) Then
                singleByteMappings.put(intSrc, dest)
            ElseIf (srcLength = 2) Then
                doubleByteMappings.put(intSrc, dest)
            Else
                Throw New IOException("Mapping code should be 1 or two bytes and not " & src.Length)
            End If
        End Sub

        '/**
        ' * This will add a CID mapping.
        ' *
        ' * @param src The CID to the mapping.
        ' * @param dest The dest to the mapping.
        ' *
        ' * @throws IOException if the src is invalid.
        ' */
        Public Sub addCIDMapping(ByVal src As Integer, ByVal dest As String)
            cid2charMappings.put(src, dest)
            char2CIDMappings.put(dest, src)
        End Sub

        '/**
        ' * This will add a CID Range.
        ' *
        ' * @param from starting charactor of the CID range.
        ' * @param to ending character of the CID range.
        ' * @param cid the cid to be started with.
        ' *
        ' */
        Public Sub addCIDRange(ByVal from As Char, ByVal [to] As Char, ByVal cid As Integer)
            cidRanges.add(0, New CIDRange(from, [to], cid))
        End Sub

        '/**
        ' * This will add a codespace range.
        ' *
        ' * @param range A single codespace range.
        ' */
        Public Sub addCodespaceRange(ByVal range As CodespaceRange)
            codeSpaceRanges.add(range)
        End Sub

        '/**
        ' * Getter for property codeSpaceRanges.
        ' *
        ' * @return Value of property codeSpaceRanges.
        ' */
        Public Function getCodeSpaceRanges() As List(Of CodespaceRange)
            Return codeSpaceRanges
        End Function

        '/**
        ' * Implementation of the usecmap operator.  This will
        ' * copy all of the mappings from one cmap to another.
        ' * 
        ' * @param cmap The cmap to load mappings from.
        ' */
        Public Sub useCmap(ByVal cmap As CMap)
            Me.codeSpaceRanges.addAll(cmap.codeSpaceRanges)
            Me.singleByteMappings.putAll(cmap.singleByteMappings)
            Me.doubleByteMappings.putAll(cmap.doubleByteMappings)
        End Sub

        '/**
        ' *  Check whether the given byte array is in codespace ranges or not.
        ' *  
        ' *  @param code The byte array to look for in the codespace range.
        ' *  
        ' *  @return true if the given byte array is in the codespace range.
        ' */
        Public Function isInCodeSpaceRanges(ByVal code() As Byte) As Boolean
            Return isInCodeSpaceRanges(code, 0, code.Length)
        End Function

        '/**
        ' *  Check whether the given byte array is in codespace ranges or not.
        ' *  
        ' *  @param code The byte array to look for in the codespace range.
        ' *  @param offset The starting offset within the byte array.
        ' *  @param length The length of the part of the array.
        ' *  
        ' *  @return true if the given byte array is in the codespace range.
        ' */
        Public Function isInCodeSpaceRanges(ByVal code() As Byte, ByVal offset As Integer, ByVal length As Integer) As Boolean
            Dim it As Iterator(Of CodespaceRange) = codeSpaceRanges.iterator()
            While (it.hasNext())
                Dim range As CodespaceRange = it.next()
                If (range IsNot Nothing AndAlso range.isInRange(code, offset, length)) Then
                    Return True
                End If
            End While
            Return False
        End Function

        '/**
        ' * Returns the WMode of a CMap.
        ' *
        ' * 0 represents a horizontal and 1 represents a vertical orientation.
        ' * 
        ' * @return the wmode
        ' */
        Public Function getWMode() As Integer
            Return wmode
        End Function

        '/**
        ' * Sets the WMode of a CMap.
        ' * 
        ' * @param newWMode the new WMode.
        ' */
        Public Sub setWMode(ByVal newWMode As Integer)
            wmode = newWMode
        End Sub

        '/**
        ' * Returns the name of the CMap.
        ' * 
        ' * @return the CMap name.
        ' */
        Public Function getName() As String
            Return cmapName
        End Function

        '/**
        ' * Sets the name of the CMap.
        ' * 
        ' * @param name the CMap name.
        ' */
        Public Sub setName(ByVal name As String)
            cmapName = name
        End Sub

        '/**
        ' * Returns the version of the CMap.
        ' * 
        ' * @return the CMap version.
        ' */
        Public Function getVersion() As String
            Return cmapVersion
        End Function

        '/**
        ' * Sets the version of the CMap.
        ' * 
        ' * @param version the CMap version.
        ' */
        Public Sub setVersion(ByVal version As String)
            cmapVersion = version
        End Sub

        '/**
        ' * Returns the type of the CMap.
        ' * 
        ' * @return the CMap type.
        ' */
        Public Shadows Function [getType]() As Integer
            Return cmapType
        End Function

        '/**
        ' * Sets the type of the CMap.
        ' * 
        ' * @param type the CMap type.
        ' */
        Public Sub setType(ByVal type As Integer)
            cmapType = type
        End Sub

        '/**
        ' * Returns the registry of the CIDSystemInfo.
        ' * 
        ' * @return the registry.
        ' */
        Public Function getRegistry() As String
            Return registry
        End Function

        '/**
        ' * Sets the registry of the CIDSystemInfo.
        ' * 
        ' * @param newRegistry the registry.
        ' */
        Public Sub setRegistry(ByVal newRegistry As String)
            registry = newRegistry
        End Sub

        '/**
        ' * Returns the ordering of the CIDSystemInfo.
        ' * 
        ' * @return the ordering.
        ' */
        Public Function getOrdering() As String
            Return ordering
        End Function

        '/**
        ' * Sets the ordering of the CIDSystemInfo.
        ' * 
        ' * @param newOrdering the ordering.
        ' */
        Public Sub setOrdering(ByVal newOrdering As String)
            ordering = newOrdering
        End Sub

        '/**
        ' * Returns the supplement of the CIDSystemInfo.
        ' * 
        ' * @return the supplement.
        ' */
        Public Function getSupplement() As Integer
            Return supplement
        End Function

        '/**
        ' * Sets the supplement of the CIDSystemInfo.
        ' * 
        ' * @param newSupplement the supplement.
        ' */
        Public Sub setSupplement(ByVal newSupplement As Integer)
            supplement = newSupplement
        End Sub

        '/** 
        ' * Returns the mapping for the space character.
        ' * 
        ' * @return the mapped code for the space character
        ' */
        Public Function getSpaceMapping() As Integer
            Return spaceMapping
        End Function

    End Class

End Namespace
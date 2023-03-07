Imports System.IO

Namespace org.fontbox.cmap

    '/**
    ' * This class represents a CMap file.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class CMap

        Private codeSpaceRanges As FinSeA.List = New FinSeA.ArrayList()
        Private singleByteMappings As FinSeA.Map = New FinSeA.HashMap()
        Private doubleByteMappings As FinSeA.Map = New FinSeA.HashMap()

        '/**
        ' * Creates a new instance of CMap.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * This will tell if this cmap has any one byte mappings.
        ' * 
        ' * @return true If there are any one byte mappings, false otherwise.
        ' */
        Public Function hasOneByteMappings() As Boolean
            Return singleByteMappings.size() > 0
        End Function

        '/**
        ' * This will tell if this cmap has any two byte mappings.
        ' * 
        ' * @return true If there are any two byte mappings, false otherwise.
        ' */
        Public Function hasTwoByteMappings() As Boolean
            Return doubleByteMappings.size() > 0
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
            Dim result As String = vbNullString
            Dim key As NInteger = Nothing
            If (length = 1) Then
        
                key = (code(offset) + 256) Mod 256
                result = singleByteMappings.get(key)
            ElseIf (length = 2) Then
                Dim intKey As Integer = (code(offset) + 256) Mod 256
                intKey <<= 8
                intKey += (code(offset + 1) + 256) Mod 256
                key = intKey

                result = doubleByteMappings.get(key)
            End If

            Return result
        End Function

        '/**
        ' * This will add a mapping.
        ' *
        ' * @param src The src to the mapping.
        ' * @param dest The dest to the mapping.
        ' *
        ' * @throws IOException if the src is invalid.
        ' */
        Public Sub addMapping(ByVal src() As Byte, ByVal dest As String)  'throws IOException
            If (src.Length = 1) Then
                singleByteMappings.put(New NInteger(src(0)), dest)
            ElseIf (src.Length = 2) Then
                Dim intSrc As Integer = src(0) And &HFF
                intSrc <<= 8
                intSrc = intSrc Or (src(1) And &HFF)
                doubleByteMappings.put(New NInteger(intSrc), dest)
            Else
                Throw New IOException("Mapping code should be 1 or two bytes and not " & src.Length)
            End If
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
        Public Function getCodeSpaceRanges() As FinSeA.List
            Return codeSpaceRanges
        End Function

    End Class

End Namespace
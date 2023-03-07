Imports System.IO

Namespace org.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class NamingTable
        Inherits TTFTable

        ''A tag that identifies this table type.
        Public Const TAG = "name"

        Private nameRecords As FinSeA.List = New FinSeA.ArrayList()

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim formatSelector As Integer = data.readUnsignedShort()
            Dim numberOfNameRecords As Integer = data.readUnsignedShort()
            Dim offsetToStartOfStringStorage As Integer = data.readUnsignedShort()
            For i As Integer = 0 To numberOfNameRecords - 1
                Dim nr As NameRecord = New NameRecord()
                nr.initData(ttf, data)
                nameRecords.add(nr)
            Next
            For i As Integer = 0 To numberOfNameRecords - 1
                Dim nr As NameRecord = nameRecords.get(i)
                data.seek(getOffset() + (2 * 3) + numberOfNameRecords * 2 * 6 + nr.getStringOffset())
                Dim platform As Integer = nr.getPlatformId()
                Dim encoding As Integer = nr.getPlatformEncodingId()
                Dim charset As String = "ISO-8859-1"
                If (platform = 3 AndAlso encoding = 1) Then
                    charset = "UTF-16"
                ElseIf (platform = 2) Then
                    If (encoding = 0) Then
                        charset = "US-ASCII"
                    ElseIf (encoding = 1) Then
                        'not sure is this is correct??
                        charset = "ISO-10646-1"
                    ElseIf (encoding = 2) Then
                        charset = "ISO-8859-1"
                    End If
                End If
                Dim [string] As String = data.readString(nr.getStringLength(), charset)
                nr.setString([string])
            Next
        End Sub

        '/**
        ' * This will get the name records for this naming table.
        ' * 
        ' * @return A list of NameRecord objects.
        ' */
        Public Function getNameRecords() As FinSeA.List
            Return nameRecords
        End Function

    End Class

End Namespace

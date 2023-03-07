Namespace org.apache.fontbox.cff.charset

    '/**
    ' * This is the superclass for all CFFFont charsets.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public MustInherit Class CFFCharset

        Private entries As List(Of Entry) = New ArrayList(Of Entry)()

        '/**
        ' * Determines if the charset is font specific or not.
        ' * @return if the charset is font specific
        ' */
        Public Overridable Function isFontSpecific() As Boolean
            Return False
        End Function

        '/**
        ' * Returns the SID corresponding to the given name.
        ' * @param name the given SID
        ' * @return the corresponding SID
        ' */
        Public Function getSID(ByVal name As String) As Integer
            For Each entry As Entry In Me.entries
                If ((entry.entryName).equals(name)) Then
                    Return entry.entrySID
                End If
            Next
            Return -1
        End Function

        '/**
        ' * Returns the name corresponding to the given SID.
        ' * @param sid the given SID
        ' * @return the corresponding name
        ' */
        Public Function getName(ByVal sid As Integer) As String
            For Each entry As Entry In Me.entries
                If (entry.entrySID = sid) Then
                    Return entry.entryName
                End If
            Next
            Return vbNullString
        End Function

        '/**
        ' * Adds a new SID/name combination to the charset.
        ' * @param sid the given SID
        ' * @param name the given name
        ' */
        Public Sub register(ByVal sid As Integer, ByVal name As String)
            entries.add(New Entry(sid, name))
        End Sub

        '/**
        ' * Add a single entry.
        ' * @param entry the entry to be added
        ' */
        Public Sub addEntry(ByVal entry As Entry)
            entries.add(entry)
        End Sub

        '/**
        ' * A list of all entries within Me charset.
        ' * @return a list of all entries
        ' */
        Public Function getEntries() As List(Of Entry)
            Return entries
        End Function

        '/**
        ' * This class represents a single SID/name mapping of the charset.
        ' *
        ' */
        Public NotInheritable Class Entry
            Friend entrySID As Integer
            Friend entryName As String

            '/**
            ' * Create a new instance of Entry with the given values.
            ' * @param sid the SID
            ' * @param name the Name
            ' */
            Protected Friend Sub New(ByVal sid As Integer, ByVal name As String)
                Me.entrySID = sid
                Me.entryName = name
            End Sub

            '/**
            ' * The SID of Me entry.
            ' * @return the SID
            ' */
            Public Function getSID() As Integer
                Return entrySID
            End Function

            '/**
            ' * The Name of Me entry.
            ' * @return the name
            ' */
            Public Function getName() As String
                Return entryName
            End Function

            Public Overrides Function toString() As String
                Return "[sid=" & entrySID & ", name=" & entryName & "]"
            End Function

        End Class


    End Class

End Namespace
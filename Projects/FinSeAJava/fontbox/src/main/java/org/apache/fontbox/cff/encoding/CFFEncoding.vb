Namespace org.apache.fontbox.cff.encoding

    '/**
    ' * This is the superclass for all CFFFont encodings.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public MustInherit Class CFFEncoding

        Private entries As List(Of Entry) = New ArrayList(Of Entry)()

        '/**
        ' * Determines if the encoding is font specific or not.
        ' * @return if the encoding is font specific
        ' */
        Public Overridable Function isFontSpecific() As Boolean
            Return False
        End Function

        '/**
        ' * Returns the code corresponding to the given SID.
        ' * @param sid the given SID
        ' * @return the corresponding code
        ' */
        Public Function getCode(ByVal sid As Integer) As Integer
            For Each entry As Entry In entries
                If (entry.entrySID = sid) Then
                    Return entry.entryCode
                End If
            Next
            Return -1
        End Function

        '/**
        ' * Returns the SID corresponding to the given code.
        ' * @param code the given code
        ' * @return the corresponding SID
        ' */
        Public Function getSID(ByVal code As Integer) As Integer
            For Each entry As Entry In entries
                If (entry.entryCode = code) Then
                    Return entry.entrySID
                End If
            Next
            Return -1
        End Function

        '/**
        ' * Adds a new code/SID combination to the encoding.
        ' * @param code the given code
        ' * @param sid the given SID
        ' */
        Public Sub register(ByVal code As Integer, ByVal sid As Integer)
            entries.add(New Entry(code, sid))
        End Sub

        '/**
        ' * Add a single entry.
        ' * @param entry the entry to be added
        ' */
        Public Sub addEntry(ByVal entry As Entry)
            entries.add(entry)
        End Sub

        '/**
        ' * A list of all entries within Me encoding.
        ' * @return a list of all entries
        ' */
        Public Function getEntries() As List(Of Entry)
            Return entries
        End Function

        '/**
        ' * This class represents a single code/SID mapping of the encoding.
        ' *
        ' */
        Public Class Entry

            Friend entryCode As Integer
            Friend entrySID As Integer

            '/**
            ' * Create a new instance of Entry with the given values.
            ' * @param code the code
            ' * @param sid the SID
            ' */
            Protected Friend Sub New(ByVal code As Integer, ByVal sid As Integer)
                Me.entryCode = code
                Me.entrySID = sid
            End Sub

            '/**
            ' * The code of the entry.
            ' * @return the code
            ' */
            Public Function getCode() As Integer
                Return Me.entryCode
            End Function

            '/**
            ' * The SID of the entry.
            ' * @return the SID
            ' */
            Public Function getSID() As Integer
                Return Me.entrySID
            End Function

            Public Overrides Function toString() As String
                Return "[code=" & entryCode & ", sid=" & entrySID & "]"
            End Function

        End Class


    End Class

End Namespace
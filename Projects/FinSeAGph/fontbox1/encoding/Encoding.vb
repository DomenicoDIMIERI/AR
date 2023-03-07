Imports System.IO

Namespace org.fontbox.encoding

    '/**
    ' * This is an interface to a text encoder.
    ' *
    ' * @author Ben Litchfield
    ' * @version $Revision: 1.1 $
    ' */
    Public MustInherit Class Encoding

        ''' <summary>
        ''' This is a mapping from a character code to a character name.
        ''' </summary>
        ''' <remarks></remarks>
        Protected codeToName As FinSeA.Map = New FinSeA.HashMap()

        ''' <summary>
        ''' This is a mapping from a character name to a character code.
        ''' </summary>
        ''' <remarks></remarks>
        Protected nameToCode As FinSeA.Map = New FinSeA.HashMap()

        Private Shared NAME_TO_CHARACTER As FinSeA.Map = New FinSeA.HashMap()
        Private Shared CHARACTER_TO_NAME As FinSeA.Map = New HashMap()

        Shared Sub New()
        End Sub


        '/**
        ' * This will add a character encoding.
        ' *
        ' * @param code The character code that matches the character.
        ' * @param name The name of the character.
        ' */
        Protected Sub addCharacterEncoding(ByVal code As Integer, ByVal name As String)
            Dim intCode As NInteger = code
            codeToName.put(intCode, name)
            nameToCode.put(name, intCode)
        End Sub

        '/**
        ' * This will get the character code for the name.
        ' *
        ' * @param name The name of the character.
        ' *
        ' * @return The code for the character.
        ' *
        ' * @throws IOException If there is no character code for the name.
        ' */
        Public Function getCode(ByVal name As String) As Integer
            Dim code As NInteger = nameToCode.get(name)
            If (code.HasValue = False) Then
                Throw New IOException("No character code for character name '" & name & "'")
            End If
            Return code.Value
        End Function

        '/**
        ' * This will take a character code and get the name from the code.
        ' *
        ' * @param code The character code.
        ' *
        ' * @return The name of the character.
        ' *
        ' * @throws IOException If there is no name for the code.
        ' */
        Public Function getName(ByVal code As Integer) As String
            Dim name As String = codeToName.get(New NInteger(code))
            If (name = vbNullString) Then
                'lets be forgiving for now
                name = "space"
                '//throw new IOException( getClass().getName() +
                '//                       ": No name for character code '" + code + "'" );
            End If
            Return name
        End Function

        '/**
        ' * This will take a character code and get the name from the code.
        ' *
        ' * @param c The character.
        ' *
        ' * @return The name of the character.
        ' *
        ' * @throws IOException If there is no name for the character.
        ' */
        Public Function getNameFromCharacter(ByVal c As Char) As String
            Dim name As String = CHARACTER_TO_NAME.get("" & c)
            If (name = vbNullString) Then
                Throw New IOException("No name for character '" & c & "'")
            End If
            Return name
        End Function

        '/**
        ' * This will get the character from the code.
        ' *
        ' * @param code The character code.
        ' *
        ' * @return The printable character for the code.
        ' *
        ' * @throws IOException If there is not name for the character.
        ' */
        Public Function getCharacter(ByVal code As Integer) As String
            Dim character As String = getCharacter(getName(code))
            Return character
        End Function

        '/**
        ' * This will get the character from the name.
        ' *
        ' * @param name The name of the character.
        ' *
        ' * @return The printable character for the code.
        ' */
        Public Shared Function getCharacter(ByVal name As String) As String
            Dim character As String = NAME_TO_CHARACTER.get(name)
            If (character = vbNullString) Then
                character = name
            End If
            Return character
        End Function


    End Class

End Namespace
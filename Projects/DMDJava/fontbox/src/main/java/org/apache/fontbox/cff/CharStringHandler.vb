Namespace org.apache.fontbox.cff

    '/**
    ' * A Handler for CharStringCommands.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public MustInherit Class CharStringHandler


        '/**
        ' * Handler for a sequence of CharStringCommands.
        ' * 
        ' * @param sequence of CharStringCommands
        ' * 
        ' * @return may return a command sequence of a subroutine
        ' */
        '@SuppressWarnings(value = { "unchecked" })
        Public Overridable Function handleSequence(ByVal sequence As List(Of Object)) As List(Of NInteger)
            Dim numbers As List(Of NInteger) = Nothing
            Dim offset As Integer = 0
            Dim size As Integer = sequence.size()
            For i As Integer = 0 To size - 1
                Dim [object] As Object = sequence.get(i)
                If (TypeOf ([object]) Is CharStringCommand) Then
                    If (numbers Is Nothing) Then
                        numbers = sequence.subList(offset, i)
                    Else
                        numbers.addAll(sequence.subList(offset, i))
                    End If
                    Dim stack As List(Of NInteger) = handleCommand(numbers, DirectCast([object], CharStringCommand))
                    If (stack IsNot Nothing AndAlso Not stack.isEmpty()) Then
                        numbers = stack
                    Else
                        numbers = Nothing
                    End If
                    offset = i + 1
                End If
            Next
            If (numbers IsNot Nothing AndAlso Not numbers.isEmpty()) Then
                Return numbers
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * Handler for CharStringCommands.
        ' *  
        ' * @param numbers a list of numbers
        ' * @param command the CharStringCommand
        ' * 
        ' * @return may return a command sequence of a subroutine
        ' */
        Public MustOverride Function handleCommand(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand) As List(Of NInteger)


    End Class

End Namespace
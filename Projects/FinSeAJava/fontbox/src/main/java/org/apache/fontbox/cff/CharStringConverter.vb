Imports FinSeA.Sistema

Namespace org.apache.fontbox.cff

    '**
    ' * A class to translate Type2 CharString command sequence to Type1 CharString command sequence.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class CharStringConverter
        Inherits CharStringHandler

        Private defaultWidthX As Integer = 0
        Private nominalWidthX As Integer = 0
        Private sequence As List(Of Object) = Nothing
        Private pathCount As Integer = 0

        '/**
        ' * Constructor.
        ' * 
        ' * @param defaultWidth default width
        ' * @param nominalWidth nominal width
        ' * 
        ' * @deprecated Use {@link CharStringConverter#CharStringConverter(int, int)} instead
        ' */
        Public Sub New(ByVal defaultWidth As Integer, ByVal nominalWidth As Integer, ByVal fontGlobalSubrIndex As IndexData, ByVal fontLocalSubrIndex As IndexData)
            defaultWidthX = defaultWidth
            nominalWidthX = nominalWidth
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param defaultWidth default width
        ' * @param nominalWidth nominal width
        ' * 
        ' */
        Public Sub New(ByVal defaultWidth As Integer, ByVal nominalWidth As Integer)
            defaultWidthX = defaultWidth
            nominalWidthX = nominalWidth
        End Sub
        '/**
        ' * Converts a sequence of Type1/Type2 commands into a sequence of CharStringCommands.
        ' * @param commandSequence the type1/type2 sequence
        ' * @return the CHarStringCommandSequence
        ' */
        Public Function convert(ByVal commandSequence As List(Of Object)) As List(Of Object)
            sequence = New ArrayList(Of Object)()
            pathCount = 0
            handleSequence(commandSequence)
            Return sequence
        End Function

        Public Overrides Function handleCommand(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand) As List(Of NInteger)

            If (CharStringCommand.TYPE1_VOCABULARY.containsKey(command.getKey())) Then
                Return handleType1Command(numbers, command)
            Else
                Return handleType2Command(numbers, command)
            End If
        End Function

        Private Function handleType1Command(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand) As List(Of NInteger)
            Dim name As String = CharStringCommand.TYPE1_VOCABULARY.get(command.getKey())

            If ("hstem".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() Mod 2 <> 0)
                expandStemHints(numbers, True)
            ElseIf ("vstem".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() Mod 2 <> 0)
                expandStemHints(numbers, False)
            ElseIf ("vmoveto".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() > 1)
                markPath()
                addCommand(numbers, command)
            ElseIf ("rlineto".Equals(name)) Then
                addCommandList(split(numbers, 2), command)
            ElseIf ("hlineto".Equals(name)) Then
                drawAlternatingLine(numbers, True)
            ElseIf ("vlineto".Equals(name)) Then
                drawAlternatingLine(numbers, False)
            ElseIf ("rrcurveto".Equals(name)) Then
                addCommandList(split(numbers, 6), command)
            ElseIf ("endchar".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() > 0)
                closePath()
                addCommand(numbers, command)
            ElseIf ("rmoveto".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() > 2)
                markPath()
                addCommand(numbers, command)
            ElseIf ("hmoveto".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() > 1)
                markPath()
                addCommand(numbers, command)
            ElseIf ("vhcurveto".Equals(name)) Then
                drawAlternatingCurve(numbers, False)
            ElseIf ("hvcurveto".Equals(name)) Then
                drawAlternatingCurve(numbers, True)
            ElseIf ("return".Equals(name)) Then
                Return numbers
            Else
                addCommand(numbers, command)
            End If
            Return Nothing
        End Function

        ' @SuppressWarnings(value = { "unchecked" })
        Private Function handleType2Command(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand) As List(Of NInteger)
            Dim name As String = CharStringCommand.TYPE2_VOCABULARY.get(command.getKey())
            If ("hflex".Equals(name)) Then
                Dim first As List(Of NInteger) = New ArrayList(Of NInteger)({numbers.get(0), 0, numbers.get(1), numbers.get(2), numbers.get(3), 0})
                Dim second As List(Of NInteger) = New ArrayList(Of NInteger)({numbers.get(4), 0, numbers.get(5), -numbers.get(2).Value(), numbers.get(6), 0})
                addCommandList(New ArrayList(Of NInteger)({first, second}), New CharStringCommand(8))
            ElseIf ("flex".Equals(name)) Then
                Dim first As List(Of NInteger) = numbers.subList(0, 6)
                Dim second As List(Of NInteger) = numbers.subList(6, 12)
                addCommandList(Collections.asList(Of NInteger)(first, second), New CharStringCommand(8))
            ElseIf ("hflex1".Equals(name)) Then
                Dim first As List(Of NInteger) = Collections.asList(numbers.get(0), numbers.get(1), numbers.get(2), numbers.get(3), numbers.get(4), 0)
                Dim second As List(Of NInteger) = Collections.asList(numbers.get(5), 0, numbers.get(6), numbers.get(7), numbers.get(8), 0)
                addCommandList(Collections.asList(first, second), New CharStringCommand(8))
            ElseIf ("flex1".Equals(name)) Then
                Dim dx As Integer = 0
                Dim dy As Integer = 0
                For i As Integer = 0 To 5 - 1
                    dx += numbers.get(i * 2).Value
                    dy += numbers.get(i * 2 + 1).Value
                Next
                Dim first As List(Of NInteger) = numbers.subList(0, 6)
                Dim second As List(Of NInteger) = Collections.asList(Of NInteger)(numbers.get(6), numbers.get(7), numbers.get(8), numbers.get(9), IIf(Math.Abs(dx) > Math.Abs(dy), numbers.get(10), -dx), IIf(Math.Abs(dx) > Math.Abs(dy), -dy, numbers.get(10)))
                addCommandList(Collections.asList(first, second), New CharStringCommand(8))
            ElseIf ("hstemhm".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() Mod 2 <> 0)
                expandStemHints(numbers, True)
            ElseIf ("hintmask".Equals(name) OrElse "cntrmask".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() Mod 2 <> 0)
                If (numbers.size() > 0) Then
                    expandStemHints(numbers, False)
                End If
            ElseIf ("vstemhm".Equals(name)) Then
                numbers = clearStack(numbers, numbers.size() Mod 2 <> 0)
                expandStemHints(numbers, False)
            ElseIf ("rcurveline".Equals(name)) Then
                addCommandList(split(numbers.subList(0, numbers.size() - 2), 6), New CharStringCommand(8))
                addCommand(numbers.subList(numbers.size() - 2, numbers.size()), New CharStringCommand(5))
            ElseIf ("rlinecurve".Equals(name)) Then
                addCommandList(split(numbers.subList(0, numbers.size() - 6), 2), New CharStringCommand(5))
                addCommand(numbers.subList(numbers.size() - 6, numbers.size()), New CharStringCommand(8))
            ElseIf ("vvcurveto".Equals(name)) Then
                drawCurve(numbers, False)
            ElseIf ("hhcurveto".Equals(name)) Then
                drawCurve(numbers, True)
            Else
                addCommand(numbers, command)
            End If
            Return Nothing
        End Function

        Private Function clearStack(ByVal numbers As List(Of NInteger), ByVal flag As Boolean) As List(Of NInteger)
            If (sequence.size() = 0) Then
                If (flag) Then
                    addCommand(New ArrayList(Of NInteger)({0, numbers.get(0).Value + nominalWidthX}), New CharStringCommand(13))
                    numbers = numbers.subList(1, numbers.size())
                Else
                    addCommand(New ArrayList(Of NInteger)({0, defaultWidthX}), New CharStringCommand(13))
                End If
            End If

            Return numbers
        End Function

        Private Sub expandStemHints(ByVal numbers As List(Of NInteger), ByVal horizontal As Boolean)
            ' TODO
        End Sub

        Private Sub markPath()
            If (pathCount > 0) Then
                closePath()
            End If
            pathCount += 1
        End Sub

        Private Sub closePath()
            Dim command As CharStringCommand
            If (pathCount > 0) Then
                command = sequence.get(sequence.size() - 1)
            Else
                command = Nothing
            End If

            Dim closepathCommand As New CharStringCommand(9)
            If (command IsNot Nothing AndAlso Not closepathCommand.equals(command)) Then
                addCommand(New ArrayList(Of NInteger), closepathCommand)
            End If
        End Sub

        Private Sub drawAlternatingLine(ByVal numbers As List(Of NInteger), ByVal horizontal As Boolean)
            While (numbers.size() > 0)
                addCommand(numbers.subList(0, 1), New CharStringCommand(CInt(IIf(horizontal, 6, 7))))
                numbers = numbers.subList(1, numbers.size())
                horizontal = Not horizontal
            End While
        End Sub

        Private Sub drawAlternatingCurve(ByVal numbers As List(Of NInteger), ByVal horizontal As Boolean)
            While (numbers.size() > 0)
                Dim last As Boolean = numbers.size() = 5
                If (horizontal) Then
                    addCommand(New ArrayList(Of NInteger)({numbers.get(0), 0, numbers.get(1), numbers.get(2), IIf(last, numbers.get(4), 0), numbers.get(3)}), New CharStringCommand(8))
                Else
                    addCommand(New ArrayList(Of NInteger)({0, numbers.get(0), numbers.get(1), numbers.get(2), numbers.get(3), IIf(last, numbers.get(4), 0)}), New CharStringCommand(8))
                End If
                numbers = numbers.subList(IIf(last, 5, 4), numbers.size())
                horizontal = Not horizontal
            End While
        End Sub

        Private Sub drawCurve(ByVal numbers As List(Of NInteger), ByVal horizontal As Boolean)
            While (numbers.size() > 0)
                Dim first As Boolean = numbers.size() Mod 4 = 1

                If (horizontal) Then
                    addCommand(New ArrayList(Of NInteger)({numbers.get(IIf(first, 1, 0)), IIf(first, numbers.get(0), 0), numbers.get(IIf(first, 2, 1)), numbers.get(IIf(first, 3, 2)), numbers.get(IIf(first, 4, 3)), 0}), New CharStringCommand(8))
                Else
                    addCommand(New ArrayList(Of NInteger)({IIf(first, numbers.get(0), 0), numbers.get(IIf(first, 1, 0)), numbers.get(IIf(first, 2, 1)), numbers.get(IIf(first, 3, 2)), 0, numbers.get(IIf(first, 4, 3))}), New CharStringCommand(8))
                End If
                numbers = numbers.subList(IIf(first, 5, 4), numbers.size())
            End While
        End Sub

        Private Sub addCommandList(ByVal numbers As List(Of List(Of NInteger)), ByVal command As CharStringCommand)
            For i As Integer = 0 To numbers.size() - 1
                addCommand(numbers.get(i), command)
            Next
        End Sub

        Private Sub addCommand(ByVal numbers As List(Of NInteger), ByVal command As CharStringCommand)
            sequence.addAll(numbers)
            sequence.add(command)
        End Sub

        Private Shared Function split(Of E)(ByVal list As List(Of E), ByVal size As Integer) As List(Of List(Of E))
            Dim result As List(Of List(Of E)) = New ArrayList(Of List(Of E))
            For i As Integer = 0 To CInt(list.size() / size) - 1
                result.add(list.subList(i * size, (i + 1) * size))
            Next
            Return result
        End Function

    End Class

End Namespace
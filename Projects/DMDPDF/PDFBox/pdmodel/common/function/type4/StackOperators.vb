'import java.util.LinkedList;
'import java.util.List;
'import java.util.Stack;
Imports FinSeA.Io


Namespace org.apache.pdfbox.pdmodel.common.function.type4


    '/**
    ' * Provides the stack operators such as "pop" and "dup".
    ' *
    ' * @version $Revision$
    ' */
    Class StackOperators


        '/** Implements the "copy" operator. */
        Class Copy
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim n As Integer = CType(stack.Pop(), Number).intValue()
                If (n > 0) Then
                    Dim size As Integer = stack.size()
                    'Need to copy to a new list to avoid ConcurrentModificationException
                    Dim copy As List(Of Object) = New ArrayList(Of Object)(stack.subList(size - n, size))
                    For Each o As Object In copy
                        stack.Push(copy)
                    Next
                End If
            End Sub

        End Class

        '/** Implements the "dup" operator. */
        Class Dup
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                stack.Push(stack.Peek())
            End Sub

        End Class

        '/** Implements the "exch" operator. */
        Class Exch
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim any2 As Object = stack.Pop()
                Dim any1 As Object = stack.Pop()
                stack.Push(any2)
                stack.Push(any1)
            End Sub


        End Class

        '/** Implements the "index" operator. */
        Class Index
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim n As Integer = CType(stack.Pop(), Number).intValue()
                If (n < 0) Then
                    Throw New ArgumentOutOfRangeException("rangecheck: " & n)
                End If
                Dim size As Integer = stack.size()
                stack.Push(stack.get(size - n - 1))
            End Sub

        End Class

        '* Implements the "pop" operator. */
        Class Pop
            Inherits baseoperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                stack.pop()
            End Sub

        End Class

        '/** Implements the "roll" operator. */
        Class Roll
            Inherits BaseOperator

            Public Overrides Sub execute(ByVal context As ExecutionContext)
                Dim stack As Stack(Of Object) = context.getStack()
                Dim j As Integer = DirectCast(stack.Pop(), Number).intValue()
                Dim n As Integer = DirectCast(stack.Pop(), Number).intValue()
                If (j = 0) Then
                    Return 'Nothing to do
                End If
                If (n < 0) Then
                    Throw New ArgumentOutOfRangeException("rangecheck: " & n)
                End If

                Dim rolled As LinkedList(Of Object) = New LinkedList(Of Object)
                Dim moved As LinkedList(Of Object) = New LinkedList(Of Object)
                If (j < 0) Then
                    'negative roll
                    Dim n1 As Integer = n + j
                    For i As Integer = 0 To n1 - 1
                        moved.addFirst(stack.pop())
                    Next
                    For i As Integer = j To 0 - 1
                        rolled.AddFirst(stack.Pop())
                    Next
                    stack.addAll(moved)
                    stack.addAll(rolled)
                Else
                    'positive roll
                    Dim n1 As Integer = n - j
                    For i As Integer = j To 0 + 1 Step -1
                        rolled.AddFirst(stack.Pop())
                    Next
                    For i As Integer = 0 To n1 - 1
                        moved.AddFirst(stack.Pop())
                    Next
                    stack.addAll(rolled)
                    stack.addAll(moved)
            End If
            End Sub

        End Class

    End Class

End Namespace

Imports FinSeA.Exceptions

Namespace org.apache.pdfbox.pdmodel.common.function.type4


    '/**
    ' * Represents an instruction sequence, a combination of values, operands and nested procedures.
    ' *
    ' * @version $Revision$
    ' */
    Public Class InstructionSequence
        'Inherits BaseOperator

        Private instructions As List = New ArrayList

        '/**
        ' * Add a name (ex. an operator)
        ' * @param name the name
        ' */
        Public Sub addName(ByVal name As String)
            Me.instructions.add(name)
        End Sub

        '/**
        ' * Adds an int value.
        ' * @param value the value
        ' */
        Public Sub addInteger(ByVal value As Integer)
            Me.instructions.add(value)
        End Sub

        '/**
        ' * Adds a real value.
        ' * @param value the value
        ' */
        Public Sub addReal(ByVal value As Single)
            Me.instructions.add(value)
        End Sub

        '/**
        ' * Adds a bool value.
        ' * @param value the value
        ' */
        Public Sub addBoolean(ByVal value As Boolean)
            Me.instructions.add(value)
        End Sub

        '/**
        ' * Adds a proc (sub-sequence of instructions).
        ' * @param child the child proc
        ' */
        Public Sub addProc(ByVal child As InstructionSequence)
            Me.instructions.add(child)
        End Sub

        '/**
        ' * Executes the instruction sequence.
        ' * @param context the execution context
        ' */
        Public Sub execute(ByVal context As ExecutionContext)
            Dim stack As Stack(Of Object) = context.getStack()
            For Each o As Object In instructions
                If (TypeOf (o) Is String) Then
                    Dim name As String = o
                    Dim cmd As [Operator] = context.getOperators().getOperator(name)
                    If (cmd IsNot Nothing) Then
                        cmd.execute(context)
                    Else
                        Throw New UnsupportedOperationException("Unknown operator or name: " & name)
                    End If
                Else
                    stack.push(o)
                End If


                'Handles top-level procs that simply need to be executed
                While (Not (stack.size = 0) AndAlso TypeOf (stack.peek()) Is InstructionSequence)
                    Dim nested As InstructionSequence = stack.pop()
                    nested.execute(context)
                End While
            Next

        End Sub


    End Class

End Namespace
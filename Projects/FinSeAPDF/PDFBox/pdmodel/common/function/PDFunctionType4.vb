Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function.type4
Imports System.IO
Imports FinSeA.Exceptions

Namespace org.apache.pdfbox.pdmodel.common.function


    '/**
    ' * This class represents a type 4 function in a PDF document.
    ' * <p>
    ' * See section 3.9.4 of the PDF 1.4 Reference.
    ' *
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDFunctionType4
        Inherits PDFunction

        Private Shared ReadOnly OPERATORS As New Operators()

        Private ReadOnly instructions As InstructionSequence

        '/**
        ' * Constructor.
        ' *
        ' * @param functionStream The function stream.
        ' * @throws IOException if an I/O error occurs while reading the function
        ' */
        Public Sub New(ByVal functionStream As COSBase) 'throws IOException
            MyBase.New(functionStream)
            Me.instructions = InstructionSequenceBuilder.parse(getPDStream().getInputStreamAsString())
        End Sub


        Public Overrides Function getFunctionType() As Integer
            Return 4
        End Function

        Public Overrides Function eval(ByVal input() As Single) As Single() ' throws IOException
            'Setup the input values
            Dim numberOfInputValues As Integer = input.Length
            Dim context As New ExecutionContext(OPERATORS)
            For i As Integer = numberOfInputValues - 1 To 0 Step -1
                Dim domain As PDRange = getDomainForInput(i)
                Dim value As Single = clipToRange(input(i), domain.getMin(), domain.getMax())
                context.getStack().push(value)
            Next

            'Execute the type 4 function.
            instructions.execute(context)

            'Extract the output values
            Dim numberOfOutputValues As Integer = getNumberOfOutputParameters()
            Dim numberOfActualOutputValues As Integer = context.getStack().size()
            If (numberOfActualOutputValues < numberOfOutputValues) Then
                Throw New IllegalStateException("The type 4 function returned " & numberOfActualOutputValues & " values but the Range entry indicates that " & numberOfOutputValues & " values be returned.")
            End If
            Dim outputValues() As Single = Array.CreateInstance(GetType(Single), numberOfOutputValues)
            For i As Integer = numberOfOutputValues - 1 To 0 Step -1
                Dim range As PDRange = getRangeForOutput(i)
                outputValues(i) = context.popReal()
                outputValues(i) = clipToRange(outputValues(i), range.getMin(), range.getMax())
            Next

            'Return the resulting array
            Return outputValues
        End Function

    End Class

End Namespace
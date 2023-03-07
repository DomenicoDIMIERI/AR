'imports java.util.concurrent.ConcurrentHashMap;

Namespace org.apache.pdfbox.util


    '/**
    ' * This class represents an Operator in the content stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.14 $
    ' */
    Public Class PDFOperator

        Private theOperator As String
        Private imageData() As Byte
        Private imageParameters As ImageParameters

        '/** map for singleton operator objects; use {@link ConcurrentHashMap} for better scalability with multiple threads */
        Private Shared operators As ConcurrentHashMap(Of String, PDFOperator) = New ConcurrentHashMap(Of String, PDFOperator)

        '/**
        ' * Constructor.
        ' *
        ' * @param aOperator The operator that this object will represent.
        ' */
        Private Sub New(ByVal aOperator As String)
            theOperator = aOperator
            If (aOperator.startsWith("/")) Then
                Throw New RuntimeException("Operators are not allowed to start with / '" & aOperator & "'")
            End If
        End Sub

        '/**
        ' * This is used to create/cache operators in the system.
        ' *
        ' * @param operator The operator for the system.
        ' *
        ' * @return The operator that matches the operator keyword.
        ' */
        Public Shared Function getOperator(ByVal [operator] As String) As PDFOperator
            Dim operation As PDFOperator = Nothing
            If ([operator].Equals("ID") OrElse [operator].Equals("BI")) Then
                'we can't cache the ID operators.
                operation = New PDFOperator([operator])
            Else
                operation = operators.get([operator])
                If (operation Is Nothing) Then
                    ' another thread may has already added an operator of this kind
                    ' make sure that we get the same operator
                    operation = operators.putIfAbsent([operator], New PDFOperator([operator]))
                    If (operation Is Nothing) Then
                        operation = operators.get([operator])
                    End If
                End If
            End If
            Return operation
        End Function

        '/**
        ' * This will get the operation that this operator represents.
        ' *
        ' * @return The string representation of the operation.
        ' */
        Public Function getOperation() As String
            Return theOperator
        End Function

        '/**
        ' * This will print a string rep of this class.
        ' *
        ' * @return A string rep of this class.
        ' */
        Public Overrides Function toString() As String
            Return "PDFOperator{" & theOperator & "}"
        End Function

        '/**
        ' * This is the special case for the ID operator where there are just random
        ' * bytes inlined the stream.
        ' *
        ' * @return Value of property imageData.
        ' */
        Public Function getImageData() As Byte()
            Return Me.imageData
        End Function

        '/**
        ' * This will set the image data, this is only used for the ID operator.
        ' *
        ' * @param imageDataArray New value of property imageData.
        ' */
        Public Sub setImageData(ByVal imageDataArray() As Byte)
            imageData = imageDataArray
        End Sub

        '/**
        ' * This will get the image parameters, this is only valid for BI operators.
        ' *
        ' * @return The image parameters.
        ' */
        Public Function getImageParameters() As ImageParameters
            Return imageParameters
        End Function

        '/**
        ' * This will set the image parameters, this is only valid for BI operators.
        ' *
        ' * @param params The image parameters.
        ' */
        Public Sub setImageParameters(ByVal params As ImageParameters)
            imageParameters = params
        End Sub

    End Class

End Namespace

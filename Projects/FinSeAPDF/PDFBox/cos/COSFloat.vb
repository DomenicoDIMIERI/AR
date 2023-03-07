Imports System.IO
Imports FinSeA.org.apache.pdfbox.exceptions

Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents a floating point number in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class COSFloat
        Inherits COSNumber

        Private value As Decimal
        Private valueAsString As String

        '/**
        ' * Constructor.
        ' *
        ' * @param aFloat The primitive Single object that this object wraps.
        ' */
        Public Sub New(ByVal aFloat As Single)
            setValue(aFloat)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param aFloat The primitive Single object that this object wraps.
        ' *
        ' * @throws IOException If aFloat is not a Single.
        ' */
        Public Sub New(ByVal aFloat As String) 'throws IOException
            Try
                valueAsString = aFloat
                value = Decimal.Parse(valueAsString)
            Catch e As FormatException
                Throw New FormatException("Error expected floating point number actual='" & aFloat & "'", e)
            End Try
        End Sub

        '/**
        ' * Set the value of the Single object.
        ' *
        ' * @param floatValue The new Single value.
        ' */
        Public Sub setValue(ByVal floatValue As Single)
            ' use a BigDecimal as intermediate state to avoid 
            ' a floating point string representation of the Single value
            value = New Decimal(Val(floatValue))
            valueAsString = removeNullDigits(value.ToString())
        End Sub

        Private Function removeNullDigits(ByVal value As String) As String
            ' remove fraction digit "0" only
            If (value.IndexOf(".") > -1 AndAlso Not value.EndsWith(".0")) Then
                While (value.EndsWith("0") AndAlso Not value.EndsWith(".0"))
                    value = value.Substring(0, value.Length() - 1)
                End While
            End If
            Return value
        End Function

        '/**
        ' * The value of the Single object that this one wraps.
        ' *
        ' * @return The value of this object.
        ' */
        Public Overrides Function floatValue() As Single
            Return CSng(value)
        End Function

        '/**
        ' * The value of the double object that this one wraps.
        ' *
        ' * @return The double of this object.
        ' */
        Public Overrides Function doubleValue() As Double
            Return CDbl(value)
        End Function

        '/**
        ' * This will get the long value of this object.
        ' *
        ' * @return The long value of this object,
        ' */
        Public Overrides Function longValue() As Long
            Return CLng(value)
        End Function

        '/**
        ' * This will get the integer value of this object.
        ' *
        ' * @return The int value of this object,
        ' */
        Public Overrides Function intValue() As Integer
            Return CInt(value)
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function equals(ByVal o As Object) As Boolean
            Return (TypeOf (o) Is COSFloat) AndAlso (CType(o, COSFloat).value = value)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.value.GetHashCode()
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function toString() As String
            Return "COSFloat{" & valueAsString & "}"
        End Function

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return visitor.visitFromFloat(Me)
        End Function

        '/**
        ' * This will output this string as a PDF object.
        ' *
        ' * @param output The stream to write to.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub writePDF(ByVal output As Stream) 'throws IOException
            Dim buffer() As Byte = System.Text.Encoding.UTF7.GetBytes(Me.valueAsString)
            output.write(buffer, 0, UBound(buffer)) 'valueAsString.getBytes("ISO-8859-1"))
        End Sub

    End Class

End Namespace
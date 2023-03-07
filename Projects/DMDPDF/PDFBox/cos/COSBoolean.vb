Imports System.IO

Imports FinSeA.org.apache.pdfbox.exceptions

Namespace org.apache.pdfbox.cos


    '/**
    ' * This class represents a boolean value in the PDF document.
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.14 $
    ' */
    Public Class COSBoolean
        Inherits COSBase

        ''' <summary>
        ''' The true boolean token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TRUE_BYTES() As Byte = {116, 114, 117, 101} '"true".getBytes( "ISO-8859-1" );

        ''' <summary>
        ''' The false boolean token.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly FALSE_BYTES() As Byte = {102, 97, 108, 115, 101} '"false".getBytes( "ISO-8859-1" );

        ''' <summary>
        ''' The PDF true value
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly [TRUE] As New COSBoolean(True)

        ''' <summary>
        ''' The PDF false value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly [FALSE] As New COSBoolean(False)

        Private value As Boolean

        '/**
        ' * Constructor.
        ' *
        ' * @param aValue The boolean value.
        ' */
        Private Sub New(ByVal aValue As Boolean)
            Me.value = aValue
        End Sub

        '/**
        ' * This will get the value that this object wraps.
        ' *
        ' * @return The boolean value of this object.
        ' */
        Public Function getValue() As Boolean
            Return Me.value
        End Function

        '/**
        ' * This will get the value that this object wraps.
        ' *
        ' * @return The boolean value of this object.
        ' */
        Public Function getValueAsObject() As Boolean
            Return value '(value?Boolean.TRUE:Boolean.FALSE);
        End Function

        '/**
        ' * This will get the boolean value.
        ' *
        ' * @param value Parameter telling which boolean value to get.
        ' *
        ' * @return The single boolean instance that matches the parameter.
        ' */
        Public Shared Function getBoolean(ByVal value As Boolean) As COSBoolean
            If (value) Then
                Return COSBoolean.TRUE
            Else
                Return COSBoolean.FALSE
            End If
        End Function

        '   /**
        '* This will get the boolean value.
        '*
        '* @param value Parameter telling which boolean value to get.
        '*
        '* @return The single boolean instance that matches the parameter.
        '*/
        'public shared Function getBoolean( Boolean value ) As COSBoolean 
        '{
        '    return getBoolean( value.booleanValue() );
        '}

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return visitor.visitFromBoolean(Me)
        End Function

        '/**
        ' * Return a string representation of this object.
        ' *
        ' * @return The string value of this object.
        ' */
        Public Overrides Function toString() As String
            Return CStr(value) 'String.valueOf(value)
        End Function

        '/**
        ' * This will write this object out to a PDF stream.
        ' *
        ' * @param output The stream to write this object out to.
        ' *
        ' * @throws IOException If an error occurs while writing out this object.
        ' */
        Public Sub writePDF(ByVal output As Stream) 'throws IOException
            If (value) Then
                output.Write(TRUE_BYTES, 0, UBound(TRUE_BYTES))
            Else
                output.Write(FALSE_BYTES, 0, UBound(FALSE_BYTES))
            End If
        End Sub

    End Class

End Namespace
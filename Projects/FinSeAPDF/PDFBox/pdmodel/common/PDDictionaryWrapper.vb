Imports FinSeA.org.apache.pdfbox.cos


Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * A wrapper for a COS dictionary.
    ' *
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' *
    ' */
    Public Class PDDictionaryWrapper
        Implements COSObjectable

        Private dictionary As COSDictionary

        '/**
        ' * Default constructor
        ' */
        Public Sub New()
            Me.dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Creates a new instance with a given COS dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            If (dictionary Is Nothing) Then Throw New ArgumentNullException("dictionary")
            Me.dictionary = dictionary
        End Sub


        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dictionary
        End Function

        '/**
        ' * Gets the COS dictionary.
        ' * 
        ' * @return the COS dictionary
        ' */
        Protected Function getCOSDictionary() As COSDictionary
            Return Me.dictionary
        End Function


        Public Overrides Function equals(ByVal obj As Object) As Boolean
            If (Me Is obj) Then Return True
            If (TypeOf (obj) Is PDDictionaryWrapper) Then
                Return Me.dictionary.Equals(DirectCast(obj, PDDictionaryWrapper).dictionary)
            End If
            Return False
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.dictionary.GetHashCode()

        End Function

    End Class

End Namespace

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf


    '/**
    ' * A standard attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public MustInherit Class PDStandardAttributeObject
        Inherits PDAttributeObject

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Creates a new standard attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Is the attribute with the given name specified in Me attribute object?
        ' * 
        ' * @param name the attribute name
        ' * @return <code>true</code> if the attribute is specified,
        ' * <code>false</code> otherwise
        ' */
        Public Function isSpecified(ByVal name As Single) As Boolean
            Return Me.getCOSDictionary().getDictionaryObject(name) IsNot Nothing
        End Function


        '/**
        ' * Gets a string attribute value.
        ' * 
        ' * @param name the attribute name
        ' * @return the string attribute value
        ' */
        Protected Function getString(ByVal name As String) As String
            Return Me.getCOSDictionary().getString(name)
        End Function

        '/**
        ' * Sets a string attribute value.
        ' * 
        ' * @param name the attribute name
        ' * @param value the string attribute value
        ' */
        Protected Sub setString(ByVal name As String, ByVal value As String)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setString(name, value)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Gets an array of strings.
        ' * 
        ' * @param name the attribute name
        ' * @return the array of strings
        ' */
        Protected Function getArrayOfString(ByVal name As String) As String()
            Dim v As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            If (TypeOf (v) Is COSArray) Then
                Dim array As COSArray = v
                Dim strings() As String = System.Array.CreateInstance(GetType(String), array.size())
                For i As Integer = 0 To array.size() - 1
                    strings(i) = DirectCast(array.getObject(i), COSName).getName()
                Next
                Return strings
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets an array of strings.
        ' * 
        ' * @param name the attribute name
        ' * @param values the array of strings
        ' */
        Protected Sub setArrayOfString(ByVal name As String, ByVal values() As String)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To values.Length - 1
                array.add(New COSString(values(i)))
            Next
            Me.getCOSDictionary().setItem(name, array)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Gets a name value.
        ' * 
        ' * @param name the attribute name
        ' * @return the name value
        ' */
        Protected Function getName(ByVal name As String) As String
            Return Me.getCOSDictionary().getNameAsString(name)
        End Function

        '/**
        ' * Gets a name value.
        ' * 
        ' * @param name the attribute name
        ' * @param defaultValue the default value
        ' * @return the name value
        ' */
        Protected Function getName(ByVal name As String, ByVal defaultValue As String) As String
            Return Me.getCOSDictionary().getNameAsString(name, defaultValue)
        End Function

        '/**
        ' * Gets a name value or array of name values.
        ' * 
        ' * @param name the attribute name
        ' * @param defaultValue the default value
        ' * @return a String or array of Strings
        ' */
        Protected Function getNameOrArrayOfName(ByVal name As String, ByVal defaultValue As String) As Object
            Dim v As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            If (TypeOf (v) Is COSArray) Then
                Dim array As COSArray = v
                Dim names() As String = System.Array.CreateInstance(GetType(String), array.size())
                For i As Integer = 0 To array.size() - 1
                    Dim item As COSBase = array.getObject(i)
                    If (TypeOf (item) Is COSName) Then
                        names(i) = DirectCast(item, COSName).getName()
                    End If
                Next
                Return names
            End If
            If (TypeOf (v) Is COSName) Then
                Return DirectCast(v, COSName).getName()
            End If
            Return defaultValue
        End Function

        '/**
        ' * Sets a name value.
        ' * 
        ' * @param name the attribute name
        ' * @param value the name value
        ' */
        Protected Sub setName(ByVal name As String, ByVal value As String)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setName(name, value)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Sets an array of name values.
        ' * 
        ' * @param name the attribute name
        ' * @param values the array of name values
        ' */
        Protected Sub setArrayOfName(ByVal name As String, ByVal values() As String)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To values.Length - 1
                Array.add(COSName.getPDFName(values(i)))
            Next
            Me.getCOSDictionary().setItem(name, Array)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Gets a number or a name value.
        ' * 
        ' * @param name the attribute name
        ' * @param defaultValue the default name
        ' * @return a NFloat or a String
        ' */
        Protected Function getNumberOrName(ByVal name As String, ByVal defaultValue As String) As Object
            Dim value As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            If (TypeOf (value) Is COSNumber) Then
                Return DirectCast(value, COSNumber).floatValue()
            End If
            If (TypeOf (value) Is COSName) Then
                Return DirectCast(value, COSName).getName()
            End If
            Return defaultValue
        End Function

        '/**
        ' * Gets an integer.
        ' * 
        ' * @param name the attribute name
        ' * @param defaultValue the default value
        ' * @return the integer
        ' */
        Protected Function getInteger(ByVal name As String, ByVal defaultValue As Integer) As Integer
            Return Me.getCOSDictionary().getInt(name, defaultValue)
        End Function

        '/**
        ' * Sets an integer.
        ' * 
        ' * @param name the attribute name
        ' * @param value the integer
        ' */
        Protected Sub setInteger(ByVal name As String, ByVal value As Integer)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setInt(name, value)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Gets a number value.
        ' * 
        ' * @param name the attribute name
        ' * @param defaultValue the default value
        ' * @return the number value
        ' */
        Protected Function getNumber(ByVal name As String, ByVal defaultValue As Single) As Single
            Return Me.getCOSDictionary().getFloat(name, defaultValue)
        End Function

        '/**
        ' * Gets a number value.
        ' * 
        ' * @param name the attribute name
        ' * @return the number value
        ' */
        Protected Function getNumber(ByVal name As String) As Single
            Return Me.getCOSDictionary().getFloat(name)
        End Function

        '/**
        ' * An "unspecified" default Single value.
        ' */
        Protected Shared UNSPECIFIED As Single = -1.0

        '/**
        ' * Gets a number or an array of numbers.
        ' * 
        ' * @param name the attribute name
        ' * @param defaultValue the default value
        ' * @return a NFloat or an array of floats
        ' */
        Protected Function getNumberOrArrayOfNumber(ByVal name As String, ByVal defaultValue As Single) As Object
            Dim v As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            If (TypeOf (v) Is COSArray) Then
                Dim array As COSArray = v
                Dim values() As Single = System.Array.CreateInstance(GetType(Single), array.size())
                For i As Integer = 0 To array.size() - 1
                    Dim item As COSBase = array.getObject(i)
                    If (TypeOf (item) Is COSNumber) Then
                        values(i) = DirectCast(item, COSNumber).floatValue()
                    End If
                Next
                Return values
            End If
            If (TypeOf (v) Is COSNumber) Then
                Return DirectCast(v, COSNumber).floatValue()
            End If
            If (defaultValue = UNSPECIFIED) Then
                Return Nothing
            End If
            Return defaultValue
        End Function

        '/**
        ' * Sets a Single number.
        ' * 
        ' * @param name the attribute name
        ' * @param value the Single number
        ' */
        Protected Sub setNumber(ByVal name As String, ByVal value As Single)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setFloat(name, value)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Sets an integer number.
        ' * 
        ' * @param name the attribute name
        ' * @param value the integer number
        ' */
        Protected Sub setNumber(ByVal name As String, ByVal value As Integer)
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setInt(name, value)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Sets an array of Single numbers.
        ' * 
        ' * @param name the attribute name
        ' * @param values the Single numbers
        ' */
        Protected Sub setArrayOfNumber(ByVal name As String, ByVal values() As Single)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To values.Length - 1
                array.add(New COSFloat(values(i)))
            Next
            Dim oldBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setItem(name, array)
            Dim newBase As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.potentiallyNotifyChanged(oldBase, newBase)
        End Sub

        '/**
        ' * Gets a colour.
        ' * 
        ' * @param name the attribute name
        ' * @return the colour
        ' */
        Protected Overridable Function getColor(ByVal name As String) As PDGamma
            Dim c As COSArray = Me.getCOSDictionary().getDictionaryObject(name)
            If (c IsNot Nothing) Then
                Return New PDGamma(c)
            End If
            Return Nothing
        End Function

        '/**
        ' * Gets a single colour or four colours.
        ' * 
        ' * @param name the attribute name
        ' * @return the single ({@link PDGamma}) or a ({@link PDFourColours})
        ' */
        Protected Function getColorOrFourColors(ByVal name As String) As Object
            Dim array As COSArray = Me.getCOSDictionary().getDictionaryObject(name)
            If (array Is Nothing) Then
                Return Nothing
            End If
            If (array.size() = 3) Then
                ' only one colour
                Return New PDGamma(array)
            ElseIf (array.size() = 4) Then
                Return New PDFourColours(array)
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets a colour.
        ' * 
        ' * @param name the attribute name
        ' * @param value the colour
        ' */
        Protected Sub setColor(ByVal name As String, ByVal value As PDGamma)
            Dim oldValue As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setItem(name, value)
            Dim newValue As COSBase
            If (value Is Nothing) Then
                newValue = Nothing
            Else
                newValue = value.getCOSObject()
            End If
            Me.potentiallyNotifyChanged(oldValue, newValue)
        End Sub

        '/**
        ' * Sets four colours.
        ' * 
        ' * @param name the attribute name
        ' * @param value the four colours
        ' */
        Protected Sub setFourColors(ByVal name As String, ByVal value As PDFourColours)
            Dim oldValue As COSBase = Me.getCOSDictionary().getDictionaryObject(name)
            Me.getCOSDictionary().setItem(name, value)
            Dim newValue As COSBase = Nothing
            If (value IsNot Nothing) Then newValue = value.getCOSObject()
            Me.potentiallyNotifyChanged(oldValue, newValue)
        End Sub

    End Class

End Namespace

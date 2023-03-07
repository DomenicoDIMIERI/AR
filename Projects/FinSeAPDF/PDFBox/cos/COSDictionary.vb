Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util
Imports System.Text

Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents a dictionary where name/value pairs reside.
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.32 $
    ' */
    Public Class COSDictionary
        Inherits COSBase

        Private Const PATH_SEPARATOR = "/"

        ''' <summary>
        ''' The name-value pairs of this dictionary. The pairs are kept in the order they were added to the dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected items As Map(Of COSName, COSBase) = New LinkedHashMap(Of COSName, COSBase)()

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Copy Constructor.  This will make a shallow copy of this dictionary.
        ' *
        ' * @param dict The dictionary to copy.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            items.putAll(dict.items)
        End Sub

        '/**
        ' * @see java.util.Map#containsValue(java.lang.Object)
        ' *
        ' * @param value The value to find in the map.
        ' *
        ' * @return true if the map contains this value.
        ' */
        Public Function containsValue(ByVal value As Object) As Boolean
            Dim contains As Boolean = items.containsValue(value)
            If (Not contains AndAlso TypeOf (value) Is COSObject) Then
                contains = items.containsValue(DirectCast(value, COSObject).getObject())
            End If
            Return contains
        End Function

        '/**
        ' * Search in the map for the value that matches the parameter
        ' * and return the first key that maps to that value.
        ' *
        ' * @param value The value to search for in the map.
        ' * @return The key for the value in the map or null if it does not exist.
        ' */
        Public Function getKeyForValue(ByVal value As Object) As COSName
            For Each entry As Map.Entry(Of COSName, COSBase) In items.entrySet()
                Dim nextValue As Object = entry.Value
                If (nextValue.Equals(value) OrElse (TypeOf (nextValue) Is COSObject AndAlso DirectCast(nextValue, COSObject).getObject().Equals(value))) Then
                    Return entry.Key
                End If
            Next
            Return Nothing
        End Function

        '/**
        ' * This will return the number of elements in this dictionary.
        ' *
        ' * @return The number of elements in the dictionary.
        ' */
        Public Function size() As Integer
            Return items.size()
        End Function

        '/**
        ' * This will clear all items in the map.
        ' */
        Public Sub clear()
            items.clear()
        End Sub

        '/**
        ' * This will get an object from this dictionary.  If the object is a reference then it will
        ' * dereference it and get it from the document.  If the object is COSNull then
        ' * null will be returned.
        ' *
        ' * @param key The key to the object that we are getting.
        ' *
        ' * @return The object that matches the key.
        ' */
        Public Overridable Function getDictionaryObject(ByVal key As String) As COSBase
            Return getDictionaryObject(COSName.getPDFName(key))
        End Function

        '/**
        ' * This is a special case of getDictionaryObject that takes multiple keys, it will handle
        ' * the situation where multiple keys could get the same value, ie if either CS or ColorSpace
        ' * is used to get the colorspace.
        ' * This will get an object from this dictionary.  If the object is a reference then it will
        ' * dereference it and get it from the document.  If the object is COSNull then
        ' * null will be returned.
        ' *
        ' * @param firstKey The first key to try.
        ' * @param secondKey The second key to try.
        ' *
        ' * @return The object that matches the key.
        ' * 
        ' * @deprecated  use {@link #getDictionaryObject(COSName, COSName)} using COSName constants instead
        ' */
        Public Function getDictionaryObject(ByVal firstKey As String, ByVal secondKey As String) As COSBase
            Dim retval As COSBase = getDictionaryObject(COSName.getPDFName(firstKey))
            If (retval Is Nothing) Then
                retval = getDictionaryObject(COSName.getPDFName(secondKey))
            End If
            Return retval
        End Function

        '/**
        ' * This is a special case of getDictionaryObject that takes multiple keys, it will handle
        ' * the situation where multiple keys could get the same value, ie if either CS or ColorSpace
        ' * is used to get the colorspace.
        ' * This will get an object from this dictionary.  If the object is a reference then it will
        ' * dereference it and get it from the document.  If the object is COSNull then
        ' * null will be returned.
        ' *
        ' * @param firstKey The first key to try.
        ' * @param secondKey The second key to try.
        ' *
        ' * @return The object that matches the key.
        ' */
        Public Function getDictionaryObject(ByVal firstKey As COSName, ByVal secondKey As COSName) As COSBase
            Dim retval As COSBase = getDictionaryObject(firstKey)
            If (retval Is Nothing AndAlso secondKey IsNot Nothing) Then
                retval = getDictionaryObject(secondKey)
            End If
            Return retval
        End Function

        '/**
        ' * This is a special case of getDictionaryObject that takes multiple keys, it will handle
        ' * the situation where multiple keys could get the same value, ie if either CS or ColorSpace
        ' * is used to get the colorspace.
        ' * This will get an object from this dictionary.  If the object is a reference then it will
        ' * dereference it and get it from the document.  If the object is COSNull then
        ' * null will be returned.
        ' *
        ' * @param keyList The list of keys to find a value.
        ' *
        ' * @return The object that matches the key.
        ' */
        Public Function getDictionaryObject(ByVal keyList As String()) As COSBase
            Dim retval As COSBase = Nothing
            For i As Integer = 0 To keyList.Length - 1
                If (retval IsNot Nothing) Then Exit For
                retval = getDictionaryObject(COSName.getPDFName(keyList(i)))
            Next
            Return retval
        End Function

        '/**
        ' * This will get an object from this dictionary.  If the object is a reference then it will
        ' * dereference it and get it from the document.  If the object is COSNull then
        ' * null will be returned.
        ' *
        ' * @param key The key to the object that we are getting.
        ' *
        ' * @return The object that matches the key.
        ' */
        Public Overridable Function getDictionaryObject(ByVal key As COSName) As COSBase
            Dim retval As COSBase = items.get(key)
            If (TypeOf (retval) Is COSObject) Then
                retval = DirectCast(retval, COSObject).getObject()
            End If
            If (TypeOf (retval) Is COSNull) Then
                retval = Nothing
            End If
            Return retval
        End Function

        '/**
        ' * This will set an item in the dictionary.  If value is null then the result
        ' * will be the same as removeItem( key ).
        ' *
        ' * @param key The key to the dictionary object.
        ' * @param value The value to the dictionary object.
        ' */
        Public Sub setItem(ByVal key As COSName, ByVal value As COSBase)
            If (value Is Nothing) Then
                removeItem(key)
            Else
                items.put(key, value)
            End If
        End Sub

        '/**
        ' * This will set an item in the dictionary.  If value is null then the result
        ' * will be the same as removeItem( key ).
        ' *
        ' * @param key The key to the dictionary object.
        ' * @param value The value to the dictionary object.
        ' */
        Public Sub setItem(ByVal key As COSName, ByVal value As COSObjectable)
            Dim base As COSBase = Nothing
            If (value IsNot Nothing) Then
                base = value.getCOSObject()
            End If
            setItem(key, base)
        End Sub

        '/**
        ' * This will set an item in the dictionary.  If value is null then the result
        ' * will be the same as removeItem( key ).
        ' *
        ' * @param key The key to the dictionary object.
        ' * @param value The value to the dictionary object.
        ' */
        Public Sub setItem(ByVal key As String, ByVal value As COSObjectable)
            setItem(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This will set an item in the dictionary.
        ' *
        ' * @param key The key to the dictionary object.
        ' * @param value The value to the dictionary object.
        ' */
        Public Sub setBoolean(ByVal key As String, ByVal value As Boolean)
            setItem(COSName.getPDFName(key), COSBoolean.getBoolean(value))
        End Sub

        '/**
        ' * This will set an item in the dictionary.
        ' *
        ' * @param key The key to the dictionary object.
        ' * @param value The value to the dictionary object.
        ' */
        Public Sub setBoolean(ByVal key As COSName, ByVal value As Boolean)
            setItem(key, COSBoolean.getBoolean(value))
        End Sub

        '/**
        ' * This will set an item in the dictionary.  If value is null then the result
        ' * will be the same as removeItem( key ).
        ' *
        ' * @param key The key to the dictionary object.
        ' * @param value The value to the dictionary object.
        ' */
        Public Sub setItem(ByVal key As String, ByVal value As COSBase)
            setItem(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSName
        ' * object.  If it is null then the object will be removed.
        ' *
        ' * @param key The key to the object,
        ' * @param value The string value for the name.
        ' */
        Public Sub setName(ByVal key As String, ByVal value As String)
            setName(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSName
        ' * object.  If it is null then the object will be removed.
        ' *
        ' * @param key The key to the object,
        ' * @param value The string value for the name.
        ' */
        Public Sub setName(ByVal key As COSName, ByVal value As String)
            Dim name As COSName = Nothing
            If (value IsNot Nothing) Then
                name = COSName.getPDFName(value)
            End If
            setItem(key, name)
        End Sub

        '/**
        ' * Set the value of a date entry in the dictionary.
        ' *
        ' * @param key The key to the date value.
        ' * @param date The date value.
        ' */
        Public Sub setDate(ByVal key As String, ByVal [date] As NDate)
            setDate(COSName.getPDFName(key), [date])
        End Sub

        '/**
        ' * Set the date object.
        ' *
        ' * @param key The key to the date.
        ' * @param date The date to set.
        ' */
        Public Sub setDate(ByVal key As COSName, ByVal [date] As NDate)
            setString(key, DateConverter.toString([date]))
        End Sub

        '/**
        ' * Set the value of a date entry in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary.
        ' * @param key The key to the date value.
        ' * @param date The date value.
        ' */
        Public Sub setEmbeddedDate(ByVal embedded As String, ByVal key As String, ByVal [date] As NDate)
            setEmbeddedDate(embedded, COSName.getPDFName(key), [date])
        End Sub

        '/**
        ' * Set the date object.
        ' *
        ' * @param embedded The embedded dictionary.
        ' * @param key The key to the date.
        ' * @param date The date to set.
        ' */
        Public Sub setEmbeddedDate(ByVal embedded As String, ByVal key As COSName, ByVal [date] As NDate)
            Dim dic As COSDictionary = getDictionaryObject(embedded)
            If (dic Is Nothing AndAlso [date].HasValue) Then
                dic = New COSDictionary()
                setItem(embedded, dic)
            End If
            If (dic IsNot Nothing) Then
                dic.setDate(key, [date])
            End If
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSString
        ' * object.  If it is null then the object will be removed.
        ' *
        ' * @param key The key to the object,
        ' * @param value The string value for the name.
        ' */
        Public Sub setString(ByVal key As String, ByVal value As String)
            setString(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSString
        ' * object.  If it is null then the object will be removed.
        ' *
        ' * @param key The key to the object,
        ' * @param value The string value for the name.
        ' */
        Public Sub setString(ByVal key As COSName, ByVal value As String)
            Dim name As COSString = Nothing
            If (value IsNot Nothing) Then
                name = New COSString(value)
            End If
            setItem(key, name)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSString
        ' * object.  If it is null then the object will be removed.
        ' *
        ' * @param embedded The embedded dictionary to set the item in.
        ' * @param key The key to the object,
        ' * @param value The string value for the name.
        ' */
        Public Sub setEmbeddedString(ByVal embedded As String, ByVal key As String, ByVal value As String)
            setEmbeddedString(embedded, COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSString
        ' * object.  If it is null then the object will be removed.
        ' *
        ' * @param embedded The embedded dictionary to set the item in.
        ' * @param key The key to the object,
        ' * @param value The string value for the name.
        ' */
        Public Sub setEmbeddedString(ByVal embedded As String, ByVal key As COSName, ByVal value As String)
            Dim dic As COSDictionary = getDictionaryObject(embedded)
            If (dic Is Nothing AndAlso value IsNot Nothing) Then
                dic = New COSDictionary()
                setItem(embedded, dic)
            End If
            If (dic IsNot Nothing) Then
                dic.setString(key, value)
            End If
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSInteger
        ' * object.
        ' *
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setInt(ByVal key As String, ByVal value As Integer)
            setInt(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSInteger
        ' * object.
        ' *
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setInt(ByVal key As COSName, ByVal value As Integer)
            setItem(key, COSInteger.get(value))
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSInteger
        ' * object.
        ' *
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setLong(ByVal key As String, ByVal value As Long)
            setLong(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSInteger
        ' * object.
        ' *
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setLong(ByVal key As COSName, ByVal value As Long)
            Dim intVal As COSInteger = Nothing
            intVal = COSInteger.get(value)
            setItem(key, intVal)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSInteger
        ' * object.
        ' *
        ' * @param embeddedDictionary The embedded dictionary.
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setEmbeddedInt(ByVal embeddedDictionary As String, ByVal key As String, ByVal value As Integer)
            setEmbeddedInt(embeddedDictionary, COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSInteger
        ' * object.
        ' *
        ' * @param embeddedDictionary The embedded dictionary.
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setEmbeddedInt(ByVal embeddedDictionary As String, ByVal key As COSName, ByVal value As Integer)
            Dim embedded As COSDictionary = getDictionaryObject(embeddedDictionary)
            If (embedded Is Nothing) Then
                embedded = New COSDictionary()
                setItem(embeddedDictionary, embedded)
            End If
            embedded.setInt(key, value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSFloat
        ' * object.
        ' *
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setFloat(ByVal key As String, ByVal value As Single)
            setFloat(COSName.getPDFName(key), value)
        End Sub

        '/**
        ' * This is a convenience method that will convert the value to a COSFloat
        ' * object.
        ' *
        ' * @param key The key to the object,
        ' * @param value The int value for the name.
        ' */
        Public Sub setFloat(ByVal key As COSName, ByVal value As Single)
            Dim fltVal As COSFloat = New COSFloat(value)
            setItem(key, fltVal)
        End Sub

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' */
        Public Function getNameAsString(ByVal key As String) As String
            Return getNameAsString(COSName.getPDFName(key))
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' */
        Public Function getNameAsString(ByVal key As COSName) As String
            Dim retval As String = vbNullString
            Dim name As COSBase = getDictionaryObject(key)
            If (name IsNot Nothing) Then
                If (TypeOf (name) Is COSName) Then
                    retval = DirectCast(name, COSName).getName()
                ElseIf (TypeOf (name) Is COSString) Then
                    retval = DirectCast(name, COSString).getString()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The name converted to a string.
        ' */
        Public Function getNameAsString(ByVal key As String, ByVal defaultValue As String) As String
            Return getNameAsString(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The name converted to a string.
        ' */
        Public Function getNameAsString(ByVal key As COSName, ByVal defaultValue As String) As String
            Dim retval As String = getNameAsString(key)
            If (retval = vbNullString) Then
                retval = defaultValue
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' */
        Public Function getString(ByVal key As String) As String
            Return getString(COSName.getPDFName(key))
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' */
        Public Function getString(ByVal key As COSName) As String
            Dim retval As String = vbNullString
            Dim value As COSBase = getDictionaryObject(key)
            If (value IsNot Nothing AndAlso TypeOf (value) Is COSString) Then
                retval = DirectCast(value, COSString).getString()
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' */
        Public Function getString(ByVal key As String, ByVal defaultValue As String) As String
            Return getString(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' */
        Public Function getString(ByVal key As COSName, ByVal defaultValue As String) As String
            Dim retval As String = getString(key)
            If (retval = vbNullString) Then
                retval = defaultValue
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary.
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' */
        Public Function getEmbeddedString(ByVal embedded As String, ByVal key As String) As String
            Return getEmbeddedString(embedded, COSName.getPDFName(key), vbNullString)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary.
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' */
        Public Function getEmbeddedString(ByVal embedded As String, ByVal key As COSName) As String
            Return getEmbeddedString(embedded, key, vbNullString)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary.
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' */
        Public Function getEmbeddedString(ByVal embedded As String, ByVal key As String, ByVal defaultValue As String) As String
            Return getEmbeddedString(embedded, COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary.
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' */
        Public Function getEmbeddedString(ByVal embedded As String, ByVal key As COSName, ByVal defaultValue As String) As String
            Dim retval As String = defaultValue
            Dim dic As COSDictionary = getDictionaryObject(embedded)
            If (dic IsNot Nothing) Then
                retval = dic.getString(key, defaultValue)
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getDate(ByVal key As String) As NDate
            Return getDate(COSName.getPDFName(key))
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' *
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getDate(ByVal key As COSName) As NDate
            Dim [date] As COSString = getDictionaryObject(key)
            Return DateConverter.toCalendar([date])
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a date.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getDate(ByVal key As String, ByVal defaultValue As NDate) As NDate
            Return getDate(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a date.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getDate(ByVal key As COSName, ByVal defaultValue As NDate) As NDate
            Dim retval As NDate = getDate(key)
            If (retval.HasValue = False) Then
                retval = defaultValue
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary to get.
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getEmbeddedDate(ByVal embedded As String, ByVal key As String) As NDate
            Return getEmbeddedDate(embedded, COSName.getPDFName(key), Nothing)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a name and convert it to a string.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary to get.
        ' * @param key The key to the item in the dictionary.
        ' * @return The name converted to a string.
        ' *
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getEmbeddedDate(ByVal embedded As String, ByVal key As COSName) As NDate
            Return getEmbeddedDate(embedded, key, Nothing)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a date.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary to get.
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getEmbeddedDate(ByVal embedded As String, ByVal key As String, ByVal defaultValue As NDate) As NDate
            Return getEmbeddedDate(embedded, COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a date.  Null is returned
        ' * if the entry does not exist in the dictionary.
        ' *
        ' * @param embedded The embedded dictionary to get.
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The default value to return.
        ' * @return The name converted to a string.
        ' * @throws IOException If there is an error converting to a date.
        ' */
        Public Function getEmbeddedDate(ByVal embedded As String, ByVal key As COSName, ByVal defaultValue As NDate) As NDate
            Dim retval As NDate = defaultValue
            Dim eDic As COSDictionary = getDictionaryObject(embedded)
            If (eDic IsNot Nothing) Then
                retval = eDic.getDate(key, defaultValue)
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a cos boolean and convert it to a primitive boolean.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value returned if the entry is null.
        ' *
        ' * @return The value converted to a boolean.
        ' */
        Public Function getBoolean(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
            Return getBoolean(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a COSBoolean and convert it to a primitive boolean.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value returned if the entry is null.
        ' *
        ' * @return The entry converted to a boolean.
        ' */
        Public Function getBoolean(ByVal key As COSName, ByVal defaultValue As Boolean) As Boolean
            Return getBoolean(key, Nothing, defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a COSBoolean and convert it to a primitive boolean.
        ' *
        ' * @param firstKey The first key to the item in the dictionary.
        ' * @param secondKey The second key to the item in the dictionary.
        ' * @param defaultValue The value returned if the entry is null.
        ' *
        ' * @return The entry converted to a boolean.
        ' */
        Public Function getBoolean(ByVal firstKey As COSName, ByVal secondKey As COSName, ByVal defaultValue As Boolean) As Boolean
            Dim retval As Boolean = defaultValue
            Dim bool As COSBase = getDictionaryObject(firstKey, secondKey)
            If (bool IsNot Nothing AndAlso TypeOf (bool) Is COSBoolean) Then
                retval = DirectCast(bool, COSBoolean).getValue()
            End If
            Return retval
        End Function

        '/**
        ' * Get an integer from an embedded dictionary.  Useful for 1-1 mappings.  default:-1
        ' *
        ' * @param embeddedDictionary The name of the embedded dictionary.
        ' * @param key The key in the embedded dictionary.
        ' *
        ' * @return The value of the embedded integer.
        ' */
        Public Function getEmbeddedInt(ByVal embeddedDictionary As String, ByVal key As String) As Integer
            Return getEmbeddedInt(embeddedDictionary, COSName.getPDFName(key))
        End Function

        '/**
        ' * Get an integer from an embedded dictionary.  Useful for 1-1 mappings.  default:-1
        ' *
        ' * @param embeddedDictionary The name of the embedded dictionary.
        ' * @param key The key in the embedded dictionary.
        ' *
        ' * @return The value of the embedded integer.
        ' */
        Public Function getEmbeddedInt(ByVal embeddedDictionary As String, ByVal key As COSName) As Integer
            Return getEmbeddedInt(embeddedDictionary, key, -1)
        End Function

        '/**
        ' * Get an integer from an embedded dictionary.  Useful for 1-1 mappings.
        ' *
        ' * @param embeddedDictionary The name of the embedded dictionary.
        ' * @param key The key in the embedded dictionary.
        ' * @param defaultValue The value if there is no embedded dictionary or it does not contain the key.
        ' *
        ' * @return The value of the embedded integer.
        ' */
        Public Function getEmbeddedInt(ByVal embeddedDictionary As String, ByVal key As String, ByVal defaultValue As Integer) As Integer
            Return getEmbeddedInt(embeddedDictionary, COSName.getPDFName(key), defaultValue)
        End Function


        '/**
        ' * Get an integer from an embedded dictionary.  Useful for 1-1 mappings.
        ' *
        ' * @param embeddedDictionary The name of the embedded dictionary.
        ' * @param key The key in the embedded dictionary.
        ' * @param defaultValue The value if there is no embedded dictionary or it does not contain the key.
        ' *
        ' * @return The value of the embedded integer.
        ' */
        Public Function getEmbeddedInt(ByVal embeddedDictionary As String, ByVal key As COSName, ByVal defaultValue As Integer) As Integer
            Dim retval As Integer = defaultValue
            Dim embedded As COSDictionary = getDictionaryObject(embeddedDictionary)
            If (embedded IsNot Nothing) Then
                retval = embedded.getInt(key, defaultValue)
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an int.  -1 is returned if there is no value.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The integer value.
        ' */
        Public Function getInt(ByVal key As String) As Integer
            Return getInt(COSName.getPDFName(key), -1)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an int.  -1 is returned if there is no value.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The integer value..
        ' */
        Public Function getInt(ByVal key As COSName) As Integer
            Return getInt(key, -1)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param keyList The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The integer value.
        ' */
        Public Function getInt(ByVal keyList As String(), ByVal defaultValue As Integer) As Integer
            Dim retval As Integer = defaultValue
            Dim obj As COSBase = getDictionaryObject(keyList)
            If (obj IsNot Nothing AndAlso TypeOf (obj) Is COSNumber) Then
                retval = DirectCast(obj, COSNumber).intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The integer value.
        ' */
        Public Function getInt(ByVal key As String, ByVal defaultValue As Integer) As Integer
            Return getInt(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The integer value.
        ' */
        Public Function getInt(ByVal key As COSName, ByVal defaultValue As Integer) As Integer
            Return getInt(key, Nothing, defaultValue)
        End Function


        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value -1 will be returned.
        ' *
        ' * @param firstKey The first key to the item in the dictionary.
        ' * @param secondKey The second key to the item in the dictionary.
        ' * @return The integer value.
        ' */
        Public Function getInt(ByVal firstKey As COSName, ByVal secondKey As COSName) As Integer
            Return getInt(firstKey, secondKey, -1)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param firstKey The first key to the item in the dictionary.
        ' * @param secondKey The second key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The integer value.
        ' */
        Public Function getInt(ByVal firstKey As COSName, ByVal secondKey As COSName, ByVal defaultValue As Integer) As Integer
            Dim retval As Integer = defaultValue
            Dim obj As COSBase = getDictionaryObject(firstKey, secondKey)
            If (obj IsNot Nothing AndAlso TypeOf (obj) Is COSNumber) Then
                retval = DirectCast(obj, COSNumber).intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an long.  -1 is returned if there is no value.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' *
        ' * @return The long value.
        ' */
        Public Function getLong(ByVal key As String) As Long
            Return getLong(COSName.getPDFName(key), -1L)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an long.  -1 is returned if there is no value.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The long value.
        ' */
        Public Function getLong(ByVal key As COSName) As Long
            Return getLong(key, -1L)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an long.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param keyList The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The long value.
        ' */
        Public Function getLong(ByVal keyList As String(), ByVal defaultValue As Long) As Long
            Dim retval As Long = defaultValue
            Dim obj As COSBase = getDictionaryObject(keyList)
            If (obj IsNot Nothing AndAlso TypeOf (obj) Is COSNumber) Then
                retval = DirectCast(obj, COSNumber).longValue()
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The integer value.
        ' */
        Public Function getLong(ByVal key As String, ByVal defaultValue As Long) As Long
            Return getLong(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an integer.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The integer value.
        ' */
        Public Function getLong(ByVal key As COSName, ByVal defaultValue As Long) As Long
            Dim retval As Long = defaultValue
            Dim obj As COSBase = getDictionaryObject(key)
            If (obj IsNot Nothing AndAlso TypeOf (obj) Is COSNumber) Then
                retval = DirectCast(obj, COSNumber).longValue()
            End If
            Return retval
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an Single.  -1 is returned if there is no value.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The Single value.
        ' */
        Public Function getFloat(ByVal key As String) As Single
            Return getFloat(COSName.getPDFName(key), -1)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an Single.  -1 is returned if there is no value.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @return The Single value.
        ' */
        Public Function getFloat(ByVal key As COSName) As Single
            Return getFloat(key, -1)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be a Single.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The Single value.
        ' */
        Public Function getFloat(ByVal key As String, ByVal defaultValue As Single) As Single
            Return getFloat(COSName.getPDFName(key), defaultValue)
        End Function

        '/**
        ' * This is a convenience method that will get the dictionary object that
        ' * is expected to be an Single.  If the dictionary value is null then the
        ' * default Value will be returned.
        ' *
        ' * @param key The key to the item in the dictionary.
        ' * @param defaultValue The value to return if the dictionary item is null.
        ' * @return The Single value.
        ' */
        Public Function getFloat(ByVal key As COSName, ByVal defaultValue As Single) As Single
            Dim retval As Single = defaultValue
            Dim obj As COSBase = getDictionaryObject(key)
            If (obj IsNot Nothing AndAlso TypeOf (obj) Is COSNumber) Then
                retval = DirectCast(obj, COSNumber).floatValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will remove an item for the dictionary.  This
        ' * will do nothing of the object does not exist.
        ' *
        ' * @param key The key to the item to remove from the dictionary.
        ' */
        Public Sub removeItem(ByVal key As COSName)
            items.remove(key)
        End Sub

        '/**
        ' * This will do a lookup into the dictionary.
        ' *
        ' * @param key The key to the object.
        ' *
        ' * @return The item that matches the key.
        ' */
        Public Overridable Function getItem(ByVal key As COSName) As COSBase
            Return items.get(key)
        End Function

        '/**
        ' * This will do a lookup into the dictionary.
        ' * 
        ' * @param key The key to the object.
        ' *
        ' * @return The item that matches the key.
        ' */
        Public Function getItem(ByVal key As String) As COSBase
            Return getItem(COSName.getPDFName(key))
        End Function

        '/**
        ' * This will get the keys for all objects in the dictionary in the sequence that
        ' * they were added.
        ' *
        ' * @deprecated Use the {@link #entrySet()} method instead.
        ' * @return a list of the keys in the sequence of insertion
        ' */
        Public Function keyList() As List(Of COSName)
            Return New ArrayList(Of COSName)(items.keySet())
        End Function

        '/**
        ' * Returns the names of the entries in this dictionary. The returned
        ' * set is in the order the entries were added to the dictionary.
        ' *
        ' * @since Apache PDFBox 1.1.0
        ' * @return names of the entries in this dictionary
        ' */
        Public Function keySet() As [Set](Of COSName)
            Return items.keySet()
        End Function

        '/**
        ' * Returns the name-value entries in this dictionary. The returned
        ' * set is in the order the entries were added to the dictionary.
        ' *
        ' * @since Apache PDFBox 1.1.0
        ' * @return name-value entries in this dictionary
        ' */
        Public Function entrySet() As [Set](Of Map.Entry(Of COSName, COSBase))
            Return items.entrySet()
        End Function

        '/**
        ' * This will get all of the values for the dictionary.
        ' *
        ' * @return All the values for the dictionary.
        ' */
        Public Function getValues() As ICollection(Of COSBase)
            Return items.Values()
        End Function

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return The object that the visitor returns.
        ' *
        ' * @throws COSVisitorException If there is an error visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object
            Return visitor.visitFromDictionary(Me)
        End Function

        '/**
        ' * This will add all of the dictionarys keys/values to this dictionary.
        ' * Only called when adding keys to a trailer that already exists.
        ' *
        ' * @param dic The dic to get the keys from.
        ' */
        Public Sub addAll(ByVal dic As COSDictionary)
            For Each entry As Map.Entry(Of COSName, COSBase) In dic.entrySet()
                '/*
                ' * If we're at a second trailer, we have a linearized
                ' * pdf file, meaning that the first Size entry represents
                ' * all of the objects so we don't need to grab the second.
                ' */
                If (Not entry.Key.getName().Equals("Size") OrElse Not items.containsKey(COSName.getPDFName("Size"))) Then
                    setItem(entry.Key, entry.Value)
                End If
            Next
        End Sub

        '/**
        ' * @see java.util.Map#containsKey(Object)
        ' *
        ' * @param name The key to find in the map.
        ' * @return true if the map contains this key.
        ' */  
        Public Function containsKey(ByVal name As COSName) As Boolean
            Return Me.items.containsKey(name)
        End Function

        '/**
        ' * @see java.util.Map#containsKey(Object)
        ' *
        ' * @param name The key to find in the map.
        ' * @return true if the map contains this key.
        ' */
        Public Function containsKey(ByVal name As String) As Boolean
            Return containsKey(COSName.getPDFName(name))
        End Function

        '/**
        ' * This will add all of the dictionarys keys/values to this dictionary, but only
        ' * if they don't already exist.  If a key already exists in this dictionary then
        ' * nothing is changed.
        ' *
        ' * @param dic The dic to get the keys from.
        ' */
        Public Sub mergeInto(ByVal dic As COSDictionary)
            For Each entry As Map.Entry(Of COSName, COSBase) In dic.entrySet()
                If (getItem(entry.Key) Is Nothing) Then
                    setItem(entry.Key, entry.Value)
                End If
            Next
        End Sub

        '/**
        ' * Nice method, gives you every object you want
        ' * Arrays works properly too. Try "P/Annots/[k]/Rect"
        ' * where k means the index of the Annotsarray.
        ' *
        ' * @param objPath the relative path to the object.
        ' * @return the object
        ' */
        Public Function getObjectFromPath(ByVal objPath As String) As COSBase
            Dim retval As COSBase = Nothing
            Dim path As String() = objPath.Split(PATH_SEPARATOR)
            retval = Me

            For i As Integer = 0 To path.Length - 1
                If (TypeOf (retval) Is COSArray) Then
                    Dim idx As Integer = New NInteger(Strings.Replace(Strings.Replace(path(i), "\]", ""), "\[", "")).Value
                    retval = DirectCast(retval, COSArray).getObject(idx)
                ElseIf (TypeOf (retval) Is COSDictionary) Then
                    retval = DirectCast(retval, COSDictionary).getDictionaryObject(path(i))
                End If
            Next
            Return retval
        End Function


        Public Overrides Function toString() As String
            Dim retVal As New StringBuilder("COSDictionary{")
            For Each key As COSName In items.keySet()
                retVal.Append("(")
                retVal.Append(key)
                retVal.Append(":")
                If (getDictionaryObject(key) IsNot Nothing) Then
                    retVal.Append(getDictionaryObject(key).ToString())
                Else
                    retVal.Append("<null>")
                End If
                retVal.Append(") ")
            Next
            retVal.Append("}")
            Return retVal.ToString()
        End Function

        Function TRUETYPE_FONT() As Object
            Throw New NotImplementedException
        End Function

    End Class

End Namespace
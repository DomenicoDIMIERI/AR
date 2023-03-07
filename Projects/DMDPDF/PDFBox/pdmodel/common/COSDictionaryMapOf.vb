Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * This is a Map that will automatically sync the contents to a COSDictionary.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class COSDictionaryMap(Of K, V)
        Inherits COSDictionaryMap
        Implements Map(Of K, V)

        '/**
        ' * Constructor for this map.
        ' *
        ' * @param actualsMap The map with standard java objects as values.
        ' * @param dicMap The map with COSBase objects as values.
        ' */
        Public Sub New(ByVal actualsMap As Map(Of K, V), ByVal dicMap As COSDictionary)
            MyBase.New(actualsMap, dicMap)
        End Sub

        Public Shadows Function containsKey(ByVal key As K) As Boolean Implements Map(Of K, V).containsKey
            Return MyBase.containsKey(key)
        End Function

        Public Shadows Function containsValue(ByVal value As V) As Boolean Implements Map(Of K, V).containsValue
            Return MyBase.containsValue(value)
        End Function

        Public Shadows Function [get](ByVal key As K) As V Implements Map(Of K, V).get
            Return MyBase.get(key)
        End Function

        Public Shadows Function [put](ByVal key As K, ByVal value As V) As V Implements Map(Of K, V).put
            Return MyBase.put(key, value)
        End Function

        Public Shadows Function remove(ByVal key As K) As V Implements Map(Of K, V).remove
            Return MyBase.remove(key)
        End Function

        Public Shadows Sub putAll(ByVal t As Map(Of K, V)) Implements Map(Of K, V).putAll
            MyBase.putAll(t)
        End Sub

        Public Shadows Function keySet() As [Set](Of K) Implements Map(Of K, V).keySet
            Return MyBase.keySet
        End Function

        Public Shadows ReadOnly Property values() As ICollection(Of V) Implements Map(Of K, V).Values
            Get
                Return MyBase.values
            End Get
        End Property

        Public Shadows Function entrySet() As [Set](Of Map.Entry(Of K, V)) Implements Map(Of K, V).entrySet
            Return MyBase.entrySet
        End Function


        ''/**
        '' * This will take a map&lt;java.lang.String,org.apache.pdfbox.pdmodel.COSObjectable&gt;
        '' * and convert it into a COSDictionary&lt;COSName,COSBase&gt;.
        '' *
        '' * @param someMap A map containing COSObjectables
        '' *
        '' * @return A proper COSDictionary
        '' */
        'Public Shared Function convert(ByVal someMap As Map(Of String, V)) As COSDictionary
        '    'Iterator<?> iter = someMap.keySet().iterator();
        '    Dim dic As New COSDictionary()
        '    'While (iter.hasNext())
        '    For Each name As String In someMap.keySet()
        '        'Dim name As String = iter.next()
        '        Dim [object] As COSObjectable = someMap.get(name)
        '        dic.setItem(COSName.getPDFName(name), [object].getCOSObject())
        '        'End While
        '    Next
        '    Return dic
        'End Function

        ''/**
        '' * This will take a COS dictionary and convert it into COSDictionaryMap.  All cos
        '' * objects will be converted to their primitive form.
        '' *
        '' * @param map The COS mappings.
        '' * @return A standard java map.
        '' * @throws IOException If there is an error during the conversion.
        '' */
        'Public Shared Function convertBasicTypesToMap(ByVal map As COSDictionary) As COSDictionaryMap(Of String, Object) 'throws IOException
        '    Dim retval As COSDictionaryMap(Of String, Object) = Nothing
        '    If (map IsNot Nothing) Then
        '        Dim actualMap As Map(Of String, Object) = New HashMap(Of String, Object)
        '        For Each key As COSName In map.keySet()
        '            Dim cosObj As COSBase = map.getDictionaryObject(key)
        '            Dim actualObject As Object = Nothing
        '            If (TypeOf (cosObj) Is COSString) Then
        '                actualObject = DirectCast(cosObj, COSString).getString()
        '            ElseIf (TypeOf (cosObj) Is COSInteger) Then
        '                actualObject = New NInteger(DirectCast(cosObj, COSInteger).intValue())
        '            ElseIf (TypeOf (cosObj) Is COSName) Then
        '                actualObject = DirectCast(cosObj, COSName).getName()
        '            ElseIf (TypeOf (cosObj) Is COSFloat) Then
        '                actualObject = New Nullable(Of Single)(DirectCast(cosObj, COSFloat).floatValue())
        '            ElseIf (TypeOf (cosObj) Is COSBoolean) Then
        '                actualObject = IIf(DirectCast(cosObj, COSBoolean).getValue(), True, False)
        '            Else
        '                Throw New IOException("Error:unknown type of object to convert:" & cosObj.ToString)
        '            End If
        '            actualMap.put(key.getName(), actualObject)
        '        Next
        '        retval = New COSDictionaryMap(Of String, Object)(actualMap, map)
        '    End If

        '    Return retval
        'End Function

    End Class

End Namespace
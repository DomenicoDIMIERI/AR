Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * This is a Map that will automatically sync the contents to a COSDictionary.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class COSDictionaryMap
        Implements Map

        Private map As COSDictionary
        Private actuals As Map

        '/**
        ' * Constructor for this map.
        ' *
        ' * @param actualsMap The map with standard java objects as values.
        ' * @param dicMap The map with COSBase objects as values.
        ' */
        Public Sub New(ByVal actualsMap As Map, ByVal dicMap As COSDictionary)
            actuals = actualsMap
            map = dicMap
        End Sub


        Public Function size() As Integer Implements map.size
            Return map.size()
        End Function

        Public Function isEmpty() As Boolean Implements map.isEmpty
            Return size() = 0
        End Function

        Public Function containsKey(ByVal key As Object) As Boolean Implements map.containsKey
            Return map.keySet().contains(key)
        End Function

        Public Function containsValue(ByVal value As Object) As Boolean Implements map.containsValue
            Return actuals.containsValue(value)
        End Function

        Public Function [get](ByVal key As Object) As Object Implements map.get
            Return actuals.get(key)
        End Function

        Public Function [put](ByVal key As Object, ByVal value As Object) As Object Implements map.put
            Dim [object] As COSObjectable = value
            Dim keyv As Object = key
            map.setItem(COSName.getPDFName(CStr(keyv)), [object].getCOSObject())
            Return actuals.put(key, value)
        End Function

        Public Function remove(ByVal key As Object) As Object Implements map.remove
            Dim key1 As Object = key
            map.removeItem(COSName.getPDFName(CStr(key1)))
            Return actuals.remove(key)
        End Function



        Public Sub clear() Implements map.clear
            map.clear()
            actuals.clear()
        End Sub

        Public Function keySet() As [Set] Implements map.keySet
            Return actuals.keySet()
        End Function

        Public ReadOnly Property values() As ICollection Implements map.Values
            Get
                Return actuals.Values()
            End Get
        End Property



        Public Overrides Function equals(ByVal o As Object) As Boolean Implements map.equals
            Dim retval As Boolean = False
            If (TypeOf (o) Is COSDictionaryMap) Then
                Dim other As COSDictionaryMap = o
                retval = other.map.Equals(Me.map)
            End If
            Return retval
        End Function

        Public Overrides Function toString() As String
            Return actuals.ToString()
        End Function

        Public Function hashCode() As Integer Implements map.hashCode
            Return map.GetHashCode()
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return map.GetHashCode
        End Function




        '/**
        ' * This will take a map&lt;java.lang.String,org.apache.pdfbox.pdmodel.COSObjectable&gt;
        ' * and convert it into a COSDictionary&lt;COSName,COSBase&gt;.
        ' *
        ' * @param someMap A map containing COSObjectables
        ' *
        ' * @return A proper COSDictionary
        ' */
        Public Shared Function convert(ByVal someMap As Map(Of String, Object)) As COSDictionary
            'Iterator<?> iter = someMap.keySet().iterator();
            Dim dic As New COSDictionary()
            'While (iter.hasNext())
            For Each name As String In someMap.keySet()
                'Dim name As String = iter.next()
                Dim [object] As COSObjectable = someMap.get(name)
                dic.setItem(COSName.getPDFName(name), [object].getCOSObject())
                'End While
            Next
            Return dic
        End Function

        '/**
        ' * This will take a COS dictionary and convert it into COSDictionaryMap.  All cos
        ' * objects will be converted to their primitive form.
        ' *
        ' * @param map The COS mappings.
        ' * @return A standard java map.
        ' * @throws IOException If there is an error during the conversion.
        ' */
        Public Shared Function convertBasicTypesToMap(ByVal map As COSDictionary) As COSDictionaryMap(Of String, Object) 'throws IOException
            Dim retval As COSDictionaryMap(Of String, Object) = Nothing
            If (map IsNot Nothing) Then
                Dim actualMap As Map(Of String, Object) = New HashMap(Of String, Object)
                For Each key As COSName In map.keySet()
                    Dim cosObj As COSBase = map.getDictionaryObject(key)
                    Dim actualObject As Object = Nothing
                    If (TypeOf (cosObj) Is COSString) Then
                        actualObject = DirectCast(cosObj, COSString).getString()
                    ElseIf (TypeOf (cosObj) Is COSInteger) Then
                        actualObject = New NInteger(DirectCast(cosObj, COSInteger).intValue())
                    ElseIf (TypeOf (cosObj) Is COSName) Then
                        actualObject = DirectCast(cosObj, COSName).getName()
                    ElseIf (TypeOf (cosObj) Is COSFloat) Then
                        actualObject = New Nullable(Of Single)(DirectCast(cosObj, COSFloat).floatValue())
                    ElseIf (TypeOf (cosObj) Is COSBoolean) Then
                        actualObject = IIf(DirectCast(cosObj, COSBoolean).getValue(), True, False)
                    Else
                        Throw New IOException("Error:unknown type of object to convert:" & cosObj.ToString)
                    End If
                    actualMap.put(key.getName(), actualObject)
                Next
                retval = New COSDictionaryMap(Of String, Object)(actualMap, map)
            End If

            Return retval
        End Function


        Public Function entrySet() As [Set](Of Map.Entry(Of Object, Object)) Implements map.entrySet
            Return Collections.unmodifiableSet(actuals.entrySet())
        End Function
         
        Public Sub putAll(m As Map) Implements map.putAll
            Throw New NotImplementedException("Not yet implemented")
        End Sub
    End Class

End Namespace
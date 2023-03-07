Namespace java
    Public Class LinkedHashMap
        Inherits HashMap

        ''' <summary>
        ''' Returns true if this map should remove its eldest entry.
        ''' </summary>
        ''' <param name="eldest"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function removeEldestEntry(ByVal eldest As Map.Entry(Of Object, Object)) As Boolean
            Throw New NotImplementedException
        End Function

    End Class

End Namespace
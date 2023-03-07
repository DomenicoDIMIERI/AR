Imports FinSeA.Sistema

Namespace org.apache.fontbox.cff

    '/**
    ' * Class holding the IndexData of a CFF font. 
    ' */
    Public Class IndexData

        Private count As Integer
        Private offset() As Integer
        Private data() As Integer

        '/**
        ' * Constructor.
        ' * 
        ' * @param count number of index values
        ' */
        Public Sub New(ByVal count As Integer)
            Me.count = count
            Me.offset = Array.CreateInstance(GetType(Integer), count + 1)
        End Sub

        Public Function getBytes(ByVal index As Integer) As Byte()
            Dim length As Integer = offset(index + 1) - offset(index)
            Dim bytes() As Byte = Array.CreateInstance(GetType(Byte), length)
            For i As Integer = 0 To length - 1
                bytes(i) = CByte(data(offset(index) - 1 + i))
            Next
            Return bytes
        End Function

        Public Overrides Function toString() As String
            Return Me.GetType().Name & "[count=" & count & ", offset=" & Arrays.ToString(offset) & ", data=" & Arrays.ToString(data) & "]"
        End Function

        '/**
        ' * Returns the count value.
        ' * @return the count value
        ' */
        Public Function getCount() As Integer
            Return count
        End Function

        '/**
        ' * Sets the offset value to the given value.
        ' * @param index the index of the offset value
        ' * @param value the given offset value
        ' */
        Public Sub setOffset(ByVal index As Integer, ByVal value As Integer)
            offset(index) = value
        End Sub

        '/**
        ' * Returns the offset at the given index.
        ' * @param index the index
        ' * @return the offset value at the given index
        ' */
        Public Function getOffset(ByVal index As Integer) As Integer
            Return offset(index)
        End Function

        '/**
        ' * Initializes the data array with the given size.
        ' * @param dataSize the size of the data array
        ' */
        Public Sub initData(ByVal dataSize As Integer)
            data = Array.CreateInstance(GetType(Integer), dataSize)
        End Sub

        '/**
        ' * Sets the data value to the given value.
        ' * @param index the index of the data value
        ' * @param value the given data value
        ' */
        Public Sub setData(ByVal index As Integer, ByVal value As Integer)
            data(index) = value
        End Sub

    End Class

End Namespace
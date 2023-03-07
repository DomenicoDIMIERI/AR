Imports FinSeA.Sistema

Namespace org.apache.fontbox.cff

    '/**
    ' * This class represents a CharStringCommand.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class CharStringCommand


        Private commandKey As Key = Nothing

        '/**
        ' * Constructor with one value.
        ' * 
        ' * @param b0 value
        ' */
        Public Sub New(ByVal b0 As Integer)
            setKey(New Key(b0))
        End Sub

        '/**
        ' * Constructor with two values.
        ' * 
        ' * @param b0 value1
        ' * @param b1 value2
        ' */
        Public Sub New(ByVal b0 As Integer, ByVal b1 As Integer)
            setKey(New Key(b0, b1))
        End Sub

        '/**
        ' * Constructor with an array as values.
        ' * 
        ' * @param values array of values
        ' */
        Public Sub New(ByVal values() As Integer)
            setKey(New Key(values))
        End Sub

        '/**
        ' * The key of the CharStringCommand.
        ' * @return the key
        ' */
        Public Function getKey() As Key
            Return commandKey
        End Function

        Private Sub setKey(ByVal key As Key)
            commandKey = key
        End Sub

        Public Overrides Function toString() As String
            Return getKey().toString()
        End Function

        Public Function hashCode() As Integer
            Return Me.GetHashCode
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return getKey().hashCode()
        End Function

        Public Overrides Function equals(ByVal [object] As Object) As Boolean
            If (TypeOf ([object]) Is CharStringCommand) Then
                Dim that As CharStringCommand = [object]
                Return getKey().equals(that.getKey())
            End If
            Return False
        End Function

        '/**
        ' * A static class to hold one or more int values as key. 
        ' */
        Public Class Key
            Private keyValues() As Integer = Nothing

            '/**
            ' * Constructor with one value.
            ' * 
            ' * @param b0 value
            ' */
            Public Sub New(ByVal b0 As Integer)
                setValue({b0})
            End Sub

            '/**
            ' * Constructor with two values.
            ' * 
            ' * @param b0 value1
            ' * @param b1 value2
            ' */
            Public Sub New(ByVal b0 As Integer, ByVal b1 As Integer)
                setValue({b0, b1})
            End Sub

            '/**
            ' * Constructor with an array as values.
            ' * 
            ' * @param values array of values
            ' */
            Public Sub New(ByVal values() As Integer)
                setValue(values)
            End Sub

            '/**
            ' * Array the with the values.
            ' * 
            ' * @return array with the values
            ' */
            Public Function getValue() As Integer()
                Return keyValues
            End Function

            Private Sub setValue(ByVal value() As Integer)
                keyValues = value
            End Sub

            Public Overrides Function toString() As String
                Return Arrays.ToString(getValue())
            End Function

            Public Function hashCode() As Integer
                Return Me.GetHashCode
            End Function

            Public Overrides Function GetHashCode() As Integer
                If (keyValues(0) = 12) Then
                    If (keyValues.Length > 1) Then
                        Return keyValues(0) Xor keyValues(1)
                    End If
                End If
                Return keyValues(0)
            End Function

            Public Overrides Function equals(ByVal [object] As Object) As Boolean
                If (TypeOf ([object]) Is Key) Then
                    Dim that As Key = [object]
                    If (keyValues(0) = 12 AndAlso that.keyValues(0) = 12) Then
                        If (keyValues.Length > 1 AndAlso that.keyValues.Length > 1) Then
                            Return keyValues(1) = that.keyValues(1)
                        End If

                        Return keyValues.Length = that.keyValues.Length
                    End If
                    Return keyValues(0) = that.keyValues(0)
                End If
                Return False
            End Function
        End Class

        ''' <summary>
        ''' A map with the Type1 vocabulary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly TYPE1_VOCABULARY As Map(Of Key, String) = InitType1Map()

        Shared Function InitType1Map() As Map(Of Key, String)
            Dim map As Map(Of Key, String) = New LinkedHashMap(Of Key, String)()
            map.put(New Key(1), "hstem")
            map.put(New Key(3), "vstem")
            map.put(New Key(4), "vmoveto")
            map.put(New Key(5), "rlineto")
            map.put(New Key(6), "hlineto")
            map.put(New Key(7), "vlineto")
            map.put(New Key(8), "rrcurveto")
            map.put(New Key(9), "closepath")
            map.put(New Key(10), "callsubr")
            map.put(New Key(11), "return")
            map.put(New Key(12), "escape")
            map.put(New Key(12, 0), "dotsection")
            map.put(New Key(12, 1), "vstem3")
            map.put(New Key(12, 2), "hstem3")
            map.put(New Key(12, 6), "seac")
            map.put(New Key(12, 7), "sbw")
            map.put(New Key(12, 12), "div")
            map.put(New Key(12, 16), "callothersubr")
            map.put(New Key(12, 17), "pop")
            map.put(New Key(12, 33), "setcurrentpoint")
            map.put(New Key(13), "hsbw")
            map.put(New Key(14), "endchar")
            map.put(New Key(21), "rmoveto")
            map.put(New Key(22), "hmoveto")
            map.put(New Key(30), "vhcurveto")
            map.put(New Key(31), "hvcurveto")

            Return Collections.unmodifiableMap(map)
        End Function

        '/**
        ' * A map with the Type2 vocabulary.
        ' */
        Public Shared ReadOnly TYPE2_VOCABULARY As Map(Of Key, String) = InitType2Map()

        Private Shared Function InitType2Map() As Map(Of Key, String)
            Dim map As Map(Of Key, String) = New LinkedHashMap(Of Key, String)()
            map.put(New Key(1), "hstem")
            map.put(New Key(3), "vstem")
            map.put(New Key(4), "vmoveto")
            map.put(New Key(5), "rlineto")
            map.put(New Key(6), "hlineto")
            map.put(New Key(7), "vlineto")
            map.put(New Key(8), "rrcurveto")
            map.put(New Key(10), "callsubr")
            map.put(New Key(11), "return")
            map.put(New Key(12), "escape")
            map.put(New Key(12, 3), "and")
            map.put(New Key(12, 4), "or")
            map.put(New Key(12, 5), "not")
            map.put(New Key(12, 9), "abs")
            map.put(New Key(12, 10), "add")
            map.put(New Key(12, 11), "sub")
            map.put(New Key(12, 12), "div")
            map.put(New Key(12, 14), "neg")
            map.put(New Key(12, 15), "eq")
            map.put(New Key(12, 18), "drop")
            map.put(New Key(12, 20), "put")
            map.put(New Key(12, 21), "get")
            map.put(New Key(12, 22), "ifelse")
            map.put(New Key(12, 23), "random")
            map.put(New Key(12, 24), "mul")
            map.put(New Key(12, 26), "sqrt")
            map.put(New Key(12, 27), "dup")
            map.put(New Key(12, 28), "exch")
            map.put(New Key(12, 29), "index")
            map.put(New Key(12, 30), "roll")
            map.put(New Key(12, 34), "hflex")
            map.put(New Key(12, 35), "flex")
            map.put(New Key(12, 36), "hflex1")
            map.put(New Key(12, 37), "flex1")
            map.put(New Key(14), "endchar")
            map.put(New Key(18), "hstemhm")
            map.put(New Key(19), "hintmask")
            map.put(New Key(20), "cntrmask")
            map.put(New Key(21), "rmoveto")
            map.put(New Key(22), "hmoveto")
            map.put(New Key(23), "vstemhm")
            map.put(New Key(24), "rcurveline")
            map.put(New Key(25), "rlinecurve")
            map.put(New Key(26), "vvcurveto")
            map.put(New Key(27), "hhcurveto")
            map.put(New Key(28), "shortint")
            map.put(New Key(29), "callgsubr")
            map.put(New Key(30), "vhcurveto")
            map.put(New Key(31), "hvcurveto")

            Return Collections.unmodifiableMap(map)
        End Function


    End Class

End Namespace
Imports FinSeA.Sistema

Namespace org.apache.fontbox.cff

    '**
    ' * This class represents a CFF operator.
    ' * @author Villu Ruusmann
    ' * @version $Revision: 1.0 $
    ' */
    Public Class CFFOperator

        Private operatorKey As Key = Nothing
        Private operatorName As String = vbNullString

        Private Sub New(ByVal key As Key, ByVal name As String)
            setKey(key)
            setName(name)
        End Sub

        '/**
        ' * The key of the operator.
        ' * @return the key
        ' */
        Public Function getKey() As Key
            Return operatorKey
        End Function

        Private Sub setKey(ByVal key As Key)
            operatorKey = key
        End Sub

        '/**
        ' * The name of the operator.
        ' * @return the name
        ' */
        Public Function getName() As String
            Return operatorName
        End Function

        Private Sub setName(ByVal name As String)
            operatorName = name
        End Sub

        Public Overrides Function toString() As String
            Return getName()
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return getKey().hashCode()
        End Function

        Public Function hashCode() As Integer
            Return Me.GetHashCode
        End Function

        Public Overrides Function equals(ByVal [object] As Object) As Boolean
            If (TypeOf ([object]) Is CFFOperator) Then
                Dim that As CFFOperator = [object]
                Return getKey().equals(that.getKey())
            End If
            Return False
        End Function

        Private Shared Sub register(ByVal key As Key, ByVal name As String)
            Dim [operator] As New CFFOperator(key, name)
            keyMap.put(key, [operator])
            nameMap.put(name, [operator])
        End Sub

        '/**
        ' * Returns the operator corresponding to the given key.
        ' * @param key the given key
        ' * @return the corresponding operator
        ' */
        Public Shared Function getOperator(ByVal key As Key) As CFFOperator
            Return keyMap.get(key)
        End Function

        '/**
        ' * Returns the operator corresponding to the given name.
        ' * @param key the given name
        ' * @return the corresponding operator
        ' */
        Public Shared Function getOperator(ByVal name As String) As CFFOperator
            Return nameMap.get(name)
        End Function

        '/**
        ' * This class is a holder for a key value. It consists of one or two bytes.  
        ' * @author Villu Ruusmann
        ' */
        Public NotInheritable Class Key

            Private value() As Integer = Nothing

            '/**
            ' * Constructor.
            ' * @param b0 the one byte value
            ' */
            Public Sub New(ByVal b0 As Integer)
                Me.New({b0})
            End Sub

            '/**
            ' * Constructor.
            ' * @param b0 the first byte of a two byte value
            ' * @param b1 the second byte of a two byte value
            ' */
            Public Sub New(ByVal b0 As Integer, ByVal b1 As Integer)
                Me.New({b0, b1})
            End Sub

            Private Sub New(ByVal value() As Integer)
                setValue(value)
            End Sub

            '/**
            ' * Returns the value of the key.
            ' * @return the value
            ' */
            Public Function getValue() As Integer()
                Return value
            End Function

            Private Sub setValue(ByVal value() As Integer)
                Me.value = value
            End Sub


            Public Overrides Function toString() As String
                Return Arrays.ToString(getValue())
            End Function

            Public Overrides Function GetHashCode() As Integer
                Return Arrays.hashCode(getValue())
            End Function

            Public Function hashCode() As Integer
                Return Me.GetHashCode
            End Function

            Public Overrides Function equals(ByVal [object] As Object) As Boolean
                If (TypeOf ([object]) Is Key) Then
                    Dim that As Key = [object]
                    Return Sistema.Arrays.Compare(getValue(), that.getValue()) = 0
                End If
                Return False
            End Function
        End Class

        Private Shared ReadOnly keyMap As Map(Of CFFOperator.Key, CFFOperator) = New LinkedHashMap(Of CFFOperator.Key, CFFOperator)()
        Private Shared ReadOnly nameMap As Map(Of String, CFFOperator) = New LinkedHashMap(Of String, CFFOperator)()

        Shared Sub New()
            ' Top DICT
            register(New Key(0), "version")
            register(New Key(1), "Notice")
            register(New Key(12, 0), "Copyright")
            register(New Key(2), "FullName")
            register(New Key(3), "FamilyName")
            register(New Key(4), "Weight")
            register(New Key(12, 1), "isFixedPitch")
            register(New Key(12, 2), "ItalicAngle")
            register(New Key(12, 3), "UnderlinePosition")
            register(New Key(12, 4), "UnderlineThickness")
            register(New Key(12, 5), "PaintType")
            register(New Key(12, 6), "CharstringType")
            register(New Key(12, 7), "FontMatrix")
            register(New Key(13), "UniqueID")
            register(New Key(5), "FontBBox")
            register(New Key(12, 8), "StrokeWidth")
            register(New Key(14), "XUID")
            register(New Key(15), "charset")
            register(New Key(16), "Encoding")
            register(New Key(17), "CharStrings")
            register(New Key(18), "Private")
            register(New Key(12, 20), "SyntheticBase")
            register(New Key(12, 21), "PostScript")
            register(New Key(12, 22), "BaseFontName")
            register(New Key(12, 23), "BaseFontBlend")
            register(New Key(12, 30), "ROS")
            register(New Key(12, 31), "CIDFontVersion")
            register(New Key(12, 32), "CIDFontRevision")
            register(New Key(12, 33), "CIDFontType")
            register(New Key(12, 34), "CIDCount")
            register(New Key(12, 35), "UIDBase")
            register(New Key(12, 36), "FDArray")
            register(New Key(12, 37), "FDSelect")
            register(New Key(12, 38), "FontName")

            ' Private DICT
            register(New Key(6), "BlueValues")
            register(New Key(7), "OtherBlues")
            register(New Key(8), "FamilyBlues")
            register(New Key(9), "FamilyOtherBlues")
            register(New Key(12, 9), "BlueScale")
            register(New Key(12, 10), "BlueShift")
            register(New Key(12, 11), "BlueFuzz")
            register(New Key(10), "StdHW")
            register(New Key(11), "StdVW")
            register(New Key(12, 12), "StemSnapH")
            register(New Key(12, 13), "StemSnapV")
            register(New Key(12, 14), "ForceBold")
            register(New Key(12, 15), "LanguageGroup")
            register(New Key(12, 16), "ExpansionFactor")
            register(New Key(12, 17), "initialRandomSeed")
            register(New Key(19), "Subrs")
            register(New Key(20), "defaultWidthX")
            register(New Key(21), "nominalWidthX")
        End Sub

    End Class

End Namespace
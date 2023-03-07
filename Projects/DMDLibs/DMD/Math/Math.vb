Imports DMD.Sistema


Public NotInheritable Class Math
    Private Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Shared ReadOnly PI As Double = System.Math.PI
    Public Shared ReadOnly SQRT2 As Double = System.Math.Sqrt(2)
    Public Shared ReadOnly TOLMIN As Double = 0.000001
    Public Shared ReadOnly POWERSOF2() As UInt32 = {1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152, 4194304, 8388608, 16777216, 33554432, 67108864, 134217728, 268435456, 536870912, 1073741824, 2147483648} ', 4294967296}
     
    Public Shared Function hexdec(ByVal sHexNum As String) As Integer
        Return CInt("&H" & sHexNum)
    End Function

    ''' <summary>
    ''' Converte un angolo sessadecimale in un angolo in radianti
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function toRadians(ByVal value As Double) As Double
        Return value * Math.PI / 180
    End Function

#Region "Sign"

    Public Shared Function Sign(ByVal value As Double) As Integer
        Return System.Math.Sign(value)
    End Function

    Public Shared Function Sign(ByVal value As Decimal) As Integer
        Return System.Math.Sign(value)
    End Function

    Public Shared Function Sign(ByVal value As Integer) As Integer
        Return System.Math.Sign(value)
    End Function

    Public Shared Function Sign(ByVal value As SByte) As Integer
        Return System.Math.Sign(value)
    End Function

    Public Shared Function Sign(ByVal value As Short) As Integer
        Return System.Math.Sign(value)
    End Function

    Public Shared Function Sign(ByVal value As Long) As Integer
        Return System.Math.Sign(value)
    End Function

#End Region

#Region "sqrt"

    Public Shared Function Pow(ByVal base As Double, ByVal power As Double) As Double
        Return System.Math.Pow(base, power)
    End Function

#End Region

#Region "sqrt"

    Public Shared Function Sqrt(ByVal value As Double) As Double
        Return System.Math.Sqrt(value)
    End Function

#End Region

#Region "TestBit"

    Public Shared Function TestBit(ByVal value As Int32, ByVal bitPosition As Integer) As Boolean
        Return TestBit(Convert.ToUInt32(value), bitPosition)
    End Function

    Public Shared Function TestBit(ByVal value As UInt32, ByVal bitPosition As Integer) As Boolean
        If (bitPosition < 0 Or bitPosition > 31) Then Throw New ArgumentOutOfRangeException("bitPosition deve essere comprso tra 0 e 31")
        Return (value And POWERSOF2(bitPosition)) = POWERSOF2(bitPosition)
    End Function

#End Region

#Region "SetBit"

    Public Shared Function SetBit(ByVal value As Int32, ByVal bitPosition As Integer, ByVal bit As Boolean) As Boolean
        Return Convert.ToInt32(SetBit(Convert.ToUInt32(value), bitPosition, bit))
    End Function

    Public Shared Function SetBit(ByVal value As UInt32, ByVal bitPosition As Integer, ByVal bit As Boolean) As Boolean
        If (bitPosition < 0 Or bitPosition > 31) Then Throw New ArgumentOutOfRangeException("bitPosition deve essere comprso tra 0 e 31")
        If (bit) Then
            Return (value Or POWERSOF2(bitPosition))
        Else
            Return (value And Not POWERSOF2(bitPosition))
        End If
    End Function

#End Region

#Region "Mul"

    Public Shared Function Mul(ByVal a As Nullable(Of Double), ByVal b As Nullable(Of Double)) As Nullable(Of Double)
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value * b.Value
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Div"

    Public Shared Function Div(ByVal numeratore As Double?, ByVal denominatore As Double?) As Double?
        If (numeratore.HasValue AndAlso denominatore.HasValue) Then
            Return numeratore.Value / denominatore.Value
        Else
            Return Nothing
        End If
    End Function


#End Region

#Region "Sum"

    ''' <summary>
    ''' Somma due valori, se uno dei valori è null la somma è NULL
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Sum(ByVal a As Nullable(Of Double), ByVal b As Nullable(Of Double)) As Nullable(Of Double)
        If (a.HasValue AndAlso b.HasValue) Then
            Return a.Value + b.Value
        Else
            Return Nothing
        End If
    End Function

    

    Public Shared Function Sum(ByVal arr() As Nullable(Of Double)) As Nullable(Of Double)
        Dim ret As Nullable(Of Double) = Nothing
        For i As Integer = 0 To UBound(arr)
            If (arr(i).HasValue) Then
                If (ret.HasValue) Then
                    ret = ret.Value + arr(i).Value
                Else
                    ret = arr(i)
                End If
            End If
        Next
        Return ret
    End Function

    Public Shared Function Sum(ByVal arr() As Nullable(Of Decimal)) As Nullable(Of Decimal)
        Dim ret As Nullable(Of Decimal) = Nothing
        For i As Integer = 0 To UBound(arr)
            If (arr(i).HasValue) Then
                If (ret.HasValue) Then
                    ret = ret.Value + arr(i).Value
                Else
                    ret = arr(i)
                End If
            End If
        Next
        Return ret
    End Function

#End Region

#Region "Ave"

    ''' <summary>
    ''' Media aritmetica tra due valori. Se uno dei valori è NULL viene restituito NULL
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Ave(ByVal a As Nullable(Of Double), ByVal b As Nullable(Of Double)) As Nullable(Of Double)
        If (a.HasValue AndAlso b.HasValue) Then
            Return (a.Value + b.Value) / 2
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Media aritmetica tra due valori. Se uno dei valori è NULL viene escluso
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AveNulls(ByVal a As Nullable(Of Double), ByVal b As Nullable(Of Double)) As Nullable(Of Double)
        If (a.HasValue) Then
            If (b.HasValue) Then
                Return (a.Value + b.Value) / 2
            Else
                Return a.Value
            End If
        Else
            If (b.HasValue) Then
                Return b.Value
            Else
                Return Nothing
            End If
        End If
    End Function

#End Region

#Region "Sum"

    ''' <summary>
    ''' Somma due valori. Se uno dei valori è NULL viene considerato come 0
    ''' </summary>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SumNulls(ByVal a As Nullable(Of Double), ByVal b As Nullable(Of Double)) As Nullable(Of Double)
        If (a.HasValue) Then
            If (b.HasValue) Then
                Return a.Value + b.Value
            Else
                Return a.Value
            End If
        Else
            If (b.HasValue) Then
                Return b.Value
            Else
                Return Nothing
            End If
        End If
    End Function

#End Region

#Region "Max"

    Public Shared Function Max(Of T As Structure)(ByVal a As Nullable(Of T), ByVal b As Nullable(Of T)) As Nullable(Of T)
        If (a.HasValue) Then
            If (b.HasValue) Then
                Return Max(a.Value, b.Value)
            Else
                Return a.Value
            End If
        ElseIf (b.HasValue) Then
            Return b.Value
        Else
            Return Nothing
        End If
    End Function


    Public Shared Function Max(Of T)(ByVal a As T, ByVal b As T) As T
        Return IIf(Compare(a, b) > 0, a, b)
    End Function

    Public Shared Function Max(Of T)(ByVal items() As T) As T
        Dim c As Integer = 0
        Dim m As T = items(0)
        For i As Integer = 1 To UBound(items)
            c = Compare(m, items(i))
            If (c > 0) Then m = items(i)
        Next
        Return m
    End Function

    Public Shared Function Max(Of T)(ByVal col As IEnumerable(Of T)) As T
        Dim v As Boolean = False
        Dim ret As T
        For Each item As T In col
            If (v = False) Then
                ret = item
                v = True
            Else
                If Compare(item, ret) > 0 Then
                    ret = item
                End If
            End If
        Next
        Return ret
    End Function



#End Region

#Region "Compare"


    Public Shared Function Compare(Of T)(ByVal a As T, ByVal b As T) As Integer
        Return Arrays.Compare(a, b, Arrays.DefaultComparer)
    End Function

    Public Shared Function Compare(Of T)(ByVal a As T, ByVal b As T, ByVal comparer As Object) As Integer
        Return Arrays.Compare(a, b, comparer)
    End Function

#End Region

    Public Shared Function bindec(ByVal lBinaryNum As String) As Integer
        'Return parseInt(lBinaryNum.ToString(), 2)
        Dim ret As Integer = 0
        Dim p As Integer = 1
        For i As Integer = 1 To Len(lBinaryNum)
            If (Mid(lBinaryNum, i, 1) <> "0") Then ret += p
            p *= 2
        Next
        Return ret
    End Function

    Public Shared Function Sin(ByVal x As Single) As Single
        Return System.Math.Sin(x)
    End Function


    Public Shared Function Sin(ByVal x As Double) As Double
        Return System.Math.Sin(x)
    End Function

    Public Shared Function Cos(ByVal x As Single) As Single
        Return System.Math.Cos(x)
    End Function
    Public Shared Function Cos(ByVal x As Double) As Double
        Return System.Math.Cos(x)
    End Function

    Public Shared Function deg2rad(ByVal fDegrees As Double) As Double
        Return ((2 * Math.PI) / 360) * fDegrees
    End Function

    Public Shared Function dechex(ByVal lDecimalNum As Integer) As String
        Return Hex(lDecimalNum)
    End Function

    Public Shared Function getHex(ByVal num As Integer) As String
        Const hexStr As String = "0123456789ABCDEF"
        Dim Hex As String = ""
        If (num >= 16) Then
            Hex = hexStr.Chars(num / 16)
            num = num Mod 16
        End If
        Hex &= hexStr.Chars(num / 16)
        Return Hex
    End Function

    Public Shared Function hexCodeAt(ByVal text As String, ByVal index As Integer) As Integer
        'Return CInt("&H" & text.Chars(0).ToString(16))
        Return CInt("&H" & text.Chars(index))
    End Function



    Public Shared Function Abs(ByVal value As Double) As Double
        Return System.Math.Abs(value)
    End Function

#Region "Floor"
    ''' <summary>
    ''' Approssima un numero al suo valore intero per difetto 
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Floor(ByVal value As Decimal) As Decimal
        Return System.Math.Floor(value)
    End Function

    Public Shared Function Floor(ByVal value As Double) As Double
        Return System.Math.Floor(value)
    End Function

#End Region

#Region "Min"

    Public Shared Function Min(Of T)(ByVal a As T, ByVal b As T) As T
        Return IIf(Compare(a, b) < 0, a, b)
    End Function

    Public Shared Function Min(Of T As Structure)(ByVal a As Nullable(Of T), ByVal b As Nullable(Of T)) As Nullable(Of T)
        If (a.HasValue) Then
            If (b.HasValue) Then
                Return Min(a.Value, b.Value)
            Else
                Return a.Value
            End If
        ElseIf (b.HasValue) Then
            Return b.Value
        Else
            Return Nothing
        End If
    End Function


    Public Shared Function Min(Of T)(ByVal col As IEnumerable(Of T)) As T
        Dim v As Boolean = False
        Dim ret As T
        For Each item As T In col
            If (v = False) Then
                ret = item
                v = True
            Else
                If Compare(item, ret) < 0 Then
                    ret = item
                End If
            End If
        Next
        Return ret
    End Function
#End Region



    ''' <summary>
    ''' Tenta di ottenere un valore booleano dall'espressione e restituisce un codice di errore
    ''' </summary>
    ''' <param name="expr"></param>
    ''' <param name="errorCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TryParseBoolean(ByVal expr As String, ByRef errorCode As Integer) As Boolean
        Dim ret As Boolean
        If Not Boolean.TryParse(expr, ret) Then
            errorCode = 255
        Else
            errorCode = 0
        End If
        Return ret
    End Function


    ''' <summary>
    ''' Cerca uno zero della funzione f(x) all'interno dell'intervallo [a, b] utilizzando il metodo delle bisezioni
    ''' </summary>
    ''' <param name="fEv"></param>
    ''' <param name="a"></param>
    ''' <param name="b"></param>
    ''' <param name="tolmin1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FindZero( _
                    ByVal fEv As FunEvaluator, _
                    ByVal a As Double, _
                    ByVal b As Double, _
                    Optional ByVal tolmin1 As Double = 0.0000000001 _
                    ) As Double
        Dim fA, fB, fM, m As Double
        If (tolmin1 <= 0) Then tolmin1 = Math.TOLMIN
        fA = fEv.EvalFunction(a)
        If (Abs(fA) < tolmin1) Then Return a
        fB = fEv.EvalFunction(b)
        If (Abs(fB) < tolmin1) Then Return b
        m = (a + b) / 2
        fM = fEv.EvalFunction(m)
        If (Abs(fM) < tolmin1) Then Return m
        If (fA * fM < 0) Then Return FindZero(fEv, a, m, tolmin1)
        If (fM * fB < 0) Then Return FindZero(fEv, m, b, tolmin1)
        Return a
    End Function

#Region "Ceil"

    Public Shared Function Ceiling(ByVal d As Double) As Double
        Return System.Math.Ceiling(d)
    End Function

    Public Shared Function Ceiling(ByVal d As Decimal) As Decimal
        Return System.Math.Ceiling(d)
    End Function

#End Region

#Region "Round"

    Shared Function round(ByVal a As Double) As Double
        Return System.Math.Round(a)
    End Function

    Shared Function round(ByVal a As Decimal) As Decimal
        Return System.Math.Round(a)
    End Function

    Shared Function round(ByVal a As Double, ByVal digits As Integer) As Double
        Return System.Math.Round(a, digits)
    End Function

    Shared Function round(ByVal a As Decimal, ByVal digits As Integer) As Decimal
        Return System.Math.Round(a, digits)
    End Function

#End Region

#Region "atan"

    Public Shared Function atan(p As Double) As Double
        Return System.Math.Atan(p)
    End Function

#End Region


#Region "atan2"

    Public Shared Function atan2(ByVal y As Double, ByVal x As Double) As Double
        Return System.Math.Atan2(y, x)
    End Function

#End Region

#Region "acos"

    Shared Function acos(p1 As Double) As Double
        Return System.Math.Acos(p1)
    End Function

#End Region

    Shared Function toDegrees(ByVal angleRad As Double) As Double
        Return angleRad * 180 / PI
    End Function

    Shared Function Log10(ByVal value As Double) As Double
        Return System.Math.Log10(value)
    End Function

    Shared Function Log(ByVal value As Double) As Double
        Return System.Math.Log(value)
    End Function

    Shared Function Log(ByVal value As Double, ByVal newBase As Double) As Double
        Return System.Math.Log(value, newBase)
    End Function

    Public Shared Function tan(ByVal angleRad As Double) As Double
        Return System.Math.Tan(angleRad)
    End Function

    Public Shared Function asin(angleRad As Double) As Integer
        Return System.Math.Asin(angleRad)
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
 
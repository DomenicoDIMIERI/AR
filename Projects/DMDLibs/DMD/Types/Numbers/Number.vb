'Public Class Number
'    Private m_Value As Object

'    Public Sub New(ByVal value As Double)
'        Me.m_Value = value
'    End Sub

'    Function floatValue() As Single
'        Return CSng(Me.m_Value)
'    End Function

'    Function intValue() As Integer
'        Return CInt(Me.m_Value)
'    End Function

'    Function longValue() As Long
'        Return CLng(Me.m_Value)
'    End Function

'    Function isInteger() As Object
'        Return CLng(Me.m_Value) = Me.m_Value
'    End Function

'    Function doubleValue() As Double
'        Return CDbl(Me.m_Value)
'    End Function



'End Class


Public Interface Number

    Function byteValue() As Byte
    Function doubleValue() As Double
    Function floatValue() As Single
    Function intValue() As Integer
    Function longValue() As Long
    Function shortValue() As Short

    Function isInteger() As Object


End Interface

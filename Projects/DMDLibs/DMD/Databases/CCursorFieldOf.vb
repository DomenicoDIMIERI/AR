Imports DMD.Sistema
Imports System.Xml.Serialization

Partial Public Class Databases




    Public Class CCursorField(Of T As Structure)
        Inherits CCursorField

        Public Sub New()
        End Sub

        Public Sub New(ByVal fieldName As String, Optional ByVal op As OP = OP.OP_EQ, Optional ByVal nullable As Boolean = False)
            MyBase.New(fieldName, DBUtils.GetADOType(GetType(T)), op, nullable)
        End Sub

        <XmlIgnore> _
        Public Shadows Property Value As Nullable(Of T)
            Get
                If TypeOf (MyBase.Value) Is DBNull Then
                    Return Nothing
                Else
                    Return CType(MyBase.Value, T)
                End If
            End Get
            Set(value As Nullable(Of T))
                If value.HasValue Then
                    MyBase.Value = value
                Else
                    MyBase.Value = DBNull.Value
                End If
            End Set
        End Property

        <XmlIgnore> _
        Public Shadows Property Value1 As Nullable(Of T)
            Get
                If TypeOf (MyBase.Value1) Is DBNull Then
                    Return Nothing
                Else
                    Return CType(MyBase.Value1, T)
                End If
            End Get
            Set(value As Nullable(Of T))
                If value.HasValue Then
                    MyBase.Value1 = value
                Else
                    MyBase.Value1 = DBNull.Value
                End If
            End Set
        End Property

        Public Overloads Sub ValueIn(ByVal values() As T)
            If (values Is Nothing OrElse UBound(values) < 0) Then Throw New ArgumentNullException("values")
            Dim tmp As Object()
            ReDim tmp(UBound(values))
            For i As Integer = 0 To UBound(values)
                tmp(i) = values(i)
            Next
            MyBase.ValueIn(tmp)
        End Sub

        Public Overloads Sub ValueIn(ByVal values() As Nullable(Of T))
            If (values Is Nothing OrElse UBound(values) < 0) Then Throw New ArgumentNullException("values")
            Dim tmp As Object()
            ReDim tmp(UBound(values))
            For i As Integer = 0 To UBound(values)
                tmp(i) = values(i)
            Next
            MyBase.ValueIn(tmp)
        End Sub

    End Class

End Class

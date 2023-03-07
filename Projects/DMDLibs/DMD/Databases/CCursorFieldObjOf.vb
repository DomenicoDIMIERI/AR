Imports DMD.Sistema
Imports System.Xml.Serialization

Partial Public Class Databases


    Public Class CCursorFieldObj(Of T)
        Inherits CCursorField

        Public Sub New()
        End Sub

        Public Sub New(ByVal fieldName As String, Optional ByVal op As OP = OP.OP_EQ, Optional ByVal nullable As Boolean = False)
            MyBase.New(fieldName, DBUtils.GetADOType(GetType(T)), op, nullable)
        End Sub

        <XmlIgnore> _
        Public Shadows Property Value As T
            Get
                If TypeOf (MyBase.Value) Is DBNull Then
                    Return Nothing
                Else
                    Return MyBase.Value
                End If
            End Get
            Set(value As T)
                If value Is Nothing Then
                    MyBase.Value = DBNull.Value
                Else
                    MyBase.Value = value
                End If
            End Set
        End Property

        <XmlIgnore>
        Public Shadows Property Value1 As T
            Get
                If TypeOf (MyBase.Value1) Is DBNull Then
                    Return Nothing
                Else
                    Return MyBase.Value1
                End If
            End Get
            Set(value As T)
                If value Is Nothing Then
                    MyBase.Value1 = DBNull.Value
                Else
                    MyBase.Value1 = value
                End If
            End Set
        End Property

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            MyBase.SetFieldInternal(fieldName, fieldValue)
        End Sub

        Protected Overrides Function parseFieldValues(value As Object) As Object()
            Return MyBase.parseFieldValues(value)
        End Function

    End Class




End Class

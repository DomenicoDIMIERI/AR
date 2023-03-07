Imports DMD.Sistema
Imports System.Xml.Serialization

Partial Public Class Databases


    Public Class CCursorFieldDBDate
        Inherits CCursorField

        Public Sub New()
        End Sub

        Public Sub New(ByVal fieldName As String, Optional ByVal op As OP = OP.OP_EQ, Optional ByVal nullable As Boolean = False)
            MyBase.New(fieldName, adDataTypeEnum.adWChar, op, nullable)
        End Sub

        <XmlIgnore> _
        Public Shadows Property Value As Date?
            Get
                If TypeOf (MyBase.Value) Is DBNull Then
                    Return Nothing
                Else
                    Return DBUtils.FromDBDateStr(MyBase.Value)
                End If
            End Get
            Set(value As Date?)
                If value Is Nothing Then
                    MyBase.Value = DBNull.Value
                Else
                    MyBase.Value = DBUtils.ToDBDateStr(value)
                End If
            End Set
        End Property

        <XmlIgnore> _
        Public Shadows Property Value1 As Date?
            Get
                If TypeOf (MyBase.Value1) Is DBNull Then
                    Return Nothing
                Else
                    Return DBUtils.FromDBDateStr(MyBase.Value1)
                End If
            End Get
            Set(value As Date?)
                If value Is Nothing Then
                    MyBase.Value1 = DBNull.Value
                Else
                    MyBase.Value1 = DBUtils.ToDBDateStr(value)
                End If
            End Set
        End Property

        Public Shadows Sub Between(ByVal value1 As Date?, ByVal value2 As Date?)
            MyBase.Between(DBUtils.ToDBDateStr(value1), DBUtils.ToDBDateStr(value2))
        End Sub

        Public Overrides Function GetSQL() As String
            Return MyBase.GetSQL()
        End Function

        Public Overrides Function GetSQL(nomeCampoOverride As String) As String
            Return MyBase.GetSQL(nomeCampoOverride)
        End Function

    End Class




End Class

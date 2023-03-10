Imports System.Xml.Serialization

Public partial class Databases

    Public MustInherit Class DBObjectCursor(Of T As DBObject)
        Inherits DBObjectCursor

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return Activator.CreateInstance(GetType(T))
        End Function

        <XmlIgnore> _
        Public Shadows Property Item As T
            Get
                Return MyBase.Item
            End Get
            Set(value As T)
                MyBase.Item = value
            End Set
        End Property

    End Class


End Class

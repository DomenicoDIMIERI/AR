Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    Public Class EMailMessagesCursor
        Inherits CCustomerCallsCursor

        Public Sub New()
            MyBase.ClassName.Value = "EMailMessage"
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CEMailMessage
        End Function

        Public Shadows Property Item As CEMailMessage
            Get
                Return MyBase.Item
            End Get
            Set(value As CEMailMessage)
                MyBase.Item = value
            End Set
        End Property
    End Class



End Class
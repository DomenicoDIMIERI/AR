Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Cursore sulla tabella degli appunti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CAppuntiCursor
        Inherits CCustomerCallsCursor

        Public Sub New()
            MyBase.ClassName.Value = "CAppunto"
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAppunto
        End Function

        Public Shadows Property Item As CAppunto
            Get
                Return MyBase.Item
            End Get
            Set(value As CAppunto)
                MyBase.Item = value
            End Set
        End Property

    End Class


End Class


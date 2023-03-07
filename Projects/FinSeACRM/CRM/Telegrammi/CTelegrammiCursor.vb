Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Cursore sulla tabella dei telegrammi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTelegrammiCursor
        Inherits CCustomerCallsCursor

    
        Public Sub New()
            MyBase.ClassName.Value = "CTelegramma"
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTelegramma
        End Function


        Public Shadows Property Item As CTelegramma
            Get
                Return MyBase.Item
            End Get
            Set(value As CTelegramma)
                MyBase.Item = value
            End Set
        End Property

         

    End Class


End Class


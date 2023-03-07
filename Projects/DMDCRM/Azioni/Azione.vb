Imports FinSeA.Databases

Partial Public Class Anagrafica1

    ''' <summary>
    ''' Rappresenta un'azione svolta dall'utente
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Azione
        Inherits DBObject

        Public Sub New()
        End Sub



        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Anagrafica1.Azioni.Module
        End Function

        Protected Overrides Function GetTableName() As String
            Return "tbl_ANAAction"
        End Function
    End Class

End Class
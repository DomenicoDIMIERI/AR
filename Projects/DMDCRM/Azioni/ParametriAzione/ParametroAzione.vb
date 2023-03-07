Imports FinSeA.Databases

Partial Public Class Anagrafica1

    ''' <summary>
    ''' Rappresenta un parametro per un'azione
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ParametroAzione
        Inherits DBObject

        Private m_Nome As String
        Private m_TipoValore As System.TypeCode
        Private m_ValoriAmmessi As CCollection

        Public Sub New()
        End Sub



        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Nothing
        End Function

        Protected Overrides Function GetTableName() As String
            Return "tbl_ANAActPA"
        End Function
    End Class

End Class
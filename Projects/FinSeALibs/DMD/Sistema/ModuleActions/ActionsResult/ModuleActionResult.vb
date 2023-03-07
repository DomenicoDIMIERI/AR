Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Racchiude informazioni sullo stato dell'esecuzione di un'azione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class ModuleActionResult
        Inherits DBObjectBase


        Public Sub New()
        End Sub


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing 'TODO
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SYSModActRes"
        End Function
    End Class


End Class
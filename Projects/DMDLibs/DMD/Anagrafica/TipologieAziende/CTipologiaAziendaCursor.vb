Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
  
    Public Class CTipologiaAziendaCursor
        Inherits DBObjectCursor(Of CTipologiaAzienda)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_RichiedeValutazione As New CCursorFieldObj(Of String)("RichVal")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property RichiedeValutazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_RichiedeValutazione
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.TipologieAzienda.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TipologieAzienda"
        End Function
    End Class
     

End Class
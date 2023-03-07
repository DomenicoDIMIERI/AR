Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
      
  
    Public Class CFormeGiuridicheAziendaCursor
        Inherits DBObjectCursor(Of CFormaGiuridicaAzienda)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.FormeGiuridicheAzienda.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FormeGiuridicheAzienda"
        End Function
    End Class
 


End Class
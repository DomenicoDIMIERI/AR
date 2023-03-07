Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
 
Partial Class WebSite

  
    Public Class CIPInfoCursor
        Inherits DBObjectCursorBase(Of CIPInfo)

        Private m_IP As New CCursorFieldObj(Of String)("IP")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")

        Public Sub New()
        End Sub

        Public ReadOnly Property IP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IP
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return LOGConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return IPInfo.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_IPInfo"
        End Function
    End Class


End Class

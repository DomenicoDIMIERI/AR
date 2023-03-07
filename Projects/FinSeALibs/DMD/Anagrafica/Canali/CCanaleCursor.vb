Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
 
    ''' <summary>
    ''' Cursore sulla tabella dei canali
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCanaleCursor
        Inherits DBObjectCursor(Of CCanale)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Valid As New CCursorField(Of Boolean)("Valid")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property Valid As CCursorField(Of Boolean)
            Get
                Return Me.m_Valid
            End Get
        End Property


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Canali.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ANACanali"
        End Function

    End Class


End Class
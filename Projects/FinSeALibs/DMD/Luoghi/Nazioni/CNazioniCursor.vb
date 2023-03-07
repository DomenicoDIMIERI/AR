Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public Class CNazioniCursor
        Inherits LuogoCursorISTAT(Of CNazione)

        Private m_NumeroAbitanti As New CCursorField(Of Integer)("NumeroAbitanti")
        Private m_NomeAbitanti As New CCursorFieldObj(Of String)("NomeAbitanti")
        Private m_SantoPatrono As New CCursorFieldObj(Of String)("SantoPatrono")
        Private m_GiornoFestivo As New CCursorFieldObj(Of String)("GiornoFestivo")
        Private m_Prefisso As New CCursorFieldObj(Of String)("Prefisso")
        Private m_Sigla As New CCursorFieldObj(Of String)("Sigla")

        Public Sub New()
        End Sub

        Public ReadOnly Property NumeroAbitanti As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroAbitanti
            End Get
        End Property

        Public ReadOnly Property NomeAbitanti As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAbitanti
            End Get
        End Property

        Public ReadOnly Property SantoPatrono As CCursorFieldObj(Of String)
            Get
                Return Me.m_SantoPatrono
            End Get
        End Property

        Public ReadOnly Property GiornoFestivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_GiornoFestivo
            End Get
        End Property

        Public ReadOnly Property Prefisso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Prefisso
            End Get
        End Property

        Public ReadOnly Property Sigla As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sigla
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Luoghi.Nazioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Nazioni"
        End Function
    End Class


End Class
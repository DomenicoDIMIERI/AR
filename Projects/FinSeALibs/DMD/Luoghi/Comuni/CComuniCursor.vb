Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public Class CComuniCursor
        Inherits LuogoCursorISTAT(Of CComune)

        Private m_NumeroAbitanti As CCursorField(Of Integer)
        Private m_NomeAbitanti As CCursorFieldObj(Of String)
        Private m_SantoPatrono As CCursorFieldObj(Of String)
        Private m_GiornoFestivo As CCursorFieldObj(Of String)
        Private m_CAP As CCursorFieldObj(Of String)
        Private m_Prefisso As CCursorFieldObj(Of String)
        Private m_Provincia As CCursorFieldObj(Of String)
        Private m_Sigla As CCursorFieldObj(Of String)
        Private m_Regione As CCursorFieldObj(Of String)

        Public Sub New()
            Me.m_NumeroAbitanti = New CCursorField(Of Integer)("NumeroAbitanti")
            Me.m_NomeAbitanti = New CCursorFieldObj(Of String)("NomeAbitanti")
            Me.m_SantoPatrono = New CCursorFieldObj(Of String)("SantoPatrono")
            Me.m_GiornoFestivo = New CCursorFieldObj(Of String)("GiornoFestivo")
            Me.m_CAP = New CCursorFieldObj(Of String)("CAP")
            Me.m_Prefisso = New CCursorFieldObj(Of String)("Prefisso")
            Me.m_Provincia = New CCursorFieldObj(Of String)("Provincia")
            Me.m_Sigla = New CCursorFieldObj(Of String)("Sigla")
            Me.m_Regione = New CCursorFieldObj(Of String)("Regione")
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

        Public ReadOnly Property CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_CAP
            End Get
        End Property

        Public ReadOnly Property Prefisso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Prefisso
            End Get
        End Property

        Public ReadOnly Property Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_Provincia
            End Get
        End Property

        Public ReadOnly Property Sigla As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sigla
            End Get
        End Property

        Public ReadOnly Property Regione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Regione
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CComune
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Comuni"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Luoghi.Comuni.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class


End Class
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CAssicurazioniCursor
        Inherits DBObjectCursor(Of CAssicurazione)


        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_MeseScattoEta As New CCursorField(Of Integer)("MeseScattoEta")
        Private m_MeseScattoAnzianita As New CCursorField(Of Integer)("MeseScattoAnzianita")

        Public Sub New()
        End Sub


        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property MeseScattoEta As CCursorField(Of Integer)
            Get
                Return Me.m_MeseScattoEta
            End Get
        End Property

        Public ReadOnly Property MeseScattoAnzianita As CCursorField(Of Integer)
            Get
                Return Me.m_MeseScattoAnzianita
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.Assicurazioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Assicurazioni"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAssicurazione
        End Function

    End Class

End Class
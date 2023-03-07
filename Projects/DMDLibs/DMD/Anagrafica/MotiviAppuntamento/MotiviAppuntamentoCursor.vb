Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public Class MotiviAppuntamentoCursor
        Inherits DBObjectCursor(Of MotivoAppuntamento)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Flags As New CCursorField(Of MotivoAppuntamentoFlags)("Flags")
        Private m_TipoAppuntamento As New CCursorFieldObj(Of String)("TipoAppuntamento")

        Public Sub New()
        End Sub

        Public ReadOnly Property TipoAppuntamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoAppuntamento
            End Get
        End Property


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

        Public ReadOnly Property Flags As CCursorField(Of MotivoAppuntamentoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMMotiviAppuntamento"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.MotiviAppuntamento.Module
        End Function


        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

    End Class


End Class
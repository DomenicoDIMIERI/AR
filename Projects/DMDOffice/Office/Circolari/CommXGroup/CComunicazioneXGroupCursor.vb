Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Office


    Public Class CComunicazioneXGroupCursor
        Inherits DBObjectCursorBase(Of CComunicazioneXGroup)

        Private m_IDComunicazione As New CCursorField(Of Integer)("Comunicazione")
        Private m_IDGruppo As New CCursorField(Of Integer)("Gruppo")
        Private m_Allow As New CCursorField(Of Boolean)("Allow")
        Private m_Negate As New CCursorField(Of Boolean)("Negate")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDGruppo As CCursorField(Of Integer)
            Get
                Return Me.m_IDGruppo
            End Get
        End Property

        Public ReadOnly Property IDComunicazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDComunicazione
            End Get
        End Property

        Public ReadOnly Property Allow As CCursorField(Of Boolean)
            Get
                Return Me.m_Allow
            End Get
        End Property

        Public ReadOnly Property Negate As CCursorField(Of Boolean)
            Get
                Return Me.m_Negate
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Comunicazioni.Module
        End Function

        'Public Function GetLink() As String
        '    Return WebSite.Configuration.URL & "/?_m=" & GetID(Comunicazioni.Module) & "&_a=get&ID=" & GetID(Me)
        'End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ComunicazioniXGruppo"
        End Function

        Protected Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function



    End Class

End Class




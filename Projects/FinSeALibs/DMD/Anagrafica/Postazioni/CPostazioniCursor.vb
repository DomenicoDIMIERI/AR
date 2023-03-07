Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Cursore sulla tabella delle postazioni di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CPostazioniCursor
        Inherits DBObjectCursorPO(Of CPostazione)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Flags As New CCursorField(Of FlagsPostazioneLavoro)("Flags")
        Private m_IDUtentePrincipale As New CCursorField(Of Integer)("IDUtentePrincipale")
        Private m_NomeUtentePrincipale As New CCursorFieldObj(Of String)("NomeUtentePrincipale")
        Private m_NomeReparto As New CCursorFieldObj(Of String)("NomeReparto")
        Private m_InternoTelefonico As New CCursorFieldObj(Of String)("InternoTelefonico")
        Private m_SistemaOperativo As New CCursorFieldObj(Of String)("SistemaOperativo")
        Private m_Note As New CCursorFieldObj(Of String)("Note")


        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of FlagsPostazioneLavoro)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDUtentePrincipale As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtentePrincipale
            End Get
        End Property

        Public ReadOnly Property NomeUtentePrincipale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUtentePrincipale
            End Get
        End Property

        Public ReadOnly Property NomeReparto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeReparto
            End Get
        End Property

        Public ReadOnly Property InternoTelefonico As CCursorFieldObj(Of String)
            Get
                Return Me.m_InternoTelefonico
            End Get
        End Property

        Public ReadOnly Property SistemaOperativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_SistemaOperativo
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CPostazione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PostazioniLavoro"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Postazioni.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function



    End Class


End Class
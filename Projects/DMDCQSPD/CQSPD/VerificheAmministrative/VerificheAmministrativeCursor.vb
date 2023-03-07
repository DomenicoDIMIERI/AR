Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable>
    Public Class VerificheAmministrativeCursor
        Inherits DBObjectCursor(Of VerificaAmministrativa)

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_StatoVerifica As New CCursorField(Of StatoVerificaAmministrativa)("StatoVerifica")
        Private m_EsitoVerifica As New CCursorField(Of EsitoVerificaAmministrativa)("EsitoVerifica")
        Private m_DettaglioEsitoVerifica As New CCursorFieldObj(Of String)("DettaglioEsitoVerifica")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDOggettoVerificato As New CCursorField(Of Integer)("IDOggettoVerificato")
        Private m_TipoOggettoVerificato As New CCursorFieldObj(Of String)("TipoOggettoVerificato")


        Public Sub New()
        End Sub

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property StatoVerifica As CCursorField(Of StatoVerificaAmministrativa)
            Get
                Return Me.m_StatoVerifica
            End Get
        End Property

        Public ReadOnly Property EsitoVerifica As CCursorField(Of EsitoVerificaAmministrativa)
            Get
                Return Me.m_EsitoVerifica
            End Get
        End Property

        Public ReadOnly Property DettaglioEsitoVerifica As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsitoVerifica
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDOggettoVerificato As CCursorField(Of Integer)
            Get
                Return Me.m_IDOggettoVerificato
            End Get
        End Property

        Public ReadOnly Property TipoOggettoVerificato As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoOggettoVerificato
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.VerificheAmministrative.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDVerificheAmministrative"
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class

   
End Class

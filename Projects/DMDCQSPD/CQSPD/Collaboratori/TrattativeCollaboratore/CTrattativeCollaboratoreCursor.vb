Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Serializable>
    Public Class CTrattativeCollaboratoreCursor
        Inherits DBObjectCursor(Of CTrattativaCollaboratore)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Richiesto As New CCursorField(Of Boolean)("Richiesto")
        Private m_TipoCalcolo As New CCursorField(Of CQSPDTipoProvvigioneEnum)("TipoCalcolo")
        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")
        Private m_NomeCollaboratore As New CCursorFieldObj(Of String)("NomeCollaboratore")
        Private m_IDCessionario As New CCursorField(Of Integer)("IDCessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_IDProdotto As New CCursorField(Of Integer)("IDProdotto")
        Private m_NomeProdotto As New CCursorFieldObj(Of String)("NomeProdotto")
        Private m_StatoTrattativa As New CCursorField(Of StatoTrattativa)("StatoTrattativa")
        Private m_SpreadProposto As New CCursorField(Of Double)("SpreadProposto")
        Private m_SpreadRichiesto As New CCursorField(Of Double)("SpreadRichiesto")
        Private m_SpreadApprovato As New CCursorField(Of Double)("SpreadApprovato")
        Private m_SpreadFidelizzazione As New CCursorField(Of Double)("SpreadFidelizzazione")
        Private m_Flags As New CCursorField(Of TrattativaCollaboratoreFlags)("Flags")
        Private m_ValoreBase As New CCursorField(Of Double)("ValoreBase")
        Private m_ValoreMax As New CCursorField(Of Double)("ValoreMax")
        Private m_Formula As New CCursorFieldObj(Of String)("Formula")
        Private m_IDPropostoDa As New CCursorField(Of Integer)("IDPropostoDa")
        Private m_PropostoIl As New CCursorField(Of Date)("PropostoIl")
        Private m_IDApprovatoDa As New CCursorField(Of Integer)("IDApprovatoDa")
        Private m_ApprovatoIl As New CCursorField(Of Date)("ApprovatoIl")
        Private m_Note As New CCursorFieldObj(Of String)("Note")


        Public Sub New()
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.TrattativeCollaboratore.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDCollabBUI"
        End Function

        Public ReadOnly Property TipoCalcolo As CCursorField(Of CQSPDTipoProvvigioneEnum)
            Get
                Return Me.m_TipoCalcolo
            End Get
        End Property

        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Public ReadOnly Property Richiesto As CCursorField(Of Boolean)
            Get
                Return Me.m_Richiesto
            End Get
        End Property

        Public ReadOnly Property NomeCollaboratore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCollaboratore
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_IDCessionario
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property IDProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_IDProdotto
            End Get
        End Property

        Public ReadOnly Property NomeProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProdotto
            End Get
        End Property

        Public ReadOnly Property StatoTrattativa As CCursorField(Of StatoTrattativa)
            Get
                Return Me.m_StatoTrattativa
            End Get
        End Property

        Public ReadOnly Property SpreadProposto As CCursorField(Of Double)
            Get
                Return Me.m_SpreadProposto
            End Get
        End Property

        Public ReadOnly Property SpreadRichiesto As CCursorField(Of Double)
            Get
                Return Me.m_SpreadRichiesto
            End Get
        End Property

        Public ReadOnly Property SpreadApprovato As CCursorField(Of Double)
            Get
                Return Me.m_SpreadApprovato
            End Get
        End Property

        Public ReadOnly Property SpreadFidelizzazione As CCursorField(Of Double)
            Get
                Return Me.m_SpreadFidelizzazione
            End Get

        End Property
        Public ReadOnly Property Flags As CCursorField(Of TrattativaCollaboratoreFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property ValoreBase As CCursorField(Of Double)
            Get
                Return Me.m_ValoreBase
            End Get
        End Property

        Public ReadOnly Property ValoreMax As CCursorField(Of Double)
            Get
                Return Me.m_ValoreMax
            End Get
        End Property

        Public ReadOnly Property Formula As CCursorFieldObj(Of String)
            Get
                Return Me.m_Formula
            End Get
        End Property

        Public ReadOnly Property IDPropostoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDPropostoDa
            End Get
        End Property

        Public ReadOnly Property PropostoIl As CCursorField(Of Date)
            Get
                Return Me.m_PropostoIl
            End Get
        End Property

        Public ReadOnly Property IDApprovatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDApprovatoDa
            End Get
        End Property

        Public ReadOnly Property ApprovatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ApprovatoIl
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class

   
End Class

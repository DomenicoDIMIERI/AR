Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Cursore sulla tabella delle voci di manutenzione
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VociManutenzioneCursor
        Inherits DBObjectCursorPO(Of VoceManutenzione)

        Private m_IDManutenzione As New CCursorField(Of Integer)("IDManutenzione")
        Private m_Categoria1 As New CCursorFieldObj(Of String)("Categoria1")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IDOggettoRimosso As New CCursorField(Of Integer)("IDOggettoRimosso")
        Private m_NomeOggettoRimosso As New CCursorFieldObj(Of String)("NomeOggettoRimosso")
        Private m_IDOggetto As New CCursorField(Of Integer)("IDOggetto")
        Private m_NomeOggetto As New CCursorFieldObj(Of String)("NomeOggetto")
        Private m_ValoreImponibile As New CCursorField(Of Decimal)("ValoreImponibile")
        Private m_ValoreIvato As New CCursorField(Of Decimal)("ValoreIvato")
        Private m_Azione As New CCursorField(Of AzioneManutenzione)("Azione")
        Private m_Flags As New CCursorField(Of Integer)("Flags")


        Public Sub New()
        End Sub

        Public ReadOnly Property Azione As CCursorField(Of AzioneManutenzione)
            Get
                Return Me.m_Azione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDManutenzione As CCursorField(Of Integer)
            Get
                Return Me.m_IDManutenzione
            End Get
        End Property

        Public ReadOnly Property Categoria1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria1
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IDOggettoRimosso As CCursorField(Of Integer)
            Get
                Return Me.m_IDOggettoRimosso
            End Get
        End Property

        Public ReadOnly Property NomeOggettoRimosso As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOggettoRimosso
            End Get
        End Property

        Public ReadOnly Property IDOggetto As CCursorField(Of Integer)
            Get
                Return Me.m_IDOggetto
            End Get
        End Property

        Public ReadOnly Property NomeOggetto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOggetto
            End Get
        End Property

        Public ReadOnly Property ValoreImponibile As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreImponibile
            End Get
        End Property

        Public ReadOnly Property ValoreIvato As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreIvato
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New VoceManutenzione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ManutenzioniVoci"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Manutenzioni.Voci.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function



    End Class


End Class
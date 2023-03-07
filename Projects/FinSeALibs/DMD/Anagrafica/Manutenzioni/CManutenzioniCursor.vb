Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Cursore sulla tabella delle postazioni di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CManutenzioniCursor
        Inherits DBObjectCursorPO(Of CManutenzione)

        Private m_IDPostazione As New CCursorField(Of Integer)("IDPostazione")
        Private m_NomePostazione As New CCursorFieldObj(Of String)("NomePostazione")
        Private m_DataInizioIntervento As New CCursorField(Of DateTime)("DataInizioIntervento")
        Private m_DataFineIntervento As New CCursorField(Of DateTime)("DataFineIntervento")
        Private m_ValoreImponibile As New CCursorField(Of Decimal)("ValoreImponibile")
        Private m_ValoreIvato As New CCursorField(Of Decimal)("ValoreIvato")
        Private m_CostoSpedizione As New CCursorField(Of Decimal)("CostoSpedizione")
        Private m_AltreSpese As New CCursorField(Of Decimal)("AltreSpese")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Categoria1 As New CCursorFieldObj(Of String)("Categoria1")
        Private m_Categoria2 As New CCursorFieldObj(Of String)("Categoria2")
        Private m_Categoria3 As New CCursorFieldObj(Of String)("Categoria3")
        Private m_Categoria4 As New CCursorFieldObj(Of String)("Categoria4")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_IDAziendaFornitrice As New CCursorField(Of Integer)("IDAziendaFornitrice")
        Private m_NomeAziendaFornitrice As New CCursorFieldObj(Of String)("NomeAziendaFornitrice")
        Private m_IDRegistrataDa As New CCursorField(Of Integer)("IDRegistrataDa")
        Private m_NomeRegistrataDa As New CCursorFieldObj(Of String)("NomeRegistrataDa")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDDocumento As New CCursorField(Of Integer)("IDDocumento")
        Private m_NumeroDocumento As New CCursorFieldObj(Of String)("NumeroDocumento")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDPostazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDPostazione
            End Get
        End Property

        Public ReadOnly Property NomePostazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePostazione
            End Get
        End Property

        Public ReadOnly Property DataInizioIntervento As CCursorField(Of DateTime)
            Get
                Return Me.m_DataInizioIntervento
            End Get
        End Property

        Public ReadOnly Property DataFineIntervento As CCursorField(Of DateTime)
            Get
                Return Me.m_DataFineIntervento
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

        Public ReadOnly Property CostoSpedizione As CCursorField(Of Decimal)
            Get
                Return Me.m_CostoSpedizione
            End Get
        End Property

        Public ReadOnly Property AltreSpese As CCursorField(Of Decimal)
            Get
                Return Me.m_AltreSpese
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Categoria1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria1
            End Get
        End Property

        Public ReadOnly Property Categoria2 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria2
            End Get
        End Property

        Public ReadOnly Property Categoria3 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria3
            End Get
        End Property

        Public ReadOnly Property Categoria4 As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria4
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property IDAziendaFornitrice As CCursorField(Of Integer)
            Get
                Return Me.m_IDAziendaFornitrice
            End Get
        End Property

        Public ReadOnly Property NomeAziendaFornitrice As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAziendaFornitrice
            End Get
        End Property

        Public ReadOnly Property IDRegistrataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDRegistrataDa
            End Get
        End Property

        Public ReadOnly Property NomeRegistrataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRegistrataDa
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDDocumento As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumento
            End Get
        End Property

        Public ReadOnly Property NumeroDocumento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroDocumento
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CManutenzione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Manutenzioni"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Manutenzioni.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function



    End Class


End Class
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     



    ''' <summary>
    ''' Cursore sulla tabella degli impiegati
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CImpiegatiCursor
        Inherits DBObjectCursor(Of CImpiegato)

        Private m_PersonaID As CCursorField(Of Integer)
        Private m_NomePersona As CCursorFieldObj(Of String)
        Private m_AziendaID As CCursorField(Of Integer)
        Private m_NomeAzienda As CCursorFieldObj(Of String)
        Private m_IDSede As CCursorField(Of Integer)
        Private m_NomeSede As CCursorFieldObj(Of String)
        Private m_Posizione As CCursorFieldObj(Of String)
        Private m_Ufficio As CCursorFieldObj(Of String)
        Private m_DataAssunzione As CCursorField(Of Date)
        Private m_DataLicenziamento As CCursorField(Of Date)
        Private m_StipendioNetto As CCursorField(Of Decimal)
        Private m_StipendioLordo As CCursorField(Of Decimal)
        Private m_TFR As CCursorField(Of Decimal)
        Private m_MensilitaPercepite As CCursorField(Of Integer)
        Private m_PercTFRAzienda As CCursorField(Of Double)
        Private m_NomeFPC As CCursorFieldObj(Of String)
        Private m_TipoContratto As CCursorFieldObj(Of String)
        Private m_TipoRapporto As CCursorFieldObj(Of String)
        Private m_Flags As CCursorField(Of ImpiegoFlags)

        Public Sub New()
            Me.m_PersonaID = New CCursorField(Of Integer)("Persona")
            Me.m_NomePersona = New CCursorFieldObj(Of String)("NomePersona")
            Me.m_AziendaID = New CCursorField(Of Integer)("Azienda")
            Me.m_NomeAzienda = New CCursorFieldObj(Of String)("NomeAzienda")
            Me.m_IDSede = New CCursorField(Of Integer)("IDSede")
            Me.m_NomeSede = New CCursorFieldObj(Of String)("NomeSede")
            Me.m_Posizione = New CCursorFieldObj(Of String)("Posizione")
            Me.m_Ufficio = New CCursorFieldObj(Of String)("Ufficio")
            Me.m_DataAssunzione = New CCursorField(Of Date)("DataAssunzione")
            Me.m_DataLicenziamento = New CCursorField(Of Date)("DataLicenziamento")
            Me.m_StipendioNetto = New CCursorField(Of Decimal)("StipendioNetto")
            Me.m_StipendioLordo = New CCursorField(Of Decimal)("StipendioLordo")
            Me.m_TFR = New CCursorField(Of Decimal)("TFR")
            Me.m_MensilitaPercepite = New CCursorField(Of Integer)("MensilitaPercepite")
            Me.m_PercTFRAzienda = New CCursorField(Of Double)("PercTFRAzienda")
            Me.m_NomeFPC = New CCursorFieldObj(Of String)("NomeFPC")
            Me.m_TipoContratto = New CCursorFieldObj(Of String)("TipoContratto")
            Me.m_TipoRapporto = New CCursorFieldObj(Of String)("TipoRapporto")
            Me.m_Flags = New CCursorField(Of ImpiegoFlags)("Flags")
        End Sub

        Public ReadOnly Property PersonaID As CCursorField(Of Integer)
            Get
                Return Me.m_PersonaID
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ImpiegoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property AziendaID As CCursorField(Of Integer)
            Get
                Return Me.m_AziendaID
            End Get
        End Property

        Public ReadOnly Property NomeAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAzienda
            End Get
        End Property

        Public ReadOnly Property IDSede As CCursorField(Of Integer)
            Get
                Return Me.m_IDSede
            End Get
        End Property

        Public ReadOnly Property NomeSede As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeSede
            End Get
        End Property

        Public ReadOnly Property Posizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Posizione
            End Get
        End Property

        Public ReadOnly Property Ufficio As CCursorFieldObj(Of String)
            Get
                Return Me.m_Ufficio
            End Get
        End Property

        Public ReadOnly Property DataAssunzione As CCursorField(Of Date)
            Get
                Return Me.m_DataAssunzione
            End Get
        End Property

        Public ReadOnly Property DataLicenziamento As CCursorField(Of Date)
            Get
                Return Me.m_DataLicenziamento
            End Get
        End Property

        Public ReadOnly Property StipendioNetto As CCursorField(Of Decimal)
            Get
                Return Me.m_StipendioNetto
            End Get
        End Property

        Public ReadOnly Property StipendioLordo As CCursorField(Of Decimal)
            Get
                Return Me.m_StipendioLordo
            End Get
        End Property

        Public ReadOnly Property TFR As CCursorField(Of Decimal)
            Get
                Return Me.m_TFR
            End Get
        End Property

        Public ReadOnly Property MensilitaPercepite As CCursorField(Of Integer)
            Get
                Return Me.m_MensilitaPercepite
            End Get
        End Property

        Public ReadOnly Property PercTFRAzienda As CCursorField(Of Double)
            Get
                Return Me.m_PercTFRAzienda
            End Get
        End Property

        Public ReadOnly Property NomeFPC As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFPC
            End Get
        End Property

        Public ReadOnly Property TipoContratto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContratto
            End Get
        End Property

        Public ReadOnly Property TipoRapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoRapporto
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CImpiegato
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Impiegati"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

    End Class
     

End Class
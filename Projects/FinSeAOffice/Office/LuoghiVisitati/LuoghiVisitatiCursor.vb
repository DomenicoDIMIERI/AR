Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella dei luoghi attraversati dall'utente per le commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LuoghiVisitatiCursor
        Inherits DBObjectCursor(Of LuogoVisitato)

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_IDUscita As New CCursorField(Of Integer)("IDUscita")
        Private m_IndirizzoEtichetta As New CCursorFieldObj(Of String)("Indirizzo_Etichetta")
        Private m_IndirizzoVia As New CCursorFieldObj(Of String)("Indirizzo_Via")
        Private m_IndirizzoCitta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        Private m_IndirizzoProvincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        Private m_IndirizzoCAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        Private m_OraArrivo As New CCursorField(Of Date)("OraArrivo")
        Private m_OraPartenza As New CCursorField(Of Date)("OraPartenza")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Lat As New CCursorField(Of Double)("Lat")
        Private m_Lng As New CCursorField(Of Double)("Lng")
        Private m_Alt As New CCursorField(Of Double)("Alt")
        Private m_IDLuogo As New CCursorField(Of Integer)("IDLuogo")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_TipoMateriale As New CCursorFieldObj(Of String)("TipoMateriale")
        Private m_ConsegnatiAMano As New CCursorField(Of Integer)("ConsegnatiAMano")
        Private m_ConsegnatiPostale As New CCursorField(Of Integer)("ConsegnatiPostale")
        Private m_ConsegnatiAuto As New CCursorField(Of Integer)("ConsegnatiAuto")
        Private m_ConsegnatiAltro As New CCursorField(Of Integer)("ConsegnatiAltro")
        Private m_Progressivo As New CCursorField(Of Integer)("Progressivo")

        Public ReadOnly Property Progressivo As CCursorField(Of Integer)
            Get
                Return Me.m_Progressivo
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property IDUscita As CCursorField(Of Integer)
            Get
                Return Me.m_IDUscita
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Etichetta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoEtichetta
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoVia
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCitta
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoProvincia
            End Get
        End Property

        Public ReadOnly Property Indirizzo_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCAP
            End Get
        End Property

        Public ReadOnly Property OraArrivo As CCursorField(Of Date)
            Get
                Return Me.m_OraArrivo
            End Get
        End Property

        Public ReadOnly Property OraPartenza As CCursorField(Of Date)
            Get
                Return Me.m_OraPartenza
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.LuoghiVisitati.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeLuoghiV"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public ReadOnly Property Lat As CCursorField(Of Double)
            Get
                Return Me.m_Lat
            End Get
        End Property

        Public ReadOnly Property Lng As CCursorField(Of Double)
            Get
                Return Me.m_Lng
            End Get
        End Property

        Public ReadOnly Property Alt As CCursorField(Of Double)
            Get
                Return Me.m_Alt
            End Get
        End Property

        Public ReadOnly Property IDLuogo As CCursorField(Of Integer)
            Get
                Return Me.m_IDLuogo
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property TipoMateriale As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoMateriale
            End Get
        End Property

        Public ReadOnly Property ConsegnatiAMano As CCursorField(Of Integer)
            Get
                Return Me.m_ConsegnatiAMano
            End Get
        End Property

        Public ReadOnly Property ConsegnatiPostale As CCursorField(Of Integer)
            Get
                Return Me.m_ConsegnatiPostale
            End Get
        End Property

        Public ReadOnly Property ConsegnatiAuto As CCursorField(Of Integer)
            Get
                Return Me.m_ConsegnatiAuto
            End Get
        End Property

        Public ReadOnly Property ConsegnatiAltro As CCursorField(Of Integer)
            Get
                Return Me.m_ConsegnatiAltro
            End Get
        End Property

    End Class



End Class
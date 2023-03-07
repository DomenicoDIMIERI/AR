Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CAltriPreventiviCursor
        Inherits DBObjectCursor(Of CAltroPreventivo)

        Private m_IDRichiestaDiFinanziamento As New CCursorField(Of Integer)("IDRichiestaDiFinanziamento")
        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_DataAccettazione As New CCursorField(Of Date)("DataAccettazione")
        Private m_IDIstituto As New CCursorField(Of Integer)("IDIstituto")
        Private m_NomeIstituto As New CCursorFieldObj(Of String)("NomeIstituto")
        Private m_IDAgenzia As New CCursorField(Of Integer)("IDAgenzia")
        Private m_NomeAgenzia As New CCursorFieldObj(Of String)("NomeAgenzia")
        Private m_IDAgente As New CCursorField(Of Integer)("IDAgente")
        Private m_NomeAgente As New CCursorFieldObj(Of String)("NomeAgente")
        Private m_Rata As New CCursorField(Of Decimal)("Rata")
        Private m_Durata As New CCursorField(Of Integer)("Durata")
        Private m_MontanteLordo As New CCursorField(Of Decimal)("MontanteLordo")
        Private m_NettoRicavo As New CCursorField(Of Decimal)("NettoRicavo")
        Private m_TAN As New CCursorField(Of Double)("TAN")
        Private m_TAEG As New CCursorField(Of Double)("TAEG")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_NettoAllaMano As New CCursorField(Of Decimal)("NettoAllaMano")
        Private m_Flags As New CCursorField(Of Integer)("Flags")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDRichiestaDiFinanziamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaDiFinanziamento
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property DataAccettazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAccettazione
            End Get
        End Property

        Public ReadOnly Property IDIstituto As CCursorField(Of Integer)
            Get
                Return Me.m_IDIstituto
            End Get
        End Property

        Public ReadOnly Property NomeIstituto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeIstituto
            End Get
        End Property

        Public ReadOnly Property IDAgenzia As CCursorField(Of Integer)
            Get
                Return Me.m_IDAgenzia
            End Get
        End Property

        Public ReadOnly Property NomeAgenzia As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgenzia
            End Get
        End Property

        Public ReadOnly Property IDAgente As CCursorField(Of Integer)
            Get
                Return Me.m_IDAgente
            End Get
        End Property

        Public ReadOnly Property NomeAgente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgente
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Integer)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property MontanteLordo As CCursorField(Of Decimal)
            Get
                Return Me.m_MontanteLordo
            End Get
        End Property

        Public ReadOnly Property NettoRicavo As CCursorField(Of Decimal)
            Get
                Return Me.m_NettoRicavo
            End Get
        End Property

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
            End Get
        End Property

        Public ReadOnly Property TAEG As CCursorField(Of Double)
            Get
                Return Me.m_TAEG
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property NettoAllaMano As CCursorField(Of Decimal)
            Get
                Return Me.m_NettoAllaMano
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteAltriPrev"
        End Function
    End Class


End Class

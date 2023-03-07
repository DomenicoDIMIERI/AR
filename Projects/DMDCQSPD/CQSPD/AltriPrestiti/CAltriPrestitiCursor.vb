Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    Public Class CAltriPrestitiCursor
        Inherits DBObjectCursor(Of CAltroPrestito)

        Private m_Tipo As New CCursorField(Of TipoEstinzione)("Tipo")
        Private m_IDIstituto As New CCursorField(Of Integer)("IDIstituto")
        Private m_NomeIstituto As New CCursorFieldObj(Of String)("NomeIstituto")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_Scadenza As New CCursorField(Of Date)("Scadenza")
        Private m_Rata As New CCursorField(Of Decimal)("Rata")
        Private m_Durata As New CCursorField(Of Integer)("Durata")
        Private m_TAN As New CCursorField(Of Double)("TAN")
        Private m_IDPratica As New CCursorField(Of Integer)("IDPratica")
        Private m_DecorrenzaPratica As New CCursorField(Of Date)("DecorrenzaPratica")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property DecorrenzaPratica As CCursorField(Of Date)
            Get
                Return Me.m_DecorrenzaPratica
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorField(Of TipoEstinzione)
            Get
                Return Me.m_Tipo
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

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property Scadenza As CCursorField(Of Date)
            Get
                Return Me.m_Scadenza
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

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
            End Get
        End Property


        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CEstinzione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDAltriPrestiti"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return AltriPrestiti.Module
        End Function
    End Class


End Class
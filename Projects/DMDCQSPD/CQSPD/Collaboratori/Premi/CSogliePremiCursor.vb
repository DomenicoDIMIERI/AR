Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Serializable>
    Public Class CSogliePremiCursor
        Inherits DBObjectCursor(Of CSogliaPremio)

        Private m_SetPremiID As New CCursorField(Of Integer)("SetPremi")
        Private m_Soglia As New CCursorField(Of Decimal)("Soglia")
        Private m_Fisso As New CCursorField(Of Decimal)("Fisso")
        Private m_PercSuML As New CCursorField(Of Double)("PercSuML")
        Private m_PercSuProvvAtt As New CCursorField(Of Double)("PercSuProvvAtt")
        Private m_PercSuNetto As New CCursorField(Of Double)("PercSuNetto")

        Public Sub New()
        End Sub


        Public ReadOnly Property SetPremiID As CCursorField(Of Integer)
            Get
                Return Me.m_SetPremiID
            End Get
        End Property

        Public ReadOnly Property Soglia As CCursorField(Of Decimal)
            Get
                Return Me.m_Soglia
            End Get
        End Property

        Public ReadOnly Property Fisso As CCursorField(Of Decimal)
            Get
                Return Me.m_Fisso
            End Get
        End Property

        Public ReadOnly Property PercSuML As CCursorField(Of Double)
            Get
                Return Me.m_PercSuML
            End Get
        End Property

        Public ReadOnly Property PercSuProvvAtt As CCursorField(Of Double)
            Get
                Return Me.m_PercSuProvvAtt
            End Get
        End Property

        Public ReadOnly Property PercSuNetto As CCursorField(Of Double)
            Get
                Return Me.m_PercSuNetto
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers_SogliePremi"
        End Function
    End Class



End Class

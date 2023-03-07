Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    ''' <summary>
    ''' Cursore sulla tabella delle regole sugli stati pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CStatoPratRuleCursor
        Inherits DBObjectCursor(Of CStatoPratRule)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IDSource As New CCursorField(Of Integer)("IDSource")
        Private m_IDTarget As New CCursorField(Of Integer)("IDTarget")
        Private m_Order As New CCursorField(Of Integer)("Order")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_Flags As New CCursorField(Of FlagsRegolaStatoPratica)("Attivo")

        Public Sub New()
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CStatoPratRule
        End Function

        Public ReadOnly Property Flags As CCursorField(Of FlagsRegolaStatoPratica)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheSTR"
        End Function


        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IDSource As CCursorField(Of Integer)
            Get
                Return Me.m_IDSource
            End Get
        End Property

        Public ReadOnly Property IDTarget As CCursorField(Of Integer)
            Get
                Return Me.m_IDTarget
            End Get
        End Property

        Public ReadOnly Property Order As CCursorField(Of Integer)
            Get
                Return Me.m_Order
            End Get
        End Property

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return StatiPratRules.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class

Imports DMD.Databases

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella delle entrate/uscite di prima nota
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RigaPrimaNotaCursor
        Inherits DBObjectCursorPO(Of RigaPrimaNota)

        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_DescrizioneMovimento As New CCursorFieldObj(Of String)("DescrizioneMovimento")
        Private m_Entrate As New CCursorField(Of Decimal)("Entrate")
        Private m_Uscite As New CCursorField(Of Decimal)("Uscite")

        Public Sub New()
        End Sub

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property DescrizioneMovimento As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneMovimento
            End Get
        End Property

        Public ReadOnly Property Entrate As CCursorField(Of Decimal)
            Get
                Return Me.m_Entrate
            End Get
        End Property

        Public ReadOnly Property Uscite As CCursorField(Of Decimal)
            Get
                Return Me.m_Uscite
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As Sistema.CModule
            Return PrimaNota.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficePrimaNota"
        End Function



    End Class



End Class
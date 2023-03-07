Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office


    Public Class MarcatureIngressoUscitaCursor
        Inherits DBObjectCursorPO(Of MarcaturaIngressoUscita)

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_IDDispositivo As New CCursorField(Of Integer)("IDDispositivo")
        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_Operazione As New CCursorField(Of TipoMarcaturaIO)("Operazione")
        Private m_IDReparto As New CCursorField(Of Integer)("IDReparto")
        Private m_NomeReparto As New CCursorFieldObj(Of String)("NomeReparto")
        Private m_MetodiRiconoscimentoUsati As New CCursorField(Of MetodoRiconoscimento)("MetodiRiconoscimento")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property IDDispositivo As CCursorField(Of Integer)
            Get
                Return Me.m_IDDispositivo
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property Operazione As CCursorField(Of TipoMarcaturaIO)
            Get
                Return Me.m_Operazione
            End Get
        End Property

        Public ReadOnly Property IDReparto As CCursorField(Of Integer)
            Get
                Return Me.m_IDReparto
            End Get
        End Property

        Public ReadOnly Property NomeReparto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeReparto
            End Get
        End Property

        Public ReadOnly Property MetodiRiconoscimentoUsati As CCursorField(Of MetodoRiconoscimento)
            Get
                Return Me.m_MetodiRiconoscimentoUsati
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeUserIO"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Marcature.Module
        End Function

    End Class


End Class
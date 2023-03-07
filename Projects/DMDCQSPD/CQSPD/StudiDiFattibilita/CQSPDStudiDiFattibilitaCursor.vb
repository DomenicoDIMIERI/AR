Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle consulenze
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CQSPDStudiDiFattibilitaCursor
        Inherits DBObjectCursorPO(Of CQSPDStudioDiFattibilita)

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_OraInizio As New CCursorField(Of Date)("OraInizio")
        Private m_OraFine As New CCursorField(Of Date)("OraFine")
        Private m_IDRichiesta As New CCursorField(Of Integer)("IDRichiesta")
        Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_NomeConsulente As New CCursorFieldObj(Of String)("NomeConsulente")
        Private m_DecorrenzaPratica As New CCursorField(Of Date)("DecorrenzaPratica")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property TipoContesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
            End Get
        End Property

        Public ReadOnly Property DecorrenzaPratica As CCursorField(Of Date)
            Get
                Return Me.m_DecorrenzaPratica
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property IDRichiesta As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiesta
            End Get
        End Property

        Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulente
            End Get
        End Property

        Public ReadOnly Property NomeConsulente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConsulente
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property OraInizio As CCursorField(Of Date)
            Get
                Return Me.m_OraInizio
            End Get
        End Property

        Public ReadOnly Property OraFine As CCursorField(Of Date)
            Get
                Return Me.m_OraFine
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CQSPDStudioDiFattibilita
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDGrpConsulenze"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Consulenze.Module
        End Function


    End Class


End Class

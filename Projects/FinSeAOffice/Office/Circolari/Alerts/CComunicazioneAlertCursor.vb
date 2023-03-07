Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Office

    Public Class CComunicazioneAlertCursor
        Inherits DBObjectCursor(Of CComunicazioneAlert)

        Private m_IDComunicazione As New CCursorField(Of Integer)("IDComunicazione")
        Private m_IDUser As New CCursorField(Of Integer)("IDUser")
        Private m_Via As New CCursorFieldObj(Of String)("Via")
        Private m_Param As New CCursorFieldObj(Of String)("Param")
        Private m_StatoComunicazione As New CCursorFieldObj(Of StatoAlertComunicazione)("StatoComunicazione")
        Private m_DataConsegna As New CCursorField(Of Date)("DataConsegna")
        Private m_DataLettura As New CCursorField(Of Date)("DataLettura")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDComunicazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDComunicazione
            End Get
        End Property

        Public ReadOnly Property IDUser As CCursorField(Of Integer)
            Get
                Return Me.m_IDUser
            End Get
        End Property

        Public ReadOnly Property Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_Via
            End Get
        End Property

        Public ReadOnly Property Param As CCursorFieldObj(Of String)
            Get
                Return Me.m_Param
            End Get
        End Property

        Public ReadOnly Property StatoComunicazione As CCursorFieldObj(Of StatoAlertComunicazione)
            Get
                Return Me.m_StatoComunicazione
            End Get
        End Property

        Public ReadOnly Property DataConsegna As CCursorField(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
        End Property

        Public ReadOnly Property DataLettura As CCursorField(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
        End Property

        Protected Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ComunicazioniAlert"
        End Function
    End Class


End Class




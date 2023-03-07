Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei dispositivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DispositivoCursor
        Inherits DBObjectCursorPO(Of Dispositivo)

        Private m_UserID As New CCursorField(Of Integer)("UserID")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Modello As New CCursorFieldObj(Of String)("Modello")
        Private m_Seriale As New CCursorFieldObj(Of String)("Seriale")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_DataAcquisto As New CCursorField(Of Date)("DataAcquisto")
        Private m_DataDismissione As New CCursorField(Of Date)("DataDismissione")
        Private m_StatoDispositivo As New CCursorField(Of StatoDispositivo)("StatoDispositivo")

        Public Sub New()
        End Sub

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Modello As CCursorFieldObj(Of String)
            Get
                Return Me.m_Modello
            End Get
        End Property

        Public ReadOnly Property Seriale As CCursorFieldObj(Of String)
            Get
                Return Me.m_Seriale
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property DataAcquisto As CCursorField(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
        End Property

        Public ReadOnly Property DataDismissione As CCursorField(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
        End Property

        Public ReadOnly Property StatoDispositivo As CCursorField(Of StatoDispositivo)
            Get
                Return Me.m_StatoDispositivo
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return Office.Dispositivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDevices"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class
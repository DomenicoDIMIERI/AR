Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

'Namespace DMD


Partial Class SMSGateway



    Public Class OutServiceCursor
        Inherits DMD.Databases.DBObjectCursorBase(Of OutService)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_ComPort As New CCursorFieldObj(Of String)("ComPort")
        Private m_DeviceSerialNumber As New CCursorFieldObj(Of String)("DeviceSerialNumber")
        Private m_Credito As New CCursorField(Of Decimal)("Credito")
        Private m_SogliaCredito As New CCursorField(Of Decimal)("SogliaCredito")
        Private m_NotificaA As New CCursorFieldObj(Of String)("NotificaA")
        Private m_CostoSMS As New CCursorField(Of Decimal)("CostoSMS")
        Private m_ScadenzaCredito As New CCursorField(Of Date)("ScadenzaCredito")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_UserName As New CCursorFieldObj(Of String)("UserName")
        Private m_Password As New CCursorFieldObj(Of String)("Password")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property ComPort As CCursorFieldObj(Of String)
            Get
                Return Me.m_ComPort
            End Get
        End Property

        Public ReadOnly Property DeviceSerialNumber As CCursorFieldObj(Of String)
            Get
                Return Me.m_DeviceSerialNumber
            End Get
        End Property

        Public ReadOnly Property Credito As CCursorField(Of Decimal)
            Get
                Return Me.m_Credito
            End Get
        End Property

        Public ReadOnly Property SogliaCredito As CCursorField(Of Decimal)
            Get
                Return Me.m_SogliaCredito
            End Get
        End Property

        Public ReadOnly Property NotificaA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NotificaA
            End Get
        End Property

        Public ReadOnly Property CostoSMS As CCursorField(Of Decimal)
            Get
                Return Me.m_CostoSMS
            End Get
        End Property

        Public ReadOnly Property ScadenzaCredito As CCursorField(Of Date)
            Get
                Return Me.m_ScadenzaCredito
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property Password As CCursorFieldObj(Of String)
            Get
                Return Me.m_Password
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return SMSGateway.Database
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OutServices"
        End Function
    End Class


End Class

'End Namespace